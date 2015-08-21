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
    /// Provides constants representing the dimensions of a point, a curve and a surface.
    /// </summary>
    /// <remarks>
    /// Also provides constants representing the dimensions of the empty geometry and
    /// non-empty geometries, and the wildcard constant <see cref="Dontcare"/> meaning "any dimension".
    /// These constants are used as the entries in <see cref="IntersectionMatrix"/>s.
    /// </remarks>
    public enum Dimension
    {
        /// <summary>
        /// Dimension value of a point (0).
        /// </summary>
        Point = 0,

        /// <summary>
        /// Dimension value of a curve (1).
        /// </summary>
        Curve = 1,

        /// <summary>
        /// Dimension value of a surface (2).
        /// </summary>
        Surface = 2,

        /// <summary>
        /// Dimension value of a empty point (-1).
        /// </summary>
        False = -1,

        /// <summary>
        /// Dimension value of non-empty geometries (= {Point,Curve,A}).
        /// </summary>
        True = -2,

        /// <summary>
        /// Dimension value for any dimension (= {False, True}).
        /// </summary>
        Dontcare = -3
    }

    /// <summary>
    /// Class containing static methods for conversions
    /// between dimension values and characters.
    /// </summary>
    public class DimensionUtility
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
        /// Possible values are <c>T, F, * , 0, 1, 2</c>.</returns>
        public static char ToDimensionSymbol(Dimension dimensionValue)
        {
            switch (dimensionValue)
            {
                case Dimension.False:
                    return SymFalse;
                case Dimension.True:
                    return SymTrue;
                case Dimension.Dontcare:
                    return SymDontcare;
                case Dimension.Point:
                    return SymP;
                case Dimension.Curve:
                    return SymL;
                case Dimension.Surface:
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
        public static Dimension ToDimensionValue(char dimensionSymbol)
        {
            switch (Char.ToUpper(dimensionSymbol))
            {
                case SymFalse:
                    return Dimension.False;
                case SymTrue:
                    return Dimension.True;
                case SymDontcare:
                    return Dimension.Dontcare;
                case SymP:
                    return Dimension.Point;
                case SymL:
                    return Dimension.Curve;
                case SymA:
                    return Dimension.Surface;
                default:
                    throw new ArgumentOutOfRangeException("Unknown dimension symbol: " + dimensionSymbol);
            }
        }

        #endregion
    }
}