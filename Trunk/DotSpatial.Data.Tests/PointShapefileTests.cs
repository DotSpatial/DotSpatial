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
            var path = FileTools.PathToTestFile(@"Shapefiles\Yield\Yield 2012.shp");
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
        public void CanSave_IndexModeTrue(string path)
        {
            path = FileTools.PathToTestFile(path);
            var expected = new PointShapefile(path);
            Assert.IsTrue(expected.IndexMode);
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
        [TestCase(@"Shapefiles\cities.shp")]
        public void CanSave_IndexModeFalse(string path)
        {
            path = FileTools.PathToTestFile(path);
            var expected = new PointShapefile(path);
            var count = expected.Features.Count; // Force to load all features into memory
            Assert.AreEqual(count, expected.Count);
            Assert.IsTrue(!expected.IndexMode);
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
