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
    [DefaultEvent("PositionChanging"),
    ToolboxItem(false)]
    public class GradientSlider : Control
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

        private RoundedHandle _leftHandle;
        private float _max;
        private Color _maxColor;
        private float _min;
        private Color _minColor;
        private RoundedHandle _rightHandle;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GradientControl
        /// </summary>
        public GradientSlider()
        {
            _min = 0F;
            _max = 1F;
            _leftHandle = new RoundedHandle(this);
            _leftHandle.Position = .2F;
            _rightHandle = new RoundedHandle(this);
            _rightHandle.Position = .8F;
            _minColor = Color.Transparent;
            _maxColor = Color.Blue;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the floating point position of the left slider.  This must range
        /// between 0 and 1, and to the left of the right slider, (therefore with a value lower than the right slider.)
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         TypeConverter(typeof(ExpandableObjectConverter))]
        public RoundedHandle LeftHandle
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
        public RoundedHandle RightHandle
        {
            get { return _rightHandle; }
            set
            {
                _rightHandle = value;
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
            get { return _minColor; }
            set
            {
                _minColor = value;
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
        /// Gets or sets the color associated with the maximum value.
        /// </summary>
        [Description("Gets or sets the color associated with the maximum value.")]
        public Color MaximumColor
        {
            get { return _maxColor; }
            set
            {
                _maxColor = value;
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
            if (Width == 0 || Height == 0) return;
            LinearGradientBrush lgb = new LinearGradientBrush(ClientRectangle, BackColor.Lighter(.2F), BackColor.Darker(.2F), LinearGradientMode.Vertical);
            g.FillRectangle(lgb, ClientRectangle);
            lgb.Dispose();

            int l = Convert.ToInt32((Width * (_leftHandle.Position - _min)) / (_max - _min));
            int r = Convert.ToInt32((Width * (_rightHandle.Position - _min)) / (_max - _min));

            Rectangle a = new Rectangle(0, 5, l, Height - 10);
            Rectangle b = new Rectangle(l, 5, r - l, Height - 10);
            Rectangle c = new Rectangle(r, 5, Right - r, Height - 10);

            if (a.Width > 0)
            {
                SolidBrush sb = new SolidBrush(_minColor);
                g.FillRectangle(sb, a);
                sb.Dispose();
            }
            if (b.Width > 0)
            {
                LinearGradientBrush center = new LinearGradientBrush(new Point(b.X, 0), new Point(b.Right, 0), _minColor, _maxColor);
                g.FillRectangle(center, b);
                center.Dispose();
            }
            if (c.Width > 0)
            {
                SolidBrush sb = new SolidBrush(_maxColor);
                g.FillRectangle(sb, c);
                sb.Dispose();
            }
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
                    _leftHandle.IsDragging = true;
                }
                Rectangle r = _rightHandle.GetBounds();
                if (r.Contains(e.Location) && _rightHandle.Visible)
                {
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
            if (_rightHandle.IsDragging)
            {
                float x = e.X;
                int min = 0;
                if (_leftHandle.Visible) min = _leftHandle.Width;
                if (x > Width) x = Width;
                if (x < min) x = min;
                _rightHandle.Position = _min + (x / Width) * (_max - _min);
                if (_leftHandle.Visible)
                {
                    float lw = _leftHandle.Width / (float)Width * (_max - _min);
                    if (_leftHandle.Position > _rightHandle.Position - lw)
                    {
                        _leftHandle.Position = _rightHandle.Position - lw;
                    }
                }
                OnPositionChanging();
            }
            if (_leftHandle.IsDragging)
            {
                float x = e.X;
                int max = Width;
                if (_rightHandle.Visible) max = Width - _rightHandle.Width;
                if (x > max) x = max;
                if (x < 0) x = 0;
                _leftHandle.Position = _min + (x / Width) * (_max - _min);
                if (_rightHandle.Visible)
                {
                    float rw = _rightHandle.Width / (float)Width * (_max - _min);
                    if (_rightHandle.Position < _leftHandle.Position + rw)
                    {
                        _rightHandle.Position = _leftHandle.Position + rw;
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