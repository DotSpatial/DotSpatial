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

using System.Collections.Generic;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Nodes a set of <see cref="ISegmentString" />s by
    /// performing a brute-force comparison of every segment to every other one.
    /// This has n^2 performance, so is too slow for use on large numbers of segments.
    /// </summary>
    public class SimpleNoder : SinglePassNoder
    {
        #region Fields

        private IList<ISegmentString> _nodedSegStrings;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleNoder"/> class.
        /// </summary>
        public SimpleNoder() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleNoder"/> class.
        /// </summary>
        /// <param name="segInt"></param>
        public SimpleNoder(ISegmentIntersector segInt) : base(segInt) { }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="e1"></param>
        private void ComputeIntersects(ISegmentString e0, ISegmentString e1)
        {
            IList<Coordinate> pts0 = e0.Coordinates;
            IList<Coordinate> pts1 = e1.Coordinates;
            for (int i0 = 0; i0 < pts0.Count - 1; i0++)
                for (int i1 = 0; i1 < pts1.Count - 1; i1++)
                    SegmentIntersector.ProcessIntersections(e0, i0, e1, i1);
        }

        /// <summary>
        /// Computes the noding for a collection of <see cref="ISegmentString" />s.
        /// Some Noders may add all these nodes to the input <see cref="ISegmentString" />s;
        /// others may only add some or none at all.
        /// </summary>
        /// <param name="inputSegStrings"></param>
        public override void ComputeNodes(IList<ISegmentString> inputSegStrings)
        {
            _nodedSegStrings = inputSegStrings;
            foreach (var edge0 in inputSegStrings)
                foreach (var edge1 in inputSegStrings)
                    ComputeIntersects(edge0, edge1);
        }

        /// <summary>
        /// Returns a <see cref="IList{ISegmentString}"/> of fully noded <see cref="NodedSegmentString"/>s.
        /// The <see cref="NodedSegmentString"/>s have the same context as their parent.
        /// </summary>
        /// <returns></returns>
        public override IList<ISegmentString> GetNodedSubstrings()
        {
            return NodedSegmentString.GetNodedSubstrings(_nodedSegStrings);
        }

        #endregion
    }
}