using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected.StatePlane
{
    /// <summary>
    /// This class contains all the tests for the StatePlane.Nad1983Cors96IntlFeet category of Projected coordinate systems.
    /// </summary>
    [TestFixture]
    public class Nad1983Cors96IntlFeet
    {
        [Test]
        [TestCaseSource(nameof(GetProjections))]
        public void ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.StatePlane.Nad1983Cors96IntlFeet);
        }
    }
}