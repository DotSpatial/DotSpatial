using System;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    [TestFixture]
    class LabelSymbolizerTests
    {
        [Test]
        public void Ctor_ValidProperties()
        {
            var target = new LabelSymbolizer();
            Assert.IsTrue(ValidEnumValue(target.Alignment));
            Assert.IsTrue(ValidEnumValue(target.FontStyle));
            Assert.IsTrue(ValidEnumValue(target.LabelPlacementMethod));
            Assert.IsTrue(ValidEnumValue(target.Orientation));
            Assert.IsTrue(ValidEnumValue(target.PartsLabelingMethod));
            Assert.IsTrue(ValidEnumValue(target.ScaleMode));
        }

        private static bool ValidEnumValue<T>(T value)
        {
            return ((T[])Enum.GetValues(typeof(T))).Contains(value);
        }
    }
}
