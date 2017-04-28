using System;
using System.Windows.Forms;
using DotSpatial.Plugins.ExtensionManager.Properties;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// DownloadForm
    /// </summary>
    internal partial class DownloadForm : Form
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadForm"/> class.
        /// </summary>
        public DownloadForm()
        {
            InitializeComponent();
            uxDownloadStatus.Clear();
            progressBar.Value = 0;
            FormClosing += DownloadFormClosing;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the progressbar to the given percentage.
        /// </summary>
        /// <param name="percent">Percent to update the progressbar.</param>
        public void SetProgressBarPercent(int percent)
        {
            try
            {
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke((Action)(() => { progressBar.Value = percent; }));
                }
                else
                {
                    progressBar.Value = percent;
                }
            }
            catch (ObjectDisposedException)
            {
            }
        }

        /// <summary>
        /// Shows the given message.
        /// </summary>
        /// <param name="message">Message that should be shown.</param>
        public void Show(string message)
        {
            if (uxDownloadStatus.InvokeRequired)
            {
                uxDownloadStatus.Invoke((Action)(() => { uxDownloadStatus.Text = message; }));
            }
            else
            {
                uxDownloadStatus.Text = message;
            }
        }

        /// <summary>
        /// Shows the download status.
        /// </summary>
        /// <param name="dependentPackage">Package whose status gets shown.</param>
        public void ShowDownloadStatus(PackageDependency dependentPackage)
        {
            if (uxDownloadStatus.InvokeRequired)
            {
                uxDownloadStatus.Invoke((Action)(() => { uxDownloadStatus.Text = Resources.DownloadingTheDependencies + string.Format(Resources.Downloading, dependentPackage.Id); }));
            }
            else
            {
                uxDownloadStatus.Text = Resources.DownloadingTheDependencies + string.Format(Resources.Downloading, dependentPackage.Id);
            }
        }

        /// <summary>
        /// Shows the download status of the given package.
        /// </summary>
        /// <param name="pack">Package to show download status for.</param>
        public void ShowDownloadStatus(IPackage pack)
        {
            if (uxDownloadStatus.InvokeRequired)
            {
                uxDownloadStatus.Invoke((Action)(() => { uxDownloadStatus.Text = string.Format(Resources.Downloading, pack.Id); }));
            }
            else
            {
                uxDownloadStatus.Text = string.Format(Resources.Downloading, pack.Id);
            }
        }

        private void Button1Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DownloadFormClosing(object sender, FormClosingEventArgs e)
        {
            // Hide form when closed by user
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        #endregion
    }
}