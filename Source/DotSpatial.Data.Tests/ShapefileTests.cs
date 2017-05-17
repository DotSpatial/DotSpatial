// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.IO;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for Shapefile.
    /// </summary>
    [TestFixture]
    internal class ShapefileTests
    {
        #region Fields

        private readonly string _shapefiles = Path.Combine(@"Data", @"Shapefiles");

        #endregion

        #region Methods

        /// <summary>
        /// This test checks that Shapefiles with Numeric columns using up to 15 decimal digits precision
        /// are loaded as double instead of as string.
        /// </summary>
        /// <remarks>
        /// Issue: https://github.com/DotSpatial/DotSpatial/issues/893
        /// </remarks>
        [Test]
        public void NumericColumnAsDoubleTest()
        {
            var shapeFile = Shapefile.OpenFile(Path.Combine(_shapefiles, @"OGR-numeric\ogr-numeric.shp"));
            Assert.AreEqual("System.Double", shapeFile.DataTable.Columns[2].DataType.FullName);
        }

        /// <summary>
        /// Checks that select by attribute returns the all the features found by the given filterExpression.
        /// </summary>
        [Test]
        public void SelectByAttribute()
        {
            var shapefile = Shapefile.OpenFile(Path.Combine(_shapefiles, "lakes.shp"));
            var features = shapefile.SelectByAttribute("[NAME]='Great Salt Lake'");
            Assert.AreEqual(1, features.Count);
        }

        /// <summary>
        /// Checks that select by attribute returns the all the features indizes found by the given filterExpression.
        /// </summary>
        [Test]
        public void SelectIndexByAttribute()
        {
            var shapeFile = Shapefile.OpenFile(Path.Combine(_shapefiles, "lakes.shp"));
            var features = shapeFile.SelectIndexByAttribute("[NAME]='Great Salt Lake'");
            Assert.AreEqual(1, features.Count);
        }

        /// <summary>
        /// This test checks whether the exported shapesfiles equal the original shapefiles.
        /// </summary>
        /// <param name="filename">Name of the original shapefile used for exporting.</param>
        /// <param name="indexMode">Indicates whether the IndexMode export routine should be used.</param>
        [Test]
        [TestCase("counties.shp", true)]
        [TestCase("cities.shp", true)]
        [TestCase("rivers.shp", true)]
        [TestCase("counties.shp", false)]
        [TestCase("cities.shp", false)]
        [TestCase("rivers.shp", false)]
        public void ShapeFileExport(string filename, bool indexMode)
        {
            // TODO needs test cases for multipoints
            string originalFileName = Path.Combine(new[] { _shapefiles, filename });

            var original = (Shapefile)DataManager.DefaultDataManager.OpenFile(originalFileName);
            original.IndexMode = indexMode;
            var package = original.ExportShapefilePackage();

            // check archive has correct number of contained files
            // shp, shx, dbf & prj
            Assert.IsNotNull(package.ShpFile);
            Assert.IsNotNull(package.ShxFile);
            Assert.IsNotNull(package.DbfFile);
            Assert.IsNotNull(package.PrjFile);

            string tempFileBase = Path.GetRandomFileName();
            string shpName = Path.Combine(Path.GetTempPath(), $"{tempFileBase}.shp");
            string shxName = Path.Combine(Path.GetTempPath(), $"{tempFileBase}.shx");
            string dbfName = Path.Combine(Path.GetTempPath(), $"{tempFileBase}.dbf");

            SaveStream(shpName, package.ShpFile);
            SaveStream(shxName, package.ShxFile);
            SaveStream(dbfName, package.DbfFile);

            // open the shape file from the archive
            var newExport = (Shapefile)DataManager.DefaultDataManager.OpenFile(shpName);

            // compare the in memory representations of the original and the extract
            try
            {
                Assert.AreEqual(original.Features.Count, newExport.Features.Count);
                for (var j = 0; j < original.Features.Count; j += 100)
                {
                    Assert.AreEqual(original.Features[j].DataRow, original.Features[j].DataRow);
                    Assert.AreEqual(original.Features[j].Geometry.Coordinates, newExport.Features[j].Geometry.Coordinates);
                }
            }
            finally
            {
                // this method deletes the other files too
                FileTools.DeleteShapeFile(shpName);
            }
        }

        /// <summary>
        /// Saves a stream to disk.
        /// </summary>
        /// <param name="path">Path of the destination file.</param>
        /// <param name="content">Stream with the content that should be saved to the file.</param>
        private static void SaveStream(string path, Stream content)
        {
            using (var fs = new FileStream(path, FileMode.Create))
            {
                content.CopyTo(fs);
                fs.Flush();
            }
        }

        #endregion
    }
}