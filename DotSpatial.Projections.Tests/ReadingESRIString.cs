//using System;
//using System.Text;
//using System.Collections.Generic;
//using System.Linq;
//using NUnit.Framework;
//using DotSpatial.Projections;
//using DotSpatial.Data;
//using DotSpatial.Topology;
//using System.IO;




//namespace DotSpatial.Projections.Tests
//{
//    /// <summary>
//    /// Summary description for ReadingESRIString
//    /// </summary>
//    [TestFixture]
//    public class ReadingESRIString
//    {
//        public ReadingESRIString()
//        {
//            //
//            // TODO: Add constructor logic here
//            //
//        }

//        private TestContext testContextInstance;

//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region Additional test attributes
//        //
//        // You can use the following additional attributes as you write your tests:
//        //
//        // Use ClassInitialize to run code before running the first test in the class
//        // [ClassInitialize()]
//        // public static void MyClassInitialize(TestContext testContext) { }
//        //
//        // Use ClassCleanup to run code after all tests in a class have run
//        // [ClassCleanup()]
//        // public static void MyClassCleanup() { }
//        //
//        // Use TestInitialize to run code before running each test 
//        // [TestInitialize()]
//        // public void MyTestInitialize() { }
//        //
//        // Use TestCleanup to run code after each test has run
//        // [TestCleanup()]
//        // public void MyTestCleanup() { }
//        //
//        #endregion

//        [Test]
//        public void ReadingESRIPrj()
//        {
//            FeatureSet fs = new FeatureSet();
//            fs.Open("C:\\Temp\\counties.shp");
//            FeatureSet fscheck = new FeatureSet();
//            fscheck.Open("C:\\Temp\\counties_nospatialreference.shp");
//            ProjectionInfo pESRI = new ProjectionInfo();
//            StreamReader re = File.OpenText("C:\\Program Files\\ArcGIS\\Coordinate Systems\\Geographic Coordinate Systems\\World\\WGS 1984.prj");
//            //pESRI.ReadEsriString(re.ReadLine());
//            //fscheck.Projection.GeographicInfo.ReadEsriString(re.ReadLine());
//            fscheck.Projection.ReadEsriString(re.ReadLine());
//            fscheck.SaveAs("C:\\Temp\\projection_check.shp", true);
            
//            Assert.AreEqual(fs.Projection.GeographicInfo.Name, fscheck.Projection.GeographicInfo.Name);
//            for (int i = 0; i < fscheck.Features.Count; i++)
//            {
//                for (int t = 0; t < fscheck.Features[i].Coordinates.Count; t++)
//                {
//                    Assert.AreEqual(fs.Features[i].Coordinates[t].X, fscheck.Features[i].Coordinates[t].X);
//                    Assert.AreEqual(fs.Features[i].Coordinates[t].Y, fscheck.Features[i].Coordinates[t].Y);
//                }
//            }
//        }
      


//        [Test]
//        public void CompairingKnowCoordianteSystem()
//        {
//            FeatureSet fs = new FeatureSet();
//            fs.Open("C:\\Temp\\counties.shp");
//            FeatureSet fscheck = new FeatureSet();
//            fscheck.Open("C:\\Temp\\counties_nospatialreference.shp");
//            fscheck.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
//            fscheck.SaveAs("C:\\Temp\\projection_check.shp", true);
//            Assert.AreEqual(fs.Projection.Name, fscheck.Projection.Name);
//            Assert.AreEqual(fs.Projection.GeographicInfo.Name, fscheck.Projection.GeographicInfo.Name);
//            for (int i = 0; i < fscheck.Features.Count; i++)
//            {
//                for (int t = 0; t < fscheck.Features[i].Coordinates.Count; t++)
//                {
//                    Assert.AreEqual(fs.Features[i].Coordinates[t].X, fscheck.Features[i].Coordinates[t].X);
//                    Assert.AreEqual(fs.Features[i].Coordinates[t].Y, fscheck.Features[i].Coordinates[t].Y);
//                }
//            }
//        }

//        [Test]
//        public void ESRIWebMecatorTest()
//        {
//            FeatureSet fs = new FeatureSet();
//            fs.Open("C:\\Temp\\counties_nospatialreference.shp");
//            fs.SaveAs("C:\\Temp\\test_MapWinow.shp", true);
//            fs.SaveAs("C:\\Temp\\test_ESRI.shp", true);

//            FeatureSet fscheck = new FeatureSet();
//            fscheck.Open("C:\\Temp\\test_MapWinow.shp");
//            FeatureSet fscheckESRI = new FeatureSet();
//            fscheckESRI.Open("C:\\Temp\\test_ESRI.shp");
//            ProjectionInfo pend = KnownCoordinateSystems.Projected.World.WebMercator;
//            ProjectionInfo pESRIend = new ProjectionInfo();
//            StreamReader re = File.OpenText("C:\\Program Files\\ArcGIS\\Coordinate Systems\\Projected Coordinate Systems\\World\\Mercator (world).prj");
//            pESRIend.ReadEsriString(re.ReadLine());
//            fscheckESRI.Reproject(pend);
//            fscheck.Reproject(pend);
            
//            for (int i = 0; i < fscheck.Features.Count; i++)
//            {
//                for (int t = 0; t < fscheck.Features[i].Coordinates.Count; t++)
//                {
//                    Assert.AreEqual(fscheck.Features[i].Coordinates[t].X, fscheckESRI.Features[i].Coordinates[t].X);
//                    Assert.AreEqual(fscheck.Features[i].Coordinates[t].Y, fscheckESRI.Features[i].Coordinates[t].Y);
//                }
//            }
            
//        }

//        [Test]
//        public void ESRIWebMecatorTestPartII()
//        {
//            FeatureSet fs = new FeatureSet();
//            fs.Open("C:\\Temp\\counties.shp");
//            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.World.WGS1984;
//            fs.SaveAs("C:\\Temp\\test_MapWinow.shp", true);
//            fs.SaveAs("C:\\Temp\\test_ESRI.shp", true);

//            FeatureSet fscheck = new FeatureSet();
//            fscheck.Open("C:\\Temp\\test_MapWinow.shp");
//            FeatureSet fscheckESRI = new FeatureSet();
//            fscheckESRI.Open("C:\\Temp\\test_ESRI.shp");
//            ProjectionInfo pend = KnownCoordinateSystems.Projected.World.WebMercator;
//            ProjectionInfo pESRIend = new ProjectionInfo();
//            StreamReader re = File.OpenText("C:\\Program Files\\ArcGIS\\Coordinate Systems\\Geographic Coordinate Systems\\World\\WGS 1984.prj");
//            pESRIend.ReadEsriString(re.ReadLine());

//            for (int i = 0; i < fscheck.Features.Count; i++)
//            {
//                for (int t = 0; t < fscheck.Features[i].Coordinates.Count; t++)
//                {
//                    double[] xy = new double[2];
//                    double[] z = new double[1];
//                    double[] xycheck = new double[2];
//                    double[] zcheck = new double[1];

//                    //xy[1] = 0;
//                    //xy[0] = 0;
//                    //z[0] = 0;

//                    xy[0] = fscheck.Features[i].Coordinates[t].X;
//                    xy[1] = fscheck.Features[i].Coordinates[t].Y;
//                    z[0] = 0;
//                    Reproject.ReprojectPoints(xy, z, pStart, pend, 0, 1);

//                    xycheck[0] = fscheck.Features[i].Coordinates[t].X;
//                    xycheck[1] = fscheck.Features[i].Coordinates[t].Y;
//                    zcheck[0] = 0;
//                    Reproject.ReprojectPoints(xycheck, zcheck, pESRIend, pend, 0, 1);

//                    if (Math.Abs(xycheck[0] - xy[0]) > 0.00000001)
//                    {
//                        Assert.Fail("The longitude was off by " + (xycheck[0] - xy[0]) + " meters from the ESRI String");
//                    }
//                    if (Math.Abs(xycheck[1] - xy[1]) > 0.00000001)
//                    {
//                        Assert.Fail("The latitude was off by " + (xycheck[1] - xy[1]) + " meters from teh ESRI String");
//                    }

//                }

//            }

            

//        }



//    }
//}
