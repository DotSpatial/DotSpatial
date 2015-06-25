using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StatePlaneNad1983Harn category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class StatePlaneNad1983Harn
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void StatePlaneNad1983HarnProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.StatePlaneNad1983Harn);
        }
    }
}