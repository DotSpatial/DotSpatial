using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class MultiPointShapefileTests
    {
        [Test]
        [TestCase(@"Shapefiles\multipoint.shape\multipoint.shp")]
        public void CanOpen(string path)
        {
            path = FileTools.PathToTestFile(path);
            var target = new MultiPointShapefile(path);
            Assert.IsTrue(target.Count > 0);
            for (var i = 0; i < target.Count; i++)
            {
                var shape = target.GetShape(i, false);
                Assert.IsNotNull(shape);
            }
        }

        [Test]
        [TestCase(@"Shapefiles\multipoint.shape\multipoint.shp")]
        public void CanSave_IndexModeTrue(string path)
        {
            path = FileTools.PathToTestFile(path);
            var expected = new MultiPointShapefile(path);
            Assert.IsTrue(expected.IndexMode);
            var newFile = FileTools.GetTempFileName(".shp");
            expected.SaveAs(newFile, true);

            try
            {
                var actual = new MultiPointShapefile(path);
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
        [TestCase(@"Shapefiles\multipoint.shape\multipoint.shp")]
        public void CanSave_IndexModeFalse(string path)
        {
            path = FileTools.PathToTestFile(path);
            var expected = new MultiPointShapefile(path);
            var count = expected.Features.Count; // Force to load all features into memory
            Assert.AreEqual(count, expected.Count);
            Assert.IsTrue(!expected.IndexMode);
            var newFile = FileTools.GetTempFileName(".shp");
            expected.SaveAs(newFile, true);

            try
            {
                var actual = new MultiPointShapefile(path);
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