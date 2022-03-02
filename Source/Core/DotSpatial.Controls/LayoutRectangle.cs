// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A control that draws a standard colored rectangle to the print layout.
    /// </summary>
    public class LayoutRectangle : LayoutElement
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRectangle"/> class.
        /// </summary>
        public LayoutRectangle()
        {
            Name = "Rectangle";
            Background = new PolygonSymbolizer(Color.Transparent, Color.Black, 2.0);
            ResizeStyle = ResizeStyle.HandledInternally;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Doesn't need to do anything now because the drawing code is in the background property of the base class.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="printing">Indicates whether the content is printed or previewed.</param>
        public override void Draw(Graphics g, bool printing)
        {
        }

        #endregion
    }
}