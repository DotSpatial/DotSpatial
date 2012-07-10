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
        public static IEnumerable<Feed> GetFeeds() { return null; }

        public static void Add(Feed feed, ListView listview)
        {
            // Save settings...
            ListViewItem listViewItem = new ListViewItem();
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
            listViewItem.Text = feed.Name;
            try
            {
                PackageRepositoryFactory.Default.CreateRepository(feed.Url);
            }
            catch (UriFormatException)
            {
                MessageBox.Show("Enter a valid package feed URL.");
                return;
            }
            listViewItem.SubItems.Add(feed.Url);
            listview.Items.Add(listViewItem);
            Properties.Settings.Default.SourceName.Add(feed.Name);
            Properties.Settings.Default.SourceUrls.Add(feed.Url);
            Properties.Settings.Default.Save();
        }

        public static void Remove(Feed feed, ListView listview)
        {
            listview.SelectedItems[0].Remove();
            Properties.Settings.Default.SourceName.Remove(feed.Name);
            Properties.Settings.Default.SourceUrls.Remove(feed.Url);
            Properties.Settings.Default.Save();
        }
    }
}