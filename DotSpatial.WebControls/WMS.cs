using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml;
using System.Globalization;
using DotSpatial.Data;
using System.IO;
using System.Net;
using System.Drawing;
//using System.Windows.Forms;
using System.Diagnostics;
using DotSpatial.Controls;
using DotSpatial.WebControls;
using System.Web.UI.WebControls;
using DotSpatial.Projections;

namespace DotSpatial.MapWebClient
{

    public class WMSClient : IWebClient
    {
        public string Server { get; set; }
        public WebProxy Proxy { get; set; }

        public string Version { get; set; }

        private string _CRS;
        public string CRS 
        { 
            get
            {
                return _CRS;
            }
            set
            {
                _CRS = value;

                string [] t = _CRS.Split(':');

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
                                if (prj != null) Projection = prj;
                            }
                            catch
                            {
                            }
                            
                        }
                    }
                }
            }
        }


        public ProjectionInfo Projection
        {
            get;
            set;
        }

        public double Opacity { get; set; }


        private ServiceDescription _serviceDescription;
        public ServiceDescription ServiceDescription
        {
            get
            {
                return _serviceDescription;
            }
            set
            {
                _serviceDescription = value;
            }
        }

        private ServerLayer _layer;
        public ServerLayer Layer
        {
            get
            {
                return _layer;
            }
            set
            {
                _layer = value;
            }
        }

        private XmlNode _vendorSpecificCapabilities;
        private string[] _exceptionFormats;

        private XmlNamespaceManager _nsmgr;
        private WmsOnlineResource[] _getMapRequests;
        private Collection<string> _getMapOutputFormats;

        public WMSClient()
        {
            Opacity = 100;
            _layer = new ServerLayer();

        }

        public void ReadCapabilities(string server="")
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
                Uri uri = new Uri(CreateCapabiltiesRequest(Server));
                stream = GetRemoteXmlStream(uri, Proxy);
            }

           
            XmlDocument xml = GetXml(stream);
            ParseCapabilities(xml);

        }

        public string CreateCapabiltiesRequest(string url)
        {
            var strReq = new StringBuilder(url);
            
            if (!url.Contains("?"))
                strReq.Append("?");

            if (!strReq.ToString().EndsWith("&") && !strReq.ToString().EndsWith("?"))
                strReq.Append("&");

            if (!url.ToLower().Contains("service=wms"))
                strReq.AppendFormat("SERVICE=WMS&");

            if (!url.ToLower().Contains("request=getcapabilities"))
                strReq.AppendFormat("REQUEST=GetCapabilities&");

            if (!url.ToLower().Contains("version=") && Version !=null )
                strReq.AppendFormat("version="+Version+"&");

            return strReq.ToString();
        }

        private static Stream GetRemoteXmlStream(Uri uri, WebProxy proxy)
        {
            WebRequest myRequest = WebRequest.Create(uri);
            if (proxy != null) myRequest.Proxy = proxy;

            WebResponse myResponse = myRequest.GetResponse();
            Stream stream = myResponse.GetResponseStream();

            return stream;
        }

        /// <summary>
        /// Downloads servicedescription from WMS service
        /// </summary>
        /// <returns>XmlDocument from Url. Null if Url is empty or inproper XmlDocument</returns>
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
                throw new ApplicationException("Could not download capabilities", ex);
            }
        }


        private void ParseCapabilities(XmlDocument doc)
        {
            _nsmgr = new XmlNamespaceManager(doc.NameTable);

            if (doc.DocumentElement.Attributes["version"] != null)
            {
                Version = doc.DocumentElement.Attributes["version"].Value;

                if (Version != "1.0.0" && Version != "1.1.0" && Version != "1.1.1" && Version != "1.3.0")
                    throw new ApplicationException("WMS Version " + Version + " not supported");

                _nsmgr.AddNamespace(String.Empty, "http://www.opengis.net/wms");
                _nsmgr.AddNamespace("sm", Version == "1.3.0" ? "http://www.opengis.net/wms" : "");
                _nsmgr.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
                _nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            }
            else
                throw (new ApplicationException("No service version number found!"));


            XmlNode xnService = doc.DocumentElement.SelectSingleNode("sm:Service", _nsmgr);
            XmlNode xnCapability = doc.DocumentElement.SelectSingleNode("sm:Capability", _nsmgr);
            if (xnService != null)
                ParseServiceDescription(xnService, _nsmgr);
            else
                throw (new ApplicationException("No service tag found!"));


            if (xnCapability != null)
                ParseCapability(xnCapability);
            else
                throw (new ApplicationException("No capability tag found!"));

        }



        public Extent GetMaxSize()
        {
            Extent x;

            if (CRS == "EPSG:4326")
            {
                x = _layer.LatLonBoundingBox;
            }
            else
            {
                int i = Array.IndexOf(_layer.Crs, CRS);

                x = _layer.CrsExtent[i];
            }

            return x;
        }


        /// <summary>
        /// Parses service description node
        /// </summary>
        /// <param name="xnlServiceDescription"></param>
        private void ParseServiceDescription(XmlNode xnlServiceDescription, XmlNamespaceManager _nsmgr)
        {
            XmlNode node = xnlServiceDescription.SelectSingleNode("sm:Title", _nsmgr);
            _serviceDescription.Title = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:OnlineResource/@xlink:href", _nsmgr);
            _serviceDescription.OnlineResource = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:Abstract", _nsmgr);
            _serviceDescription.Abstract = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:Fees", _nsmgr);
            _serviceDescription.Fees = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:AccessConstraints", _nsmgr);
            _serviceDescription.AccessConstraints = (node != null ? node.InnerText : null);

            XmlNodeList xnlKeywords = xnlServiceDescription.SelectNodes("sm:KeywordList/sm:Keyword", _nsmgr);
            if (xnlKeywords != null)
            {
                _serviceDescription.Keywords = new string[xnlKeywords.Count];
                for (var i = 0; i < xnlKeywords.Count; i++)
                    _serviceDescription.Keywords[i] = xnlKeywords[i].InnerText;
            }
            //Contact information
            _serviceDescription.ContactInformation = new ServiceDescription.WmsContactInformation();
            node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactAddress/sm:Address", _nsmgr);
            _serviceDescription.ContactInformation.Address.Address = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactAddress/sm:AddressType", _nsmgr);
            _serviceDescription.ContactInformation.Address.AddressType = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactAddress/sm:City", _nsmgr);
            _serviceDescription.ContactInformation.Address.City = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactAddress/sm:Country", _nsmgr);
            _serviceDescription.ContactInformation.Address.Country = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactAddress/sm:PostCode", _nsmgr);
            _serviceDescription.ContactInformation.Address.PostCode = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactElectronicMailAddress", _nsmgr);
            _serviceDescription.ContactInformation.Address.StateOrProvince = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactElectronicMailAddress", _nsmgr);
            _serviceDescription.ContactInformation.ElectronicMailAddress = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactFacsimileTelephone", _nsmgr);
            _serviceDescription.ContactInformation.FacsimileTelephone = (node != null ? node.InnerText : null);
            node =
                xnlServiceDescription.SelectSingleNode(
                    "sm:ContactInformation/sm:ContactPersonPrimary/sm:ContactOrganisation", _nsmgr);
            _serviceDescription.ContactInformation.PersonPrimary.Organisation = (node != null ? node.InnerText : null);
            node =
                xnlServiceDescription.SelectSingleNode(
                    "sm:ContactInformation/sm:ContactPersonPrimary/sm:ContactPerson", _nsmgr);
            _serviceDescription.ContactInformation.PersonPrimary.Person = (node != null ? node.InnerText : null);
            node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactVoiceTelephone", _nsmgr);
            _serviceDescription.ContactInformation.VoiceTelephone = (node != null ? node.InnerText : null);
        }

        /// <summary>
        /// Parses capability node
        /// </summary>
        /// <param name="xnCapability"></param>
        private void ParseCapability(XmlNode xnCapability)
        {
            XmlNode xnRequest = xnCapability.SelectSingleNode("sm:Request", _nsmgr);
            if (xnRequest == null)
                throw (new Exception("Request parameter not specified in Service Description"));
            ParseRequest(xnRequest);
            XmlNode xnLayer = xnCapability.SelectSingleNode("sm:Layer", _nsmgr);
            if (xnLayer == null)
                throw (new Exception("No layer tag found in Service Description"));
            _layer = ParseLayer(xnLayer);

            XmlNode xnException = xnCapability.SelectSingleNode("sm:Exception", _nsmgr);
            if (xnException != null)
                ParseExceptions(xnException);

            _vendorSpecificCapabilities = xnCapability.SelectSingleNode("sm:VendorSpecificCapabilities", _nsmgr);
        }

        /// <summary>
        /// Parses valid exceptions
        /// </summary>
        /// <param name="xnlExceptionNode"></param>
        private void ParseExceptions(XmlNode xnlExceptionNode)
        {
            XmlNodeList xnlFormats = xnlExceptionNode.SelectNodes("sm:Format", _nsmgr);
            if (xnlFormats != null)
            {
                _exceptionFormats = new string[xnlFormats.Count];
                for (int i = 0; i < xnlFormats.Count; i++)
                {
                    _exceptionFormats[i] = xnlFormats[i].InnerText;
                }
            }
        }

        /// <summary>
        /// Parses request node
        /// </summary>
        /// <param name="xmlRequestNode"></param>
        private void ParseRequest(XmlNode xmlRequestNode)
        {
            XmlNode xnGetMap = xmlRequestNode.SelectSingleNode("sm:GetMap", _nsmgr);
            ParseGetMapRequest(xnGetMap);
            //TODO: figure out what we need to do with lines below:
            //XmlNode xnGetFeatureInfo = xmlRequestNodes.SelectSingleNode("sm:GetFeatureInfo", nsmgr);
            //XmlNode xnCapa = xmlRequestNodes.SelectSingleNode("sm:GetCapabilities", nsmgr); <-- We don't really need this do we?			
        }

        /// <summary>
        /// Parses GetMap request nodes
        /// </summary>
        /// <param name="getMapRequestNodes"></param>
        private void ParseGetMapRequest(XmlNode getMapRequestNodes)
        {
            XmlNode xnlHttp = getMapRequestNodes.SelectSingleNode("sm:DCPType/sm:HTTP", _nsmgr);
            if (xnlHttp != null && xnlHttp.HasChildNodes)
            {
                _getMapRequests = new WmsOnlineResource[xnlHttp.ChildNodes.Count];
                for (int i = 0; i < xnlHttp.ChildNodes.Count; i++)
                {
                    var wor = new WmsOnlineResource();
                    wor.Type = xnlHttp.ChildNodes[i].Name;
                    wor.OnlineResource =
                        xnlHttp.ChildNodes[i].SelectSingleNode("sm:OnlineResource", _nsmgr).Attributes["xlink:href"].
                            InnerText;
                    _getMapRequests[i] = wor;
                }
            }
            XmlNodeList xnlFormats = getMapRequestNodes.SelectNodes("sm:Format", _nsmgr);
            //_GetMapOutputFormats = new Collection<string>(xnlFormats.Count);
            _getMapOutputFormats = new Collection<string>();
            for (int i = 0; i < xnlFormats.Count; i++)
                _getMapOutputFormats.Add(xnlFormats[i].InnerText);
        }

        /// <summary>
        /// Iterates through the layer nodes recursively
        /// </summary>
        /// <param name="xmlLayer"></param>
        /// <returns></returns>
        private ServerLayer ParseLayer(XmlNode xmlLayer)
        {
            var layer = new ServerLayer();
            XmlNode node = xmlLayer.SelectSingleNode("sm:Name", _nsmgr);
            layer.Name = (node != null ? node.InnerText : null);
            node = xmlLayer.SelectSingleNode("sm:Title", _nsmgr);
            layer.Title = (node != null ? node.InnerText : null);
            node = xmlLayer.SelectSingleNode("sm:Abstract", _nsmgr);
            layer.Abstract = (node != null ? node.InnerText : null);
            XmlAttribute attr = xmlLayer.Attributes["queryable"];
            layer.Queryable = (attr != null && attr.InnerText == "1");


            XmlNodeList xnlKeywords = xmlLayer.SelectNodes("sm:KeywordList/sm:Keyword", _nsmgr);
            if (xnlKeywords != null)
            {
                layer.Keywords = new string[xnlKeywords.Count];
                for (int i = 0; i < xnlKeywords.Count; i++)
                    layer.Keywords[i] = xnlKeywords[i].InnerText;
            }

            if (Version == "1.3.0")
            {
                XmlNodeList xnlCrs = xmlLayer.SelectNodes("sm:CRS", _nsmgr);
                if (xnlCrs != null)
                {
                    layer.Crs = new string[xnlCrs.Count];
                    for (int i = 0; i < xnlCrs.Count; i++)
                        layer.Crs[i] = xnlCrs[i].InnerText;
                }
            }
            else
            {
                XmlNodeList xnlCrs = xmlLayer.SelectNodes("sm:SRS", _nsmgr);
                if (xnlCrs != null)
                {
                    layer.Crs = new string[xnlCrs.Count];
                    for (int i = 0; i < xnlCrs.Count; i++)
                        layer.Crs[i] = xnlCrs[i].InnerText;
                }
            }


            XmlNodeList xnlStyle = xmlLayer.SelectNodes("sm:Style", _nsmgr);
            if (xnlStyle != null)
            {
                layer.Style = new LayerStyle[xnlStyle.Count];
                for (int i = 0; i < xnlStyle.Count; i++)
                {
                    node = xnlStyle[i].SelectSingleNode("sm:Name", _nsmgr);
                    layer.Style[i].Name = (node != null ? node.InnerText : null);
                    node = xnlStyle[i].SelectSingleNode("sm:Title", _nsmgr);
                    layer.Style[i].Title = (node != null ? node.InnerText : null);
                    node = xnlStyle[i].SelectSingleNode("sm:Abstract", _nsmgr);
                    layer.Style[i].Abstract = (node != null ? node.InnerText : null);
                    node = xnlStyle[i].SelectSingleNode("sm:LegendUrl", _nsmgr);
                    if (node != null)
                    {
                        layer.Style[i].LegendUrl = new WmsStyleLegend();
                        layer.Style[i].LegendUrl.Width = int.Parse(node.Attributes["width"].InnerText);
                        layer.Style[i].LegendUrl.Width = int.Parse(node.Attributes["height"].InnerText);
                        layer.Style[i].LegendUrl.OnlineResource.OnlineResource =
                            node.SelectSingleNode("sm:OnlineResource", _nsmgr).Attributes["xlink:href"].InnerText;
                        layer.Style[i].LegendUrl.OnlineResource.Type =
                            node.SelectSingleNode("sm:Format", _nsmgr).InnerText;
                    }
                    node = xnlStyle[i].SelectSingleNode("sm:StyleSheetURL", _nsmgr);
                    if (node != null)
                    {
                        layer.Style[i].StyleSheetUrl = new WmsOnlineResource();
                        layer.Style[i].StyleSheetUrl.OnlineResource =
                            node.SelectSingleNode("sm:OnlineResource", _nsmgr).Attributes["xlink:href"].InnerText;
                        //layer.Style[i].StyleSheetUrl.OnlineResource = node.SelectSingleNode("sm:Format", nsmgr).InnerText;
                    }
                }
            }

            XmlNodeList xnlLayers = xmlLayer.SelectNodes("sm:Layer", _nsmgr);
            if (xnlLayers != null)
            {
                //layer.ChildLayers = new WmsServerLayer[xnlLayers.Count];

                for (int i = 0; i < xnlLayers.Count; i++)
                    layer.ChildLayers.Add(ParseLayer(xnlLayers[i]));
            }

            if (Version == "1.3.0")
            {
                XmlNode GeographicBoundingBox = xmlLayer.SelectSingleNode("sm:EX_GeographicBoundingBox", _nsmgr);
                if (GeographicBoundingBox != null)
                {
                    double minx, miny, maxx, maxy;

                    XmlNode w = GeographicBoundingBox.SelectSingleNode("sm:westBoundLongitude", _nsmgr);
                    XmlNode e = GeographicBoundingBox.SelectSingleNode("sm:eastBoundLongitude", _nsmgr);
                    XmlNode s = GeographicBoundingBox.SelectSingleNode("sm:southBoundLatitude", _nsmgr);
                    XmlNode n = GeographicBoundingBox.SelectSingleNode("sm:northBoundLatitude", _nsmgr);

                    if (!double.TryParse(w.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out minx) &
                        !double.TryParse(s.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out miny) &
                        !double.TryParse(e.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out maxx) &
                        !double.TryParse(n.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out maxy))
                        throw new ArgumentException("Invalid LatLonBoundingBox on layer '" + layer.Name + "'");

                    layer.LatLonBoundingBox = new Extent(minx, miny, maxx, maxy);
                }
            }
            else
            {
                XmlNode GeographicBoundingBox = xmlLayer.SelectSingleNode("sm:LatLonBoundingBox", _nsmgr);
                if (GeographicBoundingBox != null)
                {
                    double minx, miny, maxx, maxy;

                    if (!double.TryParse(GeographicBoundingBox.Attributes["minx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out minx) &
                        !double.TryParse(GeographicBoundingBox.Attributes["miny"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out miny) &
                        !double.TryParse(GeographicBoundingBox.Attributes["maxx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxx) &
                        !double.TryParse(GeographicBoundingBox.Attributes["maxy"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxy))
                        throw new ArgumentException("Invalid LatLonBoundingBox on layer '" + layer.Name + "'");

                    layer.LatLonBoundingBox = new Extent(minx, miny, maxx, maxy);
                }

            }

            if (Version == "1.3.0")
            {
                XmlNodeList CrsExtent = xmlLayer.SelectNodes("sm:BoundingBox", _nsmgr);

                if (CrsExtent != null)
                {
                    layer.CrsExtent = new Extent[layer.Crs.Count()];

                    foreach (XmlNode nd in CrsExtent)
                    {
                        string CRS = nd.Attributes["CRS"].Value;

                        double minx, miny, maxx, maxy;

                        int i = Array.IndexOf(layer.Crs, CRS);

                        if (!double.TryParse(nd.Attributes["minx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out minx) &
                            !double.TryParse(nd.Attributes["miny"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out miny) &
                            !double.TryParse(nd.Attributes["maxx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxx) &
                            !double.TryParse(nd.Attributes["maxy"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxy))
                            throw new ArgumentException("Invalid BoundingBox on CRS on layer '" + layer.Name + "'");

                        layer.CrsExtent[i] = new Extent(minx, miny, maxx, maxy);
                    }

                }



                XmlNode xmlMaxScaleDenominator = xmlLayer.SelectSingleNode("sm:MaxScaleDenominator", _nsmgr);
                if (xmlMaxScaleDenominator != null)
                {
                    double.TryParse(xmlMaxScaleDenominator.InnerText, out layer.MaxScaleDenominator);
                }
                else
                {
                    layer.MaxScaleDenominator = 0;
                }

                XmlNode xmlMinScaleDenominator = xmlLayer.SelectSingleNode("sm:MinScaleDenominator", _nsmgr);
                if (xmlMinScaleDenominator != null)
                {
                    double.TryParse(xmlMinScaleDenominator.InnerText, out layer.MinScaleDenominator);
                }
                else
                {
                    layer.MinScaleDenominator = 0;
                }

            }
            else
            {

                XmlNodeList CrsExtent = xmlLayer.SelectNodes("sm:BoundingBox", _nsmgr);

                if (CrsExtent != null)
                {

                    int n = layer.Crs.Count();

                    if (n <= 0) n = 1;

                    layer.CrsExtent = new Extent[n];

                    foreach (XmlNode nd in CrsExtent)
                    {
                        string CRS = nd.Attributes["SRS"].Value;

                        double minx, miny, maxx, maxy;

                        int i = Array.IndexOf(layer.Crs, CRS);

                        if (i < 0) i = 0;

                        if (!double.TryParse(nd.Attributes["minx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out minx) &
                            !double.TryParse(nd.Attributes["miny"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out miny) &
                            !double.TryParse(nd.Attributes["maxx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxx) &
                            !double.TryParse(nd.Attributes["maxy"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxy))
                            throw new ArgumentException("Invalid BoundingBox on CRS on layer '" + layer.Name + "'");

                        layer.CrsExtent[i] = new Extent(minx, miny, maxx, maxy);
                    }

                }

                XmlNode ScaleHint = xmlLayer.SelectSingleNode("sm:ScaleHint", _nsmgr);

                if (ScaleHint != null)
                {
                    double min, max;

                    if (!double.TryParse(ScaleHint.Attributes["min"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out min) ||
                        !double.TryParse(ScaleHint.Attributes["max"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out max))
                    {
                        throw new ArgumentException("No hint scale on layer '" + layer.Name + "'");
                    }

                    layer.MinScaleHint = min;
                    layer.MaxScaleHint = max;
                }
            }


            return layer;
        }

        private List<ServerLayer> GetVisibleLayer(ServerLayer layer=null,List<ServerLayer> list = null)
        {
            if (list == null)
            {
                list = new List<ServerLayer>();
            }

            if (layer == null) layer = _layer;
            
            if (layer.visible == true)
            {
                    if (layer.Name != null) list.Add(layer);

                    if (layer.ChildLayers != null)
                    {
                        foreach (ServerLayer l in layer.ChildLayers)
                        {
                            GetVisibleLayer(l, list);
                        }
                    }

            }


            return list;
        }



        //private List<ServerLayer> GetLayerFromHint(double Hint)
        //{
        //    List<ServerLayer> list = new List<ServerLayer>();

        //    if (_layer.visible == true)
        //    {
        //        //if (Hint >= _layer.MinScaleHint && Hint <= _layer.MaxScaleHint)
        //        {
        //            if (_layer.Name != null) list.Add(_layer);
        //        }

        //        CicleLayerFromInt(Hint, _layer, ref list);
        //    }


        //    return list;
        //}

        //private void CicleLayerFromInt(double Hint, ServerLayer layer, ref List<ServerLayer> list)
        //{

        //    if (layer.ChildLayers != null)
        //    {
        //        foreach (ServerLayer l in layer.ChildLayers)
        //        {
        //            if (l.visible == true)
        //            {
        //                //if (Hint >= l.MinScaleHint && Hint <= l.MaxScaleHint)
        //                {
        //                    if (l.Name != null) list.Add(l);
        //                }

        //                CicleLayerFromInt(Hint, l, ref list);

        //            }
        //        }
        //    }

        //}

        string[] keysPop(string[] keys)
        {
            string[] k = new string[keys.Count() - 1];

            Array.Copy(keys, 1, k, 0, k.Count());

            return k;
        }

        public void CheckLayer(string[] keys, bool check, ServerLayer l = null)
        {
            if (keys.Count() == 0) return;

            if (l == null)
            {
                if (_layer.Title == keys[0])
                {
                    if (keys.Count() == 1)
                    {
                        if (_layer.Title == keys[0])
                        {
                            _layer.visible = check;
                            return;
                        }
                    }
                    else
                    {
                        string[] k = keysPop(keys);
                        CheckLayer(k, check, _layer);
                    }
                }
            }
            else
            {
                foreach (ServerLayer cl in l.ChildLayers)
                {

                    if (keys.Count() == 1)
                    {
                        if (cl.Title == keys[0])
                        {
                            cl.visible = check;
                            return;
                        }
                    }
                    else
                    {
                        if (cl.Title == keys[0])
                        {
                            string[] k = keysPop(keys);
                            CheckLayer(k, check, cl);
                        }
                    }
                }
            }

        }

        public void List(WebLegend tree, ServerLayer l = null, TreeNode parent = null)
        {
            if (l == null)
            {
                TreeNode tn = new TreeNode(_layer.Title);
                tn.ShowCheckBox = true;
                tn.Checked = _layer.visible;
                tn.Expand();

                if (_layer.Style != null)
                {
                    if (_layer.Style.Count() > 0)
                    {
                        if (_layer.Style[0].LegendUrl.OnlineResource.OnlineResource != "")
                        {
                            tn.ImageUrl = _layer.Style[0].LegendUrl.OnlineResource.OnlineResource;
                        }
                    }
                

                    List(tree, _layer, tn);
                }

                tree.Nodes.Add(tn);
            }
            else
            {
                //foreach (WmsServerLayer cl in l.ChildLayers)
                for (int i = l.ChildLayers.Count() - 1; i >= 0; i--)
                {
                    ServerLayer cl = l.ChildLayers[i];

                    TreeNode tn = new TreeNode(cl.Title);// + " " + cl.Name);
                    tn.ShowCheckBox = true;
                    tn.Checked = cl.visible;

                    List(tree, cl, tn);

                    tn.Expand();

                    parent.ChildNodes.Add(tn);

                }
            }
        }

        public string GetHTML(ref GDIMap m, Size size, string DivID)
        {
            string htm = "";

            int w = size.Width;
            int h = size.Height;

            Rectangle Rect = m.ProjToPixel(m.ViewExtents);

            Extent WmsEx = m.ViewExtents;

            if(Projection != m.Projection)
            {

                double[] xy = new double[4];

                xy[0] = m.ViewExtents.MinX;
                xy[1] = m.ViewExtents.MinY;
                xy[2] = m.ViewExtents.MaxX;
                xy[3] = m.ViewExtents.MaxY;

                double[] z = { };


                Projections.Reproject.ReprojectPoints(xy, z, m.Projection, Projection, 0, 2);

                WmsEx.MinX = Math.Min(xy[0], xy[2]);
                WmsEx.MinY = Math.Min(xy[1], xy[3]);

                WmsEx.MaxX = Math.Max(xy[0], xy[2]);
                WmsEx.MaxY = Math.Max(xy[1], xy[3]);

                if (double.IsNaN(WmsEx.MinX) | double.IsNaN(WmsEx.MinY) | double.IsNaN(WmsEx.MaxX) | double.IsNaN(WmsEx.MaxY))
                {
                    htm += "<div id=\"Back_" + DivID + "\" style=\"position:absolute; left:" + Rect.Left.ToString() + "px; top:" + Rect.Top.ToString() + "px; width:" + Rect.Width.ToString() + "px; height:" + Rect.Height.ToString() + "px; \">";
                    htm += "<p>Out of WMS zone</p>";
                    htm += "</div>";

                    return htm;
                }
            }

            htm += "<div id=\"Back_" + DivID + "\" style=\"position:absolute; left:" + Rect.Left.ToString() + "px; top:" + Rect.Top.ToString() + "px; width:" + Rect.Width.ToString() + "px; height:" + Rect.Height.ToString() + "px; \">";

            double Hint = Math.Sqrt(Math.Pow(m.ViewExtents.Width / size.Width, 2) + Math.Pow(m.ViewExtents.Height / size.Height, 2));

            List<ServerLayer> list = GetVisibleLayer();

            if (list == null)
            {
                htm += "<p>NO WMS LAYERS</p>";
            }
            else if (list != null & list.Count > 0)
            {


                int num = list.Count;

                string Lays = "";
                for (int i = 0; i < num - 1; i++)
                {
                    Lays += list[i].Name + ",";
                }
                Lays += list[num - 1].Name;


                string svr = Server;

                if (svr == null)
                {
                    throw new ApplicationException("WMS server must not be null");
                }

                if (svr.Contains('?'))
                    svr += "&";
                else
                    svr += "?";


                string f = svr;
                f += "Version=" + Version;
                f += "&REQUEST=GetMap";
                f += "&Layers=" + Lays;

                if (Version=="1.3.0")
                {
                    f += "&crs=" + CRS;
                }
                else
                {
                    f += "&srs=" + CRS;
                }
                
                f += "&format=image/png";

                f += "&styles=";

                f += "&bbox={0},{1},{2},{3}&width={4}&height={5}";

                f += "&TRANSPARENT=TRUE";

                //string t = string.Format(CultureInfo.InvariantCulture, f, m.ViewExtents.MinX, m.ViewExtents.MinY, m.ViewExtents.MaxX, m.ViewExtents.MaxY, size.Width, size.Height);
                string t = string.Format(CultureInfo.InvariantCulture, f, WmsEx.MinX, WmsEx.MinY, WmsEx.MaxX, WmsEx.MaxY, size.Width, size.Height);

                string o = "";
                if (Opacity != 100)
                {
                    //o = "filter:alpha(opacity=30);opacity: 0.3;";
                    o = "filter:alpha(opacity=" + Opacity.ToString() + "); opacity: " + (Opacity / 100).ToString(CultureInfo.InvariantCulture) + "; ";
                }

                htm += "<img alt=\"\" style=\"position:absolute; " + o + "left:0px; top:0px; width:" + size.Width.ToString() + "px; height:" + size.Height.ToString() + "px; \" src=\"" + t + "\" />";

            }
            
            htm += "</div>";

            return htm;

        }






        #region WMS Data structures

        #region Nested type: WmsLayerStyle

        /// <summary>
        /// Structure for storing information about a WMS Layer Style
        /// </summary>
        public struct LayerStyle
        {
            /// <summary>
            /// Abstract
            /// </summary>
            public string Abstract;

            /// <summary>
            /// Legend
            /// </summary>
            public WmsStyleLegend LegendUrl;

            /// <summary>
            /// Name
            /// </summary>
            public string Name;

            /// <summary>
            /// Style Sheet Url
            /// </summary>
            public WmsOnlineResource StyleSheetUrl;

            /// <summary>
            /// Title
            /// </summary>
            public string Title;
        }

        #endregion

        #region Nested type: WmsOnlineResource

        /// <summary>
        /// Structure for storing info on an Online Resource
        /// </summary>
        public struct WmsOnlineResource
        {
            /// <summary>
            /// URI of online resource
            /// </summary>
            public string OnlineResource;

            /// <summary>
            /// Type of online resource (Ex. request method 'Get' or 'Post')
            /// </summary>
            public string Type;
        }

        #endregion

        #region Nested type: WmsServerLayer

        /// <summary>
        /// Structure for holding information about a WMS Layer 
        /// </summary>
        public class ServerLayer
        {
            /// <summary>
            /// Abstract
            /// </summary>
            public string Abstract;

            /// <summary>
            /// Collection of child layers
            /// </summary>
            public List<ServerLayer> ChildLayers = new List<ServerLayer>();

            /// <summary>
            /// Coordinate Reference Systems supported by layer
            /// </summary>
            public string[] Crs;

            /// <summary>
            /// Bounding Box in each Coordinate Reference Systems supported by layer
            /// </summary>
            public Extent[] CrsExtent;

            /// <summary>
            /// Keywords
            /// </summary>
            public string[] Keywords;

            /// <summary>
            /// Latitudal/longitudal extent of this layer
            /// </summary>
            public Extent LatLonBoundingBox;

            /// <summary>
            /// Unique name of this layer used for requesting layer
            /// </summary>
            public string Name;

            /// <summary>
            /// Specifies whether this layer is queryable using GetFeatureInfo requests
            /// </summary>
            public bool Queryable;

            /// <summary>
            /// List of styles supported by layer
            /// </summary>
            public LayerStyle[] Style;

            /// <summary>
            /// Layer title
            /// </summary>
            public string Title;

            /// <summary>
            /// MinScaleDenominator
            /// </summary>
            public double MinScaleDenominator;

            /// <summary>
            /// MaxScaleDenominator
            /// </summary>
            public double MaxScaleDenominator;

            /// <summary>
            /// MinScaleHint
            /// </summary>
            public double MinScaleHint;

            /// <summary>
            /// MaxScaleHint
            /// </summary>
            public double MaxScaleHint;



            /// <summary>
            /// LayerVisible
            /// </summary>
            public bool visible = true;

        }

        #endregion

        #region Nested type: WmsStyleLegend

        /// <summary>
        /// Structure for storing WMS Legend information
        /// </summary>
        public struct WmsStyleLegend
        {
            /// <summary>
            /// Online resource for legend style 
            /// </summary>
            public WmsOnlineResource OnlineResource;

            /// <summary>
            /// Size of legend
            /// </summary>
            public int Width;
            public int Height;
        }

        #endregion

        #endregion


    }



    /// <summary>
    /// WMS service Description
    /// </summary>
    public struct ServiceDescription
    {
        /// <summary>
        /// Optional narrative description providing additional information
        /// </summary>
        public string Abstract;

        /// <summary>
        /// <para>The optional element "AccessConstraints" may be omitted if it do not apply to the server. If
        /// the element is present, the reserved word "none" (case-insensitive) shall be used if there are no
        /// access constraints, as follows: "none".</para>
        /// <para>When constraints are imposed, no precise syntax has been defined for the text content of these elements, but
        /// client applications may display the content for user information and action.</para>
        /// </summary>
        public string AccessConstraints;

        /// <summary>
        /// Optional WMS contact information
        /// </summary>
        public WmsContactInformation ContactInformation;

        /// <summary>
        /// The optional element "Fees" may be omitted if it do not apply to the server. If
        /// the element is present, the reserved word "none" (case-insensitive) shall be used if there are no
        /// fees, as follows: "none".
        /// </summary>
        public string Fees;

        /// <summary>
        /// Optional list of keywords or keyword phrases describing the server as a whole to help catalog searching
        /// </summary>
        public string[] Keywords;

        /// <summary>
        /// Maximum number of layers allowed (0=no restrictions)
        /// </summary>
        public int LayerLimit;

        /// <summary>
        /// Maximum height allowed in pixels (0=no restrictions)
        /// </summary>
        public int MaxHeight;

        /// <summary>
        /// Maximum width allowed in pixels (0=no restrictions)
        /// </summary>
        public int MaxWidth;

        /// <summary>
        /// Mandatory Top-level web address of service or service provider.
        /// </summary>
        public string OnlineResource;

        /// <summary>
        /// Mandatory Human-readable title for pick lists
        /// </summary>
        public string Title;

        /// <summary>
        /// Initializes a WmsServiceDescription object
        /// </summary>
        /// <param name="title">Mandatory Human-readable title for pick lists</param>
        /// <param name="onlineResource">Top-level web address of service or service provider.</param>
        public ServiceDescription(string title, string onlineResource)
        {
            Title = title;
            OnlineResource = onlineResource;
            Keywords = null;
            Abstract = "";
            ContactInformation = new WmsContactInformation();
            Fees = "";
            AccessConstraints = "";
            LayerLimit = 0;
            MaxWidth = 0;
            MaxHeight = 0;
        }

        public struct WmsContactInformation
        {
            /// <summary>
            /// Address
            /// </summary>
            public ContactAddress Address;

            /// <summary>
            /// Email address
            /// </summary>
            public string ElectronicMailAddress;

            /// <summary>
            /// Fax number
            /// </summary>
            public string FacsimileTelephone;

            /// <summary>
            /// Primary contact person
            /// </summary>
            public ContactPerson PersonPrimary;

            /// <summary>
            /// Position of contact person
            /// </summary>
            public string Position;

            /// <summary>
            /// Telephone
            /// </summary>
            public string VoiceTelephone;

            #region Nested type: ContactAddress

            /// <summary>
            /// Information about a contact address for the service.
            /// </summary>
            public struct ContactAddress
            {
                /// <summary>
                /// Contact address
                /// </summary>
                public string Address;

                /// <summary>
                /// Type of address (usually "postal").
                /// </summary>
                public string AddressType;

                /// <summary>
                /// Contact City
                /// </summary>
                public string City;

                /// <summary>
                /// Country of contact address
                /// </summary>
                public string Country;

                /// <summary>
                /// Zipcode of contact
                /// </summary>
                public string PostCode;

                /// <summary>
                /// State or province of contact
                /// </summary>
                public string StateOrProvince;
            }

            #endregion

            #region Nested type: ContactPerson

            /// <summary>
            /// Information about a contact person for the service.
            /// </summary>
            public struct ContactPerson
            {
                /// <summary>
                /// Organisation of primary person
                /// </summary>
                public string Organisation;

                /// <summary>
                /// Primary contact person
                /// </summary>
                public string Person;
            }

            #endregion
        }

    }
    # region XXX

    //public class WMSCapabilities
    //{
    //    #region Fields

    //    private string _server;
    //    private string _CRS = "EPSG:4326"; //default to WGS84
    //    private double _opacity = 100;

    //    private XmlNode _vendorSpecificCapabilities;
    //    private XmlNamespaceManager _nsmgr;
    //    private string[] _exceptionFormats;
    //    private Collection<string> _getMapOutputFormats;
    //    private WmsOnlineResource[] _getMapRequests;
    //    private WmsServerLayer _layer;
    //    private WmsServiceDescription _serviceDescription;
    //    private string _version;

    //    #endregion

    //    #region Properties

    //    /// <summary>
    //    /// Opacity
    //    /// </summary>
    //    public double Opacity
    //    {
    //        get { return _opacity; }
    //        set { _opacity = value; }
    //    }

    //    /// <summary>
    //    /// Exposes the capabilitie's VendorSpecificCapabilities as XmlNode object. External modules 
    //    /// could use this to parse the vendor specific capabilities for their specific purpose.
    //    /// </summary>
    //    public XmlNode VendorSpecificCapabilities
    //    {
    //        get { return _vendorSpecificCapabilities; }
    //    }

    //    /// <summary>
    //    /// Gets the service description
    //    /// </summary>
    //    public WmsServiceDescription ServiceDescription
    //    {
    //        get { return _serviceDescription; }
    //    }

    //    /// <summary>
    //    /// Gets the version of the WMS server (ex. "1.3.0")
    //    /// </summary>
    //    public string Version
    //    {
    //        get { return _version; }
    //    }

    //    /// <summary>
    //    /// Gets a list of available image mime type formats
    //    /// </summary>
    //    public Collection<string> GetMapOutputFormats
    //    {
    //        get { return _getMapOutputFormats; }
    //    }

    //    /// <summary>
    //    /// Gets a list of available exception mime type formats
    //    /// </summary>
    //    public string[] ExceptionFormats
    //    {
    //        get { return _exceptionFormats; }
    //    }

    //    /// <summary>
    //    /// Gets the available GetMap request methods and Online Resource URI
    //    /// </summary>
    //    public WmsOnlineResource[] GetMapRequests
    //    {
    //        get { return _getMapRequests; }
    //    }

    //    /// <summary>
    //    /// Gets the hiarchial layer structure
    //    /// </summary>
    //    public WmsServerLayer Layer
    //    {
    //        get { return _layer; }
    //    }

    //    public string CRS
    //    {
    //        get { return _CRS; }
    //        set { _CRS = value; }
    //    }
    //    #endregion


    //    public Extent GetMaxSize()
    //    {
    //        Extent x;

    //        if (_CRS == "EPSG:4326")
    //        {
    //            x = _layer.LatLonBoundingBox;
    //        }
    //        else
    //        {
    //            int i = Array.IndexOf(_layer.Crs, _CRS);

    //            x = _layer.CrsExtent[i];
    //        }

    //        return x;
    //    }
 
    //    public void  ReadCapabilities(string server, string XMLCustomFile="",  WebProxy proxy=null)
    //    {
    //        Uri u;

    //        if (XMLCustomFile == "")
    //        {
    //            u = new Uri(server);
    //        }
    //        else
    //        {
    //            u = new Uri(XMLCustomFile);
    //        }
                                
            
    //        Stream stream;
    //        if (u.IsAbsoluteUri && u.IsFile) //assume web if relative because IsFile is not supported on relative paths
    //        {
    //            stream = File.OpenRead(u.LocalPath);
    //        }
    //        else
    //        {
    //            Uri uri = new Uri(CreateCapabiltiesRequest(server));
    //            stream = GetRemoteXmlStream(uri, proxy);
    //        }

    //        XmlDocument xml = GetXml(stream);
    //        ParseCapabilities(xml);

    //        _server = server;
    //    }

    //    public static string CreateCapabiltiesRequest(string url)
    //    {
    //        var strReq = new StringBuilder(url);
    //        if (!url.Contains("?"))
    //            strReq.Append("?");
    //        if (!strReq.ToString().EndsWith("&") && !strReq.ToString().EndsWith("?"))
    //            strReq.Append("&");
    //        if (!url.ToLower().Contains("service=wms"))
    //            strReq.AppendFormat("SERVICE=WMS&");
    //        if (!url.ToLower().Contains("request=getcapabilities"))
    //            strReq.AppendFormat("REQUEST=GetCapabilities&");
    //        //if (!url.ToLower().Contains("version=1.1.1"))
    //        //    strReq.AppendFormat("version=1.1.1&");
    //        return strReq.ToString();
    //    }

    //    /// <summary>
    //    /// Downloads servicedescription from WMS service
    //    /// </summary>
    //    /// <returns>XmlDocument from Url. Null if Url is empty or inproper XmlDocument</returns>
    //    private XmlDocument GetXml(Stream stream)
    //    {
    //        try
    //        {
    //            var r = new XmlTextReader(stream);
    //            r.XmlResolver = null;
    //            var doc = new XmlDocument();
    //            doc.XmlResolver = null;
    //            doc.Load(r);
    //            stream.Close();
    //            _nsmgr = new XmlNamespaceManager(doc.NameTable);
    //            return doc;
    //        }
    //        catch (Exception ex)
    //        {
    //            throw new ApplicationException("Could not download capabilities", ex);
    //        }
    //    }

    //    private static Stream GetRemoteXmlStream(Uri uri, WebProxy proxy)
    //    {
    //        WebRequest myRequest = WebRequest.Create(uri);
    //        if (proxy != null) myRequest.Proxy = proxy;

    //        WebResponse myResponse = myRequest.GetResponse();
    //        Stream stream = myResponse.GetResponseStream();

    //        return stream;
    //    }


    //    /// <summary>
    //    /// Parses a servicedescription and stores the data in the ServiceDescription property
    //    /// </summary>
    //    /// <param name="doc">XmlDocument containing a valid Service Description</param>
    //    private void ParseCapabilities(XmlDocument doc)
    //    {
    //        if (doc.DocumentElement.Attributes["version"] != null)
    //        {
    //            _version = doc.DocumentElement.Attributes["version"].Value;

    //            if (Version != "1.0.0" && Version != "1.1.0" && Version != "1.1.1" && Version != "1.3.0")
    //                throw new ApplicationException("WMS Version " + Version + " not supported");

    //            _nsmgr.AddNamespace(String.Empty, "http://www.opengis.net/wms");
    //            _nsmgr.AddNamespace("sm", Version == "1.3.0" ? "http://www.opengis.net/wms" : "");
    //            _nsmgr.AddNamespace("xlink", "http://www.w3.org/1999/xlink");
    //            _nsmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
    //        }
    //        else
    //            throw (new ApplicationException("No service version number found!"));

    //        XmlNode xnService = doc.DocumentElement.SelectSingleNode("sm:Service", _nsmgr);
    //        XmlNode xnCapability = doc.DocumentElement.SelectSingleNode("sm:Capability", _nsmgr);
    //        if (xnService != null)
    //            ParseServiceDescription(xnService);
    //        else
    //            throw (new ApplicationException("No service tag found!"));


    //        if (xnCapability != null)
    //            ParseCapability(xnCapability);
    //        else
    //            throw (new ApplicationException("No capability tag found!"));
    //    }

    //    /// <summary>
    //    /// Parses service description node
    //    /// </summary>
    //    /// <param name="xnlServiceDescription"></param>
    //    private void ParseServiceDescription(XmlNode xnlServiceDescription)
    //    {
    //        XmlNode node = xnlServiceDescription.SelectSingleNode("sm:Title", _nsmgr);
    //        _serviceDescription.Title = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:OnlineResource/@xlink:href", _nsmgr);
    //        _serviceDescription.OnlineResource = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:Abstract", _nsmgr);
    //        _serviceDescription.Abstract = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:Fees", _nsmgr);
    //        _serviceDescription.Fees = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:AccessConstraints", _nsmgr);
    //        _serviceDescription.AccessConstraints = (node != null ? node.InnerText : null);

    //        XmlNodeList xnlKeywords = xnlServiceDescription.SelectNodes("sm:KeywordList/sm:Keyword", _nsmgr);
    //        if (xnlKeywords != null)
    //        {
    //            _serviceDescription.Keywords = new string[xnlKeywords.Count];
    //            for (var i = 0; i < xnlKeywords.Count; i++)
    //                ServiceDescription.Keywords[i] = xnlKeywords[i].InnerText;
    //        }
    //        //Contact information
    //        _serviceDescription.ContactInformation = new WmsServiceDescription.WmsContactInformation();
    //        node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactAddress/sm:Address", _nsmgr);
    //        _serviceDescription.ContactInformation.Address.Address = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactAddress/sm:AddressType",
    //                                                      _nsmgr);
    //        _serviceDescription.ContactInformation.Address.AddressType = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactAddress/sm:City", _nsmgr);
    //        _serviceDescription.ContactInformation.Address.City = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactAddress/sm:Country", _nsmgr);
    //        _serviceDescription.ContactInformation.Address.Country = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactAddress/sm:PostCode", _nsmgr);
    //        _serviceDescription.ContactInformation.Address.PostCode = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactElectronicMailAddress", _nsmgr);
    //        _serviceDescription.ContactInformation.Address.StateOrProvince = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactElectronicMailAddress", _nsmgr);
    //        _serviceDescription.ContactInformation.ElectronicMailAddress = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactFacsimileTelephone", _nsmgr);
    //        _serviceDescription.ContactInformation.FacsimileTelephone = (node != null ? node.InnerText : null);
    //        node =
    //            xnlServiceDescription.SelectSingleNode(
    //                "sm:ContactInformation/sm:ContactPersonPrimary/sm:ContactOrganisation", _nsmgr);
    //        _serviceDescription.ContactInformation.PersonPrimary.Organisation = (node != null ? node.InnerText : null);
    //        node =
    //            xnlServiceDescription.SelectSingleNode(
    //                "sm:ContactInformation/sm:ContactPersonPrimary/sm:ContactPerson", _nsmgr);
    //        _serviceDescription.ContactInformation.PersonPrimary.Person = (node != null ? node.InnerText : null);
    //        node = xnlServiceDescription.SelectSingleNode("sm:ContactInformation/sm:ContactVoiceTelephone", _nsmgr);
    //        _serviceDescription.ContactInformation.VoiceTelephone = (node != null ? node.InnerText : null);
    //    }

    //    /// <summary>
    //    /// Parses capability node
    //    /// </summary>
    //    /// <param name="xnCapability"></param>
    //    private void ParseCapability(XmlNode xnCapability)
    //    {
    //        XmlNode xnRequest = xnCapability.SelectSingleNode("sm:Request", _nsmgr);
    //        if (xnRequest == null)
    //            throw (new Exception("Request parameter not specified in Service Description"));
    //        ParseRequest(xnRequest);
    //        XmlNode xnLayer = xnCapability.SelectSingleNode("sm:Layer", _nsmgr);
    //        if (xnLayer == null)
    //            throw (new Exception("No layer tag found in Service Description"));
    //        _layer = ParseLayer(xnLayer);

    //        XmlNode xnException = xnCapability.SelectSingleNode("sm:Exception", _nsmgr);
    //        if (xnException != null)
    //            ParseExceptions(xnException);

    //        _vendorSpecificCapabilities = xnCapability.SelectSingleNode("sm:VendorSpecificCapabilities", _nsmgr);
    //    }

    //    /// <summary>
    //    /// Parses valid exceptions
    //    /// </summary>
    //    /// <param name="xnlExceptionNode"></param>
    //    private void ParseExceptions(XmlNode xnlExceptionNode)
    //    {
    //        XmlNodeList xnlFormats = xnlExceptionNode.SelectNodes("sm:Format", _nsmgr);
    //        if (xnlFormats != null)
    //        {
    //            _exceptionFormats = new string[xnlFormats.Count];
    //            for (int i = 0; i < xnlFormats.Count; i++)
    //            {
    //                _exceptionFormats[i] = xnlFormats[i].InnerText;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Parses request node
    //    /// </summary>
    //    /// <param name="xmlRequestNode"></param>
    //    private void ParseRequest(XmlNode xmlRequestNode)
    //    {
    //        XmlNode xnGetMap = xmlRequestNode.SelectSingleNode("sm:GetMap", _nsmgr);
    //        ParseGetMapRequest(xnGetMap);
    //        //TODO: figure out what we need to do with lines below:
    //        //XmlNode xnGetFeatureInfo = xmlRequestNodes.SelectSingleNode("sm:GetFeatureInfo", nsmgr);
    //        //XmlNode xnCapa = xmlRequestNodes.SelectSingleNode("sm:GetCapabilities", nsmgr); <-- We don't really need this do we?			
    //    }

    //    /// <summary>
    //    /// Parses GetMap request nodes
    //    /// </summary>
    //    /// <param name="getMapRequestNodes"></param>
    //    private void ParseGetMapRequest(XmlNode getMapRequestNodes)
    //    {
    //        XmlNode xnlHttp = getMapRequestNodes.SelectSingleNode("sm:DCPType/sm:HTTP", _nsmgr);
    //        if (xnlHttp != null && xnlHttp.HasChildNodes)
    //        {
    //            _getMapRequests = new WmsOnlineResource[xnlHttp.ChildNodes.Count];
    //            for (int i = 0; i < xnlHttp.ChildNodes.Count; i++)
    //            {
    //                var wor = new WmsOnlineResource();
    //                wor.Type = xnlHttp.ChildNodes[i].Name;
    //                wor.OnlineResource =
    //                    xnlHttp.ChildNodes[i].SelectSingleNode("sm:OnlineResource", _nsmgr).Attributes["xlink:href"].
    //                        InnerText;
    //                _getMapRequests[i] = wor;
    //            }
    //        }
    //        XmlNodeList xnlFormats = getMapRequestNodes.SelectNodes("sm:Format", _nsmgr);
    //        //_GetMapOutputFormats = new Collection<string>(xnlFormats.Count);
    //        _getMapOutputFormats = new Collection<string>();
    //        for (int i = 0; i < xnlFormats.Count; i++)
    //            _getMapOutputFormats.Add(xnlFormats[i].InnerText);
    //    }

    //    /// <summary>
    //    /// Iterates through the layer nodes recursively
    //    /// </summary>
    //    /// <param name="xmlLayer"></param>
    //    /// <returns></returns>
    //    private WmsServerLayer ParseLayer(XmlNode xmlLayer)
    //    {
    //        var layer = new WmsServerLayer();
    //        XmlNode node = xmlLayer.SelectSingleNode("sm:Name", _nsmgr);
    //        layer.Name = (node != null ? node.InnerText : null);
    //        node = xmlLayer.SelectSingleNode("sm:Title", _nsmgr);
    //        layer.Title = (node != null ? node.InnerText : null);
    //        node = xmlLayer.SelectSingleNode("sm:Abstract", _nsmgr);
    //        layer.Abstract = (node != null ? node.InnerText : null);
    //        XmlAttribute attr = xmlLayer.Attributes["queryable"];
    //        layer.Queryable = (attr != null && attr.InnerText == "1");


    //        XmlNodeList xnlKeywords = xmlLayer.SelectNodes("sm:KeywordList/sm:Keyword", _nsmgr);
    //        if (xnlKeywords != null)
    //        {
    //            layer.Keywords = new string[xnlKeywords.Count];
    //            for (int i = 0; i < xnlKeywords.Count; i++)
    //                layer.Keywords[i] = xnlKeywords[i].InnerText;
    //        }

    //        if (Version == "1.3.0")
    //        {
    //            XmlNodeList xnlCrs = xmlLayer.SelectNodes("sm:CRS", _nsmgr);
    //            if (xnlCrs != null)
    //            {
    //                layer.Crs = new string[xnlCrs.Count];
    //                for (int i = 0; i < xnlCrs.Count; i++)
    //                    layer.Crs[i] = xnlCrs[i].InnerText;
    //            }
    //        }
    //        else
    //        {
    //            XmlNodeList xnlCrs = xmlLayer.SelectNodes("sm:SRS", _nsmgr);
    //            if (xnlCrs != null)
    //            {
    //                layer.Crs = new string[xnlCrs.Count];
    //                for (int i = 0; i < xnlCrs.Count; i++)
    //                    layer.Crs[i] = xnlCrs[i].InnerText;
    //            }
    //        }


    //        XmlNodeList xnlStyle = xmlLayer.SelectNodes("sm:Style", _nsmgr);
    //        if (xnlStyle != null)
    //        {
    //            layer.Style = new WmsLayerStyle[xnlStyle.Count];
    //            for (int i = 0; i < xnlStyle.Count; i++)
    //            {
    //                node = xnlStyle[i].SelectSingleNode("sm:Name", _nsmgr);
    //                layer.Style[i].Name = (node != null ? node.InnerText : null);
    //                node = xnlStyle[i].SelectSingleNode("sm:Title", _nsmgr);
    //                layer.Style[i].Title = (node != null ? node.InnerText : null);
    //                node = xnlStyle[i].SelectSingleNode("sm:Abstract", _nsmgr);
    //                layer.Style[i].Abstract = (node != null ? node.InnerText : null);
    //                node = xnlStyle[i].SelectSingleNode("sm:LegendUrl", _nsmgr);
    //                if (node != null)
    //                {
    //                    layer.Style[i].LegendUrl = new WmsStyleLegend();
    //                    layer.Style[i].LegendUrl.Width = int.Parse(node.Attributes["width"].InnerText);
    //                    layer.Style[i].LegendUrl.Width = int.Parse(node.Attributes["height"].InnerText);
    //                    layer.Style[i].LegendUrl.OnlineResource.OnlineResource =
    //                        node.SelectSingleNode("sm:OnlineResource", _nsmgr).Attributes["xlink:href"].InnerText;
    //                    layer.Style[i].LegendUrl.OnlineResource.Type =
    //                        node.SelectSingleNode("sm:Format", _nsmgr).InnerText;
    //                }
    //                node = xnlStyle[i].SelectSingleNode("sm:StyleSheetURL", _nsmgr);
    //                if (node != null)
    //                {
    //                    layer.Style[i].StyleSheetUrl = new WmsOnlineResource();
    //                    layer.Style[i].StyleSheetUrl.OnlineResource =
    //                        node.SelectSingleNode("sm:OnlineResource", _nsmgr).Attributes["xlink:href"].InnerText;
    //                    //layer.Style[i].StyleSheetUrl.OnlineResource = node.SelectSingleNode("sm:Format", nsmgr).InnerText;
    //                }
    //            }
    //        }

    //        XmlNodeList xnlLayers = xmlLayer.SelectNodes("sm:Layer", _nsmgr);
    //        if (xnlLayers != null)
    //        {
    //            layer.ChildLayers = new WmsServerLayer[xnlLayers.Count];
    //            for (int i = 0; i < xnlLayers.Count; i++)
    //                layer.ChildLayers[i] = ParseLayer(xnlLayers[i]);
    //        }
            
    //        if (Version == "1.3.0")
    //        {
    //            XmlNode GeographicBoundingBox = xmlLayer.SelectSingleNode("sm:EX_GeographicBoundingBox", _nsmgr);
    //            if (GeographicBoundingBox != null)
    //            {
    //                double minx, miny, maxx, maxy;

    //                XmlNode w = GeographicBoundingBox.SelectSingleNode("sm:westBoundLongitude", _nsmgr);
    //                XmlNode e = GeographicBoundingBox.SelectSingleNode("sm:eastBoundLongitude", _nsmgr);
    //                XmlNode s = GeographicBoundingBox.SelectSingleNode("sm:southBoundLatitude", _nsmgr);
    //                XmlNode n = GeographicBoundingBox.SelectSingleNode("sm:northBoundLatitude", _nsmgr);

    //                if (!double.TryParse(w.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out minx) &
    //                    !double.TryParse(s.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out miny) &
    //                    !double.TryParse(e.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out maxx) &
    //                    !double.TryParse(n.InnerText, NumberStyles.Any, CultureInfo.InvariantCulture, out maxy))
    //                    throw new ArgumentException("Invalid LatLonBoundingBox on layer '" + layer.Name + "'");

    //                layer.LatLonBoundingBox = new Extent(minx, miny, maxx, maxy);
    //            }
    //        }
    //        else
    //        {
    //            XmlNode GeographicBoundingBox = xmlLayer.SelectSingleNode("sm:LatLonBoundingBox", _nsmgr);
    //            if (GeographicBoundingBox != null)
    //            {
    //                double minx, miny, maxx, maxy;

    //                if (!double.TryParse(GeographicBoundingBox.Attributes["minx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out minx) &
    //                    !double.TryParse(GeographicBoundingBox.Attributes["miny"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out miny) &
    //                    !double.TryParse(GeographicBoundingBox.Attributes["maxx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxx) &
    //                    !double.TryParse(GeographicBoundingBox.Attributes["maxy"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxy))
    //                    throw new ArgumentException("Invalid LatLonBoundingBox on layer '" + layer.Name + "'");

    //                layer.LatLonBoundingBox = new Extent(minx, miny, maxx, maxy);
    //            }

    //        }

    //        if (Version == "1.3.0")
    //        {
    //            XmlNodeList CrsExtent = xmlLayer.SelectNodes("sm:BoundingBox", _nsmgr);

    //            if (CrsExtent != null)
    //            {
    //                layer.CrsExtent = new Extent[layer.Crs.Count()];

    //                foreach (XmlNode nd in CrsExtent)
    //                {
    //                    string CRS = nd.Attributes["CRS"].Value;

    //                    double minx, miny, maxx, maxy;

    //                    int i = Array.IndexOf(layer.Crs, CRS);

    //                    if (!double.TryParse(nd.Attributes["minx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out minx) &
    //                        !double.TryParse(nd.Attributes["miny"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out miny) &
    //                        !double.TryParse(nd.Attributes["maxx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxx) &
    //                        !double.TryParse(nd.Attributes["maxy"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxy))
    //                        throw new ArgumentException("Invalid BoundingBox on CRS on layer '" + layer.Name + "'");

    //                    layer.CrsExtent[i] = new Extent(minx, miny, maxx, maxy);
    //                }

    //            }



    //            XmlNode xmlMaxScaleDenominator = xmlLayer.SelectSingleNode("sm:MaxScaleDenominator", _nsmgr);
    //            if (xmlMaxScaleDenominator != null)
    //            {
    //                double.TryParse(xmlMaxScaleDenominator.InnerText, out layer.MaxScaleDenominator);
    //            }
    //            else
    //            {
    //                layer.MaxScaleDenominator = 0;
    //            }

    //            XmlNode xmlMinScaleDenominator = xmlLayer.SelectSingleNode("sm:MinScaleDenominator", _nsmgr);
    //            if (xmlMinScaleDenominator != null)
    //            {
    //                double.TryParse(xmlMinScaleDenominator.InnerText, out layer.MinScaleDenominator);
    //            }
    //            else
    //            {
    //                layer.MinScaleDenominator = 0;
    //            }

    //        }
    //        else
    //        {

    //            XmlNodeList CrsExtent = xmlLayer.SelectNodes("sm:BoundingBox", _nsmgr);

    //            if (CrsExtent != null)
    //            {

    //                int n = layer.Crs.Count();

    //                if (n <= 0) n = 1;
                    
    //                layer.CrsExtent = new Extent[n];

    //                foreach (XmlNode nd in CrsExtent)
    //                {
    //                    string CRS = nd.Attributes["SRS"].Value;

    //                    double minx, miny, maxx, maxy;

    //                    int i = Array.IndexOf(layer.Crs, CRS);

    //                    if (i < 0) i = 0;

    //                    if (!double.TryParse(nd.Attributes["minx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out minx) &
    //                        !double.TryParse(nd.Attributes["miny"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out miny) &
    //                        !double.TryParse(nd.Attributes["maxx"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxx) &
    //                        !double.TryParse(nd.Attributes["maxy"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out maxy))
    //                        throw new ArgumentException("Invalid BoundingBox on CRS on layer '" + layer.Name + "'");

    //                    layer.CrsExtent[i] = new Extent(minx, miny, maxx, maxy);
    //                }

    //            }

    //            XmlNode ScaleHint = xmlLayer.SelectSingleNode("sm:ScaleHint", _nsmgr);

    //            if (ScaleHint != null)
    //            {
    //                double min, max;

    //                if (!double.TryParse(ScaleHint.Attributes["min"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out min) ||
    //                    !double.TryParse(ScaleHint.Attributes["max"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out max))
    //                {
    //                    throw new ArgumentException("No hint scale on layer '" + layer.Name + "'");
    //                }

    //                layer.MinScaleHint = min;
    //                layer.MaxScaleHint = max;
    //            }
    //        }


    //        return layer;
    //    }

    //    private List<WmsServerLayer> GetLayerFromHint(double Hint)
    //    {
    //       List<WmsServerLayer> list = new List<WmsServerLayer>();

    //       if (_layer.visible == true)
    //       {
    //           //if (Hint >= _layer.MinScaleHint && Hint <= _layer.MaxScaleHint)
    //           {
    //               if(_layer.Name != null) list.Add(_layer);
    //           }

    //           CicleLayerFromInt(Hint, _layer, ref list);
    //       }


    //        return list;
    //    }

    //    private void CicleLayerFromInt(double Hint, WmsServerLayer layer, ref List<WmsServerLayer> list )
    //    {

    //        Debug.Print(layer.Name + " " + layer.MinScaleHint + " " + layer.MaxScaleHint);

    //        foreach (WmsServerLayer l in layer.ChildLayers)
    //        {
    //            if (l.visible == true)
    //            {
    //                //if (Hint >= l.MinScaleHint && Hint <= l.MaxScaleHint)
    //                {
    //                    if (l.Name != null) list.Add(l);
    //                }

    //                CicleLayerFromInt(Hint, l, ref list);

    //            }
    //        }

    //    }

    //    public TileInfoSet[] GetTile(Extent extent, Size size)
    //    {
    //        TileInfoSet [] Ts = null;

    //        double Hint = Math.Sqrt(Math.Pow(extent.Width / size.Width, 2) + Math.Pow(extent.Height / size.Height, 2));

    //        List<WmsServerLayer> list = GetLayerFromHint(Hint);

    //        if (list != null)
    //        {

    //            int num = list.Count;
    //            Ts = new TileInfoSet[num];

    //            for (int i = 0; i < num; i++)
    //            {
    //                Ts[i] = new TileInfoSet();
    //                Ts[i].cols = 1;
    //                Ts[i].rows = 1;


    //                Ts[i].Tiles = new TileInfo[1][];
    //                Ts[i].Tiles[0] = new TileInfo[1];



    //                TileInfo t = new TileInfo();

    //                t.col = 0;
    //                t.row = 0;
    //                t.Extent = extent;

    //                //Tiler.WMS_Format = "http://servizigis.arpa.emr.it/ArcGIS/services/PortaleGIS/BaseCartografica/mapserver/WMSServer?VERSION=1.1.1&Service=BaseCartografica&REQUEST=GetMap&LAYERS=0&styles=&bbox={0},{1},{2},{3}&width={4}&height={5}&srs=EPSG:25832&format=image/png";


    //                string f =  _server + "?" +
    //                           "Version=1.1.1"+// + Version +
    //                           //"&Service=" + list[i].Service +
    //                           "&REQUEST=GetMap" +
    //                           "&Layers=" + list[i].Name +
    //                           "&styles=" +
    //                           "&bbox={0},{1},{2},{3}&width={4}&height={5}" +
    //                           "&srs=" + _CRS +
    //                           "&crs=1"+
    //                           "&format=image/png&TRANSPARENT=TRUE";

    //                t.url = string.Format(CultureInfo.InvariantCulture, f, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, size.Width, size.Height);
    //                Ts[i].Tiles[0][0] = t;
    //            }

    //        }



    //        return Ts;


    //    }

    //    string[] keysPop(string[] keys)
    //    {
    //        string[] k = new string[keys.Count() - 1];

    //        Array.Copy(keys, 1, k, 0, k.Count());

    //        return k;
    //    }

    //    public void CheckLayer(string[] keys, bool check, WmsServerLayer l = null)
    //    {
    //        if (keys.Count() == 0) return;
            
    //        if (l == null)
    //        {
    //            if (_layer.Title == keys[0])
    //            {
    //                if (keys.Count() == 1)
    //                {
    //                    if (_layer.Title == keys[0])
    //                    {
    //                        _layer.visible = check;
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    string[] k = keysPop(keys);
    //                    CheckLayer(k, check, _layer);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            foreach (WmsServerLayer cl in l.ChildLayers)
    //            {

    //                if (keys.Count() == 1)
    //                {
    //                    if (cl.Title == keys[0])
    //                    {
    //                        cl.visible = check;
    //                        return;
    //                    }
    //                }
    //                else
    //                {
    //                    if (cl.Title == keys[0])
    //                    {
    //                        string[] k = keysPop(keys);
    //                        CheckLayer(k, check, cl);
    //                    }
    //                }
    //            }
    //        }

    //    }

    //    public void List(WebLegend tree, WmsServerLayer l = null, TreeNode parent = null)
    //    {
    //        if (l == null)
    //        {
    //            TreeNode tn = new TreeNode(_layer.Title);
    //            tn.ShowCheckBox = true;
    //            tn.Checked = _layer.visible;
    //            tn.Expand();

    //            if (_layer.Style.Count() > 0)
    //            {
    //                if (_layer.Style[0].LegendUrl.OnlineResource.OnlineResource != "")
    //                {
    //                    tn.ImageUrl = _layer.Style[0].LegendUrl.OnlineResource.OnlineResource;
    //                }
    //            }

    //            List(tree, _layer, tn);

    //            tree.Nodes.Add(tn);
    //        }
    //        else
    //        {
    //            //foreach (WmsServerLayer cl in l.ChildLayers)
    //            for (int i = l.ChildLayers.Count() - 1; i >= 0; i--)
    //            {
    //                WmsServerLayer cl = l.ChildLayers[i];

    //                TreeNode tn = new TreeNode(cl.Title);// + " " + cl.Name);
    //                tn.ShowCheckBox = true;
    //                tn.Checked = cl.visible;

    //                List(tree, cl, tn);

    //                tn.Expand();

    //                parent.ChildNodes.Add(tn);

    //            }
    //        }
    //    }

    //    public string GetHTML(ref Map m, Size size)
    //    {
    //        string HTML="";

    //        double Hint = Math.Sqrt(Math.Pow(m.ViewExtents.Width / size.Width, 2) + Math.Pow(m.ViewExtents.Height / size.Height, 2));

    //        List<WmsServerLayer> list = GetLayerFromHint(Hint);

    //        if (list != null & list.Count>0)
    //        {
    //            HTML = "<p>WMS not defined</p>";

    //            int num = list.Count;

    //            string Lays = "";
    //            for (int i = 0; i < num-1; i++)
    //            {
    //                Lays += list[i].Name + ",";
    //            }
    //            Lays += list[num - 1].Name;


    //            string svr = _server;

    //            if (svr.Contains('?'))
    //                svr += "&";
    //            else
    //                svr += "?";
   

    //            string f = svr +
    //                        "Version=1.1.1" +// + Version +
    //                        //"&Service=" + list[i].Service +
    //                        "&REQUEST=GetMap" +
    //                        "&Layers=" + Lays +
    //                        "&styles=" +
    //                        "&bbox={0},{1},{2},{3}&width={4}&height={5}" +
    //                        "&srs=" + _CRS +
    //                        //"&crs=" +_CRS +
    //                        "&format=image/png&TRANSPARENT=TRUE";

    //            string t = string.Format(CultureInfo.InvariantCulture, f, m.ViewExtents.MinX, m.ViewExtents.MinY, m.ViewExtents.MaxX, m.ViewExtents.MaxY, size.Width, size.Height);

    //            string o = "";
    //            if (_opacity != 100)
    //            {
    //                //o = "filter:alpha(opacity=30);opacity: 0.3;";
    //                o = "filter:alpha(opacity=" + _opacity.ToString() + "); opacity: " + (_opacity / 100).ToString(CultureInfo.InvariantCulture) + "; ";
    //            }

    //            HTML = "<img alt=\"\" style=\"position:absolute; " + o + "left:0px; top:0px; width:" + size.Width.ToString() + "px; height:" + size.Height.ToString() + "px; \" src=\"" + t + "\" />";

    //        }

    //        return HTML;

    //    }

    //    public string GetHTMLOld(ref Map m, Size size, string ClientID)
    //    {
    //        TileInfoSet[] Ts = GetTile(m.ViewExtents, size);
            
    //        if (Ts.Count() == 0)
    //        {
    //            return "<p> No layer </p>";
    //        }

    //        string htm = "";

    //        int l = 0;
    //        int t = 0;
    //        int w = (int)((double)size.Width / (double)Ts[0].cols);
    //        int h = (int)((double)size.Height / (double)Ts[0].rows);

    //        int numSet = Ts.GetLength(0);

    //        Rectangle Rect = m.ProjToPixel(m.ViewExtents);

    //        htm += "<div id=\"Back_" + ClientID + "\" style=\"position:absolute; left:" + Rect.Left.ToString() + "px; top:" + Rect.Top.ToString() + "px; width:" + Rect.Width.ToString() + "px; height:" + Rect.Height.ToString() + "px; z-index:1; \">";

    //        for (int r = 0; r < Ts[0].rows; r++)
    //        {
    //            for (int c = 0; c < Ts[0].cols; c++)
    //            {
    //                l = c * w;
    //                t = r * h;

    //                for (int set = 0; set < numSet; set++)
    //                {
    //                    htm += "<img alt=\"\" style=\"position:absolute; left: " + l.ToString() + "px; top:" + t.ToString() + "px; width:" + w.ToString() + "px; height:" + h.ToString() + "px; \" src=\"" + Ts[set].Tiles[r][c].url + "\" />";
    //                }
    //            }

    //        }

    //        htm += "</div>";

    //        return htm;
    //    }

    //    #region WMS Data structures

    //    #region Nested type: WmsLayerStyle

    //    /// <summary>
    //    /// Structure for storing information about a WMS Layer Style
    //    /// </summary>
    //    public struct WmsLayerStyle
    //    {
    //        /// <summary>
    //        /// Abstract
    //        /// </summary>
    //        public string Abstract;

    //        /// <summary>
    //        /// Legend
    //        /// </summary>
    //        public WmsStyleLegend LegendUrl;

    //        /// <summary>
    //        /// Name
    //        /// </summary>
    //        public string Name;

    //        /// <summary>
    //        /// Style Sheet Url
    //        /// </summary>
    //        public WmsOnlineResource StyleSheetUrl;

    //        /// <summary>
    //        /// Title
    //        /// </summary>
    //        public string Title;
    //    }

    //    #endregion

    //    #region Nested type: WmsOnlineResource

    //    /// <summary>
    //    /// Structure for storing info on an Online Resource
    //    /// </summary>
    //    public struct WmsOnlineResource
    //    {
    //        /// <summary>
    //        /// URI of online resource
    //        /// </summary>
    //        public string OnlineResource;

    //        /// <summary>
    //        /// Type of online resource (Ex. request method 'Get' or 'Post')
    //        /// </summary>
    //        public string Type;
    //    }

    //    #endregion

    //    #region Nested type: WmsServerLayer

    //    /// <summary>
    //    /// Structure for holding information about a WMS Layer 
    //    /// </summary>
    //    public class WmsServerLayer
    //    {
    //        /// <summary>
    //        /// Abstract
    //        /// </summary>
    //        public string Abstract;

    //        /// <summary>
    //        /// Collection of child layers
    //        /// </summary>
    //        public WmsServerLayer[] ChildLayers;

    //        /// <summary>
    //        /// Coordinate Reference Systems supported by layer
    //        /// </summary>
    //        public string[] Crs;

    //        /// <summary>
    //        /// Bounding Box in each Coordinate Reference Systems supported by layer
    //        /// </summary>
    //        public Extent[] CrsExtent;

    //        /// <summary>
    //        /// Keywords
    //        /// </summary>
    //        public string[] Keywords;

    //        /// <summary>
    //        /// Latitudal/longitudal extent of this layer
    //        /// </summary>
    //        public Extent LatLonBoundingBox;

    //        /// <summary>
    //        /// Unique name of this layer used for requesting layer
    //        /// </summary>
    //        public string Name;

    //        /// <summary>
    //        /// Specifies whether this layer is queryable using GetFeatureInfo requests
    //        /// </summary>
    //        public bool Queryable;

    //        /// <summary>
    //        /// List of styles supported by layer
    //        /// </summary>
    //        public WmsLayerStyle[] Style;

    //        /// <summary>
    //        /// Layer title
    //        /// </summary>
    //        public string Title;

    //        /// <summary>
    //        /// MinScaleDenominator
    //        /// </summary>
    //        public double MinScaleDenominator;

    //        /// <summary>
    //        /// MaxScaleDenominator
    //        /// </summary>
    //        public double MaxScaleDenominator;

    //        /// <summary>
    //        /// MinScaleHint
    //        /// </summary>
    //        public double MinScaleHint;

    //        /// <summary>
    //        /// MaxScaleHint
    //        /// </summary>
    //        public double MaxScaleHint;



    //        /// <summary>
    //        /// LayerVisible
    //        /// </summary>
    //        public bool visible = true;

    //    }

    //    #endregion

    //    #region Nested type: WmsStyleLegend

    //    /// <summary>
    //    /// Structure for storing WMS Legend information
    //    /// </summary>
    //    public struct WmsStyleLegend
    //    {
    //        /// <summary>
    //        /// Online resource for legend style 
    //        /// </summary>
    //        public WmsOnlineResource OnlineResource;

    //        /// <summary>
    //        /// Size of legend
    //        /// </summary>
    //        public int Width;
    //        public int Height;
    //    }

    //    #endregion

    //    #endregion

    //}
    #endregion

}