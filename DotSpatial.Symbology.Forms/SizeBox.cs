// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/14/2009 11:49:37 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This represents a single box inside of a symbol size chooser.
    /// </summary>
    public class SizeBox
    {
        #region Private Variables

        private Color _backColor;
        private Rectangle _bounds;
        private bool _isSelected;
        private int _roundingRadius;
        private Color _selectionColor;
        private Size2D _size;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// The graphics device and clip rectangle are in the parent coordinates.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        /// <param name="symbol">The symbol to use for drawing.</param>
        public void Draw(Graphics g, Rectangle clipRectangle, ISymbol symbol)
        {
            Color topLeft;
            Color bottomRight;
            if (_isSelected)
            {
                topLeft = _selectionColor.Darker(.3F);
                bottomRight = _selectionColor.Lighter(.3F);
            }
            else
            {
                topLeft = _backColor.Lighter(.3F);
                bottomRight = _backColor.Darker(.3F);
            }
            LinearGradientBrush b = new LinearGradientBrush(_bounds, topLeft, bottomRight, LinearGradientMode.ForwardDiagonal);
            GraphicsPath gp = new GraphicsPath();
            gp.AddRoundedRectangle(Bounds, _roundingRadius);
            g.FillPath(b, gp);
            gp.Dispose();
            b.Dispose();

            Matrix old = g.Transform;
            Matrix shift = g.Transform;
            shift.Translate(_bounds.Left + _bounds.Width / 2, _bounds.Top + _bounds.Height / 2);
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
        /// <param name="g"></param>
        /// <param name="symbol"></param>
        protected virtual void OnDrawSymbol(Graphics g, ISymbol symbol)
        {
            ISymbol symbolTemp = symbol.Copy();
            symbolTemp.Size = _size;
            symbolTemp.Draw(g, 1);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the base color for the normal background color for this box.
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Gets or sets the rectangular bounds for this size box.
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Gets or sets the size
        /// </summary>
        public Size2D Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Gets or sets whether or not this item should draw itself as though it has been selected.
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        /// <summary>
        /// Gets or sets the selection color for this selection.
        /// </summary>
        public Color SelectionColor
        {
            get { return _selectionColor; }
            set { _selectionColor = value; }
        }

        /// <summary>
        /// Gets or sets the rouding radius for this box.
        /// </summary>
        public int RoundingRadius
        {
            get { return _roundingRadius; }
            set { _roundingRadius = value; }
        }

        #endregion
    }
}