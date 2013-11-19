using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using DotSpatial.Plugins.WebMap.Configuration;

namespace DotSpatial.Plugins.WebMap
{
    public class ServiceProvider
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the ServiceProvider class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        public ServiceProvider(string name, string url = null)
        {
            Url = url;
            Name = name;
        }

        #endregion

        #region Public Properties

        public string Name { get; private set; }

        public string Url { get; private set; }

        #endregion

        #region Public Methods

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
                    var name = Properties.Resources.ResourceManager.GetString(service.Key) ?? service.Key;
                    yield return new ServiceProvider(name, service.Url);
                }

            }
            else
            {
                // Default services which used when config section not found
                yield return new ServiceProvider(Properties.Resources.EsriWorldHydroBasemap);
                yield return new ServiceProvider(Properties.Resources.EsriHydroBaseMap);
                yield return new ServiceProvider(Properties.Resources.EsriWorldStreetMap);
                yield return new ServiceProvider(Properties.Resources.EsriWorldImagery);
                yield return new ServiceProvider(Properties.Resources.EsriWorldTopo);
                yield return new ServiceProvider(Properties.Resources.BingRoads);
                yield return new ServiceProvider(Properties.Resources.BingAerial);
                yield return new ServiceProvider(Properties.Resources.BingHybrid);
                yield return new ServiceProvider(Properties.Resources.GoogleMap);
                yield return new ServiceProvider(Properties.Resources.GoogleSatellite);
                yield return new ServiceProvider(Properties.Resources.GoogleLabels);
                yield return new ServiceProvider(Properties.Resources.GoogleTerrain);
                yield return new ServiceProvider(Properties.Resources.YahooNormal);
                yield return new ServiceProvider(Properties.Resources.YahooSatellite);
                yield return new ServiceProvider(Properties.Resources.YahooHybrid);
                yield return new ServiceProvider(Properties.Resources.OpenStreetMap);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}