using System;
using System.Collections.Generic;
using BruTile;
using BruTile.Web;

namespace DotSpatial.Plugins.WebMap.WMS
{
    public class WmsTileSource : ITileSource
    {
        private readonly ITileProvider _provider;
        private readonly ITileSchema _schema;

        public WmsTileSource(ITileProvider provider, ITileSchema schema)
        {
            _provider = provider;
            _schema = schema;
        }

        public ITileProvider Provider
        {
            get { return _provider; }
        }

        public ITileSchema Schema
        {
            get { return _schema; }
        }

        public static WmsTileSource Create(WmsServerInfo serverInfo)
        {
            var href = serverInfo.OnlineResource;
            var layers = new List<string>
                             {
                                 serverInfo.Layer
                             };

            var schema =
                //new GlobalMercator("image/png");
                //new UnProjected();
                new WmsTileSchema("schema", new Extent(-180, -90, 180, 90), "CRS:84", "image/png", 256, 4.291534423828125e-10, AxisDirection.Normal); 
            var request = new WmscRequest(new Uri(href), schema, layers,
                                          new List<string>(),               // styles
                                          new Dictionary<string, string>(), // custom parameters
                                          serverInfo.Version);
            var provider = new WebTileProvider(request);
            return new WmsTileSource(provider, schema);
        }
    }
}