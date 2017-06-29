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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Operation.Buffer
{
    /// <summary>
    /// Computes the raw offset curve for a
    /// single <c>Geometry</c> component (ring, line or point).
    /// A raw offset curve line is not noded -
    /// it may contain self-intersections (and usually will).
    /// The final buffer polygon is computed by forming a topological graph
    /// of all the noded raw curves and tracing outside contours.
    /// The points in the raw curve are rounded to the required precision model.
    /// </summary>
    public class OffsetCurveBuilder
    {
        /// <summary>
        /// The default number of facets into which to divide a fillet of 90 degrees.
        /// A value of 8 gives less than 2% max error in the buffer distance.
        /// For a max error smaller of 1%, use QS = 12
        /// </summary>
        public const int DEFAULT_QUADRANT_SEGMENTS = 8;

        /*
        * The angle quantum with which to approximate a fillet curve
        * (based on the input # of quadrant segments)
        */
        private readonly double _filletAngleQuantum;
        private readonly LineIntersector _li;
        private readonly LineSegment _offset0 = new LineSegment();
        private readonly LineSegment _offset1 = new LineSegment();
        private readonly PrecisionModel _precisionModel;
        private readonly LineSegment _seg0 = new LineSegment();
        private readonly LineSegment _seg1 = new LineSegment();
        private double _distance;
        private BufferStyle _endCapStyle = BufferStyle.CapRound;
        private IList<Coordinate> _ptList;
        private Coordinate _s0, _s1, _s2;
        private PositionType _side = 0;

        /// <summary>
        ///
        /// </summary>
        /// <param name="precisionModel"></param>
        public OffsetCurveBuilder(PrecisionModel precisionModel) : this(precisionModel, DEFAULT_QUADRANT_SEGMENTS) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="precisionModel"></param>
        /// <param name="quadrantSegments"></param>
        public OffsetCurveBuilder(PrecisionModel precisionModel, int quadrantSegments)
        {
            _precisionModel = precisionModel;
            // compute intersections in full precision, to provide accuracy
            // the points are rounded as they are inserted into the curve line
            _li = new RobustLineIntersector();
            int limitedQuadSegs = quadrantSegments < 1 ? 1 : quadrantSegments;
            _filletAngleQuantum = Math.PI / 2.0 / limitedQuadSegs;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual BufferStyle EndCapStyle
        {
            get
            {
                return _endCapStyle;
            }
            set
            {
                _endCapStyle = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private IList<Coordinate> Coordinates
        {
            get
            {
                // check that points are a ring - add the startpoint again if they are not
                if (_ptList.Count > 1)
                {
                    Coordinate start = _ptList.First();
                    Coordinate end = _ptList.Last();
                    if (!start.Equals(end))
                        AddPt(start);
                }
                return _ptList;
            }
        }

        /// <summary>
        /// This method handles single points as well as lines.
        /// Lines are assumed to not be closed (the function will not
        /// fail for closed lines, but will generate superfluous line caps).
        /// </summary>
        /// <param name="inputPts"></param>
        /// <param name="distance"></param>
        /// <returns> A List of Coordinate[].</returns>
        public virtual IList GetLineCurve(IList<Coordinate> inputPts, double distance)
        {
            IList lineList = new ArrayList();
            // a zero or negative width buffer of a line/point is empty
            if (distance <= 0.0)
                return lineList;
            Init(distance);
            if (inputPts.Count <= 1)
            {
                switch (_endCapStyle)
                {
                    case BufferStyle.CapRound:
                        AddCircle(inputPts[0], distance);
                        break;
                    case BufferStyle.CapSquare:
                        AddSquare(inputPts[0], distance);
                        break;
                    default:
                        // default is for buffer to be empty (e.g. for a butt line cap);
                        break;
                }
            }
            else ComputeLineBufferCurve(inputPts);
            IList<Coordinate> lineCoord = Coordinates;
            lineList.Add(lineCoord);
            return lineList;
        }

        /// <summary>
        /// This method handles the degenerate cases of single points and lines,
        /// as well as rings.
        /// </summary>
        /// <returns>A List of Coordinate[].</returns>
        public virtual IList GetRingCurve(IList<Coordinate> inputPts, PositionType side, double distance)
        {
            IList lineList = new ArrayList();
            Init(distance);
            if (inputPts.Count <= 2)
                return GetLineCurve(inputPts, distance);
            // optimize creating ring for for zero distance
            if (distance == 0.0)
            {
                lineList.Add(CopyCoordinates(inputPts));
                return lineList;
            }
            ComputeRingBufferCurve(inputPts, side);
            lineList.Add(Coordinates);
            return lineList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        private static IList<Coordinate> CopyCoordinates(IEnumerable<Coordinate> pts)
        {
            return pts.CloneList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="distance"></param>
        private void Init(double distance)
        {
            _distance = distance;
            //maxCurveSegmentError = distance * (1 - Math.Cos(_filletAngleQuantum / 2.0));
            _ptList = new List<Coordinate>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputPts"></param>
        private void ComputeLineBufferCurve(IList<Coordinate> inputPts)
        {
            int n = inputPts.Count - 1;

            // compute points for left side of line
            InitSideSegments(inputPts[0], inputPts[1], PositionType.Left);
            for (int i = 2; i <= n; i++)
                AddNextSegment(inputPts[i], true);
            AddLastSegment();
            // add line cap for end of line
            AddLineEndCap(inputPts[n - 1], inputPts[n]);

            // compute points for right side of line
            InitSideSegments(inputPts[n], inputPts[n - 1], PositionType.Left);
            for (int i = n - 2; i >= 0; i--)
                AddNextSegment(inputPts[i], true);
            AddLastSegment();

            // add line cap for start of line
            AddLineEndCap(inputPts[1], inputPts[0]);
            ClosePts();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputPts"></param>
        /// <param name="side"></param>
        private void ComputeRingBufferCurve(IList<Coordinate> inputPts, PositionType side)
        {
            int n = inputPts.Count - 1;
            InitSideSegments(inputPts[n - 1], inputPts[0], side);
            for (int i = 1; i <= n; i++)
            {
                bool addStartPoint = i != 1;
                AddNextSegment(inputPts[i], addStartPoint);
            }
            ClosePts();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pt"></param>
        private void AddPt(Coordinate pt)
        {
            Coordinate bufPt = new Coordinate(pt);
            _precisionModel.MakePrecise(bufPt);
            // don't add duplicate points
            Coordinate lastPt = null;
            if (_ptList.Count >= 1)
                lastPt = _ptList.Last();
            if (lastPt != null && bufPt.Equals(lastPt)) return;
            _ptList.Add(bufPt);
        }

        /// <summary>
        ///
        /// </summary>
        private void ClosePts()
        {
            if (_ptList.Count < 1) return;
            Coordinate startPt = new Coordinate(_ptList[0]);
            Coordinate lastPt = _ptList[_ptList.Count - 1];
            if (startPt.Equals(lastPt)) return;
            _ptList.Add(startPt);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="side"></param>
        private void InitSideSegments(Coordinate s1, Coordinate s2, PositionType side)
        {
            _s1 = s1;
            _s2 = s2;
            _side = side;
            _seg1.SetCoordinates(s1, s2);
            ComputeOffsetSegment(_seg1, side, _distance, _offset1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="addStartPoint"></param>
        private void AddNextSegment(Coordinate p, bool addStartPoint)
        {
            // s0-s1-s2 are the coordinates of the previous segment and the current one
            _s0 = _s1;
            _s1 = _s2;
            _s2 = p;
            _seg0.SetCoordinates(_s0, _s1);
            ComputeOffsetSegment(_seg0, _side, _distance, _offset0);
            _seg1.SetCoordinates(_s1, _s2);
            ComputeOffsetSegment(_seg1, _side, _distance, _offset1);

            // do nothing if points are equal
            if (_s1.Equals(_s2)) return;

            int orientation = CgAlgorithms.ComputeOrientation(_s0, _s1, _s2);
            bool outsideTurn =
                (orientation == CgAlgorithms.CLOCKWISE && _side == PositionType.Left)
            || (orientation == CgAlgorithms.COUNTER_CLOCKWISE && _side == PositionType.Right);

            if (orientation == 0)
            {
                // lines are collinear
                _li.ComputeIntersection(_s0, _s1, _s1, _s2);
                int numInt = _li.IntersectionNum;
                /*
                * if numInt is < 2, the lines are parallel and in the same direction.
                * In this case the point can be ignored, since the offset lines will also be
                * parallel.
                */
                if (numInt >= 2)
                    /*
                    * segments are collinear but reversing.  Have to add an "end-cap" fillet
                    * all the way around to other direction
                    * This case should ONLY happen for LineStrings, so the orientation is always CW.
                    * (Polygons can never have two consecutive segments which are parallel but reversed,
                    * because that would be a self intersection.
                    */
                    AddFillet(_s1, _offset0.P1, _offset1.P0, CgAlgorithms.CLOCKWISE, _distance);
            }
            else if (outsideTurn)
            {
                // add a fillet to connect the endpoints of the offset segments
                if (addStartPoint)
                    AddPt(_offset0.P1);
                AddFillet(_s1, _offset0.P1, _offset1.P0, orientation, _distance);
                AddPt(_offset1.P0);
            }
            else
            {
                // inside turn
                /*
                 * add intersection point of offset segments (if any)
                 */
                _li.ComputeIntersection(_offset0.P0, _offset0.P1, _offset1.P0, _offset1.P1);
                if (_li.HasIntersection)
                    AddPt(_li.GetIntersection(0));
                else
                {
                    /*
                    * If no intersection, it means the angle is so small and/or the offset so large
                    * that the offsets segments don't intersect.
                    * In this case we must add a offset joining curve to make sure the buffer line
                    * is continuous and tracks the buffer correctly around the corner.
                    * Notice that the joining curve won't appear in the final buffer.
                    *
                    * The intersection test above is vulnerable to robustness errors;
                    * i.e. it may be that the offsets should intersect very close to their
                    * endpoints, but don't due to rounding.  To handle this situation
                    * appropriately, we use the following test:
                    * If the offset points are very close, don't add a joining curve
                    * but simply use one of the offset points
                    */
                    if (new Coordinate(_offset0.P1).Distance(_offset1.P0) < _distance / 1000.0)
                        AddPt(_offset0.P1);
                    else
                    {
                        // add endpoint of this segment offset
                        AddPt(_offset0.P1);
                        // <FIX> MD - add in centre point of corner, to make sure offset closer lines have correct topology
                        AddPt(_s1);
                        AddPt(_offset1.P0);
                    }
                }
            }
        }

        /// <summary>
        /// Add last offset point.
        /// </summary>
        private void AddLastSegment()
        {
            AddPt(_offset1.P1);
        }

        /// <summary>
        /// Compute an offset segment for an input segment on a given side and at a given distance.
        /// The offset points are computed in full double precision, for accuracy.
        /// </summary>
        /// <param name="seg">The segment to offset.</param>
        /// <param name="side">The side of the segment the offset lies on.</param>
        /// <param name="distance">The offset distance.</param>
        /// <param name="offset">The points computed for the offset segment.</param>
        private static void ComputeOffsetSegment(ILineSegmentBase seg, PositionType side, double distance, ILineSegmentBase offset)
        {
            int sideSign = side == PositionType.Left ? 1 : -1;
            double dx = seg.P1.X - seg.P0.X;
            double dy = seg.P1.Y - seg.P0.Y;
            double len = Math.Sqrt(dx * dx + dy * dy);
            // u is the vector that is the length of the offset, in the direction of the segment
            double ux = sideSign * distance * dx / len;
            double uy = sideSign * distance * dy / len;
            offset.P0.X = seg.P0.X - uy;
            offset.P0.Y = seg.P0.Y + ux;
            offset.P1.X = seg.P1.X - uy;
            offset.P1.Y = seg.P1.Y + ux;
        }

        /// <summary>
        /// Add an end cap around point p1, terminating a line segment coming from p0.
        /// </summary>
        private void AddLineEndCap(Coordinate p0, Coordinate p1)
        {
            LineSegment seg = new LineSegment(p0, p1);

            LineSegment offsetL = new LineSegment();
            ComputeOffsetSegment(seg, PositionType.Left, _distance, offsetL);
            LineSegment offsetR = new LineSegment();
            ComputeOffsetSegment(seg, PositionType.Right, _distance, offsetR);

            double dx = p1.X - p0.X;
            double dy = p1.Y - p0.Y;
            double angle = Math.Atan2(dy, dx);

            switch (_endCapStyle)
            {
                case BufferStyle.CapRound:
                    // add offset seg points with a fillet between them
                    AddPt(offsetL.P1);
                    AddFillet(p1, angle + Math.PI / 2, angle - Math.PI / 2, CgAlgorithms.CLOCKWISE, _distance);
                    AddPt(offsetR.P1);
                    break;
                case BufferStyle.CapButt:
                    // only offset segment points are added
                    AddPt(offsetL.P1);
                    AddPt(offsetR.P1);
                    break;
                case BufferStyle.CapSquare:
                    // add a square defined by extensions of the offset segment endpoints
                    Coordinate squareCapSideOffset = new Coordinate();
                    squareCapSideOffset.X = Math.Abs(_distance) * Math.Cos(angle);
                    squareCapSideOffset.Y = Math.Abs(_distance) * Math.Sin(angle);

                    Coordinate squareCapLOffset = new Coordinate(
                        offsetL.P1.X + squareCapSideOffset.X,
                        offsetL.P1.Y + squareCapSideOffset.Y);
                    Coordinate squareCapROffset = new Coordinate(
                        offsetR.P1.X + squareCapSideOffset.X,
                        offsetR.P1.Y + squareCapSideOffset.Y);
                    AddPt(squareCapLOffset);
                    AddPt(squareCapROffset);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p">Base point of curve.</param>
        /// <param name="p0">Start point of fillet curve.</param>
        /// <param name="p1">Endpoint of fillet curve.</param>
        /// <param name="direction"></param>
        /// <param name="distance"></param>
        private void AddFillet(Coordinate p, Coordinate p0, Coordinate p1, int direction, double distance)
        {
            double dx0 = p0.X - p.X;
            double dy0 = p0.Y - p.Y;
            double startAngle = Math.Atan2(dy0, dx0);
            double dx1 = p1.X - p.X;
            double dy1 = p1.Y - p.Y;
            double endAngle = Math.Atan2(dy1, dx1);

            if (direction == CgAlgorithms.CLOCKWISE)
            {
                if (startAngle <= endAngle)
                    startAngle += 2.0 * Math.PI;
            }
            else
            {
                // direction == CounterClockwise
                if (startAngle >= endAngle)
                    startAngle -= 2.0 * Math.PI;
            }

            AddPt(p0);
            AddFillet(p, startAngle, endAngle, direction, distance);
            AddPt(p1);
        }

        /// <summary>
        /// Adds points for a fillet.  The start and end point for the fillet are not added -
        /// the caller must add them if required.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        /// <param name="direction">Is -1 for a CW angle, 1 for a CCW angle.</param>
        /// <param name="distance"></param>
        private void AddFillet(Coordinate p, double startAngle, double endAngle, int direction, double distance)
        {
            int directionFactor = direction == CgAlgorithms.CLOCKWISE ? -1 : 1;

            double totalAngle = Math.Abs(startAngle - endAngle);
            int nSegs = (int)(totalAngle / _filletAngleQuantum + 0.5);

            if (nSegs < 1) return;    // no segments because angle is less than increment - nothing to do!

            // choose angle increment so that each segment has equal length
            const double initAngle = 0.0;
            double currAngleInc = totalAngle / nSegs;

            double currAngle = initAngle;
            Coordinate pt = new Coordinate();
            while (currAngle < totalAngle)
            {
                double angle = startAngle + directionFactor * currAngle;
                pt.X = p.X + distance * Math.Cos(angle);
                pt.Y = p.Y + distance * Math.Sin(angle);
                AddPt(pt);
                currAngle += currAngleInc;
            }
        }

        /// <summary>
        /// Adds a CW circle around a point.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="distance"></param>
        private void AddCircle(Coordinate p, double distance)
        {
            // add start point
            Coordinate pt = new Coordinate(p.X + distance, p.Y);
            AddPt(pt);
            AddFillet(p, 0.0, 2.0 * Math.PI, -1, distance);
        }

        /// <summary>
        /// Adds a CW square around a point
        /// </summary>
        /// <param name="p"></param>
        /// <param name="distance"></param>
        private void AddSquare(Coordinate p, double distance)
        {
            // add start point
            AddPt(new Coordinate(p.X + distance, p.Y + distance));
            AddPt(new Coordinate(p.X + distance, p.Y - distance));
            AddPt(new Coordinate(p.X - distance, p.Y - distance));
            AddPt(new Coordinate(p.X - distance, p.Y + distance));
            AddPt(new Coordinate(p.X + distance, p.Y + distance));
        }
    }
}