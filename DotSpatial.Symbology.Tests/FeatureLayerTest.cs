using System;
using System.Data;
using System.IO;
using System.Threading;
using DotSpatial.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotSpatial.Symbology.Tests
{


    /// <summary>
    ///This is a test class for FeatureLayerTest and is intended
    ///to contain all FeatureLayerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FeatureLayerTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
        #endregion



        /// <summary>
        ///A test for ExportSelection
        ///</summary>
        [TestMethod()]
        public void ExportSelectionTest()
        {
            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "soils.shp");
            string fileOut = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "soilsExport.shp");

            ShapefileLayerProvider provider = new ShapefileLayerProvider();
            var target = (FeatureLayer)provider.OpenLayer(filename, false, null, null);
            target.SelectByAttribute("[BPEJ_K_S42]>7710");

            Assert.IsTrue(target.Selection.Count > 0);

            target.ExportSelection(fileOut);

            File.Delete(fileOut);
        }

        /// <summary>
        ///A test for ExportSelection http://dotspatial.codeplex.com/workitem/203
        ///</summary>
        [TestMethod()]
        public void ExportSelectionTestWithCulture()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("cs-CZ");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("cs-CZ");

            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "soils.shp");
            string fileOut = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "soilsExport.shp");

            ShapefileLayerProvider provider = new ShapefileLayerProvider();
            var target = (FeatureLayer)provider.OpenLayer(filename, false, null, null);
            target.SelectByAttribute("[BPEJ_K_S42]>7710");

            Assert.IsTrue(target.Selection.Count > 0);

            target.ExportSelection(fileOut);

            File.Delete(fileOut);
        }



        /// <summary>
        /// Attempts reading DBF with OLEDB.
        /// </summary>
        [TestMethod()]
        [Ignore]
        public void AttemptReadingDBFWithOLEDB()
        {
            //FeatureLayer target = new MapPolygonLayer();
            string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "soils.shp");
            string dBaseFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles");

            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source = " + dBaseFile + ";Extended Properties =dBase IV;";
            System.Data.OleDb.OleDbConnection dBaseConnection;
            dBaseConnection = new System.Data.OleDb.OleDbConnection(connectionString);
            dBaseConnection.Open();
            System.Data.OleDb.OleDbCommand dBaseCommand;
            dBaseCommand = new System.Data.OleDb.OleDbCommand("SELECT * FROM [soils]", dBaseConnection);
            System.Data.OleDb.OleDbDataReader dBaseDataReader;
            dBaseDataReader = dBaseCommand.ExecuteReader(CommandBehavior.SequentialAccess);
        }

    }
}
