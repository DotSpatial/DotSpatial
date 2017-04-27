using System.Configuration;

namespace DotSpatial.Plugins.WebMap.Configuration
{
    /// <summary>
    /// The WebMap configuration section.
    /// </summary>
    public class WebMapConfigurationSection : ConfigurationSection
    {
        #region Properties

        /// <summary>
        /// Gets the services.
        /// </summary>
        [ConfigurationProperty("Services", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(ServiceProviderElementCollection))]
        public ServiceProviderElementCollection Services => (ServiceProviderElementCollection)this["Services"];

        #endregion
    }
}