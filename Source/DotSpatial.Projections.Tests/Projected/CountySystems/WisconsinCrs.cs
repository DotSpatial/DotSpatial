using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected.CountySystems
{
    /// <summary>
    /// This class contains all the tests for the CountySystems.WisconsinCrs category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class WisconsinCrs
    {
        [Test]
        [TestCaseSource(nameof(GetMetersProjections))]
        [TestCaseSource(nameof(GetUsFeetProjections))]
        public void WisconsinProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetMetersProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.CountySystems.WisconsinCrs.Meters);
        }

        private static IEnumerable<ProjectionInfoDesc> GetUsFeetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.CountySystems.WisconsinCrs.UsFeet);
        }
    }
}