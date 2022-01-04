// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Globalization;
using System.IO;
using System.Threading;
using DotSpatial.Controls;
using DotSpatial.Tests.Common;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace DotSpatial.Serialization.Tests
{
    /// <summary>
    /// This is a test class for XmlDeserializerTest and is intended
    /// to contain all XmlDeserializerTest Unit Tests.
    /// </summary>
    [TestClass]
    public class XmlDeserializerTest
    {
        private readonly string _folder = Common.AbsolutePath("Data");

        #region Methods

        /// <summary>
        /// Checks that a map gets deserialied correctly if a french culture is used.
        /// </summary>
        [TestMethod]
        public void DeserializeFrenchCultureTest()
        {
            // Sets the culture to French (France)
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-FR");

            // Sets the UI culture to French (France)
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr-FR");

            XmlDeserializer target = new XmlDeserializer();
            Map map = new Map();
            string path = Path.Combine(_folder, "DeserializeTest.map.xml");
            target.Deserialize(map, File.ReadAllText(path));
        }

        /// <summary>
        /// Checks that a map gets deserialized correctly.
        /// </summary>
        [TestMethod]
        public void DeserializeTest()
        {
            XmlDeserializer target = new XmlDeserializer();
            Map map = new Map();
            string path = Path.Combine(_folder, "DeserializeTest.map.xml");
            target.Deserialize(map, File.ReadAllText(path));
        }

        #endregion
    }
}