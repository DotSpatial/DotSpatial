// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a specialized FeatureLayer that specifically handles drawing tiled images.
    /// </summary>
    public class MapTiledImageLayer : TiledImageLayer
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapTiledImageLayer"/> class.
        /// </summary>
        /// <param name="baseImage">The image that is the base of this layer.</param>
        public MapTiledImageLayer(ITiledImage baseImage)
            : base(baseImage)
        {
            Configure(baseImage);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This will draw any features that intersect this region. To specify the features
        /// directly, use OnDrawFeatures. This will not clear existing buffer content.
        /// For that call Initialize instead.
        /// </summary>
        /// <param name="args">A GeoArgs clarifying the transformation from geographic to image space.</param>
        /// <param name="regions">The geographic regions to draw.</param>
        public void DrawRegions(MapArgs args, List<Extent> regions)
        {
            List<Rectangle> clipRects = args.ProjToPixel(regions);
            DrawWindows(args, regions, clipRects);
        }

        private void Configure(ITiledImage baseImage)
        {
            IsVisible = true;
            LegendText = Path.GetFileName(baseImage.Filename);
            OnFinishedLoading();
        }

        /// <summary>
        /// This draws to the back buffer. If the back buffer doesn't exist, this will create one.
        /// This will not flip the back buffer to the front.
        /// </summary>
        /// <param name="args">A GeoArgs clarifying the transformation from geographic to image space.</param>
        /// <param name="regions">The geographic regions to draw.</param>
        /// <param name="clipRectangles">The clip rectangles.</param>
        private void DrawWindows(MapArgs args, IList<Extent> regions, IList<Rectangle> clipRectangles)
        {
            Graphics g = args.Device;
            int numBounds = Math.Min(regions.Count, clipRectangles.Count);

            for (int i = 0; i < numBounds; i++)
            {
                Bitmap bmp = DataSet.GetBitmap(regions[i], clipRectangles[i].Size);
                if (bmp != null) g.DrawImage(bmp, clipRectangles[i]);
            }

            if (args.Device == null) g.Dispose();
        }

        #endregion
    }
}