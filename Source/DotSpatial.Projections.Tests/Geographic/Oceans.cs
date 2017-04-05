using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the ocean categoriese of Geographic coordinate systems.
    /// </summary>
    [TestFixture]
    public class Oceans
    {
        [Test]
        [TestCaseSource(nameof(GetPacificOceanProjections))]
        [TestCaseSource(nameof(GetIndianOceanProjections))]
        public void OceansTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(true, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetPacificOceanProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.PacificOcean);
        }


        private static IEnumerable<ProjectionInfoDesc> GetIndianOceanProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.IndianOcean);
        }

       

    }
}
