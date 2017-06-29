// ********************************************************************************************************
// Product Name: Originally written for Sketchpad, but copied to DotSpatial.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/15/2009 10:21:05 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
    internal class AnglePicker : Control
    {
        /// <summary>
        /// Occurs when the angle is changed either by dragging or by typing a hard value.
        /// </summary>
        public event EventHandler AngleChanged;

        /// <summary>
        /// Occurs after the mouse up event when dragging the angle
        /// </summary>
        public event EventHandler AngleChosen;

        #region Private Variables

        private int _angle;
        private BorderStyle _borderStyle;
        private bool _cancelRotation;
        private Color _circleBorderColor;
        private BorderStyle _circleBorderStyle;
        private Color _circleFillColor;
        private bool _clockwise;
        private Color _knobColor;
        private int _knobRadius;
        private bool _knobVisible;
        private Color _pieFillColor;
        private int _snap;
        private int _startAngle;
        private ContentAlignment _textAlignment;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        #endregion

        #region Constructors

        /// <summary>
        /// A class designed to allow easy rotation to change the angle
        /// </summary>
        public AnglePicker()
        {
            _knobVisible = true;
            _knobRadius = 5;
            _circleBorderStyle = BorderStyle.Fixed3D;
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the angle measured in degrees, counter-clockwise from the x axis.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the angle measured in degrees, counter clockwise from the x axis.")]
        public int Angle
        {
            get
            {
                int a = _angle - _startAngle;
                if (_clockwise)
                {
                    return -a;
                }
                return a;
            }
            set
            {
                _angle = value;
                if (_clockwise) _angle = -_angle;
                _angle += _startAngle;

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the current border style for the entire picker control
        /// </summary>
        public BorderStyle BorderStyle
        {
            get { return _borderStyle; }
            set { _borderStyle = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if the values should increase in the
        /// clockwise direction instead of the counter clockwise direction
        /// </summary>
        public bool Clockwise
        {
            get { return _clockwise; }
            set { _clockwise = value; }
        }

        /// <summary>
        /// Gets or sets the fill color of the circle
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the fill color of the circle")]
        public Color CircleFillColor
        {
            get { return _circleFillColor; }
            set
            {
                _circleFillColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the border color of the circle
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the fill color of the circle")]
        public Color CircleBorderColor
        {
            get { return _circleBorderColor; }
            set
            {
                _circleBorderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the border style for the circle.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the border style for the circle.")]
        public BorderStyle CircleBorderStyle
        {
            get { return _circleBorderStyle; }
            set { _circleBorderStyle = value; }
        }

        /// <summary>
        /// Gets or sets the color of the angular pie section illustrating the angle
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the color of the angular pie section illustrating the angle")]
        public Color PieFillColor
        {
            get { return _pieFillColor; }
            set
            {
                _pieFillColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the text in the control.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the alignment of the text in the control.")]
        public ContentAlignment TextAlignment
        {
            get { return _textAlignment; }
            set
            {
                _textAlignment = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color of the circle that illustrates the position of the current angle
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the color of the circle that illustrates the position of the current angle")]
        public Color KnobColor
        {
            get { return _knobColor; }
            set
            {
                _knobColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a boolean.  If this is false, then the knob will not be drawn.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets a boolean.  If this is false, then the knob will not be drawn.")]
        public bool KnobVisible
        {
            get { return _knobVisible; }
            set
            {
                _knobVisible = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets an integer in degrees specifying the snapping tollerance for multiples of 45 degrees.
        /// </summary>
        [Category("Behavior"), Description(" Gets or sets an integer in degrees specifying the snapping tollerance for multiples of 45 degrees.")]
        public int Snap
        {
            get { return _snap; }
            set { _snap = value; }
        }

        /// <summary>
        /// Gets or sets the start angle in degrees measured counter clockwise from the X axis.
        /// For instance, for an azimuth angle that starts at the top, this should be set
        /// to 90.
        /// </summary>
        public int StartAngle
        {
            get { return _startAngle; }
            set { _startAngle = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the angle changed event
        /// </summary>
        protected virtual void OnAngleChanged()
        {
            if (AngleChanged != null) AngleChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Prevent flicker
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        private void DrawBorder(Graphics g, Rectangle r)
        {
            if (_borderStyle == BorderStyle.None) return;
            if (_borderStyle == BorderStyle.FixedSingle)
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

        /// <summary>
        /// Paints the current state of the control
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            Rectangle r = new Rectangle(0, 0, Width, Height);
            r.Inflate(1, 1);

            Rectangle circle = new Rectangle(_knobRadius, _knobRadius, Width - 2 * _knobRadius, Height - 2 * _knobRadius);

            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);

            DrawBorder(g, r);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            //string angleText = _angle.ToString() + ((char)176).ToString();
            //SizeF textSize = g.MeasureString(angleText, Font);
            //Rectangle textBox = new Rectangle(0, 0, Convert.ToInt32(textSize.Width) + 4, Convert.ToInt32(textSize.Height) + 4);

            // Erase background.
            //Brush b = new SolidBrush(BackColor);

            // 9/23/09 LCW: try to make transparent

            g.Clear(BackColor);
            // g.Clear(Color.Transparent);

            // b.Dispose();

            //GetRectangles(ref circle, ref textBox);

            Brush b = new SolidBrush(_circleFillColor);
            g.FillEllipse(b, circle);
            b.Dispose();

            Pen p = Pens.Black;
            Point center = new Point(circle.X + circle.Width / 2, circle.Y + circle.Height / 2);
            Point knob = new Point();
            double a = _angle * Math.PI / 180;

            knob.X = Convert.ToInt32(center.X + (double)circle.Width / 2 * Math.Cos(a));
            knob.Y = Convert.ToInt32(center.Y - (double)circle.Height / 2 * Math.Sin(a));

            if (_angle != 0.0)
            {
                // not entirely sure why, but this compensation needs to exist to translate
                // elliptical angles into the correct pie angle.
                double dx = knob.X - center.X;
                double dy = -(knob.Y - center.Y);
                double angle = Math.Atan(dy / dx);
                if (dx < 0) angle += Math.PI;
                if (angle > Math.PI) angle = angle - Math.PI * 2;
                int fillAngle = Convert.ToInt32(angle * 180 / Math.PI);

                b = new SolidBrush(_pieFillColor);
                if (_clockwise)
                {
                    g.FillPie(b, circle, 360 - _startAngle, Angle);
                }
                else
                {
                    g.FillPie(b, circle, (360 - _startAngle - fillAngle), fillAngle);
                }

                b.Dispose();
            }

            DrawCircleBorder(g, circle);

            g.DrawLine(p, center, knob);

            DrawKnob(g, knob);

            //b = new SolidBrush(ForeColor);
            // g.DrawString(angleText, Font, b, (float)textBox.X, (float)textBox.Y);
            // b.Dispose();

            g.Dispose();
            pe.Graphics.DrawImageUnscaled(bmp, 0, 0);

            base.OnPaint(pe);
        }

        private void GetRectangles(ref Rectangle circle, ref Rectangle textBox)
        {
            if (_textAlignment == ContentAlignment.BottomCenter || _textAlignment == ContentAlignment.BottomLeft || _textAlignment == ContentAlignment.BottomRight)
            {
                int h = Height - (textBox.Height + 4) - _knobRadius * 2; // leave room at the bottom for the text
                int x = Width / 2 - h / 2; // Center the circle on the control above the text.
                if (x < _knobRadius) x = _knobRadius;
                int y = _knobRadius;
                circle = new Rectangle(x, y, h, h);
                textBox.Y = Height - textBox.Height - 2;
                if (_textAlignment == ContentAlignment.BottomCenter) textBox.X = Width / 2 - textBox.Width / 2;
                if (_textAlignment == ContentAlignment.BottomLeft) textBox.X = 0;
                if (_textAlignment == ContentAlignment.BottomRight) textBox.X = Width - textBox.Width;
            }
            if (_textAlignment == ContentAlignment.TopCenter || _textAlignment == ContentAlignment.TopLeft || _textAlignment == ContentAlignment.TopRight)
            {
                int h = Height - textBox.Height - _knobRadius * 2;
                int y = textBox.Height + _knobRadius + 2;
                int x = Width / 2 - h / 2;
                circle = new Rectangle(x, y, h, h);
                textBox.Y = 2;
                if (_textAlignment == ContentAlignment.TopLeft) textBox.X = 0;
                if (_textAlignment == ContentAlignment.TopCenter) textBox.X = Width / 2 - textBox.Width / 2;
                if (_textAlignment == ContentAlignment.TopRight) textBox.X = Width - textBox.Width;
            }
            if (_textAlignment == ContentAlignment.MiddleLeft)
            {
                int w = Width - textBox.Width - _knobRadius * 2;
                int x = textBox.Width;
                int y = Height / 2 - w / 2;
                circle = new Rectangle(x, y, w, w);
                textBox.Y = Height / 2 - textBox.Height / 2;
                textBox.X = 2;
            }
            if (_textAlignment == ContentAlignment.MiddleRight)
            {
                int w = Width - textBox.Width - _knobRadius * 2;
                int y = Height / 2 - w / 2;
                if (y < _knobRadius) y = _knobRadius;
                int x = _knobRadius;
                circle = new Rectangle(x, y, w, w);
                textBox.Y = Height / 2 - textBox.Height / 2;
                textBox.X = Width - textBox.Width + 2;
            }
            if (_textAlignment == ContentAlignment.MiddleCenter)
            {
                int diameter;
                if (Width < Height)
                {
                    diameter = Width;
                    //y = Height / 2 - diameter / 2;
                }
                else
                {
                    diameter = Height;
                    //x = Width / 2 - diameter / 2;
                }
                textBox.Y = Height / 2 - textBox.Height / 2;
                textBox.X = Width / 2 - textBox.Width / 2;
            }
        }

        private void DrawCircleBorder(Graphics g, Rectangle circle)
        {
            Pen p;
            if (_circleBorderStyle == BorderStyle.None) return;
            if (_circleBorderStyle == BorderStyle.FixedSingle)
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

        private Color Lighten(Color color, int shift)
        {
            int r, g, b;
            r = color.R + shift;
            if (r > 255) r = 255;
            g = color.G + shift;
            if (g > 255) g = 255;
            b = color.B + shift;
            if (b > 255) b = 255;
            return Color.FromArgb(color.A, r, g, b);
        }

        private Color Darken(Color color, int shift)
        {
            int r, g, b;
            r = color.R - shift;
            if (r < 0) r = 0;
            g = color.G - shift;
            if (g < 0) g = 0;
            b = color.B - shift;
            if (b < 0) b = 0;
            return Color.FromArgb(color.A, r, g, b);
        }

        /// <summary>
        /// Handles the case where we are rotating the handle
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _cancelRotation == false)
            {
                double dx = Convert.ToDouble(e.X - Width / 2) / Width;
                double dy = Convert.ToDouble(Height / 2 - e.Y) / Height;
                if (dx == 0 && dy == 0) return;
                double angle = Math.Atan(dy / dx);
                if (dx < 0) angle += Math.PI;
                if (angle > Math.PI) angle = angle - Math.PI * 2;
                _angle = Convert.ToInt32(angle * 180 / Math.PI);

                if (_snap > 0)
                {
                    if (_angle < -180 + _snap) _angle = -180;
                    if (_angle > -135 - _snap && _angle < -135 + _snap) _angle = -135;
                    if (_angle > -90 - _snap && _angle < -90 + _snap) _angle = -90;
                    if (_angle > -45 - _snap && _angle < -45 + _snap) _angle = -45;
                    if (_angle > 0 - _snap && _angle < 0 + _snap) _angle = 0;
                    if (_angle > 45 - _snap && _angle < 45 + _snap) _angle = 45;
                    if (_angle > 90 - _snap && _angle < 90 + _snap) _angle = 90;
                    if (_angle > 135 - _snap && _angle < 135 + _snap) _angle = 135;
                    if (_angle > 180 - _snap) _angle = 180;
                }

                Invalidate();
                OnAngleChanged();
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the Mouse Up event
        /// </summary>
        /// <param name="e"></param>
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
        /// Fires the AngleChosen event
        /// </summary>
        protected virtual void OnAngleChosen()
        {
            if (AngleChosen != null) AngleChosen(this, EventArgs.Empty);
        }

        /// <summary>
        /// Forces this control to redraw while changing size.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            Invalidate();
            base.OnResize(e);
        }

        #endregion
    }
}