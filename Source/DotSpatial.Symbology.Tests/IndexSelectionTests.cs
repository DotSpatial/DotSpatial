// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// Tests for IndexSelection.
    /// </summary>
    [TestFixture]
    internal class IndexSelectionTests
    {
        /// <summary>
        /// Checks that only the attributes of the given field names get returned.
        /// </summary>
        [Test]
        public void GetAttributesWithFieldNames()
        {
            var fs = new FeatureSet(FeatureType.Point);
            fs.DataTable.Columns.Add("Column1");
            fs.DataTable.Columns.Add("Column2");
            fs.AddFeature(new Point(0, 0));

            var fl = new PointLayer(fs);

            var target = new IndexSelection(fl);
            var attributesTable = target.GetAttributes(0, 1, new[] { "Column1" });
            Assert.AreEqual(1, attributesTable.Columns.Count);
            Assert.AreEqual("Column1", attributesTable.Columns[0].ColumnName);
        }
    }
}
