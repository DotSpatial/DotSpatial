// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Globalization;
using System.Linq;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// Tests for Utils.
    /// </summary>
    [TestFixture]
    internal class UtilsTests
    {
        /// <summary>
        /// Tests if SigFig produces the expected results.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="numFigures">The number of figures.</param>
        /// <param name="expected">The expected result.</param>
        [Test]
        [TestCase(1.0, 1, 1.0)]
        [TestCase(0, 1, 0)]
        [TestCase(-1.0, 1, -1.0)]
        [TestCase(1.0, 10, 1.0)]
        [TestCase(1.23456, 2, 1.2)]
        [TestCase(1.23456, 3, 1.23)]
        [TestCase(1.23456, 10, 1.23456)]
        public void SigFig(double value, int numFigures, double expected)
        {
            var actual = Utils.SigFig(value, numFigures);
            AssertExt.AreEqual15(expected, actual);
        }

        /// <summary>
        /// Tests whether ByteRange produces the expected result.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="expected">The expected result.</param>
        [Test]
        [TestCase(12.5, 12)]
        [TestCase(256.5, 255)]
        [TestCase(-1, 0)]
        public void ByteRange(double value, int expected)
        {
            var actual = Utils.ByteRange(value);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Tests whether GetNearestValue produces the expected result.
        /// </summary>
        /// <param name="strValues">The values to get the nearest value from.</param>
        /// <param name="value">The value for which the nearest value should be found.</param>
        /// <param name="expected">The expected result.</param>
        [Test]
        [TestCase("1,2,3,4,5", 3, 3)]
        [TestCase("1,2,3,4,5", 2.6, 3)]
        [TestCase("1,2,3,4,5", 10, 0)]
        public void GetNearestValue(string strValues, double value, double expected)
        {
            var values = strValues.Split(',').Select(_ => double.Parse(_, CultureInfo.InvariantCulture)).ToList();
            var actual = Utils.GetNearestValue(value, values);
            AssertExt.AreEqual15(actual, expected);
        }
    }
}
