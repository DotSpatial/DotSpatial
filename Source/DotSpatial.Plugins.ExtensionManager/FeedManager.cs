using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using DotSpatial.Plugins.ExtensionManager.Properties;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// FeedManager
    /// </summary>
    internal static class FeedManager
    {
        #region Methods

        /// <summary>
        /// Adds the given feed.
        /// </summary>
        /// <param name="feed">Feed to add.</param>
        public static void Add(Feed feed)
        {
            if (Settings.Default.SourceName.Contains(feed.Name))
            {
                MessageBox.Show(string.Format(Resources.SourceName0AlreadyExists, feed.Name));
                return;
            }

            if (Settings.Default.SourceUrls.Contains(feed.Url))
            {
                MessageBox.Show(string.Format(Resources.SourceURL0AlreadyExists, feed.Url));
                return;
            }

            if (!feed.IsValid())
            {
                MessageBox.Show(Resources.EnterValidPackageFeedUrl);
                return;
            }

            Settings.Default.SourceName.Add(feed.Name);
            Settings.Default.SourceUrls.Add(feed.Url);
            Settings.Default.Save();
        }

        /// <summary>
        /// Clears the feeds.
        /// </summary>
        public static void ClearFeeds()
        {
            Settings.Default.SourceName.Clear();
            Settings.Default.SourceUrls.Clear();
            Settings.Default.Save();
        }

        /// <summary>
        /// Return a list of feeds to autoupdate when app starts.
        /// </summary>
        /// <returns>A list of feeds to autoupdate when app starts.</returns>
        public static StringCollection GetAutoUpdateFeeds()
        {
            return Settings.Default.FeedsToAutoUpdate ?? (Settings.Default.FeedsToAutoUpdate = new StringCollection());
        }

        /// <summary>
        /// Gets all feeds.
        /// </summary>
        /// <returns>Feeds.</returns>
        public static IEnumerable<Feed> GetFeeds()
        {
            for (int j = 0; j < Settings.Default.SourceUrls.Count; j++)
            {
                Feed feed = new Feed
                {
                    Name = Settings.Default.SourceName[j],
                    Url = Settings.Default.SourceUrls[j]
                };
                yield return feed;
            }
        }

        /// <summary>
        /// Roves the given feed from the list.
        /// </summary>
        /// <param name="feed">Feed to remove.</param>
        public static void Remove(Feed feed)
        {
            Settings.Default.SourceName.Remove(feed.Name);
            Settings.Default.SourceUrls.Remove(feed.Url);
            Settings.Default.Save();
        }

        /// <summary>
        /// Add or remove feeds to be autoupdated when app starts.
        /// </summary>
        /// <param name="feed">Feed that gets added or removed.</param>
        /// <param name="toAdd">Indicates whether feed should be added or removed.</param>
        public static void ToggleAutoUpdate(string feed, bool toAdd)
        {
            {
                if (toAdd && !Settings.Default.FeedsToAutoUpdate.Contains(feed))
                {
                    Settings.Default.FeedsToAutoUpdate.Add(feed);
                }
                else if (!toAdd && Settings.Default.FeedsToAutoUpdate.Contains(feed))
                {
                    Settings.Default.FeedsToAutoUpdate.Remove(feed);
                }

                Settings.Default.Save();
            }
        }

        #endregion
    }
}