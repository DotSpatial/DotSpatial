using System;
using System.Linq;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Topology;
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
        public void CanCreateMultiPartPolygons(string path)
        {
            var target = new RasterToPolygon();
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            
            var p = new GdalRasterProvider();
            var raster = p.Open(file);
            var outShape = new PolygonShapefile {Filename = Path.ChangeExtension(file, ".shp")};
            target.Execute(raster, outShape, new MockProgressHandler());

            var mpCount = outShape.Features.Count(t => t.BasicGeometry is MultiPolygon);
            Assert.That(mpCount > 0);
        }

        [Test]
        [TestCase(@"Data\DEM_w.tif", @"Data\DEM_p.tif")]
        [TestCase(@"Data\c1w.tif", @"Data\c1p.tif")]
        public void NoMultiPartPolygonsWithConnectionGrid(string rasterFile, string flowDirectionGridFile)
        {
            rasterFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rasterFile);
            flowDirectionGridFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, flowDirectionGridFile);

            var p = new GdalRasterProvider();
            var raster = p.Open(rasterFile);
            var flowDirectionGrid = p.Open(flowDirectionGridFile);

            var target = new RasterToPolygon();
            var outShape = new PolygonShapefile { Filename = Path.ChangeExtension(rasterFile, ".shp") };
            target.Execute(raster, flowDirectionGrid, outShape, new MockProgressHandler());

            var mpCount = outShape.Features.Count(t => t.BasicGeometry is MultiPolygon);
            Assert.That(mpCount == 0);
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
}
