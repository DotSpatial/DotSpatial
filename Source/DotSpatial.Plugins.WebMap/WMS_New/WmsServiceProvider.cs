using System.Windows.Forms;
using BruTile.Cache;

namespace DotSpatial.Plugins.WebMap.WMS_New
{
    public class WmsServiceProvider : BrutileServiceProvider
    {
        private WmsInfo _data;

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

        public override bool NeedConfigure
        {
            get { return _data == null; }
        }
    }
}