using System;
using System.IO;
using DotSpatial.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotSpatial.Data;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    ///This is a test class for MapTest and is intended
    ///to contain all MapTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MapLayerCollectionTest
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
        ///A test for SelectedLayer.
        ///After the selected layer is removed, Layers.SelectedLayer should be null
        ///</summary>
        [TestMethod()]
        public void SelectedLayerNullIfLayerRemoved()
        {
            DotSpatial.Controls.Map map = new DotSpatial.Controls.Map();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test-randomPts.shp");
            IMapLayer myLayer = map.AddLayer(path);

            Assert.IsNotNull(myLayer, "the added map layer should not be null.");

            myLayer.IsSelected = true;
            Assert.AreEqual(map.Layers.SelectedLayer, myLayer, "the selected layer should be equal to myLayer.");

            //now remove all layers
            map.Layers.Clear();

            //selectedLayer should be null
            Assert.IsNull(map.Layers.SelectedLayer, "SelectedLayer should be null after removing all layers");
        }

        /// <summary>
        ///A test for MapFrame property
        ///is the mapFrame null when adding a group?
        ///</summary>
        [TestMethod()]
        public void MapFrameIsNotNull_Group()
        {
            DotSpatial.Controls.Map map = new DotSpatial.Controls.Map();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "test-randomPts.shp");

            IFeatureSet fs = FeatureSet.Open(path);
            MapPointLayer myLayer = new MapPointLayer(fs);

            MapGroup grp = new MapGroup(map, "group1");
            map.Layers.Add(grp);
            grp.Layers.Add(myLayer);

            Assert.IsNotNull(myLayer.MapFrame, "mapFrame of layer in group should not be null.");
        }
    }
}