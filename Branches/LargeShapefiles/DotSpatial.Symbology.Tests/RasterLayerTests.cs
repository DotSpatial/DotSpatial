using System;
using System.IO;
using DotSpatial.Data;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    [TestFixture]
    class RasterLayerTests
    {
        [Test]
        public void BitmapGetter_DisposeOnChange()
        {
            var raster = Raster.Create(FileTools.GetTempFileName(".bgd"), String.Empty, 1, 1, 1, typeof(byte), new[] { String.Empty });
            try
            {
                var target = new RasterLayer(raster) { BitmapGetter = new ImageData() };
                var bitmapGetter = (DisposeBase)target.BitmapGetter;
                target.BitmapGetter = null;
                Assert.IsTrue(bitmapGetter.IsDisposed);
            }
            finally
            {
                File.Delete(raster.Filename);
            }
        }
    }
}
