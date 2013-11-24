using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using DotSpatial.Plugins.WebMap.Properties;
using DotSpatial.Topology;

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
        
        public Tile[,] GetTiles(Envelope envelope, Rectangle bounds)
        {
            var mapTopLeft = envelope.TopLeft();
            var mapBottomRight = envelope.BottomRight();

            //Clip the coordinates so they are in the range of the web mercator projection
            mapTopLeft.Y = TileCalculator.Clip(mapTopLeft.Y, TileCalculator.MinLatitude, TileCalculator.MaxLatitude);
            mapTopLeft.X = TileCalculator.Clip(mapTopLeft.X, TileCalculator.MinLongitude, TileCalculator.MaxLongitude);

            mapBottomRight.Y = TileCalculator.Clip(mapBottomRight.Y, TileCalculator.MinLatitude, TileCalculator.MaxLatitude);
            mapBottomRight.X = TileCalculator.Clip(mapBottomRight.X, TileCalculator.MinLongitude, TileCalculator.MaxLongitude);

            var zoom = TileCalculator.DetermineZoomLevel(envelope, bounds);

            var topLeftTileXY = TileCalculator.LatLongToTileXY(mapTopLeft, zoom);
            var btmRightTileXY = TileCalculator.LatLongToTileXY(mapBottomRight, zoom);

            var tileMatrix = new Tile[(int)(btmRightTileXY.X - topLeftTileXY.X) + 1, (int)(btmRightTileXY.Y - topLeftTileXY.Y) + 1];

            Parallel.For((int) topLeftTileXY.Y, (int) btmRightTileXY.Y + 1,
                         y => Parallel.For((int) topLeftTileXY.X, (int) btmRightTileXY.X + 1,
                                           x =>
                                               {
                                                   var currTopLeftPixXY = TileCalculator.TileXYToTopLeftPixelXY(x, y);
                                                   var currTopLeftCoord =TileCalculator.PixelXYToLatLong((int) currTopLeftPixXY.X,
                                                                                       (int) currTopLeftPixXY.Y, zoom);

                                                   var currBtmRightPixXY = TileCalculator.TileXYToBottomRightPixelXY(x,
                                                                                                                     y);
                                                   var currBtmRightCoord =TileCalculator.PixelXYToLatLong((int) currBtmRightPixXY.X,
                                                                                       (int) currBtmRightPixXY.Y, zoom);

                                                   var currEnv = new Envelope(currTopLeftCoord, currBtmRightCoord);
                                                   tileMatrix[x - (int)topLeftTileXY.X, y - (int)topLeftTileXY.Y] = GetTile(x, y, currEnv, zoom);
                                               }
                                  ));

            return tileMatrix;
        }

        private Tile GetTile(int x, int y, Envelope envelope, int zoom)
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
            return new Tile(x, y, zoom, envelope, bm);
        }
    }
}