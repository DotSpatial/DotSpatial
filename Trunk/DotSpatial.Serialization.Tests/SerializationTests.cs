using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Tests.Common;
using MapWindow.Tests.XML.TestData;
using NUnit.Framework;

namespace DotSpatial.Serialization.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        private readonly List<string> _filesToRemove = new List<string>();

        [TestFixtureTearDown]
        public void Clear()
        {
            foreach (var tempFile in _filesToRemove)
            {
                try
                {
                    File.Delete(tempFile);
                }
                catch (Exception)
                {
                    // ignore exceptions
                }
            }
        }

        [Test]
        public void TestSimpleGraph()
        {
            var rootNode = new Node(1);
            var intNode = new Node(42);

            rootNode.Nodes.Add(intNode);
            intNode.Nodes.Add(rootNode);

            var g = new Graph(rootNode);

            var s = new XmlSerializer();
            var result = s.Serialize(g);

            var d = new XmlDeserializer();
            var newGraph = d.Deserialize<Graph>(result);

            Assert.IsNotNull(newGraph);
            Assert.AreEqual(g.Root.Data, newGraph.Root.Data);
            Assert.AreEqual(g.Root.Nodes[0].Data, newGraph.Root.Nodes[0].Data);
            Assert.AreEqual(g.Root.Nodes[0].Nodes[0].Data, newGraph.Root.Nodes[0].Nodes[0].Data);
            Assert.AreSame(newGraph.Root, newGraph.Root.Nodes[0].Nodes[0]);
        }

        [Test]
        public void TestGraphWithEnumNode()
        {
            var rootNode = new Node(1);
            var intNode = new Node(42);
            var enumNode = new Node(TestEnum.Two);

            rootNode.Nodes.Add(intNode);
            rootNode.Nodes.Add(enumNode);

            intNode.Nodes.Add(rootNode);

            var g = new Graph(rootNode);

            var s = new XmlSerializer();
            var result = s.Serialize(g);

            var d = new XmlDeserializer();
            var newGraph = d.Deserialize<Graph>(result);

            Assert.IsNotNull(newGraph);
            Assert.AreEqual(g.Root.Data, newGraph.Root.Data);
            Assert.AreEqual(g.Root.Nodes[0].Data, newGraph.Root.Nodes[0].Data);
            Assert.AreEqual(g.Root.Nodes[1].Data, newGraph.Root.Nodes[1].Data);
            Assert.AreEqual(g.Root.Nodes[0].Nodes[0].Data, newGraph.Root.Nodes[0].Nodes[0].Data);
            Assert.AreSame(newGraph.Root, newGraph.Root.Nodes[0].Nodes[0]);
        }

        [Test]
        public void TestGraphWithStringNode()
        {
            var rootNode = new Node(1);
            var intNode = new Node(42);
            var stringNode = new Node("test string with <invalid> characters!");

            rootNode.Nodes.Add(intNode);
            rootNode.Nodes.Add(stringNode);

            intNode.Nodes.Add(rootNode);

            var g = new Graph(rootNode);

            var s = new XmlSerializer();
            var result = s.Serialize(g);

            var d = new XmlDeserializer();
            var newGraph = d.Deserialize<Graph>(result);

            Assert.IsNotNull(newGraph);
            Assert.AreEqual(g.Root.Data, newGraph.Root.Data);
            Assert.AreEqual(g.Root.Nodes[0].Data, newGraph.Root.Nodes[0].Data);
            Assert.AreEqual(g.Root.Nodes[1].Data, newGraph.Root.Nodes[1].Data);
            Assert.AreEqual(g.Root.Nodes[0].Nodes[0].Data, newGraph.Root.Nodes[0].Nodes[0].Data);
            Assert.AreSame(newGraph.Root, newGraph.Root.Nodes[0].Nodes[0]);
        }

        [Test]
        public void TestDictionary()
        {
            var dictionary = new Dictionary<int, object>();
            dictionary.Add(1, new Node(42));
            dictionary.Add(2, "Hello <insert name here>!");

            var s = new XmlSerializer();
            var result = s.Serialize(dictionary);

            var d = new XmlDeserializer();
            var newDictionary = d.Deserialize<Dictionary<int, object>>(result);

            foreach (var key in dictionary.Keys)
            {
                Assert.AreEqual(dictionary[key], newDictionary[key]);
            }
        }

        [Test]
        public void TestMapPointLayer()
        {
            var filename = FileTools.PathToTestFile(@"Shapefiles\Cities\cities.shp");

            var fs = FeatureSet.Open(filename);
            var l = new MapPointLayer(fs);
            var s = new XmlSerializer();
            var result = s.Serialize(l);

            var d = new XmlDeserializer();
            var newPointLayer = d.Deserialize<MapPointLayer>(result);

            Assert.IsNotNull(newPointLayer);
            Assert.IsTrue(string.Equals(filename, Path.GetFullPath(newPointLayer.DataSet.Filename), StringComparison.OrdinalIgnoreCase));
        }
      
        [Test]
        [Ignore] // todo: fix reprojections
        public void TestMapFrameIsNotNull()
        {
            var filename = FileTools.PathToTestFile(@"Shapefiles\Cities\cities.shp");
            var projectFileName = FileTools.GetTempFileName(".dspx");
            _filesToRemove.Add(projectFileName);

            var manager = new AppManager();
            var map = new Map();
            manager.Map = map;

            var fs = FeatureSet.Open(filename);
            var l = new MapPointLayer(fs);
            map.Layers.Add(l);
            Assert.Greater(map.Layers.Count, 0);

            manager.SerializationManager.SaveProject(projectFileName);
            Assert.True(File.Exists(projectFileName));

            //reopen the project
            map.Layers.Clear();
            Assert.AreEqual(map.Layers.Count, 0);

            manager.SerializationManager.OpenProject(projectFileName);
            Assert.Greater(map.Layers.Count, 0);
            Assert.IsNotNull(map.Layers[0].MapFrame);
        }
        
        [Test]
        [Ignore] // todo: fix reprojections
        public void TestMapFrameIsNotNull_Group()
        {
            var filename = FileTools.PathToTestFile(@"Shapefiles\Cities\cities.shp");
            var projectFileName = FileTools.GetTempFileName(".dspx");
            _filesToRemove.Add(projectFileName);
            
            var manager = new AppManager();
            var map = new Map();
            manager.Map = map;

            //new map group added to map
            var grp = new MapGroup(map, "group1");

            //new map layer added to group
            var fs = FeatureSet.Open(filename);
            var l = new MapPointLayer(fs);

            //add layer to group
            grp.Layers.Add(l);

            Assert.Greater(map.Layers.Count, 0);
            Assert.IsNotNull(l.MapFrame);

            manager.SerializationManager.SaveProject(projectFileName);
            Assert.True(File.Exists(projectFileName));

            //reopen the project
            map.Layers.Clear();
            Assert.AreEqual(map.Layers.Count, 0);

            manager.SerializationManager.OpenProject(projectFileName);

            var layers = map.GetAllLayers();
            Assert.IsNotNull(layers[0].MapFrame);
        }

        [Test]
        public void TestFormatter()
        {
            var obj = new ObjectWithIntMember(0xBEEF);
            var s = new XmlSerializer();
            var xml = s.Serialize(obj);

            var d = new XmlDeserializer();
            var result1 = d.Deserialize<ObjectWithIntMember>(xml);
            Assert.IsNotNull(result1);
            Assert.AreEqual(0xBEEF, result1.Number);

            var result2 = new ObjectWithIntMember(0);
            d.Deserialize(result2, xml);
            Assert.IsNotNull(result2);
            Assert.AreEqual(0xBEEF, result2.Number);
        }

        [Test]
        public void TestPointSerializationMap()
        {
            var pt = new Point(1, 2);
            var s = new XmlSerializer();
            var xml = s.Serialize(pt);

            var d = new XmlDeserializer();
            var result = d.Deserialize<Point>(xml);

            Assert.AreEqual(pt, result);
        }

        [Test]
        public void TestRectangleSerializationMap()
        {
            var rectangle = new Rectangle(1, 1, 2, 2);
            var s = new XmlSerializer();
            var xml = s.Serialize(rectangle);

            var d = new XmlDeserializer();
            var result = d.Deserialize<Rectangle>(xml);

            Assert.AreEqual(1, result.X);
            Assert.AreEqual(1, result.Y);
            Assert.AreEqual(2, result.Width);
            Assert.AreEqual(2, result.Height);
        }

        public enum TestEnum
        {
            Default,
            One,
            Two
        }

    }


    #region SerializationMap classes

    // ReSharper disable UnusedMember.Global
    public class PointSerializationMap : SerializationMap
// ReSharper restore UnusedMember.Global
    {
        public PointSerializationMap()
            :base(typeof(Point))
        {
            var t = typeof(Point);

            var x = t.GetField("x", BindingFlags.Instance | BindingFlags.NonPublic);
            var y = t.GetField("y", BindingFlags.Instance | BindingFlags.NonPublic);

            Serialize(x, "X").AsConstructorArgument(0);
            Serialize(y, "Y").AsConstructorArgument(1);
        }
    }

// ReSharper disable UnusedMember.Global
    public class RectangleSerializationMap : SerializationMap
// ReSharper restore UnusedMember.Global
    {
        public RectangleSerializationMap()
            : base(typeof(Rectangle))
        {
            var t = typeof(Rectangle);

            var x = t.GetField("x", BindingFlags.Instance | BindingFlags.NonPublic);
            var y = t.GetField("y", BindingFlags.Instance | BindingFlags.NonPublic);
            var width = t.GetField("width", BindingFlags.Instance | BindingFlags.NonPublic);
            var height = t.GetField("height", BindingFlags.Instance | BindingFlags.NonPublic);

            Serialize(x, "X").AsConstructorArgument(0);
            Serialize(y, "Y").AsConstructorArgument(1);
            Serialize(width, "Width").AsConstructorArgument(2);
            Serialize(height, "Height").AsConstructorArgument(3);
        }
    }

    #endregion
}