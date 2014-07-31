using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using BruTile;
using BruTile.Cache;
using BruTile.Predefined;
using BruTile.Web;
using DotSpatial.Plugins.WebMap.Configuration;
using DotSpatial.Plugins.WebMap.Properties;
using DotSpatial.Plugins.WebMap.WMS;
using DotSpatial.Plugins.WebMap.Yahoo;

namespace DotSpatial.Plugins.WebMap
{
    public static class ServiceProviderFactory
    {
        public static IEnumerable<ServiceProvider> GetDefaultServiceProviders()
        {
            WebMapConfigurationSection section = null;
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
                section = (WebMapConfigurationSection)config.GetSection("webMapConfigurationSection");
            }
            catch (Exception e)
            {
                Debug.Write("Section webMapConfigurationSection not found: " + e);
            }

            if (section != null)
            {
                foreach (ServiceProviderElement service in section.Services)
                {
                    if (service.Ignore) continue;
                    var name = Resources.ResourceManager.GetString(service.Key) ?? service.Key;
                    yield return Create(name, service.Url);
                }

            }
            else
            {
                // Default services which used when config section not found
                yield return Create(Resources.EsriWorldHydroBasemap);
                yield return Create(Resources.EsriHydroBaseMap);
                yield return Create(Resources.EsriWorldStreetMap);
                yield return Create(Resources.EsriWorldImagery);
                yield return Create(Resources.EsriWorldTopo);
                yield return Create(Resources.BingRoads);
                yield return Create(Resources.BingAerial);
                yield return Create(Resources.BingHybrid);
                yield return Create(Resources.GoogleMap);
                yield return Create(Resources.GoogleSatellite);
                yield return Create(Resources.GoogleLabels);
                yield return Create(Resources.GoogleTerrain);
                yield return Create(Resources.YahooNormal);
                yield return Create(Resources.YahooSatellite);
                yield return Create(Resources.YahooHybrid);
                yield return Create(Resources.OpenStreetMap);
                yield return Create(Resources.WMSMap);
            }
        }

        public static ServiceProvider Create(string name, string url = null)
        {
            var servEq = (Func<string, bool>)
                (s => name.Equals(s, StringComparison.InvariantCultureIgnoreCase));

            var fileCache =
                (Func<ITileCache<byte[]>>)
                    (() => new FileCache(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            "TileCache", name), "", new TimeSpan(30, 0, 0, 0)));

            if (servEq(Resources.EsriWorldHydroBasemap))
            {
                return new BrutileServiceProvider(name, TileSource.Create(KnownTileServers.EsriWorldHydroBasemap), fileCache());
            }
            if (servEq(Resources.EsriHydroBaseMap))
            {
                return new BrutileServiceProvider(name,
                    new ArcGisTileSource("http://bmproto.esri.com/ArcGIS/rest/services/Hydro/HydroBase2009/MapServer/",
                        new GlobalSphericalMercator()), fileCache());
            }
            if (servEq(Resources.EsriWorldStreetMap))
            {
                return new BrutileServiceProvider(name,  new ArcGisTileSource("http://server.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/",
                    new GlobalSphericalMercator()), fileCache());
            }
            if (servEq(Resources.EsriWorldImagery))
            {
                return new BrutileServiceProvider(name, 
                    new ArcGisTileSource("http://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/",
                        new GlobalSphericalMercator()), fileCache());
            }
            if (servEq(Resources.EsriWorldTopo))
            {
                return new BrutileServiceProvider(name, TileSource.Create(KnownTileServers.EsriWorldTopo), fileCache());
            }
            if (servEq(Resources.BingHybrid))
            {
                return new BrutileServiceProvider(name, new BingTileSource(new BingRequest(BingRequest.UrlBingStaging, String.Empty, BingMapType.Hybrid)), fileCache());
            }
            if (servEq(Resources.BingAerial))
            {
                return new BrutileServiceProvider(name, new BingTileSource(new BingRequest(BingRequest.UrlBingStaging, String.Empty, BingMapType.Aerial)), fileCache());
            }
            if (servEq(Resources.BingRoads))
            {
                return new BrutileServiceProvider(name, new BingTileSource(new BingRequest(BingRequest.UrlBingStaging, String.Empty, BingMapType.Roads)), fileCache());
            }
            if (servEq(Resources.GoogleSatellite))
            {
                return new BrutileServiceProvider(name, new GoogleTileSource(GoogleMapType.GoogleSatellite), fileCache());
            }
            if (servEq(Resources.GoogleMap))
            {
                return new BrutileServiceProvider(name, new GoogleTileSource(GoogleMapType.GoogleMap), fileCache());
            }
            if (servEq(Resources.GoogleLabels))
            {
                return new BrutileServiceProvider(name, new GoogleTileSource(GoogleMapType.GoogleLabels), fileCache());
            }
            if (servEq(Resources.GoogleTerrain))
            {
                return new BrutileServiceProvider(name, new GoogleTileSource(GoogleMapType.GoogleTerrain), fileCache());
            }
            if (servEq(Resources.YahooNormal))
            {
                return new BrutileServiceProvider(name, new YahooTileSource(YahooMapType.Normal), fileCache());
            }
            if (servEq(Resources.YahooSatellite))
            {
                return new BrutileServiceProvider(name, new YahooTileSource(YahooMapType.Satellite), fileCache());
            }
            if (servEq(Resources.YahooHybrid))
            {
                return new BrutileServiceProvider(name, new YahooTileSource(YahooMapType.Hybrid), fileCache());
            }
            if (servEq(Resources.OpenStreetMap))
            {
                return new BrutileServiceProvider(name, TileSource.Create(), fileCache());
            }
            if (servEq(Resources.WMSMap))
            {
                return new WmsServiceProvider(name);
            }

            // No Match
            return new OtherServiceProvider(name, url);
        }
    }
}