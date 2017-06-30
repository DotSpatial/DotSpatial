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
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Operation.Valid
{
    /// <summary>
    /// Implements the algorithsm required to compute the <see cref="Geometry.IsValid" />
    /// method for <see cref="Geometry" />s.
    /// See the documentation for the various geometry types for a specification of validity.
    /// </summary>
    public class IsValidOp
    {
        private readonly Geometry _parentGeometry;  // the base Geometry to be validated

        /**
         * If the following condition is TRUE JTS will validate inverted shells and exverted holes (the Esri SDE model).
         */
        private bool _isSelfTouchingRingFormingHoleValid;
        private TopologyValidationError _validErr;

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentGeometry"></param>
        public IsValidOp(Geometry parentGeometry)
        {
            _parentGeometry = parentGeometry;
        }

        /// <summary>
        /// <para>
        /// Gets/Sets whether polygons using Self-Touching Rings to form
        /// holes are reported as valid.
        /// If this flag is set, the following Self-Touching conditions
        /// are treated as being valid:
        /// - The shell ring self-touches to create a hole touching the shell.
        /// - A hole ring self-touches to create two holes touching at a point.
        /// </para>
        /// <para>
        /// The default (following the OGC SFS standard)
        /// is that this condition is not valid (<c>false</c>).
        /// </para>
        /// <para>
        /// This does not affect whether Self-Touching Rings
        /// disconnecting the polygon interior are considered valid
        /// (these are considered to be invalid under the SFS, and many other
        /// spatial models as well).
        /// This includes "bow-tie" shells,
        /// which self-touch at a single point causing the interior to be disconnected,
        /// and "C-shaped" holes which self-touch at a single point causing an island to be formed.
        /// </para>
        /// </summary>
        /// <value>States whether geometry with this condition is valid.</value>
        public bool IsSelfTouchingRingFormingHoleValid
        {
            get
            {
                return _isSelfTouchingRingFormingHoleValid;
            }
            set
            {
                _isSelfTouchingRingFormingHoleValid = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                CheckValid(_parentGeometry);
                return _validErr == null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual TopologyValidationError ValidationError
        {
            get
            {
                CheckValid(_parentGeometry);
                return _validErr;
            }
        }

        /// <summary>
        /// Checks whether a coordinate is valid for processing.
        /// Coordinates are valid iff their x and y ordinates are in the
        /// range of the floating point representation.
        /// </summary>
        /// <param name="coord">The coordinate to validate.</param>
        /// <returns><c>true</c> if the coordinate is valid.</returns>
        public static bool IsValidCoordinate(Coordinate coord)
        {
            if (Double.IsNaN(coord.X))
                return false;
            if (Double.IsInfinity(coord.X))
                return false;
            if (Double.IsNaN(coord.Y))
                return false;
            if (Double.IsInfinity(coord.Y))
                return false;
            return true;
        }

        /// <summary>
        /// Find a point from the list of testCoords
        /// that is NOT a node in the edge for the list of searchCoords.
        /// </summary>
        /// <param name="testCoords"></param>
        /// <param name="searchRing"></param>
        /// <param name="graph"></param>
        /// <returns>The point found, or <c>null</c> if none found.</returns>
        public static Coordinate FindPointNotNode(IList<Coordinate> testCoords, ILinearRing searchRing, GeometryGraph graph)
        {
            // find edge corresponding to searchRing.
            Edge searchEdge = graph.FindEdge(searchRing);
            // find a point in the testCoords which is not a node of the searchRing
            EdgeIntersectionList eiList = searchEdge.EdgeIntersectionList;
            // somewhat inefficient - is there a better way? (Use a node map, for instance?)
            foreach (Coordinate pt in testCoords)
                if (!eiList.IsIntersection(pt))
                    return pt;
            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        private void CheckValid(IGeometry g)
        {
            _validErr = null;

            if (g.IsEmpty) return;
            CheckValidCoordinates(g);
            if (g is ILineString)
                CheckValidLineString(g);
            ILinearRing r = g as ILinearRing;
            if (r != null) CheckValidRing(r);
            IPolygon p = g as IPolygon;
            if (p != null) CheckValidPolygon(p);
            IMultiPolygon mp = g as IMultiPolygon;
            if (mp != null) CheckValidMultipolygon(mp);
            else
            {
                IGeometryCollection gc = g as IGeometryCollection;
                if (gc != null) CheckValidCollection(gc);
            }
        }

        /// <summary>
        /// Checks validity of a Point.
        /// </summary>
        /// <param name="g"></param>
        private void CheckValidCoordinates(IBasicGeometry g)
        {
            CheckInvalidCoordinates(g.Coordinates);
        }

        /// <summary>
        /// Checks validity of a LineString.
        /// Almost anything goes for lineStrings!
        /// </summary>
        /// <param name="g"></param>
        private void CheckValidLineString(IGeometry g)
        {
            GeometryGraph graph = new GeometryGraph(0, g);
            CheckTooFewPoints(graph);
        }

        /// <summary>
        /// Checks validity of a LinearRing.
        /// </summary>
        /// <param name="g"></param>
        private void CheckValidRing(ILinearRing g)
        {
            CheckClosedRing(g);
            if (_validErr != null) return;
            GeometryGraph graph = new GeometryGraph(0, g);
            LineIntersector li = new RobustLineIntersector();
            graph.ComputeSelfNodes(li, true);
            CheckNoSelfIntersectingRings(graph);
        }

        /// <summary>
        /// Checks the validity of a polygon and sets the validErr flag.
        /// </summary>
        /// <param name="g"></param>
        private void CheckValidPolygon(IPolygon g)
        {
            CheckClosedRings(g);
            if (_validErr != null) return;

            GeometryGraph graph = new GeometryGraph(0, g);
            CheckConsistentArea(graph);
            if (_validErr != null) return;
            if (!IsSelfTouchingRingFormingHoleValid)
            {
                CheckNoSelfIntersectingRings(graph);
                if (_validErr != null) return;
            }
            CheckHolesInShell(g, graph);
            if (_validErr != null) return;
            CheckHolesNotNested(g, graph);
            if (_validErr != null) return;
            CheckConnectedInteriors(graph);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        private void CheckValidMultipolygon(IMultiPolygon g)
        {
            foreach (Polygon p in g.Geometries)
            {
                CheckInvalidCoordinates(p);
                if (_validErr != null) return;
                CheckClosedRings(p);
                if (_validErr != null) return;
            }

            GeometryGraph graph = new GeometryGraph(0, g);
            CheckTooFewPoints(graph);
            if (_validErr != null) return;
            CheckConsistentArea(graph);
            if (_validErr != null) return;
            if (!IsSelfTouchingRingFormingHoleValid)
            {
                CheckNoSelfIntersectingRings(graph);
                if (_validErr != null) return;
            }
            foreach (Polygon p in g.Geometries)
            {
                CheckHolesInShell(p, graph);
                if (_validErr != null) return;
            }
            foreach (Polygon p in g.Geometries)
            {
                CheckHolesNotNested(p, graph);
                if (_validErr != null) return;
            }
            CheckShellsNotNested(g, graph);
            if (_validErr != null) return;
            CheckConnectedInteriors(graph);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gc"></param>
        private void CheckValidCollection(IGeometryCollection gc)
        {
            foreach (Geometry g in gc.Geometries)
            {
                CheckValid(g);
                if (_validErr != null) return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coords"></param>
        private void CheckInvalidCoordinates(IEnumerable<Coordinate> coords)
        {
            foreach (Coordinate c in coords)
            {
                if (!IsValidCoordinate(c))
                {
                    _validErr = new TopologyValidationError(TopologyValidationErrorType.InvalidCoordinate, c);
                    return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="poly"></param>
        private void CheckInvalidCoordinates(IPolygon poly)
        {
            CheckInvalidCoordinates(poly.Shell.Coordinates);
            if (_validErr != null) return;
            foreach (LineString ls in poly.Holes)
            {
                CheckInvalidCoordinates(ls.Coordinates);
                if (_validErr != null) return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="poly"></param>
        private void CheckClosedRings(IPolygon poly)
        {
            CheckClosedRing(poly.Shell);
            if (_validErr != null) return;
            foreach (LineString ls in poly.Holes)
            {
                CheckClosedRing((LinearRing)ls);
                if (_validErr != null) return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ring"></param>
        private void CheckClosedRing(ILinearRing ring)
        {
            if (!ring.IsClosed)
                _validErr = new TopologyValidationError(TopologyValidationErrorType.RingNotClosed, ring.Coordinates[0]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="graph"></param>
        private void CheckTooFewPoints(GeometryGraph graph)
        {
            if (graph.HasTooFewPoints)
            {
                _validErr = new TopologyValidationError(TopologyValidationErrorType.TooFewPoints, graph.InvalidPoint);
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="graph"></param>
        private void CheckConsistentArea(GeometryGraph graph)
        {
            ConsistentAreaTester cat = new ConsistentAreaTester(graph);
            bool isValidArea = cat.IsNodeConsistentArea;
            if (!isValidArea)
            {
                _validErr = new TopologyValidationError(TopologyValidationErrorType.SelfIntersection, cat.InvalidPoint);
                return;
            }
            if (cat.HasDuplicateRings)
            {
                _validErr = new TopologyValidationError(TopologyValidationErrorType.DuplicateRings, cat.InvalidPoint);
                return;
            }
        }

        /// <summary>
        /// Check that there is no ring which self-intersects (except of course at its endpoints).
        /// This is required by OGC topology rules (but not by other models
        /// such as Esri SDE, which allow inverted shells and exverted holes).
        /// </summary>
        /// <param name="graph"></param>
        private void CheckNoSelfIntersectingRings(GeometryGraph graph)
        {
            for (IEnumerator i = graph.GetEdgeEnumerator(); i.MoveNext(); )
            {
                Edge e = (Edge)i.Current;
                CheckNoSelfIntersectingRing(e.EdgeIntersectionList);
                if (_validErr != null) return;
            }
        }

        /// <summary>
        /// Check that a ring does not self-intersect, except at its endpoints.
        /// Algorithm is to count the number of times each node along edge occurs.
        /// If any occur more than once, that must be a self-intersection.
        /// </summary>
        private void CheckNoSelfIntersectingRing(EdgeIntersectionList eiList)
        {
            var nodeSet = new HashSet<Coordinate>();
            bool isFirst = true;
            foreach (EdgeIntersection ei in eiList)
            {
                if (isFirst)
                {
                    isFirst = false;
                    continue;
                }
                if (nodeSet.Contains(ei.Coordinate))
                {
                    _validErr = new TopologyValidationError(TopologyValidationErrorType.RingSelfIntersection, ei.Coordinate);
                    return;
                }
                nodeSet.Add(ei.Coordinate);
            }
        }

        /// <summary>
        /// Tests that each hole is inside the polygon shell.
        /// This routine assumes that the holes have previously been tested
        /// to ensure that all vertices lie on the shell or inside it.
        /// A simple test of a single point in the hole can be used,
        /// provide the point is chosen such that it does not lie on the
        /// boundary of the shell.
        /// </summary>
        /// <param name="p">The polygon to be tested for hole inclusion.</param>
        /// <param name="graph">A GeometryGraph incorporating the polygon.</param>
        private void CheckHolesInShell(IPolygon p, GeometryGraph graph)
        {
            ILinearRing shell = p.Shell;

            IPointInRing pir = new McPointInRing(shell);
            for (int i = 0; i < p.NumHoles; i++)
            {
                LinearRing hole = (LinearRing)p.GetInteriorRingN(i);
                Coordinate holePt = FindPointNotNode(hole.Coordinates, shell, graph);

                /*
                 * If no non-node hole vertex can be found, the hole must
                 * split the polygon into disconnected interiors.
                 * This will be caught by a subsequent check.
                 */
                if (holePt == null)
                    return;

                bool outside = !pir.IsInside(holePt);
                if (outside)
                {
                    _validErr = new TopologyValidationError(TopologyValidationErrorType.HoleOutsideShell, holePt);
                    return;
                }
            }
        }

        /// <summary>
        /// Tests that no hole is nested inside another hole.
        /// This routine assumes that the holes are disjoint.
        /// To ensure this, holes have previously been tested
        /// to ensure that:
        /// They do not partially overlap
        /// (checked by <c>checkRelateConsistency</c>).
        /// They are not identical
        /// (checked by <c>checkRelateConsistency</c>).
        /// </summary>
        private void CheckHolesNotNested(IPolygon p, GeometryGraph graph)
        {
            QuadtreeNestedRingTester nestedTester = new QuadtreeNestedRingTester(graph);
            foreach (LinearRing innerHole in p.Holes)
                nestedTester.Add(innerHole);
            bool isNonNested = nestedTester.IsNonNested();
            if (!isNonNested)
                _validErr = new TopologyValidationError(TopologyValidationErrorType.NestedHoles, nestedTester.NestedPoint);
        }

        /// <summary>
        /// Tests that no element polygon is wholly in the interior of another element polygon.
        /// Preconditions:
        /// Shells do not partially overlap.
        /// Shells do not touch along an edge.
        /// No duplicate rings exists.
        /// This routine relies on the fact that while polygon shells may touch at one or
        /// more vertices, they cannot touch at ALL vertices.
        /// </summary>
        private void CheckShellsNotNested(IGeometry mp, GeometryGraph graph)
        {
            for (int i = 0; i < mp.NumGeometries; i++)
            {
                Polygon p = (Polygon)mp.GetGeometryN(i);
                LinearRing shell = (LinearRing)p.ExteriorRing;
                for (int j = 0; j < mp.NumGeometries; j++)
                {
                    if (i == j)
                        continue;
                    Polygon p2 = (Polygon)mp.GetGeometryN(j);
                    CheckShellNotNested(shell, p2, graph);
                    if (_validErr != null) return;
                }
            }
        }

        /// <summary>
        /// Check if a shell is incorrectly nested within a polygon.  This is the case
        /// if the shell is inside the polygon shell, but not inside a polygon hole.
        /// (If the shell is inside a polygon hole, the nesting is valid.)
        /// The algorithm used relies on the fact that the rings must be properly contained.
        /// E.g. they cannot partially overlap (this has been previously checked by
        /// <c>CheckRelateConsistency</c>).
        /// </summary>
        private void CheckShellNotNested(LinearRing shell, Polygon p, GeometryGraph graph)
        {
            IList<Coordinate> shellPts = shell.Coordinates;
            // test if shell is inside polygon shell
            LinearRing polyShell = (LinearRing)p.ExteriorRing;
            IList<Coordinate> polyPts = polyShell.Coordinates;
            Coordinate shellPt = FindPointNotNode(shellPts, polyShell, graph);
            // if no point could be found, we can assume that the shell is outside the polygon
            if (shellPt == null) return;
            bool insidePolyShell = CgAlgorithms.IsPointInRing(shellPt, polyPts);
            if (!insidePolyShell) return;
            // if no holes, this is an error!
            if (p.NumHoles <= 0)
            {
                _validErr = new TopologyValidationError(TopologyValidationErrorType.NestedShells, shellPt);
                return;
            }

            /*
             * Check if the shell is inside one of the holes.
             * This is the case if one of the calls to checkShellInsideHole
             * returns a null coordinate.
             * Otherwise, the shell is not properly contained in a hole, which is an error.
             */
            Coordinate badNestedPt = null;
            for (int i = 0; i < p.NumHoles; i++)
            {
                LinearRing hole = (LinearRing)p.GetInteriorRingN(i);
                badNestedPt = CheckShellInsideHole(shell, hole, graph);
                if (badNestedPt == null) return;
            }
            _validErr = new TopologyValidationError(TopologyValidationErrorType.NestedShells, badNestedPt);
        }

        /// <summary>
        /// This routine checks to see if a shell is properly contained in a hole.
        /// It assumes that the edges of the shell and hole do not
        /// properly intersect.
        /// </summary>
        /// <param name="shell"></param>
        /// <param name="hole"></param>
        /// <param name="graph"></param>
        /// <returns>
        /// <c>null</c> if the shell is properly contained, or
        /// a Coordinate which is not inside the hole if it is not.
        /// </returns>
        private static Coordinate CheckShellInsideHole(LinearRing shell, LinearRing hole, GeometryGraph graph)
        {
            IList<Coordinate> shellPts = shell.Coordinates;
            IList<Coordinate> holePts = hole.Coordinates;
            // TODO: improve performance of this - by sorting pointlists?
            Coordinate shellPt = FindPointNotNode(shellPts, hole, graph);
            // if point is on shell but not hole, check that the shell is inside the hole
            if (shellPt != null)
            {
                bool insideHole = CgAlgorithms.IsPointInRing(shellPt, holePts);
                if (!insideHole) return shellPt;
            }
            Coordinate holePt = FindPointNotNode(holePts, shell, graph);
            // if point is on hole but not shell, check that the hole is outside the shell
            if (holePt != null)
            {
                bool insideShell = CgAlgorithms.IsPointInRing(holePt, shellPts);
                return insideShell ? holePt : null;
            }
            throw new ShellHoleIdentityException();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="graph"></param>
        private void CheckConnectedInteriors(GeometryGraph graph)
        {
            ConnectedInteriorTester cit = new ConnectedInteriorTester(graph);
            if (!cit.IsInteriorsConnected())
                _validErr = new TopologyValidationError(TopologyValidationErrorType.DisconnectedInteriors, cit.Coordinate);
        }
    }
}