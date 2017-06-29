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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/22/2009 11:21:12 AM
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
    /// GradientControl
    /// </summary>
    [DefaultEvent("PositionChanging")]
    public class TwoColorSlider : Control
    {
        /// <summary>
        /// Occurs as the user is adjusting the positions on either of the sliders
        /// </summary>
        public event EventHandler PositionChanging;

        /// <summary>
        /// Occurs after the user has finished adjusting the positions of either of the sliders and has released control
        /// </summary>
        public event EventHandler PositionChanged;

        #region Private Variables

        private float _dx;
        private bool _inverted;
        private TwoColorHandle _leftHandle;
        private float _max;
        private Color _maxColor;
        private float _min;
        private Color _minColor;
        private TwoColorHandle _rightHandle;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GradientControl
        /// </summary>
        public TwoColorSlider()
        {
            _min = 0;
            _max = 360;
            _leftHandle = new TwoColorHandle(this);
            _leftHandle.Position = .2F;
            _leftHandle.IsLeft = true;
            _rightHandle = new TwoColorHandle(this);
            _rightHandle.Position = .8F;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the saturation of the specified values to set the left and right values for this slider.
        /// </summary>
        /// <param name="startColor">The color that specifies the left saturation</param>
        /// <param name="endColor">The color that specifies the right saturation</param>
        public void SetSaturation(Color startColor, Color endColor)
        {
            float sStart = startColor.GetSaturation();
            float sEnd = endColor.GetSaturation();
            _inverted = sEnd < sStart;
            LeftValue = sStart;
            RightValue = sEnd;
        }

        /// <summary>
        /// Uses the lightness of hte specified values ot set the left and right values for this slider
        /// </summary>
        /// <param name="startColor">The color that specifies the left lightness</param>
        /// <param name="endColor">The color that specifies the right lightness</param>
        public void SetLightness(Color startColor, Color endColor)
        {
            float lStart = startColor.GetBrightness();
            float lEnd = endColor.GetBrightness();
            _inverted = lEnd < lStart;
            LeftValue = lStart;
            RightValue = lEnd;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the inverted values.
        /// </summary>
        public bool Inverted
        {
            get { return _inverted; }
            set
            {
                _inverted = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the floating point position of the left slider.  This must range
        /// between 0 and 1, and to the left of the right slider, (therefore with a value lower than the right slider.)
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(ExpandableObjectConverter))]
        public TwoColorHandle LeftHandle
        {
            get
            {
                return _leftHandle;
            }
            set
            {
                _leftHandle = value;
            }
        }

        /// <summary>
        /// The value represented by the left handle, taking into account
        /// whether or not the slider has been reversed.
        /// </summary>
        public float LeftValue
        {
            get
            {
                if (_inverted)
                {
                    return _max - _leftHandle.Position;
                }
                return _leftHandle.Position;
            }
            set
            {
                if (_inverted)
                {
                    _leftHandle.Position = _max - value;
                    return;
                }
                _leftHandle.Position = value;
            }
        }

        /// <summary>
        /// The value represented by the right handle, taking into account
        /// whether or not the slider has been reversed.
        /// </summary>
        public float RightValue
        {
            get
            {
                if (_inverted)
                {
                    return _max - (_rightHandle.Position);
                }
                return _rightHandle.Position;
            }
            set
            {
                if (_inverted)
                {
                    _rightHandle.Position = _max - value;
                    return;
                }
                _rightHandle.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the floating point position of the right slider.  This must range
        /// between 0 and 1, and to the right of the left slider.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(ExpandableObjectConverter))]
        public TwoColorHandle RightHandle
        {
            get { return _rightHandle; }
            set
            {
                _rightHandle = value;
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
        /// Gets or sets the maximum allowed value for the slider.
        /// </summary>
        [Description("Gets or sets the maximum allowed value for the slider.")]
        public float Maximum
        {
            get { return _max; }
            set
            {
                _max = value;
                if (_max < _rightHandle.Position) _rightHandle.Position = _max;
                if (_max < _leftHandle.Position) _leftHandle.Position = _max;
            }
        }

        /// <summary>
        /// Gets or sets the minimum allowed value for the slider.
        /// </summary>
        [Description("Gets or sets the minimum allowed value for the slider.")]
        public float Minimum
        {
            get { return _min; }
            set
            {
                _min = value;
                if (_leftHandle.Position < _min) _leftHandle.Position = _min;
                if (_rightHandle.Position < _min) _rightHandle.Position = _min;
            }
        }

        /// <summary>
        /// Gets or sets the color associated with the minimum color
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

        #endregion

        #region Protected Methods

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
        /// Controls the actual drawing for this gradient slider control.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            GraphicsPath gp = new GraphicsPath();
            Rectangle innerRect = new Rectangle(_leftHandle.Width, 3, Width - 1 - _rightHandle.Width - _leftHandle.Width,
                                                Height - 1 - 6);
            gp.AddRoundedRectangle(innerRect, 2);

            if (Width == 0 || Height == 0) return;
            // Create a rounded gradient effect as the backdrop that other colors will be drawn to
            LinearGradientBrush silver = new LinearGradientBrush(ClientRectangle, BackColor.Lighter(.2F), BackColor.Darker(.6F), LinearGradientMode.Vertical);
            g.FillPath(silver, gp);
            silver.Dispose();

            LinearGradientBrush lgb = new LinearGradientBrush(innerRect, MinimumColor, MaximumColor, LinearGradientMode.Horizontal);
            g.FillPath(lgb, gp);
            lgb.Dispose();

            g.DrawPath(Pens.Gray, gp);
            gp.Dispose();

            if (Enabled)
            {
                _leftHandle.Draw(g);
                _rightHandle.Draw(g);
            }
        }

        /// <summary>
        /// Initiates slider dragging
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Enabled)
            {
                Rectangle l = _leftHandle.GetBounds();
                if (l.Contains(e.Location) && _leftHandle.Visible)
                {
                    _dx = _leftHandle.GetBounds().Right - e.X;
                    _leftHandle.IsDragging = true;
                }
                Rectangle r = _rightHandle.GetBounds();
                if (r.Contains(e.Location) && _rightHandle.Visible)
                {
                    _dx = e.X - _rightHandle.GetBounds().Left;
                    _rightHandle.IsDragging = true;
                }
            }
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles slider dragging
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            float w = Width - _leftHandle.Width;
            if (_rightHandle.IsDragging)
            {
                float x = e.X - _dx;

                int min = 0;
                if (_leftHandle.Visible) min = _leftHandle.Width;
                if (x > w) x = w;
                if (x < min) x = min;
                _rightHandle.Position = _min + (x / w) * (_max - _min);
                if (_leftHandle.Visible)
                {
                    if (_leftHandle.Position > _rightHandle.Position)
                    {
                        _leftHandle.Position = _rightHandle.Position;
                    }
                }
                OnPositionChanging();
            }
            if (_leftHandle.IsDragging)
            {
                float x = e.X + _dx;
                int max = Width;
                if (_rightHandle.Visible) max = Width - _rightHandle.Width;
                if (x > max) x = max;
                if (x < 0) x = 0;
                _leftHandle.Position = _min + (x / w) * (_max - _min);
                if (_rightHandle.Visible)
                {
                    if (_rightHandle.Position < _leftHandle.Position)
                    {
                        _rightHandle.Position = _leftHandle.Position;
                    }
                }
                OnPositionChanging();
            }
            Invalidate();
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the mouse up situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_rightHandle.IsDragging) _rightHandle.IsDragging = false;
                if (_leftHandle.IsDragging) _leftHandle.IsDragging = false;
                OnPositionChanged();
            }
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the Position Changing event while either slider is being dragged
        /// </summary>
        protected virtual void OnPositionChanging()
        {
            if (PositionChanging != null) PositionChanging(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the Position Changed event after sliders are released
        /// </summary>
        protected virtual void OnPositionChanged()
        {
            if (PositionChanged != null) PositionChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}