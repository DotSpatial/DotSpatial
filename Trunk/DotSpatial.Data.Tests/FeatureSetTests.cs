using System;
using System.Data;
using System.IO;
using DotSpatial.Projections;
using DotSpatial.Tests.Common;
using DotSpatial.Topology;
using DotSpatial.Topology.Geometries;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    public class FeatureSetTests
    {
        private readonly string _shapefiles = Path.Combine(@"Data", @"Shapefiles");

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
        ///A test for FilePath http://dotspatial.codeplex.com/workitem/232
        ///</summary>
        [Test]
        public void FilePathTestWithSpaces()
        {
            FeatureSet target = new FeatureSet();
            string relPath1 = @"folder";
            string relPath2 = @"name\states.shp";
            string relativeFilePath = relPath1 + " " +  relPath2;
            string expectedFullPath = Path.Combine(Directory.GetCurrentDirectory(), relPath1) +
                                      " " + relPath2;
            string actualFilePath;
            target.FilePath = relativeFilePath;
            actualFilePath = target.FilePath;
            Assert.AreEqual(relativeFilePath, actualFilePath);

            string actualFileName = target.Filename;
            Assert.AreEqual(expectedFullPath, actualFileName);
        }

        /// <summary>
        ///A test for FilePath http://dotspatial.codeplex.com/workitem/232
        ///</summary>
        [Test]
        public void FilePathTest1()
        {
            FeatureSet target = new FeatureSet();
            string relativeFilePath = @"inner\states.shp";
            string expectedFullPath = Path.Combine(Directory.GetCurrentDirectory(),relativeFilePath);

            string actualFilePath;
            target.FilePath = relativeFilePath;
            actualFilePath = target.FilePath;
            Assert.AreEqual(relativeFilePath, actualFilePath);

            string actualFileName = target.Filename;
            Assert.AreEqual(expectedFullPath, actualFileName);
        }

        /// <summary>
        ///A test for FilePath http://dotspatial.codeplex.com/workitem/232
        ///</summary>
        [Test]
        public void FilePathTest2()
        {
            FeatureSet target = new FeatureSet();
            string relativeFilePath = @"..\..\states.shp";
            string expectedFullPath = Path.GetFullPath(relativeFilePath);

            string actualFilePath;
            target.FilePath = relativeFilePath;
            actualFilePath = target.FilePath;
            Assert.AreEqual(relativeFilePath, actualFilePath);

            string actualFileName = target.Filename;
            Assert.AreEqual(expectedFullPath, actualFileName);
        }

        [Test(Description = @"https://dotspatial.codeplex.com/workitem/25169")]
        public void UtmProjection_SamePoints_AfterSaveLoadShapeFile()
        {
            var fs = new FeatureSet(FeatureType.Point)
            {
                Projection = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone33N // set any UTM projection
            };

            const double originalX = 13.408056;
            const double originalY = 52.518611;

            var wgs = KnownCoordinateSystems.Geographic.World.WGS1984;
            var c = new[] { originalX, originalY };
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
                var fs0 = (Point) openFs.Features[0].Geometry;
                var c1 = new[] {fs0.X, fs0.Y};
                Reproject.ReprojectPoints(c1, z, openFs.Projection, wgs, 0, 1); // reproject back to wgs1984

                Assert.IsTrue(Math.Abs(originalX - c1[0]) < 1e-8);
                Assert.IsTrue(Math.Abs(originalY - c1[1]) < 1e-8);
            }
            finally
            {
                FileTools.DeleteShapeFile(tmpFile);
            }
        }

        [Test(Description = @"https://dotspatial.codeplex.com/discussions/535704")]
        public void CoordinateType_WriteOnSaveAs()
        {
            var outfile = FileTools.GetTempFileName(".shp");
            IFeatureSet fs = new FeatureSet();
            var c = new Coordinate(10.1, 20.2, 3.3, 4.4);

            fs.CoordinateType = CoordinateType.Z;
            fs.Projection = KnownCoordinateSystems.Geographic.World.WGS1984;
            fs.DataTable.Columns.Add(new DataColumn(("ID"), typeof(int)));

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

        [Test]
        public void MultiPoint_SaveAsWorking()
        {
            var vertices = new[]
            {
                new Coordinate(10.1, 20.2, 3.3, 4.4),
                new Coordinate(11.1, 22.2, 3.3, 4.4)
            };

            var mp = new MultiPoint(vertices);
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

        [Test]
        public void FeatureLookupIsNotNull()
        {
            var target = new FeatureSet();
            Assert.IsNotNull(target.FeatureLookup);
        }
    }
}
