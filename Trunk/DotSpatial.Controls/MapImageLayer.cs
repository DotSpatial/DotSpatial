// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2008 2:46:23 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// GeoImageLayer
    /// </summary>
    public class MapImageLayer : ImageLayer, IMapImageLayer
    {
        #region Events

        /// <summary>
        /// Fires an event that indicates to the parent map-frame that it should first
        /// redraw the specified clip
        /// </summary>
        public event EventHandler<ClipArgs> BufferChanged;

        #endregion

        #region Private Variables

        private Image _backBuffer; // draw to the back buffer, and swap to the stencil when done.
        private Extent _bufferExtent; // the geographic extent of the current buffer.
        private Rectangle _bufferRectangle;
        private bool _isInitialized;
        private Image _stencil; // draw features to the stencil
        private Color transparent;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new default instance of a MapImageLayer
        /// </summary>
        public MapImageLayer()
        {
        }

        /// <summary>
        /// Creates a new instance of GeoImageLayer
        /// </summary>
        public MapImageLayer(IImageData baseImage)
            : base(baseImage)
        {
            Configure(baseImage);
        }

        /// <summary>
        /// Creates a new instance of a GeoImageLayer
        /// </summary>
        /// <param name="baseImage">The image to draw as a layer</param>
        /// <param name="container">The Layers collection that keeps track of the image layer</param>
        public MapImageLayer(IImageData baseImage, ICollection<ILayer> container)
            : base(baseImage, container)
        {
            Configure(baseImage);
        }

        /// <summary>
        /// Creates a new instance of a GeoImageLayer
        /// </summary>
        /// <param name="baseImage">The image to draw as a layer</param>
        /// <param name="transparent">The color to make transparent when drawing the image.</param>
        public MapImageLayer(IImageData baseImage, Color transparent)
            : base(baseImage)
        {
            Configure(baseImage);
            this.transparent = transparent;
        }

        private void Configure(IImageData baseImage)
        {
            _bufferRectangle = new Rectangle(0, 0, baseImage.Width, baseImage.Height);
            _bufferExtent = baseImage.Bounds.Extent;
            base.IsVisible = true;
            MyExtent = baseImage.Extent;
            base.LegendText = Path.GetFileName(baseImage.Filename);
            OnFinishedLoading();
        }

        #endregion

        #region Methods

        /// <summary>
        /// This will draw any features that intersect this region.  To specify the features
        /// directly, use OnDrawFeatures.  This will not clear existing buffer content.
        /// For that call Initialize instead.
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back buffer that will be drawn to as part of the initialization process.
        /// </summary>
        public Image BackBuffer
        {
            get { return _backBuffer; }
            set { _backBuffer = value; }
        }

        /// <summary>
        /// Gets the current buffer.
        /// </summary>
        public Image Buffer
        {
            get { return _stencil; }
            set { _stencil = value; }
        }

        /// <summary>
        /// Gets or sets the geographic region represented by the buffer
        /// Calling Initialize will set this automatically.
        /// </summary>
        public Extent BufferExtent
        {
            get { return _bufferExtent; }
            set { _bufferExtent = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle in pixels to use as the back buffer.
        /// Calling Initialize will set this automatically.
        /// </summary>
        public Rectangle BufferRectangle
        {
            get { return _bufferRectangle; }
            set { _bufferRectangle = value; }
        }

        /// <summary>
        /// Gets or sets whether the image layer is initialized
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool IsInitialized
        {
            get { return _isInitialized; }
            set { _isInitialized = value; }
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

        #endregion

        #region Private Methods

        /// <summary>
        /// This draws to the back buffer.  If the Backbuffer doesn't exist, this will create one.
        /// This will not flip the back buffer to the front.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="regions"></param>
        /// <param name="clipRectangles"></param>
        private void DrawWindows(MapArgs args, IList<Extent> regions, IList<Rectangle> clipRectangles)
        {
            Graphics g;
            if (args.Device != null)
            {
                g = args.Device; // A device on the MapArgs is optional, but overrides the normal buffering behaviors.
            }
            else
            {
                if (_backBuffer == null) _backBuffer = new Bitmap(_bufferRectangle.Width, _bufferRectangle.Height);
                g = Graphics.FromImage(_backBuffer);
            }
            int numBounds = Math.Min(regions.Count, clipRectangles.Count);

            for (int i = 0; i < numBounds; i++)
            {
                // For panning tiles, the region needs to be expanded.
                // This is not always 1 pixel.  When very zoomed in, this could be many pixels,
                // but should correspond to 1 pixel in the source image.

                int dx = (int)Math.Ceiling(DataSet.Bounds.AffineCoefficients[1] * clipRectangles[i].Width / regions[i].Width);
                Rectangle r = RectangleExt.ExpandBy(clipRectangles[i], dx * 2);
                if (r.X < 0) r.X = 0;
                if (r.Y < 0) r.Y = 0;
                if (r.Width > 2 * clipRectangles[i].Width) r.Width = 2 * clipRectangles[i].Width;
                if (r.Height > 2 * clipRectangles[i].Height) r.Height = 2 * clipRectangles[i].Height;
                Extent env = regions[i].Reproportion(clipRectangles[i], r);
                Bitmap bmp;
                try
                {
                    bmp = DataSet.GetBitmap(env, r);
                    if (!transparent.Name.Equals("0")) { bmp.MakeTransparent(transparent); }
                }
                catch
                {
                    continue;
                }
                if (bmp == null) continue;
                g.DrawImage(bmp, r);
                bmp.Dispose();
            }
            if (args.Device == null) g.Dispose();
        }

        #endregion
    }
}