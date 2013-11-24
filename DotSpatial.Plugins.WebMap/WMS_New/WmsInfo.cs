using BruTile.Web.Wms;

namespace DotSpatial.Plugins.WebMap.WMS_New
{
    public class WmsInfo
    {
        public WmsInfo(string serverUrl, WmsCapabilities wmsCapabilities, Layer layer, BoundingBox boundingBox)
        {
            BoundingBox = boundingBox;
            Layer = layer;
            WmsCapabilities = wmsCapabilities;
            ServerUrl = serverUrl;
        }

        public string ServerUrl { get; private set; }
        public WmsCapabilities WmsCapabilities { get; private set; }
        public Layer Layer { get; private set; }
        public BoundingBox BoundingBox { get; private set; }
    }
}