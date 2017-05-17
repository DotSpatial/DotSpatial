// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;
using DotSpatial.Data;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// Tests for ImageLayer.
    /// </summary>
    [TestFixture]
    internal class ImageLayerTests
    {
        #region Methods

        /// <summary>
        /// Checks that the legend text equals the file name for file based datasets.
        /// </summary>
        [Test]
        public void LegendTextEqualsFileNameWhenDataSetIsRealFile()
        {
            var target = new ImageLayer();
            Assert.IsNull(target.LegendText);
            var imageData = new ImageData
            {
                Filename = Path.GetTempFileName()
            };
            target.DataSet = imageData;

            // Legend should be equal to filename
            Assert.AreEqual(Path.GetFileName(imageData.Filename), target.LegendText);
        }

        /// <summary>
        /// Checks that the legend text is not changed if a dataset, that is not file based, gets assigned.
        /// </summary>
        [Test]
        public void LegendTextIsNotChangedWhenDataSetIsNotRealFile()
        {
            const string LegendText = "Custom Legend Text";
            var target = new ImageLayer
            {
                LegendText = LegendText
            };
            Assert.AreEqual(LegendText, target.LegendText);
            var imageData = new ImageData
            {
                Filename = string.Empty
            };
            target.DataSet = imageData; // assign dataset

            // Legend should be same
            Assert.AreEqual(LegendText, target.LegendText);
        }

        #endregion
    }
}