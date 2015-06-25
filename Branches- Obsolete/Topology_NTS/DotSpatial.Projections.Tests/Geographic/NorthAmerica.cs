
using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the NorthAmerica category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class NorthAmerica
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void NorthAmericaTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.NorthAmerica);
        }
    }
}
