// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Extensions;
using DotSpatial.Plugins.ExtensionManager.Properties;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// Shows the DotSpatial sample projects.
    /// </summary>
    internal partial class SampleProjectsForm : Form
    {
        #region Fields
        private readonly DownloadForm _downloadDialog = new DownloadForm();
        private readonly Packages _packages = new Packages();
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleProjectsForm"/> class.
        /// </summary>
        /// <param name="app">The app manager.</param>
        public SampleProjectsForm(AppManager app)
        {
            InitializeComponent();
            App = app;
            uxFeedSelection.SelectedIndexChanged += UxFeedSelectionSelectedIndexChanged;
            uxFeedSelection.SelectedIndex = 0;
            SampleProjects = new List<SampleProjectInfo>();
            listBoxTemplates.SelectedIndexChanged += ListBoxTemplatesSelectedIndexChanged;
            uxOnlineProjects.SelectedIndexChanged += UxOnlineProjectsSelectedIndexChanged;
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
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the app manager.
        /// </summary>
        public AppManager App { get; set; }

        /// <summary>
        /// Gets or sets the sample projects.
        /// </summary>
        public IEnumerable<SampleProjectInfo> SampleProjects { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the progress bar.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        public void DataServiceProgressAvailable(object sender, ProgressEventArgs e)
        {
            if (e.PercentComplete > 0)
            {
                _downloadDialog.SetProgressBarPercent(e.PercentComplete);
            }
        }

        /// <summary>
        /// Shows which package is currently installing.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        public void PackageInstalling(object sender, PackageOperationEventArgs e)
        {
            _downloadDialog.SetProgressBarPercent(100);
            _downloadDialog.Show(string.Format(Resources.Installing, e.Package));
        }

        private static string GetPackageFolderName(IPackage pack)
        {
            return $"{pack.Id}.{pack.Version}";
        }

        private static string GetPackagePath(IPackage pack)
        {
            return Path.Combine(AppManager.AbsolutePathToExtensions, "Packages", GetPackageFolderName(pack));
        }

        private static bool IsPackageInstalled(IPackage pack)
        {
            string packagePath = GetPackagePath(pack);

            // return Directory.Exists(packagePath) &&  // Directory.EnumerateFiles(packagePath, "*.dspx", SearchOption.AllDirectories).Any<string>();
            if (!Directory.Exists(packagePath)) return false;
            return EnumerableExtensions.Any(Directory.EnumerateFiles(packagePath, "*.dspx", SearchOption.AllDirectories));
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            SampleProjectInfo sample = listBoxTemplates.SelectedItem as SampleProjectInfo;
            OpenSampleProject(sample);
            Close();
        }

        private void BtnOkOnlineClick(object sender, EventArgs e)
        {
            IPackage pack = uxOnlineProjects.SelectedItem as IPackage;
            if (pack == null) return;

            btnInstall.Enabled = false;
            _downloadDialog.Show();

            var inactiveExtensions = App.Extensions.Where(a => a.IsActive == false).ToArray();

            Task task = Task.Factory.StartNew(() =>
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
                                MessageBox.Show(string.Format(Resources.CannotDownloadCheckInternetConnection, dependentPackage.Id));
                                return;
                            }
                        }
                    }

                    App.ProgressHandler.Progress(null, 0, string.Format(Resources.Downloading, pack.Title));
                    _downloadDialog.ShowDownloadStatus(pack);
                    _downloadDialog.SetProgressBarPercent(0);

                    _packages.Install(pack.Id);
                });
            task.ContinueWith(
                t =>
                    {
                        App.ProgressHandler.Progress(null, 0, string.Format(Resources.Installing, pack.Title));
                        UpdateInstalledProjectsList();

                        // Load the extension.
                        App.RefreshExtensions();
                        App.ProgressHandler.Progress(null, 50, string.Format(Resources.Installing, pack.Title));

                        // Activate the extension(s) that was installed.
                        var extensions = App.Extensions.Where(a => !inactiveExtensions.Contains(a) && !a.IsActive).ToList();

                        if (extensions.Count > 0 && !App.EnsureRequiredImportsAreAvailable())
                            return;

                        foreach (var item in extensions)
                        {
                            item.TryActivate();
                        }

                        App.ProgressHandler.Progress(null, 0, Resources.Ready);
                        _downloadDialog.Visible = false;
                    },
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void BtnUninstallClick(object sender, EventArgs e)
        {
            SampleProjectInfo sample = listBoxTemplates.SelectedItem as SampleProjectInfo;
            UninstallSampleProject(sample);
            UpdateInstalledProjectsList();
        }

        private string CopyToDocumentsFolder(string projectFile)
        {
            string directoryName = Path.GetDirectoryName(projectFile);
            if (directoryName == null) return string.Empty;

            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string text = Path.Combine(folderPath, "DotSpatial");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(projectFile);
            string text2 = Path.Combine(text, fileNameWithoutExtension);
            if (!Directory.Exists(text2))
            {
                Directory.CreateDirectory(text2);
            }

            string[] files = Directory.GetFiles(directoryName);
            for (int i = 0; i < files.Length; i++)
            {
                string text3 = files[i];
                if (text3 != null) File.Copy(text3, Path.Combine(text2, Path.GetFileName(text3)), true);
            }

            return Path.Combine(text2, Path.GetFileName(projectFile));
        }

        private IEnumerable<SampleProjectInfo> FindSampleProjectFiles()
        {
            List<SampleProjectInfo> list = new List<SampleProjectInfo>();
            if (Directory.Exists(AppManager.AbsolutePathToExtensions))
            {
                foreach (string current in Directory.EnumerateFiles(AppManager.AbsolutePathToExtensions, "*.dspx", SearchOption.AllDirectories))
                {
                    list.Add(new SampleProjectInfo
                    {
                        AbsolutePathToProjectFile = current,
                        Name = Path.GetFileNameWithoutExtension(current),
                        Description = "description",
                        Version = "1.0"
                    });
                }
            }

            return list;
        }

        private void ListBoxTemplatesDoubleClick(object sender, EventArgs e)
        {
            SampleProjectInfo sample = listBoxTemplates.SelectedItem as SampleProjectInfo;
            OpenSampleProject(sample);
            Close();
        }

        private void ListBoxTemplatesSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTemplates.SelectedIndex >= 0 && listBoxTemplates.SelectedItem.ToString() != Resources.NoProjectTemplatesPleaseInstall)
            {
                btnUninstall.Enabled = true;
                return;
            }

            btnUninstall.Enabled = false;
        }

        private void OpenSampleProject(SampleProjectInfo sample)
        {
            string absolutePathToProjectFile = sample.AbsolutePathToProjectFile;
            try
            {
                App.SerializationManager.OpenProject(absolutePathToProjectFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Resources.ErrorFilePath, ex.Message, absolutePathToProjectFile));
            }
        }

        private void TemplateFormLoad(object sender, EventArgs e)
        {
            UpdateInstalledProjectsList();
        }

        private void UninstallSampleProject(SampleProjectInfo sample)
        {
            if (App.SerializationManager.CurrentProjectFile == sample.AbsolutePathToProjectFile)
            {
                MessageBox.Show(string.Format(Resources.CannotUninstallProjectOpen, sample.Name));
                return;
            }

            string directoryName = Path.GetDirectoryName(sample.AbsolutePathToProjectFile);
            if (directoryName != null)
            {
                DirectoryInfo parent = Directory.GetParent(directoryName);
                try
                {
                    foreach (string current in Directory.EnumerateFiles(directoryName))
                    {
                        File.Delete(current);
                    }

                    Directory.Delete(directoryName);
                    FileInfo[] files = parent.GetFiles();
                    for (int i = 0; i < files.Length; i++)
                    {
                        FileInfo fileInfo = files[i];
                        fileInfo.Delete();
                    }

                    if (!EnumerableExtensions.Any(parent.GetDirectories()) && !EnumerableExtensions.Any(parent.GetFiles()))
                    {
                        parent.Delete();
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(string.Format(Resources.SomeFilesCouldNotBeUninstalled, ex.Message));
                }
            }

            MessageBox.Show(Resources.ProjectSuccessfullyUninstalled);
        }

        private void UpdateInstalledProjectsList()
        {
            SampleProjects = FindSampleProjectFiles();
            if (!EnumerableExtensions.Any(SampleProjects))
            {
                listBoxTemplates.DataSource = null;
                listBoxTemplates.Items.Add(Resources.NoProjectTemplatesPleaseInstall);
                return;
            }

            listBoxTemplates.DataSource = SampleProjects;
            listBoxTemplates.DisplayMember = "Name";
            uxOnlineProjects.SelectedIndex = 0;
            btnInstall.Enabled = true;
        }

        private void UpdatePackageList()
        {
            uxOnlineProjects.Items.Add("Loading...");
            Task<IPackage[]> task = Task.Factory.StartNew(() => (from p in _packages.Repo.GetPackages() where p.IsLatestVersion && (p.Tags.Contains("DotSpatialSampleProject") || p.Tags.Contains("SampleProject")) select p).ToArray());
            task.ContinueWith(
                t =>
                    {
                        uxOnlineProjects.Items.Clear();
                        if (t.Exception == null)
                        {
                            uxOnlineProjects.Items.AddRange(t.Result);
                            return;
                        }

                        uxOnlineProjects.Items.Add(t.Exception.Message);
                    },
                TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void UxFeedSelectionSelectedIndexChanged(object sender, EventArgs e)
        {
            string feedUrl = uxFeedSelection.SelectedIndex == 1 ? "https://nuget.org/api/v2/" : "https://www.myget.org/F/cuahsi/";
            _packages.SetNewSource(feedUrl);
            UpdatePackageList();
        }

        private void UxOnlineProjectsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (uxOnlineProjects.Items.Count == 0)
            {
                btnInstall.Enabled = false;
                return;
            }

            IPackage package = uxOnlineProjects.SelectedItem as IPackage;
            if (package == null)
            {
                btnInstall.Enabled = false;
                return;
            }

            if (IsPackageInstalled(package))
            {
                btnInstall.Enabled = false;
                return;
            }

            btnInstall.Enabled = true;
        }

        #endregion
    }
}