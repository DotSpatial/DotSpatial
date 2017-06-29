using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    public static class FeedManager
    {
        public static IEnumerable<Feed> GetFeeds()
        {
            for (int j = 0; j < Properties.Settings.Default.SourceUrls.Count; j++)
            {
                Feed feed = new Feed();
                feed.Name = Properties.Settings.Default.SourceName[j];
                feed.Url = Properties.Settings.Default.SourceUrls[j];
                yield return feed;
            }
        }

        public static void Add(Feed feed)
        {
            if (Properties.Settings.Default.SourceName.Contains(feed.Name))
            {
                MessageBox.Show(String.Format("Source name '{0}' already exists.", feed.Name));
                return;
            }
            if (Properties.Settings.Default.SourceUrls.Contains(feed.Url))
            {
                MessageBox.Show(String.Format("Source URL '{0}' already exists.", feed.Url));
                return;
            }

            if (!feed.IsValid())
            {
                MessageBox.Show("Enter a valid package feed URL.");
                return;
            }

            Properties.Settings.Default.SourceName.Add(feed.Name);
            Properties.Settings.Default.SourceUrls.Add(feed.Url);
            Properties.Settings.Default.Save();
        }

        public static void Remove(Feed feed)
        {
            Properties.Settings.Default.SourceName.Remove(feed.Name);
            Properties.Settings.Default.SourceUrls.Remove(feed.Url);
            Properties.Settings.Default.Save();
        }

        public static void ClearFeeds()
        {
            Properties.Settings.Default.SourceName.Clear();
            Properties.Settings.Default.SourceUrls.Clear();
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Add or remove feeds to be autoupdated when app starts.
        /// </summary>
        /// <param name="appManager">The app manager.</param>
        /// <param name="path">URL of feed.</param>
        public static void ToggleAutoUpdate(string feed, Boolean toAdd)
        {
            {
                if (toAdd && !Properties.Settings.Default.FeedsToAutoUpdate.Contains(feed))
                {
                    Properties.Settings.Default.FeedsToAutoUpdate.Add(feed);
                }
                else if (!toAdd && Properties.Settings.Default.FeedsToAutoUpdate.Contains(feed))
                {
                    Properties.Settings.Default.FeedsToAutoUpdate.Remove(feed);
                }
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// Return a list of feeds to autoupdate when app starts.
        /// </summary>
        /// <param name="appManager">The app manager.</param>
        public static System.Collections.Specialized.StringCollection getAutoUpdateFeeds()
        {
            if (Properties.Settings.Default.FeedsToAutoUpdate == null)
            {
                Properties.Settings.Default.FeedsToAutoUpdate = new System.Collections.Specialized.StringCollection();
            }
            return Properties.Settings.Default.FeedsToAutoUpdate;
        }

        /// <summary>
        /// Check if a specific URL is in the autoupdate list.
        /// </summary>
        /// <param name="appManager">The app manager.</param>
        /// <param name="feed">The feed to check.</param>
        public static bool isAutoUpdateFeed(string feed)
        {
            return Properties.Settings.Default.FeedsToAutoUpdate.Contains(feed);
        }
    }
}