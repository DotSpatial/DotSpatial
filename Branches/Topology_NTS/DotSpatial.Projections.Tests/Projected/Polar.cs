using System.IO;
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the Polar category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class Polar
    {

        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }


        [Test]
        public void NorthPoleAzimuthalEquidistant()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.NorthPoleAzimuthalEquidistant;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NorthPoleGnomonic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.NorthPoleGnomonic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NorthPoleLambertAzimuthalEqualArea()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.NorthPoleLambertAzimuthalEqualArea;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NorthPoleOrthographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.NorthPoleOrthographic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NorthPoleStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.NorthPoleStereographic;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void Perroud1950TerreAdeliePolarStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.Perroud1950TerreAdeliePolarStereographic;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void Petrels1972TerreAdeliePolarStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.Petrels1972TerreAdeliePolarStereographic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthPoleAzimuthalEquidistant()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.SouthPoleAzimuthalEquidistant;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthPoleGnomonic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.SouthPoleGnomonic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthPoleLambertAzimuthalEqualArea()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.SouthPoleLambertAzimuthalEqualArea;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthPoleOrthographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.SouthPoleOrthographic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthPoleStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.SouthPoleStereographic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void UPSNorth()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.UPSNorth;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void UPSSouth()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.UPSSouth;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void WGS1984AntarcticPolarStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.WGS1984AntarcticPolarStereographic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void WGS1984AustralianAntarcticLambert()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.WGS1984AustralianAntarcticLambert;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void WGS1984AustralianAntarcticPolarStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.WGS1984AustralianAntarcticPolarStereographic;
            Tester.TestProjection(pStart);
        }
    }
}