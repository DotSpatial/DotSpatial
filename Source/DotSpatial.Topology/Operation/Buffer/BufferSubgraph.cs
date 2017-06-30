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

using System;
using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology.GeometriesGraph;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Operation.Buffer
{
    /// <summary>
    /// A connected subset of the graph of
    /// <c>DirectedEdges</c> and <c>Node</c>s.
    /// Its edges will generate either
    /// a single polygon in the complete buffer, with zero or more holes, or
    /// one or more connected holes.
    /// </summary>
    public class BufferSubgraph : IComparable
    {
        private readonly IList _dirEdgeList = new ArrayList();
        private readonly RightmostEdgeFinder _finder;
        private readonly IList _nodes = new ArrayList();
        private Coordinate _rightMostCoord;

        /// <summary>
        ///
        /// </summary>
        public BufferSubgraph()
        {
            _finder = new RightmostEdgeFinder();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IList DirectedEdges
        {
            get
            {
                return _dirEdgeList;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IList Nodes
        {
            get
            {
                return _nodes;
            }
        }

        /// <summary>
        /// Gets the rightmost coordinate in the edges of the subgraph.
        /// </summary>
        public virtual Coordinate RightMostCoordinate
        {
            get
            {
                return _rightMostCoord;
            }
        }

        #region IComparable Members

        /// <summary>
        /// BufferSubgraphs are compared on the x-value of their rightmost Coordinate.
        /// This defines a partial ordering on the graphs such that:
        /// g1 >= g2 - Ring(g2) does not contain Ring(g1)
        /// where Polygon(g) is the buffer polygon that is built from g.
        /// This relationship is used to sort the BufferSubgraphs so that shells are guaranteed to
        /// be built before holes.
        /// </summary>
        public virtual int CompareTo(Object o)
        {
            BufferSubgraph graph = (BufferSubgraph)o;
            if (RightMostCoordinate.X < graph.RightMostCoordinate.X)
                return -1;
            if (RightMostCoordinate.X > graph.RightMostCoordinate.X)
                return 1;
            return 0;
        }

        #endregion

        /// <summary>
        /// Creates the subgraph consisting of all edges reachable from this node.
        /// Finds the edges in the graph and the rightmost coordinate.
        /// </summary>
        /// <param name="node">A node to start the graph traversal from.</param>
        public virtual void Create(Node node)
        {
            AddReachable(node);
            _finder.FindEdge(_dirEdgeList);
            _rightMostCoord = _finder.Coordinate;
        }

        /// <summary>
        /// Adds all nodes and edges reachable from this node to the subgraph.
        /// Uses an explicit stack to avoid a large depth of recursion.
        /// </summary>
        /// <param name="startNode">A node known to be in the subgraph.</param>
        private void AddReachable(Node startNode)
        {
            Stack nodeStack = new Stack();
            nodeStack.Push(startNode);
            while (nodeStack.Count != 0)
            {
                Node node = (Node)nodeStack.Pop();
                Add(node, nodeStack);
            }
        }

        /// <summary>
        /// Adds the argument node and all its out edges to the subgraph
        /// </summary>
        /// <param name="node">The node to add.</param>
        /// <param name="nodeStack">The current set of nodes being traversed.</param>
        private void Add(Node node, Stack nodeStack)
        {
            node.IsVisited = true;
            _nodes.Add(node);
            for (IEnumerator i = node.Edges.GetEnumerator(); i.MoveNext(); )
            {
                DirectedEdge de = (DirectedEdge)i.Current;
                _dirEdgeList.Add(de);
                DirectedEdge sym = de.Sym;
                Node symNode = sym.Node;
                /*
                * Notice: this is a depth-first traversal of the graph.
                * This will cause a large depth of recursion.
                * It might be better to do a breadth-first traversal.
                */
                if (!symNode.IsVisited)
                    nodeStack.Push(symNode);
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ClearVisitedEdges()
        {
            for (IEnumerator it = _dirEdgeList.GetEnumerator(); it.MoveNext(); )
            {
                DirectedEdge de = (DirectedEdge)it.Current;
                de.IsVisited = false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outsideDepth"></param>
        public virtual void ComputeDepth(int outsideDepth)
        {
            ClearVisitedEdges();
            // find an outside edge to assign depth to
            DirectedEdge de = _finder.Edge;
            // right side of line returned by finder is on the outside
            de.SetEdgeDepths(PositionType.Right, outsideDepth);
            CopySymDepths(de);
            ComputeDepths(de);
        }

        /// <summary>
        /// Compute depths for all dirEdges via breadth-first traversal of nodes in graph.
        /// </summary>
        /// <param name="startEdge">Edge to start processing with.</param>
        // <FIX> MD - use iteration & queue rather than recursion, for speed and robustness
        private static void ComputeDepths(DirectedEdge startEdge)
        {
            var nodesVisited = new HashSet<Node>();
            Queue nodeQueue = new Queue();
            Node startNode = startEdge.Node;
            nodeQueue.Enqueue(startNode);
            nodesVisited.Add(startNode);
            startEdge.IsVisited = true;
            while (nodeQueue.Count != 0)
            {
                Node n = (Node)nodeQueue.Dequeue();
                nodesVisited.Add(n);
                // compute depths around node, starting at this edge since it has depths assigned
                ComputeNodeDepth(n);
                // add all adjacent nodes to process queue, unless the node has been visited already
                IEnumerator i = n.Edges.GetEnumerator();
                while (i.MoveNext())
                {
                    DirectedEdge de = (DirectedEdge)i.Current;
                    DirectedEdge sym = de.Sym;
                    if (sym.IsVisited) continue;
                    Node adjNode = sym.Node;
                    if (!(nodesVisited.Contains(adjNode)))
                    {
                        nodeQueue.Enqueue(adjNode);
                        nodesVisited.Add(adjNode);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="n"></param>
        private static void ComputeNodeDepth(Node n)
        {
            // find a visited dirEdge to start at
            DirectedEdge startEdge = null;
            IEnumerator i = n.Edges.GetEnumerator();
            while (i.MoveNext())
            {
                DirectedEdge de = (DirectedEdge)i.Current;
                if (de.IsVisited || de.Sym.IsVisited)
                {
                    startEdge = de;
                    break;
                }
            }

            // MD - testing  Result: breaks algorithm
            Assert.IsTrue(startEdge != null, "unable to find edge to compute depths at " + n.Coordinate);
            ((DirectedEdgeStar)n.Edges).ComputeDepths(startEdge);

            // copy depths to sym edges
            IEnumerator j = n.Edges.GetEnumerator();
            while (j.MoveNext())
            {
                DirectedEdge de = (DirectedEdge)j.Current;
                de.IsVisited = true;
                CopySymDepths(de);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="de"></param>
        private static void CopySymDepths(DirectedEdge de)
        {
            DirectedEdge sym = de.Sym;
            sym.SetDepth(PositionType.Left, de.GetDepth(PositionType.Right));
            sym.SetDepth(PositionType.Right, de.GetDepth(PositionType.Left));
        }

        /// <summary>
        /// Find all edges whose depths indicates that they are in the result area(s).
        /// Since we want polygon shells to be
        /// oriented CW, choose dirEdges with the interior of the result on the RHS.
        /// Mark them as being in the result.
        /// Interior Area edges are the result of dimensional collapses.
        /// They do not form part of the result area boundary.
        /// </summary>
        public virtual void FindResultEdges()
        {
            for (IEnumerator it = _dirEdgeList.GetEnumerator(); it.MoveNext(); )
            {
                DirectedEdge de = (DirectedEdge)it.Current;
                /*
                * Select edges which have an interior depth on the RHS
                * and an exterior depth on the LHS.
                * Notice that because of weird rounding effects there may be
                * edges which have negative depths!  Negative depths
                * count as "outside".
                */
                // <FIX> - handle negative depths
                if (de.GetDepth(PositionType.Right) >= 1 && de.GetDepth(PositionType.Left) <= 0 && !de.IsInteriorAreaEdge)
                    de.IsInResult = true;
            }
        }
    }
}