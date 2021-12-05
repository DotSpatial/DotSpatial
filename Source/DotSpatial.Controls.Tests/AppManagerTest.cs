// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    /// This is a test class for AppManagerTest and is intended
    /// to contain all AppManagerTest Unit Tests.
    /// </summary>
    [TestFixture]
    public class AppManagerTest
    {
        #region Methods

        /// <summary>
        /// A test for GetCustomSettingDefault.
        /// </summary>
        [Test]
        public void GetCustomSettingDefaultTest()
        {
            Map map = new Map();
            AppManager target = new AppManager
            {
                Map = map
            };

            string uniqueName = "customsettingname";
            var expected = DateTime.Now;
            var actual = target.SerializationManager.GetCustomSetting(uniqueName, expected);

            // checks that the default value is returned correctly
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// A test for GetCustomSettingFromFile.
        /// </summary>
        [Test]
        public void GetCustomSettingFromFileTest()
        {
            Map map = new Map();
            AppManager target = new AppManager
            {
                Map = map
            };

            string uniqueName = "customsettingname";
            var expected = DateTime.Now;
            target.SerializationManager.SetCustomSetting(uniqueName, expected);

            var actual = target.SerializationManager.GetCustomSetting(uniqueName, DateTime.Now.AddDays(1));
            Assert.AreEqual(expected, actual);

            string path = Path.Combine(Common.AbsolutePath("TestFiles"), "SerializeTestWithCustomSettings.map.xml.dspx");

            target.SerializationManager.SaveProject(path);

            target.SerializationManager.OpenProject(path);
            actual = target.SerializationManager.GetCustomSetting(uniqueName, DateTime.Now.AddDays(1));
            Assert.AreEqual(expected.ToLongDateString(), actual.ToLongDateString());

            File.Delete(path);
        }

        /// <summary>
        /// A test for GetCustomSettingFromMemory.
        /// </summary>
        [Test]
        public void GetCustomSettingFromMemoryTest()
        {
            Map map = new Map();
            AppManager target = new AppManager
            {
                Map = map
            };

            string uniqueName = "customsettingname";
            var expected = DateTime.Now;
            target.SerializationManager.SetCustomSetting(uniqueName, expected);

            var actual = target.SerializationManager.GetCustomSetting(uniqueName, DateTime.Now.AddDays(1));
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}