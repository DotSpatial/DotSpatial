// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using NUnit.Framework;

namespace DotSpatial.Positioning.Tests
{
    /// <summary>
    /// Tests for Distance.
    /// </summary>
    [TestFixture]
    public class DistanceTests
    {
        /// <summary>
        /// Checks that valid values are parsed correctly.
        /// </summary>
        /// <param name="input">The input value.</param>
        /// <param name="value">The expected parsed value.</param>
        /// <param name="unit">The expected parsed unit.</param>
        [Test]
        [TestCase("50 m", 50, DistanceUnit.Meters)]
        [TestCase("-5m", -5, DistanceUnit.Meters)]
        public void ParseValidValues(string input, double value, DistanceUnit unit)
        {
            var actual = Distance.Parse(input);
            Assert.AreEqual(value, actual.Value);
            Assert.AreEqual(unit, actual.Units);
        }

        /// <summary>
        /// Checks that invalid values throw an ArgumentException.
        /// </summary>
        /// <param name="input">The input value.</param>
        [Test]
        [TestCase("3 this should not be valid")]
        [TestCase("this should not be valid")]
        public void ParseInvalidValues(string input)
        {
            Assert.Throws<ArgumentException>(() => Distance.Parse(input));
        }
    }
}
