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

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Non-robust versions of various fundamental Computational Geometric algorithms,
    /// FOR TESTING PURPOSES ONLY!.
    /// The non-robustness is due to rounding error in floating point computation.
    /// </summary>
    public static class NonRobustCgAlgorithms
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ring"></param>
        /// <returns></returns>
        public static bool IsPointInRing(Coordinate p, Coordinate[] ring)
        {
            int i;		    // point index; i1 = i-1 mod n
            int crossings = 0;	// number of edge/ray crossings
            int nPts = ring.Length;

            /* For each line edge l = (i-1, i), see if it crosses ray from test point in positive x direction. */
            for (i = 1; i < nPts; i++)
            {
                int i1 = i - 1;		    // point index; i1 = i-1 mod n
                Coordinate p1 = ring[i];
                Coordinate p2 = ring[i1];
                double x1 = p1.X - p.X;
                double y1 = p1.Y - p.Y;
                double x2 = p2.X - p.X;
                double y2 = p2.Y - p.Y;

                if (((y1 > 0) && (y2 <= 0)) || ((y2 > 0) && (y1 <= 0)))
                {
                    /* e straddles x axis, so compute intersection. */
                    double xInt = (x1 * y2 - x2 * y1) / (y2 - y1);		    // x intersection of e with ray
                    /* crosses ray if strictly positive intersection. */
                    if (0.0 < xInt) crossings++;
                }
            }

            /* p is inside if an odd number of crossings. */
            return (crossings % 2) == 1;
        }

        /// <summary>
        /// Computes whether a ring defined by an array of <c>Coordinate</c> is
        /// oriented counter-clockwise.
        /// This will handle coordinate lists which contain repeated points.
        /// </summary>
        /// <param name="ring">an array of coordinates forming a ring.</param>
        /// <returns>
        /// <c>true</c> if the ring is oriented counter-clockwise.
        /// throws <c>ArgumentException</c> if the ring is degenerate (does not contain 3 different points)
        /// </returns>
        public static bool IsCcw(Coordinate[] ring)
        {
            // # of points without closing endpoint
            int nPts = ring.Length - 1;

            // check that this is a valid ring - if not, simply return a dummy value
            if (nPts < 4)
                return false;

            // algorithm to check if a Ring is stored in CCW order
            // find highest point
            Coordinate hip = ring[0];
            int hii = 0;
            for (int i = 1; i <= nPts; i++)
            {
                Coordinate p = ring[i];
                if (p.Y > hip.Y)
                {
                    hip = p;
                    hii = i;
                }
            }

            // find different point before highest point
            int iPrev = hii;
            do
                iPrev = (iPrev - 1) % nPts;
            while (ring[iPrev].Equals(hip) && iPrev != hii);

            // find different point after highest point
            int iNext = hii;
            do
                iNext = (iNext + 1) % nPts;
            while (ring[iNext].Equals(hip) && iNext != hii);

            Coordinate prev = ring[iPrev];
            Coordinate next = ring[iNext];
            if (prev.Equals(hip) || next.Equals(hip) || prev.Equals(next))
                throw new ArgumentException("degenerate ring (does not contain 3 different points)");

            // translate so that hip is at the origin.
            // This will not affect the area calculation, and will avoid
            // finite-accuracy errors (i.e very small vectors with very large coordinates)
            // This also simplifies the discriminant calculation.
            double prev2X = prev.X - hip.X;
            double prev2Y = prev.Y - hip.Y;
            double next2X = next.X - hip.X;
            double next2Y = next.Y - hip.Y;

            // compute cross-product of vectors hip->next and hip->prev
            // (e.g. area of parallelogram they enclose)
            double disc = next2X * prev2Y - next2Y * prev2X;

            /* If disc is exactly 0, lines are collinear.  There are two possible cases:
                    (1) the lines lie along the x axis in opposite directions
                    (2) the line lie on top of one another

                    (2) should never happen, so we're going to ignore it!
                        (Might want to assert this)

                    (1) is handled by checking if next is left of prev ==> CCW
            */
            if (disc == 0.0)
                return (prev.X > next.X); // poly is CCW if prev x is right of next x
            return (disc > 0.0);      // if area is positive, points are ordered CCW
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static int ComputeOrientation(Coordinate p1, Coordinate p2, Coordinate q)
        {
            double dx1 = p2.X - p1.X;
            double dy1 = p2.Y - p1.Y;
            double dx2 = q.X - p2.X;
            double dy2 = q.Y - p2.Y;
            double det = dx1 * dy2 - dx2 * dy1;

            if (det > 0.0) return 1;
            if (det < 0.0) return -1;
            return 0;
        }
    }
}