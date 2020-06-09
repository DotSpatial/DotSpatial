// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// ScrollingControl that provides autoscroll and custom draw that won't crash mono.
    /// </summary>
    [ToolboxItem(false)]
    public class ScrollingControl : Control
    {
        #region Fields
        private readonly Brush _controlBrush;
        private Brush _backcolorBrush;
        private Brush _backImageBrush;
        private Rectangle _controlRectangle;
        private Rectangle _documentRectangle;
        private bool _firstDrawing;
        private Size _pageSize;
        private Label _lblCorner;
        private HScrollBar _scrHorizontal;
        private VScrollBar _scrVertical;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollingControl"/> class.
        /// </summary>
        public ScrollingControl()
        {
            InitializeComponent();
            _backcolorBrush = new SolidBrush(base.BackColor);
            _controlBrush = new SolidBrush(SystemColors.Control);
            if (base.BackgroundImage != null)
            {
                _backImageBrush = new TextureBrush(base.BackgroundImage);
            }

            MinimumSize = new Size(5, 5);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the base drawing content has been rendered to the page.
        /// </summary>
        public event EventHandler<PaintEventArgs> Initialized;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color to use for this control.
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }

            set
            {
                _backcolorBrush?.Dispose();
                _backcolorBrush = new SolidBrush(value);
                base.BackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the background image for this control.
        /// </summary>
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }

            set
            {
                base.BackgroundImage = value;
                _backImageBrush?.Dispose();
                if (value == null) return;
                _backImageBrush = new TextureBrush(BackgroundImage);
                Size s = _pageSize;
                if (s.Width < BackgroundImage.Width) s.Width = BackgroundImage.Width;
                if (s.Height < BackgroundImage.Height) s.Height = BackgroundImage.Height;
                _pageSize = s;
            }
        }

        /// <summary>
        /// Gets or sets the rectangular region of the control in page coordinates.
        /// </summary>
        public Rectangle ControlRectangle
        {
            get
            {
                return _controlRectangle;
            }

            set
            {
                _controlRectangle = value;
            }
        }

        /// <summary>
        /// Gets or sets the rectangle for the entire content, whether on the page buffer or not. X and Y for this are always 0.
        /// </summary>
        public virtual Rectangle DocumentRectangle
        {
            get
            {
                return _documentRectangle;
            }

            set
            {
                _documentRectangle = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not horizontal scrolling is enabled.
        /// </summary>
        public bool HorizontalScrollEnabled
        {
            get
            {
                return _scrHorizontal.Enabled;
            }

            set
            {
                _scrHorizontal.Enabled = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the page for this control has been drawn.
        /// </summary>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// Gets or sets the page image being used as a buffer. This is useful
        /// for content changes that need to be made rapidly. First refresh
        /// a small region of this page, and then invalidate the client rectangle.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Bitmap Page { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the scrolling should be reset on every resize or not.
        /// </summary>
        public bool ResetOnResize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the vertical scroll should be permitted.
        /// </summary>
        public bool VerticalScrollEnabled
        {
            get
            {
                return _scrVertical.Enabled;
            }

            set
            {
                _scrVertical.Enabled = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a rectangle in document coordinates for the specified rectangle in client coordinates.
        /// </summary>
        /// <param name="rect">Rectangle in client coordinates.</param>
        /// <returns>Rectangle in document coordinates.</returns>
        public Rectangle ClientToDocument(Rectangle rect)
        {
            return new Rectangle(rect.X + _controlRectangle.X, rect.Y + _controlRectangle.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Translates a rectangle from document coordinates to coordinates relative to the client control.
        /// </summary>
        /// <param name="rect">Rectangle in document coordinates.</param>
        /// <returns>Rectangle in client coordinates.</returns>
        public Rectangle DocumentToClient(Rectangle rect)
        {
            return new Rectangle(rect.X - _controlRectangle.X, rect.Y - _controlRectangle.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Recalculates the size and visibility of the scroll bars based on the current document.
        /// </summary>
        public void ResetScroll()
        {
            _controlRectangle.Width = ClientRectangle.Width;
            _controlRectangle.Height = ClientRectangle.Height;
            int dw = _documentRectangle.Width;
            int dh = _documentRectangle.Height;
            int cw = Width;
            int ch = Height;
            if (dw == 0 || dh == 0) return; // prevent divide by 0
            if (cw == 0 || ch == 0) return;
            _scrHorizontal.LargeChange = (cw * cw) / dw;
            _scrVertical.LargeChange = (ch * ch) / dh;
            _scrHorizontal.Maximum = dw;
            _scrVertical.Maximum = dh;
            if (dw <= cw)
            {
                _scrHorizontal.Visible = false;
            }
            else
            {
                if (_scrHorizontal.Enabled) _scrHorizontal.Visible = true;
            }

            if (dh <= ch)
            {
                _scrVertical.Visible = false;
            }
            else
            {
                if (_scrVertical.Enabled) _scrVertical.Visible = true;
            }

            _lblCorner.Visible = _scrVertical.Visible || _scrHorizontal.Visible;
        }

        /// <summary>
        /// Disposes the unmanaged memory objects and optionally disposes the managed memory objects.
        /// </summary>
        /// <param name="disposing">Indicates whether managed objects should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _backcolorBrush?.Dispose();
                _controlBrush?.Dispose();
                _backImageBrush?.Dispose();
                Page?.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Occurs during custom drawing.
        /// </summary>
        /// <param name="e">The paint event args.</param>
        protected virtual void OnDraw(PaintEventArgs e)
        {
            if (_firstDrawing == false)
            {
                ResetScroll();
                _firstDrawing = true;
            }

            e.Graphics.FillRectangle(_backcolorBrush, e.ClipRectangle); // in client coordinates, the clip-rectangle is the area to clear
            e.Graphics.DrawImage(Page, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Occurs during custom drawing when erasing things.
        /// </summary>
        /// <param name="e">The paint event args.</param>
        protected virtual void OnDrawBackground(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Occurs when scrolling horizontally.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnHorizontalScroll(object sender, ScrollEventArgs e)
        {
            _controlRectangle.Y = _scrVertical.Value;
            Initialize();
            Invalidate();
        }

        /// <summary>
        /// Fires the Initialized event.
        /// </summary>
        /// <param name="e">The paint event args.</param>
        protected virtual void OnInitialize(PaintEventArgs e)
        {
            Initialized?.Invoke(this, e);
        }

        /// <summary>
        /// On Paint only paints the specified clip rectangle, but paints it from the page buffer.
        /// </summary>
        /// <param name="e">The paint event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clip = e.ClipRectangle;
            if (clip.IsEmpty) clip = ClientRectangle;
            if (IsInitialized == false || Page == null)
            {
                Initialize(); // redraw the entire page buffer if necessary
            }

            using (var buffer = new Bitmap(clip.Width, clip.Height))
            using (var g = Graphics.FromImage(buffer))
            using (var mat = new Matrix())
            {
                mat.Translate(-clip.X, -clip.Y); // draw in "client" coordinates
                g.Transform = mat;

                OnDraw(new PaintEventArgs(g, clip)); // draw content to the small temporary buffer.
                e.Graphics.DrawImage(buffer, clip); // draw from our small, temporary buffer to the screen
            }
        }

        /// <summary>
        /// Prevent flicker by preventing this.
        /// </summary>
        /// <param name="e">The paint event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do Nothing
        }

        /// <summary>
        /// Occurens when resizing.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnResize(EventArgs e)
        {
            ResetScroll();
            base.OnResize(e);
        }

        /// <summary>
        /// Occurs when scrolling vertically.
        /// </summary>
        /// <param name="sender">The sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnVerticalScroll(object sender, ScrollEventArgs e)
        {
            _controlRectangle.X = _scrHorizontal.Value;
            Initialize();
            Invalidate();
        }

        // Redraws the entire contents of the control, even if the clip rectangle is smaller.
        private void Initialize()
        {
            if (_documentRectangle.IsEmpty)
            {
                _documentRectangle = ClientRectangle;
            }

            if (_controlRectangle.IsEmpty)
            {
                _controlRectangle = ClientRectangle;
            }
            else
            {
                _controlRectangle.Width = ClientRectangle.Width;
                _controlRectangle.Height = ClientRectangle.Height;
            }

            Page = new Bitmap(Width, Height);

            Graphics g = Graphics.FromImage(Page);
            g.Clear(BackColor);
            if (BackgroundImage != null)
            {
                if (BackgroundImageLayout == ImageLayout.None)
                {
                    g.DrawImage(BackgroundImage, ClientRectangle, _controlRectangle, GraphicsUnit.Pixel);
                }

                if (BackgroundImageLayout == ImageLayout.Center)
                {
                    int x = (Width - BackgroundImage.Width) / 2;
                    int y = (Height - BackgroundImage.Height) / 2;
                    g.DrawImage(BackgroundImage, new Point(x, y));
                }

                if (BackgroundImageLayout == ImageLayout.Stretch || BackgroundImageLayout == ImageLayout.Zoom)
                {
                    g.DrawImage(BackgroundImage, ClientRectangle);
                }

                if (BackgroundImageLayout == ImageLayout.Tile)
                {
                    g.FillRectangle(_backImageBrush, ClientRectangle);
                }
            }

            Matrix mat = g.Transform;
            Matrix oldMat = g.Transform;
            mat.Translate(-_controlRectangle.X, -_controlRectangle.Y);
            g.Transform = mat;
            OnInitialize(new PaintEventArgs(g, ClientRectangle));
            g.Transform = oldMat;
            g.Dispose();
        }

        private void InitializeComponent()
        {
            _scrVertical = new VScrollBar();
            _scrHorizontal = new HScrollBar();
            _lblCorner = new Label();
            SuspendLayout();

            // scrVertical
            _scrVertical.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Right;
            _scrVertical.Location = new Point(170, 0);
            _scrVertical.Name = "_scrVertical";
            _scrVertical.Size = new Size(17, 411);
            _scrVertical.TabIndex = 0;
            _scrVertical.Scroll += ScrVerticalScroll;

            // scrHorizontal
            _scrHorizontal.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left) | AnchorStyles.Right;
            _scrHorizontal.Location = new Point(0, 411);
            _scrHorizontal.Name = "_scrHorizontal";
            _scrHorizontal.Size = new Size(169, 17);
            _scrHorizontal.TabIndex = 1;
            _scrHorizontal.Scroll += ScrHorizontalScroll;

            // lblCorner
            _lblCorner.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _lblCorner.Location = new Point(169, 411);
            _lblCorner.AutoSize = false;
            _lblCorner.Text = null;
            _lblCorner.Size = new Size(18, 17);
            _lblCorner.BackColor = SystemColors.Control;

            // ScrollingControl
            Controls.Add(_scrHorizontal);
            Controls.Add(_scrVertical);
            Controls.Add(_lblCorner);
            Name = "ScrollingControl";
            Size = new Size(187, 428);
            ResumeLayout(false);
        }

        private void ScrHorizontalScroll(object sender, ScrollEventArgs e)
        {
            OnVerticalScroll(sender, e);
        }

        private void ScrVerticalScroll(object sender, ScrollEventArgs e)
        {
            OnHorizontalScroll(sender, e);
        }

        #endregion
    }
}