using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class PointShapefileTests
    {
        [Test]
        public void CanLoadPointZWithoutM()
        {
            var path = FileTools.PathToTestFile(@"Shapefiles\shp-no-m\SPATIAL_F_LUFTNINGSVENTIL.shp");
            var target = new PointShapefile(path);
            Assert.AreEqual(CoordinateType.Z, target.CoordinateType);
            Assert.IsTrue(target.Count > 0);
            for (var i = 0; i < target.Count; i++)
            {
                var shape = target.GetShape(i, false);
                Assert.IsNotNull(shape.Z);
                Assert.IsNotNull(shape.M);
                Assert.IsTrue(shape.M[0] < -1e38);
            }
        }
    }
}
