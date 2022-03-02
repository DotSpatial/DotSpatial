// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using DotSpatial.Plugins.WebMap;
using NetTopologySuite.Geometries;

namespace DotSpatial.Plugins.WebMap.Tiling
{
    /// <summary>
    /// The tile manager manages the way tiles are gotten.
    /// </summary>
    internal class TileManager
    {
        #region Fields

        private readonly ServiceProvider _serviceProvider;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TileManager"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider that gets the tiles.</param>
        public TileManager(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the tiles.
        /// </summary>
        /// <param name="envelope">Envelope that indicates for which region the tiles are needed.</param>
        /// <param name="bounds">Bounds needed for zoom level calculation.</param>
        /// <param name="bw">The background worker.</param>
        /// <returns>The tiles needed for the envelope.</returns>
        public Tiles GetTiles(Envelope envelope, Rectangle bounds, BackgroundWorker bw)
        {
            Coordinate mapTopLeft = new(envelope.MinX, envelope.MaxY);
            Coordinate mapBottomRight = new(envelope.MaxX, envelope.MinY);

            // Clip the coordinates so they are in the range of the web mercator projection
            mapTopLeft.Y = TileCalculator.Clip(mapTopLeft.Y, TileCalculator.MinLatitude, TileCalculator.MaxLatitude);
            mapTopLeft.X = TileCalculator.Clip(mapTopLeft.X, TileCalculator.MinLongitude, TileCalculator.MaxLongitude);

            mapBottomRight.Y = TileCalculator.Clip(mapBottomRight.Y, TileCalculator.MinLatitude, TileCalculator.MaxLatitude);
            mapBottomRight.X = TileCalculator.Clip(mapBottomRight.X, TileCalculator.MinLongitude, TileCalculator.MaxLongitude);

            var zoom = TileCalculator.DetermineZoomLevel(envelope, bounds);

            var topLeftTileXy = TileCalculator.LatLongToTileXy(mapTopLeft, zoom);
            var btmRightTileXy = TileCalculator.LatLongToTileXy(mapBottomRight, zoom);

            var tileMatrix = new Bitmap[(int)(btmRightTileXy.X - topLeftTileXy.X) + 1, (int)(btmRightTileXy.Y - topLeftTileXy.Y) + 1];
            var po = new ParallelOptions
            {
                MaxDegreeOfParallelism = -1
            };
            Parallel.For((int)topLeftTileXy.Y, (int)btmRightTileXy.Y + 1, po, (y, loopState) => Parallel.For((int)topLeftTileXy.X, (int)btmRightTileXy.X + 1, po, (x, loopState2) =>
                {
                    if (bw.CancellationPending)
                    {
                        loopState.Stop();
                        loopState2.Stop();
                        return;
                    }

                    var currEnv = GetTileEnvelope(x, y, zoom);
                    tileMatrix[x - (int)topLeftTileXy.X, y - (int)topLeftTileXy.Y] = GetTile(x, y, currEnv, zoom);
                }));

            return new Tiles(
                tileMatrix,
                GetTileEnvelope((int)topLeftTileXy.X, (int)topLeftTileXy.Y, zoom), // top left tile = tileMatrix[0,0]
                GetTileEnvelope((int)btmRightTileXy.X, (int)btmRightTileXy.Y, zoom)); // bottom right tile = tileMatrix[last, last]
        }

        /// <summary>
        /// Get tile envelope in WGS-84 coordinates.
        /// </summary>
        /// <param name="x">x index.</param>
        /// <param name="y">y index.</param>
        /// <param name="zoom">zoom.</param>
        /// <returns>Envelope in WGS-84.</returns>
        private static Envelope GetTileEnvelope(int x, int y, int zoom)
        {
            var currTopLeftPixXy = TileCalculator.TileXyToTopLeftPixelXy(x, y);
            var currTopLeftCoord = TileCalculator.PixelXyToLatLong((int)currTopLeftPixXy.X, (int)currTopLeftPixXy.Y, zoom);

            var currBtmRightPixXy = TileCalculator.TileXyToBottomRightPixelXy(x, y);
            var currBtmRightCoord = TileCalculator.PixelXyToLatLong((int)currBtmRightPixXy.X, (int)currBtmRightPixXy.Y, zoom);
            return new Envelope(currTopLeftCoord, currBtmRightCoord);
        }

        private Bitmap GetTile(int x, int y, Envelope envelope, int zoom)
        {
            Bitmap bm;
            try
            {
                bm = _serviceProvider.GetBitmap(x, y, envelope, zoom) ?? new Bitmap(new MemoryStream(Resources.nodata));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                bm = new Bitmap(new MemoryStream(Resources.nodata));
            }

            return bm;
        }

        #endregion
    }
}