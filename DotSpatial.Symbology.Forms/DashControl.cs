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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/1/2009 9:07:49 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DashControl
    /// </summary>
    public class DashControl : Control
    {
        #region Events

        /// <summary>
        /// Occurs any time any action has occured that changes the pattern.
        /// </summary>
        public event EventHandler PatternChanged;

        #endregion

        #region Private Variables

        private readonly Timer _highlightTimer;
        private SizeF _blockSize;
        private Color _buttonDownDarkColor;
        private Color _buttonDownLitColor;
        private Color _buttonUpDarkColor;
        private Color _butttonUpLitColor;
        private List<SquareButton> _horizontalButtons;
        DashSliderHorizontal _horizontalSlider;
        private Color _lineColor;
        private double _lineWidth;
        private List<SquareButton> _verticalButtons;
        DashSliderVertical _verticalSlider;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DashControl
        /// </summary>
        public DashControl()
        {
            _blockSize.Width = 10F;
            _blockSize.Height = 10F;
            _horizontalSlider = new DashSliderHorizontal();
            _horizontalSlider.Size = new SizeF(_blockSize.Width, _blockSize.Height * 3 / 2);
            _verticalSlider = new DashSliderVertical();
            _verticalSlider.Size = new SizeF(_blockSize.Width * 3 / 2, _blockSize.Height);
            _buttonDownDarkColor = SystemColors.ControlDark;
            _buttonDownLitColor = SystemColors.ControlDark;
            _buttonUpDarkColor = SystemColors.Control;
            _butttonUpLitColor = SystemColors.Control;
            _highlightTimer = new Timer();
            _highlightTimer.Interval = 100;
            _highlightTimer.Tick += HighlightTimerTick;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the boolean pattern for the horizontal patterns that control the custom
        /// dash style.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool[] DashButtons
        {
            get
            {
                bool[] result = new bool[_horizontalButtons.Count];
                for (int i = 0; i < _horizontalButtons.Count; i++)
                {
                    result[i] = _horizontalButtons[i].IsDown;
                }
                return result;
            }
            set
            {
                SetHorizontalPattern(value);
            }
        }

        /// <summary>
        /// Gets or sets the boolean pattern for the vertical patterns that control the custom
        /// compound array.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool[] CompoundButtons
        {
            get
            {
                bool[] result = new bool[_verticalButtons.Count];
                for (int i = 0; i < _verticalButtons.Count; i++)
                {
                    result[i] = _verticalButtons[i].IsDown;
                }
                return result;
            }
            set
            {
                SetVerticalPattern(value);
            }
        }

        /// <summary>
        /// Gets or sets the color for all the buttons when they are pressed and inactive
        /// </summary>
        [Description("Gets or sets the base color for all the buttons when they are pressed and inactive")]
        public Color ButtonDownDarkColor
        {
            get { return _buttonDownDarkColor; }
            set { _buttonDownDarkColor = value; }
        }

        /// <summary>
        /// Gets or sets the base color for all the buttons when they are pressed and active
        /// </summary>
        [Description("Gets or sets the base color for all the buttons when they are pressed and active")]
        public Color ButtonDownLitColor
        {
            get { return _buttonDownLitColor; }
            set { _buttonDownLitColor = value; }
        }

        /// <summary>
        /// Gets or sets the base color for all the buttons when they are not pressed and not active.
        /// </summary>
        [Description("Gets or sets the base color for all the buttons when they are not pressed and not active.")]
        public Color ButtonUpDarkColor
        {
            get { return _buttonUpDarkColor; }
            set { _buttonUpDarkColor = value; }
        }

        /// <summary>
        /// Gets or sets the base color for all the buttons when they are not pressed but are active.
        /// </summary>
        [Description("Gets or sets the base color for all the buttons when they are not pressed but are active.")]
        public Color ButtonUpLitColor
        {
            get { return _butttonUpLitColor; }
            set { _butttonUpLitColor = value; }
        }

        /// <summary>
        /// Gets or sets the line width for the actual line being described, regardless of scale mode.
        /// </summary>
        public double LineWidth
        {
            get { return _lineWidth; }
            set { _lineWidth = value; }
        }

        /// <summary>
        /// Gets the width of a square in the same units used for the line width.
        /// </summary>
        public double SquareWidth
        {
            get { return _lineWidth * Width / _blockSize.Width; }
        }

        /// <summary>
        /// Gets the height of the square
        /// </summary>
        public double SquareHeight
        {
            get { return _lineWidth * Height / _blockSize.Height; }
        }

        /// <summary>
        /// Gets or sets the position of the sliders.  The X describes the horizontal placement
        /// of the horizontal slider, while the Y describes the vertical placement of the vertical slider.
        /// </summary>
        [Description("Gets or sets the position of the sliders.  The X describes the horizontal placement of the horizontal slider, while the Y describes the vertical placement of the vertical slider."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DashSliderHorizontal HorizontalSlider
        {
            get { return _horizontalSlider; }
            set { _horizontalSlider = value; }
        }

        /// <summary>
        /// Gets or sets the floating point size of each block in pixels.
        /// </summary>
        [Description("Gets or sets the floating point size of each block in pixels.")]
        public SizeF BlockSize
        {
            get { return _blockSize; }
            set
            {
                _blockSize = value;
                if (HorizontalSlider != null) HorizontalSlider.Size = new SizeF(_blockSize.Width, _blockSize.Height * 3 / 2);
                if (VerticalSlider != null) VerticalSlider.Size = new SizeF(_blockSize.Width * 3 / 2, _blockSize.Height);
            }
        }

        /// <summary>
        /// Gets or sets the color of the line
        /// </summary>
        [Description("Gets or sets the color that should be used for the filled sections of the line.")]
        public Color LineColor
        {
            get { return _lineColor; }
            set { _lineColor = value; }
        }

        /// <summary>
        /// Gets or sets the vertical Slider
        /// </summary>
        [Description("Gets or sets the image to use as the vertical slider.  If this is null, a simple triangle will be used."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DashSliderVertical VerticalSlider
        {
            get { return _verticalSlider; }
            set { _verticalSlider = value; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public float[] GetDashPattern()
        {
            bool pressed = true;
            List<float> pattern = new List<float>();
            float previousPosition = 0F;
            int i = 0;
            foreach (SquareButton button in _horizontalButtons)
            {
                if (button.IsDown != pressed)
                {
                    float position = (i / ((float)_verticalButtons.Count));
                    if (position == 0) position = .0000001f;
                    pattern.Add(position - previousPosition);
                    previousPosition = position;
                    pressed = !pressed;
                }
                i++;
            }
            float final = (i / (float)_verticalButtons.Count);
            if (final == 0) final = 0.000001F;
            pattern.Add(final - previousPosition);
            if (pattern.Count % 2 == 1) pattern.Add(.00001F);

            if (pattern.Count == 0) return null;
            return pattern.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        public float[] GetCompoundArray()
        {
            bool pressed = false;
            List<float> pattern = new List<float>();
            int i = 0;
            foreach (SquareButton button in _verticalButtons)
            {
                if (button.IsDown != pressed)
                {
                    float position = i / (float)_verticalButtons.Count;
                    pattern.Add(position);
                    pressed = !pressed;
                }
                i++;
            }
            if (pressed)
            {
                pattern.Add(1F);
            }
            if (pattern.Count == 0) return null;
            return pattern.ToArray();
        }

        /// <summary>
        /// Sets the pattern of squares for this pen by working with the given dash and compound patterns.
        /// </summary>
        /// <param name="stroke">Completely defines the ICartographicStroke that is being used to set the pattern.</param>
        public void SetPattern(ICartographicStroke stroke)
        {
            _lineColor = stroke.Color;
            _lineWidth = stroke.Width;

            if (stroke.DashButtons == null)
            {
                _horizontalButtons = null;
                _horizontalSlider.Position = new PointF(50F, 0F);
            }
            else
            {
                SetHorizontalPattern(stroke.DashButtons);
            }
            if (stroke.CompoundButtons == null)
            {
                _verticalButtons = null;
                _verticalSlider.Position = new PointF(0F, 20F);
            }
            else
            {
                SetVerticalPattern(stroke.CompoundButtons);
            }

            CalculatePattern();
            Invalidate();
        }

        /// <summary>
        /// Sets the horizontal pattern for this control
        /// </summary>
        /// <param name="buttonPattern"></param>
        protected virtual void SetHorizontalPattern(bool[] buttonPattern)
        {
            _horizontalSlider.Position = new PointF((buttonPattern.Length + 1) * _blockSize.Width, 0);
            _horizontalButtons = new List<SquareButton>();
            for (int i = 0; i < buttonPattern.Length; i++)
            {
                SquareButton sq = new SquareButton();
                sq.Bounds = new RectangleF((i + 1) * _blockSize.Width, 0, _blockSize.Width, _blockSize.Height);
                sq.ColorDownDark = _buttonDownDarkColor;
                sq.ColorDownLit = _buttonDownLitColor;
                sq.ColorUpDark = _buttonUpDarkColor;
                sq.ColorUpLit = _butttonUpLitColor;
                sq.IsDown = buttonPattern[i];
                _horizontalButtons.Add(sq);
            }
        }

        /// <summary>
        /// Sets the vertical pattern for this control
        /// </summary>
        /// <param name="buttonPattern"></param>
        protected virtual void SetVerticalPattern(bool[] buttonPattern)
        {
            _verticalSlider.Position = new PointF(0, (buttonPattern.Length + 1) * _blockSize.Width);
            _verticalButtons = new List<SquareButton>();
            for (int i = 0; i < buttonPattern.Length; i++)
            {
                SquareButton sq = new SquareButton();
                sq.Bounds = new RectangleF(0, (i + 1) * _blockSize.Width, _blockSize.Width, _blockSize.Height);
                sq.ColorDownDark = _buttonDownDarkColor;
                sq.ColorDownLit = _buttonDownLitColor;
                sq.ColorUpDark = _buttonUpDarkColor;
                sq.ColorUpLit = _butttonUpLitColor;
                sq.IsDown = buttonPattern[i];
                _verticalButtons.Add(sq);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs when the dash control needs to calculate the pattern
        /// </summary>
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            CalculatePattern();
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
        /// Creates a bitmap to draw to instead of drawing directly to the image.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clip = new Rectangle();
            if (clip.IsEmpty) clip = ClientRectangle;
            Bitmap bmp = new Bitmap(clip.Width, clip.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.TranslateTransform(clip.X, clip.Y);
            g.Clear(BackColor);
            OnDraw(g, e.ClipRectangle);
            g.Dispose();
            e.Graphics.DrawImage(bmp, clip, clip, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Actually controls the basic drawing control.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            Brush b = new SolidBrush(LineColor);
            foreach (SquareButton vButton in _verticalButtons)
            {
                foreach (SquareButton hButton in _horizontalButtons)
                {
                    float x = hButton.Bounds.X;
                    float y = vButton.Bounds.Y;
                    if (hButton.IsDown && vButton.IsDown)
                    {
                        g.FillRectangle(b, x, y, _blockSize.Width, _blockSize.Height);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.White, x, y, _blockSize.Width, _blockSize.Height);
                    }
                    g.DrawRectangle(Pens.Gray, x, y, _blockSize.Width, _blockSize.Height);
                }
            }
            for (int v = 0; v < Height / _blockSize.Height; v++)
            {
                float y = v * _blockSize.Height;
                if (y < clipRectangle.Y - _blockSize.Height) continue;
                if (y > clipRectangle.Bottom + _blockSize.Height) continue;
                for (int u = 0; u < Width / _blockSize.Width; u++)
                {
                    float x = u * _blockSize.Width;
                    if (x < clipRectangle.X - _blockSize.Width) continue;
                    if (x > clipRectangle.Right + _blockSize.Width) continue;
                    g.DrawRectangle(Pens.Gray, x, y, _blockSize.Width, _blockSize.Height);
                }
            }

            foreach (SquareButton button in _horizontalButtons)
            {
                button.Draw(g, clipRectangle);
            }
            foreach (SquareButton button in _verticalButtons)
            {
                button.Draw(g, clipRectangle);
            }
            _horizontalSlider.Draw(g, clipRectangle);
            _verticalSlider.Draw(g, clipRectangle);
        }

        /// <summary>
        /// Handles the mouse down event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Sliders have priority over buttons
            if (e.Button != MouseButtons.Left) return;
            if (VerticalSlider.Bounds.Contains(new PointF(e.X, e.Y)))
            {
                VerticalSlider.IsDragging = true;
                return;
                // Vertical is drawn on top, so if they are both selected, select vertical
            }
            if (HorizontalSlider.Bounds.Contains(new PointF(e.X, e.Y)))
            {
                HorizontalSlider.IsDragging = true;
                return;
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles mouse movement
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            _highlightTimer.Stop();
            // Activate buttons only if the mouse is over them.  Inactivate them otherwise.
            bool invalid = UpdateHighlight(e.Location);

            // Sliders
            RectangleF area = new RectangleF();
            if (HorizontalSlider.IsDragging)
            {
                area = HorizontalSlider.Bounds;
                PointF loc = HorizontalSlider.Position;
                loc.X = e.X;
                HorizontalSlider.Position = loc;

                area = RectangleF.Union(area, HorizontalSlider.Bounds);
            }
            area.Inflate(10F, 10F);
            if (invalid == false) Invalidate(new Region(area));
            if (VerticalSlider.IsDragging)
            {
                area = VerticalSlider.Bounds;
                PointF loc = VerticalSlider.Position;
                loc.Y = e.Y;
                VerticalSlider.Position = loc;
                area = RectangleF.Union(area, VerticalSlider.Bounds);
            }
            area.Inflate(10F, 10F);
            if (invalid == false) Invalidate(new Region(area));
            if (invalid) Invalidate();
            base.OnMouseMove(e);
            _highlightTimer.Start();
        }

        /// <summary>
        /// Handles the mouse up event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) return;
            bool invalid = false;
            bool handled = false;
            if (_horizontalSlider.IsDragging)
            {
                CalculatePattern();
                _horizontalSlider.IsDragging = false;
                invalid = true;
                handled = true;
            }
            if (_verticalSlider.IsDragging)
            {
                CalculatePattern();
                _verticalSlider.IsDragging = false;
                invalid = true;
                handled = true;
            }
            if (handled == false)
            {
                foreach (SquareButton button in _horizontalButtons)
                {
                    invalid = invalid || button.UpdatePressed(e.Location);
                }
                foreach (SquareButton button in _verticalButtons)
                {
                    invalid = invalid || button.UpdatePressed(e.Location);
                }
            }
            if (invalid)
            {
                Invalidate();
                OnPatternChanged();
            }
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the pattern changed event.
        /// </summary>
        protected virtual void OnPatternChanged()
        {
            if (PatternChanged != null) PatternChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Forces a calculation during the resizing that changes the pattern squares.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CalculatePattern();
        }

        #endregion

        #region Event Handlers

        private void HighlightTimerTick(object sender, EventArgs e)
        {
            Point pt = PointToClient(MousePosition);
            // If the mouse is still in the control, then the mouse move is enough
            // to update the highlight.
            if (ClientRectangle.Contains(pt) == false)
            {
                _highlightTimer.Stop();
                UpdateHighlight(pt);
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Updates the highlight based on mouse position.
        /// </summary>
        /// <returns></returns>
        private bool UpdateHighlight(Point location)
        {
            bool invalid = false;
            foreach (SquareButton button in _horizontalButtons)
            {
                invalid = invalid || button.UpdateLight(location);
            }
            foreach (SquareButton button in _verticalButtons)
            {
                invalid = invalid || button.UpdateLight(location);
            }
            return invalid;
        }

        private void CalculatePattern()
        {
            // Horizontal
            PointF loc = HorizontalSlider.Position;
            if (loc.X > Width) loc.X = Width;
            int hCount = (int)Math.Ceiling(loc.X / _blockSize.Width);
            loc.X = hCount * _blockSize.Width;
            HorizontalSlider.Position = loc;
            List<SquareButton> newButtonsH = new List<SquareButton>();
            int start = 1;
            if (_horizontalButtons != null)
            {
                int minLen = Math.Min(_horizontalButtons.Count, hCount - 1);
                for (int i = 0; i < minLen; i++)
                {
                    newButtonsH.Add(_horizontalButtons[i]);
                }
                start = minLen + 1;
            }
            for (int j = start; j < hCount; j++)
            {
                SquareButton sq = new SquareButton();
                sq.Bounds = new RectangleF(j * _blockSize.Width, 0, _blockSize.Width, _blockSize.Height);
                sq.ColorDownDark = _buttonDownDarkColor;
                sq.ColorDownLit = _buttonDownLitColor;
                sq.ColorUpDark = _buttonUpDarkColor;
                sq.ColorUpLit = _butttonUpLitColor;
                sq.IsDown = true;
                newButtonsH.Add(sq);
            }
            _horizontalButtons = newButtonsH;

            // Vertical
            loc = VerticalSlider.Position;
            if (loc.Y > Height) loc.Y = Height;
            int vCount = (int)Math.Ceiling(loc.Y / _blockSize.Height);
            loc.Y = (vCount) * _blockSize.Height;
            VerticalSlider.Position = loc;
            List<SquareButton> buttons = new List<SquareButton>();
            start = 1;
            if (_verticalButtons != null)
            {
                int minLen = Math.Min(_verticalButtons.Count, vCount - 1);
                for (int i = 0; i < minLen; i++)
                {
                    buttons.Add(_verticalButtons[i]);
                }
                start = minLen + 1;
            }
            for (int j = start; j < vCount; j++)
            {
                SquareButton sq = new SquareButton();
                sq.Bounds = new RectangleF(0, j * _blockSize.Height, _blockSize.Width, _blockSize.Height);
                sq.ColorDownDark = _buttonDownDarkColor;
                sq.ColorDownLit = _buttonDownLitColor;
                sq.ColorUpDark = _buttonUpDarkColor;
                sq.ColorUpLit = _butttonUpLitColor;
                sq.IsDown = true;
                buttons.Add(sq);
            }
            _verticalButtons = buttons;
        }

        #endregion
    }
}