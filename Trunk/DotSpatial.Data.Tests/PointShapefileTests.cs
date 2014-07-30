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
            for (var i = 0; i < target.ShapeIndices.Count; i++)
            {
                var shape = target.GetShape(i, false);
                Assert.IsNotNull(shape.Z);
                Assert.IsNotNull(shape.M);
                Assert.IsTrue(shape.M[0] < -1e38);
            }
        }

        [Test]
        public void CanLoadShapePointWithNullShapes()
        {
            const string path = @"Data\Shapefiles\Yield\Yield 2012.shp";
            var target = new PointShapefile(path);
            Assert.IsNotNull(target);

            Shape nullShape = null;
            for (var i = 0; i < target.ShapeIndices.Count; i++)
            {
                var shape = target.GetShape(i, false);
                if (shape.Range.ShapeType == ShapeType.NullShape)
                {
                    nullShape = shape;
                    break;
                }
            }
            Assert.IsNotNull(nullShape);
            Assert.AreEqual(0, nullShape.Vertices[0]);
            Assert.AreEqual(0, nullShape.Vertices[1]);
        }
    }
}
