using System.IO;
using DotSpatial.Data;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    [TestFixture]
    class RasterMagicTests
    {
        [Test]
        public void RasterMath_OutputHasSameBounds()
        {
            // Prepare input raster
            const double xllcorner = 3267132.224761;
            const double yllcorner = 5326939.203029;
            const int ncols = 39;
            const int nrows = 57;
            const double cellsize = 500;
            const double x2 = xllcorner + (cellsize * ncols);
            const double y2 = yllcorner + (cellsize * nrows);
            var myExtent = new Extent(xllcorner, yllcorner, x2, y2);
            var source = new Raster<int>(nrows, ncols)
            {
                Bounds = new RasterBounds(nrows, ncols, myExtent),
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
            IRaster outRaster = new Raster {Filename = FileTools.GetTempFileName(".bgd")};
            target.Execute(source, source, outRaster, new MockProgressHandler());
            outRaster = Raster.Open(outRaster.Filename);
            File.Delete(outRaster.Filename);

            Assert.AreEqual(source.NumColumns, outRaster.NumColumns);
            Assert.AreEqual(source.NumRows, outRaster.NumRows);
            Assert.AreEqual(source.Bounds.Extent, outRaster.Bounds.Extent);
        }
    }
}
