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
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Models an OGC-style <code>LineString</code>
    /// </summary>
    /// <remarks>
    /// A LineString consists of a sequence of two or more vertices,
    /// along with all points along the linearly-interpolated curves
    /// (line segments) between each
    /// pair of consecutive vertices.
    /// Consecutive vertices may be equal.
    /// The line segments in the line may intersect each other (in other words,
    /// the linestring may "curl back" in itself and self-intersect.
    /// Linestrings with exactly two identical points are invalid.
    /// <para>A linestring must have either 0 or 2 or more points.
    /// If these conditions are not met, the constructors throw an <see cref="ArgumentException"/>.
    /// </para>
    /// </remarks>
    [Serializable]
    public class LineString : Geometry, ILineString
    {
        #region Fields

        /// <summary>
        /// Represents an empty <c>LineString</c>.
        /// </summary>
        public static readonly ILineString Empty = new GeometryFactory().CreateLineString(new Coordinate[] { });

        /// <summary>
        /// The points of this <c>LineString</c>.
        /// </summary>
        private ICoordinateSequence _points;

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
                points = new List<Coordinate>();
            if (points.Count == 1)
                throw new ArgumentException("point array must contain 0 or > 1 elements");
            _points = DefaultFactory.CoordinateSequenceFactory.Create(points);
        }

        /// <summary>
        /// Creates a new LineString using the default factory
        /// </summary>
        /// <param name="points">
        /// The points of the linestring, or <c>null</c>
        /// to create the empty point. Consecutive points may not be equal.
        /// </param>
        public LineString(IList<Coordinate> points)
            : this(points, DefaultFactory) { }

        /// <summary>
        /// Creates a new topologically complete LineString from a LineStringBase
        /// </summary>
        /// <param name="lineStringBase"></param>
        public LineString(IBasicLineString lineStringBase)
            : this(lineStringBase.Coordinates, DefaultFactory) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lineString">
        /// The points of the linestring, or <c>null</c>
        /// to create the empty point. Consecutive points may not be equal.
        /// </param>
        /// <param name="factory"></param>
        public LineString(IBasicLineString lineString, IGeometryFactory factory)
            : this(lineString.Coordinates, factory) { }


        /// <summary>
        /// Creates an empty linestring using the specified factory.
        /// </summary>
        /// <param name="factory">An IGeometryFactory to use when specifying this linestring.</param>
        public LineString(IGeometryFactory factory)
            : base(factory)
        {
            _points = factory.CoordinateSequenceFactory.Create();
        }


        /// <summary>
        /// Rather than using the factory trends, this will create a coordinate sequence by simply using the specified list of coordinates.
        /// </summary>
        /// <param name="coordinates">The list of coordinates to use as a new ICoordinateSequence</param>
        public LineString(IEnumerable<Coordinate> coordinates)
            : base(DefaultFactory)
        {
            if (coordinates == null)
            {
                _points = DefaultFactory.CoordinateSequenceFactory.Create();
                return;
            }

            if (!coordinates.GetType().IsArray) //fixed arrays cause errors in LinearRing.ValidateConstruction
            {
                _points = DefaultFactory.CoordinateSequenceFactory.Create(coordinates as IList<Coordinate>);
                if (_points != null) return;
            }
            _points = DefaultFactory.CoordinateSequenceFactory.Create(coordinates.ToList());
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
                _points = DefaultFactory.CoordinateSequenceFactory.Create();
                return;
            }
            IList<Coordinate> coords = coordinates.Select(c => new Coordinate(c)).ToList();
            _points = DefaultFactory.CoordinateSequenceFactory.Create(coords);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineString"/> class.
        /// </summary>
        /// <param name="points">
        /// The points of the linestring, or <c>null</c>
        /// to create the empty point. Consecutive points may not be equal.
        /// </param>
        /// <param name="factory"></param>
        /// <exception cref="ArgumentException">If too few points are provided</exception>
        public LineString(ICoordinateSequence points, IGeometryFactory factory)
            : base(factory)
        {
            if (points == null)
                points = factory.CoordinateSequenceFactory.Create(new Coordinate[] { });
            if (points.Count == 1)
                throw new ArgumentException("Invalid number of points in LineString (found "
                      + points.Count + " - must be 0 or >= 2)");
            _points = points;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns the value of the angle between the <see cref="StartPoint" />
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
                return (new BoundaryOp(this)).GetBoundary();
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
                return _points.GetCoordinate(0);
            }
        }

        /// <summary>
        /// Gets a System.Array of the coordinates
        /// </summary>
        public override IList<Coordinate> Coordinates
        {
            get
            {
                return _points.ToList();
            }
            set
            {
                _points = DefaultFactory.CoordinateSequenceFactory.Create(value);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public ICoordinateSequence CoordinateSequence
        {
            get
            {
                return _points;
            }
        }

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
                    return null;
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
        /// Returns the name of this object's interface.
        /// </summary>
        /// <returns>"LineString"</returns>
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
                    return false;
                return GetCoordinateN(0).Equals2D(GetCoordinateN(NumPoints - 1));
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

        public override OgcGeometryType OgcGeometryType
        {
            get { return OgcGeometryType.LineString; }
        }

        /// <summary>
        /// Gets the 0 index point as a valid implementation of IPoint interface
        /// </summary>
        public virtual IPoint StartPoint
        {
            get
            {
                if (IsEmpty)
                    return null;
                return GetPointN(0);
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets the ICoordinate that exists at the Nth index
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public virtual Coordinate this[int n]
        {
            get
            {
                return _points.GetCoordinate(n);
            }
            set
            {
                _points[n] = value;
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
            for (int i = 0; i < _points.Count; i++)
                filter.Filter(_points.GetCoordinate(i));
        }

        public override void Apply(ICoordinateSequenceFilter filter)
        {
            if (_points.Count == 0)
                return;
            for (int i = 0; i < _points.Count; i++)
            {
                filter.Filter(_points, i);
                if (filter.Done)
                    break;
            }
            if (filter.GeometryChanged)
                GeometryChanged();
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
        /// Performs a CompareTo opperation assuming that the specified object is a LineString
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override int CompareToSameClass(object o)
        {
            ILineString line = o as LineString;
            // MD - optimized implementation
            int i = 0;
            int j = 0;
            if (line != null)
            {
                while (i < _points.Count && j < line.CoordinateSequence.Count)
                {
                    int comparison = _points.GetCoordinate(i).CompareTo(line.CoordinateSequence.GetCoordinate(j));
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
                if (j < line.CoordinateSequence.Count)
                    return -1;
            }
            return 0;
        }

        protected internal override int CompareToSameClass(Object o, IComparer<ICoordinateSequence> comp)
        {
            Assert.IsTrue(o is ILineString);
            ILineString line = (LineString)o;
            return comp.Compare(_points, line.CoordinateSequence);
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
        /// Tests the coordinates of this LineString against another geometry and returns true if they are identical.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public override bool EqualsExact(IGeometry other, double tolerance)
        {
            if (!IsEquivalentClass(other))
                return false;

            ILineString otherLineString = (ILineString)other;
            if (_points.Count != otherLineString.NumPoints)
                return false;

            for (int i = 0; i < _points.Count; i++)
                if (!Equal(_points.GetCoordinate(i), otherLineString.GetCoordinateN(i), tolerance))
                    return false;
            return true;
        }

        /// <summary>
        /// Returns the Nth coordinate of the coordinate sequence making up this LineString
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public virtual Coordinate GetCoordinateN(int n)
        {
            return _points.GetCoordinate(n);
        }

        public override double[] GetOrdinates(Ordinate ordinate)
        {
            if (IsEmpty)
                return new double[0];

            var ordinateFlag = OrdinatesUtility.ToOrdinatesFlag(ordinate);
            if ((_points.Ordinates & ordinateFlag) != ordinateFlag)
                return CreateArray(_points.Count, Coordinate.NullOrdinate);

            return CreateArray(_points, ordinate);
        }

        /// <summary>
        /// Returns the N'th point as an Implementation of IPoint.  The specific
        /// implementation is just the DotSpatial.Geometries.Point
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public virtual IPoint GetPointN(int n)
        {
            return Factory.CreatePoint(_points.GetCoordinate(n));
        }

        /// <summary>
        /// Returns true if the given point is a vertex of this <c>LineString</c>.
        /// </summary>
        /// <param name="pt">The <c>Coordinate</c> to check.</param>
        /// <returns><c>true</c> if <c>pt</c> is one of this <c>LineString</c>'s vertices.</returns>
        public virtual bool IsCoordinate(Coordinate pt)
        {
            for (int i = 0; i < _points.Count; i++)
                if (_points.GetCoordinate(i).Equals(pt))
                    return true;
            return false;
        }

        /// <summary>
        /// Tests other and returns true if other is a LineString
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected override bool IsEquivalentClass(IGeometry other)
        {
            return other is ILineString;
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
                if (!_points.GetCoordinate(i).Equals(_points.GetCoordinate(j)))
                {
                    if (_points.GetCoordinate(i).CompareTo(_points.GetCoordinate(j)) > 0)
                        Coordinates = Coordinates.Reverse().ToList();
                    return;
                }
            }
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

        /// <summary>
        /// Returns a copy of this ILineString
        /// </summary>
        protected override void OnCopy(Geometry copy)
        { //TODO does this do anything ?
            base.OnCopy(copy);
            LineString ls = copy as LineString;
            if (ls == null) return;
            ls._points = (ICoordinateSequence)_points.Clone();
        }

        /// <summary>
        /// Creates a <see cref="LineString" /> whose coordinates are in the reverse order of this objects.
        /// </summary>
        /// <returns>A <see cref="LineString" /> with coordinates in the reverse order.</returns>
        public override IGeometry Reverse()
        {
            var seq = _points.Reversed();
            return Factory.CreateLineString(seq);
        }

        /// <summary>
        /// Rotates the LineString by the given radian angle around the Origin.
        /// </summary>
        /// <param name="origin">Coordinate the LineString gets rotated around.</param>
        /// <param name="radAngle">Rotation angle in radian.</param>
        public override void Rotate(Coordinate origin, double radAngle)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                base.RotateCoordinateRad(origin, ref _points[i].X, ref _points[i].Y, radAngle);
            }
        }

        #endregion
    }
}