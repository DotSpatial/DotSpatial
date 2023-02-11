using NUnit.Framework;

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
        [SetUp]
        public void Initialize()
        {
            
        }


        /// <summary>
        /// Test for NorthPoleAzimuthalEquidistant       
        /// </summary>
        [Test, Category("Projection")]
        public void NorthPoleAzimuthalEquidistant()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.NorthPoleAzimuthalEquidistant;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NorthPoleGnomonic       
        /// </summary>
        [Test, Category("Projection")]
        public void NorthPoleGnomonic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.NorthPoleGnomonic;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NorthPoleLambertAzimuthalEqualArea       
        /// </summary>
        [Test, Category("Projection")]
        public void NorthPoleLambertAzimuthalEqualArea()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.NorthPoleLambertAzimuthalEqualArea;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NorthPoleOrthographic       
        /// </summary>
        [Test, Category("Projection")]
        public void NorthPoleOrthographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.NorthPoleOrthographic;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NorthPoleStereographic       
        /// </summary>
        [Test, Category("Projection")]
        public void NorthPoleStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.NorthPoleStereographic;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Perroud1950TerreAdeliePolarStereographic       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("Verify")]
        public void Perroud1950TerreAdeliePolarStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.Perroud1950TerreAdeliePolarStereographic;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Petrels1972TerreAdeliePolarStereographic       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("Verify")]
        public void Petrels1972TerreAdeliePolarStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.Petrels1972TerreAdeliePolarStereographic;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for SouthPoleAzimuthalEquidistant       
        /// </summary>
        [Test, Category("Projection")]
        public void SouthPoleAzimuthalEquidistant()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.SouthPoleAzimuthalEquidistant;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for SouthPoleGnomonic       
        /// </summary>
        [Test, Category("Projection")]
        public void SouthPoleGnomonic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.SouthPoleGnomonic;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for SouthPoleLambertAzimuthalEqualArea       
        /// </summary>
        [Test, Category("Projection")]
        public void SouthPoleLambertAzimuthalEqualArea()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.SouthPoleLambertAzimuthalEqualArea;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for SouthPoleOrthographic       
        /// </summary>
        [Test, Category("Projection")]
        public void SouthPoleOrthographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.SouthPoleOrthographic;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for SouthPoleStereographic       
        /// </summary>
        [Test, Category("Projection")]
        public void SouthPoleStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.SouthPoleStereographic;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for UPSNorth       
        /// </summary>
        [Test, Category("Projection")]
        public void UPSNorth()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.UPSNorth;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for UPSSouth       
        /// </summary>
        [Test, Category("Projection")]
        public void UPSSouth()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.UPSSouth;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for WGS1984AntarcticPolarStereographic       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("Verify")]
        public void WGS1984AntarcticPolarStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.WGS1984AntarcticPolarStereographic;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for WGS1984AustralianAntarcticLambert       
        /// </summary>
        [Test, Category("Projection")]
        public void WGS1984AustralianAntarcticLambert()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.WGS1984AustralianAntarcticLambert;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for WGS1984AustralianAntarcticPolarStereographic       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("Verify")]
        public void WGS1984AustralianAntarcticPolarStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Polar.WGS1984AustralianAntarcticPolarStereographic;
            Tester.TestProjection(pStart);
        }
    }
}