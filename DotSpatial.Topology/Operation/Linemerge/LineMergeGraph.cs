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
using DotSpatial.Topology.Planargraph;

namespace DotSpatial.Topology.Operation.Linemerge
{
    /// <summary>
    /// A planar graph of edges that is analyzed to sew the edges together. The
    /// <c>marked</c> flag on <c>com.vividsolutions.planargraph.Edge</c>s
    /// and <c>com.vividsolutions.planargraph.Node</c>s indicates whether they have been
    /// logically deleted from the graph.
    /// </summary>
    public class LineMergeGraph : PlanarGraph
    {
        /// <summary>
        /// Adds an Edge, DirectedEdges, and Nodes for the given LineString representation
        /// of an edge.
        /// </summary>
        public virtual void AddEdge(LineString lineString)
        {
            if (lineString.IsEmpty)
                return;
            IList<Coordinate> coordinates = CoordinateArrays.RemoveRepeatedPoints(lineString.Coordinates);
            Coordinate startCoordinate = coordinates[0];
            Coordinate endCoordinate = coordinates[coordinates.Count - 1];
            Node startNode = GetNode(startCoordinate);
            Node endNode = GetNode(endCoordinate);
            DirectedEdge directedEdge0 = new LineMergeDirectedEdge(startNode, endNode,
                                                coordinates[1], true);
            DirectedEdge directedEdge1 = new LineMergeDirectedEdge(endNode, startNode,
                                                coordinates[coordinates.Count - 2], false);
            Edge edge = new LineMergeEdge(lineString);
            edge.SetDirectedEdges(directedEdge0, directedEdge1);
            Add(edge);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        private Node GetNode(Coordinate coordinate)
        {
            Node node = FindNode(coordinate);
            if (node == null)
            {
                node = new Node(coordinate);
                Add(node);
            }
            return node;
        }
    }
}