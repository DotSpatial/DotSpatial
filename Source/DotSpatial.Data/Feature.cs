// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 12:00:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using DotSpatial.NTSExtension;
using DotSpatial.Serialization;
using GeoAPI.Geometries;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// A generic vector feature that has the geometry and data components but no drawing information
    /// </summary>
    [ToolboxItem(false)]
    public class Feature : IFeature
    {
        #region Private Variables

        /// <summary>
        /// Gets or sets the column to use when sorting lists of features.
        /// If this is set to a column not in the field, the FID is used instead.
        /// This should be assigned before attempting to sort features.  Because
        /// this is static, it only has to be set once, and will affect
        /// all the individual comparisions until it is set differently.
        /// </summary>
        public static string ComparisonField;

        private IGeometry _basicGeometry;
        private DataRow _dataRow;
        private int _numParts;
        private IFeatureSet _parentFeatureSet;
        private FeatureType _featureType;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a feature from the specified shape.  This will not handle the attribute content,
        /// which should be handles separately, with full knowledge of the desired schema.
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
                _basicGeometry = new Point(c);
                _featureType = FeatureType.Point;
            }
            if (shape.Range.FeatureType == FeatureType.MultiPoint)
            {
                List<Coordinate> coords = new List<Coordinate>();
                foreach (PartRange part in shape.Range.Parts)
                {
                    for (int i = part.StartIndex; i <= part.EndIndex; i++)
                    {
                        Coordinate c = new Coordinate(shape.Vertices[i * 2], shape.Vertices[i * 2 + 1]);
                        if (shape.Z != null) c.Z = shape.Z[i];
                        if (shape.M != null) c.M = shape.M[i];
                        coords.Add(c);
                    }
                }
                _basicGeometry = new MultiPoint(coords.CastToPointArray());
                _featureType = FeatureType.MultiPoint;
            }

            if (shape.Range.FeatureType == FeatureType.Line)
            {
                List<ILineString> strings = new List<ILineString>();
                foreach (PartRange part in shape.Range.Parts)
                {
                    List<Coordinate> coords = new List<Coordinate>();
                    for (int i = part.StartIndex; i <= part.EndIndex; i++)
                    {
                        Coordinate c = new Coordinate(shape.Vertices[i * 2], shape.Vertices[i * 2 + 1]);
                        if (shape.Z != null) c.Z = shape.Z[i];
                        if (shape.M != null) c.M = shape.M[i];
                        coords.Add(c);
                    }
                    strings.Add(new LineString(coords.ToArray()));
                }
                if (strings.Count > 1)
                {
                    _basicGeometry = new MultiLineString(strings.ToArray());
                }
                else if (strings.Count == 1)
                {
                    _basicGeometry = strings[0];
                }
                _featureType = FeatureType.Line;
            }
            if (shape.Range.FeatureType == FeatureType.Polygon)
            {
                ReadPolygonShape(shape);
            }

        }

        /// <summary>
        /// Creates a complete geometric feature based on a single point.  The attribute datarow is null.
        /// </summary>
        /// <param name="point">The vertex</param>
        public Feature(Vertex point)
            : this(new Point(point.X, point.Y))
        {
        }

        /// <summary>
        /// Creates a single point feature from a new point.
        /// </summary>
        /// <param name="c"></param>
        public Feature(Coordinate c)
            : this(new Point(c))
        {
        }

        /// <summary>
        /// Creates a feature from a geometry
        /// </summary>
        /// <param name="geometry">The geometry to turn into a feature</param>
        public Feature(IGeometry geometry)
        {
            _basicGeometry = geometry;
            _featureType = FeatureTypeFromGeometryType(geometry);
            _dataRow = null;
        }

        /// <summary>
        /// This constructor allows the creation of a feature but will automatically
        /// add the feature to the parent featureset.
        /// </summary>
        /// <param name="geometry">The IBasicGeometry to use for this feature</param>
        /// <param name="parent">The IFeatureSet to add this feature to.</param>
        public Feature(IGeometry geometry, IFeatureSet parent)
        {
            _basicGeometry = geometry;
            _featureType = FeatureTypeFromGeometryType(geometry);
            _dataRow = parent.DataTable.NewRow();
            parent.Features.Add(this);
        }

        /// <summary>
        /// Constructs a new Feature
        /// </summary>
        public Feature()
        {
            _dataRow = null;
            _basicGeometry = null;
            _featureType = FeatureType.Unspecified;
            //_envelopSource = CacheTypes.Cached;
        }

        /// <summary>
        /// Creates a new instance of a feature, by specifying the feature type enumeration and a
        /// set of coordinates that can be either a list or an array as long as it is enumerable.
        /// </summary>
        /// <param name="featureType">The feature type</param>
        /// <param name="coordinates">The coordinates to pass</param>
        public Feature(FeatureType featureType, IEnumerable<Coordinate> coordinates)
        {
            switch (featureType)
            {
                case FeatureType.Line:
                    _dataRow = null;
                    _basicGeometry = new LineString(coordinates.ToArray());
                    //_envelopSource = CacheTypes.Dynamic;
                    break;
                case FeatureType.MultiPoint:
                    _dataRow = null;
                    _basicGeometry = new MultiPoint(coordinates.CastToPointArray());
                    //_envelopSource = CacheTypes.Dynamic;
                    break;
                case FeatureType.Point:
                    _dataRow = null;
                    _basicGeometry = new Point(coordinates.First());
                    //_envelopSource = CacheTypes.Dynamic;
                    break;
                case FeatureType.Polygon:
                    _dataRow = null;
                    _basicGeometry = new Polygon(new LinearRing(coordinates.ToArray()));
                    //_envelopSource = CacheTypes.Dynamic;
                    break;
            }
            _featureType = featureType;
        }

        /// <summary>
        /// The FID comparison will be very slow, so this should only be used
        /// if the ComparisonField property is set.  This will allow sorting
        /// features based on their data-row attributes.  The data rows
        /// do not have to be identical, as long as both contain a column
        /// with the comparison field.
        /// </summary>
        /// <param name="other">The other IFeature to compare to.</param>
        /// <returns>An integer that controls the sorting based on the values for the specified field name.</returns>
        int IComparable<IFeature>.CompareTo(IFeature other)
        {
            DataRow oDr = other.DataRow;
            if (_dataRow != null && oDr != null)
            {
                if (_dataRow.Table != null && oDr.Table != null)
                {
                    int myIndex = _dataRow.Table.Columns.IndexOf(ComparisonField);
                    int oIndex = oDr.Table.Columns.IndexOf(ComparisonField);
                    IComparable c = _dataRow[myIndex] as IComparable;
                    if (c != null)
                    {
                        return c.CompareTo(oDr[oIndex]);
                    }
                }
            }

            int myFid = Fid;
            int oFid = other.Fid;
            return myFid.CompareTo(oFid);
        }

        /// <summary>
        /// This returns itself as the first geometry
        /// </summary>
        /// <returns>An IBasicGeometry interface</returns>
        /// <exception cref="IndexOutOfRangeException">Index cannot be less than 0 or greater than 1</exception>
        public IGeometry GetBasicGeometryN(int index)
        {
            return _basicGeometry.GetGeometryN(index);
        }

        private void ReadPolygonShape(Shape shape)
        {
            List<ILinearRing> shells = new List<ILinearRing>();
            List<ILinearRing> holes = new List<ILinearRing>();
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
                    if (CGAlgorithms.IsCCW(ring.Coordinates))
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
                holes = new List<ILinearRing>();
            }
            //// Now we have a list of all shells and all holes
            List<ILinearRing>[] holesForShells = new List<ILinearRing>[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                holesForShells[i] = new List<ILinearRing>();
            }

            // Find holes
            for (int i = 0; i < holes.Count; i++)
            {
                ILinearRing testRing = holes[i];
                ILinearRing minShell = null;
                Envelope minEnv = null;
                Envelope testEnv = testRing.EnvelopeInternal;
                Coordinate testPt = testRing.Coordinates[0];
                for (int j = 0; j < shells.Count; j++)
                {
                    ILinearRing tryRing = shells[j];
                    Envelope tryEnv = tryRing.EnvelopeInternal;
                    if (minShell != null)
                        minEnv = minShell.EnvelopeInternal;

                    // Check if this new containing ring is smaller than the current minimum ring
                    if (tryEnv.Contains(testEnv) && (CGAlgorithms.IsPointInRing(testPt, tryRing.Coordinates) || (PointInList(testPt, tryRing.Coordinates))))
                    {
                        if (minShell == null || minEnv.Contains(tryEnv))
                        {
                            minShell = tryRing;
                        }
                        holesForShells[j].Add(holes[i]);
                    }
                }
            }

            var polygons = new IPolygon[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                polygons[i] = new Polygon(shells[i], holesForShells[i].ToArray());
            }

            if (polygons.Length == 1)
            {
                _basicGeometry = polygons[0];
            }
            else
            {
                // It's a multi part
                _basicGeometry = new MultiPolygon(polygons);
            }
            _featureType = FeatureType.Polygon;
        }

        /// <summary>
        /// Test if a point is in a list of coordinates.
        /// </summary>
        /// <param name="testPoint">TestPoint the point to test for.</param>
        /// <param name="pointList">PointList the list of points to look through.</param>
        /// <returns>true if testPoint is a point in the pointList list.</returns>
        private static bool PointInList(Coordinate testPoint, IEnumerable<Coordinate> pointList)
        {
            foreach (Coordinate p in pointList)
                if (p.Equals2D(testPoint))
                    return true;
            return false;
        }

        #endregion

        /// <summary>
        /// Determines the FeatureType of this feature based on the given geometry.
        /// </summary>
        /// <param name="geometry">Geometry that is used to determine the FeatureType.</param>
        /// <returns>Unspecified if the geometry was null otherwise the FeatureType that corresponds to the geometries OgcGeometryType.</returns>
        private FeatureType FeatureTypeFromGeometryType(IGeometry geometry)
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
            }
            return featureType;
        }


        #region Methods

        /// <summary>
        /// Creates a deep copy of this feature.
        /// </summary>
        /// <returns>Returns a deep copy of this feature as an IFeature</returns>
        IFeature IFeature.Copy()
        {
            return Copy();
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
        /// Creates a deep copy of this feature.
        /// </summary>
        /// <returns>Returns a deep copy of this feature as an object</returns>
        object ICloneable.Clone()
        {
            Feature clone = (Feature)MemberwiseClone();
            OnCopy(clone);
            return clone;
        }

        /// <summary>
        /// Forces the geometry to update its envelope, and then updates the cached envelope of the feature.
        /// </summary>
        public void UpdateEnvelope()
        {
            if (_basicGeometry == null) return;
            _basicGeometry.UpdateEnvelope();
            if (ShapeIndex != null) ShapeIndex.CalculateExtents(); //Changed by jany_ (2015-07-09) must be updated because sometimes ShapeIndizes are used although IndexMode is false
        }

        /// <summary>
        /// Creates a new GML string describing the location of this point
        /// </summary>
        /// <returns>A String representing the Geographic Markup Language version of this point</returns>
        public virtual string ExportToGml()
        {
            var geo = (_basicGeometry as Geometry);
            return (geo == null) ? "" : geo.ToGMLFeature().ToString();
        }

        /// <summary>
        /// Returns the Well-known Binary representation of this <c>Geometry</c>.
        /// For a definition of the Well-known Binary format, see the OpenGIS Simple
        /// Features Specification.
        /// </summary>
        /// <returns>The Well-known Binary representation of this <c>Geometry</c>.</returns>
        public virtual byte[] ToBinary()
        {
            return _basicGeometry.AsBinary();
        }

        /// <summary>
        /// Creates a new shape based on this feature by itself.
        /// </summary>
        /// <returns>A Shape object</returns>
        public Shape ToShape()
        {
            return new Shape(this);
        }

        /// <summary>
        /// Copies this feature, creating an independant, but identical feature.
        /// </summary>
        /// <returns></returns>
        public Feature Copy()
        {
            Feature clone = (Feature)MemberwiseClone();
            clone.Geometry = Geometry.Copy();
            if (ParentFeatureSet != null && ParentFeatureSet.DataTable != null)
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
        /// Occurs during the cloning process and this method also duplicates the envelope and basic geometry.
        /// </summary>
        /// <param name="copy">The feature being copied</param>
        protected virtual void OnCopy(Feature copy)
        {
            copy.Geometry = _basicGeometry.Copy();
            // This provides an overrideable interface for modifying the copy behavior.
        }

        #endregion

        /// <summary>
        /// Gets or sets the integer number of parts associated with this feature.
        /// Setting this will set a cached value on the feature that is separate
        /// from the geometry and set the NumParts to Cached.
        /// </summary>
        public int NumParts
        {
            get
            {
                if (NumPartsSource == CacheTypes.Cached)
                {
                    return _numParts;
                }
                int count = 0;

                if (FeatureType != FeatureType.Polygon)
                    return _basicGeometry.NumGeometries;
                IPolygon p = _basicGeometry as IPolygon;
                if (p == null)
                {
                    // we have a multi-polygon situation
                    for (int i = 0; i < _basicGeometry.NumGeometries; i++)
                    {
                        p = _basicGeometry.GetGeometryN(i) as IPolygon;
                        if (p == null) continue;
                        count += 1; // Shell
                        count += p.NumInteriorRings; // Holes
                    }
                }
                else
                {
                    // The feature is a polygon, not a multi-polygon
                    count += 1; // Shell
                    count += p.NumInteriorRings; // Holes
                }
                return count;
            }
            set
            {
                _numParts = value;
                NumPartsSource = CacheTypes.Cached;
            }
        }

        /// <summary>
        /// Gets or sets a DotSpatial.Data.CacheTypes enumeration.  If the value
        /// is dynamic, then NumParts will be read from the geometry on this feature.
        /// If it is cached, then the value is separate from the geometry.
        /// </summary>
        public CacheTypes NumPartsSource { get; set; }

        #region IFeature Members

        /// <summary>
        /// Gets or sets a valid IGeometry associated with the data elements of this feature.
        /// This will be enough geometry information to cast into a full fledged geometry
        /// that can be used in coordination with DotSpatial.Analysis
        /// </summary>
        public virtual IGeometry Geometry
        {
            get
            {
                return _basicGeometry;
            }
            set
            {
                _basicGeometry = value;
                FeatureType = FeatureTypeFromGeometryType(_basicGeometry);
                //EnvelopeSource = CacheTypes.Cached;
            }
        }

        /// <summary>
        /// If the geometry for this shape was loaded from a file, this contains the size
        /// of this shape in 16-bit words as per the Esri Shapefile specification.
        /// </summary>
        public int ContentLength
        {
            get { return ShapeIndex != null ? ShapeIndex.ContentLength : 0; }
            set
            {
                // nothing
            }
        }

        /// <summary>
        /// Gets the datarow containing all the attributes related to this geometry.
        /// This will query the parent feature layer's data Table by FID and then
        /// cache the value locally.  If no parent feature layer exists, then
        /// this is meaningless.  You should create a new Feature by doing
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
        /// Returns either Point, Polygon or Line.
        /// </summary>
        public FeatureType FeatureType
        {
            get
            {
                return _basicGeometry == null ? FeatureType.Unspecified : _featureType;
            }
            private set { _featureType = value; }
        }

        /// <summary>
        /// Gets the key that is associated with this feature.  This returns -1 if
        /// this feature is not a member of a feature layer.
        /// </summary>
        public virtual int Fid
        {
            get
            {
                if (_parentFeatureSet.IndexMode || !_parentFeatureSet.AttributesPopulated)
                {
                    return RecordNumber - 1; // -1 because RecordNumber for shapefiles is 1-based.
                    // todo: The better will be remove RecordNumber from public interface to avoid ±1 issues.
                }
                return _parentFeatureSet.Features.IndexOf(this);
            }
        }

        /// <summary>
        /// Shows the type of geometry for this feature
        /// </summary>
        public virtual string GeometryType
        {
            get
            {
                if (_basicGeometry == null) return string.Empty;
                return _basicGeometry.GeometryType;
            }
        }

        /// <summary>
        /// Returns the NumGeometries in the BasicGeometry of this feature
        /// </summary>
        public virtual int NumGeometries
        {
            get
            {
                if (_basicGeometry == null) return 0;
                return _basicGeometry.NumGeometries;
            }
        }

        /// <summary>
        /// Gets the integer number of points associated with features.
        /// </summary>
        public virtual int NumPoints
        {
            get
            {
                if (_basicGeometry == null) return 0;
                return _basicGeometry.NumPoints;
            }
        }

        /// <summary>
        /// An index value that is saved in some file formats.
        /// </summary>
        public int RecordNumber
        {
            get { return ShapeIndex != null ? ShapeIndex.RecordNumber : -1; }
            set
            {
                // nothing
            }
        }

        /// <summary>
        /// Gets a reference to the IFeatureLayer that contains this item.
        /// </summary>
        public virtual IFeatureSet ParentFeatureSet
        {
            get { return _parentFeatureSet; }
            set { _parentFeatureSet = value; }
        }

        /// <summary>
        /// When a shape is loaded from a Shapefile, this will identify whether M or Z values are used
        /// and whether or not the shape is null.
        /// </summary>
        public ShapeType ShapeType
        {
            get { return ShapeIndex != null ? ShapeIndex.ShapeType : ShapeType.NullShape; }
            set
            {
                // nothing

                // todo: Remove setters for ShapeType/RecordNumber/ContentLength from public interface
                // They all must be available only through ShapeIndex property.
            }
        }

        /// <summary>
        /// This is simply a quick access to the Vertices list for this specific
        /// feature. If the Vertices have not yet been defined, this will be null.
        /// </summary>
        public ShapeRange ShapeIndex { get; set; }

        #endregion
    }
}