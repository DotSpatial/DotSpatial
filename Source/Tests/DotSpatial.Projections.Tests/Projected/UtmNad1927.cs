// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the UtmNad1927 category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class UtmNad1927
    {
        /// <summary>
        /// Tests for the UtmNad1927 category of Projected coordinate systems.
        /// </summary>
        /// <param name="pInfo"></param>
        [Test, Category("Projection")]
        [TestCaseSource("GetProjections")]
        [Ignore("")] // GridShiftMissingException
        public void UtmNad1927Tests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
        private static IEnumerable<ProjectionInfoDesc> GetProjections()
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.UtmNad1927);
        }
    }
}