using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the SpheroidBased category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class SpheroidBased
    {
        [Test]
        [TestCaseSource(nameof(GetProjections))]
        public void SpheroidBasedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(true, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.SpheroidBased);
        }
    }
}
