using System.Linq;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class PointShapefileTests
    {
        /// <summary>
        /// Checks whether point shapefiles that have a z but no m value can be loaded.
        /// </summary>
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

        /// <summary>
        /// Checks whether point shapefiles that contain NullShapes can be loaded without loosing the NullShapes.
        /// </summary>
        [Test]
        public void CanLoadShapePointWithNullShapes()
        {
            const string path = @"Data\Shapefiles\Yield\Yield 2012.shp";
            var target = new PointShapefile(path);
            Assert.IsNotNull(target);
            Assert.AreEqual(target.ShapeIndices.Count(d => d.ShapeType == ShapeType.NullShape), 1792);
            Assert.AreEqual(target.Features.Count(d => d.Geometry.IsEmpty), 1792);
        }

        /// <summary>
        /// Checks whether point shapefiles that contain NullShapes can be exported without loosing the NullShapes.
        /// </summary>
        /// <param name="indexMode"></param>
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void CanExportPointShapeWithNullShapes(bool indexMode)
        {
            const string path = @"Data\Shapefiles\Yield\Yield 2012.shp";
            var target = new PointShapefile(path);
            Assert.IsTrue(target.Features.Count > 0);
            target.IndexMode = indexMode;

            var exportPath = FileTools.GetTempFileName(".shp");
            target.SaveAs(exportPath, true);

            try
            {
                var actual = new PointShapefile(exportPath);
                Assert.IsNotNull(actual);
                Assert.AreEqual(target.ShapeIndices.Count, actual.ShapeIndices.Count);
                Assert.AreEqual(target.ShapeIndices.Count(d => d.ShapeType == ShapeType.NullShape), actual.ShapeIndices.Count(d => d.ShapeType == ShapeType.NullShape));
                Assert.AreEqual(target.Features.Count, actual.Features.Count);
                Assert.AreEqual(target.Features.Count(d => d.Geometry.IsEmpty), actual.Features.Count(d => d.Geometry.IsEmpty));
            }
            finally
            {
                FileTools.DeleteShapeFile(exportPath);
            }
        }

    }
}
