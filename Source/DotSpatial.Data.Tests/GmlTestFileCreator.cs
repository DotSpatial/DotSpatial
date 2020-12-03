using System;
using System.IO;

namespace DotSpatial.Data.Tests
{
    internal class GmlTestFileCreator
    {
        private static string gmlFile = "gmlTestFile.gml";
        internal static string CreateGmlTestFile()
        {
            var fullfilename = $"{Directory.GetCurrentDirectory()}\\{gmlFile}";
            File.WriteAllText(fullfilename, @"<?xml version=""1.0"" encoding=""UTF-8""?><wfs:FeatureCollection xmlns=""http://www.opengis.net/wfs"" xmlns:wfs=""http://www.opengis.net/wfs"" xmlns:geonode=""http://worldmap.harvard.edu/"" xmlns:gml=""http://www.opengis.net/gml"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:schemaLocation=""http://worldmap.harvard.edu/ http://worldmap.harvard.edu/geoserver/wfs?service=WFS&amp;version=1.0.0&amp;request=DescribeFeatureType&amp;typeName=geonode%3Amypolygon_px6 http://www.opengis.net/wfs http://worldmap.harvard.edu/geoserver/schemas/wfs/1.0.0/WFS-basic.xsd""><gml:boundedBy><gml:Box srsName=""http://www.opengis.net/gml/srs/epsg.xml#4326""><gml:coordinates xmlns:gml=""http://www.opengis.net/gml"" decimal=""."" cs="","" ts="" "">79.39064025,11.6278574 79.44763182,11.6971218</gml:coordinates></gml:Box></gml:boundedBy><gml:featureMember><geonode:mypolygon_px6 fid=""mypolygon_px6.1""><gml:boundedBy><gml:Box srsName=""http://www.opengis.net/gml/srs/epsg.xml#4326""><gml:coordinates xmlns:gml=""http://www.opengis.net/gml"" decimal=""."" cs="","" ts="" "">79.39064025,11.6278574 79.44763182,11.6971218</gml:coordinates></gml:Box></gml:boundedBy><geonode:the_geom><gml:Polygon srsName=""http://www.opengis.net/gml/srs/epsg.xml#4326""><gml:outerBoundaryIs><gml:LinearRing><gml:coordinates xmlns:gml=""http://www.opengis.net/gml"" decimal=""."" cs="","" ts="" "">79.39064025,11.67795814 79.40574645,11.6971218 79.44763182,11.68400996 79.44694518,11.65307702 79.42256926,11.6278574 79.39613341,11.63525539 79.39064025,11.67795814</gml:coordinates></gml:LinearRing></gml:outerBoundaryIs></gml:Polygon></geonode:the_geom><geonode:Name>Boundary1</geonode:Name><geonode:Description>This is sample boundary created for testing purpose&lt;BR&gt;</geonode:Description><geonode:Start_Date>2012-04-15T18:30:00</geonode:Start_Date><geonode:End_Date>2012-04-15T18:30:00</geonode:End_Date><geonode:String_Value_1>Surya</geonode:String_Value_1></geonode:mypolygon_px6></gml:featureMember></wfs:FeatureCollection>");
            return fullfilename;
        }

        internal static void DeleteGmlTestFile()
        {
            var fullfilename = $"{Directory.GetCurrentDirectory()}\\{gmlFile}";
            File.Delete(fullfilename);
        }
    }
}