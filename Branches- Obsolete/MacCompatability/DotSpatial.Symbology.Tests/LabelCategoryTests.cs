using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    [TestFixture]
    class LabelCategoryTests
    {
        [Test]
        public void NotNullProps_WhenCtor()
        {
            var target = new LabelCategory();
            Assert.IsNotNull(target.SelectionSymbolizer);
            Assert.IsNotNull(target.Symbolizer);
            Assert.IsNotNull(target.ToString());
        }
    }
}
