using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StateSystems category of Projected coordinate systems.
    /// </summary>
    [TestFixture]
    public class StateSystems
    {
        [Test]
        [TestCaseSource(nameof(GetProjections))]
        public void ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.StateSystems);
        }

        //[Test]
        //[Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        //public void NAD1927AlaskaAlbersFeet()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1927AlaskaAlbersFeet;
        //    Tester.TestProjection(pStart);
        //}

        //[Test]
        //[Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        //public void NAD1927AlaskaAlbersMeters()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1927AlaskaAlbersMeters;
        //    Tester.TestProjection(pStart);
        //}

        //[Test]
        //[Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        //public void NAD1927CaliforniaTealeAlbers()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1927CaliforniaTealeAlbers;
        //    Tester.TestProjection(pStart);
        //}

        //[Test]
        //[Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        //public void NAD1927GeorgiaStatewideAlbers()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1927GeorgiaStatewideAlbers;
        //    Tester.TestProjection(pStart);
        //}

        //[Test]
        //[Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        //public void NAD1927TexasStatewideMappingSystem()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1927TexasStatewideMappingSystem;
        //    Tester.TestProjection(pStart);
        //}

        //[Test]
        //public void NAD1983CaliforniaTealeAlbers()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983CaliforniaTealeAlbers;
        //    Tester.TestProjection(pStart);
        //}

        //[Test]
        //public void NAD1983GeorgiaStatewideLambert()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983GeorgiaStatewideLambert;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NAD1983HARNOregonStatewideLambert()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983HARNOregonStatewideLambert;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NAD1983HARNOregonStatewideLambertFeetIntl()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983HARNOregonStatewideLambertFeetIntl;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NAD1983IdahoTM()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983IdahoTM;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NAD1983OregonStatewideLambert()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983OregonStatewideLambert;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NAD1983OregonStatewideLambertFeetIntl()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983OregonStatewideLambertFeetIntl;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NAD1983TexasCentricMappingSystemAlbers()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983TexasCentricMappingSystemAlbers;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NAD1983TexasCentricMappingSystemLambert()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983TexasCentricMappingSystemLambert;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NAD1983TexasStatewideMappingSystem()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.StateSystems.NAD1983TexasStatewideMappingSystem;
        //    Tester.TestProjection(pStart);
        //}
    }
}