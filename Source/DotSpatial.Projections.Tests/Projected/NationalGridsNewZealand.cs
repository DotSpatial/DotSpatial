using System.Collections.Generic;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
      /// <summary>
    /// This class contains all the tests for the NationalGridsNewZealand category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class NationalGridsNewZealand
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            GridShift.InitializeExternalGrids(Common.AbsolutePath("GeogTransformGrids"), false);
        }

        [Test]
        [TestCaseSource("GetProjections")]
        public void NationalGridsNewZealandTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.NationalGridsNewZealand);
        }
    }
}