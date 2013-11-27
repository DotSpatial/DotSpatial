using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using BruTile;
using BruTile.Cache;
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
            try
            {
                var zoomS = zoom.ToString(CultureInfo.InvariantCulture);
                var index = new TileIndex(x, y, zoomS);
                var tc = TileCache;
                var bytes = tc != null ? tc.Find(index) : null;
                if (bytes == null)
                {
                    var extent = ToBrutileExtent(envelope);
                    var tileInfo = ts.Schema.GetTilesInView(extent, zoomS).FirstOrDefault();
                    if (tileInfo == null)
                    {
                        return null;
                    }
                    tileInfo.Index = index;
                    bytes = ts.Provider.GetTile(tileInfo);
                    var bm = new Bitmap(new MemoryStream(bytes));
                    if (tc != null)
                    {
                        tc.Add(index, bytes);
                    }
                    return bm;
                }
                return new Bitmap(new MemoryStream(bytes));
            }
            catch (Exception ex)
            {
                if (ex is WebException ||
                    ex is TimeoutException)
                {
                    return ExceptionToBitmap(ex, TileSource.Schema.Width, TileSource.Schema.Height);
                }
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        protected static Extent ToBrutileExtent(IEnvelope extent)
        {
            return new Extent(extent.Minimum.X, extent.Minimum.Y, extent.Maximum.X, extent.Maximum.Y);
        }
    }
}