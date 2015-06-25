using System;
using System.Globalization;
using System.IO;
using System.Threading;
using DotSpatial.Controls;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;

namespace DotSpatial.Serialization.Tests
{

    /// <summary>
    ///This is a test class for XmlDeserializerTest and is intended
    ///to contain all XmlDeserializerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class XmlDeserializerTest
    {
        [TestMethod()]
        public void DeserializeTest()
        {
            XmlDeserializer target = new XmlDeserializer();
            DotSpatial.Controls.Map map = new DotSpatial.Controls.Map();
            string path = Path.Combine("Data", "DeserializeTest.map.xml");
            target.Deserialize(map, File.ReadAllText(path));
        }

        [TestMethod()]
        public void DeserializeFrenchCultureTest()
        {
            // Sets the culture to French (France)
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");
            // Sets the UI culture to French (France)
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

            XmlDeserializer target = new XmlDeserializer();
            DotSpatial.Controls.Map map = new DotSpatial.Controls.Map();
            string path = Path.Combine("Data", "DeserializeTest.map.xml");
            target.Deserialize(map, File.ReadAllText(path));
        }

    }

}
