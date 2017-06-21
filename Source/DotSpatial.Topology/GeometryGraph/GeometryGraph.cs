// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://dotspatial.codeplex.com/license and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.GeometriesGraph.Index;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// A GeometryGraph is a graph that models a given Geometry.
    /// </summary>
    public class GeometryGraph : PlanarGraph
    {
        private readonly int _argIndex;  // the index of this point as an argument to a spatial function (used for labelling)

        /// <summary>
        /// The lineEdgeMap is a map of the linestring components of the
        /// parentGeometry to the edges which are derived from them.
        /// This is used to efficiently perform findEdge queries
        /// </summary>
        private readonly IDictionary _lineEdgeMap = new Hashtable();

        private readonly IGeometry _parentGeom;

        private ICollection _boundaryNodes;
        private bool _hasTooFewPoints;
        private Coordinate _invalidPoint;

        /// <summary>
        /// If this flag is true, the Boundary Determination Rule will used when deciding
        /// whether nodes are in the boundary or not
        /// </summary>
        private bool _useBoundaryDeterminationRule;

        /// <summary>
        ///
        /// </summary>
        /// <param name="argIndex"></param>
        /// <param name="parentGeom"></param>
        public GeometryGraph(int argIndex, IGeometry parentGeom)
        {
            _argIndex = argIndex;
            _parentGeom = parentGeom;
            if (parentGeom != null)
                Add(parentGeom);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool HasTooFewPoints
        {
            get
            {
                return _hasTooFewPoints;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate InvalidPoint
        {
            get
            {
                return _invalidPoint;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IGeometry Geometry
        {
            get
            {
                return _parentGeom;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual ICollection BoundaryNodes
        {
            get
            {
                if (_boundaryNodes == null)
                {
                    _boundaryNodes = Nodes.GetBoundaryNodes(_argIndex);
                }
                return _boundaryNodes;
            }
        }

        /// <summary>
        /// This method implements the Boundary Determination Rule
        /// for determining whether
        /// a component (node or edge) that appears multiple times in elements
        /// of a MultiGeometry is in the boundary or the interior of the Geometry.
        /// The SFS uses the "Mod-2 Rule", which this function implements.
        /// An alternative (and possibly more intuitive) rule would be
        /// the "At Most One Rule":
        /// isInBoundary = (componentCount == 1)
        /// </summary>
        public static bool IsInBoundary(int boundaryCount)
        {
            // the "Mod-2 Rule"
            return boundaryCount % 2 == 1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="boundaryCount"></param>
        /// <returns></returns>
        public static LocationType DetermineBoundary(int boundaryCount)
        {
            return IsInBoundary(boundaryCount) ? LocationType.Boundary : LocationType.Interior;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static EdgeSetIntersector CreateEdgeSetIntersector()
        {
            // various options for computing intersections, from slowest to fastest
            return new SimpleMcSweepLineIntersector();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual Coordinate[] GetBoundaryPoints()
        {
            ICollection coll = BoundaryNodes;
            Coordinate[] pts = new Coordinate[coll.Count];
            int i = 0;
            for (IEnumerator it = coll.GetEnumerator(); it.MoveNext(); )
            {
                Node node = (Node)it.Current;
                pts[i++] = (Coordinate)node.Coordinate.Clone();
            }
            return pts;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public virtual Edge FindEdge(ILineString line)
        {
            return (Edge)_lineEdgeMap[line];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edgelist"></param>
        public virtual void ComputeSplitEdges(IList edgelist)
        {
            for (IEnumerator i = Edges.GetEnumerator(); i.MoveNext(); )
            {
                Edge e = (Edge)i.Current;
                e.EdgeIntersectionList.AddSplitEdges(edgelist);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        private void Add(IGeometry g)
        {
            if (g.IsEmpty)
                return;

            // check if this Geometry should obey the Boundary Determination Rule
            // all collections except MultiPolygons obey the rule
            if (g is GeometryCollection && !(g is MultiPolygon))
                _useBoundaryDeterminationRule = true;

            if (g is Polygon)
                AddPolygon((Polygon)g);
            // LineString also handles LinearRings
            else if (g is LineString)
                AddLineString(g);
            else if (g is Point)
                AddPoint(g.Coordinate);
            else AddCollection(g);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gc"></param>
        private void AddCollection(IGeometry gc)
        {
            for (int i = 0; i < gc.NumGeometries; i++)
            {
                IGeometry g = gc.GetGeometryN(i);
                Add(g);
            }
        }

        /// <summary>
        /// The left and right topological location arguments assume that the ring is oriented CW.
        /// If the ring is in the opposite orientation,
        /// the left and right locations must be interchanged.
        /// </summary>
        /// <param name="lr"></param>
        /// <param name="cwLeft"></param>
        /// <param name="cwRight"></param>
        private void AddPolygonRing(IBasicGeometry lr, LocationType cwLeft, LocationType cwRight)
        {
            IList<Coordinate> coord = CoordinateArrays.RemoveRepeatedPoints(lr.Coordinates);
            if (coord.Count < 4)
            {
                _hasTooFewPoints = true;
                _invalidPoint = coord[0];
                return;
            }
            LocationType left = cwLeft;
            LocationType right = cwRight;
            if (CgAlgorithms.IsCounterClockwise(coord))
            {
                left = cwRight;
                right = cwLeft;
            }
            Edge e = new Edge(coord, new Label(_argIndex, LocationType.Boundary, left, right));
            _lineEdgeMap.Add(lr, e);
            InsertEdge(e);
            // insert the endpoint as a node, to mark that it is on the boundary
            InsertPoint(_argIndex, coord[0], LocationType.Boundary);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        private void AddPolygon(Polygon p)
        {
            AddPolygonRing(p.ExteriorRing, LocationType.Exterior, LocationType.Interior);

            for (int i = 0; i < p.NumHoles; i++)
                // Holes are topologically labelled opposite to the shell, since
                // the interior of the polygon lies on their opposite side
                // (on the left, if the hole is oriented CW)
                AddPolygonRing(p.GetInteriorRingN(i), LocationType.Interior, LocationType.Exterior);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        private void AddLineString(IBasicGeometry line)
        {
            IList<Coordinate> coord = CoordinateArrays.RemoveRepeatedPoints(line.Coordinates);

            if (coord.Count < 2)
            {
                _hasTooFewPoints = true;
                _invalidPoint = coord[0];
                return;
            }

            // add the edge for the LineString
            // line edges do not have locations for their left and right sides
            Edge e = new Edge(coord, new Label(_argIndex, LocationType.Interior));
            _lineEdgeMap.Add(line, e);
            InsertEdge(e);

            /*
            * Add the boundary points of the LineString, if any.
            * Even if the LineString is closed, add both points as if they were endpoints.
            * This allows for the case that the node already exists and is a boundary point.
            */
            Assert.IsTrue(coord.Count >= 2, "found LineString with single point");
            InsertBoundaryPoint(_argIndex, coord[0]);
            InsertBoundaryPoint(_argIndex, coord[coord.Count - 1]);
        }

        /// <summary>
        /// Add an Edge computed externally.  The label on the Edge is assumed
        /// to be correct.
        /// </summary>
        /// <param name="e"></param>
        public virtual void AddEdge(Edge e)
        {
            InsertEdge(e);
            IList<Coordinate> coord = e.Coordinates;
            // insert the endpoint as a node, to mark that it is on the boundary
            InsertPoint(_argIndex, coord[0], LocationType.Boundary);
            InsertPoint(_argIndex, coord[coord.Count - 1], LocationType.Boundary);
        }

        /// <summary>
        /// Add a point computed externally.  The point is assumed to be a
        /// Point Geometry part, which has a location of INTERIOR.
        /// </summary>
        /// <param name="pt"></param>
        public virtual void AddPoint(Coordinate pt)
        {
            InsertPoint(_argIndex, pt, LocationType.Interior);
        }

        /// <summary>
        /// Compute self-nodes, taking advantage of the Geometry type to
        /// minimize the number of intersection tests.  (E.g. rings are
        /// not tested for self-intersection, since they are assumed to be valid).
        /// </summary>
        /// <param name="li">The <c>LineIntersector</c> to use.</param>
        /// <param name="computeRingSelfNodes">If <c>false</c>, intersection checks are optimized to not test rings for self-intersection.</param>
        /// <returns>The SegmentIntersector used, containing information about the intersections found.</returns>
        public virtual SegmentIntersector ComputeSelfNodes(LineIntersector li, bool computeRingSelfNodes)
        {
            SegmentIntersector si = new SegmentIntersector(li, true, false);
            EdgeSetIntersector esi = CreateEdgeSetIntersector();
            // optimized test for Polygons and Rings
            if (!computeRingSelfNodes &&
               (_parentGeom is ILinearRing || _parentGeom is IPolygon || _parentGeom is IMultiPolygon))
                esi.ComputeIntersections(Edges, si, false);
            else esi.ComputeIntersections(Edges, si, true);
            AddSelfIntersectionNodes(_argIndex);
            return si;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        /// <param name="li"></param>
        /// <param name="includeProper"></param>
        /// <returns></returns>
        public virtual SegmentIntersector ComputeEdgeIntersections(GeometryGraph g, LineIntersector li, bool includeProper)
        {
            SegmentIntersector si = new SegmentIntersector(li, includeProper, true);
            si.SetBoundaryNodes(BoundaryNodes, g.BoundaryNodes);
            EdgeSetIntersector esi = CreateEdgeSetIntersector();
            esi.ComputeIntersections(Edges, g.Edges, si);
            return si;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="argIndex"></param>
        /// <param name="coord"></param>
        /// <param name="onLocation"></param>
        private void InsertPoint(int argIndex, Coordinate coord, LocationType onLocation)
        {
            Node n = Nodes.AddNode(coord);
            Label lbl = n.Label;
            if (lbl == null)
                n.Label = new Label(argIndex, onLocation);
            else lbl.SetLocation(argIndex, onLocation);
        }

        /// <summary>
        /// Adds points using the mod-2 rule of SFS.  This is used to add the boundary
        /// points of dim-1 geometries (Curves/MultiCurves).  According to the SFS,
        /// an endpoint of a Curve is on the boundary
        /// if it is in the boundaries of an odd number of Geometries.
        /// </summary>
        /// <param name="argIndex"></param>
        /// <param name="coord"></param>
        private void InsertBoundaryPoint(int argIndex, Coordinate coord)
        {
            Node n = Nodes.AddNode(coord);
            Label lbl = n.Label;
            // the new point to insert is on a boundary
            int boundaryCount = 1;
            // determine the current location for the point (if any)
            LocationType loc = LocationType.Null;
            if (lbl != null)
                loc = lbl.GetLocation(argIndex, PositionType.On);
            if (loc == LocationType.Boundary)
                boundaryCount++;

            // determine the boundary status of the point according to the Boundary Determination Rule
            LocationType newLoc = DetermineBoundary(boundaryCount);
            if (lbl != null) lbl.SetLocation(argIndex, newLoc);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="argIndex"></param>
        private void AddSelfIntersectionNodes(int argIndex)
        {
            for (IEnumerator i = Edges.GetEnumerator(); i.MoveNext(); )
            {
                Edge e = (Edge)i.Current;
                LocationType eLoc = e.Label.GetLocation(argIndex);
                for (IEnumerator eiIt = e.EdgeIntersectionList.GetEnumerator(); eiIt.MoveNext(); )
                {
                    EdgeIntersection ei = (EdgeIntersection)eiIt.Current;
                    AddSelfIntersectionNode(argIndex, ei.Coordinate, eLoc);
                }
            }
        }

        /// <summary>
        /// Add a node for a self-intersection.
        /// If the node is a potential boundary node (e.g. came from an edge which
        /// is a boundary) then insert it as a potential boundary node.
        /// Otherwise, just add it as a regular node.
        /// </summary>
        /// <param name="argIndex"></param>
        /// <param name="coord"></param>
        /// <param name="loc"></param>
        private void AddSelfIntersectionNode(int argIndex, Coordinate coord, LocationType loc)
        {
            // if this node is already a boundary node, don't change it
            if (IsBoundaryNode(argIndex, coord))
                return;
            if (loc == LocationType.Boundary && _useBoundaryDeterminationRule)
                InsertBoundaryPoint(argIndex, coord);
            else InsertPoint(argIndex, coord, loc);
        }
    }
}