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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 12:30:41 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IFeatureSymbolizer
    /// </summary>
    public interface IFeatureSymbolizer : ILegendItem
    {
        #region Methods

        /// <summary>
        /// Draws a simple rectangle in the specified location.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="target"></param>
        void Draw(Graphics g, Rectangle target);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating whether or not this specific feature should be drawn.
        /// </summary>
        bool IsVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets a ScaleModes enumeration that determines whether non-coordinate drawing
        /// properties like width or size use pixels or world coordinates.  If pixels are
        /// specified, a back transform is used to approximate pixel sizes.
        /// </summary>
        ScaleMode ScaleMode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the smoothing mode to use that controls advanced features like
        /// anti-aliasing.  By default this is set to antialias.
        /// </summary>
        bool Smoothing
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the graphics unit to work with.
        /// </summary>
        GraphicsUnit Units
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the outline, assuming that the symbolizer either supports outlines, or
        /// else by using a second symbol layer.
        /// </summary>
        /// <param name="outlineColor">The color of the outline</param>
        /// <param name="width">The width of the outline in pixels</param>
        void SetOutline(Color outlineColor, double width);

        #endregion
    }
}