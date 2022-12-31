using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using GeoAPI.Geometries;
using Point = System.Drawing.Point;
using PointShape = DotSpatial.Symbology.PointShape;

namespace DemoCustomLayer.DemoCustomLayerExtension
{
    public class MyCustomLayer2 : Layer, IMapLayer
    {

        #region Events

        /// <summary>
        /// Occurs when drawing content has changed on the buffer for this layer
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Private Variables

        // private bool _isPreventingOverlap;
        // private KDTree _regularTree;

        #endregion

        #region Constructors

        /// <summary>
        /// This creates a blank MapPointLayer with the DataSet set to an empty new featureset of the Point featuretype.
        /// </summary>
        public MyCustomLayer2()
        {
            Configure();

            //assign the data set
            DataSet = new MyCustomDataSet();
        }

        public new MyCustomDataSet DataSet { get; set; }

        public int ChunkSize { get; set; }

        public bool EditMode
        {
            get { return false; }
        }

        private void Configure()
        {
            ChunkSize = 50000;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Call StartDrawing before using this.
        /// </summary>
        /// <param name="rectangles">The rectangular region in pixels to clear.</param>
        /// <param name= "color">The color to use when clearing.  Specifying transparent
        /// will replace content with transparent pixels.</param>
        public void Clear(List<Rectangle> rectangles, Color color)
        {
            if (BackBuffer == null) return;
            Graphics g = Graphics.FromImage(BackBuffer);
            foreach (Rectangle r in rectangles)
            {
                if (r.IsEmpty == false)
                {
                    g.Clip = new Region(r);
                    g.Clear(color);
                }
            }
            g.Dispose();
        }

        /// <summary>
        /// This will draw any features that intersect this region.  To specify the features
        /// directly, use OnDrawFeatures.  This will not clear existing buffer content.
        /// For that call Initialize instead.
        /// </summary>
        /// <param name="args">A GeoArgs clarifying the transformation from geographic to image space</param>
        /// <param name="regions">The geographic regions to draw</param>
        public virtual void DrawRegions(MapArgs args, List<Extent> regions)
        {
            double minX = args.MinX;
            double maxY = args.MaxY;
            double dx = args.Dx;
            double dy = args.Dy;
            if (Double.IsInfinity(dx) || Double.IsInfinity(dy)) return;

            foreach (Extent boundingBox in regions)
            {
                Graphics g = args.Device ?? Graphics.FromImage(BackBuffer);
                Matrix origTransform = g.Transform;
                FeatureType featureType = FeatureType.Point;

                //reads the point array from the data source
                //DataSet implements or uses IShapeSource to read the points that are within the bounding box
                double[] vertices = DataSet.GetPointArray(boundingBox);

                //setup the point symbol
                Color randomColor = CreateRandomColor();
                Bitmap normalSymbol = CreateDefaultSymbol(randomColor, 4);

                //run the drawing operation
                int numPoints = vertices.Length / 2;

                for (int index = 0; index < numPoints; index++)
                {
                    Bitmap bmp = normalSymbol;

                    if (featureType == FeatureType.Point)
                    {
                        Point pt = new Point
                        {
                            X = Convert.ToInt32((vertices[index * 2] - minX) * dx),
                            Y = Convert.ToInt32((maxY - vertices[index * 2 + 1]) * dy)
                        };

                        Matrix shift = origTransform.Clone();
                        shift.Translate(pt.X, pt.Y);
                        g.Transform = shift;

                        g.DrawImageUnscaled(bmp, -bmp.Width / 2, -bmp.Height / 2);
                    }
                }

                if (args.Device == null) g.Dispose();
                else g.Transform = origTransform;
            }
        }

        /// <summary>
        /// Indicates that the drawing process has been finalized and swaps the back buffer
        /// to the front buffer.
        /// </summary>
        public void FinishDrawing()
        {
            OnFinishDrawing();
            if (Buffer != null && Buffer != BackBuffer) Buffer.Dispose();
            Buffer = BackBuffer;
        }

        /// <summary>
        /// Copies any current content to the back buffer so that drawing should occur on the
        /// back buffer (instead of the fore-buffer).  Calling draw methods without
        /// calling this may cause exceptions.
        /// </summary>
        /// <param name="preserve">Boolean, true if the front buffer content should be copied to the back buffer
        /// where drawing will be taking place.</param>
        public void StartDrawing(bool preserve)
        {
            Bitmap backBuffer = new Bitmap(BufferRectangle.Width, BufferRectangle.Height);
            if (Buffer != null)
            {
                if (Buffer.Width == backBuffer.Width && Buffer.Height == backBuffer.Height)
                {
                    if (preserve)
                    {
                        Graphics g = Graphics.FromImage(backBuffer);
                        g.DrawImageUnscaled(Buffer, 0, 0);
                    }
                }
            }
            if (BackBuffer != null && BackBuffer != Buffer) BackBuffer.Dispose();
            BackBuffer = backBuffer;
            OnStartDrawing();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the OnBufferChanged event
        /// </summary>
        /// <param name="clipRectangles">The Rectangle in pixels</param>
        protected virtual void OnBufferChanged(List<Rectangle> clipRectangles)
        {
            if (BufferChanged != null)
            {
                ClipArgs e = new ClipArgs(clipRectangles);
                BufferChanged(this, e);
            }
        }


        /// <summary>
        /// Indiciates that whatever drawing is going to occur has finished and the contents
        /// are about to be flipped forward to the front buffer.
        /// </summary>
        protected virtual void OnFinishDrawing()
        {
        }

        /// <summary>
        /// Occurs when a new drawing is started, but after the BackBuffer has been established.
        /// </summary>
        protected virtual void OnStartDrawing()
        {
        }

        #endregion

        #region Private  Methods

        private Color CreateRandomColor()
        {
            Random rnd = new Random();
            Color randomColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            return randomColor;
        }

        private Bitmap CreateDefaultSymbol(Color color, int symbolSize)
        {
            double scaleSize = 1;
            Size2D size = new Size2D(symbolSize, symbolSize);
            Bitmap normalSymbol = new Bitmap((int)(size.Width * scaleSize) + 1, (int)(size.Height * scaleSize) + 1);
            Graphics bg = Graphics.FromImage(normalSymbol);

            Random rnd = new Random();
            Color randomColor = Color.FromArgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
            PointSymbolizer sym = new PointSymbolizer(randomColor, PointShape.Rectangle, 4);
            PointCategory category = new PointCategory(sym);
            bg.SmoothingMode = category.Symbolizer.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
            Matrix trans = bg.Transform;

            trans.Translate(((float)(size.Width * scaleSize) / 2 - 1), (float)(size.Height * scaleSize) / 2 - 1);
            bg.Transform = trans;
            category.Symbolizer.Draw(bg, 1);
            return normalSymbol;
        }

        //todo: this is a new for this class
        public void DrawRegions(MapArgs args, List<Extent> regions, bool selected)
        {
            throw new NotImplementedException();
        }

        //private void DrawPoints(MapArgs e, Extent bBox)
        //{
        //    Graphics g = e.Device ?? Graphics.FromImage(_backBuffer);
        //    Matrix origTransform = g.Transform;
        //    FeatureType featureType = FeatureType.Point;

        //    double minX = e.MinX;
        //    double maxY = e.MaxY;
        //    double dx = e.Dx;
        //    double dy = e.Dy;

        //    //reads the point array from the data source
        //    double[] vertices = DataSet.GetPointArray(bBox);

        //    //setup the point symbol
        //    Color randomColor = CreateRandomColor();
        //    Bitmap normalSymbol = CreateDefaultSymbol(randomColor, 4);

        //    //runs the drawing
        //    int numPoints = vertices.Length / 2;

        //    for (int index=0; index < numPoints; index++)
        //    {
        //        Bitmap bmp = normalSymbol;

        //        if (featureType == FeatureType.Point)
        //        {
        //            Point pt = new Point();
        //            pt.X = Convert.ToInt32((vertices[index * 2] - minX) * dx);
        //            pt.Y = Convert.ToInt32((maxY - vertices[index * 2 + 1]) * dy);

        //            Matrix shift = origTransform.Clone();
        //            shift.Translate(pt.X, pt.Y);
        //            g.Transform = shift;

        //            g.DrawImageUnscaled(bmp, -bmp.Width / 2, -bmp.Height / 2);
        //        }
        //    }

        //    if (e.Device == null) g.Dispose();
        //    else g.Transform = origTransform;
        //}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back buffer that will be drawn to as part of the initialization process.
        /// </summary>
        [ShallowCopy, Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BackBuffer { get; set; }

        /// <summary>
        /// Gets the current buffer.
        /// </summary>
        [ShallowCopy, Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image Buffer { get; set; }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        [ShallowCopy,
        Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Envelope BufferEnvelope { get; set; }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        [ShallowCopy,
        Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle BufferRectangle { get; set; }

        public override Extent Extent
        {
            get { return DataSet.Extent; }
        }

        #endregion
    }
}
