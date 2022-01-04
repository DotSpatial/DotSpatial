
using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the NationalGridsIndia category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class NationalGridsIndia
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void NationalGridsIndiaProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.NationalGridsIndia);
        }
    }
}