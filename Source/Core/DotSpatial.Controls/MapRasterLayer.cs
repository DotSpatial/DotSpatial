// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Symbology;
using NetTopologySuite.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles raster drawing.
    /// </summary>
    public class MapRasterLayer : RasterLayer, IMapRasterLayer
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRasterLayer"/> class from the specified fileName.
        /// </summary>
        /// <param name="fileName">Filename of the corresponding raster file.</param>
        /// <param name="symbolizer">Symbolizer used for drawing the raster data.</param>
        public MapRasterLayer(string fileName, IRasterSymbolizer symbolizer)
            : base(fileName, symbolizer)
        {
            LegendText = Path.GetFileNameWithoutExtension(fileName);
            if ((long)DataSet.NumRows * DataSet.NumColumns > MaxCellsInMemory)
            {
                string pyrFile = Path.ChangeExtension(fileName, ".mwi");
                BitmapGetter = File.Exists(pyrFile) && File.Exists(Path.ChangeExtension(pyrFile, ".mwh")) ? new PyramidImage(pyrFile) : CreatePyramidImage(pyrFile, DataManager.DefaultDataManager.ProgressHandler);
            }
            else
            {
                Bitmap bmp = new(DataSet.NumColumns, DataSet.NumRows);
                symbolizer.Raster = DataSet;

                DataSet.DrawToBitmap(symbolizer, bmp);
                var id = new InRamImage(bmp)
                {
                    Bounds = DataSet.Bounds
                };
                BitmapGetter = id;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRasterLayer"/> class and the specified image data to use for rendering it.
        /// </summary>
        /// <param name="baseRaster">Raster used as data for the layer.</param>
        /// <param name="baseImage">ImageData used for rendering.</param>
        public MapRasterLayer(IRaster baseRaster, ImageData baseImage)
            : base(baseRaster)
        {
            LegendText = Path.GetFileNameWithoutExtension(baseRaster.Filename);
            BitmapGetter = baseImage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapRasterLayer"/> class, and will create a "FallLeaves" image based on the raster values.
        /// </summary>
        /// <param name="raster">The raster to use.</param>
        public MapRasterLayer(IRaster raster)
            : base(raster)
        {
            LegendText = Path.GetFileNameWithoutExtension(raster.Filename);

            // string imageFile = Path.ChangeExtension(raster.Filename, ".png");
            // if (File.Exists(imageFile)) File.Delete(imageFile);
            if ((long)raster.NumRows * raster.NumColumns > MaxCellsInMemory)
            {
                // For huge images, assume that GDAL or something was needed anyway,
                // and we would rather avoid having to re-create the pyramids if there is any chance
                // that the old values will work ok.
                string pyrFile = Path.ChangeExtension(raster.Filename, ".mwi");
                if (File.Exists(pyrFile) && File.Exists(Path.ChangeExtension(pyrFile, ".mwh")))
                {
                    BitmapGetter = new PyramidImage(pyrFile);
                    LegendText = Path.GetFileNameWithoutExtension(raster.Filename);
                }
                else
                {
                    BitmapGetter = CreatePyramidImage(pyrFile, DataManager.DefaultDataManager.ProgressHandler);
                }
            }
            else
            {
                // Ensure smaller images match the scheme.
                Bitmap bmp = new(raster.NumColumns, raster.NumRows);
                raster.PaintColorSchemeToBitmap(Symbolizer, bmp, raster.ProgressHandler);

                var id = new InRamImage(bmp) { Bounds = { AffineCoefficients = raster.Bounds.AffineCoefficients } };
                BitmapGetter = id;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back buffer that will be drawn to as part of the initialization process.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ShallowCopy]
        public Image BackBuffer { get; set; }

        /// <summary>
        /// Gets or sets the current buffer.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ShallowCopy]
        public Image Buffer { get; set; }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ShallowCopy]
        public Envelope BufferEnvelope { get; set; }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ShallowCopy]
        public Rectangle BufferRectangle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image layer is initialized.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool IsInitialized { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Call StartDrawing before using this.
        /// </summary>
        /// <param name="rectangles">The rectangular region in pixels to clear.</param>
        /// <param name= "color">The color to use when clearing. Specifying transparent
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
        /// This will draw any features that intersect this region. To specify the features
        /// directly, use OnDrawFeatures. This will not clear existing buffer content.
        /// For that call Initialize instead.
        /// </summary>
        /// <param name="args">A GeoArgs clarifying the transformation from geographic to image space.</param>
        /// <param name="regions">The geographic regions to draw.</param>
        /// <param name="selected">Indicates whether to draw the normal colored features or the selection colored features. Because rasters can't be selected they won't be drawn if selected is true.</param>
        public void DrawRegions(MapArgs args, List<Extent> regions, bool selected)
        {
            if (selected) return;
            List<Rectangle> clipRects = args.ProjToPixel(regions);
            DrawWindows(args, regions, clipRects);
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
        /// back buffer (instead of the fore-buffer). Calling draw methods without
        /// calling this may cause exceptions.
        /// </summary>
        /// <param name="preserve">Boolean, true if the front buffer content should be copied to the back buffer
        /// where drawing will be taking place.</param>
        public void StartDrawing(bool preserve)
        {
            Bitmap backBuffer = new(BufferRectangle.Width, BufferRectangle.Height);
            if (Buffer?.Width == backBuffer.Width && Buffer.Height == backBuffer.Height)
            {
                if (preserve)
                {
                    Graphics g = Graphics.FromImage(backBuffer);
                    g.DrawImageUnscaled(Buffer, 0, 0);
                }
            }

            if (BackBuffer != null && BackBuffer != Buffer) BackBuffer.Dispose();
            BackBuffer = backBuffer;
            OnStartDrawing();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Buffer != BackBuffer && Buffer != null)
                {
                    Buffer.Dispose();
                    Buffer = null;
                }

                if (BackBuffer != null)
                {
                    BackBuffer.Dispose();
                    BackBuffer = null;
                }

                BufferEnvelope = null;
                BufferRectangle = Rectangle.Empty;
                IsInitialized = false;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Fires the OnBufferChanged event.
        /// </summary>
        /// <param name="clipRectangles">The Rectangle in pixels.</param>
        protected virtual void OnBufferChanged(List<Rectangle> clipRectangles)
        {
            if (BufferChanged != null)
            {
                ClipArgs e = new(clipRectangles);
                BufferChanged(this, e);
            }
        }

        /// <summary>
        /// Indicates that whatever drawing is going to occur has finished and the contents
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

        /// <summary>
        /// This draws to the back buffer. If the back buffer doesn't exist, this will create one.
        /// This will not flip the back buffer to the front.
        /// </summary>
        /// <param name="args">The map arguments.</param>
        /// <param name="regions">The regions. </param>
        /// <param name="clipRectangles">The clip rectangles.</param>
        private void DrawWindows(MapArgs args, IList<Extent> regions, IList<Rectangle> clipRectangles)
        {
            Graphics g;
            if (args.Device != null)
            {
                g = args.Device; // A device on the MapArgs is optional, but overrides the normal buffering behaviors.
            }
            else
            {
                if (BackBuffer == null) BackBuffer = new Bitmap(BufferRectangle.Width, BufferRectangle.Height);
                g = Graphics.FromImage(BackBuffer);
            }

            int numBounds = Math.Min(regions.Count, clipRectangles.Count);

            for (int i = 0; i < numBounds; i++)
            {
                using Bitmap bmp = BitmapGetter.GetBitmap(regions[i], clipRectangles[i]);
                if (bmp != null) g.DrawImage(bmp, new Rectangle(0, 0, clipRectangles[i].Width, clipRectangles[i].Height));
            }

            if (args.Device == null) g.Dispose();
        }

        #endregion
    }
}