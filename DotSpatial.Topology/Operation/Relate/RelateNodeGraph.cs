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
    /// Implements the simple graph of Nodes and EdgeEnd which is all that is
    /// required to determine topological relationships between Geometries.
    /// Also supports building a topological graph of a single Geometry, to
    /// allow verification of valid topology.
    /// It is not necessary to create a fully linked
    /// PlanarGraph to determine relationships, since it is sufficient
    /// to know how the Geometries interact locally around the nodes.
    /// In fact, this is not even feasible, since it is not possible to compute
    /// exact intersection points, and hence the topology around those nodes
    /// cannot be computed robustly.
    /// The only Nodes that are created are for improper intersections;
    /// that is, nodes which occur at existing vertices of the Geometries.
    /// Proper intersections (e.g. ones which occur between the interior of line segments)
    /// have their topology determined implicitly, without creating a Node object
    /// to represent them.
    /// </summary>
    public class RelateNodeGraph
    {
        private readonly NodeMap _nodes = new NodeMap(new RelateNodeFactory());

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
        /// <param name="geomGraph"></param>
        public virtual void Build(GeometryGraph geomGraph)
        {
            // compute nodes for intersections between previously noded edges
            ComputeIntersectionNodes(geomGraph, 0);
            /*
            * Copy the labelling for the nodes in the parent Geometry.  These override
            * any labels determined by intersections.
            */
            CopyNodesAndLabels(geomGraph, 0);

            /*
            * Build EdgeEnds for all intersections.
            */
            EdgeEndBuilder eeBuilder = new EdgeEndBuilder();
            IList eeList = eeBuilder.ComputeEdgeEnds(geomGraph.GetEdgeEnumerator());
            InsertEdgeEnds(eeList);
        }

        /// <summary>
        /// Insert nodes for all intersections on the edges of a Geometry.
        /// Label the created nodes the same as the edge label if they do not already have a label.
        /// This allows nodes created by either self-intersections or
        /// mutual intersections to be labelled.
        /// Endpoint nodes will already be labelled from when they were inserted.
        /// Precondition: edge intersections have been computed.
        /// </summary>
        /// <param name="geomGraph"></param>
        /// <param name="argIndex"></param>
        public virtual void ComputeIntersectionNodes(GeometryGraph geomGraph, int argIndex)
        {
            for (IEnumerator edgeIt = geomGraph.GetEdgeEnumerator(); edgeIt.MoveNext(); )
            {
                Edge e = (Edge)edgeIt.Current;
                LocationType eLoc = e.Label.GetLocation(argIndex);
                for (IEnumerator eiIt = e.EdgeIntersectionList.GetEnumerator(); eiIt.MoveNext(); )
                {
                    EdgeIntersection ei = (EdgeIntersection)eiIt.Current;
                    RelateNode n = (RelateNode)_nodes.AddNode(ei.Coordinate);
                    if (eLoc == LocationType.Boundary)
                        n.SetLabelBoundary(argIndex);
                    else if (n.Label.IsNull(argIndex))
                        n.SetLabel(argIndex, LocationType.Interior);
                }
            }
        }

        /// <summary>
        /// Copy all nodes from an arg point into this graph.
        /// The node label in the arg point overrides any previously computed
        /// label for that argIndex.
        /// (E.g. a node may be an intersection node with
        /// a computed label of Boundary,
        /// but in the original arg Geometry it is actually
        /// in the interior due to the Boundary Determination Rule).
        /// </summary>
        /// <param name="geomGraph"></param>
        /// <param name="argIndex"></param>
        public virtual void CopyNodesAndLabels(GeometryGraph geomGraph, int argIndex)
        {
            for (IEnumerator nodeIt = geomGraph.GetNodeEnumerator(); nodeIt.MoveNext(); )
            {
                Node graphNode = (Node)nodeIt.Current;
                Node newNode = _nodes.AddNode(graphNode.Coordinate);
                newNode.SetLabel(argIndex, graphNode.Label.GetLocation(argIndex));
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ee"></param>
        public virtual void InsertEdgeEnds(IList ee)
        {
            for (IEnumerator i = ee.GetEnumerator(); i.MoveNext(); )
            {
                EdgeEnd e = (EdgeEnd)i.Current;
                _nodes.Add(e);
            }
        }
    }
}