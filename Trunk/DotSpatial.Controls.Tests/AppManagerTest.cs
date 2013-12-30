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

            string path = Path.GetFullPath(Path.Combine("TestFiles", "SerializeTestWithCustomSettings.map.xml.dspx"));

            target.SerializationManager.SaveProject(path);

            target.SerializationManager.OpenProject(path);
            actual = target.SerializationManager.GetCustomSetting<DateTime>(uniqueName, DateTime.Now.AddDays(1));
            Assert.AreEqual(expected.ToLongDateString(), actual.ToLongDateString());

            File.Delete(path);
        }


    }
}
