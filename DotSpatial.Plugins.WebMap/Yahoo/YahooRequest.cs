using System;
using BruTile;
using BruTile.Web;

namespace DotSpatial.Plugins.WebMap.Yahoo
{
    internal class YahooRequest : IRequest
    {
        //http://maps1.yimg.com/hx/tl?b=1&v=4.3&t=h&.intl=en&x=14&y=5&z=7&r=1
        private const string hybridURL = @"http://maps{0}.yimg.com/hx/tl?v=4.3&t=h&.intl=en&x={1}&y={2}&z={3}&r=1";
        //http://maps3.yimg.com/ae/ximg?v=1.9&t=a&s=256&.intl=en&x=15&y=7&z=7&r=1
        private const string sateliteURL = @"http://maps{0}.yimg.com/ae/ximg?v=1.9&t=a&s=256&.intl=en&x={1}&y={2}&z={3}&r=1";
        //http://maps1.yimg.com/hx/tl?b=1&v=4.3&.intl=en&x=12&y=7&z=7&r=1
        private const string normalURL = @"http://maps{0}.yimg.com/hx/tl?v=4.3&.intl=en&x={1}&y={2}&z={3}&r=1";

        private YahooMapType _mapType;

        public YahooRequest(YahooMapType mapType)
        {
            _mapType = mapType;
        }

        public Uri GetUri(TileInfo info)
        {
            var x = info.Index.Col;
            var y = info.Index.Row;
            var zoomLevel = Convert.ToInt32(info.Index.Level);

            string url = string.Empty;

            int serverNum = ((GetServerNum(x, y, 2)) + 1);
            int posY = (((1 << zoomLevel) >> 1) - 1 - y);
            int zoom = zoomLevel + 1;

            switch (_mapType)
            {
                case YahooMapType.Normal:
                {
                    url = string.Format(normalURL, serverNum, x, posY, zoom);
                }
                    break;
                case YahooMapType.Satellite:
                {
                    url = string.Format(sateliteURL, serverNum, x, posY, zoom);
                }
                    break;
                case YahooMapType.Hybrid:
                {
                    url = string.Format(hybridURL, serverNum, x, posY, zoom);
                }
                    break;
            }

            return new Uri(url);
        }


        private int GetServerNum(int x, int y, int max)
        {
            return (x + 2 * y) % max;
        }
    }
}