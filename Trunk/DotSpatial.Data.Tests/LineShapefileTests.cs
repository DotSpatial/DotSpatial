using System.IO;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class LineShapefileTests
    {
        [Test]
        public void CanReadLineShapeWithNullShapes()
        {
            const string path = @"Data\Shapefiles\Archi\ARCHI_13-01-01.shp";
            var target = new LineShapefile(path);
            Assert.IsNotNull(target);
            Assert.IsTrue(target.ShapeIndices.Any(d => d.ShapeType == ShapeType.NullShape));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void CanExportLineShapeWithNullShapes(bool indexMode)
        {
            const string path = @"Data\Shapefiles\Archi\ARCHI_13-01-01.shp";
            var target = new LineShapefile(path);
            Assert.IsTrue(target.Features.Count > 0);
            target.IndexMode = indexMode;

            var exportPath = Path.ChangeExtension(Path.GetTempFileName(), ".shp");
            target.SaveAs(exportPath, true);

            try
            {
                var actual = new LineShapefile(exportPath);
                Assert.IsNotNull(actual);
                Assert.AreEqual(target.ShapeIndices.Count, actual.ShapeIndices.Count);
                if (indexMode)
                {
                    Assert.AreEqual(target.ShapeIndices.Count(d => d.ShapeType == ShapeType.NullShape), actual.ShapeIndices.Count(d => d.ShapeType == ShapeType.NullShape));
                }
                Assert.AreEqual(target.Features.Count, actual.Features.Count);
            }
            finally
            {
                File.Delete(exportPath);
                File.Delete(Path.ChangeExtension(exportPath, ".dbf"));
                File.Delete(Path.ChangeExtension(exportPath, ".shx"));
                File.Delete(Path.ChangeExtension(exportPath, ".prj"));
            }
        }
    }
}
