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
using System.Collections.Generic;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Nodes a set of <see cref="SegmentString" />s by
    /// performing a brute-force comparison of every segment to every other one.
    /// This has n^2 performance, so is too slow for use on large numbers of segments.
    /// </summary>
    public class SimpleNoder : SinglePassNoder
    {
        private IList _nodedSegStrings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleNoder"/> class.
        /// </summary>
        public SimpleNoder() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleNoder"/> class.
        /// </summary>
        /// <param name="segInt"></param>
        public SimpleNoder(ISegmentIntersector segInt)
            : base(segInt) { }

        /// <summary>
        /// Returns a <see cref="IList"/> of fully noded <see cref="SegmentString"/>s.
        /// The <see cref="SegmentString"/>s have the same context as their parent.
        /// </summary>
        /// <returns></returns>
        public override IList GetNodedSubstrings()
        {
            return SegmentString.GetNodedSubstrings(_nodedSegStrings);
        }

        /// <summary>
        /// Computes the noding for a collection of <see cref="SegmentString" />s.
        /// Some Noders may add all these nodes to the input <see cref="SegmentString" />s;
        /// others may only add some or none at all.
        /// </summary>
        /// <param name="inputSegStrings"></param>
        public override void ComputeNodes(IList inputSegStrings)
        {
            _nodedSegStrings = inputSegStrings;
            foreach (object obj0 in inputSegStrings)
            {
                SegmentString edge0 = (SegmentString)obj0;
                foreach (object obj1 in inputSegStrings)
                {
                    SegmentString edge1 = (SegmentString)obj1;
                    ComputeIntersects(edge0, edge1);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="e1"></param>
        private void ComputeIntersects(SegmentString e0, SegmentString e1)
        {
            IList<Coordinate> pts0 = e0.Coordinates;
            IList<Coordinate> pts1 = e1.Coordinates;
            for (int i0 = 0; i0 < pts0.Count - 1; i0++)
                for (int i1 = 0; i1 < pts1.Count - 1; i1++)
                    SegmentIntersector.ProcessIntersections(e0, i0, e1, i1);
        }
    }
}