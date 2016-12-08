using System.IO;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Tests.Common;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    [TestFixture]
    internal class ClipPolygonWithPolygonTests
    {
        private readonly string _shapefiles = Path.Combine(@"Data", @"Shapefiles");

        /// <summary>
        /// After clipping Europe with Belgium the test verifies that the clipping outputs the correct number of features,
        /// and that the attributes columns from the original file are set on the output.
        /// It is important that the input and output files are loaded exactly as they are here to match how they are loaded
        /// when using the ToolDialog for ClipPolygonWithPolygon.
        /// </summary>
        /// <remarks>
        /// Issue: https://github.com/DotSpatial/DotSpatial/issues/892
        /// </remarks>
        [Test]
        public void CopyAttributesToClipped()
        {
            var target = new ClipPolygonWithPolygon();

            // load input 1 Shapefile as IFeatureSet
            IFeatureSet europeShape = Shapefile.OpenFile(@"Data\ClipPolygonWithPolygonTests\EUR_countries.shp");

            // load input 2 Shapefile as IFeatureSet
            IFeatureSet belgiumShape = Shapefile.OpenFile(@"Data\ClipPolygonWithPolygonTests\Belgium.shp");

            // set output file as IFeatureSet shapefile
            IFeatureSet outputShape = new Shapefile()
            {
                Filename = FileTools.GetTempFileName(".shp")
            };

            target.Execute(europeShape, belgiumShape, outputShape, new MockProgressHandler());

            // the output file needs to be closed and re-opened, because the DataTable in memory does not match the DataTable on disk
            outputShape.Close();
            var outputFile = FeatureSet.Open(outputShape.Filename);

            try
            {
                // output shapefile attribute columns should match the input 1 attribute columns
                Assert.That(outputFile.DataTable.Columns[0].Caption.Equals("ID"));
                Assert.That(outputFile.DataTable.Columns[1].Caption.Equals("Name"));

                string[,] dataValues = { {"BE", "Belgium"}, {"DE", "Germany"}, {"LU", "Luxembourg"} };

                var mpCount = 0;
                foreach (var feature in outputFile.Features)
                {
                    Assert.That(feature.DataRow.ItemArray.Length == 2 && feature.DataRow.ItemArray[0].Equals(dataValues[mpCount, 0]) && feature.DataRow.ItemArray[1].Equals(dataValues[mpCount, 1]));
                    mpCount++;
                }
                Assert.That(mpCount == 3);
            }
            finally
            {
                FileTools.DeleteShapeFile(outputShape.Filename);
            }
        }
    }
}
