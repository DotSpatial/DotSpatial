// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This represents a single box inside of a symbol size chooser.
    /// </summary>
    public class SizeBox
    {
        #region Fields

        private Color _backColor;
        private Color _selectionColor;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the base color for the normal background color for this box.
        /// </summary>
        public Color BackColor
        {
            get
            {
                return _backColor;
            }

            set
            {
                _backColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the rectangular bounds for this size box.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this item should draw itself as though it has been selected.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the rounding radius for this box.
        /// </summary>
        public int RoundingRadius { get; set; }

        /// <summary>
        /// Gets or sets the selection color for this selection.
        /// </summary>
        public Color SelectionColor
        {
            get
            {
                return _selectionColor;
            }

            set
            {
                _selectionColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public Size2D Size { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The graphics device and clip rectangle are in the parent coordinates.
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        /// <param name="symbol">The symbol to use for drawing.</param>
        public void Draw(Graphics g, Rectangle clipRectangle, ISymbol symbol)
        {
            Color topLeft;
            Color bottomRight;
            if (IsSelected)
            {
                topLeft = _selectionColor.Darker(.3F);
                bottomRight = _selectionColor.Lighter(.3F);
            }
            else
            {
                topLeft = _backColor.Lighter(.3F);
                bottomRight = _backColor.Darker(.3F);
            }

            LinearGradientBrush b = new LinearGradientBrush(Bounds, topLeft, bottomRight, LinearGradientMode.ForwardDiagonal);
            GraphicsPath gp = new GraphicsPath();
            gp.AddRoundedRectangle(Bounds, RoundingRadius);
            g.FillPath(b, gp);
            gp.Dispose();
            b.Dispose();

            Matrix old = g.Transform;
            Matrix shift = g.Transform;
            shift.Translate(Bounds.Left + (Bounds.Width / 2), Bounds.Top + (Bounds.Height / 2));
            g.Transform = shift;

            if (symbol != null)
            {
                OnDrawSymbol(g, symbol);
            }

            g.Transform = old;
        }

        /// <summary>
        /// This draws the specified symbol using the "Size" property from this box rather than the real size.
        /// The transform has been zeroed on the center of this box.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="symbol">Symbol that gets drawn.</param>
        protected virtual void OnDrawSymbol(Graphics g, ISymbol symbol)
        {
            ISymbol symbolTemp = symbol.Copy();
            symbolTemp.Size = Size;
            symbolTemp.Draw(g, 1);
        }

        #endregion
    }
}