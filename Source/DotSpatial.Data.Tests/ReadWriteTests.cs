// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;
using System.Linq;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for read and write operations.
    /// </summary>
    [TestFixture]
    public class ReadWriteTests
    {
        #region Fields

        private readonly string _shapefiles = Path.Combine(@"Data", @"Shapefiles");

        #endregion

        #region Methods

        /// <summary>
        /// Tests that the saved polygon shapefile equals the original.
        /// </summary>
        /// <param name="filename">The filename.</param>
        [Test]
        [TestCase("counties.shp")]
        [TestCase("cities.shp")]
        [TestCase("rivers.shp")]
        public void PolygonShapeFile(string filename)
        {
            var testFile = Path.Combine(new[] { _shapefiles, filename });
            var newFile = Path.Combine(new[] { Path.GetTempPath(), filename });

            var original = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(testFile);
            original.Filename = newFile;
            original.Save();

            var newSave = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(newFile);

            try
            {
                Assert.AreEqual(original.Features.Count, newSave.Features.Count);
                for (var j = 0; j < original.Features.Count; j += 100)
                {
                    Assert.AreEqual(original.Features.ElementAt(j).Geometry.Coordinates, newSave.Features.ElementAt(j).Geometry.Coordinates);
                }
            }
            finally
            {
                FileTools.DeleteShapeFile(newFile);
            }
        }

        #endregion
    }
}