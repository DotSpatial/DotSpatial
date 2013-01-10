using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuGet;
using System.Windows.Forms;
using System.Net;
using DotSpatial.Controls;
using System.Security.Permissions;
using System.Security;
using System.Threading.Tasks;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class Update
    {
        public Update(Packages package, ListViewHelper adder, AppManager Appmanager)
        {
            this.packages = package;
            this.Add = adder;
            this.App = Appmanager;
        }

        private readonly Packages packages;
        private readonly ListViewHelper Add;
        private AppManager App;
        private GetPackage getpack;
        private ListView listview;
        private TabPage tab;
        private IEnumerable<IPackage> list = null;
        private const string HideReleaseFromEndUser = "HideReleaseFromEndUser";

        public void RefreshUpdate(ListView lv, TabPage tp)
        {
            listview = lv;
            tab = tp;;
            list = null;

            getpack = new GetPackage(packages);
            IEnumerable<IPackage> localPackages = getpack.GetPackagesFromExtensions(App.Extensions);
            List<String> packageNames = null;
            if (localPackages.Count() > 0) 
            { 
                getUpdatesFromLocal(localPackages);
                packageNames = getPackageNames(localPackages);
            }
            
            getUpdatesFromOnline(packageNames);

            setListView();
        }

        private void setListView()
        {
            int Count = list.Count();

            listview.Invoke((Action)(() =>
            {
                listview.Clear();
                Add.AddPackages(list, listview, 0);
                tab.Text = String.Format("Updates ({0})", Count);
                if (Count == 0)
                {
                    listview.Clear();
                    listview.Items.Add("No updates available for the selected feed.");
                }
            }));
        }

        private void getUpdatesFromLocal(IEnumerable<IPackage> localPackages)
        {
            try
            {
                list = packages.Repo.GetUpdates(localPackages, false, false);
            }
            catch (WebException)
            {
                listview.Invoke((Action)(() =>
                {
                    listview.Clear();
                    listview.Items.Add("Updates could not be retrieved for the selected feed.");
                    listview.Items.Add("Try again later or change the feed.");
                }));
            }
        }

        private void getUpdatesFromOnline(List<String> packageNames)
        {
            IEnumerable<IPackage> onlinePacks = null;
            List<IPackage> updatePacks = new List<IPackage>();
            Task<PackageList> task = getPackagesFromOnline();

            task.ContinueWith(t =>
            {
                if (t.Result != null)
                {
                    onlinePacks = t.Result.packages;
                }
            }).Wait();

            foreach (IPackage pack in onlinePacks)
            {
                if (IsPackageUpdateable(pack))
                {
                    if (packageNames == null)
                    {
                        updatePacks.Add(pack);
                    }
                    else if (!packageNames.Contains(pack.Id))
                    {
                        updatePacks.Add(pack);
                    }
                }
            }
            if (list == null)
            {
                list = updatePacks;
            }
            else
            {
                list = list.Concat(updatePacks);
            }
        }

        private Task<PackageList> getPackagesFromOnline()
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
                    info.packages = result.ToArray();
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

        private bool IsPackageUpdateable(IPackage pack)
        {
            // will we be able to uninstall?
            var ext = App.GetExtension(pack.Id);
            if (ext == null) return false;

            //Checks if current version is greater than or equal to posted version, return false if true.
            //Assumes that version numbers in assembly files are accurate (not currently true).
            SemanticVersion extVersion = SemanticVersion.Parse(ext.Version);
            if (extVersion >= pack.Version) return false;

            string assemblyLocation = ExtensionManagerForm.GetExtensionPath(ext);

            // The original file may be in a different location than the extensions directory. During an update, the
            // original file will be deleted and the new package will be placed in the packages folder in the extensions
            // directory.
            FileIOPermission deletePermission = new FileIOPermission(FileIOPermissionAccess.Write, assemblyLocation);

            try
            {
                deletePermission.Demand();
            }
            catch (SecurityException)
            {
                return false;
            }

            return true;
        }

        private List<String> getPackageNames(IEnumerable<IPackage> packs)
        {
            List<String> packNames = new List<String>();
            foreach (IPackage p in packs)
            {
                packNames.Add(p.Id);
            }
            return packNames;
        }
    }
}