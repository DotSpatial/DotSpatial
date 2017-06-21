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
using System.Text;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// A LineIntersector is an algorithm that can both test whether
    /// two line segments intersect and compute the intersection point
    /// if they do.
    /// The intersection point may be computed in a precise or non-precise manner.
    /// Computing it precisely involves rounding it to an integer.  (This assumes
    /// that the input coordinates have been made precise by scaling them to
    /// an integer grid.)
    /// </summary>
    public abstract class LineIntersector
    {
        private Coordinate[,] _inputLines = new Coordinate[2, 2];

        /// <summary>
        /// The indexes of the endpoints of the intersection lines, in order along
        /// the corresponding line
        /// </summary>
        private int[,] _intLineIndex;

        private Coordinate[] _intPt = new Coordinate[2];
        private bool _isProper;

        /// <summary>
        /// If MakePrecise is true, computed intersection coordinates will be made precise
        /// using <c>Coordinate.MakePrecise</c>.
        /// </summary>
        private PrecisionModel _precisionModel;

        private IntersectionType _result;

        /// <summary>
        ///
        /// </summary>
        protected LineIntersector()
        {
            _intPt[0] = new Coordinate();
            _intPt[1] = new Coordinate();
            _result = 0;
        }

        /// <summary>
        /// Gets or sets the first intersection coordinate, if any.
        /// </summary>
        protected Coordinate PointA
        {
            get { return _intPt[0]; }
            set { _intPt[0] = value; }
        }

        /// <summary>
        /// Gets or sets the second intersection coordiante, if any.
        /// </summary>
        protected Coordinate PointB
        {
            get { return _intPt[0]; }
            set { _intPt[0] = value; }
        }

        /// <summary>
        /// Force computed intersection to be rounded to a given precision model
        /// </summary>
        [Obsolete("Use PrecisionModel instead")]
        public virtual PrecisionModel MakePrecise
        {
            set
            {
                _precisionModel = value;
            }
        }

        /// <summary>
        /// Force computed intersection to be rounded to a given precision model.
        /// No getter is provided, because the precision model is not required to be specified.
        /// </summary>
        public virtual PrecisionModel PrecisionModel
        {
            get
            {
                return _precisionModel;
            }
            set
            {
                _precisionModel = value;
            }
        }

        /// <summary>
        /// Computes the "edge distance" of an intersection point p along a segment.
        /// The edge distance is a metric of the point along the edge.
        /// The metric used is a robust and easy to compute metric function.
        /// It is not equivalent to the usual Euclidean metric.
        /// It relies on the fact that either the x or the y ordinates of the
        /// points in the edge are unique, depending on whether the edge is longer in
        /// the horizontal or vertical direction.
        /// Notice: This function may produce incorrect distances
        /// for inputs where p is not precisely on p1-p2
        /// (E.g. p = (139, 9) p1 = (139, 10), p2 = (280, 1) produces distanct 0.0, which is incorrect.
        /// My hypothesis is that the function is safe to use for points which are the
        /// result of rounding points which lie on the line, but not safe to use for truncated points.
        /// </summary>
        public static double ComputeEdgeDistance(Coordinate p, Coordinate p0, Coordinate p1)
        {
            double dx = Math.Abs(p1.X - p0.X);
            double dy = Math.Abs(p1.Y - p0.Y);

            double dist;   // sentinel value
            if (p.Equals(p0))
                dist = 0.0;
            else if (p.Equals(p1))
            {
                dist = dx > dy ? dx : dy;
            }
            else
            {
                double pdx = Math.Abs(p.X - p0.X);
                double pdy = Math.Abs(p.Y - p0.Y);
                if (dx > dy)
                    dist = pdx;
                else dist = pdy;

                // <FIX>: hack to ensure that non-endpoints always have a non-zero distance
                if (dist == 0.0 && !p.Equals(p0))
                    dist = Math.Max(pdx, pdy);
            }
            Assert.IsTrue(!(dist == 0.0 && !p.Equals(p0)), "Bad distance calculation");
            return dist;
        }

        /// <summary>
        /// This function is non-robust, since it may compute the square of large numbers.
        /// Currently not sure how to improve this.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double NonRobustComputeEdgeDistance(Coordinate p, Coordinate p1, Coordinate p2)
        {
            double dx = p.X - p1.X;
            double dy = p.Y - p1.Y;
            double dist = Math.Sqrt(dx * dx + dy * dy);   // dummy value
            Assert.IsTrue(!(dist == 0.0 && !p.Equals(p1)), "Invalid distance calculation");
            return dist;
        }

        /// <summary>
        /// Compute the intersection of a point p and the line p1-p2.
        /// This function computes the bool value of the hasIntersection test.
        /// The actual value of the intersection (if there is one)
        /// is equal to the value of <c>p</c>.
        /// </summary>
        public abstract void ComputeIntersection(Coordinate p, Coordinate p1, Coordinate p2);

        /// <summary>
        /// Computes the intersection of the lines p1-p2 and p3-p4.
        /// This function computes both the bool value of the hasIntersection test
        /// and the (approximate) value of the intersection point itself (if there is one).
        /// </summary>
        public virtual void ComputeIntersection(Coordinate p1, Coordinate p2, Coordinate p3, Coordinate p4)
        {
            _inputLines[0, 0] = new Coordinate(p1);
            _inputLines[0, 1] = new Coordinate(p2);
            _inputLines[1, 0] = new Coordinate(p3);
            _inputLines[1, 1] = new Coordinate(p4);
            _result = ComputeIntersect(p1, p2, p3, p4);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        public abstract IntersectionType ComputeIntersect(Coordinate p1, Coordinate p2, Coordinate q1, Coordinate q2);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_inputLines[0, 0]).Append("-");
            sb.Append(_inputLines[0, 1]).Append(" ");
            sb.Append(_inputLines[1, 0]).Append("-");
            sb.Append(_inputLines[1, 1]).Append(" : ");

            if (IsEndPoint) sb.Append(" endpoint");
            if (_isProper) sb.Append(" proper");
            if (IsCollinear) sb.Append(" collinear");

            return sb.ToString();
        }

        /// <summary>
        /// Returns the intIndex'th intersection point.
        /// </summary>
        /// <param name="intIndex">is 0 or 1.</param>
        /// <returns>The intIndex'th intersection point.</returns>
        public virtual Coordinate GetIntersection(int intIndex)
        {
            return new Coordinate(_intPt[intIndex]);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ComputeIntLineIndex()
        {
            if (_intLineIndex == null)
            {
                _intLineIndex = new int[2, 2];
                ComputeIntLineIndex(0);
                ComputeIntLineIndex(1);
            }
        }

        /// <summary>
        /// Test whether a point is a intersection point of two line segments.
        /// Notice that if the intersection is a line segment, this method only tests for
        /// equality with the endpoints of the intersection segment.
        /// It does not return true if the input point is internal to the intersection segment.
        /// </summary>
        /// <returns><c>true</c> if the input point is one of the intersection points.</returns>
        public virtual bool IsIntersection(Coordinate pt)
        {
            for (int i = 0; i < (int)_result; i++)
                if (new Coordinate(_intPt[i]).Equals2D(pt))
                    return true;
            return false;
        }

        /// <summary>
        /// Tests whether either intersection point is an interior point of one of the input segments.
        /// </summary>
        /// <returns>
        /// <c>true</c> if either intersection point is in the interior of one of the input segment.
        /// </returns>
        public virtual bool IsInteriorIntersection()
        {
            if (IsInteriorIntersection(0))
                return true;
            if (IsInteriorIntersection(1))
                return true;
            return false;
        }

        /// <summary>
        /// Tests whether either intersection point is an interior point of the specified input segment.
        /// </summary>
        /// <returns>
        /// <c>true</c> if either intersection point is in the interior of the input segment.
        /// </returns>
        public virtual bool IsInteriorIntersection(int inputLineIndex)
        {
            Coordinate ptI;
            for (int i = 0; i < (int)_result; i++)
            {
                ptI = new Coordinate(_intPt[i]);
                if (!(ptI.Equals2D(_inputLines[inputLineIndex, 0]) || ptI.Equals2D(_inputLines[inputLineIndex, 1])))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Computes the coordinate of the intIndex'th intersection point in the direction of
        /// a specified input line segment.
        /// </summary>
        /// <param name="segmentIndex">The segment index from 0 to 1.</param>
        /// <param name="intIndex">The integer intersection index from 0 to 1.</param>
        /// <returns>
        /// The coordinate of the intIndex'th intersection point in the direction of the specified input line segment.
        /// </returns>
        public virtual Coordinate GetIntersectionAlongSegment(int segmentIndex, int intIndex)
        {
            // lazily compute int line array
            ComputeIntLineIndex();
            return new Coordinate(_intPt[_intLineIndex[segmentIndex, intIndex]]);
        }

        /// <summary>
        /// Computes the index of the intIndex'th intersection point in the direction of
        /// a specified input line segment, and returns the integer index.
        /// </summary>
        /// <param name="segmentIndex">The integer segment index from 0 to 1.</param>
        /// <param name="intIndex">The integer intersection index from 0 to 1.</param>
        /// <returns>
        /// The integer index of the intersection point along the segment (0 or 1).
        /// </returns>
        public virtual int GetIndexAlongSegment(int segmentIndex, int intIndex)
        {
            ComputeIntLineIndex();
            return _intLineIndex[segmentIndex, intIndex];
        }

        /// <summary>
        /// Computes the integer line index of the specified integer segment index.
        /// </summary>
        /// <param name="segmentIndex">The integer index of the segment.</param>
        protected virtual void ComputeIntLineIndex(int segmentIndex)
        {
            double dist0 = GetEdgeDistance(segmentIndex, 0);
            double dist1 = GetEdgeDistance(segmentIndex, 1);
            if (dist0 > dist1)
            {
                _intLineIndex[segmentIndex, 0] = 0;
                _intLineIndex[segmentIndex, 1] = 1;
            }
            else
            {
                _intLineIndex[segmentIndex, 0] = 1;
                _intLineIndex[segmentIndex, 1] = 0;
            }
        }

        /// <summary>
        /// Computes the "edge distance" of an intersection point along the specified input line segment.
        /// </summary>
        /// <param name="segmentIndex">The integer segment index from 0 to 1.</param>
        /// <param name="intIndex">The integer intersection index from 0 to 1.</param>
        /// <returns>The edge distance of the intersection point.</returns>
        public virtual double GetEdgeDistance(int segmentIndex, int intIndex)
        {
            double dist = ComputeEdgeDistance(_intPt[intIndex], _inputLines[segmentIndex, 0], _inputLines[segmentIndex, 1]);
            return dist;
        }

        #region Properties

        /// <summary>
        /// Tests whether the input geometries intersect.
        /// </summary>
        /// <returns><c>true</c> if the input geometries intersect.</returns>
        public virtual bool HasIntersection
        {
            get
            {
                return _result != IntersectionType.NoIntersection;
            }
        }

        /// <summary>
        /// Returns the number of intersection points found.  This will be either 0, 1 or 2.
        /// </summary>
        public virtual int IntersectionNum
        {
            get
            {
                return (int)_result;
            }
        }

        /// <summary>
        /// Gets the array of intersection points
        /// </summary>
        public Coordinate[] IntersectionPoints
        {
            get
            {
                return _intPt;
            }
            protected set
            {
                _intPt = value;
            }
        }

        /// <summary>
        /// Gets or sets a two dimensional array of coordinates representing the input lines for the calculation
        /// </summary>
        protected Coordinate[,] InputLines
        {
            get
            {
                return _inputLines;
            }
            set
            {
                _inputLines = value;
            }
        }

        /// <summary>
        /// This is true if the intersection forms a line
        /// </summary>
        protected virtual bool IsCollinear
        {
            get
            {
                return _result == IntersectionType.Collinear;
            }
        }

        /// <summary>
        /// Gets Whether this is both propper and has an intersection
        /// </summary>
        protected virtual bool IsEndPoint
        {
            get
            {
                return HasIntersection && !_isProper;
            }
        }

        /// <summary>
        /// Tests whether an intersection is proper.
        /// The intersection between two line segments is considered proper if
        /// they intersect in a single point in the interior of both segments
        /// (e.g. the intersection is a single point and is not equal to any of the endpoints).
        /// The intersection between a point and a line segment is considered proper
        /// if the point lies in the interior of the segment (e.g. is not equal to either of the endpoints).
        /// </summary>
        /// <returns><c>true</c>  if the intersection is proper.</returns>
        public virtual bool IsProper
        {
            get
            {
                return HasIntersection && _isProper;
            }
            protected set
            {
                _isProper = value;
            }
        }

        /// <summary>
        /// Gets or sets the integer result
        /// </summary>
        protected IntersectionType Result
        {
            get { return _result; }
            set { _result = value; }
        }

        #endregion
    }
}