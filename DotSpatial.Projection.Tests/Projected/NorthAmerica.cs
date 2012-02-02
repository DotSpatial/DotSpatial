
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the NorthAmerica category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class NorthAmerica
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            TestSetupHelper.CopyProj4();
        }

        [Test]
        public void AlaskaAlbersEqualAreaConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.AlaskaAlbersEqualAreaConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CanadaAlbersEqualAreaConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.CanadaAlbersEqualAreaConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CanadaLambertConformalConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.CanadaLambertConformalConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void HawaiiAlbersEqualAreaConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.HawaiiAlbersEqualAreaConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NorthAmericaAlbersEqualAreaConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.NorthAmericaAlbersEqualAreaConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NorthAmericaEquidistantConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.NorthAmericaEquidistantConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NorthAmericaLambertConformalConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.NorthAmericaLambertConformalConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void USAContiguousAlbersEqualAreaConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.USAContiguousAlbersEqualAreaConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void USAContiguousAlbersEqualAreaConicUSGS()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.USAContiguousAlbersEqualAreaConicUSGS;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void USAContiguousEquidistantConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.USAContiguousEquidistantConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void USAContiguousLambertConformalConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NorthAmerica.USAContiguousLambertConformalConic;
            Tester.TestProjection(pStart);
        }
    }
}