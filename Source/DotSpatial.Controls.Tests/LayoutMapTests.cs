// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using NUnit.Framework;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    /// Tests for LayoutMap.
    /// </summary>
    [TestFixture]
    internal class LayoutMapTests
    {
        /// <summary>
        /// Tests whether the constructor throws ArgumentNullException when no map is given.
        /// </summary>
        [Test]
        public void CtorExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => new LayoutMap(null));
        }
    }
}
