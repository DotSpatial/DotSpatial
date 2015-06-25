using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the Nad1983IntlFeet category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class Nad1983IntlFeet
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void Nad1983IntlFeetTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.Nad1983IntlFeet);
        }
    }
}