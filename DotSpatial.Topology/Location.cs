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

namespace DotSpatial.Topology
{
    /// <summary>
    ///
    /// </summary>
    public static class Location
    {
        /// <summary>
        /// Converts the location value to a location symbol, for example, <c>EXTERIOR => 'e'</c>.
        /// </summary>
        /// <param name="locationValue"></param>
        /// <returns>Either 'e', 'b', 'i' or '-'.</returns>
        public static char ToLocationSymbol(LocationType locationValue)
        {
            switch (locationValue)
            {
                case LocationType.Exterior:
                    return 'e';
                case LocationType.Boundary:
                    return 'b';
                case LocationType.Interior:
                    return 'i';
                case LocationType.Null:
                    return '-';
            }
            throw new ArgumentException("Unknown location value: " + locationValue);
        }
    }
}