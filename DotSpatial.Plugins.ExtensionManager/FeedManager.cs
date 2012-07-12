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
                string i = "1";
                Convert.ToInt32(i);
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
    }
}