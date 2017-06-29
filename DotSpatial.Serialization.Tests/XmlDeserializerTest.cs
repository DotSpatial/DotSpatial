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

        [TestMethod()]
        public void DeserializeTest()
        {
            XmlDeserializer target = new XmlDeserializer();
            DotSpatial.Controls.Map map = new DotSpatial.Controls.Map();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "DeserializeTest.map.xml");
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
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "DeserializeTest.map.xml");
            target.Deserialize(map, File.ReadAllText(path));
        }

    }

}
