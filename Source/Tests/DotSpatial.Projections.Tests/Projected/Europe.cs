// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the Europe category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class Europe
    {
        /// <summary>
        /// Tests for the Europe category of Projected coordinate systems.
        /// </summary>
        /// <param name="pInfo"></param>
        [Test, Category("Projection")]
        [TestCaseSource("GetProjections")]
        public void EuropeProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        /// <summary>
        /// Test for ETRS1989LAEA       
        /// </summary>
        [Test, Category("Projection")]
        public void ETRS1989LAEA()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.WGS1984;
            ProjectionInfo pEnd = KnownCoordinateSystems.Projected.Europe.ETRS1989LAEA;

            // Vienna, Austria
            double lon = 16.4;
            double lat = 48.2;

            double[] xy = new double[] { lon, lat };
            double[] z = new double[] { 0 };

            Reproject.ReprojectPoints(xy, z, pStart, pEnd, 0, 1);

            Reproject.ReprojectPoints(xy, z, pEnd, pStart, 0, 1);

            // Test X
            Assert.AreEqual(lon, xy[0], 0.00001);

            // Test Y
            Assert.AreEqual(lat, xy[1], 0.00001);
        }

        /// <summary>
        /// Test for PlateCarreeFromEsriString       
        /// </summary>
        [Test, Category("Projection")]
        public void ETRS1989LAEAFromEsriStringInThai()
        {
            var projectionInfo = KnownCoordinateSystems.Projected.Europe.ETRS1989LAEA;
            var toEsriString = projectionInfo.ToEsriString();

            // Set the culture to Thai (Thailand)
            Thread.CurrentThread.CurrentCulture = new CultureInfo("th-TH");

            var projectionInfoFromEsriString = ProjectionInfo.FromEsriString(toEsriString);

            // Test that the angular unit was parsed correctly
            Assert.AreEqual("Degree", projectionInfoFromEsriString.GeographicInfo.Unit.Name);
            Assert.AreEqual(0.01745, projectionInfoFromEsriString.GeographicInfo.Unit.Radians, 0.00001);
        }

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
        private static IEnumerable<ProjectionInfoDesc> GetProjections()
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.Europe);
        }
    }
}