
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the World category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class World
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
        public void GRS1980()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.GRS1980;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ITRF1988()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.ITRF1988;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ITRF1989()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.ITRF1989;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ITRF1990()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.ITRF1990;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ITRF1991()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.ITRF1991;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ITRF1992()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.ITRF1992;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ITRF1993()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.ITRF1993;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ITRF1994()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.ITRF1994;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ITRF1996()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.ITRF1996;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ITRF1997()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.ITRF1997;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ITRF2000()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.ITRF2000;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NSWC9Z2()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.NSWC9Z2;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void WGS1966()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.WGS1966;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void WGS1972()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.WGS1972;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void WGS1972TBE()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.WGS1972TBE;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void WGS1984()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.WGS1984;
            Tester.TestProjection(pStart);
        }
    }
}
