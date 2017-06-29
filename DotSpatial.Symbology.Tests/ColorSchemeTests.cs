using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    [TestFixture]
    class ColorSchemeTests
    {
        [Test]
        public void ApplyScheme_Produce2CategoriesForNonEqualValues()
        {
            var target = new ColorScheme();
            target.ApplyScheme(ColorSchemeType.Desert, 0, 1);
            Assert.AreEqual(2, target.Categories.Count);
        }

        [Test]
        public void ApplyScheme_Produce1CategoryForEqualValues()
        {
            var target = new ColorScheme();
            target.ApplyScheme(ColorSchemeType.Desert, 1, 1);
            Assert.AreEqual(1, target.Categories.Count);
        }
    }
}
