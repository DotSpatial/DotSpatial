using System;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;
using System.IO;
using DotSpatial.Serialization;
using DotSpatial.Projections;
using System.Collections.Generic;
using DotSpatial.Symbology;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    ///This is a test class for MapTest and is intended
    ///to contain all MapTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MapTest
    {
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
        public void MapExtentsChangedEvent_OpeningProjectTest()
        {
            string shapeFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "50mil_us_states.shp");
            
            Map myMap = new Map();
            AppManager manager = new AppManager();
            manager.Map = myMap;

            bool eventIsFired = false;

            //setup the event handler
            myMap.ViewExtentsChanged += delegate(object sender, DotSpatial.Data.ExtentArgs e)
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
        ///A test for ZoomToMaxExtent
        ///</summary>
        [TestMethod()]
        public void ZoomToMaxExtentTest()
        {
            XmlDeserializer target = new XmlDeserializer();
            DotSpatial.Controls.Map map = new DotSpatial.Controls.Map();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "testproject1.dspx");

            target.Deserialize(map, File.ReadAllText(path));

            map.ZoomToMaxExtent();
        }

        /// <summary>
        /// A test to find out whether the default projection of a new map is WGS84.
        /// </summary>
        [TestMethod()]
        public void DefaultProjectionIsWgs84Test()
        {
            Map map = new Map();
            Assert.IsNotNull(map.Projection);
            Assert.AreEqual(map.Projection, DotSpatial.Projections.KnownCoordinateSystems.Geographic.World.WGS1984);
        }

        /// <summary>
        /// A test to find out if the ProjectionChanged() event fires when the ProjectionEsriString
        /// property of the map is changed.
        /// </summary>
        [TestMethod()]
        public void ProjectionChangedEventFireTest()
        {
            bool eventIsFired = false;
            
            Map map = new Map();
            map.ProjectionChanged += delegate(object sender, EventArgs e)
            {
                eventIsFired = true;
            };

            const string esri = "GEOGCS[\"GCS_North_American_1983\",DATUM[\"D_North_American_1983\",SPHEROID[\"GRS_1980\",6378137,298.257222101004]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            map.ProjectionEsriString = esri;

            Assert.IsTrue(eventIsFired, "the ProjectionChanged event should be fired when Map.ProjectionEsriString is changed.");
        }

        /// <summary>
        /// Test if the new GetAllLayers() method returns the correct number of layers if the map has groups
        /// </summary>
        [TestMethod()]
        public void GetAllLayersTest()
        {
            var map = new Map();
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
            var map = new Map();
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