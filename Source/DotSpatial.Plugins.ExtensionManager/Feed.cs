using System;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// Feed
    /// </summary>
    internal class Feed
    {
        #region Properties

        /// <summary>
        /// Gets or sets the feeds name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the feeds url.
        /// </summary>
        public string Url { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a value indicating whether the feed is valid.
        /// </summary>
        /// <returns>True, if feed is valid.</returns>
        public bool IsValid()
        {
            if (!Url.StartsWith("http"))
            {
                return false;
            }

            // We use the nuget PackageRepositoryFactory to test whether the url seems valid.
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

        #endregion
    }
}