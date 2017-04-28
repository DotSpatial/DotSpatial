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
    /// <summary>
    /// Update
    /// </summary>
    internal class Update
    {
        #region Fields

        private const string SupportsAutoUpdate = "SupportsAutoUpdate";
        private readonly AppManager _app;
        private readonly string _appName;
        private readonly Packages _packages;
        private GetPackage _getpack;
        private List<IPackage> _list;

        private bool _updateApp;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Update"/> class.
        /// </summary>
        /// <param name="package">Packages that get updated.</param>
        /// <param name="appmanager">The AppManager.</param>
        public Update(Packages package, AppManager appmanager)
        {
            _packages = package;
            _app = appmanager;

            // find name of app
            string name = Assembly.GetEntryAssembly().GetName().Name;
            int i;
            for (i = 0; i < name.Length; i++)
            {
                if (!char.IsLetter(name[i]))
                    break;
            }

            _appName = name.Substring(0, i);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs auto updates from the default feed.
        /// </summary>
        /// <param name="app">The AppManager.</param>
        public static void AutoUpdateController(AppManager app)
        {
            Packages packages = new Packages();
            var feeds = FeedManager.GetAutoUpdateFeeds();

            if (feeds.Count > 0)
            {
                Update update = new Update(packages, app);
                update.AutoUpdate();
            }
        }

        /// <summary>
        /// Checks if the user is running as an admin.
        /// </summary>
        /// <returns>True, if the user is admin.</returns>
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
        /// Checks if the to versions are equal in their minor and major version.
        /// </summary>
        /// <param name="version1">First version to check.</param>
        /// <param name="version2">Second version to check.</param>
        /// <returns>True, if major and minor are the same.</returns>
        public static bool IsSameRelease(Version version1, Version version2)
        {
            return version1.Major == version2.Major && version1.Minor == version2.Minor;
        }

        /// <summary>
        /// Auto update any packages found in current feed.
        /// </summary>
        internal void AutoUpdate()
        {
            var file = Path.Combine(AppManager.AbsolutePathToExtensions, "updates.txt");

            // skip autoUpdating if already done
            if (File.Exists(file))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception)
                {
                }

                return;
            }

            // find updates
            GetAllAvailableUpdates();

            if (_list != null && _list.Count > 0)
            {
                // save packages to update
                string[] updates = new string[_list.Count * 2];
                for (int i = 0; i < _list.Count; i++)
                {
                    updates[i * 2] = _list[i].Id;
                    updates[i * 2 + 1] = _list[i].Version.ToString();
                }

                try
                {
                    // don't autoupdate in debug mode
                    if (Debugger.IsAttached)
                        throw new Exception();

                    File.WriteAllLines(file, updates);

                    // open updater
                    var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    if (path == null) Environment.Exit(-1);

                    var updaterSource = Path.Combine(path, "Updater.exe");
                    if (!File.Exists(updaterSource))
                        throw new Exception();
                    var updaterPath = Path.Combine(AppManager.AbsolutePathToExtensions, "Updater.exe");
                    File.Copy(updaterSource, updaterPath, true);
                    Process updater = new Process
                    {
                        StartInfo =
                        {
                            FileName = updaterPath,
                            Arguments = '"' + Assembly.GetEntryAssembly().Location + '"'
                        }
                    };

                    // elevate privelages if the app needs updating
                    if (_updateApp && !IsAdminRole())
                    {
                        updater.StartInfo.UseShellExecute = true;
                        updater.StartInfo.Verb = "runas";
                    }

                    updater.Start();
                    Environment.Exit(0);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Checks for updates for the main application.
        /// </summary>
        private void CheckUpdateApp()
        {
            // find app package
            var packs = _packages.Repo.FindPackagesById(_appName);

            // compare app version
            var programVersion = SemanticVersion.Parse(Assembly.GetEntryAssembly().GetName().Version.ToString());
            foreach (var pack in packs.Reverse())
            {
                if (pack != null && pack.Version > programVersion && IsSameRelease(pack.Version.Version, programVersion.Version))
                {
                    _list.Insert(0, pack);
                    _updateApp = true;
                    break;
                }
            }
        }

        /// <summary>
        /// Gets updates for the installed packages.
        /// </summary>
        private void GetAllAvailableUpdates()
        {
            _list = null;

            // Look for packages to be updated in the folder where Extension Manager downloads new packages.
            _getpack = new GetPackage(_packages);
            var localPackages = _getpack.GetPackagesFromExtensions(_app.Extensions).ToList();

            if (localPackages.Count > 0)
            {
                GetAvailableUpdatesFromLocal(localPackages);
            }
        }

        /// <summary>
        /// Finds all available updates.
        /// </summary>
        /// <param name="localPackages">Packages to check for update.</param>
        private void GetAvailableUpdatesFromLocal(IEnumerable<IPackage> localPackages)
        {
            IEnumerable<IPackage> updates = null;
            try
            {
                updates = _packages.Repo.GetUpdates(localPackages, false, true);
            }
            catch (Exception)
            {
            }

            if (updates != null)
            {
                List<IPackage> result = new List<IPackage>();
                var sortedPackages = updates.OrderBy(item => item.Id).ThenByDescending(item => item.Version);

                string id = string.Empty;
                foreach (var package in sortedPackages)
                {
                    if (id != package.Id && package.Tags != null && package.Tags.Contains(SupportsAutoUpdate))
                    {
                        result.Add(package);
                        id = package.Id;
                    }
                }

                _list = result;
            }

            try
            {
                CheckUpdateApp();
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}