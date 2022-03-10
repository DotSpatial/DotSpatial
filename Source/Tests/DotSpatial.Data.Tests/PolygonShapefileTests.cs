// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Tests.Common;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for polygon shapefiles.
    /// </summary>
    [TestFixture]
    internal class PolygonShapefileTests
    {
        #region Methods

        /// <summary>
        /// Checks whether large shapefiles can be read.
        /// </summary>
        [Test]
        public void CanReadShapefileWithManyByteBlocks()
        {
            string path = Common.AbsolutePath(@"Data\Shapefiles\nos80k\nos80k.shp");
            var target = new PolygonShapefile(path);
            Assert.IsNotNull(target);
            Assert.IsNotNull(target.ShapeIndices);
        }

        /// <summary>
        /// The file contains a MultiPolygon that consist of one polygon with a hole that contains
        /// another polygon with a hole. Each of these polygons should only contain one hole.
        /// </summary>
        [Test]
        public void EachHoleExistsOnlyOnce()
        {
            string filePath = Common.AbsolutePath(@"Data\Shapefiles\Multipolygon\PolygonInHole.shp"); // Replace with path to file
            using var file = new PolygonShapefile(filePath);
            Assert.AreEqual(1, file.Features.Count, "Expected the shapefile to have only 1 feature.");

            IFeature feature = file.GetFeature(0);
            Assert.AreEqual(2, feature.Geometry.NumGeometries, "Expect the feature to consist out of 2 geometries.");

            for (int i = 0; i < feature.Geometry.NumGeometries; i++)
            {
                var polygon = (Polygon)feature.Geometry.GetGeometryN(i);
                Assert.AreEqual(1, polygon.NumInteriorRings, "Expect each polygon part to have 1 hole.");
            }
        }

        #endregion

    }
}