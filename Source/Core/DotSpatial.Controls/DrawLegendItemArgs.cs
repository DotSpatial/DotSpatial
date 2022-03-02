// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Represents the argument used for drawing legend items.
    /// </summary>
    public class DrawLegendItemArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawLegendItemArgs"/> class.
        /// </summary>
        /// <param name="g">A Graphics surface to draw on.</param>
        /// <param name="item">The legend item to draw.</param>
        /// <param name="clipRectangle">The bounds that drawing should occur within.</param>
        /// <param name="topLeft">The position of the top left corner where drawing should start.</param>
        public DrawLegendItemArgs(Graphics g, ILegendItem item, Rectangle clipRectangle, PointF topLeft)
        {
            TopLeft = topLeft;
            Graphics = g;
            Item = item;
            ClipRectangle = clipRectangle;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the rectangle that limits where drawing should occur.
        /// </summary>
        public Rectangle ClipRectangle { get; protected set; }

        /// <summary>
        /// Gets or sets the graphics object for drawing to.
        /// </summary>
        public Graphics Graphics { get; protected set; }

        /// <summary>
        /// Gets or sets the interface for the legend item being drawn.
        /// </summary>
        public ILegendItem Item { get; protected set; }

        /// <summary>
        /// Gets or sets the point that is the top left position where this item should start drawing, counting indentation.
        /// </summary>
        public PointF TopLeft { get; protected set; }

        #endregion
    }
}