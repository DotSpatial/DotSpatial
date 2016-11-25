﻿// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/5/2010 6:59:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Data;
using GeoAPI.Geometries;
using NetTopologySuite.IO;

namespace DotSpatial.Data
{
    /// <summary>
    ///Represents strongly named DataRow class.
    ///</summary>
    public class FeatureRow : DataRow, IFeatureRow
    {
        #region Private Variables

        private readonly FeatureTable _featureTable;
        private IExtent _extent;
        private IGeometry _geometry;
        private Shape _shape;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the FeatureRow class.
        /// </summary>
        /// <param name="rb"></param>
        internal FeatureRow(DataRowBuilder rb)
            : base(rb)
        {
            _featureTable = ((FeatureTable)(Table));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Stores the shape into the WKB for storage in a database, overwriting any existing value.
        /// </summary>
        public void StoreShape()
        {
            this[_featureTable.GeometryColumn] = _shape.ToGeometry(_featureTable.GeometryFactory).AsBinary();
        }

        /// <summary>
        /// Stores the goemetry as WKB, overwriting any existing value.
        /// </summary>
        public void StoreGeometry()
        {
            this[_featureTable.GeometryColumn] = _geometry.AsBinary();
        }

        /// <summary>
        /// Sets the geometry field to DBNull.
        /// </summary>
        public void SetWellKnwonBinaryNull()
        {
            this[_featureTable.GeometryColumn] = Convert.DBNull;
        }

        #endregion

        #region Properties

        /// <summary>
        /// A long integer representing the FID.
        /// </summary>
        public long FID
        {
            get
            {
                return ((long)(this[_featureTable.FidColumn]));
            }
            set
            {
                this[_featureTable.FidColumn] = value;
            }
        }

        /// <summary>
        /// The Byte form of the Geometry
        /// </summary>
        public byte[] WellKnownBinary
        {
            get
            {
                try
                {
                    return ((byte[])(this[_featureTable.GeometryColumn]));
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

        /// <summary>
        /// From shapefiles, this should be cached.  Any other source must likely
        /// create the extent from a geometry, and possibly all the way from WKB.
        /// </summary>
        public IExtent Extent
        {
            get
            {
                if (_extent == null)
                {
                    if (_geometry == null)
                    {
                        if (IsWellKnownBinaryNull())
                        {
                            if (_shape == null)
                            {
                                return null;
                            }
                        }
                    }
                    // Accessing property here to try to create geometry.
                    IGeometry lazyGeom = Geometry;
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
        /// A lazily created and cached geometry.
        /// </summary>
        public IGeometry Geometry
        {
            get
            {
                if (_geometry == null)
                {
                    if (IsWellKnownBinaryNull())
                    {
                        return _shape == null ? null : _shape.ToGeometry(_featureTable.GeometryFactory);
                    }
                    WKBReader reader = new WKBReader(_featureTable.GeometryFactory);
                    byte[] geometryBytes = (byte[])this[_featureTable.GeometryColumn];

                    _geometry = reader.Read(geometryBytes);
                    return _geometry;
                }
                return _geometry;
            }
            set { _geometry = value; }
        }

        /// <summary>
        /// Boolean, true if the extent has been created, false otherwise.
        /// </summary>
        public bool IsExtentCreated
        {
            get { return _extent != null; }
        }

        /// <summary>
        /// Boolean, true if the geometry has already been created, false otherwise.
        /// </summary>
        public bool IsGeometryCreated
        {
            get { return (_geometry != null); }
        }

        /// <summary>
        /// Boolean, true if the shape has already been created, false otherwise.
        /// </summary>
        public bool IsShapeCreated
        {
            get { return (_shape != null); }
        }

        /// <summary>
        /// When reading from WKB, the Shape is lazily created.  When reading from sources like
        /// a shapefile, the Shape is actually pre-fetched because the value exists as a shape
        /// rather than a geometry.
        /// </summary>
        public Shape Shape
        {
            get { return _shape ?? (_shape = new Shape(Geometry, FeatureType.Unspecified)); }
            set { _shape = value; }
        }

        /// <summary>
        /// Gets whether the geometry is null.
        /// </summary>
        /// <returns>Boolean, true if the column is null.</returns>
        public bool IsWellKnownBinaryNull()
        {
            return IsNull(_featureTable.GeometryColumn);
        }

        #endregion
    }
}