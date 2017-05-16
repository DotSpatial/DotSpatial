// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Legend item with feature symbolization support.
    /// </summary>
    public interface IFeatureSymbolizer : ILegendItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not this specific feature should be drawn.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or Sets a ScaleModes enumeration that determines whether non-coordinate drawing
        /// properties like width or size use pixels or world coordinates. If pixels are
        /// specified, a back transform is used to approximate pixel sizes.
        /// </summary>
        ScaleMode ScaleMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether things should be anti-aliased. By default this is set to antialias.
        /// </summary>
        bool Smoothing { get; set; }

        /// <summary>
        /// Gets or sets the graphics unit to work with.
        /// </summary>
        GraphicsUnit Units { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Draws a simple rectangle in the specified location.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="target">The rectangle that gets drawn.</param>
        void Draw(Graphics g, Rectangle target);

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