// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for double rasters.
    /// </summary>
    [TestFixture]
    public class DoubleRasterTest
    {
        /// <summary>
        /// Creates a raster and checks whether the contained values are the same as the values that were written to the raster.
        /// </summary>
        [Test]
        public void SmallRasterTest()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "doubletest.BGD");

            const double Xllcorner = 3267132.224761;
            const double Yllcorner = 5326939.203029;
            const int Ncols = 1098;
            const int Nrows = 725;
            const double Cellsize = 500;
            double x2 = Xllcorner + (Cellsize * Ncols);
            double y2 = Yllcorner + (Cellsize * Nrows);
            Extent myExtent = new Extent(Xllcorner, Yllcorner, x2, y2);
            var output = Raster.Create(path, string.Empty, Ncols, Nrows, 1, typeof(double), new[] { string.Empty });
            output.Bounds = new RasterBounds(Nrows, Ncols, myExtent);
            output.NoDataValue = -9999;
            int mRow = output.Bounds.NumRows;
            int mCol = output.Bounds.NumColumns;

            // fill all cells to value=2 only for testing
            for (int row = 0; row < mRow; row++)
            {
                for (int col = 0; col < mCol; col++)
                {
                    output.Value[row, col] = 2d;
                }
            }

            output.Save();

            IRaster testRaster = Raster.Open(path);
            for (int row = 0; row < mRow; row++)
            {
                for (int col = 0; col < mCol; col++)
                {
                    Assert.AreEqual(output.Value[row, col], testRaster.Value[row, col]);
                }
            }

            File.Delete(path);
        }
    }
}
