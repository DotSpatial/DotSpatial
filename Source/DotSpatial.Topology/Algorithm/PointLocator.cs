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

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Computes the topological relationship (Location) of a single point to a Geometry.
    /// The algorithm obeys the SFS boundaryDetermination rule to correctly determine
    /// whether the point lies on the boundary or not.
    /// Notice that instances of this class are not reentrant.
    /// </summary>
    public class PointLocator
    {
        private bool _isIn;            // true if the point lies in or on any Geometry element
        private int _numBoundaries;    // the number of sub-elements whose boundaries the point lies in

        /// <summary>
        /// Convenience method to test a point for intersection with a Geometry
        /// </summary>
        /// <param name="p">The coordinate to test.</param>
        /// <param name="geom">The Geometry to test.</param>
        /// <returns><c>true</c> if the point is in the interior or boundary of the Geometry.</returns>
        public virtual bool Intersects(Coordinate p, IGeometry geom)
        {
            return Locate(p, geom) != LocationType.Exterior;
        }

        /// <summary>
        /// Computes the topological relationship ({Location}) of a single point to a Geometry.
        /// It handles both single-element and multi-element Geometries.
        /// The algorithm for multi-part Geometries takes into account the boundaryDetermination rule.
        /// </summary>
        /// <returns>The Location of the point relative to the input Geometry.</returns>
        public virtual LocationType Locate(Coordinate p, IGeometry geom)
        {
            if (geom.IsEmpty)
                return LocationType.Exterior;
            if (geom is ILineString)
                return LocateInLineString(p, (ILineString)geom);
            if (geom is IPolygon)
                return LocateInPolygon(p, (IPolygon)geom);

            _isIn = false;
            _numBoundaries = 0;
            ComputeLocation(p, geom);
            if (GeometryGraph.IsInBoundary(_numBoundaries))
                return LocationType.Boundary;
            if (_numBoundaries > 0 || _isIn)
                return LocationType.Interior;
            return LocationType.Exterior;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="geom"></param>
        private void ComputeLocation(Coordinate p, IGeometry geom)
        {
            if (geom is ILineString)
                UpdateLocationInfo(Locate(p, geom));
            else if (geom is IPolygon)
                UpdateLocationInfo(Locate(p, geom));
            else if (geom is IMultiLineString)
            {
                IMultiLineString ml = (IMultiLineString)geom;
                foreach (ILineString l in ml.Geometries)
                    UpdateLocationInfo(Locate(p, l));
            }
            else if (geom is IMultiPolygon)
            {
                IMultiPolygon mpoly = (IMultiPolygon)geom;
                foreach (IPolygon poly in mpoly.Geometries)
                    UpdateLocationInfo(Locate(p, poly));
            }
            else if (geom is IGeometryCollection)
            {
                IEnumerator geomi = new GeometryCollection.Enumerator((IGeometryCollection)geom);
                while (geomi.MoveNext())
                {
                    IGeometry g2 = (IGeometry)geomi.Current;
                    if (g2 != geom)
                        ComputeLocation(p, g2);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="loc"></param>
        private void UpdateLocationInfo(LocationType loc)
        {
            if (loc == LocationType.Interior)
                _isIn = true;
            if (loc == LocationType.Boundary)
                _numBoundaries++;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        private static LocationType LocateInLineString(Coordinate p, ILineString l)
        {
            IList<Coordinate> pt = l.Coordinates;
            if (!l.IsClosed)
                if (p.Equals(pt[0]) || p.Equals(pt[pt.Count - 1]))
                    return LocationType.Boundary;
            if (CgAlgorithms.IsOnLine(p, pt))
                return LocationType.Interior;
            return LocationType.Exterior;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ring"></param>
        /// <returns></returns>
        private static LocationType LocateInPolygonRing(Coordinate p, IBasicGeometry ring)
        {
            // can this test be folded into IsPointInRing?
            if (CgAlgorithms.IsOnLine(p, ring.Coordinates))
                return LocationType.Boundary;
            if (CgAlgorithms.IsPointInRing(p, ring.Coordinates))
                return LocationType.Interior;
            return LocationType.Exterior;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="poly"></param>
        /// <returns></returns>
        private static LocationType LocateInPolygon(Coordinate p, IPolygon poly)
        {
            if (poly.IsEmpty)
                return LocationType.Exterior;
            LinearRing shell = (LinearRing)poly.Shell;
            LocationType shellLoc = LocateInPolygonRing(p, shell);
            if (shellLoc == LocationType.Exterior)
                return LocationType.Exterior;
            if (shellLoc == LocationType.Boundary)
                return LocationType.Boundary;
            // now test if the point lies in or on the holes
            foreach (LinearRing hole in poly.Holes)
            {
                LocationType holeLoc = LocateInPolygonRing(p, hole);
                if (holeLoc == LocationType.Interior)
                    return LocationType.Exterior;
                if (holeLoc == LocationType.Boundary)
                    return LocationType.Boundary;
            }
            return LocationType.Interior;
        }
    }
}