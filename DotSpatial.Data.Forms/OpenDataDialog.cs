// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/10/2008 2:15:33 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// frmOpenDataDialog
    /// </summary>
    public class OpenDataDialog : Form
    {
        private DirectoryView directoryView1;

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenDataDialog));
            this.directoryView1 = new DotSpatial.Data.Forms.DirectoryView();
            this.SuspendLayout();
            //
            // directoryView1
            //
            resources.ApplyResources(this.directoryView1, "directoryView1");
            this.directoryView1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 600, 495);
            this.directoryView1.Directory = null;
            this.directoryView1.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 600, 1);
            this.directoryView1.HorizontalScrollEnabled = true;
            this.directoryView1.IsInitialized = false;
            this.directoryView1.MinimumSize = new System.Drawing.Size(5, 5);
            this.directoryView1.Name = "directoryView1";
            this.directoryView1.ResetOnResize = false;
            this.directoryView1.SelectedItem = null;
            this.directoryView1.VerticalScrollEnabled = true;
            //
            // OpenDataDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.directoryView1);
            this.Name = "OpenDataDialog";
            this.ResumeLayout(false);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of frmOpenDataDialog
        /// </summary>
        public OpenDataDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        #region Private Functions

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}