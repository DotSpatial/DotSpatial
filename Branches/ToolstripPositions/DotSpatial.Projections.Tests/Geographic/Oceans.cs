using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the Oceans category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class Oceans
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void OceansTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(true, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.Oceans);
        }
    }
}
