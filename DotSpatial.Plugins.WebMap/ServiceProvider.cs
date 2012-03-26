// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceProvider.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using DotSpatial.Plugins.WebMap.Resources;

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
        public ServiceProvider(string name, string url)
        {
            Url = url;
            Name = name;
        }

        public ServiceProvider()
        {
        }

        #endregion

        #region Public Properties

        public string Name { get; set; }

        public string Url { get; set; }

        #endregion

        #region Public Methods

        public static IEnumerable<ServiceProvider> GetDefaultServiceProviders()
        {
            foreach (var item in Services.Default.List)
            {
                var serviceDescArr = item.Split(',');

                var serviceName = serviceDescArr[0];
                var serviceUrl = serviceDescArr[1];

                yield return new ServiceProvider(serviceName, serviceUrl);
            }

            yield return new ServiceProvider(Properties.Resources.BingHybrid, null);
            yield return new ServiceProvider(Properties.Resources.GoogleSatellite, null);
            yield return new ServiceProvider(Properties.Resources.GoogleMap, null);
            yield return new ServiceProvider(Properties.Resources.YahooSatellite, null);
            yield return new ServiceProvider(Properties.Resources.YahooMap, null);
            yield return new ServiceProvider(Properties.Resources.WMSMap, null);
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}