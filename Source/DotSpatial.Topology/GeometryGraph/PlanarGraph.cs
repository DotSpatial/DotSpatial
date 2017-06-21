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
using System.IO;
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// The computation of the <c>IntersectionMatrix</c> relies on the use of a structure
    /// called a "topology graph". The topology graph contains nodes and edges
    /// corresponding to the nodes and line segments of a <c>Geometry</c>. Each
    /// node and edge in the graph is labeled with its topological location relative to
    /// the source point.
    /// Notice that there is no requirement that points of self-intersection be a vertex.
    /// Thus to obtain a correct topology graph, <c>Geometry</c>s must be
    /// self-noded before constructing their graphs.
    /// Two fundamental operations are supported by topology graphs:
    /// Computing the intersections between all the edges and nodes of a single graph
    /// Computing the intersections between the edges and nodes of two different graphs
    /// </summary>
    public class PlanarGraph
    {
        #region Private Variables

        private readonly IList _edgeEndList = new ArrayList();
        private IList _edges = new ArrayList();
        private NodeMap _nodes;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a Planar Graph
        /// </summary>
        /// <param name="nodeFact">A node Factory</param>
        public PlanarGraph(NodeFactory nodeFact)
        {
            _nodes = new NodeMap(nodeFact);
        }

        /// <summary>
        /// Creates a new instance of a Planar Graph using a default NodeFactory
        /// </summary>
        public PlanarGraph()
        {
            _nodes = new NodeMap(new NodeFactory());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new EdgeEnd to the planar graph
        /// </summary>
        /// <param name="e">The EdgeEnd to add</param>
        public virtual void Add(EdgeEnd e)
        {
            _nodes.Add(e);
            _edgeEndList.Add(e);
        }

        /// <summary>
        /// Add a set of edges to the graph.  For each edge two DirectedEdges
        /// will be created.  DirectedEdges are NOT linked by this method.
        /// </summary>
        /// <param name="edgesToAdd"></param>
        public virtual void AddEdges(IList edgesToAdd)
        {
            // create all the nodes for the edges
            for (IEnumerator it = edgesToAdd.GetEnumerator(); it.MoveNext(); )
            {
                Edge e = (Edge)it.Current;
                _edges.Add(e);

                DirectedEdge de1 = new DirectedEdge(e, true);
                DirectedEdge de2 = new DirectedEdge(e, false);
                de1.Sym = de2;
                de2.Sym = de1;

                Add(de1);
                Add(de2);
            }
        }

        /// <summary>
        /// Adds the specified node to the geometry graph's NodeMap
        /// </summary>
        /// <param name="node">The node to add</param>
        /// <returns>The node after the addition</returns>
        public virtual Node AddNode(Node node)
        {
            return _nodes.AddNode(node);
        }

        /// <summary>
        /// Adds a new ICoordinate as though it were a Node to the node map
        /// </summary>
        /// <param name="coord">An ICoordinate to add</param>
        /// <returns>The newly added node</returns>
        public virtual Node AddNode(Coordinate coord)
        {
            return _nodes.AddNode(coord);
        }

        /// <returns>
        /// The node if found; null otherwise
        /// </returns>
        /// <param name="coord"></param>
        public virtual Node Find(Coordinate coord)
        {
            return _nodes.Find(coord);
        }

        /// <summary>
        /// Returns the EdgeEnd which has edge e as its base edge
        /// (MD 18 Feb 2002 - this should return a pair of edges).
        /// </summary>
        /// <param name="e"></param>
        /// <returns> The edge, if found <c>null</c> if the edge was not found.</returns>
        public virtual EdgeEnd FindEdgeEnd(Edge e)
        {
            for (IEnumerator i = EdgeEnds.GetEnumerator(); i.MoveNext(); )
            {
                EdgeEnd ee = (EdgeEnd)i.Current;
                if (ee.Edge == e)
                    return ee;
            }
            return null;
        }

        /// <summary>
        /// Returns the edge whose first two coordinates are p0 and p1.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns> The edge, if found <c>null</c> if the edge was not found.</returns>
        public virtual Edge FindEdge(Coordinate p0, Coordinate p1)
        {
            for (int i = 0; i < _edges.Count; i++)
            {
                Edge e = (Edge)_edges[i];
                IList<Coordinate> eCoord = e.Coordinates;
                if (p0.Equals(eCoord[0]) && p1.Equals(eCoord[1]))
                    return e;
            }
            return null;
        }

        /// <summary>
        /// Returns the edge which starts at p0 and whose first segment is
        /// parallel to p1.
        /// </summary>
        /// <param name="p0"></param>
        ///<param name="p1"></param>
        /// <returns> The edge, if found <c>null</c> if the edge was not found.</returns>
        public virtual Edge FindEdgeInSameDirection(Coordinate p0, Coordinate p1)
        {
            for (int i = 0; i < _edges.Count; i++)
            {
                Edge e = (Edge)_edges[i];
                IList<Coordinate> eCoord = e.Coordinates;
                if (MatchInSameDirection(p0, p1, eCoord[0], eCoord[1]))
                    return e;
                if (MatchInSameDirection(p0, p1, eCoord[eCoord.Count - 1], eCoord[eCoord.Count - 2]))
                    return e;
            }
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetNodeEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetEdgeEnumerator()
        {
            return _edges.GetEnumerator();
        }

        /// <summary>
        /// Adds a new EdgeEnd to the planar graph
        /// </summary>
        /// <param name="e"></param>
        protected virtual void InsertEdge(Edge e)
        {
            _edges.Add(e);
        }

        /// <summary>
        /// Link the DirectedEdges at the nodes of the graph.
        /// This allows clients to link only a subset of nodes in the graph, for
        /// efficiency (because they know that only a subset is of interest).
        /// </summary>
        public virtual void LinkResultDirectedEdges()
        {
            for (IEnumerator nodeit = _nodes.GetEnumerator(); nodeit.MoveNext(); )
            {
                Node node = (Node)nodeit.Current;
                ((DirectedEdgeStar)node.Edges).LinkResultDirectedEdges();
            }
        }

        /// <summary>
        /// Link the DirectedEdges at the nodes of the graph.
        /// This allows clients to link only a subset of nodes in the graph, for
        /// efficiency (because they know that only a subset is of interest).
        /// </summary>
        public virtual void LinkAllDirectedEdges()
        {
            for (IEnumerator nodeit = _nodes.GetEnumerator(); nodeit.MoveNext(); )
            {
                Node node = (Node)nodeit.Current;
                ((DirectedEdgeStar)node.Edges).LinkAllDirectedEdges();
            }
        }

        /// <summary>
        /// The coordinate pairs match if they define line segments lying in the same direction.
        /// E.g. the segments are parallel and in the same quadrant
        /// (as opposed to parallel and opposite!).
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="ep0"></param>
        /// <param name="ep1"></param>
        private static bool MatchInSameDirection(Coordinate p0, Coordinate p1, Coordinate ep0, Coordinate ep1)
        {
            if (!p0.Equals(ep0))
                return false;
            return CgAlgorithms.ComputeOrientation(p0, p1, ep1) == CgAlgorithms.COLLINEAR &&
                   QuadrantOp.Quadrant(p0, p1) == QuadrantOp.Quadrant(ep0, ep1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void WriteEdges(StreamWriter outstream)
        {
            outstream.WriteLine("Edges:");
            for (int i = 0; i < _edges.Count; i++)
            {
                outstream.WriteLine("edge " + i + ":");
                Edge e = (Edge)_edges[i];
                e.Write(outstream);
                e.EdgeIntersectionList.Write(outstream);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a list of edge ends
        /// </summary>
        public virtual IList EdgeEnds
        {
            get
            {
                return _edgeEndList;
            }
        }

        /// <summary>
        /// Gets or sets the list of edges.
        /// </summary>
        public IList Edges
        {
            get { return _edges; }
            set { _edges = value; }
        }

        /// <summary>
        /// Gets or sets the NodeMap for this graph
        /// </summary>
        public virtual NodeMap Nodes
        {
            get { return _nodes; }
            set { _nodes = value; }
        }

        /// <summary>
        /// Gets a list of the actual values contained in the nodes
        /// </summary>
        public virtual IList NodeValues
        {
            get { return new ArrayList(_nodes.Values); }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomIndex"></param>
        /// <param name="coord"></param>
        /// <returns></returns>
        public virtual bool IsBoundaryNode(int geomIndex, Coordinate coord)
        {
            Node node = _nodes.Find(coord);
            if (node == null)
                return false;
            Label label = node.Label;
            if (label != null && label.GetLocation(geomIndex) == LocationType.Boundary)
                return true;
            return false;
        }

        #endregion

        #region Static

        /// <summary>
        /// For nodes in the Collection, link the DirectedEdges at the node that are in the result.
        /// This allows clients to link only a subset of nodes in the graph, for
        /// efficiency (because they know that only a subset is of interest).
        /// </summary>
        /// <param name="nodes"></param>
        public static void LinkResultDirectedEdges(IList nodes)
        {
            for (IEnumerator nodeit = nodes.GetEnumerator(); nodeit.MoveNext(); )
            {
                Node node = (Node)nodeit.Current;
                ((DirectedEdgeStar)node.Edges).LinkResultDirectedEdges();
            }
        }

        #endregion
    }
}