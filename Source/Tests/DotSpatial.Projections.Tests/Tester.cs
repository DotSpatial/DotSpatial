// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    /// <summary>
    /// Contains the code that actually tests an input projection.
    /// </summary>
    public static class Tester
    {
        private static readonly ProjectionInfo _pEnd = KnownCoordinateSystems.Geographic.World.WGS1984;
        private static readonly string pEndProj4 = _pEnd.ToProj4String();

        /// <summary>
        /// The code that actually tests an input projection.
        /// </summary>
        public static void TestProjection(ProjectionInfo pStart)
        {
            double[] xy = new double[2];
            double x = 0;
            if (pStart.FalseEasting != null)
            {
                x = pStart.FalseEasting.Value;
                xy[0] = pStart.FalseEasting.Value;
            }

            double[] z = new double[1];
            Reproject.ReprojectPoints(xy, z, pStart, _pEnd, 0, 1);

            double y = 0;
            string source = pStart.ToProj4String();
            Proj4 prj = new Proj4();
            prj.ProjectPoint(ref x, ref y, source, pEndProj4);
            if (Math.Abs(x - xy[0]) > 0.00000001)
            {
                Assert.Fail("The longitude was off by {0} decimal degrees from proj4", (x - xy[0]));
            }

            if (Math.Abs(y - xy[1]) > 0.00000001)
            {
                Assert.Fail("The latitude was off by {0} decimal degrees from proj4", (y - xy[1]));
            }

            z[0] = 0;
            Reproject.ReprojectPoints(xy, z, _pEnd, pStart, 0, 1);
            prj.ProjectPoint(ref x, ref y, pEndProj4, source);
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
