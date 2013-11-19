using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BruTile;
using BruTile.Cache;
using BruTile.Predefined;
using BruTile.Web;
using DotSpatial.Data;
using DotSpatial.Plugins.WebMap.Properties;
using DotSpatial.Plugins.WebMap.WMS;
using DotSpatial.Plugins.WebMap.Yahoo;
using DotSpatial.Topology;
using Exception = System.Exception;
using Extent = BruTile.Extent;
using Point = DotSpatial.Topology.Point;

namespace DotSpatial.Plugins.WebMap.Tiling
{
    internal class TileManager
    {
        #region Private
        
        private readonly string _tileServerName;
        private readonly string _tileServerUrl;
        private WmsServerInfo WmsServerInfo { get; set; }
        private readonly ITileSource _tileSource;
        private FileCache TileCache { get; set; }

        #endregion

        public TileManager(string tileServerName, string tileServerUrl, WmsServerInfo wmsServerInfo)
        {
            if (tileServerName == null) throw new ArgumentNullException("tileServerName");

            _tileServerUrl = tileServerUrl;
            _tileServerName = tileServerName;
            WmsServerInfo = wmsServerInfo;
            TileCache = new FileCache(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TileCache", _tileServerName), "", new TimeSpan(30, 0, 0, 0));
            _tileSource = InitializeBrutileProvider();
        }
        
        public Tile[,] GetTiles(Envelope envelope, Rectangle bounds)
        {
            Coordinate mapTopLeft = envelope.TopLeft();
            Coordinate mapBottomRight = envelope.BottomRight();

            //Clip the coordinates so they are in the range of the web mercator projection
            mapTopLeft.Y = TileCalculator.Clip(mapTopLeft.Y, TileCalculator.MinLatitude, TileCalculator.MaxLatitude);
            mapTopLeft.X = TileCalculator.Clip(mapTopLeft.X, TileCalculator.MinLongitude, TileCalculator.MaxLongitude);

            mapBottomRight.Y = TileCalculator.Clip(mapBottomRight.Y, TileCalculator.MinLatitude, TileCalculator.MaxLatitude);
            mapBottomRight.X = TileCalculator.Clip(mapBottomRight.X, TileCalculator.MinLongitude, TileCalculator.MaxLongitude);

            int zoom = TileCalculator.DetermineZoomLevel(envelope, bounds);

            Point topLeftTileXY = TileCalculator.LatLongToTileXY(mapTopLeft, zoom);
            Point btmRightTileXY = TileCalculator.LatLongToTileXY(mapBottomRight, zoom);

            var tileMatrix = new Tile[(int)(btmRightTileXY.X - topLeftTileXY.X) + 1, (int)(btmRightTileXY.Y - topLeftTileXY.Y) + 1];

            Parallel.For((int) topLeftTileXY.Y, (int) btmRightTileXY.Y + 1,
                         y => Parallel.For((int) topLeftTileXY.X, (int) btmRightTileXY.X + 1,
                                           x =>
                                               {
                                                   var currTopLeftPixXY = TileCalculator.TileXYToTopLeftPixelXY(x, y);
                                                   var currTopLeftCoord =TileCalculator.PixelXYToLatLong((int) currTopLeftPixXY.X,
                                                                                       (int) currTopLeftPixXY.Y, zoom);

                                                   var currBtmRightPixXY = TileCalculator.TileXYToBottomRightPixelXY(x,
                                                                                                                     y);
                                                   var currBtmRightCoord =TileCalculator.PixelXYToLatLong((int) currBtmRightPixXY.X,
                                                                                       (int) currBtmRightPixXY.Y, zoom);

                                                   var currEnv = new Envelope(currTopLeftCoord, currBtmRightCoord);

                                                   var tile = GetTile(x, y, currEnv, zoom);
                                                   tileMatrix[x - (int) topLeftTileXY.X, y - (int) topLeftTileXY.Y] =tile;
                                               }
                                  ));

            return tileMatrix;
        }

        private Tile GetTile(int x, int y, Envelope envelope, int zoom)
        {
            var bitmap = GetViaBrutile(x, y, zoom, envelope);
            if (bitmap != null)
            {
                return new Tile(x, y, zoom, envelope, bitmap);
            }

            try
            {
                var url = _tileServerUrl;
                if (url == null)
                {
                    var noDataTile = new Tile(x, y, zoom, envelope, Resources.nodata);
                    return noDataTile;
                }

                if (url.Contains("{key}"))
                {
                    var quadKey = TileCalculator.TileXYToBingQuadKey(x, y, zoom);
                    url = url.Replace("{key}", quadKey);
                }
                else
                {
                    url = url.Replace("{zoom}", zoom.ToString());
                    url = url.Replace("{x}", x.ToString());
                    url = url.Replace("{y}", y.ToString());
                }

                var client = new WebClient();
                var stream = client.OpenRead(url);

                if (stream != null)
                    bitmap = new Bitmap(stream);

                var tile = new Tile(x, y, zoom, envelope, bitmap);

                if (stream != null)
                {
                    stream.Flush();
                    stream.Close();
                }

                return tile;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new Tile(x, y, zoom, envelope, Resources.nodata);
            }
        }

        private Bitmap GetViaBrutile(int x, int y, int zoom, IEnvelope envelope)
        {
            if (_tileSource == null) return null;

            try
            {
                // try cache first
                var index = new TileIndex(x, y, zoom.ToString(CultureInfo.InvariantCulture));
                var bytes = TileCache.Find(index);
                if (bytes == null)
                {
                    var extent = ToBrutileExtent(new Data.Extent(envelope));
                    var tileInfo = _tileSource.Schema.GetTilesInView(extent, zoom).First();
                    tileInfo.Index = index;
                    bytes = _tileSource.Provider.GetTile(tileInfo);
                    TileCache.Add(index, bytes);
                }
                return new Bitmap(new MemoryStream(bytes));
            }
            catch (Exception ex)
            {
                if (ex is WebException ||
                    ex is TimeoutException)
                {
                    using (var bitmap = new Bitmap(_tileSource.Schema.Width, _tileSource.Schema.Height))
                    {
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.DrawString(ex.Message, new Font(FontFamily.GenericSansSerif, 12), new SolidBrush(Color.Black), 
                                new RectangleF(0, 0, _tileSource.Schema.Width, _tileSource.Schema.Height));
                        }

                        using (var m = new MemoryStream())
                        {
                            bitmap.Save(m, ImageFormat.Png);
                            return new Bitmap(m);
                        }
                    }
                }
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        private ITileSource InitializeBrutileProvider()
        {
            var servEq = (Func<string, bool>) 
                (s => _tileServerName.Equals(s, StringComparison.InvariantCultureIgnoreCase));

            if (servEq(Resources.EsriWorldHydroBasemap))
            {
                return TileSource.Create(KnownTileServers.EsriWorldHydroBasemap);
            }
            if (servEq(Resources.EsriHydroBaseMap))
            {
                return new ArcGisTileSource("http://bmproto.esri.com/ArcGIS/rest/services/Hydro/HydroBase2009/MapServer/",
                    new GlobalMercator());
            }
            if (servEq(Resources.EsriWorldStreetMap))
            {
                return new ArcGisTileSource("http://server.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/",
                    new GlobalMercator());
            }
            if (servEq(Resources.EsriWorldImagery))
            {
                return new ArcGisTileSource("http://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/",
                    new GlobalMercator());
            }
            if (servEq(Resources.EsriWorldTopo))
            {
                return TileSource.Create(KnownTileServers.EsriWorldTopo);
            }
            if (servEq(Resources.BingHybrid))
            {
                return new BingTileSource(new BingRequest(BingRequest.UrlBingStaging, string.Empty, BingMapType.Hybrid));
            }
            if (servEq(Resources.BingAerial))
            {
                return new BingTileSource(new BingRequest(BingRequest.UrlBingStaging, string.Empty, BingMapType.Aerial));
            }
            if (servEq(Resources.BingRoads))
            {
                return new BingTileSource(new BingRequest(BingRequest.UrlBingStaging, string.Empty, BingMapType.Roads));
            }
            if (servEq(Resources.GoogleSatellite))
            {
                return new GoogleTileSource(GoogleMapType.GoogleSatellite);
            }
            if (servEq(Resources.GoogleMap))
            {
                return new GoogleTileSource(GoogleMapType.GoogleMap);
            }
            if (servEq(Resources.GoogleLabels))
            {
                return new GoogleTileSource(GoogleMapType.GoogleLabels);
            }
            if (servEq(Resources.GoogleTerrain))
            {
                return new GoogleTileSource(GoogleMapType.GoogleTerrain);
            }
            if (servEq(Resources.YahooNormal))
            {
                return new YahooTileSource(YahooMapType.Normal);
            }
            if (servEq(Resources.YahooSatellite))
            {
                return new YahooTileSource(YahooMapType.Satellite);
            }
            if (servEq(Resources.YahooHybrid))
            {
                return new YahooTileSource(YahooMapType.Hybrid);
            }
            if (servEq(Resources.OpenStreetMap))
            {
                return TileSource.Create();
            }
            if (servEq(Resources.WMSMap))
            {
                return WmsTileSource.Create(WmsServerInfo);
            }

            // No Match
            return null;
        }

        private static Extent ToBrutileExtent(IExtent extent)
        {
            return new Extent(extent.MinX, extent.MinY, extent.MaxX, extent.MaxY);
        }
    }
}