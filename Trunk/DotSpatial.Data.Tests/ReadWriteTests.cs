using System.IO;
using DotSpatial.Tests.Common;
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
        public void DataTheSameAfterSaveAs(string filename)
        {
            var testFile = Path.Combine(new[] { _shapefiles, filename });
            var newFile = Path.Combine(new[] { Path.GetTempPath(), filename });

            var original = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(testFile);;
            
            original.SaveAs(newFile, true);
            var newSave = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(newFile);

            try
            {
                Assert.AreEqual(original.Count, newSave.Count);
                for (var j = 0; j < original.Count; j++)
                {
                    var originalShape = original.GetFeature(j);
                    var newSaveShape = newSave.GetFeature(j);

                    Assert.AreEqual(originalShape.Coordinates, newSaveShape.Coordinates);
                    Assert.AreEqual(originalShape.DataRow.ItemArray, newSaveShape.DataRow.ItemArray);
                }
            }
            finally
            {
                FileTools.DeleteShapeFile(newFile);
            }
        }
    }
}