// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
using NetTopologySuite.Geometries;
using Renci.Data.Interop.OpenGIS.Gml;
using Renci.Data.Interop.OpenGIS.Wfs;
using NtsPoint = NetTopologySuite.Geometries.Point;

namespace DotSpatial.Plugins.WFSClient.Classes
{
    /// <summary>
    /// WfsClient.
    /// </summary>
    internal class WfsClient
    {
        #region Fields

        private string _crs;

        private XmlNamespaceManager _nsmgr;
        private FeatureType _typeGeometry;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the CRS.
        /// </summary>
        public string Crs
        {
            get
            {
                return _crs;
            }

            set
            {
                _crs = value;
                string[] t = _crs.Split(':');

                if (t.Length == 2)
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
                                Fea.Projection = prj;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                else if (t.Length == 6)
                {
                    try
                    {
                        int code = Convert.ToInt32(t[5]);
                        ProjectionInfo prj = ProjectionInfo.FromEpsgCode(code);
                        if (prj != null)
                        {
                            Projection = prj;
                            Fea.Projection = prj;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the FeatureSet.
        /// </summary>
        public FeatureSet Fea { get; set; }

        /// <summary>
        /// Gets or sets the fields.
        /// </summary>
        public Dictionary<string, string> Fields { get; set; }

        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        public string Geometry { get; set; }

        /// <summary>
        /// Gets or sets the projection.
        /// </summary>
        public ProjectionInfo Projection { get; set; }

        /// <summary>
        /// Gets or sets the proxy.
        /// </summary>
        public WebProxy Proxy { get; set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the WfsCapabilitiesType.
        /// </summary>
        public WfsCapabilitiesType Wfs { get; set; }

        /// <summary>
        /// Gets or sets the xml.
        /// </summary>
        public string Xml { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether the geographic field is valid.
        /// </summary>
        /// <param name="geographicField">The geographic field to check.</param>
        /// <returns>True, if geographicField is gml:PointPropertyType or gml:MultiSurfacePropertyType or gml:MultiLineStringPropertyType.</returns>
        public static bool IsGeographicFieldValid(string geographicField)
        {
            return geographicField == "gml:PointPropertyType" || geographicField == "gml:MultiSurfacePropertyType" || geographicField == "gml:MultiLineStringPropertyType";
        }

        /// <summary>
        /// Creates a capabilities request.
        /// </summary>
        /// <param name="url">Url of the server.</param>
        /// <returns>The created request.</returns>
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

        /// <summary>
        /// Reads the capabilities.
        /// </summary>
        /// <param name="server">Server to read the capabilities from.</param>
        public void ReadCapabilities(string server = "")
        {
            if (server != string.Empty)
                Server = server;

            Uri u = new(Server);

            Stream stream;
            if (u.IsAbsoluteUri && u.IsFile)
            {
                // assume web if relative because IsFile is not supported on relative paths
                stream = File.OpenRead(u.LocalPath);
            }
            else
            {
                Uri = new Uri(CreateCapabiltiesRequest(Server));
                stream = GetRemoteXmlStream(Uri, Proxy);
            }

            ParseGetCapabilities(stream);
        }

        /// <summary>
        /// Reads the DescribeFeatureType.
        /// </summary>
        /// <param name="server">Server to read the DescribeFeatureType from.</param>
        public void ReadDescribeFeatureType(string server = "")
        {
            foreach (var ope in Wfs.OperationsMetadata.Operations)
            {
                if (ope.Name == "DescribeFeatureType")
                {
                    foreach (var typ in ope.Dcps)
                    {
                        server = typ.Item.GetMethods[0].Href;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(server))
                Server = server;

            Uri u = new(Server);

            Stream stream;
            if (u.IsAbsoluteUri && u.IsFile)
            {
                // assume web if relative because IsFile is not supported on relative paths
                stream = File.OpenRead(u.LocalPath);
            }
            else
            {
                Uri = new Uri(CreateDescribeFeatureRequest(Server));
                stream = GetRemoteXmlStream(Uri, Proxy);
            }

            XmlDocument xml = GetXml(stream);
            ParseDescribeFeatureType(xml);
        }

        /// <summary>
        /// Reads a feature from the given server.
        /// </summary>
        /// <param name="server">Server to read a feature from.</param>
        public void ReadFeature(string server)
        {
            foreach (var ope in Wfs.OperationsMetadata.Operations)
            {
                if (ope.Name == "GetFeature")
                {
                    foreach (var typ in ope.Dcps)
                    {
                        server = typ.Item.GetMethods[0].Href;
                    }
                }
            }

            if (server != string.Empty)
                Server = server;

            Uri u = new(Server);
            Stream stream;
            if (u.IsAbsoluteUri && u.IsFile)
            {
                // assume web if relative because IsFile is not supported on relative paths
                stream = File.OpenRead(u.LocalPath);
            }
            else
            {
                Uri = new Uri(CreateFeatureRequest(Server));
                stream = GetRemoteXmlStream(Uri, Proxy);
            }

            XmlDocument xml = GetXml(stream);
            ParseFeature(xml, Uri);
        }

        private static Coordinate[] ExtractCoordinates(DirectPositionListType rings)
        {
            string[] listpoints = rings.Text.Split(' ');
            List<Coordinate> lstCoor = new();

            for (int i = 0; i < listpoints.Length; i += 2)
            {
                lstCoor.Add(new Coordinate(Convert.ToDouble(listpoints[i], CultureInfo.InvariantCulture), Convert.ToDouble(listpoints[i + 1], CultureInfo.InvariantCulture)));
            }

            return lstCoor.ToArray();
        }

        private static void ExtractInteriorPolygon(LinearRing[] holes, PolygonType sur)
        {
            Collection<AbstractRingPropertyType> lin = sur.Interior;
            int i = 0;
            foreach (AbstractRingPropertyType ringis in lin)
            {
                if (ringis.Ring is LinearRingType lii)
                {
                    foreach (DirectPositionListType rings in lii.Items)
                    {
                        var lstCoor = ExtractCoordinates(rings);

                        holes[i] = new LinearRing(lstCoor);
                        i++;
                    }
                }
            }
        }

        private static PolygonType ExtractShellPolygon(ref LinearRing shell, SurfacePropertyType member)
        {
            PolygonType sur = member.Surface as PolygonType;
            if (sur?.Exterior.Ring is LinearRingType li)
            {
                foreach (DirectPositionListType rings in li.Items)
                {
                    var lstCoor = ExtractCoordinates(rings);

                    shell = new LinearRing(lstCoor);
                }
            }

            return sur;
        }

        private static Stream GetRemoteXmlStream(Uri uri, WebProxy proxy)
        {
            WebRequest myRequest = WebRequest.Create(uri);
            if (proxy != null) myRequest.Proxy = proxy;

            WebResponse myResponse = myRequest.GetResponse();

            StreamReader streamReader = new(myResponse.GetResponseStream(), true);
            try
            {
                var target = streamReader.ReadToEnd();
            }
            finally
            {
                streamReader.Close();
            }


            Stream stream = myResponse.GetResponseStream();

            return stream;
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
                    strReq = new StringBuilder(url.Substring(0, url.IndexOf("?", StringComparison.Ordinal)));
                    strReq.Append("?");
                }
            }

            if (!strReq.ToString().EndsWith("&") && !strReq.ToString().EndsWith("?"))
                strReq.Append("&");

            if (!url.ToLower().Contains("service=wfs"))
                strReq.AppendFormat("SERVICE=WFS&");

            if (!url.ToLower().Contains("request=describefeatureType"))
                strReq.AppendFormat("REQUEST=DescribeFeatureType&");

            if (!url.ToLower().Contains("version=") && Wfs.Version != null)
                strReq.AppendFormat("version=" + Wfs.Version + "&");

            if (!url.ToLower().Contains("TypeName=") && TypeName != null)
                strReq.AppendFormat("TypeName=" + TypeName + "&");
            return strReq.ToString();
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
                    strReq = new StringBuilder(url.Substring(0, url.IndexOf("?", StringComparison.Ordinal)));
                    strReq.Append("?");
                }
            }

            if (!strReq.ToString().EndsWith("&") && !strReq.ToString().EndsWith("?"))
                strReq.Append("&");

            if (!url.ToLower().Contains("service=wfs"))
                strReq.AppendFormat("SERVICE=WFS&");

            if (!url.ToLower().Contains("request=getfeature"))
                strReq.AppendFormat("REQUEST=GetFeature&");

            if (!url.ToLower().Contains("version=") && Wfs.Version != null)
                strReq.AppendFormat("version=" + Wfs.Version + "&");

            if (!url.ToLower().Contains("TypeName=") && TypeName != null)
                strReq.AppendFormat("TypeName=" + TypeName);
            return strReq.ToString();
        }

        private XPathNodeIterator CreateFields(XPathNavigator nav, FeatureType type)
        {
            string exp = @"/wfs:FeatureCollection/child::*[name() = 'gml:featureMember' or name() = 'gml:featureMembers']/child::*";
            XPathNodeIterator iterator = nav.Select(exp, _nsmgr);
            Fea = new FeatureSet(type);
            if (iterator.Count > 0)
            {
                foreach (string fieldName in Fields.Keys)
                {
                    if (fieldName != Geometry)
                    {
                        Fea.DataTable.Columns.Add(new DataColumn(fieldName, GetType(fieldName)));
                    }
                }
            }

            return iterator;
        }

        private IFeature ExtractGeographicData(XmlNode c)
        {
            string geoData = string.Empty;
            Geometry geo = null;
            if (_typeGeometry == FeatureType.Point)
            {
                foreach (XmlNode e in c)
                {
                    if (e.LocalName == Geometry)
                    {
                        geoData = e.InnerText;
                    }
                }

                string point = Convert.ToString(geoData);
                var pointValue = point.Split(' ');
                geo = new NtsPoint(
                    Convert.ToDouble(pointValue[0], CultureInfo.InvariantCulture),
                    Convert.ToDouble(pointValue[1], CultureInfo.InvariantCulture));
            }

            if (_typeGeometry == FeatureType.Polygon)
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

            if (_typeGeometry == FeatureType.Line)
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

            IFeature feat = Fea.AddFeature(geo);

            return feat;
        }

        private LinearRing ExtractLineString(LineStringPropertyType member)
        {
            var membe1 = member.LineString;
            foreach (DirectPositionListType rings in membe1.Items)
            {
                var lstCoor = ExtractCoordinates(rings);
                return new LinearRing(lstCoor);
            }

            return null;
        }

        private bool ExtractReference(XmlNode c)
        {
            if (_typeGeometry == FeatureType.Point || _typeGeometry == FeatureType.Line || _typeGeometry == FeatureType.Polygon)
            {
                foreach (XmlNode e in c)
                {
                    if (e.LocalName == Geometry)
                    {
                        if (e.FirstChild.Attributes != null) Crs = e.FirstChild.Attributes[1].InnerXml;
                        return false;
                    }
                }
            }

            return true;
        }

        private void GetGeometry()
        {
            if (Fields[Geometry] == "gml:PointPropertyType")
                _typeGeometry = FeatureType.Point;
            if (Fields[Geometry] == "gml:MultiSurfacePropertyType")
                _typeGeometry = FeatureType.Polygon;
            if (Fields[Geometry] == "gml:MultiLineStringPropertyType")
                _typeGeometry = FeatureType.Line;
        }

        private Geometry GetPolygon(MultiSurfaceType multi)
        {
            var p = new Polygon[multi.SurfaceMemberItems.Count];

            int npoly = 0;
            foreach (SurfacePropertyType member in multi.SurfaceMemberItems)
            {
                LinearRing shell = null;
                PolygonType sur = ExtractShellPolygon(ref shell, member);

                if (sur.Interior.Count == 0 && shell != null)
                {
                    p[npoly] = new Polygon(shell);
                }
                else
                {
                    var holes = new LinearRing[sur.Interior.Count];
                    ExtractInteriorPolygon(holes, sur);
                    p[npoly] = new Polygon(shell, holes);
                }

                npoly++;
            }

            return new MultiPolygon(p);
        }

        private Geometry GetPolyline(MultiLineStringType multi)
        {
            var lines = new LineString[multi.LineStringMembers.Count];
            int nLin = 0;
            foreach (LineStringPropertyType member in multi.LineStringMembers)
            {
                lines[nLin] = ExtractLineString(member);
                nLin++;
            }

            return new MultiLineString(lines);
        }

        private Type GetType(string fieldName)
        {
            if (Fields[fieldName].ToUpper().Contains("DOUBLE"))
                return typeof(double);

            if (Fields[fieldName].ToUpper().Contains("STRING"))
                return typeof(string);

            if (Fields[fieldName].ToUpper().Contains("LONG"))
                return typeof(long);

            if (Fields[fieldName].ToUpper().Contains("INT"))
                return typeof(int);

            return typeof(string);
        }

        private XmlDocument GetXml(Stream stream)
        {
            try
            {
                var r = new XmlTextReader(stream)
                {
                    XmlResolver = null
                };

                var doc = new XmlDocument
                {
                    XmlResolver = null
                };

                doc.Load(r);
                stream.Close();
                return doc;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not download XML", ex);
            }
        }

        private void ParseCapabilities(XmlDocument doc)
        {
            //// _nsmgr = new XmlNamespaceManager(doc.NameTable);

            //// if (doc.DocumentElement.Attributes["version"] != null)
            ////{
            ////    Version = doc.DocumentElement.Attributes["version"].Value;

            ////    if (Version != "1.0.0" && Version != "1.1.0" && Version != "1.1.1" && Version != "1.3.0")
            ////        throw new ApplicationException("WFS Version " + Version + " not supported");

            ////    _nsmgr.AddNamespace(String.Empty, "http://www.opengis.net/wms");
            ////    _nsmgr.AddNamespace("sm", Version == "1.3.0" ? "http://www.opengis.net/wms" : "");
            ////    _nsmgr.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
            ////    _nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            ////}
            //// else
            ////    throw (new ApplicationException("No service version number found!"));

            //// XmlNode xnService = doc.DocumentElement.SelectSingleNode("sm:Service", _nsmgr);
            //// XmlNode xnCapability = doc.DocumentElement.SelectSingleNode("sm:Capability", _nsmgr);
            //// if (xnService != null)
            ////    ParseServiceDescription(xnService, _nsmgr);
            //// else
            ////    throw (new ApplicationException("No service tag found!"));

            //// if (xnCapability != null)
            ////    ParseCapability(xnCapability);
            //// else
            ////    throw (new ApplicationException("No capability tag found!"));
        }

        private void ParseDescribeFeatureType(XmlDocument doc)
        {
            Fields = new Dictionary<string, string>();

            if (doc.DocumentElement == null) return;

            Xml = doc.InnerXml;
            _nsmgr = new XmlNamespaceManager(doc.NameTable);

            foreach (XmlNode nodes in doc.DocumentElement.Attributes)
            {
                if (nodes.Prefix == "xmlns")
                    _nsmgr.AddNamespace(nodes.Name, nodes.NamespaceURI);
            }

            var t = doc.DocumentElement["xsd:complexType"] ?? doc.DocumentElement["complexType"];
            if (t == null) return;

            var complexContent = t["xsd:complexContent"] ?? t["complexContent"];
            if (complexContent == null) return;

            var extension = complexContent["xsd:extension"] ?? complexContent["extension"];
            if (extension == null) return;

            var sequence = extension["xsd:sequence"] ?? extension["sequence"];
            if (sequence == null) return;

            foreach (XmlNode ele in sequence)
            {
                if (ele.Attributes != null) Fields.Add(ele.Attributes["name"].Value, ele.Attributes["type"].Value);
            }
        }

        private void ParseFeature(XmlDocument xdoc, Uri uri)
        {
            Crs = string.Empty;

            Xml = xdoc.InnerXml;
            XPathNavigator nav = xdoc.CreateNavigator();
            _nsmgr.AddNamespace("wfs", "http://www.opengis.net/wfs");

            if (xdoc.DocumentElement == null) return;

            string number;
            try
            {
                number = xdoc.DocumentElement.Attributes["numberOfFeatures"].Value;
            }
            catch
            {
                return;
            }

            if (Convert.ToInt32(number) == 0) return;

            XPathNodeIterator iterator = SelectTypeGeometry(nav);

            try
            {
                bool getGeoReference = true;
                while (iterator.MoveNext())
                {
                    XPathNavigator node = iterator.Current;
                    var nod = node.UnderlyingObject;
                    XmlNode c = (XmlNode)nod;

                    if (getGeoReference)
                        getGeoReference = ExtractReference(c);

                    IFeature feat = ExtractGeographicData(c);

                    if (c != null)
                    {
                        foreach (XmlNode e in c)
                        {
                            if (e.LocalName != Geometry && Fields.Keys.Contains(e.LocalName))
                                feat.DataRow[e.LocalName] = e.InnerText;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ParseGetCapabilities(Stream stream)
        {
            XmlSerializer serializer = new(typeof(WfsCapabilitiesType));
            Wfs = (WfsCapabilitiesType)serializer.Deserialize(stream);
        }

        private XPathNodeIterator SelectTypeGeometry(XPathNavigator nav)
        {
            GetGeometry();
            var iterator = CreateFields(nav, _typeGeometry);
            return iterator;
        }

        #endregion
    }
}