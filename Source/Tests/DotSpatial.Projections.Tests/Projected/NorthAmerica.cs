// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the NorthAmerica category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class NorthAmerica
    {
        /// <summary>
        /// Test for USAContiguousLambertConformalConic       
        /// </summary>
        [Test]
        public void USAContiguousLambertConformalConic()
        {
            //Sets up a array to contain the x and y coordinates
            double[] first = new double[] { 0, 1 };

            //Defines the starting coordiante system
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.WGS1984;

            //Defines the ending coordiante system
            ProjectionInfo pEnd = KnownCoordinateSystems.Projected.NorthAmerica.USAContiguousLambertConformalConic;

            // Calls the reproject function that will transform the input location to the output locaiton
            Reproject.ReprojectPoints(first, null, pStart, pEnd, 0, 1);

            Assert.AreEqual(first[0], 10723420.030693574);
            Assert.AreEqual(first[1], 1768929.0089786104);
        }

        /// <summary>
        /// Tests for the NorthAmerica category of Projected coordinate systems.
        /// </summary>
        /// <param name="pInfo"></param>
        [Test]
        [TestCaseSource("GetProjections")]
        public void NorthAmericaTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
        private static IEnumerable<ProjectionInfoDesc> GetProjections()
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.NorthAmerica);
        }
    }
}