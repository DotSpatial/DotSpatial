using System;
using System.IO;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    ///This is a test class for AppManagerTest and is intended
    ///to contain all AppManagerTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class AppManagerTest
    {
        /// <summary>
        ///A test for GetCustomSettingDefault
        ///</summary>
        [Test]
        public void GetCustomSettingDefaultTest()
        {
            Map map = new Map();
            AppManager target = new AppManager();
            target.Map = map;

            string uniqueName = "customsettingname";
            var expected = DateTime.Now;
            var actual = target.SerializationManager.GetCustomSetting(uniqueName, expected);
            // checks that the default value is returned correctly
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetCustomSettingFromMemory
        ///</summary>
        [Test]
        public void GetCustomSettingFromMemoryTest()
        {
            Map map = new Map();
            AppManager target = new AppManager();
            target.Map = map;

            string uniqueName = "customsettingname";
            var expected = DateTime.Now;
            target.SerializationManager.SetCustomSetting(uniqueName, expected);

            var actual = target.SerializationManager.GetCustomSetting(uniqueName, DateTime.Now.AddDays(1));
            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for GetCustomSettingFromFile
        ///</summary>
        [Test]
        public void GetCustomSettingFromFileTest()
        {
            var map = new Map();
            var target = new AppManager {Map = map};

            var uniqueName = Guid.NewGuid().ToString();
            var expected = DateTime.Now;
            target.SerializationManager.SetCustomSetting(uniqueName, expected);

            var actual = target.SerializationManager.GetCustomSetting(uniqueName, DateTime.Now.AddDays(1));
            Assert.AreEqual(expected, actual);

            var path = FileTools.GetTempFileName(".dspx");
            try
            {
                target.SerializationManager.SaveProject(path);
                target.SerializationManager.OpenProject(path);
                actual = target.SerializationManager.GetCustomSetting(uniqueName, DateTime.Now.AddDays(1));
                Assert.AreEqual(expected.ToLongDateString(), actual.ToLongDateString());

            }
            finally
            {
                File.Delete(path);    
            }
        }
    }
}
