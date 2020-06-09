// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager.Updater
{
    /// <summary>
    /// Updater.
    /// </summary>
    public partial class Updater : Form
    {
        #region Fields

        private const string ExtensionManager = "DotSpatial.Plugins.ExtensionManager";
        private const int ScrollBarMargin = 25;
        private readonly ListViewHelper _add = new ListViewHelper();
        private readonly string _appPath;
        private readonly string _packagePath;
        private readonly Packages _packages;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Updater"/> class.
        /// </summary>
        /// <param name="args">The path of the application.</param>
        public Updater(string[] args)
        {
            InitializeComponent();

            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (path != null) _packagePath = Path.Combine(path, "Packages");

            Load += FormLoad;
            uxUpdates.HandleCreated += SetTheme;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            _packages = new Packages(_packagePath);
            uxUpdates.TileSize = new Size(uxUpdates.Width - ScrollBarMargin, 22);

            if (args.Length > 0) _appPath = args[0];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the window theme.
        /// </summary>
        /// <param name="hWnd">The handle.</param>
        /// <param name="pszSubAppName">The sub app name.</param>
        /// <param name="pszSubIdList">The sub id list.</param>
        /// <returns>An integer.</returns>
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        private static bool IsAdminRole()
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

        private static void SetTheme(object sender, EventArgs e)
        {
            SetWindowTheme(((ListView)sender).Handle, "Explorer", null);
        }

        private void FormLoad(object sender, EventArgs e)
        {
            var task = Task.Factory.StartNew(
                () =>
                {
                    var localUpdateable = GetLocalUpdateable();

                    if (localUpdateable != null || localUpdateable.Length > 0) PerformUpdates(localUpdateable);
                });

            task.ContinueWith(
                t =>
                {
                    if (_appPath != null && File.Exists(_appPath))
                    {
                        Process app = new Process
                                      {
                                          StartInfo =
                                          {
                                              FileName = _appPath
                                          }
                                      };
                        app.Start();
                    }

                    Application.Exit();
                });
        }

        private string[] GetLocalUpdateable()
        {
            try
            {
                var file = Path.Combine(Path.GetDirectoryName(_packagePath), "updates.txt");
                string[] updates = File.ReadAllLines(file);
                return updates;
            }
            catch (Exception)
            {
            }

            return null;
        }

        private void PerformUpdates(string[] updates)
        {
            List<string> updated = new List<string>();

            if (updates.Contains(ExtensionManager) && updates.Length != 2) SelfUpdate(updates);

            for (int i = 0; i < updates.Length / 2; i++)
            {
                UpdatesLabel.Invoke((Action)(() => { UpdatesLabel.Text = string.Format("Performing Update {0} of {1}, {2}.", i + 1, updates.Length / 2, updates[i * 2].Substring(updates[i * 2].LastIndexOf('.') + 1)); }));

                try
                {
                    if (IsAdminRole() && i == 0 && !updates[i].ToLowerInvariant().Contains("plugins")) UpdateApp(updates[i * 2], updates[(i * 2) + 1]);
                    else UpdatePackage(updates[i * 2], updates[(i * 2) + 1]);
                    updated.Insert(0, updates[i * 2]);
                    _add.AddChecked(updated, uxUpdates);
                }
                catch (Exception)
                {
                }
            }

            UpdatesLabel.Invoke((Action)(() => { UpdatesLabel.Text = "All Updates Completed"; }));
        }

        private void SelfUpdate(string[] updates)
        {
            List<string> updated = new List<string>();
            var updateList = updates.ToList();

            for (int i = 0; i < updates.Length / 2; i++)
            {
                if (updates[i * 2] != ExtensionManager) continue;

                UpdatesLabel.Invoke((Action)(() => { UpdatesLabel.Text = string.Format("Performing Update {0} of {1}, {2}.", 1, 1, updates[i * 2].Substring(updates[i * 2].LastIndexOf('.') + 1)); }));

                try
                {
                    // perform update
                    UpdatePackage(updates[i * 2], updates[(i * 2) + 1]);
                    updated.Insert(0, updates[i * 2]);
                    _add.AddChecked(updated, uxUpdates);

                    // copy new updater to temp location
                    var updaterPath = Path.GetDirectoryName(_packagePath);
                    string[] files = Directory.GetFiles(_packagePath, "Updater.exe", SearchOption.AllDirectories);
                    File.Copy(files[0], Path.Combine(updaterPath, "Updater(1).exe"), true);

                    // remove current update and save updates
                    updateList.RemoveRange(i * 2, 2);
                    File.WriteAllLines(Path.Combine(updaterPath, "updates.txt"), updateList);

                    // start new updater
                    Process updater = new Process
                                      {
                                          StartInfo =
                                          {
                                              FileName = Path.Combine(updaterPath, "Updater(1).exe"),
                                              Arguments = '"' + _appPath + '"'
                                          }
                                      };
                    updater.Start();
                    Application.Exit();
                }
                catch (Exception)
                {
                }
            }
        }

        private void UpdateApp(string id, string version)
        {
            var path = Path.GetDirectoryName(_appPath);
            var source = Path.Combine(_packagePath, id + '.' + version, "lib");

            if (path.Contains(id))
            {
                // install new app package
                _packages.Manager.InstallPackage(id, SemanticVersion.Parse(version), true, false);

                // copy over essential files
                if (File.Exists(Path.Combine(path, "unins000.exe"))) File.Copy(Path.Combine(path, "unins000.exe"), Path.Combine(source, "unins000.exe"), true);
                if (File.Exists(Path.Combine(path, "unins000.dat"))) File.Copy(Path.Combine(path, "unins000.dat"), Path.Combine(source, "unins000.dat"), true);
                if (Directory.Exists(Path.Combine(path, "Plugins"))) Directory.Move(Path.Combine(path, "Plugins"), Path.Combine(source, "Plugins"));
                if (Directory.Exists(Path.Combine(path, "Application Extensions"))) Directory.Move(Path.Combine(path, "Application Extensions"), Path.Combine(source, "Application Extensions"));
                if (Directory.Exists(Path.Combine(path, "Windows Extensions"))) Directory.Move(Path.Combine(path, "Windows Extensions"), Path.Combine(source, "Windows Extensions"));
                if (Directory.Exists(Path.Combine(path, "Mono Extensions"))) Directory.Move(Path.Combine(path, "Mono Extensions"), Path.Combine(source, "Mono Extensions"));

                // perform update
                Directory.Delete(path, true);
                Directory.Move(source, path);
                Directory.Delete(Path.Combine(_packagePath, id + '.' + version), true);
            }
            else
            {
                throw new Exception();
            }
        }

        private void UpdatePackage(string id, string version)
        {
            _packages.Update(id, SemanticVersion.Parse(version));
        }

        #endregion
    }
}