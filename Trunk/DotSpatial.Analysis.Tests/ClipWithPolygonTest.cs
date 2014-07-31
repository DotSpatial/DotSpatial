using DotSpatial.Data;
using System.IO;
using DotSpatial.Tests.Common;
using NUnit.Framework;


namespace DotSpatial.Analysis.Tests
{
    /// <summary>
    ///This is a test class for ClipWithPolygonTest and is intended
    ///to contain all ClipWithPolygonTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ClipWithPolygonTest
    {
        /// <summary>
        ///A test for ClipRasterWithPolygon
        ///</summary>
        [Test]
        public void ClipRasterWithPolygonTest()
        {
            var shapeFilePath = FileTools.PathToTestFile(@"Shapefiles\elbe_watershed1\elbe_watershed1.shp");
            var rasterFilePath = FileTools.PathToTestFile(@"Rasters\kriging.bgd");
            var resultFilePath = FileTools.GetTempFileName(".bgd");

            try
            {
                var lClipPolygon = Shapefile.OpenFile(shapeFilePath);
                var lGridToClip = Raster.OpenFile(rasterFilePath, false);

                var lGridAfterClip = new Raster { Filename = resultFilePath };
                ClipRaster.ClipRasterWithPolygon(lClipPolygon.Features[0], lGridToClip, lGridAfterClip.Filename);
                var ras2 = Raster.Open(lGridAfterClip.Filename);
                Assert.AreEqual(lGridToClip.NoDataValue, ras2.NoDataValue);
            }
            finally
            {
                File.Delete(resultFilePath);
            }
        }
    }
}
