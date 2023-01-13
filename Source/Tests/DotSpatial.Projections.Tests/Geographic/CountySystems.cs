// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the CountySystems category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class CountySystems
    {
        /// <summary>
        /// Tests for the CountySystems category of Geographic coordinate systems.
        /// </summary>
        /// <param name="pInfo"></param>
        [Test, Category("Projection")]
        [TestCaseSource("GetProjections")]
        public void CountySystemsGeographicTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(true, pInfo.ProjectionInfo.IsLatLon);
        }

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
        private static IEnumerable<ProjectionInfoDesc> GetProjections()
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.CountySystems);
        }
    }
}
