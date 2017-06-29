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
using DotSpatial.Topology.Index.Bintree;
using DotSpatial.Topology.Index.Chain;

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Implements <c>IPointInRing</c>
    /// using a <c>MonotoneChain</c>s and a <c>BinTree</c> index to increase performance.
    /// </summary>
    public sealed class McPointInRing : IPointInRing
    {
        private readonly Interval _interval = new Interval();
        private readonly ILinearRing _ring;
        private int _crossings;  // number of segment/ray crossings
        private Bintree _tree;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ring"></param>
        public McPointInRing(ILinearRing ring)
        {
            _ring = ring;
            BuildIndex();
        }

        #region IPointInRing Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public bool IsInside(Coordinate pt)
        {
            _crossings = 0;

            // test all segments intersected by ray from pt in positive x direction
            Envelope rayEnv = new Envelope(Double.NegativeInfinity, Double.PositiveInfinity, pt.Y, pt.Y);
            _interval.Min = pt.Y;
            _interval.Max = pt.Y;
            IList segs = _tree.Query(_interval);

            McSelecter mcSelecter = new McSelecter(this, pt);
            for (IEnumerator i = segs.GetEnumerator(); i.MoveNext(); )
            {
                MonotoneChain mc = (MonotoneChain)i.Current;
                TestMonotoneChain(rayEnv, mcSelecter, mc);
            }

            /*
            *  p is inside if number of crossings is odd.
            */
            if ((_crossings % 2) == 1)
                return true;
            return false;
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        private void BuildIndex()
        {
            _tree = new Bintree();

            IList<Coordinate> pts = CoordinateArrays.RemoveRepeatedPoints(_ring.Coordinates);
            IList mcList = MonotoneChainBuilder.GetChains(pts);

            for (int i = 0; i < mcList.Count; i++)
            {
                MonotoneChain mc = (MonotoneChain)mcList[i];
                Envelope mcEnv = mc.Envelope;
                _interval.Min = mcEnv.Minimum.Y;
                _interval.Max = mcEnv.Maximum.Y;
                _tree.Insert(_interval, mc);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rayEnv"></param>
        /// <param name="mcSelecter"></param>
        /// <param name="mc"></param>
        private static void TestMonotoneChain(IEnvelope rayEnv, MonotoneChainSelectAction mcSelecter, MonotoneChain mc)
        {
            mc.Select(rayEnv, mcSelecter);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="seg"></param>
        private void TestLineSegment(Coordinate p, ILineSegmentBase seg)
        {
            /*
            *  Test if segment crosses ray from test point in positive x direction.
            */
            Coordinate p1 = seg.P0;
            Coordinate p2 = seg.P1;
            double x1 = p1.X - p.X;
            double y1 = p1.Y - p.Y;
            double x2 = p2.X - p.X;
            double y2 = p2.Y - p.Y;

            if (((y1 > 0) && (y2 <= 0)) || ((y2 > 0) && (y1 <= 0)))
            {
                /*
                *  segment straddles x axis, so compute intersection.
                */
                double xInt = RobustDeterminant.SignOfDet2X2(x1, y1, x2, y2) / (y2 - y1);  // x intersection of segment with ray

                /*
                *  crosses ray if strictly positive intersection.
                */
                if (0.0 < xInt)
                    _crossings++;
            }
        }

        #region Nested type: McSelecter

        /// <summary>
        ///
        /// </summary>
        private class McSelecter : MonotoneChainSelectAction
        {
            private readonly McPointInRing _container;
            private readonly Coordinate _p = Coordinate.Empty;

            /// <summary>
            ///
            /// </summary>
            /// <param name="container"></param>
            /// <param name="p"></param>
            public McSelecter(McPointInRing container, Coordinate p)
            {
                _container = container;
                _p = p;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="ls"></param>
            public override void Select(LineSegment ls)
            {
                _container.TestLineSegment(_p, ls);
            }
        }

        #endregion
    }
}