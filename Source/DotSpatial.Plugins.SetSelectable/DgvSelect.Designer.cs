using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Plugins.SetSelectable.Properties;

namespace DotSpatial.Plugins.SetSelectable
{
    partial class DgvSelect
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private IContainer components = null;
        internal DataGridView DGV_Layer;
        internal ToolStripContainer ToolStripContainer1;
        internal ToolStrip ToolStrip1;
        internal ToolStripButton TSB_CheckAll;
        internal ToolStripButton TSB_CheckNone;
        internal ToolStripButton TSB_SelectAll;
        internal ToolStripButton TSB_SelectNone;
        internal DataGridViewImageColumn DataGridViewImageColumn1;

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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(DgvSelect));
            this.ToolStripContainer1 = new ToolStripContainer();
            this.DGV_Layer = new DataGridView();
            this.ToolStrip1 = new ToolStrip();
            this.TSB_CheckAll = new ToolStripButton();
            this.TSB_CheckNone = new ToolStripButton();
            this.TSB_SelectAll = new ToolStripButton();
            this.TSB_SelectNone = new ToolStripButton();
            this.DataGridViewImageColumn1 = new DataGridViewImageColumn();
            this.DGVC_Selectable = new DataGridViewCheckBoxColumn();
            this.DGVC_LayerName = new DataGridViewTextBoxColumn();
            this.DGVC_Count = new DataGridViewTextBoxColumn();
            this.DGVC_Unselect = new DataGridViewImageColumn();
            this.ToolStripContainer1.ContentPanel.SuspendLayout();
            this.ToolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.ToolStripContainer1.SuspendLayout();
            ((ISupportInitialize)(this.DGV_Layer)).BeginInit();
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
            this.DGV_Layer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.DGV_Layer.BackgroundColor = Color.White;
            this.DGV_Layer.BorderStyle = BorderStyle.None;
            this.DGV_Layer.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.DGV_Layer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Layer.ColumnHeadersVisible = false;
            this.DGV_Layer.Columns.AddRange(new DataGridViewColumn[] {
            this.DGVC_Selectable,
            this.DGVC_LayerName,
            this.DGVC_Count,
            this.DGVC_Unselect});
            this.DGV_Layer.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.DGV_Layer.MultiSelect = false;
            this.DGV_Layer.Name = "DGV_Layer";
            this.DGV_Layer.RowHeadersVisible = false;
            this.DGV_Layer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Layer.CellContentClick += new DataGridViewCellEventHandler(this.DgvLayerCellContentClick);
            this.DGV_Layer.CellToolTipTextNeeded += new DataGridViewCellToolTipTextNeededEventHandler(this.DgvLayerCellToolTipTextNeeded);
            // 
            // ToolStrip1
            // 
            resources.ApplyResources(this.ToolStrip1, "ToolStrip1");
            this.ToolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            this.ToolStrip1.Items.AddRange(new ToolStripItem[] {
            this.TSB_CheckAll,
            this.TSB_CheckNone,
            this.TSB_SelectAll,
            this.TSB_SelectNone});
            this.ToolStrip1.LayoutStyle = ToolStripLayoutStyle.Flow;
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Stretch = true;
            // 
            // TSB_CheckAll
            // 
            resources.ApplyResources(this.TSB_CheckAll, "TSB_CheckAll");
            this.TSB_CheckAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.TSB_CheckAll.Image = Resources.checkall;
            this.TSB_CheckAll.Name = "TSB_CheckAll";
            this.TSB_CheckAll.Click += new EventHandler(this.TsbCheckAllClick);
            // 
            // TSB_CheckNone
            // 
            resources.ApplyResources(this.TSB_CheckNone, "TSB_CheckNone");
            this.TSB_CheckNone.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.TSB_CheckNone.Image = Resources.uncheckall;
            this.TSB_CheckNone.Name = "TSB_CheckNone";
            this.TSB_CheckNone.Click += new EventHandler(this.TsbCheckNoneClick);
            // 
            // TSB_SelectAll
            // 
            resources.ApplyResources(this.TSB_SelectAll, "TSB_SelectAll");
            this.TSB_SelectAll.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.TSB_SelectAll.Image = Resources.select_all;
            this.TSB_SelectAll.Name = "TSB_SelectAll";
            this.TSB_SelectAll.Click += new EventHandler(this.TsbSelectAllClick);
            // 
            // TSB_SelectNone
            // 
            resources.ApplyResources(this.TSB_SelectNone, "TSB_SelectNone");
            this.TSB_SelectNone.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.TSB_SelectNone.Image = Resources.select_none;
            this.TSB_SelectNone.Name = "TSB_SelectNone";
            this.TSB_SelectNone.Click += new EventHandler(this.TsbSelectNoneClick);
            // 
            // DataGridViewImageColumn1
            // 
            resources.ApplyResources(this.DataGridViewImageColumn1, "DataGridViewImageColumn1");
            this.DataGridViewImageColumn1.Image = Resources.select_none;
            this.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1";
            this.DataGridViewImageColumn1.ReadOnly = true;
            this.DataGridViewImageColumn1.Resizable = DataGridViewTriState.True;
            // 
            // DGVC_Selectable
            // 
            this.DGVC_Selectable.DataPropertyName = "DGVC_Selectable";
            this.DGVC_Selectable.FalseValue = "0";
            resources.ApplyResources(this.DGVC_Selectable, "DGVC_Selectable");
            this.DGVC_Selectable.Name = "DGVC_Selectable";
            this.DGVC_Selectable.Resizable = DataGridViewTriState.False;
            this.DGVC_Selectable.TrueValue = "1";
            // 
            // DGVC_LayerName
            // 
            this.DGVC_LayerName.DataPropertyName = "DGVC_LayerName";
            resources.ApplyResources(this.DGVC_LayerName, "DGVC_LayerName");
            this.DGVC_LayerName.Name = "DGVC_LayerName";
            this.DGVC_LayerName.ReadOnly = true;
            this.DGVC_LayerName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // DGVC_Count
            // 
            this.DGVC_Count.DataPropertyName = "DGVC_Count";
            resources.ApplyResources(this.DGVC_Count, "DGVC_Count");
            this.DGVC_Count.Name = "DGVC_Count";
            this.DGVC_Count.ReadOnly = true;
            this.DGVC_Count.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // DGVC_Unselect
            // 
            resources.ApplyResources(this.DGVC_Unselect, "DGVC_Unselect");
            this.DGVC_Unselect.Image = Resources.select_none;
            this.DGVC_Unselect.Name = "DGVC_Unselect";
            this.DGVC_Unselect.ReadOnly = true;
            this.DGVC_Unselect.Resizable = DataGridViewTriState.True;
            // 
            // DGV_Select
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.ToolStripContainer1);
            this.Name = "DGV_Select";
            this.Load += new EventHandler(this.DgvSelectLoad);
            this.ToolStripContainer1.ContentPanel.ResumeLayout(false);
            this.ToolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.ToolStripContainer1.TopToolStripPanel.PerformLayout();
            this.ToolStripContainer1.ResumeLayout(false);
            this.ToolStripContainer1.PerformLayout();
            ((ISupportInitialize)(this.DGV_Layer)).EndInit();
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewCheckBoxColumn DGVC_Selectable;
        private DataGridViewTextBoxColumn DGVC_LayerName;
        private DataGridViewTextBoxColumn DGVC_Count;
        private DataGridViewImageColumn DGVC_Unselect;




    }
}
