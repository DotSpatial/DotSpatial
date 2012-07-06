using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class PackageListHelper
    {
        private readonly Packages packages;
        private const string HideReleaseFromEndUser = "HideReleaseFromEndUser";
        private readonly ListViewHelper add;
        private readonly Paging paging;

        public PackageListHelper(Packages packageHelper, ListViewHelper adder, Paging Pages)
        {
            this.packages = packageHelper;
            this.add = adder;
            this.paging = Pages;
        }

        public void DisplayPackages(ListView listview, int pagenumber, TabPage tab)
        {
            paging.ResetButtons(tab);
            listview.Items.Clear();
            listview.Items.Add("Loading...");

            Task<PackageList> task = GetPackages(pagenumber);
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
                    add.AddPackages(t.Result.packages, listview, pagenumber);
                    paging.CreateButtons(t.Result.TotalPackageCount);
                    paging.AddButtons(tab);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private Task<PackageList> GetPackages(int pagenumber)
        {
            var task = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        var result = from item in packages.Repo.GetPackages()
                                     where item.IsLatestVersion && (item.Tags == null || !item.Tags.Contains(HideReleaseFromEndUser))
                                     select item;

                        var info = new PackageList();
                        info.TotalPackageCount = result.Count();
                        info.packages = result.Skip(pagenumber * Paging.PageSize).Take(Paging.PageSize).ToArray();
                        return info;
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