
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the Europe category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class Europe
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
        [Ignore]
        public void EMEP150KilometerGrid()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Europe.EMEP150KilometerGrid;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void EMEP50KilometerGrid()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Europe.EMEP50KilometerGrid;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989LAEA()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Europe.ETRS1989LAEA;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989LCC()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Europe.ETRS1989LCC;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeAlbersEqualAreaConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Europe.EuropeAlbersEqualAreaConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeEquidistantConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Europe.EuropeEquidistantConic;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeLambertConformalConic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Europe.EuropeLambertConformalConic;
            Tester.TestProjection(pStart);
        }
    }
}