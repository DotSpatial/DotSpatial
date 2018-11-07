using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Plugins.SpatiaLite
{
    partial class FrmAddLayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAddLayer));
            this.label1 = new System.Windows.Forms.Label();
            this.dgGeometryColumns = new System.Windows.Forms.DataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgGeometryColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // dgGeometryColumns
            // 
            this.dgGeometryColumns.AllowUserToAddRows = false;
            this.dgGeometryColumns.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.dgGeometryColumns, "dgGeometryColumns");
            this.dgGeometryColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgGeometryColumns.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgGeometryColumns.Name = "dgGeometryColumns";
            this.dgGeometryColumns.ReadOnly = true;
            this.dgGeometryColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // FrmAddLayer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.dgGeometryColumns);
            this.Controls.Add(this.label1);
            this.Name = "FrmAddLayer";
            ((System.ComponentModel.ISupportInitialize)(this.dgGeometryColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private DataGridView dgGeometryColumns;
        private Button btnOK;
        private Label label2;
    }
}