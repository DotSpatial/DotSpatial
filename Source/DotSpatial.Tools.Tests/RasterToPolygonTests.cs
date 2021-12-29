// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Linq;
using DotSpatial.Data;
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Tests.Common;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    /// <summary>
    /// Tests for the raster to polygon tool.
    /// </summary>
    [TestFixture]
    internal class RasterToPolygonTests
    {
        private readonly string _folder = Common.AbsolutePath(@"Data");

        /// <summary>
        /// Tests that the tool can create multipart polygons from the given raster.
        /// </summary>
        /// <param name="pFile">Raster that gets converted.</param>
        [Test]
        [TestCase(@"DEM_w.tif")]
        [TestCase(@"DanSite1w.tif")]
        [TestCase(@"DanSite2w.tif")]
        [TestCase(@"c1w.tif")]
        public void CanCreateMultiPartPolygons(string pFile)
        {
            var file = System.IO.Path.Combine(_folder, pFile);
            var target = new RasterToPolygon();
            var p = new GdalRasterProvider();
            var raster = p.Open(file);
            var outShape = new PolygonShapefile { Filename = FileTools.GetTempFileName(".shp") };
            target.Execute(raster, outShape, new MockProgressHandler());
            FileTools.DeleteShapeFile(outShape.Filename);

            var mpCount = outShape.Features.Count(t => t.Geometry is MultiPolygon);
            Assert.That(mpCount > 0);
        }

        /// <summary>
        /// Checks that the tool doesn't create multipart polygons  when a connection grid is used.
        /// </summary>
        /// <param name="pRasterFile">The raster file.</param>
        /// <param name="pFlowDirectionGridFile">The connection grid file.</param>
        [Test]
        [TestCase(@"DEM_w.tif", @"DEM_p.tif")]
        [TestCase(@"c1w.tif", @"c1p.tif")]
        public void NoMultiPartPolygonsWithConnectionGrid(string pRasterFile, string pFlowDirectionGridFile)
        {
            var rasterFile = System.IO.Path.Combine(_folder, pRasterFile);
            var flowDirectionGridFile = System.IO.Path.Combine(_folder, pFlowDirectionGridFile);

            var p = new GdalRasterProvider();
            var raster = p.Open(rasterFile);
            var flowDirectionGrid = p.Open(flowDirectionGridFile);

            var target = new RasterToPolygon();
            var outShape = new PolygonShapefile { Filename = FileTools.GetTempFileName(".shp") };
            target.Execute(raster, flowDirectionGrid, outShape, new MockProgressHandler());
            FileTools.DeleteShapeFile(outShape.Filename);

            var mpCount = outShape.Features.Count(t => t.Geometry is MultiPolygon);
            Assert.That(mpCount == 0);
        }
    }
}
