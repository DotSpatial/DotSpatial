using DotSpatial.Projections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotSpatial.Projection
{
    /// <summary>
    ///This is a test class for ProjectionInfoTest and is intended
    ///to contain all ProjectionInfoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProjectionInfoTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        //
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //

        #endregion Additional test attributes

        /// <summary>
        /// Proj4 string esri comparison test.
        /// </summary>
        [TestMethod()]
        public void Proj4EsriComparisonTest()
        {
            ProjectionInfo infoBuiltIn = KnownCoordinateSystems.Geographic.World.WGS1984;

            const string esri = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223562997]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            ProjectionInfo infoFromEsri = ProjectionInfo.FromEsriString(esri);

            string expected = infoFromEsri.ToProj4String();
            string actual = infoBuiltIn.ToProj4String();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToProj4String
        ///</summary>
        [TestMethod()]
        public void ToProj4StringTestDoesReadMatchToString()
        {
            string proj4String = "+proj=longlat +ellps=WGS84 +no_defs ";

            ProjectionInfo target = ProjectionInfo.FromProj4String(proj4String);

            Assert.AreEqual(proj4String.Trim(), target.ToProj4String().Trim());
        }

        /// <summary>
        ///A test for http://dotspatial.codeplex.com/workitem/188 WGS1984
        ///</summary>
        [TestMethod()]
        public void ToEsriStringWGS1984Test()
        {
            ProjectionInfo p1 = KnownCoordinateSystems.Geographic.World.WGS1984;

            const string esri = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223562997]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            ProjectionInfo p2 = ProjectionInfo.FromEsriString(esri);

            string expected = p2.ToEsriString();
            string actual = p1.ToEsriString();

            Assert.AreEqual(expected, actual);

            Assert.IsTrue(p1.Equals(p2));
        }

        /// <summary>
        ///A test for ProjectionInfo Constructor
        ///</summary>
        [TestMethod()]
        public void ProjectionInfoConstructorTest()
        {
            string proj4String = string.Empty;
            ProjectionInfo actual = ProjectionInfo.FromProj4String(proj4String);
            ProjectionInfo expected = new ProjectionInfo();

            Assert.AreEqual(expected.ToProj4String(), actual.ToProj4String());
        }

        /// <summary>
        ///A test for ProjectionInfo Constructor
        ///</summary>
        [TestMethod()]
        public void ProjectionInfoConstructorTest2()
        {
            string proj4String = null;
            ProjectionInfo actual = ProjectionInfo.FromProj4String(proj4String);
            ProjectionInfo expected = new ProjectionInfo();

            Assert.AreEqual(expected.ToProj4String(), actual.ToProj4String());
        }

        /// <summary>
        ///A test for ProjectionInfo Constructor
        ///</summary>
        [TestMethod()]
        public void ProjectionInfoConstructorTest1()
        {
            string proj4String = "+proj=longlat +ellps=WGS84 +no_defs ";
            ProjectionInfo expected = ProjectionInfo.FromProj4String(proj4String);

            ProjectionInfo actual = new ProjectionInfo();
            actual.GeographicInfo.Datum.Spheroid = new Spheroid("WGS84");
            actual.NoDefs = true;
            actual.IsLatLon = true;

            Assert.AreEqual(expected.ToProj4String(), actual.ToProj4String());
        }

        /// <summary>
        ///A test for ToEsriString NorthAmericanDatum1983
        ///</summary>
        [TestMethod()]
        public void ToEsriStringNorthAmericanDatum1983Test()
        {
            ProjectionInfo p1 = KnownCoordinateSystems.Geographic.NorthAmerica.NorthAmericanDatum1983;

            const string esri = "GEOGCS[\"GCS_North_American_1983\",DATUM[\"D_North_American_1983\",SPHEROID[\"GRS_1980\",6378137,298.257222101004]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            ProjectionInfo p2 = ProjectionInfo.FromEsriString(esri);

            string expected = p2.ToEsriString();
            string actual = p1.ToEsriString();

            Assert.AreEqual(expected, actual);

            Assert.IsTrue(p1.Equals(p2));
        }
    }
}