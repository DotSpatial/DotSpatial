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
        private readonly FileCache _tileCache;

        public BrutileServiceProvider(string name, ITileSource tileSource, bool useCache = true) : base(name)
        {
            if (useCache)
            {
                _tileCache = new FileCache(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "TileCache", name), "", new TimeSpan(30, 0, 0, 0));
            }
            TileSource = tileSource;
        }

        public override Bitmap GetBitmap(int x, int y, Envelope envelope, int zoom)
        {
            if (TileSource == null) return null;
            try
            {
                var index = new TileIndex(x, y, zoom.ToString(CultureInfo.InvariantCulture));
                var bytes = _tileCache != null? _tileCache.Find(index) : null;
                if (bytes == null)
                {
                    var extent = ToBrutileExtent(envelope);
                    var tileInfo = TileSource.Schema.GetTilesInView(extent, zoom).First();
                    tileInfo.Index = index;
                    bytes = TileSource.Provider.GetTile(tileInfo);
                    if (_tileCache != null)
                    {
                        _tileCache.Add(index, bytes);
                    }
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

        private static Extent ToBrutileExtent(IEnvelope extent)
        {
            return new Extent(extent.Minimum.X, extent.Minimum.Y, extent.Maximum.X, extent.Maximum.Y);
        }
    }
}