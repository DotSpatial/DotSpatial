
using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the GausKrugerBeijing1954 category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class GausKrugerBeijing1954
    {
        [Test]
        [TestCaseSource("GetProjections")]
        public void GausKrugerBeijing1954Tests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.GausKrugerBeijing1954);
        }
    }
}