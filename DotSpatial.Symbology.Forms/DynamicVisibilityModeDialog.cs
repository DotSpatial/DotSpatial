// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/7/2010 9:23:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DynamicVisibilityModeDialog
    /// </summary>
    public class DynamicVisibilityModeDialog : Form
    {
        private DynamicVisibilityMode _dynamicVisiblityMode;
        private Button btnZoomedIn;
        private Button btnZoomedOut;
        private Label imgZoomedIn;
        private Label imgZoomedOut;
        private Label label1;
        private Button btnAlways;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynamicVisibilityModeDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.btnZoomedIn = new System.Windows.Forms.Button();
            this.btnZoomedOut = new System.Windows.Forms.Button();
            this.imgZoomedOut = new System.Windows.Forms.Label();
            this.imgZoomedIn = new System.Windows.Forms.Label();
            this.btnAlways = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnZoomedIn
            // 
            resources.ApplyResources(this.btnZoomedIn, "btnZoomedIn");
            this.btnZoomedIn.Name = "btnZoomedIn";
            this.btnZoomedIn.UseVisualStyleBackColor = true;
            this.btnZoomedIn.Click += new System.EventHandler(this.btnZoomedIn_Click);
            // 
            // btnZoomedOut
            // 
            resources.ApplyResources(this.btnZoomedOut, "btnZoomedOut");
            this.btnZoomedOut.Name = "btnZoomedOut";
            this.btnZoomedOut.UseVisualStyleBackColor = true;
            this.btnZoomedOut.Click += new System.EventHandler(this.btnZoomedOut_Click);
            // 
            // imgZoomedOut
            // 
            resources.ApplyResources(this.imgZoomedOut, "imgZoomedOut");
            this.imgZoomedOut.Name = "imgZoomedOut";
            // 
            // imgZoomedIn
            // 
            resources.ApplyResources(this.imgZoomedIn, "imgZoomedIn");
            this.imgZoomedIn.Name = "imgZoomedIn";
            // 
            // btnAlways
            // 
            resources.ApplyResources(this.btnAlways, "btnAlways");
            this.btnAlways.Name = "btnAlways";
            this.btnAlways.UseVisualStyleBackColor = true;
            this.btnAlways.Click += new System.EventHandler(this.btnAlways_Click);
            // 
            // DynamicVisibilityModeDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAlways);
            this.Controls.Add(this.imgZoomedIn);
            this.Controls.Add(this.imgZoomedOut);
            this.Controls.Add(this.btnZoomedOut);
            this.Controls.Add(this.btnZoomedIn);
            this.Controls.Add(this.label1);
            this.Name = "DynamicVisibilityModeDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DynamicVisibilityModeDialog
        /// </summary>
        public DynamicVisibilityModeDialog()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the dynamic visiblity mode for this dialog.
        /// This stores the result from this dialog.
        /// </summary>
        public DynamicVisibilityMode DynamicVisibilityMode
        {
            get { return _dynamicVisiblityMode; }
            set { _dynamicVisiblityMode = value; }
        }

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

        private void btnZoomedOut_Click(object sender, EventArgs e)
        {
            _dynamicVisiblityMode = DynamicVisibilityMode.ZoomedOut;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnZoomedIn_Click(object sender, EventArgs e)
        {
            _dynamicVisiblityMode = DynamicVisibilityMode.ZoomedIn;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAlways_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            Close();
        }
    }
}