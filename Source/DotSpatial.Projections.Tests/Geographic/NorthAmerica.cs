using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the NorthAmerica category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class NorthAmerica
    {
        [Test]
        [TestCaseSource(nameof(GetProjections))]
        public void NorthAmericaTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(true, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.NorthAmerica)
                // fixme: ignore GridShiftMissingException
                .Where(_ => _.Name != "NorthAmericanDatum1927");
        }
    }
}
