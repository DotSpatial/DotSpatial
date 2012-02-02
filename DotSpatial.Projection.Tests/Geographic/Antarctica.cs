using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the Antarctica category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class Antarctica
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
        public void AustralianAntarctic1998()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Antarctica.AustralianAntarctic1998;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CampAreaAstro()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Antarctica.CampAreaAstro;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void DeceptionIsland()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Antarctica.DeceptionIsland;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Petrels1972()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Antarctica.Petrels1972;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PointeGeologiePerroud1950()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Antarctica.PointeGeologiePerroud1950;
            Tester.TestProjection(pStart);
        }
    }
}
