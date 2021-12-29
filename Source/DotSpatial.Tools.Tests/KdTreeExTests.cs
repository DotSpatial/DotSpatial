// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    /// <summary>
    /// Tests for KdTreeEx.
    /// </summary>
    [TestFixture]
    internal class KdTreeExTests
    {
        /// <summary>
        /// Tests that the MethodFindBestMatchNodeFound is set and the search works without error.
        /// </summary>
        [Test]
        public void MethodFindBestMatchNodeFound()
        {
            var kd = new KdTreeEx<object>();
            Assert.IsTrue(kd.MethodFindBestMatchNodeFound);
            Assert.DoesNotThrow(() => kd.Search(new Coordinate()));
        }
    }
}
