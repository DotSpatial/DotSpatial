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
                return (bool)this["Ignore"];
            }

            set
            {
                this["Ignore"] = value;
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
                return (string)this["Key"];
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
                return (string)this["Url"];
            }

            set
            {
                this["Url"] = value;
            }
        }

        #endregion
    }
}