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
    public partial class ExtensionManagerForm : Form
    {
        #region Constants and Fields
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        private const string HideReleaseFromEndUser = "HideReleaseFromEndUser";
        private const int ScrollBarMargin = 25;

        private readonly DownloadForm downloadDialog = new DownloadForm();
        private readonly ListViewHelper Add = new ListViewHelper();
        private readonly Packages packages = new Packages();
        private readonly Paging paging;

        private bool AllowProtectedCheck { get; set; }
        private bool restartNeccesary = false;
        private int currentPageNumber;
        private GetPackage getpack;
        private AppManager _App;
        private string AppName;

        public Update Updates;

        #endregion

        #region Constructors and Destructors

        public ExtensionManagerForm()
        {
            InitializeComponent();

            getpack = new GetPackage(packages);
            paging = new Paging(packages, Add);

            Installed.TileSize = new Size((Installed.Width - ScrollBarMargin) / 4, 45);
            Installed.HandleCreated += SetTheme;
            Installed.MultiSelect = false;

            uxPackages.TileSize = new Size((uxPackages.Width - ScrollBarMargin)/4, 45);
            uxPackages.HandleCreated += SetTheme;
            uxPackages.MultiSelect = false;

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

            paging.PageChanged += new EventHandler<PageSelectedEventArgs>(Add_PageChanged);
            FormClosing += ExtensionManager_FormClosing;
            tabControl.Deselecting += tab_deselecting;
            tabControl.SelectedIndexChanged += tab_selected;

            //find name of app
            string name = Assembly.GetEntryAssembly().GetName().Name;
            int i;
            for (i = 0; i < name.Length; i++)
            {
                if (!Char.IsLetter(name[i]))
                    break;
            }
            AppName = name.Substring(0, i);
        }

        #endregion

        #region Public Properties

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

        #region Events

        private void Add_PageChanged(object sender, PageSelectedEventArgs e)
        {
            currentPageNumber = e.SelectedPage - 1;
            richTextBox1.Clear();
            DisplayPackages();
        }

        private void dataService_ProgressAvailable(object sender, ProgressEventArgs e)
        {
            if (e.PercentComplete > 0)
            {
                downloadDialog.SetProgressBarPercent(e.PercentComplete);
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

        private void ExtensionManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            downloadDialog.Close();
        }

        private void ExtensionManagerForm_Load(object sender, EventArgs e)
        {
            tabControl.SelectedTab = tabOnline;

            if(FeedManager.getAutoUpdateFeeds().Count > 0)
                checkBox1.Checked = true;

            SearchAndClearIcons();
        }

        private void InstallButton_Click(object sender, EventArgs e)
        {
            if (uxPackages.SelectedItems.Count < 1)
                return;

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
                DisplayPackages();
            },
            TaskScheduler.FromCurrentSynchronizationContext());
        }


        private void Installed_SelectedItemChanged(object sender, ListViewItemSelectionChangedEventArgs e)
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

                AppendToInstalledTab("Created by", string.Join(",", ToArrayOfStrings(pack.Authors)));
                AppendToInstalledTab("Id", pack.Id);
                AppendToInstalledTab("Version", pack.Version.ToString());
                AppendToInstalledTab("Description", pack.Description);
            }

            if (!restartNeccesary)
                uxUninstall.Enabled = true;
        }

        private void Package_Installing(object sender, PackageOperationEventArgs e)
        {
            downloadDialog.SetProgressBarPercent(100);
            downloadDialog.Show("Installing " + e.Package);
        }

        private void Online_SelectedItemChanged(object sender, ListViewItemSelectionChangedEventArgs e)
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

                AppendToOnlineTab("Created by", string.Join(",", ToArrayOfStrings(pack.Authors)));
                AppendToOnlineTab("Id", pack.Id);
                AppendToOnlineTab("Version", pack.Version.ToString());
                AppendToOnlineTab("Description", pack.Description);
                AppendToOnlineTab("Downloads", pack.DownloadCount.ToString());

                IEnumerable<PackageDependency> dependency = pack.Dependencies;
                if (dependency.Count() > 0)
                {
                    richTextBox1.SelectionColor = Color.Gray;
                    richTextBox1.AppendText("Dependencies " + ": ");
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

                if (App.GetExtension(pack.Id) == null)
                    uxInstall.Enabled = true;
                else
                    uxInstall.Enabled = false;
            }
        }

        private void SetTheme(object sender, EventArgs e)
        {
            SetWindowTheme(((ListView)sender).Handle, "Explorer", null);
        }

        private void tab_deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (restartNeccesary)
            {
                if (tabControl.SelectedIndex == 0)
                    MessageBox.Show("Please restart " + AppName +" before attempting to download any plugins.");
                e.Cancel = true;
            }
        }

        private void tab_selected(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabInstalled)
                ShowInstalled();
            if (tabControl.SelectedTab == tabOnline)
                DisplayPackages();
        }

        private void uxClear_Click(object sender, EventArgs e)
        {
            uxSearchText.Clear();
            DisplayPackages();
            uxSearchText.Text = "Search";
            uxClear.Visible = false;
            uxSearch.Visible = true;
        }

        private void uxSearch_Click(object sender, EventArgs e)
        {
            Search();
            uxSearch.Visible = false;
            uxClear.Visible = true;
        }

        private void uxSearchText_Click(object sender, EventArgs e)
        {
            uxSearchText.Clear();
        }

        private void uxSearchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
                uxSearch.Visible = false;
                uxClear.Visible = true;
            }
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

        private void uxUninstall_Click(object sender, EventArgs e)
        {
            if (Installed.SelectedItems.Count < 1)
                return;

            var selectedPackage = Installed.SelectedItems[0].Tag as IPackage;
            IExtension selectedExtension = App.GetExtension(selectedPackage.Id);
            uxUninstall.Enabled = false;

            // Remove the selected package.
            App.ProgressHandler.Progress(null, 0, "Uninstalling" + selectedPackage.Id);

            //Make backup

            if (selectedExtension.DeactivationAllowed)
                    App.EnsureDeactivated(selectedPackage.Id);
            App.MarkPackageForRemoval(GetPackageFolderName(selectedPackage));

            // hack: we should really try to refresh the list, using what ever category the user
            // has selected.
            App.ProgressHandler.Progress(null, 0, "Ready.");

            MessageBox.Show("The extension will finish uninstalling when you restart the application.");
            restartNeccesary = true;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Appends a label and some text to the text box on the online tab.
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="label">The label for your text.  eg. "Label":</param>
        /// <typeparam name="string"></typeparam>
        /// <param name="text">The text to be diplayed.</param>
        /// <returns></returns>
        private void AppendToOnlineTab(string label, string text)
        {
            richTextBox1.SelectionColor = Color.Gray;
            richTextBox1.AppendText(label + ": ");
            richTextBox1.SelectionColor = Color.Black;
            if (text != null)
                richTextBox1.AppendText(text);
            richTextBox1.AppendText(Environment.NewLine + Environment.NewLine);
        }

        /// <summary>
        /// Appends a label and some text to the text box on the installed tab.
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <param name="label">The label for your text.  eg. "Label":</param>
        /// <typeparam name="string"></typeparam>
        /// <param name="text">The text to be diplayed.</param>
        /// <returns></returns>
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
        /// Sets a feed to be used with auto-updating.
        /// </summary>
        /// <typeparam name="ListViewItem"></typeparam>
        /// <param name="item">The feed that should be used for auto updates</param>
        /// <returns></returns>
        private void changeAutoUpdateSetting(ListViewItem item)
        {
            FeedManager.ToggleAutoUpdate(item.SubItems[1].Text, item.Checked);
        }

        /// <summary>
        /// Refreashes and displays the online packages and Updates.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns></returns>
        private void DisplayPackages()
        {
            paging.DisplayPackages(uxPackages, currentPageNumber, tabOnline, App);
            uxInstall.Enabled = false;
            richTextBox1.Clear();
        }

        private string GetPackageFolderName(IPackage pack)
        {
            string path = Path.Combine(AppManager.AbsolutePathToExtensions, AppManager.PackageDirectory);
            string[] folder = Directory.GetDirectories(path, pack.Id + "*");

            if (folder.Length > 0)
                return Path.GetDirectoryName(folder[0] + '/');
            else
                return String.Format("{0}.{1}", pack.Id, pack.Version);
        }

        /// <summary>
        /// Searches the online feed selected for the specified search term
        /// in uxSearchText.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns></returns>
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

        /// <summary>
        /// Initialized icons and their event handlers.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns></returns>
        private void SearchAndClearIcons()
        {
            Image SearchIcon = Resources.google_custom_search;
            Image ClearIcon = Resources.draw_eraser;
            ImageList image = new ImageList();
            image.Images.Add(SearchIcon);
            image.Images.Add(ClearIcon);
            image.ImageSize = new Size(20, 20);
            uxSearch.Image = image.Images[0];
            uxClear.Image = image.Images[1];
            uxSearch.Click += new EventHandler(this.uxSearch_Click);
            uxClear.Click += new EventHandler(this.uxClear_Click);
            uxClear.Visible = false;
        }

        /// <summary>
        /// Populates the installed extensions list based on the selected category.
        /// </summary>
        /// <typeparam></typeparam>
        /// <param></param>
        /// <returns></returns>
        private void ShowInstalled()
        {
            var localPackages = from item in packages.Manager.LocalRepository.GetPackages()
                                where item.Id.Contains("Plugins") && !item.Id.Contains(AppName + ".Plugins") 
                                && App.GetExtension(item.Id) != null && App.GetExtension(item.Id).DeactivationAllowed
                                select item;

            Add.AddPackages(localPackages.ToArray(), Installed, currentPageNumber);
            uxUninstall.Enabled = false;
            richTextBox2.Clear();
        }

        /// <summary>
        /// Gives you an array of strings.
        /// </summary>
        /// <typeparam name="IEnumerable\<T\>"></typeparam>
        /// <param name="source">Enumerable source object</param>
        /// <returns>List of strings from the enumerable source object</returns>
        private static string[] ToArrayOfStrings<T>(IEnumerable<T> source)
        {
            string[] array = new string[source.Count()];
            for (int i = 0; i < source.Count(); i++)
                array[i] = source.ElementAt(i).ToString();
            return array;
        }

        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
                FeedManager.ToggleAutoUpdate(packages.Repo.Source, true);
            else
                FeedManager.ToggleAutoUpdate(packages.Repo.Source, false);
        }
    }
}