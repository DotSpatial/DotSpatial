// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// Tests for Range.
    /// </summary>
    [TestFixture]
    internal class RangeTests
    {
        #region Methods

        /// <summary>
        /// Tests whether the given range contains the given values.
        /// </summary>
        /// <param name="range">Range that gets checked.</param>
        /// <param name="value">Value that gets checked.</param>
        /// <param name="expected">Indicates whether the value should be inside the range.</param>
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
            var minmax = range.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
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
            var target = new Range(min, max)
            {
                MinIsInclusive = minIsInclusive,
                MaxIsInclusive = maxIsInclusive
            };
            var actual = target.Contains(value);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}