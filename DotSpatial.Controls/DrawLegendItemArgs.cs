// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/2/2008 4:47:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    public class DrawLegendItemArgs
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of DrawLegendItemArgs
        /// </summary>
        /// <param name="g">A Graphics surface to draw on</param>
        /// <param name="item">The legend item to draw</param>
        /// <param name="clipRectangle">The bounds that drawing should occur within</param>
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
        /// Gets the interface for the legend item being drawn
        /// </summary>
        public ILegendItem Item { get; protected set; }

        /// <summary>
        /// Gets the rectangle that limits where drawing should occur
        /// </summary>
        public Rectangle ClipRectangle { get; protected set; }

        /// <summary>
        /// Gets or sets the point that is the top left position where this item should start drawing, counting indentation.
        /// </summary>
        public PointF TopLeft { get; protected set; }

        /// <summary>
        /// Gets the graphics object for drawing to
        /// </summary>
        public Graphics Graphics { get; protected set; }

        #endregion
    }
}