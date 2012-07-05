using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class PackageListHelper
    {
        public PackageListHelper(Packages packageHelper, ListViewHelper adder)
        {
            this.packages = packageHelper;
            this.add = adder;
        }

        private readonly Packages packages;
        private const string HideReleaseFromEndUser = "HideReleaseFromEndUser";
        private readonly ListViewHelper add;

        public void DisplayPackages(ListView listview, int pagenumber, TabPage tab)
        {
            listview.Items.Clear();
            listview.Items.Add("Loading...");

            Task<NuGet.IPackage[]> task = GetAllPackages();
            task.ContinueWith(t =>
            {
                listview.Items.Clear();
                if (t.Result == null)
                {
                    listview.Items.Add("No packages could be retrieved for the selected feed.");
                    listview.Items.Add("Try again later or Select another feed.");
                }
                else
                {
                    add.AddPackages(t.Result, listview, pagenumber);
                    add.CreateButtons(t.Result);
                    add.AddButtons(tab);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Task<NuGet.IPackage[]> GetAllPackages()
        {
            var task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var result = from item in packages.Repo.GetPackages()
                                     where item.IsLatestVersion && (item.Tags == null || !item.Tags.Contains(HideReleaseFromEndUser))
                                     select item;

                        return result.ToArray();
                    }
                    catch (InvalidOperationException)
                    {
                        // This usually means the url was bad.
                        return null;
                    }
                });
            return task;
        }
    }
}