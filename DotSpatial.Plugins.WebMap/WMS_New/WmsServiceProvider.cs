using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using BruTile;
using BruTile.Cache;
using DotSpatial.Projections;
using DotSpatial.Topology;

namespace DotSpatial.Plugins.WebMap.WMS_New
{
    public class WmsServiceProvider : BrutileServiceProvider
    {
        private WmsInfo _data;
        private static readonly ProjectionInfo Wgs84Proj = ProjectionInfo.FromEsriString(KnownCoordinateSystems.Geographic.World.WGS1984.ToEsriString());
        private static readonly WGS84Schema Wgs84Schema = new WGS84Schema();

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
            return base.GetBitmap(x, y, envelope, zoom);

            // try with DS reprojections
            var ts = TileSource;
            if (ts == null) return null;
            try
            {
                var index = new TileIndex(x, y, zoom.ToString(CultureInfo.InvariantCulture));
                var tc = TileCache;
                var bytes = tc != null ? tc.Find(index) : null;
                if (bytes == null)
                {
                    var extent = ToBrutileExtent(envelope);
                    
                    var tileInfo = Wgs84Schema.GetTilesInView(extent, zoom.ToString()).FirstOrDefault();
                    if (tileInfo == null) return null;

                    //var an = Wgs84Schema.GetExtentOfTilesInView(extent, zoom.ToString(CultureInfo.InvariantCulture));
                    //an = tileInfo.Extent;

                    double[] mapVertices =
                    {
                        tileInfo.Extent.MinX, tileInfo.Extent.MaxY,
                        tileInfo.Extent.MaxX, tileInfo.Extent.MinY
                    };

                    //var mapVertices = new[]
                    //{
                    //    envelope.TopLeft().X, envelope.TopLeft().Y,
                    //    envelope.BottomRight().X, envelope.BottomRight().Y
                    //};
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
                    return ExceptionToBitmap(ex, TileSource.Schema.Width, TileSource.Schema.Height);
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