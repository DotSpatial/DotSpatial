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
using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Operation.Relate
{
    /// <summary>
    /// An EdgeEndBuilder creates EdgeEnds for all the "split edges"
    /// created by the intersections determined for an Edge.
    /// Computes the <c>EdgeEnd</c>s which arise from a noded <c>Edge</c>.
    /// </summary>
    public class EdgeEndBuilder
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public virtual IList ComputeEdgeEnds(IEnumerator edges)
        {
            IList l = new ArrayList();
            for (IEnumerator i = edges; i.MoveNext(); )
            {
                Edge e = (Edge)i.Current;
                ComputeEdgeEnds(e, l);
            }
            return l;
        }

        /// <summary>
        /// Creates stub edges for all the intersections in this
        /// Edge (if any) and inserts them into the graph.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="l"></param>
        public virtual void ComputeEdgeEnds(Edge edge, IList l)
        {
            EdgeIntersectionList eiList = edge.EdgeIntersectionList;
            // ensure that the list has entries for the first and last point of the edge
            eiList.AddEndpoints();

            IEnumerator it = eiList.GetEnumerator();
            EdgeIntersection eiCurr = null;
            // no intersections, so there is nothing to do
            if (!it.MoveNext()) return;
            EdgeIntersection eiNext = (EdgeIntersection)it.Current;
            do
            {
                EdgeIntersection eiPrev = eiCurr;
                eiCurr = eiNext;
                eiNext = null;

                if (it.MoveNext())
                    eiNext = (EdgeIntersection)it.Current;

                if (eiCurr != null)
                {
                    CreateEdgeEndForPrev(edge, l, eiCurr, eiPrev);
                    CreateEdgeEndForNext(edge, l, eiCurr, eiNext);
                }
            }
            while (eiCurr != null);
        }

        /// <summary>
        /// Create a EdgeStub for the edge before the intersection eiCurr.
        /// The previous intersection is provided
        /// in case it is the endpoint for the stub edge.
        /// Otherwise, the previous point from the parent edge will be the endpoint.
        /// eiCurr will always be an EdgeIntersection, but eiPrev may be null.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="l"></param>
        /// <param name="eiCurr"></param>
        /// <param name="eiPrev"></param>
        public virtual void CreateEdgeEndForPrev(Edge edge, IList l, EdgeIntersection eiCurr, EdgeIntersection eiPrev)
        {
            int iPrev = eiCurr.SegmentIndex;
            if (eiCurr.Distance == 0.0)
            {
                // if at the start of the edge there is no previous edge
                if (iPrev == 0)
                    return;
                iPrev--;
            }
            Coordinate pPrev = edge.GetCoordinate(iPrev);
            // if prev intersection is past the previous vertex, use it instead
            if (eiPrev != null && eiPrev.SegmentIndex >= iPrev)
                pPrev = eiPrev.Coordinate;

            Label label = new Label(edge.Label);
            // since edgeStub is oriented opposite to it's parent edge, have to flip sides for edge label
            label.Flip();
            EdgeEnd e = new EdgeEnd(edge, eiCurr.Coordinate, pPrev, label);
            l.Add(e);
        }

        /// <summary>
        /// Create a StubEdge for the edge after the intersection eiCurr.
        /// The next intersection is provided
        /// in case it is the endpoint for the stub edge.
        /// Otherwise, the next point from the parent edge will be the endpoint.
        /// eiCurr will always be an EdgeIntersection, but eiNext may be null.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="l"></param>
        /// <param name="eiCurr"></param>
        /// <param name="eiNext"></param>
        public virtual void CreateEdgeEndForNext(Edge edge, IList l, EdgeIntersection eiCurr, EdgeIntersection eiNext)
        {
            int iNext = eiCurr.SegmentIndex + 1;
            // if there is no next edge there is nothing to do
            if (iNext >= edge.NumPoints && eiNext == null)
                return;

            Coordinate pNext = edge.GetCoordinate(iNext);
            // if the next intersection is in the same segment as the current, use it as the endpoint
            if (eiNext != null && eiNext.SegmentIndex == eiCurr.SegmentIndex)
                pNext = eiNext.Coordinate;

            EdgeEnd e = new EdgeEnd(edge, eiCurr.Coordinate, pNext, new Label(edge.Label));
            l.Add(e);
        }
    }
}