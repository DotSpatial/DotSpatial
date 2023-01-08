// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace DotSpatial.Projections.Tests
{
    /// <summary>
    /// Contains the tests for Krovak projections.
    /// </summary>
    [TestClass()]
    public class KrovakTest
    {
        /// <summary>
        /// Test for Wgs84ToKrovakWithTransform
        /// </summary>
        [TestMethod()]
        public void Wgs84ToKrovakWithTransform()
        {
            double[] myOut = new double[2];
            myOut[0] = 12.8069888666667;
            myOut[1] = 49.4522626972222;
            double[] myZ = new double[1];
            myZ[0] = 0;

            ProjectionInfo source = KnownCoordinateSystems.Geographic.World.WGS1984;
            string projJTSK2 = "+proj=krovak +lat_0=49.5 +lon_0=24.83333333333333 +alpha=0 +k=0.9999 +x_0=0 +y_0=0 +ellps=bessel +towgs84=570.8,85.7,462.8,4.998,1.587,5.261,3.56 +units=m +no_defs";
            ProjectionInfo myJTSKPI = ProjectionInfo.FromProj4String(projJTSK2);

            Reproject.ReprojectPoints(myOut, myZ, source, myJTSKPI, 0, myZ.Length);

            Assert.AreEqual(myOut[0], -868208.52, 1.0);
            Assert.AreEqual(myOut[1], -1095793.96, 1.0);
        }

        /// <summary>
        /// Test for Wgs84ToKrovak_KnownCoordSys
        /// </summary>
        [TestMethod()]
        public void Wgs84ToKrovak_KnownCoordSys()
        {
            double[] myOut = new double[2];
            myOut[0] = 12.8069888666667;
            myOut[1] = 49.4522626972222;
            double[] myZ = new double[1];
            myZ[0] = 0;

            ProjectionInfo source = KnownCoordinateSystems.Geographic.World.WGS1984;
            ProjectionInfo myJTSKPI = KnownCoordinateSystems.Projected.NationalGrids.SJTSKKrovakEastNorth;

            Reproject.ReprojectPoints(myOut, myZ, source, myJTSKPI, 0, myZ.Length);

            Assert.AreEqual(myOut[0], -868208.52, 1.0);
            Assert.AreEqual(myOut[1], -1095793.96, 1.0);
        }

        /// <summary>
        /// Test for KrovakToWgs84WithTransform
        /// </summary>
        [TestMethod()]
        public void KrovakToWgs84WithTransform()
        {
            double[] myOut = new double[2];
            myOut[0] = -868208.52;
            myOut[1] = -1095793.96;
            double[] myZ = new double[1];
            myZ[0] = 0;

            ProjectionInfo projWGS84 = KnownCoordinateSystems.Geographic.World.WGS1984;
            string projJTSK2 = "+proj=krovak +lat_0=49.5 +lon_0=24.83333333333333 +alpha=0 +k=0.9999 +x_0=0 +y_0=0 +ellps=bessel +towgs84=570.8,85.7,462.8,4.998,1.587,5.261,3.56 +units=m +no_defs";
            ProjectionInfo myJTSKPI = ProjectionInfo.FromProj4String(projJTSK2);

            Reproject.ReprojectPoints(myOut, myZ, myJTSKPI, projWGS84, 0, myZ.Length);

            Assert.AreEqual(myOut[0], 12.8069888666667, 1.0);
            Assert.AreEqual(myOut[1], 49.4522626972222, 1.0);
        }

        /// <summary>
        /// Test for KrovakToWgs84_KnownCoordSys
        /// </summary>
        [TestMethod()]
        public void KrovakToWgs84_KnownCoordSys()
        {
            double[] myOut = new double[2];
            myOut[0] = -868208.52;
            myOut[1] = -1095793.96;
            double[] myZ = new double[1];
            myZ[0] = 0;

            ProjectionInfo projWGS84 = KnownCoordinateSystems.Geographic.World.WGS1984;

            //this was declared but never used so I commented it out. dpa 4/30/2013
            //string projJTSK2 = "+proj=krovak +lat_0=49.5 +lon_0=24.83333333333333 +alpha=0 +k=0.9999 +x_0=0 +y_0=0 +ellps=bessel +towgs84=570.8,85.7,462.8,4.998,1.587,5.261,3.56 +units=m +no_defs";
            ProjectionInfo myJTSKPI = KnownCoordinateSystems.Projected.NationalGrids.SJTSKKrovakEastNorth;

            Reproject.ReprojectPoints(myOut, myZ, myJTSKPI, projWGS84, 0, myZ.Length);

            Assert.AreEqual(myOut[0], 12.8069888666667, 1.0);
            Assert.AreEqual(myOut[1], 49.4522626972222, 1.0);
        }

        /// <summary>
        /// Test for KrovakToWgs84_EsriString
        /// </summary>
        [TestMethod()]
        public void KrovakToWgs84_EsriString()
        {
            double[] myOut = new double[2];
            myOut[0] = -868208.52;
            myOut[1] = -1095793.96;
            double[] myZ = new double[1];
            myZ[0] = 0;

            string jtskEsriString = @"PROJCS[""S-JTSK_Krovak_East_North"",GEOGCS[""GCS_S_JTSK"",DATUM[""D_S_JTSK"",SPHEROID[""Bessel_1841"",6377397.155,299.1528128]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]],PROJECTION[""Krovak""],PARAMETER[""False_Easting"",0.0],PARAMETER[""False_Northing"",0.0],PARAMETER[""Pseudo_Standard_Parallel_1"",78.5],PARAMETER[""Scale_Factor"",0.9999],PARAMETER[""Azimuth"",30.28813975277778],PARAMETER[""Longitude_Of_Center"",24.83333333333333],PARAMETER[""Latitude_Of_Center"",49.5],PARAMETER[""X_Scale"",-1.0],PARAMETER[""Y_Scale"",1.0],PARAMETER[""XY_Plane_Rotation"",90.0],UNIT[""Meter"",1.0]]";

            ProjectionInfo projWGS84 = KnownCoordinateSystems.Geographic.World.WGS1984;

            ProjectionInfo myJTSKPI = ProjectionInfo.FromEsriString(jtskEsriString);

            Reproject.ReprojectPoints(myOut, myZ, myJTSKPI, projWGS84, 0, myZ.Length);

            Assert.AreEqual(myOut[0], 12.8069888666667, 1.0);
            Assert.AreEqual(myOut[1], 49.4522626972222, 1.0);
        }

        /// <summary>
        /// Test for KrovakToWgs84_EsriString_2
        /// </summary>
        [TestMethod()]
        public void KrovakToWgs84_EsriString_2()
        {
            double[] myOut = new double[2];
            myOut[0] = -651782.34;
            myOut[1] = -978039.99;
            double[] myZ = new double[1];
            myZ[0] = 0;

            string jtskEsriString = @"PROJCS[""S-JTSK_Krovak_East_North"",GEOGCS[""GCS_S_JTSK"",DATUM[""D_S_JTSK"",SPHEROID[""Bessel_1841"",6377397.155,299.1528128]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]],PROJECTION[""Krovak""],PARAMETER[""False_Easting"",0.0],PARAMETER[""False_Northing"",0.0],PARAMETER[""Pseudo_Standard_Parallel_1"",78.5],PARAMETER[""Scale_Factor"",0.9999],PARAMETER[""Azimuth"",30.28813975277778],PARAMETER[""Longitude_Of_Center"",24.83333333333333],PARAMETER[""Latitude_Of_Center"",49.5],PARAMETER[""X_Scale"",-1.0],PARAMETER[""Y_Scale"",1.0],PARAMETER[""XY_Plane_Rotation"",90.0],UNIT[""Meter"",1.0]]";

            ProjectionInfo projWGS84 = KnownCoordinateSystems.Geographic.World.WGS1984;

            ProjectionInfo myJTSKPI = ProjectionInfo.FromEsriString(jtskEsriString);

            Reproject.ReprojectPoints(myOut, myZ, myJTSKPI, projWGS84, 0, myZ.Length);

            Assert.IsTrue(myOut[1] < 52);
            //Assert.AreEqual(myOut[0], 12.8069888666667, 1.0);
            //Assert.AreEqual(myOut[1], 49.4522626972222, 1.0);
        }
    }
}
