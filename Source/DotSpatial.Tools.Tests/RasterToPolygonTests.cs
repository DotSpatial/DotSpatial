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
        /// <summary>
        /// Tests that the tool can create multipart polygons from the given raster.
        /// </summary>
        /// <param name="file">Raster that gets converted.</param>
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
            var outShape = new PolygonShapefile { Filename = FileTools.GetTempFileName(".shp") };
            target.Execute(raster, outShape, new MockProgressHandler());
            FileTools.DeleteShapeFile(outShape.Filename);

            var mpCount = outShape.Features.Count(t => t.Geometry is MultiPolygon);
            Assert.That(mpCount > 0);
        }

        /// <summary>
        /// Checks that the tool doesn't create multipart polygons  when a connection grid is used.
        /// </summary>
        /// <param name="rasterFile">The raster file.</param>
        /// <param name="flowDirectionGridFile">The connection grid file.</param>
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

            var mpCount = outShape.Features.Count(t => t.Geometry is MultiPolygon);
            Assert.That(mpCount == 0);
        }
    }
}
