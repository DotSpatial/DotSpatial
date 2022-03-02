// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Data;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// Represents strongly named DataRow class.
    /// </summary>
    public class FeatureRow : DataRow, IFeatureRow
    {
        #region Fields

        private readonly FeatureTable _featureTable;
        private IExtent _extent;
        private Geometry _geometry;
        private Shape _shape;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureRow"/> class.
        /// </summary>
        /// <param name="rb">The datarow builder.</param>
        internal FeatureRow(DataRowBuilder rb)
            : base(rb)
        {
            _featureTable = (FeatureTable)Table;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the extent. From shapefiles, this should be cached. Any other source must likely
        /// create the extent from a geometry, and possibly all the way from WKB.
        /// </summary>
        public IExtent Extent
        {
            get
            {
                if (_extent == null)
                {
                    if (_geometry == null && IsWellKnownBinaryNull() && _shape == null)
                    {
                        return null;
                    }

                    // Accessing property here to try to create geometry.
                    Geometry lazyGeom = Geometry;
                    if (lazyGeom == null) return null;

                    _extent = lazyGeom.EnvelopeInternal.ToExtent();
                }

                return _extent;
            }

            set
            {
                _extent = value;
            }
        }

        /// <summary>
        /// Gets or sets a long integer representing the FID.
        /// </summary>
        public long Fid
        {
            get
            {
                return (long)this[_featureTable.FidColumn];
            }

            set
            {
                this[_featureTable.FidColumn] = value;
            }
        }

        /// <summary>
        /// Gets or sets a lazily created and cached geometry.
        /// </summary>
        public Geometry Geometry
        {
            get
            {
                if (_geometry == null)
                {
                    if (IsWellKnownBinaryNull())
                    {
                        return _shape?.ToGeometry(_featureTable.GeometryFactory);
                    }

                    WKBReader reader = new WKBReader();
                    byte[] geometryBytes = (byte[])this[_featureTable.GeometryColumn];

                    _geometry = reader.Read(geometryBytes);
                    return _geometry;
                }

                return _geometry;
            }

            set
            {
                _geometry = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the extent has been created.
        /// </summary>
        public bool IsExtentCreated => _extent != null;

        /// <summary>
        /// Gets a value indicating whether the geometry has already been created.
        /// </summary>
        public bool IsGeometryCreated => _geometry != null;

        /// <summary>
        /// Gets a value indicating whether the shape has already been created.
        /// </summary>
        public bool IsShapeCreated => _shape != null;

        /// <summary>
        /// Gets or sets the shape. When reading from WKB, the Shape is lazily created. When reading from sources like
        /// a shapefile, the Shape is actually pre-fetched because the value exists as a shape rather than a geometry.
        /// </summary>
        public Shape Shape
        {
            get
            {
                return _shape ?? (_shape = new Shape(Geometry, FeatureType.Unspecified));
            }

            set
            {
                _shape = value;
            }
        }

        /// <summary>
        /// Gets or sets the Byte form of the Geometry.
        /// </summary>
        public byte[] WellKnownBinary
        {
            get
            {
                try
                {
                    return (byte[])this[_featureTable.GeometryColumn];
                }
                catch (InvalidCastException e)
                {
                    throw new StrongTypingException("The value for column \'GEOMETRY\' in table \'FeatureTable\' is DBNull.", e);
                }
            }

            set
            {
                this[_featureTable.GeometryColumn] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets whether the geometry is null.
        /// </summary>
        /// <returns>Boolean, true if the column is null.</returns>
        public bool IsWellKnownBinaryNull()
        {
            return IsNull(_featureTable.GeometryColumn);
        }

        /// <summary>
        /// Sets the geometry field to DBNull.
        /// </summary>
        public void SetWellKnwonBinaryNull()
        {
            this[_featureTable.GeometryColumn] = Convert.DBNull;
        }

        /// <summary>
        /// Stores the geometry as WKB, overwriting any existing value.
        /// </summary>
        public void StoreGeometry()
        {
            this[_featureTable.GeometryColumn] = _geometry.AsBinary();
        }

        /// <summary>
        /// Stores the shape into the WKB for storage in a database, overwriting any existing value.
        /// </summary>
        public void StoreShape()
        {
            this[_featureTable.GeometryColumn] = _shape.ToGeometry(_featureTable.GeometryFactory).AsBinary();
        }

        #endregion
    }
}