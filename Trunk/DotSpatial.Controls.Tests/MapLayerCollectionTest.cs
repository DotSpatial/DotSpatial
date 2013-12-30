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
        /// <summary>
        ///A test for SelectedLayer.
        ///After the selected layer is removed, Layers.SelectedLayer should be null
        ///</summary>
        [TestMethod()]
        public void SelectedLayerNullIfLayerRemoved()
        {
            var map = new Map();
            var path = Path.Combine("TestFiles", "test-randomPts.shp");
            var myLayer = map.AddLayer(path);

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
            var map = new Map();
            var path = Path.Combine("TestFiles", "test-randomPts.shp");

            var fs = FeatureSet.Open(path);
            var myLayer = new MapPointLayer(fs);

            var grp = new MapGroup(map, "group1");
            map.Layers.Add(grp);
            grp.Layers.Add(myLayer);

            Assert.IsNotNull(myLayer.MapFrame, "mapFrame of layer in group should not be null.");
        }
    }
}