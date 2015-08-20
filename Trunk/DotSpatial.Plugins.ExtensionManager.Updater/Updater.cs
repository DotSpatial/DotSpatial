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
    public partial class Updater : Form
    {
        #region Constants and Fields

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        private const string HideReleaseFromEndUser = "HideReleaseFromEndUser";
        private const string HideFromAutoUpdate = "HideFromAutoUpdate";
        private const string ExtensionManager = "DotSpatial.Plugins.ExtensionManager";
        private const int ScrollBarMargin = 25;
        private readonly ListViewHelper Add = new ListViewHelper();
        private readonly Packages packages;
        private string PackagePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Packages");
        private string AppPath = null;

        #endregion

        #region Constructors and Destructors

        public Updater(string[] args)
        {
            InitializeComponent();
            Load += Form_Load;
            uxUpdates.HandleCreated += SetTheme;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            packages = new Packages(PackagePath);
            uxUpdates.TileSize = new Size(uxUpdates.Width - ScrollBarMargin, 22);

            if (args.Length > 0)
                AppPath = args[0];
        }

        #endregion

        #region Events

        private void Form_Load(object sender, EventArgs e)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var localUpdateable = GetLocalUpdateable();

                if (localUpdateable != null || localUpdateable.Length > 0)
                    PerformUpdates(localUpdateable);
            });

            task.ContinueWith(t =>
            {
                if (AppPath != null && File.Exists(AppPath))
                {
                    Process app = new Process();
                    app.StartInfo.FileName = AppPath;
                    app.Start();
                }
                Application.Exit();
            });
        }

        private void SetTheme(object sender, EventArgs e)
        {
            SetWindowTheme(((ListView)sender).Handle, "Explorer", null);
        }

        #endregion

        #region Private Methods

        private string[] GetLocalUpdateable()
        {
            try
            {
                var file = Path.Combine(Path.GetDirectoryName(PackagePath), "updates.txt");
                string[] updates = File.ReadAllLines(file);
                return updates;
            }
            catch (Exception) { }

            return null;
        }

        private bool IsAdminRole()
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

        private void PerformUpdates(string[] updates)
        {
            List<string> updated = new List<string>();

            if (updates.Contains(ExtensionManager) && updates.Length != 2)
                SelfUpdate(updates);

            for (int i = 0; i < updates.Length/2; i++)
            {
                UpdatesLabel.Invoke((Action)(() =>
                {
                    UpdatesLabel.Text = string.Format("Performing Update {0} of {1}, {2}.", i + 1, 
                        updates.Length/2, updates[i * 2].Substring(updates[i * 2].LastIndexOf('.') + 1));
                }));

                try
                {
                    if (IsAdminRole() && i == 0 && !updates[i].ToLowerInvariant().Contains("plugins"))
                        UpdateApp(updates[i * 2], updates[i * 2 + 1]);
                    else
                        UpdatePackage(updates[i * 2], updates[i * 2 + 1]);
                    updated.Insert(0, updates[i * 2]);
                    Add.AddChecked(updated, uxUpdates);
                }
                catch(Exception){}
            }

            UpdatesLabel.Invoke((Action)(() =>
            {
                UpdatesLabel.Text = "All Updates Completed";
            }));
        }

        private void SelfUpdate(string[] updates)
        {
            List<string> updated = new List<string>();
            var updateList = updates.ToList();

            for (int i = 0; i < updates.Length / 2; i++)
            {
                if (updates[i * 2] != ExtensionManager)
                    continue;

                UpdatesLabel.Invoke((Action)(() =>
                {
                    UpdatesLabel.Text = string.Format("Performing Update {0} of {1}, {2}.", 1,
                        1, updates[i * 2].Substring(updates[i * 2].LastIndexOf('.') + 1));
                }));

                try
                {
                    //perform update
                    UpdatePackage(updates[i * 2], updates[i * 2 + 1]);
                    updated.Insert(0, updates[i * 2]);
                    Add.AddChecked(updated, uxUpdates);

                    //copy new updater to temp location
                    var UpdaterPath = Path.GetDirectoryName(PackagePath);
                    String[] files = Directory.GetFiles(PackagePath, "Updater.exe", SearchOption.AllDirectories);
                    File.Copy(files[0], Path.Combine(UpdaterPath, "Updater(1).exe"), true);

                    //remove current update and save updates
                    updateList.RemoveRange(i * 2, 2);
                    File.WriteAllLines(Path.Combine(UpdaterPath, "updates.txt"), updateList);

                    //start new updater
                    Process updater = new Process();
                    updater.StartInfo.FileName = Path.Combine(UpdaterPath, "Updater(1).exe");
                    updater.StartInfo.Arguments = '"' + AppPath + '"';
                    updater.Start();
                    Application.Exit();
                }
                catch (Exception) { }
            }
        }

        private void UpdatePackage(string id, string version)
        {
            packages.Update(id, SemanticVersion.Parse(version));
        }

        private void UpdateApp(string id, string version)
        {
            var path = Path.GetDirectoryName(AppPath);
            var source = Path.Combine(PackagePath, id + '.' + version, "lib");

            if (path.Contains(id))
            {
                //install new app package
                packages.Manager.InstallPackage(id, SemanticVersion.Parse(version), true, false);

                //copy over essential files
                if(File.Exists(Path.Combine(path, "unins000.exe")))
                    File.Copy(Path.Combine(path, "unins000.exe"), Path.Combine(source, "unins000.exe"), true);
                if (File.Exists(Path.Combine(path, "unins000.dat")))
                    File.Copy(Path.Combine(path, "unins000.dat"), Path.Combine(source, "unins000.dat"), true);
                if (Directory.Exists(Path.Combine(path, "Plugins")))
                    Directory.Move(Path.Combine(path, "Plugins"), Path.Combine(source, "Plugins"));
                if (Directory.Exists(Path.Combine(path, "Application Extensions")))
                    Directory.Move(Path.Combine(path, "Application Extensions"), Path.Combine(source, "Application Extensions"));
                if (Directory.Exists(Path.Combine(path, "Windows Extensions")))
                    Directory.Move(Path.Combine(path, "Windows Extensions"), Path.Combine(source, "Windows Extensions"));
                if (Directory.Exists(Path.Combine(path, "Mono Extensions")))
                    Directory.Move(Path.Combine(path, "Mono Extensions"), Path.Combine(source, "Mono Extensions"));

                //perform update
                Directory.Delete(path, true);
                Directory.Move(source, path);
                Directory.Delete(Path.Combine(PackagePath, id + '.' + version), true);
            }
            else
                throw new Exception();
        }

        #endregion
    }
}
