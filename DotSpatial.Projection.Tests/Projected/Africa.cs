using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the Africa category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class ProjectedAfrica
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
        public void AfricaAlbersEqualAreaConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Africa.AfricaAlbersEqualAreaConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AfricaEquidistantConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Africa.AfricaEquidistantConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AfricaLambertConformalConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Africa.AfricaLambertConformalConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AfricaSinusoidal()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Africa.AfricaSinusoidal;
            Tester.TestProjection(pStart);
        }
    }
}