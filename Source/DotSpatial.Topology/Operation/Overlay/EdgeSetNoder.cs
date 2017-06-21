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
using DotSpatial.Topology.GeometriesGraph;
using DotSpatial.Topology.GeometriesGraph.Index;

namespace DotSpatial.Topology.Operation.Overlay
{
    /// <summary>
    /// Nodes a set of edges.
    /// Takes one or more sets of edges and constructs a
    /// new set of edges consisting of all the split edges created by
    /// noding the input edges together.
    /// </summary>
    public class EdgeSetNoder
    {
        private readonly IList _inputEdges = new ArrayList();
        private readonly LineIntersector _li;

        /// <summary>
        ///
        /// </summary>
        /// <param name="li"></param>
        public EdgeSetNoder(LineIntersector li)
        {
            _li = li;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IList NodedEdges
        {
            get
            {
                EdgeSetIntersector esi = new SimpleMcSweepLineIntersector();
                SegmentIntersector si = new SegmentIntersector(_li, true, false);
                esi.ComputeIntersections(_inputEdges, si, true);

                IList splitEdges = new ArrayList();
                IEnumerator i = _inputEdges.GetEnumerator();
                while (i.MoveNext())
                {
                    Edge e = (Edge)i.Current;
                    e.EdgeIntersectionList.AddSplitEdges(splitEdges);
                }
                return splitEdges;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edges"></param>
        public virtual void AddEdges(IList edges)
        {
            // inputEdges.addAll(edges);
            foreach (object obj in edges)
                _inputEdges.Add(obj);
        }
    }
}