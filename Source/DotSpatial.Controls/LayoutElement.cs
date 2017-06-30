// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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

    // CGX
    //=========================================================================================
    #region Converteur

    public class Converter
    {
        //=====================================================================================
        #region Simple Unit Converter

        /// <summary>
        /// Convertion Centième de pouce vers Millimetre
        /// </summary>
        public static double OneHundredthInches2Millimeters(double dOneHundredthInches)
        {
            double dRet = -9999;

            try
            {
                dRet = Math.Round(dOneHundredthInches * 0.254, 2);
            }
            catch (Exception) { }

            return dRet;
        }

        /// <summary>
        /// Convertion Millimetre vers Centième de pouce
        /// </summary>
        public static double Millimeters2OneHundredthInches(double dMillimeter)
        {
            double dRet = -9999;

            try
            {
                dRet = Math.Round(dMillimeter / 0.254, 2);
            }
            catch (Exception) { }

            return dRet;
        }

        /// <summary>
        /// Convertion Centième de pouce vers Centimetre
        /// </summary>
        public static double OneHundredthInches2Centimeters(double dOneHundredthInches)
        {
            double dRet = -9999;

            try
            {
                dRet = Math.Round(dOneHundredthInches * 0.0254, 2);
            }
            catch (Exception) { }

            return dRet;
        }

        /// <summary>
        /// Convertion Centimetre vers Centième de pouce 
        /// </summary>
        public static double Centimeters2OneHundredthInches(double dCentimeter)
        {
            double dRet = -9999;

            try
            {
                dRet = Math.Round(dCentimeter / 0.0254, 2);
            }
            catch (Exception) { }

            return dRet;
        }

        /// <summary>
        /// Convertion Millimetre vers Centimetre
        /// </summary>
        public static double Millimeters2Centimeters(double dMillimeter)
        {
            double dRet = -9999;

            try
            {
                dRet = Math.Round(dMillimeter / 10, 2);
            }
            catch (Exception) { }

            return dRet;
        }

        /// <summary>
        /// Convertion Centimetre vers Millimetre 
        /// </summary>
        public static double Centimeters2Millimeters(double dCentimeter)
        {
            double dRet = -9999;

            try
            {
                dRet = Math.Round(dCentimeter * 10, 2);
            }
            catch (Exception) { }

            return dRet;
        }
        #endregion

        //=====================================================================================
        #region Size Converter

        /// <summary>
        /// Convertion Centième de pouce vers Millimetre
        /// </summary>
        public static SizeF OneHundredthInches2Millimeters(SizeF sOneHundredthInches)
        {
            SizeF sRet = new SizeF(-9999, -9999);

            try
            {
                sRet = new SizeF(
                    (float)Converter.OneHundredthInches2Millimeters(sOneHundredthInches.Width),
                    (float)Converter.OneHundredthInches2Millimeters(sOneHundredthInches.Height));
            }
            catch (Exception) { }

            return sRet;
        }

        /// <summary>
        /// Convertion Millimetre vers Centième de pouce
        /// </summary>
        public static SizeF Millimeters2OneHundredthInches(SizeF sMillimeter)
        {
            SizeF sRet = new SizeF(-9999, -9999);

            try
            {
                sRet = new SizeF(
                    (float)Converter.Millimeters2OneHundredthInches(sMillimeter.Width),
                    (float)Converter.Millimeters2OneHundredthInches(sMillimeter.Height));
            }
            catch (Exception) { }

            return sRet;
        }

        /// <summary>
        /// Convertion Centième de pouce vers Centimetre
        /// </summary>
        public static SizeF OneHundredthInches2Centimeters(SizeF sOneHundredthInches)
        {
            SizeF sRet = new SizeF(-9999, -9999);

            try
            {
                sRet = new SizeF(
                    (float)Converter.OneHundredthInches2Centimeters(sOneHundredthInches.Width),
                    (float)Converter.OneHundredthInches2Centimeters(sOneHundredthInches.Height));
            }
            catch (Exception) { }

            return sRet;
        }

        /// <summary>
        /// Convertion Centimetre vers Centième de pouce 
        /// </summary>
        public static SizeF Centimeters2OneHundredthInches(SizeF sCentimeter)
        {
            SizeF sRet = new SizeF(-9999, -9999);

            try
            {
                sRet = new SizeF(
                    (float)Converter.Centimeters2OneHundredthInches(sCentimeter.Width),
                    (float)Converter.Centimeters2OneHundredthInches(sCentimeter.Height));
            }
            catch (Exception) { }

            return sRet;
        }

        /// <summary>
        /// Convertion Millimetre vers Centimetre
        /// </summary>
        public static SizeF Millimeters2Centimeters(SizeF sMillimeter)
        {
            SizeF sRet = new SizeF(-9999, -9999);

            try
            {
                sRet = new SizeF(
                    (float)Converter.Millimeters2Centimeters(sMillimeter.Width),
                    (float)Converter.Millimeters2Centimeters(sMillimeter.Height));
            }
            catch (Exception) { }

            return sRet;
        }

        /// <summary>
        /// Convertion Centimetre vers Millimetre 
        /// </summary>
        public static SizeF Centimeters2Millimeters(SizeF sCentimeter)
        {
            SizeF sRet = new SizeF(-9999, -9999);

            try
            {
                sRet = new SizeF(
                    (float)Converter.Centimeters2Millimeters(sCentimeter.Width),
                    (float)Converter.Centimeters2Millimeters(sCentimeter.Height));
            }
            catch (Exception) { }

            return sRet;
        }

        #endregion
    }

    #endregion
    // CGX END

    /// <summary>
    /// The interface for all elements that can be added to the layout control
    /// </summary>
    [Serializable]
    public abstract class LayoutElement
    {
        #region Fields

        private IPolygonSymbolizer _background = new PolygonSymbolizer(Color.Transparent, Color.Transparent);
        private PointF _location;
        private string _name;
        private ResizeStyle _resizeStyle;
        private bool _resizing;
        private SizeF _size;
        // CGX
        private SizeF _customSize;
        private string _sGroup = string.Empty;
        private eSizeUnit _eSizeUnit = eSizeUnit.OneHundredthInches;
        // Fin CGX
        private Bitmap _thumbNail;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutElement"/> class.
        /// </summary>
        protected LayoutElement()
        {
            _background.ItemChanged += BackgroundItemChanged;
        }

        #endregion

        #region Events

        /// <summary>
        /// Fires when the layout element is invalidated
        /// </summary>
        public event EventHandler Invalidated;

        /// <summary>
        /// Fires when the size of this element has been adjusted by the user
        /// </summary>
        public event EventHandler SizeChanged;

        /// <summary>
        /// Fires when the preview thumbnail for this element has been updated
        /// </summary>
        public event EventHandler ThumbnailChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the line symbolizer that draws the outline
        /// </summary>
        [TypeConverter(typeof(GeneralTypeConverter))]
        [Browsable(true)]
        [Category("Symbol")]
        [Editor(typeof(PolygonSymbolizerEditor), typeof(UITypeEditor))]
        public IPolygonSymbolizer Background
        {
            get
            {
                return _background;
            }

            set
            {
                _background = value;
            }
        }

        /// <summary>
        /// Gets or sets the location of the top left corner of the control in 1/100 of an inch paper coordinants
        /// </summary>
        [Browsable(true)]
        [Category("Layout")]
        public Point Location
        {
            get
            {
                return new Point(Convert.ToInt32(_location.X), Convert.ToInt32(_location.Y));
            }

            set
            {
                _location = new PointF(value.X, value.Y);
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the location of the top left corner of the control in 1/100 of an inch paper coordinants
        /// </summary>
        [Browsable(false)]
        public PointF LocationF
        {
            get
            {
                return _location;
            }

            set
            {
                _location = value;
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the name of the element
        /// </summary>
        [Browsable(true)]
        [Category("Layout")]
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the rectangle of the element in 1/100th of an inch paper coordinants
        /// </summary>
        [Browsable(false)]
        public RectangleF Rectangle
        {
            get
            {
                return new RectangleF(_location, _size);
            }

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
        /// Gets or sets if this element can handle redraw events on resize.
        /// </summary>
        [Browsable(false)]
        public ResizeStyle ResizeStyle
        {
            get
            {
                return _resizeStyle;
            }

            set
            {
                _resizeStyle = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether whether the control is resizing.
        /// </summary>
        [Browsable(false)]
        public bool Resizing
        {
            get
            {
                return _resizing;
            }

            set
            {
                _resizing = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the element in 1/100 of an inch paper coordinants.
        /// </summary>
        [Browsable(true)]
        [Category("Layout")]
        public SizeF Size
        {
            get
            {
                return new SizeF(_size.Width, _size.Height);
            }

            set
            {
                if (value.Width < 10)
                    value.Width = 10;
                if (value.Height < 10)
                    value.Height = 10;
                _size = value;

                RefreshElement();
            }
        }

        // CGX
        [Browsable(true), Category("Layout")]
        bool _Visible = true;
        public bool Visible
        {
            get { return _Visible; }
            set { _Visible = value; OnInvalidate(); }
        }
        // CGX END


        /// <summary>
        /// Gets the thumbnail that appears in the LayoutListView.
        /// </summary>
        [Browsable(false)]
        public Bitmap ThumbNail
        {
            get
            {
                return _thumbNail;
            }

            private set
            {
                _thumbNail?.Dispose();
                _thumbNail = value;
                OnThumbnailChanged();
            }
        }

        // CGX

        /// <summary>
        /// 
        /// </summary>
        public string Group
        {
            get { return _sGroup; }
            set { _sGroup = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true), Category("Size on Layout")]
        public SizeF CustomSize
        {
            get { return _customSize; }
            set
            {
                _customSize = value;
                this.Size = ComputeSize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        public enum eSizeUnit
        {
            Millimeters,
            Centimeters,
            OneHundredthInches,
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iUnit"></param>
        public void UnitFromInt(int iUnit)
        {
            Unit = (eSizeUnit)iUnit;
        }

        /// <summary>
        /// 
        /// </summary>
        [Browsable(true), Category("Size on Layout")]
        public eSizeUnit Unit
        {
            get { return _eSizeUnit; }
            set
            {
                eSizeUnit eOLD = _eSizeUnit;
                _eSizeUnit = value;

                // Recalcul de la taille à afficher
                switch (eOLD)
                {
                    case eSizeUnit.Centimeters:
                        if (_eSizeUnit == eSizeUnit.Millimeters) CustomSize = Converter.Centimeters2Millimeters(_customSize);
                        if (_eSizeUnit == eSizeUnit.OneHundredthInches) CustomSize = Converter.Centimeters2OneHundredthInches(_customSize);
                        break;
                    case eSizeUnit.Millimeters:
                        if (_eSizeUnit == eSizeUnit.Centimeters) CustomSize = Converter.Millimeters2Centimeters(_customSize);
                        if (_eSizeUnit == eSizeUnit.OneHundredthInches) CustomSize = Converter.Millimeters2OneHundredthInches(_customSize);
                        break;
                    case eSizeUnit.OneHundredthInches:
                        if (_eSizeUnit == eSizeUnit.Millimeters) CustomSize = Converter.OneHundredthInches2Millimeters(_customSize);
                        if (_eSizeUnit == eSizeUnit.Centimeters) CustomSize = Converter.OneHundredthInches2Centimeters(_customSize);
                        break;
                }
            }
        } // Fin CGX
        #endregion

        #region Methods

        // CGX
        /// <summary>
        /// 
        /// </summary>
        void LayoutElement_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                SizeF pSize = this.Size;
                switch (_eSizeUnit)
                {
                    case eSizeUnit.Centimeters: pSize = Converter.OneHundredthInches2Centimeters(this.Size); break;
                    case eSizeUnit.Millimeters: pSize = Converter.OneHundredthInches2Millimeters(this.Size); break;
                }
                if (CustomSize != pSize)
                    CustomSize = pSize;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        private SizeF ComputeSize()
        {
            SizeF rSize = _customSize;

            try
            {
                // Calcul de la taille à stocker
                switch (_eSizeUnit)
                {
                    case eSizeUnit.Centimeters: rSize = Converter.Centimeters2OneHundredthInches(_customSize); break;
                    case eSizeUnit.Millimeters: rSize = Converter.Millimeters2OneHundredthInches(_customSize); break;
                }
            }
            catch (Exception) { }

            return rSize;
        }

        // Fin CGX


        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">If true then we a actually printing not previewing so we should make it as high quality as possible</param>
        public abstract void Draw(Graphics g, bool printing);

        /// <summary>
        /// Draws the elements background behind everything else.
        /// </summary>
        /// <param name="g">Graphics object used for painting.</param>
        /// <param name="printing">If true then we a actually printing not previewing so we should make it as high quality as possible</param>
        public void DrawBackground(Graphics g, bool printing)
        {
            if (Background != null)
            {   
                // CGX
                //graphicspath gp = new graphicspath();
                //gp.addrectangle(rectangle);
                //foreach (ipattern mypattern in background.patterns)
                //{
                //    mypattern.bounds = rectangle;
                //    mypattern.fillpath(g, gp);
                //}
            }
        }

        /// <summary>
        /// Draws the elements outline on top of everything else.
        /// </summary>
        /// <param name="g">Graphics object used for painting.</param>
        /// <param name="printing">If true then we a actually printing not previewing so we should make it as high quality as possible</param>
        public void DrawOutline(Graphics g, bool printing)
        {
            if (Background.OutlineSymbolizer == null || Background.Patterns.Count < 1) return;

            /* ORIGINAL
            RectangleF tempRect = Rectangle;
            float width = (float)(Background.GetOutlineWidth() / 2.0D);
            tempRect.Inflate(width, width);

            if (printing)
            {
                // Changed by jany_ 2014-08-20
                // normal ClipBounds are big enough for the whole outline to be painted
                // when printing ClipBounds get set to Rectangle -> parts of the Rectangle get printed outside of the Clip
                RectangleF clip = Rectangle;
                float w = (float)Background.GetOutlineWidth();
                clip.Inflate(w, w);
                g.Clip = new Region(clip);
            }

            // Makes sure the rectangle is big enough to draw
            if (!(tempRect.Width > 0) || !(tempRect.Height > 0)) return;

            foreach (IPattern outlineSymbol in Background.Patterns)
            {
                if (!outlineSymbol.UseOutline)
                    continue;

                using (GraphicsPath gp = new GraphicsPath())
                {
                    gp.AddLine(tempRect.X, tempRect.Y, tempRect.X + tempRect.Width, tempRect.Y);
                    gp.AddLine(tempRect.X + tempRect.Width, tempRect.Y, tempRect.X + tempRect.Width, tempRect.Y + tempRect.Height);
                    gp.AddLine(tempRect.X + tempRect.Width, tempRect.Y + tempRect.Height, tempRect.X, tempRect.Y + tempRect.Height);
                    gp.AddLine(tempRect.X, tempRect.Y + tempRect.Height, tempRect.X, tempRect.Y);
                    outlineSymbol.Outline.DrawPath(g, gp, 1D);
                }
            }*/

            // CGX
            RectangleF tempRect = this.Rectangle;
            //if (!printing)
            //{
            //    float width = (float)(this.Background.GetOutlineWidth() / 2.0D);
            //    tempRect.Inflate(width, width);
            //}

            //Makes sure the rectangle is big enough to draw
            if (tempRect.Width > 0 && tempRect.Height > 0)
            {
                foreach (IPattern outlineSymbol in this.Background.Patterns)
                {
                    if (outlineSymbol.UseOutline == false)
                        continue;

                    double dWidth2 = outlineSymbol.Outline.GetWidth();
                    string sName = this.Name;

                    GraphicsPath gp = new GraphicsPath();
                    gp.AddLine(tempRect.X, tempRect.Y, tempRect.X + tempRect.Width, tempRect.Y);
                    //outlineSymbol.Outline.DrawPath(g, gp, dWidth2);

                    //gp = new GraphicsPath();
                    gp.AddLine(tempRect.X + tempRect.Width, tempRect.Y, tempRect.X + tempRect.Width, tempRect.Y + tempRect.Height);
                    //outlineSymbol.Outline.DrawPath(g, gp, dWidth2);

                    //gp = new GraphicsPath();
                    gp.AddLine(tempRect.X + tempRect.Width, tempRect.Y + tempRect.Height, tempRect.X, tempRect.Y + tempRect.Height);
                    //outlineSymbol.Outline.DrawPath(g, gp, dWidth2);

                    //gp = new GraphicsPath();
                    gp.AddLine(tempRect.X, tempRect.Y + tempRect.Height, tempRect.X, tempRect.Y);

                    outlineSymbol.Outline.DrawPath(g, gp, dWidth2);
                    gp.Dispose();

                    //GraphicsPath gp = new GraphicsPath();
                    //gp.AddLine(tempRect.X, tempRect.Y, tempRect.X + tempRect.Width, tempRect.Y);
                    //outlineSymbol.Outline.DrawPath(g, gp, 1.0D);

                    //gp = new GraphicsPath();
                    //gp.AddLine(tempRect.X + tempRect.Width, tempRect.Y, tempRect.X + tempRect.Width, tempRect.Y + tempRect.Height);
                    //outlineSymbol.Outline.DrawPath(g, gp, 1.0D);

                    //gp = new GraphicsPath();
                    //gp.AddLine(tempRect.X + tempRect.Width, tempRect.Y + tempRect.Height, tempRect.X, tempRect.Y + tempRect.Height);
                    //outlineSymbol.Outline.DrawPath(g, gp, 1.0D);

                    //gp = new GraphicsPath();
                    //gp.AddLine(tempRect.X, tempRect.Y + tempRect.Height, tempRect.X, tempRect.Y);
                    //outlineSymbol.Outline.DrawPath(g, gp, 1.0D);
                    //gp.Dispose();
                }
            } // FIN CGX
        }

        /// <summary>
        /// Returns true if the point in paper coordinants intersects with the rectangle of the element.
        /// </summary>
        /// <param name="paperPoint">Point to check for intersection.</param>
        /// <returns>True if the point intersects with this.</returns>
        public bool IntersectsWith(PointF paperPoint)
        {
            return IntersectsWith(new RectangleF(paperPoint.X, paperPoint.Y, 0F, 0F));
        }

        /// <summary>
        /// Returns true if the rectangle in paper coordinants intersects with the rectangle of the element.
        /// </summary>
        /// <param name="paperRectangle">Rectangle to check for intersection.</param>
        /// <returns>True, if the rectangle intersects with this.</returns>
        public bool IntersectsWith(RectangleF paperRectangle)
        {
            return new RectangleF(LocationF, Size).IntersectsWith(paperRectangle);
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
        /// This returns the objects name as a string.
        /// </summary>
        /// <returns>The objects name.</returns>
        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Call this when it needs to updated
        /// </summary>
        protected virtual void OnInvalidate()
        {
            Invalidated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires when the size of the element changes
        /// </summary>
        protected virtual void OnSizeChanged()
        {
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires when the thumbnail gets modified
        /// </summary>
        protected virtual void OnThumbnailChanged()
        {
            ThumbnailChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the thumbnail when needed.
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

        /// <summary>
        /// Fires when the background is modified
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void BackgroundItemChanged(object sender, EventArgs e)
        {
            OnInvalidate();
            UpdateThumbnail();
        }

        #endregion
    }
}