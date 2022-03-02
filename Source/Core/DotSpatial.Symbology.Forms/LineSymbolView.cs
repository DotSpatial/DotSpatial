// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// LineSymbolView.
    /// </summary>
    [ToolboxItem(false)]
    public class LineSymbolView : Control
    {
        #region Fields

        private ILineSymbolizer _symbolizer;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the way that the border of this control will be drawn.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the way that the border of this control will be drawn.")]
        public BorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the symbolizer being drawn in this view.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILineSymbolizer Symbolizer
        {
            get
            {
                return _symbolizer;
            }

            set
            {
                _symbolizer = value;
                Invalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the point symbol in the view.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clip">The clip rectangle.</param>
        protected virtual void OnDraw(Graphics g, Rectangle clip)
        {
            if (BorderStyle == BorderStyle.Fixed3D)
            {
                g.DrawLine(Pens.White, 0, Height - 1, Width - 1, Height - 1);
                g.DrawLine(Pens.White, Width - 1, 0, Width - 1, Height - 1);
                g.DrawLine(Pens.Gray, 0, 0, 0, Height - 1);
                g.DrawLine(Pens.Gray, 0, 0, Width - 1, 0);
            }

            if (BorderStyle == BorderStyle.FixedSingle)
            {
                g.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
            }

            int w = Width;
            int h = Height;
            if (_symbolizer == null) return;

            int lineWidth = Convert.ToInt32(_symbolizer.GetWidth());
            if (lineWidth > 128) lineWidth = 128;
            if (lineWidth < 1) lineWidth = 1;

            GraphicsPath gp = new GraphicsPath();
            gp.AddLines(new[] { new Point(lineWidth, (h * 2) / 3), new Point(w / 3, h / 3), new Point((w * 2) / 3, (h * 2) / 3), new Point(w - lineWidth, h / 3) });
            _symbolizer.DrawPath(g, gp, 1);
        }

        /// <summary>
        /// Custom drawing.
        /// </summary>
        /// <param name="e">The event args.</param>
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

        /// <summary>
        /// Prevents flicker by preventing the white background being drawn here.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        #endregion
    }
}