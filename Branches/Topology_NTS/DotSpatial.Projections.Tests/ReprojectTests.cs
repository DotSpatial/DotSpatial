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
    }
}
