using System.Linq;
using DotSpatial.Data;
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    [TestFixture]
    class RasterToPolygonTests
    {
        [Test]
        [TestCase(@"Data\DEM_w.tif")]
        [TestCase(@"Data\DanSite1w.tif")]
        [TestCase(@"Data\DanSite2w.tif")]
        [TestCase(@"Data\c1w.tif")]
        public void CanCreateMultiPartPolygons(string file)
        {
            var target = new RasterToPolygon();
            var p = new GdalRasterProvider();
            var raster = p.Open(file);
            var outShape = new PolygonShapefile {Filename = FileTools.GetTempFileName(".shp")};
            target.Execute(raster, outShape, new MockProgressHandler());
            FileTools.DeleteShapeFile(outShape.Filename);

            var mpCount = outShape.Features.Count(t => t.BasicGeometry is MultiPolygon);
            Assert.That(mpCount > 0);
        }

        [Test]
        [TestCase(@"Data\DEM_w.tif", @"Data\DEM_p.tif")]
        [TestCase(@"Data\c1w.tif", @"Data\c1p.tif")]
        public void NoMultiPartPolygonsWithConnectionGrid(string rasterFile, string flowDirectionGridFile)
        {
            var p = new GdalRasterProvider();
            var raster = p.Open(rasterFile);
            var flowDirectionGrid = p.Open(flowDirectionGridFile);

            var target = new RasterToPolygon();
            var outShape = new PolygonShapefile { Filename = FileTools.GetTempFileName(".shp") };
            target.Execute(raster, flowDirectionGrid, outShape, new MockProgressHandler());
            FileTools.DeleteShapeFile(outShape.Filename);

            var mpCount = outShape.Features.Count(t => t.BasicGeometry is MultiPolygon);
            Assert.That(mpCount == 0);
        }
    }

    class MockProgressHandler : ICancelProgressHandler
    {
        public void Progress(string key, int percent, string message)
        {
            //nothing
        }

        public bool Cancel { get { return false; } }
    }
}
