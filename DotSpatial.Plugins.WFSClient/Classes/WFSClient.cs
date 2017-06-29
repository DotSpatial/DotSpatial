namespace WFSPlugin
{ 
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Xml.XPath;
    using DotSpatial.Data;
    using DotSpatial.Projections;
    using Renci.Data.Interop.OpenGIS.Wfs;
    using Renci.Data.Interop.OpenGIS.Gml;
    using DotSpatial.Topology;
    class WFSClient
    {
        #region Fields

        public string Server { get; set; }
        public WebProxy Proxy { get; set; }
        public string Version { get; set; }
        public ProjectionInfo Projection { get; set; }
        public string TypeName { get; set; }
        public string _CRS;
        public string Geometry { get; set; }
        public FeatureSet fea { get; set; }
        public string xml=null;
        public Uri uri;
        #endregion

        #region Global Variables

        public WfsCapabilitiesType wfs;
        private DotSpatial.Topology.FeatureType typeGeometry;
        private XmlNamespaceManager _nsmgr;
        public Dictionary<string, string> fields;

        #endregion

        #region GetCapabilities

        public void ReadCapabilities(string server = "")
        {

            if (server != "")
                Server = server;

            Uri u = new Uri(Server);

            Stream stream;
            if (u.IsAbsoluteUri && u.IsFile) //assume web if relative because IsFile is not supported on relative paths
            {
                stream = File.OpenRead(u.LocalPath);
            }
            else
            {
                uri = new Uri(CreateCapabiltiesRequest(Server));
                stream = GetRemoteXmlStream(uri, Proxy);
            }

            ParseGetCapabilities(stream);
        }

        public string CreateCapabiltiesRequest(string url)
        {
            var strReq = new StringBuilder(url);

            if (!url.Contains("?"))
                strReq.Append("?");

            if (!strReq.ToString().EndsWith("&") && !strReq.ToString().EndsWith("?"))
                strReq.Append("&");

            if (!url.ToLower().Contains("service=wfs"))
                strReq.AppendFormat("SERVICE=WFS&");

            if (!url.ToLower().Contains("request=getcapabilities"))
                strReq.AppendFormat("REQUEST=GetCapabilities&");

            if (!url.ToLower().Contains("version=") && Version != null)
                strReq.AppendFormat("version=" + Version + "&");

            return strReq.ToString();
        }

        private void ParseGetCapabilities(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(WfsCapabilitiesType));
            wfs = (WfsCapabilitiesType)serializer.Deserialize(stream);

        }

        #endregion

        #region DecribeFeatureType

        public void ReadDescribeFeatureType(string server = "")
        {
            foreach (var ope in wfs.OperationsMetadata.Operations)
            {
                if (ope.Name == "DescribeFeatureType")
                {
                    foreach (var typ in ope.Dcps)
                    {
                        server = typ.Item.GetMethods[0].Href;
                    }
                }
            }


            if (server != "")
                Server = server;

            Uri u = new Uri(Server);

            Stream stream;
            if (u.IsAbsoluteUri && u.IsFile) //assume web if relative because IsFile is not supported on relative paths
            {
                stream = File.OpenRead(u.LocalPath);
            }
            else
            {
                 uri = new Uri(CreateDescribeFeatureRequest(Server));
                stream = GetRemoteXmlStream(uri, Proxy);
            }

             XmlDocument xml = GetXml(stream);
             ParseDescribeFeatureType(xml);

        }

        private string CreateDescribeFeatureRequest(string url)
        {
            var strReq = new StringBuilder(url);

            if (!url.Contains("?"))
            {
                strReq.Append("?");
            }
            else
            {
                if (!url.EndsWith("?"))
                {
                    strReq = new StringBuilder(url.Substring(0, url.IndexOf("?")));
                    strReq.Append("?");
                }


            }

            if (!strReq.ToString().EndsWith("&") && !strReq.ToString().EndsWith("?"))
                strReq.Append("&");

            if (!url.ToLower().Contains("service=wfs"))
                strReq.AppendFormat("SERVICE=WFS&");

            if (!url.ToLower().Contains("request=describefeatureType"))
                strReq.AppendFormat("REQUEST=DescribeFeatureType&");

            if (!url.ToLower().Contains("version=") && wfs.Version != null)
                strReq.AppendFormat("version=" + wfs.Version + "&");

            if (!url.ToLower().Contains("TypeName=") && TypeName != null)
                strReq.AppendFormat("TypeName=" + TypeName + "&");
            return strReq.ToString();
        }

        private void ParseDescribeFeatureType(XmlDocument doc)
        {
            xml = doc.InnerXml;

            fields = new Dictionary<string, string>();
            _nsmgr = new XmlNamespaceManager(doc.NameTable);
            foreach (XmlNode nodes in doc.DocumentElement.Attributes)
            {
                if (nodes.Prefix == "xmlns")
                    _nsmgr.AddNamespace(nodes.Name, nodes.NamespaceURI);
            }
            var t = doc.DocumentElement["xsd:complexType"] == null ? doc.DocumentElement["complexType"] : doc.DocumentElement["xsd:complexType"];
            if (t == null) return;
            var complexContent = t["xsd:complexContent"] == null ? t["complexContent"] : t["xsd:complexContent"];
            var extension = complexContent["xsd:extension"] == null ? complexContent["extension"] : complexContent["xsd:extension"];
            var sequence = extension["xsd:sequence"] == null ? extension["sequence"] : extension["xsd:sequence"];
            fields = new Dictionary<string, string>();

            foreach (XmlNode ele in sequence)
            {
                fields.Add(ele.Attributes["name"].Value.ToString(),
                    ele.Attributes["type"].Value.ToString());

            }
        }

         #endregion

        #region GetFeature

        public void ReadFeature(string server)
        {
            foreach (var ope in wfs.OperationsMetadata.Operations)
            {
                if (ope.Name == "GetFeature")
                {
                    foreach (var typ in ope.Dcps)
                    {
                        server = typ.Item.GetMethods[0].Href;
                    }
                }
            }


            if (server != "")
                Server = server;

            Uri u = new Uri(Server);
            Stream stream;
            if (u.IsAbsoluteUri && u.IsFile) //assume web if relative because IsFile is not supported on relative paths
            {
                stream = File.OpenRead(u.LocalPath);
            }
            else
            {
                uri = new Uri(CreateFeatureRequest(Server));
                stream = GetRemoteXmlStream(uri, Proxy);
            }

            XmlDocument xml = GetXml(stream);
            ParseFeature(xml, uri);
        }

        private string CreateFeatureRequest(string url)
        {

            var strReq = new StringBuilder(url);

            if (!url.Contains("?"))
            {
                strReq.Append("?");
            }
            else
            {
                if (!url.EndsWith("?"))
                {
                    strReq = new StringBuilder(url.Substring(0, url.IndexOf("?")));
                    strReq.Append("?");
                }


            }

            if (!strReq.ToString().EndsWith("&") && !strReq.ToString().EndsWith("?"))
                strReq.Append("&");

            if (!url.ToLower().Contains("service=wfs"))
                strReq.AppendFormat("SERVICE=WFS&");

            if (!url.ToLower().Contains("request=getfeature"))
                strReq.AppendFormat("REQUEST=GetFeature&");

            if (!url.ToLower().Contains("version=") && wfs.Version != null)
                strReq.AppendFormat("version=" + wfs.Version + "&");

            if (!url.ToLower().Contains("TypeName=") && TypeName != null)
                strReq.AppendFormat("TypeName=" + TypeName);
            return strReq.ToString();
        }

        private void ParseFeature(XmlDocument xdoc, Uri uri)
        {
            CRS = "";
            
            xml = xdoc.InnerXml;
            XPathNavigator nav = xdoc.CreateNavigator();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nav.NameTable);
            _nsmgr.AddNamespace("wfs", "http://www.opengis.net/wfs");

            string number = "0";
            try
            {
                number = xdoc.DocumentElement.Attributes["numberOfFeatures"].Value.ToString();
            }
            catch
            {
                return;

            }

            if (Convert.ToInt32(number) == 0) return;


            XPathNodeIterator iterator = null;
            iterator = SelectTypeGeometry(nav, iterator);

            try
            {
                bool GetGeoReference = true;
                while (iterator.MoveNext())
                {
                    XPathNavigator node = iterator.Current;
                    var nod = node.UnderlyingObject;
                    XmlNode c = ((XmlNode)nod);

                    if (GetGeoReference)
                     GetGeoReference = ExtractReference(c);

                    IFeature feat = ExtractGeographicData(c);

                    foreach (XmlNode e in c)
                    {
                        if (!(e.LocalName == Geometry) && fields.Keys.Contains(e.LocalName))
                            feat.DataRow[e.LocalName] = e.InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private bool ExtractReference(XmlNode c)
        {
           // ProjectionInfo pro=null;
            if (typeGeometry == DotSpatial.Topology.FeatureType.Point)
            {
                foreach (XmlNode e in c)
                {
                    if (e.LocalName == Geometry)
                    {
                        CRS = e.FirstChild.Attributes[1].InnerXml;
                        return false;
                    }
                }
            }

            if (typeGeometry == DotSpatial.Topology.FeatureType.Line)
            {
                foreach (XmlNode e in c)
                {
                    if (e.LocalName == Geometry)
                    {
                        CRS = e.FirstChild.Attributes[1].InnerXml;
                        return false;
                    }
                }
            }

            if (typeGeometry == DotSpatial.Topology.FeatureType.Polygon)
            {
                foreach (XmlNode e in c)
                {
                    if (e.LocalName == Geometry)
                    {
                        CRS = e.FirstChild.Attributes[1].InnerXml;
                        return false;
                    }
                }
            }


            return true;

        }

        private IFeature ExtractGeographicData(XmlNode c)
        {
            string geoData = "";
            IBasicGeometry geo=null;
            string[] pointValue=null;
            if (typeGeometry == DotSpatial.Topology.FeatureType.Point)
            {
            foreach (XmlNode e in c)
            {
                if (e.LocalName == Geometry)
                {
                    geoData = e.InnerText;
                }
              }
             string point = Convert.ToString(geoData);
             pointValue = point.Split(' ');
             geo = new Point(Convert.ToDouble(pointValue[0]), Convert.ToDouble(pointValue[1]));
            }

            if (typeGeometry == DotSpatial.Topology.FeatureType.Polygon)
            {
                foreach (XmlNode e in c)
                {
                    if (e.LocalName == Geometry)
                    {
                        var t = e.FirstChild.OuterXml;
                        var s = new XmlSerializer(typeof(MultiSurfaceType));
                        MultiSurfaceType multi = s.Deserialize(new StringReader(t)) as MultiSurfaceType;
                        geo = GetPolygon(multi);
                       // geoData = e.InnerText;
                    }
                }
            }

            if (typeGeometry == DotSpatial.Topology.FeatureType.Line)
            {
                foreach (XmlNode e in c)
                {
                    if (e.LocalName == Geometry)
                    {
                        var t = e.FirstChild.OuterXml;
                        var s = new XmlSerializer(typeof(MultiLineStringType));
                        MultiLineStringType multi = s.Deserialize(new StringReader(t)) as MultiLineStringType;
                        geo = GetPolyline(multi);
                        // geoData = e.InnerText;
                    }
                }
            }



            IFeature feat = fea.AddFeature(geo);
         
            return feat;
        }

        private IBasicGeometry GetPolyline(MultiLineStringType multi)
        {

             ILinearRing[] lines = new LinearRing[multi.LineStringMembers.Count];
             int nLin = 0;
             foreach (LineStringPropertyType member in multi.LineStringMembers)
             {
                 lines[nLin]= ExtractLineString(member);
                 nLin++;
             }

            return new MultiLineString(lines);
        }

        private LinearRing ExtractLineString( LineStringPropertyType member)
        {
            var membe1 = member.LineString;
            foreach (DirectPositionListType rings in membe1.Items)
            {
                List<Coordinate> lstCoor = ExtractCoordinates(rings);
                  return  new LinearRing(lstCoor);
               
            }
            return null;

        }

        private IBasicGeometry GetPolygon(MultiSurfaceType multi)
        {
            Polygon[] p = new Polygon[multi.SurfaceMemberItems.Count];
                ;
           
            int npoly = 0;
            foreach (SurfacePropertyType member in multi.SurfaceMemberItems)
            {
                ILinearRing shell = null;
                ILinearRing[] holes = null;
                PolygonType sur = ExtractShellPolygon(ref shell, member);

                if (sur.Interior.Count == 0  && shell !=null)
                        p[npoly] = new Polygon(shell);
                else
                {
                    holes = new ILinearRing[sur.Interior.Count];
                    ExtractInteriorPolygon(holes, sur);
                    p[npoly] = new Polygon(shell, holes);
                }
                npoly++;
            }
            return new MultiPolygon(p);

        }

        private static void ExtractInteriorPolygon(ILinearRing[] holes, PolygonType sur)
        {
            Collection<AbstractRingPropertyType> lin = sur.Interior as Collection<AbstractRingPropertyType>;
            int i = 0;
            foreach (AbstractRingPropertyType ringis in lin)
            {
                LinearRingType lii = ringis.Ring as LinearRingType;

                foreach (DirectPositionListType rings in lii.Items)
                {
                    List<Coordinate> lstCoor = ExtractCoordinates(rings);

                    holes[i]=new LinearRing(lstCoor);
                    i++;
                }

            }
        }

        private static PolygonType ExtractShellPolygon(ref ILinearRing shell, SurfacePropertyType member)
        {
            PolygonType sur = member.Surface as PolygonType;
            LinearRingType li = sur.Exterior.Ring as LinearRingType;
            foreach (DirectPositionListType rings in li.Items)
            {
                List<Coordinate> lstCoor = ExtractCoordinates(rings);

                shell = new LinearRing(lstCoor);

            }
            return sur;
        }

        private static List<Coordinate> ExtractCoordinates(DirectPositionListType rings)
        {
            string[] listpoints = rings.Text.Split(' ');
            int num = listpoints.Count() / 2;
            List<Coordinate> lstCoor = new List<Coordinate>();

            for (int i = 0; i < listpoints.Count(); i = i + 2)
                lstCoor.Add(new Coordinate(Convert.ToDouble(listpoints[i]), Convert.ToDouble(listpoints[i + 1])));

            return lstCoor;
        }

        private XPathNodeIterator SelectTypeGeometry(XPathNavigator nav, XPathNodeIterator iterator)
        {
            GetGeometry();
            iterator = CreateFields(nav, typeGeometry);
            return iterator;
        }

        public static bool  IsGeographicFieldValid(string geographicField)
        {
            if (geographicField == "gml:PointPropertyType" || geographicField == "gml:MultiSurfacePropertyType"
                || geographicField == "gml:MultiLineStringPropertyType")
                return true;
            else
                return false;

        }

        private void GetGeometry()
        {
            if (fields[Geometry] == "gml:PointPropertyType")
                typeGeometry = DotSpatial.Topology.FeatureType.Point;
            if (fields[Geometry] == "gml:MultiSurfacePropertyType")
                typeGeometry = DotSpatial.Topology.FeatureType.Polygon;
            if (fields[Geometry] == "gml:MultiLineStringPropertyType")
                typeGeometry = DotSpatial.Topology.FeatureType.Line;
        }

        private XPathNodeIterator CreateFields(XPathNavigator nav, DotSpatial.Topology.FeatureType type)
        {

            string exp = @"/wfs:FeatureCollection/child::*[name() = 'gml:featureMember' or name() = 'gml:featureMembers']/child::*";
            XPathNodeIterator iterator = nav.Select(exp, _nsmgr);
            fea = new FeatureSet(type);
            if (iterator.Count > 0)
            {
                foreach (string fieldName in fields.Keys)
                {
                    if (fieldName != Geometry)
                    {
                        fea.DataTable.Columns.Add(new DataColumn(fieldName, GetType(fieldName)));
                    }
                }
            }
            return iterator;
        }

        #endregion

        #region Auxiliar Methods
        
        public string CRS
        {
            get
            {
                return _CRS;
            }
            set
            {
                _CRS = value;

                string[] t = _CRS.Split(':');

                if (t != null)
                {
                    if (t.Count() == 2)
                    {
                        if (t[0].ToUpper() == "EPSG")
                        {
                            int code = Convert.ToInt32(t[1]);

                            try
                            {
                                ProjectionInfo prj = ProjectionInfo.FromEpsgCode(code);
                                if (prj != null)
                                {
                                    Projection = prj;
                                    fea.Projection = prj;
                                }
                            }
                            catch
                            {
                            }

                        }
                    }
                    if (t.Count() == 6)
                    {
                        try
                        {
                            int code = Convert.ToInt32(t[5]);
                            ProjectionInfo prj = ProjectionInfo.FromEpsgCode(code);
                            if (prj != null)
                            {
                                Projection = prj;
                                fea.Projection = prj;
                            }
                        }
                        catch
                        {
                        }


                    }

                }
            }
        }

        private Type GetType(string fieldName)
        {
           if (fields[fieldName].ToUpper().Contains("DOUBLE"))
               return typeof(double);

           if (fields[fieldName].ToUpper().Contains("STRING"))
               return typeof(string);

           if (fields[fieldName].ToUpper().Contains("LONG"))
               return typeof(long);

           if (fields[fieldName].ToUpper().Contains("INT"))
               return typeof(int);

           return typeof(string);
        }
       
        private XmlDocument GetXml(Stream stream)
        {
            try
            {
                var r = new XmlTextReader(stream);
                r.XmlResolver = null;

                var doc = new XmlDocument();

                doc.XmlResolver = null;
                doc.Load(r);

                stream.Close();

                return doc;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not download XML", ex);
            }
        }
        private static Stream GetRemoteXmlStream(Uri uri, WebProxy proxy)
        {
            WebRequest myRequest = WebRequest.Create(uri);
            if (proxy != null) myRequest.Proxy = proxy;

            WebResponse myResponse = myRequest.GetResponse();
            Stream stream = myResponse.GetResponseStream();

            return stream;
        }
        private void ParseCapabilities______________________(XmlDocument doc)
        {
            //_nsmgr = new XmlNamespaceManager(doc.NameTable);

            //if (doc.DocumentElement.Attributes["version"] != null)
            //{
            //    Version = doc.DocumentElement.Attributes["version"].Value;

            //    if (Version != "1.0.0" && Version != "1.1.0" && Version != "1.1.1" && Version != "1.3.0")
            //        throw new ApplicationException("WFS Version " + Version + " not supported");

            //    _nsmgr.AddNamespace(String.Empty, "http://www.opengis.net/wms");
            //    _nsmgr.AddNamespace("sm", Version == "1.3.0" ? "http://www.opengis.net/wms" : "");
            //    _nsmgr.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
            //    _nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //}
            //else
            //    throw (new ApplicationException("No service version number found!"));


            //XmlNode xnService = doc.DocumentElement.SelectSingleNode("sm:Service", _nsmgr);
            //XmlNode xnCapability = doc.DocumentElement.SelectSingleNode("sm:Capability", _nsmgr);
            //if (xnService != null)
            //    ParseServiceDescription(xnService, _nsmgr);
            //else
            //    throw (new ApplicationException("No service tag found!"));


            //if (xnCapability != null)
            //    ParseCapability(xnCapability);
            //else
            //    throw (new ApplicationException("No capability tag found!"));

        }
        
        #endregion



    }

}
