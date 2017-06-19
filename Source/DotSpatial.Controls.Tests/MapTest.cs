// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using DotSpatial.Projections;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    /// This is a test class for MapTest and is intended
    /// to contain all MapTest Unit Tests
    /// </summary>
    [TestClass]
    public class MapTest
    {
        /// <summary>
        /// A test for ZoomToMaxExtent
        /// </summary>
        [TestMethod]
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
        [TestMethod]
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
        [TestMethod]
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
        /// Test if the new GetAllLayers() method returns the correct number of layers if the map has groups
        /// </summary>
        [TestMethod]
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
        /// Test if the new GetAllGroups() method returns the correct number of layers if the map has groups
        /// </summary>
        [TestMethod]
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
    }
}