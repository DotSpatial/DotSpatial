using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Plugins.SetSelectable
{
    partial class DgvSelect
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private IContainer components = null;
        internal DataGridView DgvLayer;
        internal ToolStripContainer ToolStripContainer1;
        internal ToolStrip ToolStrip1;
        internal ToolStripButton TsbCheckAll;
        internal ToolStripButton TsbCheckNone;
        internal ToolStripButton TsbSelectAll;
        internal ToolStripButton TsbSelectNone;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DgvSelect));
            this.ToolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.DgvLayer = new System.Windows.Forms.DataGridView();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.TsbCheckAll = new System.Windows.Forms.ToolStripButton();
            this.TsbCheckNone = new System.Windows.Forms.ToolStripButton();
            this.TsbSelectAll = new System.Windows.Forms.ToolStripButton();
            this.TsbSelectNone = new System.Windows.Forms.ToolStripButton();
            this.DgvcSelectable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DgvcLayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DgvcUnselect = new System.Windows.Forms.DataGridViewImageColumn();
            this.DgvcCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToolStripContainer1.ContentPanel.SuspendLayout();
            this.ToolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.ToolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvLayer)).BeginInit();
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
            this.ToolStripContainer1.ContentPanel.Controls.Add(this.DgvLayer);
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
            // DgvLayer
            // 
            resources.ApplyResources(this.DgvLayer, "DgvLayer");
            this.DgvLayer.AllowUserToAddRows = false;
            this.DgvLayer.AllowUserToDeleteRows = false;
            this.DgvLayer.AllowUserToResizeRows = false;
            this.DgvLayer.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.DgvLayer.BackgroundColor = System.Drawing.Color.White;
            this.DgvLayer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DgvLayer.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.DgvLayer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvLayer.ColumnHeadersVisible = false;
            this.DgvLayer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DgvcSelectable,
            this.DgvcLayerName,
            this.DgvcUnselect,
            this.DgvcCount});
            this.DgvLayer.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.DgvLayer.MultiSelect = false;
            this.DgvLayer.Name = "DgvLayer";
            this.DgvLayer.RowHeadersVisible = false;
            this.DgvLayer.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DgvLayer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvLayerCellContentClick);
            this.DgvLayer.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.DgvLayerCellToolTipTextNeeded);
            // 
            // ToolStrip1
            // 
            resources.ApplyResources(this.ToolStrip1, "ToolStrip1");
            this.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TsbCheckAll,
            this.TsbCheckNone,
            this.TsbSelectAll,
            this.TsbSelectNone});
            this.ToolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Stretch = true;
            // 
            // TsbCheckAll
            // 
            resources.ApplyResources(this.TsbCheckAll, "TsbCheckAll");
            this.TsbCheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TsbCheckAll.Image = global::DotSpatial.Plugins.SetSelectable.Resources.checkall;
            this.TsbCheckAll.Name = "TsbCheckAll";
            this.TsbCheckAll.Click += new System.EventHandler(this.TsbCheckAllClick);
            // 
            // TsbCheckNone
            // 
            resources.ApplyResources(this.TsbCheckNone, "TsbCheckNone");
            this.TsbCheckNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TsbCheckNone.Image = global::DotSpatial.Plugins.SetSelectable.Resources.uncheckall;
            this.TsbCheckNone.Name = "TsbCheckNone";
            this.TsbCheckNone.Click += new System.EventHandler(this.TsbCheckNoneClick);
            // 
            // TsbSelectAll
            // 
            resources.ApplyResources(this.TsbSelectAll, "TsbSelectAll");
            this.TsbSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TsbSelectAll.Image = global::DotSpatial.Plugins.SetSelectable.Resources.select_all;
            this.TsbSelectAll.Name = "TsbSelectAll";
            this.TsbSelectAll.Click += new System.EventHandler(this.TsbSelectAllClick);
            // 
            // TsbSelectNone
            // 
            resources.ApplyResources(this.TsbSelectNone, "TsbSelectNone");
            this.TsbSelectNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.TsbSelectNone.Image = global::DotSpatial.Plugins.SetSelectable.Resources.select_none;
            this.TsbSelectNone.Name = "TsbSelectNone";
            this.TsbSelectNone.Click += new System.EventHandler(this.TsbSelectNoneClick);
            // 
            // DgvcSelectable
            // 
            this.DgvcSelectable.DataPropertyName = "DgvcSelectable";
            this.DgvcSelectable.FalseValue = "0";
            resources.ApplyResources(this.DgvcSelectable, "DgvcSelectable");
            this.DgvcSelectable.Name = "DgvcSelectable";
            this.DgvcSelectable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DgvcSelectable.TrueValue = "1";
            // 
            // DgvcLayerName
            // 
            this.DgvcLayerName.DataPropertyName = "DgvcLayerName";
            resources.ApplyResources(this.DgvcLayerName, "DgvcLayerName");
            this.DgvcLayerName.Name = "DgvcLayerName";
            this.DgvcLayerName.ReadOnly = true;
            this.DgvcLayerName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DgvcUnselect
            // 
            resources.ApplyResources(this.DgvcUnselect, "DgvcUnselect");
            this.DgvcUnselect.Image = global::DotSpatial.Plugins.SetSelectable.Resources.select_none;
            this.DgvcUnselect.Name = "DgvcUnselect";
            this.DgvcUnselect.ReadOnly = true;
            this.DgvcUnselect.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // DgvcCount
            // 
            this.DgvcCount.DataPropertyName = "DgvcCount";
            resources.ApplyResources(this.DgvcCount, "DgvcCount");
            this.DgvcCount.Name = "DgvcCount";
            this.DgvcCount.ReadOnly = true;
            this.DgvcCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DgvSelect
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ToolStripContainer1);
            this.Name = "DgvSelect";
            this.Load += new System.EventHandler(this.DgvSelectLoad);
            this.ToolStripContainer1.ContentPanel.ResumeLayout(false);
            this.ToolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.ToolStripContainer1.TopToolStripPanel.PerformLayout();
            this.ToolStripContainer1.ResumeLayout(false);
            this.ToolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvLayer)).EndInit();
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }




        #endregion

        private DataGridViewCheckBoxColumn DgvcSelectable;
        private DataGridViewTextBoxColumn DgvcLayerName;
        private DataGridViewImageColumn DgvcUnselect;
        private DataGridViewTextBoxColumn DgvcCount;
    }
}
