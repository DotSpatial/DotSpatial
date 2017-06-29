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

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Specifies and implements various fundamental Computational Geometric algorithms.
    /// The algorithms supplied in this class are robust for double-precision floating point.
    /// </summary>
    public static class CgAlgorithms
    {
        /// <summary>
        /// A value that indicates an orientation of clockwise, or a right turn.
        /// </summary>
        public const int CLOCKWISE = -1;
        /// <summary>
        /// A value that indicates an orientation of clockwise, or a right turn.
        /// </summary>
        public const int RIGHT = CLOCKWISE;

        /// <summary>
        /// A value that indicates an orientation of counterclockwise, or a left turn.
        /// </summary>
        public const int COUNTER_CLOCKWISE = 1;
        /// <summary>
        /// A value that indicates an orientation of counterclockwise, or a left turn.
        /// </summary>
        public const int LEFT = COUNTER_CLOCKWISE;

        /// <summary>
        /// A value that indicates an orientation of collinear, or no turn (straight).
        /// </summary>
        public const int COLLINEAR = 0;
        /// <summary>
        /// A value that indicates an orientation of collinear, or no turn (straight).
        /// </summary>
        public const int STRAIGHT = COLLINEAR;

        /// <summary>
        /// Returns the index of the direction of the point <c>q</c>
        /// relative to a vector specified by <c>p1-p2</c>.
        /// </summary>
        /// <param name="p1">The origin point of the vector.</param>
        /// <param name="p2">The final point of the vector.</param>
        /// <param name="q">The point to compute the direction to.</param>
        /// <returns>
        /// 1 if q is counter-clockwise (left) from p1-p2,
        /// -1 if q is clockwise (right) from p1-p2,
        /// 0 if q is collinear with p1-p2.
        /// </returns>
        public static int OrientationIndex(Coordinate p1, Coordinate p2, Coordinate q)
        {
            // travelling along p1->p2, turn counter clockwise to get to q return 1,
            // travelling along p1->p2, turn clockwise to get to q return -1,
            // p1, p2 and q are colinear return 0.
            double dx1 = p2.X - p1.X;
            double dy1 = p2.Y - p1.Y;
            double dx2 = q.X - p2.X;
            double dy2 = q.Y - p2.Y;
            return RobustDeterminant.SignOfDet2X2(dx1, dy1, dx2, dy2);
        }

        /// <summary>
        /// Test whether a point lies inside a ring.
        /// The ring may be oriented in either direction.
        /// If the point lies on the ring boundary the result of this method is unspecified.
        /// This algorithm does not attempt to first check the point against the envelope
        /// of the ring.
        /// </summary>
        /// <param name="p">Point to check for ring inclusion.</param>
        /// <param name="ring">Assumed to have first point identical to last point.</param>
        /// <returns><c>true</c> if p is inside ring.</returns>
        public static bool IsPointInRing(Coordinate p, IList<Coordinate> ring)
        {
            int i;
            int crossings = 0;  // number of segment/ray crossings
            int nPts = ring.Count;

            /*
            *  For each segment l = (i-1, i), see if it crosses ray from test point in positive x direction.
            */
            for (i = 1; i < nPts; i++)
            {
                int i1 = i - 1;             // point index; i1 = i-1
                Coordinate p1 = ring[i];
                Coordinate p2 = ring[i1];
                double x1 = p1.X - p.X;          // translated coordinates
                double y1 = p1.Y - p.Y;
                double x2 = p2.X - p.X;
                double y2 = p2.Y - p.Y;

                if (((y1 > 0) && (y2 <= 0)) || ((y2 > 0) && (y1 <= 0)))
                {
                    /*
                    *  segment straddles x axis, so compute intersection.
                    */
                    double xInt = RobustDeterminant.SignOfDet2X2(x1, y1, x2, y2) / (y2 - y1);        // x intersection of segment with ray

                    /*
                    *  crosses ray if strictly positive intersection.
                    */
                    if (0.0 < xInt)
                        crossings++;
                }
            }

            /*
            *  p is inside if number of crossings is odd.
            */
            if ((crossings % 2) == 1)
                return true;
            return false;
        }

        /// <summary>
        /// Test whether a point lies on the line segments defined by a
        /// list of coordinates.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pt"></param>
        /// <returns>
        /// <c>true</c> true if
        /// the point is a vertex of the line or lies in the interior of a line
        /// segment in the linestring.
        /// </returns>
        public static bool IsOnLine(Coordinate p, IList<Coordinate> pt)
        {
            LineIntersector lineIntersector = new RobustLineIntersector();
            for (int i = 1; i < pt.Count; i++)
            {
                Coordinate p0 = pt[i - 1];
                Coordinate p1 = pt[i];
                lineIntersector.ComputeIntersection(p, p0, p1);
                if (lineIntersector.HasIntersection)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Computes whether a ring defined by an array of <see cref="Coordinate" />s is oriented counter-clockwise.
        /// The list of points is assumed to have the first and last points equal.
        /// This will handle coordinate lists which contain repeated points.
        /// This algorithm is only guaranteed to work with valid rings.
        /// If the ring is invalid (e.g. self-crosses or touches),
        /// the computed result may not be correct.
        /// </summary>>
        /// <param name="ring"></param>
        /// <returns></returns>
        public static bool IsCounterClockwise(IList<Coordinate> ring)
        {
            // # of points without closing endpoint
            int nPts = ring.Count - 1;

            // find highest point
            Coordinate hiPt = ring[0];
            int hiIndex = 0;
            for (int i = 1; i <= nPts; i++)
            {
                Coordinate p = ring[i];
                if (p.Y > hiPt.Y)
                {
                    hiPt = p;
                    hiIndex = i;
                }
            }

            // find distinct point before highest point
            int iPrev = hiIndex;
            do
            {
                iPrev = iPrev - 1;
                if (iPrev < 0) iPrev = nPts;
            }
            while (ring[iPrev].Equals2D(hiPt) && iPrev != hiIndex);

            // find distinct point after highest point
            int iNext = hiIndex;
            do
                iNext = (iNext + 1) % nPts;
            while (ring[iNext].Equals2D(hiPt) && iNext != hiIndex);

            Coordinate prev = new Coordinate(ring[iPrev]);
            Coordinate next = new Coordinate(ring[iNext]);

            /*
             * This check catches cases where the ring contains an A-B-A configuration of points.
             * This can happen if the ring does not contain 3 distinct points
             * (including the case where the input array has fewer than 4 elements),
             * or it contains coincident line segments.
             */
            if (prev.Equals2D(hiPt) || next.Equals2D(hiPt) || prev.Equals2D(next))
                return false;

            int disc = ComputeOrientation(prev, new Coordinate(hiPt), next);

            /*
             *  If disc is exactly 0, lines are collinear.  There are two possible cases:
             *  (1) the lines lie along the x axis in opposite directions
             *  (2) the lines lie on top of one another
             *
             *  (1) is handled by checking if next is left of prev ==> CCW
             *  (2) will never happen if the ring is valid, so don't check for it
             *  (Might want to assert this)
             */
            bool isCcw;
            if (disc == 0)
                // poly is CCW if prev x is right of next x
                isCcw = (prev.X > next.X);
            else
                // if area is positive, points are ordered CCW
                isCcw = (disc > 0);
            return isCcw;
        }

        /// <summary>
        /// Computes the orientation of a point q to the directed line segment p1-p2.
        /// The orientation of a point relative to a directed line segment indicates
        /// which way you turn to get to q after travelling from p1 to p2.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="q"></param>
        /// <returns>
        /// 1 if q is counter-clockwise from p1-p2,
        /// -1 if q is clockwise from p1-p2,
        /// 0 if q is collinear with p1-p2-
        /// </returns>
        public static int ComputeOrientation(Coordinate p1, Coordinate p2, Coordinate q)
        {
            return OrientationIndex(p1, p2, q);
        }

        /// <summary>
        /// Computes the distance from a point p to a line segment AB.
        /// Notice: NON-ROBUST!
        /// </summary>
        /// <param name="p">The point to compute the distance for.</param>
        /// <param name="a">One point of the line.</param>
        /// <param name="b">Another point of the line (must be different to A).</param>
        /// <returns> The distance from p to line segment AB.</returns>
        public static double DistancePointLine(Coordinate p, Coordinate a, Coordinate b)
        {
            // if start == end, then use pt distance
            if (a.Equals(b))
                return p.Distance(a);

            // otherwise use comp.graphics.algorithms Frequently Asked Questions method
            /*(1)     	      AC dot AB
                        r =   ---------
                              ||AB||^2

		                r has the following meaning:
		                r=0 Point = A
		                r=1 Point = B
		                r<0 Point is on the backward extension of AB
		                r>1 Point is on the forward extension of AB
		                0<r<1 Point is interior to AB
	        */

            double r = ((p.X - a.X) * (b.X - a.X) + (p.Y - a.Y) * (b.Y - a.Y))
                        /
                        ((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));

            if (r <= 0.0) return p.Distance(a);
            if (r >= 1.0) return p.Distance(b);

            /*(2)
		                    (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
		                s = -----------------------------
		             	                Curve^2

		                Then the distance from C to Point = |s|*Curve.
	        */

            double s = ((a.Y - p.Y) * (b.X - a.X) - (a.X - p.X) * (b.Y - a.Y))
                        /
                        ((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));

            return Math.Abs(s) * Math.Sqrt(((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y)));
        }

        /// <summary>
        /// Computes the perpendicular distance from a point p
        /// to the (infinite) line containing the points AB
        /// </summary>
        /// <param name="p">The point to compute the distance for.</param>
        /// <param name="a">One point of the line.</param>
        /// <param name="b">Another point of the line (must be different to A).</param>
        /// <returns>The perpendicular distance from p to line AB.</returns>
        public static double DistancePointLinePerpendicular(Coordinate p, Coordinate a, Coordinate b)
        {
            // use comp.graphics.algorithms Frequently Asked Questions method
            /*(2)
                            (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
                        s = -----------------------------
                                         Curve^2

                        Then the distance from C to Point = |s|*Curve.
            */

            double s = ((a.Y - p.Y) * (b.X - a.X) - (a.X - p.X) * (b.Y - a.Y))
                        /
                        ((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));

            return Math.Abs(s) * Math.Sqrt(((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y)));
        }

        /// <summary>
        /// Computes the distance from a line segment AB to a line segment CD.
        /// Notice: NON-ROBUST!
        /// </summary>
        /// <param name="a">A point of one line.</param>
        /// <param name="b">The second point of the line (must be different to A).</param>
        /// <param name="c">One point of the line.</param>
        /// <param name="d">Another point of the line (must be different to A).</param>
        /// <returns>The distance from line segment AB to line segment CD.</returns>
        public static double DistanceLineLine(Coordinate a, Coordinate b, Coordinate c, Coordinate d)
        {
            // check for zero-length segments
            if (a.Equals(b))
                return DistancePointLine(a, c, d);
            if (c.Equals(d))
                return DistancePointLine(d, a, b);

            // AB and CD are line segments
            /* from comp.graphics.algo

	            Solving the above for r and s yields
				            (Ay-Cy)(Dx-Cx)-(Ax-Cx)(Dy-Cy)
	                    r = ----------------------------- (eqn 1)
				            (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)

		 	                (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
		                s = ----------------------------- (eqn 2)
			                (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)
	            Let Point be the position vector of the intersection point, then
		            Point=A+r(B-A) or
		            Px=Ax+r(Bx-Ax)
		            Py=Ay+r(By-Ay)
	            By examining the values of r & s, you can also determine some other
                limiting conditions:
		            If 0<=r<=1 & 0<=s<=1, intersection exists
		            r<0 or r>1 or s<0 or s>1 line segments do not intersect
		            If the denominator in eqn 1 is zero, AB & CD are parallel
		            If the numerator in eqn 1 is also zero, AB & CD are collinear.

	        */
            double rTop = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
            double rBot = (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

            double sTop = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);
            double sBot = (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

            if ((rBot == 0) || (sBot == 0))
                return Math.Min(DistancePointLine(a, c, d),
                        Math.Min(DistancePointLine(b, c, d),
                        Math.Min(DistancePointLine(c, a, b),
                        DistancePointLine(d, a, b))));

            double s = sTop / sBot;
            double r = rTop / rBot;

            if ((r < 0) || (r > 1) || (s < 0) || (s > 1))
                //no intersection
                return Math.Min(DistancePointLine(a, c, d),
                        Math.Min(DistancePointLine(b, c, d),
                        Math.Min(DistancePointLine(c, a, b),
                        DistancePointLine(d, a, b))));

            return 0.0; //intersection exists
        }

        /// <summary>
        /// Returns the signed area for a ring.  The area is positive if the ring is oriented CW.
        /// </summary>
        /// <param name="ring"></param>
        /// <returns>Area in Meters (by default) when using projected coordinates.</returns>
        public static double SignedArea(IList<Coordinate> ring)
        {
            if (ring.Count < 3)
                return 0.0;

            double sum = 0.0;
            for (int i = 0; i < ring.Count - 1; i++)
            {
                double bx = ring[i].X;
                double by = ring[i].Y;
                double cx = ring[i + 1].X;
                double cy = ring[i + 1].Y;
                sum += (bx + cx) * (cy - by);
            }

            // wrap the last point to the first if needed.
            Coordinate lastCoordinate = ring[ring.Count - 1];
            Coordinate firstCoordinate = ring[0];
            if (firstCoordinate.X != lastCoordinate.X && firstCoordinate.Y != lastCoordinate.Y)
            {
                sum += (lastCoordinate.X + firstCoordinate.X) * (firstCoordinate.Y - lastCoordinate.Y);
            }

            return -sum / 2.0;
        }

        /// <summary>
        /// Computes the length of a linestring specified by a sequence of points.
        /// </summary>
        /// <param name="pts">The points specifying the linestring.</param>
        /// <returns>The length of the linestring.</returns>
        public static double Length(IList<Coordinate> pts)
        {
            if (pts.Count < 1)
                return 0.0;

            double sum = 0.0;
            for (int i = 1; i < pts.Count; i++)
                sum += pts[i].Distance(pts[i - 1]);

            return sum;
        }
    }
}