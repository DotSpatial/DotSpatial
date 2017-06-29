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
using DotSpatial.Topology;

namespace DotSpatial.Plugins.WebMap.WMS
{
    public class WmsServiceProvider : BrutileServiceProvider
    {
        private WmsInfo _data;
        private static readonly ProjectionInfo Wgs84Proj = ProjectionInfo.FromEsriString(KnownCoordinateSystems.Geographic.World.WGS1984.ToEsriString());

        public WmsServiceProvider(string name) : 
            base(name, null, new MemoryCache<byte[]>())
        {
            Configure = delegate
            {
                using (var wmsDialog = new WMSServerParameters(_data))
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

        public override Bitmap GetBitmap(int x, int y, Envelope envelope, int zoom)
        {
            var ts = TileSource;
            if (ts == null) return null;

            var zoomS = zoom.ToString(CultureInfo.InvariantCulture);
            try
            {
                var index = new TileIndex(x, y, zoom.ToString(CultureInfo.InvariantCulture));
                var tc = TileCache;
                var bytes = tc != null ? tc.Find(index) : null;
                if (bytes == null)
                {
                    var mapVertices = new[]
                    {
                        envelope.TopLeft().X, envelope.TopLeft().Y,
                        envelope.BottomRight().X, envelope.BottomRight().Y
                    };
                    double[] viewExtentZ = { 0.0, 0.0 };
                    Reproject.ReprojectPoints(mapVertices, viewExtentZ, Wgs84Proj, _data.CrsProjectionInfo, 0, mapVertices.Length / 2);
                    var geogEnv = new Envelope(mapVertices[0], mapVertices[2], mapVertices[1], mapVertices[3]);
                    bytes = ts.Provider.GetTile(new TileInfo {Extent = ToBrutileExtent(geogEnv), Index = index});
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
                    return ExceptionToBitmap(ex, TileSource.Schema.GetTileWidth(zoomS), TileSource.Schema.GetTileHeight(zoomS));
                }
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public override bool NeedConfigure
        {
            get { return _data == null; }
        }
    }
}