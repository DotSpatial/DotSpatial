// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A Color lever is a control that shows a color on a plate, and has a knob on a lever on the side of the plate for
    /// controlling the opacity, where the up position is opaque and the bottom position is transparent.
    /// </summary>
    [DefaultEvent("ColorChanging")]
    [ToolboxBitmap(typeof(ColorLever), "GradientControls.ColorLever.ico")]
    public class ColorLever : Control
    {
        #region Fields

        private Color _color;
        private bool _isDragging;
        private Color _knobColor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorLever"/> class.
        /// </summary>
        public ColorLever()
        {
            _color = SymbologyGlobal.RandomLightColor(1F);
            BorderWidth = 5;
            KnobRadius = 7;
            BarLength = 5;
            BarWidth = 5;
            _knobColor = Color.SteelBlue;
            var ttHelp = new ToolTip();
            ttHelp.SetToolTip(this, "Click to change the color, or rotate the lever to control opacity.");
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever either the color or the opacity for this control is adjusted
        /// </summary>
        public event EventHandler ColorChanged;

        /// <summary>
        /// Because the color is changed any time the opacity is changed, while the lever is being
        /// adjusted, the color is being updated as well as the opacity. Therefore, this is fired
        /// both when the color is changed directly or when the slider is being adjusted.
        /// </summary>
        public event EventHandler ColorChanging;

        /// <summary>
        /// Occurs as the opacity is being adjusted by the slider, but while the slider is
        /// still being dragged.
        /// </summary>
        public event EventHandler OpacityChanging;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the double valued angle for this control.
        /// </summary>
        [Editor(typeof(AngleEditor), typeof(UITypeEditor))]
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets the bar length.
        /// </summary>
        public int BarLength { get; set; }

        /// <summary>
        /// Gets or sets the width of the bar connecting the knob.
        /// </summary>
        public int BarWidth { get; set; }

        /// <summary>
        /// Gets or sets the border width.
        /// </summary>
        public int BorderWidth { get; set; }

        /// <summary>
        /// Gets or sets the color being displayed on the color plate.
        /// </summary>
        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the lever should be reversed, rather than simply rotating it.
        /// </summary>
        public bool Flip { get; set; }

        /// <summary>
        /// Gets or sets the knob color.
        /// </summary>
        public Color KnobColor
        {
            get
            {
                return _knobColor;
            }

            set
            {
                _knobColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the radius to use for the knob on the opacity lever.
        /// </summary>
        public int KnobRadius { get; set; }

        /// <summary>
        /// Gets or sets the opacity for the color lever. This will also
        /// adjust the knob position.
        /// </summary>
        [Editor(typeof(OpacityEditor), typeof(UITypeEditor))]
        public float Opacity
        {
            get
            {
                return _color.GetOpacity();
            }

            set
            {
                _color = _color.ToTransparent(value);
                Invalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the current transform matrix to calculate the coordinates in terms of the unrotated, unflipped image.
        /// </summary>
        /// <param name="location">The location that gets transformed.</param>
        /// <returns>The resulting standard position.</returns>
        protected Point ClientToStandard(Point location)
        {
            Point result = location;
            Matrix flipRot = new Matrix();
            Transform(flipRot);
            flipRot.Invert();
            Point[] transformed = { result };
            flipRot.TransformPoints(transformed);
            return transformed[0];
        }

        /// <summary>
        /// Fires a ColorChanged event whenver the opacity or color have been altered.
        /// </summary>
        protected virtual void OnColorChanged()
        {
            ColorChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the ColorChanging evetn whenever the color is changing either directly, or by the use of the opacity lever.
        /// </summary>
        protected virtual void OnColorChanging()
        {
            ColorChanging?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Controls the actual drawing for this gradient slider control.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            Matrix old = g.Transform;
            Matrix flipRot = g.Transform;

            Transform(flipRot);

            g.Transform = flipRot;
            DrawColorSemicircle(g);
            DrawKnob(g);
            DrawLever(g, clipRectangle);
            DrawHand(g);
            g.Transform = old;
        }

        /// <summary>
        /// Handles the mouse down position for dragging the lever.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // turn the mouse coordinates into a standardized orientation where the control looks like the letter D
                Point loc = ClientToStandard(e.Location);
                Rectangle knob = GetKnobBounds();
                if (knob.Contains(loc))
                {
                    _isDragging = true;
                    Cursor.Hide();
                }
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles the mouse move event to handle when the lever is dragged.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                // turn the mouse coordinates into a standardized orientation where the control looks like the letter D
                Point loc = ClientToStandard(e.Location);
                Point center = new Point(KnobRadius, Height / 2);
                double dx = loc.X - center.X;
                double dy = loc.Y - center.Y;
                if (dx == 0 && dy == 0) return;

                double h;
                if (dx > 0)
                {
                    h = (1 - (dy / Math.Sqrt((dx * dx) + (dy * dy)))) / 2;
                }
                else
                {
                    h = dy > 0 ? 0 : 1;
                }

                Opacity = (float)h;

                Rectangle knob = GetKnobBounds();
                if (knob.Contains(loc) == false)
                {
                    // nudge the hidden mouse so that it at least stays in the balpark of the knob.
                    center = new Point(knob.X + (knob.Width / 2), knob.Y + (knob.Height / 2));
                    dx = loc.X - center.X;
                    dy = loc.Y - center.Y;
                    double len = Math.Sqrt((dx * dx) + (dy * dy));
                    double rx = dx / len;
                    double ry = dy / len;
                    Point c = new Point(Convert.ToInt32(center.X + rx), Convert.ToInt32(center.Y + ry));
                    Point client = StandardToClient(c);
                    Point screen = PointToScreen(client);
                    Cursor.Position = screen;
                }

                OnOpacityChanging();
                OnColorChanging();
                Invalidate();
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Controls the mouse up for ending the drag movement, or possibly launching the color dialog to change the base color.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_isDragging)
                {
                    _isDragging = false;
                    Rectangle knob = GetKnobBounds();
                    Point center = new Point(knob.X + (knob.Width / 2), knob.Y + (knob.Height / 2));
                    Point client = StandardToClient(center);
                    Point screen = PointToScreen(client);
                    Cursor.Position = screen;
                    Cursor.Show();
                    OnColorChanging();
                    OnColorChanged();
                }
                else
                {
                    ColorDialog cdlg = new ColorDialog();
                    if (cdlg.ShowDialog() == DialogResult.OK)
                    {
                        _color = cdlg.Color;
                        OnColorChanging();
                        OnColorChanged();
                    }
                }

                Invalidate();
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the OpacityChanging event when the opacity is being changed.
        /// </summary>
        protected virtual void OnOpacityChanging()
        {
            OpacityChanging?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Draw the clipped portion.
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
        /// Prevent flicker.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Transforms a point from the standard orientation of the control into client coordinates.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>The resulting client coordinates.</returns>
        protected Point StandardToClient(Point location)
        {
            Point result = location;
            Matrix flipRot = new Matrix();
            Transform(flipRot);
            Point[] transformed = { result };
            flipRot.TransformPoints(transformed);
            return transformed[0];
        }

        private void DrawColorSemicircle(Graphics g)
        {
            Rectangle bounds = GetSemicircleBounds();
            if (bounds.Width <= 0 || bounds.Height <= 0) return;

            GraphicsPath gp = new GraphicsPath();

            gp.AddPie(new Rectangle(-bounds.Width, bounds.Y, bounds.Width * 2, bounds.Height), -90, 180);
            Rectangle roundBounds = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
            LinearGradientBrush lgb = new LinearGradientBrush(roundBounds, BackColor.Lighter(.5F), BackColor.Darker(.5F), LinearGradientMode.ForwardDiagonal);
            g.FillPath(lgb, gp);
            lgb.Dispose();
            gp.Dispose();

            gp = new GraphicsPath();

            Rectangle innerRound = GetInnerSemicircleBounds();
            if (innerRound.Width <= 0 || innerRound.Height <= 0) return;

            gp.AddPie(new Rectangle(innerRound.X - innerRound.Width, innerRound.Y, innerRound.Width * 2, innerRound.Height), -90, 180);
            PointF center = new PointF(innerRound.X + (innerRound.Width / 2), innerRound.Y + (innerRound.Height / 2));
            float x = center.X - innerRound.Width;
            float y = center.Y - innerRound.Height;
            float w = innerRound.Width * 2;
            float h = innerRound.Height * 2;
            RectangleF circum = new RectangleF(x, y, w, h);

            GraphicsPath coloring = new GraphicsPath();
            coloring.AddEllipse(circum);
            PathGradientBrush pgb = new PathGradientBrush(coloring)
            {
                CenterColor = _color.Lighter(.2F),
                SurroundColors = new[] { _color.Darker(.2F) },
                CenterPoint = new PointF(innerRound.X + 3, innerRound.Y + (innerRound.Height / 3))
            };
            g.FillPath(pgb, gp);
            lgb.Dispose();
            gp.Dispose();
        }

        private void DrawHand(Graphics g)
        {
            if (_isDragging == false) return;

            Rectangle r = GetKnobBounds();
            Icon ico = SymbologyFormsImages.Pan;
            if (ico != null) g.DrawIcon(ico, r.X + (r.Width / 2) - (ico.Width / 2), r.Y + (r.Height / 2) - (ico.Height / 2));
        }

        private void DrawKnob(Graphics g)
        {
            Rectangle bounds = GetKnobBounds();
            LinearGradientBrush lgb = new LinearGradientBrush(bounds, _knobColor.Lighter(.3F), _knobColor.Darker(.3F), LinearGradientMode.ForwardDiagonal);
            g.FillEllipse(lgb, bounds);
            lgb.Dispose();
        }

        private void DrawLever(Graphics g, Rectangle clipRectangle)
        {
            Point start = new Point(KnobRadius, Height / 2);
            Rectangle knob = GetKnobBounds();
            PointF center = new PointF(knob.X + KnobRadius, knob.Y + KnobRadius);
            double dx = center.X - start.X;
            double dy = center.Y - start.Y;
            double len = Math.Sqrt((dx * dx) + (dy * dy));
            double sx = dx / len;
            double sy = dy / len;
            PointF kJoint = new PointF((float)(center.X - (sx * KnobRadius)), (float)(center.Y - (sy * KnobRadius)));
            PointF sJoint = new PointF((float)(center.X - (sx * (KnobRadius + BarLength))), (float)(center.Y - (sy * (BarLength + KnobRadius))));
            Pen back = new Pen(BackColor.Darker(.2F), BarWidth)
            {
                EndCap = LineCap.Round,
                StartCap = LineCap.Round
            };
            Pen front = new Pen(BackColor, (float)BarWidth / 2)
            {
                EndCap = LineCap.Round,
                StartCap = LineCap.Round
            };
            g.DrawLine(back, sJoint, kJoint);
            g.DrawLine(front, sJoint, kJoint);
            back.Dispose();
            front.Dispose();
        }

        private Rectangle GetInnerSemicircleBounds()
        {
            Rectangle result = GetSemicircleBounds();
            result.X += BorderWidth;
            result.Width -= BorderWidth * 2;
            result.Y += BorderWidth;
            result.Height -= BorderWidth * 2;

            return result;
        }

        private double GetKnobAngle()
        {
            return Math.Acos(((double)_color.GetOpacity() * 2) - 1);
        }

        private Rectangle GetKnobBounds()
        {
            Rectangle result = new Rectangle
            {
                Width = KnobRadius * 2,
                Height = KnobRadius * 2
            };
            double scale = Height - (KnobRadius * 2) - 1;
            result.Y = Convert.ToInt32((1 - _color.GetOpacity()) * scale);
            double angle = GetKnobAngle();
            scale = Width - (KnobRadius * 2) - 1;
            result.X = Convert.ToInt32(scale * Math.Sin(angle));
            return result;
        }

        // calculate so that the bar can rotate freely all the way around the rotation axis.
        private Rectangle GetSemicircleBounds()
        {
            int l = BarLength + (KnobRadius * 2);
            Rectangle result = new Rectangle(0, l, Width - l, Height - (2 * l));
            return result;
        }

        private void Transform(Matrix flipRot)
        {
            flipRot.Translate(Width / 2F, Height / 2F);
            if (Flip)
            {
                flipRot.Scale(-1F, 1F);
            }

            flipRot.Rotate(-(float)Angle);
            double ang = Angle * Math.PI / 180;
            float scale = (float)(1 / (1 + ((Math.Sqrt(2) - 1) * Math.Abs(Math.Sin(ang)))));
            flipRot.Scale(scale, scale); // A rotated square would go outside the bounds of the control, so resize.
            flipRot.Translate(-Width / 2F, -Height / 2F);
        }

        #endregion
    }
}