using System;
using System.Collections.Generic;
using System.Globalization;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    [TestFixture]
    class MeridianTests
    {
        /// <summary>
        /// Test for DefaultCtor       
        /// </summary>
        [Test]
        public void DefaultCtor()
        {
            var target = new Meridian();
            Assert.IsNotNull(target);
            Assert.AreEqual(Proj4Meridian.Greenwich.ToString(), target.Name);
        }

        [Test]
        [TestCaseSource("WellKnownMeridians")]
        public void CtorLongitudeName(Tuple<Proj4Meridian, double, int> meridian)
        {
            var target = new Meridian(meridian.Item2, meridian.Item1.ToString());
            Assert.IsNotNull(target);
            Assert.AreEqual(meridian.Item2, target.Longitude);
            Assert.AreEqual(meridian.Item1.ToString(), target.Name);
        }

        [Test]
        [TestCaseSource("WellKnownMeridians")]
        public void CtorProj4Meridian(Tuple<Proj4Meridian, double, int> meridian)
        {
            var target = new Meridian(meridian.Item1);
            Assert.IsNotNull(target);
            Assert.AreEqual(meridian.Item3, target.Code);
            Assert.AreEqual(meridian.Item2, target.Longitude);
            Assert.AreEqual(meridian.Item1.ToString(), target.Name);
        }

        [Test]
        [TestCaseSource("WellKnownMeridians")]
        public void CtorStandardMeridianName(Tuple<Proj4Meridian, double, int> meridian)
        {
            var target = new Meridian(meridian.Item1.ToString());
            Assert.IsNotNull(target);
            Assert.AreEqual(meridian.Item3, target.Code);
            Assert.AreEqual(meridian.Item2, target.Longitude);
            Assert.AreEqual(meridian.Item1.ToString(), target.Name);
        }

        [Test]
        [TestCaseSource("WellKnownMeridians")]
        public void ReadCode(Tuple<Proj4Meridian, double, int> meridian)
        {
            var target = new Meridian();
            target.ReadCode(meridian.Item3);
            Assert.AreEqual(meridian.Item3, target.Code);
            Assert.AreEqual(meridian.Item2, target.Longitude);
            Assert.AreEqual(meridian.Item1.ToString(), target.Name);
        }

        [Test]
        [TestCaseSource("WellKnownMeridians")]
        public void AssignMeridian(Tuple<Proj4Meridian, double, int> meridian)
        {
            var target = new Meridian();
            target.AssignMeridian(meridian.Item1);
            Assert.AreEqual(meridian.Item3, target.Code);
            Assert.AreEqual(meridian.Item2, target.Longitude);
            Assert.AreEqual(meridian.Item1.ToString(), target.Name);
        }

        [Test]
        [TestCaseSource("WellKnownMeridians")]
        public void PmStringValue(Tuple<Proj4Meridian, double, int> meridian)
        {
            var target = new Meridian {pm = meridian.Item1.ToString()};
            Assert.AreEqual(meridian.Item3, target.Code);
            Assert.AreEqual(meridian.Item2, target.Longitude);
            Assert.AreEqual(meridian.Item1.ToString(), target.Name);
        }


        [Test]
        [TestCaseSource("WellKnownMeridians")]
        public void PmDoubleValue(Tuple<Proj4Meridian, double, int> meridian)
        {
            var target = new Meridian { pm = meridian.Item2.ToString(CultureInfo.InvariantCulture) };
            Assert.AreEqual(meridian.Item3, target.Code);
            Assert.AreEqual(meridian.Item2, target.Longitude);
            Assert.AreEqual(meridian.Item1.ToString(), target.Name);
        }


#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
        private static IEnumerable<Tuple<Proj4Meridian, double, int>> WellKnownMeridians()
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen
        {
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Greenwich, 0, 8901);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Lisbon, -9.131906111, 8902);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Paris, 2.337229167, 8903);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Bogota, -74.08091667, 8904);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Madrid, -3.687938889, 8905);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Rome, 12.45233333, 8906);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Bern, 7.439583333, 8907);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Jakarta, 106.8077194, 8908);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Ferro, -17.66666667, 8909);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Brussels, 4.367975, 8910);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Stockholm, 18.05827778, 8911);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Athens, 23.7163375, 8912);
            yield return new Tuple<Proj4Meridian, double, int>(Proj4Meridian.Oslo, 10.72291667, 8913);
        }
    }
}
