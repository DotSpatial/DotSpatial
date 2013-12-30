using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the Asia category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class Asia
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void AsiaLambertConformalConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Asia.AsiaLambertConformalConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AsiaNorthAlbersEqualAreaConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Asia.AsiaNorthAlbersEqualAreaConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AsiaNorthEquidistantConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Asia.AsiaNorthEquidistantConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AsiaNorthLambertConformalConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Asia.AsiaNorthLambertConformalConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AsiaSouthAlbersEqualAreaConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Asia.AsiaSouthAlbersEqualAreaConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AsiaSouthEquidistantConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Asia.AsiaSouthEquidistantConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AsiaSouthLambertConformalConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Asia.AsiaSouthLambertConformalConic;
            Tester.TestProjection(pStart);
        }
    }
}