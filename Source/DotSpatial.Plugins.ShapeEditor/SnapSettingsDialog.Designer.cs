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
            resources = new System.ComponentModel.ComponentResourceManager(typeof(SnapSettingsDialog));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvLayer = new System.Windows.Forms.DataGridView();
            this.dgvcSnappable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvcLayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcSnapVertices = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvcSnapStartPoint = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvcSnapEndPoint = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvcSnapEdges = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cbPerformSnap = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLayer)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(325, 509);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
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
            this.dgvLayer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcSnappable,
            this.dgvcLayerName,
            this.dgvcSnapVertices,
            this.dgvcSnapStartPoint,
            this.dgvcSnapEndPoint,
            this.dgvcSnapEdges});
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
            this.dgvLayer.Size = new System.Drawing.Size(392, 437);
            this.dgvLayer.TabIndex = 2;
            this.dgvLayer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvLayerCellContentClick);
            // 
            // dgvcSnappable
            // 
            
            this.dgvcSnappable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvcSnappable.DataPropertyName = "Snappable";
            this.dgvcSnappable.FalseValue = "0";
            this.dgvcSnappable.Name = "dgvcSnappable";
            this.dgvcSnappable.ReadOnly = true;
            this.dgvcSnappable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvcSnappable.TrueValue = "1";
            this.dgvcSnappable.Width = 64;
            // 
            // dgvcLayerName
            // 
            
            this.dgvcLayerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcLayerName.DataPropertyName = "LayerName";
            this.dgvcLayerName.Name = "dgvcLayerName";
            this.dgvcLayerName.ReadOnly = true;
            // 
            // dgvcSnapVertices
            // 
            
            this.dgvcSnapVertices.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvcSnapVertices.DataPropertyName = "SnapVertices";
            this.dgvcSnapVertices.FalseValue = "0";
            this.dgvcSnapVertices.Name = "dgvcSnapVertices";
            this.dgvcSnapVertices.ReadOnly = true;
            this.dgvcSnapVertices.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvcSnapVertices.TrueValue = "1";
            this.dgvcSnapVertices.Width = 51;
            // 
            // dgvcSnapStartPoint
            // 
            
            this.dgvcSnapStartPoint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvcSnapStartPoint.DataPropertyName = "SnapStartPoint";
            this.dgvcSnapStartPoint.FalseValue = "0";
            this.dgvcSnapStartPoint.Name = "dgvcSnapStartPoint";
            this.dgvcSnapStartPoint.ReadOnly = true;
            this.dgvcSnapStartPoint.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvcSnapStartPoint.TrueValue = "1";
            this.dgvcSnapStartPoint.Width = 35;
            // 
            // dgvcSnapEndPoint
            // 
            
            this.dgvcSnapEndPoint.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvcSnapEndPoint.DataPropertyName = "SnapEndPoint";
            this.dgvcSnapEndPoint.FalseValue = "0";
            this.dgvcSnapEndPoint.Name = "dgvcSnapEndPoint";
            this.dgvcSnapEndPoint.ReadOnly = true;
            this.dgvcSnapEndPoint.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvcSnapEndPoint.TrueValue = "1";
            this.dgvcSnapEndPoint.Width = 32;
            // 
            // dgvcSnapEdges
            // 
            
            this.dgvcSnapEdges.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.dgvcSnapEdges.DataPropertyName = "SnapEdges";
            this.dgvcSnapEdges.FalseValue = "0";
            this.dgvcSnapEdges.Name = "dgvcSnapEdges";
            this.dgvcSnapEdges.ReadOnly = true;
            this.dgvcSnapEdges.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvcSnapEdges.TrueValue = "1";
            this.dgvcSnapEdges.Width = 43;
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
            this.cbPerformSnap.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 13);
            this.label1.TabIndex = 1;
            // 
            // SnapSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 544);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPerformSnap);
            this.Controls.Add(this.dgvLayer);
            this.Controls.Add(this.btnSave);
            this.Name = "SnapSettingsDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = resources.GetString("SettingsText"); // "Snap Settings";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnSave;
        private CheckBox cbPerformSnap;
        private Label label1;
        private DataGridViewCheckBoxColumn dgvcSnappable;
        private DataGridViewTextBoxColumn dgvcLayerName;
        private DataGridViewCheckBoxColumn dgvcSnapVertices;
        private DataGridViewCheckBoxColumn dgvcSnapStartPoint;
        private DataGridViewCheckBoxColumn dgvcSnapEndPoint;
        private DataGridViewCheckBoxColumn dgvcSnapEdges;
        internal DataGridView dgvLayer;

        private void UpdateRessources()
        {
            resources.ApplyResources(this.btnSave, "btnSave");
            resources.ApplyResources(this.dgvcSnappable, "dgvcSnappable");
            resources.ApplyResources(this.dgvcLayerName, "dgvcLayerName");
            resources.ApplyResources(this.dgvcSnapVertices, "dgvcSnapVertices");
            resources.ApplyResources(this.dgvcSnapStartPoint, "dgvcSnapStartPoint");
            resources.ApplyResources(this.dgvcSnapEndPoint, "dgvcSnapEndPoint");
            resources.ApplyResources(this.dgvcSnapEdges, "dgvcSnapEdges");
            resources.ApplyResources(this.cbPerformSnap, "cbPerformSnap");
            resources.ApplyResources(this.label1, "label1");
            this.Text = resources.GetString("SettingsText");
        }
    }
}