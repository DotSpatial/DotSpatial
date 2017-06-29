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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/6/2010 11:28:53 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Topology;
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Data
{
    /// <summary>
    /// The shape caries information about the raw vertices as well as a shapeRange.
    /// It is effectively away to move around a single shape.
    /// </summary>
    public class Shape : ICloneable
    {
        #region Private Variables

        private object[] _attributes;
        private double[] _m;
        private ShapeRange _shapeRange;
        private double[] _vertices;
        private double[] _z;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Shape
        /// </summary>
        public Shape()
        {
        }

        /// <summary>
        /// Creates a new shape type where the shaperange exists and has a type specified
        /// </summary>
        /// <param name="type"></param>
        public Shape(FeatureType type)
        {
            _shapeRange = new ShapeRange(type);
        }

        /// <summary>
        /// Creates a shape based on the specified feature.  This shape will be standing alone,
        /// all by itself.  The fieldnames and field types will be null.
        /// </summary>
        /// <param name="feature"></param>
        public Shape(IFeature feature)
            : this((IBasicGeometry)feature)
        {
            if (Equals(feature, null))
                throw new ArgumentNullException("feature");
            if (feature.NumPoints == 0)
                throw new ArgumentOutOfRangeException("feature", "The IFeature.NumPoints of the parameter feature must be greater than 0.");
            _attributes = feature.DataRow.ItemArray;
        }

        /// <summary>
        /// Creates a shape based on the specified geometry.  This shape will be standing alone,
        /// all by itself.  The attributes will be null.
        /// </summary>
        /// <param name="geometry">The geometry to create a shape from.</param>
        public Shape(IBasicGeometry geometry)
        {
            if (Equals(geometry, null))
                throw new ArgumentNullException("geometry");

            var coords = geometry.Coordinates;
            _vertices = new double[geometry.NumPoints * 2];
            _z = new double[geometry.NumPoints];
            _m = new double[geometry.NumPoints];
            for (var i = 0; i < coords.Count; i++)
            {
                var c = coords[i];
                _vertices[i * 2] = c.X;
                _vertices[i * 2 + 1] = c.Y;
                _z[i] = c.Z;
                _m[i] = c.M;
            }
            _shapeRange = ShapeRangeFromGeometry(geometry, _vertices, 0);
        }

        /// <summary>
        /// Creates a point shape from a coordinate
        /// </summary>
        /// <param name="coord"></param>
        public Shape(Coordinate coord)
        {
            if (Equals(coord, null))
                throw new ArgumentNullException("coord");
            
            if (!double.IsNaN(coord.Z))
            {
                _z = new[] {coord.Z};
            }
            if (!double.IsNaN(coord.M))
            {
                _m = new[] {coord.M};
            }

            _shapeRange = new ShapeRange(FeatureType.Point);
            _vertices = new [] {coord.X, coord.Y};
            _shapeRange.Parts.Add(new PartRange(_vertices, 0, 0, FeatureType.Point) {NumVertices = 1});
            _shapeRange.Extent = new Extent(coord.X, coord.Y, coord.X, coord.Y);
        }

        /// <summary>
        /// Creates a point shape from a vertex
        /// </summary>
        /// <param name="coord"></param>
        public Shape(Vertex coord)
        {
            _shapeRange = new ShapeRange(FeatureType.Point);
            _vertices = new [] { coord.X, coord.Y };
            _shapeRange.Parts.Add(new PartRange(_vertices, 0, 0, FeatureType.Point) {NumVertices = 1});
            _shapeRange.Extent = new Extent(coord.X, coord.Y, coord.X, coord.Y);
        }

        /// <summary>
        /// Creates a clockwise polygon shape from an extent
        /// </summary>
        /// <param name="extent"></param>
        public Shape(IExtent extent)
        {
            if (Equals(extent, null))
                throw new ArgumentNullException("extent");
            
            _shapeRange = new ShapeRange(FeatureType.Polygon);
            double xMin = extent.MinX;
            double yMin = extent.MinY;
            double xMax = extent.MaxX;
            double yMax = extent.MaxY;
            _vertices = new[] { xMin, yMax, xMax, yMax, xMax, yMin, xMin, yMin };
            _shapeRange.Parts.Add(new PartRange(_vertices, 0, 0, FeatureType.Polygon) {NumVertices = 4});
        }

        /// <summary>
        /// Creates a clockwise polygon shape from an envelope
        /// </summary>
        /// <param name="envelope"></param>
        public Shape(IEnvelope envelope)
        {
            if (Equals(envelope, null))
                throw new ArgumentNullException("envelope");

            _shapeRange = new ShapeRange(FeatureType.Polygon);
            double xMin = envelope.Minimum.X;
            double yMin = envelope.Minimum.Y;
            double xMax = envelope.Maximum.X;
            double yMax = envelope.Maximum.Y;
            _vertices = new[] {xMin, yMax, xMax, yMax, xMax, yMin, xMin, yMin};
            _shapeRange.Parts.Add(new PartRange(_vertices, 0, 0, FeatureType.Polygon) {NumVertices = 4});
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts this shape into a Geometry using the default factory.
        /// </summary>
        /// <returns>The geometry version of this shape.</returns>
        public IGeometry ToGeometry()
        {
            return ToGeometry(Geometry.DefaultFactory);
        }

        /// <summary>
        /// Converts this shape into a Geometry.
        /// </summary>
        /// <returns>The geometry version of this shape.</returns>
        public IGeometry ToGeometry(IGeometryFactory factory)
        {
            if (_shapeRange.FeatureType == FeatureType.Polygon)
            {
                return FromPolygon(factory);
            }
            if (_shapeRange.FeatureType == FeatureType.Line)
            {
                return FromLine(factory);
            }
            if (_shapeRange.FeatureType == FeatureType.MultiPoint)
            {
                return FromMultiPoint(factory);
            }
            if (_shapeRange.FeatureType == FeatureType.Point)
            {
                return FromPoint(factory);
            }
            return null;
        }

        /// <summary>
        /// Get the point for this shape if this is a point shape.
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        protected IGeometry FromPoint(IGeometryFactory factory)
        {
            if (factory == null) factory = Geometry.DefaultFactory;
            foreach (PartRange part in _shapeRange.Parts)
            {
                foreach (Vertex vertex in part)
                {
                    var c = new Coordinate(vertex.X, vertex.Y);
                    return factory.CreatePoint(c);
                }
            }
            return null;
        }

        /// <summary>
        /// Creates a new MultiPoint geometry from a MultiPoint shape
        /// </summary>
        /// <param name="factory">The IGeometryFactory to use to create the new shape.</param>
        /// <returns></returns>
        protected IGeometry FromMultiPoint(IGeometryFactory factory)
        {
            if (factory == null) factory = Geometry.DefaultFactory;
            var coords = new List<Coordinate>();
            foreach (var part in _shapeRange.Parts)
            {
                GetCoordinates(part, coords);
            }
            return factory.CreateMultiPoint(coords);
        }

        /// <summary>
        /// Gets the line for the specified index
        /// </summary>
        /// <returns>A LineString or MultiLineString geometry created from this shape.</returns>
        protected IGeometry FromLine(IGeometryFactory factory)
        {
            if (factory == null) factory = Geometry.DefaultFactory;
            var lines = new List<IBasicLineString>();
            foreach (var part in _shapeRange.Parts)
            {
                var coords = GetCoordinates(part);
                lines.Add(factory.CreateLineString(coords));
            }
            if (lines.Count == 1) return (IGeometry)lines[0];
            return factory.CreateMultiLineString(lines.ToArray());
        }

        private List<Coordinate> GetCoordinates(VertexRange part,  List<Coordinate> coords = null)
        {
            if (coords == null)
            {
                coords = new List<Coordinate>();
            }
            int i = part.StartIndex;
            foreach (var d in part)
            {
                var c = new Coordinate(d.X, d.Y);
                if (M != null && M.Length > 0) c.M = M[i];
                if (Z != null && Z.Length > 0) c.Z = Z[i];
                i++;
                coords.Add(c);
            }
            return coords;
        }

        /// <summary>
        /// Creates a Polygon or MultiPolygon from this Polygon shape.
        /// </summary>
        /// <param name="factory">The IGeometryFactory to use to create the new IGeometry.</param>
        /// <returns>The IPolygon or IMultiPolygon created from this shape.</returns>
        protected IGeometry FromPolygon(IGeometryFactory factory)
        {
            if (factory == null) factory = Geometry.DefaultFactory;
            List<ILinearRing> shells = new List<ILinearRing>();
            List<ILinearRing> holes = new List<ILinearRing>();
            foreach (var part in _shapeRange.Parts)
            {
                var coords = GetCoordinates(part);
                var ring = factory.CreateLinearRing(coords);
                if (_shapeRange.Parts.Count == 1)
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
                }
            }
            //// Now we have a list of all shells and all holes
            List<ILinearRing>[] holesForShells = new List<ILinearRing>[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                holesForShells[i] = new List<ILinearRing>();
            }

            // Find holes
            foreach (ILinearRing t in holes)
            {
                ILinearRing testRing = t;
                ILinearRing minShell = null;
                IEnvelope minEnv = null;
                IEnvelope testEnv = testRing.EnvelopeInternal;
                Coordinate testPt = testRing.Coordinates[0];
                for (int j = 0; j < shells.Count; j++)
                {
                    ILinearRing tryRing = shells[j];
                    IEnvelope tryEnv = tryRing.EnvelopeInternal;
                    if (minShell != null)
                        minEnv = minShell.EnvelopeInternal;
                    var isContained = tryEnv.Contains(testEnv)
                                       && (CgAlgorithms.IsPointInRing(testPt, tryRing.Coordinates)
                                           || (PointInList(testPt, tryRing.Coordinates)));

                    // Check if this new containing ring is smaller than the current minimum ring
                    if (isContained)
                    {
                        if (minShell == null || minEnv.Contains(tryEnv))
                        {
                            minShell = tryRing;
                        }
                        holesForShells[j].Add(t);
                    }
                }
            }

            var polygons = new IPolygon[shells.Count];
            for (int i = 0; i < shells.Count; i++)
            {
                polygons[i] = factory.CreatePolygon(shells[i], holesForShells[i].ToArray());
            }

            if (polygons.Length == 1)
            {
                return polygons[0];
            }
            // It's a multi part
            return factory.CreateMultiPolygon(polygons);
        }

        /// <summary>
        /// Test if a point is in a list of coordinates.
        /// </summary>
        /// <param name="testPoint">TestPoint the point to test for.</param>
        /// <param name="pointList">PointList the list of points to look through.</param>
        /// <returns>true if testPoint is a point in the pointList list.</returns>
        private static bool PointInList(Coordinate testPoint, IEnumerable<Coordinate> pointList)
        {
            return pointList.Any(p => p.Equals2D(testPoint));
        }

        /// <summary>
        /// Copies the field names and types from the parent feature set if they are currently null.
        /// Attempts to copy the members of the feature's datarow.  This assumes the features have been
        /// loaded into memory and are available on the feature's DataRow property.
        /// </summary>
        /// <param name="feature">An IFeature to copy the attributes from.  If the schema is null, this will try to use the parent featureset schema.</param>
        public void CopyAttributes(IFeature feature)
        {
            object[] dr = feature.DataRow.ItemArray;
            _attributes = new object[dr.Length];
            Array.Copy(dr, _attributes, dr.Length);
        }

        /// <summary>
        /// Create a ShapeRange from a Geometry to use in constructing a Shape
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="vertices"></param>
        /// <param name="offset">offset into vertices array where this feature starts</param>
        /// <returns></returns>
        public static ShapeRange ShapeRangeFromGeometry(IBasicGeometry geometry, double[] vertices, int offset)
        {
            var featureType = geometry.FeatureType;
            ShapeRange shx = new ShapeRange(featureType) { Extent = new Extent(geometry.Envelope) };
            int vIndex = offset / 2;
            int shapeStart = vIndex;
            for (int part = 0; part < geometry.NumGeometries; part++)
            {
                PartRange prtx = new PartRange(vertices, shapeStart, vIndex - shapeStart, featureType);
                IBasicPolygon bp = geometry.GetBasicGeometryN(part) as IBasicPolygon;
                if (bp != null)
                {
                    // Account for the Shell
                    prtx.NumVertices = bp.Shell.NumPoints;

                    vIndex += bp.Shell.NumPoints;

                    // The part range should be adjusted to no longer include the holes
                    foreach (var hole in bp.Holes)
                    {
                        PartRange holex = new PartRange(vertices, shapeStart, vIndex - shapeStart, featureType)
                        {
                            NumVertices = hole.NumPoints
                        };
                        shx.Parts.Add(holex);
                        vIndex += hole.NumPoints;
                    }
                }
                else
                {
                    int numPoints = geometry.GetBasicGeometryN(part).NumPoints;

                    // This is not a polygon, so just add the number of points.
                    vIndex += numPoints;
                    prtx.NumVertices = numPoints;
                }

                shx.Parts.Add(prtx);
            }
            return shx;
        }

        /// <summary>
        /// Create a ShapeRange from a Feature to use in constructing a Shape
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="vertices"></param>
        /// <param name="offset">offset into vertices array where this feature starts</param>
        /// <returns></returns>
        public static ShapeRange ShapeRangeFromFeature(IFeature feature, double[] vertices, int offset)
        {
            return ShapeRangeFromGeometry(feature.BasicGeometry, vertices, offset);
        }

        /// <summary>
        /// Create a ShapeRange from a Feature to use in constructing a Shape
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static ShapeRange ShapeRangeFromFeature(IFeature feature, double[] vertices)
        {
            return ShapeRangeFromFeature(feature, vertices, 0);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the minimum M
        /// </summary>
        public double MinM { get; set; }

        /// <summary>
        ///  Gets or sets the maximum M
        /// </summary>
        public double MaxM { get; set; }

        /// <summary>
        ///  Gets or sets the minimum Z
        /// </summary>
        public double MinZ { get; set; }

        /// <summary>
        ///  Gets or sets the maximum Z
        /// </summary>
        public double MaxZ { get; set; }

        /// <summary>
        /// Gives a way to cycle through the vertices of this shape.
        /// </summary>
        public ShapeRange Range
        {
            get { return _shapeRange; }
            set { _shapeRange = value; }
        }

        /// <summary>
        /// The double vertices in X1, Y1, X2, Y2, ..., Xn, Yn order.
        /// </summary>
        public double[] Vertices
        {
            get { return _vertices; }
            set
            {
                _vertices = value;
                foreach (PartRange part in _shapeRange.Parts)
                {
                    part.Vertices = value;
                }
            }
        }

        /// <summary>
        /// The Z values if any
        /// </summary>
        public double[] Z
        {
            get { return _z; }
            set { _z = value; }
        }

        /// <summary>
        /// The M values if any, organized in order.
        /// </summary>
        public double[] M
        {
            get { return _m; }
            set { _m = value; }
        }

        /// <summary>
        /// Gets or sets the attributes.  Since the most likely use is to copy values from one source to
        /// another, this should be an independant array in each shape and be deep-copied.
        /// </summary>
        public object[] Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        /// <summary>
        /// Without changing the feature type or anything else, simply update the local coordinates
        /// to include the new coordinates.  All the new coordinates will be considered one part.
        /// Since point and multi-point shapes don't have parts, they will just be appended to the
        /// original part.
        /// </summary>
        public void AddPart(IEnumerable<Coordinate> coordinates, CoordinateType coordType)
        {
            bool hasM = (coordType == CoordinateType.M || coordType == CoordinateType.Z);
            bool hasZ = (coordType == CoordinateType.Z);
            List<double> vertices = new List<double>();
            List<double> z = new List<double>();
            List<double> m = new List<double>();
            int numPoints = 0;
            int oldNumPoints = (_vertices != null) ? _vertices.Length / 2 : 0;
            foreach (Coordinate coordinate in coordinates)
            {
                if (_shapeRange.Extent == null) _shapeRange.Extent = new Extent();
                _shapeRange.Extent.ExpandToInclude(coordinate.X, coordinate.Y);
                vertices.Add(coordinate.X);
                vertices.Add(coordinate.Y);
                if (hasM) m.Add(coordinate.M);
                if (hasZ) z.Add(coordinate.Z);
                numPoints++;
            }
            // Using public accessor also updates individual part references
            Vertices = (_vertices != null) ? _vertices.Concat(vertices).ToArray() : vertices.ToArray();
            if (hasZ) _z = (_z != null) ? _z.Concat(z).ToArray() : z.ToArray();
            if (hasM) _m = (_m != null) ? _m.Concat(m).ToArray() : m.ToArray();

            if (_shapeRange.FeatureType == FeatureType.MultiPoint || _shapeRange.FeatureType == FeatureType.Point)
            {
                // Only one part exists
                _shapeRange.Parts[0].NumVertices += numPoints;
            }
            else
            {
                PartRange part = new PartRange(_vertices, _shapeRange.StartIndex, oldNumPoints, _shapeRange.FeatureType);
                part.NumVertices = numPoints;
                _shapeRange.Parts.Add(part);
            }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// This creates a duplicate shape, also copying the vertex array to
        /// a new array containing just this shape, as well as duplicating the attribute array.
        /// The FieldNames and FieldTypes are a shallow copy since this shouldn't change.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            if (_shapeRange == null) return new Shape();

            Shape copy = (Shape)MemberwiseClone();
            int numPoints = _shapeRange.NumPoints;
            int start = _shapeRange.StartIndex;

            copy.Range = _shapeRange.Copy();
            // Be sure to set vertex array AFTER the shape range to update part indices correctly.
            double[] verts = new double[numPoints * 2];
            Array.Copy(_vertices, start, verts, 0, numPoints * 2);
            copy.Vertices = verts;
            if (_z != null && (_z.Length - start) >= numPoints)
            {
                copy.Z = new double[numPoints];
                Array.Copy(_z, start, copy._z, 0, numPoints);
            }
            if (_m != null && (_m.Length - start) >= numPoints)
            {
                copy.M = new double[numPoints];
                Array.Copy(_m, start, copy._m, 0, numPoints);
            }
            // Update the start-range to work like a stand-alone shape.
            copy.Range.StartIndex = 0;

            // Copy the attributes (handling the null case)
            if (null == _attributes)
                copy.Attributes = null;
            else
            {
                copy.Attributes = new object[_attributes.Length];
                Array.Copy(_attributes, 0, copy._attributes, 0, _attributes.Length);
            }
            return copy;
        }

        #endregion
    }
}