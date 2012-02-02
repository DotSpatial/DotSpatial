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
using DotSpatial.Topology;
using DotSpatial.Topology.Algorithm;

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

        private IBasicGeometry _basicGeometry;
        private int _contentLength;
        private DataRow _dataRow;
        private CacheTypes _envelopSource;
        private IEnvelope _envelope;
        private int _numParts;
        private CacheTypes _numPartsSource;
        private long _offset;
        private IFeatureSet _parentFeatureSet;
        private int _recordNumber;
        private ShapeRange _shapeIndex;
        private ShapeType _shapeType;

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
                    }
                }
                _basicGeometry = new MultiPoint(coords);
            }
            if (shape.Range.FeatureType == FeatureType.Line)
            {
                List<IBasicLineString> strings = new List<IBasicLineString>();
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
                    strings.Add(new LineString(coords));
                }
                if (strings.Count > 1)
                {
                    _basicGeometry = new MultiLineString(strings);
                }
                else if (strings.Count == 1)
                {
                    _basicGeometry = strings[0];
                }
            }
            if (shape.Range.FeatureType == FeatureType.Polygon)
            {
                ReadPolygonShape(shape);
            }
            else
            {
                // These properties are set by ReadPolygonShape, so set them here for everybody else.
                RecordNumber = shape.Range.RecordNumber;
                ContentLength = shape.Range.ContentLength;
                ShapeType = shape.Range.ShapeType;
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
        public Feature(IBasicGeometry geometry)
        {
            _basicGeometry = geometry;
            _dataRow = null;
        }

        /// <summary>
        /// This constructor allows the creation of a feature but will automatically
        /// add the feature to the parent featureset.
        /// </summary>
        /// <param name="geometry">The IBasicGeometry to use for this feature</param>
        /// <param name="parent">The IFeatureSet to add this feature to.</param>
        public Feature(IBasicGeometry geometry, IFeatureSet parent)
        {
            _basicGeometry = geometry;
            _dataRow = parent.DataTable.NewRow();
            _envelopSource = CacheTypes.Dynamic;
            parent.Features.Add(this);
        }

        /// <summary>
        /// Constructs a new Feature
        /// </summary>
        public Feature()
        {
            _dataRow = null;
            _basicGeometry = null;
            _envelopSource = CacheTypes.Cached;
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
                    _basicGeometry = new LineString(coordinates);
                    _envelopSource = CacheTypes.Dynamic;
                    break;
                case FeatureType.MultiPoint:
                    _dataRow = null;
                    _basicGeometry = new MultiPoint(coordinates);
                    _envelopSource = CacheTypes.Dynamic;
                    break;
                case FeatureType.Point:
                    _dataRow = null;
                    _basicGeometry = new Point(coordinates.First());
                    _envelopSource = CacheTypes.Dynamic;
                    break;
                case FeatureType.Polygon:
                    _dataRow = null;
                    _basicGeometry = new Polygon(coordinates);
                    _envelopSource = CacheTypes.Dynamic;
                    break;
            }
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

            int myFID = Fid;
            int oFID = other.Fid;
            return myFID.CompareTo(oFID);
        }

        /// <summary>
        /// This returns itself as the first geometry
        /// </summary>
        /// <returns>An IBasicGeometry interface</returns>
        /// <exception cref="IndexOutOfRangeException">Index cannot be less than 0 or greater than 1</exception>
        public IBasicGeometry GetBasicGeometryN(int index)
        {
            return _basicGeometry.GetBasicGeometryN(index);
        }

        private void ReadPolygonShape(Shape shape)
        {
            _envelope = shape.Range.Extent.ToEnvelope();
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
                LinearRing ring = new LinearRing(coords);
                if (shape.Range.Parts.Count == 1)
                {
                    shells.Add(ring);
                }
                else
                {
                    if (CgAlgorithms.IsCounterClockwise(ring.Coordinates))
                    {
                        holes.Add(ring);
                    }
                    else
                    {
                        shells.Add(ring);
                    }
                    //if (part.IsHole())
                    //{
                    //    holes.Add(ring);
                    //}
                    //else
                    //{
                    //    shells.Add(ring);
                    //}
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
                IEnvelope minEnv = null;
                IEnvelope testEnv = testRing.EnvelopeInternal;
                Coordinate testPt = testRing.Coordinates[0];
                ILinearRing tryRing;
                for (int j = 0; j < shells.Count; j++)
                {
                    tryRing = shells[j];
                    IEnvelope tryEnv = tryRing.EnvelopeInternal;
                    if (minShell != null)
                        minEnv = minShell.EnvelopeInternal;
                    bool isContained = false;

                    if (tryEnv.Contains(testEnv)
                        && (CgAlgorithms.IsPointInRing(testPt, tryRing.Coordinates)
                            || (PointInList(testPt, tryRing.Coordinates))))
                    {
                        isContained = true;
                    }

                    // Check if this new containing ring is smaller than the current minimum ring
                    if (isContained)
                    {
                        if (minShell == null || minEnv.Contains(tryEnv))
                        {
                            minShell = tryRing;
                        }
                        holesForShells[j].Add(holes[i]);
                    }
                }
            }

            IPolygon[] polygons = new Polygon[shells.Count];
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

            RecordNumber = shape.Range.RecordNumber;
            ContentLength = shape.Range.ContentLength;
            ShapeType = shape.Range.ShapeType;
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
            _envelope = _basicGeometry.Envelope;
        }

        /// <summary>
        /// Creates a new GML string describing the location of this point
        /// </summary>
        /// <returns>A String representing the Geographic Markup Language version of this point</returns>
        public virtual string ExportToGml()
        {
            return _basicGeometry.ExportToGml();
        }

        /// <summary>
        /// Returns the Well-known Binary representation of this <c>Geometry</c>.
        /// For a definition of the Well-known Binary format, see the OpenGIS Simple
        /// Features Specification.
        /// </summary>
        /// <returns>The Well-known Binary representation of this <c>Geometry</c>.</returns>
        public virtual byte[] ToBinary()
        {
            return _basicGeometry.ToBinary();
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
            clone.BasicGeometry = BasicGeometry.Copy();
            clone.Envelope = Envelope.Copy();
            DataTable table = ParentFeatureSet.DataTable;
            clone._dataRow = table.NewRow();
            if (DataRow != null)
            {
                for (int i = 0; i < ParentFeatureSet.DataTable.Columns.Count; i++)
                {
                    clone._dataRow[i] = DataRow[i];
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
            copy.Envelope = Envelope.Copy();
            copy.BasicGeometry = _basicGeometry.Copy();
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
                if (_numPartsSource == CacheTypes.Cached)
                {
                    return _numParts;
                }
                int count = 0;

                if (_basicGeometry.FeatureType != FeatureType.Polygon)
                    return _basicGeometry.NumGeometries;
                IBasicPolygon p = _basicGeometry as IBasicPolygon;
                if (p == null)
                {
                    // we have a multi-polygon situation
                    for (int i = 0; i < _basicGeometry.NumGeometries; i++)
                    {
                        p = _basicGeometry.GetBasicGeometryN(i) as IBasicPolygon;
                        if (p == null) continue;
                        count += 1; // Shell
                        count += p.NumHoles; // Holes
                    }
                }
                else
                {
                    // The feature is a polygon, not a multi-polygon
                    count += 1; // Shell
                    count += p.NumHoles; // Holes
                }
                return count;
            }
            set
            {
                _numParts = value;
                _numPartsSource = CacheTypes.Cached;
            }
        }

        /// <summary>
        /// Gets or sets a DotSpatial.Data.CacheTypes enumeration.  If the value
        /// is dynamic, then NumParts will be read from the geometry on this feature.
        /// If it is cached, then the value is separate from the geometry.
        /// </summary>
        public CacheTypes NumPartsSource
        {
            get { return _numPartsSource; }
            set { _numPartsSource = value; }
        }

        /// <summary>
        /// This specifies the offset, if any in the data file
        /// </summary>
        public long Offset
        {
            get
            {
                return _offset;
            }
            protected set
            {
                _offset = value;
            }
        }

        #region IFeature Members

        /// <summary>
        /// Gets or sets a valid IBasicGeometry associated with the data elements of this feature.
        /// This will be enough geometry information to cast into a full fledged geometry
        /// that can be used in coordination with DotSpatial.Analysis
        /// </summary>
        public virtual IBasicGeometry BasicGeometry
        {
            get
            {
                return _basicGeometry;
            }
            set
            {
                _basicGeometry = value;
                EnvelopeSource = CacheTypes.Cached;
            }
        }

        /// <summary>
        /// If the geometry for this shape was loaded from a file, this contains the size
        /// of this shape in 16-bit words as per the ESRI Shapefile specification.
        /// </summary>
        public int ContentLength
        {
            get { return _contentLength; }
            set { _contentLength = value; }
        }

        /// <summary>
        /// Returns an array of coordinates corresponding to the basic feature.
        /// </summary>
        public virtual IList<Coordinate> Coordinates
        {
            get
            {
                if (_basicGeometry == null) return null;
                return _basicGeometry.Coordinates;
            }
            set
            {
                if (_basicGeometry == null) return;
                _basicGeometry.Coordinates = value;
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
        /// This is an envelope, but specified by the file, not by calculating from the geometry.
        /// To obtain a calculated envelope, calling the DynamicEnvelope()
        /// </summary>
        public virtual IEnvelope Envelope
        {
            get
            {
                if (_envelopSource == CacheTypes.Cached && _envelope != null) return _envelope;
                if (BasicGeometry == null) return _envelope;
                _envelope = BasicGeometry.Envelope;
                return _envelope;
            }
            set
            {
                // Setting the geometry envelope may not even be possible. Setting this automatically caches an envelope.
                _envelopSource = CacheTypes.Cached;
                _envelope = value;
            }
        }

        /// <summary>
        /// Gets or sets a DotSpatial.Data.CacheTypes enumeration specifying whether the Envelope property
        /// returns a cached value in this object or is retrieved directly from the geometry.  The
        /// initial case for Shapefiles is to use a cache.  Setting the envelope assumes that you
        /// are going to use a cached value and will set this to Cached.  Setting this to Dynamic
        /// will cause the Envelope property to reference the geometry.
        /// </summary>
        public virtual CacheTypes EnvelopeSource
        {
            get { return _envelopSource; }
            set
            {
                _envelopSource = value;
                if (_basicGeometry != null)
                {
                    _envelope = _envelopSource == CacheTypes.Dynamic ? _basicGeometry.Envelope : _basicGeometry.Envelope.Copy();
                }
                else
                {
                    _envelope = new Envelope();
                }
            }
        }

        /// <summary>
        /// Returns either Point, Polygon or Line
        /// </summary>
        public FeatureType FeatureType
        {
            get
            {
                if (_basicGeometry == null) return FeatureType.Unspecified;
                return _basicGeometry.FeatureType;
            }
        }

        /// <summary>
        /// Gets the key that is associated with this feature.  This returns -1 if
        /// this feature is not a member of a feature layer.
        /// </summary>
        public virtual int Fid
        {
            get
            {
                if (_parentFeatureSet.AttributesPopulated == false)
                {
                    return _recordNumber;
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
            get { return _recordNumber; }
            set { _recordNumber = value; }
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
            get { return _shapeType; }
            set { _shapeType = value; }
        }

        /// <summary>
        /// This is simply a quick access to the Vertices list for this specific
        /// feature.  If the Vertices have not yet been defined, this will be null.
        /// </summary>
        public ShapeRange ShapeIndex
        {
            get { return _shapeIndex; }
            set { _shapeIndex = value; }
        }

        #endregion
    }
}