using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    [TestFixture]
    class RangeTests
    {
        [Test]
        [TestCase("(-∞; +∞)", 1, true)]
        [TestCase("(-∞; 5]", 1, true)]
        [TestCase("(-∞; 5)", 1, true)]
        [TestCase("(-∞; 5]", 5, true)]
        [TestCase("(-∞; 5)", 5, false)]
        [TestCase("(-∞; 5]", 6, false)]
        [TestCase("(-∞; 5)", 6, false)]
        [TestCase("[5; 5]", 5, true)]
        public void Contains(string range, double value, bool expected)
        {
            // Parse
            var minmax = range.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                              .Select(s => s.Trim())
                              .ToArray();
            var minIsInclusive = minmax[0][0] == '[';
            var maxIsInclusive = minmax[1][minmax[1].Length - 1] == ']';
            var minStr = minmax[0].Substring(1);
            var maxStr = minmax[1].Substring(0, minmax[1].Length - 1);
            double? min = null, max = null;
            if (minStr != "-∞")
            {
                min = double.Parse(minStr, NumberStyles.Float, CultureInfo.InvariantCulture);
            }
            if (maxStr != "+∞")
            {
                max = double.Parse(maxStr, NumberStyles.Float, CultureInfo.InvariantCulture);
            }

            // Test
            var target = new Range(min, max) {MinIsInclusive = minIsInclusive, MaxIsInclusive = maxIsInclusive};
            var actual = target.Contains(value);
            Assert.AreEqual(expected, actual);
        }
    }
}
