using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using BruTile;
using BruTile.Web;

namespace DotSpatial.Plugins.WebMap.WMS
{
    public class WmsRequest : IRequest
    {
        private readonly string _fixedUrl;

        public WmsRequest(Uri baseUrl, TileSchema schema, IEnumerable<string> layers, IEnumerable<string> styles, IDictionary<string, string> customParameters, string version)
        {
            // Prepare url string
            var au = baseUrl.AbsoluteUri;
            if (au.IndexOf("SERVICE=WMS", StringComparison.OrdinalIgnoreCase) == -1)
            {
                if (!au.EndsWith("?")) au += "?";
                au += "&SERVICE=WMS";
            }
            var url = new StringBuilder(au);
            if (!string.IsNullOrEmpty(version)) url.AppendFormat("&VERSION={0}", version);
            url.Append("&REQUEST=GetMap");
            url.AppendFormat("&FORMAT={0}", schema.Format);
            var crsFormat = !string.IsNullOrEmpty(version) && string.CompareOrdinal(version, "1.3.0") >= 0 ? "&CRS={0}" : "&SRS={0}";
            url.AppendFormat(crsFormat, schema.Srs);
            url.AppendFormat("&LAYERS={0}", ToCommaSeparatedValues(layers));
            url.AppendFormat("&STYLES={0}", ToCommaSeparatedValues(styles));
            url.AppendFormat("&WIDTH={0}", schema.Width);
            url.AppendFormat("&HEIGHT={0}", schema.Height);
            if (customParameters != null)
            {
                foreach (var name in customParameters.Keys)
                {
                    url.AppendFormat("&{0}={1}", name, customParameters[name]);
                }
            }
            _fixedUrl = url.ToString();
        }

        /// <summary>
        /// Generates a URI at which to get the data for a tile.
        /// </summary>
        /// <param name="info">Information about a tile.</param>
        /// <returns>The URI at which to get the data for the specified tile.</returns>
        public Uri GetUri(TileInfo info)
        {
            return new Uri(_fixedUrl + string.Format("&BBOX={0}", info.Extent));
        }

        private static string ToCommaSeparatedValues(IEnumerable<string> items)
        {
            if (items == null) return string.Empty;
            var result = new StringBuilder();
            foreach (var str in items)
            {
                result.AppendFormat(CultureInfo.InvariantCulture, ",{0}", str);
            }
            if (result.Length > 0) result.Remove(0, 1);
            return result.ToString();
        }
    }
}