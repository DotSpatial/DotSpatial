
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the NationalGridsNorway category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class NationalGridsNorway
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
        public void NGO1948BaerumKommune()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948BaerumKommune;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948Bergenhalvoen()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948Bergenhalvoen;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948NorwayZone1()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948NorwayZone1;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948NorwayZone2()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948NorwayZone2;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948NorwayZone3()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948NorwayZone3;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948NorwayZone4()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948NorwayZone4;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948NorwayZone5()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948NorwayZone5;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948NorwayZone6()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948NorwayZone6;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948NorwayZone7()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948NorwayZone7;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948NorwayZone8()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948NorwayZone8;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948OsloKommune()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948OsloKommune;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NGO1948OsloNorwayZone1()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948OsloNorwayZone1;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NGO1948OsloNorwayZone2()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948OsloNorwayZone2;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NGO1948OsloNorwayZone3()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948OsloNorwayZone3;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NGO1948OsloNorwayZone4()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948OsloNorwayZone4;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NGO1948OsloNorwayZone5()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948OsloNorwayZone5;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NGO1948OsloNorwayZone6()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948OsloNorwayZone6;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NGO1948OsloNorwayZone7()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948OsloNorwayZone7;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NGO1948OsloNorwayZone8()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNorway.NGO1948OsloNorwayZone8;
            Tester.TestProjection(pStart);
        }
    }
}