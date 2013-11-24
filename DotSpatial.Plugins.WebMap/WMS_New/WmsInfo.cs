using BruTile.Web.Wms;

namespace DotSpatial.Plugins.WebMap.WMS_New
{
    public class WmsInfo
    {
        public WmsCapabilities WmsCapabilities { get; set; }
        public Layer Layer { get; set; }
        public BoundingBox BoundingBox { get; set; }
    }
}