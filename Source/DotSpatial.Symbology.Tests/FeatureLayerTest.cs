// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using DotSpatial.Controls;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// This is a test class for FeatureLayerTest and is intended
    /// to contain all FeatureLayerTest Unit Tests.
    /// </summary>
    [TestFixture]
    internal class FeatureLayerTest
    {
        #region Methods

        /// <summary>
        /// A test for ExportSelection.
        /// </summary>
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

        /// <summary>
        /// A test for ExportSelection http://dotspatial.codeplex.com/workitem/203.
        /// </summary>
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

        /// <summary>
        /// Tests whether FeatureLayer.Select honors the SelectionEnabled property.
        /// </summary>
        /// <param name="selectionEnabled">Indicates the state of the SelectionEnabled property.</param>
        /// <param name="catSelectionEnabled">Indicates the state of the categories SelectionEnabled property.</param>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Select(bool selectionEnabled, bool catSelectionEnabled)
        {
            var fl = GetFeatureLayer(out PolygonCategory cat);
            fl.SelectionEnabled = selectionEnabled;
            cat.SelectionEnabled = catSelectionEnabled;
            Envelope e = new Envelope(-72, -66, 40, 48);
            Assert.AreEqual(selectionEnabled, fl.Select(e, e, ClearStates.False));

            var resultValue = 7;

            if (!selectionEnabled)
            {
                resultValue = 0;
            }
            else if (!catSelectionEnabled)
            {
                resultValue = 6;
            }

            Assert.AreEqual(resultValue, fl.Selection.Count, "Error selecting 50mil_us_states");
        }

        /// <summary>
        /// Tests whether FeatureLayer.InvertSelection honors the SelectionEnabled property.
        /// </summary>
        /// <param name="selectionEnabled">Indicates the state of the SelectionEnabled property.</param>
        /// <param name="catSelectionEnabled">Indicates the state of the categories SelectionEnabled property.</param>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void InvertSelection(bool selectionEnabled, bool catSelectionEnabled)
        {
            var fl = GetFeatureLayer(out PolygonCategory cat);
            Envelope e = new Envelope(-72, -66, 40, 48);
            Assert.IsTrue(fl.Select(e, e, ClearStates.False));

            fl.SelectionEnabled = selectionEnabled;
            cat.SelectionEnabled = catSelectionEnabled;

            Envelope e2 = new Envelope(-78, -66, 40, 48);
            Assert.AreEqual(selectionEnabled, fl.InvertSelection(e2, e2));

            var resultValue = 3;

            if (!selectionEnabled)
            {
                resultValue = 7;
            }
            else if (!catSelectionEnabled)
            {
                resultValue = 4;
            }

            Assert.AreEqual(resultValue, fl.Selection.Count, "Error inverting selection 50mil_us_states");
        }

        /// <summary>
        /// Tests whether FeatureLayer.UnSelect honors the SelectionEnabled property.
        /// </summary>
        /// <param name="selectionEnabled">Indicates the state of the SelectionEnabled property.</param>
        /// <param name="catSelectionEnabled">Indicates the state of the categories SelectionEnabled property.</param>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void UnSelect(bool selectionEnabled, bool catSelectionEnabled)
        {
            var fl = GetFeatureLayer(out PolygonCategory cat);
            Envelope e = new Envelope(-72, -66, 40, 48);
            Assert.IsTrue(fl.Select(e, e, ClearStates.False));

            fl.SelectionEnabled = selectionEnabled;
            cat.SelectionEnabled = catSelectionEnabled;

            Envelope e2 = new Envelope(-78, -66, 40, 48);
            Assert.AreEqual(selectionEnabled, fl.UnSelect(e2, e2));

            var resultValue = 0;

            if (!selectionEnabled)
            {
                resultValue = 7;
            }
            else if (!catSelectionEnabled)
            {
                resultValue = 1;
            }

            Assert.AreEqual(resultValue, fl.Selection.Count, "Error inverting selection 50mil_us_states");
        }

        /// <summary>
        /// Tests whether FeatureLayer.SelectAll honors the SelectionEnabled property.
        /// </summary>
        /// <param name="selectionEnabled">Indicates the state of the SelectionEnabled property.</param>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SelectAll(bool selectionEnabled)
        {
            var fl = GetFeatureLayer(out _);
            fl.SelectionEnabled = selectionEnabled;
            fl.SelectAll();

            Assert.AreEqual(selectionEnabled ? fl.DataSet.ShapeIndices.Count : 0, fl.Selection.Count, "Error inverting selection 50mil_us_states");
        }

        /// <summary>
        /// Tests whether FeatureLayer.UnSelectAll honors the SelectionEnabled property.
        /// </summary>
        /// <param name="selectionEnabled">Indicates the state of the SelectionEnabled property.</param>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UnSelectAll(bool selectionEnabled)
        {
            var fl = GetFeatureLayer(out _);
            fl.SelectAll();
            fl.SelectionEnabled = selectionEnabled;
            fl.UnSelectAll();

            Assert.AreEqual(selectionEnabled ? 0 : fl.DataSet.ShapeIndices.Count, fl.Selection.Count, "Error inverting selection 50mil_us_states");
        }

        /// <summary>
        /// Gets the FeatureLayer that is used for the selection tests.
        /// </summary>
        /// <param name="cat">Reference to the second category.</param>
        /// <returns>The FeatureLayer.</returns>
        private static IFeatureLayer GetFeatureLayer(out PolygonCategory cat)
        {
            // load layer with us states
            ShapefileLayerProvider provider = new ShapefileLayerProvider();
            var fl = (IFeatureLayer)provider.OpenLayer(Path.Combine(@"TestFiles", "50mil_us_states.shp"), false, null, null);
            Assert.IsNotNull(fl);

            // add two categories for testing category.SelectionEnabled
            PolygonScheme scheme = new PolygonScheme();
            scheme.ClearCategories();
            scheme.AddCategory(new PolygonCategory(Color.LightBlue, Color.DarkBlue, 1)
            {
                FilterExpression = "[FIPS] >= 10",
                LegendText = ">= 10"
            });
            cat = new PolygonCategory(Color.Pink, Color.DarkRed, 1)
            {
                FilterExpression = "[FIPS] < 10",
                LegendText = "< 10"
            };
            scheme.AddCategory(cat);
            fl.Symbology = scheme;
            Assert.IsTrue(cat.SelectionEnabled, "Categories must be initialized with SelectionEnabled = true.");
            return fl;
        }

        #endregion
    }
}