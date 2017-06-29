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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/7/2009 10:20:32 AM
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
    /// A vertical scroll control adds a scroll bar in the vertical case, but never in the horizontal case.
    /// This is useful if the content should simply autosize to fit the horizontal width of the control.
    /// </summary>
    [ToolboxItem(false)]
    public class VerticalScrollControl : Control
    {
        #region Events

        /// <summary>
        /// Occurs after the base drawing content has been rendered to the page.
        /// </summary>
        public event EventHandler<PaintEventArgs> Initialized;

        #endregion

        #region Private Variables

        private readonly Brush _controlBrush;
        private Brush _backImageBrush;
        private Brush _backcolorBrush;
        private Bitmap _buffer;

        private Rectangle _controlRectangle;
        private Rectangle _documentRectangle;
        private bool _firstDrawing;
        private bool _isInitialized;
        private VScrollBar _scrVertical;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ScrollingControl
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

        private void InitializeComponent()
        {
            _scrVertical = new VScrollBar();
            SuspendLayout();
            //
            // scrVertical
            //
            _scrVertical.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom)
                                 | AnchorStyles.Right;
            _scrVertical.Location = new Point(170, 0);
            _scrVertical.Name = "_scrVertical";
            _scrVertical.Size = new Size(17, 425);
            _scrVertical.TabIndex = 0;
            _scrVertical.Scroll += scrVertical_Scroll;
            //
            // VerticalScrollControl
            //
            Controls.Add(_scrVertical);
            Name = "ScrollingControl";
            Size = new Size(187, 428);
            ResumeLayout(false);
        }

        private void scrVertical_Scroll(object sender, ScrollEventArgs e)
        {
            _controlRectangle.Y = _scrVertical.Value;
            IsInitialized = false;
            Invalidate();
        }

        #region Methods

        /// <summary>
        /// Gets a rectangle in document coordinates for hte specified rectangle in client coordinates
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Rectangle ClientToDocument(Rectangle rect)
        {
            Rectangle result = rect;
            result.X += _controlRectangle.X;
            result.Y += _controlRectangle.Y;
            return result;
        }

        /// <summary>
        /// Translates a rectangle from document coordinates to coordinates relative to the client control
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
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
            else
            {
                if (_scrVertical.Enabled) _scrVertical.Visible = true;
            }

            //if (_documentRectangle.Width > 3 * Width || _documentRectangle.Height > 3 * Height)
            //{
            //    ResetPage();
            //}
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the background color to use for this control
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (_backcolorBrush != null) _backcolorBrush.Dispose();
                _backcolorBrush = new SolidBrush(value);
                base.BackColor = value;
            }
        }

        /// <summary>
        ///
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
                if (_backImageBrush != null) _backImageBrush.Dispose();
                if (value != null) _backImageBrush = new TextureBrush(BackgroundImage);
            }
        }

        /// <summary>
        /// Gets the rectangular region of the control in page coordinates.
        /// </summary>
        public Rectangle ControlRectangle
        {
            get { return _controlRectangle; }
            set { _controlRectangle = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle for the entire content, whether on the page buffer or not.  X and Y for this
        /// are always 0.
        /// </summary>
        public virtual Rectangle DocumentRectangle
        {
            get { return _documentRectangle; }
            set { _documentRectangle = value; }
        }

        /// <summary>
        /// Gets or sets whether or not the page for this control has been drawn.
        /// </summary>
        public bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
        }

        ///// <summary>
        ///// Gets or sets the page image being used as a buffer.  This is useful
        ///// for content changes that need to be made rapidly.  First refresh
        ///// a small region of this page, and then invalidate the client rectangle.
        ///// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public Bitmap Page
        //{
        //    get { return _page; }
        //    set { _page = value; }
        //}

        ///// <summary>
        ///// A page is the buffered scrollable content.  Content off the page
        ///// must be drawn to the page before it can be displayed in the client.
        ///// The Page Rectangle is in document coordinates.
        ///// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public Rectangle PageRectangle
        //{
        //    get { return _pageRectangle; }
        //    set { _pageRectangle = value; }
        //}

        /// <summary>
        /// Gets or sets a boolean indicating whether the vertical scroll should be permitted
        /// </summary>
        public bool VerticalScrollEnabled
        {
            get { return _scrVertical.Enabled; }
            set { _scrVertical.Enabled = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Prevent flicker by preventing this
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Do Nothing
        }

        /// <summary>
        /// On Paint only paints the specified clip rectangle, but paints
        /// it from the page buffer.
        /// </summary>
        /// <param name="e"></param>
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
        /// Occurs during custom drawing when erasing things
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawBackground(PaintEventArgs e)
        {
            //  e.Graphics.FillRectangle(_backcolorBrush, e.ClipRectangle);

            //  e.Graphics.DrawImage(_page, e.ClipRectangle, ClientToPage(e.ClipRectangle), GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Occurs during custom drawing
        /// </summary>
        /// <param name="e"></param>
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
        /// Disposes the unmanaged memory objects and optionally disposes
        /// the managed memory objects
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (_backcolorBrush != null) _backcolorBrush.Dispose();
            if (_controlBrush != null) _controlBrush.Dispose();
            if (_backImageBrush != null) _backImageBrush.Dispose();
            if (_buffer != null) _buffer.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Fires the Initialized event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnInitialize(PaintEventArgs e)
        {
            if (Initialized != null) Initialized(this, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            // ResetPage();
            ResetScroll();
        }

        #endregion

        #region Private Methods

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
                    //g.DrawImage(BackgroundImage, new Point(0, 0));

                    g.FillRectangle(_backImageBrush, ClientRectangle);
                }
            }
            g.TranslateTransform(-(float)_controlRectangle.X, -(float)_controlRectangle.Y);
            OnInitialize(new PaintEventArgs(g, ClientRectangle));
            g.Dispose();
        }

        #endregion
    }
}