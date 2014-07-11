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
using System.Diagnostics;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class Update
    {
        public Update(Packages package, AppManager Appmanager)
        {
            this.packages = package;
            this.App = Appmanager;
        }

        private readonly Packages packages;
        private AppManager App;
        private GetPackage getpack;
        private List<IPackage> list = null;
        private const string HideReleaseFromEndUser = "HideReleaseFromEndUser";
        private const string HideFromAutoUpdate = "HideFromAutoUpdate";
        private bool updateApp = false;

        /// <summary>
        /// Gets updates for the installed packages.
        /// </summary>
        private void getAllAvailableUpdates()
        {
            list = null;

            //Look for packages to be updated in the folder where Extension Manager downloads new packages.
            getpack = new GetPackage(packages);
            IEnumerable<IPackage> localPackages = getpack.GetPackagesFromExtensions(App.Extensions);

            if (localPackages.Count() > 0)
            {
                getAvailableUpdatesFromLocal(localPackages);
            }
        }

        /// <summary>
        /// Finds all available updates.
        /// </summary>
        private void getAvailableUpdatesFromLocal(IEnumerable<IPackage> localPackages)
        {
            IEnumerable<IPackage> updates = null;
            try
            {
                updates = packages.Repo.GetUpdates(localPackages, false, true);
            }
            catch (Exception){}

            if (updates != null || updates.Count() < 1)
            {
                List<IPackage> result = new List<IPackage>();
                var sortedPackages = updates.OrderBy(item => item.Id)
                                .ThenByDescending(item => item.Version);

                String id = "";

                foreach (var package in sortedPackages)
                {
                    if (id != package.Id && (package.Tags == null || (!package.Tags.Contains(HideReleaseFromEndUser) 
                        && !package.Tags.Contains(HideFromAutoUpdate))))
                    {
                        result.Add(package);
                        id = package.Id;
                    }
                }

                list = result;
            }
            try
            {
                CheckUpdateApp();
            }
            catch (Exception) { }

        }

        /// <summary>
        /// Checks for updates for the main application.
        /// </summary>
        private void CheckUpdateApp()
        {
            if (System.Reflection.Assembly.GetEntryAssembly().FullName.Contains("_dev"))
                throw new Exception();

            //find app name
            string name = Assembly.GetEntryAssembly().GetName().Name;
            int i;
            for (i = 0; i < name.Length; i++)
            {
                if (!Char.IsLetter(name[i]))
                    break;
            }
            name = name.Substring(0, i);

            //find app package
            var packs = packages.Repo.FindPackagesById(name);

            //compare app version
            var programVersion = SemanticVersion.Parse(Assembly.GetEntryAssembly().GetName().Version.ToString());
            foreach (var pack in packs.Reverse())
            {
                if (pack != null && pack.Version > programVersion && IsSameRelease(pack.Version.Version, programVersion.Version))
                {
                    list.Insert(0, pack);
                    updateApp = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Checks if the user is running as an admin.
        /// </summary>
        public static bool IsSameRelease(Version version1, Version version2)
        {
            if (version1.Major != version2.Major)
                return false;
            if (version1.Minor != version2.Minor)
                return false;
            return true;
        }

        /// <summary>
        /// Checks if the user is running as an admin.
        /// </summary>
        public static bool IsAdminRole()
        {
            try
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// performs auto updates from the default feed.
        /// </summary>
        public static void autoUpdateController(AppManager app)
        {
            Packages packages = new Packages();
            System.Collections.Specialized.StringCollection feeds = FeedManager.getAutoUpdateFeeds();
            if (feeds.Count > 0)
            {
                Update update = new Update(packages, app);
                update.autoUpdate();
            }
        }

        /// <summary>
        ///autoUpdate any packages found in current feed.
        /// </summary>
        internal void autoUpdate()
        {
            var file = Path.Combine(AppManager.AbsolutePathToExtensions, "updates.txt");

            //skip autoUpdating if already done
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch(Exception) { }
                return;
            }

            //find updates
            getAllAvailableUpdates();
            
            if (list != null && list.Count > 0)
            {
                MessageBox.Show("hello");
                //save packages to update
                string[] updates = new string[list.Count * 2];
                for (int i = 0; i < list.Count; i++)
                {
                    updates[i * 2] = list[i].Id;
                    updates[i * 2 + 1] = list[i].Version.ToString();
                }
                try
                {
                    File.WriteAllLines(file, updates);

                    //open updater
                    var updaterPath = Path.Combine(AppManager.AbsolutePathToExtensions, "DotSpatial.Updater.exe");
                    if (File.Exists(updaterPath))
                    {
                        Process updater = new Process();
                        updater.StartInfo.FileName = updaterPath;
                        updater.StartInfo.Arguments = '"' + System.Reflection.Assembly.GetEntryAssembly().Location + '"';

                        if (updateApp && !IsAdminRole())
                        {
                            updater.StartInfo.UseShellExecute = true;
                            updater.StartInfo.Verb = "runas";
                        }
                        updater.Start();
                        Environment.Exit(0);
                    }
                }
                catch (Exception e) { }
            }
        }
    }
}