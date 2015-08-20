using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using DotSpatial.Topology.Geometries;
using MWPoint = DotSpatial.Topology.Geometries.Point;

namespace DotSpatial.Plugins.WebMap.Tiling
{
    internal static class TileCalculator
    {
        /* Adapted from methods at http://msdn.microsoft.com/en-us/library/bb259689.aspx */

        public const double MinLatitude = -85.05112878;
        public const double MaxLatitude = 85.05112878;
        public const double MinLongitude = -180;
        public const double MaxLongitude = 180;
        public const double EarthRadiusKms = 6378137;

        public const double MaxWebMercX = 20037497.2108402;
        public const double MinWebMercX = -20037497.2108402;
        public const double MaxWebMercY = 20037508.3430388;
        public const double MinWebMercY = -20037508.3430388;

        /// <summary>
        ///
        /// </summary>
        /// <param name="envelope"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public static int DetermineZoomLevel(Envelope envelope, Rectangle rectangle)
        {
            double metersAcross = EarthRadiusKms * envelope.Width * Math.PI / 180; //find the arc length represented by the displayed map
            metersAcross *= Math.Cos(envelope.Center().Y * Math.PI / 180); //correct for the center latitude

            double metersAcrossPerPixel = metersAcross / rectangle.Width; //find the resolution in meters per pixel

            //find zoomlevel such that metersAcrossPerPix is close
            for (int i = 2; i < 19; i++)
            {
                double groundRes = GroundResolution(envelope.Center().Y, i);

                if (metersAcrossPerPixel > groundRes)
                {
                    //fix zoom level..
                    //changed to a slightly lower zoom level to increase readability
                    if (i > 2 && i < 18) return i - 1;
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Clips a number to the specified minimum and maximum values.
        /// </summary>
        /// <param name="n">The number to clip.</param>
        /// <param name="minValue">Minimum allowable value.</param>
        /// <param name="maxValue">Maximum allowable value.</param>
        /// <returns>The clipped value.</returns>
        public static double Clip(double n, double minValue, double maxValue)
        {
            return Math.Min(Math.Max(n, minValue), maxValue);
        }

        /// <summary>
        /// Determines the map width and height (in pixels) at a specified level
        /// of detail.
        /// </summary>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail)
        /// to 23 (highest detail).  </param>
        /// <returns>The map width and height in pixels.</returns>
        public static uint MapSize(int levelOfDetail)
        {
            return (uint)256 << levelOfDetail;
        }

        /// <summary>
        /// Determines the ground resolution (in meters per pixel) at a specified
        /// latitude and level of detail.
        /// </summary>
        /// <param name="latitude">Latitude (in degrees) at which to measure the
        /// ground resolution.  </param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail)
        /// to 23 (highest detail).  </param>
        /// <returns>The ground resolution, in meters per pixel.</returns>
        public static double GroundResolution(double latitude, int levelOfDetail)
        {
            latitude = Clip(latitude, MinLatitude, MaxLatitude);
            return Math.Cos(latitude * Math.PI / 180) * 2 * Math.PI * EarthRadiusKms / MapSize(levelOfDetail);
        }

        /// <summary>
        /// Determines the map scale at a specified latitude, level of detail,
        /// and screen resolution.
        /// </summary>
        /// <param name="latitude">Latitude (in degrees) at which to measure the
        /// map scale.  </param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail)
        /// to 23 (highest detail).  </param>
        /// <param name="screenDpi">Resolution of the screen, in dots per inch.</param>
        /// <returns>The map scale, expressed as the denominator N of the ratio 1 : N.</returns>
        public static double MapScale(double latitude, int levelOfDetail, int screenDpi)
        {
            return GroundResolution(latitude, levelOfDetail) * screenDpi / 0.0254;
        }

        /// <summary>
        /// Converts a point from latitude/longitude WGS-84 coordinates (in degrees)
        /// into pixel XY coordinates at a specified level of detail.
        /// </summary>
        /// <param name="latitude">Latitude of the point, in degrees.</param>
        /// <param name="longitude">Longitude of the point, in degrees.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail)
        /// to 23 (highest detail).  </param>
        public static MWPoint LatLongToPixelXY(double latitude, double longitude, int levelOfDetail)
        {
            latitude = Clip(latitude, MinLatitude, MaxLatitude);
            longitude = Clip(longitude, MinLongitude, MaxLongitude);

            var x = (longitude + 180) / 360;
            var sinLatitude = Math.Sin(latitude * Math.PI / 180);
            var y = 0.5 - Math.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Math.PI);

            var mapSize = MapSize(levelOfDetail);

            var pixelXY = new MWPoint
            {
                X = (int)Clip(x * mapSize + 0.5, 0, mapSize - 1),
                Y = (int)Clip(y * mapSize + 0.5, 0, mapSize - 1)
            };

            return pixelXY;
        }

        /// <summary>
        /// Converts a point from latitude/longitude WGS-84 coordinates (in degrees)
        /// into pixel XY coordinates at a specified level of detail.
        /// </summary>
        /// <param name="coord">Latitude, Longitude Coordinate of the point, in degrees.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail)
        /// to 23 (highest detail).  </param>
        public static MWPoint LatLongToPixelXY(Coordinate coord, int levelOfDetail)
        {
            return LatLongToPixelXY(coord.Y, coord.X, levelOfDetail);
        }

        /// <summary>
        /// Converts a pixel from pixel XY coordinates at a specified level of detail
        /// into latitude/longitude WGS-84 coordinates (in degrees).
        /// </summary>
        /// <param name="pixelX">X coordinate of the point, in pixels.</param>
        /// <param name="pixelY">Y coordinates of the point, in pixels.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail)
        /// to 23 (highest detail).  </param>
        public static Coordinate PixelXYToLatLong(int pixelX, int pixelY, int levelOfDetail)
        {
            double mapSize = MapSize(levelOfDetail);
            double x = (Clip(pixelX, 0, mapSize - 1) / mapSize) - 0.5;
            double y = 0.5 - (Clip(pixelY, 0, mapSize - 1) / mapSize);

            double latitude = 90 - 360 * Math.Atan(Math.Exp(-y * 2 * Math.PI)) / Math.PI;
            double longitude = 360 * x;

            return new Coordinate(longitude, latitude);
        }

        /// <summary>
        /// Converts pixel XY coordinates into tile XY coordinates of the tile containing
        /// the specified pixel.
        /// </summary>
        /// <param name="pixelX">Pixel X coordinate.</param>
        /// <param name="pixelY">Pixel Y coordinate.</param>
        public static MWPoint PixelXYToTileXY(int pixelX, int pixelY)
        {
            return new MWPoint(pixelX / 256, pixelY / 256);
        }

        /// <summary>
        /// Converts pixel XY coordinates into tile XY coordinates of the tile containing
        /// the specified pixel.
        /// </summary>
        /// <param name="point">Pixel X,Y point.</param>
        public static MWPoint PixelXYToTileXY(MWPoint point)
        {
            return PixelXYToTileXY((int)point.X, (int)point.Y);
        }

        /// <summary>
        /// Converts tile XY coordinates into pixel XY coordinates of the upper-left pixel
        /// of the specified tile.
        /// </summary>
        /// <param name="tileX">Tile X coordinate.</param>
        /// <param name="tileY">Tile Y coordinate.</param>
        public static MWPoint TileXYToTopLeftPixelXY(int tileX, int tileY)
        {
            return new MWPoint(tileX * 256, tileY * 256);
        }

        /// <summary>
        /// Converts tile XY coordinates into pixel XY coordinates of the bottom-right pixel
        /// of the specified tile.
        /// </summary>
        /// <param name="tileX"></param>
        /// <param name="tileY"></param>
        /// <returns></returns>
        public static MWPoint TileXYToBottomRightPixelXY(int tileX, int tileY)
        {
            return new MWPoint((tileX + 1) * 256 - 1, (tileY + 1) * 256 - 1);
        }

        /// <summary>
        /// Converts a WGS-84 Lat/Long coordinate to the tile XY of the tile containing
        /// that point at the given levelOfDetail
        /// </summary>
        /// <param name="coord">WGS-84 Lat/Long</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail)
        /// to 23 (highest detail).  </param>
        /// <returns>Tile XY Point</returns>
        public static MWPoint LatLongToTileXY(Coordinate coord, int levelOfDetail)
        {
            MWPoint pixelXY = LatLongToPixelXY(coord.Y, coord.X, levelOfDetail);
            MWPoint tileXY = PixelXYToTileXY(pixelXY);

            return tileXY;
        }

        /// <summary>
        /// Converts tile XY coordinates into a QuadKey at a specified level of detail.
        /// </summary>
        /// <param name="tileX">Tile X coordinate.</param>
        /// <param name="tileY">Tile Y coordinate.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail)
        /// to 23 (highest detail).  </param>
        /// <returns>A string containing the Bing QuadKey.</returns>
        public static string TileXYToBingQuadKey(int tileX, int tileY, int levelOfDetail)
        {
            var quadKey = new StringBuilder();
            for (var i = levelOfDetail; i > 0; i--)
            {
                var digit = '0';
                var mask = 1 << (i - 1);
                if ((tileX & mask) != 0)
                {
                    digit++;
                }
                if ((tileY & mask) != 0)
                {
                    digit++;
                    digit++;
                }
                quadKey.Append(digit);
            }
            return quadKey.ToString();
        }

        /// <summary>
        /// Takes a 2-dimensional array of tiles and stitches it into a single Bitmap for display on the Map
        /// </summary>
        /// <param name="tiles">2-dimensional array of tiles, [x by y]</param>
        /// <param name="opacity">Opacity of final image</param>
        /// <returns>Bitmap of the tiles stitched together</returns>
        public static Bitmap StitchTiles(Bitmap[,] tiles, short opacity)
        {
            var width = tiles.GetLength(0) * 256;
            var height = tiles.GetLength(1) * 256;

            //create a bitmap to hold the combined image
            var finalImage = new Bitmap(width, height);

            //get a graphics object from the image so we can draw on it
            using (var g = Graphics.FromImage(finalImage))
            {
                //set background color
                g.Clear(Color.Transparent);

                using (var iaPic = new ImageAttributes())
                {
                    var cmxPic = new ColorMatrix {Matrix33 = opacity/100f};
                    iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //go through each image and "draw" it on the final image
                    for (var y = 0; y < tiles.GetLength(1); y++)
                    {
                        for (var x = 0; x < tiles.GetLength(0); x++)
                        {
                            if (tiles[x, y] != null)
                            {
                                var tile = tiles[x, y];
                                g.DrawImage(tile, new Rectangle(x*256, y*256, tile.Width, tile.Height),
                                    0, 0, tile.Width, tile.Height, GraphicsUnit.Pixel, iaPic);
                            }
                        }
                    }
                }
            }

            return finalImage;
        }
    }
}