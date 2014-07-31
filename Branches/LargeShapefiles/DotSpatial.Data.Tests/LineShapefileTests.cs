using System.IO;
using System.Linq;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class LineShapefileTests
    {
        [Test]
        public void CanReadLineShapeWithNullShapes()
        {
            var path = FileTools.PathToTestFile(@"Shapefiles\Archi\ARCHI_13-01-01.shp");
            var target = new LineShapefile(path);
            Assert.IsNotNull(target);
            Assert.IsTrue(target.ShapeIndices.Any(d => d.ShapeType == ShapeType.NullShape));
        }

        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void CanExportLineShapeWithNullShapes(bool indexMode)
        {
            var path = FileTools.PathToTestFile(@"Shapefiles\Archi\ARCHI_13-01-01.shp");
            var target = new LineShapefile(path);
            Assert.IsTrue(target.Features.Count > 0);
            target.IndexMode = indexMode;

            var exportPath = FileTools.GetTempFileName(".shp");
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
                FileTools.DeleteShapeFile(exportPath);
            }
        }
    }
}
