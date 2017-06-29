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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/22/2009 9:03:50 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// LabelAlignmentButton
    /// </summary>
    public class LabelAlignmentButton
    {
        #region Private Variables

        private Color _backColor;
        private Rectangle _bounds;
        private bool _highlighted;
        private bool _selected;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LabelAlignmentButton
        /// </summary>
        public LabelAlignmentButton()
        {
        }

        /// <summary>
        /// Creates a new instance of the button with the specified rectangle as the bounds.
        /// </summary>
        /// <param name="bounds">The bounds relative to the parent client</param>
        /// <param name="backColor">The background color</param>
        public LabelAlignmentButton(Rectangle bounds, Color backColor)
        {
            _bounds = bounds;
            _backColor = backColor;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Instructs this button to draw itself.
        /// </summary>
        /// <param name="g">The graphics surface to draw to.</param>
        public void Draw(Graphics g)
        {
            if (_bounds.Width == 0 || _bounds.Height == 0) return;
            Pen border = null;
            // Pen innerBorder = Pens.White;
            Brush fill = null;
            if (!_selected && !_highlighted)
            {
                border = new Pen(Color.Gray);
                fill = new LinearGradientBrush(Bounds, BackColor.Lighter(.2f), BackColor.Darker(.2f), 45);
            }
            if (!_selected && _highlighted)
            {
                border = new Pen(Color.FromArgb(216, 240, 250));
                fill = new LinearGradientBrush(Bounds, Color.FromArgb(245, 250, 253), Color.FromArgb(232, 245, 253), LinearGradientMode.Vertical);
            }
            if (_selected && !_highlighted)
            {
                border = new Pen(Color.FromArgb(153, 222, 253));
                fill = new LinearGradientBrush(Bounds, Color.FromArgb(241, 248, 253), Color.FromArgb(213, 239, 252), LinearGradientMode.Vertical);
            }
            if (_selected && _highlighted)
            {
                border = new Pen(Color.FromArgb(182, 230, 251));
                fill = new LinearGradientBrush(Bounds, Color.FromArgb(232, 246, 253), Color.FromArgb(196, 232, 250), LinearGradientMode.Vertical);
            }
            GraphicsPath gp = new GraphicsPath();
            gp.AddRoundedRectangle(Bounds, 2);
            if (fill != null) g.FillPath(fill, gp);
            if (border != null) g.DrawPath(border, gp);
            gp.Dispose();
            if (fill != null) fill.Dispose();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the bounds for this button.
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Gets or sets the color used as the background color.
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Boolean, true if this button is currently highlighted (mouse over)
        /// </summary>
        public bool Highlighted
        {
            get { return _highlighted; }
            set { _highlighted = value; }
        }

        /// <summary>
        /// Boolean, true if this button is currently selected
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        #endregion
    }
}