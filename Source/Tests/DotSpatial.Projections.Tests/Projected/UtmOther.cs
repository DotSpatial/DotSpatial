using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the UtmOther category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class UtmOther
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void UtmOtherTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.UtmOther)
                // fixme: Ignore GridShiftMissingException
                .Where(_ => _.Name != "NAD1927BLMZone14N" &&
                            _.Name != "NAD1927BLMZone15N" &&
                            _.Name != "NAD1927BLMZone16N" &&
                            _.Name != "NAD1927BLMZone17N");
        }
    }
}