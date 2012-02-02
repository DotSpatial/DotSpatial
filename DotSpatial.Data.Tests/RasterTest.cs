using System;
using DotSpatial.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    ///This is a test class for RasterTest and is intended
    ///to contain all RasterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RasterTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion Additional test attributes

        /// <summary>
        ///A test for GetNoDataCellCount
        ///</summary>
        [TestMethod()]
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
        }

        /// <summary>
        ///A test for SaveAs
        ///</summary>
        [TestMethod()]
        public void SaveAsTest()
        {
            if (DotSpatial.Data.DataManager.DefaultDataManager.PreferredProviders.Count == 0)
            {
                DotSpatial.Data.Rasters.GdalExtension.GdalRasterProvider lGdalRasterProvider = new DotSpatial.Data.Rasters.GdalExtension.GdalRasterProvider();
            }

            string GridDataFolder = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\..\..\..\Data\Grids\";

            DotSpatial.Data.IRaster sourceGrid = Raster.Open(GridDataFolder + @"elev_cm_ESRI\elev_cm_clip2\hdr.adf");
            Double sourceGridMaximum = sourceGrid.Maximum;

            string savedGridName = GridDataFolder + @"elev_cm.tif";
            sourceGrid.SaveAs(savedGridName);

            Assert.AreEqual(sourceGrid.Maximum, sourceGridMaximum, 0.0001);

            DotSpatial.Data.IRaster savedSourceGrid = Raster.Open(savedGridName);

            Assert.AreEqual(sourceGridMaximum, savedSourceGrid.Maximum, 0.0001);

            sourceGrid.Close();
            savedSourceGrid.Close();
            System.IO.File.Delete(savedGridName);
        }
    }
}