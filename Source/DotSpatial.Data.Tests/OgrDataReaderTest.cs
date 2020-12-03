using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using DotSpatial.Data.Rasters.GdalExtension;
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace DotSpatial.Data.Tests
{
    [TestClass]
    public class OgrDataReaderTest
    {
        private readonly double _tolerance = 0.000000001;
        
        /// <summary>
        /// A test for OgrDataReader, tests if able to read a kml file.
        /// </summary>
        [TestMethod]
        public void OgrDataReaderTestReadKml()
        {
            var kmlTestFile = KmlTestFileCreator.CreateKmlTestFile();
            using (var dr = new OgrDataReader(kmlTestFile))
            {
                var layer = dr.GetLayers().Keys.First();
                Assert.NotNull(layer);
                Assert.AreEqual(658, layer.Vertex.Length);
                var geom = layer.Features[0];
                Assert.AreEqual(5, geom.NumParts());
                Assert.AreEqual(-47.616070987315659, layer.Vertex[0], _tolerance);
                Assert.AreEqual(-21.6161403350719, layer.Vertex[1], _tolerance);
                Assert.AreEqual(-47.61150467733979, layer.Vertex[656], _tolerance);
                Assert.AreEqual(-21.73314675187378, layer.Vertex[657], _tolerance);
                //layer.SaveAs($"OgrDataReaderTestReadKml\\OgrDataReaderTestReadKml.shp",true);
            }

            KmlTestFileCreator.DeleteKmlTestFile();
        }

        [TestMethod]
        public void OgrDataReaderTestReadKmz()
        {
            var kmzTestFile = KmlTestFileCreator.CreateKmzTestFile();
            using (var dr = new OgrDataReader(kmzTestFile))
            {
                var layer = dr.GetLayers().Keys.First();
                Assert.NotNull(layer);
                Assert.AreEqual(658, layer.Vertex.Length);
                var geom = layer.Features[0];
                Assert.AreEqual(5, geom.NumParts());
                Assert.AreEqual(-47.616070987315659, layer.Vertex[0], _tolerance);
                Assert.AreEqual(-21.6161403350719, layer.Vertex[1], _tolerance);
                Assert.AreEqual(-47.61150467733979, layer.Vertex[656], _tolerance);
                Assert.AreEqual(-21.73314675187378, layer.Vertex[657], _tolerance);
                //layer.SaveAs($"OgrDataReaderTestReadKmz\\OgrDataReaderTestReadKmz.shp", true);
            }

            KmlTestFileCreator.DeleteKmzTestFile();
        }

        [TestMethod]
        public void OgrDataReaderTestReadGml()
        {
            var gmlTestFile = GmlTestFileCreator.CreateGmlTestFile();
            using (var dr = new OgrDataReader(gmlTestFile))
            {
                var layer = dr.GetLayers().Keys.First();
                Assert.NotNull(layer);
                Assert.AreEqual(14, layer.Vertex.Length);
                var geom = layer.Features[0];
                Assert.AreEqual(1, geom.NumParts());
                Assert.AreEqual(79.39064025, layer.Vertex[0], _tolerance);
                Assert.AreEqual(11.67795814, layer.Vertex[1], _tolerance);
                Assert.AreEqual(79.39064025, layer.Vertex[12], _tolerance);
                Assert.AreEqual(11.67795814, layer.Vertex[13], _tolerance);
                //layer.SaveAs($"OgrDataReaderTestReadGml\\OgrDataReaderTestReadGml.shp", true);
            }

            GmlTestFileCreator.DeleteGmlTestFile();
        }

        [TestMethod]
        public void OgrDataReaderTestReadGeoJSON()
        {
            var geoJSONTestFile = GeoJSONTestFileCreator.CreateGeoJSONTestFile();
            using (var dr = new OgrDataReader(geoJSONTestFile))
            {
                var layer = dr.GetLayers().Keys.First();
                Assert.NotNull(layer);
                Assert.AreEqual(14, layer.Vertex.Length);
                var geom = layer.Features[0];
                Assert.AreEqual(1, geom.NumParts());
                Assert.AreEqual(79.39064024773653, layer.Vertex[0], _tolerance);
                Assert.AreEqual(11.677958136567844, layer.Vertex[1], _tolerance);
                Assert.AreEqual(79.39064024773653, layer.Vertex[12], _tolerance);
                Assert.AreEqual(11.677958136567844, layer.Vertex[13], _tolerance);
                //layer.SaveAs($"OgrDataReaderTestReadGeoJSON\\OgrDataReaderTestReadGeoJSON.shp", true);
            }

            GeoJSONTestFileCreator.DeleteGeoJSONTestFile();
        }

        [TestMethod]
        public void OgrDataReaderTestReadGpx()
        {
            var gpxTestFile = GpxTestFileCreator.CreateMapSourceSimpleWaypointERouteTestFile();
            //standard gpx reader only reads waypoints!
            using (var dr = new OgrDataReader(gpxTestFile))
            {
                var layer = dr.GetLayers().Keys.First();
                Assert.NotNull(layer);
                Assert.AreEqual(8, layer.Vertex.Length);
                var geom = layer.Features[0];
                Assert.AreEqual(1, geom.NumParts());
                Assert.AreEqual(-45.707975439727306, layer.Vertex[0], _tolerance);
                Assert.AreEqual(-23.719357447698712, layer.Vertex[1], _tolerance);
                Assert.AreEqual(-45.714390277862549, layer.Vertex[6], _tolerance);
                Assert.AreEqual(-23.71721469797194, layer.Vertex[7], _tolerance);
                //layer.SaveAs($"OgrDataReaderTestReadGpx\\OgrDataReaderTestReadGpx.shp", true);
            }

            GpxTestFileCreator.DeleteMapSourceSimpleWaypointERouteTestFile();
        }
        
        [TestMethod]
        public void OgrDataReaderTestGpsBabelFileNameExtractor()
        {
            var gpxTestFile = GpxTestFileCreator.CreateMapSourceSimpleWaypointERouteTestFile();
            var extracted = new GpsBabelFileNameExtractor().ExtractFileName($"GPSBabel:gpx:features=tracks:{gpxTestFile}");
            Assert.AreEqual(gpxTestFile, extracted);
            GpxTestFileCreator.DeleteMapSourceSimpleWaypointERouteTestFile();
        }

        [TestMethod]
        public void OgrDataReaderTestReadGpxWaypointsViaBabel()
        {
            var babelPath = GetFullPathFromWindows("GPSbabel.exe");
            if (String.IsNullOrEmpty(babelPath))
            {
                Assert.Ignore("GPSbabel.exe was not found! Ignoring Babel based tests! If left to continue a (System.ApplicationException : Could not create process gpsbabel...) would have been thrown! ");
                return;
            }

            var gpxTestFile = GpxTestFileCreator.CreateMapSourceSimpleWaypointERouteTestFile();
            using (var dr = new OgrDataReader($"GPSBabel:gpx:features=waypoints:{gpxTestFile}", new GpsBabelFileNameExtractor()))
            {
                var layer = dr.GetLayers().Keys.First();
                Assert.NotNull(layer);
                Assert.AreEqual(8, layer.Vertex.Length);
                var geom = layer.Features[0];
                Assert.AreEqual(1, geom.NumParts());
                Assert.AreEqual(-45.707975439727306, layer.Vertex[0], _tolerance);
                Assert.AreEqual(-23.719357447698712, layer.Vertex[1], _tolerance);
                Assert.AreEqual(-45.714390277862549, layer.Vertex[6], _tolerance);
                Assert.AreEqual(-23.71721469797194, layer.Vertex[7], _tolerance);
                //layer.SaveAs($"OgrDataReaderTestReadGpxWaypointsViaBabel\\OgrDataReaderTestReadGpxWaypointsViaBabel.shp", true);
            }

            GpxTestFileCreator.DeleteMapSourceSimpleWaypointERouteTestFile();
        }

        [TestMethod]
        public void OgrDataReaderTestReadGpxRoutesViaBabel()
        {
            var babelPath = GetFullPathFromWindows("GPSbabel.exe");
            if (String.IsNullOrEmpty(babelPath))
            {
                Assert.Ignore("GPSbabel.exe was not found! Ignoring Babel based tests! If left to continue a (System.ApplicationException : Could not create process gpsbabel...) would have been thrown! ");
                return;
            }

            var gpxTestFile = GpxTestFileCreator.CreateMapSourceSimpleWaypointERouteTestFile();
            using (var dr = new OgrDataReader($"GPSBabel:gpx:features=routes:{gpxTestFile}", new GpsBabelFileNameExtractor()))
            {
                var layer = dr.GetLayers().Keys.First();
                Assert.NotNull(layer);
                Assert.AreEqual(4, layer.Vertex.Length);
                var geom = layer.Features[0];
                Assert.AreEqual(1, geom.NumParts());
                Assert.AreEqual(-45.707053346559405, layer.Vertex[0], _tolerance);
                Assert.AreEqual(-23.718632999807596, layer.Vertex[1], _tolerance);
                Assert.AreEqual(-45.714390277862549, layer.Vertex[2], _tolerance);
                Assert.AreEqual(-23.71721469797194, layer.Vertex[3], _tolerance);
                //layer.SaveAs($"OgrDataReaderTestReadGpxRoutesViaBabel\\OgrDataReaderTestReadGpxRoutesViaBabel.shp", true);
            }

            GpxTestFileCreator.DeleteMapSourceSimpleWaypointERouteTestFile();
        }

        [TestMethod]
        public void OgrDataReaderTestReadGpxTracksViaBabel()
        {
            var babelPath = GetFullPathFromWindows("GPSbabel.exe");
            if (String.IsNullOrEmpty(babelPath))
            {
                Assert.Ignore("GPSbabel.exe was not found! Ignoring Babel based tests! If left to continue a (System.ApplicationException : Could not create process gpsbabel...) would have been thrown! ");
                return;
            }

            var gpxTestFile = GpxTestFileCreator.CreateTrackMakerGpsGpxTestFile();
            using (var dr = new OgrDataReader($"GPSBabel:gpx:features=tracks:{gpxTestFile}", new GpsBabelFileNameExtractor()))
            {
                var layer = dr.GetLayers().Keys.First();
                Assert.NotNull(layer);
                Assert.AreEqual(658, layer.Vertex.Length);
                var geom = layer.Features[0];
                Assert.AreEqual(5, layer.Features.Count);
                Assert.AreEqual(-47.616119460, layer.Vertex[0], _tolerance);
                Assert.AreEqual(-21.616149930, layer.Vertex[1], _tolerance);
                Assert.AreEqual(-47.611553182, layer.Vertex[656], _tolerance);
                Assert.AreEqual(-21.733156522, layer.Vertex[657], _tolerance);
                //layer.SaveAs($"OgrDataReaderTestReadGpxTracksViaBabel\\OgrDataReaderTestReadGpxTracksViaBabel.shp", true);
            }

            GpxTestFileCreator.DeleteTrackMakerGpsGpxTestFile();
        }

        /// <summary>
        /// Gets the full path of the given executable filename as if the user had entered this
        /// executable in a shell. So, for example, the Windows PATH environment variable will
        /// be examined. If the filename can't be found by Windows, null is returned.</summary>
        /// <param name="exeName"></param>
        /// <returns>The full path if successful, or null otherwise.</returns>
        public static string GetFullPathFromWindows(string exeName)
        {
            if (exeName.Length >= MAX_PATH)
                throw new ArgumentException($"The executable name '{exeName}' must have less than {MAX_PATH} characters.",
                    nameof(exeName));

            StringBuilder sb = new StringBuilder(exeName, MAX_PATH);
            return PathFindOnPath(sb, null) ? sb.ToString() : null;
        }

        // https://docs.microsoft.com/en-us/windows/desktop/api/shlwapi/nf-shlwapi-pathfindonpathw
        // https://www.pinvoke.net/default.aspx/shlwapi.PathFindOnPath
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = false)]
        private static extern bool PathFindOnPath([In, Out] StringBuilder pszFile, [In] string[] ppszOtherDirs);

        // from MAPIWIN.h :
        private const int MAX_PATH = 260;
    }
}
