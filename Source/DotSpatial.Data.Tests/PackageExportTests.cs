using System.IO;
using System.Linq;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    internal class PackageExportTests
    {
        private readonly string _shapefiles = Path.Combine(@"Data", @"Shapefiles");

        [Test]
        [TestCase("counties.shp")]
        [TestCase("cities.shp")]
        [TestCase("rivers.shp")]
        public void ShapeFileExport(string filename)
        {
            string originalFileName = Path.Combine(new[] { _shapefiles, filename });
            
            var original = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(originalFileName);

            var package = original.ExportShapefilePackage();

            //check archive has correct number of contained files
            // shp, shx, dbf & prj
            Assert.IsNotNull(package.ShpFile);
            Assert.IsNotNull(package.ShxFile);
            Assert.IsNotNull(package.DbfFile);
            // prj may be null

            string temp_file_base = Path.GetRandomFileName();
            string shp_name = Path.Combine(Path.GetTempPath(), string.Format("{0}.shp", temp_file_base));
            string shx_name = Path.Combine(Path.GetTempPath(), string.Format("{0}.shx", temp_file_base));
            string dbf_name = Path.Combine(Path.GetTempPath(), string.Format("{0}.dbf", temp_file_base));

            SaveStream(shp_name, package.ShpFile);
            SaveStream(shx_name, package.ShxFile);
            SaveStream(dbf_name, package.DbfFile);

            // open the shape file from the archive
            var newExport = (IFeatureSet)DataManager.DefaultDataManager.OpenFile(shp_name);

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
                // this method deletes the other files too
                FileTools.DeleteShapeFile(shp_name);
            }
        }

        /// <summary>
        /// save a stream to disk
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        private static void SaveStream(string path, Stream content)
        {
            using (var fs = new FileStream(path, FileMode.Create))
            {
                content.CopyTo(fs);
                fs.Flush();
            }
        }
    }
}
