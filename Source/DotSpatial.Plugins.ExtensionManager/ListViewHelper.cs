// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Plugins.ExtensionManager.Properties;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// ListViewHelper.
    /// </summary>
    internal class ListViewHelper
    {
        #region Methods

        /// <summary>
        /// Adds the packages to the listview.
        /// </summary>
        /// <param name="list">List with the packages.</param>
        /// <param name="listView">Listview to add the packages to.</param>
        /// <param name="pagenumber">PageNumber.</param>
        public void AddPackages(IEnumerable<IPackage> list, ListView listView, int pagenumber)
        {
            if (list == null)
            {
                return;
            }

            ImageList imageList = new ImageList
            {
                ImageSize = new Size(32, 32),
                ColorDepth = ColorDepth.Depth32Bit
            };

            // Add a default image at position 0;
            imageList.Images.Add(Resources.box_closed_32x32);
            listView.LargeImageList = imageList;
            listView.SmallImageList = imageList;

            var tasks = new List<Task<Image>>();

            listView.BeginUpdate();
            listView.Items.Clear();

            var pagelist = list.ToArray();
            foreach (var package in pagelist)
            {
                ListViewItem item = new ListViewItem(package.Id.Substring(package.Id.LastIndexOf('.') + 1))
                {
                    ImageIndex = 0
                };

                listView.Items.Add(item);
                item.Tag = package;

                if (package.IconUrl != null)
                {
                    var task = BeginGetImage(package.IconUrl.ToString());
                    tasks.Add(task);
                }
            }

            listView.EndUpdate();

            Task<Image>[] taskArray = tasks.ToArray();
            if (taskArray.Length == 0) return;

            Task.Factory.ContinueWhenAll(
                taskArray,
                t =>
                {
                    for (int i = 0; i < taskArray.Length; i++)
                    {
                        var image = taskArray[i].Result;
                        if (image != null)
                        {
                            imageList.Images.Add(image);
                            int imageCount = imageList.Images.Count;

                            // hack: for some reason i can be greater than the number of items in the listview.
                            // This is probably because we don't cancel the existing thread when the feed changes.
                            // todo: use CancellationToken
                            // this can also happen when the form closes.
                            if (listView.Items.Count < i + 1)
                                return;

                            var l = listView.Items[i];
                            l.ImageIndex = imageCount - 1;
                        }
                    }
                },
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static Task<Image> BeginGetImage(string iconUrl)
        {
            var task = Task.Factory.StartNew(
                () =>
                {
                    Image image = LoadImage(iconUrl);
                    return image;
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.Default); // Had to use TaskScheduler.Default so that the threads were not attached to the parent (Main UI thread for RefreshPackageList)

            return task;
        }

        private static Image LoadImage(string url)
        {
            if (url.Substring(0, 4) != "http")
            {
                return null;
            }

            try
            {
                WebRequest request = WebRequest.Create(url);
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream == null) return null;
                    return new Bitmap(responseStream);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}