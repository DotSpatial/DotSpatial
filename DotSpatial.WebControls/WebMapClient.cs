using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotSpatial.Projections;
using DotSpatial.Data;
using System.Drawing;
using System.Net;
using DotSpatial.Controls;
using System.Globalization;
using DotSpatial.WebControls;

namespace DotSpatial.MapWebClient
{
    /// <summary>
    /// WebServiceType
    /// </summary>
    public enum WebServiceType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        ///// <summary>
        ///// WMS
        ///// </summary>
        //WMS = -1,

        /// <summary>
        /// GoogleMap
        /// </summary>
        GoogleMap = 1,
        /// <summary>
        /// GoogleSatellite
        /// </summary>
        GoogleSatellite = 4,
        /// <summary>
        /// GoogleLabels
        /// </summary>
        GoogleLabels = 8,
        /// <summary>
        /// GoogleTerrain
        /// </summary>
        GoogleTerrain = 16,
        /// <summary>
        /// GoogleHybrid
        /// </summary>
        GoogleHybrid = 20,

        /// <summary>
        /// GoogleMapChina
        /// </summary>
        GoogleMapChina = 22,
        /// <summary>
        /// GoogleSatelliteChina
        /// </summary>
        GoogleSatelliteChina = 24,
        /// <summary>
        /// GoogleLabelsChina
        /// </summary>
        GoogleLabelsChina = 26,
        /// <summary>
        /// GoogleTerrainChina
        /// </summary>
        GoogleTerrainChina = 28,
        /// <summary>
        /// GoogleHybridChina
        /// </summary>
        GoogleHybridChina = 29,

        /// <summary>
        /// OpenStreetMap
        /// </summary>
        OpenStreetMap = 32,
        /// <summary>
        /// OpenStreetOsm
        /// </summary>
        OpenStreetOsm = 33,
        /// <summary>
        /// OpenStreetMapSurfer
        /// </summary>
        OpenStreetMapSurfer = 34,
        /// <summary>
        /// OpenStreetMapSurferTerrain
        /// </summary>
        OpenStreetMapSurferTerrain = 35,
        /// <summary>
        /// OpenSeaMapLabels
        /// </summary>
        OpenSeaMapLabels = 36,
        /// <summary>
        /// OpenSeaMapHybrid
        /// </summary>
        OpenSeaMapHybrid = 37,
        /// <summary>
        /// OpenCycleMap
        /// </summary>
        OpenCycleMap = 38,

        /// <summary>
        /// YahooMap
        /// </summary>
        YahooMap = 64,
        /// <summary>
        /// YahooSatellite
        /// </summary>
        YahooSatellite = 128,
        /// <summary>
        /// YahooLabels
        /// </summary>
        YahooLabels = 256,
        /// <summary>
        /// YahooHybrid
        /// </summary>
        YahooHybrid = 333,

        /// <summary>
        /// BingMap
        /// </summary>
        BingMap = 444,
        /// <summary>
        /// BingMap_New
        /// </summary>
        BingMap_New = 455,
        /// <summary>
        /// BingSatellite
        /// </summary>
        BingSatellite = 555,
        /// <summary>
        /// BingHybrid
        /// </summary>
        BingHybrid = 666,

        /// <summary>
        /// ArcGIS_StreetMap_World_2D
        /// </summary>
        ArcGIS_StreetMap_World_2D = 777,
        /// <summary>
        /// ArcGIS_Imagery_World_2D
        /// </summary>
        ArcGIS_Imagery_World_2D = 788,
        /// <summary>
        /// ArcGIS_ShadedRelief_World_2D
        /// </summary>
        ArcGIS_ShadedRelief_World_2D = 799,
        /// <summary>
        /// ArcGIS_Topo_US_2D
        /// </summary>
        ArcGIS_Topo_US_2D = 811,

        /// <summary>
        /// ArcGIS_World_Physical_Map
        /// </summary>
        ArcGIS_World_Physical_Map = 822,
        /// <summary>
        /// ArcGIS_World_Shaded_Relief
        /// </summary>
        ArcGIS_World_Shaded_Relief = 833,
        /// <summary>
        /// ArcGIS_World_Street_Map
        /// </summary>
        ArcGIS_World_Street_Map = 844,
        /// <summary>
        /// ArcGIS_World_Terrain_Base
        /// </summary>
        ArcGIS_World_Terrain_Base = 855,
        /// <summary>
        /// ArcGIS_World_Topo_Map
        /// </summary>
        ArcGIS_World_Topo_Map = 866,

        /// <summary>
        /// GoogleMapKorea
        /// </summary>
        GoogleMapKorea = 4001,
        /// <summary>
        /// GoogleSatelliteKorea
        /// </summary>
        GoogleSatelliteKorea = 4002,
        /// <summary>
        /// GoogleLabelsKorea
        /// </summary>
        GoogleLabelsKorea = 4003,
        /// <summary>
        /// GoogleHybridKorea
        /// </summary>
        GoogleHybridKorea = 4005,

    }

    /// <summary>
    /// WebServiceType
    /// </summary>
    [Serializable()]
    public class WebMapClient
    {

 
        public WebProxy Proxy { get; set; }

        public List<Object> Servicies = new List<object>();

        public void AddService(IWebClient Service)
        {
            if (Service.GetType() == typeof(WMSClient) || Service.GetType() == typeof(WMTClient))
            {
                if (Proxy != null)
                {
                    Service.Proxy = Proxy;
                }

                Servicies.Add(Service);
            }
            else
            {
                throw new Exception("Invalid service type");
            }
        }

        //public void AddService(WebServiceType ServiceType, string WMSserver = "", string XMLCustomFile = "")
        //{
            
        //    switch (ServiceType)
        //    {
        //        case (WebServiceType.WMS):
        //            {
        //                WMSCapabilities WmsCapabilities = new WMSCapabilities();
        //                WmsCapabilities.ReadCapabilities(WMSserver, XMLCustomFile, Proxy);

        //                Servicies.Add(WmsCapabilities);
        //            }
        //            break;
                
        //        default:
        //            {
        //                WMTClient WmtClient = new WMTClient();
        //                WmtClient.Create(ServiceType, Proxy);

        //                Servicies.Add(WmtClient);
        //            }
        //            break;
        //    }

        //}

        public string GetHTML(ref GDIMap m, Size size, string ClientID)
        {
            string htm="";

            Rectangle Rect = m.ProjToPixel(m.ViewExtents);
 
            htm += "<div id=\"Back_" + ClientID + "\" style=\"position:absolute; left:" + Rect.Left.ToString() + "px; top:" + Rect.Top.ToString() + "px; width:" + Rect.Width.ToString() + "px; height:" + Rect.Height.ToString() + "px; z-index:1; \">";

            for (int i = 0; i < Servicies.Count(); i++)
            {

                if (Servicies.ElementAt(i).GetType() == typeof(WMSClient))
                {
                    WMSClient WmsClient = (WMSClient)Servicies.ElementAt(i);

                    htm += WmsClient.GetHTML(ref m, size, ClientID + "_" + i.ToString());
                }
                else
                {           
                    WMTClient WmtClient = (WMTClient)Servicies.ElementAt(i);

                    if (WmtClient.Visible)
                    {
                        htm += WmtClient.GetHTML(ref m, size, ClientID + "_" + i.ToString());
                    }
                }
            }
            htm += "</div>";


            return htm;
        }

       
        public Extent GetMaxExtent()
        {

            Extent extent= new Extent();
            Extent x = new Extent();

            for (int i = 0; i < Servicies.Count(); i++)
            {
                if (Servicies.ElementAt(i).GetType() == typeof(WMSClient))
                {
                    WMSClient WmsClient = (WMSClient)Servicies.ElementAt(i);
                    x=WmsClient.GetMaxSize();
                }
                else
                {
                    WMTClient WmtClient = (WMTClient)Servicies.ElementAt(i);
                    x=WmtClient.Extent();

                }

                if (i == 0)
                    extent = x;
                else
                    extent.ExpandToInclude(x);

            }

            return extent;
        }

        public void List(WebLegend tree)
        {
            for (int i = Servicies.Count() - 1; i>=0; i--)
            {
                if (Servicies.ElementAt(i).GetType() == typeof(WMSClient))
                {
                    WMSClient WmsClient = (WMSClient)Servicies.ElementAt(i);
                    WmsClient.List(tree);
                }
                else
                {
                    WMTClient WmtClient = (WMTClient)Servicies.ElementAt(i);
                    WmtClient.List(tree);

                }
            }
        }


        public void Check(string[] keys, bool check)
        {
            for (int i = 0; i < Servicies.Count(); i++)
            {
                if (Servicies.ElementAt(i).GetType() == typeof(WMSClient))
                {
                    WMSClient WmsClient = (WMSClient)Servicies.ElementAt(i);

                    WmsClient.CheckLayer(keys, check);

                }
                else
                {
                    WMTClient WmtClient = (WMTClient)Servicies.ElementAt(i);
                    if (keys[0] == WmtClient.Name)
                    {
                        WmtClient.Visible = check;
                    }
                }
            }
        }
    }
}
