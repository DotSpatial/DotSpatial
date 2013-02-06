using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BruTile;
using BruTile.Cache;
using BruTile.PreDefined;
using BruTile.Web;
using DotSpatial.Plugins.WebMap.Resources;
using DotSpatial.Plugins.WebMap.WMS;
using DotSpatial.Projections;
using DotSpatial.Topology;
using Exception = System.Exception;
using Point = DotSpatial.Topology.Point;

namespace DotSpatial.Plugins.WebMap.Tiling
{
    internal class TileManager
    {
        #region Private

        private TileCache _tileCache;
        private string _tileServerName;
        private string _tileServerUrl;

        #endregion

        #region Properties

        public string TileServerURL
        {
            get
            {
                return _tileServerUrl;
            }
        }

        public string TileServerName
        {
            get
            {
                return _tileServerName;
            }
        }

        public WmsServerInfo WmsServerInfo { get; set; }

        #endregion

        public bool ShowErrorInTile = true;

        /// <summary>
        ///
        /// </summary>
        public TileManager()
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tileServerName"></param>
        /// <param name="tileServerUrl"></param>
        public TileManager(string tileServerName, string tileServerUrl)
        {
            _tileServerUrl = tileServerUrl;
            _tileServerName = tileServerName;

            _tileCache = new TileCache(_tileServerName);
        }

        public ITileSource TileSource { get; private set; }

        public ITileCache<byte[]> TileCache { get; private set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tileServerName"></param>
        /// <param name="tileServerUrl"></param>
        public void ChangeService(string tileServerName, string tileServerUrl)
        {
            _tileServerUrl = tileServerUrl;
            _tileServerName = tileServerName;

            _tileCache = new TileCache(_tileServerName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="envelope"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="envelope"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public Tile GetTile(int x, int y, Envelope envelope, int zoom)
        {
            Bitmap bitmap = _tileCache.Get(zoom, x, y);
            if (null == bitmap)
            {
                bitmap = GetViaBrutile(x, y, zoom, envelope);
            }
            if (null != bitmap)
            {
                var tile = new Tile(x, y, zoom, envelope, bitmap);
                return tile;
            }
            try
            {
                string url = _tileServerUrl;

                if (url.Contains("{key}"))
                {
                    string quadKey = TileCalculator.TileXYToBingQuadKey(x, y, zoom);
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

                //Put the tile in the cache
                _tileCache.Put(tile);

                return tile;
            }
            catch (Exception ex)
            {
                // We may see a 400 (Bad Request) when the user is zoomed in too far.
                Debug.WriteLine(ex.Message);

                //Return a No Data Available tile
                var noDataTile = new Tile(x, y, zoom, envelope, resources.NoDataTile);

                return noDataTile;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="point"></param>
        /// <param name="envelope"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public Tile GetTile(Point point, Envelope envelope, int zoom)
        {
            return GetTile((int)point.X, (int)point.Y, envelope, zoom);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public Tile GetTileFromLatLong(Coordinate coord, int zoom)
        {
            var tileXY = TileCalculator.LatLongToTileXY(coord, zoom);

            //Figure out the extent of the tile so that it can be made into MWImageData
            var tileTopLeftXY = TileCalculator.TileXYToTopLeftPixelXY((int)tileXY.X, (int)tileXY.Y);
            var tileBottomRightXY = TileCalculator.TileXYToTopLeftPixelXY((int)tileXY.X + 1, (int)tileXY.Y + 1);

            var tileTopLeft = TileCalculator.PixelXYToLatLong((int)tileTopLeftXY.X, (int)tileTopLeftXY.Y, zoom);
            var tileBottomRight = TileCalculator.PixelXYToLatLong((int)tileBottomRightXY.X, (int)tileBottomRightXY.Y, zoom);

            var envelope = new Envelope(tileTopLeft, tileBottomRight);

            return GetTile(tileXY, envelope, zoom);
        }

        private Bitmap GetViaBrutile(int x, int y, int zoom, IEnvelope envelope)
        {
            if (!InitializeBrutileProvider())
                return null;

            var extent = ToBrutileExtent(new Data.Extent(envelope));
            var tiles = TileSource.Schema.GetTilesInView(extent, zoom).ToList();
            //Debug.Assert(tiles.Count() == 1);

            var tileInfo = tiles[0];
            tileInfo.Index = new TileIndex(x, y, zoom);

            try
            {
                byte[] bytes = TileSource.Provider.GetTile(tileInfo);
                return new Bitmap(new MemoryStream(bytes));
            }
            catch (WebException ex)
            {
                if (ShowErrorInTile)
                {
                    //hack: an issue with this method is that one an error tile is in the memory cache it will stay even
                    //if the error is resolved. PDD.
                    using (var bitmap = new Bitmap(TileSource.Schema.Width, TileSource.Schema.Height))
                    {
                        using (var graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.DrawString(ex.Message, new Font(FontFamily.GenericSansSerif, 12), new SolidBrush(Color.Black), new RectangleF(0, 0, TileSource.Schema.Width, TileSource.Schema.Height));
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
            catch (Exception ex)
            {
                //todo: log and use other ways to report to user.
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        private bool InitializeBrutileProvider()
        {
            if (this.TileServerName.Equals(Properties.Resources.BingHybrid, StringComparison.InvariantCultureIgnoreCase))
            {
                string token = String.Empty;
                string url = string.IsNullOrWhiteSpace(token)
                ? BingRequest.UrlBingStaging
                : BingRequest.UrlBing;

                TileSource = new BingTileSource(new BingRequest(url, string.Empty, BingMapType.Hybrid));
                TileCache = new MemoryCache<byte[]>(100, 200);

                return true;
            }

            if (this.TileServerName.Equals(Properties.Resources.GoogleSatellite, StringComparison.InvariantCultureIgnoreCase))
            {
                TileSource = new GoogleTileSource(GoogleMapType.GoogleSatellite);
                TileCache = new MemoryCache<byte[]>(100, 200);

                return true;
            }

            if (this.TileServerName.Equals(Properties.Resources.GoogleMap, StringComparison.InvariantCultureIgnoreCase))
            {
                TileSource = new GoogleTileSource(GoogleMapType.GoogleMap);
                TileCache = new MemoryCache<byte[]>(100, 200);

                return true;
            }

            if (this.TileServerName.Equals(Properties.Resources.WMSMap, StringComparison.InvariantCultureIgnoreCase))
            {
                TileSource = WmsTileSource.Create(WmsServerInfo);
                TileCache = new MemoryCache<byte[]>(100, 200);

                return true;
            }

            TileSource = null;
            TileCache = null;

            return false;
        }

        private static Extent ToBrutileExtent(Data.Extent extent)
        {
            return new Extent(extent.MinX, extent.MinY, extent.MaxX, extent.MaxY);
        }
    }
}