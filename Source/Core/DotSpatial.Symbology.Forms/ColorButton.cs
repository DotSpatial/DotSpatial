// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A button that is a certain color with beveled edges. The default use of this control is to work as a simple
    /// color dialog launcher that happens to show a preview of the currently selected color.
    /// </summary>
    [DefaultEvent("ColorChanged")]
    public class ColorButton : Control
    {
        #region Fields

        private Color _color;
        private bool _isDown;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorButton"/> class.
        /// </summary>
        public ColorButton()
        {
            _color = Color.Blue;
            LaunchDialogOnClick = true;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the color has changed.
        /// </summary>
        public event EventHandler ColorChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the floating point radius between the outside of the button and the flat central portion.
        /// </summary>
        [Description("Gets or sets the floating point radius between the outside of the button and the flat central portion.")]
        public int BevelRadius { get; set; }

        /// <summary>
        /// Gets or sets the color of this button.
        /// </summary>
        [Description("Gets or sets the color of this button")]
        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
                Invalidate();
                OnColorChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button should launch a
        /// color dialog to alter its color when it is clicked.
        /// </summary>
        [Description("Gets or sets a boolean that indicates whether this button should launch a color dialog to alter its color when it is clicked.")]
        public bool LaunchDialogOnClick { get; set; }

        /// <summary>
        /// Gets or sets the rounding radius that controls how rounded this button appears.
        /// </summary>
        [Description("Gets or sets the rounding radius that controls how rounded this button appears")]
        public int RoundingRadius { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the color but will not fire the ColorChanged event.
        /// </summary>
        /// <param name="color">The color.</param>
        public void SetColorQuietly(Color color)
        {
            _color = color;
            Invalidate();
        }

        /// <summary>
        /// Clicking launches a color dialog by default.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnClick(EventArgs e)
        {
            if (LaunchDialogOnClick)
            {
                using (ColorDialog cd = new ColorDialog
                {
                    AnyColor = true,
                    CustomColors = new[] { ToDialogColor(_color) },
                    Color = _color
                })
                {
                    if (cd.ShowDialog() != DialogResult.OK) return;
                    Color = cd.Color;
                }
            }

            base.OnClick(e);
        }

        /// <summary>
        /// Fires the ColorChanged event.
        /// </summary>
        protected virtual void OnColorChanged()
        {
            ColorChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Custom drawing code.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);

            // Even when fully transparent, I would like to see a "glass like" reflective appearance
            Color first = _isDown ? Color.FromArgb(80, Color.DarkCyan).Darker(.3F) : Color.FromArgb(80, Color.LightCyan.Lighter(.3F));
            Color second = _isDown ? Color.FromArgb(80, Color.LightCyan.Lighter(.3F)) : Color.FromArgb(80, Color.DarkCyan).Darker(.3F);
            Color first2 = _isDown ? _color.Darker(.4F) : _color.Lighter(.3F);
            Color second2 = _isDown ? _color.Lighter(.2F) : _color.Darker(.3F);

            int rad = Math.Min(Math.Min(RoundingRadius, Width / 2), Height / 2);

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRoundedRectangle(bounds, rad);

                using (var crystalBrush = new LinearGradientBrush(new Point(0, 0), new Point(bounds.Width, bounds.Height), first, second))
                {
                    g.FillPath(crystalBrush, gp);
                }

                using (var lgb = new LinearGradientBrush(new Point(0, 0), new Point(bounds.Width, bounds.Height), first2, second2))
                {
                    g.FillPath(lgb, gp);
                }
            }

            var bevel2 = BevelRadius * 2;
            if (Width >= bevel2 && Height >= bevel2)
            {
                Rectangle inner = new Rectangle(bounds.Left + BevelRadius, bounds.Top + BevelRadius, bounds.Width - bevel2, bounds.Height - bevel2);
                using (var gp = new GraphicsPath())
                {
                    int rRad = RoundingRadius - BevelRadius;
                    if (rRad < 0) rRad = 0;
                    gp.AddRoundedRectangle(inner, rRad);

                    Color cPlain = Color.FromArgb(20, Color.Cyan);
                    using (SolidBrush back = new SolidBrush(BackColor))
                    {
                        g.FillPath(back, gp);
                    }

                    using (SolidBrush crystalFlat = new SolidBrush(cPlain))
                    {
                        g.FillPath(crystalFlat, gp);
                    }

                    using (Brush b = new SolidBrush(_color))
                    {
                        g.FillPath(b, gp);
                    }
                }
            }
        }

        /// <summary>
        /// when the mouse down event is received this also "depresses" the button.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                _isDown = true;
            }

            base.OnMouseDown(e);
            Invalidate();
        }

        /// <summary>
        /// Handles the situation where the mouse is moving up.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                _isDown = false;
            }

            base.OnMouseUp(e);
            Invalidate();
        }

        /// <summary>
        /// Sets up a bitmap to use as a double buffer for doing all the drawing code.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clip = e.ClipRectangle;
            if (clip.IsEmpty)
            {
                clip = ClientRectangle;
            }

            Bitmap bmp = new Bitmap(clip.Width, clip.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.Clear(BackColor);
            g.TranslateTransform(-clip.X, -clip.Y);
            OnDraw(g, clip);
            e.Graphics.DrawImage(bmp, clip, new Rectangle(0, 0, clip.Width, clip.Height), GraphicsUnit.Pixel);
            g.Dispose();
        }

        /// <summary>
        /// Cancels the on paint background event to prevent flicker.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        private static int ToDialogColor(Color color)
        {
            return color.R + (color.G << 8) + (color.B << 16);
        }

        #endregion
    }
}