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
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Operation.Distance
{
    /// <summary>
    /// Computes the distance and
    /// closest points between two <c>Geometry</c>s.
    /// The distance computation finds a pair of points in the input geometries
    /// which have minimum distance between them.  These points may
    /// not be vertices of the geometries, but may lie in the interior of
    /// a line segment. In this case the coordinate computed is a close
    /// approximation to the exact point.
    /// The algorithms used are straightforward O(n^2)
    /// comparisons.  This worst-case performance could be improved on
    /// by using Voronoi techniques.
    /// </summary>
    public class DistanceOp
    {
        private readonly IGeometry[] _geom;
        private readonly PointLocator _ptLocator = new PointLocator();
        private readonly double _terminateDistance;
        private double _minDistance = Double.MaxValue;
        private GeometryLocation[] _minDistanceLocation;

        /// <summary>
        /// Constructs a <see cref="DistanceOp" />  that computes the distance and closest points between
        /// the two specified geometries.
        /// </summary>
        /// <param name="g0"></param>
        /// <param name="g1"></param>
        private DistanceOp(IGeometry g0, IGeometry g1)
            : this(g0, g1, 0) { }

        /// <summary>
        /// Constructs a <see cref="DistanceOp" /> that computes the distance and closest points between
        /// the two specified geometries.
        /// </summary>
        /// <param name="g0"></param>
        /// <param name="g1"></param>
        /// <param name="terminateDistance">The distance on which to terminate the search.</param>
        public DistanceOp(IGeometry g0, IGeometry g1, double terminateDistance)
        {
            _geom = new Geometry[2];
            _geom[0] = g0;
            _geom[1] = g1;
            _terminateDistance = terminateDistance;
        }

        /// <summary>
        /// Compute the distance between the closest points of two geometries.
        /// </summary>
        /// <param name="g0">A <c>Geometry</c>.</param>
        /// <param name="g1">Another <c>Geometry</c>.</param>
        /// <returns>The distance between the geometries.</returns>
        public static double Distance(IGeometry g0, IGeometry g1)
        {
            DistanceOp distOp = new DistanceOp(g0, g1);
            return distOp.Distance();
        }

        /// <summary>
        /// Test whether two geometries lie within a given distance of each other.
        /// </summary>
        /// <param name="g0"></param>
        /// <param name="g1"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static bool IsWithinDistance(IGeometry g0, IGeometry g1, double distance)
        {
            DistanceOp distOp = new DistanceOp(g0, g1, distance);
            return distOp.Distance() <= distance;
        }

        /// <summary>
        /// Compute the the closest points of two geometries.
        /// The points are presented in the same order as the input Geometries.
        /// </summary>
        /// <param name="g0">A <c>Geometry</c>.</param>
        /// <param name="g1">Another <c>Geometry</c>.</param>
        /// <returns>The closest points in the geometries.</returns>
        public static Coordinate[] ClosestPoints(IGeometry g0, IGeometry g1)
        {
            DistanceOp distOp = new DistanceOp(g0, g1);
            return distOp.ClosestPoints();
        }

        /// <summary>
        /// Report the distance between the closest points on the input geometries.
        /// </summary>
        /// <returns>The distance between the geometries.</returns>
        public virtual double Distance()
        {
            ComputeMinDistance();
            return _minDistance;
        }

        /// <summary>
        /// Report the coordinates of the closest points in the input geometries.
        /// The points are presented in the same order as the input Geometries.
        /// </summary>
        /// <returns>A pair of <c>Coordinate</c>s of the closest points.</returns>
        public virtual Coordinate[] ClosestPoints()
        {
            ComputeMinDistance();
            Coordinate[] closestPts = new[] { _minDistanceLocation[0].Coordinate,
                                                         _minDistanceLocation[1].Coordinate };
            return closestPts;
        }

        /// <summary>
        /// Report the locations of the closest points in the input geometries.
        /// The locations are presented in the same order as the input Geometries.
        /// </summary>
        /// <returns>A pair of {GeometryLocation}s for the closest points.</returns>
        public virtual GeometryLocation[] ClosestLocations()
        {
            ComputeMinDistance();
            return _minDistanceLocation;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="locGeom"></param>
        /// <param name="flip"></param>
        private void UpdateMinDistance(GeometryLocation[] locGeom, bool flip)
        {
            // if not set then don't update
            if (locGeom[0] == null)
                return;
            if (flip)
            {
                _minDistanceLocation[0] = locGeom[1];
                _minDistanceLocation[1] = locGeom[0];
            }
            else
            {
                _minDistanceLocation[0] = locGeom[0];
                _minDistanceLocation[1] = locGeom[1];
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ComputeMinDistance()
        {
            if (_minDistanceLocation != null)
                return;
            _minDistanceLocation = new GeometryLocation[2];
            ComputeContainmentDistance();
            if (_minDistance <= _terminateDistance)
                return;
            ComputeLineDistance();
        }

        /// <summary>
        ///
        /// </summary>
        private void ComputeContainmentDistance()
        {
            IList polys0 = PolygonExtracter.GetPolygons(_geom[0]);
            IList polys1 = PolygonExtracter.GetPolygons(_geom[1]);

            GeometryLocation[] locPtPoly = new GeometryLocation[2];
            // test if either point is wholely inside the other
            if (polys1.Count > 0)
            {
                IList insideLocs0 = ConnectedElementLocationFilter.GetLocations(_geom[0]);
                ComputeInside(insideLocs0, polys1, locPtPoly);
                if (_minDistance <= _terminateDistance)
                {
                    _minDistanceLocation[0] = locPtPoly[0];
                    _minDistanceLocation[1] = locPtPoly[1];
                    return;
                }
            }
            if (polys0.Count > 0)
            {
                IList insideLocs1 = ConnectedElementLocationFilter.GetLocations(_geom[1]);
                ComputeInside(insideLocs1, polys0, locPtPoly);
                if (_minDistance <= _terminateDistance)
                {
                    // flip locations, since we are testing geom 1 VS geom 0
                    _minDistanceLocation[0] = locPtPoly[1];
                    _minDistanceLocation[1] = locPtPoly[0];
                    return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="locs"></param>
        /// <param name="polys"></param>
        /// <param name="locPtPoly"></param>
        private void ComputeInside(IList locs, IList polys, GeometryLocation[] locPtPoly)
        {
            for (int i = 0; i < locs.Count; i++)
            {
                GeometryLocation loc = (GeometryLocation)locs[i];
                for (int j = 0; j < polys.Count; j++)
                {
                    Polygon poly = (Polygon)polys[j];
                    ComputeInside(loc, poly, locPtPoly);
                    if (_minDistance <= _terminateDistance)
                        return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ptLoc"></param>
        /// <param name="poly"></param>
        /// <param name="locPtPoly"></param>
        private void ComputeInside(GeometryLocation ptLoc, IGeometry poly, GeometryLocation[] locPtPoly)
        {
            Coordinate pt = ptLoc.Coordinate;
            if (LocationType.Exterior == _ptLocator.Locate(pt, poly)) return;
            _minDistance = 0.0;
            locPtPoly[0] = ptLoc;
            GeometryLocation locPoly = new GeometryLocation(poly, pt);
            locPtPoly[1] = locPoly;
            return;
        }

        /// <summary>
        ///
        /// </summary>
        private void ComputeLineDistance()
        {
            GeometryLocation[] locGeom = new GeometryLocation[2];

            /*
             * Geometries are not wholely inside, so compute distance from lines and points
             * of one to lines and points of the other
             */
            IList lines0 = LinearComponentExtracter.GetLines(_geom[0]);
            IList lines1 = LinearComponentExtracter.GetLines(_geom[1]);

            IList pts0 = PointExtracter.GetPoints(_geom[0]);
            IList pts1 = PointExtracter.GetPoints(_geom[1]);

            // bail whenever minDistance goes to zero, since it can't get any less
            ComputeMinDistanceLines(lines0, lines1, locGeom);
            UpdateMinDistance(locGeom, false);
            if (_minDistance <= _terminateDistance) return;

            locGeom[0] = null;
            locGeom[1] = null;
            ComputeMinDistanceLinesPoints(lines0, pts1, locGeom);
            UpdateMinDistance(locGeom, false);
            if (_minDistance <= _terminateDistance) return;

            locGeom[0] = null;
            locGeom[1] = null;
            ComputeMinDistanceLinesPoints(lines1, pts0, locGeom);
            UpdateMinDistance(locGeom, true);
            if (_minDistance <= _terminateDistance) return;

            locGeom[0] = null;
            locGeom[1] = null;
            ComputeMinDistancePoints(pts0, pts1, locGeom);
            UpdateMinDistance(locGeom, false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lines0"></param>
        /// <param name="lines1"></param>
        /// <param name="locGeom"></param>
        private void ComputeMinDistanceLines(IList lines0, IList lines1, GeometryLocation[] locGeom)
        {
            for (int i = 0; i < lines0.Count; i++)
            {
                LineString line0 = (LineString)lines0[i];
                for (int j = 0; j < lines1.Count; j++)
                {
                    LineString line1 = (LineString)lines1[j];
                    ComputeMinDistance(line0, line1, locGeom);
                    if (_minDistance <= _terminateDistance) return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="points0"></param>
        /// <param name="points1"></param>
        /// <param name="locGeom"></param>
        private void ComputeMinDistancePoints(IList points0, IList points1, GeometryLocation[] locGeom)
        {
            for (int i = 0; i < points0.Count; i++)
            {
                Point pt0 = (Point)points0[i];
                for (int j = 0; j < points1.Count; j++)
                {
                    Point pt1 = (Point)points1[j];
                    double dist = pt0.Coordinate.Distance(pt1.Coordinate);
                    if (dist < _minDistance)
                    {
                        _minDistance = dist;
                        // this is wrong - need to determine closest points on both segments!!!
                        locGeom[0] = new GeometryLocation(pt0, 0, pt0.Coordinate);
                        locGeom[1] = new GeometryLocation(pt1, 0, pt1.Coordinate);
                    }
                    if (_minDistance <= _terminateDistance) return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="points"></param>
        /// <param name="locGeom"></param>
        private void ComputeMinDistanceLinesPoints(IList lines, IList points, GeometryLocation[] locGeom)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                LineString line = (LineString)lines[i];
                for (int j = 0; j < points.Count; j++)
                {
                    Point pt = (Point)points[j];
                    ComputeMinDistance(line, pt, locGeom);
                    if (_minDistance <= _terminateDistance) return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="line0"></param>
        /// <param name="line1"></param>
        /// <param name="locGeom"></param>
        private void ComputeMinDistance(ILineString line0, ILineString line1, GeometryLocation[] locGeom)
        {
            if (line0.EnvelopeInternal.Distance(line1.EnvelopeInternal) > _minDistance) return;
            IList<Coordinate> coord0 = line0.Coordinates;
            IList<Coordinate> coord1 = line1.Coordinates;
            // brute force approach!
            for (int i = 0; i < coord0.Count - 1; i++)
            {
                for (int j = 0; j < coord1.Count - 1; j++)
                {
                    double dist = CgAlgorithms.DistanceLineLine(
                                                    coord0[i], coord0[i + 1],
                                                    coord1[j], coord1[j + 1]);
                    if (dist < _minDistance)
                    {
                        _minDistance = dist;
                        LineSegment seg0 = new LineSegment(coord0[i], coord0[i + 1]);
                        LineSegment seg1 = new LineSegment(coord1[j], coord1[j + 1]);
                        Coordinate[] closestPt = seg0.ClosestPoints(seg1);
                        locGeom[0] = new GeometryLocation(line0, i, new Coordinate(closestPt[0]));
                        locGeom[1] = new GeometryLocation(line1, j, new Coordinate(closestPt[1]));
                    }
                    if (_minDistance <= _terminateDistance) return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        /// <param name="pt"></param>
        /// <param name="locGeom"></param>
        private void ComputeMinDistance(ILineString line, Point pt, GeometryLocation[] locGeom)
        {
            if (line.EnvelopeInternal.Distance(pt.EnvelopeInternal) > _minDistance) return;
            IList<Coordinate> coord0 = line.Coordinates;
            Coordinate coord = pt.Coordinate;
            // brute force approach!
            for (int i = 0; i < coord0.Count - 1; i++)
            {
                double dist = CgAlgorithms.DistancePointLine(coord, coord0[i], coord0[i + 1]);
                if (dist < _minDistance)
                {
                    _minDistance = dist;
                    LineSegment seg = new LineSegment(coord0[i], coord0[i + 1]);
                    Coordinate segClosestPoint = new Coordinate(seg.ClosestPoint(coord));
                    locGeom[0] = new GeometryLocation(line, i, segClosestPoint);
                    locGeom[1] = new GeometryLocation(pt, 0, coord);
                }
                if (_minDistance <= _terminateDistance) return;
            }
        }
    }
}