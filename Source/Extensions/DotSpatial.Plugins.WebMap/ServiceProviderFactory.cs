// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using BruTile;
using BruTile.Cache;
using BruTile.Predefined;
using BruTile.Web;
using DotSpatial.Plugins.WebMap.Configuration;
using DotSpatial.Plugins.WebMap.WMS;

namespace DotSpatial.Plugins.WebMap
{
    /// <summary>
    /// The service provider factory can be used to create service providers.
    /// </summary>
    public static class ServiceProviderFactory
    {
        #region Methods

        /// <summary>
        /// Creates a new service provider.
        /// </summary>
        /// <param name="name">Name of the service provider that should be crreated.</param>
        /// <param name="url">Url for the service provider.</param>
        /// <returns>The created service provider.</returns>
        public static ServiceProvider Create(string name, string url = null)
        {
            bool servEq(string s) => name.Equals(s, StringComparison.InvariantCultureIgnoreCase);

            ITileCache<byte[]> fileCache() => new FileCache(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TileCache", name), string.Empty, new TimeSpan(30, 0, 0, 0));

            if (servEq(Resources.EsriHydroBaseMap))
            {
                return new BrutileServiceProvider(name, new ArcGisTileSource("http://bmproto.esri.com/ArcGIS/rest/services/Hydro/HydroBase2009/MapServer/", new GlobalSphericalMercator()), fileCache());
            }

            if (servEq(Resources.EsriWorldStreetMap))
            {
                return new BrutileServiceProvider(name, new ArcGisTileSource("http://server.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/", new GlobalSphericalMercator()), fileCache());
            }

            if (servEq(Resources.EsriWorldImagery))
            {
                return new BrutileServiceProvider(name, new ArcGisTileSource("http://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/", new GlobalSphericalMercator()), fileCache());
            }

            if (servEq(Resources.EsriWorldTopo))
            {
                return new BrutileServiceProvider(name, KnownTileSources.Create(KnownTileSource.EsriWorldTopo), fileCache());
            }

            if (servEq(Resources.BingHybrid))
            {
                return new BrutileServiceProvider(name, KnownTileSources.Create(KnownTileSource.BingHybrid), fileCache());
            }

            if (servEq(Resources.BingAerial))
            {
                return new BrutileServiceProvider(name, KnownTileSources.Create(KnownTileSource.BingAerial), fileCache());
            }

            if (servEq(Resources.BingRoads))
            {
                return new BrutileServiceProvider(name, KnownTileSources.Create(KnownTileSource.BingRoads), fileCache());
            }

            if (servEq(Resources.GoogleMap))
            {
                return new BrutileServiceProvider(name, CreateGoogleTileSource("http://mt{s}.google.com/vt/lyrs=m@130&hl=en&x={x}&y={y}&z={z}"), fileCache());
            }

            if (servEq(Resources.GoogleTerrain))
            {
                return new BrutileServiceProvider(name, CreateGoogleTileSource("http://mt{s}.google.com/vt/lyrs=t@125,r@130&hl=en&x={x}&y={y}&z={z}"), fileCache());
            }

            if (servEq(Resources.OpenStreetMap))
            {
                return new BrutileServiceProvider(name, KnownTileSources.Create(), fileCache());
            }

            if (servEq(Resources.WMSMap))
            {
                return new WmsServiceProvider(name);
            }

            // No Match
            return new OtherServiceProvider(name, url);
        }

        /// <summary>
        /// Gets the default service providers.
        /// </summary>
        /// <returns>An IEnumerable of the default service providers.</returns>
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
                // Default services which are used when config section can't be found
                yield return Create(Resources.EsriHydroBaseMap);
                yield return Create(Resources.EsriWorldStreetMap);
                yield return Create(Resources.EsriWorldImagery);
                yield return Create(Resources.EsriWorldTopo);
                yield return Create(Resources.BingRoads);
                yield return Create(Resources.BingAerial);
                yield return Create(Resources.BingHybrid);
                yield return Create(Resources.GoogleMap);
                yield return Create(Resources.GoogleTerrain);
                yield return Create(Resources.OpenStreetMap);
                yield return Create(Resources.WMSMap);
            }
        }

        private static ITileSource CreateGoogleTileSource(string urlFormatter)
        {
            return new HttpTileSource(new GlobalSphericalMercator(), urlFormatter, new[] { "0", "1", "2", "3" }, tileFetcher: FetchGoogleTile);
        }

        private static byte[] FetchGoogleTile(Uri arg)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "http://maps.google.com/");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", @"Mozilla / 5.0(Windows; U; Windows NT 6.0; en - US; rv: 1.9.1.7) Gecko / 20091221 Firefox / 3.5.7");

            return httpClient.GetByteArrayAsync(arg).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        #endregion
    }
}