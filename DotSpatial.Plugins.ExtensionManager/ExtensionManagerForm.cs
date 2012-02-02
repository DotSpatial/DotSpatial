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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Controls;
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
        }

        #endregion

        #region Public Properties

        public AppManager App { get; set; }

        #endregion

        #region Methods

        private void InstallButton_Click(object sender, EventArgs e)
        {
            if (uxPackages.SelectedItem != null)
            {
                installButton.Enabled = false;
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

                                          if (extensions.Any() && !App.EnsureRequiredImportsAreAvailable())
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

        private void UpdatePackageList()
        {
            uxPackages.Items.Add("Loading...");
            var task = Task.Factory.StartNew(() =>
                                                 {
                                                     var result = packages.Repo.GetPackages().Where(p => p.IsLatestVersion && (p.Tags == null || !p.Tags.Contains(HideReleaseFromEndUser))).ToArray();
                                                     return result;
                                                 });

            task.ContinueWith((t) =>
                                  {
                                      uxPackages.Items.Clear();
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

                if (App.Extensions.Where(t => t.Name == pack.Id).FirstOrDefault() == null)
                    //todo: add an uninstall button.
                    installButton.Enabled = true;
                else
                    installButton.Enabled = false;
            }
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
            if (Directory.Exists(AppManager.AbsolutePathToExtensions))
                Process.Start(AppManager.AbsolutePathToExtensions);
        }

        #endregion
    }
}