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
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.GeometriesGraph;
using DotSpatial.Topology.Noding;
using DotSpatial.Topology.Operation.Overlay;

namespace DotSpatial.Topology.Operation.Buffer
{
    ///<summary>
    /// Builds the buffer geometry for a given input geometry and precision model.
    /// Allows setting the level of approximation for circular arcs,
    /// and the precision model in which to carry out the computation.
    ///</summary>
    /// <remarks>
    /// When computing buffers in floating point double-precision 
    /// it can happen that the process of iterated noding can fail to converge (terminate).
    /// In this case a <see cref="TopologyException"/> will be thrown.
    /// Retrying the computation in a fixed precision
    /// can produce more robust results.
    /// </remarks>
    internal class BufferBuilder
    {
        #region Fields

        private readonly IBufferParameters _bufParams;
        private readonly EdgeList _edgeList = new EdgeList();
        private IGeometryFactory _geomFact;
        private PlanarGraph _graph;
        private INoder _workingNoder;
        private IPrecisionModel _workingPrecisionModel;

        #endregion

        #region Constructors

        ///<summary>Creates a new BufferBuilder</summary>
        public BufferBuilder(IBufferParameters bufParams)
        {
            _bufParams = bufParams;
        }

        #endregion

        #region Properties

        ///<summary>
        /// Sets the <see cref="INoder"/> to use during noding.
        /// This allows choosing fast but non-robust noding, or slower
        /// but robust noding.
        ///</summary>
        public INoder Noder
        {
            get { return _workingNoder; }
            set { _workingNoder = value; }
        }

        ///<summary>
        /// Sets the precision model to use during the curve computation and noding, 
        /// if it is different to the precision model of the Geometry.
        ///</summary>
        ///<remarks>
        /// If the precision model is less than the precision of the Geometry precision model,
        /// the Geometry must have previously been rounded to that precision.
        ///</remarks>
        public virtual IPrecisionModel WorkingPrecisionModel
        {
            get { return _workingPrecisionModel; }
            set { _workingPrecisionModel = value; }
        }

        #endregion

        #region Methods

        public IGeometry Buffer(IGeometry g, double distance)
        {
            IPrecisionModel precisionModel = _workingPrecisionModel ?? (IPrecisionModel) g.PrecisionModel;

            // factory must be the same as the one used by the input
            _geomFact = g.Factory;

            OffsetCurveBuilder curveBuilder = new OffsetCurveBuilder(precisionModel, _bufParams);

            OffsetCurveSetBuilder curveSetBuilder = new OffsetCurveSetBuilder(g, distance, curveBuilder);

            var bufferSegStrList = curveSetBuilder.GetCurves();

            // short-circuit test
            if (bufferSegStrList.Count <= 0)
            {
                return CreateEmptyResultGeometry();
            }

            ComputeNodedEdges(bufferSegStrList, precisionModel);
            _graph = new PlanarGraph(new OverlayNodeFactory());
            _graph.AddEdges(_edgeList.Edges);

            IEnumerable<BufferSubgraph> subgraphList = CreateSubgraphs(_graph);
            PolygonBuilder polyBuilder = new PolygonBuilder(_geomFact);
            BuildSubgraphs(subgraphList, polyBuilder);
            var resultPolyList = polyBuilder.Polygons;

            // just in case...
            if (resultPolyList.Count <= 0)
                return CreateEmptyResultGeometry();

            IGeometry resultGeom = _geomFact.BuildGeometry(resultPolyList);
            return resultGeom;
        }

        /// <summary>
        /// Completes the building of the input subgraphs by depth-labelling them,
        /// and adds them to the PolygonBuilder.
        /// </summary>
        /// <remarks>
        /// The subgraph list must be sorted in rightmost-coordinate order.
        /// </remarks>
        /// <param name="subgraphList"> the subgraphs to build</param>
        /// <param name="polyBuilder"> the PolygonBuilder which will build the final polygons</param>
        private static void BuildSubgraphs(IEnumerable<BufferSubgraph> subgraphList, PolygonBuilder polyBuilder)
        {
            var processedGraphs = new List<BufferSubgraph>();
            foreach (var subgraph in subgraphList)
            {
                Coordinate p = subgraph.RightMostCoordinate;
                var locater = new SubgraphDepthLocater(processedGraphs);
                int outsideDepth = locater.GetDepth(p);
                subgraph.ComputeDepth(outsideDepth);
                subgraph.FindResultEdges();
                processedGraphs.Add(subgraph);
                polyBuilder.Add(new List<EdgeEnd>(
                    Utilities.Caster.Upcast<DirectedEdge, EdgeEnd>(subgraph.DirectedEdges)), subgraph.Nodes);
            }
        }

        private void ComputeNodedEdges(IList<ISegmentString> bufferSegStrList, IPrecisionModel precisionModel)
        {
            var noder = GetNoder(precisionModel);
            noder.ComputeNodes(bufferSegStrList);
            var nodedSegStrings = noder.GetNodedSubstrings();

            foreach (var segStr in nodedSegStrings)
            {
                var pts = segStr.Coordinates;
                if (pts.Count == 2 && pts[0].Equals2D(pts[1]))
                    continue;

                var oldLabel = (Label)segStr.Context;
                var edge = new Edge(segStr.Coordinates, new Label(oldLabel));

                InsertUniqueEdge(edge);
            }
        }

        private static IGeometry ConvertSegStrings(IEnumerator<ISegmentString> it)
        {
            var fact = new GeometryFactory();
            var lines = new List<IGeometry>();
            while (it.MoveNext())
            {
                ISegmentString ss = it.Current;
                ILineString line = fact.CreateLineString(ss.Coordinates);
                lines.Add(line);
            }
            return fact.BuildGeometry(lines);
        }

        ///<summary>
        /// Gets the standard result for an empty buffer.
        /// Since buffer always returns a polygonal result, this is chosen to be an empty polygon.
        ///</summary>
        /// <returns>The empty result geometry</returns>
        private IGeometry CreateEmptyResultGeometry()
        {
            IGeometry emptyGeom = _geomFact.CreatePolygon(null, null);
            return emptyGeom;
        }

        private static IEnumerable<BufferSubgraph> CreateSubgraphs(PlanarGraph graph)
        {
            var subgraphList = new List<BufferSubgraph>();
            foreach (Node node in graph.Nodes)
            {
                if (!node.IsVisited)
                {
                    var subgraph = new BufferSubgraph();
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

        private INoder GetNoder(IPrecisionModel precisionModel)
        {
            if (_workingNoder != null) return _workingNoder;

            // otherwise use a fast (but non-robust) noder
            var noder = new McIndexNoder(new IntersectionAdder(new RobustLineIntersector { PrecisionModel = precisionModel}));
            
            return noder;
        }

        /// <summary>
        /// Inserted edges are checked to see if an identical edge already exists.
        /// If so, the edge is not inserted, but its label is merged
        /// with the existing edge.
        /// </summary>
        /// <param name="e"></param>
        protected void InsertUniqueEdge(Edge e)
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

        #endregion
    }
}