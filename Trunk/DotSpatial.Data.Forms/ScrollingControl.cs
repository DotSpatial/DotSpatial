// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/2/2008 9:28:05 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// ScrollingControl that provides autoscroll and custom draw that won't crash mono
    /// </summary>
    [ToolboxItem(false)]
    public class ScrollingControl : Control
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

        private Rectangle _controlRectangle;
        private Rectangle _documentRectangle;
        private bool _firstDrawing;
        private bool _isInitialized;
        private Bitmap _page; // the page is always the size of the control
        private Size _pageSize;
        private bool _resetOnResize;
        private Label lblCorner;
        private HScrollBar scrHorizontal;
        private VScrollBar scrVertical;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ScrollingControl
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
            base.MinimumSize = new Size(5, 5);
        }

        #endregion

        private void InitializeComponent()
        {
            scrVertical = new VScrollBar();
            scrHorizontal = new HScrollBar();
            lblCorner = new Label();
            SuspendLayout();
            //
            // scrVertical
            //
            scrVertical.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom)
                                 | AnchorStyles.Right;
            scrVertical.Location = new Point(170, 0);
            scrVertical.Name = "scrVertical";
            scrVertical.Size = new Size(17, 411);
            scrVertical.TabIndex = 0;
            scrVertical.Scroll += scrVertical_Scroll;
            //
            // scrHorizontal
            //
            scrHorizontal.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left)
                                   | AnchorStyles.Right;
            scrHorizontal.Location = new Point(0, 411);
            scrHorizontal.Name = "scrHorizontal";
            scrHorizontal.Size = new Size(169, 17);
            scrHorizontal.TabIndex = 1;
            scrHorizontal.Scroll += scrHorizontal_Scroll;
            //
            // lblCorner
            //
            lblCorner.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblCorner.Location = new Point(169, 411);
            lblCorner.AutoSize = false;
            lblCorner.Text = null;
            lblCorner.Size = new Size(18, 17);
            lblCorner.BackColor = SystemColors.Control;
            //
            // ScrollingControl
            //
            Controls.Add(scrHorizontal);
            Controls.Add(scrVertical);
            Controls.Add(lblCorner);
            Name = "ScrollingControl";
            Size = new Size(187, 428);
            ResumeLayout(false);
        }

        private void scrHorizontal_Scroll(object sender, ScrollEventArgs e)
        {
            OnVerticalScroll(sender, e);
        }

        private void scrVertical_Scroll(object sender, ScrollEventArgs e)
        {
            OnHorizontalScroll(sender, e);
        }

        /// <summary>
        /// Occurs when scrolling vertically
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnVerticalScroll(object sender, ScrollEventArgs e)
        {
            _controlRectangle.X = scrHorizontal.Value;
            Initialize();
            Invalidate();
        }

        /// <summary>
        /// Occurs when scrolling horizontally
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnHorizontalScroll(object sender, ScrollEventArgs e)
        {
            _controlRectangle.Y = scrVertical.Value;
            Initialize();
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
            return new Rectangle(rect.X + _controlRectangle.X, rect.Y + _controlRectangle.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// Translates a rectangle from document coordinates to coordinates relative to the client control
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
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
            scrHorizontal.LargeChange = (cw * cw) / dw;
            scrVertical.LargeChange = (ch * ch) / dh;
            scrHorizontal.Maximum = dw;
            scrVertical.Maximum = dh;
            if (dw <= cw)
            {
                scrHorizontal.Visible = false;
            }
            else
            {
                if (scrHorizontal.Enabled) scrHorizontal.Visible = true;
            }
            if (dh <= ch)
            {
                scrVertical.Visible = false;
            }
            else
            {
                if (scrVertical.Enabled) scrVertical.Visible = true;
            }

            lblCorner.Visible = scrVertical.Visible || scrHorizontal.Visible;
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
                if (value == null) return;
                _backImageBrush = new TextureBrush(BackgroundImage);
                Size s = _pageSize;
                if (s.Width < BackgroundImage.Width) s.Width = BackgroundImage.Width;
                if (s.Height < BackgroundImage.Height) s.Height = BackgroundImage.Height;
                _pageSize = s;
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

        /// <summary>
        /// Gets or sets a boolean indicating whether or not horizontal scrolling is enabled
        /// </summary>
        public bool HorizontalScrollEnabled
        {
            get { return scrHorizontal.Enabled; }
            set { scrHorizontal.Enabled = value; }
        }

        /// <summary>
        /// Gets or sets the page image being used as a buffer.  This is useful
        /// for content changes that need to be made rapidly.  First refresh
        /// a small region of this page, and then invalidate the client rectangle.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Bitmap Page
        {
            get { return _page; }
            set { _page = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether or not the scrolling
        /// should be reset on every resize or not.
        /// </summary>
        public bool ResetOnResize
        {
            get { return _resetOnResize; }
            set { _resetOnResize = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether the vertical scroll should be permitted
        /// </summary>
        public bool VerticalScrollEnabled
        {
            get { return scrVertical.Enabled; }
            set { scrVertical.Enabled = value; }
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
            if (IsInitialized == false || _page == null)
            {
                Initialize(); // redraw the entire page buffer if necessary
            }

            using (var buffer = new Bitmap(clip.Width, clip.Height))
            using (var g = Graphics.FromImage(buffer))
            using(var mat = new Matrix())
            {
                mat.Translate(-clip.X, -clip.Y); // draw in "client" coordinates
                g.Transform = mat;

                OnDraw(new PaintEventArgs(g, clip)); // draw content to the small temporary buffer.
                e.Graphics.DrawImage(buffer, clip); // draw from our small, temporary buffer to the screen
            }
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
            e.Graphics.DrawImage(_page, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Disposes the unmanaged memory objects and optionally disposes
        /// the managed memory objects
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_backcolorBrush != null) _backcolorBrush.Dispose();
                if (_controlBrush != null) _controlBrush.Dispose();
                if (_backImageBrush != null) _backImageBrush.Dispose();
                if (_page != null) _page.Dispose();
            }
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
            ResetScroll();
            base.OnResize(e);
        }

        #endregion

        #region Private Methods

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

            _page = new Bitmap(Width, Height);

            Graphics g = Graphics.FromImage(_page);
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
                    //g.DrawImage(BackgroundImage, new Point(0, 0));

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

        #endregion
    }
}