using System.Collections.Generic;
using BruTile.Web.Wms;

namespace DotSpatial.Plugins.WebMap.WMS_New
{
    public class WmsInfo
    {
        public WmsInfo(string serverUrl, WmsCapabilities wmsCapabilities, Layer layer, Dictionary<string, string> customParameters,
            string crs, string style)
        {
            CustomParameters = customParameters;
            CRS = crs;
            Style = style;
            Layer = layer;
            WmsCapabilities = wmsCapabilities;
            ServerUrl = serverUrl;
        }

        public string ServerUrl { get; private set; }
        public WmsCapabilities WmsCapabilities { get; private set; }
        public Layer Layer { get; private set; }
        public string CRS { get; private set; }
        public string Style { get; private set; }
        public Dictionary<string, string> CustomParameters { get; private set; }
    }
}