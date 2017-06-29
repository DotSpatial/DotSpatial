using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    partial class AttributeDialog
    {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttributeDialog));
            this.btnClose = new System.Windows.Forms.Button();
            this.tableEditorControl1 = new DotSpatial.Symbology.Forms.TableEditorControl();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
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
            this.Controls.Add(this.tableEditorControl1);
            this.Controls.Add(this.btnClose);
            this.Name = "AttributeDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private TableEditorControl tableEditorControl1;
        private Button btnClose;
    }
}
