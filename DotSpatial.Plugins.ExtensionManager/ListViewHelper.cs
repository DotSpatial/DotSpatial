using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class ListViewHelper
    {
        public void AddPackages(IEnumerable<IPackage> list, ListView listView, int pagenumber)
        {
            if (list == null)
            {
                return;
            }

            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(32, 32);
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            // Add a default image at position 0;
            imageList.Images.Add(DotSpatial.Plugins.ExtensionManager.Properties.Resources.box_closed_32x32);
            listView.LargeImageList = imageList;
            listView.SmallImageList = imageList;

            var tasks = new List<Task<Image>>();

            listView.BeginUpdate();
            listView.Items.Clear();

            var pagelist = list.ToArray();
            foreach (var package in pagelist)
            {
                ListViewItem item = new ListViewItem(package.Id);
                string description = null;
                if (package.Description.Length > 56)
                {
                    description = package.Description.Substring(0, 53) + "...";
                }
                else
                {
                    description = package.Description;
                }
                item.SubItems.Add(description);
                item.ImageIndex = 0;

                listView.Items.Add(item);
                item.Tag = package;

                var task = BeginGetImage(package.IconUrl.ToString());
                tasks.Add(task);
            }
            listView.EndUpdate();

            Task<Image>[] taskArray = tasks.ToArray();
            if (taskArray.Count() == 0) return;

            Task.Factory.ContinueWhenAll(taskArray, t =>
            {
                for (int i = 0; i < taskArray.Length; i++)
                {
                    var image = taskArray[i].Result;
                    if (image != null)
                    {
                        imageList.Images.Add(image);
                        int imageCount = imageList.Images.Count;

                        //hack: for some reason i can be greater than the number of items in the listview.
                        // This is probably because we don't cancel the existing thread when the feed changes.
                        // todo: use CancellationToken
                        // this can also happen when the form closes.

                        var l = listView.Items[i];
                        l.ImageIndex = imageCount - 1;
                    }
                }
            },
         new System.Threading.CancellationToken(), TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private static Task<Image> BeginGetImage(string iconUrl)
        {
            var task = Task.Factory.StartNew(() =>
            {
                Image image = LoadImage(iconUrl);
                return image;

                // Had to use TaskScheduler.Default so that the threads were not attached to the parent (Main UI thread for RefreshPackageList)
            },
            new System.Threading.CancellationToken(), TaskCreationOptions.None, TaskScheduler.Default);

            return task;
        }

        private static Image LoadImage(string url)
        {
            if (url.Substring(0, 4) != "http")
            {
                return null;
            }
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);

            try
            {
                System.Net.WebResponse response = request.GetResponse();
                System.IO.Stream responseStream = response.GetResponseStream();

                Bitmap bmp = new Bitmap(responseStream);

                responseStream.Dispose();

                return bmp;
            }
            catch (System.Net.WebException)
            {
                return null;
            }
        }
    }
}