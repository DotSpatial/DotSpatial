using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class PointShapefileTests
    {
        [Test]
        public void CanReadPointZWithoutM()
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

        [Test]
        public void CanLoadShapePointWithNullShapes()
        {
            var path = FileTools.PathToTestFile(@"Shapefiles\Yield\Yield 2012.shp");
            var target = new PointShapefile(path);
            Assert.IsNotNull(target);
            Assert.IsTrue(target.Count > 0);

            Shape nullShape = null;
            for (var i = 0; i < target.Count; i++)
            {
                var shape = target.GetShape(i, false);
                if (shape.Range.ShapeType == ShapeType.NullShape)
                {
                    nullShape = shape;
                    break;
                }
            }
            Assert.IsNotNull(nullShape);
            Assert.IsNull(nullShape.Vertices);
        }

        [Test]
        [TestCase(@"Shapefiles\Cities\cities.shp")]
        public void CanOpen(string path)
        {
            path = FileTools.PathToTestFile(path);
            var target = new PointShapefile(path);
            Assert.IsTrue(target.Count > 0);
            for (var i = 0; i < target.Count; i++)
            {
                var shape = target.GetShape(i, false);
                Assert.IsNotNull(shape);
            }
        }

        [Test]
        [TestCase(@"Shapefiles\Cities\cities.shp")]
        public void CanUpdateExtent(string path)
        {
            path = FileTools.PathToTestFile(path);
            var target = new PointShapefile(path);
            Assert.IsTrue(target.Count > 0);
            target.Extent = null;
            target.UpdateExtent();
            Assert.IsNotNull(target.Extent);
        }

        [Test]
        [TestCase(@"Shapefiles\Cities\cities.shp")]
        public void CanSave_FeaturesInRamFalse(string path)
        {
            path = FileTools.PathToTestFile(path);
            var expected = new PointShapefile(path);
            Assert.IsTrue(expected.Count > 0);
            Assert.IsFalse(expected.FeaturesInRam);
            var newFile = FileTools.GetTempFileName(".shp");
            expected.SaveAs(newFile, true);

            try
            {
                var actual = new PointShapefile(path);
                Assert.AreEqual(expected.Count, actual.Count);
                for (var i = 0; i < expected.Count; i++)
                {
                    var targetF = expected[i];
                    var actualF = actual[i];

                    Assert.AreEqual(targetF.Coordinates, actualF.Coordinates);
                    Assert.AreEqual(targetF.DataRow.ItemArray, actualF.DataRow.ItemArray);
                }
            }
            finally
            {
                FileTools.DeleteShapeFile(newFile);
            }
        }

        [Test]
        [TestCase(@"Shapefiles\Cities\cities.shp")]
        public void CanSave_FeaturesInRamTrue(string path)
        {
            path = FileTools.PathToTestFile(path);
            var expected = new PointShapefile(path);
            Assert.IsTrue(expected.Count > 0);
            var count = expected.Features.Count; // Force to load all features into memory
            Assert.AreEqual(count, expected.Count);
            Assert.IsTrue(expected.FeaturesInRam);
            var newFile = FileTools.GetTempFileName(".shp");
            expected.SaveAs(newFile, true);

            try
            {
                var actual = new PointShapefile(path);
                Assert.AreEqual(expected.Count, actual.Count);
                for (var i = 0; i < expected.Count; i++)
                {
                    var targetF = expected[i];
                    var actualF = actual[i];

                    Assert.AreEqual(targetF.Coordinates, actualF.Coordinates);
                    Assert.AreEqual(targetF.DataRow.ItemArray, actualF.DataRow.ItemArray);
                }
            }
            finally
            {
                FileTools.DeleteShapeFile(newFile);
            }
        }
    }
}
