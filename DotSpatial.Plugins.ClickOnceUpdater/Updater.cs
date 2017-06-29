// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Updater.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
    /// An updater that works for applications deployed using ClickOnce
    /// </summary>
    public class Updater : Extension
    {
        #region Constants and Fields

        private UpdateCheckInfo versionInfo;

        #endregion

        #region Public Methods

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
            UpdateProgress("Checking for updates.");

            // http://blogs.msdn.com/b/ukadc/archive/2010/09/16/taming-clickonce-taking-charge-of-updates.aspx
            // Why use the ThreadPool instead of CheckForUpdateAsync?
            // Some network conditions, e.g. Hotel or Guest Wi-Fi, can
            // make ClickOnce throw internally in such a way the exception
            // is tricky to catch. Instead, use CheckForUpdates (sync)
            // and handle the exception directly.
            ThreadPool.QueueUserWorkItem(delegate
                                             {
                                                 try
                                                 {
                                                     var deployment = ApplicationDeployment.CurrentDeployment;

                                                     // http://msdn.microsoft.com/en-us/library/ms404263(VS.80).aspx
                                                     try
                                                     {
                                                         versionInfo = deployment.CheckForDetailedUpdate();
                                                     }
                                                     catch (DeploymentDownloadException dde)
                                                     {
                                                         MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                                                         return;
                                                     }
                                                     catch (InvalidDeploymentException ide)
                                                     {
                                                         MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                                                         return;
                                                     }
                                                     catch (InvalidOperationException ioe)
                                                     {
                                                         MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                                                         return;
                                                     }

                                                     if (versionInfo.UpdateAvailable)
                                                     {
                                                         UpdateProgress("Update Found.");
                                                         if (versionInfo.IsUpdateRequired)
                                                         {
                                                             // Display a message that the app MUST reboot. Display the minimum required version.
                                                             MessageBox.Show("This application has detected a mandatory update from your current " +
                                                                             "version to version " + versionInfo.MinimumRequiredVersion +
                                                                             ". The application will now install the update and restart.",
                                                                             "Update Available", MessageBoxButtons.OK,
                                                                             MessageBoxIcon.Information);
                                                         }

                                                         deployment.CheckForUpdateProgressChanged += deployment_CheckForUpdateProgressChanged;
                                                         deployment.UpdateCompleted += deployment_UpdateCompleted;
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
            App.ProgressHandler.Progress("Updating", 0, message);
        }

        private void deployment_CheckForUpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            App.ProgressHandler.Progress("Updating", e.ProgressPercentage, "Updating...");
        }

        private void deployment_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Trace.WriteLine(e.Error);
            }

            if (versionInfo.IsUpdateRequired)
            {
                MessageBox.Show("The application has been upgraded, and will now restart.");
                Application.Restart();
            }
            else
            {
                UpdateProgress("Update installed.");
            }
        }

        #endregion
    }
}