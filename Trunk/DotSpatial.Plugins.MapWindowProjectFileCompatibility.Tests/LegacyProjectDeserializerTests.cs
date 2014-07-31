using System;
using System.IO;
using DotSpatial.Controls;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility.Tests
{
    [TestFixture]
    public class LegacyProjectDeserializerTests
    {
        [Test]
        [TestCase(@"MWProjects\United States_Ver1\United States.mwprj", "projectfile", Description = "Test Case for projectfile Ver1")]
        [TestCase(@"MWProjects\United States_Ver2\United States.mwprj", "projectfile.2", Description = "Test Case for projectfile Ver2")]
        public void CanOpenProjectFile(string mprojFile, string projectFileVer)
        {
            mprojFile = FileTools.PathToTestFile(mprojFile);
            var map = new Map();
            var target = new LegacyProjectDeserializer(map);
            var curDir = Environment.CurrentDirectory;
            var fileInfo = new FileInfo(mprojFile);

            try
            {
                dynamic parser = DynamicXMLNode.Load(mprojFile);
                Assert.AreEqual(projectFileVer, (string)parser["type"]);
                Assert.AreEqual(0, map.Layers.Count);
                Environment.CurrentDirectory = fileInfo.DirectoryName;
                target.OpenFile(fileInfo.FullName);
                Assert.Greater(map.Layers.Count, 0);
            }
            finally
            {
                // Restore current directory
                Environment.CurrentDirectory = curDir;
            }
        }
    }
}
