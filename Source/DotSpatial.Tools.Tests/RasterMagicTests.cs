// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;
using DotSpatial.Data;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    /// <summary>
    /// Tests for RasterMagic.
    /// </summary>
    [TestFixture]
    internal class RasterMagicTests
    {
        /// <summary>
        /// Tests that the output of the raster math operation has the same bounds as the input.
        /// </summary>
        [Test]
        public void RasterMathOutputHasSameBounds()
        {
            // Prepare input raster
            const double Xllcorner = 3267132.224761;
            const double Yllcorner = 5326939.203029;
            const int Ncols = 39;
            const int Nrows = 57;
            const double Cellsize = 500;
            const double X2 = Xllcorner + (Cellsize * Ncols);
            const double Y2 = Yllcorner + (Cellsize * Nrows);
            var myExtent = new Extent(Xllcorner, Yllcorner, X2, Y2);
            var source = new Raster<int>(Nrows, Ncols)
            {
                Bounds = new RasterBounds(Nrows, Ncols, myExtent),
                NoDataValue = -9999
            };
            var mRow = source.Bounds.NumRows;
            var mCol = source.Bounds.NumColumns;

            var i = 0;
            for (var row = 0; row < mRow; row++)
            {
                for (var col = 0; col < mCol; col++)
                {
                    source.Value[row, col] = i++;
                }
            }

            var target = new RasterMultiply();
            IRaster outRaster = new Raster { Filename = FileTools.GetTempFileName(".bgd") };
            target.Execute(source, source, outRaster, new MockProgressHandler());
            outRaster = Raster.Open(outRaster.Filename);
            File.Delete(outRaster.Filename);

            Assert.AreEqual(source.NumColumns, outRaster.NumColumns);
            Assert.AreEqual(source.NumRows, outRaster.NumRows);
            Assert.AreEqual(source.Bounds.Extent, outRaster.Bounds.Extent);
        }
    }
}
