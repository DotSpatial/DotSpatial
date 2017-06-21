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

namespace DotSpatial.Topology.Operation.Polygonize
{
    /// <summary>
    /// A <c>DirectedEdge</c> of a <c>PolygonizeGraph</c>, which represents
    /// an edge of a polygon formed by the graph.
    /// May be logically deleted from the graph by setting the <c>marked</c> flag.
    /// </summary>
    public class PolygonizeDirectedEdge : DirectedEdge
    {
        private EdgeRing _edgeRing;
        private long _label = -1;
        private PolygonizeDirectedEdge _next;

        /// <summary>
        /// Constructs a directed edge connecting the <c>from</c> node to the
        /// <c>to</c> node.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="directionPt">
        /// Specifies this DirectedEdge's direction (given by an imaginary
        /// line from the <c>from</c> node to <c>directionPt</c>).
        /// </param>
        /// <param name="edgeDirection">
        /// Whether this DirectedEdge's direction is the same as or
        /// opposite to that of the parent Edge (if any).
        /// </param>
        public PolygonizeDirectedEdge(Node from, Node to, Coordinate directionPt, bool edgeDirection)
            : base(from, to, directionPt, edgeDirection) { }

        /// <summary>
        /// Returns the identifier attached to this directed edge.
        /// Attaches an identifier to this directed edge.
        /// </summary>
        public virtual long Label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        /// <summary>
        /// Returns the next directed edge in the EdgeRing that this directed edge is a member of.
        /// Sets the next directed edge in the EdgeRing that this directed edge is a member of.
        /// </summary>
        public virtual PolygonizeDirectedEdge Next
        {
            get
            {
                return _next;
            }
            set
            {
                _next = value;
            }
        }

        /// <summary>
        /// Returns the ring of directed edges that this directed edge is
        /// a member of, or null if the ring has not been set.
        /// </summary>
        public virtual bool IsInRing
        {
            get
            {
                return Ring != null;
            }
        }

        /// <summary>
        /// Gets/Sets the ring of directed edges that this directed edge is
        /// a member of.
        /// </summary>
        public virtual EdgeRing Ring
        {
            get
            {
                return _edgeRing;
            }
            set
            {
                _edgeRing = value;
            }
        }
    }
}