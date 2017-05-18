// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// Tests for LabelSymbolizer.
    /// </summary>
    [TestFixture]
    internal class LabelSymbolizerTests
    {
        /// <summary>
        /// Checks that the constructor uses valid enum values for initialization.
        /// </summary>
        [Test]
        public void CtorValidProperties()
        {
            var target = new LabelSymbolizer();
            Assert.IsTrue(ValidEnumValue(target.Alignment));
            Assert.IsTrue(ValidEnumValue(target.FontStyle));
            Assert.IsTrue(ValidEnumValue(target.LabelPlacementMethod));
            Assert.IsTrue(ValidEnumValue(target.Orientation));
            Assert.IsTrue(ValidEnumValue(target.PartsLabelingMethod));
            Assert.IsTrue(ValidEnumValue(target.ScaleMode));
        }

        private static bool ValidEnumValue<T>(T value)
        {
            return ((T[])Enum.GetValues(typeof(T))).Contains(value);
        }
    }
}
