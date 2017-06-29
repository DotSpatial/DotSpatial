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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/22/2009 8:34:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// LabelAlignmentControl
    /// </summary>
    [ToolboxItem(false), DefaultEvent("ValueChanged"), DefaultProperty("Value")]
    public class LabelAlignmentPicker : Control
    {
        #region Events

        /// <summary>
        /// Occurs when the value has changed
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Private Variables

        private readonly Dictionary<ContentAlignment, LabelAlignmentButton> _buttons;
        private readonly Timer _exitTimer;
        private ContentAlignment _highlight;
        private int _padding;
        private bool _timerIsActive;
        private ContentAlignment _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LabelAlignmentControl
        /// </summary>
        public LabelAlignmentPicker()
        {
            _value = ContentAlignment.MiddleCenter;
            _padding = 5;
            _buttons = new Dictionary<ContentAlignment, LabelAlignmentButton>();
            _exitTimer = new Timer();
            _exitTimer.Interval = 100;
            _exitTimer.Tick += ExitTimerTick;
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
            _highlight = _value;
            _exitTimer.Stop();
            _timerIsActive = false;
            Invalidate();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prevent flicker
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Custom drawing
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
        /// Handles the actual drawing for this control.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clipRectangle"></param>
        protected virtual void OnDraw(Graphics g, Rectangle clipRectangle)
        {
            foreach (KeyValuePair<ContentAlignment, LabelAlignmentButton> pair in _buttons)
            {
                pair.Value.Draw(g);
            }
        }

        /// <summary>
        /// A mouse up event will alter the button that is currently selected
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            LabelAlignmentButton button;
            if (_buttons.TryGetValue(_value, out button))
            {
                if (button.Bounds.Contains(e.Location)) return;
            }
            foreach (KeyValuePair<ContentAlignment, LabelAlignmentButton> pair in _buttons)
            {
                if (pair.Value.Bounds.Contains(e.Location))
                {
                    _value = pair.Key;
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
        /// Fires the Value Changed event
        /// </summary>
        /// <param name="sender"></param>
        protected virtual void OnValueChanged(LabelAlignmentButton sender)
        {
            if (ValueChanged != null) ValueChanged(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Handles mouse movements that highlight internal buttons
        /// </summary>
        /// <param name="e"></param>
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
        /// Occurs when this is resized
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            ResizeButtons();
            base.OnResize(e);
            Invalidate();
        }

        private void ResizeButtons()
        {
            _buttons.Clear();
            int w = (Width > _padding * 2) ? (Width - _padding * 2) / 3 : Width / 3;
            int h = (Height > _padding * 2) ? (Height - _padding * 2) / 3 : Height / 3;
            int wp = w + _padding;
            int hp = h + _padding;

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
            if (_buttons.TryGetValue(_value, out button))
            {
                button.Selected = true;
            }
            if (_buttons.TryGetValue(_highlight, out button))
            {
                button.Highlighted = true;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the content alignment for this control
        /// </summary>
        public ContentAlignment Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Gets an integer value representing how much to separate the
        /// interior region of the buttons.
        /// </summary>
        public new int Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }

        #endregion
    }
}