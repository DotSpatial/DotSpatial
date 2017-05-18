// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for the BinaryRasterProvider.
    /// </summary>
    [TestFixture]
    internal class BinaryRasterProviderTests
    {
        #region Methods

        /// <summary>
        /// Tests that a created BinaryRaster still contains the same data after beeing saved to file and loaded again.
        /// </summary>
        /// <param name="type">Type of the raster.</param>
        [TestCase(typeof(byte))]
        [TestCase(typeof(short))]
        [TestCase(typeof(int))]
        [TestCase(typeof(long))]
        [TestCase(typeof(float))]
        [TestCase(typeof(double))]
        [TestCase(typeof(sbyte))]
        [TestCase(typeof(ushort))]
        [TestCase(typeof(uint))]
        [TestCase(typeof(ulong))]
        [TestCase(typeof(bool))]
        [Test]
        public void CreateRasterForGivenType(Type type)
        {
            var target = new BinaryRasterProvider();
            var fileName = FileTools.GetTempFileName(".bgd");

            try
            {
                var raster = target.Create(fileName, string.Empty, 2, 2, 1, type, null);
                Assert.IsNotNull(raster);

                // assume that any tested type contains 0 and 1 values
                raster.Value[0, 0] = 1;
                raster.Value[0, 1] = 0;
                raster.Value[1, 0] = 0;
                raster.Value[1, 1] = 1;

                raster.Save();

                // Now open the file
                var openRaster = target.Open(fileName);
                Assert.IsNotNull(raster);

                for (var i = 0; i < openRaster.NumRows; i++)
                {
                    for (var j = 0; j < openRaster.NumColumns; j++)
                    {
                        Assert.AreEqual(raster.Value[i, j], openRaster.Value[i, j]);
                    }
                }
            }
            finally
            {
                File.Delete(fileName);
            }
        }

        #endregion
    }
}