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
    /// Forms NTS LineStrings out of a the graph of <c>DirectedEdge</c>s
    /// created by an <c>OverlayOp</c>.
    /// </summary>
    public class LineBuilder
    {
        private readonly IGeometryFactory _geometryFactory;

        private readonly IList _lineEdgesList = new ArrayList();
        private readonly OverlayOp _op;
        private readonly PointLocator _ptLocator;
        private readonly IList _resultLineList = new ArrayList();

        /// <summary>
        ///
        /// </summary>
        /// <param name="op"></param>
        /// <param name="geometryFactory"></param>
        /// <param name="ptLocator"></param>
        public LineBuilder(OverlayOp op, IGeometryFactory geometryFactory, PointLocator ptLocator)
        {
            _op = op;
            _geometryFactory = geometryFactory;
            _ptLocator = ptLocator;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="opCode"></param>
        /// <returns>
        /// A list of the LineStrings in the result of the specified overlay operation.
        /// </returns>
        public virtual IList Build(SpatialFunction opCode)
        {
            FindCoveredLineEdges();
            CollectLines(opCode);
            BuildLines(opCode);
            return _resultLineList;
        }

        /// <summary>
        /// Find and mark L edges which are "covered" by the result area (if any).
        /// L edges at nodes which also have A edges can be checked by checking
        /// their depth at that node.
        /// L edges at nodes which do not have A edges can be checked by doing a
        /// point-in-polygon test with the previously computed result areas.
        /// </summary>
        private void FindCoveredLineEdges()
        {
            // first set covered for all L edges at nodes which have A edges too
            IEnumerator nodeit = _op.Graph.Nodes.GetEnumerator();
            while (nodeit.MoveNext())
            {
                Node node = (Node)nodeit.Current;
                ((DirectedEdgeStar)node.Edges).FindCoveredLineEdges();
            }

            /*
             * For all Curve edges which weren't handled by the above,
             * use a point-in-poly test to determine whether they are covered
             */
            IEnumerator it = _op.Graph.EdgeEnds.GetEnumerator();
            while (it.MoveNext())
            {
                DirectedEdge de = (DirectedEdge)it.Current;
                Edge e = de.Edge;
                if (de.IsLineEdge && !e.IsCoveredSet)
                {
                    bool isCovered = _op.IsCoveredByA(de.Coordinate);
                    e.IsCovered = isCovered;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="opCode"></param>
        private void CollectLines(SpatialFunction opCode)
        {
            IEnumerator it = _op.Graph.EdgeEnds.GetEnumerator();
            while (it.MoveNext())
            {
                DirectedEdge de = (DirectedEdge)it.Current;
                CollectLineEdge(de, opCode, _lineEdgesList);
                CollectBoundaryTouchEdge(de, opCode, _lineEdgesList);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="de"></param>
        /// <param name="opCode"></param>
        /// <param name="edges"></param>
        public void CollectLineEdge(DirectedEdge de, SpatialFunction opCode, IList edges)
        {
            Label label = de.Label;
            Edge e = de.Edge;
            // include Curve edges which are in the result
            if (de.IsLineEdge)
            {
                if (!de.IsVisited && OverlayOp.IsResultOfOp(label, opCode) && !e.IsCovered)
                {
                    edges.Add(e);
                    de.VisitedEdge = true;
                }
            }
        }

        /// <summary>
        /// Collect edges from Area inputs which should be in the result but
        /// which have not been included in a result area.
        /// This happens ONLY:
        /// during an intersection when the boundaries of two
        /// areas touch in a line segment
        /// OR as a result of a dimensional collapse.
        /// </summary>
        /// <param name="de"></param>
        /// <param name="opCode"></param>
        /// <param name="edges"></param>
        public virtual void CollectBoundaryTouchEdge(DirectedEdge de, SpatialFunction opCode, IList edges)
        {
            Label label = de.Label;
            if (de.IsLineEdge)
                return;         // only interested in area edges
            if (de.IsVisited)
                return;         // already processed
            if (de.IsInteriorAreaEdge)
                return; // added to handle dimensional collapses
            if (de.Edge.IsInResult)
                return;     // if the edge linework is already included, don't include it again

            // sanity check for labelling of result edgerings
            Assert.IsTrue(!(de.IsInResult || de.Sym.IsInResult) || !de.Edge.IsInResult);
            // include the linework if it's in the result of the operation
            if (OverlayOp.IsResultOfOp(label, opCode) && opCode == SpatialFunction.Intersection)
            {
                edges.Add(de.Edge);
                de.VisitedEdge = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="opCode"></param>
        private void BuildLines(SpatialFunction opCode)
        {
            for (IEnumerator it = _lineEdgesList.GetEnumerator(); it.MoveNext(); )
            {
                Edge e = (Edge)it.Current;
                ILineString line = _geometryFactory.CreateLineString(e.Coordinates);
                _resultLineList.Add(line);
                e.IsInResult = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edgesList"></param>
        private void LabelIsolatedLines(IList edgesList)
        {
            IEnumerator it = edgesList.GetEnumerator();
            while (it.MoveNext())
            {
                Edge e = (Edge)it.Current;
                Label label = e.Label;
                if (e.IsIsolated)
                {
                    if (label.IsNull(0))
                        LabelIsolatedLine(e, 0);
                    else LabelIsolatedLine(e, 1);
                }
            }
        }

        /// <summary>
        /// Label an isolated node with its relationship to the target point.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="targetIndex"></param>
        private void LabelIsolatedLine(Edge e, int targetIndex)
        {
            LocationType loc = _ptLocator.Locate(e.Coordinate, _op.GetArgGeometry(targetIndex));
            e.Label.SetLocation(targetIndex, loc);
        }
    }
}