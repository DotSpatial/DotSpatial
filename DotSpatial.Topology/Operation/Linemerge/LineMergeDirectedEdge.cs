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

using DotSpatial.Topology.Planargraph;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Operation.Linemerge
{
    /// <summary>
    /// A <c>com.vividsolutions.jts.planargraph.DirectedEdge</c> of a <c>LineMergeGraph</c>.
    /// </summary>
    public class LineMergeDirectedEdge : DirectedEdge
    {
        /// <summary>
        /// Constructs a LineMergeDirectedEdge connecting the <c>from</c> node to the <c>to</c> node.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="directionPt">
        /// specifies this DirectedEdge's direction (given by an imaginary
        /// line from the <c>from</c> node to <c>directionPt</c>).
        /// </param>
        /// <param name="edgeDirection">
        /// whether this DirectedEdge's direction is the same as or
        /// opposite to that of the parent Edge (if any).
        /// </param>
        public LineMergeDirectedEdge(Node from, Node to, Coordinate directionPt, bool edgeDirection)
            : base(from, to, directionPt, edgeDirection) { }

        /// <summary>
        /// Returns the directed edge that starts at this directed edge's end point, or null
        /// if there are zero or multiple directed edges starting there.
        /// </summary>
        public virtual LineMergeDirectedEdge Next
        {
            get
            {
                if (ToNode.Degree != 2)
                    return null;
                if (ToNode.OutEdges.Edges[0] == Sym)
                    return (LineMergeDirectedEdge)ToNode.OutEdges.Edges[1];
                Assert.IsTrue(ToNode.OutEdges.Edges[1] == Sym);
                return (LineMergeDirectedEdge)ToNode.OutEdges.Edges[0];
            }
        }
    }
}