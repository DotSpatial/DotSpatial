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
using System.Linq;

namespace DotSpatial.Topology.Planargraph
{
    /// <summary>
    /// A node in a <c>PlanarGraph</c> is a location where 0 or more <c>Edge</c>s
    /// meet. A node is connected to each of its incident Edges via an outgoing
    /// DirectedEdge. Some clients using a <c>PlanarGraph</c> may want to
    /// subclass <c>Node</c> to add their own application-specific
    /// data and methods.
    /// </summary>
    public class Node : GraphComponent
    {
        /// <summary>
        /// The collection of DirectedEdges that leave this Node.
        /// </summary>
        protected readonly DirectedEdgeStar DeStar;

        /// <summary>
        /// The location of this Node.
        /// </summary>
        private Coordinate _location;

        /// <summary>
        /// Constructs a Node with the given location.
        /// </summary>
        /// <param name="location"></param>
        public Node(Coordinate location) : this(location, new DirectedEdgeStar()) { }

        /// <summary>
        /// Constructs a Node with the given location and collection of outgoing DirectedEdges.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="deStar"></param>
        public Node(Coordinate location, DirectedEdgeStar deStar)
        {
            _location = location;
            DeStar = deStar;
        }

        /// <summary>
        /// Returns the location of this Node.
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                return _location;
            }
        }

        /// <summary>
        /// Returns the collection of DirectedEdges that leave this Node.
        /// </summary>
        public virtual DirectedEdgeStar OutEdges
        {
            get
            {
                return DeStar;
            }
        }

        /// <summary>
        /// Returns the number of edges around this Node.
        /// </summary>
        public virtual int Degree
        {
            get
            {
                return DeStar.Degree;
            }
        }

        /// <summary>
        /// Tests whether this component has been removed from its containing graph.
        /// </summary>
        /// <value></value>
        public override bool IsRemoved
        {
            get
            {
                return _location == null;
            }
        }

        /// <summary>
        /// Returns all Edges that connect the two nodes (which are assumed to be different).
        /// </summary>
        /// <param name="node0"></param>
        /// <param name="node1"></param>
        /// <returns></returns>
        public static IList GetEdgesBetween(Node node0, Node node1)
        {
            var edges0 = DirectedEdge.ToEdges(node0.OutEdges.Edges);
            var edges1 = DirectedEdge.ToEdges(node1.OutEdges.Edges);
            var toRemove = edges1.Cast<Edge>().Where(edges0.Contains).ToList();
            foreach (var edge in toRemove)
            {
                edges0.Remove(edge);
            }
            return edges0;
        }

        /// <summary>
        /// Adds an outgoing DirectedEdge to this Node.
        /// </summary>
        /// <param name="de"></param>
        public virtual void AddOutEdge(DirectedEdge de)
        {
            DeStar.Add(de);
        }

        /// <summary>
        /// Returns the zero-based index of the given Edge, after sorting in ascending order
        /// by angle with the positive x-axis.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public virtual int GetIndex(Edge edge)
        {
            return DeStar.GetIndex(edge);
        }

        /// <summary>
        /// Removes this node from its containing graph.
        /// </summary>
        internal void Remove()
        {
            _location = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "NODE: " + _location.ToString() + ": " + Degree;
        }
    }
}