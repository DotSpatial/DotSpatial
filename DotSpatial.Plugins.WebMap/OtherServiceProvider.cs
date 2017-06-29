using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using DotSpatial.Plugins.WebMap.Tiling;
using DotSpatial.Topology;
using Microsoft.VisualBasic;

namespace DotSpatial.Plugins.WebMap
{
    public class OtherServiceProvider : ServiceProvider
    {
        private string _url;

        public OtherServiceProvider(string name, string url) : base(name)
        {
            _url = url;
            Configure = delegate
            {
                var dialogDefault = string.IsNullOrWhiteSpace(_url)
                    ? "http://tiles.virtualearth.net/tiles/h{key}.jpeg?g=461&mkt=en-us&n=z"
                    : _url;
                var guiUrl = Interaction.InputBox("Please provide the Url for the service.",
                    DefaultResponse: dialogDefault);
                if (!string.IsNullOrWhiteSpace(guiUrl))
                {
                    _url = guiUrl;
                    return true;
                }
                return false;
            };
        }

        public override Bitmap GetBitmap(int x, int y, Envelope envelope, int zoom)
        {
            try
            {
                var url = _url;
                if (url == null)
                {
                    return null;
                }

                if (url.Contains("{key}"))
                {
                    var quadKey = TileCalculator.TileXYToBingQuadKey(x, y, zoom);
                    url = url.Replace("{key}", quadKey);
                }
                else
                {
                    url = url.Replace("{zoom}", zoom.ToString(CultureInfo.InvariantCulture));
                    url = url.Replace("{x}", x.ToString(CultureInfo.InvariantCulture));
                    url = url.Replace("{y}", y.ToString(CultureInfo.InvariantCulture));
                }

                using (var client = new WebClient())
                {
                    var stream = client.OpenRead(url);
                    if (stream != null)
                    {
                        var bitmap = new Bitmap(stream);
                        stream.Flush();
                        stream.Close();
                        return bitmap;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is WebException ||
                    ex is TimeoutException)
                {
                    return ExceptionToBitmap(ex, 256, 256);
                }
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        public override bool NeedConfigure
        {
            get { return string.IsNullOrWhiteSpace(_url); }
        }
    }
}