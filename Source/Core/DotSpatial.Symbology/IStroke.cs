// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for Stroke.
    /// </summary>
    public interface IStroke : IDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the stroke style for this stroke.
        /// </summary>
        StrokeStyle StrokeStyle { get; }

        #endregion

        #region Methods

        /// <summary>
        /// This is an optional expression that allows drawing to the specified GraphicsPath.
        /// Overriding this allows for unconventional behavior to be included, such as
        /// specifying marker decorations, rather than simply returning a pen. A pen
        /// is also returned publicly for convenience.
        /// </summary>
        /// <param name="g">The Graphics device to draw to.</param>
        /// <param name="path">the GraphicsPath to draw.</param>
        /// <param name="scaleWidth">This is 1 for symbolic drawing, but could be
        /// any number for geographic drawing.</param>
        void DrawPath(Graphics g, GraphicsPath path, double scaleWidth);

        /// <summary>
        /// Gets a color to represent this line. If the stroke doesn't work as a color,
        /// then this color will be gray.
        /// </summary>
        /// <returns>The color.</returns>
        Color GetColor();

        /// <summary>
        /// Sets the color of this stroke to the specified color if possible.
        /// </summary>
        /// <param name="color">The color to assign to this color.</param>
        void SetColor(Color color);

        /// <summary>
        /// Casts this stroke to the appropriate pen.
        /// </summary>
        /// <param name="width">The width of the pen.</param>
        /// <returns>The created pen.</returns>
        Pen ToPen(double width);

        #endregion
    }
}