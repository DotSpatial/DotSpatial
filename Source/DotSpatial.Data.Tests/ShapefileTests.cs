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
    }
}
