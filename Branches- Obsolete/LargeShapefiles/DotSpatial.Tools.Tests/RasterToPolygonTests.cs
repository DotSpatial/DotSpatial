using System;
using System.Linq;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Tests.Common;
using DotSpatial.Topology;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    [TestFixture]
    class RasterToPolygonTests
    {
        [Test]
        [TestCase(@"Rasters\DEM_w.tif")]
        [TestCase(@"Rasters\DanSite1w.tif")]
        [TestCase(@"Rasters\DanSite2w.tif")]
        [TestCase(@"Rasters\c1w.tif")]
        public void CanCreateMultiPartPolygons(string file)
        {
            file = FileTools.PathToTestFile(file);
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
        [TestCase(@"Rasters\DEM_w.tif", @"Rasters\DEM_p.tif")]
        [TestCase(@"Rasters\c1w.tif", @"Rasters\c1p.tif")]
        public void NoMultiPartPolygonsWithConnectionGrid(string rasterFile, string flowDirectionGridFile)
        {
            rasterFile = FileTools.PathToTestFile(rasterFile);
            flowDirectionGridFile = FileTools.PathToTestFile(flowDirectionGridFile);

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
