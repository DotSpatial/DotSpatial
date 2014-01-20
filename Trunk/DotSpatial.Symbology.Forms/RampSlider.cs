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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/7/2009 2:40:12 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// RampSlider
    /// </summary>
    [DefaultEvent("ValueChanged"), DefaultProperty("Value")]
    [ToolboxBitmap(typeof(RampSlider), "RampSlider.ico")]
    public class RampSlider : Control
    {
        #region Events

        /// <summary>
        /// Occurs when the slider has been controlled to alter the value.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Private Variables

        private ColorButton _colorButton;
        private Point _dragOffset;
        private bool _flipRamp;
        private bool _flipText;
        private bool _ignore;
        private bool _invertRamp;
        private bool _isDragging;
        private double _max;
        private Color _maxColor;
        private double _min;
        private Color _minColor;
        private string _numberFormat;
        private Orientation _orientation;
        private float _rampRadius;
        private Rectangle _rampRectangle;
        private string _rampText;
        private bool _rampTextBehindRamp;
        private Color _rampTextColor;
        private Font _rampTextFont;
        private ContentAlignment _rampTextPosition;
        private bool _showMaximum;
        private bool _showMinimum;
        private bool _showTicks;
        private bool _showValue;
        private Color _sliderColor;
        private float _sliderRadius;
        private Rectangle _sliderRectangle;
        private Rectangle _textRectangle;
        private Color _tickColor;
        private float _tickSpacing;
        private double _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RampSlider
        /// </summary>
        public RampSlider()
        {
            _showMaximum = true;
            _showMinimum = true;
            _showValue = true;
            _minColor = Color.FromArgb(0, Color.Green);
            _maxColor = Color.Green;
            _sliderColor = Color.Blue;
            _rampRadius = 10;
            _sliderRadius = 4;
            _rampTextColor = Color.Black;
            _rampTextFont = base.Font;
            _rampTextPosition = ContentAlignment.MiddleCenter;
            _tickColor = Color.DarkGray;
            _value = 1;
            _max = 1;
            _tickSpacing = 5;
            _numberFormat = "#.00";
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a color button.  In this case, the ramp slider will display the opacity range for
        /// the color specified by the color button.  The color button may become transparent, but the
        /// opacity range will always show transparent to opaque, matching the RGB signature of the color
        /// from the color button.
        /// </summary>
        [Category("Connections"), Description("Gets or sets the color button that this should become the opacity slider for.")]
        public ColorButton ColorButton
        {
            get { return _colorButton; }
            set
            {
                if (_colorButton != null)
                {
                    _colorButton.ColorChanged -= ColorButtonColorChanged;
                }
                _colorButton = value;
                if (_colorButton != null)
                {
                    _colorButton.ColorChanged += ColorButtonColorChanged;
                }
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether the minimum and maximum should exchange places
        /// </summary>
        [Category("Ramp"),
         Description("Gets or sets a boolean indicating whether the minimum and maximum should exchange places")]
        public bool FlipRamp
        {
            get { return _flipRamp; }
            set
            {
                _flipRamp = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a boolean that represents whether to mirror the shape of the ramp.
        /// If the ramp is horizontal, this will change whether the top or the bottom of the
        /// ramp changes.
        /// </summary>
        [Category("Ramp"),
         Description("Gets or sets a boolean that represents whether to mirror the shape of the ramp.  If the ramp is horizontal, this will change whether the top or the bottom of the ramp changes.")]
        public bool InvertRamp
        {
            get { return _invertRamp; }
            set { _invertRamp = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to draw any labeled values on the opposite side of the ramp.
        /// </summary>
        [Category("Text"),
         Description("Gets or sets a value indicating whether to draw any labeled values on the opposite side of the ramp.")]
        public bool FlipText
        {
            get { return _flipText; }
            set { _flipText = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value of this slider.
        /// </summary>
        [Category("Behavior"),
         Description("Gets or sets the maximum value of this slider")]
        public double Maximum
        {
            get { return _max; }
            set
            {
                _max = value;
                if (_min > _max) _min = _max;
                if (_value > _max) _value = _max;
                PerformLayout(); // invalidates only if SuspendLayout is not active
            }
        }

        /// <summary>
        /// Gets or sets the color at the maximum of the ramp
        /// </summary>
        [Category("Ramp"),
         Description("Gets or sets the color at the top of the ramp.")]
        public Color MaximumColor
        {
            get { return _maxColor; }
            set
            {
                _maxColor = value;
                PerformLayout(); // invalidates only if SuspendLayout is not active
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value, and therefore the minimum position of the slider.
        /// If this is set above value or maximum, those values will be set equal to this value.
        /// </summary>
        [Category("Behavior"),
         Description("Gets or sets the minimum allowed value")]
        public double Minimum
        {
            get { return _min; }
            set
            {
                _min = value;
                if (_min > _max) _max = value;
                if (_max > _value) _value = value;
                PerformLayout(); // invalidates only if SuspendLayout is not active
            }
        }

        /// <summary>
        /// Gets or sets the color of the minimum of the ramp.
        /// </summary>
        [Category("Ramp"),
         Description("Gets or sets the color of the bottom of the ramp.")]
        public Color MinimumColor
        {
            get { return _minColor; }
            set
            {
                _minColor = value;
                PerformLayout(); // invalidates only if SuspendLayout is not active
            }
        }

        /// <summary>
        /// Gets or sets the Number format to use for the min, max and value.
        /// </summary>
        [Category("Text"),
         Description("Gets or sets the Number format to use for the min, max and value.")]
        public string NumberFormat
        {
            get { return _numberFormat; }
            set
            {
                _numberFormat = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets whether the ramp and text should be organized horizontally or vertically.
        /// </summary>
        [Category("Appearance"),
         Description("Gets or sets whether the ramp and text should be organized horizontally or vertically.")]
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the numeric radius that controls the rounding on the ramp.
        /// </summary>
        [Category("Ramp"),
         Description("Gets or sets the numeric radius that controls the rounding on the ramp.")]
        public float RampRadius
        {
            get { return _rampRadius; }
            set { _rampRadius = value; }
        }

        /// <summary>
        /// Gets or sets the text that appears behind the ramp, if any.
        /// </summary>
        [Category("Ramp Text"),
         Description("Gets or sets the text that appears behind the ramp, if any.")]
        public string RampText
        {
            get { return _rampText; }
            set { _rampText = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that, if ture, will cause the text to be drawn behind the ramp instead of in front of the ramp.
        /// This can be useful if attempting to illustrate opacity by using some text.
        /// </summary>
        [Category("Ramp Text"),
         Description("Gets or sets a boolean that, if ture, will cause the text to be drawn behind the ramp instead of in front of the ramp.  This can be useful if attempting to illustrate opacity by using some text.")]
        public bool RampTextBehindRamp
        {
            get { return _rampTextBehindRamp; }
            set { _rampTextBehindRamp = value; }
        }

        /// <summary>
        /// Controls the actual text of the ramp
        /// </summary>
        [Category("Ramp Text"),
         Description("Gets or sets the text that appears behind the ramp, if any.")]
        public Font RampTextFont
        {
            get { return _rampTextFont; }
            set { _rampTextFont = value; }
        }

        /// <summary>
        /// Gets or sets the color of the text that appears in the ramp
        /// </summary>
        [Category("Ramp Text"),
         Description("Gets or sets the color of the text ")]
        public Color RampTextColor
        {
            get { return _rampTextColor; }
            set { _rampTextColor = value; }
        }

        /// <summary>
        /// Gets or sets the positioning of ramp text in relative to the ramp itself.
        /// </summary>
        [Category("Ramp Text"),
         Description("Gets or sets the positioning of ramp text in relative to the ramp itself.")]
        public ContentAlignment RampTextAlignment
        {
            get { return _rampTextPosition; }
            set { _rampTextPosition = value; }
        }

        /// <summary>
        /// Gets or sets a boolean, that if true will label the maximum value.
        /// </summary>
        [Category("Text"),
         Description("Gets or sets a boolean, that if true will label the maximum value.")]
        public bool ShowMaximum
        {
            get { return _showMaximum; }
            set
            {
                _showMaximum = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a boolean, that if true will label the minimum value.
        /// </summary>
        [Category("Text"),
         Description("Gets or sets a boolean, that if true will label the minimum value.")]
        public bool ShowMinimum
        {
            get { return _showMinimum; }
            set
            {
                _showMinimum = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a boolean, that if true will label the chosen value.
        /// </summary>
        [Category("Appearance"),
         Description("Gets or sets a boolean, that if true will label the chosen value.")]
        public bool ShowValue
        {
            get { return _showValue; }
            set
            {
                _showValue = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether to draw tickmarks along the straight axis of the ramp.
        /// </summary>
        [Category("Appearance"),
         Description("Gets or sets a boolean indicating whether to draw tickmarks along the straight axis of the ramp.")]
        public bool ShowTicks
        {
            get { return _showTicks; }
            set
            {
                _showTicks = value;
                PerformLayout(); // invalidates only if SuspendLayout is not active
            }
        }

        /// <summary>
        /// Gets or sets the basic color of the slider.
        /// </summary>
        [Category("Slider"),
         Description("Gets or sets the basic color of the slider.")]
        public Color SliderColor
        {
            get { return _sliderColor; }
            set
            {
                _sliderColor = value;
                PerformLayout(); // invalidates only if SuspendLayout is not active
            }
        }

        /// <summary>
        /// Gets or sets the numeric radius that controls the rounding on the ramp.
        /// </summary>
        [Category("Slider"),
         Description("Gets or sets the numeric radius that controls the rounding on the ramp.")]
        public float SliderRadius
        {
            get { return _sliderRadius; }
            set { _sliderRadius = value; }
        }

        /// <summary>
        /// Gets or sets the color that will be used to draw the ticks.
        /// </summary>
        [Category("Appearance"),
         Description("Gets or sets the color that will be used to draw the ticks.")]
        public Color TickColor
        {
            get { return _tickColor; }
            set
            {
                _tickColor = value;
                PerformLayout(); // invalidates only if SuspendLayout is not active
            }
        }

        /// <summary>
        /// Gets or sets the spacing to use for the ticks.  These will be drawn between
        /// the minimum and maximum values.
        /// </summary>
        public float TickSpacing
        {
            get { return _tickSpacing; }
            set { _tickSpacing = value; }
        }

        /// <summary>
        /// Gets or sets a value ranging from Min to Max.
        /// </summary>
        [Category("Behavior"),
         Description("Gets or sets the position of the slider relative to the left side of the slider control")]
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                PerformLayout(); // invalidates only if SuspendLayout is not active
                Refresh();
            }
        }

        private void ColorButtonColorChanged(object sender, EventArgs e)
        {
            if (_ignore) return; // prevent infinite loop
            _ignore = true;
            Color c = _colorButton.Color;
            _maxColor = c.ToOpaque();
            _minColor = c.ToTransparent(0);
            _colorButton.Color = c.ToTransparent((float)Value);
            Invalidate();
            _ignore = false;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Does the actual drawing in client coordinates
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            _rampRectangle = RampBounds(g);
            if (_rampTextBehindRamp) OnDrawRampText(g, clipRectangle);
            OnDrawRamp(g, clipRectangle);
            if (_rampTextBehindRamp == false) OnDrawRampText(g, clipRectangle);
            OnDrawText(g, clipRectangle);
            OnDrawTicks(g, clipRectangle);
            OnDrawSlider(g, clipRectangle);
        }

        /// <summary>
        /// Draws the tick marks on this slider
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDrawTicks(Graphics g, Rectangle clipRectangle)
        {
            if (_showTicks == false) return;
            SizeF minSize = g.MeasureString(_min.ToString(), Font);
            SizeF maxSize = g.MeasureString(_max.ToString(), Font);
            Rectangle bounds;
            bool invert;
            if (_showMaximum || _showMinimum || _showValue)
            {
                bounds = TextBounds(g);
                invert = _flipText;
            }
            else
            {
                bounds = RampBounds(g);
                bounds.Height -= 1;
                bounds.Width -= 1;
                invert = !_flipText;
            }
            Pen p = new Pen(_tickColor);

            if (_orientation == Orientation.Horizontal)
            {
                float left = bounds.X + minSize.Width;
                float y1 = bounds.Y;
                float y2 = bounds.Y + 4;
                if (invert)
                {
                    y1 = bounds.Bottom;
                    y2 = bounds.Bottom - 4;
                }
                float span = bounds.Width - minSize.Width - maxSize.Width;
                if (span > 0)
                {
                    int count = (int)(span / _tickSpacing);
                    for (int i = 0; i < count; i++)
                    {
                        float x = left + i * _tickSpacing;
                        g.DrawLine(p, new PointF(x, y1), new PointF(x, y2));
                    }
                }
            }
            else
            {
                float top = bounds.Y + maxSize.Height;
                float x1 = bounds.X;
                float x2 = bounds.X + 4;
                if (invert)
                {
                    x1 = bounds.Right;
                    x2 = bounds.Right - 4;
                }

                float span = bounds.Height - minSize.Height - maxSize.Height;
                if (span > 0)
                {
                    int count = (int)(span / _tickSpacing);
                    for (int i = 0; i < count; i++)
                    {
                        float y = top + i * _tickSpacing;
                        g.DrawLine(p, new PointF(x1, y), new PointF(x2, y));
                    }
                }
            }
        }

        /// <summary>
        /// Draws the text that appears inside the ramp.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDrawRampText(Graphics g, Rectangle clipRectangle)
        {
            if (_orientation == Orientation.Horizontal)
            {
                RectangleF bounds = new RectangleF(_rampRectangle.X, _rampRectangle.Y, _rampRectangle.Width, _rampRectangle.Height);
                DrawRampTextHorizontal(g, bounds);
            }
            else
            {
                DrawRampTextVertical(g);
            }
        }

        /// <summary>
        /// Draws the ramp itself.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDrawRamp(Graphics g, Rectangle clipRectangle)
        {
            RectangleF bounds = new RectangleF(_rampRectangle.X, _rampRectangle.Y, _rampRectangle.Width, _rampRectangle.Height);
            if (_orientation == Orientation.Horizontal)
            {
                DrawRampHorizontal(g, bounds);
            }
            else
            {
                DrawRampVertical(g);
            }
        }

        /// <summary>
        /// Draws the slider itself in client coordinates
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDrawSlider(Graphics g, Rectangle clipRectangle)
        {
            Color light = _sliderColor.Lighter(.3F);
            Color dark = _sliderColor.Darker(.3F);
            Point tl = new Point();
            Point br = new Point();
            double val = (Value - Minimum) / (Maximum - Minimum);
            int x, y, w, h;

            if (_orientation == Orientation.Horizontal)
            {
                w = 10;
                int span = Width - w - 1;
                float vSpan = _rampRectangle.Height - _rampRadius * 2;
                y = _rampRectangle.Top;
                if (_flipRamp)
                {
                    x = (int)(span * (1 - val));
                    // h = (int)(_rampRadius * 2 + vSpan * (1 - val));
                }
                else
                {
                    x = (int)(val * span);
                }
                h = (int)(_rampRadius * 2 + vSpan * val);
                if (h < 10) h = 10;
                if (_invertRamp == false)
                {
                    y = _rampRectangle.Bottom - h;
                }
            }
            else
            {
                h = 10;
                int span = Height - h - 1;
                x = _rampRectangle.Left;
                float hSpan = _rampRectangle.Width - _rampRadius * 2;
                if (_flipRamp)
                {
                    y = (int)(span * val);
                    //w = (int)(_rampRadius * 2 + hSpan * (1 - val));
                }
                else
                {
                    y = (int)(span * (1 - val));
                }
                w = (int)(_rampRadius * 2 + hSpan * val);
                if (w < 10) w = 10;
                if (_invertRamp == false)
                {
                    x = _rampRectangle.Right - w;
                }
            }
            if (x > _rampRectangle.Width) x = _rampRectangle.Width;
            if (x < 0) x = 0;
            if (y > _rampRectangle.Height) y = _rampRectangle.Height;
            if (y < 0) y = 0;
            tl.X = x;
            tl.Y = y;
            br.X = x + w;
            br.Y = y + h;
            try
            {
                LinearGradientBrush lgb = new LinearGradientBrush(tl, br, light, dark);
                Rectangle bounds = new Rectangle(x, y, w, h);
                _sliderRectangle = bounds;
                GraphicsPath gp = new GraphicsPath();
                gp.AddRoundedRectangle(bounds, (int)Math.Round(_sliderRadius));
                g.FillPath(lgb, gp);
                g.DrawPath(Pens.Gray, gp);
                gp.Dispose();
                GraphicsPath bottomRight = new GraphicsPath();
                bottomRight.AddRoundedRectangleBottomRight(bounds, (int)Math.Round(_sliderRadius));
                g.DrawPath(Pens.DarkGray, bottomRight);
                bottomRight.Dispose();
                lgb.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Draws the text for Minimum and Maximum
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDrawText(Graphics g, Rectangle clipRectangle)
        {
            _textRectangle = TextBounds(g);
            SizeF labelSize = LabelSize(g);
            SizeF minSize = g.MeasureString(Minimum.ToString(), Font);
            SizeF maxSize = g.MeasureString(Maximum.ToString(), Font);
            Brush fontBrush = new SolidBrush(ForeColor);
            PointF centerTl = new PointF((Width - labelSize.Width) / 2, (Height - labelSize.Height) / 2);
            if (_orientation == Orientation.Vertical)
            {
                //if (Height < labelSize.Height) return;
                if (_showValue)
                {
                    g.DrawString(Value.ToString(_numberFormat), Font, fontBrush, new PointF(_textRectangle.X, centerTl.Y));
                }
                //if (Height < labelSize.Height * 3) return;
                if (_flipRamp)
                {
                    if (_showMinimum)
                    {
                        g.DrawString(Minimum.ToString(_numberFormat), Font, fontBrush, new PointF(_textRectangle.X, _textRectangle.Top));
                    }
                    if (_showMaximum)
                    {
                        g.DrawString(Maximum.ToString(_numberFormat), Font, fontBrush, new PointF(_textRectangle.X, _textRectangle.Bottom - labelSize.Height));
                    }
                }
                else
                {
                    if (_showMaximum)
                    {
                        g.DrawString(Maximum.ToString(_numberFormat), Font, fontBrush, new PointF(_textRectangle.X, _textRectangle.Top));
                    }
                    if (_showMinimum)
                    {
                        g.DrawString(Minimum.ToString(_numberFormat), Font, fontBrush, new PointF(_textRectangle.X, _textRectangle.Bottom - labelSize.Height));
                    }
                }
            }
            else
            {
                //if (Width < labelSize.Width) return;
                if (_showValue)
                {
                    g.DrawString(Value.ToString(_numberFormat), Font, fontBrush, new PointF(centerTl.X, _textRectangle.Top));
                }
                //if (Width < labelSize.Width * 3) return;
                if (_flipRamp)
                {
                    if (_showMaximum)
                    {
                        g.DrawString(Maximum.ToString(_numberFormat), Font, fontBrush, new PointF(_textRectangle.X, _textRectangle.Top));
                    }
                    if (_showMinimum)
                    {
                        g.DrawString(Minimum.ToString(_numberFormat), Font, fontBrush, new PointF(_textRectangle.Width - minSize.Width, _textRectangle.Top));
                    }
                }
                else
                {
                    if (_showMinimum)
                    {
                        g.DrawString(Minimum.ToString(_numberFormat), Font, fontBrush, new PointF(_textRectangle.X, _textRectangle.Top));
                    }
                    if (_showMaximum)
                    {
                        g.DrawString(Maximum.ToString(_numberFormat), Font, fontBrush, new PointF(_textRectangle.Width - maxSize.Width, _textRectangle.Top));
                    }
                }
            }
        }

        /// <summary>
        /// Fires the MouseDown event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_sliderRectangle.Contains(e.Location))
                {
                    _isDragging = true;
                    _dragOffset.X = e.X - _sliderRectangle.X;
                    _dragOffset.Y = e.Y - _sliderRectangle.Y;
                }
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles the Mouse up scenario to stop dragging
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
                OnValueChanged();
            }
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the Value Changed event
        /// </summary>
        protected virtual void OnValueChanged()
        {
            if (_colorButton != null)
            {
                _colorButton.Color = _colorButton.Color.ToOpaque().ToTransparent((float)Value);
            }
            if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the dragging code
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                if (_orientation == Orientation.Horizontal)
                {
                    int x = e.X - _dragOffset.X;
                    if (x > _rampRectangle.Width - _sliderRectangle.Width)
                    {
                        x = _rampRectangle.Width - _sliderRectangle.Width;
                    }
                    if (x < 0)
                    {
                        x = 0;
                    }

                    Value = Minimum + (x / (double)(_rampRectangle.Width - _sliderRectangle.Width)) * (Maximum - Minimum);
                }
                else
                {
                    int y = e.Y - _dragOffset.Y;
                    if (y > _rampRectangle.Height - _sliderRectangle.Height)
                    {
                        y = _rampRectangle.Height - _sliderRectangle.Height;
                    }
                    if (y < 0)
                    {
                        y = 0;
                    }
                    Value = Minimum + (y / (double)(_rampRectangle.Width - _sliderRectangle.Width)) * (Maximum - Minimum);
                }
                Invalidate();
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Prevent flicker
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Draw the clipped portion
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

        /// <summary>
        /// Occurs on the resize event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            Invalidate();
            base.OnResize(e);
        }

        #region Private Functions

        private void DrawRampHorizontal(Graphics g, RectangleF bounds)
        {
            PointF ptMin = new PointF();
            PointF ptMax = new PointF();
            PointF peak = new PointF();
            GraphicsPath gp = new GraphicsPath();
            float radius = _rampRadius;
            if (radius > bounds.Height / 2) radius = bounds.Height / 2F;
            if (radius > bounds.Width / 2) radius = bounds.Width / 2F;
            float w = radius * 2;
            Matrix old = g.Transform;
            if (_flipRamp)
            {
                Matrix flip = g.Transform;
                flip.Translate(2 * bounds.X + bounds.Width, 0);
                flip.Scale(-1, 1);
                g.Transform = flip;
            }
            if (_invertRamp)
            {
                Matrix invert = g.Transform;
                invert.Translate(0, 2 * bounds.Y + bounds.Height);
                invert.Scale(1, -1);
                g.Transform = invert;
            }

            ptMin.X = bounds.Left;
            ptMin.Y = bounds.Bottom - 1;

            ptMax.X = bounds.Right;
            ptMax.Y = bounds.Bottom - 1;

            peak.X = bounds.Right - 1;
            peak.Y = bounds.Top;

            double a = Math.Atan((Height - w) / (Width - w));
            if (_orientation == Orientation.Vertical)
            {
                a = Math.Atan((Width - w) / (Height - w));
            }
            float ax = (float)Math.Sin(a) * radius;
            float ay = (float)Math.Cos(a) * radius;
            float af = (float)(a * 180 / Math.PI);

            gp.AddLine(new PointF(ptMin.X + radius, ptMin.Y), new PointF(ptMax.X - radius, ptMax.Y));
            RectangleF lr = new RectangleF(ptMax.X - w, ptMax.Y - w, w, w);
            gp.AddArc(lr, 90, -90F);

            gp.AddLine(new PointF(ptMax.X, ptMax.Y - radius), new PointF(peak.X, peak.Y + radius));
            RectangleF tr = new RectangleF(peak.X - w, peak.Y, w, w);
            gp.AddArc(tr, 0F, -(90 + af));

            gp.AddLine(new PointF(peak.X - (radius + ax), peak.Y + radius - ay), new PointF(ptMin.X + radius - ax, ptMin.Y - radius - ay));
            RectangleF bl = new RectangleF(ptMin.X, ptMin.Y - w, w, w);
            gp.AddArc(bl, 270F - af, -(180F - af));

            gp.CloseFigure();

            LinearGradientBrush br = new LinearGradientBrush(ptMin, ptMax, _minColor, _maxColor);
            g.FillPath(br, gp);
            g.DrawPath(Pens.Gray, gp);
            g.Transform = old;
        }

        private void DrawRampVertical(Graphics g)
        {
            Matrix old = g.Transform;
            Matrix rot = g.Transform;
            rot.RotateAt(-90F, new PointF(_rampRectangle.X, _rampRectangle.Y));
            rot.Translate(-_rampRectangle.Height, 0);
            g.Transform = rot;
            RectangleF bounds = new RectangleF(_rampRectangle.X, _rampRectangle.Y, _rampRectangle.Height, _rampRectangle.Width);
            DrawRampHorizontal(g, bounds);
            g.Transform = old;
        }

        private void DrawRampTextHorizontal(Graphics g, RectangleF bounds)
        {
            Brush b = new SolidBrush(_rampTextColor);
            SizeF labelSize = g.MeasureString(_rampText, _rampTextFont);

            PointF position = new PointF(_rampRectangle.X, _rampRectangle.Y);

            switch (_rampTextPosition)
            {
                case ContentAlignment.BottomCenter:
                    position.X = bounds.Left + bounds.Width / 2 - labelSize.Width / 2;
                    position.Y = bounds.Bottom - labelSize.Height;
                    break;
                case ContentAlignment.BottomLeft:
                    position.X = bounds.Left;
                    position.Y = bounds.Bottom - labelSize.Height;
                    break;
                case ContentAlignment.BottomRight:
                    position.X = bounds.Right - labelSize.Width;
                    position.Y = bounds.Height - labelSize.Height;
                    break;
                case ContentAlignment.MiddleCenter:
                    position.X = bounds.Left + bounds.Width / 2 - labelSize.Width / 2;
                    position.Y = bounds.Left + bounds.Height / 2 - labelSize.Height / 2;
                    break;
                case ContentAlignment.MiddleLeft:
                    position.X = bounds.Left;
                    position.Y = bounds.Left + bounds.Height / 2 - labelSize.Height / 2;
                    break;
                case ContentAlignment.MiddleRight:
                    position.X = bounds.Right - labelSize.Width;
                    position.Y = bounds.Left + bounds.Height / 2 - labelSize.Height / 2;
                    break;
                case ContentAlignment.TopCenter:
                    position.X = bounds.Left + bounds.Width / 2 - labelSize.Width / 2;
                    position.Y = bounds.Top;
                    break;
                case ContentAlignment.TopLeft:
                    position.X = bounds.Left;
                    position.Y = bounds.Top;
                    break;
                case ContentAlignment.TopRight:
                    position.X = bounds.Right - labelSize.Width;
                    position.Y = bounds.Top;
                    break;
            }
            Matrix old = g.Transform;
            g.DrawString(_rampText, _rampTextFont, b, position);
            g.Transform = old;
        }

        private void DrawRampTextVertical(Graphics g)
        {
            Matrix old = g.Transform;
            Matrix rot = g.Transform;
            if (_invertRamp)
            {
                rot.RotateAt(90F, new PointF(_rampRectangle.X, _rampRectangle.Y));
                rot.Translate(0, -_rampRectangle.Width);
            }
            else
            {
                rot.RotateAt(-90F, new PointF(_rampRectangle.X, _rampRectangle.Y));
                rot.Translate(-_rampRectangle.Height, 0);
            }
            g.Transform = rot;
            RectangleF bounds = new RectangleF(_rampRectangle.X, _rampRectangle.Y, _rampRectangle.Height, _rampRectangle.Width);
            DrawRampTextHorizontal(g, bounds);
            g.Transform = old;
        }

        private Rectangle RampBounds(Graphics g)
        {
            Rectangle result = new Rectangle();
            if (_showMaximum || _showMinimum || _showValue)
            {
                SizeF labelSize = LabelSize(g);
                if (_orientation == Orientation.Horizontal)
                {
                    result.Width = Width;
                    result.Height = Height - (int)Math.Ceiling(labelSize.Height);
                    result.X = 0;

                    if (_flipText)
                    {
                        // Text on top
                        result.Y = Height - result.Height;
                    }
                    else
                    {
                        // Text on bottom
                        result.Y = 0;
                    }
                }
                else
                {
                    result.Height = Height;
                    result.Width = Width - (int)Math.Ceiling(labelSize.Width);
                    result.Y = 0;
                    if (_flipText)
                    {
                        result.X = Width - result.Width;
                    }
                    else
                    {
                        result.X = 0;
                    }
                }
            }
            else
            {
                result = new Rectangle(0, 0, Width, Height);
            }
            result.Width -= 1;
            result.Height -= 1;
            return result;
        }

        private SizeF LabelSize(Graphics g)
        {
            SizeF v = g.MeasureString(Value.ToString(_numberFormat), Font);
            SizeF max = g.MeasureString(Maximum.ToString(_numberFormat), Font);
            SizeF min = g.MeasureString(Maximum.ToString(_numberFormat), Font);
            SizeF result = v;
            result.Height = Math.Max(Math.Max(v.Height, max.Height), min.Height);
            result.Width = Math.Max(Math.Max(v.Width, max.Width), min.Width);
            return result;
        }

        private Rectangle TextBounds(Graphics g)
        {
            SizeF labelSize = LabelSize(g);
            Rectangle result = new Rectangle();
            if (_orientation == Orientation.Horizontal)
            {
                result.Y = 0;
                result.Width = Width;
                result.Height = (int)Math.Ceiling(labelSize.Height);
                if (_flipText)
                {
                    result.Y = 0;
                }
                else
                {
                    result.Y = Height - result.Height;
                }
            }
            else
            {
                result.Y = 0;
                result.Width = (int)Math.Ceiling(labelSize.Width);
                result.Height = Height;
                if (_flipText)
                {
                    result.X = 0;
                }
                else
                {
                    result.X = Width - result.Width;
                }
            }
            result.Width -= 1;
            result.Height -= 1;
            return result;
        }

        #endregion

        #endregion
    }
}