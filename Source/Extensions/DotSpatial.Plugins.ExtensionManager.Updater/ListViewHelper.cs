// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Plugins.ExtensionManager.Updater.Properties;

namespace DotSpatial.Plugins.ExtensionManager.Updater
{
    /// <summary>
    /// ListViewHelper.
    /// </summary>
    public class ListViewHelper
    {
        #region Methods

        /// <summary>
        /// Adds the items from the given list to the given listView.
        /// </summary>
        /// <param name="list">List that contains the items that should be added.</param>
        /// <param name="listView">ListView the items get added to.</param>
        public void AddChecked(IEnumerable<string> list, ListView listView)
        {
            if (list == null)
            {
                return;
            }

            ImageList imageList = new ImageList
            {
                ImageSize = new Size(16, 16),
                ColorDepth = ColorDepth.Depth32Bit
            };

            // Add a default image at position 0;
            imageList.Images.Add(Resources.Tick_16x16);

            var pagelist = list.ToArray();
            ListViewItem[] items = new ListViewItem[pagelist.Length];
            for (int i = 0; i < pagelist.Length; i++)
            {
                items[i] = new ListViewItem(pagelist[i].Substring(pagelist[i].LastIndexOf('.') + 1))
                {
                    ImageIndex = 0
                };
            }

            listView.Invoke(
                (Action)(() =>
                {
                    listView.LargeImageList = imageList;
                    listView.SmallImageList = imageList;

                    listView.BeginUpdate();
                    listView.Items.Clear();
                    listView.Items.AddRange(items);

                    listView.EndUpdate();
                }));
        }

        #endregion
    }
}