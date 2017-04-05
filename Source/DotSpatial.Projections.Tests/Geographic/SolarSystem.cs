using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the SolarSystem category of Geographic coordinate systems.
    /// </summary>
    [TestFixture]
    public class SolarSystem
    {
        [Test]
        [TestCaseSource(nameof(GetEarthProjections))]
        [TestCaseSource(nameof(GetJupiterProjections))]
        [TestCaseSource(nameof(GetMarsProjections))]
        [TestCaseSource(nameof(GetMercuryProjections))]
        [TestCaseSource(nameof(GetNeptuneProjections))]
        [TestCaseSource(nameof(GetPlutoProjections))]
        [TestCaseSource(nameof(GetSaturnProjections))]
        [TestCaseSource(nameof(GetUranusProjections))]
        [TestCaseSource(nameof(GetVenusProjections))]
        public void SolarSystemGeographicTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(true, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetEarthProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.SolarSystem.Earth);
        }
        private static IEnumerable<ProjectionInfoDesc> GetJupiterProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.SolarSystem.Jupiter);
        }
        private static IEnumerable<ProjectionInfoDesc> GetMarsProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.SolarSystem.Mars);
        }
        private static IEnumerable<ProjectionInfoDesc> GetMercuryProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.SolarSystem.Mercury);
        }
        private static IEnumerable<ProjectionInfoDesc> GetNeptuneProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.SolarSystem.Neptune);
        }
        private static IEnumerable<ProjectionInfoDesc> GetPlutoProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.SolarSystem.Pluto);
        }
        private static IEnumerable<ProjectionInfoDesc> GetSaturnProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.SolarSystem.Saturn);
        }
        private static IEnumerable<ProjectionInfoDesc> GetUranusProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.SolarSystem.Uranus);
        }
        private static IEnumerable<ProjectionInfoDesc> GetVenusProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.SolarSystem.Venus);
        }
    }
}
