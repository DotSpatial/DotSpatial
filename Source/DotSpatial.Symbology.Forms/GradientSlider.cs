// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
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
    [ToolboxItem(false)]
    public class GradientSlider : Control
    {
        #region Fields

        private float _max;
        private Color _maxColor;
        private float _min;
        private Color _minColor;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientSlider"/> class.
        /// </summary>
        public GradientSlider()
        {
            _min = 0F;
            _max = 1F;
            LeftHandle = new RoundedHandle(this)
            {
                Position = .2F
            };
            RightHandle = new RoundedHandle(this)
            {
                Position = .8F
            };
            _minColor = Color.Transparent;
            _maxColor = Color.Blue;
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
        /// Gets or sets the floating point position of the left slider. This must range
        /// between 0 and 1, and to the left of the right slider, (therefore with a value lower than the right slider.)
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public RoundedHandle LeftHandle { get; set; }

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
                return _maxColor;
            }

            set
            {
                _maxColor = value;
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
        /// Gets or sets the color associated with the minimum color
        /// </summary>
        [Description("Gets or sets the color associated with the minimum value")]
        public Color MinimumColor
        {
            get
            {
                return _minColor;
            }

            set
            {
                _minColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the floating point position of the right slider. This must range
        /// between 0 and 1, and to the right of the left slider.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public RoundedHandle RightHandle { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Controls the actual drawing for this gradient slider control.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            if (Width == 0 || Height == 0) return;

            LinearGradientBrush lgb = new LinearGradientBrush(ClientRectangle, BackColor.Lighter(.2F), BackColor.Darker(.2F), LinearGradientMode.Vertical);
            g.FillRectangle(lgb, ClientRectangle);
            lgb.Dispose();

            int l = Convert.ToInt32((Width * (LeftHandle.Position - _min)) / (_max - _min));
            int r = Convert.ToInt32((Width * (RightHandle.Position - _min)) / (_max - _min));

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
                    LeftHandle.IsDragging = true;
                }

                Rectangle r = RightHandle.GetBounds();
                if (r.Contains(e.Location) && RightHandle.Visible)
                {
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
            if (RightHandle.IsDragging)
            {
                float x = e.X;
                int min = 0;
                if (LeftHandle.Visible) min = LeftHandle.Width;
                if (x > Width) x = Width;
                if (x < min) x = min;
                RightHandle.Position = _min + ((x / Width) * (_max - _min));
                if (LeftHandle.Visible)
                {
                    float lw = LeftHandle.Width / (float)Width * (_max - _min);
                    if (LeftHandle.Position > RightHandle.Position - lw)
                    {
                        LeftHandle.Position = RightHandle.Position - lw;
                    }
                }

                OnPositionChanging();
            }

            if (LeftHandle.IsDragging)
            {
                float x = e.X;
                int max = Width;
                if (RightHandle.Visible) max = Width - RightHandle.Width;
                if (x > max) x = max;
                if (x < 0) x = 0;
                LeftHandle.Position = _min + ((x / Width) * (_max - _min));
                if (RightHandle.Visible)
                {
                    float rw = RightHandle.Width / (float)Width * (_max - _min);
                    if (RightHandle.Position < LeftHandle.Position + rw)
                    {
                        RightHandle.Position = LeftHandle.Position + rw;
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
        /// Prevent flicker
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