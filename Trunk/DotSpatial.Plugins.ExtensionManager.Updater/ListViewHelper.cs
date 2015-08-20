using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Plugins.ExtensionManager.Updater.Properties;

namespace DotSpatial.Plugins.ExtensionManager.Updater
{
    public class ListViewHelper
    {

        public void AddChecked(IEnumerable<string> list, ListView listView)
        {
            if (list == null)
            {
                return;
            }

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(16, 16);
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            // Add a default image at position 0;
            imageList.Images.Add(Resources.Tick_16x16);

            var pagelist = list.ToArray();
            ListViewItem[] items = new ListViewItem[pagelist.Length];
            for (int i = 0; i < pagelist.Length; i++)
            {
                ListViewItem item = new ListViewItem(pagelist[i].Substring(pagelist[i].LastIndexOf('.') + 1));
                item.ImageIndex = 0;
                items[i]= item;
            }

            listView.Invoke((Action)(() =>
            {
                listView.LargeImageList = imageList;
                listView.SmallImageList = imageList;

                listView.BeginUpdate();
                listView.Items.Clear();
                listView.Items.AddRange(items);

                listView.EndUpdate();
            }));
        }
    }
}