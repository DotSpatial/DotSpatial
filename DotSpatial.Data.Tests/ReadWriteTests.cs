using System.IO;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    public class ReadWriteTests
    {
        private readonly string _shapefiles = Path.Combine(@"Data", @"Shapefiles");

        [Test]
        [TestCase("counties.shp")]
        [TestCase("cities.shp")]
        [TestCase("rivers.shp")]
        public void PolygonShapeFile(string filename)
        {
            var testFile = Path.Combine(new[] { _shapefiles, filename });
            var newFile = Path.Combine(new[] { _shapefiles, "testSaves", filename });

            var original = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(testFile);;

            original.Filename = newFile;
            original.Save();

            var newSave = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(newFile);

            Assert.AreEqual(original.Features.Count, newSave.Features.Count);

            for (var j = 0; j < original.Features.Count; j+=100)
            {
                Assert.AreEqual(original.Features.ElementAt(j).Coordinates, newSave.Features.ElementAt(j).Coordinates);
            }
        }
    }
}