using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the UtmNad1983 category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class UtmNad1983
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void UtmNad1983Tests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.UtmNad1983);
        }
    }
}