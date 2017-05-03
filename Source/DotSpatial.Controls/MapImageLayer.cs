// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2008 2:46:23 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a specialized layer that specifically handles image drawing.
    /// </summary>
    public class MapImageLayer : ImageLayer, IMapImageLayer
    {
        #region Fields

        private Color _transparent;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapImageLayer"/> class.
        /// </summary>
        public MapImageLayer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapImageLayer"/> class.
        /// </summary>
        /// <param name="baseImage">The image to draw as a layer</param>
        public MapImageLayer(IImageData baseImage)
            : base(baseImage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapImageLayer"/> class.
        /// </summary>
        /// <param name="baseImage">The image to draw as a layer</param>
        /// <param name="container">The Layers collection that keeps track of the image layer</param>
        public MapImageLayer(IImageData baseImage, ICollection<ILayer> container)
            : base(baseImage, container)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapImageLayer"/> class.
        /// </summary>
        /// <param name="baseImage">The image to draw as a layer</param>
        /// <param name="transparent">The color to make transparent when drawing the image.</param>
        public MapImageLayer(IImageData baseImage, Color transparent)
            : base(baseImage)
        {
            _transparent = transparent;
        }

        #endregion

        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip.
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back buffer that will be drawn to as part of the initialization process.
        /// </summary>
        public Image BackBuffer { get; set; }

        /// <summary>
        /// Gets or sets the current buffer.
        /// </summary>
        public Image Buffer { get; set; }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        public Extent BufferExtent { get; set; }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
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
        /// This will draw any features that intersect this region. To specify the features directly, use OnDrawFeatures.
        /// This will not clear existing buffer content. For that call Initialize instead.
        /// </summary>
        /// <param name="args">A GeoArgs clarifying the transformation from geographic to image space</param>
        /// <param name="regions">The geographic regions to draw</param>
        public void DrawRegions(MapArgs args, List<Extent> regions)
        {
            List<Rectangle> clipRects = args.ProjToPixel(regions);
            for (int i = clipRects.Count - 1; i >= 0; i--)
            {
                if (clipRects[i].Width != 0 && clipRects[i].Height != 0) continue;
                regions.RemoveAt(i);
                clipRects.RemoveAt(i);
            }

            DrawWindows(args, regions, clipRects);
        }

        /// <summary>
        /// Fires the OnBufferChanged event.
        /// </summary>
        /// <param name="clipRectangles">The Rectangle in pixels</param>
        protected virtual void OnBufferChanged(List<Rectangle> clipRectangles)
        {
            BufferChanged?.Invoke(this, new ClipArgs(clipRectangles));
        }

        /// <summary>
        /// This updates the things that depend on the DataSet so that they fit to the changed DataSet.
        /// </summary>
        /// <param name="value">DataSet that was changed.</param>
        protected override void OnDataSetChanged(IImageData value)
        {
            base.OnDataSetChanged(value);

            BufferRectangle = value == null ? Rectangle.Empty : new Rectangle(0, 0, value.Width, value.Height);
            BufferExtent = value?.Bounds.Extent;
            MyExtent = value?.Extent;
            OnFinishedLoading();
        }

        /// <summary>
        /// This draws to the back buffer. If the Backbuffer doesn't exist, this will create one.
        /// This will not flip the back buffer to the front.
        /// </summary>
        /// <param name="args">The map args.</param>
        /// <param name="regions">The regions.</param>
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
                // For panning tiles, the region needs to be expanded.
                // This is not always 1 pixel. When very zoomed in, this could be many pixels,
                // but should correspond to 1 pixel in the source image.
                int dx = (int)Math.Ceiling(DataSet.Bounds.AffineCoefficients[1] * clipRectangles[i].Width / regions[i].Width);
                int dy = (int)Math.Ceiling(-DataSet.Bounds.AffineCoefficients[5] * clipRectangles[i].Height / regions[i].Height);

                Rectangle r = clipRectangles[i].ExpandBy(dx * 2, dy * 2);
                if (r.X < 0) r.X = 0;
                if (r.Y < 0) r.Y = 0;
                if (r.Width > 2 * clipRectangles[i].Width) r.Width = 2 * clipRectangles[i].Width;
                if (r.Height > 2 * clipRectangles[i].Height) r.Height = 2 * clipRectangles[i].Height;
                Extent env = regions[i].Reproportion(clipRectangles[i], r);
                Bitmap bmp = null;
                try
                {
                    bmp = DataSet.GetBitmap(env, r);
                    if (!_transparent.Name.Equals("0"))
                    {
                        bmp.MakeTransparent(_transparent);
                    }
                }
                catch
                {
                    bmp?.Dispose();
                    continue;
                }

                if (bmp == null) continue;

                if (Symbolizer != null && Symbolizer.Opacity < 1)
                {
                    ColorMatrix matrix = new ColorMatrix
                    {
                        Matrix33 = Symbolizer.Opacity // draws the image not completely opaque
                    };
                    using (var attributes = new ImageAttributes())
                    {
                        attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        g.DrawImage(bmp, r, 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
                    }
                }
                else
                {
                    g.DrawImage(bmp, r);
                }

                bmp.Dispose();
            }

            if (args.Device == null) g.Dispose();
        }

        #endregion
    }
}