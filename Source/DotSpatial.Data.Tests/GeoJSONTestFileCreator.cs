using System;
using System.IO;

namespace DotSpatial.Data.Tests
{
    internal class GeoJSONTestFileCreator
    {
        private static string geoJSONFile = "GeoJSONTestFile.gml";
        internal static string CreateGeoJSONTestFile()
        {
            var fullfilename = $"{Directory.GetCurrentDirectory()}\\{geoJSONFile}";
            File.WriteAllText(fullfilename, @"{""type"":""FeatureCollection"",""features"":[{""type"":""Feature"",""id"":""mypolygon_px6.1"",""geometry"":{""type"":""Polygon"",""coordinates"":[[[79.39064024773653,11.677958136567844],[79.40574644890594,11.697121798928404],[79.44763182487713,11.684009962648828],[79.44694517936921,11.65307701972681],[79.42256926384478,11.627857397680973],[79.39613341179829,11.635255390466018],[79.39064024773653,11.677958136567844]]]},""geometry_name"":""the_geom"",""properties"":{""Name"":""Boundary1"",""Description"":""This is sample boundary created for testing purpose< BR > "",""Start_Date"":""2012 - 04 - 15T18:30:00Z"",""End_Date"":""2012 - 04 - 15T18:30:00Z"",""String_V1"":""Surya"",""String_V2"":null,""Number_V1"":null,""Number_V2"":null,""bbox"":[79.39064024773653,11.627857397680973,79.44763182487713,11.697121798928404]}}],""crs"":{""type"":""EPSG"",""properties"":{""code"":""4326""}},""bbox"":[79.3906402477365,11.627857397681,79.4476318248771,11.6971217989284]}");
            return fullfilename;
        }

        internal static void DeleteGeoJSONTestFile()
        {
            var fullfilename = $"{Directory.GetCurrentDirectory()}\\{geoJSONFile}";
            File.Delete(fullfilename);
        }
    }
}