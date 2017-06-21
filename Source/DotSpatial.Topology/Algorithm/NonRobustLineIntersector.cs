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
    /// A non-robust version of <c>LineIntersector</c>.
    /// </summary>
    public class NonRobustLineIntersector : LineIntersector
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>
        /// <c>true</c> if both numbers are positive or if both numbers are negative,
        /// <c>false</c> if both numbers are zero.
        /// </returns>
        public static bool IsSameSignAndNonZero(double a, double b)
        {
            if (a == 0 || b == 0)
                return false;
            return (a < 0 && b < 0) || (a > 0 && b > 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public override void ComputeIntersection(Coordinate p, Coordinate p1, Coordinate p2)
        {
            /*
            *  Coefficients of line eqns.
            */

            /*
            *  'Sign' values
            */

            IsProper = false;

            /*
            *  Compute a1, b1, c1, where line joining points 1 and 2
            *  is "a1 x  +  b1 y  +  c1  =  0".
            */
            double a1 = p2.Y - p1.Y;
            double b1 = p1.X - p2.X;
            double c1 = p2.X * p1.Y - p1.X * p2.Y;

            /*
            *  Compute r3 and r4.
            */
            double r = a1 * p.X + b1 * p.Y + c1;

            // if r != 0 the point does not lie on the line
            if (r != 0)
            {
                Result = IntersectionType.NoIntersection;
                return;
            }

            // Point lies on line - check to see whether it lies in line segment.

            double dist = RParameter(p1, p2, p);
            if (dist < 0.0 || dist > 1.0)
            {
                Result = IntersectionType.NoIntersection;
                return;
            }

            IsProper = true;
            if (p.Equals(p1) || p.Equals(p2))
            {
                IsProper = false;
            }

            Result = IntersectionType.PointIntersection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <returns></returns>
        public override IntersectionType ComputeIntersect(Coordinate p1, Coordinate p2, Coordinate p3, Coordinate p4)
        {
            /*
            *  Coefficients of line eqns.
            */

            /*
            *  'Sign' values
            */

            IsProper = false;

            /*
            *  Compute a1, b1, c1, where line joining points 1 and 2
            *  is "a1 x  +  b1 y  +  c1  =  0".
            */
            double a1 = p2.Y - p1.Y;
            double b1 = p1.X - p2.X;
            double c1 = p2.X * p1.Y - p1.X * p2.Y;

            /*
            *  Compute r3 and r4.
            */
            double r3 = a1 * p3.X + b1 * p3.Y + c1;
            double r4 = a1 * p4.X + b1 * p4.Y + c1;

            /*
            *  Check signs of r3 and r4.  If both point 3 and point 4 lie on
            *  same side of line 1, the line segments do not intersect.
            */
            if (r3 != 0 && r4 != 0 && IsSameSignAndNonZero(r3, r4))
            {
                return IntersectionType.NoIntersection;
            }

            /*
            *  Compute a2, b2, c2
            */
            double a2 = p4.Y - p3.Y;
            double b2 = p3.X - p4.X;
            double c2 = p4.X * p3.Y - p3.X * p4.Y;

            /*
            *  Compute r1 and r2
            */
            double r1 = a2 * p1.X + b2 * p1.Y + c2;
            double r2 = a2 * p2.X + b2 * p2.Y + c2;

            /*
            *  Check signs of r1 and r2.  If both point 1 and point 2 lie
            *  on same side of second line segment, the line segments do
            *  not intersect.
            */
            if (r1 != 0 && r2 != 0 && IsSameSignAndNonZero(r1, r2))
            {
                return IntersectionType.NoIntersection;
            }

            /*
            *  Line segments intersect: compute intersection point.
            */
            double denom = a1 * b2 - a2 * b1;
            if (denom == 0)
                return ComputeCollinearIntersection(p1, p2, p3, p4);

            double numX = b1 * c2 - b2 * c1;
            double x = numX / denom;

            double numY = a2 * c1 - a1 * c2;
            double y = numY / denom;

            PointA = new Coordinate(x, y);

            // check if this is a proper intersection BEFORE truncating values,
            // to avoid spurious equality comparisons with endpoints
            IsProper = true;
            if (PointA.Equals(p1) || PointA.Equals(p2) || PointA.Equals(p3) || PointA.Equals(p4))
                IsProper = false;

            // truncate computed point to precision grid
            if (PrecisionModel != null)
                PrecisionModel.MakePrecise(PointA);

            return IntersectionType.PointIntersection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <returns></returns>
        private IntersectionType ComputeCollinearIntersection(Coordinate p1, Coordinate p2, Coordinate p3, Coordinate p4)
        {
            Coordinate q3;
            Coordinate q4;
            double t3;
            double t4;
            const double r1 = 0;
            const double r2 = 1;
            double r3 = RParameter(p1, p2, p3);
            double r4 = RParameter(p1, p2, p4);

            // make sure p3-p4 is in same direction as p1-p2
            if (r3 < r4)
            {
                q3 = new Coordinate(p3);
                t3 = r3;
                q4 = new Coordinate(p4);
                t4 = r4;
            }
            else
            {
                q3 = new Coordinate(p4);
                t3 = r4;
                q4 = new Coordinate(p3);
                t4 = r3;
            }

            // check for no intersection
            if (t3 > r2 || t4 < r1)
                return IntersectionType.NoIntersection;

            // check for single point intersection
            if (q4 == p1)
            {
                PointA = p1;
                return IntersectionType.PointIntersection;
            }
            if (q3 == p2)
            {
                PointA = p2;
                return IntersectionType.PointIntersection;
            }

            // intersection MUST be a segment - compute endpoints
            PointA = p1;
            if (t3 > r1) PointA = q3;

            PointB = p2;
            if (t4 < r2) PointB = q4;

            return IntersectionType.Collinear;
        }

        /// <summary>
        /// RParameter computes the parameter for the point p
        /// in the parameterized equation
        /// of the line from p1 to p2.
        /// This is equal to the 'distance' of p along p1-p2.
        /// </summary>
        private static double RParameter(Coordinate p1, Coordinate p2, Coordinate p)
        {
            // compute maximum delta, for numerical stability
            // also handle case of p1-p2 being vertical or horizontal
            double r;
            double dx = Math.Abs(p2.X - p1.X);
            double dy = Math.Abs(p2.Y - p1.Y);

            if (dx > dy)
                r = (p.X - p1.X) / (p2.X - p1.X);
            else r = (p.Y - p1.Y) / (p2.Y - p1.Y);

            return r;
        }
    }
}