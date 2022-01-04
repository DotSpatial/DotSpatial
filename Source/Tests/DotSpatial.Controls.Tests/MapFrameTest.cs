// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Symbology;
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    /// This is a test class for MapFrame and is intended to contain all MapFrame Unit Tests.
    /// </summary>
    [TestClass]
    public class MapFrameTest
    {
        /// <summary>
        /// Test if the new GetAllLayers() method returns the correct number of layers if the map has groups.
        /// </summary>
        [TestMethod]
        public void GetAllLayersTest()
        {
            var map = new MapFrame();
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
        /// Test if the new GetAllLayers() method returns the correct number of layers if the map has groups.
        /// </summary>
        [TestMethod]
        public void GetAllGroupsTest()
        {
            var map = new MapFrame();
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