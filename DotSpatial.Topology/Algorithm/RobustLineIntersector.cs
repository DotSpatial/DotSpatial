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

using System.Diagnostics;

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// A robust version of <c>LineIntersector</c>.
    /// </summary>
    public class RobustLineIntersector : LineIntersector
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public override void ComputeIntersection(Coordinate p, Coordinate p1, Coordinate p2)
        {
            IsProper = false;
            // do between check first, since it is faster than the orientation test
            if (Envelope.Intersects(p1, p2, p))
            {
                if ((CgAlgorithms.OrientationIndex(p1, p2, p) == 0) && (CgAlgorithms.OrientationIndex(p2, p1, p) == 0))
                {
                    IsProper = true;
                    if (p.Equals(p1) || p.Equals(p2))
                        IsProper = false;
                    Result = IntersectionType.PointIntersection;
                    return;
                }
            }
            Result = IntersectionType.NoIntersection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        public override IntersectionType ComputeIntersect(Coordinate p1, Coordinate p2, Coordinate q1, Coordinate q2)
        {
            IsProper = false;

            // first try a fast test to see if the envelopes of the lines intersect
            if (!Envelope.Intersects(p1, p2, q1, q2))
                return IntersectionType.NoIntersection;

            // for each endpoint, compute which side of the other segment it lies
            // if both endpoints lie on the same side of the other segment,
            // the segments do not intersect
            int pq1 = CgAlgorithms.OrientationIndex(p1, p2, q1);
            int pq2 = CgAlgorithms.OrientationIndex(p1, p2, q2);

            if ((pq1 > 0 && pq2 > 0) || (pq1 < 0 && pq2 < 0))
                return IntersectionType.NoIntersection;

            int qp1 = CgAlgorithms.OrientationIndex(q1, q2, p1);
            int qp2 = CgAlgorithms.OrientationIndex(q1, q2, p2);

            if ((qp1 > 0 && qp2 > 0) || (qp1 < 0 && qp2 < 0))
                return IntersectionType.NoIntersection;

            bool collinear = (pq1 == 0 && pq2 == 0 && qp1 == 0 && qp2 == 0);
            if (collinear)
                return ComputeCollinearIntersection(p1, p2, q1, q2);

            /*
            *  Check if the intersection is an endpoint. If it is, copy the endpoint as
            *  the intersection point. Copying the point rather than computing it
            *  ensures the point has the exact value, which is important for
            *  robustness. It is sufficient to simply check for an endpoint which is on
            *  the other line, since at this point we know that the inputLines must
            *  intersect.
            */
            if (pq1 == 0 || pq2 == 0 || qp1 == 0 || qp2 == 0)
            {
                IsProper = false;
                if (pq1 == 0)
                    IntersectionPoints[0] = new Coordinate(q1);
                if (pq2 == 0)
                    IntersectionPoints[0] = new Coordinate(q2);
                if (qp1 == 0)
                    IntersectionPoints[0] = new Coordinate(p1);
                if (qp2 == 0)
                    IntersectionPoints[0] = new Coordinate(p2);
            }
            else
            {
                IsProper = true;
                IntersectionPoints[0] = Intersection(p1, p2, q1, q2);
            }
            return IntersectionType.PointIntersection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        private IntersectionType ComputeCollinearIntersection(Coordinate p1, Coordinate p2, Coordinate q1, Coordinate q2)
        {
            bool p1Q1P2 = Envelope.Intersects(p1, p2, q1);
            bool p1Q2P2 = Envelope.Intersects(p1, p2, q2);
            bool q1P1Q2 = Envelope.Intersects(q1, q2, p1);
            bool q1P2Q2 = Envelope.Intersects(q1, q2, p2);

            if (p1Q1P2 && p1Q2P2)
            {
                IntersectionPoints[0] = q1;
                IntersectionPoints[1] = q2;
                return IntersectionType.Collinear;
            }
            if (q1P1Q2 && q1P2Q2)
            {
                IntersectionPoints[0] = p1;
                IntersectionPoints[1] = p2;
                return IntersectionType.Collinear;
            }
            if (p1Q1P2 && q1P1Q2)
            {
                IntersectionPoints[0] = q1;
                IntersectionPoints[1] = p1;
                return q1.Equals(p1) ? IntersectionType.PointIntersection : IntersectionType.Collinear;
            }
            if (p1Q1P2 && q1P2Q2)
            {
                IntersectionPoints[0] = q1;
                IntersectionPoints[1] = p2;
                return q1.Equals(p2) ? IntersectionType.PointIntersection : IntersectionType.Collinear;
            }
            if (p1Q2P2 && q1P1Q2)
            {
                IntersectionPoints[0] = q2;
                IntersectionPoints[1] = p1;
                return q2.Equals(p1) ? IntersectionType.PointIntersection : IntersectionType.Collinear;
            }
            if (p1Q2P2 && q1P2Q2)
            {
                IntersectionPoints[0] = q2;
                IntersectionPoints[1] = p2;
                return q2.Equals(p2) ? IntersectionType.PointIntersection : IntersectionType.Collinear;
            }
            return IntersectionType.NoIntersection;
        }

        /// <summary>
        /// This method computes the actual value of the intersection point.
        /// To obtain the maximum precision from the intersection calculation,
        /// the coordinates are normalized by subtracting the minimum
        /// ordinate values (in absolute value).  This has the effect of
        /// removing common significant digits from the calculation to
        /// maintain more bits of precision.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        private Coordinate Intersection(Coordinate p1, Coordinate p2, Coordinate q1, Coordinate q2)
        {
            Coordinate n1 = new Coordinate(p1);
            Coordinate n2 = new Coordinate(p2);
            Coordinate n3 = new Coordinate(q1);
            Coordinate n4 = new Coordinate(q2);
            Coordinate normPt = new Coordinate();
            NormalizeToEnvCentre(n1, n2, n3, n4, normPt);

            Coordinate intPt = HCoordinate.Intersection(n1, n2, n3, n4);

            intPt.X += normPt.X;
            intPt.Y += normPt.Y;

            /*
             *
             * MD - May 4 2005 - This is still a problem.  Here is a failure case:
             *
             * LINESTRING (2089426.5233462777 1180182.3877339689, 2085646.6891757075 1195618.7333999649)
             * LINESTRING (1889281.8148903656 1997547.0560044837, 2259977.3672235999 483675.17050843034)
             * int point = (2097408.2633752143, 1144595.8008114607)
             */
            if (!IsInSegmentEnvelopes(intPt))
                Trace.WriteLine("Intersection outside segment envelopes: " + intPt);

            /*
            // disabled until a better solution is found
            if (!IsInSegmentEnvelopes(intPt))
            {
                Trace.WriteLine("first value outside segment envelopes: " + intPt);

                IteratedBisectionIntersector ibi = new IteratedBisectionIntersector(p1, p2, q1, q2);
                intPt = ibi.Intersection;
            }
            if (!IsInSegmentEnvelopes(intPt))
            {
                Trace.WriteLine("ERROR - outside segment envelopes: " + intPt);

                IteratedBisectionIntersector ibi = new IteratedBisectionIntersector(p1, p2, q1, q2);
                Coordinate testPt = ibi.Intersection;
            }
            */

            if (PrecisionModel != null)
                PrecisionModel.MakePrecise(intPt);

            return intPt;
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="n1"></param>
        ///// <param name="n2"></param>
        ///// <param name="n3"></param>
        ///// <param name="n4"></param>
        ///// <param name="normPt"></param>
        //private void NormalizeToMinimum(Coordinate n1, Coordinate n2, Coordinate n3, Coordinate n4, Coordinate normPt)
        //{
        //    normPt.X = SmallestInAbsValue(n1.X, n2.X, n3.X, n4.X);
        //    normPt.Y = SmallestInAbsValue(n1.Y, n2.Y, n3.Y, n4.Y);
        //    n1.X -= normPt.X; n1.Y -= normPt.Y;
        //    n2.X -= normPt.X; n2.Y -= normPt.Y;
        //    n3.X -= normPt.X; n3.Y -= normPt.Y;
        //    n4.X -= normPt.X; n4.Y -= normPt.Y;
        //}

        /// <summary>
        ///  Normalize the supplied coordinates to
        /// so that the midpoint of their intersection envelope
        /// lies at the origin.
        /// </summary>
        /// <param name="n00"></param>
        /// <param name="n01"></param>
        /// <param name="n10"></param>
        /// <param name="n11"></param>
        /// <param name="normPt"></param>
        private static void NormalizeToEnvCentre(Coordinate n00, Coordinate n01, Coordinate n10, Coordinate n11, Coordinate normPt)
        {
            double minX0 = n00.X < n01.X ? n00.X : n01.X;
            double minY0 = n00.Y < n01.Y ? n00.Y : n01.Y;
            double maxX0 = n00.X > n01.X ? n00.X : n01.X;
            double maxY0 = n00.Y > n01.Y ? n00.Y : n01.Y;

            double minX1 = n10.X < n11.X ? n10.X : n11.X;
            double minY1 = n10.Y < n11.Y ? n10.Y : n11.Y;
            double maxX1 = n10.X > n11.X ? n10.X : n11.X;
            double maxY1 = n10.Y > n11.Y ? n10.Y : n11.Y;

            double intMinX = minX0 > minX1 ? minX0 : minX1;
            double intMaxX = maxX0 < maxX1 ? maxX0 : maxX1;
            double intMinY = minY0 > minY1 ? minY0 : minY1;
            double intMaxY = maxY0 < maxY1 ? maxY0 : maxY1;

            double intMidX = (intMinX + intMaxX) / 2.0;
            double intMidY = (intMinY + intMaxY) / 2.0;
            normPt.X = intMidX;
            normPt.Y = intMidY;

            n00.X -= normPt.X; n00.Y -= normPt.Y;
            n01.X -= normPt.X; n01.Y -= normPt.Y;
            n10.X -= normPt.X; n10.Y -= normPt.Y;
            n11.X -= normPt.X; n11.Y -= normPt.Y;
        }

        ///// <summary>
        /////
        ///// </summary>
        ///// <param name="x1"></param>
        ///// <param name="x2"></param>
        ///// <param name="x3"></param>
        ///// <param name="x4"></param>
        ///// <returns></returns>
        //private double SmallestInAbsValue(double x1, double x2, double x3, double x4)
        //{
        //    double x = x1;
        //    double xabs = Math.Abs(x);
        //    if (Math.Abs(x2) < xabs)
        //    {
        //        x = x2;
        //        xabs = Math.Abs(x2);
        //    }
        //    if (Math.Abs(x3) < xabs)
        //    {
        //        x = x3;
        //        xabs = Math.Abs(x3);
        //    }
        //    if (Math.Abs(x4) < xabs)
        //        x = x4;
        //    return x;
        //}

        /// <summary>
        /// Test whether a point lies in the envelopes of both input segments.
        /// A correctly computed intersection point should return <c>true</c>
        /// for this test.
        /// Since this test is for debugging purposes only, no attempt is
        /// made to optimize the envelope test.
        /// </summary>
        /// <param name="intPt"></param>
        /// <returns><c>true</c> if the input point lies within both input segment envelopes.</returns>
        private bool IsInSegmentEnvelopes(Coordinate intPt)
        {
            Envelope env0 = new Envelope(InputLines[0, 0], InputLines[0, 1]);
            Envelope env1 = new Envelope(InputLines[1, 0], InputLines[1, 1]);
            return env0.Contains(intPt) && env1.Contains(intPt);
        }
    }
}