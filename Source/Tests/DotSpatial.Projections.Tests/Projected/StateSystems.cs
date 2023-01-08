
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StateSystems category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class StateSystems
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            
        }

        /// <summary>
        /// Test for NAD1927AlaskaAlbersFeet       
        /// </summary>
        [Test]
        [Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        public void NAD1927AlaskaAlbersFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1927AlaskaAlbersFeet;
            Tester.TestProjection(pStart);
        }

        /// <summary>
        /// Test for NAD1927AlaskaAlbersMeters       
        /// </summary>
        [Test]
        [Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        public void NAD1927AlaskaAlbersMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1927AlaskaAlbersMeters;
            Tester.TestProjection(pStart);
        }

        /// <summary>
        /// Test for NAD1927CaliforniaTealeAlbers       
        /// </summary>
        [Test]
        [Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        public void NAD1927CaliforniaTealeAlbers()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1927CaliforniaTealeAlbers;
            Tester.TestProjection(pStart);
        }

        /// <summary>
        /// Test for NAD1927GeorgiaStatewideAlbers       
        /// </summary>
        [Test]
        [Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        public void NAD1927GeorgiaStatewideAlbers()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1927GeorgiaStatewideAlbers;
            Tester.TestProjection(pStart);
        }

        /// <summary>
        /// Test for NAD1927TexasStatewideMappingSystem       
        /// </summary>
        [Test]
        [Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        public void NAD1927TexasStatewideMappingSystem()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1927TexasStatewideMappingSystem;
            Tester.TestProjection(pStart);
        }

        /// <summary>
        /// Test for NAD1983CaliforniaTealeAlbers       
        /// </summary>
        [Test]
        public void NAD1983CaliforniaTealeAlbers()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983CaliforniaTealeAlbers;
            Tester.TestProjection(pStart);
        }

        /// <summary>
        /// Test for NAD1983GeorgiaStatewideLambert       
        /// </summary>
        [Test]
        public void NAD1983GeorgiaStatewideLambert()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983GeorgiaStatewideLambert;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NAD1983HARNOregonStatewideLambert       
        /// </summary>
        [Test]
        public void NAD1983HARNOregonStatewideLambert()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983HARNOregonStatewideLambert;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NAD1983HARNOregonStatewideLambertFeetIntl       
        /// </summary>
        [Test]
        public void NAD1983HARNOregonStatewideLambertFeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983HARNOregonStatewideLambertFeetIntl;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NAD1983IdahoTM       
        /// </summary>
        [Test]
        public void NAD1983IdahoTM()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983IdahoTM;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NAD1983OregonStatewideLambert       
        /// </summary>
        [Test]
        public void NAD1983OregonStatewideLambert()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983OregonStatewideLambert;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NAD1983OregonStatewideLambertFeetIntl       
        /// </summary>
        [Test]
        public void NAD1983OregonStatewideLambertFeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983OregonStatewideLambertFeetIntl;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NAD1983TexasCentricMappingSystemAlbers       
        /// </summary>
        [Test]
        public void NAD1983TexasCentricMappingSystemAlbers()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983TexasCentricMappingSystemAlbers;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NAD1983TexasCentricMappingSystemLambert       
        /// </summary>
        [Test]
        public void NAD1983TexasCentricMappingSystemLambert()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983TexasCentricMappingSystemLambert;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for NAD1983TexasStatewideMappingSystem       
        /// </summary>
        [Test]
        public void NAD1983TexasStatewideMappingSystem()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983TexasStatewideMappingSystem;
            Tester.TestProjection(pStart);
        }
    }
}