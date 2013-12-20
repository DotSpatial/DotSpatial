using System;
using DotSpatial.Data;
using System.IO;
using DotSpatial.Data.Rasters.GdalExtension;
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
        /// <summary>
        ///A test for ClipRasterWithPolygon
        ///</summary>
        [TestMethod()]
        public void ClipRasterWithPolygonTest()
        {

            if (DataManager.DefaultDataManager.PreferredProviders.Count == 0)
            {
                //GdalRasterProvider lGdalRasterProvider = new GdalRasterProvider();
            }

            String path = ".";
            String shapeFilePath = Path.Combine(path, "Data", "elbe_watershed1.shp");
            String rasterFilePath = Path.Combine(path, "Data", "kriging.bgd" );
            String resultFilePath = Path.Combine(path, "Data", "clipResult.bgd" );

            Shapefile lClipPolygon = Shapefile.OpenFile(shapeFilePath);
            IRaster lGridToClip = Raster.OpenFile(rasterFilePath, false);

            Raster lGridAfterClip = new Raster();
            lGridAfterClip.Filename = resultFilePath;

            ClipRaster.ClipRasterWithPolygon(lClipPolygon.Features[0], lGridToClip, lGridAfterClip.Filename);

            IRaster ras2 = Raster.Open(lGridAfterClip.Filename);

            Assert.AreEqual(lGridToClip.NoDataValue, ras2.NoDataValue);
        }
    }
}
