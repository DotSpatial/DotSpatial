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
    }
}