using System;
using NUnit.Framework;

namespace DotSpatial.Positioning.Tests
{
    [TestFixture]
    public class DistanceTests
    {
        [Test]
        [TestCase("50 m", 50, DistanceUnit.Meters)]
        public void Parse_ValidValues(string input, double value, DistanceUnit unit)
        {
            var actual = Distance.Parse(input);
            Assert.AreEqual(value, actual.Value);
            Assert.AreEqual(unit, actual.Units);
        }

        [Test]
        [TestCase("3 this should not be valid")]
        [TestCase("this should not be valid")]
        public void Parse_InvalidValues(string input)
        {
            Assert.Throws<ArgumentException>(() => Distance.Parse(input));
        }
    }
}
