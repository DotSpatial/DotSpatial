using System.Globalization;
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
            string filename = Path.Combine("TestFiles", "soils.shp");
            string fileOut = Path.Combine("TestFiles", "soilsExport.shp");

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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("cs-CZ");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("cs-CZ");

            string filename = Path.Combine("TestFiles", "soils.shp");
            string fileOut = Path.Combine("TestFiles", "soilsExport.shp");

            ShapefileLayerProvider provider = new ShapefileLayerProvider();
            var target = (FeatureLayer)provider.OpenLayer(filename, false, null, null);
            target.SelectByAttribute("[BPEJ_K_S42]>7710");

            Assert.IsTrue(target.Selection.Count > 0);

            target.ExportSelection(fileOut);

            File.Delete(fileOut);
        }
    }
}
