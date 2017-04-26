using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace DotSpatial.Plugins.ClickOnceUpdater
{
    /// <summary>
    /// An updater that works for applications deployed using ClickOnce.
    /// </summary>
    public class Updater : Extension
    {
        #region Constants and Fields

        private UpdateCheckInfo _versionInfo;

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public override void Activate()
        {
            StartUpdating();
            base.Activate();
        }

        /// <summary>
        /// Starts the ClickOnce updating.
        /// </summary>
        public void StartUpdating()
        {
            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                return;
            }

            UpdateProgress(MessageStrings.CheckingForUpdates);

            // http://blogs.msdn.com/b/ukadc/archive/2010/09/16/taming-clickonce-taking-charge-of-updates.aspx
            // Why use the ThreadPool instead of CheckForUpdateAsync?
            // Some network conditions, e.g. Hotel or Guest Wi-Fi, can make ClickOnce throw internally in such a way the exception
            // is tricky to catch. Instead, use CheckForUpdates (sync) and handle the exception directly.
            ThreadPool.QueueUserWorkItem(delegate
                                             {
                                                 try
                                                 {
                                                     var deployment = ApplicationDeployment.CurrentDeployment;

                                                     // http://msdn.microsoft.com/en-us/library/ms404263(VS.80).aspx
                                                     try
                                                     {
                                                         _versionInfo = deployment.CheckForDetailedUpdate();
                                                     }
                                                     catch (DeploymentDownloadException dde)
                                                     {
                                                         MessageBox.Show(string.Format(MessageStrings.CantDownloadNewVersion, dde.Message));
                                                         return;
                                                     }
                                                     catch (InvalidDeploymentException ide)
                                                     {
                                                         MessageBox.Show(string.Format(MessageStrings.CantCheckForNewVersionClickOnceDeploymentIsCorrupt, ide.Message));
                                                         return;
                                                     }
                                                     catch (InvalidOperationException ioe)
                                                     {
                                                         MessageBox.Show(string.Format(MessageStrings.CantUpdateLikelyNotAClickOnceApplicationError, ioe.Message));
                                                         return;
                                                     }

                                                     if (_versionInfo.UpdateAvailable)
                                                     {
                                                         UpdateProgress(MessageStrings.UpdateFound);
                                                         if (_versionInfo.IsUpdateRequired)
                                                         {
                                                             // Display a message that the app MUST reboot. Display the minimum required version.
                                                             MessageBox.Show(
                                                                 string.Format(MessageStrings.DetectedMandatoryUpdateWillNowInstallAndRestart, _versionInfo.MinimumRequiredVersion),
                                                                 MessageStrings.UpdateAvailable,
                                                                 MessageBoxButtons.OK,
                                                                 MessageBoxIcon.Information);
                                                         }

                                                         deployment.CheckForUpdateProgressChanged += DeploymentCheckForUpdateProgressChanged;
                                                         deployment.UpdateCompleted += DeploymentUpdateCompleted;
                                                         deployment.UpdateAsync();
                                                     }
                                                     else
                                                     {
                                                         UpdateProgress("No updates.");
                                                     }
                                                 }
                                                 catch (Exception ex)
                                                 {
                                                     Trace.WriteLine(ex.Message);
                                                 }
                                             });
        }

        #endregion

        #region Methods

        private void UpdateProgress(string message)
        {
            App.ProgressHandler.Progress(MessageStrings.Updating, 0, message);
        }

        private void DeploymentCheckForUpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            App.ProgressHandler.Progress(MessageStrings.Updating, e.ProgressPercentage, MessageStrings.Updating + "...");
        }

        private void DeploymentUpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Trace.WriteLine(e.Error);
            }

            if (_versionInfo.IsUpdateRequired)
            {
                MessageBox.Show(MessageStrings.ApplicationUpgradeRestartNow);
                Application.Restart();
            }
            else
            {
                UpdateProgress(MessageStrings.UpdateInstalled);
            }
        }

        #endregion
    }
}