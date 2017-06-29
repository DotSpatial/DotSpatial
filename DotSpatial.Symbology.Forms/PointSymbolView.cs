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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/5/2009 3:04:50 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// PointSymbolView
    /// </summary>
    [ToolboxItem(false)]
    public class PointSymbolView : Control
    {
        #region Private Variables

        private BorderStyle _borderStyle;
        private IPointSymbolizer _symbolizer;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Draws the point symbol in the view
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clip"></param>
        protected virtual void OnDraw(Graphics g, Rectangle clip)
        {
            if (_borderStyle == BorderStyle.Fixed3D)
            {
                g.DrawLine(Pens.White, 0, Height - 1, Width - 1, Height - 1);
                g.DrawLine(Pens.White, Width - 1, 0, Width - 1, Height - 1);
                g.DrawLine(Pens.Gray, 0, 0, 0, Height - 1);
                g.DrawLine(Pens.Gray, 0, 0, Width - 1, 0);
            }
            if (_borderStyle == BorderStyle.FixedSingle)
            {
                g.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
            }

            if (_symbolizer == null) return;
            Size2D size = _symbolizer.GetSize();
            int w = (int)size.Width;
            int h = (int)size.Height;
            if (w < 1 || h < 1) return;
            Rectangle symbolBounds = new Rectangle(Width / 2 - w / 2, Height / 2 - h / 2, w, h);
            if (!symbolBounds.IntersectsWith(clip)) return;
            _symbolizer.Draw(g, symbolBounds);
        }

        /// <summary>
        /// prevents flicker by preventing the white background being drawn here
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Custom drawing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clip = e.ClipRectangle;
            if (clip.IsEmpty) clip = ClientRectangle;
            Bitmap bmp = new Bitmap(clip.Width, clip.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.TranslateTransform(-clip.X, -clip.Y);
            g.Clip = new Region(clip);
            g.Clear(BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            OnDraw(g, clip);
            g.Dispose();
            e.Graphics.DrawImage(bmp, clip, new Rectangle(0, 0, clip.Width, clip.Height), GraphicsUnit.Pixel);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer being drawn in this view.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IPointSymbolizer Symbolizer
        {
            get { return _symbolizer; }
            set
            {
                _symbolizer = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the way that the border of this control will be drawn.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the way that the border of this control will be drawn.")]
        public BorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set { _borderStyle = value; }
        }

        #endregion
    }
}