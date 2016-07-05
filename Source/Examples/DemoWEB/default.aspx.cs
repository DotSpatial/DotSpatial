using System;
using DotSpatial.MapWebClient;
using DotSpatial.Projections;
using DotSpatial.Data;
using DotSpatial.Controls;
using DotSpatial.Symbology;
using System.Drawing;

namespace DemoWEB
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateMap();

                WebToolBar1.WebMapID = "WebMap1";
                WebToolBar1.CreateStandardButtons();

                WebLegend1.WebMapID = "WebMap1";
            }
        }

        private void CreateMap()
        {

            string basePath = Server.MapPath(@"~\Shape");
            WebMap1.Projection = KnownCoordinateSystems.Projected.World.WebMercator;
            WebMap1.MapViewExtents = new Extent(-20037508.342789, -20037508.342789, 20037508.342789, 20037508.342789);


            WebMapClient client = new WebMapClient();


            WMTClient wmt1 = new WMTClient();
            wmt1.Create(WebServiceType.BingHybrid);


            string WMSServerWMS0 = "http://maps.ngdc.noaa.gov/soap/web_mercator/marine_geology/MapServer/WMSServer";
            WMSClient wms0 = new WMSClient();

            wms0.ReadCapabilities(WMSServerWMS0);
            wms0.CRS = "EPSG:3857";
            wms0.Projection = KnownCoordinateSystems.Projected.World.WebMercator;


            string WMSServerWMS1 = "http://maps.ngdc.noaa.gov/soap/web_mercator/graticule/MapServer/WMSServer";

            WMSClient wms1 = new WMSClient();

            wms1.ReadCapabilities(WMSServerWMS1);
            wms1.CRS = "EPSG:3857";
            wms1.Projection = KnownCoordinateSystems.Projected.World.WebMercator;


            client.AddService(wmt1);
            client.AddService(wms0);
            client.AddService(wms1);

            WebMap1.Back = client;


            IMapFeatureLayer countriesLayer = (IMapFeatureLayer)WebMap1.AddLayer(basePath + @"\10m_admin_0_countries.shp");
            PolygonSymbolizer symbCountries = new PolygonSymbolizer(Color.FromArgb(0, 191, 0));
            symbCountries.SetFillColor(Color.Transparent);
            symbCountries.OutlineSymbolizer = new LineSymbolizer(Color.Magenta, 1);
            countriesLayer.Symbolizer = symbCountries;


            IMapFeatureLayer graticules30Layer = (IMapFeatureLayer)WebMap1.AddLayer(basePath + @"\10m_graticules_30.shp");
            LineSymbolizer symbGratitules30 = new LineSymbolizer(Color.Red, 1);
            graticules30Layer.Symbolizer = symbGratitules30;

            graticules30Layer.IsVisible = false;
        }

    }
}


