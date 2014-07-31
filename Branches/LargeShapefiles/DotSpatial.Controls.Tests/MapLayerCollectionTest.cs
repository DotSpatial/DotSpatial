using System.IO;
using DotSpatial.Tests.Common;
using DotSpatial.Data;
using NUnit.Framework;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    ///This is a test class for MapTest and is intended
    ///to contain all MapTest Unit Tests
    ///</summary>
    [TestFixture]
    public class MapLayerCollectionTest
    {
        /// <summary>
        ///A test for SelectedLayer.
        ///After the selected layer is removed, Layers.SelectedLayer should be null
        ///</summary>
        [Test]
        public void SelectedLayerNullIfLayerRemoved()
        {
            var map = new Map();
            var path = FileTools.PathToTestFile(@"Shapefiles\Lakes\lakes.shp");
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
        [Test]
        public void MapFrameIsNotNull_Group()
        {
            var map = new Map();
            var path = FileTools.PathToTestFile(@"Shapefiles\Cities\cities.shp");

            var fs = FeatureSet.Open(path);
            var myLayer = new MapPointLayer(fs);

            var grp = new MapGroup(map, "group1");
            map.Layers.Add(grp);
            grp.Layers.Add(myLayer);

            Assert.IsNotNull(myLayer.MapFrame, "mapFrame of layer in group should not be null.");
        }
    }
}