using System;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    internal class Feed
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public bool IsValid()
        {
            if (!Url.StartsWith("http"))
            {
                return false;
            }

            //  We use the nuget PackageRepositoryFactory to test whether the url seems valid.
            try
            {
                PackageRepositoryFactory.Default.CreateRepository(Url);
            }
            catch (UriFormatException)
            {
                return false;
            }

            return true;
        }
    }
}