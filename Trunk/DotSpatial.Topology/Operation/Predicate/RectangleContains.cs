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

using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Operation.Predicate
{
    /// <summary>
    /// Optimized implementation of spatial predicate "contains"
    /// for cases where the first <c>Geometry</c> is a rectangle.
    /// As a further optimization,
    /// this class can be used directly to test many geometries against a single rectangle.
    /// </summary>
    public class RectangleContains
    {
        #region Fields

        private readonly IEnvelope _rectEnv;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new contains computer for two geometries.
        /// </summary>
        /// <param name="rectangle">A rectangular geometry.</param>
        public RectangleContains(IPolygon rectangle)
        {
            _rectEnv = rectangle.EnvelopeInternal;
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Contains(IPolygon rectangle, IGeometry b)
        {
            RectangleContains rc = new RectangleContains(rectangle);
            return rc.Contains(b);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public bool Contains(IGeometry geom)
        {
            if (!_rectEnv.Contains(geom.EnvelopeInternal))
                return false;
            // check that geom is not contained entirely in the rectangle boundary
            if (IsContainedInBoundary(geom))
                return false;
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        private bool IsContainedInBoundary(IGeometry geom)
        {
            // polygons can never be wholely contained in the boundary
            if (geom is IPolygon)
                return false;
            if (geom is IPoint)
                return IsPointContainedInBoundary((IPoint)geom);
            if (geom is ILineString)
                return IsLineStringContainedInBoundary((ILineString)geom);

            for (int i = 0; i < geom.NumGeometries; i++)
            {
                IGeometry comp = geom.GetGeometryN(i);
                if (!IsContainedInBoundary(comp))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Tests if a line segment is contained in the boundary of the target rectangle.
        /// </summary>
        /// <param name="p0">an endpoint of the segment</param>
        /// <param name="p1">an endpoint of the segment</param>
        /// <returns>true if the line segment is contained in the boundary</returns>
        private bool IsLineSegmentContainedInBoundary(Coordinate p0, Coordinate p1)
        {
            if (p0.Equals(p1))
                return IsPointContainedInBoundary(p0);
            // we already know that the segment is contained in the rectangle envelope
            if (p0.X == p1.X)
            {
                if (p0.X == _rectEnv.Minimum.X || p0.X == _rectEnv.Maximum.X)
                    return true;
            }
            else if (p0.Y == p1.Y)
            {
                if (p0.Y == _rectEnv.Minimum.Y || p0.Y == _rectEnv.Maximum.Y)
                    return true;
            }
            /*
             * Either both x and y values are different
             * or one of x and y are the same, but the other ordinate is not the same as a boundary ordinate
             * In either case, the segment is not wholely in the boundary
             */
            return false;
        }

        /// <summary>
        /// Tests if a linestring is completely contained in the boundary of the target rectangle.
        /// </summary>
        /// <param name="line">the linestring to test</param>
        /// <returns>true if the linestring is contained in the boundary</returns>
        private bool IsLineStringContainedInBoundary(ILineString line)
        {
            ICoordinateSequence seq = line.CoordinateSequence;
            Coordinate p0 = new Coordinate();
            Coordinate p1 = new Coordinate();
            for (int i = 0; i < seq.Count - 1; i++)
            {
                seq.GetCoordinate(i, p0);
                seq.GetCoordinate(i + 1, p1);
                if (!IsLineSegmentContainedInBoundary(p0, p1))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool IsPointContainedInBoundary(IPoint point)
        {
            return IsPointContainedInBoundary(point.Coordinate);
        }
        /// <summary>
        /// Tests if a point is contained in the boundary of the target rectangle.
        /// </summary>
        /// <param name="pt">the point to test</param>
        /// <returns>true if the point is contained in the boundary</returns>
        private bool IsPointContainedInBoundary(Coordinate pt)
        {
            // we already know that the point is contained in the rectangle envelope
            return pt.X == _rectEnv.Minimum.X || pt.X == _rectEnv.Maximum.X || pt.Y == _rectEnv.Minimum.Y || pt.Y == _rectEnv.Maximum.Y;
        }

        #endregion
    }
}