using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotSpatial.MapWebClient;
using DotSpatial.Projections;
using DotSpatial.Data;
using DotSpatial.Controls;
using DotSpatial.Symbology;
using System.Drawing;
using System.Net;

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

            String BasePath = Server.MapPath(@"~\Shape");
            WebMap1.Projection = KnownCoordinateSystems.Projected.World.WebMercator;
            WebMap1.MapViewExtents = new Extent(-20037508.342789, -20037508.342789, 20037508.342789, 20037508.342789);


            WebMapClient client = new WebMapClient();


            WMTClient WMT1 = new WMTClient();
            WMT1.Create(WebServiceType.BingHybrid);
            
            
            string WMSServerWMS0 = "http://maps.ngdc.noaa.gov/soap/web_mercator/nos_hydro/MapServer/WMSServer";
            WMSClient WMS0 = new WMSClient();

            WMS0.ReadCapabilities(WMSServerWMS0);
            WMS0.CRS = "EPSG:3857";
            WMS0.Projection = KnownCoordinateSystems.Projected.World.WebMercator;


            string WMSServerWMS1 = "http://maps.ngdc.noaa.gov/soap/web_mercator/graticule/MapServer/WMSServer";

            WMSClient WMS1 = new WMSClient();

            WMS1.ReadCapabilities(WMSServerWMS1);
            WMS1.CRS = "EPSG:3857";
            WMS1.Projection = KnownCoordinateSystems.Projected.World.WebMercator;


            client.AddService(WMT1);
            client.AddService(WMS0);
            client.AddService(WMS1);

            WebMap1.Back = client;


            IMapFeatureLayer CountriesLayer = (IMapFeatureLayer)WebMap1.AddLayer(BasePath + @"\10m_admin_0_countries.shp");
            PolygonSymbolizer SymbCountries = new PolygonSymbolizer(Color.FromArgb(0, 191, 0));
            SymbCountries.SetFillColor(Color.Transparent);
            SymbCountries.OutlineSymbolizer = new LineSymbolizer(Color.Magenta, 1);
            CountriesLayer.Symbolizer = SymbCountries;


            IMapFeatureLayer Graticules30Layer = (IMapFeatureLayer)WebMap1.AddLayer(BasePath + @"\10m_graticules_30.shp");
            LineSymbolizer SymbGratitules30 = new LineSymbolizer(Color.Red, 1);
            Graticules30Layer.Symbolizer = SymbGratitules30;

            Graticules30Layer.IsVisible = false;



        }

    }
}


