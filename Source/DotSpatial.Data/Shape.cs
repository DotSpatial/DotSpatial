// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
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
using DotSpatial.Serialization;
using GeoAPI.Geometries;
using NetTopologySuite.Algorithm;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// The shape caries information about the raw vertices as well as a shapeRange.
    /// It is effectively away to move around a single shape.
    /// </summary>
    public class Shape : ICloneable
    {
        #region Fields

        private double[] _vertices;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape"/> class.
        /// </summary>
        public Shape()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape"/> class where the shaperange exists and has a type specified.
        /// </summary>
        /// <param name="featureType">Feature type of the shape range.</param>
        public Shape(FeatureType featureType)
        {
            Range = new ShapeRange(featureType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape"/> class based on the specified feature.
        /// This shape will be standing alone, all by itself. The fieldnames and field types will be null.
        /// </summary>
        /// <param name="feature">Feature used for creating the shape.</param>
        public Shape(IFeature feature)
            : this(feature.Geometry, feature.FeatureType)
        {
            if (Equals(feature, null)) throw new ArgumentNullException(nameof(feature));
            if (feature.Geometry.NumPoints == 0) throw new ArgumentOutOfRangeException(nameof(feature), DataStrings.Shape_ZeroPointsError);

            if (feature.DataRow != null) Attributes = feature.DataRow.ItemArray;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape"/> class based on the specified geometry. This shape will be standing alone,
        /// all by itself. The attributes will be null.
        /// </summary>
        /// <param name="geometry">The geometry to create a shape from.</param>
        /// <param name="featureType">Feature type of the shape range.</param>
        public Shape(IGeometry geometry, FeatureType featureType)
        {
            if (featureType == FeatureType.Unspecified)
            {
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
                    default:
                        featureType = FeatureType.Unspecified;
                        break;
                }
            }

            if (Equals(geometry, null)) throw new ArgumentNullException(nameof(geometry));

            var coords = geometry.Coordinates;
            _vertices = new double[geometry.NumPoints * 2];
            Z = new double[geometry.NumPoints];
            M = new double[geometry.NumPoints];
            for (var i = 0; i < coords.Length; i++)
            {
                var c = coords[i];
                _vertices[i * 2] = c.X;
                _vertices[i * 2 + 1] = c.Y;
                Z[i] = c.Z;
                M[i] = c.M;
            }

            Range = ShapeRangeFromGeometry(geometry, featureType, _vertices, 0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape"/> class.
        /// </summary>
        /// <param name="coord">Coordinate used for creating the point shape.</param>
        public Shape(Coordinate coord)
        {
            if (Equals(coord, null)) throw new ArgumentNullException(nameof(coord));

            if (!double.IsNaN(coord.Z))
            {
                Z = new[] { coord.Z };
            }

            if (!double.IsNaN(coord.M))
            {
                M = new[] { coord.M };
            }

            Range = new ShapeRange(FeatureType.Point);
            _vertices = new[] { coord.X, coord.Y };
            Range.Parts.Add(
                new PartRange(_vertices, 0, 0, FeatureType.Point)
                {
                    NumVertices = 1
                });
            Range.Extent = new Extent(coord.X, coord.Y, coord.X, coord.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape"/> class.
        /// </summary>
        /// <param name="coord">Vertex that is used to create the point shape.</param>
        public Shape(Vertex coord)
        {
            Range = new ShapeRange(FeatureType.Point);
            _vertices = new[] { coord.X, coord.Y };
            Range.Parts.Add(
                new PartRange(_vertices, 0, 0, FeatureType.Point)
                {
                    NumVertices = 1
                });
            Range.Extent = new Extent(coord.X, coord.Y, coord.X, coord.Y);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape"/> class that is a clockwise polygon.
        /// </summary>
        /// <param name="extent">Extent that is used to create the polygon shape.</param>
        public Shape(IExtent extent)
        {
            if (Equals(extent, null)) throw new ArgumentNullException(nameof(extent));

            Range = new ShapeRange(FeatureType.Polygon);
            double xMin = extent.MinX;
            double yMin = extent.MinY;
            double xMax = extent.MaxX;
            double yMax = extent.MaxY;
            _vertices = new[] { xMin, yMax, xMax, yMax, xMax, yMin, xMin, yMin };
            Range.Parts.Add(
                new PartRange(_vertices, 0, 0, FeatureType.Polygon)
                {
                    NumVertices = 4
                });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape"/> class that is a clockwise polygon.
        /// </summary>
        /// <param name="envelope">Envelope that is used to create the polygon shape.</param>
        public Shape(Envelope envelope)
        {
            if (Equals(envelope, null)) throw new ArgumentNullException(nameof(envelope));

            Range = new ShapeRange(FeatureType.Polygon);
            double xMin = envelope.MinX;
            double yMin = envelope.MinY;
            double xMax = envelope.MaxX;
            double yMax = envelope.MaxY;
            _vertices = new[] { xMin, yMax, xMax, yMax, xMax, yMin, xMin, yMin };
            Range.Parts.Add(
                new PartRange(_vertices, 0, 0, FeatureType.Polygon)
                {
                    NumVertices = 4
                });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the attributes. Since the most likely use is to copy values from one source to
        /// another, this should be an independant array in each shape and be deep-copied.
        /// </summary>
        public object[] Attributes { get; set; }

        /// <summary>
        /// Gets or sets the M values if any, organized in order.
        /// </summary>
        public double[] M { get; set; }

        /// <summary>
        ///  Gets or sets the maximum M.
        /// </summary>
        public double MaxM { get; set; }

        /// <summary>
        ///  Gets or sets the maximum Z.
        /// </summary>
        public double MaxZ { get; set; }

        /// <summary>
        /// Gets or sets the minimum M.
        /// </summary>
        public double MinM { get; set; }

        /// <summary>
        ///  Gets or sets the minimum Z.
        /// </summary>
        public double MinZ { get; set; }

        /// <summary>
        /// Gets or sets the range. This gives a way to cycle through the vertices of this shape.
        /// </summary>
        public ShapeRange Range { get; set; }

        /// <summary>
        /// Gets or sets the double vertices in X1, Y1, X2, Y2, ..., Xn, Yn order.
        /// </summary>
        public double[] Vertices
        {
            get
            {
                return _vertices;
            }

            set
            {
                _vertices = value;
                foreach (PartRange part in Range.Parts)
                {
                    part.Vertices = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Z values if any.
        /// </summary>
        public double[] Z { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Create a ShapeRange from a Feature to use in constructing a Shape.
        /// </summary>
        /// <param name="feature">Feature to use in constructing the shape.</param>
        /// <param name="vertices">The vertices of all the features.</param>
        /// <param name="offset">offset into vertices array where this feature starts</param>
        /// <returns>The shape range constructed from the given feature.</returns>
        public static ShapeRange ShapeRangeFromFeature(IFeature feature, double[] vertices, int offset)
        {
            return ShapeRangeFromGeometry(feature.Geometry, feature.FeatureType, vertices, offset);
        }

        /// <summary>
        /// Create a ShapeRange from a Feature to use in constructing a Shape. This assumes that vertices either contains only the vertices of this shape or this is the first shape.
        /// </summary>
        /// <param name="feature">Feature to use in constructing the shape.</param>
        /// <param name="vertices">The vertices of all the features.</param>
        /// <returns>The shape range constructed from the given feature.</returns>
        public static ShapeRange ShapeRangeFromFeature(IFeature feature, double[] vertices)
        {
            return ShapeRangeFromFeature(feature, vertices, 0);
        }

        /// <summary>
        /// Create a ShapeRange from a Geometry to use in constructing a Shape.
        /// </summary>
        /// <param name="geometry">The geometry used for constructing the shape.</param>
        /// <param name="featureType">The feature type of the shape.</param>
        /// <param name="vertices">The vertices of all the features.</param>
        /// <param name="offset">offset into vertices array where this feature starts</param>
        /// <returns>The shape range constructed from the given geometry.</returns>
        public static ShapeRange ShapeRangeFromGeometry(IGeometry geometry, FeatureType featureType, double[] vertices, int offset)
        {
            ShapeRange shx = new ShapeRange(featureType)
            {
                Extent = new Extent(geometry.EnvelopeInternal)
            };
            int vIndex = offset / 2;
            int shapeStart = vIndex;
            for (int part = 0; part < geometry.NumGeometries; part++)
            {
                PartRange prtx = new PartRange(vertices, shapeStart, vIndex - shapeStart, featureType);
                IPolygon bp = geometry.GetGeometryN(part) as IPolygon;
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
                    int numPoints = geometry.GetGeometryN(part).NumPoints;

                    // This is not a polygon, so just add the number of points.
                    vIndex += numPoints;
                    prtx.NumVertices = numPoints;
                }

                shx.Parts.Add(prtx);
            }

            return shx;
        }

        /// <summary>
        /// Without changing the feature type or anything else, simply update the local coordinates
        /// to include the new coordinates. All the new coordinates will be considered one part.
        /// Since point and multi-point shapes don't have parts, they will just be appended to the
        /// original part.
        /// </summary>
        /// <param name="coordinates">Coordinates that get added.</param>
        /// <param name="coordType">Coordinate type of the coordinates.</param>
        public void AddPart(IEnumerable<Coordinate> coordinates, CoordinateType coordType)
        {
            bool hasM = coordType == CoordinateType.M || coordType == CoordinateType.Z;
            bool hasZ = coordType == CoordinateType.Z;
            List<double> vertices = new List<double>();
            List<double> z = new List<double>();
            List<double> m = new List<double>();
            int numPoints = 0;
            int oldNumPoints = _vertices?.Length / 2 ?? 0;
            foreach (Coordinate coordinate in coordinates)
            {
                if (Range.Extent == null) Range.Extent = new Extent();
                Range.Extent.ExpandToInclude(coordinate.X, coordinate.Y);
                vertices.Add(coordinate.X);
                vertices.Add(coordinate.Y);
                if (hasM) m.Add(coordinate.M);
                if (hasZ) z.Add(coordinate.Z);
                numPoints++;
            }

            // Using public accessor also updates individual part references
            Vertices = _vertices?.Concat(vertices).ToArray() ?? vertices.ToArray();
            if (hasZ) Z = Z?.Concat(z).ToArray() ?? z.ToArray();
            if (hasM) M = M?.Concat(m).ToArray() ?? m.ToArray();

            if (Range.FeatureType == FeatureType.MultiPoint || Range.FeatureType == FeatureType.Point)
            {
                // Only one part exists
                Range.Parts[0].NumVertices += numPoints;
            }
            else
            {
                PartRange part = new PartRange(_vertices, Range.StartIndex, oldNumPoints, Range.FeatureType)
                {
                    NumVertices = numPoints
                };
                Range.Parts.Add(part);
            }
        }

        /// <summary>
        /// This creates a duplicate shape, also copying the vertex array to
        /// a new array containing just this shape, as well as duplicating the attribute array.
        /// The FieldNames and FieldTypes are a shallow copy since this shouldn't change.
        /// </summary>
        /// <returns>The copy.</returns>
        public object Clone()
        {
            if (Range == null) return new Shape();

            Shape copy = (Shape)MemberwiseClone();
            int numPoints = Range.NumPoints;
            int start = Range.StartIndex;

            copy.Range = Range.Copy();

            // Be sure to set vertex array AFTER the shape range to update part indices correctly.
            double[] verts = new double[numPoints * 2];
            Array.Copy(_vertices, start, verts, 0, numPoints * 2);
            copy.Vertices = verts;
            if (Z != null && (Z.Length - start) >= numPoints)
            {
                copy.Z = new double[numPoints];
                Array.Copy(Z, start, copy.Z, 0, numPoints);
            }

            if (M != null && (M.Length - start) >= numPoints)
            {
                copy.M = new double[numPoints];
                Array.Copy(M, start, copy.M, 0, numPoints);
            }

            // Update the start-range to work like a stand-alone shape.
            copy.Range.StartIndex = 0;

            // Copy the attributes (handling the null case)
            if (Attributes == null)
            {
                copy.Attributes = null;
            }
            else
            {
                copy.Attributes = new object[Attributes.Length];
                Array.Copy(Attributes, 0, copy.Attributes, 0, Attributes.Length);
            }

            return copy;
        }

        /// <summary>
        /// Copies the field names and types from the parent feature set if they are currently null.
        /// Attempts to copy the members of the feature's datarow. This assumes the features have been
        /// loaded into memory and are available on the feature's DataRow property.
        /// </summary>
        /// <param name="feature">An IFeature to copy the attributes from. If the schema is null, this will try to use the parent featureset schema.</param>
        public void CopyAttributes(IFeature feature)
        {
            object[] dr = feature.DataRow.ItemArray;
            Attributes = new object[dr.Length];
            Array.Copy(dr, Attributes, dr.Length);
        }

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
        /// <param name="factory">The geometry factory used for creating the geometry.</param>
        /// <returns>The geometry version of this shape.</returns>
        public IGeometry ToGeometry(IGeometryFactory factory)
        {
            if (Range.FeatureType == FeatureType.Polygon)
            {
                return FromPolygon(factory);
            }

            if (Range.FeatureType == FeatureType.Line)
            {
                return FromLine(factory);
            }

            if (Range.FeatureType == FeatureType.MultiPoint)
            {
                return FromMultiPoint(factory);
            }

            if (Range.FeatureType == FeatureType.Point)
            {
                return FromPoint(factory);
            }

            return null;
        }

        /// <summary>
        /// Converts this shape to a line geometry.
        /// </summary>
        /// <param name="factory">The geometry factory used for creating the geometry.</param>
        /// <returns>A LineString or MultiLineString geometry created from this shape.</returns>
        protected IGeometry FromLine(IGeometryFactory factory)
        {
            if (factory == null) factory = Geometry.DefaultFactory;
            var lines = new List<ILineString>();
            foreach (var part in Range.Parts)
            {
                var coords = GetCoordinates(part);
                lines.Add(factory.CreateLineString(coords.ToArray()));
            }

            if (lines.Count == 1) return lines[0];

            return factory.CreateMultiLineString(lines.ToArray());
        }

        /// <summary>
        /// Creates a new MultiPoint geometry from a MultiPoint shape.
        /// </summary>
        /// <param name="factory">The IGeometryFactory to use to create the new shape.</param>
        /// <returns>The resulting multipoint.</returns>
        protected IGeometry FromMultiPoint(IGeometryFactory factory)
        {
            if (factory == null) factory = Geometry.DefaultFactory;
            var coords = new List<Coordinate>();
            foreach (var part in Range.Parts)
            {
                GetCoordinates(part, coords);
            }

            return factory.CreateMultiPoint(coords.ToArray());
        }

        /// <summary>
        /// Get the point for this shape if this is a point shape.
        /// </summary>
        /// <param name="factory">The geometry factory used for creating the geometry.</param>
        /// <returns>The resulting point geometry.</returns>
        protected IGeometry FromPoint(IGeometryFactory factory)
        {
            if (factory == null) factory = Geometry.DefaultFactory;
            foreach (PartRange part in Range.Parts)
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
        /// Creates a Polygon or MultiPolygon from this Polygon shape.
        /// </summary>
        /// <param name="factory">The IGeometryFactory to use to create the new IGeometry.</param>
        /// <returns>The IPolygon or IMultiPolygon created from this shape.</returns>
        protected IGeometry FromPolygon(IGeometryFactory factory)
        {
            if (factory == null) factory = Geometry.DefaultFactory;
            List<ILinearRing> shells = new List<ILinearRing>();
            List<ILinearRing> holes = new List<ILinearRing>();
            foreach (var part in Range.Parts)
            {
                var coords = GetCoordinates(part);
                var ring = factory.CreateLinearRing(coords.ToArray());
                if (Range.Parts.Count == 1)
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

            // Now we have a list of all shells and all holes
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
                Envelope minEnv = null;
                Envelope testEnv = testRing.EnvelopeInternal;
                Coordinate testPt = testRing.Coordinates[0];
                for (int j = 0; j < shells.Count; j++)
                {
                    ILinearRing tryRing = shells[j];
                    Envelope tryEnv = tryRing.EnvelopeInternal;
                    if (minShell != null) minEnv = minShell.EnvelopeInternal;
                    var isContained = tryEnv.Contains(testEnv) && (CGAlgorithms.IsPointInRing(testPt, tryRing.Coordinates) || PointInList(testPt, tryRing.Coordinates));

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

        private List<Coordinate> GetCoordinates(VertexRange part, List<Coordinate> coords = null)
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

        #endregion
    }
}