using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the UtmWgs1972 category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class UtmWgs1972
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void UtmWgs1972ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.UtmWgs1972);
        }
    }
}