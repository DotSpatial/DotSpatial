using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NuGet;

namespace DotSpatial.Plugins.ExtensionManager
{
    public partial class DownloadForm : Form
    {
        private readonly Packages packages = new Packages();

        public DownloadForm()
        {
            InitializeComponent();
            uxDownloadStatus.Clear();
            progressBar.Value = 0;
            FormClosing += Download_FormClosing;
        }

        public void ShowDownloadStatus(PackageDependency dependentPackage)
        {
            if (uxDownloadStatus.InvokeRequired)
            {
                uxDownloadStatus.Invoke((Action)(() =>
                {
                    uxDownloadStatus.Text = "Downloading the dependencies\n" + "Downloading " + dependentPackage.Id;
                }));
            }
            else
            {
                uxDownloadStatus.Text = "Downloading the dependencies\n" + "Downloading " + dependentPackage.Id;
            }
        }

        public void ShowDownloadStatus(IPackage pack)
        {
            if (uxDownloadStatus.InvokeRequired)
            {
                uxDownloadStatus.Invoke((Action)(() =>
                {
                    uxDownloadStatus.Text = "Downloading " + pack.Id;
                }));
            }
            else
            {
                uxDownloadStatus.Text = "Downloading " + pack.Id;
            }
        }

        public void Show(String message)
        {
            if (uxDownloadStatus.InvokeRequired)
            {
                uxDownloadStatus.Invoke((Action)(() =>
                {
                    uxDownloadStatus.Text = message;
                }));
            }
            else
            {
                uxDownloadStatus.Text = message;
            }
        }

        public void SetProgressBarPercent(int percent)
        {
            try
            {
                if (progressBar.InvokeRequired)
                {
                    progressBar.Invoke((Action)(() =>
                    {

                        progressBar.Value = percent;


                    }));
                }
                else
                {
                        progressBar.Value = percent;       
                }
            }
            catch (ObjectDisposedException)
            {
                return;
            }
        }

        private void Download_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hide form when closed by user
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}