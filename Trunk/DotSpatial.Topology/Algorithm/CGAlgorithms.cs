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
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Mathematics;

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Specifies and implements various fundamental Computational Geometric algorithms.
    /// The algorithms supplied in this class are robust for double-precision floating point.
    /// </summary>
    public static class CgAlgorithms
    {
        #region Constant Fields

        /// <summary>
        /// A value that indicates an orientation of clockwise, or a right turn.
        /// </summary>
        public const int Clockwise = -1;

        /// <summary>
        /// A value that indicates an orientation of collinear, or no turn (straight).
        /// </summary>
        public const int Collinear = 0;

        /// <summary>
        /// A value that indicates an orientation of counterclockwise, or a left turn.
        /// </summary>
        public const int CounterClockwise = 1;

        /// <summary>
        /// A value that indicates an orientation of counterclockwise, or a left turn.
        /// </summary>
        public const int Left = CounterClockwise;

        /// <summary> 
        /// A value that indicates an orientation of clockwise, or a right turn.
        /// </summary>
        public const int Right = Clockwise;

        /// <summary>
        /// A value that indicates an orientation of collinear, or no turn (straight).
        /// </summary>
        public const int Straight = Collinear;

        #endregion

        #region Methods

        /// <summary>
        /// Computes the orientation of a point q to the directed line segment p1-p2.
        /// The orientation of a point relative to a directed line segment indicates
        /// which way you turn to get to q after travelling from p1 to p2.
        /// </summary>
        /// <param name="p1">The first vertex of the line segment</param>
        /// <param name="p2">The second vertex of the line segment</param>
        /// <param name="q">The point to compute the relative orientation of</param>
        /// <returns> 
        /// 1 if q is counter-clockwise from p1-p2,
        /// or -1 if q is clockwise from p1-p2,
        /// or 0 if q is collinear with p1-p2
        /// </returns>
        public static int ComputeOrientation(Coordinate p1, Coordinate p2, Coordinate q)
        {
            return OrientationIndex(p1, p2, q);
        }

        /// <summary> 
        /// Computes the distance from a line segment AB to a line segment CD.
        /// Note: NON-ROBUST!
        /// </summary>
        /// <param name="a">A point of one line.</param>
        /// <param name="b">The second point of the line (must be different to A).</param>
        /// <param name="c">One point of the line.</param>
        /// <param name="d">Another point of the line (must be different to A).</param>
        /// <returns>The distance from line segment AB to line segment CD.</returns>
        public static double DistanceLineLine(Coordinate a, Coordinate b, Coordinate c, Coordinate d)
        {
            // check for zero-length segments
            if (a.Equals(b)) return DistancePointLine(a, c, d);
            if (c.Equals(d)) return DistancePointLine(d, a, b);

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
            bool noIntersection = false;
            if (!Envelope.Intersects(a, b, c, d))
                noIntersection = true;
            else
            {
                double denom = (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

                if (denom == 0)
                    noIntersection = true;
                else
                {
                    double rNum = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
                    double sNum = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);

                    double s = sNum / denom;
                    double r = rNum / denom;

                    if ((r < 0) || (r > 1) || (s < 0) || (s > 1))
                        noIntersection = true;
                }
            }
            if (noIntersection)
            {
                return MathUtil.Min(
                      DistancePointLine(a, c, d),
                      DistancePointLine(b, c, d),
                      DistancePointLine(c, a, b),
                      DistancePointLine(d, a, b));
            }
            // segments intersect
            return 0.0;
        }

        /// <summary> 
        /// Computes the distance from a point p to a line segment AB.
        /// Note: NON-ROBUST!
        /// </summary>
        /// <param name="p">The point to compute the distance for.</param>
        /// <param name="a">One point of the line.</param>
        /// <param name="b">Another point of the line (must be different to A).</param>
        /// <returns> The distance from p to line segment AB.</returns>
        public static double DistancePointLine(Coordinate p, Coordinate a, Coordinate b)
        {
            // if start = end, then just compute distance to one of the endpoints
            if (a.Equals(b)) return p.Distance(a);

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

            double len2 = ((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
            double r = ((p.X - a.X) * (b.X - a.X) + (p.Y - a.Y) * (b.Y - a.Y)) / len2;

            if (r <= 0.0) return p.Distance(a);
            if (r >= 1.0) return p.Distance(b);

            /*(2)
		                    (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
		                s = -----------------------------
		             	                Curve^2

		                Then the distance from C to Point = |s|*Curve.
      
                        This is the same calculation as {@link #distancePointLinePerpendicular}.
                        Unrolled here for performance.
	        */

            double s = ((a.Y - p.Y) * (b.X - a.X) - (a.X - p.X) * (b.Y - a.Y)) / len2;
            return Math.Abs(s) * Math.Sqrt(len2);
        }

        /// <summary>
        /// Computes the distance from a point to a sequence of line segments.
        /// </summary>
        /// <param name="p">A point</param>
        /// <param name="line">A sequence of contiguous line segments defined by their vertices</param>
        /// <returns>The minimum distance between the point and the line segments</returns>
        /// <exception cref="ArgumentException">If there are too few points to make up a line (at least one?)</exception>
        public static double DistancePointLine(Coordinate p, IList<Coordinate> line)
        {
            if (line.Count == 0)
                throw new ArgumentException("Line array must contain at least one vertex");

            // this handles the case of length = 1
            double minDistance = p.Distance(line[0]);
            for (int i = 0; i < line.Count - 1; i++)
            {
                double dist = DistancePointLine(p, line[i], line[i + 1]);
                if (dist < minDistance) minDistance = dist;
            }
            return minDistance;
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
            double len2 = ((b.X - a.X) * (b.X - a.X) + (b.Y - a.Y) * (b.Y - a.Y));
            double s = ((a.Y - p.Y) * (b.X - a.X) - (a.X - p.X) * (b.Y - a.Y)) / len2;
            return Math.Abs(s) * Math.Sqrt(len2);
        }

        /// <summary>
        /// Computes whether a ring defined by an array of <see cref="Coordinate" />s is oriented counter-clockwise.
        /// </summary>>
        /// <remarks>
        /// <list type="Bullet">
        /// <item>The list of points is assumed to have the first and last points equal.</item>
        /// <item>This will handle coordinate lists which contain repeated points.</item>
        /// </list>
        /// <para>This algorithm is only guaranteed to work with valid rings. If the ring is invalid (e.g. self-crosses or touches), the computed result may not be correct.</para>
        /// </remarks>
        /// <param name="ring">An array of <see cref="Coordinate"/>s froming a ring</param>
        /// <returns>true if the ring is oriented <see cref="Orientation.CounterClockwise"/></returns>
        /// <exception cref="ArgumentException">If there are too few points to determine orientation (&lt;4)</exception>
        public static bool IsCounterClockwise(IList<Coordinate> ring)
        {
            // # of points without closing endpoint
            int nPts = ring.Count - 1;

            // sanity check
            if (nPts < 3)
                throw new ArgumentException("Ring has fewer than 4 points, so orientation cannot be determined");

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
            if (prev.Equals2D(hiPt) || next.Equals2D(hiPt) || prev.Equals2D(next)) return false;

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
        /// Computes whether a ring defined by a coordinate sequence is oriented counter-clockwise.
        /// </summary>>
        /// <remarks>
        /// <list type="Bullet">
        /// <item>The list of points is assumed to have the first and last points equal.</item>
        /// <item>This will handle coordinate lists which contain repeated points.</item>
        /// </list>
        /// <para>This algorithm is only guaranteed to work with valid rings. If the ring is invalid (e.g. self-crosses or touches), the computed result may not be correct.</para>
        /// </remarks>
        /// <param name="ring">A coordinate sequence froming a ring</param>
        /// <returns>true if the ring is oriented <see cref="Orientation.CounterClockwise"/></returns>
        /// <exception cref="ArgumentException">If there are too few points to determine orientation (&lt;4)</exception>
        public static bool IsCounterClockwise(ICoordinateSequence ring)
        {
            return IsCounterClockwise(ring.ToList());
        }

        /// <summary> 
        /// Tests whether a point lies on the line segments defined by a
        /// list of coordinates.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pt"></param>
        /// <returns>true if the point is a vertex of the line
        /// or lies in the interior of a line segment in the linestring
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
            return LocatePointInRing(p, ring) != LocationType.Exterior;

        }

        /// <summary>
        /// Computes the length of a linestring specified by a sequence of points.
        /// </summary>
        /// <param name="pts">The points specifying the linestring</param>
        /// <returns>The length of the linestring</returns>
        public static double Length(ICoordinateSequence pts)
        {
            // optimized for processing CoordinateSequences
            int n = pts.Count;
            if (n <= 1) return 0.0;

            double len = 0.0;

            Coordinate p = pts.GetCoordinate(0);
            double x0 = p.X;
            double y0 = p.Y;

            for (int i = 1; i < n; i++)
            {
                p = pts.GetCoordinate(i);
                double x1 = p.X;
                double y1 = p.Y;
                double dx = x1 - x0;
                double dy = y1 - y0;

                len += Math.Sqrt(dx * dx + dy * dy);

                x0 = x1;
                y0 = y1;
            }
            return len;
        }

        ///<summary>
        /// Determines whether a point lies in the interior, on the boundary, or in the exterior of a ring.
        ///</summary>
        /// <remarks>
        /// <para>The ring may be oriented in either direction.</para>
        /// <para>This method does <i>not</i> first check the point against the envelope of the ring.</para>
        /// </remarks>
        /// <param name="p">Point to check for ring inclusion</param>
        /// <param name="ring">An array of coordinates representing the ring (which must have first point identical to last point)</param>
        /// <returns>The <see cref="Location"/> of p relative to the ring</returns>
        public static LocationType LocatePointInRing(Coordinate p, IList<Coordinate> ring)
        {
            return RayCrossingCounter.LocatePointInRing(p, ring);
        }

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
            /**
             * MD - 9 Aug 2010
             * It seems that the basic algorithm is slightly orientation dependent,
             * when computing the orientation of a point very close to a line.
             * This is possibly due to the arithmetic in the translation to the origin.
             * 
             * For instance, the following situation produces identical results 
             * in spite of the inverse orientation of the line segment:
             * 
             * Coordinate p0 = new Coordinate(219.3649559090992, 140.84159161824724);
             * Coordinate p1 = new Coordinate(168.9018919682399, -5.713787599646864);
             * 
             * Coordinate p = new Coordinate(186.80814046338352, 46.28973405831556);
             * int orient = orientationIndex(p0, p1, p);
             * int orientInv = orientationIndex(p1, p0, p);

             * A way to force consistent results is to normalize the orientation of the vector
             * using the following code.
             * However, this may make the results of orientationIndex inconsistent
             * through the triangle of points, so it's not clear this is 
             * an appropriate patch.
             * 
             */
            return CgAlgorithmsDoubleDouble.OrientationIndex(p1, p2, q);
        }

        /// <summary>
        /// Computes the signed area for a ring.
        /// <remarks>
        /// <para>
        /// The signed area is
        /// </para>  
        /// <list type="Table">
        /// <item>positive</item><description>if the ring is oriented CW</description>
        /// <item>negative</item><description>if the ring is oriented CCW</description>
        /// <item>zero</item><description>if the ring is degenerate or flat</description>
        /// </list>
        /// </remarks>
        /// </summary>
        /// <param name="ring">The coordinates of the ring</param>
        /// <returns>The signed area of the ring</returns>
        public static double SignedArea(IList<Coordinate> ring)
        {
            if (ring.Count < 3) return 0.0;

            double sum = 0.0;
            /**
             * Based on the Shoelace formula.
             * http://en.wikipedia.org/wiki/Shoelace_formula
             */
            double x0 = ring[0].X;
            for (int i = 1; i < ring.Count - 1; i++)
            {
                double x = ring[i].X - x0;
                double y1 = ring[i + 1].Y;
                double y2 = ring[i - 1].Y;
                sum += x * (y2 - y1);
            }
            return sum / 2.0;
        }

        /// <summary>
        /// Computes the signed area for a ring.
        /// <remarks>
        /// <para>
        /// The signed area is
        /// </para>  
        /// <list type="Table">
        /// <item>positive</item><description>if the ring is oriented CW</description>
        /// <item>negative</item><description>if the ring is oriented CCW</description>
        /// <item>zero</item><description>if the ring is degenerate or flat</description>
        /// </list>
        /// </remarks>
        /// </summary>
        /// <param name="ring">The coordinates forming the ring</param>
        /// <returns>The signed area of the ring</returns>
        public static double SignedArea(ICoordinateSequence ring)
        {
            int n = ring.Count;
            if (n < 3) return 0.0;
            /**
             * Based on the Shoelace formula.
             * http://en.wikipedia.org/wiki/Shoelace_formula
             */
            Coordinate p0 = new Coordinate();
            Coordinate p1 = ring.GetCoordinate(0);
            Coordinate p2 = ring.GetCoordinate(1);
            double x0 = p1.X;
            p2.X -= x0;
            double sum = 0.0;
            for (int i = 1; i < n - 1; i++)
            {
                p0.Y = p1.Y;
                p1.X = p2.X;
                p1.Y = p2.Y;
                p2 = ring.GetCoordinate(i + 1);
                p2.X -= x0;
                sum += p1.X * (p0.Y - p2.Y);
            }
            return sum / 2.0;
        }

        #endregion
    }
}