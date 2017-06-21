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

using DotSpatial.Topology.Index;
using DotSpatial.Topology.Index.Chain;
using DotSpatial.Topology.Index.Strtree;

namespace DotSpatial.Topology.Noding.Snapround
{
    /// <summary>
    /// "Snaps" all <see cref="SegmentString" />s in a <see cref="ISpatialIndex" /> containing
    /// <see cref="MonotoneChain" />s to a given <see cref="HotPixel" />.
    /// </summary>
    public class McIndexPointSnapper
    {
        private readonly StRtree _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="McIndexPointSnapper"/> class.
        /// </summary>
        /// <param name="index"></param>
        public McIndexPointSnapper(ISpatialIndex index)
        {
            _index = (StRtree)index;
        }

        /// <summary>
        /// Snaps (nodes) all interacting segments to this hot pixel.
        /// The hot pixel may represent a vertex of an edge,
        /// in which case this routine uses the optimization
        /// of not noding the vertex itself
        /// </summary>
        /// <param name="hotPixel">The hot pixel to snap to.</param>
        /// <param name="parentEdge">The edge containing the vertex, if applicable, or <c>null</c>.</param>
        /// <param name="vertexIndex"></param>
        /// <returns><c>true</c> if a node was added for this pixel.</returns>
        public bool Snap(HotPixel hotPixel, SegmentString parentEdge, int vertexIndex)
        {
            Envelope pixelEnv = hotPixel.GetSafeEnvelope();
            HotPixelSnapAction hotPixelSnapAction = new HotPixelSnapAction(hotPixel, parentEdge, vertexIndex);
            _index.Query(pixelEnv, new QueryVisitor(pixelEnv, hotPixelSnapAction));
            return hotPixelSnapAction.IsNodeAdded;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hotPixel"></param>
        /// <returns></returns>
        public bool Snap(HotPixel hotPixel)
        {
            return Snap(hotPixel, null, -1);
        }

        #region Nested type: HotPixelSnapAction

        /// <summary>
        ///
        /// </summary>
        public class HotPixelSnapAction : MonotoneChainSelectAction
        {
            private readonly HotPixel _hotPixel;
            private readonly SegmentString _parentEdge;
            private readonly int _vertexIndex;
            private bool _isNodeAdded;

            /// <summary>
            /// Initializes a new instance of the <see cref="HotPixelSnapAction"/> class.
            /// </summary>
            /// <param name="hotPixel"></param>
            /// <param name="parentEdge"></param>
            /// <param name="vertexIndex"></param>
            public HotPixelSnapAction(HotPixel hotPixel, SegmentString parentEdge, int vertexIndex)
            {
                _hotPixel = hotPixel;
                _parentEdge = parentEdge;
                _vertexIndex = vertexIndex;
            }

            /// <summary>
            ///
            /// </summary>
            public bool IsNodeAdded
            {
                get
                {
                    return _isNodeAdded;
                }
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="mc"></param>
            /// <param name="startIndex"></param>
            public override void Select(MonotoneChain mc, int startIndex)
            {
                SegmentString ss = (SegmentString)mc.Context;
                // don't snap a vertex to itself
                if (_parentEdge != null)
                    if (ss == _parentEdge && startIndex == _vertexIndex)
                        return;
                _isNodeAdded = SimpleSnapRounder.AddSnappedNode(_hotPixel, ss, startIndex);
            }
        }

        #endregion

        #region Nested type: QueryVisitor

        /// <summary>
        ///
        /// </summary>
        private class QueryVisitor : IItemVisitor
        {
            readonly HotPixelSnapAction _action;
            readonly Envelope _env;

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

            #region IItemVisitor Members

            /// <summary>
            /// </summary>
            /// <param name="item"></param>
            public void VisitItem(object item)
            {
                MonotoneChain testChain = (MonotoneChain)item;
                testChain.Select(_env, _action);
            }

            #endregion
        }

        #endregion
    }
}