using System;
using DotSpatial.Data;
using System.IO;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;


namespace DotSpatial.Analysis.Tests
{
    /// <summary>
    ///This is a test class for ClipWithPolygonTest and is intended
    ///to contain all ClipWithPolygonTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClipWithPolygonTest
    {


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
        #endregion


        /// <summary>
        ///A test for ClipRasterWithPolygon
        ///</summary>
        [TestMethod()]
        public void ClipRasterWithPolygonTest()
        {

            if (DotSpatial.Data.DataManager.DefaultDataManager.PreferredProviders.Count == 0)
            {
                DotSpatial.Data.Rasters.GdalExtension.GdalRasterProvider lGdalRasterProvider = new DotSpatial.Data.Rasters.GdalExtension.GdalRasterProvider();
            }

            String path = AppDomain.CurrentDomain.BaseDirectory;
            String shapeFilePath = System.IO.Path.Combine(path, "Data", "elbe_watershed1.shp");
            String rasterFilePath = System.IO.Path.Combine(path, "Data", "kriging.bgd" );
            String resultFilePath = System.IO.Path.Combine(path, "Data", "clipResult.bgd" );

            DotSpatial.Data.Shapefile lClipPolygon = DotSpatial.Data.Shapefile.OpenFile(shapeFilePath);
            DotSpatial.Data.IRaster lGridToClip = DotSpatial.Data.Raster.OpenFile(rasterFilePath, false);

            DotSpatial.Data.Raster lGridAfterClip = new DotSpatial.Data.Raster();
            lGridAfterClip.Filename = resultFilePath;

            DotSpatial.Analysis.ClipRaster.ClipRasterWithPolygon(lClipPolygon.Features[0], lGridToClip, lGridAfterClip.Filename);

            IRaster ras2 = Raster.Open(lGridAfterClip.Filename);

            Assert.AreEqual(lGridToClip.NoDataValue, ras2.NoDataValue);
        }
    }
}
