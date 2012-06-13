using System;
using System.Collections.Generic;
using System.Linq;

using System.Drawing;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class PackageListHelper
    {
        public PackageListHelper(Packages packageHelper)
        {
            this.packages = packageHelper;
        }

        private readonly Packages packages;
        private const string HideReleaseFromEndUser = "HideReleaseFromEndUser";
        private readonly ListViewHelper add = new ListViewHelper();

        public void DisplayPackages(ListView listview)
        {
            listview.Items.Clear();
            listview.Items.Add("Loading...");

            Task<NuGet.IPackage[]> task = GetAllPackages();
            task.ContinueWith(t =>
            {
                add.AddPackages(t.Result, listview);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Task<NuGet.IPackage[]> GetAllPackages()
        {
            var task = Task.Factory.StartNew(() =>
                                            {
                                                var result = from item in packages.Repo.GetPackages()
                                                             where item.IsLatestVersion && (item.Tags == null || !item.Tags.Contains(HideReleaseFromEndUser))
                                                             select item;

                                                return result.ToArray();
                                            });
            return task;
        }
    }
}