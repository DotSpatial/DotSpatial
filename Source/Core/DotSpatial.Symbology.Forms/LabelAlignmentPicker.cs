// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// LabelAlignmentControl.
    /// </summary>
    [ToolboxItem(false)]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    public class LabelAlignmentPicker : Control
    {
        #region Fields

        private readonly Dictionary<ContentAlignment, LabelAlignmentButton> _buttons;
        private readonly Timer _exitTimer;
        private ContentAlignment _highlight;
        private bool _timerIsActive;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelAlignmentPicker"/> class.
        /// </summary>
        public LabelAlignmentPicker()
        {
            Value = ContentAlignment.MiddleCenter;
            Padding = 5;
            _buttons = new Dictionary<ContentAlignment, LabelAlignmentButton>();
            _exitTimer = new Timer
            {
                Interval = 100
            };
            _exitTimer.Tick += ExitTimerTick;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the value has changed
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an integer value representing how much to separate the
        /// interior region of the buttons.
        /// </summary>
        public new int Padding { get; set; }

        /// <summary>
        /// Gets or sets the content alignment for this control.
        /// </summary>
        public ContentAlignment Value { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the actual drawing for this control.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            foreach (KeyValuePair<ContentAlignment, LabelAlignmentButton> pair in _buttons)
            {
                pair.Value.Draw(g);
            }
        }

        /// <summary>
        /// Handles mouse movements that highlight internal buttons.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!_timerIsActive)
            {
                _timerIsActive = true;
                _exitTimer.Start();
            }

            LabelAlignmentButton button;
            if (_buttons.TryGetValue(_highlight, out button))
            {
                if (button.Bounds.Contains(e.Location)) return;
            }

            foreach (KeyValuePair<ContentAlignment, LabelAlignmentButton> pair in _buttons)
            {
                if (pair.Value.Bounds.Contains(e.Location))
                {
                    pair.Value.Highlighted = true;
                    _highlight = pair.Key;
                }
                else
                {
                    pair.Value.Highlighted = false;
                }
            }

            Invalidate();
            base.OnMouseMove(e);
        }

        /// <summary>
        /// A mouse up event will alter the button that is currently selected.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            LabelAlignmentButton button;
            if (_buttons.TryGetValue(Value, out button))
            {
                if (button.Bounds.Contains(e.Location)) return;
            }

            foreach (KeyValuePair<ContentAlignment, LabelAlignmentButton> pair in _buttons)
            {
                if (pair.Value.Bounds.Contains(e.Location))
                {
                    Value = pair.Key;
                    pair.Value.Selected = true;
                    OnValueChanged(pair.Value);
                }
                else
                {
                    pair.Value.Selected = false;
                }
            }

            base.OnMouseUp(e);
            Invalidate();
        }

        /// <summary>
        /// Custom drawing.
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
        }

        /// <summary>
        /// Occurs when this is resized.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnResize(EventArgs e)
        {
            ResizeButtons();
            base.OnResize(e);
            Invalidate();
        }

        /// <summary>
        /// Fires the Value Changed event.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        protected virtual void OnValueChanged(LabelAlignmentButton sender)
        {
            ValueChanged?.Invoke(sender, EventArgs.Empty);
        }

        private void ExitTimerTick(object sender, EventArgs e)
        {
            // as long as the mouse is over this control, keep ticking.
            if (ClientRectangle.Contains(PointToClient(MousePosition))) return;

            LabelAlignmentButton button;
            if (_buttons.TryGetValue(_highlight, out button))
            {
                button.Highlighted = false;
            }

            _highlight = Value;
            _exitTimer.Stop();
            _timerIsActive = false;
            Invalidate();
        }

        private void ResizeButtons()
        {
            _buttons.Clear();
            int w = (Width > Padding * 2) ? (Width - (Padding * 2)) / 3 : Width / 3;
            int h = (Height > Padding * 2) ? (Height - (Padding * 2)) / 3 : Height / 3;
            int wp = w + Padding;
            int hp = h + Padding;

            _buttons.Add(ContentAlignment.TopLeft, new LabelAlignmentButton(new Rectangle(0, 0, w, h), BackColor));
            _buttons.Add(ContentAlignment.TopCenter, new LabelAlignmentButton(new Rectangle(wp, 0, w, h), BackColor));
            _buttons.Add(ContentAlignment.TopRight, new LabelAlignmentButton(new Rectangle(wp * 2, 0, w, h), BackColor));
            _buttons.Add(ContentAlignment.MiddleLeft, new LabelAlignmentButton(new Rectangle(0, hp, w, h), BackColor));
            _buttons.Add(ContentAlignment.MiddleCenter, new LabelAlignmentButton(new Rectangle(wp, hp, w, h), BackColor));
            _buttons.Add(ContentAlignment.MiddleRight, new LabelAlignmentButton(new Rectangle(wp * 2, hp, w, h), BackColor));
            _buttons.Add(ContentAlignment.BottomLeft, new LabelAlignmentButton(new Rectangle(0, hp * 2, w, h), BackColor));
            _buttons.Add(ContentAlignment.BottomCenter, new LabelAlignmentButton(new Rectangle(wp, hp * 2, w, h), BackColor));
            _buttons.Add(ContentAlignment.BottomRight, new LabelAlignmentButton(new Rectangle(wp * 2, hp * 2, w, h), BackColor));
            LabelAlignmentButton button;
            if (_buttons.TryGetValue(Value, out button))
            {
                button.Selected = true;
            }

            if (_buttons.TryGetValue(_highlight, out button))
            {
                button.Highlighted = true;
            }
        }

        #endregion
    }
}