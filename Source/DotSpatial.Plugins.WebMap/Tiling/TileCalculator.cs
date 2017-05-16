﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using DotSpatial.NTSExtension;
using GeoAPI.Geometries;
using NtsPoint = NetTopologySuite.Geometries.Point;

namespace DotSpatial.Plugins.WebMap.Tiling
{
    /// <summary>
    /// Class that is used for tile calculation.
    /// </summary>
    internal static class TileCalculator
    {
        #region Fields

        /// <summary>
        /// The earth radius in kms.
        /// </summary>
        public const double EarthRadiusKms = 6378137;

        /// <summary>
        /// The maximal latitude.
        /// </summary>
        public const double MaxLatitude = 85.05112878;

        /// <summary>
        /// The maximal longitude.
        /// </summary>
        public const double MaxLongitude = 180;

        /// <summary>
        /// The maximal webmercator X.
        /// </summary>
        public const double MaxWebMercX = 20037497.2108402;

        /// <summary>
        /// The maximal webmercator Y.
        /// </summary>
        public const double MaxWebMercY = 20037508.3430388;

        /* Adapted from methods at http://msdn.microsoft.com/en-us/library/bb259689.aspx */

            /// <summary>
            /// The minimal latitude.
            /// </summary>
        public const double MinLatitude = -85.05112878;

        /// <summary>
        /// The minimal longitude.
        /// </summary>
        public const double MinLongitude = -180;

        /// <summary>
        /// The minimal webmercator X.
        /// </summary>
        public const double MinWebMercX = -20037497.2108402;

        /// <summary>
        /// The minimal webmercator Y.
        /// </summary>
        public const double MinWebMercY = -20037508.3430388;

        #endregion

        #region Methods

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
        /// Determines the zoom level from which the tiles should be gotten.
        /// </summary>
        /// <param name="envelope">Envelope of the region the tiles are needed for.</param>
        /// <param name="rectangle">Screen rectangle used for zoom level calculation.</param>
        /// <returns>The zoom level.</returns>
        public static int DetermineZoomLevel(Envelope envelope, Rectangle rectangle)
        {
            double metersAcross = EarthRadiusKms * envelope.Width * Math.PI / 180; // find the arc length represented by the displayed map
            metersAcross *= Math.Cos(envelope.Center().Y * Math.PI / 180); // correct for the center latitude

            double metersAcrossPerPixel = metersAcross / rectangle.Width; // find the resolution in meters per pixel

            // find zoomlevel such that metersAcrossPerPix is close
            for (int i = 2; i < 19; i++)
            {
                double groundRes = GroundResolution(envelope.Center().Y, i);

                if (metersAcrossPerPixel > groundRes)
                {
                    // fix zoom level.
                    // changed to a slightly lower zoom level to increase readability
                    if (i > 2 && i < 18) return i - 1;
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Determines the ground resolution (in meters per pixel) at a specified latitude and level of detail.
        /// </summary>
        /// <param name="latitude">Latitude (in degrees) at which to measure the ground resolution.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>The ground resolution, in meters per pixel.</returns>
        public static double GroundResolution(double latitude, int levelOfDetail)
        {
            latitude = Clip(latitude, MinLatitude, MaxLatitude);
            return Math.Cos(latitude * Math.PI / 180) * 2 * Math.PI * EarthRadiusKms / MapSize(levelOfDetail);
        }

        /// <summary>
        /// Converts a point from latitude/longitude WGS-84 coordinates (in degrees)
        /// into pixel XY coordinates at a specified level of detail.
        /// </summary>
        /// <param name="latitude">Latitude of the point, in degrees.</param>
        /// <param name="longitude">Longitude of the point, in degrees.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>Pixel XY coordinate</returns>
        public static NtsPoint LatLongToPixelXy(double latitude, double longitude, int levelOfDetail)
        {
            latitude = Clip(latitude, MinLatitude, MaxLatitude);
            longitude = Clip(longitude, MinLongitude, MaxLongitude);

            var x = (longitude + 180) / 360;
            var sinLatitude = Math.Sin(latitude * Math.PI / 180);
            var y = 0.5 - (Math.Log((1 + sinLatitude) / (1 - sinLatitude)) / (4 * Math.PI));

            var mapSize = MapSize(levelOfDetail);

            return new NtsPoint((int)Clip((x * mapSize) + 0.5, 0, mapSize - 1), (int)Clip((y * mapSize) + 0.5, 0, mapSize - 1));
        }

        /// <summary>
        /// Converts a point from latitude/longitude WGS-84 coordinates (in degrees)
        /// into pixel XY coordinates at a specified level of detail.
        /// </summary>
        /// <param name="coord">Latitude, Longitude Coordinate of the point, in degrees.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail) to 23 (highest detail). </param>
        /// <returns>Pixel XY coordinate.</returns>
        public static NtsPoint LatLongToPixelXy(Coordinate coord, int levelOfDetail)
        {
            return LatLongToPixelXy(coord.Y, coord.X, levelOfDetail);
        }

        /// <summary>
        /// Converts a WGS-84 Lat/Long coordinate to the tile XY of the tile containing that point at the given levelOfDetail.
        /// </summary>
        /// <param name="coord">WGS-84 Lat/Long</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>Tile XY Point</returns>
        public static NtsPoint LatLongToTileXy(Coordinate coord, int levelOfDetail)
        {
            NtsPoint pixelXy = LatLongToPixelXy(coord.Y, coord.X, levelOfDetail);
            NtsPoint tileXy = PixelXyToTileXy(pixelXy);

            return tileXy;
        }

        /// <summary>
        /// Determines the map scale at a specified latitude, level of detail, and screen resolution.
        /// </summary>
        /// <param name="latitude">Latitude (in degrees) at which to measure the map scale.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <param name="screenDpi">Resolution of the screen, in dots per inch.</param>
        /// <returns>The map scale, expressed as the denominator N of the ratio 1 : N.</returns>
        public static double MapScale(double latitude, int levelOfDetail, int screenDpi)
        {
            return GroundResolution(latitude, levelOfDetail) * screenDpi / 0.0254;
        }

        /// <summary>
        /// Determines the map width and height (in pixels) at a specified level of detail.
        /// </summary>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>The map width and height in pixels.</returns>
        public static uint MapSize(int levelOfDetail)
        {
            return 256U << levelOfDetail;
        }

        /// <summary>
        /// Converts a pixel from pixel XY coordinates at a specified level of detail into latitude/longitude WGS-84 coordinates (in degrees).
        /// </summary>
        /// <param name="pixelX">X coordinate of the point, in pixels.</param>
        /// <param name="pixelY">Y coordinates of the point, in pixels.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>The resulting WGS84 coordinate.</returns>
        public static Coordinate PixelXyToLatLong(int pixelX, int pixelY, int levelOfDetail)
        {
            double mapSize = MapSize(levelOfDetail);
            double x = (Clip(pixelX, 0, mapSize - 1) / mapSize) - 0.5;
            double y = 0.5 - (Clip(pixelY, 0, mapSize - 1) / mapSize);

            double latitude = 90 - (360 * Math.Atan(Math.Exp(-y * 2 * Math.PI)) / Math.PI);
            double longitude = 360 * x;

            return new Coordinate(longitude, latitude);
        }

        /// <summary>
        /// Converts pixel XY coordinates into tile XY coordinates of the tile containing the specified pixel.
        /// </summary>
        /// <param name="pixelX">Pixel X coordinate.</param>
        /// <param name="pixelY">Pixel Y coordinate.</param>
        /// <returns>Tile XY coordinate.</returns>
        public static NtsPoint PixelXyToTileXy(int pixelX, int pixelY)
        {
            return new NtsPoint(pixelX / 256, pixelY / 256);
        }

        /// <summary>
        /// Converts pixel XY coordinates into tile XY coordinates of the tile containing the specified pixel.
        /// </summary>
        /// <param name="point">Pixel X,Y point.</param>
        /// <returns>Tile XY coordinate</returns>
        public static NtsPoint PixelXyToTileXy(NtsPoint point)
        {
            return PixelXyToTileXy((int)point.X, (int)point.Y);
        }

        /// <summary>
        /// Takes a 2-dimensional array of tiles and stitches it into a single Bitmap for display on the Map.
        /// </summary>
        /// <param name="tiles">2-dimensional array of tiles, [x by y]</param>
        /// <param name="opacity">Opacity of final image</param>
        /// <returns>Bitmap of the tiles stitched together</returns>
        public static Bitmap StitchTiles(Bitmap[,] tiles, short opacity)
        {
            var width = tiles.GetLength(0) * 256;
            var height = tiles.GetLength(1) * 256;

            // create a bitmap to hold the combined image
            var finalImage = new Bitmap(width, height);

            // get a graphics object from the image so we can draw on it
            using (var g = Graphics.FromImage(finalImage))
            {
                // set background color
                g.Clear(Color.Transparent);

                using (var iaPic = new ImageAttributes())
                {
                    var cmxPic = new ColorMatrix
                                     {
                                         Matrix33 = opacity / 100f
                                     };
                    iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    // go through each image and "draw" it on the final image
                    for (var y = 0; y < tiles.GetLength(1); y++)
                    {
                        for (var x = 0; x < tiles.GetLength(0); x++)
                        {
                            if (tiles[x, y] != null)
                            {
                                var tile = tiles[x, y];
                                g.DrawImage(tile, new Rectangle(x * 256, y * 256, tile.Width, tile.Height), 0, 0, tile.Width, tile.Height, GraphicsUnit.Pixel, iaPic);
                            }
                        }
                    }
                }
            }

            return finalImage;
        }

        /// <summary>
        /// Converts tile XY coordinates into a QuadKey at a specified level of detail.
        /// </summary>
        /// <param name="tileX">Tile X coordinate.</param>
        /// <param name="tileY">Tile Y coordinate.</param>
        /// <param name="levelOfDetail">Level of detail, from 1 (lowest detail) to 23 (highest detail).</param>
        /// <returns>A string containing the Bing QuadKey.</returns>
        public static string TileXyToBingQuadKey(int tileX, int tileY, int levelOfDetail)
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
        /// Converts tile XY coordinates into pixel XY coordinates of the bottom-right pixel of the specified tile.
        /// </summary>
        /// <param name="tileX">Tile X coordinate.</param>
        /// <param name="tileY">Tile Y coordinate.</param>
        /// <returns>The coordinate of the bottom-right pixel of the tile.</returns>
        public static NtsPoint TileXyToBottomRightPixelXy(int tileX, int tileY)
        {
            return new NtsPoint(((tileX + 1) * 256) - 1, ((tileY + 1) * 256) - 1);
        }

        /// <summary>
        /// Converts tile XY coordinates into pixel XY coordinates of the upper-left pixel of the specified tile.
        /// </summary>
        /// <param name="tileX">Tile X coordinate.</param>
        /// <param name="tileY">Tile Y coordinate.</param>
        /// <returns>The coordinate of the upper-left pixel of the tile.</returns>
        public static NtsPoint TileXyToTopLeftPixelXy(int tileX, int tileY)
        {
            return new NtsPoint(tileX * 256, tileY * 256);
        }

        #endregion
    }
}