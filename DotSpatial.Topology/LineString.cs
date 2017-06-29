// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Operation;

namespace DotSpatial.Topology
{
    /// <summary>
    /// Basic implementation of <c>LineString</c>.
    /// </summary>
    [Serializable]
    public class LineString : Geometry, ILineString
    {
        #region Variables

        /// <summary>
        /// Represents an empty <c>LineString</c>.
        /// </summary>
        public static readonly ILineString Empty = new GeometryFactory().CreateLineString(new Coordinate[] { });

        /// <summary>
        /// The points of this <c>LineString</c>.
        /// </summary>
        private IList<Coordinate> _points;

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="points">
        /// The points of the linestring, or <c>null</c>
        /// to create the empty point. Consecutive points may not be equal.
        /// </param>
        /// <param name="factory"></param>
        public LineString(IList<Coordinate> points, IGeometryFactory factory)
            : base(factory)
        {
            if (points == null)
                points = new Coordinate[] { };
            if (points.Count == 1)
                throw new ArgumentException("point array must contain 0 or > 1 elements");
            _points = points;
        }

        /// <summary>
        /// Creates a new LineString using the default factory
        /// </summary>
        /// <param name="points">
        /// The points of the linestring, or <c>null</c>
        /// to create the empty point. Consecutive points may not be equal.
        /// </param>
        public LineString(IList<Coordinate> points)
            : base(DefaultFactory)
        {
            if (points == null)
                points = new Coordinate[] { };
            if (points.Count == 1)
                throw new ArgumentException("point array must contain 0 or >1 elements");
            _points = points;
        }

        /// <summary>
        /// Creates a new topologically complete LineString from a LineStringBase
        /// </summary>
        /// <param name="lineStringBase"></param>
        public LineString(IBasicLineString lineStringBase)
            : base(DefaultFactory)
        {
            if (lineStringBase.NumPoints == 0)
                _points = new Coordinate[] { };
            if (lineStringBase.NumPoints == 1)
                throw new ArgumentException("point array must contain 0 or > 1 elements");
            _points = lineStringBase.Coordinates;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lineString">
        /// The points of the linestring, or <c>null</c>
        /// to create the empty point. Consecutive points may not be equal.
        /// </param>
        /// <param name="factory"></param>
        public LineString(IBasicLineString lineString, IGeometryFactory factory)
            : base(factory)
        {
            if (lineString.Coordinates == null)
                _points = new Coordinate[] { };
            if (lineString.NumPoints == 1)
                throw new ArgumentException("point array must contain 0 or >1 elements");
            _points = lineString.Coordinates;
        }

        /* BEGIN ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Creates an empty linestring using the specified factory.
        /// </summary>
        /// <param name="factory">An IGeometryFactory to use when specifying this linestring.</param>
        public LineString(IGeometryFactory factory)
            : base(factory)
        {
            _points = new Coordinate[] { };
        }

        /* END ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Rather than using the factory trends, this will create a coordinate sequence by simply using the specified list of coordinates.
        /// </summary>
        /// <param name="coordinates">The list of coordinates to use as a new ICoordinateSequence</param>
        public LineString(IEnumerable<Coordinate> coordinates)
            : base(DefaultFactory)
        {
            if (coordinates == null)
            {
                _points = new List<Coordinate>();
                return;
            }
            _points = coordinates as IList<Coordinate>;
            if (_points != null) return;
            _points = new List<Coordinate>();
            foreach (Coordinate c in coordinates)
            {
                _points.Add(c);
            }
        }

        /// <summary>
        /// Rather than using the factory trends, this will create a coordinate sequence by simply using the specified list of coordinates.
        /// </summary>
        /// <param name="coordinates">The list of coordinates to use as a new ICoordinateSequence</param>
        public LineString(IEnumerable<ICoordinate> coordinates)
            : base(DefaultFactory)
        {
            if (coordinates == null)
            {
                _points = new List<Coordinate>();
                return;
            }
            _points = coordinates as IList<Coordinate>;
            if (_points != null) return;
            _points = new List<Coordinate>();
            foreach (ICoordinate c in coordinates)
            {
                _points.Add(new Coordinate(c));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applys a given ICoordinateFilter to this LineString
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(ICoordinateFilter filter)
        {
            foreach (Coordinate c in _points)
            {
                filter.Filter(c);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(IGeometryFilter filter)
        {
            filter.Filter(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filter"></param>
        public override void Apply(IGeometryComponentFilter filter)
        {
            filter.Filter(this);
        }

        /// <summary>
        /// Performs a CompareTo opperation assuming that the specified object is a LineString
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override int CompareToSameClass(object o)
        {
            LineString line = o as LineString;
            // MD - optimized implementation
            int i = 0;
            int j = 0;
            if (line != null)
            {
                while (i < _points.Count && j < line._points.Count)
                {
                    int comparison = _points[i].CompareTo(line._points[j]);
                    if (comparison != 0)
                        return comparison;
                    i++;
                    j++;
                }
            }
            if (i < _points.Count)
                return 1;
            if (line != null)
            {
                if (j < line._points.Count)
                    return -1;
            }
            return 0;
        }

        /// <summary>
        /// Tests the coordinates of this LineString against another geometry and returns true if they are identical.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public override bool EqualsExact(IGeometry other, double tolerance)
        {
            if (!IsEquivalentClass(other))
                return false;

            LineString otherLineString = (LineString)other;
            if (_points.Count != otherLineString._points.Count)
                return false;

            for (int i = 0; i < _points.Count; i++)
                if (!Equal(new Coordinate(_points[i]), otherLineString._points[i], tolerance))
                    return false;
            return true;
        }

        /// <summary>
        /// Given the specified test point, this checks each segment, and will
        /// return the closest point on the specified segment.
        /// </summary>
        /// <param name="testPoint">The point to test.</param>
        /// <returns></returns>
        public override Coordinate ClosestPoint(Coordinate testPoint)
        {
            Coordinate closest = Coordinate;
            double dist = double.MaxValue;
            for (int i = 0; i < _points.Count - 1; i++)
            {
                LineSegment s = new LineSegment(_points[i], _points[i + 1]);
                Coordinate temp = s.ClosestPoint(testPoint);
                double tempDist = testPoint.Distance(temp);
                if (tempDist < dist)
                {
                    dist = tempDist;
                    closest = temp;
                }
            }
            return closest;
        }

        /// <summary>
        /// Returns the N'th point as an Implementation of IPoint.  The specific
        /// implementation is just the DotSpatial.Geometries.Point
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public virtual IPoint GetPointN(int n)
        {
            return new Point(_points[n]);
        }

        /// <summary>
        /// Returns true if the given point is a vertex of this <c>LineString</c>.
        /// </summary>
        /// <param name="pt">The <c>Coordinate</c> to check.</param>
        /// <returns><c>true</c> if <c>pt</c> is one of this <c>LineString</c>'s vertices.</returns>
        public virtual bool IsCoordinate(Coordinate pt)
        {
            Coordinate coord = new Coordinate(pt);
            for (int i = 0; i < _points.Count; i++)
                if (coord == _points[i])
                    return true;
            return false;
        }

        /// <summary>
        /// Normalizes a <c>LineString</c>.  A normalized linestring
        /// has the first point which is not equal to it's reflected point
        /// less than the reflected point.
        /// </summary>
        public override void Normalize()
        {
            for (int i = 0; i < _points.Count / 2; i++)
            {
                int j = _points.Count - 1 - i;
                // skip equal points on both ends
                if (!_points[i].Equals(_points[j]))
                {
                    if (_points[i].CompareTo(_points[j]) > 0)
                        _points.Reverse();
                    return;
                }
            }
        }

        /// <summary>
        /// Creates a <see cref="LineString" /> whose coordinates are in the reverse order of this objects.
        /// </summary>
        /// <returns>A <see cref="LineString" /> with coordinates in the reverse order.</returns>
        public virtual ILineString Reverse()
        {
            List<Coordinate> result = Coordinates.CloneList();
            result.Reverse();
            return new LineString(result);
        }

        /// <summary>
        /// Returns the Envelope of this LineString
        /// </summary>
        /// <returns>An IEnvelope interface for the envelope containing this LineString</returns>
        protected override IEnvelope ComputeEnvelopeInternal()
        {
            if (IsEmpty)
                return new Envelope();

            //Convert to array, then access array directly, to avoid the function-call overhead
            //of calling Getter millions of times. ToArray may be inefficient for
            //non-BasicCoordinateSequence CoordinateSequences. [Jon Aquino]

            double minx = _points[0].X;
            double miny = _points[0].Y;
            double maxx = _points[0].X;
            double maxy = _points[0].Y;
            for (int i = 1; i < _points.Count; i++)
            {
                minx = minx < _points[i].X ? minx : _points[i].X;
                maxx = maxx > _points[i].X ? maxx : _points[i].X;
                miny = miny < _points[i].Y ? miny : _points[i].Y;
                maxy = maxy > _points[i].Y ? maxy : _points[i].Y;
            }
            return new Envelope(minx, maxx, miny, maxy);
        }

        /// <summary>
        /// Returns a copy of this ILineString
        /// </summary>
        protected override void OnCopy(Geometry copy)
        {
            base.OnCopy(copy);
            LineString ls = copy as LineString;
            if (ls == null) return;
            ls.Coordinates = new List<Coordinate>();
            foreach (Coordinate coordinate in _points)
            {
                ls.Coordinates.Add(coordinate);
            }
        }

        /// <summary>
        /// Returns the Nth coordinate of the coordinate sequence making up this LineString
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public virtual Coordinate GetCoordinateN(int n)
        {
            return _points[n];
        }

        /// <summary>
        /// Tests other and returns true if other is a LineString
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected override bool IsEquivalentClass(IGeometry other)
        {
            return other is LineString;
        }

        /// <summary>
        /// Calculates a new linestring representing a linestring that is offset by
        /// distance to the left.  Negative distances will be to the right.  The final
        /// LineString may be shorter or longer than the original.  Left is determined
        /// by the vector direction of the segment between the 0th and 1st points.
        /// Outside bends will be circular curves, rather than extended angles.
        /// </summary>
        /// <param name="distance">The double distance to create the offset LineString</param>
        /// <returns>A valid ILineString interface created from calculations performed on this LineString</returns>
        public ILineString Offset(double distance)
        {
            if (distance == 0) return this.Copy();

            throw new NotImplementedException("This is not yet implemented");

            // The problem with drawing it segment by segment is that segments further down can
            // interfere with the propper pathway.  Trying to clip the boundary of the buffer instead.

            // The problem with doing a buffer is that you are always left with an outer polygon,
            // and complex shapes will produce "unioned" buffer results that are incorrect from
            // the standpoint of drawing an offset.
        }

        #endregion

        #region Properties

        /* BEGIN ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Gets the integer count of the number of points in this LineString
        /// </summary>
        /// <value></value>
        public virtual int Count
        {
            get
            {
                return _points.Count;
            }
        }

        /// <summary>
        /// Gets the ICoordinate that exists at the Nth index
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public virtual Coordinate this[int n]
        {
            get
            {
                return _points[n];
            }
            set
            {
                _points[n] = value;
            }
        }

        /// <summary>
        /// Gets the value of the angle between the <see cref="StartPoint" />
        /// and the <see cref="EndPoint" />.
        /// </summary>
        public virtual double Angle
        {
            get
            {
                double deltaX = EndPoint.X - StartPoint.X;
                double deltaY = EndPoint.Y - StartPoint.Y;
                double length = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                double angleRAD = Math.Asin(Math.Abs(EndPoint.Y - StartPoint.Y) / length);
                double angle = (angleRAD * 180) / Math.PI;

                if (((StartPoint.X < EndPoint.X) && (StartPoint.Y > EndPoint.Y)) ||
                    ((StartPoint.X > EndPoint.X) && (StartPoint.Y < EndPoint.Y)))
                    angle = 360 - angle;
                return angle;
            }
        }

        /* END ADDED BY MPAUL42: monoGIS team */

        /// <summary>
        /// Gets a MultiPoint geometry that contains the StartPoint and Endpoint
        /// </summary>
        public override IGeometry Boundary
        {
            get
            {
                if (IsEmpty)
                    return Factory.CreateGeometryCollection(null);
                if (IsClosed)
                    return MultiPoint.Empty;
                return Factory.CreateMultiPoint(new[] { StartPoint.Coordinate, EndPoint.Coordinate });
            }
        }

        /// <summary>
        /// Gets False if the LineString is closed, or Point (0) otherwise, representing the endpoints
        /// </summary>
        public override DimensionType BoundaryDimension
        {
            get
            {
                if (IsClosed)
                {
                    return DimensionType.False;
                }
                return DimensionType.Point;
            }
        }

        /// <summary>
        /// Gets the 0th coordinate
        /// </summary>
        public override Coordinate Coordinate
        {
            get
            {
                if (IsEmpty) return null;
                return _points.First();
            }
        }

        /// <summary>
        /// Gets a System.Array of the coordinates
        /// </summary>
        public override IList<Coordinate> Coordinates
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
            }
        }

        /// <summary>
        /// Gets the dimensionality of a Curve(1)
        /// </summary>
        public override DimensionType Dimension
        {
            get
            {
                return DimensionType.Curve;
            }
        }

        /// <summary>
        /// Gets the point corresponding to NumPoints-1 and returns it as an IPoint interface
        /// </summary>
        public virtual IPoint EndPoint
        {
            get
            {
                if (IsEmpty)
                {
                    return null;
                }
                return GetPointN(NumPoints - 1);
            }
        }

        /// <summary>
        /// This will always contain Line, even if it is technically empty
        /// </summary>
        public override FeatureType FeatureType
        {
            get
            {
                return FeatureType.Line;
            }
        }

        /// <summary>
        /// Gets a string that says "LineString"
        /// </summary>
        public override string GeometryType
        {
            get
            {
                return "LineString";
            }
        }

        /// <summary>
        /// Gets a boolean that is true if the EndPoint is geometrically equal to the StartPoint in 2 Dimensions
        /// </summary>
        public virtual bool IsClosed
        {
            get
            {
                if (IsEmpty)
                {
                    return false;
                }
                return new Coordinate(GetCoordinateN(0)).Equals2D(GetCoordinateN(NumPoints - 1));
            }
        }

        /// <summary>
        /// Gets a boolean value that returns true if the count of points in this LineString is 0.
        /// </summary>
        public override bool IsEmpty
        {
            get
            {
                return _points.Count == 0;
            }
        }

        /// <summary>
        /// Gets a boolean that is true if this LineString is both closed (has the same start and end point)
        /// and simple (does not self-intersect)
        /// </summary>
        public virtual bool IsRing
        {
            get
            {
                return IsClosed && IsSimple;
            }
        }

        /// <summary>
        /// Gets a boolean that is true if any part of this LineString intersects with itself
        /// </summary>
        public override bool IsSimple
        {
            get
            {
                return (new IsSimpleOp()).IsSimple(this);
            }
        }

        /// <summary>
        /// Returns the length of this <c>LineString</c>
        /// </summary>
        /// <returns>The length of the polygon.</returns>
        public override double Length
        {
            get
            {
                return CgAlgorithms.Length(_points);
            }
        }

        /// <summary>
        /// Gets an integer count of the points.
        /// </summary>
        public override int NumPoints
        {
            get
            {
                return _points.Count;
            }
        }

        /// <summary>
        /// Gets the 0 index point as a valid implementation of IPoint interface
        /// </summary>
        public virtual IPoint StartPoint
        {
            get
            {
                if (IsEmpty)
                {
                    return null;
                }
                return GetPointN(0);
            }
        }

        #endregion
    }
}