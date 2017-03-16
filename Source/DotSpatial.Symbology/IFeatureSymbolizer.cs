// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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
    /// Legend item with feature symbolization support.
    /// </summary>
    public interface IFeatureSymbolizer : ILegendItem
    {
        #region Methods

        /// <summary>
        /// Draws a simple rectangle in the specified location.
        /// </summary>
        void Draw(Graphics g, Rectangle target);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating whether or not this specific feature should be drawn.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or Sets a ScaleModes enumeration that determines whether non-coordinate drawing
        /// properties like width or size use pixels or world coordinates.  If pixels are
        /// specified, a back transform is used to approximate pixel sizes.
        /// </summary>
        ScaleMode ScaleMode { get; set; }

        /// <summary>
        /// Gets or sets the smoothing mode to use that controls advanced features like
        /// anti-aliasing.  By default this is set to antialias.
        /// </summary>
        bool Smoothing { get; set; }

        /// <summary>
        /// Gets or sets the graphics unit to work with.
        /// </summary>
        GraphicsUnit Units { get; set; }

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