using System;
using System.Globalization;
using System.IO;
using System.Threading;
using DotSpatial.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotSpatial.Serialization.Tests
{


    /// <summary>
    ///This is a test class for XmlDeserializerTest and is intended
    ///to contain all XmlDeserializerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class XmlDeserializerTest
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

        [TestMethod()]
        public void DeserializeTest()
        {
            XmlDeserializer target = new XmlDeserializer();
            DotSpatial.Controls.Map map = new DotSpatial.Controls.Map();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "DeserializeTest.map.xml");
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
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "DeserializeTest.map.xml");
            target.Deserialize(map, File.ReadAllText(path));
        }

    }

}
