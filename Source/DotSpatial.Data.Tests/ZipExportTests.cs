using System.IO;
using System.Linq;
using DotSpatial.Tests.Common;
using NUnit.Framework;
using Ionic.Zip;

namespace DotSpatial.Data.Tests
{

    [TestFixture]
    class ZipExportTests
    {
        private readonly string _shapefiles = Path.Combine(@"Data", @"Shapefiles");

        [Test]
        [TestCase("counties.shp")]
        [TestCase("cities.shp")]
        [TestCase("rivers.shp")]
        public void ShapeFileExport(string filename)
        {

            string originalFileName = Path.Combine(new[] { _shapefiles, filename });
            string tempExtractDir = Path.Combine(new[] { Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetTempFileName()) });
            string newExtractedFileName = Path.Combine(new[] { tempExtractDir, filename });
            string newZipArchiveFileName = Path.Combine(new[] { tempExtractDir, Path.ChangeExtension(Path.GetTempFileName(), ".zip" )});

            var original = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(originalFileName); 

            ZipFile outputZip = original.ExportZipFile(filename);

            //check archive has correct number of contained files
            // shp, shx, dbf & prj
            Assert.AreEqual(4, outputZip.Entries.Count);

            // commit the zip file to disk, this must be done before extraction is possible 
            outputZip.Save(newZipArchiveFileName);
            //extract to the temp directory
            outputZip.ExtractAll( tempExtractDir );

            // open the shape file from the archive
            var newExport =  (IFeatureSet)DataManager.DefaultDataManager.OpenFile(newExtractedFileName);

            //compare the in memory representations of the original and the extract
            try
            {
                Assert.AreEqual(original.Features.Count, newExport.Features.Count);
                for (var j = 0; j < original.Features.Count; j += 100)
                {
                    Assert.AreEqual(original.Features.ElementAt(j).Geometry.Coordinates,
                        newExport.Features.ElementAt(j).Geometry.Coordinates);
                }
            }
            finally
            {
                FileTools.DeleteShapeFile(newExtractedFileName);
                DirectoryInfo di = new DirectoryInfo(tempExtractDir);
                di.Delete(true);
            }
        }
    }
}
