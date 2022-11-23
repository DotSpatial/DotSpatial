// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    /// <summary>
    ///This is a test class for CoordinateSystemTypeTest and is intended
    ///to test the new CoordinateSystemType field for ProjectionInfo.
    ///</summary>
    [TestFixture]
    public class CoordinateSystemTypeTest
    {
        /// <summary>
        /// CoordinateSystemType vs IsLatLon comparison test.
        /// </summary>
        [Test]
        public void CoordSysIsLatLonComparisonTest()
        {
            //test projected coordinate systems
            ICoordinateSystemCategoryHolder CoordSysCategoryHolder = (ICoordinateSystemCategoryHolder)KnownCoordinateSystems.Projected;
            TestCategoryHolder(CoordSysCategoryHolder);

            //test geographic coordinate systems
            CoordSysCategoryHolder = KnownCoordinateSystems.Geographic;
            TestCategoryHolder(CoordSysCategoryHolder);

            //test some user defined projections
            System.Diagnostics.Debug.Print("{0}==========================================", "User Defined");
            ProjectionInfo prj = ProjectionInfo.FromEsriString("PROJCS[\"Sphere_Vertical_Perspective\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.2572235629972]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]], PARAMETER[\"Scale_Factor\",1],UNIT[\"Meter\",1]]");
            DebugProjection("CustMaj", "CustMin", prj);

            //prj = ProjectionInfo.FromEsriString("PROJCS[\"Sphere_Vertical_Perspective\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Vertical_Near_Side_Perspective\"],PARAMETER[\"False_Easting\",0.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Longitude_Of_Center\",0.0],PARAMETER[\"Latitude_Of_Center\",0.0],PARAMETER[\"Height\",35800000.0],UNIT[\"Meter\",1.0]]");
            //DebugProjection("CustMaj", "CustMin", prj);

            prj = ProjectionInfo.FromProj4String("+proj=wink1 +lon_0=0 +lat_ts=50.4597762521898 +x_0=0 +y_0=0 +datum=WGS84 +units=m +no_defs +type=crs");
            DebugProjection("CustMaj", "CustMin", prj);

            prj = ProjectionInfo.FromProj4String("+proj=wink1 +lon_0=0 +lat_ts=50.4597762521898 +x_0=0 +y_0=0 +R=6371000 +units=m +no_defs +type=crs");
            DebugProjection("CustMaj", "CustMin", prj);
        }

        private void TestCategoryHolder(ICoordinateSystemCategoryHolder CoordSysCategoryHolder)
        {
            System.Diagnostics.Debug.Print("{0}==========================================", CoordSysCategoryHolder.ToString());
            foreach (var majCatName in CoordSysCategoryHolder.Names)
            {
                var coordSysCat = CoordSysCategoryHolder.GetCategory(majCatName);
                if (coordSysCat != null)
                {
                    foreach (var minCatName in coordSysCat.Names)
                    {
                        ProjectionInfo prj = coordSysCat.GetProjection(minCatName);
                        DebugProjection(majCatName, minCatName, prj);
                    }
                }
            }
        }

        private string ReprojectionTest(ProjectionInfo ProjTarget)
        {
            try
            {
                //pick a standard source
                ProjectionInfo ProjSource = KnownCoordinateSystems.Geographic.World.WGS1984;
                double Xsource = -96.7196;
                double Ysource = 32.9886;
                double Zsource = 0;

                //create array of the source coordinates
                double[] xy = { Xsource, Ysource };
                double[] z = { Zsource };

                //reproject the coordinates from the source projection to target projection
                Reproject.ReprojectPoints(xy, z, ProjSource, ProjTarget, 0, 1);

                //assign reprojected coordinates from the result to the target
                double Xtarget = xy[0];
                double Ytarget = xy[1];
                double Ztarget = z[0];

                //build a stgring
                StringBuilder sb = new();
                sb.AppendLine("    Source(WGS84)                  Target(this)");
                sb.AppendFormat("  X    {0,10:N4}     {1,25:N4}\r\n", Xsource, Xtarget);
                sb.AppendFormat("  Y    {0,10:N4}     {1,25:N4}\r\n", Ysource, Ytarget);
                sb.AppendFormat("  Z    {0,10:N4}     {1,25:N4}", Zsource, Ztarget);

                //return the string
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return "  <reproject.error>";
            }
        }

        private void DebugProjection(string majCatName, string minCatName, ProjectionInfo prj)
        {
            System.Diagnostics.Debug.Print("{0}:{1}\r\n  InputString: {2}\r\n  EsriPrj: {3}\r\n  Proj4: {4}", majCatName, minCatName, prj.CoordinateSystemInputString, prj.ToEsriString(), prj.ToProj4String());
            System.Diagnostics.Debug.Print("  IsLatLon: {0}\r\n  CoordinateSystemType={1}\r\n  Transform={2}\r\n  Datum={3}", prj.IsLatLon, prj.CoordinateSystemType, prj.Transform?.Name, prj.GeographicInfo?.Datum?.Name);
            System.Diagnostics.Debug.Print("{0}", ReprojectionTest(prj));
        }
    }
}
