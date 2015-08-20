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
    }
}
