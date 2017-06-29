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
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Operation.Overlay
{
    /// <summary>
    /// The spatial functions supported by this class.
    /// These operations implement various bool combinations of the resultants of the overlay.
    /// </summary>
    public enum SpatialFunction
    {
        /// <summary>
        ///
        /// </summary>
        Intersection = 1,

        /// <summary>
        ///
        /// </summary>
        Union = 2,

        /// <summary>
        ///
        /// </summary>
        Difference = 3,

        /// <summary>
        ///
        /// </summary>
        SymDifference = 4,
    }

    /// <summary>
    /// Computes the overlay of two <c>Geometry</c>s.  The overlay
    /// can be used to determine any bool combination of the geometries.
    /// </summary>
    public class OverlayOp : GeometryGraphOperation
    {
        private readonly EdgeList _edgeList = new EdgeList();
        private readonly IGeometryFactory _geomFact;

        private readonly PlanarGraph _graph;
        private readonly PointLocator _ptLocator = new PointLocator();
        private IGeometry _resultGeom;
        private IList _resultLineList = new ArrayList();
        private IList _resultPointList = new ArrayList();
        private IList _resultPolyList = new ArrayList();

        /// <summary>
        ///
        /// </summary>
        /// <param name="g0"></param>
        /// <param name="g1"></param>
        public OverlayOp(IGeometry g0, IGeometry g1)
            : base(g0, g1)
        {
            _graph = new PlanarGraph(new OverlayNodeFactory());
            /*
            * Use factory of primary point.
            * Notice that this does NOT handle mixed-precision arguments
            * where the second arg has greater precision than the first.
            */
            _geomFact = g0.Factory;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual PlanarGraph Graph
        {
            get
            {
                return _graph;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom0"></param>
        /// <param name="geom1"></param>
        /// <param name="opCode"></param>
        /// <returns></returns>
        public static IGeometry Overlay(IGeometry geom0, IGeometry geom1, SpatialFunction opCode)
        {
            OverlayOp gov = new OverlayOp(geom0, geom1);
            IGeometry geomOv = gov.GetResultGeometry(opCode);
            return geomOv;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="label"></param>
        /// <param name="opCode"></param>
        /// <returns></returns>
        public static bool IsResultOfOp(Label label, SpatialFunction opCode)
        {
            LocationType loc0 = label.GetLocation(0);
            LocationType loc1 = label.GetLocation(1);
            return IsResultOfOp(loc0, loc1, opCode);
        }

        /// <summary>
        /// This method will handle arguments of Location.NULL correctly.
        /// </summary>
        /// <returns><c>true</c> if the locations correspond to the opCode.</returns>
        public static bool IsResultOfOp(LocationType loc0, LocationType loc1, SpatialFunction opCode)
        {
            if (loc0 == LocationType.Boundary)
                loc0 = LocationType.Interior;
            if (loc1 == LocationType.Boundary)
                loc1 = LocationType.Interior;

            switch (opCode)
            {
                case SpatialFunction.Intersection:
                    return loc0 == LocationType.Interior && loc1 == LocationType.Interior;
                case SpatialFunction.Union:
                    return loc0 == LocationType.Interior || loc1 == LocationType.Interior;
                case SpatialFunction.Difference:
                    return loc0 == LocationType.Interior && loc1 != LocationType.Interior;
                case SpatialFunction.SymDifference:
                    return (loc0 == LocationType.Interior && loc1 != LocationType.Interior)
                           || (loc0 != LocationType.Interior && loc1 == LocationType.Interior);
                default:
                    return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="funcCode"></param>
        /// <returns></returns>
        public IGeometry GetResultGeometry(SpatialFunction funcCode)
        {
            ComputeOverlay(funcCode);
            return _resultGeom;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="opCode"></param>
        private void ComputeOverlay(SpatialFunction opCode)
        {
            // copy points from input Geometries.
            // This ensures that any Point geometries
            // in the input are considered for inclusion in the result set
            CopyPoints(0);
            CopyPoints(1);

            // node the input Geometries
            Arg[0].ComputeSelfNodes(LineIntersector, false);
            Arg[1].ComputeSelfNodes(LineIntersector, false);

            // --- Needs to convert args to monotonic edges or something.

            // compute intersections between edges of the two input geometries
            Arg[0].ComputeEdgeIntersections(Arg[1], LineIntersector, true);

            IList baseSplitEdges = new ArrayList();
            Arg[0].ComputeSplitEdges(baseSplitEdges);
            Arg[1].ComputeSplitEdges(baseSplitEdges);
            // add the noded edges to this result graph
            InsertUniqueEdges(baseSplitEdges);

            ComputeLabelsFromDepths();
            ReplaceCollapsedEdges();

            _graph.AddEdges(_edgeList.Edges);
            ComputeLabelling();
            LabelIncompleteNodes();

            /*
            * The ordering of building the result Geometries is important.
            * Areas must be built before lines, which must be built before points.
            * This is so that lines which are covered by areas are not included
            * explicitly, and similarly for points.
            */
            FindResultAreaEdges(opCode);
            CancelDuplicateResultEdges();
            PolygonBuilder polyBuilder = new PolygonBuilder(_geomFact);
            polyBuilder.Add(_graph);
            _resultPolyList = polyBuilder.Polygons;

            LineBuilder lineBuilder = new LineBuilder(this, _geomFact, _ptLocator);
            _resultLineList = lineBuilder.Build(opCode);

            PointBuilder pointBuilder = new PointBuilder(this, _geomFact);
            _resultPointList = pointBuilder.Build(opCode);

            // gather the results from all calculations into a single Geometry for the result set
            _resultGeom = ComputeGeometry(_resultPointList, _resultLineList, _resultPolyList);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edges"></param>
        private void InsertUniqueEdges(IEnumerable edges)
        {
            for (IEnumerator i = edges.GetEnumerator(); i.MoveNext(); )
            {
                Edge e = (Edge)i.Current;
                InsertUniqueEdge(e);
            }
        }

        /// <summary>
        /// Insert an edge from one of the noded input graphs.
        /// Checks edges that are inserted to see if an
        /// identical edge already exists.
        /// If so, the edge is not inserted, but its label is merged
        /// with the existing edge.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void InsertUniqueEdge(Edge e)
        {
            int foundIndex = _edgeList.FindEdgeIndex(e);
            // If an identical edge already exists, simply update its label
            if (foundIndex >= 0)
            {
                Edge existingEdge = _edgeList[foundIndex];
                Label existingLabel = existingEdge.Label;

                Label labelToMerge = e.Label;
                // check if new edge is in reverse direction to existing edge
                // if so, must flip the label before merging it
                if (!existingEdge.IsPointwiseEqual(e))
                {
                    labelToMerge = new Label(e.Label);
                    labelToMerge.Flip();
                }
                Depth depth = existingEdge.Depth;
                // if this is the first duplicate found for this edge, initialize the depths
                if (depth.IsNull())
                    depth.Add(existingLabel);
                depth.Add(labelToMerge);
                existingLabel.Merge(labelToMerge);
            }
            else
            {
                // no matching existing edge was found
                // add this new edge to the list of edges in this graph
                _edgeList.Add(e);
            }
        }

        /// <summary>
        /// Update the labels for edges according to their depths.
        /// For each edge, the depths are first normalized.
        /// Then, if the depths for the edge are equal,
        /// this edge must have collapsed into a line edge.
        /// If the depths are not equal, update the label
        /// with the locations corresponding to the depths
        /// (i.e. a depth of 0 corresponds to a Location of Exterior,
        /// a depth of 1 corresponds to Interior)
        /// </summary>
        private void ComputeLabelsFromDepths()
        {
            for (IEnumerator it = _edgeList.GetEnumerator(); it.MoveNext(); )
            {
                Edge e = (Edge)it.Current;
                Label lbl = e.Label;
                Depth depth = e.Depth;
                /*
                * Only check edges for which there were duplicates,
                * since these are the only ones which might
                * be the result of dimensional collapses.
                */
                if (!depth.IsNull())
                {
                    depth.Normalize();
                    for (int i = 0; i < 2; i++)
                    {
                        if (!lbl.IsNull(i) && lbl.IsArea() && !depth.IsNull(i))
                        {
                            /*
                             * if the depths are equal, this edge is the result of
                             * the dimensional collapse of two or more edges.
                             * It has the same location on both sides of the edge,
                             * so it has collapsed to a line.
                             */
                            if (depth.GetDelta(i) == 0)
                                lbl.ToLine(i);
                            else
                            {
                                /*
                                * This edge may be the result of a dimensional collapse,
                                * but it still has different locations on both sides.  The
                                * label of the edge must be updated to reflect the resultant
                                * side locations indicated by the depth values.
                                */
                                Assert.IsTrue(!depth.IsNull(i, PositionType.Left), "depth of Left side has not been initialized");
                                lbl.SetLocation(i, PositionType.Left, depth.GetLocation(i, PositionType.Left));
                                Assert.IsTrue(!depth.IsNull(i, PositionType.Right), "depth of Right side has not been initialized");
                                lbl.SetLocation(i, PositionType.Right, depth.GetLocation(i, PositionType.Right));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// If edges which have undergone dimensional collapse are found,
        /// replace them with a new edge which is a L edge
        /// </summary>
        private void ReplaceCollapsedEdges()
        {
            IList newEdges = new ArrayList();
            IList edgesToRemove = new ArrayList();
            IEnumerator it = _edgeList.GetEnumerator();
            while (it.MoveNext())
            {
                Edge e = (Edge)it.Current;
                if (e.IsCollapsed)
                {
                    // edgeList.Remove(it.Current as Edge);
                    // Diego Guidi says:
                    // This instruction throws a "System.InvalidOperationException: Collection was modified; enumeration operation may not execute".
                    // i try to not modify edgeList here, and remove all elements at the end of iteration.
                    edgesToRemove.Add(it.Current);
                    newEdges.Add(e.CollapsedEdge);
                }
            }
            // Removing all collapsed edges at the end of iteration.
            foreach (Edge obj in edgesToRemove)
                _edgeList.Remove(obj);
            // edgeList.addAll(newEdges);
            foreach (object obj in newEdges)
                _edgeList.Add((Edge)obj);
        }

        /// <summary>
        /// Copy all nodes from an arg point into this graph.
        /// The node label in the arg point overrides any previously computed
        /// label for that argIndex.
        /// (E.g. a node may be an intersection node with
        /// a previously computed label of Boundary,
        /// but in the original arg Geometry it is actually
        /// in the interior due to the Boundary Determination Rule)
        /// </summary>
        /// <param name="argIndex"></param>
        private void CopyPoints(int argIndex)
        {
            IEnumerator i = Arg[argIndex].GetNodeEnumerator();
            while (i.MoveNext())
            {
                Node graphNode = (Node)i.Current;
                Node newNode = _graph.AddNode(graphNode.Coordinate);
                newNode.SetLabel(argIndex, graphNode.Label.GetLocation(argIndex));
            }
        }

        /// <summary>
        /// Compute initial labelling for all DirectedEdges at each node.
        /// In this step, DirectedEdges will acquire a complete labelling
        /// (i.e. one with labels for both Geometries)
        /// only if they
        /// are incident on a node which has edges for both Geometries
        /// </summary>
        private void ComputeLabelling()
        {
            IEnumerator nodeit = _graph.Nodes.GetEnumerator();
            while (nodeit.MoveNext())
            {
                Node node = (Node)nodeit.Current;
                node.Edges.ComputeLabelling(Arg);
            }
            MergeSymLabels();
            UpdateNodeLabelling();
        }

        /// <summary>
        /// For nodes which have edges from only one Geometry incident on them,
        /// the previous step will have left their dirEdges with no labelling for the other
        /// Geometry.  However, the sym dirEdge may have a labelling for the other
        /// Geometry, so merge the two labels.
        /// </summary>
        private void MergeSymLabels()
        {
            IEnumerator nodeit = _graph.Nodes.GetEnumerator();
            while (nodeit.MoveNext())
            {
                Node node = (Node)nodeit.Current;
                ((DirectedEdgeStar)node.Edges).MergeSymLabels();
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void UpdateNodeLabelling()
        {
            // update the labels for nodes
            // The label for a node is updated from the edges incident on it
            // (Notice that a node may have already been labelled
            // because it is a point in one of the input geometries)
            IEnumerator nodeit = _graph.Nodes.GetEnumerator();
            while (nodeit.MoveNext())
            {
                Node node = (Node)nodeit.Current;
                Label lbl = ((DirectedEdgeStar)node.Edges).Label;
                node.Label.Merge(lbl);
            }
        }

        /// <summary>
        /// Incomplete nodes are nodes whose labels are incomplete.
        /// (e.g. the location for one Geometry is null).
        /// These are either isolated nodes,
        /// or nodes which have edges from only a single Geometry incident on them.
        /// Isolated nodes are found because nodes in one graph which don't intersect
        /// nodes in the other are not completely labelled by the initial process
        /// of adding nodes to the nodeList.
        /// To complete the labelling we need to check for nodes that lie in the
        /// interior of edges, and in the interior of areas.
        /// When each node labelling is completed, the labelling of the incident
        /// edges is updated, to complete their labelling as well.
        /// </summary>
        private void LabelIncompleteNodes()
        {
            IEnumerator ni = _graph.Nodes.GetEnumerator();
            while (ni.MoveNext())
            {
                Node n = (Node)ni.Current;
                Label label = n.Label;
                if (n.IsIsolated)
                {
                    if (label.IsNull(0))
                        LabelIncompleteNode(n, 0);
                    else LabelIncompleteNode(n, 1);
                }
                // now update the labelling for the DirectedEdges incident on this node
                ((DirectedEdgeStar)n.Edges).UpdateLabelling(label);
            }
        }

        /// <summary>
        /// Label an isolated node with its relationship to the target point.
        /// </summary>
        private void LabelIncompleteNode(Node n, int targetIndex)
        {
            LocationType loc = _ptLocator.Locate(n.Coordinate, Arg[targetIndex].Geometry);
            n.Label.SetLocation(targetIndex, loc);
        }

        /// <summary>
        /// Find all edges whose label indicates that they are in the result area(s),
        /// according to the operation being performed.  Since we want polygon shells to be
        /// oriented CW, choose dirEdges with the interior of the result on the RHS.
        /// Mark them as being in the result.
        /// Interior Area edges are the result of dimensional collapses.
        /// They do not form part of the result area boundary.
        /// </summary>
        private void FindResultAreaEdges(SpatialFunction opCode)
        {
            IEnumerator it = _graph.EdgeEnds.GetEnumerator();
            while (it.MoveNext())
            {
                DirectedEdge de = (DirectedEdge)it.Current;
                // mark all dirEdges with the appropriate label
                Label label = de.Label;
                if (label.IsArea() && !de.IsInteriorAreaEdge &&
                    IsResultOfOp(label.GetLocation(0, PositionType.Right), label.GetLocation(1, PositionType.Right), opCode))
                    de.IsInResult = true;
            }
        }

        /// <summary>
        /// If both a dirEdge and its sym are marked as being in the result, cancel
        /// them out.
        /// </summary>
        private void CancelDuplicateResultEdges()
        {
            // remove any dirEdges whose sym is also included
            // (they "cancel each other out")
            IEnumerator it = _graph.EdgeEnds.GetEnumerator();
            while (it.MoveNext())
            {
                DirectedEdge de = (DirectedEdge)it.Current;
                DirectedEdge sym = de.Sym;
                if (de.IsInResult && sym.IsInResult)
                {
                    de.IsInResult = false;
                    sym.IsInResult = false;
                }
            }
        }

        /// <summary>
        /// This method is used to decide if a point node should be included in the result or not.
        /// </summary>
        /// <returns><c>true</c> if the coord point is covered by a result Line or Area point.</returns>
        public virtual bool IsCoveredByLa(Coordinate coord)
        {
            if (IsCovered(coord, _resultLineList))
                return true;
            if (IsCovered(coord, _resultPolyList))
                return true;
            return false;
        }

        /// <summary>
        /// This method is used to decide if an L edge should be included in the result or not.
        /// </summary>
        /// <returns><c>true</c> if the coord point is covered by a result Area point.</returns>
        public virtual bool IsCoveredByA(Coordinate coord)
        {
            if (IsCovered(coord, _resultPolyList))
                return true;
            return false;
        }

        /// <returns>
        /// <c>true</c> if the coord is located in the interior or boundary of
        /// a point in the list.
        /// </returns>
        private bool IsCovered(Coordinate coord, IEnumerable geomList)
        {
            IEnumerator it = geomList.GetEnumerator();
            while (it.MoveNext())
            {
                IGeometry geom = (IGeometry)it.Current;
                LocationType loc = _ptLocator.Locate(coord, geom);
                if (loc != LocationType.Exterior)
                    return true;
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resultPointList"></param>
        /// <param name="resultLineList"></param>
        /// <param name="resultPolyList"></param>
        /// <returns></returns>
        private IGeometry ComputeGeometry(IList resultPointList, IList resultLineList, IList resultPolyList)
        {
            ArrayList geomList = new ArrayList();
            // element geometries of the result are always in the order Point, Curve, A
            //geomList.addAll(resultPointList);
            foreach (object obj in resultPointList)
                geomList.Add(obj);

            //geomList.addAll(resultLineList);
            foreach (object obj in resultLineList)
                geomList.Add(obj);

            //geomList.addAll(resultPolyList);
            foreach (object obj in resultPolyList)
                geomList.Add(obj);

            // build the most specific point possible
            return _geomFact.BuildGeometry(geomList);
        }
    }
}