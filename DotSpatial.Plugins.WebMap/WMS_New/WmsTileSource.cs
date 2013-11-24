using System;
using System.Collections.Generic;
using BruTile;
using BruTile.Web;

namespace DotSpatial.Plugins.WebMap.WMS_New
{
    public class WmsTileSource : TileSource
    {
        private WmsTileSource(ITileProvider tileProvider, ITileSchema tileSchema) : base(tileProvider, tileSchema)
        {
        }

        public static WmsTileSource Create(WmsInfo info)
        {
            var schema = new TileSchema
            {
                Name = info.Layer.Name,
                Srs = info.BoundingBox.CRS,
                // todo: hard coded values
                Width = 256,
                Height = 256,
                Format = "image/png",
                Extent = new Extent(info.BoundingBox.MinX, info.BoundingBox.MinY, info.BoundingBox.MaxX,
                    info.BoundingBox.MaxY),
                // origin?
                OriginX = info.BoundingBox.MinX,
                OriginY = info.BoundingBox.MinY
            };
            

            var onlineResource = info.WmsCapabilities.Capability.Request.GetCapabilities.DCPType[0].Http.Get.OnlineResource;
            return new WmsTileSource(new WebTileProvider(new WmscRequest(new Uri(onlineResource.Href), schema, null, null, new Dictionary<string, string>())),
                schema);
        }
    }
}