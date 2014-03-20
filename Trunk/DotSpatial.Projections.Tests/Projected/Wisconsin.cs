using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the Wisconsin category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class Wisconsin
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void WisconsinProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.Wisconsin);
        }
    }
}