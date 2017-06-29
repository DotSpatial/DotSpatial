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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/5/2009 12:49:10 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ExportFeature
    /// </summary>
    public class ExportFeature : Form
    {
        private Button btnBrowse;
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cmbFeatureSpecification;
        private Label label1;
        private Label label2;
        private TextBox txtOutput;

        #region Private Variables

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportFeature));
            this.cmbFeatureSpecification = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbFeatureSpecification
            // 
            resources.ApplyResources(this.cmbFeatureSpecification, "cmbFeatureSpecification");
            this.cmbFeatureSpecification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFeatureSpecification.FormattingEnabled = true;
            this.cmbFeatureSpecification.Name = "cmbFeatureSpecification";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtOutput
            // 
            resources.ApplyResources(this.txtOutput, "txtOutput");
            this.txtOutput.Name = "txtOutput";
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.FolderOpen;
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ExportFeature
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbFeatureSpecification);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportFeature";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ExportFeature
        /// </summary>
        public ExportFeature()
        {
            InitializeComponent();
            cmbFeatureSpecification.Items.Add(SymbologyFormsMessageStrings.ExportFeature_FeaturesAll);
            cmbFeatureSpecification.Items.Add(SymbologyFormsMessageStrings.ExportFeature_FeaturesSelected);
            cmbFeatureSpecification.Items.Add(SymbologyFormsMessageStrings.ExportFeature_FeaturesExtent);
            cmbFeatureSpecification.SelectedIndex = 0;
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

        /// <summary>
        /// Gets the zero based integer index
        /// </summary>
        public int FeaturesIndex
        {
            get { return cmbFeatureSpecification.SelectedIndex; }
        }

        /// <summary>
        /// Gets or sets the string fileName.  Setting this will not actually use this value,
        /// but will make up a new value based on the entered value.
        /// </summary>
        public string Filename
        {
            get { return txtOutput.Text; }
            set
            {
                string baseFile = Path.GetDirectoryName(value) +
                                  Path.DirectorySeparatorChar +
                                  Path.GetFileNameWithoutExtension(value);
                int i = 1;
                string outFile = value;
                string ext = Path.GetExtension(value);

                while (File.Exists(outFile))
                {
                    outFile = baseFile + i + ext;
                    i++;
                }
                txtOutput.Text = outFile;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = DataManager.DefaultDataManager.VectorWriteFilter;
            if (ofd.ShowDialog() != DialogResult.OK) return;
            txtOutput.Text = ofd.FileName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Occurs when closing, and throws up a message box preventing "ok" with an invalid path.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel) return;
            List<string> result = DataManager.DefaultDataManager.
                GetSupportedExtensions(DataManager.DefaultDataManager.VectorWriteFilter);
            string ext = Path.GetExtension(txtOutput.Text);

            if (!result.Contains(ext))
            {
                MessageBox.Show(
                    "The path you entered does not have an extension that matches a supported format.  " +
                    "please check the filename to ensure that it has a supported extension like .shp and try again.",
                    "Invalid file format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
            base.OnClosing(e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}