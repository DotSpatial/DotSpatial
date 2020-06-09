// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DotSpatial.Projections;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    /// This contains all the Map based tests.
    /// </summary>
    [TestFixture]
    public class MapTest
    {
        /// <summary>
        /// A test for ZoomToMaxExtent.
        /// </summary>
        [Test]
        public void ZoomToMaxExtentTest()
        {
            XmlDeserializer target = new XmlDeserializer();
            Map map = new Map();
            string path = Path.Combine("TestFiles", "testproject1.dspx");

            target.Deserialize(map, File.ReadAllText(path));

            map.ZoomToMaxExtent();
        }

        /// <summary>
        /// A test to find out whether the default projection of a new map is WGS84.
        /// </summary>
        [Test]
        public void DefaultProjectionIsWgs84Test()
        {
            Map map = new Map();
            Assert.IsNotNull(map.Projection);
            Assert.AreEqual(map.Projection, KnownCoordinateSystems.Geographic.World.WGS1984);
        }

        /// <summary>
        /// A test to find out if the ProjectionChanged() event fires when the ProjectionEsriString
        /// property of the map is changed.
        /// </summary>
        [Test]
        public void ProjectionChangedEventFireTest()
        {
            bool eventIsFired = false;

            Map map = new Map();
            map.ProjectionChanged += (sender, args) => eventIsFired = true;

            const string Esri = "GEOGCS[\"GCS_North_American_1983\",DATUM[\"D_North_American_1983\",SPHEROID[\"GRS_1980\",6378137,298.257222101004]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            map.ProjectionEsriString = Esri;

            Assert.IsTrue(eventIsFired, "the ProjectionChanged event should be fired when Map.ProjectionEsriString is changed.");
        }

        /// <summary>
        /// Test if the new GetAllLayers() method returns the correct number of layers if the map has groups.
        /// </summary>
        [Test]
        public void GetAllLayersTest()
        {
            var map = new Map();
            var group = new MapGroup();
            map.Layers.Add(group);
            group.Layers.Add(new MapPolygonLayer());
            group.Layers.Add(new MapLineLayer());
            group.Layers.Add(new MapPointLayer());

            // add a nested group
            var group2 = new MapGroup();
            group.Layers.Add(group2);
            group2.Layers.Add(new MapPointLayer());
            group2.Layers.Add(new MapLineLayer());
            group2.Layers.Add(new MapPolygonLayer());

            List<ILayer> layerList = map.GetAllLayers();
            Assert.AreEqual(layerList.Count, 6);
        }

        /// <summary>
        /// Test if the new GetAllGroups() method returns the correct number of layers if the map has groups.
        /// </summary>
        [Test]
        public void GetAllGroupsTest()
        {
            var map = new Map();
            var group = new MapGroup();
            map.Layers.Add(group);
            group.Layers.Add(new MapPolygonLayer());
            group.Layers.Add(new MapLineLayer());
            group.Layers.Add(new MapPointLayer());

            // add a nested group
            var group2 = new MapGroup();
            group.Layers.Add(group2);
            group2.Layers.Add(new MapPointLayer());
            group2.Layers.Add(new MapLineLayer());
            group2.Layers.Add(new MapPolygonLayer());

            List<IGroup> groupList = map.GetAllGroups();
            Assert.AreEqual(groupList.Count, 2);
        }

        /// <summary>
        /// Check whether Maps select functions honor the SelectionEnabled properties.
        /// </summary>
        [Test(Description = "Check whether Maps select functions honor the SelectionEnabled properties.")]
        public void SelectionTest()
        {
            IMap map = new Map();

            // load layer with us states
            IMapLayer ml = map.AddLayer(Path.Combine(@"Testfiles", "50mil_us_states.shp"));
            IFeatureLayer fl = ml as IFeatureLayer;
            Assert.IsNotNull(fl);

            // add two categories for testing category.SelectionEnabled
            PolygonScheme scheme = new PolygonScheme();
            scheme.ClearCategories();
            scheme.AddCategory(new PolygonCategory(Color.LightBlue, Color.DarkBlue, 1)
            {
                FilterExpression = "[FIPS] >= 10",
                LegendText = ">= 10"
            });
            var cat = new PolygonCategory(Color.Pink, Color.DarkRed, 1)
            {
                FilterExpression = "[FIPS] < 10",
                LegendText = "< 10"
            };
            scheme.AddCategory(cat);
            fl.Symbology = scheme;
            Assert.IsTrue(cat.SelectionEnabled, "Categories must be initialized with SelectionEnabled = true.");

            // load the second layer for testing the layers SelectionEnabled property
            IMapLayer ml2 = map.AddLayer(Path.Combine(@"Testfiles", "50m_admin_0_countries.shp"));
            ml2.SelectionEnabled = false;
            IFeatureLayer fl2 = ml2 as IFeatureLayer;
            Assert.IsNotNull(fl2);

            // select the first area
            Envelope e = new Envelope(-72, -66, 40, 48);
            Assert.IsTrue(map.Select(e, e, ClearStates.False));
            Assert.AreEqual(7, fl.Selection.Count, "Error selecting 50mil_us_states");
            Assert.AreEqual(0, fl2.Selection.Count, "Error selecting 50m_admin_0_countries");

            // invert a slighly larger area, ignoring the features of the second category
            cat.SelectionEnabled = false;
            Envelope e2 = new Envelope(-78, -66, 40, 48);
            Assert.IsTrue(map.InvertSelection(e2, e2));
            Assert.AreEqual(4, fl.Selection.Count, "Error inverting selection 50mil_us_states");
            Assert.AreEqual(0, fl2.Selection.Count, "Error inverting selection 50m_admin_0_countries");

            // add another area allowing the second layer to be selected too
            ml2.SelectionEnabled = true;
            Envelope e3 = new Envelope(-89, -77, 24, 33);
            Assert.IsTrue(map.Select(e3, e3, ClearStates.False));
            Assert.AreEqual(9, fl.Selection.Count, "Error adding to selection 50mil_us_states");
            Assert.AreEqual(2, fl2.Selection.Count, "Error adding to selection 50m_admin_0_countries");
            ml2.SelectionEnabled = false;

            // unselect the whole area ignoring the second layer and the deactivated category
            Envelope e4 = new Envelope(-89, -66, 24, 48);
            Assert.IsTrue(map.UnSelect(e4, e4));
            Assert.AreEqual(1, fl.Selection.Count, "Error unselecting 50mil_us_states");
            Assert.AreEqual(2, fl2.Selection.Count, "Error unselecting 50m_admin_0_countries");
        }
    }
}