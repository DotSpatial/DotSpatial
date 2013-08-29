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
            List<string> extraServices = new List<string>(){
            Properties.Resources.BingHybrid,
            Properties.Resources.GoogleSatellite,
            Properties.Resources.GoogleMap/*,
            Properties.Resources.WMSMap Commented out for 1.6 HydroDesktop release. See https://hydrodesktop.codeplex.com/workitem/8731 */
            };

            foreach (var item in Services.Default.List)
            {
                var serviceDescArr = item.Split(',');

                var serviceName = serviceDescArr[0];
                var serviceUrl = serviceDescArr[1];

                while (extraServices.Count > 0 && extraServices[0].ToUpper()[0] < serviceName.ToUpper()[0])
                {
                    yield return new ServiceProvider(extraServices[0], null);
                    extraServices.Remove(extraServices[0]);
                }

                yield return new ServiceProvider(serviceName, serviceUrl);
            }

            foreach (var item in extraServices)
            {
                yield return new ServiceProvider(item, null);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion
    }
}