// ********************************************************************************************************
// Product Name: DotSpatial.Layout
// Description:  The DotSpatial ILayoutElement the interface used by all elements that can be added to the layout
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Jul, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ------------------|------------|---------------------------------------------------------------
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using DotSpatial.Symbology;
using DotSpatial.Symbology.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The interface for all elements that can be added to the layout control
    /// </summary>
    [Serializable]
    public abstract class LayoutElement
    {
        #region Events

        /// <summary>
        /// Fires when the layout element is invalidated
        /// </summary>
        public event EventHandler Invalidated;
        /// <summary>
        /// Fires when the preview thumbnail for this element has been updated
        /// </summary>
        public event EventHandler ThumbnailChanged;
        /// <summary>
        /// Fires when the size of this element has been adjusted by the user
        /// </summary>
        public event EventHandler SizeChanged;

        #endregion

        private IPolygonSymbolizer _background = new PolygonSymbolizer(Color.Transparent, Color.Transparent);
        private PointF _location;
        private String _name;
        private ResizeStyle _resizeStyle;
        private bool _resizing;
        private SizeF _size;
        private Bitmap _thumbNail;

        /// <summary>
        /// Creates an instance of the layout element
        /// </summary>
        protected LayoutElement()
        {
            _background.ItemChanged += _background_ItemChanged;
        }

        /// <summary>
        /// Gets or sets the line symbolizer that draws the outline
        /// </summary>
        [TypeConverter(typeof(GeneralTypeConverter)), Browsable(true), Category("Symbol"), Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))]
        public IPolygonSymbolizer Background
        {
            get { return _background; }
            set
            {
                _background = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the element
        /// </summary>
        [Browsable(true), Category("Layout")]
        public String Name
        {
            get { return _name; }
            set { _name = value; OnInvalidate(); }
        }

        /// <summary>
        /// Gets the thumbnail that appears in the LayoutListView
        /// </summary>
        [Browsable(false)]
        public Bitmap ThumbNail
        {
            get { return _thumbNail; }
            private set
            {
                if (_thumbNail != null) _thumbNail.Dispose();
                _thumbNail = value;
                OnThumbnailChanged();
            }
        }

        /// <summary>
        /// Disables updating redraw when resizing.
        /// </summary>
        [Browsable(false)]
        public bool Resizing
        {
            get { return _resizing; }
            set { _resizing = value; }
        }

        /// <summary>
        /// Indicates if this element can handle redraw events on resize
        /// </summary>
        [Browsable(false)]
        public ResizeStyle ResizeStyle
        {
            get { return _resizeStyle; }
            set { _resizeStyle = value; }
        }

        /// <summary>
        /// Gets or sets the location of the top left corner of the control in 1/100 of an inch paper coordinants
        /// </summary>
        [Browsable(false)]
        public PointF LocationF
        {
            get { return _location; }
            set { _location = value; OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the location of the top left corner of the control in 1/100 of an inch paper coordinants
        /// </summary>
        [Browsable(true), Category("Layout")]
        public Point Location
        {
            get { return new Point(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y)); }
            set { _location = new PointF(value.X, value.Y); OnInvalidate(); }
        }

        /// <summary>
        /// Gets or sets the size of the element in 1/100 of an inch paper coordinants
        /// </summary>
        [Browsable(true), Category("Layout")]
        public SizeF Size
        {
            get { return new SizeF(_size.Width, _size.Height); }
            set
            {
                if (value.Width < 10)
                    value.Width = 10;
                if (value.Height < 10)
                    value.Height = 10;
                _size = value;

                OnSizeChanged();
            }
        }

        /// <summary>
        /// Gets or sets the rectangle of the element in 1/100th of an inch paper coordinants
        /// </summary>
        [Browsable(false)]
        public RectangleF Rectangle
        {
            get { return new RectangleF(_location, _size); }
            set
            {
                if (value.Width < 10)
                {
                    value.Width = 10;
                    if (value.X != _location.X)
                        value.X = _location.X + _size.Width - 10;
                }
                if (value.Height < 10)
                {
                    value.Height = 10;
                    if (value.Y != _location.Y)
                        value.Y = _location.Y + _size.Height - 10;
                }

                _location = value.Location;
                _size = value.Size;

                RefreshElement();
            }
        }

        /// <summary>
        /// Fires when the background is modified
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _background_ItemChanged(object sender, EventArgs e)
        {
            OnInvalidate();
            UpdateThumbnail();
        }

        /// <summary>
        /// Returns true if the point in paper coordinants intersects with the rectangle of the element
        /// </summary>
        /// <param name="paperPoint"></param>
        /// <returns></returns>
        public bool IntersectsWith(PointF paperPoint)
        {
            return IntersectsWith(new RectangleF(paperPoint.X, paperPoint.Y, 0F, 0F));
        }

        /// <summary>
        /// Returns true if the rectangle in paper coordinants intersects with the rectangle of the the element
        /// </summary>
        /// <param name="paperRectangle"></param>
        /// <returns></returns>
        public bool IntersectsWith(RectangleF paperRectangle)
        {
            return new RectangleF(LocationF, Size).IntersectsWith(paperRectangle);
        }

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">If true then we a actually printing not previewing so we should make it as high quality as possible</param>
        public abstract void Draw(Graphics g, bool printing);

        /// <summary>
        /// Draws the elements background behind everything else
        /// </summary>
        /// <param name="g"></param>
        /// <param name="printing"></param>
        public void DrawBackground(Graphics g, bool printing)
        {
            if (this.Background != null)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddRectangle(this.Rectangle);
                foreach (IPattern myPattern in this.Background.Patterns)
                {
                    myPattern.Bounds = this.Rectangle;
                    myPattern.FillPath(g, gp);
                }
            }
        }

        /// <summary>
        /// Draws the elements outline on top of everything else
        /// </summary>
        /// <param name="g"></param>
        /// <param name="printing"></param>
        public void DrawOutline(Graphics g, bool printing)
        {
            if (this.Background.OutlineSymbolizer != null && this.Background.Patterns.Count >= 1)
            {
                RectangleF tempRect = this.Rectangle;
                float width = (float)(this.Background.GetOutlineWidth() / 2.0D);
                tempRect.Inflate(width, width);

                //Makes sure the rectangle is big enough to draw
                if (tempRect.Width > 0 && tempRect.Height > 0)
                {
                    foreach (IPattern outlineSymbol in this.Background.Patterns)
                    {
                        if (outlineSymbol.UseOutline == false)
                            continue;

                        GraphicsPath gp = new GraphicsPath();
                        gp.AddLine(tempRect.X, tempRect.Y, tempRect.X + tempRect.Width, tempRect.Y);
                        outlineSymbol.Outline.DrawPath(g, gp, 1D);

                        gp = new GraphicsPath();
                        gp.AddLine(tempRect.X + tempRect.Width, tempRect.Y, tempRect.X + tempRect.Width, tempRect.Y + tempRect.Height);
                        outlineSymbol.Outline.DrawPath(g, gp, 1D);

                        gp = new GraphicsPath();
                        gp.AddLine(tempRect.X + tempRect.Width, tempRect.Y + tempRect.Height, tempRect.X, tempRect.Y + tempRect.Height);
                        outlineSymbol.Outline.DrawPath(g, gp, 1D);

                        gp = new GraphicsPath();
                        gp.AddLine(tempRect.X, tempRect.Y + tempRect.Height, tempRect.X, tempRect.Y);
                        outlineSymbol.Outline.DrawPath(g, gp, 1D);
                        gp.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Causes the element to be refreshed
        /// </summary>
        public virtual void RefreshElement()
        {
            OnSizeChanged();
            OnInvalidate();
            UpdateThumbnail();
        }

        /// <summary>
        /// This returns the objects name as a string
        /// </summary>
        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Call this when it needs to updated
        /// </summary>
        protected virtual void OnInvalidate()
        {
            var h = Invalidated;
            if (h != null) h(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires when the size of the element changes
        /// </summary>
        protected virtual void OnSizeChanged()
        {
            var h = SizeChanged;
            if (h != null) h(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires when the thumbnail gets modified
        /// </summary>
        protected virtual void OnThumbnailChanged()
        {
            var h = ThumbnailChanged;
            if (h != null) h(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the thumbnail when needed
        /// </summary>
        protected virtual void UpdateThumbnail()
        {
            if (Resizing || Size.Width == 0 || Size.Height == 0) return;
            Bitmap tempThumbNail = new Bitmap(32, 32, PixelFormat.Format32bppArgb);
            Graphics graph = Graphics.FromImage(tempThumbNail);
            graph.SmoothingMode = SmoothingMode.AntiAlias;
            graph.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            if ((Size.Width / tempThumbNail.Width) > (Size.Height / tempThumbNail.Height))
            {
                graph.ScaleTransform(32F / Size.Width, 32F / Size.Width);
                graph.TranslateTransform(-LocationF.X, -LocationF.Y);
            }
            else
            {
                graph.ScaleTransform(32F / Size.Height, 32F / Size.Height);
                graph.TranslateTransform(-LocationF.X, -LocationF.Y);
            }
            graph.Clip = new Region(Rectangle);
            DrawBackground(graph, false);
            Draw(graph, false);
            DrawOutline(graph, false);
            graph.Dispose();
            ThumbNail = tempThumbNail;
        }
    }
}