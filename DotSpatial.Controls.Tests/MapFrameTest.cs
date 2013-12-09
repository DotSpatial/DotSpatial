using System;
using System.IO;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;
using DotSpatial.Serialization;
using DotSpatial.Projections;
using System.Collections.Generic;
using DotSpatial.Symbology;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    ///This is a test class for MapFrame and is intended
    ///to contain all MapFrame Unit Tests
    ///</summary>
    [TestClass()]
    public class MapFrameTest
    {

        /// <summary>
        /// Tests whether the MapFrame_ExtentsChanged event fires
        /// after re-opening a project
        /// </summary>
        [TestMethod()]
        public void MapFrameExtentsChangedEvent_OpeningProjectTest()
        {
            string shapeFilePath = Path.Combine("TestFiles", "50mil_us_states.shp");
            
            Map myMap = new Map();
            AppManager manager = new AppManager();
            manager.Map = myMap;

            bool eventIsFired = false;

            //setup the event handler
            myMap.MapFrame.ViewExtentsChanged += delegate(object sender, DotSpatial.Data.ExtentArgs e)
            {
                eventIsFired = true;
            };

            //add a layer to map
            IMapLayer myLayer = myMap.Layers.Add(shapeFilePath);

            //test event fired first time
            ((IMapPolygonLayer)myLayer).SelectByAttribute("NAME = 'California'");
            ((IMapPolygonLayer)myLayer).ZoomToSelectedFeatures();
            Assert.IsTrue(eventIsFired, "ViewExtentsChanged event doesn't fire after layer is added.");
            eventIsFired = false;

            //open a project
            string dspxPath = Path.GetFullPath(Path.Combine("TestFiles", "testproject1.dspx"));
            manager.SerializationManager.OpenProject(dspxPath);

            //change the extent again after opening the project
            Assert.IsTrue((myMap.Layers.Count > 0), "The map should have 1 or more layers after opening the deserializeTest project.");
            myMap.ViewExtents = new DotSpatial.Data.Extent(15, 48, 20, 52);

            Assert.IsTrue(eventIsFired, "ViewExtentsChanged event doesn't fire after opening the project.");
        }


        /// <summary>
        /// Test if the new GetAllLayers() method returns the correct number of layers if the map has groups
        /// </summary>
        [TestMethod()]
        public void GetAllLayersTest()
        {
            var map = new MapFrame();
            var group = new MapGroup();
            map.Layers.Add(group);
            group.Layers.Add(new MapPolygonLayer());
            group.Layers.Add(new MapLineLayer());
            group.Layers.Add(new MapPointLayer());

            //add a nested group
            var group2 = new MapGroup();
            group.Layers.Add(group2);
            group2.Layers.Add(new MapPointLayer());
            group2.Layers.Add(new MapLineLayer());
            group2.Layers.Add(new MapPolygonLayer());

            List<ILayer> layerList = map.GetAllLayers();
            Assert.AreEqual(layerList.Count, 6);
        }

        /// <summary>
        /// Test if the new GetAllLayers() method returns the correct number of layers if the map has groups
        /// </summary>
        [TestMethod()]
        public void GetAllGroupsTest()
        {
            var map = new MapFrame();
            var group = new MapGroup();
            map.Layers.Add(group);
            group.Layers.Add(new MapPolygonLayer());
            group.Layers.Add(new MapLineLayer());
            group.Layers.Add(new MapPointLayer());

            //add a nested group
            var group2 = new MapGroup();
            group.Layers.Add(group2);
            group2.Layers.Add(new MapPointLayer());
            group2.Layers.Add(new MapLineLayer());
            group2.Layers.Add(new MapPolygonLayer());

            List<IMapGroup> groupList = map.GetAllGroups();
            Assert.AreEqual(groupList.Count, 2);
        }
    }
}