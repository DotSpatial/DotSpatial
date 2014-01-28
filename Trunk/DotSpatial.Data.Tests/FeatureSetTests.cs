using System.IO;
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
    }
}
