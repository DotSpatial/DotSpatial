using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Extensions;
using DotSpatial.Plugins.ExtensionManager.Properties;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// A Dialog that allows the user to manage extensions.
    /// </summary>
    internal partial class ExtensionManagerForm : Form
    {
        #region Fields
        private const int ScrollBarMargin = 25;
        private readonly ListViewHelper _add = new ListViewHelper();

        private readonly DownloadForm _downloadDialog = new DownloadForm();
        private readonly Packages _packages = new Packages();
        private readonly Paging _paging;
        private readonly string _appName;

        private AppManager _app;
        private int _currentPageNumber;
        private bool _restartNeccesary;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionManagerForm"/> class.
        /// </summary>
        public ExtensionManagerForm()
        {
            InitializeComponent();

            _paging = new Paging(_packages, _add);

            Installed.TileSize = new Size((Installed.Width - ScrollBarMargin) / 4, 45);
            Installed.HandleCreated += SetTheme;
            Installed.MultiSelect = false;

            uxPackages.TileSize = new Size((uxPackages.Width - ScrollBarMargin) / 4, 45);
            uxPackages.HandleCreated += SetTheme;
            uxPackages.MultiSelect = false;

            var dataService = _packages.Repo as DataServicePackageRepository;
            if (dataService != null)
            {
                dataService.ProgressAvailable += DataServiceProgressAvailable;
            }

            var packageManager = _packages.Manager;
            if (packageManager != null)
            {
                packageManager.PackageInstalling += PackageInstalling;
            }

            _paging.PageChanged += AddPageChanged;
            FormClosing += ExtensionManagerFormClosing;
            tabControl.Deselecting += TabDeselecting;
            tabControl.SelectedIndexChanged += TabSelected;

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

        #region Properties

        /// <summary>
        /// Gets or sets the AppManager.
        /// </summary>
        public AppManager App
        {
            get
            {
                if (_app == null)
                    throw new InvalidOperationException("App must be set to an instance of AppManager before the ExtensionManagerForm can be shown.");

                return _app;
            }

            set
            {
                _app = value;
            }
        }

        /// <summary>
        /// Gets or sets the updates.
        /// </summary>
        public Update Updates { get; set; }
        #endregion

        #region Methods

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        /// <summary>
        /// Gives you an array of strings.
        /// </summary>
        /// <param name="source">Enumerable source object</param>
        /// <typeparam name="T">Type of the source elements.</typeparam>
        /// <returns>List of strings from the enumerable source object</returns>
        private static string[] ToArrayOfStrings<T>(IEnumerable<T> source)
        {
            return source.Select(_ => _.ToString()).ToArray();
        }

        private void AddPageChanged(object sender, PageSelectedEventArgs e)
        {
            _currentPageNumber = e.SelectedPage - 1;
            richTextBox1.Clear();
            DisplayPackages();
        }

        /// <summary>
        /// Appends a label and some text to the text box on the installed tab.
        /// </summary>
        /// <param name="label">The label for your text.  eg. "Label":</param>
        /// <param name="text">The text to be diplayed.</param>
        private void AppendToInstalledTab(string label, string text)
        {
            richTextBox2.SelectionColor = Color.Gray;
            richTextBox2.AppendText(label + ": ");
            richTextBox2.SelectionColor = Color.Black;
            if (text != null)
                richTextBox2.AppendText(text);
            richTextBox2.AppendText(Environment.NewLine + Environment.NewLine);
        }

        /// <summary>
        /// Appends a label and some text to the text box on the online tab.
        /// </summary>
        /// <param name="label">The label for your text.  eg. "Label":</param>
        /// <param name="text">The text to be diplayed.</param>
        private void AppendToOnlineTab(string label, string text)
        {
            richTextBox1.SelectionColor = Color.Gray;
            richTextBox1.AppendText(label + ": ");
            richTextBox1.SelectionColor = Color.Black;
            if (text != null)
                richTextBox1.AppendText(text);
            richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);
        }

        private void CheckBox1CheckedChanged(object sender, EventArgs e)
        {
            FeedManager.ToggleAutoUpdate(_packages.Repo.Source, checkBox1.Checked);
        }

        private void DataServiceProgressAvailable(object sender, ProgressEventArgs e)
        {
            if (e.PercentComplete > 0)
            {
                _downloadDialog.SetProgressBarPercent(e.PercentComplete);
            }
        }

        /// <summary>
        /// Refreashes and displays the online packages and Updates.
        /// </summary>
        private void DisplayPackages()
        {
            _paging.DisplayPackages(uxPackages, _currentPageNumber, tabOnline, App);
            uxInstall.Enabled = false;
            richTextBox1.Clear();
        }

        private void ExtensionManagerFormClosing(object sender, FormClosingEventArgs e)
        {
            // hide the extension manager when closed by the user
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void ExtensionManagerFormFormClosed(object sender, FormClosedEventArgs e)
        {
            _downloadDialog.Close();
        }

        private void ExtensionManagerFormLoad(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabOnline;

            if (FeedManager.GetAutoUpdateFeeds().Count > 0)
                checkBox1.Checked = true;

            SearchAndClearIcons();
        }

        private string GetPackageFolderName(IPackage pack)
        {
            string path = Path.Combine(AppManager.AbsolutePathToExtensions, AppManager.PackageDirectory);
            string[] folder = Directory.GetDirectories(path, pack.Id + "*");

            if (folder.Length > 0)
                return Path.GetDirectoryName(folder[0] + '/');

            return $"{pack.Id}.{pack.Version}";
        }

        private void InstallButtonClick(object sender, EventArgs e)
        {
            if (uxPackages.SelectedItems.Count < 1)
                return;

            _downloadDialog.Show();
            uxInstall.Enabled = false;
            var pack = uxPackages.SelectedItems[0].Tag as IPackage;
            if (pack == null)
            {
                return;
            }

            // make a list of all the ext. that are deactivated, do the install, then activate everything,
            // except the list of those that were deactivated.
            // hack: Hope the user doesn't unload extensions while we install.
            var inactiveExtensions = App.Extensions.Where(a => !a.IsActive).ToArray();

            var task = Task.Factory.StartNew(
                () =>
                {
                    var dependency = pack.Dependencies.ToList();
                    if (dependency.Count > 0)
                    {
                        foreach (PackageDependency dependentPackage in dependency)
                        {
                            App.ProgressHandler.Progress(null, 0, string.Format(Resources.DownloadingDependency, dependentPackage.Id));
                            _downloadDialog.ShowDownloadStatus(dependentPackage);
                            _downloadDialog.SetProgressBarPercent(0);

                            var dependentpack = _packages.Install(dependentPackage.Id);
                            if (dependentpack == null)
                            {
                                string message = string.Format(Resources.CannotDownloadCheckInternetConnection, dependentPackage.Id);
                                MessageBox.Show(message);
                                return;
                            }
                        }
                    }

                    // Download the extension.
                    App.ProgressHandler.Progress(null, 0, string.Format(Resources.Downloading, pack.Title));
                    _downloadDialog.ShowDownloadStatus(pack);
                    _downloadDialog.SetProgressBarPercent(0);

                    var package = _packages.Install(pack.Id);
                    if (package == null)
                    {
                        MessageBox.Show(string.Format(Resources.CannotDownloadCheckInternetConnection, pack.Id));
                    }
                });

            // UI related work.
            task.ContinueWith(
                t =>
                {
                    // Load the extension.
                    App.RefreshExtensions();
                    IEnumerable<PackageDependency> dependency = pack.Dependencies;
                    App.ProgressHandler.Progress(null, 0, string.Format(Resources.Installing, pack.Title));

                    // Activate the extension(s) that was installed.
                    var extensions = App.Extensions.Where(a => !inactiveExtensions.Contains(a) && !a.IsActive).ToList();

                    if (extensions.Count > 0 && !App.EnsureRequiredImportsAreAvailable())
                        return;

                    foreach (var item in extensions)
                    {
                        item.TryActivate();
                    }

                    Installed.Items.Clear();
                    App.ProgressHandler.Progress(null, 0, Resources.Ready);

                    string message = string.Format(Resources.TheFollowingPackagesAreInstalled, pack.Title);

                    foreach (PackageDependency dependentPackage in dependency)
                    {
                        message += dependentPackage.Id + " , ";
                    }

                    _downloadDialog.Visible = false;
                    DisplayPackages();
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void InstalledSelectedItemChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (Installed.SelectedItems.Count == 0)
            {
                uxUninstall.Enabled = false;
                richTextBox2.Clear();
                return;
            }

            var pack = Installed.SelectedItems[0].Tag as IPackage;
            if (pack != null)
            {
                richTextBox2.Clear();

                AppendToInstalledTab(Resources.CreatedBy, string.Join(",", ToArrayOfStrings(pack.Authors)));
                AppendToInstalledTab(Resources.Id, pack.Id);
                AppendToInstalledTab(Resources.Version, pack.Version.ToString());
                AppendToInstalledTab(Resources.Description, pack.Description);
            }

            if (!_restartNeccesary)
                uxUninstall.Enabled = true;
        }

        private void OnlineSelectedItemChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (uxPackages.SelectedItems.Count == 0)
            {
                uxInstall.Enabled = false;
                richTextBox1.Clear();
                return;
            }

            var pack = uxPackages.SelectedItems[0].Tag as IPackage;
            if (pack != null)
            {
                richTextBox1.Clear();

                AppendToOnlineTab(Resources.CreatedBy, string.Join(",", ToArrayOfStrings(pack.Authors)));
                AppendToOnlineTab(Resources.Id, pack.Id);
                AppendToOnlineTab(Resources.Version, pack.Version.ToString());
                AppendToOnlineTab(Resources.Description, pack.Description);
                AppendToOnlineTab(Resources.Downloads, pack.DownloadCount.ToString());

                var dependency = pack.Dependencies.ToList();
                if (dependency.Count > 0)
                {
                    richTextBox1.SelectionColor = Color.Gray;
                    richTextBox1.AppendText(Resources.Dependencies);
                    richTextBox1.SelectionColor = Color.Black;

                    var lastitem = dependency.Last().Id;
                    foreach (PackageDependency dependentPackage in dependency)
                    {
                        if (dependentPackage.Id != lastitem)
                            richTextBox1.AppendText(dependentPackage.Id + ", ");
                        else
                            richTextBox1.AppendText(dependentPackage.Id);
                    }
                }

                uxInstall.Enabled = App.GetExtension(pack.Id) == null;
            }
        }

        private void PackageInstalling(object sender, PackageOperationEventArgs e)
        {
            _downloadDialog.SetProgressBarPercent(100);
            _downloadDialog.Show(string.Format(Resources.Installing, e.Package));
        }

        /// <summary>
        /// Searches the online feed selected for the specified search term in uxSearchText.
        /// </summary>
        private void Search()
        {
            uxPackages.Items.Clear();
            uxPackages.Items.Add("Searching...");

            var task = Task.Factory.StartNew(() =>
                {
                    string search = uxSearchText.Text;
                    IQueryable<IPackage> results = _packages.Repo.Search(search, false);
                    var query = from item in results where item.IsLatestVersion select item;
                    return query.ToList();
                });

            task.ContinueWith(
                t =>
                {
                    _add.AddPackages(t.Result, uxPackages, _currentPageNumber);

                    if (uxPackages.Items.Count == 0)
                    {
                        uxPackages.Items.Add(Resources.NoMatchingPackagesFound);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Initialized icons and their event handlers.
        /// </summary>
        private void SearchAndClearIcons()
        {
            Image searchIcon = Resources.google_custom_search;
            Image clearIcon = Resources.draw_eraser;
            ImageList image = new ImageList();
            image.Images.Add(searchIcon);
            image.Images.Add(clearIcon);
            image.ImageSize = new Size(20, 20);
            uxSearch.Image = image.Images[0];
            uxClear.Image = image.Images[1];
            uxSearch.Click += UxSearchClick;
            uxClear.Click += UxClearClick;
            uxClear.Visible = false;
        }

        private void SetTheme(object sender, EventArgs e)
        {
            SetWindowTheme(((ListView)sender).Handle, "Explorer", null);
        }

        /// <summary>
        /// Populates the installed extensions list based on the selected category.
        /// </summary>
        private void ShowInstalled()
        {
            var localPackages = from item in _packages.Manager.LocalRepository.GetPackages() where item.Id.Contains("Plugins") && !item.Id.Contains(_appName + ".Plugins") && App.GetExtension(item.Id) != null && App.GetExtension(item.Id).DeactivationAllowed select item;

            _add.AddPackages(localPackages.ToArray(), Installed, _currentPageNumber);
            uxUninstall.Enabled = false;
            richTextBox2.Clear();
        }

        private void TabDeselecting(object sender, TabControlCancelEventArgs e)
        {
            if (_restartNeccesary)
            {
                if (tabControl.SelectedIndex == 0)
                    MessageBox.Show(string.Format(Resources.PleaseRestartBeforeDownloadingPlugins, _appName));
                e.Cancel = true;
            }
        }

        private void TabSelected(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabInstalled)
                ShowInstalled();
            if (tabControl.SelectedTab == tabOnline)
                DisplayPackages();
        }

        private void UxClearClick(object sender, EventArgs e)
        {
            uxSearchText.Clear();
            DisplayPackages();
            uxSearchText.Text = Resources.Search;
            uxClear.Visible = false;
            uxSearch.Visible = true;
        }

        private void UxSearchClick(object sender, EventArgs e)
        {
            Search();
            uxSearch.Visible = false;
            uxClear.Visible = true;
        }

        private void UxSearchTextClick(object sender, EventArgs e)
        {
            uxSearchText.Clear();
        }

        private void UxSearchTextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
                uxSearch.Visible = false;
                uxClear.Visible = true;
            }
        }

        private void UxShowExtensionsFolderClick(object sender, EventArgs e)
        {
            string dir = AppManager.AbsolutePathToExtensions;
            if (Directory.Exists(dir))
            {
                Process.Start(dir);
            }
            else
            {
                MessageBox.Show(string.Format(Resources.ExtensionsFolderDoesNotExist, dir));
            }
        }

        private void UxUninstallClick(object sender, EventArgs e)
        {
            if (Installed.SelectedItems.Count < 1)
                return;

            var selectedPackage = Installed.SelectedItems[0].Tag as IPackage;
            if (selectedPackage == null) return;

            IExtension selectedExtension = App.GetExtension(selectedPackage.Id);
            uxUninstall.Enabled = false;

            // Remove the selected package.
            App.ProgressHandler.Progress(null, 0, string.Format(Resources.Uninstalling, selectedPackage.Id));

            // Make backup
            if (selectedExtension.DeactivationAllowed)
                App.EnsureDeactivated(selectedPackage.Id);

            App.MarkPackageForRemoval(GetPackageFolderName(selectedPackage));

            // hack: we should really try to refresh the list, using what ever category the user has selected.
            App.ProgressHandler.Progress(null, 0, Resources.Ready);

            MessageBox.Show(Resources.ExtensionWillFinishUninstallingOnRestart);
            _restartNeccesary = true;
        }

        #endregion
    }
}