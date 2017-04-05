using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected.Utm
{
    /// <summary>
    /// This class contains all the tests for the Utm.Wgs1972 category of Projected coordinate systems.
    /// </summary>
    [TestFixture]
    public class Wgs1972
    {
        [Test]
        [TestCaseSource(nameof(GetNorthernHemisphereProjections))]
        [TestCaseSource(nameof(GetSouthernHemisphereProjections))]
        public void ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetNorthernHemisphereProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.Utm.Wgs1972.NorthernHemisphere);
        }

        private static IEnumerable<ProjectionInfoDesc> GetSouthernHemisphereProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.Utm.Wgs1972.SouthernHemisphere);
        }
    }
}