using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Forms;
using BruTile;
using BruTile.Cache;
using DotSpatial.Projections;
using GeoAPI.Geometries;

namespace DotSpatial.Plugins.WebMap.WMS
{
    /// <summary>
    /// WMS service provider.
    /// </summary>
    public class WmsServiceProvider : BrutileServiceProvider
    {
        #region Fields

        private static readonly ProjectionInfo Wgs84Proj = ProjectionInfo.FromEsriString(KnownCoordinateSystems.Geographic.World.WGS1984.ToEsriString());
        private WmsInfo _data;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WmsServiceProvider"/> class.
        /// </summary>
        /// <param name="name">Name of the service provider.</param>
        public WmsServiceProvider(string name)
            : base(name, null, new MemoryCache<byte[]>())
        {
            Configure = () =>
                {
                    using (var wmsDialog = new WmsServerParameters(_data))
                    {
                        if (wmsDialog.ShowDialog() != DialogResult.OK) return false;

                        _data = wmsDialog.WmsInfo;
                        if (_data != null)
                        {
                            TileSource = WmsTileSource.Create(_data);
                            TileCache = new MemoryCache<byte[]>();
                            return true;
                        }

                        return false;
                    }
                };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether configuration is needed.
        /// </summary>
        public override bool NeedConfigure => _data == null;

        #endregion

        #region Methods

        /// <inheritdoc />
        public override Bitmap GetBitmap(int x, int y, Envelope envelope, int zoom)
        {
            var ts = TileSource;
            if (ts == null) return null;

            var zoomS = zoom.ToString(CultureInfo.InvariantCulture);
            try
            {
                var index = new TileIndex(x, y, zoom.ToString(CultureInfo.InvariantCulture));
                var tc = TileCache;
                var bytes = tc?.Find(index);
                if (bytes == null)
                {
                    var mapVertices = new[] { envelope.MinX, envelope.MaxY, envelope.MaxX, envelope.MinY };
                    double[] viewExtentZ = { 0.0, 0.0 };
                    Reproject.ReprojectPoints(mapVertices, viewExtentZ, Wgs84Proj, _data.CrsProjectionInfo, 0, mapVertices.Length / 2);
                    var geogEnv = new Envelope(mapVertices[0], mapVertices[2], mapVertices[1], mapVertices[3]);
                    bytes = ts.Provider.GetTile(new TileInfo
                                                    {
                                                        Extent = ToBrutileExtent(geogEnv),
                                                        Index = index
                                                    });
                    var bm = new Bitmap(new MemoryStream(bytes));
                    tc?.Add(index, bytes);

                    return bm;
                }

                return new Bitmap(new MemoryStream(bytes));
            }
            catch (Exception ex)
            {
                if (ex is WebException || ex is TimeoutException)
                {
                    return ExceptionToBitmap(ex, TileSource.Schema.GetTileWidth(zoomS), TileSource.Schema.GetTileHeight(zoomS));
                }

                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        #endregion
    }
}