using GeoAPI.Geometries;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class PolygonShapefileTests
    {
        [Test]
        public void CanReadShapefileWithManyByteBlocks()
        {
            const string path = @"Data\Shapefiles\nos80k\nos80k.shp";
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
            string filePath = @"Data\Shapefiles\Multipolygon\PolygonInHole.shp"; // Replace with path to file
            using (var file = new PolygonShapefile(filePath))
            {
                Assert.AreEqual(file.Features.Count, 1, "Expected the shapefile to have only 1 feature.");

                IFeature feature = file.GetFeature(0);
                Assert.AreEqual(feature.Geometry.NumGeometries, 2, "Expect the feature to consist out of 2 geometries.");

                for (int i = 0; i < feature.Geometry.NumGeometries; i++)
                {
                    var polygon = (IPolygon)feature.Geometry.GetGeometryN(i);
                    Assert.AreEqual(polygon.NumInteriorRings, 1, "Expect each polygon part to have 1 hole.");
                }
            }
        }
    }
}