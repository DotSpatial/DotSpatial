// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
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
// The Initial Developer of this Original Code is Ted Dunsford. Created in Fall 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// Jiri Kadlec (2009-10-30) The attribute Table editor has been moved to a separate user control
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Atrribute Table editor form
    /// </summary>
    public class AttributeDialog : Form
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AttributeDialog));
            this.btnClose = new Button();
            this.tableEditorControl1 = new TableEditorControl();
            this.SuspendLayout();
            //
            // btnClose
            //
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.btnClose_Click_1);
            //
            // tableEditorControl1
            //
            resources.ApplyResources(this.tableEditorControl1, "tableEditorControl1");
            this.tableEditorControl1.IgnoreSelectionChanged = false;
            this.tableEditorControl1.IsEditable = true;
            this.tableEditorControl1.Name = "tableEditorControl1";
            this.tableEditorControl1.ShowFileName = true;
            this.tableEditorControl1.ShowMenuStrip = true;
            this.tableEditorControl1.ShowProgressBar = false;
            this.tableEditorControl1.ShowSelectedRowsOnly = false;
            this.tableEditorControl1.ShowToolStrip = true;
            //
            // AttributeDialog
            //
            resources.ApplyResources(this, "$this");
            //
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tableEditorControl1);
            this.Name = "AttributeDialog";
            this.ResumeLayout(false);
        }

        #endregion

        #region Variables

        private Button btnClose;
        private TableEditorControl tableEditorControl1;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the attribute Table editor form
        /// <param name="featureLayer">The feature layer associated with
        /// this instance and displayed in the editor</param>
        /// </summary>
        public AttributeDialog(IFeatureLayer featureLayer)
        {
            InitializeComponent();
            if (featureLayer != null)
            {
                tableEditorControl1.FeatureLayer = featureLayer;
            }
        }

        #endregion

        #region Event Handlers

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Protected Methods

        #endregion
    }
}