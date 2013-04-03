using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Projections;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;

namespace DotSpatial.Projection.Tests
{
    [TestClass()]
    public class KrovakTest
    {
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
            DotSpatial.Projections.ProjectionInfo myJTSKPI = DotSpatial.Projections.ProjectionInfo.FromProj4String(projJTSK2);

            DotSpatial.Projections.Reproject.ReprojectPoints(myOut, myZ, source, myJTSKPI, 0, myZ.Length);

            Assert.AreEqual(myOut[0], -868208.52, 1.0);
            Assert.AreEqual(myOut[1], -1095793.96, 1.0);       
        }

        [TestMethod()]
        public void Wgs84ToKrovak_KnownCoordSys()
        {
            double[] myOut = new double[2];
            myOut[0] = 12.8069888666667;
            myOut[1] = 49.4522626972222;
            double[] myZ = new double[1];
            myZ[0] = 0;

            ProjectionInfo source = KnownCoordinateSystems.Geographic.World.WGS1984;
            DotSpatial.Projections.ProjectionInfo myJTSKPI = KnownCoordinateSystems.Projected.NationalGrids.SJTSKKrovakEastNorth;

            DotSpatial.Projections.Reproject.ReprojectPoints(myOut, myZ, source, myJTSKPI, 0, myZ.Length);

            Assert.AreEqual(myOut[0], -868208.52, 1.0);
            Assert.AreEqual(myOut[1], -1095793.96, 1.0);          
        }

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
            DotSpatial.Projections.ProjectionInfo myJTSKPI = DotSpatial.Projections.ProjectionInfo.FromProj4String(projJTSK2);

            DotSpatial.Projections.Reproject.ReprojectPoints(myOut, myZ, myJTSKPI, projWGS84, 0, myZ.Length);

            Assert.AreEqual(myOut[0], 12.8069888666667, 1.0);
            Assert.AreEqual(myOut[1], 49.4522626972222, 1.0);
        }

        [TestMethod()]
        public void KrovakToWgs84_KnownCoordSys()
        {
            double[] myOut = new double[2];
            myOut[0] = -868208.52;
            myOut[1] = -1095793.96;      
            double[] myZ = new double[1];
            myZ[0] = 0;

            ProjectionInfo projWGS84 = KnownCoordinateSystems.Geographic.World.WGS1984;
            string projJTSK2 = "+proj=krovak +lat_0=49.5 +lon_0=24.83333333333333 +alpha=0 +k=0.9999 +x_0=0 +y_0=0 +ellps=bessel +towgs84=570.8,85.7,462.8,4.998,1.587,5.261,3.56 +units=m +no_defs";
            DotSpatial.Projections.ProjectionInfo myJTSKPI = DotSpatial.Projections.KnownCoordinateSystems.Projected.NationalGrids.SJTSKKrovakEastNorth;

            DotSpatial.Projections.Reproject.ReprojectPoints(myOut, myZ, myJTSKPI, projWGS84, 0, myZ.Length);

            Assert.AreEqual(myOut[0], 12.8069888666667, 1.0);
            Assert.AreEqual(myOut[1], 49.4522626972222, 1.0);
        }

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
            
            DotSpatial.Projections.ProjectionInfo myJTSKPI = DotSpatial.Projections.ProjectionInfo.FromEsriString(jtskEsriString);

            DotSpatial.Projections.Reproject.ReprojectPoints(myOut, myZ, myJTSKPI, projWGS84, 0, myZ.Length);

            Assert.AreEqual(myOut[0], 12.8069888666667, 1.0);
            Assert.AreEqual(myOut[1], 49.4522626972222, 1.0);
        }

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

            DotSpatial.Projections.ProjectionInfo myJTSKPI = DotSpatial.Projections.ProjectionInfo.FromEsriString(jtskEsriString);

            DotSpatial.Projections.Reproject.ReprojectPoints(myOut, myZ, myJTSKPI, projWGS84, 0, myZ.Length);

            Assert.IsTrue(myOut[1] < 52);
            //Assert.AreEqual(myOut[0], 12.8069888666667, 1.0);
            //Assert.AreEqual(myOut[1], 49.4522626972222, 1.0);
        }

        [TestMethod()]
        public void wgs84ToKrovak_DOPNUL()
        {
            // Uses the official DOPNUL geodetic points from:
            // http://www.geospeleos.com/Mapovani/WGS84toSJTSK/WGS_JTSK.pdf
            // http://geo2.fsv.cvut.cz/~soukup/dip/jezek/kap6.html


        }
    }
}
