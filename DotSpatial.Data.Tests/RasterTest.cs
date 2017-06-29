using System;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Data.Rasters.GdalExtension;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    ///This is a test class for RasterTest and is intended
    ///to contain all RasterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RasterTest
    {
        /// <summary>
        ///A test for GetNoDataCellCount
        ///</summary>
        /*[TestMethod()]
        [Ignore] // Test data not exists
        public void GetNoDataCellCountTest()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\..\Data\GetNoDataCellCountTest.BGD";

            const double xllcorner = 3267132.224761;
            const double yllcorner = 5326939.203029;
            const int ncols = 512;
            const int nrows = 128;
            const int frequencyOfNoValue = 5;

            const double cellsize = 500;
            double x2 = xllcorner + (cellsize * ncols);
            double y2 = yllcorner + (cellsize * nrows);
            Extent myExtent = new Extent(xllcorner, yllcorner, x2, y2);
            Raster target;
            target = Raster.Create(path, String.Empty, ncols, nrows, 1, typeof(double), new[] { String.Empty }) as Raster;
            target.Bounds = new RasterBounds(nrows, ncols, myExtent);
            target.NoDataValue = -9999;
            int mRow = target.Bounds.NumRows;
            int mCol = target.Bounds.NumColumns;

            for (int row = 0; row < mRow; row++)
            {
                for (int col = 0; col < mCol; col++)
                {
                    if (row % frequencyOfNoValue == 0)
                        target.Value[row, col] = -9999d;
                    else
                        target.Value[row, col] = 2d;
                }
            }
            target.Save();

            long expected = (nrows / frequencyOfNoValue) * ncols + ncols;
            long actual;
            actual = target.GetNoDataCellCount();
            Assert.AreEqual(expected, actual);

            System.IO.File.Delete(path);
        }*/

        /// <summary>
        ///A test for SaveAs
        ///</summary>
        [TestMethod()]
        public void SaveAsTest()
        {
            var GridDataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Grids\");
            var p = new GdalRasterProvider();
            var sourceGrid = p.Open(GridDataFolder + @"elev_cm_ESRI\elev_cm_clip2\hdr.adf");
            var sourceGridMaximum = sourceGrid.Maximum;

            var savedGridName = GridDataFolder + @"elev_cm.tif";
            sourceGrid.SaveAs(savedGridName);

            Assert.AreEqual(sourceGrid.Maximum, sourceGridMaximum, 0.0001);

            var savedSourceGrid = Raster.Open(savedGridName);

            Assert.AreEqual(sourceGridMaximum, savedSourceGrid.Maximum, 0.0001);

            sourceGrid.Close();
            savedSourceGrid.Close();
            File.Delete(savedGridName);
        }
    }
}