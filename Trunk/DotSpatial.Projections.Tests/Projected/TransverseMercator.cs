using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the TransverseMercator category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class TransverseMercator
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void TransverseMercatorTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.TransverseMercator);
        }
    }
}