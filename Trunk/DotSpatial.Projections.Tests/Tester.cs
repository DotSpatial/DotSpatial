using System;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    public static class Tester
    {
        /// <summary>
        /// The code that actually tests an input projection
        /// </summary>
        public static void TestProjection(ProjectionInfo pStart)
        {
            var pEnd = KnownCoordinateSystems.Geographic.World.WGS1984;
            var xy = new double[2];
            double x = 0;
            if (pStart.FalseEasting != null)
            {
                x = pStart.FalseEasting.Value;
                xy[0] = pStart.FalseEasting.Value;
            }
            var z = new double[1];
            Reproject.ReprojectPoints(xy, z, pStart, pEnd, 0, 1);

            double y = 0;
            var source = pStart.ToProj4String();
            var prj = new Proj4();
            prj.ProjectPoint(ref x, ref y, source, pEnd.ToProj4String());
            if (Math.Abs(x - xy[0]) > 0.00000001)
            {
                Assert.Fail("The longitude was off by {0} decimal degrees from proj4", (x - xy[0]));
            }
            if (Math.Abs(y - xy[1]) > 0.00000001)
            {
                Assert.Fail("The latitude was off by {0} decimal degrees from proj4", (y - xy[1]));
            }
            z[0] = 0;
            Reproject.ReprojectPoints(xy, z, pEnd, pStart, 0, 1);
            prj.ProjectPoint(ref x, ref y, pEnd.ToProj4String(), source);
            if (Math.Abs(x - xy[0]) > 1 / pStart.Unit.Meters)
            {
                Assert.Fail("The X coordinate was off by {0} {1}", (x - xy[0]), pStart.GetUnitText(xy[0]));
            }
            if (Math.Abs(y - xy[1]) > 1 / pStart.Unit.Meters)
            {
                Assert.Fail("The Y coordinate was off by {0} {1}", (y - xy[1]), pStart.GetUnitText(xy[1]));
            }
        }
    }
}
