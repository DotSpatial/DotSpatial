
using System;
using System.IO;
using DotSpatial.Tests.Common;
using NUnit.Framework;


namespace DotSpatial.Data.Tests
{
    [TestFixture]
    public class DoubleRasterTest
    {
        [Test]
        public void SmallRasterTest()
        {
            string path = FileTools.GetTempFileName(".BGD");

            const double xllcorner = 3267132.224761;
            const double yllcorner = 5326939.203029;
            const int ncols = 1098;
            const int nrows = 725;
            const double cellsize = 500;
            double x2 = xllcorner + (cellsize * ncols);
            double y2 = yllcorner + (cellsize * nrows);
            Extent myExtent = new Extent(xllcorner, yllcorner, x2, y2);
            IRaster output;
            output = Raster.Create(path, String.Empty, ncols, nrows, 1, typeof(double), new[] { String.Empty });
            output.Bounds = new RasterBounds(nrows, ncols, myExtent);
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
            try
            {
                for (int row = 0; row < mRow; row++)
                {
                    for (int col = 0; col < mCol; col++)
                    {
                        Assert.AreEqual(output.Value[row, col], testRaster.Value[row, col]);
                    }
                }
            }
            finally
            {
                System.IO.File.Delete(path);    
            }
            
        }
    }
}
