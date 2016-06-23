
using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the Europe category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class Europe
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void EuropeProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        [Test]
        public void ETRS1989LAEA()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.WGS1984;
            ProjectionInfo pEnd = KnownCoordinateSystems.Projected.Europe.ETRS1989LAEA;

            // Vienna, Austria
            var lon = 16.4;
            var lat = 48.2;

            double[] xy = new double[] { lon, lat };
            double[] z = new double[] { 0 };

            Reproject.ReprojectPoints(xy, z, pStart, pEnd, 0, 1);

            Reproject.ReprojectPoints(xy, z, pEnd, pStart, 0, 1);

            // Test X
            Assert.AreEqual(lon, xy[0], 0.00001);

            // Test Y
            Assert.AreEqual(lat, xy[1], 0.00001);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.Europe);
        }
    }
}