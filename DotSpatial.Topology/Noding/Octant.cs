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

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Octants in the Cartesian plane.
    /// Octants are numbered as follows:
    ///  <para>
    ///   \2|1/
    ///  3 \|/ 0
    ///  ---+--
    ///  4 /|\ 7
    ///   /5|6\
    /// </para>
    ///  If line segments lie along a coordinate axis, the octant is the lower of the two possible values.
    /// </summary>
    public enum OctantDirection
    {
        /// <summary>
        ///
        /// </summary>
        Null = -1,

        /// <summary>
        ///
        /// </summary>
        Zero = 0,

        /// <summary>
        ///
        /// </summary>
        One = 1,

        /// <summary>
        ///
        /// </summary>
        Two = 2,

        /// <summary>
        ///
        /// </summary>
        Three = 3,

        /// <summary>
        ///
        /// </summary>
        Four = 4,

        /// <summary>
        ///
        /// </summary>
        Five = 5,

        /// <summary>
        ///
        /// </summary>
        Six = 6,

        /// <summary>
        ///
        /// </summary>
        Seven = 7,
    }

    /// <summary>
    ///  Methods for computing and working with <see cref="OctantDirection"/> of the Cartesian plane.
    /// </summary>
    public static class Octant
    {
        /// <summary>
        /// Returns the octant of a directed line segment (specified as x and y
        /// displacements, which cannot both be 0).
        /// </summary>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        /// <returns></returns>
        public static OctantDirection GetOctant(double dx, double dy)
        {
            if (dx == 0.0 && dy == 0.0)
                throw new ArgumentException("Cannot compute the octant for point ( " + dx + ", " + dy + " )");

            double adx = Math.Abs(dx);
            double ady = Math.Abs(dy);

            if (dx >= 0)
            {
                if (dy >= 0)
                {
                    return adx >= ady ? OctantDirection.Zero : OctantDirection.One;
                }
                return adx >= ady ? OctantDirection.Seven : OctantDirection.Six;
            }
            if (dy >= 0)
            {
                return adx >= ady ? OctantDirection.Three : OctantDirection.Two;
            }
            return adx >= ady ? OctantDirection.Four : OctantDirection.Five;
        }

        /// <summary>
        /// Returns the octant of a directed line segment from p0 to p1.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static OctantDirection GetOctant(Coordinate p0, Coordinate p1)
        {
            double dx = p1.X - p0.X;
            double dy = p1.Y - p0.Y;
            if (dx == 0.0 && dy == 0.0)
                throw new ArgumentException("Cannot compute the octant for two identical points " + p0);
            return GetOctant(dx, dy);
        }
    }
}