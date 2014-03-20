using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the Asia category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class Asia
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void AsiaGeographicTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.Asia);
        }
    }
}
