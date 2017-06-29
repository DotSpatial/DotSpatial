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
using DotSpatial.Topology.GeometriesGraph;
using DotSpatial.Topology.Operation.Overlay;
using DotSpatial.Topology.Operation.Polygonize;
using DotSpatial.Topology.Utilities;
using EdgeRing = DotSpatial.Topology.GeometriesGraph.EdgeRing;

namespace DotSpatial.Topology.Operation.Valid
{
    /// <summary>
    /// This class tests that the interior of an area <see cref="Geometry" />
    /// (<see cref="Polygon" /> or <see cref="MultiPolygon" />)
    /// is connected.  An area Geometry is invalid if the interior is disconnected.
    /// This can happen if:
    /// - a shell self-intersects,
    /// - one or more holes form a connected chain touching a shell at two different points,
    /// - one or more holes form a ring around a subset of the interior.
    /// If a disconnected situation is found the location of the problem is recorded.
    /// </summary>
    public class ConnectedInteriorTester
    {
        private readonly GeometryGraph _geomGraph;
        private readonly GeometryFactory _geometryFactory = new GeometryFactory();

        // save a coordinate for any disconnected interior found
        // the coordinate will be somewhere on the ring surrounding the disconnected interior
        private Coordinate _disconnectedRingcoord;

        /// <summary>
        ///
        /// </summary>
        /// <param name="geomGraph"></param>
        public ConnectedInteriorTester(GeometryGraph geomGraph)
        {
            _geomGraph = geomGraph;
        }

        /// <summary>
        ///
        /// </summary>
        public Coordinate Coordinate
        {
            get
            {
                return _disconnectedRingcoord;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        private static Coordinate FindDifferentPoint(IEnumerable<Coordinate> coord, Coordinate pt)
        {
            foreach (Coordinate c in coord)
                if (!c.Equals(pt))
                    return c;
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool IsInteriorsConnected()
        {
            // node the edges, in case holes touch the shell
            IList splitEdges = new ArrayList();
            _geomGraph.ComputeSplitEdges(splitEdges);

            // form the edges into rings
            PlanarGraph graph = new PlanarGraph(new OverlayNodeFactory());
            graph.AddEdges(splitEdges);
            SetInteriorEdgesInResult(graph);
            graph.LinkResultDirectedEdges();
            IList edgeRings = BuildEdgeRings(graph.EdgeEnds);
            /*
             * Mark all the edges for the edgeRings corresponding to the shells
             * of the input polygons.  Notice only ONE ring gets marked for each shell.
             */
            VisitShellInteriors(_geomGraph.Geometry, graph);

            /*
             * If there are any unvisited shell edges
             * (i.e. a ring which is not a hole and which has the interior
             * of the parent area on the RHS)
             * this means that one or more holes must have split the interior of the
             * polygon into at least two pieces.  The polygon is thus invalid.
             */
            return !HasUnvisitedShellEdge(edgeRings);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="graph"></param>
        private static void SetInteriorEdgesInResult(PlanarGraph graph)
        {
            foreach (DirectedEdge de in graph.EdgeEnds)
                if (de.Label.GetLocation(0, PositionType.Right) == LocationType.Interior)
                    de.IsInResult = true;
        }

        /// <summary>
        /// Form <see cref="DirectedEdge" />s in graph into Minimal EdgeRings.
        /// (Minimal Edgerings must be used, because only they are guaranteed to provide
        /// a correct isHole computation).
        /// </summary>
        /// <param name="dirEdges"></param>
        /// <returns></returns>
        private IList BuildEdgeRings(IList dirEdges)
        {
            IList edgeRings = new ArrayList();
            foreach (DirectedEdge de in dirEdges)
            {
                // if this edge has not yet been processed
                if (de.IsInResult && de.EdgeRing == null)
                {
                    MaximalEdgeRing er = new MaximalEdgeRing(de, _geometryFactory);

                    er.LinkDirectedEdgesForMinimalEdgeRings();
                    IList minEdgeRings = er.BuildMinimalRings();
                    foreach (object o in minEdgeRings)
                        edgeRings.Add(o);
                }
            }
            return edgeRings;
        }

        /// <summary>
        /// Mark all the edges for the edgeRings corresponding to the shells of the input polygons.
        /// Only ONE ring gets marked for each shell - if there are others which remain unmarked
        /// this indicates a disconnected interior.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="graph"></param>
        private static void VisitShellInteriors(IGeometry g, PlanarGraph graph)
        {
            if (g is Polygon)
            {
                Polygon p = (Polygon)g;
                VisitInteriorRing(p.Shell, graph);
            }
            if (g is MultiPolygon)
            {
                MultiPolygon mp = (MultiPolygon)g;
                foreach (Polygon p in mp.Geometries)
                    VisitInteriorRing(p.Shell, graph);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ring"></param>
        /// <param name="graph"></param>
        private static void VisitInteriorRing(IBasicGeometry ring, PlanarGraph graph)
        {
            IList<Coordinate> pts = ring.Coordinates;
            Coordinate pt0 = pts[0];
            /*
             * Find first point in coord list different to initial point.
             * Need special check since the first point may be repeated.
             */
            Coordinate pt1 = FindDifferentPoint(pts, pt0);
            Edge e = graph.FindEdgeInSameDirection(pt0, pt1);
            DirectedEdge de = (DirectedEdge)graph.FindEdgeEnd(e);
            DirectedEdge intDe = null;
            if (de.Label.GetLocation(0, PositionType.Right) == LocationType.Interior)
                intDe = de;
            else if (de.Sym.Label.GetLocation(0, PositionType.Right) == LocationType.Interior)
                intDe = de.Sym;
            Assert.IsTrue(intDe != null, "unable to find dirEdge with Interior on RHS");
            VisitLinkedDirectedEdges(intDe);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="start"></param>
        private static void VisitLinkedDirectedEdges(DirectedEdge start)
        {
            DirectedEdge startDe = start;
            DirectedEdge de = start;
            do
            {
                if (de == null) throw new NullEdgeException();
                de.IsVisited = true;
                de = de.Next;
            }
            while (de != startDe);
        }

        /// <summary>
        /// Check if any shell ring has an unvisited edge.
        /// A shell ring is a ring which is not a hole and which has the interior
        /// of the parent area on the RHS.
        /// (Notice that there may be non-hole rings with the interior on the LHS,
        /// since the interior of holes will also be polygonized into CW rings
        /// by the <c>LinkAllDirectedEdges()</c> step).
        /// </summary>
        /// <param name="edgeRings"></param>
        /// <returns><c>true</c> if there is an unvisited edge in a non-hole ring.</returns>
        private bool HasUnvisitedShellEdge(IList edgeRings)
        {
            for (int i = 0; i < edgeRings.Count; i++)
            {
                EdgeRing er = (EdgeRing)edgeRings[i];
                if (er.IsHole) continue;
                IList edges = er.Edges;
                DirectedEdge de = (DirectedEdge)edges[0];
                // don't check CW rings which are holes
                if (de.Label.GetLocation(0, PositionType.Right) != LocationType.Interior) continue;

                // must have a CW ring which surrounds the INT of the area, so check all
                // edges have been visited
                for (int j = 0; j < edges.Count; j++)
                {
                    de = (DirectedEdge)edges[j];
                    if (!de.IsVisited)
                    {
                        _disconnectedRingcoord = de.Coordinate;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}