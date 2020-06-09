// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using DotSpatial.NTSExtension;
using DotSpatial.Projections;
using DotSpatial.Tests.Common;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for FeatureSet.
    /// </summary>
    [TestFixture]
    public class FeatureSetTests
    {
        #region Fields

        private readonly string _shapefiles = Path.Combine(@"Data", @"Shapefiles");

        #endregion

        #region Methods

        /// <summary>
        /// Builds a point feature set and adds a column and 3 points.
        /// </summary>
        /// <returns>The featureset that was build.</returns>
        public IFeatureSet BuildFeatureSet()
        {
            IFeatureSet fs = new FeatureSet(FeatureType.Point);
            fs.DataTable.Columns.Add("Test", typeof(string));
            IFeature feat = fs.AddFeature(new Point(10, 10));
            feat.DataRow["Test"] = "hello";
            feat = fs.AddFeature(new Point(10, 20));
            feat.DataRow["Test"] = "hello";
            feat = fs.AddFeature(new Point(20, 10));
            feat.DataRow["Test"] = "here";
            return fs;
        }

        /// <summary>
        /// Makes sure the coordinate type gets written to the shapefile.
        /// </summary>
        [Test(Description = @"https://dotspatial.codeplex.com/discussions/535704")]
        public void CoordinateTypeWriteOnSaveAs()
        {
            var outfile = FileTools.GetTempFileName(".shp");
            IFeatureSet fs = new FeatureSet();
            var c = new Coordinate(10.1, 20.2, 3.3, 4.4);

            fs.CoordinateType = CoordinateType.Z;
            fs.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            fs.DataTable.Columns.Add(new DataColumn("ID", typeof(int)));

            IFeature f = fs.AddFeature(new Point(c));
            f.DataRow.BeginEdit();
            f.DataRow["ID"] = 1;
            f.DataRow.EndEdit();

            fs.SaveAs(outfile, true);

            var actual = FeatureSet.Open(outfile);
            try
            {
                Assert.AreEqual(fs.CoordinateType, actual.CoordinateType);
            }
            finally
            {
                FileTools.DeleteShapeFile(outfile);
            }
        }

        /// <summary>
        /// Check whether all the features get copied including the attributes.
        /// </summary>
        [Test(Description = @"Check whether all the features get copied including the attributes.")]
        public void CopyFeaturesWithAttributes()
        {
            IFeatureSet fs = BuildFeatureSet();

            IFeatureSet res5 = fs.CopyFeatures(true);
            Assert.AreEqual(res5.Features.Count, 3);
            Assert.AreEqual(res5.Features[0].DataRow["Test"], "hello");
        }

        /// <summary>
        /// Check whether all the features get copied and the attributes won't be copied.
        /// </summary>
        [Test(Description = @"Check whether all the features get copied and the attributes won't be copied.")]
        public void CopyFeaturesWithoutAttributes()
        {
            IFeatureSet fs = BuildFeatureSet();

            IFeatureSet res6 = fs.CopyFeatures(false);
            Assert.AreEqual(res6.Features.Count, 3);
            Assert.AreEqual(res6.Features[0].DataRow.ItemArray.Length, 0);
        }

        /// <summary>
        /// Check whether the correct number of features is copied including the attributes.
        /// </summary>
        [Test(Description = @"Check whether the correct number of features is copied including the attributes.")]
        public void CopySubsetWithAttributes()
        {
            IFeatureSet fs = BuildFeatureSet();

            IFeatureSet res = fs.CopySubset(string.Empty);
            Assert.AreEqual(res.Features.Count, 3);
            Assert.AreEqual(res.Features[0].DataRow["Test"], "hello");

            IFeatureSet res2 = fs.CopySubset("[Test] = 'hello'");
            Assert.AreEqual(res2.Features.Count, 2);
            Assert.AreEqual(res2.Features[0].DataRow["Test"], "hello");
        }

        /// <summary>
        /// Check whether the correct number of features is copied and the attributes won't be copied.
        /// </summary>
        [Test(Description = @"Check whether the correct number of features is copied and the attributes won't be copied.")]
        public void CopySubsetWithoutAttributes()
        {
            IFeatureSet fs = BuildFeatureSet();

            IFeatureSet res3 = fs.CopySubset(string.Empty, false);
            Assert.AreEqual(res3.Features.Count, 3);
            Assert.AreEqual(res3.Features[0].DataRow.ItemArray.Length, 0);

            IFeatureSet res4 = fs.CopySubset("[Test] = 'hello'", false);
            Assert.AreEqual(res4.Features.Count, 2);
            Assert.AreEqual(res4.Features[0].DataRow.ItemArray.Length, 0);
        }

        /// <summary>
        /// Makes sure FeatureLookup is not null after the FeatureSet was created.
        /// </summary>
        [Test]
        public void FeatureLookupIsNotNull()
        {
            var target = new FeatureSet();
            Assert.IsNotNull(target.FeatureLookup);
        }

        /// <summary>
        /// A test for FilePath http://dotspatial.codeplex.com/workitem/232.
        /// </summary>
        [Test]
        public void FilePathTest1()
        {
            FeatureSet target = new FeatureSet();
            string relativeFilePath = @"inner\states.shp";
            string expectedFullPath = Path.Combine(Directory.GetCurrentDirectory(), relativeFilePath);

            target.FilePath = relativeFilePath;
            var actualFilePath = target.FilePath;
            Assert.AreEqual(relativeFilePath, actualFilePath);

            string actualFileName = target.Filename;
            Assert.AreEqual(expectedFullPath, actualFileName);
        }

        /// <summary>
        /// A test for FilePath http://dotspatial.codeplex.com/workitem/232.
        /// </summary>
        [Test]
        public void FilePathTest2()
        {
            FeatureSet target = new FeatureSet();
            string relativeFilePath = @"..\..\states.shp";
            string expectedFullPath = Path.GetFullPath(relativeFilePath);

            target.FilePath = relativeFilePath;
            var actualFilePath = target.FilePath;
            Assert.AreEqual(relativeFilePath, actualFilePath);

            string actualFileName = target.Filename;
            Assert.AreEqual(expectedFullPath, actualFileName);
        }

        /// <summary>
        /// A test for FilePath http://dotspatial.codeplex.com/workitem/232.
        /// </summary>
        [Test]
        public void FilePathTestWithSpaces()
        {
            FeatureSet target = new FeatureSet();
            string relPath1 = @"folder";
            string relPath2 = @"name\states.shp";
            string relativeFilePath = relPath1 + " " + relPath2;
            string expectedFullPath = Path.Combine(Directory.GetCurrentDirectory(), relPath1) + " " + relPath2;
            target.FilePath = relativeFilePath;
            var actualFilePath = target.FilePath;
            Assert.AreEqual(relativeFilePath, actualFilePath);

            string actualFileName = target.Filename;
            Assert.AreEqual(expectedFullPath, actualFileName);
        }

        /// <summary>
        /// No idea what this is supposed to do.
        /// </summary>
        [Test]
        public void IndexModeToFeaturesClear()
        {
            var file = Path.Combine(_shapefiles, @"Topology_Test.shp");
            IFeatureSet fs = FeatureSet.Open(file);
            fs.FillAttributes();
            fs.Features.Clear();
            Assert.AreEqual(fs.Features.Count, 0);
            Assert.AreEqual(fs.DataTable.Rows.Count, 0);
        }

        /// <summary>
        /// Checks that multipoints can be saved to shapefile via SaveAs.
        /// </summary>
        [Test]
        public void MultiPointSaveAsWorking()
        {
            var vertices = new[] { new Coordinate(10.1, 20.2, 3.3, 4.4), new Coordinate(11.1, 22.2, 3.3, 4.4) };

            var mp = new MultiPoint(vertices.CastToPointArray());
            var f = new Feature(mp);
            var fs = new FeatureSet(f.FeatureType)
            {
                Projection = KnownCoordinateSystems.Geographic.World.WGS1984
            };
            fs.Features.Add(f);
            var fileName = FileTools.GetTempFileName(".shp");
            try
            {
                Assert.DoesNotThrow(() => fs.SaveAs(fileName, true));
            }
            catch (Exception)
            {
                FileTools.DeleteShapeFile(fileName);
            }
        }

        /// <summary>
        /// Checks that the Error NoDirEdges isn't thrown.
        /// </summary>
        [Test(Description = @"Checks that the Error NoDirEdges isn't thrown. (https://github.com/DotSpatial/DotSpatial/issues/602)")]
        public void NoDirEdges()
        {
            IFeatureSet fs1 = FeatureSet.Open(Path.Combine(_shapefiles, @"noDirEdgeFiles\catchment.shp"));
            IFeatureSet fs2 = FeatureSet.Open(Path.Combine(_shapefiles, @"noDirEdgeFiles\siteDesignArea.shp"));
            Assert.DoesNotThrow(() => fs1.Intersection(fs2, FieldJoinType.All, null));
        }

        /// <summary>
        /// Check whether polygon/line/multipoint features are saved with correct M/Z extent and value.
        /// </summary>
        /// <param name="c">The coordinate type.</param>
        /// <param name="ft">The feature type.</param>
        [Test(Description = @"Check whether polygon/line/multipoint features are saved with correct M/Z extent and value.")]
        [TestCase(CoordinateType.Regular, FeatureType.Line)]
        [TestCase(CoordinateType.M, FeatureType.Line)]
        [TestCase(CoordinateType.Z, FeatureType.Line)]
        [TestCase(CoordinateType.Regular, FeatureType.Polygon)]
        [TestCase(CoordinateType.M, FeatureType.Polygon)]
        [TestCase(CoordinateType.Z, FeatureType.Polygon)]
        [TestCase(CoordinateType.Regular, FeatureType.MultiPoint)]
        [TestCase(CoordinateType.M, FeatureType.MultiPoint)]
        [TestCase(CoordinateType.Z, FeatureType.MultiPoint)]
        public void SaveFeatureToShapefileWithMandZ(CoordinateType c, FeatureType ft)
        {
            var fileName = FileTools.GetTempFileName(".shp");

            try
            {
                List<Coordinate> coords = new List<Coordinate>
                                          {
                                              new Coordinate(1, 2, 7, 4),
                                              new Coordinate(3, 4, 5, 6),
                                              new Coordinate(5, 6, 3, 8),
                                              new Coordinate(7, 8, 9, 10),
                                              new Coordinate(1, 2, 7, 4)
                                          };

                var fs = new FeatureSet(ft)
                {
                    Projection = KnownCoordinateSystems.Geographic.World.WGS1984,
                    CoordinateType = c
                };

                switch (ft)
                {
                    case FeatureType.Line:
                        fs.AddFeature(new LineString(coords.ToArray()));
                        break;
                    case FeatureType.Polygon:
                        fs.AddFeature(new Polygon(new LinearRing(coords.ToArray())));
                        break;
                    case FeatureType.MultiPoint:
                        coords.RemoveAt(4);
                        fs.AddFeature(new MultiPoint(coords.CastToPointArray()));
                        break;
                }

                Assert.DoesNotThrow(() => fs.SaveAs(fileName, true));

                var loaded = FeatureSet.Open(fileName);

                if (c == CoordinateType.Regular)
                {
                    // regular coordinates don't have m values
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.Coordinates[0].M);
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.EnvelopeInternal.MinM);
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.EnvelopeInternal.MaxM);
                }
                else
                {
                    // m or z coordinates have m values
                    Assert.AreEqual(4, loaded.Features[0].Geometry.Coordinates[0].M);
                    Assert.AreEqual(4, loaded.Features[0].Geometry.EnvelopeInternal.MinM);
                    Assert.AreEqual(10, loaded.Features[0].Geometry.EnvelopeInternal.MaxM);
                }

                if (c == CoordinateType.Z)
                {
                    // z coordinates have z values
                    Assert.AreEqual(7, loaded.Features[0].Geometry.Coordinates[0].Z);
                    Assert.AreEqual(3, loaded.Features[0].Geometry.EnvelopeInternal.MinZ);
                    Assert.AreEqual(9, loaded.Features[0].Geometry.EnvelopeInternal.MaxZ);
                }
                else
                {
                    // regular and m coordinates don't have z values
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.Coordinates[0].Z);
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.EnvelopeInternal.MinZ);
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.EnvelopeInternal.MaxZ);
                }
            }
            finally
            {
                FileTools.DeleteShapeFile(fileName);
            }
        }

        /// <summary>
        /// Checks whether FeatureSet.OpenFile throws the right exception for the missing file.
        /// </summary>
        /// <param name="missingFile">The missing file.</param>
        /// <param name="errorMessage">The error message that gets thrown.</param>
        [Test(Description = @"Checks whether FeatureSet.OpenFile throws the right exception for the missing file.")]
        [TestCase("cities.shp", "Could not find file '{0}'.")]
        [TestCase("cities.shx", "The file {0} could not be found.")]
        [TestCase("cities.prj", "")]
        [TestCase("cities.dbf", "Could not find file '{0}'.")]
        [TestCase("rivers.shp", "Could not find file '{0}'.")]
        [TestCase("rivers.shx", "The file {0} could not be found.")]
        [TestCase("rivers.prj", "")]
        [TestCase("rivers.dbf", "Could not find file '{0}'.")]
        [TestCase("states.shp", "Could not find file '{0}'.")]
        [TestCase("states.shx", "The file {0} could not be found.")]
        [TestCase("states.prj", "")]
        [TestCase("states.dbf", "Could not find file '{0}'.")]
        public void OpenFileThrowsRightException(string missingFile, string errorMessage)
        {
            string missingFileExt = Path.GetExtension(missingFile);

            List<string> extensions = new List<string>() { ".shp", ".shx", ".dbf", ".prj" };
            extensions.Remove(missingFileExt);

            var source = Path.Combine(_shapefiles, missingFile);
            var tmpFile = FileTools.GetTempFileName(".shp");
            var missingTmpFile = Path.ChangeExtension(tmpFile, missingFileExt);

            foreach (var ext in extensions)
            {
                var sourceName = Path.ChangeExtension(source, ext);
                var tmpFileName = Path.ChangeExtension(tmpFile, ext);

                Assert.NotNull(tmpFileName, "tmpFileName != null");
                File.Copy(sourceName, tmpFileName);
            }

            if (errorMessage != string.Empty)
            {
                var e = Assert.Throws<FileNotFoundException>(() => FeatureSet.OpenFile(tmpFile));
                Assert.AreEqual(string.Format(errorMessage, missingTmpFile), e.Message);
            }
            else
            {
                Assert.DoesNotThrow(() => FeatureSet.OpenFile(tmpFile));
            }

            FileTools.DeleteShapeFile(tmpFile);
        }

        /// <summary>
        /// Check whether point features are saved with correct M/Z extent and value.
        /// </summary>
        /// <param name="c">The coordinate type.</param>
        [Test(Description = @"Check whether point features are saved with correct M/Z extent and value.")]
        [TestCase(CoordinateType.Regular)]
        [TestCase(CoordinateType.M)]
        [TestCase(CoordinateType.Z)]
        public void SavePointToShapefileWithMandZ(CoordinateType c)
        {
            var fileName = FileTools.GetTempFileName(".shp");

            try
            {
                var fs = new FeatureSet(FeatureType.Point)
                {
                    Projection = KnownCoordinateSystems.Geographic.World.WGS1984,
                    CoordinateType = c
                };

                fs.AddFeature(new Point(new Coordinate(1, 2, 7, 4)));

                Assert.DoesNotThrow(() => fs.SaveAs(fileName, true));

                var loaded = FeatureSet.Open(fileName);

                if (c == CoordinateType.Regular)
                {
                    // regular coordinates don't have m values
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.Coordinates[0].M);
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.EnvelopeInternal.MinM);
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.EnvelopeInternal.MaxM);
                }
                else
                {
                    // m or z coordinates have m values
                    Assert.AreEqual(4, loaded.Features[0].Geometry.Coordinates[0].M);
                    Assert.AreEqual(4, loaded.Features[0].Geometry.EnvelopeInternal.MinM);
                    Assert.AreEqual(4, loaded.Features[0].Geometry.EnvelopeInternal.MaxM);
                }

                if (c == CoordinateType.Z)
                {
                    // z coordinates have z values
                    Assert.AreEqual(7, loaded.Features[0].Geometry.Coordinates[0].Z);
                    Assert.AreEqual(7, loaded.Features[0].Geometry.EnvelopeInternal.MinZ);
                    Assert.AreEqual(7, loaded.Features[0].Geometry.EnvelopeInternal.MaxZ);
                }
                else
                {
                    // regular and m coordinates don't have z values
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.Coordinates[0].Z);
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.EnvelopeInternal.MinZ);
                    Assert.AreEqual(double.NaN, loaded.Features[0].Geometry.EnvelopeInternal.MaxZ);
                }
            }
            finally
            {
                FileTools.DeleteShapeFile(fileName);
            }
        }

        /// <summary>
        /// Tests whether or not the field mapper is saving the TypeCharacter correctly to shape files.
        /// Note that the TypeCharacter is derived from a field's DataType.
        /// When a shape file is read from disk, DataType is derived from the typeCode stored in the file.
        /// So when read from disk the translation goes as follows [file typeCode]->[DataType]->TypeCharacter.
        /// So there could be something lost in this translation.
        /// </summary>
        /// <param name="customFieldMapper">Whether or not to run the test with the Custom Field Mapper defined at the bottom of this document.</param>
        [TestCase(true)]
        [TestCase(false)]
        public void SaveShapeFileCustomFieldMappings(bool customFieldMapper)
        {
            if (customFieldMapper)
                FieldTypeCharacterMapperManager.Mapper = new CustomTestFieldMapper();

            var fileName = FileTools.GetTempFileName(".shp");

            try
            {
                var coords = new List<Coordinate>
                                          {
                                              new Coordinate(1, 2),
                                              new Coordinate(3, 4),
                                              new Coordinate(5, 6),
                                              new Coordinate(7, 8),
                                              new Coordinate(1, 2)
                                          };

                var fs = new FeatureSet(FeatureType.Line)
                {
                    Projection = KnownCoordinateSystems.Geographic.World.WGS1984,
                    CoordinateType = CoordinateType.Regular
                };

                fs.DataTable.Columns.Add(new DataColumn("doublefield", typeof(double)));
                fs.DataTable.Columns.Add(new DataColumn("decimalfield", typeof(decimal)));
                fs.DataTable.Columns.Add(new DataColumn("floatfield", typeof(float)));
                fs.DataTable.Columns.Add(new DataColumn("stringfield", typeof(string)));

                var feature = fs.AddFeature(new LineString(coords.ToArray()));
                feature.DataRow.BeginEdit();
                feature.DataRow["doublefield"] = 0.05d;
                feature.DataRow["decimalfield"] = 0.05m;
                feature.DataRow["floatfield"] = 0.05f;
                feature.DataRow["stringfield"] = "hello world";
                feature.DataRow.EndEdit();

                Assert.DoesNotThrow(() => fs.SaveAs(fileName, true));

                var loaded = FeatureSet.Open(fileName);
                if (customFieldMapper)
                {
                    Assert.True(new Field(loaded.DataTable.Columns[0]).TypeCharacter == FieldTypeCharacters.Double);
                    Assert.True(new Field(loaded.DataTable.Columns[1]).TypeCharacter == FieldTypeCharacters.Double);
                    Assert.True(new Field(loaded.DataTable.Columns[2]).TypeCharacter == FieldTypeCharacters.Double);
                }
                else
                {
                    Assert.True(new Field(loaded.DataTable.Columns[0]).TypeCharacter == FieldTypeCharacters.Number);
                    Assert.True(new Field(loaded.DataTable.Columns[1]).TypeCharacter == FieldTypeCharacters.Number);
                    Assert.True(new Field(loaded.DataTable.Columns[2]).TypeCharacter == FieldTypeCharacters.Number);
                }

                Assert.True(((Field)loaded.DataTable.Columns[3]).TypeCharacter == FieldTypeCharacters.Text);
            }
            finally
            {
                FileTools.DeleteShapeFile(fileName);
                FieldTypeCharacterMapperManager.Mapper = new FieldTypeCharacterMapper();
            }
        }

        /// <summary>
        /// Checks that FeatureSet.UnionShapes is not null and contains features.
        /// </summary>
        [Test]
        public void UnionFeatureSetTest()
        {
            var file = Path.Combine(_shapefiles, @"Topology_Test.shp");
            IFeatureSet fs = FeatureSet.Open(file);
            var union = fs.UnionShapes(ShapeRelateType.Intersecting);
            Assert.IsNotNull(union);
            Assert.IsTrue(union.Features.Count > 0);
        }

        /// <summary>
        /// Checks whether the WGS84 coordinates that were projected and saved to a WGS1984UTMZone33N are still the same after they are read from the shapefile and projected back to WGS84.
        /// </summary>
        [Test(Description = @"https://dotspatial.codeplex.com/workitem/25169")]
        public void UtmProjectionSamePointsAfterSaveLoadShapeFile()
        {
            var fs = new FeatureSet(FeatureType.Point)
            {
                Projection = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone33N // set any UTM projection
            };

            const double OriginalX = 13.408056;
            const double OriginalY = 52.518611;

            var wgs = KnownCoordinateSystems.Geographic.World.WGS1984;
            var c = new[] { OriginalX, OriginalY };
            var z = new[] { 0.0 };
            Reproject.ReprojectPoints(c, z, wgs, fs.Projection, 0, 1);

            var pt = new Point(c[0], c[1]);
            fs.AddFeature(pt);
            var tmpFile = FileTools.GetTempFileName(".shp");
            fs.SaveAs(tmpFile, true);

            try
            {
                // Now try to open saved shapefile
                // Points must have same location in WGS1984
                var openFs = FeatureSet.Open(tmpFile);
                var fs0 = (Point)openFs.Features[0].Geometry;
                var c1 = new[] { fs0.X, fs0.Y };
                Reproject.ReprojectPoints(c1, z, openFs.Projection, wgs, 0, 1); // reproject back to wgs1984

                Assert.IsTrue(Math.Abs(OriginalX - c1[0]) < 1e-8);
                Assert.IsTrue(Math.Abs(OriginalY - c1[1]) < 1e-8);
            }
            finally
            {
                FileTools.DeleteShapeFile(tmpFile);
            }
        }

        #endregion
    }
}