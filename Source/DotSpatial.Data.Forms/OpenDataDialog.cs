// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// OpenDataDialog.
    /// </summary>
    public class OpenDataDialog : Form
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly IContainer _components = null;

        private DirectoryView _directoryView1;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenDataDialog"/> class.
        /// </summary>
        public OpenDataDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _components?.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(OpenDataDialog));
            _directoryView1 = new DirectoryView();
            SuspendLayout();

            // directoryView1
            resources.ApplyResources(_directoryView1, "_directoryView1");
            _directoryView1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 600, 495);
            _directoryView1.Directory = null;
            _directoryView1.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 600, 1);
            _directoryView1.HorizontalScrollEnabled = true;
            _directoryView1.IsInitialized = false;
            _directoryView1.MinimumSize = new System.Drawing.Size(5, 5);
            _directoryView1.Name = "_directoryView1";
            _directoryView1.ResetOnResize = false;
            _directoryView1.SelectedItem = null;
            _directoryView1.VerticalScrollEnabled = true;

            // OpenDataDialog
            resources.ApplyResources(this, "$this");
            Controls.Add(_directoryView1);
            Name = "OpenDataDialog";
            ResumeLayout(false);
        }

        #endregion
    }
}