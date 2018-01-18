// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.IO;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Contains tests for the InRamImageData class.
    /// </summary>
    [TestFixture]
    public class InRamImageDataTests
    {
        /// <summary>
        /// This checks that the image that gets loaded in InRamImageData(fileName) ctor gets drawn to _inRamImage.
        /// </summary>
        [Test(Description = "This checks that the image that gets loaded in InRamImageData(fileName) constructor gets drawn to _inRamImage.")]
        public void InRamImageDataCtorLoadsImageWithColor()
        {
            var imagePath = Path.Combine(@"Data\Grids", "Hintergrundkarte.tif");

            using (var inram = new InRamImageData(imagePath))
            using (var bitmap = inram.GetBitmap())
            {
                Assert.AreEqual(Color.FromArgb(255, 125, 105, 72), bitmap.GetPixel(300, 300)); // if the image was not drawn correctly GetPixel returns the ARGB values for white
            }
        }
    }
}
