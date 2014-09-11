namespace DotSpatial.Plugins.SetSelectable
{
    partial class DGV_Select
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.DataGridView DGV_Layer;
        internal System.Windows.Forms.ToolStripContainer ToolStripContainer1;
        internal System.Windows.Forms.ToolStrip ToolStrip1;
        internal System.Windows.Forms.ToolStripButton TSB_CheckAll;
        internal System.Windows.Forms.ToolStripButton TSB_CheckNone;
        internal System.Windows.Forms.ToolStripButton TSB_SelectAll;
        internal System.Windows.Forms.ToolStripButton TSB_SelectNone;
        internal System.Windows.Forms.DataGridViewImageColumn DataGridViewImageColumn1;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DGV_Select));
            this.ToolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.DGV_Layer = new System.Windows.Forms.DataGridView();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TSB_CheckAll = new System.Windows.Forms.ToolStripButton();
            this.TSB_CheckNone = new System.Windows.Forms.ToolStripButton();
            this.TSB_SelectAll = new System.Windows.Forms.ToolStripButton();
            this.TSB_SelectNone = new System.Windows.Forms.ToolStripButton();
            this.DataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.DGVC_Selectable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DGVC_LayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGVC_Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DGVC_Unselect = new System.Windows.Forms.DataGridViewImageColumn();
            this.ToolStripContainer1.ContentPanel.SuspendLayout();
            this.ToolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.ToolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Layer)).BeginInit();
            this.ToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ToolStripContainer1
            // 
            resources.ApplyResources(this.ToolStripContainer1, "ToolStripContainer1");
            // 
            // ToolStripContainer1.BottomToolStripPanel
            // 
            resources.ApplyResources(this.ToolStripContainer1.BottomToolStripPanel, "ToolStripContainer1.BottomToolStripPanel");
            // 
            // ToolStripContainer1.ContentPanel
            // 
            resources.ApplyResources(this.ToolStripContainer1.ContentPanel, "ToolStripContainer1.ContentPanel");
            this.ToolStripContainer1.ContentPanel.Controls.Add(this.DGV_Layer);
            // 
            // ToolStripContainer1.LeftToolStripPanel
            // 
            resources.ApplyResources(this.ToolStripContainer1.LeftToolStripPanel, "ToolStripContainer1.LeftToolStripPanel");
            this.ToolStripContainer1.Name = "ToolStripContainer1";
            // 
            // ToolStripContainer1.RightToolStripPanel
            // 
            resources.ApplyResources(this.ToolStripContainer1.RightToolStripPanel, "ToolStripContainer1.RightToolStripPanel");
            // 
            // ToolStripContainer1.TopToolStripPanel
            // 
            resources.ApplyResources(this.ToolStripContainer1.TopToolStripPanel, "ToolStripContainer1.TopToolStripPanel");
            this.ToolStripContainer1.TopToolStripPanel.Controls.Add(this.ToolStrip1);
            // 
            // DGV_Layer
            // 
            resources.ApplyResources(this.DGV_Layer, "DGV_Layer");
            this.DGV_Layer.AllowUserToAddRows = false;
            this.DGV_Layer.AllowUserToDeleteRows = false;
            this.DGV_Layer.AllowUserToResizeRows = false;
            this.DGV_Layer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.DGV_Layer.BackgroundColor = System.Drawing.Color.White;
            this.DGV_Layer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_Layer.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.DGV_Layer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Layer.ColumnHeadersVisible = false;
            this.DGV_Layer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DGVC_Selectable,
            this.DGVC_LayerName,
            this.DGVC_Count,
            this.DGVC_Unselect});
            this.DGV_Layer.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DGV_Layer.MultiSelect = false;
            this.DGV_Layer.Name = "DGV_Layer";
            this.DGV_Layer.RowHeadersVisible = false;
            this.DGV_Layer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Layer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_Layer_CellContentClick);
            this.DGV_Layer.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.DGV_Layer_CellToolTipTextNeeded);
            // 
            // ToolStrip1
            // 
            resources.ApplyResources(this.ToolStrip1, "ToolStrip1");
            this.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSB_CheckAll,
            this.TSB_CheckNone,
            this.TSB_SelectAll,
            this.TSB_SelectNone});
            this.ToolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Stretch = true;
            // 
            // TSB_CheckAll
            // 
            resources.ApplyResources(this.TSB_CheckAll, "TSB_CheckAll");
            this.TSB_CheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_CheckAll.Image = global::DotSpatial.Plugins.SetSelectable.Properties.Resources.checkall;
            this.TSB_CheckAll.Name = "TSB_CheckAll";
            this.TSB_CheckAll.Click += new System.EventHandler(this.TSB_CheckAll_Click);
            // 
            // TSB_CheckNone
            // 
            resources.ApplyResources(this.TSB_CheckNone, "TSB_CheckNone");
            this.TSB_CheckNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_CheckNone.Image = global::DotSpatial.Plugins.SetSelectable.Properties.Resources.uncheckall;
            this.TSB_CheckNone.Name = "TSB_CheckNone";
            this.TSB_CheckNone.Click += new System.EventHandler(this.TSB_CheckNone_Click);
            // 
            // TSB_SelectAll
            // 
            resources.ApplyResources(this.TSB_SelectAll, "TSB_SelectAll");
            this.TSB_SelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_SelectAll.Image = global::DotSpatial.Plugins.SetSelectable.Properties.Resources.select_all;
            this.TSB_SelectAll.Name = "TSB_SelectAll";
            this.TSB_SelectAll.Click += new System.EventHandler(this.TSB_SelectAll_Click);
            // 
            // TSB_SelectNone
            // 
            resources.ApplyResources(this.TSB_SelectNone, "TSB_SelectNone");
            this.TSB_SelectNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TSB_SelectNone.Image = global::DotSpatial.Plugins.SetSelectable.Properties.Resources.select_none;
            this.TSB_SelectNone.Name = "TSB_SelectNone";
            this.TSB_SelectNone.Click += new System.EventHandler(this.TSB_SelectNone_Click);
            // 
            // DataGridViewImageColumn1
            // 
            resources.ApplyResources(this.DataGridViewImageColumn1, "DataGridViewImageColumn1");
            this.DataGridViewImageColumn1.Image = global::DotSpatial.Plugins.SetSelectable.Properties.Resources.select_none;
            this.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1";
            this.DataGridViewImageColumn1.ReadOnly = true;
            this.DataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // DGVC_Selectable
            // 
            this.DGVC_Selectable.DataPropertyName = "DGVC_Selectable";
            this.DGVC_Selectable.FalseValue = "0";
            resources.ApplyResources(this.DGVC_Selectable, "DGVC_Selectable");
            this.DGVC_Selectable.Name = "DGVC_Selectable";
            this.DGVC_Selectable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DGVC_Selectable.TrueValue = "1";
            // 
            // DGVC_LayerName
            // 
            this.DGVC_LayerName.DataPropertyName = "DGVC_LayerName";
            resources.ApplyResources(this.DGVC_LayerName, "DGVC_LayerName");
            this.DGVC_LayerName.Name = "DGVC_LayerName";
            this.DGVC_LayerName.ReadOnly = true;
            this.DGVC_LayerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DGVC_Count
            // 
            this.DGVC_Count.DataPropertyName = "DGVC_Count";
            resources.ApplyResources(this.DGVC_Count, "DGVC_Count");
            this.DGVC_Count.Name = "DGVC_Count";
            this.DGVC_Count.ReadOnly = true;
            this.DGVC_Count.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DGVC_Unselect
            // 
            resources.ApplyResources(this.DGVC_Unselect, "DGVC_Unselect");
            this.DGVC_Unselect.Image = global::DotSpatial.Plugins.SetSelectable.Properties.Resources.select_none;
            this.DGVC_Unselect.Name = "DGVC_Unselect";
            this.DGVC_Unselect.ReadOnly = true;
            this.DGVC_Unselect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // DGV_Select
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ToolStripContainer1);
            this.Name = "DGV_Select";
            this.Load += new System.EventHandler(this.DGV_Select_Load);
            this.ToolStripContainer1.ContentPanel.ResumeLayout(false);
            this.ToolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.ToolStripContainer1.TopToolStripPanel.PerformLayout();
            this.ToolStripContainer1.ResumeLayout(false);
            this.ToolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Layer)).EndInit();
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridViewCheckBoxColumn DGVC_Selectable;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGVC_LayerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DGVC_Count;
        private System.Windows.Forms.DataGridViewImageColumn DGVC_Unselect;




    }
}
