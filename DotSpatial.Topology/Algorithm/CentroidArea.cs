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

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Computes the centroid of an area point.
    /// Algorithm:
    /// Based on the usual algorithm for calculating
    /// the centroid as a weighted sum of the centroids
    /// of a decomposition of the area into (possibly overlapping) triangles.
    /// The algorithm has been extended to handle holes and multi-polygons.
    /// See <see href="http://www.faqs.org/faqs/graphics/algorithms-faq"/>
    /// for further details of the basic approach.
    /// </summary>
    public class CentroidArea
    {
        private readonly Coordinate _cg3 = new Coordinate(0, 0, 0, 0);              // partial centroid sum
        private double _areasum2;                            // Partial area sum
        private Coordinate _basePt;                       // the point all triangles are based at

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate Centroid
        {
            get
            {
                Coordinate cent = new Coordinate();
                cent.X = _cg3.X / 3 / _areasum2;
                cent.Y = _cg3.Y / 3 / _areasum2;
                return cent;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private Coordinate BasePoint
        {
            set
            {
                _basePt = value;
            }
        }

        /// <summary>
        /// Adds the area defined by a Geometry to the centroid total.
        /// If the point has no area it does not contribute to the centroid.
        /// </summary>
        /// <param name="geom">The point to add.</param>
        public virtual void Add(IGeometry geom)
        {
            if (geom is Polygon)
            {
                Polygon poly = geom as Polygon;
                BasePoint = poly.Shell.Coordinates[0];
                Add(poly);
            }
            else if (geom is GeometryCollection)
            {
                GeometryCollection gc = geom as GeometryCollection;
                foreach (Geometry geometry in gc.Geometries)
                    Add(geometry);
            }
        }

        /// <summary>
        /// Adds the area defined by an array of
        /// coordinates.  The array must be a ring;
        /// i.e. end with the same coordinate as it starts with.
        /// </summary>
        /// <param name="ring">An array of Coordinates.</param>
        public virtual void Add(Coordinate[] ring)
        {
            BasePoint = ring[0];
            AddShell(ring);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="poly"></param>
        private void Add(IPolygon poly)
        {
            AddShell(poly.Shell.Coordinates);

            foreach (LineString ls in poly.Holes)
                AddHole(ls.Coordinates);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        private void AddShell(IList<Coordinate> pts)
        {
            bool isPositiveArea = !CgAlgorithms.IsCounterClockwise(pts);
            for (int i = 0; i < pts.Count - 1; i++)
                AddTriangle(_basePt, pts[i], pts[i + 1], isPositiveArea);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        private void AddHole(IList<Coordinate> pts)
        {
            bool isPositiveArea = CgAlgorithms.IsCounterClockwise(pts);
            for (int i = 0; i < pts.Count - 1; i++)
                AddTriangle(_basePt, pts[i], pts[i + 1], isPositiveArea);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="isPositiveArea"></param>
        private void AddTriangle(Coordinate p0, Coordinate p1, Coordinate p2, bool isPositiveArea)
        {
            double sign = (isPositiveArea) ? 1.0 : -1.0;
            Coordinate triangleCent3;    // temporary variable to hold centroid of triangle
            Centroid3(p0, p1, p2, out triangleCent3);
            double area2 = Area2(p0, p1, p2);
            _cg3.X += sign * area2 * triangleCent3.X;
            _cg3.Y += sign * area2 * triangleCent3.Y;
            _areasum2 += sign * area2;
        }

        /// <summary>
        /// Returns three times the centroid of the triangle p1-p2-p3.
        /// The factor of 3 is
        /// left in to permit division to be avoided until later.
        /// </summary>
        private static void Centroid3(Coordinate p1, Coordinate p2, Coordinate p3, out Coordinate c)
        {
            c = new Coordinate();
            c.X = p1.X + p2.X + p3.X;
            c.Y = p1.Y + p2.Y + p3.Y;
            return;
        }

        /// <summary>
        /// Returns twice the signed area of the triangle p1-p2-p3,
        /// positive if a, b, c are oriented Ccw, and negative if cw.
        /// </summary>
        private static double Area2(Coordinate p1, Coordinate p2, Coordinate p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y);
        }
    }
}