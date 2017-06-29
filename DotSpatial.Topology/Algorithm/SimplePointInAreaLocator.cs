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

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Computes whether a point
    /// lies in the interior of an area <c>Geometry</c>.
    /// The algorithm used is only guaranteed to return correct results
    /// for points which are not on the boundary of the Geometry.
    /// </summary>
    public class SimplePointInAreaLocator
    {
        /// <summary>
        ///
        /// </summary>
        private SimplePointInAreaLocator() { }

        /// <summary>
        /// Locate is the main location function.  It handles both single-element
        /// and multi-element Geometries.  The algorithm for multi-element Geometries
        /// is more complex, since it has to take into account the boundaryDetermination rule.
        /// </summary>
        /// <param name="p">The coordinate to locate.</param>
        /// <param name="geom">The Geometry to locate the coordinate in.</param>
        public static LocationType Locate(Coordinate p, IGeometry geom)
        {
            if (geom.IsEmpty)
                return LocationType.Exterior;

            if (ContainsPoint(p, geom))
                return LocationType.Interior;
            return LocationType.Exterior;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="geom"></param>
        /// <returns></returns>
        private static bool ContainsPoint(Coordinate p, IGeometry geom)
        {
            if (geom is Polygon)
                return ContainsPointInPolygon(p, (Polygon)geom);
            if (geom is GeometryCollection)
            {
                IEnumerator geomi = new GeometryCollection.Enumerator((GeometryCollection)geom);
                while (geomi.MoveNext())
                {
                    Geometry g2 = (Geometry)geomi.Current;
                    // if (g2 != geom)  --- Diego Guidi say's: Java code tests reference equality: in C# with operator overloads we tests the object.equals()... more slower!
                    if (!ReferenceEquals(g2, geom))
                        if (ContainsPoint(p, g2))
                            return true;
                }
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="poly"></param>
        /// <returns></returns>
        public static bool ContainsPointInPolygon(Coordinate p, IPolygon poly)
        {
            if (poly.IsEmpty)
                return false;
            LinearRing shell = (LinearRing)poly.Shell;
            if (!CgAlgorithms.IsPointInRing(p, shell.Coordinates))
                return false;
            // now test if the point lies in or on the holes
            for (int i = 0; i < poly.NumHoles; i++)
            {
                LinearRing hole = (LinearRing)poly.GetInteriorRingN(i);
                if (CgAlgorithms.IsPointInRing(p, hole.Coordinates))
                    return false;
            }
            return true;
        }
    }
}