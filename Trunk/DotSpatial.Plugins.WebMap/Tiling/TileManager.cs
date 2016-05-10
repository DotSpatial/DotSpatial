using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using DotSpatial.Plugins.WebMap.Properties;
using GeoAPI.Geometries;

namespace DotSpatial.Plugins.WebMap.Tiling
{
    internal class TileManager
    {
        #region Fields

        private readonly ServiceProvider _serviceProvider;

        #endregion

        public TileManager(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Tiles GetTiles(Envelope envelope, Rectangle bounds, BackgroundWorker bw)
        {
            Coordinate mapTopLeft = new Coordinate(envelope.MinX, envelope.MaxY);
            Coordinate mapBottomRight = new Coordinate(envelope.MaxX, envelope.MinY);

            //Clip the coordinates so they are in the range of the web mercator projection
            mapTopLeft.Y = TileCalculator.Clip(mapTopLeft.Y, TileCalculator.MinLatitude, TileCalculator.MaxLatitude);
            mapTopLeft.X = TileCalculator.Clip(mapTopLeft.X, TileCalculator.MinLongitude, TileCalculator.MaxLongitude);

            mapBottomRight.Y = TileCalculator.Clip(mapBottomRight.Y, TileCalculator.MinLatitude, TileCalculator.MaxLatitude);
            mapBottomRight.X = TileCalculator.Clip(mapBottomRight.X, TileCalculator.MinLongitude, TileCalculator.MaxLongitude);

            var zoom = TileCalculator.DetermineZoomLevel(envelope, bounds);

            var topLeftTileXY = TileCalculator.LatLongToTileXY(mapTopLeft, zoom);
            var btmRightTileXY = TileCalculator.LatLongToTileXY(mapBottomRight, zoom);

            var tileMatrix = new Bitmap[(int)(btmRightTileXY.X - topLeftTileXY.X) + 1, (int)(btmRightTileXY.Y - topLeftTileXY.Y) + 1];
            var po = new ParallelOptions { MaxDegreeOfParallelism = -1 };
            Parallel.For((int)topLeftTileXY.Y, (int)btmRightTileXY.Y + 1, po,
                         (y, loopState) => Parallel.For((int)topLeftTileXY.X, (int)btmRightTileXY.X + 1, po,
                                           (x, loopState2) =>
                                           {
                                               if (bw.CancellationPending)
                                               {
                                                   loopState.Stop();
                                                   loopState2.Stop();
                                                   return;
                                               }
                                               var currEnv = GetTileEnvelope(x, y, zoom);
                                               tileMatrix[x - (int)topLeftTileXY.X, y - (int)topLeftTileXY.Y] = GetTile(x, y, currEnv, zoom);
                                           }
                                  ));

            return new Tiles(tileMatrix,
                GetTileEnvelope((int)topLeftTileXY.X, (int)topLeftTileXY.Y, zoom),  // top left tile = tileMatrix[0,0]
                GetTileEnvelope((int)btmRightTileXY.X, (int)btmRightTileXY.Y, zoom) // bottom right tile = tileMatrix[last, last]
                );
        }

        /// <summary>
        /// Get tile envelope in  WGS-84 coordinates
        /// </summary>
        /// <param name="x">x index</param>
        /// <param name="y">y index</param>
        /// <param name="zoom">zoom</param>
        /// <returns>Envelope in WGS-84</returns>
        private static Envelope GetTileEnvelope(int x, int y, int zoom)
        {
            var currTopLeftPixXY = TileCalculator.TileXYToTopLeftPixelXY(x, y);
            var currTopLeftCoord = TileCalculator.PixelXYToLatLong((int)currTopLeftPixXY.X, (int)currTopLeftPixXY.Y, zoom);

            var currBtmRightPixXY = TileCalculator.TileXYToBottomRightPixelXY(x, y);
            var currBtmRightCoord = TileCalculator.PixelXYToLatLong((int)currBtmRightPixXY.X, (int)currBtmRightPixXY.Y, zoom);
            return new Envelope(currTopLeftCoord, currBtmRightCoord);
        }

        private Bitmap GetTile(int x, int y, Envelope envelope, int zoom)
        {
            Bitmap bm;
            try
            {
                bm = _serviceProvider.GetBitmap(x, y, envelope, zoom) ?? Resources.nodata;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                bm = Resources.nodata;
            }
            return bm;
        }
    }
}