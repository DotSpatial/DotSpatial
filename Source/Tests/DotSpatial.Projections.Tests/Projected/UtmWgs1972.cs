// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the UtmWgs1972 category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class UtmWgs1972
    {
        /// <summary>
        /// Tests for the UtmWgs1972 category of Projected coordinate systems.
        /// </summary>
        /// <param name="pInfo"></param>
        [Test]
        [TestCaseSource("GetProjections")]
        public void UtmWgs1972ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
        private static IEnumerable<ProjectionInfoDesc> GetProjections()
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.UtmWgs1972);
        }
    }
}