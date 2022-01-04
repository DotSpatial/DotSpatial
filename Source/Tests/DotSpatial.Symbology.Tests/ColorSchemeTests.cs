// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// Tests for ColorScheme.
    /// </summary>
    [TestFixture]
    internal class ColorSchemeTests
    {
        /// <summary>
        /// Checks that ApplyScheme produces 2 categories if the min and max value are not the same.
        /// </summary>
        [Test]
        public void ApplySchemeProduce2CategoriesForNonEqualValues()
        {
            var target = new ColorScheme();
            target.ApplyScheme(ColorSchemeType.Desert, 0, 1);
            Assert.AreEqual(2, target.Categories.Count);
        }

        /// <summary>
        /// Checks that ApplyScheme produces only 1 category if the min and max value are the same.
        /// </summary>
        [Test]
        public void ApplySchemeProduce1CategoryForEqualValues()
        {
            var target = new ColorScheme();
            target.ApplyScheme(ColorSchemeType.Desert, 1, 1);
            Assert.AreEqual(1, target.Categories.Count);
        }
    }
}
