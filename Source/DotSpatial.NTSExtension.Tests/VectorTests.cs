// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using NUnit.Framework;

namespace DotSpatial.NTSExtension.Tests
{
    /// <summary>
    /// Tests for Vector.
    /// </summary>
    [TestFixture]
    public class VectorTests
    {
        /// <summary>
        /// Checks that valid values are parsed correctly.
        /// </summary>
        /// <param name="angle">The angle to test.</param>
        [Test]
        [TestCase(1.04719)]
        [TestCase(2.09439)]
        [TestCase(-1.04719)]
        [TestCase(-2.09439)]
        public void CheckThetaAngle(double angle)
        {
            var vector = new Vector(2.0, new Angle(angle));
            Assert.AreEqual(Math.Round(angle, 5), Math.Round(vector.Theta, 5));
        }
    }
}