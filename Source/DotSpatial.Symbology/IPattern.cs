// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for pattern.
    /// </summary>
    public interface IPattern : ICloneable, IChangeItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the rectangular bounds. This controls how the gradient is drawn, and
        /// should be set to the envelope of the entire layer being drawn
        /// </summary>
        RectangleF Bounds { get; set; }

        /// <summary>
        /// Gets or sets the line symbolizer that is the outline for this pattern.
        /// </summary>
        ILineSymbolizer Outline { get; set; }

        /// <summary>
        /// Gets the pattern type of this pattern.
        /// </summary>
        PatternType PatternType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the pattern should use the outline symbolizer.
        /// </summary>
        bool UseOutline { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Copies the properties defining the outline from the specified source onto this pattern.
        /// </summary>
        /// <param name="source">The source pattern to copy outline properties from.</param>
        void CopyOutline(IPattern source);

        /// <summary>
        /// Draws the borders for this graphics path by sequentially drawing all
        /// the strokes in the border symbolizer
        /// </summary>
        /// <param name="g">The Graphics device to draw to </param>
        /// <param name="gp">The GraphicsPath that describes the outline to draw</param>
        /// <param name="scaleWidth">The scaleWidth to use for scaling the line width </param>
        void DrawPath(Graphics g, GraphicsPath gp, double scaleWidth);

        /// <summary>
        /// Fills the specified graphics path with the pattern specified by this object
        /// </summary>
        /// <param name="g">The Graphics device to draw to</param>
        /// <param name="gp">The GraphicsPath that describes the closed shape to fill</param>
        void FillPath(Graphics g, GraphicsPath gp);

        /// <summary>
        /// Gets a color that can be used to represent this pattern. In some cases, a color is not
        /// possible, in which case, this returns Gray.
        /// </summary>
        /// <returns>A single System.Color that can be used to represent this pattern.</returns>
        Color GetFillColor();

        /// <summary>
        /// Sets the color that will attempt to be applied to the top pattern. If the pattern is
        /// not colorable, this does nothing.
        /// </summary>
        /// <param name="color">Color that is set.</param>
        void SetFillColor(Color color);

        #endregion
    }
}