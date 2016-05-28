using System;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    [TestFixture]
    class ReprojectTests
    {
        [Test]
        public void ReprojectPointsWithOtherRanging()
        {
            var geographic = ProjectionInfo.FromProj4String(KnownCoordinateSystems.Geographic.World.WGS1984.ToProj4String());
            var projected = ProjectionInfo.FromProj4String(KnownCoordinateSystems.Projected.World.WebMercator.ToProj4String());
            geographic.Over = true;
            projected.Over = true;
            double[] pointsXY = { -445, 33 };
            double[] pointsXYCopy = { -445, 33 };
            double[] pointsZ = { 0 };
            Reproject.ReprojectPoints(pointsXY, pointsZ, geographic, projected, 0, 1);
            Reproject.ReprojectPoints(pointsXY, pointsZ, projected, geographic, 0, 1);
            Assert.IsTrue(Math.Abs(pointsXY[0] - pointsXYCopy[0]) < 0.00000000001);
            Assert.IsTrue(Math.Abs(pointsXY[1] - pointsXYCopy[1]) < 0.00000000001);
        }

        [Test(Description = "Checks that reprojection for group works the same as reprojection for one point. (https://github.com/DotSpatial/DotSpatial/issues/781)")]
        public void Reprojection_NorthPoleStereographic_ForGroupTheSameAsForOnePoint()
        {
            var wgs = KnownCoordinateSystems.Geographic.World.WGS1984;
            var wgs84Points = new double[] { 10, 10, 45, 45, 80, 80 };

            const double DELTA = double.Epsilon;

            var projectionInfo = KnownCoordinateSystems.Projected.Polar.NorthPoleStereographic;
            var testProjectionPoints = new double[wgs84Points.Length];
            Array.Copy(wgs84Points, testProjectionPoints, testProjectionPoints.Length);
            Reproject.ReprojectPoints(testProjectionPoints, null, wgs, projectionInfo, 0, testProjectionPoints.Length / 2);

            var backWgs84 = new double[testProjectionPoints.Length];
            Array.Copy(testProjectionPoints, backWgs84, backWgs84.Length);

            // Reproject group from projectionInfo to wgs
            Reproject.ReprojectPoints(backWgs84, null, projectionInfo, wgs, 0, backWgs84.Length / 2);

            // Now reproject each point separately and verify that it is same as in group reprojection
            for (int i = 0; i < testProjectionPoints.Length / 2; i++)
            {
                var onePoint = new double[2];
                onePoint[0] = testProjectionPoints[i * 2];
                onePoint[1] = testProjectionPoints[i * 2 + 1];

                Reproject.ReprojectPoints(onePoint, null, projectionInfo, wgs, 0, 1);

                Assert.AreEqual(onePoint[0], backWgs84[i * 2], DELTA);
                Assert.AreEqual(onePoint[1], backWgs84[i * 2 + 1], DELTA);
            }            
        }

        [Test(Description = "Checks that there is no NANs in output for LAEA projections (https://github.com/DotSpatial/DotSpatial/issues/387)")]
        public void LAEA_Reprojection_NoNANs()
        {
            var source = ProjectionInfo.FromProj4String("proj=longlat +ellps=WGS84 +datum=WGS84 +no_defs");
            // Any LAEA projection
            var dest = ProjectionInfo.FromProj4String("+proj=laea +lat_0=52 +lon_0=10 +x_0=4321000 +y_0=3210000 +ellps=GRS80 +units=m +datum=WGS84");

            double[] vertices = { 13.5, 51.3 };
            Reproject.ReprojectPoints(vertices, null, source, dest, 0, 1);

            Assert.IsTrue(!double.IsNaN(vertices[0]));
            Assert.IsTrue(!double.IsNaN(vertices[1]));
        }
    }
}
