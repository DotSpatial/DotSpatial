using System;
using DotSpatial.Projections;
using NUnit.Framework;

namespace DotSpatial.Projection.Tests
{
    public static class Tester
    {
        /// <summary>
        /// The code that actually tests an input projection
        /// </summary>
        public static void TestProjection(ProjectionInfo pStart)
        {
            ProjectionInfo pEnd = KnownCoordinateSystems.Geographic.World.WGS1984;
            double[] xy = new double[2];
            double x = 0;
            if (pStart.FalseEasting != null)
            {
                x = pStart.FalseEasting.Value;
                xy[0] = pStart.FalseEasting.Value;
            }
            double[] z = new double[1];
            Reproject.ReprojectPoints(xy, z, pStart, pEnd, 0, 1);

            double y = 0;
            string source = pStart.ToProj4String();
            Proj4.ProjectPoint(ref x, ref y, source, pEnd.ToProj4String());
            if (Math.Abs(x - xy[0]) > 0.00000001)
            {
                Assert.Fail(String.Format("The longitude was off by {0} decimal degrees from proj4", (x - xy[0])));
            }
            if (Math.Abs(y - xy[1]) > 0.00000001)
            {
                Assert.Fail(String.Format("The latitude was off by {0} decimal degrees from proj4", (y - xy[1])));
            }
            z[0] = 0;
            Reproject.ReprojectPoints(xy, z, pEnd, pStart, 0, 1);
            Proj4.ProjectPoint(ref x, ref y, pEnd.ToProj4String(), source);
            if (Math.Abs(x - xy[0]) > 1 / pStart.Unit.Meters)
            {
                Assert.Fail(String.Format("The X coordinate was off by {0} {1}", (x - xy[0]), pStart.GetUnitText(xy[0])));
            }
            if (Math.Abs(y - xy[1]) > 1 / pStart.Unit.Meters)
            {
                Assert.Fail(String.Format("The Y coordinate was off by {0} {1}", (y - xy[1]), pStart.GetUnitText(xy[1])));
            }
        }
    }
}
