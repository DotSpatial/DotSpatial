using NUnit.Framework;
using DotSpatial.NTSExtension;
using System;

namespace DotSpatial.NtsExtension.Tests
{
    /// <summary>
    /// Tests for the vector class.
    /// </summary>
    public class Tests
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