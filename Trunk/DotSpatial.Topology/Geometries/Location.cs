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

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// The location of a <see cref="Coordinate"/> relative to a <see cref="IGeometry"/>
    /// </summary>
    public enum Location
    {
        /// <summary>
        /// DE-9IM row index of the interior of the first point and column index of
        /// the interior of the second point. Location value for the interior of a
        /// point.
        /// </summary>
        /// <remarks>int value = 0;</remarks>
        Interior = 0,

        /// <summary>
        /// DE-9IM row index of the boundary of the first point and column index of
        /// the boundary of the second point. Location value for the boundary of a
        /// point.
        /// </summary>
        /// <remarks>int value = 1;</remarks>
        Boundary = 1,

        /// <summary>
        /// DE-9IM row index of the exterior of the first point and column index of
        /// the exterior of the second point. Location value for the exterior of a
        /// point.
        /// </summary>
        /// <remarks>int value = 2;</remarks>
        Exterior = 2,

        /// <summary>
        /// Used for uninitialized location values.
        /// </summary>
        /// <remarks>int value = 1;</remarks>
        Null = -1,
    }

    /// <summary>
    /// Utility class for <see cref="Location"/> enumeration
    /// </summary>
    public class LocationUtility
    {
        #region Methods

        /// <summary>
        /// Converts the location value to a location symbol, for example, <c>EXTERIOR => 'e'</c>.
        /// </summary>
        /// <param name="locationValue"></param>
        /// <returns>Either 'e', 'b', 'i' or '-'.</returns>
        public static char ToLocationSymbol(Location locationValue)
        {
            switch (locationValue)
            {
                case Location.Exterior:
                    return 'e';
                case Location.Boundary:
                    return 'b';
                case Location.Interior:
                    return 'i';
                case Location.Null:
                    return '-';
            }
            throw new ArgumentException("Unknown location value: " + locationValue);
        }

        #endregion
    }
}