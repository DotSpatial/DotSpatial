using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using DotSpatial.Controls;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    internal class Update
    {
        private readonly Packages packages;
        private AppManager App;
        private GetPackage getpack;
        private List<IPackage> list = null;
        private const string SupportsAutoUpdate = "SupportsAutoUpdate";

        private bool updateApp = false;
        private string AppName;

        public Update(Packages package, AppManager Appmanager)
        {
            this.packages = package;
            this.App = Appmanager;

            // find name of app
            string name = Assembly.GetEntryAssembly().GetName().Name;
            int i;
            for (i = 0; i < name.Length; i++)
            {
                if (!Char.IsLetter(name[i]))
                    break;
            }
            AppName = name.Substring(0, i);
        }

        /// <summary>
        /// performs auto updates from the default feed.
        /// </summary>
        public static void autoUpdateController(AppManager app)
        {
            Packages packages = new Packages();
            var feeds = FeedManager.getAutoUpdateFeeds();

            if (feeds.Count > 0)
            {
                Update update = new Update(packages, app);
                update.autoUpdate();
            }
        }

        /// <summary>
        /// autoUpdate any packages found in current feed.
        /// </summary>
        internal void autoUpdate()
        {
            var file = Path.Combine(AppManager.AbsolutePathToExtensions, "updates.txt");

            // skip autoUpdating if already done
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception) { }
                return;
            }

            // find updates
            getAllAvailableUpdates();

            if (list != null && list.Count > 0)
            {
                // save packages to update
                string[] updates = new string[list.Count * 2];
                for (int i = 0; i < list.Count; i++)
                {
                    updates[i * 2] = list[i].Id;
                    updates[i * 2 + 1] = list[i].Version.ToString();
                }
                try
                {
                    // don't autoupdate in debug mode
                    if (Debugger.IsAttached)
                        throw new Exception();

                    File.WriteAllLines(file, updates);

                    // open updater
                    var updaterSource = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Updater.exe");
                    if (!File.Exists(updaterSource))
                        throw new Exception();
                    var updaterPath = Path.Combine(AppManager.AbsolutePathToExtensions, "Updater.exe");
                    File.Copy(updaterSource, updaterPath, true);
                    Process updater = new Process();
                    updater.StartInfo.FileName = updaterPath;
                    updater.StartInfo.Arguments = '"' + Assembly.GetEntryAssembly().Location + '"';

                    // elevate privelages if the app needs updating
                    if (updateApp && !IsAdminRole())
                    {
                        updater.StartInfo.UseShellExecute = true;
                        updater.StartInfo.Verb = "runas";
                    }
                    updater.Start();
                    Environment.Exit(0);
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Checks for updates for the main application.
        /// </summary>
        private void CheckUpdateApp()
        {
            // find app package
            var packs = packages.Repo.FindPackagesById(AppName);

            // compare app version
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
        /// Gets updates for the installed packages.
        /// </summary>
        private void getAllAvailableUpdates()
        {
            list = null;

            // Look for packages to be updated in the folder where Extension Manager downloads new packages.
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
            catch (Exception) { }

            if (updates != null)
            {
                List<IPackage> result = new List<IPackage>();
                var sortedPackages = updates.OrderBy(item => item.Id)
                                .ThenByDescending(item => item.Version);

                String id = "";
                foreach (var package in sortedPackages)
                {
                    if (id != package.Id && package.Tags != null && package.Tags.Contains(SupportsAutoUpdate))
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
    }
}