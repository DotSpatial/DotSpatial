
using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the KrugerXian1980 category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class KrugerXian1980
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void KrugerXian1980ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.KrugerXian1980);
        }
    }
}