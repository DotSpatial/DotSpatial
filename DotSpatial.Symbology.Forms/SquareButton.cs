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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/2/2009 8:56:17 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SquareButton
    /// </summary>
    public class SquareButton
    {
        #region Private Variables

        private RectangleF _bounds;
        private Color _colorDownDark;
        private Color _colorDownLit;
        private Color _colorUpDark;
        private Color _colorUpLit;
        private ButtonStates _state;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SquareButton
        /// </summary>
        public SquareButton()
        {
            _colorUpDark = SystemColors.ControlDark;
            _colorUpLit = SystemColors.Control;
            _colorDownDark = SystemColors.ControlDark;
            _colorDownLit = SystemColors.ControlDark;
            _state = ButtonStates.None;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Instructs this button to draw itself.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        public void Draw(Graphics g, Rectangle clipRectangle)
        {
            //Rectangle clip = Rectangle.Intersect(clipRectangle, Bounds.ToRectangle());
            //if (clip.IsEmpty) return;
            //g.TranslateTransform(Bounds.X, Bounds.Y);
            //clip.X -= (int)Bounds.X;
            //clip.Y -= (int)Bounds.Y;
            OnDraw(g, clipRectangle);
            //g.TranslateTransform(-Bounds.X, -Bounds.Y);
        }

        /// <summary>
        /// Gets the current color based on the current state.
        /// </summary>
        public Color GetCurrentColor()
        {
            Color baseColor;

            if (_state.IsPressed())
            {
                if (_state.IsLit())
                {
                    baseColor = _colorDownLit;
                }
                else
                {
                    baseColor = _colorDownDark;
                }
            }
            else
            {
                if (_state.IsLit())
                {
                    baseColor = _colorUpLit;
                }
                else
                {
                    baseColor = _colorUpDark;
                }
            }

            return baseColor;
        }

        /// <summary>
        /// Updates the mouse location and return true if the state had to change.
        /// </summary>
        /// <param name="mouseLocation">Updates this button appropriately based on the specified mouse location.</param>
        /// <returns>Boolean, true if a change was made.</returns>
        public bool UpdateLight(Point mouseLocation)
        {
            PointF pt = new PointF(mouseLocation.X, mouseLocation.Y);
            if (Bounds.Contains(pt))
            {
                if (_state.IsLit() == false)
                {
                    _state = _state.Lit();
                    return true;
                }
            }
            else
            {
                if (_state.IsLit())
                {
                    _state = _state.Darkened();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Updates the depressed nature of the button based on a mouse click in the specified location
        /// </summary>
        /// <param name="mouseLocation"></param>
        /// <returns></returns>
        public bool UpdatePressed(Point mouseLocation)
        {
            PointF pt = new PointF(mouseLocation.X, mouseLocation.Y);
            if (Bounds.Contains(pt))
            {
                _state = _state.InverseDepression();
                return true;
            }
            return false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the rectangular bounds for this button
        /// </summary>
        public virtual RectangleF Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Gets or sets the color for this button control when it is pressed but is not
        /// capturing the mouse.
        /// </summary>
        public Color ColorDownDark
        {
            get { return _colorDownDark; }
            set { _colorDownDark = value; }
        }

        /// <summary>
        /// Gets or sets the primary color for this button control when it is pressed
        /// and is capturing the mouse.
        /// </summary>
        public Color ColorDownLit
        {
            get { return _colorDownLit; }
            set { _colorDownLit = value; }
        }

        /// <summary>
        /// Gets or sets the color for this button control when it is not pressed and is not
        /// capturing the mouse.
        /// </summary>
        public Color ColorUpDark
        {
            get { return _colorUpDark; }
            set { _colorUpDark = value; }
        }

        /// <summary>
        /// Gets or sets the color for this button control when it is not pressed, but is
        /// currently capturing the mouse
        /// </summary>
        public Color ColorUpLit
        {
            get { return _colorUpLit; }
            set { _colorUpLit = value; }
        }

        /// <summary>
        /// Gets or sets the state of the button, including whether it is pressed and whether
        /// it is illuminated.
        /// </summary>
        public ButtonStates State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if this button is currently pressed
        /// </summary>
        public bool IsDown
        {
            get { return _state.IsPressed(); }
            set
            {
                if (value)
                {
                    _state = _state.Pressed();
                }
                else
                {
                    _state = _state.Raised();
                }
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if this button is currently lit
        /// </summary>
        public bool IsLit
        {
            get { return _state.IsLit(); }
            set
            {
                _state = value ? _state.Lit() : _state.Darkened();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Draws this square button.  The graphics object should be in client coordinates.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            Color baseColor = GetCurrentColor();

            DrawFill(g, baseColor);

            // Draw the rounded outline.

            // DrawBorders(g, baseColor);
        }

        private void DrawFill(Graphics g, Color baseColor)
        {
            PointF topLeft = new PointF(Bounds.X, Bounds.Y);
            PointF bottomRight = new PointF(Bounds.Right, Bounds.Bottom);
            LinearGradientBrush br;
            if (_state.IsPressed())
            {
                br = CreateGradientBrush(baseColor, bottomRight, topLeft);
            }
            else
            {
                br = CreateGradientBrush(baseColor, topLeft, bottomRight);
            }
            RectangleF inner = RectangleF.Inflate(Bounds, -1F, -1F);
            g.FillRectangle(br, inner);
            br.Dispose();
        }

        private void DrawBorders(Graphics g, Color baseColor)
        {
            Color light = baseColor.Lighter(.3F);
            Color dark = baseColor.Darker(.3F);
            Color topLeftColor = light;
            Color bottomRightColor = dark;
            if (_state.IsPressed())
            {
                topLeftColor = dark;
                bottomRightColor = light;
            }
            Pen topLeftPen = new Pen(topLeftColor);
            Pen bottomRightPen = new Pen(bottomRightColor);
            Pen middlePen = new Pen(baseColor);
            float l = Bounds.Left;
            float r = Bounds.Right;
            float t = Bounds.Top;
            float b = Bounds.Bottom;

            // Straight line parts
            g.DrawLine(topLeftPen, l + 1F, t, r - 1F, t);
            g.DrawLine(bottomRightPen, l + 1F, b, r - 1F, b);
            g.DrawLine(topLeftPen, l, t + 1F, l, b - 1F);
            g.DrawLine(bottomRightPen, r, t + 1F, r, b - 1F);

            // "rounded" corner lines
            g.DrawLine(topLeftPen, l, t + 2F, l + 2F, t);
            g.DrawLine(middlePen, r - 2F, t, r, t + 2F);
            g.DrawLine(middlePen, l, b - 2F, l + 2F, b);
            g.DrawLine(bottomRightPen, r, b - 2F, r - 2F, b);

            topLeftPen.Dispose();
            bottomRightPen.Dispose();
            middlePen.Dispose();
        }

        /// <summary>
        /// Creates a Gradient Brush
        /// </summary>
        /// <param name="color"></param>
        /// <param name="topLeft"></param>
        /// <param name="bottomRight"></param>
        /// <returns></returns>
        protected static LinearGradientBrush CreateGradientBrush(Color color, PointF topLeft, PointF bottomRight)
        {
            float b = color.GetBrightness();
            b += .3F;
            if (b > 1F) b = 1F;
            Color light = SymbologyGlobal.ColorFromHsl(color.GetHue(), color.GetSaturation(), b);
            float d = color.GetBrightness();
            d -= .3F;
            if (d < 0F) d = 0F;
            Color dark = SymbologyGlobal.ColorFromHsl(color.GetHue(), color.GetSaturation(), d);
            return new LinearGradientBrush(topLeft, bottomRight, light, dark);
        }

        #endregion
    }
}