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
using DotSpatial.Controls.Extensions;
using System.IO;
using System.Reflection;
using DotSpatial.Extensions;
using System.Collections.Specialized;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.RegularExpressions;

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
        private const string HideFromAutoUpdate = "HideFromAutoUpdate";
        private const string minVersion = "minVersion:";

        //Function that refreshes the listview in the updates tab.
        public void RefreshUpdate(ListView lv, TabPage tp)
        {
            listview = lv;
            tab = tp;
            getAllAvailableUpdates();

            //Refresh the list view with the updates found.
            if (list != null) { setListView(); }
        }

        private void getAllAvailableUpdates()
        {
            list = null;

            //Look for packages to be updated in the folder where Extension Manager downloads new packages.
            getpack = new GetPackage(packages);
            IEnumerable<IPackage> localPackages = getpack.GetPackagesFromExtensions(App.Extensions);
            List<String> packageNames = null;
            if (localPackages.Count() > 0)
            {
                getAvailableUpdatesFromLocal(localPackages);
                packageNames = getPackageNames(localPackages);
            }

            //Find other packages that may need updating by looking at the current feed and comparing to installed packages.
            getAvailableUpdatesFromFeed(packageNames);
        }

        //Using the class variable 'list' to refresh the packages that are eligble for update.
        private void setListView()
        {
            int Count = list.Count();

            try
            {
                if (listview.InvokeRequired)
                {
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
                else
                {
                    listview.Clear();
                    Add.AddPackages(list, listview, 0);
                    tab.Text = String.Format("Updates ({0})", Count);
                    if (Count == 0)
                    {
                        listview.Clear();
                        listview.Items.Add("No updates available for the selected feed.");
                    }
                }
            }
            catch 
            {     
                //do nothing
            }
        }

        //Looks in the folder where extensions are saved (when downloaded through the Ext Manager) and determines if updates are needed.
        private void getAvailableUpdatesFromLocal(IEnumerable<IPackage> localPackages)
        {
            try
            {
                list = packages.Repo.GetUpdates(localPackages, false, false);
            }
            catch (WebException)
            {
                if (listview != null && tab != null)
                {
                    listview.Invoke((Action)(() =>
                    {
                        listview.Clear();
                        tab.Text = "Update";
                        listview.Items.Add("Updates could not be retrieved for the selected feed.");
                        listview.Items.Add("Try again later or change the feed.");
                    }));
                }
            }
        }

        //Looks at all packages from the current feed, finds local version of it (if any), compares versions.
        //packageNames are all packages found and checked by getUpdatesFromLocal.
        private void getAvailableUpdatesFromFeed(List<String> packageNames)
        {
            IEnumerable<IPackage> onlinePacks = null;
            List<IPackage> updatePacks = new List<IPackage>();
            Task<PackageList> task = getAllPackagesFromFeed();

            //Get list of packages from current feed.
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
                    //If packageNames has no names, just add all packages that are updateable.
                    if (packageNames == null)
                    {
                        updatePacks.Add(pack);
                    }
                    else if (!packageNames.Contains(pack.Id)) //If there are packageNames, then make sure we are not adding a package we've already checked.
                    {
                        updatePacks.Add(pack);
                    }
                }
            }
            //If list is not null, add the new packages to it.
            list = list != null ? list.Concat(updatePacks) : updatePacks;
        }

        //Return list of all packages from currently selected feed.
        private Task<PackageList> getAllPackagesFromFeed()
        {
            var task = Task.Factory.StartNew(() =>
            {
                try
                {
                    List<IPackage> result = new List<IPackage>();
                    var allPackages = from item in packages.Repo.GetPackages() where
                                 (item.Tags == null || (!item.Tags.Contains(HideReleaseFromEndUser)))
                                 select item;

                    var sortedPackages = allPackages.OrderBy(item => item.Id)
                                    .ThenByDescending(item => item.Version);

                    String id = "";

                    foreach(var package in sortedPackages)
                    {
                        if (id != package.Id && MinVersionCheck(package))
                        {
                            result.Add(package);
                            id = package.Id;
                        }
                    }
                    
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

        private bool MinVersionCheck(IPackage item)
        {
            bool versionCorrect = true;

            if (item.Tags != null && item.Tags.Contains(minVersion))
            {
                var tags = item.Tags.Split(' ');

                foreach (var tag in tags)
                {
                    if (tag.Contains(minVersion))
                    {
                        var version = SemanticVersion.Parse(tag.Substring(minVersion.Length - 1));
                        var programVersion = 
                            SemanticVersion.Parse(Assembly.GetEntryAssembly().GetName().Version.ToString());
                        if (programVersion < version)
                            versionCorrect = false;
                        break;
                    }
                }
            }

            return versionCorrect;
        }

        private bool IsOnline()
        {
            try
            {
                IPHostEntry dummy = Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Cycle through feeds from settings and call autoupdate function. If any updates occur, show message box.
        public static void autoUpdateController(AppManager app, ExtensionManagerForm form)
        {
            List<String> updatesOccurred = new List<String>();
            Packages packages = new Packages();
            System.Collections.Specialized.StringCollection feeds = FeedManager.getAutoUpdateFeeds();
            Update update = new Update(packages, null, app);

            if (update.IsOnline() == true)
            {
                foreach (String feed in feeds)
                {
                    packages.SetNewSource(feed);
                    update = new Update(packages, null, app);
                    updatesOccurred.AddRange(update.autoUpdate());
                }
            }

            if (updatesOccurred.Count > 0)
            {
                String begin;
                if (updatesOccurred.Count == 1){ begin = "The following extension has been updated:"; }
                else{ begin = "The following extensions have been updated:"; }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine(begin);
                sb.AppendLine();
                for (int i = 0; i < updatesOccurred.Count && i<25; i++)
                {
                    sb.AppendLine("-" + updatesOccurred.ElementAt(i));
                }
                if (updatesOccurred.Count > 25) { sb.AppendLine("..."); }
                sb.AppendLine();
                sb.Append("Updates will finish when HydroDesktop is restarted.");
                sb.AppendLine("\n\n      Do you want to restart HydroDesktop now?");

                DialogResult dialogResult = MessageBox.Show(sb.ToString(), "Update Complete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(Application.ExecutablePath);
                    Environment.Exit(-1);
                }
                form.AutoUpdateRestartNeccesary();
            }
        }

        //autoUpdate any packages found in current feed.
        internal List<String> autoUpdate()
        {
            List<String> updatesOccurred = new List<String>();
            getAllAvailableUpdates();

            if (list != null && list.Count() > 0)
            {
                DialogResult dialogResult = MessageBox.Show("       "+list.Count()+" Updates available.\n\n"+
                    "Do you want to install them now?", "Updates available", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    foreach (IPackage p in list)
                    {
                        if ((p.Tags == null) || (!p.Tags.Contains(HideFromAutoUpdate)))
                        {
                            foreach (PackageDependency dependency in p.Dependencies)
                            {
                                if (packages.GetLocalPackage(dependency.Id) == null)
                                    packages.Update(packages.Repo.FindPackage(dependency.Id));
                            }

                            try
                            {
                                UpdatePackage(p);
                                updatesOccurred.Add(p.GetFullName());
                            }
                            catch
                            {
                                MessageBox.Show("Error updating " + p.GetFullName());
                            }
                        }
                    }
                }
            }
            return updatesOccurred;
        }

        //Mark old package for removal and download updated package.
        internal void UpdatePackage(IPackage pack)
        {
            if (pack == null) return;
        
            //We used to deactivate the package here so that the new package would be usable immediately.
            //This causes some issues, so now we just mark it for removal. It will be removed next time application is started
            //and the new package will be activated instead.
            var extension = App.GetExtension(pack.Id);

            if (IsPackageInstalled(pack))
                App.MarkPackageForRemoval(GetPackagePath(pack));
            else
            {
                //Make a backup of the extension
                try
                {
                    BackupExtension(extension);
                }
                catch (Exception)
                {
                    DialogResult dialogResult = MessageBox.Show("Unable to make a backup of the extension." +
                    "\n\nDo you want to Update without backing up?", "Backup Error", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        throw new Exception();
                    }
                }
            }

            App.ProgressHandler.Progress(null, 0, "Updating " + pack.Title);

            // get new version
            packages.Update(pack);

            App.ProgressHandler.Progress(null, 0, "");
        }

        private void BackupExtension(IExtension extension)
        {
            string path = GetExtensionPath(extension);
            string directory = "\\" + Path.GetFileName(Path.GetDirectoryName(path)) + "\\";

            //create the backup directory and set the permissions
            string destinationDirectory = Application.StartupPath + "\\backup\\";
            DirectoryInfo di = new DirectoryInfo(destinationDirectory);
            FileSystemAccessRule fsar = new FileSystemAccessRule(
                new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl,
                InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow);
            DirectorySecurity ds = null;

            if (!di.Exists)
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            ds = di.GetAccessControl();
            ds.AddAccessRule(fsar);

            //create the directory of the extension
            destinationDirectory += directory;
            di = new DirectoryInfo(destinationDirectory);
            ds = null;

            if (!di.Exists)
            {
                Directory.CreateDirectory(destinationDirectory);
            }

            ds = di.GetAccessControl();
            ds.AddAccessRule(fsar);

            File.Copy(path, destinationDirectory + Path.GetFileName(path));
            App.MarkExtensionForRemoval(path);
        }

        //Determines if a package has a local version that can be deleted and is out of date.
        private bool IsPackageUpdateable(IPackage pack)
        {
            // Is there a local version?
            var ext = App.GetExtension(pack.Id);
            if (ext == null) return false;

            //Checks if current version is greater than or equal to posted version, return false if true.
            //Assumes that version numbers in assembly files are accurate (not currently true).
            SemanticVersion extVersion = SemanticVersion.Parse(ext.Version);
            if (extVersion >= pack.Version) return false;
            string assemblyLocation = GetExtensionPath(ext);

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

        internal static string GetExtensionPath(IExtension ext)
        {
            var assembly = Assembly.GetAssembly(ext.GetType());
            return assembly.Location;
        }

        internal static bool IsPackageInstalled(IPackage pack)
        {
            string path = GetPackagePath(pack);
            return Directory.Exists(path);
        }

        internal static string GetPackagePath(IPackage pack)
        {
            string path = Path.Combine(AppManager.AbsolutePathToExtensions, AppManager.PackageDirectory);
            return FindPackageFolder(path, pack);
        }

        internal static string FindPackageFolder(string path, IPackage pack)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^^(?!p_|t_).*");
            var folder = Directory.GetDirectories(path, pack.Id + ".*")
                                      .Where(directory => reg.IsMatch(directory))
                                      .ToList();
            if (folder.Count > 0)
                return folder.First().ToString();
            else
                return "NotFound";
        }

        internal static string GetPackageFolderName(IPackage pack)
        {
            return String.Format("{0}.{1}", pack.Id, pack.Version);
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