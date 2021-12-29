// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Serialization.Tests
{
    /// <summary>
    /// TestEnum.
    /// </summary>
    public enum TestEnum
    {
        /// <summary>
        /// Default
        /// </summary>
        Default,

        /// <summary>
        /// One
        /// </summary>
        One,

        /// <summary>
        /// Two
        /// </summary>
        Two
    }

    /// <summary>
    /// Tests for Serialization.
    /// </summary>
    [TestFixture]
    public class SerializationTests
    {
        #region Fields

        private readonly List<string> _filesToRemove = new List<string>();
        private readonly string _folder = Common.AbsolutePath("Data");

        #endregion

        #region Methods

        /// <summary>
        /// Deletes the temporary files.
        /// </summary>
        [TearDown]
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

        /// <summary>
        /// Tests the serialization of dictionarys.
        /// </summary>
        [Test]
        public void TestDictionary()
        {
            var dictionary = new Dictionary<int, object>
            {
                { 1, new Node(42) },
                { 2, "Hello <insert name here>!" }
            };

            XmlSerializer s = new XmlSerializer();
            string result = s.Serialize(dictionary);

            XmlDeserializer d = new XmlDeserializer();
            Dictionary<int, object> newDictionary = d.Deserialize<Dictionary<int, object>>(result);

            foreach (var key in dictionary.Keys)
            {
                Assert.AreEqual(dictionary[key], newDictionary[key]);
            }
        }

        /// <summary>
        /// Tests the serialization of ObjectWithIntMember objects.
        /// </summary>
        [Test]
        public void TestFormatter()
        {
            ObjectWithIntMember obj = new ObjectWithIntMember(0xBEEF);
            XmlSerializer s = new XmlSerializer();
            string xml = s.Serialize(obj);

            XmlDeserializer d = new XmlDeserializer();
            ObjectWithIntMember result1 = d.Deserialize<ObjectWithIntMember>(xml);
            Assert.IsNotNull(result1);
            Assert.AreEqual(0xBEEF, result1.Number);

            ObjectWithIntMember result2 = new ObjectWithIntMember(0);
            d.Deserialize(result2, xml);
            Assert.IsNotNull(result2);
            Assert.AreEqual(0xBEEF, result2.Number);
        }

        /// <summary>
        /// Tests the serialization of a graph with enum nodes.
        /// </summary>
        [Test]
        public void TestGraphWithEnumNode()
        {
            Node rootNode = new Node(1);
            Node intNode = new Node(42);
            Node enumNode = new Node(TestEnum.Two);

            rootNode.Nodes.Add(intNode);
            rootNode.Nodes.Add(enumNode);

            intNode.Nodes.Add(rootNode);

            Graph g = new Graph(rootNode);

            XmlSerializer s = new XmlSerializer();
            string result = s.Serialize(g);

            XmlDeserializer d = new XmlDeserializer();
            Graph newGraph = d.Deserialize<Graph>(result);

            Assert.IsNotNull(newGraph);
            Assert.AreEqual(g.Root.Data, newGraph.Root.Data);
            Assert.AreEqual(g.Root.Nodes[0].Data, newGraph.Root.Nodes[0].Data);
            Assert.AreEqual(g.Root.Nodes[1].Data, newGraph.Root.Nodes[1].Data);
            Assert.AreEqual(g.Root.Nodes[0].Nodes[0].Data, newGraph.Root.Nodes[0].Nodes[0].Data);
            Assert.AreSame(newGraph.Root, newGraph.Root.Nodes[0].Nodes[0]);
        }

        /// <summary>
        /// Tests the serialization of a graph with string nodes.
        /// </summary>
        [Test]
        public void TestGraphWithStringNode()
        {
            Node rootNode = new Node(1);
            Node intNode = new Node(42);
            Node stringNode = new Node("test string with <invalid> characters!");

            rootNode.Nodes.Add(intNode);
            rootNode.Nodes.Add(stringNode);

            intNode.Nodes.Add(rootNode);

            Graph g = new Graph(rootNode);

            XmlSerializer s = new XmlSerializer();
            string result = s.Serialize(g);

            XmlDeserializer d = new XmlDeserializer();
            Graph newGraph = d.Deserialize<Graph>(result);

            Assert.IsNotNull(newGraph);
            Assert.AreEqual(g.Root.Data, newGraph.Root.Data);
            Assert.AreEqual(g.Root.Nodes[0].Data, newGraph.Root.Nodes[0].Data);
            Assert.AreEqual(g.Root.Nodes[1].Data, newGraph.Root.Nodes[1].Data);
            Assert.AreEqual(g.Root.Nodes[0].Nodes[0].Data, newGraph.Root.Nodes[0].Nodes[0].Data);
            Assert.AreSame(newGraph.Root, newGraph.Root.Nodes[0].Nodes[0]);
        }

        /// <summary>
        /// Test for DotSpatial Issue #254.
        /// </summary>
        [Test]
        public void TestMapFrameIsNotNull()
        {
            string filename = Path.Combine(_folder, "test-RandomPts.shp");
            string projectFileName = FileTools.GetTempFileName(".dspx");
            _filesToRemove.Add(projectFileName);

            AppManager manager = new AppManager();
            Map map = new Map();
            manager.Map = map;

            IFeatureSet fs = FeatureSet.Open(filename);
            MapPointLayer l = new MapPointLayer(fs);
            map.Layers.Add(l);
            Assert.Greater(map.Layers.Count, 0);

            manager.SerializationManager.SaveProject(projectFileName);
            Assert.True(File.Exists(projectFileName));

            // reopen the project
            map.Layers.Clear();
            Assert.AreEqual(map.Layers.Count, 0);

            manager.SerializationManager.OpenProject(projectFileName);
            Assert.Greater(map.Layers.Count, 0);
            Assert.IsNotNull(map.Layers[0].MapFrame);
        }

        /// <summary>
        /// Test for DotSpatial Issue #254.
        /// </summary>
        [Test]
        public void TestMapFrameIsNotNullGroup()
        {
            string filename = Path.Combine(_folder, "test-RandomPts.shp");
            string projectFileName = FileTools.GetTempFileName(".dspx");
            _filesToRemove.Add(projectFileName);

            AppManager manager = new AppManager();
            Map map = new Map();
            manager.Map = map;

            // new map group added to map
            MapGroup grp = new MapGroup(map, "group1");

            // new map layer added to group
            IFeatureSet fs = FeatureSet.Open(filename);
            MapPointLayer l = new MapPointLayer(fs);

            // add layer to group
            grp.Layers.Add(l);

            Assert.Greater(map.Layers.Count, 0);
            Assert.IsNotNull(l.MapFrame);

            manager.SerializationManager.SaveProject(projectFileName);
            Assert.True(File.Exists(projectFileName));

            // reopen the project
            map.Layers.Clear();
            Assert.AreEqual(map.Layers.Count, 0);

            manager.SerializationManager.OpenProject(projectFileName);

            List<ILayer> layers = map.GetAllLayers();
            Assert.IsNotNull(layers[0].MapFrame);
        }

        /// <summary>
        /// Test the serialization of a MapPointLayer.
        /// </summary>
        [Test]
        public void TestMapPointLayer()
        {
            string filename = Path.Combine(_folder, "test-RandomPts.shp");

            IFeatureSet fs = FeatureSet.Open(filename);
            MapPointLayer l = new MapPointLayer(fs);
            XmlSerializer s = new XmlSerializer();
            string result = s.Serialize(l);

            XmlDeserializer d = new XmlDeserializer();
            MapPointLayer newPointLayer = d.Deserialize<MapPointLayer>(result);

            Assert.IsNotNull(newPointLayer);
            Assert.AreEqual(newPointLayer.DataSet.Filename, Path.GetFullPath(filename));
        }

        /// <summary>
        /// Tests the serialization of a point.
        /// </summary>
        [Test]
        public void TestPointSerialization()
        {
            var pt = new Point(1, 2);
            var s = new XmlSerializer();
            var xml = s.Serialize(pt);

            var d = new XmlDeserializer();
            var result = d.Deserialize<Point>(xml);

            Assert.AreEqual(pt, result);
        }

        /// <summary>
        /// Tests the serialization of a rectangle.
        /// </summary>
        [Test]
        public void TestRectangleSerialization()
        {
            var rectangle = new Rectangle(1, 1, 2, 2);
            XmlSerializer s = new XmlSerializer();
            string xml = s.Serialize(rectangle);

            XmlDeserializer d = new XmlDeserializer();
            Rectangle result = d.Deserialize<Rectangle>(xml);

            Assert.AreEqual(1, result.X);
            Assert.AreEqual(1, result.Y);
            Assert.AreEqual(2, result.Width);
            Assert.AreEqual(2, result.Height);
        }

        /// <summary>
        /// Tests the serialization of a simpel graph.
        /// </summary>
        [Test]
        public void TestSimpleGraph()
        {
            Node rootNode = new Node(1);
            Node intNode = new Node(42);

            rootNode.Nodes.Add(intNode);
            intNode.Nodes.Add(rootNode);

            Graph g = new Graph(rootNode);

            XmlSerializer s = new XmlSerializer();
            string result = s.Serialize(g);

            XmlDeserializer d = new XmlDeserializer();
            Graph newGraph = d.Deserialize<Graph>(result);

            Assert.IsNotNull(newGraph);
            Assert.AreEqual(g.Root.Data, newGraph.Root.Data);
            Assert.AreEqual(g.Root.Nodes[0].Data, newGraph.Root.Nodes[0].Data);
            Assert.AreEqual(g.Root.Nodes[0].Nodes[0].Data, newGraph.Root.Nodes[0].Nodes[0].Data);
            Assert.AreSame(newGraph.Root, newGraph.Root.Nodes[0].Nodes[0]);
        }

        #endregion
    }
}