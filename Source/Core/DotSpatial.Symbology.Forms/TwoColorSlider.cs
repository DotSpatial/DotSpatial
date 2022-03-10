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
    /// TwoColorSlider.
    /// </summary>
    [DefaultEvent("PositionChanging")]
    public class TwoColorSlider : Control
    {
        #region Fields

        private float _dx;
        private bool _inverted;
        private float _max;
        private Color _maxColor;
        private float _min;
        private Color _minColor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoColorSlider"/> class.
        /// </summary>
        public TwoColorSlider()
        {
            _min = 0;
            _max = 360;
            LeftHandle = new TwoColorHandle(this)
            {
                Position = .2F,
                IsLeft = true
            };
            RightHandle = new TwoColorHandle(this)
            {
                Position = .8F
            };
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the user has finished adjusting the positions of either of the sliders and has released control
        /// </summary>
        public event EventHandler PositionChanged;

        /// <summary>
        /// Occurs as the user is adjusting the positions on either of the sliders
        /// </summary>
        public event EventHandler PositionChanging;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the values are inverted.
        /// </summary>
        public bool Inverted
        {
            get
            {
                return _inverted;
            }

            set
            {
                _inverted = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the floating point position of the left slider. This must range
        /// between 0 and 1, and to the left of the right slider, (therefore with a value lower than the right slider.)
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TwoColorHandle LeftHandle { get; set; }

        /// <summary>
        /// Gets or sets the value represented by the left handle, taking into account
        /// whether or not the slider has been reversed.
        /// </summary>
        public float LeftValue
        {
            get
            {
                if (_inverted)
                {
                    return _max - LeftHandle.Position;
                }

                return LeftHandle.Position;
            }

            set
            {
                if (_inverted)
                {
                    LeftHandle.Position = _max - value;
                    return;
                }

                LeftHandle.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowed value for the slider.
        /// </summary>
        [Description("Gets or sets the maximum allowed value for the slider.")]
        public float Maximum
        {
            get
            {
                return _max;
            }

            set
            {
                _max = value;
                if (_max < RightHandle.Position) RightHandle.Position = _max;
                if (_max < LeftHandle.Position) LeftHandle.Position = _max;
            }
        }

        /// <summary>
        /// Gets or sets the color associated with the maximum value.
        /// </summary>
        [Description("Gets or sets the color associated with the maximum value.")]
        public Color MaximumColor
        {
            get
            {
                return _inverted ? _minColor : _maxColor;
            }

            set
            {
                if (_inverted)
                {
                    _minColor = value;
                }
                else
                {
                    _maxColor = value;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value for the slider.
        /// </summary>
        [Description("Gets or sets the minimum allowed value for the slider.")]
        public float Minimum
        {
            get
            {
                return _min;
            }

            set
            {
                _min = value;
                if (LeftHandle.Position < _min) LeftHandle.Position = _min;
                if (RightHandle.Position < _min) RightHandle.Position = _min;
            }
        }

        /// <summary>
        /// Gets or sets the color associated with the minimum color.
        /// </summary>
        [Description("Gets or sets the color associated with the minimum value")]
        public Color MinimumColor
        {
            get
            {
                if (_inverted) return _maxColor;
                return _minColor;
            }

            set
            {
                if (_inverted)
                {
                    _maxColor = value;
                }
                else
                {
                    _minColor = value;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the floating point position of the right slider. This must range
        /// between 0 and 1, and to the right of the left slider.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TwoColorHandle RightHandle { get; set; }

        /// <summary>
        /// Gets or sets the value represented by the right handle, taking into account
        /// whether or not the slider has been reversed.
        /// </summary>
        public float RightValue
        {
            get
            {
                if (_inverted)
                {
                    return _max - RightHandle.Position;
                }

                return RightHandle.Position;
            }

            set
            {
                if (_inverted)
                {
                    RightHandle.Position = _max - value;
                    return;
                }

                RightHandle.Position = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the lightness of hte specified values ot set the left and right values for this slider.
        /// </summary>
        /// <param name="startColor">The color that specifies the left lightness.</param>
        /// <param name="endColor">The color that specifies the right lightness.</param>
        public void SetLightness(Color startColor, Color endColor)
        {
            float lStart = startColor.GetBrightness();
            float lEnd = endColor.GetBrightness();
            _inverted = lEnd < lStart;
            LeftValue = lStart;
            RightValue = lEnd;
        }

        /// <summary>
        /// Uses the saturation of the specified values to set the left and right values for this slider.
        /// </summary>
        /// <param name="startColor">The color that specifies the left saturation.</param>
        /// <param name="endColor">The color that specifies the right saturation.</param>
        public void SetSaturation(Color startColor, Color endColor)
        {
            float sStart = startColor.GetSaturation();
            float sEnd = endColor.GetSaturation();
            _inverted = sEnd < sStart;
            LeftValue = sStart;
            RightValue = sEnd;
        }

        /// <summary>
        /// Controls the actual drawing for this gradient slider control.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            GraphicsPath gp = new();
            Rectangle innerRect = new(LeftHandle.Width, 3, Width - 1 - RightHandle.Width - LeftHandle.Width, Height - 1 - 6);
            gp.AddRoundedRectangle(innerRect, 2);

            if (Width == 0 || Height == 0) return;

            // Create a rounded gradient effect as the backdrop that other colors will be drawn to
            LinearGradientBrush silver = new(ClientRectangle, BackColor.Lighter(.2F), BackColor.Darker(.6F), LinearGradientMode.Vertical);
            g.FillPath(silver, gp);
            silver.Dispose();

            LinearGradientBrush lgb = new(innerRect, MinimumColor, MaximumColor, LinearGradientMode.Horizontal);
            g.FillPath(lgb, gp);
            lgb.Dispose();

            g.DrawPath(Pens.Gray, gp);
            gp.Dispose();

            if (Enabled)
            {
                LeftHandle.Draw(g);
                RightHandle.Draw(g);
            }
        }

        /// <summary>
        /// Initiates slider dragging.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Enabled)
            {
                Rectangle l = LeftHandle.GetBounds();
                if (l.Contains(e.Location) && LeftHandle.Visible)
                {
                    _dx = LeftHandle.GetBounds().Right - e.X;
                    LeftHandle.IsDragging = true;
                }

                Rectangle r = RightHandle.GetBounds();
                if (r.Contains(e.Location) && RightHandle.Visible)
                {
                    _dx = e.X - RightHandle.GetBounds().Left;
                    RightHandle.IsDragging = true;
                }
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles slider dragging.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            float w = Width - LeftHandle.Width;
            if (RightHandle.IsDragging)
            {
                float x = e.X - _dx;

                int min = 0;
                if (LeftHandle.Visible) min = LeftHandle.Width;
                if (x > w) x = w;
                if (x < min) x = min;
                RightHandle.Position = _min + ((x / w) * (_max - _min));
                if (LeftHandle.Visible)
                {
                    if (LeftHandle.Position > RightHandle.Position)
                    {
                        LeftHandle.Position = RightHandle.Position;
                    }
                }

                OnPositionChanging();
            }

            if (LeftHandle.IsDragging)
            {
                float x = e.X + _dx;
                int max = Width;
                if (RightHandle.Visible) max = Width - RightHandle.Width;
                if (x > max) x = max;
                if (x < 0) x = 0;
                LeftHandle.Position = _min + ((x / w) * (_max - _min));
                if (RightHandle.Visible)
                {
                    if (RightHandle.Position < LeftHandle.Position)
                    {
                        RightHandle.Position = LeftHandle.Position;
                    }
                }

                OnPositionChanging();
            }

            Invalidate();
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the mouse up situation.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (RightHandle.IsDragging) RightHandle.IsDragging = false;
                if (LeftHandle.IsDragging) LeftHandle.IsDragging = false;
                OnPositionChanged();
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Draw the clipped portion.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clip = e.ClipRectangle;
            if (clip.IsEmpty) clip = ClientRectangle;
            Bitmap bmp = new(clip.Width, clip.Height);
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
            // base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Fires the Position Changed event after sliders are released.
        /// </summary>
        protected virtual void OnPositionChanged()
        {
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the Position Changing event while either slider is being dragged.
        /// </summary>
        protected virtual void OnPositionChanging()
        {
            PositionChanging?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}