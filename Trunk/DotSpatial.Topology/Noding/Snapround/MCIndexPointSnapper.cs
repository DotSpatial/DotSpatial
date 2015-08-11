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

using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Index;
using DotSpatial.Topology.Index.Chain;
using DotSpatial.Topology.Index.Strtree;

namespace DotSpatial.Topology.Noding.Snapround
{
    /// <summary>
    /// "Snaps" all <see cref="ISegmentString" />s in a <see cref="ISpatialIndex" /> containing
    /// <see cref="MonotoneChain" />s to a given <see cref="HotPixel" />.
    /// </summary>
    public class McIndexPointSnapper
    {
        #region Fields

        private readonly StRtree<MonotoneChain> _index;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="McIndexPointSnapper"/> class.
        /// </summary>
        /// <param name="index"></param>
        public McIndexPointSnapper(ISpatialIndex<MonotoneChain> index)
        {
            _index = (StRtree<MonotoneChain>)index;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Snaps (nodes) all interacting segments to this hot pixel.
        /// The hot pixel may represent a vertex of an edge,
        /// in which case this routine uses the optimization
        /// of not noding the vertex itself
        /// </summary>
        /// <param name="hotPixel">The hot pixel to snap to.</param>
        /// <param name="parentEdge">The edge containing the vertex, if applicable, or <c>null</c>.</param>
        /// <param name="hotPixelVertexIndex"></param>
        /// <returns><c>true</c> if a node was added for this pixel.</returns>
        public bool Snap(HotPixel hotPixel, ISegmentString parentEdge, int hotPixelVertexIndex)
        {
            Envelope pixelEnv = hotPixel.GetSafeEnvelope();
            var hotPixelSnapAction = new HotPixelSnapAction(hotPixel, parentEdge, hotPixelVertexIndex);
            _index.Query(pixelEnv, new QueryVisitor(pixelEnv, hotPixelSnapAction));
            return hotPixelSnapAction.IsNodeAdded;
        }

        /// <summary>
        /// Snaps (nodes) all interacting segments to this hot pixel.
        /// The hot pixel may represent a vertex of an edge,
        /// in which case this routine uses the optimization
        /// of not noding the vertex itself
        /// </summary>
        /// <param name="hotPixel">The hot pixel to snap to.</param>
        /// <returns><c>true</c> if a node was added for this pixel.</returns>
        public bool Snap(HotPixel hotPixel)
        {
            return Snap(hotPixel, null, -1);
        }

        #endregion

        #region Classes

        /// <summary>
        ///
        /// </summary>
        public class HotPixelSnapAction : MonotoneChainSelectAction
        {
            #region Fields

            private readonly HotPixel _hotPixel;
            // is -1 if hotPixel is not a vertex
            private readonly int _hotPixelVertexIndex;
            private readonly ISegmentString _parentEdge;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="HotPixelSnapAction"/> class.
            /// </summary>
            /// <param name="hotPixel"></param>
            /// <param name="parentEdge"></param>
            /// <param name="hotPixelVertexIndex"></param>
            public HotPixelSnapAction(HotPixel hotPixel, ISegmentString parentEdge, int hotPixelVertexIndex)
            {
                _hotPixel = hotPixel;
                _parentEdge = parentEdge;
                _hotPixelVertexIndex = hotPixelVertexIndex;
            }

            #endregion

            #region Properties

            /// <summary>
            ///
            /// </summary>
            public bool IsNodeAdded { get; private set; }

            #endregion

            #region Methods

            /// <summary>
            ///
            /// </summary>
            /// <param name="mc"></param>
            /// <param name="startIndex"></param>
            public override void Select(MonotoneChain mc, int startIndex)
            {
                var ss = (INodableSegmentString)mc.Context;
                /**
                 * Check to avoid snapping a hotPixel vertex to the same vertex.
                 * This method is called for segments which intersects the 
                 * hot pixel,
                 * so need to check if either end of the segment is equal to the hot pixel
                 * and if so, do not snap.
                 * 
                 * Sep 22 2012 - MD - currently do need to snap to every vertex,
                 * since otherwise the testCollapse1 test in SnapRoundingTest fails.
                 */
                if (_parentEdge != null && ss == _parentEdge && startIndex == _hotPixelVertexIndex) return;
                IsNodeAdded = _hotPixel.AddSnappedNode(ss, startIndex);
            }

            #endregion
        }

        /// <summary>
        ///
        /// </summary>
        private class QueryVisitor : IItemVisitor<MonotoneChain>
        {
            #region Fields

            readonly HotPixelSnapAction _action;
            readonly Envelope _env;

            #endregion

            #region Constructors

            /// <summary>
            ///
            /// </summary>
            /// <param name="env"></param>
            /// <param name="action"></param>
            public QueryVisitor(Envelope env, HotPixelSnapAction action)
            {
                _env = env;
                _action = action;
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            /// <param name="item"></param>
            public void VisitItem(MonotoneChain item)
            {
                var testChain = item;
                testChain.Select(_env, _action);
            }

            #endregion
        }

        #endregion
    }
}