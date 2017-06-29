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
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Index.Chain;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Computes the intersections between two line segments in <see cref="SegmentString" />s
    /// and adds them to each string.
    /// The <see cref="ISegmentIntersector" /> is passed to a <see cref="INoder" />.
    /// The <see cref="SegmentString.AddIntersections" /> method is called whenever the <see cref="INoder" />
    /// detects that two <see cref="SegmentString" />s might intersect.
    /// This class is an example of the Strategy pattern.
    /// </summary>
    public class IntersectionAdder : ISegmentIntersector
    {
        /**
         * These variables keep track of what types of intersections were
         * found during ALL edges that have been intersected.
         */
        private readonly LineIntersector _li;

        /// <summary>
        ///
        /// </summary>
        public int NumInteriorIntersections;

        private bool _hasInterior;

        private bool _hasIntersection;
        private bool _hasProper;
        private bool _hasProperInterior;
        private int _numIntersections;
        private int _numProperIntersections;
        private int _numTests;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntersectionAdder"/> class.
        /// </summary>
        /// <param name="li"></param>
        public IntersectionAdder(LineIntersector li)
        {
            _li = li;
        }

        /// <summary>
        ///
        /// </summary>
        public LineIntersector LineIntersector
        {
            get
            {
                return _li;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool HasIntersection
        {
            get
            {
                return _hasIntersection;
            }
        }

        /// <summary>
        /// A proper intersection is an intersection which is interior to at least two
        /// line segments.  Notice that a proper intersection is not necessarily
        /// in the interior of the entire <see cref="Geometry" />, since another edge may have
        /// an endpoint equal to the intersection, which according to SFS semantics
        /// can result in the point being on the Boundary of the <see cref="Geometry" />.
        /// </summary>
        public bool HasProperIntersection
        {
            get
            {
                return _hasProper;
            }
        }

        /// <summary>
        /// A proper interior intersection is a proper intersection which is not
        /// contained in the set of boundary nodes set for this <see cref="ISegmentIntersector" />.
        /// </summary>
        public bool HasProperInteriorIntersection
        {
            get
            {
                return _hasProperInterior;
            }
        }

        /// <summary>
        /// An interior intersection is an intersection which is
        /// in the interior of some segment.
        /// </summary>
        public bool HasInteriorIntersection
        {
            get
            {
                return _hasInterior;
            }
        }

        #region ISegmentIntersector Members

        /// <summary>
        /// This method is called by clients
        /// of the <see cref="ISegmentIntersector" /> class to process
        /// intersections for two segments of the <see cref="SegmentString" /> being intersected.
        /// Notice that some clients (such as <see cref="MonotoneChain" />s) may optimize away
        /// this call for segment pairs which they have determined do not intersect
        /// (e.g. by an disjoint envelope test).
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="segIndex0"></param>
        /// <param name="e1"></param>
        /// <param name="segIndex1"></param>
        public void ProcessIntersections(SegmentString e0, int segIndex0, SegmentString e1, int segIndex1)
        {
            if (e0 == e1 && segIndex0 == segIndex1)
                return;

            _numTests++;
            Coordinate p00 = e0.Coordinates[segIndex0];
            Coordinate p01 = e0.Coordinates[segIndex0 + 1];
            Coordinate p10 = e1.Coordinates[segIndex1];
            Coordinate p11 = e1.Coordinates[segIndex1 + 1];

            _li.ComputeIntersection(p00, p01, p10, p11);
            if (!_li.HasIntersection) return;
            _numIntersections++;
            if (_li.IsInteriorIntersection())
            {
                NumInteriorIntersections++;
                _hasInterior = true;
            }
            // if the segments are adjacent they have at least one trivial intersection,
            // the shared endpoint.  Don't bother adding it if it is the
            // only intersection.
            if (IsTrivialIntersection(e0, segIndex0, e1, segIndex1)) return;
            _hasIntersection = true;
            e0.AddIntersections(_li, segIndex0);
            e1.AddIntersections(_li, segIndex1);
            if (!_li.IsProper) return;
            _numProperIntersections++;
            _hasProper = true;
            _hasProperInterior = true;
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        private static bool IsAdjacentSegments(int i1, int i2)
        {
            return Math.Abs(i1 - i2) == 1;
        }

        /// <summary>
        /// A trivial intersection is an apparent self-intersection which in fact
        /// is simply the point shared by adjacent line segments.
        /// Notice that closed edges require a special check for the point shared by the beginning and end segments.
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="segIndex0"></param>
        /// <param name="e1"></param>
        /// <param name="segIndex1"></param>
        /// <returns></returns>
        private bool IsTrivialIntersection(SegmentString e0, int segIndex0, SegmentString e1, int segIndex1)
        {
            if (e0 == e1)
            {
                if (_li.IntersectionNum == 1)
                {
                    if (IsAdjacentSegments(segIndex0, segIndex1))
                        return true;
                    if (e0.IsClosed)
                    {
                        int maxSegIndex = e0.Count - 1;
                        if ((segIndex0 == 0 && segIndex1 == maxSegIndex) ||
                            (segIndex1 == 0 && segIndex0 == maxSegIndex))
                            return true;
                    }
                }
            }
            return false;
        }
    }
}