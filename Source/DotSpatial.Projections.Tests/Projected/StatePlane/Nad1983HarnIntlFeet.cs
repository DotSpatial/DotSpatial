using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected.StatePlane
{
    /// <summary>
    /// This class contains all the tests for the StatePlane.Nad1983HarnIntlFeet category of Projected coordinate systems.
    /// </summary>
    [TestFixture]
    public class Nad1983HarnIntlFeet
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
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.StatePlane.Nad1983HarnIntlFeet);
        }
    }
}