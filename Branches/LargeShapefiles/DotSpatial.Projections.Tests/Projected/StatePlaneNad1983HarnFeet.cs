using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StatePlaneNad1983HarnFeet category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class StatePlaneNad1983HarnFeet
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void StatePlaneNad1983HarnFeetProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet);
        }
    }
}