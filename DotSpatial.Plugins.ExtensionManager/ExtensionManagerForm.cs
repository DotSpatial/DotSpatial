// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionManagerForm.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Extensions;
using DotSpatial.Data;
using DotSpatial.Extensions;
using DotSpatial.Modeling.Forms;
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
        private readonly PackageListHelper packs;
        private readonly ListViewHelper Add = new ListViewHelper();
        private const int ScrollBarMargin = 25;

        #endregion

        #region Constructors and Destructors

        public ExtensionManagerForm()
        {
            InitializeComponent();

            packs = new PackageListHelper(packages);

            // Databind the check list box to the Name property of extension.
            uxCategoryList.DisplayMember = "Name";

            var dataService = packages.Repo as DataServicePackageRepository;
            if (dataService != null)
            {
                dataService.ProgressAvailable += new System.EventHandler<ProgressEventArgs>(dataService_ProgressAvailable);
            }

            uxPackages.TileSize = new Size(uxPackages.Width - ScrollBarMargin, 45);
            uxUpdatePackages.TileSize = new Size(uxPackages.Width - ScrollBarMargin, 45);

            // Select the first item in the drop down, kicking off a package list update.
            uxFeedSelection.SelectedIndex = 0;
        }

        #endregion

        #region Public Properties

        public void dataService_ProgressAvailable(object sender, ProgressEventArgs e)
        {
            App.ProgressHandler.Progress(null, e.PercentComplete, "Downloading");
            if (uxInstallProgress.InvokeRequired)
            {
                uxInstallProgress.Invoke((Action)(() => { uxInstallProgress.Value = e.PercentComplete; }));
            }
            else
            {
                uxInstallProgress.Value = e.PercentComplete;
            }
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
            if (uxPackages.SelectedItems.Count < 1)
            {
                return;
            }
            uxDownloadStatus.Clear();
            uxInstallProgress.Value = 0;
            uxInstallProgress.Visible = true;
            uxDownloadStatus.Visible = true;
            uxInstall.Enabled = false;

            var pack = uxPackages.SelectedItems[0].Tag as IPackage;
            if (pack == null)
            {
                return;
            }
            // make a list of all the ext. that are deactivated, do the install, then activate everything,
            // except the list of those that were deactivated.
            // hack: Hope the user doesn't unload extensions while we install.
            var inactiveExtensions = App.Extensions.Where(a => a.IsActive == false).ToArray();

            App.ProgressHandler.Progress(null, 0, "Downloading " + pack.Title);
            uxDownloadStatus.Text = "Downloading " + pack.Id;

            var task = Task.Factory.StartNew(() =>
                                                {
                                                    IEnumerable<PackageDependency> dependency = pack.Dependencies;
                                                    if (dependency.Count() > 0)
                                                    {
                                                        MessageBox.Show("Downloading the dependencies", "Downloading", MessageBoxButtons.OK);

                                                        foreach (PackageDependency dependentPackage in dependency)
                                                        {
                                                            packages.Install(dependentPackage.Id);
                                                            App.ProgressHandler.Progress(null, 0, "Downloading " + dependentPackage.Id);

                                                            if (uxDownloadStatus.InvokeRequired)
                                                            {
                                                                uxDownloadStatus.Invoke((Action)(() => { uxDownloadStatus.Text = "Downloading" + dependentPackage.Id; }));
                                                            }
                                                            else
                                                            {
                                                                uxDownloadStatus.Text = "Downloading" + dependentPackage.Id;
                                                            }
                                                        }
                                                    }
                                                    // Download the extension.
                                                    packages.Install(pack.Id);

                                                    // Load the extension.
                                                    App.RefreshExtensions();
                                                });

            // UI related work.
            task.ContinueWith((t) =>
                                  {
                                      IEnumerable<PackageDependency> dependency = pack.Dependencies;
                                      App.ProgressHandler.Progress(null, 0, "Installing " + pack.Title);

                                      // Activate the extension(s) that was installed.
                                      var extensions = App.Extensions.Where(a => !inactiveExtensions.Contains(a) && a.IsActive == false);

                                      if (extensions.Count() > 0 && !App.EnsureRequiredImportsAreAvailable())
                                          return;

                                      foreach (var item in extensions)
                                      {
                                          item.TryActivate();
                                      }

                                      // hack: we should really try to refresh the list, using what ever category the user
                                      // has selected.
                                      Installed.Items.Clear();
                                      App.ProgressHandler.Progress(null, 0, "Ready.");
                                      uxInstallProgress.Visible = false;
                                      string message = "The following packages are installed: " + pack.Title;

                                      foreach (PackageDependency dependentPackage in dependency)
                                      {
                                          message += dependentPackage.Id + " , ";
                                      }
                                      uxDownloadStatus.Text = message;
                                  }, TaskScheduler.FromCurrentSynchronizationContext());
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
            var selectedextension = Installed.SelectedItem as IExtension;
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

            // hack: we should really try to refresh the list, using what ever category the user
            // has selected.
            App.ProgressHandler.Progress(null, 0, "Ready.");

            MessageBox.Show("The extension will finish uninstalling when you restart the application.");
        }

        private void AddCategory(IExtensionCategory category)
        {
            category.App = this.App;
            uxCategoryList.Items.Add(category);
        }

        private void PackageManagerForm_Load(object sender, EventArgs e)
        {
            AddCategory(new ExtensionCategory());
            AddCategory(new ToolsCategory());
            AddCategory(new DataProviderCategory());
            AddCategory(new ApplicationExtensionCategory());

            uxDownloadStatus.Visible = false;
            uxInstallProgress.Visible = false;
        }

        private IEnumerable<IPackage> GetPackagesFromExtensions(IEnumerable<IExtension> extensions)
        {
            foreach (IExtension extension in extensions)
            {
                var package = GetPackageFromExtension(extension);
                if (package != null)
                {
                    yield return package;
                }
            }
        }

        private void RefreshUpdateList()
        {
            IEnumerable<IPackage> localPackages = GetPackagesFromExtensions(App.Extensions);
            IEnumerable<IPackage> list = packages.Repo.GetUpdates(localPackages, false, false);
            if (uxUpdatePackages.InvokeRequired)
            {
                uxUpdatePackages.Invoke((Action)(() => { Add.AddPackages(list, uxUpdatePackages); }));
            }
            else
            {
                Add.AddPackages(list, uxUpdatePackages);
            }
        }

        private void Installed_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            IExtension extension = Installed.Items[e.Index] as IExtension;
            if (extension == null)
            {
                return;
            }

            if (e.NewValue == CheckState.Checked && !extension.IsActive)
            {
                extension.TryActivate();
            }
            if (e.NewValue == CheckState.Unchecked && extension.IsActive)
            {
                extension.Deactivate();
            }
        }

        private void Installed_SelectedValueChanged(object sender, EventArgs e)
        {
            IExtension extension = Installed.SelectedItem as IExtension;

            if (extension == null)
            {
                uxUninstall.Enabled = false;
            }
            else
            {
                var package = GetPackageFromExtension(extension);

                if (package == null)
                {
                    uxUninstall.Enabled = false;
                }
                else
                {
                    uxUninstall.Enabled = true;
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

        #endregion

        private void uxSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void uxClear_Click(object sender, EventArgs e)
        {
            uxSearchText.Clear();
            packs.DisplayPackages(uxPackages);
        }

        private void Search()
        {
            string search = uxSearchText.Text;

            IQueryable<IPackage> results = packages.Repo.Search(search, false);

            var query = from item in results
                        where item.IsLatestVersion == true
                        select item;

            Add.AddPackages(query, uxPackages);

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

        private void uxCategoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var category = uxCategoryList.SelectedItem as IExtensionCategory;
            if (category != null)
            {
                Installed.Items.Clear();
                foreach (var item in category.GetItems())
                {
                    Installed.Items.Add(item.Item1, item.Item2);
                }
            }
            else
            {
                if ((String)uxCategoryList.SelectedItem == "All")
                {
                    Installed.Items.Clear();

                    foreach (var item in uxCategoryList.Items)
                    {
                        var cat = item as IExtensionCategory;
                        // The "All" string will not be a IExtensionCategory and needs to be skipped.
                        if (cat == null) continue;
                        foreach (var catItem in cat.GetItems())
                        {
                            Installed.Items.Add(catItem.Item1, catItem.Item2);
                        }
                    }
                }
            }
        }

        private void OnFeedChanged()
        {
            string feedUrl = uxFeedSelection.SelectedItem.ToString();
            packages.SetNewSource(feedUrl);
            packs.DisplayPackages(uxPackages);
            Task.Factory.StartNew(() =>
            {
                RefreshUpdateList();
            });
        }

        public void uxFeedSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnFeedChanged();
        }

        private void SelectedItemChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (uxPackages.SelectedItems.Count == 0)
            {
                return;
            }

            var pack = uxPackages.SelectedItems[0].Tag as IPackage;
            if (pack != null)
            {
                System.Drawing.Font currentFont = richTextBox1.SelectionFont;

                richTextBox1.Clear();
                richTextBox1.SelectionColor = Color.Gray;
                richTextBox1.AppendText("Created by: ");
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.AppendText(StrCat(pack.Authors, ","));

                richTextBox1.SelectionColor = Color.Gray;
                richTextBox1.AppendText(Environment.NewLine + Environment.NewLine + "Id: ");
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.AppendText(pack.Id);

                richTextBox1.SelectionColor = Color.Gray;
                richTextBox1.AppendText(Environment.NewLine + Environment.NewLine + "Version: ");
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.AppendText(pack.Version.ToString());

                richTextBox1.SelectionColor = Color.Gray;
                richTextBox1.AppendText(Environment.NewLine + Environment.NewLine + "Description: ");
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.AppendText(pack.Description);

                // For extensions that derive from Extension AssemblyProduct MUST match the Nuspec ID
                // this happens automatically for packages that are build with the packages.nuspec file.
                if (App.GetExtension(pack.Id) == null)
                {
                    uxInstall.Enabled = true;
                    //    uxUpdate.Enabled = false;
                }
                else
                {
                    uxInstall.Enabled = false;
                    //  uxUpdate.Enabled = IsPackageUpdateable(pack);
                }
            }
        }

        private void uxUpdate_Click(object sender, EventArgs e)
        {
            if (uxUpdatePackages.SelectedItems.Count < 1)
            {
                return;
            }
            uxUpdate.Enabled = false;
            var pack = uxUpdatePackages.SelectedItems[0].Tag as IPackage;
            Update(pack);
        }

        private void uxUpdateAll_Click(object sender, EventArgs e)
        {
            int i;
            for (i = 0; i < uxUpdatePackages.Items.Count; i++)
            {
                var pack = uxUpdatePackages.Items[i].Tag as IPackage;
                Update(pack);
            }
        }

        private void Update(IPackage pack)
        {
            if (pack != null)
            {
                // deactivate the old version and mark for uninstall
                var extension = App.EnsureDeactivated(pack.Id);

                if (IsPackageInstalled(pack))
                {
                    App.MarkPackageForRemoval(GetPackagePath(pack));
                }
                else

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

            // hack: we might need to refresh the installed list to show new version numbers
            // or dependencies that were retrieved with the new version.
            App.ProgressHandler.Progress(null, 0, "Ready.");
            uxUpdate.Enabled = true;
        }
    }
}