
using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the NationalGridsNorway category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class NationalGridsNorway
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void NationalGridsNorwayTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.NationalGridsNorway);
        }
    }
}