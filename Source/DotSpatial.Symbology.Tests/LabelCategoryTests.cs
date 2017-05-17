// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// Tests for LabelCategory.
    /// </summary>
    [TestFixture]
    internal class LabelCategoryTests
    {
        /// <summary>
        /// Checks that the constructor initializes the SelectionSymbolizer and Symbolizer.
        /// </summary>
        [Test]
        public void NotNullPropsWhenCtor()
        {
            var target = new LabelCategory();
            Assert.IsNotNull(target.SelectionSymbolizer);
            Assert.IsNotNull(target.Symbolizer);
            Assert.IsNotNull(target.ToString());
        }
    }
}
