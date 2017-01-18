using GeoAPI.Geometries;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class ShapeTests
    {
        [Test]
        [TestCase(1, 1, double.NaN, double.NaN)]
        [TestCase(1, 1, 1, double.NaN)]
        [TestCase(1, 1, 1, 1)]
        public void CanCreateShapeFromCoordinate(double x, double y, double z, double m)
        {
            var c = new Coordinate(x, y, z, m);
            var shape = new Shape(c);
            Assert.IsNotNull(shape);
        }

        [Test]
        public void CanConvertFeatureToShapeWhenDataRowIsMissing()
        {
            var feature = new Feature(FeatureType.Polygon, new Coordinate[]
            {
                new Coordinate(1375930, 6269230),
                new Coordinate(1376248, 6269230),
                new Coordinate(1376248, 6268860),
                new Coordinate(1375930, 6268860),
                new Coordinate(1375930, 6269230)
            });
            var shape = feature.ToShape();
            Assert.IsNotNull(shape);
        }
    }
}
