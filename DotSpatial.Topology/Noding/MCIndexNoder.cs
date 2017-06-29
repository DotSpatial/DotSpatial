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
using DotSpatial.Topology.Index;
using DotSpatial.Topology.Index.Chain;
using DotSpatial.Topology.Index.Quadtree;
using DotSpatial.Topology.Index.Strtree;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Nodes a set of <see cref="SegmentString" />s using a index based
    /// on <see cref="MonotoneChain" />s and a <see cref="ISpatialIndex" />.
    /// The <see cref="ISpatialIndex" /> used should be something that supports
    /// envelope (range) queries efficiently (such as a <see cref="Quadtree" />
    /// or <see cref="StRtree" />.
    /// </summary>
    public class McIndexNoder : SinglePassNoder
    {
        private readonly ISpatialIndex _index = new StRtree();
        private readonly IList _monoChains = new ArrayList();
        private int _idCounter;
        private int _nOverlaps; // statistics
        private IList _nodedSegStrings;

        /// <summary>
        /// Initializes a new instance of the <see cref="McIndexNoder"/> class.
        /// </summary>
        public McIndexNoder() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="McIndexNoder"/> class.
        /// </summary>
        /// <param name="segInt">The <see cref="ISegmentIntersector"/> to use.</param>
        public McIndexNoder(ISegmentIntersector segInt)
            : base(segInt) { }

        /// <summary>
        ///
        /// </summary>
        public IList MonotoneChains
        {
            get
            {
                return _monoChains;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public ISpatialIndex Index
        {
            get
            {
                return _index;
            }
        }

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
        /// Computes the noding for a collection of <see cref="SegmentString"/>s.
        /// Some Noders may add all these nodes to the input <see cref="SegmentString"/>s;
        /// others may only add some or none at all.
        /// </summary>
        /// <param name="inputSegStrings"></param>
        public override void ComputeNodes(IList inputSegStrings)
        {
            _nodedSegStrings = inputSegStrings;
            foreach (object obj in inputSegStrings)
                Add((SegmentString)obj);
            IntersectChains();
        }

        /// <summary>
        ///
        /// </summary>
        private void IntersectChains()
        {
            MonotoneChainOverlapAction overlapAction = new SegmentOverlapAction(SegmentIntersector);
            foreach (object obj in _monoChains)
            {
                MonotoneChain queryChain = (MonotoneChain)obj;
                IList overlapChains = _index.Query(queryChain.Envelope);
                foreach (object j in overlapChains)
                {
                    MonotoneChain testChain = (MonotoneChain)j;
                    /*
                     * following test makes sure we only compare each pair of chains once
                     * and that we don't compare a chain to itself
                     */
                    if (testChain.Id > queryChain.Id)
                    {
                        queryChain.ComputeOverlaps(testChain, overlapAction);
                        _nOverlaps++;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="segStr"></param>
        private void Add(SegmentString segStr)
        {
            IList segChains = MonotoneChainBuilder.GetChains(segStr.Coordinates, segStr);
            foreach (object obj in segChains)
            {
                MonotoneChain mc = (MonotoneChain)obj;
                mc.Id = _idCounter++;
                _index.Insert(mc.Envelope, mc);
                _monoChains.Add(mc);
            }
        }

        #region Nested type: SegmentOverlapAction

        /// <summary>
        ///
        /// </summary>
        public class SegmentOverlapAction : MonotoneChainOverlapAction
        {
            private readonly ISegmentIntersector _si;

            /// <summary>
            /// Initializes a new instance of the <see cref="SegmentOverlapAction"/> class.
            /// </summary>
            /// <param name="si">The <see cref="ISegmentIntersector" /></param>
            public SegmentOverlapAction(ISegmentIntersector si)
            {
                _si = si;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="mc1"></param>
            /// <param name="start1"></param>
            /// <param name="mc2"></param>
            /// <param name="start2"></param>
            public override void Overlap(MonotoneChain mc1, int start1, MonotoneChain mc2, int start2)
            {
                SegmentString ss1 = (SegmentString)mc1.Context;
                SegmentString ss2 = (SegmentString)mc2.Context;
                _si.ProcessIntersections(ss1, start1, ss2, start2);
            }
        }

        #endregion
    }
}