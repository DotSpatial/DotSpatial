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
    /// A vertical scroll control adds a scroll bar in the vertical case, but never in the horizontal case.
    /// This is useful if the content should simply autosize to fit the horizontal width of the control.
    /// </summary>
    [ToolboxItem(false)]
    public partial class VerticalScrollControl : Control
    {
        #region Fields

        private readonly Brush _controlBrush;
        private Brush _backcolorBrush;
        private Brush _backImageBrush;
        private Bitmap _buffer;

        private Rectangle _controlRectangle;
        private Rectangle _documentRectangle;
        private bool _firstDrawing;
        private VScrollBar _scrVertical;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalScrollControl"/> class.
        /// </summary>
        public VerticalScrollControl()
        {
            InitializeComponent();
            _backcolorBrush = new SolidBrush(base.BackColor);
            _controlBrush = new SolidBrush(SystemColors.Control);
            if (base.BackgroundImage != null)
            {
                _backImageBrush = new TextureBrush(base.BackgroundImage);
            }
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
        /// Gets or sets the background image.
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
                if (value != null) _backImageBrush = new TextureBrush(BackgroundImage);
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
        /// Gets or sets the rectangle for the entire content, whether on the page buffer or not.
        /// X and Y for this are always 0.
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
        /// Gets or sets a value indicating whether or not the page for this control has been drawn.
        /// </summary>
        public bool IsInitialized { get; set; }

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
        /// Gets a rectangle in document coordinates for hte specified rectangle in client coordinates.
        /// </summary>
        /// <param name="rect">Rectangle in client coordinates.</param>
        /// <returns>Rectangle in document coordinates.</returns>
        public Rectangle ClientToDocument(Rectangle rect)
        {
            Rectangle result = rect;
            result.X += _controlRectangle.X;
            result.Y += _controlRectangle.Y;
            return result;
        }

        /// <summary>
        /// Translates a rectangle from document coordinates to coordinates relative to the client control.
        /// </summary>
        /// <param name="rect">Rectangle in document coordinates.</param>
        /// <returns>Rectangle in client coordinates.</returns>
        public Rectangle DocumentToClient(Rectangle rect)
        {
            Rectangle result = rect;
            result.X -= _controlRectangle.X;
            result.Y -= _controlRectangle.Y;
            return result;
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
            _scrVertical.LargeChange = (ch * ch) / dh;
            _scrVertical.Maximum = dh;

            if (dh <= ch)
            {
                _scrVertical.Visible = false;
            }
            else if (_scrVertical.Enabled)
            {
                _scrVertical.Visible = true;
            }
        }

        /// <summary>
        /// Disposes the unmanaged memory objects and optionally disposes
        /// the managed memory objects
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            _backcolorBrush?.Dispose();
            _controlBrush?.Dispose();
            _backImageBrush?.Dispose();
            _buffer?.Dispose();
            if (disposing) components?.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Occurs during custom drawing.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnDraw(PaintEventArgs e)
        {
            if (_firstDrawing == false)
            {
                ResetScroll();
                _firstDrawing = true;
            }

            e.Graphics.FillRectangle(_backcolorBrush, e.ClipRectangle); // in client coordinates, the clip-rectangle is the area to clear
            e.Graphics.DrawImage(_buffer, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Occurs during custom drawing when erasing things
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnDrawBackground(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Fires the Initialized event
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnInitialize(PaintEventArgs e)
        {
            Initialized?.Invoke(this, e);
        }

        /// <summary>
        /// On Paint only paints the specified clip rectangle, but paints
        /// it from the page buffer.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clip = e.ClipRectangle;
            if (clip.IsEmpty) clip = ClientRectangle;
            if (IsInitialized == false || _buffer == null)
            {
                Initialize(); // redraw the entire page buffer if necessary
            }

            Bitmap buffer = new Bitmap(clip.Width, clip.Height);
            Graphics g = Graphics.FromImage(buffer);
            Matrix mat = new Matrix();
            mat.Translate(-clip.X, -clip.Y); // draw in "client" coordinates
            g.Transform = mat;

            OnDraw(new PaintEventArgs(g, clip)); // draw content to the small temporary buffer.

            g.Dispose();
            e.Graphics.DrawImage(buffer, clip); // draw from our small, temporary buffer to the screen
            buffer.Dispose();
        }

        /// <summary>
        /// Prevent flicker by preventing this
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do Nothing
        }

        /// <summary>
        /// Handles the resize event.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            ResetScroll();
        }

        // Redraws the entire contents of the page.
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
                _controlRectangle.Height = ClientRectangle.Height;
            }

            _documentRectangle.Width = Width;
            _controlRectangle.Width = Width;
            _buffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            Graphics g = Graphics.FromImage(_buffer);
            g.FillRectangle(_backcolorBrush, ClientRectangle);
            if (BackgroundImage != null)
            {
                if (BackgroundImageLayout == ImageLayout.None)
                {
                    g.DrawImage(BackgroundImage, ClientRectangle, ClientToDocument(ClientRectangle), GraphicsUnit.Pixel);
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

            g.TranslateTransform(-(float)_controlRectangle.X, -(float)_controlRectangle.Y);
            OnInitialize(new PaintEventArgs(g, ClientRectangle));
            g.Dispose();
        }

        private void ScrVerticalScroll(object sender, ScrollEventArgs e)
        {
            _controlRectangle.Y = _scrVertical.Value;
            IsInitialized = false;
            Invalidate();
        }

        #endregion
    }
}