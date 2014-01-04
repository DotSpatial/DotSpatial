using System;
using System.IO;
using DotSpatial.Data.Rasters.GdalExtension;
using NUnit.Framework;
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
        [TestMethod]
        public void GetNoDataCellCountTest()
        {
            var path = Path.ChangeExtension(Path.GetTempFileName(), "bgd");
            const double xllcorner = 3267132.224761;
            const double yllcorner = 5326939.203029;
            const int ncols = 512;
            const int nrows = 128;
            const int frequencyOfNoValue = 5;

            const double cellsize = 500;
            var x2 = xllcorner + (cellsize * ncols);
            var y2 = yllcorner + (cellsize * nrows);
            var myExtent = new Extent(xllcorner, yllcorner, x2, y2);
            var target = (Raster)Raster.Create(path, String.Empty, ncols, nrows, 1, typeof(double), new[] { String.Empty });
            target.Bounds = new RasterBounds(nrows, ncols, myExtent);
            target.NoDataValue = -9999;
            var mRow = target.Bounds.NumRows;
            var mCol = target.Bounds.NumColumns;

            for (var row = 0; row < mRow; row++)
            {
                for (var col = 0; col < mCol; col++)
                {
                    if (row % frequencyOfNoValue == 0)
                        target.Value[row, col] = -9999d;
                    else
                        target.Value[row, col] = 2d;
                }
            }
            target.Save();

            const long expected = (nrows / frequencyOfNoValue) * ncols + ncols;
            var actual = target.GetNoDataCellCount();
            Assert.AreEqual(expected, actual);

            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///A test for SaveAs
        ///</summary>
        [TestMethod]
        public void SaveAsTest()
        {
            const string GridDataFolder = @"Data\Grids\";
            var p = new GdalRasterProvider();
            var sourceGrid = p.Open(GridDataFolder + @"elev_cm_ESRI\elev_cm_clip2\hdr.adf");
            var sourceGridMaximum = sourceGrid.Maximum;

            const string savedGridName = GridDataFolder + @"elev_cm.tif";
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