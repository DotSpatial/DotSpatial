// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Configuration;

namespace DotSpatial.Plugins.WebMap.Configuration
{
    /// <summary>
    /// Represents a service provider configuration element.
    /// </summary>
    public class ServiceProviderElement : ConfigurationElement
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the provider should be ignored.
        /// </summary>
        [ConfigurationProperty("Ignore", IsRequired = false)]
        public bool Ignore
        {
            get
            {
                return (bool)this[nameof(Ignore)];
            }

            set
            {
                this[nameof(Ignore)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [ConfigurationProperty("Key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get
            {
                return (string)this[nameof(Key)];
            }

            set
            {
                this["key"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        [ConfigurationProperty("Url", IsRequired = false)]
        public string Url
        {
            get
            {
                return (string)this[nameof(Url)];
            }

            set
            {
                this[nameof(Url)] = value;
            }
        }

        #endregion
    }
}