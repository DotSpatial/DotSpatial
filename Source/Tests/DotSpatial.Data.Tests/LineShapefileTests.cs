// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Linq;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for line shapefiles.
    /// </summary>
    [TestFixture]
    internal class LineShapefileTests
    {
        /// <summary>
        /// Tests that line shapefiles with null shapes can be read.
        /// </summary>
        [Test]
        public void CanReadLineShapeWithNullShapes()
        {
            string path = Common.AbsolutePath(@"Data\Shapefiles\Archi\ARCHI_13-01-01.shp");
            var target = new LineShapefile(path);
            Assert.IsNotNull(target);
            Assert.AreEqual(11, target.ShapeIndices.Count(d => d.ShapeType == ShapeType.NullShape));
            Assert.AreEqual(11, target.Features.Count(d => d.Geometry.IsEmpty));
        }

        /// <summary>
        /// Tests that line shapefiles with null shapes can be written to file.
        /// </summary>
        /// <param name="indexMode">Indicates whether writing uses IndexMode.</param>
        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void CanExportLineShapeWithNullShapes(bool indexMode)
        {
            string path = Common.AbsolutePath(@"Data\Shapefiles\Archi\ARCHI_13-01-01.shp");
            var target = new LineShapefile(path);
            Assert.IsTrue(target.Features.Count > 0);
            target.IndexMode = indexMode;

            var exportPath = FileTools.GetTempFileName(".shp");
            target.SaveAs(exportPath, true);

            try
            {
                var actual = new LineShapefile(exportPath);
                Assert.IsNotNull(actual);
                Assert.AreEqual(target.Extent, actual.Extent);
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
