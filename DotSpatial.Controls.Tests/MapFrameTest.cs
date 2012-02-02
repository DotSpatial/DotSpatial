using System;
using System.IO;
using DotSpatial.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        /// <summary>
        /// Tests whether the MapFrame_ExtentsChanged event fires
        /// after re-opening a project
        /// </summary>
        [TestMethod()]
        public void MapFrameExtentsChangedEvent_OpeningProjectTest()
        {
            string shapeFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "50mil_us_states.shp");
            
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
            string dspxPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "testproject1.dspx");

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