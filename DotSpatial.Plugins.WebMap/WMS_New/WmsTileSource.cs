using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BruTile;
using BruTile.Predefined;
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
            TileSchema schema;
            if (info.CRS == "EPSG:3857")
            {
                schema = new GlobalSphericalMercator();
            }
            else if (info.CRS == "EPSG:4326")
            {
                // not working
                schema = new WGS84();
            }
            else
            {
                // todo: another CRS
                schema = null;
            }

            var onlineResource = info.WmsCapabilities.Capability.Request.GetCapabilities.DCPType[0].Http.Get.OnlineResource;
            return new WmsTileSource(new WebTileProvider(new WmsRequest(new Uri(onlineResource.Href), schema,
                new List<string>{info.Layer.Name},
                info.Style == null? null : new List<string>{info.Style},
                info.CustomParameters, info.WmsCapabilities.Version.VersionString)),
                schema);
        }
    }

    public class WGS84 : TileSchema
    {
        public WGS84()
        {
            var resolutions = new[]
            {
                0.703125, 0.3515625, 0.17578125, 0.087890625,
                0.0439453125, 0.02197265625, 0.010986328125, 0.0054931640625,
                0.00274658203125, 0.001373291015625, 0.0006866455078125, 3.4332275390625e-4,
                1.71661376953125e-4, 8.58306884765625e-5, 4.291534423828125e-5,
                2.1457672119140625E-5, 1.0728836059570312E-5, 5.364418029785156E-6, 2.682209014892578E-6,
                1.341104507446289E-6,
                6.705522537231445E-7, 3.3527612686157227E-7,
                1.6763806343078613E-7, 8.381903171539307E-8, 4.190951585769653E-8, 2.0954757928848267E-8,
                1.0477378964424133E-8, 5.238689482212067E-9,
                2.6193447411060333E-9, 1.3096723705530167E-9, 6.548361852765083E-10
            };
            
            for (var i = 0; i < resolutions.Length; i++)
            {
                Resolutions[i.ToString(CultureInfo.InvariantCulture)] = new Resolution
                {
                    Id = i.ToString(CultureInfo.InvariantCulture),
                    UnitsPerPixel = resolutions[i]
                };
            }
            Height = 256;
            Width = 256;
            Extent = new Extent(-180, -90, 180, 90);
            OriginX = -180;
            OriginY = -90;
            Name = "WGS 84";
            Format = "image/png";
            Axis = AxisDirection.Normal;
            Srs = "EPSG:4326";
        }
    }

    public class WmsRequest : IRequest
    {
        readonly Uri _baseUrl;
        readonly IDictionary<string, string> _customParameters;
        readonly IList<string> _layers;
        private readonly ITileSchema _schema;
        readonly IList<string> _styles;
        private readonly string _version;

        public WmsRequest(Uri baseUrl, ITileSchema schema, IList<string> layers, IList<string> styles, IDictionary<string, string> customParameters, string version)
        {
            _baseUrl = baseUrl;
            _customParameters = customParameters;
            _layers = layers;
            _schema = schema;
            _styles = styles;
            _version = version;
        }

        /// <summary>
        /// Generates a URI at which to get the data for a tile.
        /// </summary>
        /// <param name="info">Information about a tile.</param>
        /// <returns>The URI at which to get the data for the specified tile.</returns>
        public Uri GetUri(TileInfo info)
        {
            var initStr = _baseUrl.AbsoluteUri;
            if (!initStr.EndsWith("?")) initStr = initStr + "?";
            var url = new StringBuilder(initStr);
            url.Append("&SERVICE=WMS");
            if (!string.IsNullOrEmpty(_version)) url.AppendFormat("&VERSION={0}", _version);
            url.Append("&REQUEST=GetMap");
            url.AppendFormat("&BBOX={0}", TileTransform.TileToWorld(new TileRange(info.Index.Col, info.Index.Row), info.Index.Level, _schema));
            url.AppendFormat("&FORMAT={0}", _schema.Format);
            url.AppendFormat("&WIDTH={0}", _schema.Width);
            url.AppendFormat("&HEIGHT={0}", _schema.Height);
            var crsFormat = !string.IsNullOrEmpty(_version) && string.CompareOrdinal(_version, "1.3.0") >= 0 ? "&CRS={0}" : "&SRS={0}";
            url.AppendFormat(crsFormat, _schema.Srs);
            url.AppendFormat("&LAYERS={0}", ToCommaSeparatedValues(_layers));
            url.AppendFormat("&STYLES={0}", _styles != null? ToCommaSeparatedValues(_styles) : "");
            AppendCustomParameters(url);
            return new Uri(url.ToString());
        }

        private static string ToCommaSeparatedValues(IEnumerable<string> items)
        {
            var result = new StringBuilder();
            foreach (string str in items)
            {
                result.AppendFormat(CultureInfo.InvariantCulture, ",{0}", str);
            }
            if (result.Length > 0) result.Remove(0, 1);
            return result.ToString();
        }

        private void AppendCustomParameters(StringBuilder url)
        {
            if (_customParameters != null && _customParameters.Count > 0)
            {
                foreach (string name in _customParameters.Keys)
                {
                    url.AppendFormat("&{0}={1}", name, _customParameters[name]);
                }
            }
        }
    }
}