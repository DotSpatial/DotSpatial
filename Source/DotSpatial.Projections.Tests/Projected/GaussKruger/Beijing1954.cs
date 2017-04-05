using System.Collections.Generic;

using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected.GaussKruger
{
    /// <summary>
    /// This class contains all the tests for the GaussKruger.Beijing1954 category of Projected coordinate systems.
    /// </summary>
    [TestFixture]
    public class Beijing1954
    {
        [Test]
        [TestCaseSource(nameof(GetProjections))]
        public void ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.GaussKruger.Beijing1954);
        }
    }
}