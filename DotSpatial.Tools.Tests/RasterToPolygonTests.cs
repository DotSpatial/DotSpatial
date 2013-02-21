using System;
using System.Linq;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Topology;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotSpatial.Tools.Tests
{
    [TestClass]
    public class RasterToPolygonTests
    {
        [TestMethod]
        public void CanCreateMultiPartPolygons()
        {
            var target = new RasterToPolygon();
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\DEM_w.tif");
            
            var p = new GdalRasterProvider();
            var raster = p.Open(file);
            var outShape = new PolygonShapefile {Filename = Path.ChangeExtension(file, ".shp")};
            target.Execute(raster, outShape, new MockProgressHandler());

            var mpvalues = new[] {93, 61, 39, 95, 60, 75};
            foreach (var i in mpvalues)
            {
                var feature = outShape.Features.First(f => (double)f.DataRow["Value"] == i);
                Assert.IsTrue(feature.BasicGeometry is MultiPolygon);
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
}
