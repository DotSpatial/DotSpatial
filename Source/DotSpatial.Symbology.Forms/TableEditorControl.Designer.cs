using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    partial class TableEditorControl
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TableEditorControl));
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblSelectedNumber = new System.Windows.Forms.Label();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.addFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableEditingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveEditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
            this.showOnlySelectedShapesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToSelectedShapesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomToShapeBeingEditedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flashSelectedShapesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelection = new System.Windows.Forms.ToolStripMenuItem();
            this.queryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSelectedFeaturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importFieldDefinitionsFromDBFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldCalculatorToolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbtnSaveEdits = new System.Windows.Forms.ToolStripButton();
            this.tsbtnZoomToSelected = new System.Windows.Forms.ToolStripButton();
            this.tsbtnShowSelected = new System.Windows.Forms.ToolStripButton();
            this.tsbtnImportFieldsFromDBF = new System.Windows.Forms.ToolStripButton();
            this.tsbtnFieldCalculator = new System.Windows.Forms.ToolStripButton();
            this.tsbtnRefreshMap = new System.Windows.Forms.ToolStripButton();
            this.tsbtnRefresh = new System.Windows.Forms.ToolStripButton();
            this.tsbtnQuery = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Controls.Add(this.lblSelectedNumber);
            this.panel1.Controls.Add(this.lblFilePath);
            this.panel1.Name = "panel1";
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            // 
            // lblSelectedNumber
            // 
            resources.ApplyResources(this.lblSelectedNumber, "lblSelectedNumber");
            this.lblSelectedNumber.Name = "lblSelectedNumber";
            // 
            // lblFilePath
            // 
            resources.ApplyResources(this.lblFilePath, "lblFilePath");
            this.lblFilePath.Name = "lblFilePath";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEdit,
            this.mnuView,
            this.mnuSelection,
            this.mnuTools});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addFieldToolStripMenuItem,
            this.removeFieldToolStripMenuItem,
            this.renameFieldToolStripMenuItem,
            this.enableEditingToolStripMenuItem,
            this.saveEditsToolStripMenuItem});
            this.mnuEdit.Name = "mnuEdit";
            resources.ApplyResources(this.mnuEdit, "mnuEdit");
            // 
            // addFieldToolStripMenuItem
            // 
            this.addFieldToolStripMenuItem.Name = "addFieldToolStripMenuItem";
            resources.ApplyResources(this.addFieldToolStripMenuItem, "addFieldToolStripMenuItem");
            this.addFieldToolStripMenuItem.Click += new System.EventHandler(this.AddFieldToolStripMenuItemClick);
            // 
            // removeFieldToolStripMenuItem
            // 
            this.removeFieldToolStripMenuItem.Name = "removeFieldToolStripMenuItem";
            resources.ApplyResources(this.removeFieldToolStripMenuItem, "removeFieldToolStripMenuItem");
            this.removeFieldToolStripMenuItem.Click += new System.EventHandler(this.RemoveFieldToolStripMenuItemClick);
            // 
            // renameFieldToolStripMenuItem
            // 
            this.renameFieldToolStripMenuItem.Name = "renameFieldToolStripMenuItem";
            resources.ApplyResources(this.renameFieldToolStripMenuItem, "renameFieldToolStripMenuItem");
            this.renameFieldToolStripMenuItem.Click += new System.EventHandler(this.RenameFieldToolStripMenuItemClick);
            // 
            // enableEditingToolStripMenuItem
            // 
            this.enableEditingToolStripMenuItem.Checked = true;
            this.enableEditingToolStripMenuItem.CheckOnClick = true;
            this.enableEditingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableEditingToolStripMenuItem.Name = "enableEditingToolStripMenuItem";
            resources.ApplyResources(this.enableEditingToolStripMenuItem, "enableEditingToolStripMenuItem");
            // 
            // saveEditsToolStripMenuItem
            // 
            this.saveEditsToolStripMenuItem.Name = "saveEditsToolStripMenuItem";
            resources.ApplyResources(this.saveEditsToolStripMenuItem, "saveEditsToolStripMenuItem");
            this.saveEditsToolStripMenuItem.Click += new System.EventHandler(this.SaveEditsToolStripMenuItemClick);
            // 
            // mnuView
            // 
            this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOnlySelectedShapesToolStripMenuItem,
            this.zoomToSelectedShapesToolStripMenuItem,
            this.zoomToShapeBeingEditedToolStripMenuItem,
            this.flashSelectedShapesToolStripMenuItem});
            this.mnuView.Name = "mnuView";
            resources.ApplyResources(this.mnuView, "mnuView");
            // 
            // showOnlySelectedShapesToolStripMenuItem
            // 
            this.showOnlySelectedShapesToolStripMenuItem.CheckOnClick = true;
            this.showOnlySelectedShapesToolStripMenuItem.Name = "showOnlySelectedShapesToolStripMenuItem";
            resources.ApplyResources(this.showOnlySelectedShapesToolStripMenuItem, "showOnlySelectedShapesToolStripMenuItem");
            this.showOnlySelectedShapesToolStripMenuItem.Click += new System.EventHandler(this.ShowOnlySelectedShapesToolStripMenuItemClick);
            // 
            // zoomToSelectedShapesToolStripMenuItem
            // 
            this.zoomToSelectedShapesToolStripMenuItem.Name = "zoomToSelectedShapesToolStripMenuItem";
            resources.ApplyResources(this.zoomToSelectedShapesToolStripMenuItem, "zoomToSelectedShapesToolStripMenuItem");
            this.zoomToSelectedShapesToolStripMenuItem.Click += new System.EventHandler(this.ZoomToSelectedShapesToolStripMenuItemClick);
            // 
            // zoomToShapeBeingEditedToolStripMenuItem
            // 
            this.zoomToShapeBeingEditedToolStripMenuItem.Name = "zoomToShapeBeingEditedToolStripMenuItem";
            resources.ApplyResources(this.zoomToShapeBeingEditedToolStripMenuItem, "zoomToShapeBeingEditedToolStripMenuItem");
            this.zoomToShapeBeingEditedToolStripMenuItem.Click += new System.EventHandler(this.ZoomToShapeBeingEditedToolStripMenuItemClick);
            // 
            // flashSelectedShapesToolStripMenuItem
            // 
            resources.ApplyResources(this.flashSelectedShapesToolStripMenuItem, "flashSelectedShapesToolStripMenuItem");
            this.flashSelectedShapesToolStripMenuItem.Name = "flashSelectedShapesToolStripMenuItem";
            this.flashSelectedShapesToolStripMenuItem.Click += new System.EventHandler(this.FlashSelectedShapesToolStripMenuItemClick);
            // 
            // mnuSelection
            // 
            this.mnuSelection.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queryToolStripMenuItem,
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem,
            this.invertSelectionToolStripMenuItem,
            this.exportSelectedFeaturesToolStripMenuItem});
            this.mnuSelection.Name = "mnuSelection";
            resources.ApplyResources(this.mnuSelection, "mnuSelection");
            // 
            // queryToolStripMenuItem
            // 
            this.queryToolStripMenuItem.Name = "queryToolStripMenuItem";
            resources.ApplyResources(this.queryToolStripMenuItem, "queryToolStripMenuItem");
            this.queryToolStripMenuItem.Click += new System.EventHandler(this.QueryToolStripMenuItemClick);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            resources.ApplyResources(this.selectAllToolStripMenuItem, "selectAllToolStripMenuItem");
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.SelectAllToolStripMenuItemClick);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            resources.ApplyResources(this.selectNoneToolStripMenuItem, "selectNoneToolStripMenuItem");
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.SelectNoneToolStripMenuItemClick);
            // 
            // invertSelectionToolStripMenuItem
            // 
            this.invertSelectionToolStripMenuItem.Name = "invertSelectionToolStripMenuItem";
            resources.ApplyResources(this.invertSelectionToolStripMenuItem, "invertSelectionToolStripMenuItem");
            this.invertSelectionToolStripMenuItem.Click += new System.EventHandler(this.InvertSelectionToolStripMenuItemClick);
            // 
            // exportSelectedFeaturesToolStripMenuItem
            // 
            this.exportSelectedFeaturesToolStripMenuItem.Name = "exportSelectedFeaturesToolStripMenuItem";
            resources.ApplyResources(this.exportSelectedFeaturesToolStripMenuItem, "exportSelectedFeaturesToolStripMenuItem");
            this.exportSelectedFeaturesToolStripMenuItem.Click += new System.EventHandler(this.ExportSelectedFeaturesToolStripMenuItemClick);
            // 
            // mnuTools
            // 
            this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.importFieldDefinitionsFromDBFToolStripMenuItem,
            this.fieldCalculatorToolToolStripMenuItem,
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem,
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem});
            this.mnuTools.Name = "mnuTools";
            resources.ApplyResources(this.mnuTools, "mnuTools");
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            resources.ApplyResources(this.findToolStripMenuItem, "findToolStripMenuItem");
            this.findToolStripMenuItem.Click += new System.EventHandler(this.FindToolStripMenuItemClick);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            resources.ApplyResources(this.replaceToolStripMenuItem, "replaceToolStripMenuItem");
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.ReplaceToolStripMenuItemClick);
            // 
            // importFieldDefinitionsFromDBFToolStripMenuItem
            // 
            this.importFieldDefinitionsFromDBFToolStripMenuItem.Name = "importFieldDefinitionsFromDBFToolStripMenuItem";
            resources.ApplyResources(this.importFieldDefinitionsFromDBFToolStripMenuItem, "importFieldDefinitionsFromDBFToolStripMenuItem");
            this.importFieldDefinitionsFromDBFToolStripMenuItem.Click += new System.EventHandler(this.ImportFieldDefinitionsFromDbfToolStripMenuItemClick);
            // 
            // fieldCalculatorToolToolStripMenuItem
            // 
            this.fieldCalculatorToolToolStripMenuItem.Name = "fieldCalculatorToolToolStripMenuItem";
            resources.ApplyResources(this.fieldCalculatorToolToolStripMenuItem, "fieldCalculatorToolToolStripMenuItem");
            this.fieldCalculatorToolToolStripMenuItem.Click += new System.EventHandler(this.FieldCalculatorToolToolStripMenuItemClick);
            // 
            // generateOrUpdateMWShapeIDFieldsToolStripMenuItem
            // 
            resources.ApplyResources(this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem, "generateOrUpdateMWShapeIDFieldsToolStripMenuItem");
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem.Name = "generateOrUpdateMWShapeIDFieldsToolStripMenuItem";
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem.Click += new System.EventHandler(this.GenerateOrUpdateMwShapeIdFieldsToolStripMenuItemClick);
            // 
            // copyShapeIDsToSpecifiedFieldToolStripMenuItem
            // 
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem.Name = "copyShapeIDsToSpecifiedFieldToolStripMenuItem";
            resources.ApplyResources(this.copyShapeIDsToSpecifiedFieldToolStripMenuItem, "copyShapeIDsToSpecifiedFieldToolStripMenuItem");
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem.Click += new System.EventHandler(this.CopyShapeIDsToSpecifiedFieldToolStripMenuItemClick);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnSaveEdits,
            this.tsbtnZoomToSelected,
            this.tsbtnShowSelected,
            this.tsbtnImportFieldsFromDBF,
            this.tsbtnFieldCalculator,
            this.tsbtnRefreshMap,
            this.tsbtnRefresh,
            this.tsbtnQuery});
            resources.ApplyResources(this.toolStrip, "toolStrip");
            this.toolStrip.Name = "toolStrip";
            // 
            // tsbtnSaveEdits
            // 
            this.tsbtnSaveEdits.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSaveEdits.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.Disk;
            resources.ApplyResources(this.tsbtnSaveEdits, "tsbtnSaveEdits");
            this.tsbtnSaveEdits.Name = "tsbtnSaveEdits";
            this.tsbtnSaveEdits.Click += new System.EventHandler(this.TsbtnSaveEditsClick);
            // 
            // tsbtnZoomToSelected
            // 
            this.tsbtnZoomToSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnZoomToSelected.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.zoom;
            resources.ApplyResources(this.tsbtnZoomToSelected, "tsbtnZoomToSelected");
            this.tsbtnZoomToSelected.Name = "tsbtnZoomToSelected";
            this.tsbtnZoomToSelected.Click += new System.EventHandler(this.TsbtnZoomToSelectedClick);
            // 
            // tsbtnShowSelected
            // 
            this.tsbtnShowSelected.CheckOnClick = true;
            this.tsbtnShowSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnShowSelected.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.Table_edit;
            resources.ApplyResources(this.tsbtnShowSelected, "tsbtnShowSelected");
            this.tsbtnShowSelected.Name = "tsbtnShowSelected";
            this.tsbtnShowSelected.Click += new System.EventHandler(this.TsbtnShowSelectedClick);
            // 
            // tsbtnImportFieldsFromDBF
            // 
            this.tsbtnImportFieldsFromDBF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnImportFieldsFromDBF.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.down;
            resources.ApplyResources(this.tsbtnImportFieldsFromDBF, "tsbtnImportFieldsFromDBF");
            this.tsbtnImportFieldsFromDBF.Name = "tsbtnImportFieldsFromDBF";
            this.tsbtnImportFieldsFromDBF.Click += new System.EventHandler(this.TsbtnImportFieldsFromDbfClick);
            // 
            // tsbtnFieldCalculator
            // 
            this.tsbtnFieldCalculator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnFieldCalculator.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.calculator;
            resources.ApplyResources(this.tsbtnFieldCalculator, "tsbtnFieldCalculator");
            this.tsbtnFieldCalculator.Name = "tsbtnFieldCalculator";
            this.tsbtnFieldCalculator.Click += new System.EventHandler(this.TsbtnFieldCalculatorClick);
            // 
            // tsbtnRefreshMap
            // 
            this.tsbtnRefreshMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRefreshMap.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.color_scheme;
            resources.ApplyResources(this.tsbtnRefreshMap, "tsbtnRefreshMap");
            this.tsbtnRefreshMap.Name = "tsbtnRefreshMap";
            this.tsbtnRefreshMap.Click += new System.EventHandler(this.TsbtnRefreshMapClick);
            // 
            // tsbtnRefresh
            // 
            this.tsbtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRefresh.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.refresh;
            resources.ApplyResources(this.tsbtnRefresh, "tsbtnRefresh");
            this.tsbtnRefresh.Name = "tsbtnRefresh";
            this.tsbtnRefresh.Click += new System.EventHandler(this.TsbtnRefreshClick);
            // 
            // tsbtnQuery
            // 
            this.tsbtnQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnQuery.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.script;
            resources.ApplyResources(this.tsbtnQuery, "tsbtnQuery");
            this.tsbtnQuery.Name = "tsbtnQuery";
            this.tsbtnQuery.Click += new System.EventHandler(this.TsbtnQueryClick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.UpdatedLblSelectedNumber);
            // 
            // TableEditorControl
            // 
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "TableEditorControl";
            resources.ApplyResources(this, "$this");
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ToolStripMenuItem addFieldToolStripMenuItem;
        private ToolStripMenuItem copyShapeIDsToSpecifiedFieldToolStripMenuItem;
        private DataGridView dataGridView1;
        private ToolStripMenuItem enableEditingToolStripMenuItem;
        private ToolStripMenuItem exportSelectedFeaturesToolStripMenuItem;
        private ToolStripMenuItem fieldCalculatorToolToolStripMenuItem;
        private ToolStripMenuItem findToolStripMenuItem;
        private ToolStripMenuItem flashSelectedShapesToolStripMenuItem;
        private ToolStripMenuItem generateOrUpdateMWShapeIDFieldsToolStripMenuItem;
        private ToolStripMenuItem importFieldDefinitionsFromDBFToolStripMenuItem;
        private ToolStripMenuItem invertSelectionToolStripMenuItem;
        private Label lblFilePath;
        private Label lblSelectedNumber;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem mnuEdit;
        private ToolStripMenuItem mnuSelection;
        private ToolStripMenuItem mnuTools;
        private ToolStripMenuItem mnuView;
        private Panel panel1;
        private ProgressBar progressBar;
        private ToolStripMenuItem queryToolStripMenuItem;
        private ToolStripMenuItem removeFieldToolStripMenuItem;
        private ToolStripMenuItem renameFieldToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private ToolStripMenuItem saveEditsToolStripMenuItem;
        private ToolStripMenuItem selectAllToolStripMenuItem;
        private ToolStripMenuItem selectNoneToolStripMenuItem;
        private ToolStripMenuItem showOnlySelectedShapesToolStripMenuItem;
        private ToolStrip toolStrip;
        private ToolStripButton tsbtnFieldCalculator;
        private ToolStripButton tsbtnImportFieldsFromDBF;
        private ToolStripButton tsbtnQuery;
        private ToolStripButton tsbtnRefresh;
        private ToolStripButton tsbtnRefreshMap;
        private ToolStripButton tsbtnSaveEdits;
        private ToolStripButton tsbtnShowSelected;
        private ToolStripButton tsbtnZoomToSelected;
        private ToolStripMenuItem zoomToSelectedShapesToolStripMenuItem;
        private ToolStripMenuItem zoomToShapeBeingEditedToolStripMenuItem;
    }
}
