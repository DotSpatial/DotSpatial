using System.Collections.Generic;
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the NationalGridsNewZealand category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class NationalGridsNewZealand
    {
        [TestAttribute]
        [TestCaseSource("GetProjections")]
        public void NationalGridsNewZealandTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);   
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.NationalGridsNewZealand);
        }
    }
}