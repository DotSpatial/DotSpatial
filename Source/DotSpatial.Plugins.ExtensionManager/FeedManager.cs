using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using DotSpatial.Plugins.ExtensionManager.Properties;

namespace DotSpatial.Plugins.ExtensionManager
{
    internal static class FeedManager
    {
        public static IEnumerable<Feed> GetFeeds()
        {
            for (int j = 0; j < Settings.Default.SourceUrls.Count; j++)
            {
                Feed feed = new Feed();
                feed.Name = Settings.Default.SourceName[j];
                feed.Url = Settings.Default.SourceUrls[j];
                yield return feed;
            }
        }

        public static void Add(Feed feed)
        {
            if (Settings.Default.SourceName.Contains(feed.Name))
            {
                MessageBox.Show(String.Format("Source name '{0}' already exists.", feed.Name));
                return;
            }
            if (Settings.Default.SourceUrls.Contains(feed.Url))
            {
                MessageBox.Show(String.Format("Source URL '{0}' already exists.", feed.Url));
                return;
            }

            if (!feed.IsValid())
            {
                MessageBox.Show("Enter a valid package feed URL.");
                return;
            }

            Settings.Default.SourceName.Add(feed.Name);
            Settings.Default.SourceUrls.Add(feed.Url);
            Settings.Default.Save();
        }

        public static void Remove(Feed feed)
        {
            Settings.Default.SourceName.Remove(feed.Name);
            Settings.Default.SourceUrls.Remove(feed.Url);
            Settings.Default.Save();
        }

        public static void ClearFeeds()
        {
            Settings.Default.SourceName.Clear();
            Settings.Default.SourceUrls.Clear();
            Settings.Default.Save();
        }

        /// <summary>
        /// Add or remove feeds to be autoupdated when app starts.
        /// </summary>
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

        /// <summary>
        /// Return a list of feeds to autoupdate when app starts.
        /// </summary>
        public static StringCollection getAutoUpdateFeeds()
        {
            if (Settings.Default.FeedsToAutoUpdate == null)
            {
                Settings.Default.FeedsToAutoUpdate = new StringCollection();
            }
            return Settings.Default.FeedsToAutoUpdate;
        }
      
    }
}