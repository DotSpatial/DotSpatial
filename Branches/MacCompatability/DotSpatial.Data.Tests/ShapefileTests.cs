using System.IO;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class ShapefileTests
    {
        private readonly string _shapefiles = Path.Combine(@"Data", @"Shapefiles");

        [Test]
        public void SelectByAttribute()
        {
            var shapefile = Shapefile.OpenFile(Path.Combine(_shapefiles, "lakes.shp"));
            var features = shapefile.SelectByAttribute("[NAME]='Great Salt Lake'");
            Assert.AreEqual(1, features.Count);
        }

        [Test]
        public void SelectIndexByAttribute()
        {
            var shapeFile = Shapefile.OpenFile(Path.Combine(_shapefiles, "lakes.shp"));
            var features = shapeFile.SelectIndexByAttribute("[NAME]='Great Salt Lake'");
            Assert.AreEqual(1, features.Count);
        }
    }
}
