// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://dotspatial.codeplex.com/license and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology.Index.Strtree;

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Implements <c>PointInRing</c> using a <c>SIRtree</c> index to increase performance.
    /// </summary>
    public class SiRtreePointInRing : IPointInRing
    {
        private readonly LinearRing _ring;
        private int _crossings;  // number of segment/ray crossings
        private SiRtree _sirTree;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ring"></param>
        public SiRtreePointInRing(LinearRing ring)
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

            // test all segments intersected by vertical ray at pt
            IList segs = _sirTree.Query(pt.Y);

            for (IEnumerator i = segs.GetEnumerator(); i.MoveNext(); )
            {
                LineSegment seg = (LineSegment)i.Current;
                TestLineSegment(pt, seg);
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
            _sirTree = new SiRtree();
            IList<Coordinate> pts = _ring.Coordinates;
            for (int i = 1; i < pts.Count; i++)
            {
                if (pts[i - 1].Equals(pts[i])) { continue; }
                LineSegment seg = new LineSegment(pts[i - 1], pts[i]);
                _sirTree.Insert(seg.P0.Y, seg.P1.Y, seg);
            }
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
    }
}