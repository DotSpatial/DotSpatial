using System;
using System.Data;
using System.IO;
using System.Threading;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Tests.Common;
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
            string filename = FileTools.PathToTestFile(@"Shapefiles\Soils\soils.shp");
            string fileOut = FileTools.GetTempFileName(".shp");
            try
            {
                var provider = new ShapefileLayerProvider();
                var target = (FeatureLayer)provider.OpenLayer(filename, false, null, null);
                target.SelectByAttribute("[BPEJ_K_S42]>7710");

                Assert.IsTrue(target.Selection.Count > 0);
                target.ExportSelection(fileOut);

            }
            finally
            {
                FileTools.DeleteShapeFile(fileOut);
            }
        }
     
        [Test]
        public void ExportSelectionTestWithCulture()
        {
            var currentCultute = Thread.CurrentThread.CurrentCulture;
            var currentUICultute = Thread.CurrentThread.CurrentUICulture;
            string filename = FileTools.PathToTestFile(@"Shapefiles\Soils\soils.shp");
            string fileOut = FileTools.GetTempFileName(".shp");
            try
            {
                Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("cs-CZ");
                var provider = new ShapefileLayerProvider();
                var target = (FeatureLayer)provider.OpenLayer(filename, false, null, null);
                target.SelectByAttribute("[BPEJ_K_S42]>7710");
                Assert.IsTrue(target.Selection.Count > 0);
                target.ExportSelection(fileOut);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = currentCultute;
                Thread.CurrentThread.CurrentUICulture = currentUICultute;
                FileTools.DeleteShapeFile(fileOut);   
            }
            
        }
    }
}
