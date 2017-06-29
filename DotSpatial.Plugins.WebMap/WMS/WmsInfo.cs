using System.Collections.Generic;
using System.Net;
using BruTile.Web.Wms;
using DotSpatial.Projections;

namespace DotSpatial.Plugins.WebMap.WMS
{
    public class WmsInfo
    {
        public WmsInfo(string serverUrl, WmsCapabilities wmsCapabilities, Layer layer, Dictionary<string, string> customParameters,
            string crs, ProjectionInfo projectionInfo, string style, NetworkCredential credentials)
        {
            Credentials = credentials;
            CustomParameters = customParameters;
            CRS = crs;
            CrsProjectionInfo = projectionInfo;
            Style = style;
            Layer = layer;
            WmsCapabilities = wmsCapabilities;
            ServerUrl = serverUrl;
        }

        public ProjectionInfo CrsProjectionInfo { get; private set; }
        public string ServerUrl { get; private set; }
        public WmsCapabilities WmsCapabilities { get; private set; }
        public Layer Layer { get; private set; }
        public string CRS { get; private set; }
        public string Style { get; private set; }
        public Dictionary<string, string> CustomParameters { get; private set; }
        public NetworkCredential Credentials { get; private set; }
    }
}