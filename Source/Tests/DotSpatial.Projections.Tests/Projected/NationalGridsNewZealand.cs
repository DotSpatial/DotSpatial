// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

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
        /// <summary>
        /// 
        /// </summary>
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            GridShift.InitializeExternalGrids(Common.AbsolutePath("GeogTransformGrids"), false);
        }

        /// <summary>
        /// Tests for the NationalGridsNewZealand category of Projected coordinate systems.
        /// </summary>
        /// <param name="pInfo"></param>
        [Test]
        [TestCaseSource("GetProjections")]
        public void NationalGridsNewZealandTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
        private static IEnumerable<ProjectionInfoDesc> GetProjections()
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.NationalGridsNewZealand);
        }
    }
}