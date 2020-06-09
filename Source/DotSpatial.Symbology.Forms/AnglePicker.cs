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
    /// A component that allows the easy selection of a rotation situation.
    /// </summary>
    [DefaultEvent("AngleChanged")]
    internal partial class AnglePicker : Control
    {
        #region Fields

        private readonly int _knobRadius;
        private int _angle;
        private bool _cancelRotation;
        private Color _circleBorderColor;
        private Color _circleFillColor;
        private Color _knobColor;
        private bool _knobVisible;
        private Color _pieFillColor;
        private ContentAlignment _textAlignment;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AnglePicker"/> class.
        /// </summary>
        public AnglePicker()
        {
            _knobVisible = true;
            _knobRadius = 5;
            CircleBorderStyle = BorderStyle.Fixed3D;
            _circleBorderColor = Color.LightGray;
            _circleFillColor = Color.LightGray;
            _pieFillColor = Color.SteelBlue;
            _knobColor = Color.Green;
            _textAlignment = ContentAlignment.BottomCenter;
            Width = 50;
            Height = 50;
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the angle is changed either by dragging or by typing a hard value.
        /// </summary>
        public event EventHandler AngleChanged;

        /// <summary>
        /// Occurs after the mouse up event when dragging the angle
        /// </summary>
        public event EventHandler AngleChosen;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the angle measured in degrees, counter-clockwise from the x axis.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the angle measured in degrees, counter clockwise from the x axis.")]
        public int Angle
        {
            get
            {
                int a = _angle - StartAngle;
                if (Clockwise)
                {
                    return -a;
                }

                return a;
            }

            set
            {
                _angle = value;
                if (Clockwise) _angle = -_angle;
                _angle += StartAngle;

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the current border style for the entire picker control.
        /// </summary>
        public BorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the border color of the circle.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the fill color of the circle")]
        public Color CircleBorderColor
        {
            get
            {
                return _circleBorderColor;
            }

            set
            {
                _circleBorderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the border style for the circle.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the border style for the circle.")]
        public BorderStyle CircleBorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the fill color of the circle.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the fill color of the circle")]
        public Color CircleFillColor
        {
            get
            {
                return _circleFillColor;
            }

            set
            {
                _circleFillColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the values should increase in the
        /// clockwise direction instead of the counter clockwise direction.
        /// </summary>
        public bool Clockwise { get; set; }

        /// <summary>
        /// Gets or sets the color of the circle that illustrates the position of the current angle.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the color of the circle that illustrates the position of the current angle")]
        public Color KnobColor
        {
            get
            {
                return _knobColor;
            }

            set
            {
                _knobColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the knob will be drawn.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets a value indicating whether the knob will be drawn.")]
        public bool KnobVisible
        {
            get
            {
                return _knobVisible;
            }

            set
            {
                _knobVisible = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color of the angular pie section illustrating the angle.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the color of the angular pie section illustrating the angle")]
        public Color PieFillColor
        {
            get
            {
                return _pieFillColor;
            }

            set
            {
                _pieFillColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets an integer in degrees specifying the snapping tollerance for multiples of 45 degrees.
        /// </summary>
        [Category("Behavior")]
        [Description(" Gets or sets an integer in degrees specifying the snapping tollerance for multiples of 45 degrees.")]
        public int Snap { get; set; }

        /// <summary>
        /// Gets or sets the start angle in degrees measured counter clockwise from the X axis.
        /// For instance, for an azimuth angle that starts at the top, this should be set to 90.
        /// </summary>
        public int StartAngle { get; set; }

        /// <summary>
        /// Gets or sets the alignment of the text in the control.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the alignment of the text in the control.")]
        public ContentAlignment TextAlignment
        {
            get
            {
                return _textAlignment;
            }

            set
            {
                _textAlignment = value;
                Invalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the angle changed event.
        /// </summary>
        protected virtual void OnAngleChanged()
        {
            AngleChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the AngleChosen event.
        /// </summary>
        protected virtual void OnAngleChosen()
        {
            AngleChosen?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the case where we are rotating the handle.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _cancelRotation == false)
            {
                double dx = Convert.ToDouble(e.X - (Width / 2)) / Width;
                double dy = Convert.ToDouble((Height / 2) - e.Y) / Height;
                if (dx == 0 && dy == 0) return;
                double angle = Math.Atan(dy / dx);
                if (dx < 0) angle += Math.PI;
                if (angle > Math.PI) angle = angle - (Math.PI * 2);
                _angle = Convert.ToInt32(angle * 180 / Math.PI);

                if (Snap > 0)
                {
                    if (_angle < -180 + Snap) _angle = -180;
                    if (_angle > -135 - Snap && _angle < -135 + Snap) _angle = -135;
                    if (_angle > -90 - Snap && _angle < -90 + Snap) _angle = -90;
                    if (_angle > -45 - Snap && _angle < -45 + Snap) _angle = -45;
                    if (_angle > 0 - Snap && _angle < 0 + Snap) _angle = 0;
                    if (_angle > 45 - Snap && _angle < 45 + Snap) _angle = 45;
                    if (_angle > 90 - Snap && _angle < 90 + Snap) _angle = 90;
                    if (_angle > 135 - Snap && _angle < 135 + Snap) _angle = 135;
                    if (_angle > 180 - Snap) _angle = 180;
                }

                Invalidate();
                OnAngleChanged();
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the Mouse Up event.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            _cancelRotation = false;
            if (e.Button == MouseButtons.Left)
            {
                OnAngleChosen();
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Paints the current state of the control.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle r = new Rectangle(0, 0, Width, Height);
            r.Inflate(1, 1);

            Rectangle circle = new Rectangle(_knobRadius, _knobRadius, Width - (2 * _knobRadius), Height - (2 * _knobRadius));

            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);

            DrawBorder(g, r);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BackColor);

            Brush b = new SolidBrush(_circleFillColor);
            g.FillEllipse(b, circle);
            b.Dispose();

            Pen p = Pens.Black;
            Point center = new Point(circle.X + (circle.Width / 2), circle.Y + (circle.Height / 2));
            double a = _angle * Math.PI / 180;
            Point knob = new Point
            {
                X = Convert.ToInt32(center.X + ((double)circle.Width / 2 * Math.Cos(a))),
                Y = Convert.ToInt32(center.Y - ((double)circle.Height / 2 * Math.Sin(a)))
            };

            if (_angle != 0.0)
            {
                // not entirely sure why, but this compensation needs to exist to translate
                // elliptical angles into the correct pie angle.
                double dx = knob.X - center.X;
                double dy = -(knob.Y - center.Y);
                double angle = Math.Atan(dy / dx);
                if (dx < 0) angle += Math.PI;
                if (angle > Math.PI) angle = angle - (Math.PI * 2);
                int fillAngle = Convert.ToInt32(angle * 180 / Math.PI);

                b = new SolidBrush(_pieFillColor);
                if (Clockwise)
                {
                    g.FillPie(b, circle, 360 - StartAngle, Angle);
                }
                else
                {
                    g.FillPie(b, circle, 360 - StartAngle - fillAngle, fillAngle);
                }

                b.Dispose();
            }

            DrawCircleBorder(g, circle);
            g.DrawLine(p, center, knob);
            DrawKnob(g, knob);

            g.Dispose();
            e.Graphics.DrawImageUnscaled(bmp, 0, 0);

            base.OnPaint(e);
        }

        /// <summary>
        /// Prevent flicker.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Forces this control to redraw while changing size.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnResize(EventArgs e)
        {
            Invalidate();
            base.OnResize(e);
        }

        private Color Darken(Color color, int shift)
        {
            var r = color.R - shift;
            if (r < 0) r = 0;
            var g = color.G - shift;
            if (g < 0) g = 0;
            var b = color.B - shift;
            if (b < 0) b = 0;
            return Color.FromArgb(color.A, r, g, b);
        }

        private void DrawBorder(Graphics g, Rectangle r)
        {
            if (BorderStyle == BorderStyle.None) return;
            if (BorderStyle == BorderStyle.FixedSingle)
            {
                g.DrawRectangle(Pens.Black, new Rectangle(0, 0, r.Width - 1, r.Height - 1));
            }

            Point tl = new Point(0, 0);
            Point tr = new Point(r.Width - 1, 0);
            Point bl = new Point(0, r.Height - 1);
            Point br = new Point(r.Width - 1, r.Height - 1);
            if (BorderStyle == BorderStyle.Fixed3D)
            {
                g.DrawLine(Pens.White, bl, br);
                g.DrawLine(Pens.White, br, tr);
                g.DrawLine(Pens.Gray, tl, bl);
                g.DrawLine(Pens.Gray, tl, tr);
            }
        }

        private void DrawCircleBorder(Graphics g, Rectangle circle)
        {
            Pen p;
            if (CircleBorderStyle == BorderStyle.None) return;
            if (CircleBorderStyle == BorderStyle.FixedSingle)
            {
                p = new Pen(_circleBorderColor);
                g.DrawEllipse(p, circle);
                p.Dispose();
                return;
            }

            Brush b = new LinearGradientBrush(circle, Darken(_circleBorderColor, 150), Lighten(_circleBorderColor, 150), 45f);
            p = new Pen(b);
            g.DrawEllipse(p, circle);
            p.Dispose();
        }

        private void DrawKnob(Graphics g, Point location)
        {
            if (_knobVisible == false) return;
            Color light = Lighten(_knobColor, 50);
            Color dark = Darken(_knobColor, 50);
            Rectangle r = new Rectangle(location.X - _knobRadius, location.Y - _knobRadius, _knobRadius * 2, _knobRadius * 2);
            Brush b = new LinearGradientBrush(r, light, dark, 315f);
            g.FillEllipse(b, r);
            b.Dispose();
        }

        private void GetRectangles(ref Rectangle circle, ref Rectangle textBox)
        {
            if (_textAlignment == ContentAlignment.BottomCenter || _textAlignment == ContentAlignment.BottomLeft || _textAlignment == ContentAlignment.BottomRight)
            {
                int h = Height - (textBox.Height + 4) - (_knobRadius * 2); // leave room at the bottom for the text
                int x = (Width / 2) - (h / 2); // Center the circle on the control above the text.
                if (x < _knobRadius) x = _knobRadius;
                int y = _knobRadius;
                circle = new Rectangle(x, y, h, h);
                textBox.Y = Height - textBox.Height - 2;
                if (_textAlignment == ContentAlignment.BottomCenter) textBox.X = (Width / 2) - (textBox.Width / 2);
                if (_textAlignment == ContentAlignment.BottomLeft) textBox.X = 0;
                if (_textAlignment == ContentAlignment.BottomRight) textBox.X = Width - textBox.Width;
            }

            if (_textAlignment == ContentAlignment.TopCenter || _textAlignment == ContentAlignment.TopLeft || _textAlignment == ContentAlignment.TopRight)
            {
                int h = Height - textBox.Height - (_knobRadius * 2);
                int y = textBox.Height + _knobRadius + 2;
                int x = (Width / 2) - (h / 2);
                circle = new Rectangle(x, y, h, h);
                textBox.Y = 2;
                if (_textAlignment == ContentAlignment.TopLeft) textBox.X = 0;
                if (_textAlignment == ContentAlignment.TopCenter) textBox.X = (Width / 2) - (textBox.Width / 2);
                if (_textAlignment == ContentAlignment.TopRight) textBox.X = Width - textBox.Width;
            }

            if (_textAlignment == ContentAlignment.MiddleLeft)
            {
                int w = Width - textBox.Width - (_knobRadius * 2);
                int x = textBox.Width;
                int y = (Height / 2) - (w / 2);
                circle = new Rectangle(x, y, w, w);
                textBox.Y = (Height / 2) - (textBox.Height / 2);
                textBox.X = 2;
            }

            if (_textAlignment == ContentAlignment.MiddleRight)
            {
                int w = Width - textBox.Width - (_knobRadius * 2);
                int y = (Height / 2) - (w / 2);
                if (y < _knobRadius) y = _knobRadius;
                int x = _knobRadius;
                circle = new Rectangle(x, y, w, w);
                textBox.Y = (Height / 2) - (textBox.Height / 2);
                textBox.X = Width - textBox.Width + 2;
            }

            if (_textAlignment == ContentAlignment.MiddleCenter)
            {
                textBox.Y = (Height / 2) - (textBox.Height / 2);
                textBox.X = (Width / 2) - (textBox.Width / 2);
            }
        }

        private Color Lighten(Color color, int shift)
        {
            var r = color.R + shift;
            if (r > 255) r = 255;
            var g = color.G + shift;
            if (g > 255) g = 255;
            var b = color.B + shift;
            if (b > 255) b = 255;
            return Color.FromArgb(color.A, r, g, b);
        }

        #endregion
    }
}