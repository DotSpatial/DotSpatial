using System.Configuration;

namespace DotSpatial.Plugins.WebMap.Configuration
{
    public class ServiceProviderElement : ConfigurationElement
    {
        [ConfigurationProperty("Key", IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)this["Key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("Url", IsRequired = false)]
        public string Url
        {
            get { return (string)this["Url"]; }
            set { this["Url"] = value; }
        }

        [ConfigurationProperty("Ignore", IsRequired = false)]
        public bool Ignore
        {
            get { return (bool)this["Ignore"]; }
            set { this["Ignore"] = value; }
        }
    }
}