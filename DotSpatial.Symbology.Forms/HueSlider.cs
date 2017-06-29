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
    public class HueSlider : Control
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

        private int _dx;
        private int _hueShift;
        private bool _inverted;
        private HueHandle _leftHandle;
        private int _max;
        private int _min;
        private HueHandle _rightHandle;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GradientControl
        /// </summary>
        public HueSlider()
        {
            _min = 0;
            _max = 360;
            _leftHandle = new HueHandle(this);
            _leftHandle.Position = 72;
            _leftHandle.Left = true;
            _rightHandle = new HueHandle(this);
            _rightHandle.Position = 288;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Uses the hue values from the specified start and end color to set the handle positions.
        /// </summary>
        /// <param name="startColor">The start color that represents the left hue</param>
        /// <param name="endColor">The start color that represents the right hue</param>
        public void SetRange(Color startColor, Color endColor)
        {
            int hStart = (int)startColor.GetHue();
            int hEnd = (int)endColor.GetHue();
            _inverted = (hStart > hEnd);
            LeftValue = hStart;
            RightValue = hEnd;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating whether the hue pattern should be flipped.
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
        ///  Gets or sets an integer value indicating how much to adjust the hue to change where wrapping occurs.
        /// </summary>
        [Description("Gets or sets an integer value indicating how much to adjust the hue to change where wrapping occurs.")]
        public int HueShift
        {
            get { return _hueShift; }
            set
            {
                _hueShift = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the floating point position of the left slider.  This must range
        /// between 0 and 1, and to the left of the right slider, (therefore with a value lower than the right slider.)
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(ExpandableObjectConverter))]
        public HueHandle LeftHandle
        {
            get { return _leftHandle; }
            set
            {
                _leftHandle = value;
            }
        }

        /// <summary>
        /// Gets or sets the floating point position of the right slider.  This must range
        /// between 0 and 1, and to the right of the left slider.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(ExpandableObjectConverter))]
        public HueHandle RightHandle
        {
            get { return _rightHandle; }
            set
            {
                _rightHandle = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum allowed value for the slider.
        /// </summary>
        [Description("Gets or sets the maximum allowed value for the slider.")]
        public int Maximum
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
        public int Minimum
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

            LinearGradientBrush lgb = new LinearGradientBrush(innerRect, Color.White, Color.White, LinearGradientMode.Horizontal);
            Color[] colors = new Color[37];
            float[] positions = new float[37];

            for (int i = 0; i <= 36; i++)
            {
                int j = _inverted ? 36 - i : i;
                colors[j] = SymbologyGlobal.ColorFromHsl((i * 10 + _hueShift) % 360, 1, .7).ToTransparent(.7f);
                positions[i] = i / 36f;
            }

            ColorBlend cb = new ColorBlend();
            cb.Colors = colors;
            cb.Positions = positions;
            lgb.InterpolationColors = cb;
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