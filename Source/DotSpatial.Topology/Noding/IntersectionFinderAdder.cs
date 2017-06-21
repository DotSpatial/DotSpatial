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

using System.Collections;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Index.Chain;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Finds proper and interior intersections in a set of <see cref="SegmentString" />s,
    /// and adds them as nodes.
    /// </summary>
    public class IntersectionFinderAdder : ISegmentIntersector
    {
        private readonly IList _interiorIntersections;
        private readonly LineIntersector _li;

        /// <summary>
        /// Creates an intersection finder which finds all proper intersections.
        /// </summary>
        /// <param name="li">The <see cref="LineIntersector" /> to use.</param>
        public IntersectionFinderAdder(LineIntersector li)
        {
            _li = li;
            _interiorIntersections = new ArrayList();
        }

        /// <summary>
        ///
        /// </summary>
        public IList InteriorIntersections
        {
            get
            {
                return _interiorIntersections;
            }
        }

        #region ISegmentIntersector Members

        /// <summary>
        /// This method is called by clients
        /// of the <see cref="ISegmentIntersector" /> class to process
        /// intersections for two segments of the <see cref="SegmentString" />s being intersected.
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
            // don't bother intersecting a segment with itself
            if (e0 == e1 && segIndex0 == segIndex1)
                return;

            Coordinate p00 = e0.Coordinates[segIndex0];
            Coordinate p01 = e0.Coordinates[segIndex0 + 1];
            Coordinate p10 = e1.Coordinates[segIndex1];
            Coordinate p11 = e1.Coordinates[segIndex1 + 1];

            _li.ComputeIntersection(p00, p01, p10, p11);

            if (_li.HasIntersection)
            {
                if (_li.IsInteriorIntersection())
                {
                    for (int intIndex = 0; intIndex < _li.IntersectionNum; intIndex++)
                        _interiorIntersections.Add(_li.GetIntersection(intIndex));

                    e0.AddIntersections(_li, segIndex0);
                    e1.AddIntersections(_li, segIndex1);
                }
            }
        }

        #endregion
    }
}