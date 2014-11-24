using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using BruTile;
using BruTile.Cache;
using BruTile.Web;
using DotSpatial.Topology;

namespace DotSpatial.Plugins.WebMap
{
    public class BrutileServiceProvider : ServiceProvider
    {
        protected ITileSource TileSource { get; set; }
        protected ITileCache<byte[]> TileCache { get; set; }

        public BrutileServiceProvider(string name, ITileSource tileSource, ITileCache<byte[]> tileCache) : base(name)
        {
            TileCache = tileCache;
            TileSource = tileSource;
        }

        public override Bitmap GetBitmap(int x, int y, Envelope envelope, int zoom)
        {
            var ts = TileSource;
            if (ts == null) return null;

            Bitmap bitMap = null;
            var zoomS = zoom.ToString(CultureInfo.InvariantCulture);
            var extent = ToBrutileExtent(envelope);
            var tileInfo = ts.Schema.GetTilesInView(extent, zoomS).FirstOrDefault();

            try
            {
                var index = new TileIndex(x, y, zoomS);
                var tc = TileCache;
                var bytes = tc != null ? tc.Find(index) : null;
                if (bytes == null)
                {
                    if (tileInfo == null)
                    {
                        return null;
                    }
                    tileInfo.Index = index;
                    bytes = ts.Provider.GetTile(tileInfo);
                    bitMap = new Bitmap(new MemoryStream(bytes));
                    if (tc != null)
                    {
                        tc.Add(index, bytes);
                    }
                    return bitMap;
                }
                return new Bitmap(new MemoryStream(bytes));
            }
            catch (Exception ex)
            {
                if (ex is WebException ||
                    ex is TimeoutException)
                {
                    bitMap = ExceptionToBitmap(ex, TileSource.Schema.GetTileWidth(zoomS), TileSource.Schema.GetTileHeight(zoomS));
                }
                else
                    Debug.WriteLine(ex.Message);
            }

            // Esri Hyro Base Map Fix, the server doesn't put image in the response header.
            if (ts is ArcGisTileSource)
            {
                try
                {
                    string str = (ts as ArcGisTileSource).BaseUrl + "/tile/{zoom}/{y}/{x}";
                    if (str != null)
                    {
                        if (!str.Contains("{key}"))
                        {
                            str = str.Replace("{zoom}", zoomS);
                            str = str.Replace("{x}", x.ToString());
                            str = str.Replace("{y}", y.ToString());

                            Stream stream = (new WebClient()).OpenRead(str);
                            if (stream != null)
                            {
                                bitMap = new Bitmap(stream);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                }
            }

            return bitMap;
        }

        protected static Extent ToBrutileExtent(IEnvelope extent)
        {
            return new Extent(extent.Minimum.X, extent.Minimum.Y, extent.Maximum.X, extent.Maximum.Y);
        }
    }
}