// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Data;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for FeatureList.
    /// </summary>
    [TestFixture]
    public class FeatureListTests
    {
        #region Methods

        /// <summary>
        /// Test whether the added datarow stays the same.
        /// </summary>
        [Test]
        public void AddMustNotReplaceFeatureRow()
        {
            var parent = new FeatureSet(FeatureType.Point);
            var target = parent.Features;
            DataRow expected = null;
            target.FeatureAdded += (sender, args) => expected = args.Feature.DataRow;

            var addedFeature = parent.AddFeature(Point.Empty);
            var actual = addedFeature.DataRow;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}