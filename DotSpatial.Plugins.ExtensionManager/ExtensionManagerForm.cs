// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionManagerForm.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Extensions;
using DotSpatial.Data;
using DotSpatial.Extensions;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// A Dialog that allows the user to manage extensions.
    /// </summary>
    public partial class ExtensionManagerForm : Form
    {
        #region Constants and Fields

        private AppManager _App;
        private const string HideReleaseFromEndUser = "HideReleaseFromEndUser";
        private readonly Packages packages = new Packages();

        #endregion

        #region Constructors and Destructors

        public ExtensionManagerForm()
        {
            InitializeComponent();
            // Databind the check list box to the Name property of extension.
            clbApps.DisplayMember = "Name";

            UpdatePackageList();

            var dataService = packages.Repo as DataServicePackageRepository;
            if (dataService != null)
            {
                dataService.ProgressAvailable += new System.EventHandler<ProgressEventArgs>(dataService_ProgressAvailable);
            }
        }

        #endregion

        #region Public Properties

        public void dataService_ProgressAvailable(object sender, ProgressEventArgs e)
        {
            App.ProgressHandler.Progress(null, e.PercentComplete, "Downloading");
        }

        public AppManager App
        {
            get
            {
                if (_App == null)
                    throw new InvalidOperationException("App must be set to an instance of AppManager before showing ExtensionManagerForm");

                return _App;
            }
            set
            {
                _App = value;
            }
        }

        #endregion

        #region Methods

        private void InstallButton_Click(object sender, EventArgs e)
        {
            if (uxPackages.SelectedItem != null)
            {
                uxInstall.Enabled = false;
                var pack = uxPackages.SelectedItem as IPackage;

                // make a list of all the ext. that are deactivated, do the install, then activate everything,
                // except the list of those that were deactivated.
                // hack: Hope the user doesn't unload extensions while we install.
                var inactiveExtensions = App.Extensions.Where(a => a.IsActive == false).ToArray();

                App.ProgressHandler.Progress(null, 0, "Downloading " + pack.Title);

                var task = Task.Factory.StartNew(() =>
                                                    {
                                                        // Download the extension.
                                                        packages.Install(pack.Id);

                                                        // Load the extension.
                                                        App.RefreshExtensions();
                                                    });

                // UI related work.
                task.ContinueWith((t) =>
                                      {
                                          App.ProgressHandler.Progress(null, 0, "Installing " + pack.Title);

                                          // Activate the extension(s) that was installed.
                                          var extensions = App.Extensions.Where(a => !inactiveExtensions.Contains(a) && a.IsActive == false);

                                          if (extensions.Count() > 0 && !App.EnsureRequiredImportsAreAvailable())
                                              return;

                                          foreach (var item in extensions)
                                          {
                                              item.TryActivate();
                                          }

                                          UpdateApps();
                                          UpdateDataProviders();
                                          App.ProgressHandler.Progress(null, 0, "Ready.");
                                      }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private IPackage GetPackageFromExtension(IExtension extension)
        {
            string id = extension.AssemblyQualifiedName.Substring(0, extension.AssemblyQualifiedName.IndexOf(',')); // Grab the part prior to the first comma
            id = id.Substring(0, id.LastIndexOf('.')); // Grab the part prior to the last period
            var pack = packages.GetLocalPackage(id);
            return pack;
        }

        private IEnumerable<IPackage> GetPackagesDependentOn(IPackage selectedPackage)
        {
            var dependentPackages = new List<IPackage>();
            foreach (var extension in App.Extensions)
            {
                IPackage pack = GetPackageFromExtension(extension);
                if (pack == null)
                {
                    continue;
                }
                foreach (PackageDependency dependency in pack.Dependencies)
                {
                    if (dependency.Id == selectedPackage.Id)
                    {
                        dependentPackages.Add(pack);
                    }
                }
            }
            return dependentPackages;
        }

        private void uxUninstall_Click(object sender, EventArgs e)
        {
            var selectedextension = clbApps.SelectedItem as IExtension;
            IPackage selectedPackage = GetPackageFromExtension(selectedextension);
            if (selectedPackage == null)
            {
                return;
            }
            uxUninstall.Enabled = false;

            IEnumerable<IPackage> dependentPackages = GetPackagesDependentOn(selectedPackage);

            if (dependentPackages.Count() > 0)
            {
                string message = "Removing this extension, will cause the following extensions to be removed as well: ";
                foreach (IPackage dependentPackage in dependentPackages)
                {
                    message += dependentPackage.Id + ", ";
                }

                DialogResult result = MessageBox.Show(message, "Uninstall dependent extensions?", MessageBoxButtons.OKCancel);
                if (result == DialogResult.Cancel)
                {
                    return;
                }

                // Remove dependent packages
                foreach (IPackage dependentPackage in dependentPackages)
                {
                    App.ProgressHandler.Progress(null, 0, "Uninstalling" + dependentPackage.Id);
                    App.EnsureDeactivated(dependentPackage.Id);
                    App.MarkPackageForRemoval(GetPackageFolderName(dependentPackage));
                }
            }

            // Remove the selected package.
            App.ProgressHandler.Progress(null, 0, "Uninstalling" + selectedPackage.Id);
            App.EnsureDeactivated(selectedPackage.Id);
            App.MarkPackageForRemoval(GetPackageFolderName(selectedPackage));

            UpdateApps();
            UpdateDataProviders();
            App.ProgressHandler.Progress(null, 0, "Ready.");

            MessageBox.Show("The extension will finish uninstalling when you restart the application.");
        }

        private void PackageManagerForm_Load(object sender, EventArgs e)
        {
            UpdateApps();
            UpdateDataProviders();
        }

        /// <summary>
        /// Sets up the checkboxes according to information in the manager
        /// </summary>
        private void UpdateApps()
        {
            clbApps.Items.Clear();
            foreach (var extension in App.Extensions.Where(t => t.DeactivationAllowed))
            {
                clbApps.Items.Add(extension, extension.IsActive);
            }
        }

        private void UpdateDataProviders()
        {
            clbData.Items.Clear();
            foreach (IDataProvider provider in App.CompositionContainer.GetExportedValues<IDataProvider>())
            {
                clbData.Items.Add(provider.Name, true);
            }
        }

        IPackage[] originalPackages;

        private void UpdatePackageList()
        {
            uxPackages.Items.Add("Loading...");
            var task = Task.Factory.StartNew(() =>
                                                 {
                                                     var result = from item in packages.Repo.GetPackages()
                                                                  where item.IsLatestVersion && (item.Tags == null || !item.Tags.Contains(HideReleaseFromEndUser))
                                                                  select item;

                                                     return result.ToArray();
                                                 });

            task.ContinueWith((t) =>
                                  {
                                      uxPackages.Items.Clear();
                                      originalPackages = t.Result;
                                      if (t.Exception == null)
                                          uxPackages.Items.AddRange(t.Result);
                                      else
                                          uxPackages.Items.Add(t.Exception.Message);
                                  }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        // This method used to be used to apply all of the changes at once when the dialog was closed.

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clbApps_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var extension = clbApps.Items[e.Index] as IExtension;

            if (e.NewValue == CheckState.Checked && !extension.IsActive)
            {
                extension.TryActivate();
            }
            if (e.NewValue == CheckState.Unchecked && extension.IsActive)
            {
                extension.Deactivate();
            }
        }

        private void uxPackages_SelectedValueChanged(object sender, EventArgs e)
        {
            var pack = uxPackages.SelectedItem as IPackage;
            if (pack != null)
            {
                extensionDescription.Text =
                    String.Format("Created by: {0}\nId:{1}\nVersion:{2}\nDescription:{3}",
                                  StrCat(pack.Authors, ","),
                                  pack.Id,
                                  pack.Version,
                                  pack.Description);

                // For extensions that derive from Extension AssemblyProduct MUST match the Nuspec ID
                // this happens automatically for packages that are build with the packages.nuspec file.
                if (App.GetExtension(pack.Id) == null)
                {
                    uxInstall.Enabled = true;
                    uxUpdate.Enabled = false;
                }
                else
                {
                    uxInstall.Enabled = false;
                    uxUpdate.Enabled = IsPackageUpdateable(pack);
                }
            }
        }

        private bool IsPackageUpdateable(IPackage pack)
        {
            // will we be able to uninstall?
            var ext = App.GetExtension(pack.Id);
            if (ext == null) return false;

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

        private static string GetExtensionPath(IExtension ext)
        {
            var assembly = Assembly.GetAssembly(ext.GetType());
            return assembly.Location;
        }

        private static bool IsPackageInstalled(IPackage pack)
        {
            string path = GetPackagePath(pack);
            return Directory.Exists(path);
        }

        private static string GetPackagePath(IPackage pack)
        {
            string path = Path.Combine(AppManager.AbsolutePathToExtensions, AppManager.PackageDirectory, GetPackageFolderName(pack));
            return path;
        }

        private static string GetPackageFolderName(IPackage pack)
        {
            return String.Format("{0}.{1}", pack.Id, pack.Version);
        }

        /// <summary>
        /// STRs the cat.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        private static string StrCat<T>(IEnumerable<T> source, string separator)
        {
            return source.Aggregate(new StringBuilder(),
                                    (sb, i) => sb
                                                .Append(i.ToString())
                                                .Append(separator),
                                    s => s.ToString());
        }

        private void uxShowExtensionsFolder_Click(object sender, EventArgs e)
        {
            string dir = AppManager.AbsolutePathToExtensions;
            if (Directory.Exists(dir))
                Process.Start(dir);
            else
            {
                MessageBox.Show("The extensions folder does not exist. " + dir);
            }
        }

        private void uxUpdate_Click(object sender, EventArgs e)
        {
            var pack = uxPackages.SelectedItem as IPackage;
            if (pack != null)
            {
                // deactivate the old version and mark for uninstall
                var extension = App.EnsureDeactivated(pack.Id);

                if (IsPackageInstalled(pack))
                {
                    App.MarkPackageForRemoval(GetPackagePath(pack));
                }
                else
                {
                    // todo: consider removing unneeded dependencies.
                    App.MarkExtensionForRemoval(GetExtensionPath(extension));
                }

                App.ProgressHandler.Progress(null, 0, "Updating " + pack.Title);

                // get new version
                packages.Update(pack);

                App.RefreshExtensions();

                // Activate the extension(s) that was installed.
                // it is difficult to determine which version is newest, so we go and look at the when the file was
                // placed on disk.
                var newExtension = App.Extensions.Where(a => a.Name == pack.Id).OrderBy(b => File.GetCreationTime(GetExtensionPath(b))).FirstOrDefault();
                if (newExtension != null)
                {
                    newExtension.Activate();
                }

                UpdateApps();
                UpdateDataProviders();

                App.ProgressHandler.Progress(null, 0, "Ready.");
            }
        }

        #endregion

        private void uxSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void uxClear_Click(object sender, EventArgs e)
        {
            uxSearchText.Clear();
            uxPackages.Items.Clear();
            uxPackages.Items.AddRange(originalPackages);
        }

        private void Search()
        {
            string search = uxSearchText.Text;

            IQueryable<IPackage> results = packages.Repo.Search(search, false);
            uxPackages.Items.Clear();

            var query = from item in results
                        where item.IsLatestVersion == true
                        select item;

            uxPackages.Items.AddRange(query.ToArray());

            if (uxPackages.Items.Count == 0)
            {
                MessageBox.Show("Text not found");
            }
        }

        private void uxSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }
        }

        private void clbApps_SelectedValueChanged(object sender, EventArgs e)
        {
            var extension = clbApps.SelectedItem as IExtension;
            var package = GetPackageFromExtension(extension);

            if (package == null)
            {
                uxUninstall.Enabled = false;
            }
            else
            {
                uxUninstall.Enabled = IsPackageInstalled(package);
            }
        }
    }
}