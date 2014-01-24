using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class PointShapefileTests
    {
        [Test]
        public void CanReadPointZWithoutM()
        {
            const string path = @"Data\Shapefiles\shp-no-m\SPATIAL_F_LUFTNINGSVENTIL.shp";
            var target = new PointShapefile(path);
            Assert.AreEqual(CoordinateType.Z, target.CoordinateType);
            Assert.IsNotNull(target.Z);
            Assert.IsNotNull(target.M);
            Assert.IsTrue(target.M.All(d => d < -1e38));
        }

        [Test]
        public void CanLoadShapePointWithNullShapes()
        {
            const string path = @"Data\Shapefiles\Yield\Yield 2012.shp";
            var target = new PointShapefile(path);
            Assert.IsNotNull(target);
        }
    }
}
