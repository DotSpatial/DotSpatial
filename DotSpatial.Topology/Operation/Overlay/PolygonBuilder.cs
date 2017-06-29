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
    /// Forms <c>Polygon</c>s out of a graph of {DirectedEdge}s.
    /// The edges to use are marked as being in the result Area.
    /// </summary>
    public class PolygonBuilder
    {
        private readonly IGeometryFactory _geometryFactory;
        private readonly IList _shellList = new ArrayList();

        /// <summary>
        ///
        /// </summary>
        /// <param name="geometryFactory"></param>
        public PolygonBuilder(IGeometryFactory geometryFactory)
        {
            _geometryFactory = geometryFactory;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IList Polygons
        {
            get
            {
                IList resultPolyList = ComputePolygons(_shellList);
                return resultPolyList;
            }
        }

        /// <summary>
        /// Add a complete graph.
        /// The graph is assumed to contain one or more polygons,
        /// possibly with holes.
        /// </summary>
        /// <param name="graph"></param>
        public virtual void Add(PlanarGraph graph)
        {
            Add(graph.EdgeEnds, graph.NodeValues);
        }

        /// <summary>
        /// Add a set of edges and nodes, which form a graph.
        /// The graph is assumed to contain one or more polygons,
        /// possibly with holes.
        /// </summary>
        /// <param name="dirEdges"></param>
        /// <param name="nodes"></param>
        public virtual void Add(IList dirEdges, IList nodes)
        {
            PlanarGraph.LinkResultDirectedEdges(nodes);
            IList maxEdgeRings = BuildMaximalEdgeRings(dirEdges);
            IList freeHoleList = new ArrayList();
            IList edgeRings = BuildMinimalEdgeRings(maxEdgeRings, _shellList, freeHoleList);
            SortShellsAndHoles(edgeRings, _shellList, freeHoleList);
            PlaceFreeHoles(_shellList, freeHoleList);
            //Assert: every hole on freeHoleList has a shell assigned to it
        }

        /// <summary>
        /// For all DirectedEdges in result, form them into MaximalEdgeRings.
        /// </summary>
        /// <param name="dirEdges"></param>
        /// <returns></returns>
        private IList BuildMaximalEdgeRings(IEnumerable dirEdges)
        {
            IList maxEdgeRings = new ArrayList();
            for (IEnumerator it = dirEdges.GetEnumerator(); it.MoveNext(); )
            {
                DirectedEdge de = (DirectedEdge)it.Current;
                if (!de.IsInResult || !de.Label.IsArea()) continue;
                // if this edge has not yet been processed
                if (de.EdgeRing != null) continue;
                MaximalEdgeRing er = new MaximalEdgeRing(de, _geometryFactory);
                maxEdgeRings.Add(er);
                er.SetInResult();
            }
            return maxEdgeRings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="maxEdgeRings"></param>
        /// <param name="shellList"></param>
        /// <param name="freeHoleList"></param>
        /// <returns></returns>
        private static IList BuildMinimalEdgeRings(IEnumerable maxEdgeRings, IList shellList, IList freeHoleList)
        {
            IList edgeRings = new ArrayList();
            for (IEnumerator it = maxEdgeRings.GetEnumerator(); it.MoveNext(); )
            {
                MaximalEdgeRing er = (MaximalEdgeRing)it.Current;
                if (er.MaxNodeDegree > 2)
                {
                    er.LinkDirectedEdgesForMinimalEdgeRings();
                    IList minEdgeRings = er.BuildMinimalRings();
                    // at this point we can go ahead and attempt to place holes, if this EdgeRing is a polygon
                    EdgeRing shell = FindShell(minEdgeRings);
                    if (shell != null)
                    {
                        PlacePolygonHoles(shell, minEdgeRings);
                        shellList.Add(shell);
                    }
                    else
                    {
                        // freeHoleList.addAll(minEdgeRings);
                        foreach (object obj in minEdgeRings)
                            freeHoleList.Add(obj);
                    }
                }
                else edgeRings.Add(er);
            }
            return edgeRings;
        }

        /// <summary>
        /// This method takes a list of MinimalEdgeRings derived from a MaximalEdgeRing,
        /// and tests whether they form a Polygon.  This is the case if there is a single shell
        /// in the list.  In this case the shell is returned.
        /// The other possibility is that they are a series of connected holes, in which case
        /// no shell is returned.
        /// </summary>
        /// <returns>The shell EdgeRing, if there is one.</returns>
        /// <returns><c>null</c>, if all the rings are holes.</returns>
        private static EdgeRing FindShell(IEnumerable minEdgeRings)
        {
            int shellCount = 0;
            EdgeRing shell = null;
            for (IEnumerator it = minEdgeRings.GetEnumerator(); it.MoveNext(); )
            {
                EdgeRing er = (MinimalEdgeRing)it.Current;
                if (er.IsHole) continue;
                shell = er;
                shellCount++;
            }
            Assert.IsTrue(shellCount <= 1, "found two shells in MinimalEdgeRing list");
            return shell;
        }

        /// <summary>
        /// This method assigns the holes for a Polygon (formed from a list of
        /// MinimalEdgeRings) to its shell.
        /// Determining the holes for a MinimalEdgeRing polygon serves two purposes:
        /// it is faster than using a point-in-polygon check later on.
        /// it ensures correctness, since if the PIP test was used the point
        /// chosen might lie on the shell, which might return an incorrect result from the
        /// PIP test.
        /// </summary>
        /// <param name="shell"></param>
        /// <param name="minEdgeRings"></param>
        private static void PlacePolygonHoles(EdgeRing shell, IEnumerable minEdgeRings)
        {
            for (IEnumerator it = minEdgeRings.GetEnumerator(); it.MoveNext(); )
            {
                MinimalEdgeRing er = (MinimalEdgeRing)it.Current;
                if (er.IsHole)
                    er.Shell = shell;
            }
        }

        /// <summary>
        /// For all rings in the input list,
        /// determine whether the ring is a shell or a hole
        /// and add it to the appropriate list.
        /// Due to the way the DirectedEdges were linked,
        /// a ring is a shell if it is oriented CW, a hole otherwise.
        /// </summary>
        /// <param name="edgeRings"></param>
        /// <param name="shellList"></param>
        /// <param name="freeHoleList"></param>
        private static void SortShellsAndHoles(IEnumerable edgeRings, IList shellList, IList freeHoleList)
        {
            for (IEnumerator it = edgeRings.GetEnumerator(); it.MoveNext(); )
            {
                EdgeRing er = (EdgeRing)it.Current;
                er.SetInResult();
                if (er.IsHole)
                    freeHoleList.Add(er);
                else shellList.Add(er);
            }
        }

        /// <summary>
        /// This method determines finds a containing shell for all holes
        /// which have not yet been assigned to a shell.
        /// These "free" holes should
        /// all be properly contained in their parent shells, so it is safe to use the
        /// <c>findEdgeRingContaining</c> method.
        /// (This is the case because any holes which are NOT
        /// properly contained (i.e. are connected to their
        /// parent shell) would have formed part of a MaximalEdgeRing
        /// and been handled in a previous step).
        /// </summary>
        /// <param name="shellList"></param>
        /// <param name="freeHoleList"></param>
        private static void PlaceFreeHoles(IEnumerable shellList, IEnumerable freeHoleList)
        {
            for (IEnumerator it = freeHoleList.GetEnumerator(); it.MoveNext(); )
            {
                EdgeRing hole = (EdgeRing)it.Current;
                // only place this hole if it doesn't yet have a shell
                if (hole.Shell == null)
                {
                    EdgeRing shell = FindEdgeRingContaining(hole, shellList);
                    Assert.IsTrue(shell != null, "unable to assign hole to a shell");
                    hole.Shell = shell;
                }
            }
        }

        /// <summary>
        /// Find the innermost enclosing shell EdgeRing containing the argument EdgeRing, if any.
        /// The innermost enclosing ring is the <i>smallest</i> enclosing ring.
        /// The algorithm used depends on the fact that:
        /// ring A contains ring B iff envelope(ring A) contains envelope(ring B).
        /// This routine is only safe to use if the chosen point of the hole
        /// is known to be properly contained in a shell
        /// (which is guaranteed to be the case if the hole does not touch its shell).
        /// </summary>
        /// <param name="testEr"></param>
        /// <param name="shellList"></param>
        /// <returns>Containing EdgeRing, if there is one, OR
        /// null if no containing EdgeRing is found.</returns>
        private static EdgeRing FindEdgeRingContaining(EdgeRing testEr, IEnumerable shellList)
        {
            ILinearRing teString = testEr.LinearRing;
            IEnvelope testEnv = teString.EnvelopeInternal;
            Coordinate testPt = teString.Coordinates[0];

            EdgeRing minShell = null;
            IEnvelope minEnv = null;
            for (IEnumerator it = shellList.GetEnumerator(); it.MoveNext(); )
            {
                EdgeRing tryShell = (EdgeRing)it.Current;
                ILinearRing tryRing = tryShell.LinearRing;
                IEnvelope tryEnv = tryRing.EnvelopeInternal;
                if (minShell != null)
                    minEnv = minShell.LinearRing.EnvelopeInternal;
                bool isContained = false;
                if (tryEnv.Contains(testEnv) && CgAlgorithms.IsPointInRing(testPt, tryRing.Coordinates))
                    isContained = true;
                // check if this new containing ring is smaller than the current minimum ring
                if (isContained)
                {
                    if (minShell == null || minEnv.Contains(tryEnv))
                        minShell = tryShell;
                }
            }
            return minShell;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="shellList"></param>
        /// <returns></returns>
        private IList ComputePolygons(IEnumerable shellList)
        {
            IList resultPolyList = new ArrayList();
            // add Polygons for all shells
            for (IEnumerator it = shellList.GetEnumerator(); it.MoveNext(); )
            {
                EdgeRing er = (EdgeRing)it.Current;
                IPolygon poly = er.ToPolygon(_geometryFactory);
                resultPolyList.Add(poly);
            }
            return resultPolyList;
        }

        /// <summary>
        /// Checks the current set of shells (with their associated holes) to
        /// see if any of them contain the point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool ContainsPoint(Coordinate p)
        {
            for (IEnumerator it = _shellList.GetEnumerator(); it.MoveNext(); )
            {
                EdgeRing er = (EdgeRing)it.Current;
                if (er.ContainsPoint(p))
                    return true;
            }
            return false;
        }
    }
}