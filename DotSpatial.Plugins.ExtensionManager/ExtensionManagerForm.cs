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
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Extensions;
using DotSpatial.Modeling.Forms;
using DotSpatial.Extensions;
using NuGet;
using System.Net;
using DotSpatial.Data;

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
        private readonly ListViewHelper Add = new ListViewHelper();
        private const int ScrollBarMargin = 25;
        private readonly DownloadForm downloadDialog = new DownloadForm();
        private readonly Paging paging;
        private GetPackage getpack;
        private bool restartNeccesary = false;

        private int currentPageNumber;

        #endregion

        #region Constructors and Destructors

        public ExtensionManagerForm()
        {
            InitializeComponent();

            getpack = new GetPackage(packages);
            paging = new Paging(packages, Add);

            // Databind the check list box to the Name property of extension.
            uxCategoryList.DisplayMember = "Name";
            uxPackages.TileSize = new Size(uxPackages.Width - ScrollBarMargin, 45);
            uxUpdatePackages.TileSize = new Size(uxPackages.Width - ScrollBarMargin, 45);

            paging.PageChanged += new EventHandler<PageSelectedEventArgs>(Add_PageChanged);
            FormClosing += ExtensionManager_FormClosing;
            tabControl.Deselecting += tab_deselecting;
        }

        #endregion

        #region Public Properties

        private void tab_deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (restartNeccesary)
            {
                if (tabControl.SelectedIndex == 0)
                    MessageBox.Show("Please restart HydroDesktop before attempting to download or update any plugins.");
                if (tabControl.SelectedIndex == 2)
                    MessageBox.Show("Please restart HydroDesktop before attempting to download or uninstall any plugins.");
                e.Cancel = true;
            }
        }

        public void dataService_ProgressAvailable(object sender, ProgressEventArgs e)
        {
            if (e.PercentComplete > 0)
            {
                downloadDialog.SetProgressBarPercent(e.PercentComplete);
            }
        }

        public void Package_Installing(object sender, PackageOperationEventArgs e)
        {
            downloadDialog.SetProgressBarPercent(100);
            downloadDialog.Show("Installing " + e.Package);
        }

        private void Add_PageChanged(object sender, PageSelectedEventArgs e)
        {
            currentPageNumber = e.SelectedPage - 1;
            richTextBox1.Clear();
            DisplayPackagesAndUpdates();
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

        public void AutoUpdateRestartNeccesary()
        {
            restartNeccesary = true;
            tabControl.SelectedIndex = 2;
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            if (uxPackages.SelectedItems.Count < 1)
            {
                return;
            }
            downloadDialog.Show();
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

            var task = Task.Factory.StartNew(() =>
            {
                IEnumerable<PackageDependency> dependency = pack.Dependencies;
                if (dependency.Count() > 0)
                {
                    foreach (PackageDependency dependentPackage in dependency)
                    {
                        App.ProgressHandler.Progress(null, 0, "Downloading Dependency " + dependentPackage.Id);
                        downloadDialog.ShowDownloadStatus(dependentPackage);
                        downloadDialog.SetProgressBarPercent(0);

                        var dependentpack = packages.Install(dependentPackage.Id);
                        if (dependentpack == null)
                        {
                            string message = "We cannot download " + dependentPackage.Id + " Please make sure you are connected to the Internet.";
                            MessageBox.Show(message);
                            return;
                        }
                    }
                }
                // Download the extension.
                App.ProgressHandler.Progress(null, 0, "Downloading " + pack.Title);
                downloadDialog.ShowDownloadStatus(pack);
                downloadDialog.SetProgressBarPercent(0);

                var package = packages.Install(pack.Id);
                if (package == null)
                {
                    MessageBox.Show("We cannot download " + pack.Id + " Please make sure you are connected to the Internet");
                }
            });

            // UI related work.
            task.ContinueWith((t) =>
            {
                // Load the extension.
                App.RefreshExtensions();
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

                Installed.Items.Clear();
                App.ProgressHandler.Progress(null, 0, "Ready.");

                string message = "The following packages are installed: " + pack.Title;

                foreach (PackageDependency dependentPackage in dependency)
                {
                    message += dependentPackage.Id + " , ";
                }

                downloadDialog.Visible = false;

                ShowInstalledItemsBasedOnSelectedCategory();
            },
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void uxUninstall_Click(object sender, EventArgs e)
        {
            var selectedextension = Installed.SelectedItem as IExtension;
            IPackage selectedPackage = getpack.GetPackageFromExtension(selectedextension);
            if (selectedPackage == null)
            {
                return;
            }
            uxUninstall.Enabled = false;

            // Remove the selected package.
            App.ProgressHandler.Progress(null, 0, "Uninstalling" + selectedPackage.Id);

            //Check for backupfile
            bool abort = false;
            var assembly = Assembly.GetAssembly(selectedextension.GetType());
            string path = assembly.Location;
            string backupFile = Application.StartupPath + "\\backup\\" + Path.GetFileName(path);

            if(File.Exists(backupFile))
            {
                try
                {
                    if (selectedextension.DeactivationAllowed)
                        File.Move(backupFile, Application.StartupPath + "\\Plugins\\" + Path.GetFileName(path));
                    else
                        File.Move(backupFile, Application.StartupPath + "\\Application Extensions\\" + Path.GetFileName(path));
                }
                catch (Exception) 
                {
                    DialogResult dialogResult = MessageBox.Show("Unable to restore the backup of the extension." +
                    "\n\nDo you want to Uninstal without restoring the backing?", "Backup Error", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.No)
                    {
                        abort = true;
                    }
                }
            }

            if (!abort)
            {
                if (!selectedextension.DeactivationAllowed)
                    App.EnsureDeactivated(selectedPackage.Id);
                App.MarkPackageForRemoval(ExtensionManager.Update.GetPackageFolderName(selectedPackage));
            }

            // hack: we should really try to refresh the list, using what ever category the user
            // has selected.
            App.ProgressHandler.Progress(null, 0, "Ready.");

            if (!abort)
            {
                MessageBox.Show("The extension will finish uninstalling when you restart the application.");
                restartNeccesary = true;
            }
        }

        private void ExtensionManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            //hide the extension manager when closed by the user
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void AddCategory(IExtensionCategory category)
        {
            category.App = this.App;
            uxCategoryList.Items.Add(category);
        }

        private void ExtensionManagerForm_Load(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabOnline;
            AddCategory(new ExtensionCategory());
            AddCategory(new ToolsCategory());
            AddCategory(new DataProviderCategory());
            AddCategory(new ApplicationExtensionCategory());

            uxFeedSources.TileSize = new Size(uxFeedSources.Width - 25, 45);

            foreach (Feed feed in FeedManager.GetFeeds())
            {
                ListViewItem source = new ListViewItem();
                source.Text = feed.Name;
                source.SubItems.Add(feed.Url);
                uxFeedSources.Items.Add(source);
                uxFeedSelection.Items.Add(feed.Url);
                uxFeedSelection2.Items.Add(feed.Url);
                source.Checked = FeedManager.isAutoUpdateFeed(feed.Url);
            }
            // Select the first item in the drop down, kicking off a package list update.
            uxFeedSelection.SelectedIndex = 0;
            uxCategoryList.SelectedIndex = 1;
            SearchAndClearIcons();
            uxClear.Visible = false;
        }

        public void uxSearch_Click(object sender, EventArgs e)
        {
            Search();
            uxSearch.Visible = false;
            uxClear.Location = new Point(291, 42);
            uxClear.Visible = true;
        }

        public void uxClear_Click(object sender, EventArgs e)
        {
            uxSearchText.Clear();
            paging.DisplayPackages(uxPackages, currentPageNumber, tabOnline);
            uxSearchText.Text = "Search";
            uxClear.Visible = false;
            uxSearch.Visible = true;
        }

        private void SearchAndClearIcons()
        {
            Image SearchIcon = DotSpatial.Plugins.ExtensionManager.Properties.Resources.google_custom_search;
            Image ClearIcon = DotSpatial.Plugins.ExtensionManager.Properties.Resources.draw_eraser;
            ImageList image = new ImageList();
            image.Images.Add(SearchIcon);
            image.Images.Add(ClearIcon);
            image.ImageSize = new Size(20, 20);
            uxSearch.Image = image.Images[0];
            uxClear.Image = image.Images[1];
            uxSearch.Click += new EventHandler(this.uxSearch_Click);
            uxClear.Click += new EventHandler(this.uxClear_Click);
        }

        private void Installed_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            IExtension extension = Installed.Items[e.Index] as IExtension;
            if (extension == null)
            {
                return;
            }
            
            if (extension.DeactivationAllowed == false && !AllowProtectedCheck)
            {        
                if (e.NewValue == CheckState.Checked)
                {
                    e.NewValue = CheckState.Unchecked;
                }
                else if (e.NewValue == CheckState.Unchecked)
                {
                    e.NewValue = CheckState.Checked;
                }

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

        private void AppendInstalledItem(string label, string text)
        {
            richTextBox2.SelectionColor = Color.Gray;
            richTextBox2.AppendText(Environment.NewLine + Environment.NewLine + label + ": ");
            richTextBox2.SelectionColor = Color.Black;
            richTextBox2.AppendText(text);
        }

        private void Installed_SelectedValueChanged(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            uxUninstall.Enabled = false;

            if (Installed.SelectedItem == null)
            {
                return;
            }

            ITool tool = Installed.SelectedItem as ITool;
            if (tool != null)
            {
                AppendInstalledItem("Description", tool.Description);
                return;
            }

            IDataProvider dataProvider = Installed.SelectedItem as IDataProvider;
            if (dataProvider != null)
            {
                AppendInstalledItem("Description", dataProvider.Description);
                return;
            }

            IExtension extension = Installed.SelectedItem as IExtension;
            if (extension != null)
            {
                var package = getpack.GetPackageFromExtension(extension);

                if (package == null)
                {
                    AppendInstalledItem("Created by", extension.Author);
                    AppendInstalledItem("Id", extension.Name);
                    AppendInstalledItem("Version", extension.Version);
                    AppendInstalledItem("Description", extension.Description);
                }
                else
                {
                    uxUninstall.Enabled = true;
                    AppendInstalledItem("Created by", string.Join(",", ToArrayOfStrings(package.Authors)));
                    AppendInstalledItem("Id", package.Id);
                    AppendInstalledItem("Version", package.Version.ToString());
                    AppendInstalledItem("Description", package.Description);
                }
            }
        }

        /// <summary>
        /// Gives you an array of strings.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        private static string[] ToArrayOfStrings<T>(IEnumerable<T> source)
        {
            string[] array = new string[source.Count()];
            for (int i = 0; i < source.Count(); i++)
                array[i] = source.ElementAt(i).ToString();
            return array;
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

        private void Search()
        {
            uxPackages.Items.Clear();
            uxPackages.Items.Add("Searching...");

            var task = Task.Factory.StartNew(() =>
            {
                string search = uxSearchText.Text;

                IQueryable<IPackage> results = packages.Repo.Search(search, false);

                var query = from item in results
                            where item.IsLatestVersion == true
                            select item;

                return query.ToList();
            });

            task.ContinueWith(t =>
            {
                Add.AddPackages(t.Result, uxPackages, currentPageNumber);

                if (uxPackages.Items.Count == 0)
                {
                    uxPackages.Items.Add("No packages matching your search terms were found.");
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void uxSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
                uxSearch.Visible = false;
                uxClear.Location = new Point(291, 42);
                uxClear.Visible = true;
            }
        }

        private void uxCategoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowInstalledItemsBasedOnSelectedCategory();
        }

        private void ShowInstalledItemsBasedOnSelectedCategory()
        {
            var category = uxCategoryList.SelectedItem as IExtensionCategory;
            AllowProtectedCheck = true;

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
            AllowProtectedCheck = false;
        }

        private void OnFeedChanged(string source)
        {
            string feedUrl;

            if (source == uxFeedSelection.Name)
            {
                if (uxFeedSelection.SelectedIndex != uxFeedSelection2.SelectedIndex)
                {
                    uxFeedSelection2.SelectedIndex = uxFeedSelection.SelectedIndex;
                    return;
                }

                feedUrl = uxFeedSelection.SelectedItem.ToString();
            }
            else
            {
                if (uxFeedSelection.SelectedIndex != uxFeedSelection2.SelectedIndex)
                {
                    uxFeedSelection.SelectedIndex = uxFeedSelection2.SelectedIndex;
                    return;
                }

                feedUrl = uxFeedSelection2.SelectedItem.ToString();
            }

            packages.SetNewSource(feedUrl);
            currentPageNumber = 0;
            DisplayPackagesAndUpdates();
            var dataService = packages.Repo as DataServicePackageRepository;
            if (dataService != null)
            {
                dataService.ProgressAvailable += new EventHandler<ProgressEventArgs>(dataService_ProgressAvailable);
            }
            var packageManager = packages.Manager;
            if (packageManager != null)
            {
                packageManager.PackageInstalling += new EventHandler<PackageOperationEventArgs>(Package_Installing);
            }
        }

        private Update Updates;

        private void DisplayPackagesAndUpdates()
        {
            Updates = new Update(packages, Add, App);
            paging.DisplayPackages(uxPackages, currentPageNumber, tabOnline);
            uxUpdatePackages.Clear();
            uxUpdatePackages.Items.Add("Checking for updates...");
            Task.Factory.StartNew(() =>
            {
                Updates.RefreshUpdate(uxUpdatePackages, tabPage2);
            });
            
        }

        public void uxFeedSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnFeedChanged(uxFeedSelection.Name);
        }

        public void uxFeedSelection2_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnFeedChanged(uxFeedSelection2.Name);
        }

        private void AppendToOnlineTab(string label, string text)
        {
            richTextBox1.SelectionColor = Color.Gray;
            richTextBox1.AppendText(Environment.NewLine + Environment.NewLine + label + ": ");
            richTextBox1.SelectionColor = Color.Black;
            if (text != null)
                richTextBox1.AppendText(text);
        }

        private void AppendToUpdateTab(string label, string text)
        {
            richTextBox3.SelectionColor = Color.Gray;
            richTextBox3.AppendText(Environment.NewLine + Environment.NewLine + label + ": ");
            richTextBox3.SelectionColor = Color.Black;
            if(text != null)
                richTextBox3.AppendText(text);
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
                richTextBox1.Clear();

                AppendToOnlineTab("Created by", string.Join(",", ToArrayOfStrings(pack.Authors)));
                AppendToOnlineTab("Id", pack.Id);
                AppendToOnlineTab("Version", pack.Version.ToString());
                AppendToOnlineTab("Description", pack.Description);
                AppendToOnlineTab("Downloads", pack.DownloadCount.ToString());

                IEnumerable<PackageDependency> dependency = pack.Dependencies;
                if (dependency.Count() > 0)
                {
                    richTextBox1.SelectionColor = Color.Gray;
                    richTextBox1.AppendText(Environment.NewLine + Environment.NewLine + "Dependencies " + ": ");
                    richTextBox1.SelectionColor = Color.Black;

                    var lastitem = dependency.Last().Id;
                    foreach (PackageDependency dependentPackage in dependency)
                    {
                        if(dependentPackage.Id != lastitem)
                            richTextBox1.AppendText(dependentPackage.Id + ", ");
                        else
                            richTextBox1.AppendText(dependentPackage.Id);
                    }
                }

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
                    //uxUpdate.Enabled = IsPackageUpdateable(pack);
                }
            }
        }

        private void uxUpdate_SelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            var pack = e.Item.Tag as IPackage;

            if (pack != null)
            {
                richTextBox3.Clear();

                AppendToUpdateTab("Created by", string.Join(",", ToArrayOfStrings(pack.Authors)));
                AppendToUpdateTab("Id", pack.Id);
                AppendToUpdateTab("Version", pack.Version.ToString());
                AppendToUpdateTab("Description", pack.Description);
                AppendToUpdateTab("What's New", Environment.NewLine + pack.ReleaseNotes);
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
            try
            {
                UpdatePack(pack);
                showUpdateComplete();
            }
            catch
            {
                MessageBox.Show("Error updating " + pack.GetFullName());
                DisplayPackagesAndUpdates();
                uxUpdate.Enabled = true;
            }
        }

        private void uxUpdateAll_Click(object sender, EventArgs e)
        {
            ListViewItem[] Items = new ListViewItem[uxUpdatePackages.Items.Count];
            uxUpdatePackages.Items.CopyTo(Items, 0);

            for (int i = 0; i < Items.Length; i++)
            {
                var pack = Items[i].Tag as IPackage;
                try
                {
                    foreach (PackageDependency dependency in pack.Dependencies)
                    {
                        if (packages.GetLocalPackage(dependency.Id) == null)
                            packages.Update(packages.Repo.FindPackage(dependency.Id));
                    }

                    Updates.UpdatePackage(pack);
                }
                catch
                {
                    MessageBox.Show("Error updating " + pack.GetFullName());
                }
            }

            uxUpdate.Enabled = true;
            DisplayPackagesAndUpdates();
            showUpdateComplete();
        }

        private void showUpdateComplete()
        {
            MessageBox.Show(this, "Download finished. Update will complete when HydroDesktop is restarted.");
            restartNeccesary = true;
        }

        private void UpdatePack(IPackage pack)
        {
            foreach (PackageDependency dependency in pack.Dependencies)
            {
                foreach (ListViewItem item in uxUpdatePackages.Items)
                {
                    var update = item.Tag as IPackage;
                    if (dependency.Id == update.Id)
                        Updates.UpdatePackage(update);
                }

                if (packages.GetLocalPackage(dependency.Id) == null)
                    packages.Update(packages.Repo.FindPackage(dependency.Id));
            }

            Updates.UpdatePackage(pack);
            uxUpdate.Enabled = true;

            DisplayPackagesAndUpdates();
        }

        private void uxAdd_Click(object sender, EventArgs e)
        {
            Feed feed = new Feed();
            feed.Name = uxSourceName.Text.Trim();
            feed.Url = uxSourceUrl.Text.Trim();
            FeedManager.Add(feed);

            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Text = feed.Name;
            listViewItem.SubItems.Add(feed.Url);
            uxFeedSources.Items.Add(listViewItem);

            if (uxFeedSelection.Items.Contains(feed.Url))
            {
                uxSourceName.Clear();
                uxSourceUrl.Clear();
            }
            else
            {
                uxFeedSelection.Items.Add(feed.Url);
                uxFeedSelection2.Items.Add(feed.Url);
                uxSourceName.Clear();
                uxSourceUrl.Clear();
            }
        }

        private void uxRemove_Click(object sender, EventArgs e)
        {
            ListViewItem selectedItem = uxFeedSources.SelectedItems[0];
            Feed feed = new Feed();
            feed.Name = selectedItem.Text;
            feed.Url = selectedItem.SubItems[1].Text;
            uxFeedSelection.Items.Remove(feed.Url);
            uxFeedSelection2.Items.Remove(feed.Url);
            selectedItem.Remove();
            FeedManager.Remove(feed);
        }

        private void ExtensionManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            downloadDialog.Close();
        }

        private void uxSearchText_Click(object sender, EventArgs e)
        {
            uxSearchText.Clear();
        }

        private void uxApply_Click(object sender, EventArgs e)
        {
            for (int i=0; i<uxFeedSources.Items.Count; i++)
            {
                changeAutoUpdateSetting(uxFeedSources.Items[i]);
            }
            uxApply.Enabled = false;
        }

        private void changeAutoUpdateSetting(ListViewItem item)
        {
            FeedManager.ToggleAutoUpdate(item.SubItems[1].Text, item.Checked);
        }

        private void uxSourceFeed_Checked(object sender, ItemCheckedEventArgs e)
        {
            uxApply.Enabled = true;
        }

        private bool AllowProtectedCheck { get; set; }
    }
}