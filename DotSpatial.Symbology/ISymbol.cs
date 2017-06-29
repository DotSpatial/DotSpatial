// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 2:05:34 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ISymbol
    /// </summary>
    public interface ISymbol : IDescriptor
    {
        #region Methods

        /// <summary>
        /// Only copies the shared placement aspects (Size, Offset, Angle) from the specified symbol.
        /// </summary>
        /// <param name="symbol">The symbol to copy values from.</param>
        void CopyPlacement(ISymbol symbol);

        /// <summary>
        /// Takes into account the size, angle, and offset to calculate a rectangular bounding size that completely
        /// contains the current symbol.
        /// </summary>
        /// <returns>A SizeD representing a size that contains the offset, rotated symbol.</returns>
        Size2D GetBoundingSize();

        /// <summary>
        /// Draws this symbol to the graphics object given the symbolizer that specifies content
        /// across the entire set of scales.
        /// </summary>
        /// <param name="g">The graphics object should be adjusted so that (0, 0) is the center of the symbol.</param>
        /// <param name="scaleSize">If this should draw in pixels, this should be 1.  Otherwise, this should be
        /// the constant that you multiply against so that drawing using geographic units will draw in pixel units.</param>
        void Draw(Graphics g, double scaleSize);

        /// <summary>
        /// Multiplies all of the linear measurements found in this Symbol by the specified value.
        /// This is especially useful for changing units.
        /// </summary>
        /// <param name="value">The double precision floating point value to scale by.</param>
        void Scale(double value);

        /// <summary>
        /// Modifies this symbol in a way that is appropriate for indicating a selected symbol.
        /// This could mean drawing a cyan outline, or changing the color to cyan.
        /// </summary>
        void Select();

        /// <summary>
        /// Gets a color to represent this point.  If the point is using an image,
        /// then this color will be gray.
        /// </summary>
        /// <returns></returns>
        Color GetColor();

        /// <summary>
        /// Sets the primary color of this symbol to the specified color if possible
        /// </summary>
        /// <param name="color">The Color to assign</param>
        void SetColor(Color color);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the double precision floating point that controls the angle in degrees counter clockwise.
        /// </summary>
        double Angle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the 2D offset for this particular symbol
        /// </summary>
        Position2D Offset
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size
        /// </summary>
        Size2D Size
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the symbol type for this symbol.
        /// </summary>
        SymbolType SymbolType
        {
            get;
        }

        #endregion
    }
}