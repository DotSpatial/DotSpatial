﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using BruTile.Cache;
using BruTile.Predefined;
using BruTile.Web;
using DotSpatial.Plugins.WebMap.Configuration;
using DotSpatial.Plugins.WebMap.Properties;
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
            var servEq = (Func<string, bool>)(s => name.Equals(s, StringComparison.InvariantCultureIgnoreCase));

            var fileCache = (Func<ITileCache<byte[]>>)(() => new FileCache(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TileCache", name), string.Empty, new TimeSpan(30, 0, 0, 0)));

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
                yield return Create(Resources.GoogleLabels);
                yield return Create(Resources.GoogleTerrain);
                yield return Create(Resources.OpenStreetMap);
                yield return Create(Resources.WMSMap);
            }
        }

        #endregion
    }
}