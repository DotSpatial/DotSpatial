// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using NUnit.Framework;

namespace DotSpatial.Serialization.Tests
{
    /// <summary>
    /// Tests for the XmlHelper.
    /// </summary>
    [TestFixture]
    public class XmlHelperTests
    {
        #region Methods

        /// <summary>
        /// Tests that invalid characters get escaped.
        /// </summary>
        [Test]
        public void TestEscapeInvalidCharacters()
        {
            const string Input = "<>'\"&";
            string result = XmlHelper.EscapeInvalidCharacters(Input);

            Assert.AreEqual("&lt;&gt;&apos;&quot;&amp;", result);
        }

        /// <summary>
        /// Tests that already escaped invalid characters stay the same.
        /// </summary>
        [Test]
        public void TestEscapeInvalidCharactersWhenAlreadyEscaped()
        {
            const string Input = "&lt;&gt;&apos;&quot;&amp;";
            string result = XmlHelper.EscapeInvalidCharacters(Input);

            Assert.AreEqual("&lt;&gt;&apos;&quot;&amp;", result);
        }

        /// <summary>
        /// Test that escaped invalid characters get unescaped correctly.
        /// </summary>
        [Test]
        public void TestUnEscapeInvalidCharacters()
        {
            const string Input = "&lt;&gt;&apos;&quot;&amp;";
            string result = XmlHelper.UnEscapeInvalidCharacters(Input);

            Assert.AreEqual("<>'\"&", result);
        }

        #endregion
    }
}