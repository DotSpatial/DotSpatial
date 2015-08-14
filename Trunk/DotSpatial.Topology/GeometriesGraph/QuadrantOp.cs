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
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// Utility functions for working with quadrants, which are numbered as follows:
    /// <para>
    /// 1 | 0
    /// --+--
    /// 2 | 3
    /// </para>
    /// </summary>
    public class QuadrantOp
    {
        #region Fields

        /// <summary>
	    /// North-East
	    /// </summary>
	    public static readonly int NE = 0;

        /// <summary>
        /// North-West
        /// </summary>
	    public static readonly int NW = 1;

        /// <summary>
        /// South-East
        /// </summary>
	    public static readonly int SE = 3;

        /// <summary>
	    /// South-West
	    /// </summary>
        public static readonly int SW = 2;

        #endregion

        #region Constructors

        /// <summary>
        /// Only static methods!
        /// </summary>
        private QuadrantOp() { }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the right-hand quadrant of the halfplane defined by the two quadrants,
        /// or -1 if the quadrants are opposite, or the quadrant if they are identical.
        /// </summary>
        /// <param name="quad1"></param>
        /// <param name="quad2"></param>
        public static int CommonHalfPlane(int quad1, int quad2)
        {
            // if quadrants are the same they do not determine a unique common halfplane.
            // Simply return one of the two possibilities
            if (quad1 == quad2)
                return quad1;
            int diff = (quad1 - quad2 + 4) % 4;
            // if quadrants are not adjacent, they do not share a common halfplane
            if (diff == 2)
                return -1;

            int min = (quad1 < quad2) ? quad1 : quad2;
            int max = (quad1 > quad2) ? quad1 : quad2;
            // for this one case, the righthand plane is NOT the minimum index;
            if (min == 0 && max == 3)
                return 3;
            // in general, the halfplane index is the minimum of the two adjacent quadrants
            return min;
        }

        /// <summary>
        /// Returns whether the given quadrant lies within the given halfplane (specified
        /// by its right-hand quadrant).
        /// </summary>
        /// <param name="quad"></param>
        /// <param name="halfPlane"></param>
        public static bool IsInHalfPlane(int quad, int halfPlane)
        {
            if (halfPlane == SE) 
                return quad == SE || quad == SW;            
            return quad == halfPlane || quad == halfPlane + 1;
        }

        /// <summary>
        /// Returns true if the given quadrant is 0 or 1.
        /// </summary>
        /// <param name="quad"></param>
        public static bool IsNorthern(int quad)
        {
            return quad == NE || quad == NW;
        }

        /// <summary>
        /// Returns true if the quadrants are 1 and 3, or 2 and 4.
        /// </summary>
        /// <param name="quad1"></param>
        /// <param name="quad2"></param>
        public static bool IsOpposite(int quad1, int quad2)
        {
            if (quad1 == quad2)
                return false;
            int diff = (quad1 - quad2 + 4) % 4;
            // if quadrants are not adjacent, they are opposite
            if (diff == 2)
                return true;
            return false;
        }

        /// <summary>
        /// Returns the quadrant of a directed line segment (specified as x and y
        /// displacements, which cannot both be 0).
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <exception cref="ArgumentException">If the displacements are both 0</exception>
        public static int Quadrant(double dx, double dy)
        {
            if (dx == 0.0 && dy == 0.0)
                throw new ArgumentException("Cannot compute the quadrant for point ( "+ dx + ", " + dy + " )" );
            if (dx >= 0.0)
            {
                if (dy >= 0.0)
                     return NE;
                return SE;
            }
            if (dy >= 0.0) 
                return NW;
            return SW;
        }

        /// <summary>
        /// Returns the quadrant of a directed line segment from p0 to p1.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <exception cref="ArgumentException"> if the points are equal</exception>
        public static int Quadrant(Coordinate p0, Coordinate p1)
        {
            if (p1.X == p0.X && p1.Y == p0.Y)
                throw new ArgumentException("Cannot compute the quadrant for two identical points " + p0);
            
            if (p1.X >= p0.X)
            {
                if (p1.Y >= p0.Y)
                    return NE;
                return SE;
            }
            if (p1.Y >= p0.Y)
                return NW;
            return SW;
        }

        #endregion
    }
}