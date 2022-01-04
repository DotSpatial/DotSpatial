using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the UtmNad1927 category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class UtmNad1927
    {
        [Test]
        [TestCaseSource("GetProjections")]
        [Ignore("")] // GridShiftMissingException
        public void UtmNad1927Tests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.UtmNad1927);
        }
    }
}