using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Data;
using System.Drawing;
using System.Net;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using DotSpatial.Projections;
using DotSpatial.Topology;
//using System.Windows.Forms;
using DotSpatial.Controls;
using System.Globalization;
using DotSpatial.WebControls;
using System.Web.UI.WebControls;

namespace DotSpatial.MapWebClient
{

    public class TileInfo
    {
        /// <summary>
        /// Adreess of image
        /// </summary>
        public string url;

        /// <summary>
        /// Row
        /// </summary>
        public int row;

        /// <summary>
        /// Col
        /// </summary>
        public int col;

        /// <summary>
        /// Extent on the space
        /// </summary>
        public Extent Extent;
    }

    public class TileInfoSet
    {

        /// <summary>
        /// Rows Number
        /// </summary>
        public TileInfo[][] Tiles;

        /// <summary>
        /// Rows Number
        /// </summary>
        public int rows;

        /// <summary>
        /// Cols Number
        /// </summary>
        public int cols;

        /// <summary>
        /// Real Extent of the 
        /// </summary>
        public Extent Extent;

    }

    public class WMTClient : IWebClient
    {
        public bool Visible = true;
        public string Name = "Background";

        public WebServiceType Type;
        public ProjectionInfo Projection;
        
        private Extent _Extent;
        private Size _Size;

        public Extent TilesExtent;

        public WebProxy Proxy { get; set; }

        private double[] Resolutions;
        private int[] NumTileX;
        private int[] NumTileY;

        private int _NumLevels = 0;
        private int NumLevels
        {
            get
            {
                return _NumLevels;
            }
            set
            {
                _NumLevels = value;
                Array.Resize(ref Resolutions, _NumLevels);
                Array.Resize(ref NumTileX, _NumLevels);
                Array.Resize(ref NumTileY, _NumLevels);
            }
        }

        private bool InvertedAxis = true;

        public string Copyright;
  
        public void Create(WebServiceType ServiceType)
        {
            Type = ServiceType;

            switch (ServiceType)
            {

                #region -- Google --

                case (WebServiceType.GoogleMap):
                case (WebServiceType.GoogleSatellite):
                case (WebServiceType.GoogleLabels):
                case (WebServiceType.GoogleTerrain):
                case (WebServiceType.GoogleHybrid):

                case (WebServiceType.GoogleMapChina):
                case (WebServiceType.GoogleSatelliteChina):
                case (WebServiceType.GoogleLabelsChina):
                case (WebServiceType.GoogleTerrainChina):
                case (WebServiceType.GoogleHybridChina):
                    {
                        TryCorrectGoogleVersions();
                        Copyright = string.Format("©{0} Google - Map data ©{0} Tele Atlas, Imagery ©{0} TerraMetrics", DateTime.Today.Year);
                    }
                    break;
                #endregion

                #region -- Yahoo --
                case (WebServiceType.YahooMap):
                case (WebServiceType.YahooSatellite):
                case (WebServiceType.YahooLabels):
                case (WebServiceType.YahooHybrid):
                    {
                        Copyright = string.Format("© Yahoo! Inc. - Map data & Imagery ©{0} NAVTEQ", DateTime.Today.Year);
                    }
                    break;
                #endregion

                #region -- Bing --
                case (WebServiceType.BingMap):
                case (WebServiceType.BingMap_New):
                case (WebServiceType.BingSatellite):
                case (WebServiceType.BingHybrid):
                    {
                        TryCorrectBingVersions();
                        Copyright = string.Format("©{0} Microsoft Corporation, ©{0} NAVTEQ, ©{0} Image courtesy of NASA", DateTime.Today.Year);
                    }
                    break;
                #endregion

                #region -- ArcGIS --
                case (WebServiceType.ArcGIS_StreetMap_World_2D):
                case (WebServiceType.ArcGIS_Imagery_World_2D):
                case (WebServiceType.ArcGIS_ShadedRelief_World_2D):
                case (WebServiceType.ArcGIS_Topo_US_2D):
                case (WebServiceType.ArcGIS_World_Physical_Map):
                case (WebServiceType.ArcGIS_World_Shaded_Relief):
                case (WebServiceType.ArcGIS_World_Street_Map):
                case (WebServiceType.ArcGIS_World_Terrain_Base):
                case (WebServiceType.ArcGIS_World_Topo_Map):
                    {
                        Copyright = string.Format("©{0} ESRI - Map data ©{0} ArcGIS", DateTime.Today.Year);
                    }
                    break;

                #endregion

                #region -- OpenStreet --
                case (WebServiceType.OpenStreetMap):
                case (WebServiceType.OpenStreetOsm):
                case (WebServiceType.OpenStreetMapSurfer):
                case (WebServiceType.OpenStreetMapSurferTerrain):
                case (WebServiceType.OpenSeaMapLabels):
                case (WebServiceType.OpenSeaMapHybrid):
                case (WebServiceType.OpenCycleMap):
                    {
                        Copyright = string.Format("© OpenStreetMap - Map data ©{0} OpenStreetMap", DateTime.Today.Year);
                    }
                    break;
                #endregion

            }

            switch (ServiceType)
            {
                case (WebServiceType.ArcGIS_StreetMap_World_2D):
                case (WebServiceType.ArcGIS_Topo_US_2D):
                case (WebServiceType.ArcGIS_World_Topo_Map):
                    {
                        ProjectionInfo prjinfo = KnownCoordinateSystems.Geographic.World.WGS1984;
                        Extent MaxExtent = new Extent(-180.0, -90, 180, 90);
                        Size tlSize = new Size(256, 256);
                        int StartNumX = 2;
                        int StartNumY = 1;

                        CreateAuto(prjinfo, 20, MaxExtent, tlSize, StartNumX, StartNumY);
                    }
                    break;
                case (WebServiceType.ArcGIS_ShadedRelief_World_2D):
                    {
                        ProjectionInfo prjinfo = KnownCoordinateSystems.Geographic.World.WGS1984;
                        Extent MaxExtent = new Extent(-180.0, -90, 180, 90);
                        Size tlSize = new Size(256, 256);
                        int StartNumX = 2;
                        int StartNumY = 1;

                        CreateAuto(prjinfo, 12, MaxExtent, tlSize, StartNumX, StartNumY);
                    }
                    break;

                case (WebServiceType.ArcGIS_Imagery_World_2D):
                    {
                        ProjectionInfo prjinfo = KnownCoordinateSystems.Geographic.World.WGS1984;
                        Extent MaxExtent = new Extent(-180.0, -90, 180, 90);
                        Size tlSize = new Size(256, 256);
                        int StartNumX = 2;
                        int StartNumY = 1;

                        CreateAuto(prjinfo, 9, MaxExtent, tlSize, StartNumX, StartNumY);
                    }
                    break;

                default:
                    {
                        ProjectionInfo prjinfo = KnownCoordinateSystems.Projected.World.WebMercator;
                        Extent MaxExtent = new Extent(-20037508.342789, -20037508.342789, 20037508.342789, 20037508.342789);
                        Size tlSize = new Size(256, 256);
                        int StartNumX = 1;
                        int StartNumY = 1;

                        CreateAuto(prjinfo, 20, MaxExtent, tlSize, StartNumX, StartNumY);
                    }
                    break;
            }
        }

        public void CreateAuto(ProjectionInfo projection, ushort numLevels, Extent MaxExtent, Size tlSize, int StartNumX, int StartNumY)
        {
            if (numLevels <= 0 || StartNumX < 1 || StartNumY < 1 || MaxExtent.Width == 0 || MaxExtent.Height == 0)
            {
                throw new ArgumentException("Invalid CreateAuto Parameters");
            }

            Projection = projection;

            NumLevels = numLevels;
            _Size = tlSize;
            _Extent = MaxExtent;

            NumTileX[0] = StartNumX;
            NumTileY[0] = StartNumY;

            for (int l = 0; l < NumLevels; l++)
            {
                if (l > 0)
                {
                    NumTileX[l] = NumTileX[l - 1] * 2;
                    NumTileY[l] = NumTileY[l - 1] * 2;
                }

                //calcola la risoluzione di ogni pixel ai differenti livelli di zoom
                Resolutions[l] = Math.Min(_Extent.Width / (NumTileX[l] * _Size.Width), _Extent.Height / (NumTileY[l] * _Size.Height));

            }


        }

        private int GetZoomLevel(double res)
        {
            if (Resolutions.Length == 0)
            {
                throw new ArgumentException("No tile resolutions");
            }

            //smaller than smallest
            if (Resolutions[Resolutions.Length - 1] > res) return Resolutions.Length - 1;

            //bigger than biggest
            if (Resolutions[0] < res) return 0;

            int result = 0;
            double resultDistance = double.MaxValue;
            for (int i = 0; i < Resolutions.Length; i++)
            {
                double distance = Math.Abs(Resolutions[i] - res);
                if (distance < resultDistance)
                {
                    result = i;
                    resultDistance = distance;
                }
            }
            return result;
        }

        private int GetZoomLevel(Extent ext, Size sz)
        {
            double res = Math.Min(ext.Width / sz.Width, ext.Height / sz.Height);

            return GetZoomLevel(res);

        }

        private string GetUrl(int row, int col, int zoom, WebServiceType TileType)
        {

            // YandexMap
            //string VersionYandexMap = "2.16.0";
            //string VersionYandexSatellite = "1.19.0";

            /// <summary>
            /// Bing Maps Customer Identification, more info here
            /// http://msdn.microsoft.com/en-us/library/bb924353.aspx
            /// </summary>
            string BingMapsClientToken = null;

            string language = "english";

            switch (TileType)
            {
                #region -- Google --
                case WebServiceType.GoogleMap:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        // http://mt1.google.com/vt/lyrs=m@130&hl=lt&x=18683&s=&y=10413&z=15&s=Galile

                        return string.Format("http://{0}{1}.google.com/{2}/lyrs={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(row, col, 4), request, VersionGoogleMap, language, col, sec1, row, zoom, sec2);
                    }

                case WebServiceType.GoogleSatellite:
                    {
                        string server = "khm";
                        string request = "kh";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        return string.Format("http://{0}{1}.google.com/{2}/v={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(row, col, 4), request, VersionGoogleSatellite, language, col, sec1, row, zoom, sec2);
                    }

                case WebServiceType.GoogleLabels:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        // http://mt1.google.com/vt/lyrs=h@107&hl=lt&x=583&y=325&z=10&s=Ga
                        // http://mt0.google.com/vt/lyrs=h@130&hl=lt&x=1166&y=652&z=11&s=Galile

                        return string.Format("http://{0}{1}.google.com/{2}/lyrs={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(row, col, 4), request, VersionGoogleLabels, language, col, sec1, row, zoom, sec2);
                    }

                case WebServiceType.GoogleTerrain:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        return string.Format("http://{0}{1}.google.com/{2}/v={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(row, col, 4), request, VersionGoogleTerrain, language, col, sec1, row, zoom, sec2);
                    }
                #endregion

                #region -- Google (China) version --
                case WebServiceType.GoogleMapChina:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        // http://mt3.google.cn/vt/lyrs=m@123&hl=zh-CN&gl=cn&x=3419&y=1720&z=12&s=G

                        return string.Format("http://{0}{1}.google.cn/{2}/lyrs={3}&hl={4}&gl=cn&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(row, col, 4), request, VersionGoogleMapChina, "zh-CN", col, sec1, row, zoom, sec2);
                    }

                case WebServiceType.GoogleSatelliteChina:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        // http://mt1.google.cn/vt/lyrs=s@59&gl=cn&x=3417&y=1720&z=12&s=Gal

                        return string.Format("http://{0}{1}.google.cn/{2}/lyrs={3}&gl=cn&x={4}{5}&y={6}&z={7}&s={8}", server, GetServerNum(row, col, 4), request, VersionGoogleSatelliteChina, col, sec1, row, zoom, sec2);
                    }

                case WebServiceType.GoogleLabelsChina:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        // http://mt1.google.cn/vt/imgtp=png32&lyrs=h@123&hl=zh-CN&gl=cn&x=3417&y=1720&z=12&s=Gal

                        return string.Format("http://{0}{1}.google.cn/{2}/imgtp=png32&lyrs={3}&hl={4}&gl=cn&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(row, col, 4), request, VersionGoogleLabelsChina, "zh-CN", col, sec1, row, zoom, sec2);
                    }

                case WebServiceType.GoogleTerrainChina:
                    {
                        string server = "mt";
                        string request = "vt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        // http://mt2.google.cn/vt/lyrs=t@108,r@123&hl=zh-CN&gl=cn&x=3418&y=1718&z=12&s=Gali

                        return string.Format("http://{0}{1}.google.com/{2}/lyrs={3}&hl={4}&gl=cn&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(row, col, 4), request, VersionGoogleTerrainChina, "zh-CN", col, sec1, row, zoom, sec2);
                    }
                #endregion

                #region -- Google (Korea) version --
                case WebServiceType.GoogleMapKorea:
                    {
                        string server = "mt";
                        string request = "mt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        // http://mt0.gmaptiles.co.kr/mt/v=kr1.12&hl=lt&x=876&y=400&z=10&s=Gali

                        var ret = string.Format("http://{0}{1}.gmaptiles.co.kr/{2}/v={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(row, col, 4), request, VersionGoogleMapKorea, language, col, sec1, row, zoom, sec2);
                        return ret;
                    }

                case WebServiceType.GoogleSatelliteKorea:
                    {
                        string server = "khm";
                        string request = "kh";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        // http://khm1.google.co.kr/kh/v=59&x=873&y=401&z=10&s=Gali

                        return string.Format("http://{0}{1}.google.co.kr/{2}/v={3}&x={4}{5}&y={6}&z={7}&s={8}", server, GetServerNum(row, col, 4), request, VersionGoogleSatelliteKorea, col, sec1, row, zoom, sec2);
                    }

                case WebServiceType.GoogleLabelsKorea:
                    {
                        string server = "mt";
                        string request = "mt";
                        string sec1 = ""; // after &x=...
                        string sec2 = ""; // after &zoom=...
                        GetSecGoogleWords(row, col, out sec1, out sec2);

                        // http://mt3.gmaptiles.co.kr/mt/v=kr1t.12&hl=lt&x=873&y=401&z=10&s=Gali

                        return string.Format("http://{0}{1}.gmaptiles.co.kr/{2}/v={3}&hl={4}&x={5}{6}&y={7}&z={8}&s={9}", server, GetServerNum(row, col, 4), request, VersionGoogleLabelsKorea, language, col, sec1, row, zoom, sec2);
                    }
                #endregion

                #region -- Yahoo --
                case WebServiceType.YahooMap:
                    {
                        // http://maps1.yimg.com/hx/tl?b=1&v=4.3&.intl=en&x=12&y=7&z=7&r=1

                        return string.Format("http://maps{0}.yimg.com/hx/tl?v={1}&.intl={2}&x={3}&y={4}&z={5}&r=1", ((GetServerNum(row, col, 2)) + 1), VersionYahooMap, language, col, (((1 << zoom) >> 1) - 1 - row), (zoom + 1));
                    }

                case WebServiceType.YahooSatellite:
                    {
                        // http://maps3.yimg.com/ae/ximg?v=1.9&t=a&s=256&.intl=en&x=15&y=7&z=7&r=1

                        return string.Format("http://maps{0}.yimg.com/ae/ximg?v={1}&t=a&s=256&.intl={2}&x={3}&y={4}&z={5}&r=1", 3, VersionYahooSatellite, language, col, (((1 << zoom) >> 1) - 1 - row), (zoom + 1));
                    }

                case WebServiceType.YahooLabels:
                    {
                        // http://maps1.yimg.com/hx/tl?b=1&v=4.3&t=h&.intl=en&x=14&y=5&z=7&r=1

                        return string.Format("http://maps{0}.yimg.com/hx/tl?v={1}&t=h&.intl={2}&x={3}&y={4}&z={5}&r=1", 1, VersionYahooLabels, language, col, (((1 << zoom) >> 1) - 1 - row), (zoom + 1));
                    }
                #endregion

                #region -- OpenStreet --
                case WebServiceType.OpenStreetMap:
                case WebServiceType.OpenSeaMapHybrid:
                    {
                        char letter = "abc"[GetServerNum(row, col, 3)];
                        return string.Format("http://{0}.tile.openstreetmap.org/{1}/{2}/{3}.png", letter, zoom, col, row);
                    }

                case WebServiceType.OpenStreetOsm:
                    {
                        char letter = "abc"[GetServerNum(row, col, 3)];
                        return string.Format("http://{0}.tah.openstreetmap.org/Tiles/tile/{1}/{2}/{3}.png", letter, zoom, col, row);
                    }

                case WebServiceType.OpenCycleMap:
                    {
                        //http://b.tile.opencyclemap.org/cycle/13/4428/2772.png

                        char letter = "abc"[GetServerNum(row, col, 3)];
                        return string.Format("http://{0}.tile.opencyclemap.org/cycle/{1}/{2}/{3}.png", letter, zoom, col, row);
                    }

                case WebServiceType.OpenStreetMapSurfer:
                    {
                        // http://tiles1.mapsurfer.net/tms_r.ashx?x=37378&y=20826&z=16

                        return string.Format("http://tiles1.mapsurfer.net/tms_r.ashx?x={0}&y={1}&z={2}", col, row, zoom);
                    }

                case WebServiceType.OpenStreetMapSurferTerrain:
                    {
                        // http://tiles2.mapsurfer.net/tms_t.ashx?x=9346&y=5209&z=14

                        return string.Format("http://tiles2.mapsurfer.net/tms_t.ashx?x={0}&y={1}&z={2}", col, row, zoom);
                    }

                case WebServiceType.OpenSeaMapLabels:
                    {
                        // http://tiles.openseamap.org/seamark/15/17481/10495.png

                        return string.Format("http://tiles.openseamap.org/seamark/{0}/{1}/{2}.png", zoom, col, row);
                    }
                #endregion

                #region -- Bing --
                case WebServiceType.BingMap:
                    {
                        string key = TileXYToQuadKey(col, row, zoom);
                        return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/r{1}.png?g={2}&mkt={3}{4}", GetServerNum(row, col, 4), key, VersionBingMaps, language, (!string.IsNullOrEmpty(BingMapsClientToken) ? "&token=" + BingMapsClientToken : string.Empty));
                    }

                case WebServiceType.BingMap_New:
                    {
                        // http://ecn.t3.tiles.virtualearth.net/tiles/r12030012020233?g=559&mkt=en-us&lbl=l1&stl=h&shading=hill&n=z

                        string key = TileXYToQuadKey(col, row, zoom);
                        return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/r{1}.png?g={2}&mkt={3}{4}&lbl=l1&stl=h&shading=hill&n=z", GetServerNum(row, col, 4), key, VersionBingMaps, language, (!string.IsNullOrEmpty(BingMapsClientToken) ? "&token=" + BingMapsClientToken : string.Empty));
                    }

                case WebServiceType.BingSatellite:
                    {
                        string key = TileXYToQuadKey(col, row, zoom);
                        return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/a{1}.jpeg?g={2}&mkt={3}{4}", GetServerNum(row, col, 4), key, VersionBingMaps, language, (!string.IsNullOrEmpty(BingMapsClientToken) ? "&token=" + BingMapsClientToken : string.Empty));
                    }

                case WebServiceType.BingHybrid:
                    {
                        string key = TileXYToQuadKey(col, row, zoom);
                        return string.Format("http://ecn.t{0}.tiles.virtualearth.net/tiles/h{1}.jpeg?g={2}&mkt={3}{4}", GetServerNum(row, col, 4), key, VersionBingMaps, language, (!string.IsNullOrEmpty(BingMapsClientToken) ? "&token=" + BingMapsClientToken : string.Empty));
                    }
                #endregion

                #region -- ArcGIS --
                case WebServiceType.ArcGIS_StreetMap_World_2D:
                    {
                        // http://server.arcgisonline.com/ArcGIS/rest/services/ESRI_StreetMap_World_2D/MapServer/tile/0/0/0.jpg

                        return string.Format("http://server.arcgisonline.com/ArcGIS/rest/services/ESRI_StreetMap_World_2D/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                    }

                case WebServiceType.ArcGIS_Imagery_World_2D:
                    {
                        // http://server.arcgisonline.com/ArcGIS/rest/services/ESRI_Imagery_World_2D/MapServer/tile/1/0/1.jpg

                        return string.Format("http://server.arcgisonline.com/ArcGIS/rest/services/ESRI_Imagery_World_2D/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                    }

                case WebServiceType.ArcGIS_ShadedRelief_World_2D:
                    {
                        // http://server.arcgisonline.com/ArcGIS/rest/services/ESRI_ShadedRelief_World_2D/MapServer/tile/1/0/1.jpg

                        return string.Format("http://server.arcgisonline.com/ArcGIS/rest/services/ESRI_ShadedRelief_World_2D/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                    }

                case WebServiceType.ArcGIS_Topo_US_2D:
                    {
                        // http://server.arcgisonline.com/ArcGIS/rest/services/NGS_Topo_US_2D/MapServer/tile/4/3/15

                        return string.Format("http://server.arcgisonline.com/ArcGIS/rest/services/NGS_Topo_US_2D/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                    }

                case WebServiceType.ArcGIS_World_Physical_Map:
                    {
                        // http://services.arcgisonline.com/ArcGIS/rest/services/World_Physical_Map/MapServer/tile/2/0/2.jpg

                        return string.Format("http://server.arcgisonline.com/ArcGIS/rest/services/World_Physical_Map/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                    }

                case WebServiceType.ArcGIS_World_Shaded_Relief:
                    {
                        // http://services.arcgisonline.com/ArcGIS/rest/services/World_Shaded_Relief/MapServer/tile/0/0/0jpg

                        return string.Format("http://server.arcgisonline.com/ArcGIS/rest/services/World_Shaded_Relief/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                    }

                case WebServiceType.ArcGIS_World_Street_Map:
                    {
                        // http://services.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/0/0/0jpg

                        return string.Format("http://server.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                    }

                case WebServiceType.ArcGIS_World_Terrain_Base:
                    {
                        // http://services.arcgisonline.com/ArcGIS/rest/services/World_Terrain_Base/MapServer/tile/0/0/0jpg

                        return string.Format("http://server.arcgisonline.com/ArcGIS/rest/services/World_Terrain_Base/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                    }

                case WebServiceType.ArcGIS_World_Topo_Map:
                    {
                        // http://services.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/0/0/0jpg

                        return string.Format("http://server.arcgisonline.com/ArcGIS/rest/services/World_Topo_Map/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                    }

#if TESTpjbcoetzer
            case WebServiceType.ArcGIS_TestPjbcoetzer:
            {
               // http://mapping.mapit.co.za/ArcGIS/rest/services/World/MapServer/tile/Zoom/X/Y

               return string.Format("http://mapping.mapit.co.za/ArcGIS/rest/services/World/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
            }
#endif
                #endregion

                #region -- MapsLT --
                //case WebServiceType.MapsLT_OrtoFoto:
                //    {
                //        // http://www.maps.lt/ortofoto/mapslt_ortofoto_vector_512/map/_alllayers/L02/R0000001b/C00000028.jpg
                //        // http://arcgis.maps.lt/ArcGIS/rest/services/mapslt_ortofoto/MapServer/tile/0/9/13
                //        // return string.Format("http://www.maps.lt/ortofoto/mapslt_ortofoto_vector_512/map/_alllayers/L{0:00}/R{1:x8}/C{2:x8}.jpg", zoom, row, col);
                //        // http://dc1.maps.lt/cache/mapslt_ortofoto_512/map/_alllayers/L03/R0000001c/C00000029.jpg
                //        // return string.Format("http://arcgis.maps.lt/ArcGIS/rest/services/mapslt_ortofoto/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                //        // http://dc1.maps.lt/cache/mapslt_ortofoto_512/map/_alllayers/L03/R0000001d/C0000002a.jpg

                //        return string.Format("http://dc1.maps.lt/cache/mapslt_ortofoto/map/_alllayers/L{0:00}/R{1:x8}/C{2:x8}.jpg", zoom, row, col);
                //    }

                //case WebServiceType.MapsLT_OrtoFoto_2010:
                //    {
                //        return string.Format("http://dc1.maps.lt/cache/mapslt_ortofoto_2010/map/_alllayers/L{0:00}/R{1:x8}/C{2:x8}.jpg", zoom, row, col);
                //    }

                //case WebServiceType.MapsLT_Map:
                //    {
                //        // http://www.maps.lt/ortofoto/mapslt_ortofoto_vector_512/map/_alllayers/L02/R0000001b/C00000028.jpg
                //        // http://arcgis.maps.lt/ArcGIS/rest/services/mapslt_ortofoto/MapServer/tile/0/9/13
                //        // return string.Format("http://www.maps.lt/ortofoto/mapslt_ortofoto_vector_512/map/_alllayers/L{0:00}/R{1:x8}/C{2:x8}.jpg", zoom, row, col);
                //        // http://arcgis.maps.lt/ArcGIS/rest/services/mapslt/MapServer/tile/7/1162/1684.png
                //        // http://dc1.maps.lt/cache/mapslt_512/map/_alllayers/L03/R0000001b/C00000029.png

                //        // http://dc1.maps.lt/cache/mapslt/map/_alllayers/L02/R0000001c/C00000029.png
                //        return string.Format("http://dc1.maps.lt/cache/mapslt/map/_alllayers/L{0:00}/R{1:x8}/C{2:x8}.png", zoom, row, col);
                //    }

                //case WebServiceType.MapsLT_Map_2_5D:
                //    {
                //        // http://dc1.maps.lt/cache/mapslt_25d_vkkp/map/_alllayers/L01/R00007194/C0000a481.png
                //        int z = zoom;
                //        if (zoom >= 10)
                //        {
                //            z -= 10;
                //        }

                //        return string.Format("http://dc1.maps.lt/cache/mapslt_25d_vkkp/map/_alllayers/L{0:00}/R{1:x8}/C{2:x8}.png", z, row, col);
                //    }

                //case WebServiceType.MapsLT_Map_Labels:
                //    {
                //        //http://arcgis.maps.lt/ArcGIS/rest/services/mapslt_ortofoto_overlay/MapServer/tile/0/9/13
                //        //return string.Format("http://arcgis.maps.lt/ArcGIS/rest/services/mapslt_ortofoto_overlay/MapServer/tile/{0}/{1}/{2}", zoom, row, col);
                //        //http://dc1.maps.lt/cache/mapslt_ortofoto_overlay_512/map/_alllayers/L03/R0000001d/C00000029.png

                //        return string.Format("http://dc1.maps.lt/cache/mapslt_ortofoto_overlay/map/_alllayers/L{0:00}/R{1:x8}/C{2:x8}.png", zoom, row, col);
                //    }
                #endregion

                #region -- KarteLV --

                //case WebServiceType.KarteLV_Map:
                //    {
                //        // http://www.maps.lt/cache/ikartelv/map/_alllayers/L03/R00000037/C00000053.png

                //        return string.Format("http://www.maps.lt/cache/ikartelv/map/_alllayers/L{0:00}/R{1:x8}/C{2:x8}.png", zoom, row, col);
                //    }

                #endregion

                #region -- YandexMap --
                //case WebServiceType.YandexMapRu:
                //    {
                //        string server = "vec";

                //        //http://vec01.maps.yandex.ru/tiles?l=map&v=2.10.2&x=1494&y=650&z=11

                //        return string.Format("http://{0}0{1}.maps.yandex.ru/tiles?l=map&v={2}&x={3}&y={4}&z={5}", server, GetServerNum(row, col, 4) + 1, VersionYandexMap, col, row, zoom);
                //    }

                //case WebServiceType.YandexMapRuSatellite:
                //    {
                //        string server = "sat";

                //        //http://sat04.maps.yandex.ru/tiles?l=sat&v=1.18.0&x=149511&y=83513&z=18&g=Gagari

                //        return string.Format("http://{0}0{1}.maps.yandex.ru/tiles?l=sat&v={2}&x={3}&y={4}&z={5}", server, GetServerNum(row, col, 4) + 1, VersionYandexSatellite, col, row, zoom);
                //    }

                //case WebServiceType.YandexMapRuLabels:
                //    {
                //        string server = "vec";

                //        //http://vec03.maps.yandex.ru/tiles?l=skl&v=2.15.0&x=585&y=326&z=10&g=G

                //        return string.Format("http://{0}0{1}.maps.yandex.ru/tiles?l=skl&v={2}&x={3}&y={4}&z={5}", server, GetServerNum(row, col, 4) + 1, VersionYandexMap, col, row, zoom);
                //    }

                #endregion

                #region -- WMS demo --
                //case WebServiceType.MapBenderWMS:
                //    {
                //        var px1 = ProjectionForWMS.FromTileXYToPixel(T);
                //        var px2 = px1;

                //        px1.Offset(0, ProjectionForWMS.TileSize.Height);
                //        PointLatLng p1 = ProjectionForWMS.FromPixelToLatLng(px1, zoom);

                //        px2.Offset(ProjectionForWMS.TileSize.Width, 0);
                //        PointLatLng p2 = ProjectionForWMS.FromPixelToLatLng(px2, zoom);

                //        var ret = string.Format(CultureInfo.InvariantCulture, "http://mapbender.wheregroup.com/cgi-bin/mapserv?map=/data/umn/osm/osm_basic.map&VERSION=1.1.1&REQUEST=GetMap&SERVICE=WMS&LAYERS=OSM_Basic&styles=&bbox={0},{1},{2},{3}&width={4}&height={5}&srs=EPSG:4326&format=image/png", p1.Lng, p1.Lat, p2.Lng, p2.Lat, ProjectionForWMS.TileSize.Width, ProjectionForWMS.TileSize.Height);

                //        return ret;
                //    }
                #endregion

                #region -- NearMap --
                //case WebServiceType.NearMap:
                //    {
                //        // http://web1.nearmap.com/maps/hl=en&x=18681&y=10415&z=15&nml=Map_&nmg=1&s=kY8lZssipLIJ7c5

                //        return string.Format("http://web{0}.nearmap.com/maps/hl=en&x={1}&y={2}&z={3}&nml=Map_&nmg=1", GetServerNum(row, col, 3), col, row, zoom);
                //    }

                //case WebServiceType.NearMapSatellite:
                //    {
                //        // http://web2.nearmap.com/maps/hl=en&x=34&y=20&z=6&nml=Vert&s=2NYYKGF

                //        return string.Format("http://web{0}.nearmap.com/maps/hl=en&x={1}&y={2}&z={3}&nml=Vert", GetServerNum(row, col, 3), col, row, zoom);
                //    }

                //case WebServiceType.NearMapLabels:
                //    {
                //        //http://web1.nearmap.com/maps/hl=en&x=37&y=19&z=6&nml=MapT&nmg=1&s=2KbhmZZ             

                //        return string.Format("http://web{0}.nearmap.com/maps/hl=en&x={1}&y={2}&z={3}&nml=MapT&nmg=1", GetServerNum(row, col, 3), col, row, zoom);
                //    }

                #endregion

            }

            return "";
        }

        internal void GetSecGoogleWords(int row, int col, out string sec1, out string sec2)
        {
            string SecGoogleWord = "Galileo";

            sec1 = ""; // after &x=...
            //sec2 = ""; // after &zoom=...
            int seclen = ((col * 3) + row) % 8;
            sec2 = SecGoogleWord.Substring(0, seclen);
            if (row >= 10000 && row < 100000)
            {
                sec1 = "&s=";
            }
        }

        internal int GetServerNum(int row, int col, int max)
        {
            //return (T.col + 2 * T.row) % max;
            return (col + 2 * row) % max;
        }

        internal string TileXYToQuadKey(int col, int row, int levelOfDetail)
        {
            StringBuilder quadKey = new StringBuilder();
            for (int i = levelOfDetail; i > 0; i--)
            {
                char digit = '0';
                int mask = 1 << (i - 1);
                if ((col & mask) != 0)
                {
                    digit++;
                }
                if ((row & mask) != 0)
                {
                    digit++;
                    digit++;
                }
                quadKey.Append(digit);
            }
            return quadKey.ToString();
        }

        private TileInfo GetTile(int Zoom, Coordinate C)
        {
            TileInfo T = new TileInfo();

            T.col = (int)((double)((C.X - _Extent.MinX) / _Extent.Width) * (double)NumTileX[Zoom]);
            if (T.col < 0) T.col = 0;
            if (T.col > NumTileX[Zoom] - 1) T.col = NumTileX[Zoom] - 1;

            if (InvertedAxis == true)
            {
                T.row = (int)((double)((_Extent.MaxY - C.Y) / _Extent.Height) * (double)NumTileY[Zoom]);
            }
            else
            {
                T.row = (int)((double)((C.Y - _Extent.MinY) / _Extent.Height) * (double)NumTileY[Zoom]);
            }

            if (T.row < 0) T.row = 0;
            if (T.row > NumTileY[Zoom] - 1) T.row = NumTileY[Zoom] - 1;

            double cx = _Extent.Width / (double)NumTileX[Zoom];
            double cy = _Extent.Height / (double)NumTileY[Zoom];

            if (InvertedAxis == false)
            {
                T.Extent = new Extent(_Extent.MinX + (T.col * cx),
                                        _Extent.MinY + (T.row * cy),
                                        _Extent.MinX + (T.col * cx) + cx,
                                        _Extent.MinY + (T.row * cy) + cy);
            }
            else
            {
                T.Extent = new Extent(_Extent.MinX + (T.col * cx),
                                        _Extent.MaxY - (T.row * cy) - cy,
                                        _Extent.MinX + (T.col * cx) + cx,
                                        _Extent.MaxY - (T.row * cy));
            }

            //char letter = "abc"[GetServerNum(T, 3)];
            //char letter = 'a';
            //T.url = string.Format("http://{0}.tile.openstreetmap.org/{1}/{2}/{3}.png", letter, Zoom, T.col, T.row);

            T.url = GetUrl(T.row, T.col, Zoom, Type);

            return T;
        }

        public TileInfoSet[] GetTile(Extent extent, Size size)
        {
            int zoom = GetZoomLevel(extent, size);

            bool labels = false;
            WebServiceType LabelType = WebServiceType.None;
            WebServiceType BaseType = WebServiceType.None;
            int numSet = 1;


            switch (Type)
            {

                case (WebServiceType.GoogleHybrid):
                    {
                        labels = true;
                        BaseType = WebServiceType.GoogleSatellite;
                        LabelType = WebServiceType.GoogleLabels;
                    }
                    break;
                case (WebServiceType.GoogleHybridChina):
                    {
                        labels = true;
                        BaseType = WebServiceType.GoogleSatellite;
                        LabelType = WebServiceType.GoogleLabelsChina;
                    }
                    break;
                case (WebServiceType.GoogleHybridKorea):
                    {
                        labels = true;
                        BaseType = WebServiceType.GoogleSatellite;
                        LabelType = WebServiceType.GoogleLabelsKorea;
                    }
                    break;
                case (WebServiceType.YahooHybrid):
                    {
                        labels = true;
                        BaseType = WebServiceType.YahooSatellite;
                        LabelType = WebServiceType.YahooLabels;
                    }
                    break;
                case (WebServiceType.OpenSeaMapHybrid):
                    {
                        labels = true;
                        BaseType = WebServiceType.OpenStreetMap;
                        LabelType = WebServiceType.OpenSeaMapLabels;
                    }
                    break;
            }

            if (labels == true)
            {
                numSet = 2;
            }

            TileInfoSet[] Ts = new TileInfoSet[numSet];

            int set;

            for (set = 0; set < numSet; set++)
            {
                Ts[set] = new TileInfoSet();
            }

            Coordinate CUpperLeft, CBottomRight;

            if (InvertedAxis == true)
            {
                CUpperLeft = new Coordinate(extent.MinX, extent.MaxY);
                CBottomRight = new Coordinate(extent.MaxX, extent.MinY);
            }
            else
            {
                CUpperLeft = new Coordinate(extent.MinX, extent.MinY);
                CBottomRight = new Coordinate(extent.MaxX, extent.MaxY);
            }


            TileInfo UL = GetTile(zoom, CUpperLeft);
            TileInfo BR = GetTile(zoom, CBottomRight);


            for (set = 0; set < numSet; set++)
            {
                Ts[set].rows = BR.row - UL.row + 1;
                Ts[set].cols = BR.col - UL.col + 1;

                Ts[set].Extent = new Extent(UL.Extent.MinX, Math.Min(UL.Extent.MinY, BR.Extent.MinY), BR.Extent.MaxX, Math.Max(UL.Extent.MaxY, BR.Extent.MaxY));

                Ts[set].Tiles = new TileInfo[Ts[set].rows][];
            }

            double cx = _Extent.Width / (double)NumTileX[zoom];
            double cy = _Extent.Height / (double)NumTileY[zoom];

            for (int r = 0; r < Ts[0].rows; r++)
            {
                for (set = 0; set < numSet; set++)
                {
                    Ts[set].Tiles[r] = new TileInfo[Ts[set].cols];
                }

                for (int c = 0; c < Ts[0].cols; c++)
                {

                    for (set = 0; set < numSet; set++)
                    {
                        Ts[set].Tiles[r][c] = new TileInfo();

                        Ts[set].Tiles[r][c].row = r;
                        Ts[set].Tiles[r][c].col = c;


                        if (InvertedAxis == false)
                        {
                            Ts[set].Tiles[r][c].Extent = new Extent(_Extent.MinX + (Ts[set].Tiles[r][c].col * cx),
                                                    _Extent.MinY + (Ts[set].Tiles[r][c].row * cy),
                                                    _Extent.MinX + (Ts[set].Tiles[r][c].col * cx) + cx,
                                                    _Extent.MinY + (Ts[set].Tiles[r][c].row * cy) + cy);
                        }
                        else
                        {
                            Ts[set].Tiles[r][c].Extent = new Extent(_Extent.MinX + (Ts[set].Tiles[r][c].col * cx),
                                                    _Extent.MaxY - (Ts[set].Tiles[r][c].row * cy),
                                                    _Extent.MinX + (Ts[set].Tiles[r][c].col * cx) + cx,
                                                    _Extent.MaxY - (Ts[set].Tiles[r][c].row * cy) + cy);
                        }
                    }

                    if (labels == false)
                    {
                        Ts[0].Tiles[r][c].url = GetUrl(r + UL.row, c + UL.col, zoom, Type);
                    }
                    else
                    {
                        Ts[0].Tiles[r][c].url = GetUrl(r + UL.row, c + UL.col, zoom, BaseType);
                        Ts[1].Tiles[r][c].url = GetUrl(r + UL.row, c + UL.col, zoom, LabelType);
                    }

                }
            }

            return Ts;

        }

        public string GetHTML(ref GDIMap m, Size size, string DivID)
        {

            TileInfoSet[] Ts = GetTile(m.ViewExtents, size);

            string htm = "";

            int numSet = Ts.GetLength(0);

            for (int set = 0; set < numSet; set++)
            {

                if (set == 0)
                {
                    TilesExtent = Ts[set].Extent;
                }
                else
                {
                    TilesExtent.ExpandToInclude(Ts[set].Extent);
                }
            }




            Rectangle Rect = m.ProjToPixel(TilesExtent);

            int w = (int)((double)(Rect.Width + 1) / (double)Ts[0].cols);
            int h = (int)((double)(Rect.Height + 1) / (double)Ts[0].rows);

            htm += "<div id=\"Back_" + DivID + "\" style=\"position:absolute; left:" + Rect.Left.ToString() + "px; top:" + Rect.Top.ToString() + "px; width:" + Rect.Width.ToString() + "px; height:" + Rect.Height.ToString() + "px; \">";

            for (int r = 0; r < Ts[0].rows; r++)
            {
                for (int c = 0; c < Ts[0].cols; c++)
                {
                    int l = c * w;
                    int t = r * h;

                    for (int set = 0; set < numSet; set++)
                    {
                        htm += "<img alt=\"\" style=\"position:absolute; left: " + l.ToString() + "px; top:" + t.ToString() + "px; width:" + w.ToString() + "px; height:" + h.ToString() + "px; \" src=\"" + Ts[set].Tiles[r][c].url + "\" />";
                    }
                }

            }

            htm += "</div>";

            return htm;

            //Extent WmtEx;

            //{

            //    double[] xy = new double[4];

            //    xy[0] = m.ViewExtents.MinX;
            //    xy[1] = m.ViewExtents.MinY;
            //    xy[2] = m.ViewExtents.MaxX;
            //    xy[3] = m.ViewExtents.MaxY;

            //    double[] z = { };


            //    Projections.Reproject.ReprojectPoints(xy, z, m.Projection, Projection, 0, 2);

            //    WmtEx = new Extent(xy);

            //}


            //TileInfoSet[] Ts = GetTile(WmtEx, size);

            //string htm = "";

            //int numSet = Ts.GetLength(0);

            //for (int set = 0; set < numSet; set++)
            //{

            //    if (set == 0)
            //    {
            //        TilesExtent = Ts[set].Extent;
            //    }
            //    else
            //    {
            //        TilesExtent.ExpandToInclude(Ts[set].Extent);
            //    }
            //}

            //Extent MapEx;

            //{

            //    double[] xy = new double[4];

            //    xy[0] = WmtEx.MinX;
            //    xy[1] = WmtEx.MinY;
            //    xy[2] = WmtEx.MaxX;
            //    xy[3] = WmtEx.MaxY;

            //    double[] z = { };


            //    Projections.Reproject.ReprojectPoints(xy, z, m.Projection, Projection, 0, 2);

            //    MapEx = new Extent(xy);

            //}



            //Rectangle Rect = m.ProjToPixel(MapEx);

            //int w = (int)((double)(Rect.Width + 1) / (double)Ts[0].cols);
            //int h = (int)((double)(Rect.Height + 1) / (double)Ts[0].rows);

            //htm += "<div id=\"Back_" + DivID + "\" style=\"position:absolute; left:" + Rect.Left.ToString() + "px; top:" + Rect.Top.ToString() + "px; width:" + Rect.Width.ToString() + "px; height:" + Rect.Height.ToString() + "px; \">";

            //for (int r = 0; r < Ts[0].rows; r++)
            //{
            //    for (int c = 0; c < Ts[0].cols; c++)
            //    {
            //        int l = c * w;
            //        int t = r * h;

            //        for (int set = 0; set < numSet; set++)
            //        {
            //            htm += "<img alt=\"\" style=\"position:absolute; left: " + l.ToString() + "px; top:" + t.ToString() + "px; width:" + w.ToString() + "px; height:" + h.ToString() + "px; \" src=\"" + Ts[set].Tiles[r][c].url + "\" />";
            //        }
            //    }

            //}

            //htm += "</div>";

            //return htm;
        }

        public void List(WebLegend tree)
        {

            switch (Type)
            {

                #region -- Google --

                case (WebServiceType.GoogleMap):
                    {
                        Name = "Google Map";
                    }
                    break;
                case (WebServiceType.GoogleSatellite):
                    {
                        Name = "Google Satellite";
                    }
                    break;
                case (WebServiceType.GoogleLabels):
                    {
                        Name = "Google Labels";
                    }
                    break;
                case (WebServiceType.GoogleTerrain):
                    {
                        Name = "Google Terrain";
                    }
                    break;
                case (WebServiceType.GoogleHybrid):
                    {
                        Name = "Google Hybrid";
                    }
                    break;
                case (WebServiceType.GoogleMapChina):
                    {
                        Name = "Google Map";
                    }
                    break;
                case (WebServiceType.GoogleSatelliteChina):
                    {
                        Name = "Google Satellite";
                    }
                    break;
                case (WebServiceType.GoogleLabelsChina):
                    {
                        Name = "Google Labels";
                    }
                    break;
                case (WebServiceType.GoogleTerrainChina):
                    {
                        Name = "Google Terrain";
                    }
                    break;
                case (WebServiceType.GoogleHybridChina):
                    {
                        Name = "Google Hybrid";
                    }
                    break;
                #endregion

                #region -- Yahoo --
                case (WebServiceType.YahooMap):
                    {
                        Name = "Yahoo Map";
                    }
                    break;
                case (WebServiceType.YahooSatellite):
                    {
                        Name = "Yahoo Satellite";
                    }
                    break;
                case (WebServiceType.YahooLabels):
                    {
                        Name = "Yahoo Labels";
                    }
                    break;
                case (WebServiceType.YahooHybrid):
                    {
                        Name = "Yahoo Hybrid";
                    }
                    break;
                #endregion

                #region -- Bing --
                case (WebServiceType.BingMap):
                    {
                        Name = "Bing Map";
                    }
                    break;
                case (WebServiceType.BingMap_New):
                     {
                        Name = "Bing Map New";
                    }
                    break;
                case (WebServiceType.BingSatellite):
                     {
                         Name = "Bing Satellite";
                    }
                    break;
                case (WebServiceType.BingHybrid):
                    {
                        Name = "Bing Hybrid";
                    }
                    break;
                #endregion

                #region -- ArcGIS --
                case (WebServiceType.ArcGIS_StreetMap_World_2D):
                case (WebServiceType.ArcGIS_Imagery_World_2D):
                case (WebServiceType.ArcGIS_ShadedRelief_World_2D):
                case (WebServiceType.ArcGIS_Topo_US_2D):
                case (WebServiceType.ArcGIS_World_Physical_Map):
                case (WebServiceType.ArcGIS_World_Shaded_Relief):
                case (WebServiceType.ArcGIS_World_Street_Map):
                case (WebServiceType.ArcGIS_World_Terrain_Base):
                case (WebServiceType.ArcGIS_World_Topo_Map):
                    {
                        Name = "ArcGIS Map";
                    }
                    break;

                #endregion

                #region -- OpenStreet --
                case (WebServiceType.OpenStreetMap):
                case (WebServiceType.OpenStreetOsm):
                case (WebServiceType.OpenStreetMapSurfer):
                case (WebServiceType.OpenStreetMapSurferTerrain):
                case (WebServiceType.OpenSeaMapLabels):
                case (WebServiceType.OpenSeaMapHybrid):
                case (WebServiceType.OpenCycleMap):
                    {
                        Name = "Openstreetmap";
                    }
                    break;
                #endregion

            }
            TreeNode tn = new TreeNode(Name);
            tn.ShowCheckBox = true;
            tn.Checked = Visible;
            tree.Nodes.Add(tn);
        }

        public Extent Extent()
        {
            return _Extent;
        }

        #region Versions

        private string VersionGoogleMap = "m@142";
        private string VersionGoogleSatellite = "79";
        private string VersionGoogleLabels = "h@142";
        private string VersionGoogleTerrain = "t@126,r@142";

        // Google (China) version strings
        private string VersionGoogleMapChina = "m@142";
        private string VersionGoogleSatelliteChina = "s@76";
        private string VersionGoogleLabelsChina = "h@142";
        private string VersionGoogleTerrainChina = "t@126,r@142";

        // Google (Korea) version strings
        private string VersionGoogleMapKorea = "kr1.12";
        private string VersionGoogleSatelliteKorea = "71";
        private string VersionGoogleLabelsKorea = "kr1t.12";



        /// <summary>
        /// Google Maps API generated using http://greatmaps.codeplex.com/
        /// from http://code.google.com/intl/en-us/apis/maps/signup.html
        /// </summary>
        //string GoogleMapsAPIKey = @"ABQIAAAAWaQgWiEBF3lW97ifKnAczhRAzBk5Igf8Z5n2W3hNnMT0j2TikxTLtVIGU7hCLLHMAuAMt-BO5UrEWA";

        // Yahoo version strings
        private string VersionYahooMap = "4.3";
        private string VersionYahooSatellite = "1.9";
        private string VersionYahooLabels = "4.3";

        // BingMaps
        private string VersionBingMaps = "631";

        #endregion
        #region VersionCorrection
        /// <summary>
        /// true if google versions was corrected
        /// </summary>
        private bool IsCorrectedGoogleVersions = false;

        /// <summary>
        /// true if google versions was corrected
        /// </summary>
        private bool IsCorrectedBingVersions = false;



        /// <summary>
        /// timeout for map connections
        /// </summary>
        public int Timeout = 30 * 1000;

        /// <summary>
        /// Gets or sets the value of the User-agent HTTP header.
        /// </summary>
        public string UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.9.1.7) Gecko/20091221 Firefox/3.5.7";

        internal void TryCorrectGoogleVersions()
        {
            if (!IsCorrectedGoogleVersions)
            {
                string url = @"http://maps.google.com";
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                    if (Proxy != null)
                    {
                        request.Proxy = Proxy;
#if !PocketPC
                        request.PreAuthenticate = true;
#endif
                    }

                    request.UserAgent = UserAgent;
                    request.Timeout = Timeout;
                    request.ReadWriteTimeout = Timeout * 6;

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader read = new StreamReader(responseStream))
                            {
                                string html = read.ReadToEnd();

                                Regex reg = new Regex("\"*http://mt0.google.com/vt/lyrs=m@(\\d*)", RegexOptions.IgnoreCase);
                                Match mat = reg.Match(html);
                                if (mat.Success)
                                {
                                    GroupCollection gc = mat.Groups;
                                    int count = gc.Count;
                                    if (count > 0)
                                    {
                                        VersionGoogleMap = string.Format("m@{0}", gc[1].Value);
                                        VersionGoogleMapChina = VersionGoogleMap;
                                        Debug.WriteLine("TryCorrectGoogleVersions, VersionGoogleMap: " + VersionGoogleMap);
                                    }
                                }

                                reg = new Regex("\"*http://mt0.google.com/vt/lyrs=h@(\\d*)", RegexOptions.IgnoreCase);
                                mat = reg.Match(html);
                                if (mat.Success)
                                {
                                    GroupCollection gc = mat.Groups;
                                    int count = gc.Count;
                                    if (count > 0)
                                    {
                                        VersionGoogleLabels = string.Format("h@{0}", gc[1].Value);
                                        VersionGoogleLabelsChina = VersionGoogleLabels;
                                        Debug.WriteLine("TryCorrectGoogleVersions, VersionGoogleLabels: " + VersionGoogleLabels);
                                    }
                                }

                                reg = new Regex("\"*http://khm0.google.com/kh/v=(\\d*)", RegexOptions.IgnoreCase);
                                mat = reg.Match(html);
                                if (mat.Success)
                                {
                                    GroupCollection gc = mat.Groups;
                                    int count = gc.Count;
                                    if (count > 0)
                                    {
                                        VersionGoogleSatellite = gc[1].Value;
                                        VersionGoogleSatelliteKorea = VersionGoogleSatellite;
                                        VersionGoogleSatelliteChina = "s@" + VersionGoogleSatellite;
                                        Debug.WriteLine("TryCorrectGoogleVersions, VersionGoogleSatellite: " + VersionGoogleSatellite);
                                    }
                                }

                                reg = new Regex("\"*http://mt0.google.com/vt/lyrs=t@(\\d*),r@(\\d*)", RegexOptions.IgnoreCase);
                                mat = reg.Match(html);
                                if (mat.Success)
                                {
                                    GroupCollection gc = mat.Groups;
                                    int count = gc.Count;
                                    if (count > 1)
                                    {
                                        VersionGoogleTerrain = string.Format("t@{0},r@{1}", gc[1].Value, gc[2].Value);
                                        VersionGoogleTerrainChina = VersionGoogleTerrain;
                                        Debug.WriteLine("TryCorrectGoogleVersions, VersionGoogleTerrain: " + VersionGoogleTerrain);
                                    }
                                }
                            }
                        }
                    }
                    IsCorrectedGoogleVersions = true; // try it only once
                }
                catch (Exception ex)
                {
                    IsCorrectedGoogleVersions = false;
                    Debug.WriteLine("TryCorrectGoogleVersions failed: " + ex.ToString());
                }
            }
        }


        /// <summary>
        /// try to correct google versions
        /// </summary>    
        internal void TryCorrectBingVersions()
        {
            if (!IsCorrectedBingVersions)
            {
                string url = @"http://www.bing.com/maps";
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    if (Proxy != null)
                    {
                        request.Proxy = Proxy;
#if !PocketPC
                        request.PreAuthenticate = true;
#endif
                    }

                    request.UserAgent = UserAgent;
                    request.Timeout = Timeout;
                    request.ReadWriteTimeout = Timeout * 6;

                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            using (StreamReader read = new StreamReader(responseStream))
                            {
                                string html = read.ReadToEnd();

                                Regex reg = new Regex("http://ecn.t(\\d*).tiles.virtualearth.net/tiles/r(\\d*)[?*]g=(\\d*)", RegexOptions.IgnoreCase);
                                Match mat = reg.Match(html);
                                if (mat.Success)
                                {
                                    GroupCollection gc = mat.Groups;
                                    int count = gc.Count;
                                    if (count > 2)
                                    {
                                        VersionBingMaps = gc[3].Value;
                                        Debug.WriteLine("TryCorrectBingVersions, VersionBingMaps: " + VersionBingMaps);
                                    }
                                }

                            }
                        }
                    }
                    IsCorrectedBingVersions = true; // try it only once
                }
                catch (Exception ex)
                {
                    IsCorrectedBingVersions = false;
                    Debug.WriteLine("TryCorrectBingVersions failed: " + ex.ToString());
                }
            }
        }
        #endregion
    }
}