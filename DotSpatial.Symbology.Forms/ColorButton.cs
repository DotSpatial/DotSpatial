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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/9/2009 9:41:18 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A button that is a certain color with beveled edges.  The default use of this control is to work as a simple
    /// color dialog launcher that happens to show a preview of the currently selected color.
    /// </summary>
    [DefaultEvent("ColorChanged")]
    public class ColorButton : Control
    {
        #region Events

        /// <summary>
        /// Occurs when the color has changed.
        /// </summary>
        public event EventHandler ColorChanged;

        #endregion

        #region Private Variables

        private int _bevelRadius;
        private Color _color;
        private bool _isDown;
        private bool _launchDialogOnClick;
        private int _roundingRadius;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ColorButton
        /// </summary>
        public ColorButton()
        {
            _color = Color.Blue;
            _launchDialogOnClick = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the color but will not fire the ColorChanged event.
        /// </summary>
        /// <param name="color"></param>
        public void SetColorQuietly(Color color)
        {
            _color = color;
            Invalidate();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color of this button
        /// </summary>
        [Description("Gets or sets the color of this button")]
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                Invalidate();
                OnColorChanged();
            }
        }

        /// <summary>
        /// Gets or sets the floating point radius between the outside of the button and the flat central portion.
        /// </summary>
        [Description("Gets or sets the floating point radius between the outside of the button and the flat central portion.")]
        public int BevelRadius
        {
            get { return _bevelRadius; }
            set { _bevelRadius = value; }
        }

        /// <summary>
        /// Gets or sets the rounding radius that controls how rounded this button appears
        /// </summary>
        [Description("Gets or sets the rounding radius that controls how rounded this button appears")]
        public int RoundingRadius
        {
            get { return _roundingRadius; }
            set { _roundingRadius = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether this button should launch a
        /// color dialog to alter its color when it is clicked.
        /// </summary>
        [Description("Gets or sets a boolean that indicates whether this button should launch a color dialog to alter its color when it is clicked.")]
        public bool LaunchDialogOnClick
        {
            get { return _launchDialogOnClick; }
            set { _launchDialogOnClick = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the ColorChanged event
        /// </summary>
        protected virtual void OnColorChanged()
        {
            if (ColorChanged != null) ColorChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Custom drawing code
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            Brush b = new SolidBrush(_color);

            Color pressedLight = _color.Lighter(.2F);

            Color pressedDark = _color.Darker(.4F);

            Color light = _color.Lighter(.3F);

            Color dark = _color.Darker(.3F);

            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            GraphicsPath gp = new GraphicsPath();
            // Even when fully transparent, I would like to see a "glass like" reflective appearance
            LinearGradientBrush crystalBrush;
            Color cLight = Color.FromArgb(80, Color.LightCyan.Lighter(.3F));
            Color cDark = Color.FromArgb(80, Color.DarkCyan).Darker(.3F);
            if (_isDown == false)
            {
                crystalBrush = new LinearGradientBrush(new Point(0, 0), new Point(bounds.Width, bounds.Height), cLight, cDark);
            }
            else
            {
                crystalBrush = new LinearGradientBrush(new Point(0, 0), new Point(bounds.Width, bounds.Height), cDark, cLight);
            }

            LinearGradientBrush lgb;
            if (_isDown == false)
            {
                lgb = new LinearGradientBrush(new Point(0, 0), new Point(bounds.Width, bounds.Height), light, dark);
            }
            else
            {
                lgb = new LinearGradientBrush(new Point(0, 0), new Point(bounds.Width, bounds.Height), pressedDark, pressedLight);
            }

            int rad = Math.Min(Math.Min(_roundingRadius, Width / 2), Height / 2);
            gp.AddRoundedRectangle(bounds, rad);

            g.FillPath(crystalBrush, gp);
            g.FillPath(lgb, gp);
            gp.Dispose();
            if (Width < _bevelRadius * 2 || Height < _bevelRadius * 2)
            {
            }
            else
            {
                Rectangle inner = new Rectangle(bounds.Left + _bevelRadius, bounds.Top + _bevelRadius, bounds.Width - _bevelRadius * 2, bounds.Height - _bevelRadius * 2);
                gp = new GraphicsPath();
                int rRad = _roundingRadius - _bevelRadius;
                if (rRad < 0) rRad = 0;
                gp.AddRoundedRectangle(inner, rRad);

                Color cPlain = Color.FromArgb(20, Color.Cyan);
                SolidBrush back = new SolidBrush(BackColor);
                SolidBrush crystalFlat = new SolidBrush(cPlain);
                g.FillPath(back, gp);
                g.FillPath(crystalFlat, gp);
                back.Dispose();
                crystalFlat.Dispose();

                g.FillPath(b, gp);
                gp.Dispose();
            }

            b.Dispose();
        }

        /// <summary>
        /// when the mouse down event is received this also "depresses" the button
        /// </summary>
        /// <param name="e"></param>
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
        /// <param name="e"></param>
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
        /// Clicking launches a color dialog by default.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            if (_launchDialogOnClick)
            {
                ColorDialog cd = new ColorDialog();
                cd.AnyColor = true;
                cd.CustomColors = new[] { ToDialogColor(_color) };
                cd.Color = _color;
                if (cd.ShowDialog() != DialogResult.OK) return;
                Color = cd.Color;
            }

            base.OnClick(e);
        }

        private static int ToDialogColor(Color color)
        {
            return (color.R + (color.G << 8) + (color.B << 16));
        }

        /// <summary>
        /// Cancels the on paint background event to prevent flicker
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
            //OnPaint(pevent);
        }

        /// <summary>
        /// Sets up a bitmap to use as a double buffer for doing all the drawing code.
        /// </summary>
        /// <param name="e"></param>
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

        #endregion
    }
}