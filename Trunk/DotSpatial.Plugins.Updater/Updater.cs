using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NuGet;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Security.Principal;

namespace DotSpatial.Plugins.Updater
{
    public partial class Updater : Form
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        private const string HideReleaseFromEndUser = "HideReleaseFromEndUser";
        private const string HideFromAutoUpdate = "HideFromAutoUpdate";
        private const int ScrollBarMargin = 25;
        private readonly ListViewHelper Add = new ListViewHelper();
        private readonly Packages packages;
        private string PackagePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Packages");
        private string AppPath = null;

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

        private void Form_Load(object sender, EventArgs e)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var localUpdateable = GetLocalUpdateable();

                if (localUpdateable != null || localUpdateable.Length > 0)
                    performUpdates(localUpdateable);
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

        private void performUpdates(string[] updates)
        {
            List<string> updated = new List<string>();

            for (int i = 0; i < updates.Length/2; i++)
            {
                UpdatesLabel.Invoke((Action)(() =>
                {
                    UpdatesLabel.Text = string.Format("Performing Update {0} of {1}, {2}.", i * 2 + 1, 
                        updates.Length/2, updates[i * 2].Substring(updates[i * 2].LastIndexOf('.') + 1));
                }));

                try
                {
                    if (IsAdminRole() && i == 0)
                        UpdateApp(updates[i * 2], updates[i * 2 + 1]);
                    else
                        UpdatePackage(updates[i * 2], updates[i * 2 + 1]);
                    updated.Insert(0, updates[i * 2]);
                    Add.AddPackages(updated, uxUpdates, 0);
                }
                catch(Exception){}
            }

            UpdatesLabel.Invoke((Action)(() =>
            {
                UpdatesLabel.Text = "All Updates Completed";
            }));
        }

        private void UpdatePackage(string id, string version)
        {
            packages.Update(id, SemanticVersion.Parse(version));
        }

        private void UpdateApp(string id, string version)
        {
            var path = Path.GetDirectoryName(AppPath);
            if (path.Contains(id))
            {
                packages.Manager.InstallPackage(id, SemanticVersion.Parse(version), true, false);
                File.Copy(Path.Combine(path, "unins000.exe"), Path.Combine(PackagePath, id + '.' + version, "lib", "unins000.exe"), true);
                File.Copy(Path.Combine(path, "unins000.exe"), Path.Combine(PackagePath, id + '.' + version, "lib", "unins000.dat"), true);
                Directory.Delete(path, true);
                Directory.Move(Path.Combine(PackagePath, id + '.' + version, "lib"), path);
                Directory.Delete(Path.Combine(PackagePath, id + '.' + version), true);
            }
            else
                throw new Exception();
        }

        private void SetTheme(object sender, EventArgs e)
        {
            SetWindowTheme(((ListView)sender).Handle, "Explorer", null);
        }
    }
}
