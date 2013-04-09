using System;
using System.IO;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;
namespace DotSpatial.Controls.Tests
{


    /// <summary>
    ///This is a test class for AppManagerTest and is intended
    ///to contain all AppManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AppManagerTest
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
        ///A test for GetCustomSettingDefault
        ///</summary>
        [TestMethod()]
        public void GetCustomSettingDefaultTest()
        {
            DotSpatial.Controls.Map map = new DotSpatial.Controls.Map();
            AppManager target = new AppManager();
            target.Map = map;

            string uniqueName = "customsettingname";
            var expected = DateTime.Now;
            var actual = target.SerializationManager.GetCustomSetting<DateTime>(uniqueName, expected);
            // checks that the default value is returned correctly
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetCustomSettingFromMemory
        ///</summary>
        [TestMethod()]
        public void GetCustomSettingFromMemoryTest()
        {
            DotSpatial.Controls.Map map = new DotSpatial.Controls.Map();
            AppManager target = new AppManager();
            target.Map = map;

            string uniqueName = "customsettingname";
            var expected = DateTime.Now;
            target.SerializationManager.SetCustomSetting(uniqueName, expected);

            var actual = target.SerializationManager.GetCustomSetting<DateTime>(uniqueName, DateTime.Now.AddDays(1));
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetCustomSettingFromFile
        ///</summary>
        [TestMethod()]
        public void GetCustomSettingFromFileTest()
        {
            DotSpatial.Controls.Map map = new DotSpatial.Controls.Map();
            AppManager target = new AppManager();
            target.Map = map;

            string uniqueName = "customsettingname";
            var expected = DateTime.Now;
            target.SerializationManager.SetCustomSetting(uniqueName, expected);

            var actual = target.SerializationManager.GetCustomSetting<DateTime>(uniqueName, DateTime.Now.AddDays(1));
            Assert.AreEqual(expected, actual);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestFiles", "SerializeTestWithCustomSettings.map.xml.dspx");

            target.SerializationManager.SaveProject(path);

            target.SerializationManager.OpenProject(path);
            actual = target.SerializationManager.GetCustomSetting<DateTime>(uniqueName, DateTime.Now.AddDays(1));
            Assert.AreEqual(expected.ToLongDateString(), actual.ToLongDateString());

            File.Delete(path);
        }


    }
}
