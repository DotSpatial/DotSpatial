// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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