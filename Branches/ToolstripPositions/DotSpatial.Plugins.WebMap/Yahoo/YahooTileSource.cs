using BruTile;
using BruTile.Cache;
using BruTile.Predefined;
using BruTile.Web;

namespace DotSpatial.Plugins.WebMap.Yahoo
{
    internal class YahooTileSource : ITileSource
    {
        private readonly ITileSchema _tileSchema;
        private readonly ITileProvider _tileProvider;

        public ITileProvider Provider
        {
            get { return _tileProvider; }
        }

        public ITileSchema Schema
        {
            get { return _tileSchema; }
        }

        public string Title { get; private set; }

        public YahooTileSource(YahooMapType mapType)
            : this(new YahooRequest(mapType))
        {
        }

        public YahooTileSource(YahooRequest request, IPersistentCache<byte[]> persistentCache = null)
        {
            _tileSchema = new SphericalMercatorInvertedWorldSchema();
            _tileProvider = new WebTileProvider(request, persistentCache);
        }
    }

    internal enum YahooMapType
    {
        Normal,
        Satellite,
        Hybrid,
    }
}
