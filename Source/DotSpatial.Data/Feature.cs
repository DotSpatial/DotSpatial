// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using DotSpatial.NTSExtension;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// A generic vector feature that has the geometry and data components but no drawing information.
    /// </summary>
    [ToolboxItem(false)]
    public class Feature : IFeature
    {
        #region Fields

        private DataRow _dataRow;
        private FeatureType _featureType;

        private Geometry _geometry;
        private IFeatureSet _parentFeatureSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class from the specified shape. This will not
        /// handle the attribute content, which should be handles separately, with full knowledge of the desired schema.
        /// </summary>
        /// <param name="shape">The shape to read the vertices from in order to build a proper geometry.</param>
        public Feature(Shape shape)
        {
            if (shape.Range == null) return;

            ShapeIndex = shape.Range;
            if (shape.Range.FeatureType == FeatureType.Point)
            {
                Coordinate c = new Coordinate(shape.Vertices[0], shape.Vertices[1]);
                if (shape.Z != null) c.Z = shape.Z[0];
                if (shape.M != null) c.M = shape.M[0];
                _geometry = new Point(c);
                _featureType = FeatureType.Point;
            }

            if (shape.Range.FeatureType == FeatureType.MultiPoint)
            {
                List<Coordinate> coords = new List<Coordinate>();
                foreach (PartRange part in shape.Range.Parts)
                {
                    for (int i = part.StartIndex; i <= part.EndIndex; i++)
                    {
                        Coordinate c = new Coordinate(shape.Vertices[i * 2], shape.Vertices[(i * 2) + 1]);
                        if (shape.Z != null) c.Z = shape.Z[i];
                        if (shape.M != null) c.M = shape.M[i];
                        coords.Add(c);
                    }
                }

                _geometry = new MultiPoint(coords.CastToPointArray());
                _featureType = FeatureType.MultiPoint;
            }

            if (shape.Range.FeatureType == FeatureType.Line)
            {
                List<LineString> strings = new List<LineString>();
                foreach (PartRange part in shape.Range.Parts)
                {
                    List<Coordinate> coords = new List<Coordinate>();
                    for (int i = part.StartIndex; i <= part.EndIndex; i++)
                    {
                        Coordinate c = new Coordinate(shape.Vertices[i * 2], shape.Vertices[(i * 2) + 1]);
                        if (shape.Z != null) c.Z = shape.Z[i];
                        if (shape.M != null) c.M = shape.M[i];
                        coords.Add(c);
                    }

                    strings.Add(new LineString(coords.ToArray()));
                }

                if (strings.Count > 1)
                {
                    _geometry = new MultiLineString(strings.ToArray());
                }
                else if (strings.Count == 1)
                {
                    _geometry = strings[0];
                }

                _featureType = FeatureType.Line;
            }

            if (shape.Range.FeatureType == FeatureType.Polygon)
            {
                ReadPolygonShape(shape);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class based on a single point. The attribute datarow is null.
        /// </summary>
        /// <param name="point">The vertex.</param>
        public Feature(Vertex point)
            : this(new Point(point.X, point.Y))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class from a new point.
        /// </summary>
        /// <param name="c">The coordinate of the point.</param>
        public Feature(Coordinate c)
            : this(new Point(c))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class from a geometry.
        /// </summary>
        /// <param name="geometry">The geometry to turn into a feature.</param>
        public Feature(Geometry geometry)
        {
            _geometry = geometry;
            _featureType = FeatureTypeFromGeometryType(geometry);
            _dataRow = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// This will automatically add the feature to the parent featureset.
        /// </summary>
        /// <param name="geometry">The IBasicGeometry to use for this feature.</param>
        /// <param name="parent">The IFeatureSet to add this feature to.</param>
        public Feature(Geometry geometry, IFeatureSet parent)
        {
            _geometry = geometry;
            _featureType = FeatureTypeFromGeometryType(geometry);
            _dataRow = parent.DataTable.NewRow();
            parent.Features.Add(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// </summary>
        public Feature()
        {
            _dataRow = null;
            _geometry = null;
            _featureType = FeatureType.Unspecified;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class, by specifying the feature type enumeration and a
        /// set of coordinates that can be either a list or an array as long as it is enumerable.
        /// </summary>
        /// <param name="featureType">The feature type.</param>
        /// <param name="coordinates">The coordinates to pass.</param>
        public Feature(FeatureType featureType, IEnumerable<Coordinate> coordinates)
        {
            switch (featureType)
            {
                case FeatureType.Line:
                    _dataRow = null;
                    _geometry = new LineString(coordinates.ToArray());
                    break;
                case FeatureType.MultiPoint:
                    _dataRow = null;
                    _geometry = new MultiPoint(coordinates.CastToPointArray());
                    break;
                case FeatureType.Point:
                    _dataRow = null;
                    _geometry = new Point(coordinates.First());
                    break;
                case FeatureType.Polygon:
                    _dataRow = null;
                    _geometry = new Polygon(new LinearRing(coordinates.ToArray()));
                    break;
            }

            _featureType = featureType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the column to use when sorting lists of features.
        /// If this is set to a column not in the field, the FID is used instead.
        /// This should be assigned before attempting to sort features. Because
        /// this is static, it only has to be set once, and will affect
        /// all the individual comparisions until it is set differently.
        /// </summary>
        public static string ComparisonField { get; set; }

        /// <summary>
        /// Gets or sets the datarow containing all the attributes related to this geometry.
        /// This will query the parent feature layer's data Table by FID and then
        /// cache the value locally. If no parent feature layer exists, then
        /// this is meaningless. You should create a new Feature by doing
        /// FeatureLayer.Features.Add(), which will return a new Feature.
        /// </summary>
        public virtual DataRow DataRow
        {
            get
            {
                return _dataRow;
            }

            set
            {
                _dataRow = value;
            }
        }

        /// <summary>
        /// Gets either Point, Polygon or Line.
        /// </summary>
        public FeatureType FeatureType
        {
            get
            {
                return _geometry == null ? FeatureType.Unspecified : _featureType;
            }

            private set
            {
                _featureType = value;
            }
        }

        /// <summary>
        /// Gets the key that is associated with this feature. This returns -1 if
        /// this feature is not a member of a feature layer.
        /// </summary>
        public virtual int Fid
        {
            get
            {
                if (_parentFeatureSet.IndexMode || !_parentFeatureSet.AttributesPopulated)
                {
                    return ShapeIndex?.RecordNumber - 1 ?? -2; // -1 because RecordNumber for shapefiles is 1-based.

                    // todo: The better will be remove RecordNumber from public interface to avoid ±1 issues.
                }

                return _parentFeatureSet.Features.IndexOf(this);
            }
        }

        /// <summary>
        /// Gets or sets a valid Geometry associated with the data elements of this feature.
        /// This will be enough geometry information to cast into a full fledged geometry
        /// that can be used in coordination with DotSpatial.Analysis.
        /// </summary>
        public virtual Geometry Geometry
        {
            get
            {
                return _geometry;
            }

            set
            {
                _geometry = value;
                FeatureType = FeatureTypeFromGeometryType(_geometry);
            }
        }

        /// <summary>
        /// Gets or sets a reference to the IFeatureLayer that contains this item.
        /// </summary>
        public virtual IFeatureSet ParentFeatureSet
        {
            get
            {
                return _parentFeatureSet;
            }

            set
            {
                _parentFeatureSet = value;
            }
        }

        /// <summary>
        /// Gets or sets the shape index. This is simply a quick access to the Vertices list for this specific
        /// feature. If the Vertices have not yet been defined, this will be null.
        /// </summary>
        public ShapeRange ShapeIndex { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Copies this feature, creating an independant, but identical feature.
        /// </summary>
        /// <returns>The copy.</returns>
        public Feature Copy()
        {
            Feature clone = (Feature)MemberwiseClone();
            clone.Geometry = Geometry.Copy();
            if (ParentFeatureSet?.DataTable != null)
            {
                DataTable table = ParentFeatureSet.DataTable;
                clone._dataRow = table.NewRow();
                if (DataRow != null)
                {
                    for (int i = 0; i < ParentFeatureSet.DataTable.Columns.Count; i++)
                    {
                        clone._dataRow[i] = DataRow[i];
                    }
                }
            }

            return clone;
        }

        /// <summary>
        /// This uses the field names to copy attribute values from the source to this feature.
        /// Even if columns are missing or if there are extra columns, this method should work.
        /// </summary>
        /// <param name="source">The IFeature source to copy attributes from.</param>
        public void CopyAttributes(IFeature source)
        {
            if (source.DataRow == null) return;

            for (int i = 0; i < ParentFeatureSet.DataTable.Columns.Count; i++)
            {
                string name = ParentFeatureSet.DataTable.Columns[i].ColumnName;
                if (source.ParentFeatureSet.DataTable.Columns.Contains(name))
                {
                    _dataRow[i] = source.DataRow[name];
                }
            }
        }

        /// <summary>
        /// Creates a new shape based on this feature by itself.
        /// </summary>
        /// <returns>A Shape object.</returns>
        public Shape ToShape()
        {
            return new Shape(this);
        }

        /// <summary>
        /// Forces the geometry to update its envelope, and then updates the cached envelope of the feature.
        /// </summary>
        public void UpdateEnvelope()
        {
            if (_geometry == null) return;

            _geometry.GeometryChanged();
            ShapeIndex?.CalculateExtents(); // Changed by jany_ (2015-07-09) must be updated because sometimes ShapeIndizes are used although IndexMode is false
        }

        /// <summary>
        /// Creates a deep copy of this feature.
        /// </summary>
        /// <returns>Returns a deep copy of this feature as an object.</returns>
        object ICloneable.Clone()
        {
            Feature clone = (Feature)MemberwiseClone();
            OnCopy(clone);
            return clone;
        }

        /// <summary>
        /// The FID comparison will be very slow, so this should only be used
        /// if the ComparisonField property is set. This will allow sorting
        /// features based on their data-row attributes. The data rows
        /// do not have to be identical, as long as both contain a column
        /// with the comparison field.
        /// </summary>
        /// <param name="other">The other IFeature to compare to.</param>
        /// <returns>An integer that controls the sorting based on the values for the specified field name.</returns>
        int IComparable<IFeature>.CompareTo(IFeature other)
        {
            DataRow oDr = other.DataRow;
            if (_dataRow?.Table != null && oDr?.Table != null)
            {
                int myIndex = _dataRow.Table.Columns.IndexOf(ComparisonField);
                int oIndex = oDr.Table.Columns.IndexOf(ComparisonField);
                if (_dataRow[myIndex] is IComparable c)
                {
                    return c.CompareTo(oDr[oIndex]);
                }
            }

            int myFid = Fid;
            int oFid = other.Fid;
            return myFid.CompareTo(oFid);
        }

        /// <summary>
        /// Creates a deep copy of this feature.
        /// </summary>
        /// <returns>Returns a deep copy of this feature as an IFeature.</returns>
        IFeature IFeature.Copy()
        {
            return Copy();
        }

        /// <summary>
        /// Occurs during the cloning process and this method also duplicates the envelope and basic geometry.
        /// </summary>
        /// <param name="copy">The feature being copied.</param>
        protected virtual void OnCopy(Feature copy)
        {
            copy.Geometry = _geometry.Copy();

            // This provides an overrideable interface for modifying the copy behavior.
        }

        /// <summary>
        /// Determines the FeatureType of this feature based on the given geometry.
        /// </summary>
        /// <param name="geometry">Geometry that is used to determine the FeatureType.</param>
        /// <returns>Unspecified if the geometry was null otherwise the FeatureType that corresponds to the geometries OgcGeometryType.</returns>
        private static FeatureType FeatureTypeFromGeometryType(Geometry geometry)
        {
            FeatureType featureType = FeatureType.Unspecified;
            if (geometry == null) return featureType;

            switch (geometry.OgcGeometryType)
            {
                case OgcGeometryType.Point:
                    featureType = FeatureType.Point;
                    break;
                case OgcGeometryType.LineString:
                case OgcGeometryType.MultiLineString:
                    featureType = FeatureType.Line;
                    break;
                case OgcGeometryType.Polygon:
                case OgcGeometryType.MultiPolygon:
                    featureType = FeatureType.Polygon;
                    break;
                case OgcGeometryType.MultiPoint:
                    featureType = FeatureType.MultiPoint;
                    break;
                case OgcGeometryType.GeometryCollection:
                    GeometryCollection geomCollection = geometry as GeometryCollection;
                    if (geomCollection != null)
                    {
                        // Check to see if every featureType in the GeometryCollection matches
                        // else leave as FeatureType.Unspecified
                        FeatureType testFeatureType = FeatureType.Unspecified;
                        for (int i = 0; i < geomCollection.Count; i++)
                        {
                            if (i == 0)
                            {
                                testFeatureType = FeatureTypeFromGeometryType(geomCollection[i]);
                            }
                            else
                            {
                                FeatureType tempFeatureType = FeatureTypeFromGeometryType(geomCollection[i]);
                                if (testFeatureType != tempFeatureType)
                                    return FeatureType.Unspecified; // if feature types do not match, then return Unspecified
                            }
                        }

                        featureType = testFeatureType;
                    }

                    break;
            }

            return featureType;
        }

        /// <summary>
        /// Test if a point is in a list of coordinates.
        /// </summary>
        /// <param name="testPoint">TestPoint the point to test for.</param>
        /// <param name="pointList">PointList the list of points to look through.</param>
        /// <returns>true if testPoint is a point in the pointList list.</returns>
        private static bool PointInList(Coordinate testPoint, IEnumerable<Coordinate> pointList)
        {
            foreach (Coordinate p in pointList) if (p.Equals2D(testPoint)) return true;

            return false;
        }

        private void ReadPolygonShape(Shape shape)
        {
            List<LinearRing> shells = new List<LinearRing>();
            List<LinearRing> holes = new List<LinearRing>();
            foreach (PartRange part in shape.Range.Parts)
            {
                List<Coordinate> coords = new List<Coordinate>();
                int i = part.StartIndex;
                foreach (Vertex d in part)
                {
                    Coordinate c = new Coordinate(d.X, d.Y);
                    if (shape.M != null && shape.M.Length > 0) c.M = shape.M[i];
                    if (shape.Z != null && shape.Z.Length > 0) c.Z = shape.Z[i];
                    i++;
                    coords.Add(c);
                }

                LinearRing ring = new LinearRing(coords.ToArray());
                if (shape.Range.Parts.Count == 1)
                {
                    shells.Add(ring);
                }
                else
                {
                    if (ring.IsCCW)
                    {
                        holes.Add(ring);
                    }
                    else
                    {
                        shells.Add(ring);
                    }
                }
            }

            if (shells.Count == 0 && holes.Count > 0)
            {
                shells = holes;
                holes = new List<LinearRing>();
            }

            //// Now we have a list of all shells and all holes
            List<LinearRing>[] holesForShells = new List<LinearRing>[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                holesForShells[i] = new List<LinearRing>();
            }

            // Find holes
            foreach (LinearRing hole in holes)
            {
                LinearRing minShell = null;
                Envelope minEnv = null;
                Envelope testEnv = hole.EnvelopeInternal;
                Coordinate testPt = hole.Coordinates[0];
                for (int j = 0; j < shells.Count; j++)
                {
                    LinearRing tryRing = shells[j];
                    Envelope tryEnv = tryRing.EnvelopeInternal;
                    if (minShell != null) minEnv = minShell.EnvelopeInternal;

                    // Check if this new containing ring is smaller than the current minimum ring
                    if (tryEnv.Contains(testEnv) && (PointLocation.IsInRing(testPt, tryRing.Coordinates) || PointInList(testPt, tryRing.Coordinates)))
                    {
                        if (minShell == null || minEnv.Contains(tryEnv))
                        {
                            minShell = tryRing;
                        }

                        holesForShells[j].Add(hole);
                    }
                }
            }

            var polygons = new Polygon[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                polygons[i] = new Polygon(shells[i], holesForShells[i].ToArray());
            }

            if (polygons.Length == 1)
            {
                _geometry = polygons[0];
            }
            else
            {
                // It's a multi part
                _geometry = new MultiPolygon(polygons);
            }

            _featureType = FeatureType.Polygon;
        }

        #endregion
    }
}
