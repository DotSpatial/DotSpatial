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

namespace DotSpatial.Topology.Operation.Overlay
{
    /// <summary>
    /// A ring of edges which may contain nodes of degree > 2.
    /// A MaximalEdgeRing may represent two different spatial entities:
    /// a single polygon possibly containing inversions (if the ring is oriented CW)
    /// a single hole possibly containing exversions (if the ring is oriented CCW)
    /// If the MaximalEdgeRing represents a polygon,
    /// the interior of the polygon is strongly connected.
    /// These are the form of rings used to define polygons under some spatial data models.
    /// However, under the OGC SFS model, MinimalEdgeRings are required.
    /// A MaximalEdgeRing can be converted to a list of MinimalEdgeRings using the
    /// <c>BuildMinimalRings()</c> method.
    /// </summary>
    public class MaximalEdgeRing : EdgeRing
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="start"></param>
        /// <param name="geometryFactory"></param>
        public MaximalEdgeRing(DirectedEdge start, IGeometryFactory geometryFactory)
            : base(start, geometryFactory) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        public override DirectedEdge GetNext(DirectedEdge de)
        {
            return de.Next;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="de"></param>
        /// <param name="er"></param>
        public override void SetEdgeRing(DirectedEdge de, EdgeRing er)
        {
            de.EdgeRing = er;
        }

        /// <summary>
        /// For all nodes in this EdgeRing,
        /// link the DirectedEdges at the node to form minimalEdgeRings
        /// </summary>
        public virtual void LinkDirectedEdgesForMinimalEdgeRings()
        {
            DirectedEdge de = StartDe;
            do
            {
                Node node = de.Node;
                ((DirectedEdgeStar)node.Edges).LinkMinimalDirectedEdges(this);
                de = de.Next;
            }
            while (de != StartDe);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual IList BuildMinimalRings()
        {
            IList minEdgeRings = new ArrayList();
            DirectedEdge de = StartDe;
            do
            {
                if (de.MinEdgeRing == null)
                {
                    EdgeRing minEr = new MinimalEdgeRing(de, InnerGeometryFactory);
                    minEdgeRings.Add(minEr);
                }
                de = de.Next;
            }
            while (de != StartDe);
            return minEdgeRings;
        }
    }
}