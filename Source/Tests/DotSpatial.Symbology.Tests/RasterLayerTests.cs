// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;
using DotSpatial.Data;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    /// <summary>
    /// Tests for RasterLayer.
    /// </summary>
    [TestFixture]
    internal class RasterLayerTests
    {
        #region Methods

        /// <summary>
        /// Checks that the BitmapGetter gets disposed when it is changed.
        /// </summary>
        [Test]
        public void BitmapGetterDisposeOnChange()
        {
            var raster = Raster.Create(FileTools.GetTempFileName(".bgd"), string.Empty, 1, 1, 1, typeof(byte), new[] { string.Empty });
            try
            {
                var target = new RasterLayer(raster)
                {
                    BitmapGetter = new ImageData()
                };
                var bitmapGetter = (DisposeBase)target.BitmapGetter;
                target.BitmapGetter = null;
                Assert.IsTrue(bitmapGetter.IsDisposed);
            }
            finally
            {
                File.Delete(raster.Filename);
            }
        }

        #endregion
    }
}