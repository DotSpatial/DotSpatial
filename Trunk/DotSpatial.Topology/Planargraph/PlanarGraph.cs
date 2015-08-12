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

namespace DotSpatial.Topology.Planargraph
{
    /// <summary>
    /// Represents a directed graph which is embeddable in a planar surface.
    /// This class and the other classes in this package serve as a framework for
    /// building planar graphs for specific algorithms. This class must be
    /// subclassed to expose appropriate methods to construct the graph. This allows
    /// controlling the types of graph components ({DirectedEdge}s,
    /// <c>Edge</c>s and <c>Node</c>s) which can be added to the graph. An
    /// application which uses the graph framework will almost always provide
    /// subclasses for one or more graph components, which hold application-specific
    /// data and graph algorithms.
    /// </summary>
    public abstract class PlanarGraph
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        protected IList<DirectedEdge> DirEdges = new List<DirectedEdge>();

        /// <summary>
        /// 
        /// </summary>
        protected NodeMap NodeMap = new NodeMap();

        /// <summary>
        /// 
        /// </summary>
        private IList<Edge> _edges = new List<Edge>();

        #endregion

        #region Properties

        /// <summary>
        /// Returns the Nodes in this PlanarGraph.
        /// </summary>
        public ICollection<Node> Nodes
        {
            get { return NodeMap.Values; }
        }

        /// <summary>
        /// Returns the Edges that have been added to this PlanarGraph.
        /// </summary>
        public IList<Edge> Edges
        {
            get { return _edges; }
            protected set { _edges = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a node to the map, replacing any that is already at that location.
        /// Only subclasses can add Nodes, to ensure Nodes are of the right type.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>The added node.</returns>
        protected virtual void Add(Node node)
        {
            NodeMap.Add(node);
        }

        /// <summary>
        /// Adds the Edge and its DirectedEdges with this PlanarGraph.
        /// Assumes that the Edge has already been created with its associated DirectEdges.
        /// Only subclasses can add Edges, to ensure the edges added are of the right class.
        /// </summary>
        /// <param name="edge"></param>
        protected virtual void Add(Edge edge)
        {
            _edges.Add(edge);
            Add(edge.GetDirEdge(0));
            Add(edge.GetDirEdge(1));
        }

        /// <summary>
        /// Adds the Edge to this PlanarGraph; only subclasses can add DirectedEdges,
        /// to ensure the edges added are of the right class.
        /// </summary>
        /// <param name="dirEdge"></param>
        protected virtual void Add(DirectedEdge dirEdge)
        {
            DirEdges.Add(dirEdge);
        }

        /// <summary>
        /// Returns the <see cref="Node"/> at the given <paramref name="pt">location</paramref>, or <value>null</value> if no <see cref="Node"/> was there.
        /// </summary>
        /// <param name="pt">The location</param>
        /// <returns>The node found<br/>
        /// or <c>null</c> if this graph contains no node at the location
        /// </returns>
        public virtual Node FindNode(Coordinate pt)
        {
            return NodeMap.Find(pt);
        }

        /// <summary>
        /// Returns all Nodes with the given number of Edges around it.
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public virtual IList<Node> FindNodesOfDegree(int degree)
        {
            IList<Node> nodesFound = new List<Node>();
            foreach (var node in NodeMap.Values )
            {
                if (node.Degree == degree)
                    nodesFound.Add(node);
            }
            return nodesFound;
        }

        /// <summary> 
        /// Returns an Iterator over the DirectedEdges in this PlanarGraph, in the order in which they
        /// were added.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<DirectedEdge> GetDirEdgeEnumerator() 
        {            
            return DirEdges.GetEnumerator();          
        }

        /// <summary>
        /// Returns an Iterator over the Edges in this PlanarGraph, in the order in which they
        /// were added.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Edge> GetEdgeEnumerator()
        {
            return _edges.GetEnumerator(); 
        }

        /// <summary>
        /// Returns an IEnumerator over the Nodes in this PlanarGraph.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Node> GetNodeEnumerator()
        {            
            return NodeMap.GetEnumerator();          
        }

        /// <summary>
        /// Removes an Edge and its associated DirectedEdges from their from-Nodes and
        /// from this PlanarGraph. Note: This method does not remove the Nodes associated
        /// with the Edge, even if the removal of the Edge reduces the degree of a
        /// Node to zero.
        /// </summary>
        /// <param name="edge"></param>
        public virtual void Remove(Edge edge)
        {
            Remove(edge.GetDirEdge(0));
            Remove(edge.GetDirEdge(1));
            _edges.Remove(edge);
            edge.Remove();
        }

        /// <summary> 
        /// Removes a <see cref="DirectedEdge"/> from its from-<see cref="Node"/> and from this PlanarGraph.
        /// </summary>
        /// <remarks>
        /// This method does not remove the <see cref="Node"/>s associated with the DirectedEdge,
        /// even if the removal of the DirectedEdge reduces the degree of a Node to zero.
        /// </remarks>
        /// <param name="de"></param>
        public virtual void Remove(DirectedEdge de)
        {
            DirectedEdge sym = de.Sym;
            if (sym != null)
                sym.Sym = null;
            de.FromNode.Remove(de);
            de.Remove();
            DirEdges.Remove(de);
        }

        /// <summary>
        /// Removes a node from the graph, along with any associated DirectedEdges and
        /// Edges.
        /// </summary>
        /// <param name="node"></param>
        public virtual void Remove(Node node)
        {
            // unhook all directed edges
            IList<DirectedEdge> outEdges = node.OutEdges.Edges;
            foreach (DirectedEdge de in outEdges)
            {
                DirectedEdge sym = de.Sym;
                // remove the diredge that points to this node
                if (sym != null) 
                    Remove(sym);
                // remove this diredge from the graph collection
                DirEdges.Remove(de);

                Edge edge = de.Edge;
                if (edge != null)                
                    _edges.Remove(edge);                
            }
            // remove the node from the graph
            NodeMap.Remove(node.Coordinate);
            node.Remove();
        }

        #endregion
    }
}