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
    /// Class containing static methods for conversions between Dimension values and characters.
    /// </summary>
    public static class Dimension
    {
        #region Constant Fields

        /// <summary>
        /// Symbol for the A (dimension 2) pattern matrix entry
        /// </summary>
        public const char SymA = '2';

        /// <summary>
        /// Symbol for the DONTCARE pattern matrix entry
        /// </summary>
        public const char SymDontcare = '*';

        /// <summary>
        /// Symbol for the FALSE pattern matrix entry
        /// </summary>
        public const char SymFalse = 'F';

        /// <summary>
        /// Symbol for the L (dimension 1) pattern matrix entry
        /// </summary>
        public const char SymL = '1';

        /// <summary>
        /// Symbol for the P (dimension 0) pattern matrix entry
        /// </summary>
        public const char SymP = '0';

        /// <summary>
        /// Symbol for the TRUE pattern matrix entry
        /// </summary>
        public const char SymTrue = 'T';

        #endregion

        #region Methods

        /// <summary>
        /// Converts the dimension value to a dimension symbol,
        /// for example, <c>True => 'T'</c>
        /// </summary>
        /// <param name="dimensionValue">Number that can be stored in the <c>IntersectionMatrix</c>.
        /// Possible values are <c>True, False, Dontcare, 0, 1, 2</c>.</param>
        /// <returns>Character for use in the string representation of an <c>IntersectionMatrix</c>.
        /// Possible values are <c>T, F, *, 0, 1, 2</c>.</returns>
        public static char ToDimensionSymbol(DimensionType dimensionValue)
        {
            switch (dimensionValue)
            {
                case DimensionType.False:
                    return SymFalse;
                case DimensionType.True:
                    return SymTrue;
                case DimensionType.Dontcare:
                    return SymDontcare;
                case DimensionType.Point:
                    return SymP;
                case DimensionType.Curve:
                    return SymL;
                case DimensionType.Surface:
                    return SymA;
                default:
                    throw new ArgumentOutOfRangeException("Unknown dimension value: " + dimensionValue);
            }
        }

        /// <summary>
        /// Converts the dimension symbol to a dimension value,
        /// for example, <c>'*' => Dontcare</c>
        /// </summary>
        /// <param name="dimensionSymbol">Character for use in the string representation of an <c>IntersectionMatrix</c>.
        /// Possible values are <c>T, F, *, 0, 1, 2</c>.</param>
        /// <returns>Number that can be stored in the <c>IntersectionMatrix</c>.
        /// Possible values are <c>True, False, Dontcare, 0, 1, 2</c>.</returns>
        public static DimensionType ToDimensionValue(char dimensionSymbol)
        {
            switch (Char.ToUpper(dimensionSymbol))
            {
                case SymFalse:
                    return DimensionType.False;
                case SymTrue:
                    return DimensionType.True;
                case SymDontcare:
                    return DimensionType.Dontcare;
                case SymP:
                    return DimensionType.Point;
                case SymL:
                    return DimensionType.Curve;
                case SymA:
                    return DimensionType.Surface;
                default:
                    throw new ArgumentOutOfRangeException("Unknown dimension symbol: " + dimensionSymbol);
            }
        }

        #endregion
    }
}