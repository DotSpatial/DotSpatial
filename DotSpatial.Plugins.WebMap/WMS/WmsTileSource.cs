using System;
using System.Collections.Generic;
using BruTile;
using BruTile.Web;

namespace DotSpatial.Plugins.WebMap.WMS
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
                Format = "image/png",
                Srs = info.CRS,
                Height = 256,
                Width = 256,
            };

            var onlineResource = info.WmsCapabilities.Capability.Request.GetCapabilities.DCPType[0].Http.Get.OnlineResource.Href;
            return new WmsTileSource(new WebTileProvider(new WmsRequest(new Uri(onlineResource), 
                schema,
                new List<string>{info.Layer.Name},
                info.Style == null? null : new List<string>{info.Style},
                info.CustomParameters, info.WmsCapabilities.Version.VersionString)),
                schema);
        }
    }
}