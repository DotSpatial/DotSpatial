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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/26/2010 12:04:46 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// JoinDialog
    /// </summary>
    public class JoinDialog : Form
    {
        private IFeatureSet _featureSet;
        private ComboBox cbForeignField;
        private ComboBox cbLocalField;
        private Button cmdBrowse;
        private Button cmdCancel;
        private Button cmdOk;
        private Button cmdSave;
        private Label label1;
        private Label label2;
        private Label label3;
        private TextBox tbPath;
        private TextBox tbSave;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JoinDialog));
            this.tbPath = new System.Windows.Forms.TextBox();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.cbLocalField = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbForeignField = new System.Windows.Forms.ComboBox();
            this.cmdOk = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.tbSave = new System.Windows.Forms.TextBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbPath
            // 
            resources.ApplyResources(this.tbPath, "tbPath");
            this.tbPath.Name = "tbPath";
            this.tbPath.ReadOnly = true;
            // 
            // cmdBrowse
            // 
            resources.ApplyResources(this.cmdBrowse, "cmdBrowse");
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // cbLocalField
            // 
            resources.ApplyResources(this.cbLocalField, "cbLocalField");
            this.cbLocalField.FormattingEnabled = true;
            this.cbLocalField.Name = "cbLocalField";
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
            // cbForeignField
            // 
            resources.ApplyResources(this.cbForeignField, "cbForeignField");
            this.cbForeignField.FormattingEnabled = true;
            this.cbForeignField.Name = "cbForeignField";
            // 
            // cmdOk
            // 
            resources.ApplyResources(this.cmdOk, "cmdOk");
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.cmdOk_Click);
            // 
            // cmdCancel
            // 
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // tbSave
            // 
            resources.ApplyResources(this.tbSave, "tbSave");
            this.tbSave.Name = "tbSave";
            this.tbSave.ReadOnly = true;
            // 
            // cmdSave
            // 
            resources.ApplyResources(this.cmdSave, "cmdSave");
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // JoinDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.tbSave);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOk);
            this.Controls.Add(this.cbForeignField);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLocalField);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.tbPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JoinDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of JoinDialog
        /// </summary>
        public JoinDialog(IFeatureSet fs)
        {
            InitializeComponent();
            _featureSet = fs;
            foreach (DataColumn col in fs.GetColumns())
            {
                cbLocalField.Items.Add(col.ColumnName);
            }
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

        /// <summary>
        /// Gets the fileName of the created shapefile.
        /// </summary>
        public string Filename
        {
            get { return tbSave.Text; }
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xls";
            if (ofd.ShowDialog(this) != DialogResult.OK) return;
            tbPath.Text = ofd.FileName;
            List<string> cols = _featureSet.GetColumnNames(ofd.FileName);
            cbForeignField.Items.Clear();
            foreach (var col in cols)
            {
                cbForeignField.Items.Add(col);
            }
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            if (tbPath.Text == null)
            {
                MessageBox.Show("Select an excel file first.");
                return;
            }
            string local = (string)cbLocalField.SelectedItem;
            if (string.IsNullOrEmpty(local))
            {
                MessageBox.Show("Choose a local field to join on first.");
                return;
            }
            string foreign = (string)cbForeignField.SelectedItem;
            if (string.IsNullOrEmpty(foreign))
            {
                MessageBox.Show("Choose a foreign field to join on first.");
                return;
            }
            if (string.IsNullOrEmpty(tbSave.Text))
            {
                MessageBox.Show("Select an output file first.");
                return;
            }

            IFeatureSet temp = _featureSet.Join(tbPath.Text, local, foreign);
            temp.SaveAs(tbSave.Text, true);
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog ofd = new SaveFileDialog();
            ofd.Filter = DataManager.DefaultDataManager.VectorWriteFilter;
            if (ofd.ShowDialog(this) != DialogResult.OK) return;
            tbSave.Text = ofd.FileName;
        }
    }
}