// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for ShapeReader.
    /// </summary>
    [TestFixture]
    internal class ShapeReaderTests
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
            const string Path = @"Data\Shapefiles\shp-no-m\SPATIAL_F_LUFTNINGSVENTIL.shp";
            var source = new PointShapefileShapeSource(Path);

            var target = new ShapeReader(source)
            {
                PageSize = 10
            };
            int expectedStartIndex = 0;
            foreach (var page in target)
            {
                Assert.AreEqual(expectedStartIndex, page.Keys.Min(), "Shape reader skipped at least one entry.");
                expectedStartIndex = page.Keys.Max() + 1;
            }
        }
    }
}
