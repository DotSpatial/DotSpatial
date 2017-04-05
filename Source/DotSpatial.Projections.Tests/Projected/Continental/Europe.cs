using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected.Continental
{
    /// <summary>
    /// This class contains all the tests for the Continental.Europe category of Projected coordinate systems.
    /// </summary>
    [TestFixture]
    public class Europe
    {
        [Test]
        public void Etrs1989Laea()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.WGS1984;
            ProjectionInfo pEnd = KnownCoordinateSystems.Projected.Continental.Europe.ETRS1989LAEA;

            // Vienna, Austria
            var lon = 16.4;
            var lat = 48.2;

            double[] xy = { lon, lat };
            double[] z = { 0 };

            Reproject.ReprojectPoints(xy, z, pStart, pEnd, 0, 1);

            Reproject.ReprojectPoints(xy, z, pEnd, pStart, 0, 1);

            // Test X
            Assert.AreEqual(lon, xy[0], 0.00001);

            // Test Y
            Assert.AreEqual(lat, xy[1], 0.00001);
        }

        [Test]
        [TestCaseSource(nameof(GetProjections))]
        public void ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.Continental.Europe);
        }
    }
}
