using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class ShapefileTests
    {
        [Test]
        public void SelectByAttribute()
        {
            var file = FileTools.PathToTestFile(@"Shapefiles\Lakes\lakes.shp");
            var shapefile = Shapefile.OpenFile(file);
            var features = shapefile.SelectByAttribute("[NAME]='Great Salt Lake'");
            Assert.AreEqual(1, features.Count);
        }

        [Test]
        public void SelectIndexByAttribute()
        {
            var file = FileTools.PathToTestFile(@"Shapefiles\Lakes\lakes.shp");
            var shapeFile = Shapefile.OpenFile(file);
            var features = shapeFile.SelectIndexByAttribute("[NAME]='Great Salt Lake'");
            Assert.AreEqual(1, features.Count);
        }
    }
}
