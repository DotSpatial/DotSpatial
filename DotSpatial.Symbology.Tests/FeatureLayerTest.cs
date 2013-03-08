using System;
using System.Data;
using System.IO;
using System.Threading;
using DotSpatial.Controls;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{

    /// <summary>
    ///This is a test class for FeatureLayerTest and is intended
    ///to contain all FeatureLayerTest Unit Tests
    ///</summary>
    [TestFixture]
    class FeatureLayerTest
    {
        /// <summary>
        ///A test for ExportSelection
        ///</summary>
        [Test]
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
        [Test]
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
        [Test]
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
