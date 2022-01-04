using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ShapeEditor
{
    partial class SnapSettingsDialog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvLayer = new System.Windows.Forms.DataGridView();
            this.cbPerformSnap = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvcSnappable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvcLayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLayer)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(197, 509);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // dgvLayer
            // 
            this.dgvLayer.AllowUserToAddRows = false;
            this.dgvLayer.AllowUserToDeleteRows = false;
            this.dgvLayer.AllowUserToResizeColumns = false;
            this.dgvLayer.AllowUserToResizeRows = false;
            this.dgvLayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.dgvLayer.ColumnHeadersVisible = false;
            this.dgvLayer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcSnappable,
            this.dgvcLayerName});
            this.dgvLayer.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvLayer.GridColor = System.Drawing.Color.White;
            this.dgvLayer.Location = new System.Drawing.Point(8, 59);
            this.dgvLayer.MultiSelect = false;
            this.dgvLayer.Name = "dgvLayer";
            this.dgvLayer.ReadOnly = true;
            this.dgvLayer.RowHeadersVisible = false;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvLayer.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLayer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLayer.Size = new System.Drawing.Size(264, 437);
            this.dgvLayer.TabIndex = 2;
            this.dgvLayer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvLayerCellContentClick);
            // 
            // cbPerformSnap
            // 
            this.cbPerformSnap.AutoSize = true;
            this.cbPerformSnap.Checked = true;
            this.cbPerformSnap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPerformSnap.Location = new System.Drawing.Point(8, 12);
            this.cbPerformSnap.Name = "cbPerformSnap";
            this.cbPerformSnap.Size = new System.Drawing.Size(110, 17);
            this.cbPerformSnap.TabIndex = 0;
            this.cbPerformSnap.Text = "Perform Snapping";
            this.cbPerformSnap.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Snap to the following layers:";
            // 
            // dgvcSnappable
            // 
            this.dgvcSnappable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.dgvcSnappable.DataPropertyName = "Snappable";
            this.dgvcSnappable.FalseValue = "0";
            this.dgvcSnappable.HeaderText = "Snappable";
            this.dgvcSnappable.Name = "dgvcSnappable";
            this.dgvcSnappable.ReadOnly = true;
            this.dgvcSnappable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvcSnappable.ToolTipText = "Indicates whether the coordinates of the layers features can be snapped to.";
            this.dgvcSnappable.TrueValue = "1";
            this.dgvcSnappable.Width = 5;
            // 
            // dgvcLayerName
            // 
            this.dgvcLayerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcLayerName.DataPropertyName = "LayerName";
            this.dgvcLayerName.HeaderText = "Layer";
            this.dgvcLayerName.Name = "dgvcLayerName";
            this.dgvcLayerName.ReadOnly = true;
            // 
            // SnapSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 544);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPerformSnap);
            this.Controls.Add(this.dgvLayer);
            this.Controls.Add(this.btnSave);
            this.Name = "SnapSettingsDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Snap Settings";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnSave;
        internal DataGridView dgvLayer;
        private CheckBox cbPerformSnap;
        private Label label1;
        private DataGridViewCheckBoxColumn dgvcSnappable;
        private DataGridViewTextBoxColumn dgvcLayerName;
    }
}