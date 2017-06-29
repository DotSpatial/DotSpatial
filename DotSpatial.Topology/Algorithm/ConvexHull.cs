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
using System.Collections.Generic;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Computes the convex hull of a <see cref="Geometry" />.
    /// The convex hull is the smallest convex Geometry that contains all the
    /// points in the input Geometry.
    /// Uses the Graham Scan algorithm.
    /// </summary>
    public class ConvexHull
    {
        private readonly IGeometryFactory _geomFactory;
        private readonly Coordinate[] _inputPts;

        /// <summary>
        /// Create a new convex hull construction for the input <c>Geometry</c>.
        /// </summary>
        /// <param name="geometry"></param>
        public ConvexHull(IGeometry geometry)
            : this(ExtractCoordinates(geometry), geometry.Factory) { }

        /// <summary>
        /// Create a new convex hull construction for the input <see cref="Coordinate" /> array.
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="geomFactory"></param>
        public ConvexHull(Coordinate[] pts, IGeometryFactory geomFactory)
        {
            _inputPts = pts;
            _geomFactory = geomFactory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        private static Coordinate[] ExtractCoordinates(IGeometry geom)
        {
            UniqueCoordinateArrayFilter filter = new UniqueCoordinateArrayFilter();
            geom.Apply(filter);
            return filter.Coordinates;
        }

        /// <summary>
        /// Returns a <c>Geometry</c> that represents the convex hull of the input point.
        /// The point will contain the minimal number of points needed to
        /// represent the convex hull.  In particular, no more than two consecutive
        /// points will be collinear.
        /// </summary>
        /// <returns>
        /// If the convex hull contains 3 or more points, a <c>Polygon</c>;
        /// 2 points, a <c>LineString</c>;
        /// 1 point, a <c>Point</c>;
        /// 0 points, an empty <c>GeometryCollection</c>.
        /// </returns>
        public virtual IGeometry GetConvexHull()
        {
            if (_inputPts.Length == 0)
                return _geomFactory.CreateGeometryCollection(null);

            if (_inputPts.Length == 1)
                return _geomFactory.CreatePoint(_inputPts[0]);

            if (_inputPts.Length == 2)
                return _geomFactory.CreateLineString(_inputPts);

            Coordinate[] reducedPts = _inputPts;
            // use heuristic to reduce points, if large
            if (_inputPts.Length > 50)
                reducedPts = Reduce(_inputPts);

            // sort points for Graham scan.
            Coordinate[] sortedPts = PreSort(reducedPts);

            // Use Graham scan to find convex hull.
            Stack<Coordinate> cHs = GrahamScan(sortedPts);

            // Convert stack to an array.
            Coordinate[] cH = cHs.ToArray();

            // Convert array to appropriate output geometry.
            return LineOrPolygon(cH);
        }

        /// <summary>
        /// Uses a heuristic to reduce the number of points scanned to compute the hull.
        /// The heuristic is to find a polygon guaranteed to
        /// be in (or on) the hull, and eliminate all points inside it.
        /// A quadrilateral defined by the extremal points
        /// in the four orthogonal directions
        /// can be used, but even more inclusive is
        /// to use an octilateral defined by the points in the 8 cardinal directions.
        /// Notice that even if the method used to determine the polygon vertices
        /// is not 100% robust, this does not affect the robustness of the convex hull.
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        private static Coordinate[] Reduce(Coordinate[] pts)
        {
            Coordinate[] polyPts = ComputeOctRing(pts);

            // unable to compute interior polygon for some reason
            if (polyPts == null)
                return pts;

            // add points defining polygon
            var reducedSet = new SortedSet<Coordinate>();
            for (int i = 0; i < polyPts.Length; i++)
                reducedSet.Add(polyPts[i]);

            /*
             * Add all unique points not in the interior poly.
             * CgAlgorithms.IsPointInRing is not defined for points actually on the ring,
             * but this doesn't matter since the points of the interior polygon
             * are forced to be in the reduced set.
             */
            for (int i = 0; i < pts.Length; i++)
                if (!CgAlgorithms.IsPointInRing(pts[i], polyPts))
                    reducedSet.Add(pts[i]);

            Coordinate[] arr = new Coordinate[reducedSet.Count];
            reducedSet.CopyTo(arr, 0);
            return arr;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        private static Coordinate[] PreSort(Coordinate[] pts)
        {
            // find the lowest point in the set. If two or more points have
            // the same minimum y coordinate choose the one with the minimu x.
            // This focal point is put in array location pts[0].
            for (int i = 1; i < pts.Length; i++)
            {
                if ((pts[i].Y < pts[0].Y) || ((pts[i].Y == pts[0].Y)
                     && (pts[i].X < pts[0].X)))
                {
                    Coordinate t = pts[0];
                    pts[0] = pts[i];
                    pts[i] = t;
                }
            }

            // sort the points radially around the focal point.
            Array.Sort(pts, 1, pts.Length - 1, new RadialComparator(pts[0]));
            return pts;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static Stack<Coordinate> GrahamScan(Coordinate[] c)
        {
            Stack<Coordinate> ps = new Stack<Coordinate>(c.Length);
            ps.Push(c[0]);
            ps.Push(c[1]);
            ps.Push(c[2]);
            for (int i = 3; i < c.Length; i++)
            {
                Coordinate p = ps.Pop();
                while (CgAlgorithms.ComputeOrientation(ps.Peek(), p, c[i]) > 0)
                    p = ps.Pop();
                ps.Push(p);
                ps.Push(c[i]);
            }
            ps.Push(c[0]);
            return ps;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        private Stack<Coordinate> ReverseStack(Stack<Coordinate> ps)
        {
            // Do a manual reverse of the stack
            int size = ps.Count;
            Coordinate[] tempArray = new Coordinate[size];
            for (int i = 0; i < size; i++)
                tempArray[i] = ps.Pop();
            Stack<Coordinate> returnStack = new Stack<Coordinate>(size);
            foreach (Coordinate obj in tempArray)
                returnStack.Push(obj);
            return returnStack;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="c3"></param>
        /// <returns>
        /// Whether the three coordinates are collinear
        /// and c2 lies between c1 and c3 inclusive.
        /// </returns>
        private static bool IsBetween(Coordinate c1, Coordinate c2, Coordinate c3)
        {
            if (CgAlgorithms.ComputeOrientation(c1, c2, c3) != 0)
                return false;
            if (c1.X != c3.X)
            {
                if (c1.X <= c2.X && c2.X <= c3.X)
                    return true;
                if (c3.X <= c2.X && c2.X <= c1.X)
                    return true;
            }
            if (c1.Y != c3.Y)
            {
                if (c1.Y <= c2.Y && c2.Y <= c3.Y)
                    return true;
                if (c3.Y <= c2.Y && c2.Y <= c1.Y)
                    return true;
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputPts"></param>
        /// <returns></returns>
        private static Coordinate[] ComputeOctRing(Coordinate[] inputPts)
        {
            Coordinate[] octPts = ComputeOctPts(inputPts);
            CoordinateList coordList = new CoordinateList { { octPts, false } };

            // points must all lie in a line
            if (coordList.Count < 3)
                return null;

            coordList.CloseRing();
            return coordList.ToCoordinateArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputPts"></param>
        /// <returns></returns>
        private static Coordinate[] ComputeOctPts(Coordinate[] inputPts)
        {
            Coordinate[] pts = new Coordinate[8];
            for (int j = 0; j < pts.Length; j++)
                pts[j] = inputPts[0];

            for (int i = 1; i < inputPts.Length; i++)
            {
                if (inputPts[i].X < pts[0].X)
                    pts[0] = inputPts[i];

                if (inputPts[i].X - inputPts[i].Y < pts[1].X - pts[1].Y)
                    pts[1] = inputPts[i];

                if (inputPts[i].Y > pts[2].Y)
                    pts[2] = inputPts[i];

                if (inputPts[i].X + inputPts[i].Y > pts[3].X + pts[3].Y)
                    pts[3] = inputPts[i];

                if (inputPts[i].X > pts[4].X)
                    pts[4] = inputPts[i];

                if (inputPts[i].X - inputPts[i].Y > pts[5].X - pts[5].Y)
                    pts[5] = inputPts[i];

                if (inputPts[i].Y < pts[6].Y)
                    pts[6] = inputPts[i];

                if (inputPts[i].X + inputPts[i].Y < pts[7].X + pts[7].Y)
                    pts[7] = inputPts[i];
            }
            return pts;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coordinates"> The vertices of a linear ring, which may or may not be flattened (i.e. vertices collinear).</param>
        /// <returns>A 2-vertex <c>LineString</c> if the vertices are collinear;
        /// otherwise, a <c>Polygon</c> with unnecessary (collinear) vertices removed. </returns>
        private IGeometry LineOrPolygon(Coordinate[] coordinates)
        {
            coordinates = CleanRing(coordinates);
            if (coordinates.Length == 3)
                return _geomFactory.CreateLineString(new[] { coordinates[0], coordinates[1] });
            ILinearRing linearRing = _geomFactory.CreateLinearRing(coordinates);
            return _geomFactory.CreatePolygon(linearRing, null);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="original">The vertices of a linear ring, which may or may not be flattened (i.e. vertices collinear).</param>
        /// <returns>The coordinates with unnecessary (collinear) vertices removed.</returns>
        private static Coordinate[] CleanRing(Coordinate[] original)
        {
            Equals(original[0], original[original.Length - 1]);
            List<Coordinate> cleanedRing = new List<Coordinate>();
            Coordinate previousDistinctCoordinate = Coordinate.Empty;
            for (int i = 0; i <= original.Length - 2; i++)
            {
                Coordinate currentCoordinate = original[i];
                Coordinate nextCoordinate = original[i + 1];
                if (currentCoordinate.Equals(nextCoordinate))
                    continue;

                if (!previousDistinctCoordinate.IsEmpty() &&
                    IsBetween(previousDistinctCoordinate, currentCoordinate, nextCoordinate))
                    continue;
                cleanedRing.Add(currentCoordinate);
                previousDistinctCoordinate = currentCoordinate;
            }
            cleanedRing.Add(original[original.Length - 1]);
            return cleanedRing.ToArray();
        }

        #region Nested type: RadialComparator

        /// <summary>
        /// Compares <see cref="Coordinate" />s for their angle and distance
        /// relative to an origin.
        /// </summary>
        private class RadialComparator : IComparer<Coordinate>
        {
            private readonly Coordinate _origin = Coordinate.Empty;

            /// <summary>
            /// Initializes a new instance of the <see cref="RadialComparator"/> class.
            /// </summary>
            /// <param name="origin"></param>
            public RadialComparator(Coordinate origin)
            {
                _origin = origin;
            }

            #region IComparer<Coordinate> Members

            /// <summary>
            ///
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <returns></returns>
            public int Compare(Coordinate p1, Coordinate p2)
            {
                return PolarCompare(_origin, p1, p2);
            }

            #endregion

            /// <summary>
            ///
            /// </summary>
            /// <param name="o"></param>
            /// <param name="p"></param>
            /// <param name="q"></param>
            /// <returns></returns>
            private static int PolarCompare(Coordinate o, Coordinate p, Coordinate q)
            {
                double dxp = p.X - o.X;
                double dyp = p.Y - o.Y;
                double dxq = q.X - o.X;
                double dyq = q.Y - o.Y;

                int orient = CgAlgorithms.ComputeOrientation(o, p, q);

                if (orient == CgAlgorithms.COUNTER_CLOCKWISE)
                    return 1;
                if (orient == CgAlgorithms.CLOCKWISE)
                    return -1;

                // points are collinear - check distance
                double op = dxp * dxp + dyp * dyp;
                double oq = dxq * dxq + dyq * dyq;
                if (op < oq)
                    return -1;
                if (op > oq)
                    return 1;
                return 0;
            }
        }

        #endregion
    }
}