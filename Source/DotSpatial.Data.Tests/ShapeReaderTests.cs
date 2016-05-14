using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class ShapeReaderTests
    {
        /// <summary>
        /// This test is used to verify if the shape reader properly reads all entries in a shape file or if it skips any.
        /// Specifically this is used to verify that moving from one page to the next works without skipping any data sets.
        /// </summary>
        /// <remarks>
        /// Issue: https://dotspatial.codeplex.com/workitem/63623
        /// </remarks>
        [Test]
        public void ShapeReaderSkippingTest()
        {
            const string path = @"Data\Shapefiles\shp-no-m\SPATIAL_F_LUFTNINGSVENTIL.shp";
            var source = new PointShapefileShapeSource(path);

            var target = new ShapeReader(source);
            target.PageSize = 10;
            int expectedStartIndex = 0;
            foreach (var page in target)
            {
                Assert.AreEqual(expectedStartIndex, page.Keys.Min(), "Shape reader skipped at least one entry.");
                expectedStartIndex = page.Keys.Max() + 1;
            }
        }
    }
}
