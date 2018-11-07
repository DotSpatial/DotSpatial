using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    partial class LegendSettingsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;
        System.ComponentModel.ComponentResourceManager resources;

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
            resources = new System.ComponentModel.ComponentResourceManager(typeof(LegendSettingsDialog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvLayer = new System.Windows.Forms.DataGridView();
            this.dgvcShow = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvcLayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkShowLegendMenus = new System.Windows.Forms.CheckBox();
            this.chkEditLegendBoxes = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLayer)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // dgvLayer
            // 
            this.dgvLayer.AllowUserToAddRows = false;
            this.dgvLayer.AllowUserToDeleteRows = false;
            this.dgvLayer.AllowUserToResizeColumns = false;
            this.dgvLayer.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvLayer, "dgvLayer");
            this.dgvLayer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLayer.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLayer.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLayer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLayer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcShow,
            this.dgvcLayerName});
            this.dgvLayer.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLayer.GridColor = System.Drawing.Color.White;
            this.dgvLayer.MultiSelect = false;
            this.dgvLayer.Name = "dgvLayer";
            this.dgvLayer.ReadOnly = true;
            this.dgvLayer.RowHeadersVisible = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvLayer.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLayer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLayer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvLayerCellContentClick);
            // 
            // dgvcShow
            // 
            this.dgvcShow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvcShow.DataPropertyName = "Showable";
            this.dgvcShow.FalseValue = "0";
            resources.ApplyResources(this.dgvcShow, "dgvcShow");
            this.dgvcShow.Name = "dgvcShow";
            this.dgvcShow.ReadOnly = true;
            this.dgvcShow.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvcShow.TrueValue = "1";
            // 
            // dgvcLayerName
            // 
            this.dgvcLayerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcLayerName.DataPropertyName = "LayerName";
            resources.ApplyResources(this.dgvcLayerName, "dgvcLayerName");
            this.dgvcLayerName.Name = "dgvcLayerName";
            this.dgvcLayerName.ReadOnly = true;
            // 
            // chkShowLegendMenus
            // 
            resources.ApplyResources(this.chkShowLegendMenus, "chkShowLegendMenus");
            this.chkShowLegendMenus.Name = "chkShowLegendMenus";
            this.chkShowLegendMenus.UseVisualStyleBackColor = true;
            // 
            // chkEditLegendBoxes
            // 
            resources.ApplyResources(this.chkEditLegendBoxes, "chkEditLegendBoxes");
            this.chkEditLegendBoxes.Name = "chkEditLegendBoxes";
            this.chkEditLegendBoxes.UseVisualStyleBackColor = true;
            // 
            // LegendSettingsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkShowLegendMenus);
            this.Controls.Add(this.chkEditLegendBoxes);
            this.Controls.Add(this.dgvLayer);
            this.Controls.Add(this.btnSave);
            this.Name = "LegendSettingsDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.dgvLayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnSave;
        internal DataGridView dgvLayer;

        private void UpdateRessources()
        {
            resources.ApplyResources(this.chkShowLegendMenus, "chkShowLegendMenus");
            resources.ApplyResources(this.chkEditLegendBoxes, "chkEditLegendBoxes");
            resources.ApplyResources(this.btnSave, "btnSave");
            resources.ApplyResources(this.dgvcShow, "dgvcShow");
            resources.ApplyResources(this.dgvcLayerName, "dgvcLayerName");
            this.Text = resources.GetString("SettingsText");
        }

        private DataGridViewCheckBoxColumn dgvcShow;
        private DataGridViewTextBoxColumn dgvcLayerName;
        private CheckBox chkShowLegendMenus;
        private CheckBox chkEditLegendBoxes;
    }
}