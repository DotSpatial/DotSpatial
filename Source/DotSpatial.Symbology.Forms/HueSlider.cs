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
    /// HueSlider.
    /// </summary>
    [DefaultEvent("PositionChanging")]
    public class HueSlider : Control
    {
        #region Fields

        private int _dx;
        private int _hueShift;
        private bool _inverted;
        private int _max;
        private int _min;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HueSlider"/> class.
        /// </summary>
        public HueSlider()
        {
            _min = 0;
            _max = 360;
            LeftHandle = new HueHandle(this)
            {
                Position = 72,
                Left = true
            };
            RightHandle = new HueHandle(this)
            {
                Position = 288
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
        ///  Gets or sets an integer value indicating how much to adjust the hue to change where wrapping occurs.
        /// </summary>
        [Description("Gets or sets an integer value indicating how much to adjust the hue to change where wrapping occurs.")]
        public int HueShift
        {
            get
            {
                return _hueShift;
            }

            set
            {
                _hueShift = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the hue pattern should be flipped.
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
        public HueHandle LeftHandle { get; set; }

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
        public int Maximum
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
        /// Gets or sets the minimum allowed value for the slider.
        /// </summary>
        [Description("Gets or sets the minimum allowed value for the slider.")]
        public int Minimum
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
        /// Gets or sets the floating point position of the right slider. This must range
        /// between 0 and 1, and to the right of the left slider.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public HueHandle RightHandle { get; set; }

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
        /// Uses the hue values from the specified start and end color to set the handle positions.
        /// </summary>
        /// <param name="startColor">The start color that represents the left hue.</param>
        /// <param name="endColor">The start color that represents the right hue.</param>
        public void SetRange(Color startColor, Color endColor)
        {
            int hStart = (int)startColor.GetHue();
            int hEnd = (int)endColor.GetHue();
            _inverted = hStart > hEnd;
            LeftValue = hStart;
            RightValue = hEnd;
        }

        /// <summary>
        /// Controls the actual drawing for this gradient slider control.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            using (GraphicsPath gp = new GraphicsPath())
            {
                Rectangle innerRect = new Rectangle(LeftHandle.Width, 3, Width - 1 - RightHandle.Width - LeftHandle.Width, Height - 1 - 6);
                gp.AddRoundedRectangle(innerRect, 2);

                if (Width == 0 || Height == 0) return;

                // Create a rounded gradient effect as the backdrop that other colors will be drawn to
                LinearGradientBrush silver = new LinearGradientBrush(ClientRectangle, BackColor.Lighter(.2F), BackColor.Darker(.6F), LinearGradientMode.Vertical);
                g.FillPath(silver, gp);
                silver.Dispose();

                using (LinearGradientBrush lgb = new LinearGradientBrush(innerRect, Color.White, Color.White, LinearGradientMode.Horizontal))
                {
                    Color[] colors = new Color[37];
                    float[] positions = new float[37];

                    for (int i = 0; i <= 36; i++)
                    {
                        int j = _inverted ? 36 - i : i;
                        colors[j] = SymbologyGlobal.ColorFromHsl(((i * 10) + _hueShift) % 360, 1, .7).ToTransparent(.7f);
                        positions[i] = i / 36f;
                    }

                    ColorBlend cb = new ColorBlend
                    {
                        Colors = colors,
                        Positions = positions
                    };
                    lgb.InterpolationColors = cb;
                    g.FillPath(lgb, gp);
                }

                g.DrawPath(Pens.Gray, gp);
            }

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