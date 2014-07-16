using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StatePlaneOther category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class StatePlaneOther
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void StatePlaneOtherProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.StatePlaneOther);
        }
    }
}