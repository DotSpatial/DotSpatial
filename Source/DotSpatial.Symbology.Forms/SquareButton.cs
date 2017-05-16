// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SquareButton
    /// </summary>
    public class SquareButton
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SquareButton"/> class.
        /// </summary>
        public SquareButton()
        {
            ColorUpDark = SystemColors.ControlDark;
            ColorUpLit = SystemColors.Control;
            ColorDownDark = SystemColors.ControlDark;
            ColorDownLit = SystemColors.ControlDark;
            State = ButtonStates.None;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the rectangular bounds for this button.
        /// </summary>
        public virtual RectangleF Bounds { get; set; }

        /// <summary>
        /// Gets or sets the color for this button control when it is pressed but is not
        /// capturing the mouse.
        /// </summary>
        public Color ColorDownDark { get; set; }

        /// <summary>
        /// Gets or sets the primary color for this button control when it is pressed
        /// and is capturing the mouse.
        /// </summary>
        public Color ColorDownLit { get; set; }

        /// <summary>
        /// Gets or sets the color for this button control when it is not pressed and is not
        /// capturing the mouse.
        /// </summary>
        public Color ColorUpDark { get; set; }

        /// <summary>
        /// Gets or sets the color for this button control when it is not pressed, but is
        /// currently capturing the mouse
        /// </summary>
        public Color ColorUpLit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this button is currently pressed.
        /// </summary>
        public bool IsDown
        {
            get
            {
                return State.IsPressed();
            }

            set
            {
                State = value ? State.Pressed() : State.Raised();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button is currently lit.
        /// </summary>
        public bool IsLit
        {
            get
            {
                return State.IsLit();
            }

            set
            {
                State = value ? State.Lit() : State.Darkened();
            }
        }

        /// <summary>
        /// Gets or sets the state of the button, including whether it is pressed and whether
        /// it is illuminated.
        /// </summary>
        public ButtonStates State { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Instructs this button to draw itself.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        public void Draw(Graphics g, Rectangle clipRectangle)
        {
            OnDraw(g, clipRectangle);
        }

        /// <summary>
        /// Gets the current color based on the current state.
        /// </summary>
        /// <returns>The current color.</returns>
        public Color GetCurrentColor()
        {
            if (State.IsPressed())
            {
                return State.IsLit() ? ColorDownLit : ColorDownDark;
            }

            return State.IsLit() ? ColorUpLit : ColorUpDark;
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
                if (State.IsLit() == false)
                {
                    State = State.Lit();
                    return true;
                }
            }
            else
            {
                if (State.IsLit())
                {
                    State = State.Darkened();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the depressed nature of the button based on a mouse click in the specified location.
        /// </summary>
        /// <param name="mouseLocation">The location where the mouse was clicked.</param>
        /// <returns>True, if the mouse was clicked inside the button.</returns>
        public bool UpdatePressed(Point mouseLocation)
        {
            PointF pt = new PointF(mouseLocation.X, mouseLocation.Y);
            if (Bounds.Contains(pt))
            {
                State = State.InverseDepression();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a Gradient Brush.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="topLeft">The top left point.</param>
        /// <param name="bottomRight">The bottom right point.</param>
        /// <returns>A linear gradient brush.</returns>
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

        /// <summary>
        /// Draws this square button. The graphics object should be in client coordinates.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            Color baseColor = GetCurrentColor();
            DrawFill(g, baseColor);
        }

        private void DrawBorders(Graphics g, Color baseColor)
        {
            Color light = baseColor.Lighter(.3F);
            Color dark = baseColor.Darker(.3F);
            Color topLeftColor = light;
            Color bottomRightColor = dark;
            if (State.IsPressed())
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

        private void DrawFill(Graphics g, Color baseColor)
        {
            PointF topLeft = new PointF(Bounds.X, Bounds.Y);
            PointF bottomRight = new PointF(Bounds.Right, Bounds.Bottom);
            RectangleF inner = RectangleF.Inflate(Bounds, -1F, -1F);
            using (var br = State.IsPressed() ? CreateGradientBrush(baseColor, bottomRight, topLeft) : CreateGradientBrush(baseColor, topLeft, bottomRight))
            {
                g.FillRectangle(br, inner);
            }
        }

        #endregion
    }
}