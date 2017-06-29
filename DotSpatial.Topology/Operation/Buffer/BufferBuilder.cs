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
using DotSpatial.Topology.Noding;
using DotSpatial.Topology.Operation.Overlay;

namespace DotSpatial.Topology.Operation.Buffer
{
    /// <summary>
    /// Builds the buffer point for a given input point and precision model.
    /// Allows setting the level of approximation for circular arcs,
    /// and the precision model in which to carry out the computation.
    /// When computing buffers in floating point double-precision
    /// it can happen that the process of iterated noding can fail to converge (terminate).
    /// In this case a TopologyException will be thrown.
    /// Retrying the computation in a fixed precision
    /// can produce more robust results.
    /// </summary>
    public class BufferBuilder
    {
        private readonly EdgeList _edgeList = new EdgeList();
        private BufferStyle _endCapStyle = BufferStyle.CapRound;

        private IGeometryFactory _geomFact;
        private PlanarGraph _graph;
        private int _quadrantSegments = OffsetCurveBuilder.DEFAULT_QUADRANT_SEGMENTS;
        private PrecisionModel _workingPrecisionModel;

        /// <summary>
        /// Gets/Sets the number of segments used to approximate a angle fillet.
        /// </summary>
        public virtual int QuadrantSegments
        {
            get
            {
                return _quadrantSegments;
            }
            set
            {
                _quadrantSegments = value;
            }
        }

        /// <summary>
        /// Gets/Sets the precision model to use during the curve computation and noding,
        /// if it is different to the precision model of the Geometry.
        /// If the precision model is less than the precision of the Geometry precision model,
        /// the Geometry must have previously been rounded to that precision.
        /// </summary>
        public virtual PrecisionModel WorkingPrecisionModel
        {
            get
            {
                return _workingPrecisionModel;
            }
            set
            {
                _workingPrecisionModel = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual BufferStyle EndCapStyle
        {
            get
            {
                return _endCapStyle;
            }
            set
            {
                _endCapStyle = value;
            }
        }

        /// <summary>
        /// Compute the change in depth as an edge is crossed from R to L.
        /// </summary>
        /// <param name="label"></param>
        private static int DepthDelta(Label label)
        {
            LocationType lLoc = label.GetLocation(0, PositionType.Left);
            LocationType rLoc = label.GetLocation(0, PositionType.Right);
            if (lLoc == LocationType.Interior && rLoc == LocationType.Exterior)
                return 1;
            if (lLoc == LocationType.Exterior && rLoc == LocationType.Interior)
                return -1;
            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public IGeometry Buffer(IGeometry g, double distance)
        {
            PrecisionModel precisionModel = _workingPrecisionModel ?? new PrecisionModel(g.PrecisionModel);

            // factory must be the same as the one used by the input
            _geomFact = g.Factory;

            OffsetCurveBuilder curveBuilder = new OffsetCurveBuilder(precisionModel, _quadrantSegments);
            curveBuilder.EndCapStyle = _endCapStyle;
            OffsetCurveSetBuilder curveSetBuilder = new OffsetCurveSetBuilder(g, distance, curveBuilder);

            IList bufferSegStrList = curveSetBuilder.GetCurves();

            // short-circuit test
            if (bufferSegStrList.Count <= 0)
            {
                IGeometry emptyGeom = _geomFact.CreateGeometryCollection(new Geometry[0]);
                return emptyGeom;
            }

            ComputeNodedEdges(bufferSegStrList, precisionModel);
            _graph = new PlanarGraph(new OverlayNodeFactory());
            _graph.AddEdges(_edgeList.Edges);

            IList subgraphList = CreateSubgraphs(_graph);
            PolygonBuilder polyBuilder = new PolygonBuilder(_geomFact);
            BuildSubgraphs(subgraphList, polyBuilder);
            IList resultPolyList = polyBuilder.Polygons;

            IGeometry resultGeom = _geomFact.BuildGeometry(resultPolyList);
            return resultGeom;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="precisionModel"></param>
        /// <returns></returns>
        private static INoder GetNoder(PrecisionModel precisionModel)
        {
            // otherwise use a fast (but non-robust) noder
            LineIntersector li = new RobustLineIntersector();
            li.PrecisionModel = precisionModel;
            McIndexNoder noder = new McIndexNoder(new IntersectionAdder(li));
            return noder;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bufferSegStrList"></param>
        /// <param name="precisionModel"></param>
        private void ComputeNodedEdges(IList bufferSegStrList, PrecisionModel precisionModel)
        {
            INoder noder = GetNoder(precisionModel);
            noder.ComputeNodes(bufferSegStrList);
            IList nodedSegStrings = noder.GetNodedSubstrings();

            foreach (object obj in nodedSegStrings)
            {
                SegmentString segStr = (SegmentString)obj;
                Label oldLabel = (Label)segStr.Data;
                Edge edge = new Edge(segStr.Coordinates, new Label(oldLabel));
                InsertEdge(edge);
            }
        }

        /// <summary>
        /// Inserted edges are checked to see if an identical edge already exists.
        /// If so, the edge is not inserted, but its label is merged
        /// with the existing edge.
        /// </summary>
        /// <param name="e"></param>
        protected void InsertEdge(Edge e)
        {
            //<FIX> MD 8 Oct 03  speed up identical edge lookup
            // fast lookup
            Edge existingEdge = _edgeList.FindEqualEdge(e);

            // If an identical edge already exists, simply update its label
            if (existingEdge != null)
            {
                Label existingLabel = existingEdge.Label;

                Label labelToMerge = e.Label;
                // check if new edge is in reverse direction to existing edge
                // if so, must flip the label before merging it
                if (!existingEdge.IsPointwiseEqual(e))
                {
                    labelToMerge = new Label(e.Label);
                    labelToMerge.Flip();
                }
                existingLabel.Merge(labelToMerge);

                // compute new depth delta of sum of edges
                int mergeDelta = DepthDelta(labelToMerge);
                int existingDelta = existingEdge.DepthDelta;
                int newDelta = existingDelta + mergeDelta;
                existingEdge.DepthDelta = newDelta;
            }
            else
            {   // no matching existing edge was found
                // add this new edge to the list of edges in this graph
                //e.setName(name + edges.size());
                _edgeList.Add(e);
                e.DepthDelta = DepthDelta(e.Label);
            }
        }

        private static IList CreateSubgraphs(PlanarGraph graph)
        {
            ArrayList subgraphList = new ArrayList();
            foreach (object obj in graph.Nodes)
            {
                Node node = (Node)obj;
                if (!node.IsVisited)
                {
                    BufferSubgraph subgraph = new BufferSubgraph();
                    subgraph.Create(node);
                    subgraphList.Add(subgraph);
                }
            }

            /*
             * Sort the subgraphs in descending order of their rightmost coordinate.
             * This ensures that when the Polygons for the subgraphs are built,
             * subgraphs for shells will have been built before the subgraphs for
             * any holes they contain.
             */
            subgraphList.Sort();
            subgraphList.Reverse();
            return subgraphList;
        }

        /// <summary>
        /// Completes the building of the input subgraphs by depth-labelling them,
        /// and adds them to the <see cref="PolygonBuilder" />.
        /// The subgraph list must be sorted in rightmost-coordinate order.
        /// </summary>
        /// <param name="subgraphList">The subgraphs to build.</param>
        /// <param name="polyBuilder">The PolygonBuilder which will build the final polygons.</param>
        private static void BuildSubgraphs(IList subgraphList, PolygonBuilder polyBuilder)
        {
            IList processedGraphs = new ArrayList();
            foreach (object obj in subgraphList)
            {
                BufferSubgraph subgraph = (BufferSubgraph)obj;
                Coordinate p = subgraph.RightMostCoordinate;
                SubgraphDepthLocater locater = new SubgraphDepthLocater(processedGraphs);
                int outsideDepth = locater.GetDepth(p);
                subgraph.ComputeDepth(outsideDepth);
                subgraph.FindResultEdges();
                processedGraphs.Add(subgraph);
                polyBuilder.Add(subgraph.DirectedEdges, subgraph.Nodes);
            }
        }
    }
}