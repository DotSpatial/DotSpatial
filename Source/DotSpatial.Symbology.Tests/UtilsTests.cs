using System.Globalization;
using System.Linq;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    [TestFixture]
    internal class UtilsTests
    {
        [Test]
        [TestCase(1.0, 1, 1.0)]
        [TestCase(0, 1, 0)]
        [TestCase(-1.0, 1, -1.0)]
        [TestCase(1.0, 10, 1.0)]
        [TestCase(1.23456, 2, 1.2)]
        [TestCase(1.23456, 3, 1.23)]
        [TestCase(1.23456, 10, 1.23456)]
        public void SigFig(double value, int numFixgures, double expected)
        {
            var actual = Utils.SigFig(value, numFixgures);
            AssertExt.AreEqual15(expected, actual);
        }

        [Test]
        [TestCase(12.5, 12)]
        [TestCase(256.5, 255)]
        [TestCase(-1, 0)]
        public void ByteRange(double value, int expected)
        {
            var actual = Utils.ByteRange(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("1,2,3,4,5", 3, 3)]
        [TestCase("1,2,3,4,5", 2.6, 3)]
        [TestCase("1,2,3,4,5", 10, 0)]
        public void GetNearestValue(string strValues, double value, double actual)
        {
            var values = strValues.Split(',').Select(_ => double.Parse(_, CultureInfo.InvariantCulture)).ToList();
            var expected = Utils.GetNearestValue(value, values);
            AssertExt.AreEqual15(expected, actual);
        }
    }
}
