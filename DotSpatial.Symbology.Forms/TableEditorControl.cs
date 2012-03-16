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
// Kandasamy Prasanna (2009-09-11) main contributor to most of the functionality
// Jiri Kadlec (2009-10-30) Changed the form into a user control, improved selection when sorting
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Symbology.Forms.Properties;
using DotSpatial.Topology;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A Table editor user control. This may be used for displaying attributes of a feature layer.
    /// </summary>
    public class TableEditorControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs whenever the user selects, de-selects or in any way updates the row selections
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Occurs whenever the user click SelectioinZoom button.
        /// </summary>
        public event EventHandler SelectionZoom;

        /// <summary>
        /// occurs whenever the user click RefreshMap button.
        /// </summary>
        public event EventHandler MapRefreshed;

        /// <summary>
        /// This will fire when user press ZoomToShapeBeingEdited button.
        /// </summary>
        public event EventHandler ZoomToShapeBeingEdited;

        #endregion

        #region Private Variables

        private readonly SQLExpressionDialog _queryDialog = new SQLExpressionDialog();
        private IFeatureLayer _featureLayer;
        private string _fidField;
        private bool _ignoreSelectionChanged;
        private bool _ignoreTableSelectionChanged;
        private bool _isEditable = true;
        private bool _loaded;
        private List<int> _selectedRows;
        private DataTable _selection;
        private List<int> _selectionIndices;
        private bool _showOnlySelectedRows;
        private bool _virtualHooked;

        #region Windows Form Designer generated code

        private AttributeCache _attributeCache;
        private ToolStripMenuItem addFieldToolStripMenuItem;
        private IContainer components = null;
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
        private Panel panel2;
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

        #endregion

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
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
            this.panel2 = new System.Windows.Forms.Panel();
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
            this.panel2.SuspendLayout();
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
            this.lblSelectedNumber.Click += new System.EventHandler(this.lblSelectedNumber_Click);
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
            this.addFieldToolStripMenuItem.Click += new System.EventHandler(this.addFieldToolStripMenuItem_Click);
            //
            // removeFieldToolStripMenuItem
            //
            this.removeFieldToolStripMenuItem.Name = "removeFieldToolStripMenuItem";
            resources.ApplyResources(this.removeFieldToolStripMenuItem, "removeFieldToolStripMenuItem");
            this.removeFieldToolStripMenuItem.Click += new System.EventHandler(this.removeFieldToolStripMenuItem_Click);
            //
            // renameFieldToolStripMenuItem
            //
            this.renameFieldToolStripMenuItem.Name = "renameFieldToolStripMenuItem";
            resources.ApplyResources(this.renameFieldToolStripMenuItem, "renameFieldToolStripMenuItem");
            this.renameFieldToolStripMenuItem.Click += new System.EventHandler(this.renameFieldToolStripMenuItem_Click);
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
            this.saveEditsToolStripMenuItem.Click += new System.EventHandler(this.saveEditsToolStripMenuItem_Click);
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
            this.showOnlySelectedShapesToolStripMenuItem.Click += new System.EventHandler(this.showOnlySelectedShapesToolStripMenuItem_Click);
            //
            // zoomToSelectedShapesToolStripMenuItem
            //
            this.zoomToSelectedShapesToolStripMenuItem.Name = "zoomToSelectedShapesToolStripMenuItem";
            resources.ApplyResources(this.zoomToSelectedShapesToolStripMenuItem, "zoomToSelectedShapesToolStripMenuItem");
            this.zoomToSelectedShapesToolStripMenuItem.Click += new System.EventHandler(this.zoomToSelectedShapesToolStripMenuItem_Click);
            //
            // zoomToShapeBeingEditedToolStripMenuItem
            //
            this.zoomToShapeBeingEditedToolStripMenuItem.Name = "zoomToShapeBeingEditedToolStripMenuItem";
            resources.ApplyResources(this.zoomToShapeBeingEditedToolStripMenuItem, "zoomToShapeBeingEditedToolStripMenuItem");
            this.zoomToShapeBeingEditedToolStripMenuItem.Click += new System.EventHandler(this.zoomToShapeBeingEditedToolStripMenuItem_Click);
            //
            // flashSelectedShapesToolStripMenuItem
            //
            resources.ApplyResources(this.flashSelectedShapesToolStripMenuItem, "flashSelectedShapesToolStripMenuItem");
            this.flashSelectedShapesToolStripMenuItem.Name = "flashSelectedShapesToolStripMenuItem";
            this.flashSelectedShapesToolStripMenuItem.Click += new System.EventHandler(this.flashSelectedShapesToolStripMenuItem_Click);
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
            this.queryToolStripMenuItem.Click += new System.EventHandler(this.queryToolStripMenuItem_Click);
            //
            // selectAllToolStripMenuItem
            //
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            resources.ApplyResources(this.selectAllToolStripMenuItem, "selectAllToolStripMenuItem");
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            //
            // selectNoneToolStripMenuItem
            //
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            resources.ApplyResources(this.selectNoneToolStripMenuItem, "selectNoneToolStripMenuItem");
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.selectNoneToolStripMenuItem_Click);
            //
            // invertSelectionToolStripMenuItem
            //
            this.invertSelectionToolStripMenuItem.Name = "invertSelectionToolStripMenuItem";
            resources.ApplyResources(this.invertSelectionToolStripMenuItem, "invertSelectionToolStripMenuItem");
            this.invertSelectionToolStripMenuItem.Click += new System.EventHandler(this.invertSelectionToolStripMenuItem_Click);
            //
            // exportSelectedFeaturesToolStripMenuItem
            //
            this.exportSelectedFeaturesToolStripMenuItem.Name = "exportSelectedFeaturesToolStripMenuItem";
            resources.ApplyResources(this.exportSelectedFeaturesToolStripMenuItem, "exportSelectedFeaturesToolStripMenuItem");
            this.exportSelectedFeaturesToolStripMenuItem.Click += new System.EventHandler(this.exportSelectedFeaturesToolStripMenuItem_Click);
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
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            //
            // replaceToolStripMenuItem
            //
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            resources.ApplyResources(this.replaceToolStripMenuItem, "replaceToolStripMenuItem");
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            //
            // importFieldDefinitionsFromDBFToolStripMenuItem
            //
            this.importFieldDefinitionsFromDBFToolStripMenuItem.Name = "importFieldDefinitionsFromDBFToolStripMenuItem";
            resources.ApplyResources(this.importFieldDefinitionsFromDBFToolStripMenuItem, "importFieldDefinitionsFromDBFToolStripMenuItem");
            this.importFieldDefinitionsFromDBFToolStripMenuItem.Click += new System.EventHandler(this.importFieldDefinitionsFromDBFToolStripMenuItem_Click);
            //
            // fieldCalculatorToolToolStripMenuItem
            //
            this.fieldCalculatorToolToolStripMenuItem.Name = "fieldCalculatorToolToolStripMenuItem";
            resources.ApplyResources(this.fieldCalculatorToolToolStripMenuItem, "fieldCalculatorToolToolStripMenuItem");
            this.fieldCalculatorToolToolStripMenuItem.Click += new System.EventHandler(this.fieldCalculatorToolToolStripMenuItem_Click);
            //
            // generateOrUpdateMWShapeIDFieldsToolStripMenuItem
            //
            resources.ApplyResources(this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem, "generateOrUpdateMWShapeIDFieldsToolStripMenuItem");
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem.Name = "generateOrUpdateMWShapeIDFieldsToolStripMenuItem";
            this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem.Click += new System.EventHandler(this.generateOrUpdateMWShapeIDFieldsToolStripMenuItem_Click);
            //
            // copyShapeIDsToSpecifiedFieldToolStripMenuItem
            //
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem.Name = "copyShapeIDsToSpecifiedFieldToolStripMenuItem";
            resources.ApplyResources(this.copyShapeIDsToSpecifiedFieldToolStripMenuItem, "copyShapeIDsToSpecifiedFieldToolStripMenuItem");
            this.copyShapeIDsToSpecifiedFieldToolStripMenuItem.Click += new System.EventHandler(this.copyShapeIDsToSpecifiedFieldToolStripMenuItem_Click);
            //
            // panel2
            //
            this.panel2.Controls.Add(this.toolStrip);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
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
            this.tsbtnSaveEdits.Click += new System.EventHandler(this.tsbtnSaveEdits_Click);
            //
            // tsbtnZoomToSelected
            //
            this.tsbtnZoomToSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnZoomToSelected.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.zoom;
            resources.ApplyResources(this.tsbtnZoomToSelected, "tsbtnZoomToSelected");
            this.tsbtnZoomToSelected.Name = "tsbtnZoomToSelected";
            this.tsbtnZoomToSelected.Click += new System.EventHandler(this.tsbtnZoomToSelected_Click);
            //
            // tsbtnShowSelected
            //
            this.tsbtnShowSelected.CheckOnClick = true;
            this.tsbtnShowSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnShowSelected.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.Table_edit;
            resources.ApplyResources(this.tsbtnShowSelected, "tsbtnShowSelected");
            this.tsbtnShowSelected.Name = "tsbtnShowSelected";
            this.tsbtnShowSelected.Click += new System.EventHandler(this.tsbtnShowSelected_Click);
            //
            // tsbtnImportFieldsFromDBF
            //
            this.tsbtnImportFieldsFromDBF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnImportFieldsFromDBF.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.down;
            resources.ApplyResources(this.tsbtnImportFieldsFromDBF, "tsbtnImportFieldsFromDBF");
            this.tsbtnImportFieldsFromDBF.Name = "tsbtnImportFieldsFromDBF";
            this.tsbtnImportFieldsFromDBF.Click += new System.EventHandler(this.tsbtnImportFieldsFromDBF_Click);
            //
            // tsbtnFieldCalculator
            //
            this.tsbtnFieldCalculator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnFieldCalculator.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.calculator;
            resources.ApplyResources(this.tsbtnFieldCalculator, "tsbtnFieldCalculator");
            this.tsbtnFieldCalculator.Name = "tsbtnFieldCalculator";
            this.tsbtnFieldCalculator.Click += new System.EventHandler(this.tsbtnFieldCalculator_Click);
            //
            // tsbtnRefreshMap
            //
            this.tsbtnRefreshMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRefreshMap.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.color_scheme;
            resources.ApplyResources(this.tsbtnRefreshMap, "tsbtnRefreshMap");
            this.tsbtnRefreshMap.Name = "tsbtnRefreshMap";
            this.tsbtnRefreshMap.Click += new System.EventHandler(this.tsbtnRefreshMap_Click);
            //
            // tsbtnRefresh
            //
            this.tsbtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRefresh.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.refresh;
            resources.ApplyResources(this.tsbtnRefresh, "tsbtnRefresh");
            this.tsbtnRefresh.Name = "tsbtnRefresh";
            this.tsbtnRefresh.Click += new System.EventHandler(this.tsbtnRefresh_Click);
            //
            // tsbtnQuery
            //
            this.tsbtnQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnQuery.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.script;
            resources.ApplyResources(this.tsbtnQuery, "tsbtnQuery");
            this.tsbtnQuery.Name = "tsbtnQuery";
            this.tsbtnQuery.Click += new System.EventHandler(this.tsbtnQuery_Click);
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
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            //
            // TableEditorControl
            //
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "TableEditorControl";
            resources.ApplyResources(this, "$this");
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void dataGridView1_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (_attributeCache.EditRowIndex != dataGridView1.CurrentCell.RowIndex)
            {
                _attributeCache.EditRowIndex = dataGridView1.CurrentCell.RowIndex;
            }
            _attributeCache.EditRow[dataGridView1.Columns[e.ColumnIndex].Name] = e.Value;
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (_attributeCache.EditRow != null)
            {
                _attributeCache.SaveChanges();
            }
            _attributeCache.EditRowIndex = -1; // this also sets the EditRow to null;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Table editor control without any data
        /// </summary>
        public TableEditorControl()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new Table editor control for editing a feature layer's attribute values. This allows interaction
        /// with the map. If a row is selected in the Table the corresponding row is selected in the map
        /// </summary>
        /// <param name="layer">The symbolizer on the map</param>
        public TableEditorControl(IFeatureLayer layer)
        {
            FeatureLayer = layer;
            Configure();
        }

        private void Configure()
        {
            InitializeComponent();
            _selectedRows = new List<int>();
            Load += TableEditorControlLoad;
            enableEditingToolStripMenuItem.CheckedChanged += EnableEditingToolStripMenuItemCheckedChanged;
            _queryDialog.ChangesApplied += QueryDialog_ChangesApplied;

            RemoveUnusedButtonsFromToolstrip();
            if (FeatureLayer != null)
                FeatureLayer.ProgressHandler = null;
        }

        private void RemoveUnusedButtonsFromToolstrip()
        {
            //removes the toolstrip buttons that are not working
            this.toolStrip.Items.Remove(tsbtnFieldCalculator);
            this.toolStrip.Items.Remove(tsbtnImportFieldsFromDBF);
            this.toolStrip.Items.Remove(tsbtnRefresh);
            this.toolStrip.Items.Remove(tsbtnRefreshMap);
            //removes the menu items that are not working
            this.mnuSelection.DropDownItems.Remove(flashSelectedShapesToolStripMenuItem);
            this.mnuSelection.DropDownItems.Remove(zoomToShapeBeingEditedToolStripMenuItem);
            this.mnuTools.DropDownItems.Remove(copyShapeIDsToSpecifiedFieldToolStripMenuItem);
            this.mnuTools.DropDownItems.Remove(importFieldDefinitionsFromDBFToolStripMenuItem);
            this.mnuTools.DropDownItems.Remove(generateOrUpdateMWShapeIDFieldsToolStripMenuItem);
            this.mnuTools.DropDownItems.Remove(fieldCalculatorToolToolStripMenuItem);
        }

        private void ParentFormShown(object sender, EventArgs e)
        {
            SetSelectionFromLayer();
            dataGridView1.SelectionChanged += DataGridView1SelectionChanged;
        }

        private void ParentFormClosing(object sender, CancelEventArgs e)
        {
            //Need to prevent the change event from running James Rineer jrin@rti.org
            _ignoreTableSelectionChanged = true;

            if (_fidField != null)
            {
                if (_featureLayer.DataSet.DataTable.Columns.Contains(_fidField))
                    _featureLayer.DataSet.DataTable.Columns.Remove(_fidField);
            }
        }

        private void DataGridView1CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = _attributeCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        //when first loaded
        private void TableEditorControlLoad(object sender, EventArgs e)
        {
            if (ParentForm == null) return;
            ParentForm.FormClosed += ParentFormFormClosed;
        }

        //when the parent form is closed
        private void ParentFormFormClosed(object sender, FormClosedEventArgs e)
        {
            _featureLayer = null;
            Dispose();
        }

        //when the selected features are changed on the feature layer
        private void SelectedFeaturesChanged(object sender, EventArgs e)
        {
            SetSelectionFromLayer();
        }

        private void SetSelectionFromLayer()
        {
            dataGridView1.SuspendLayout();
            if (_featureLayer == null) return;
            if (_ignoreSelectionChanged) return;
            _ignoreSelectionChanged = true;
            _ignoreTableSelectionChanged = true;
            if (!_featureLayer.EditMode)
            {
                FastDrawnState[] states = _featureLayer.DrawnStates;
                if (_featureLayer.DataSet.AttributesPopulated)
                {
                    _ignoreSelectionChanged = true;
                    _ignoreTableSelectionChanged = true;
                    dataGridView1.SuspendLayout();
                    if (states == null)
                    {
                        _ignoreSelectionChanged = false;
                        _ignoreTableSelectionChanged = false;
                        return;
                    }
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        int fid = (int)row.Cells[_fidField].Value;
                        row.Selected = states[fid].Selected;
                    }
                    dataGridView1.ResumeLayout();
                    _ignoreSelectionChanged = false;
                    _ignoreTableSelectionChanged = false;
                }
                else
                {
                    if (states == null)
                    {
                        return;
                    }
                    foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                    {
                        for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                        {
                            dataGridView1.Rows[row].Selected = states[row].Selected;
                        }
                    }
                }
            }
            else
            {
                IFeatureSelection fs = _featureLayer.Selection as IFeatureSelection;
                if (fs == null) return;
                if (_featureLayer.DataSet.AttributesPopulated)
                {
                    _ignoreSelectionChanged = true;
                    _ignoreTableSelectionChanged = true;
                    dataGridView1.SuspendLayout();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        int fid = (int)row.Cells[_fidField].Value;
                        row.Selected = fs.Filter.DrawnStates[_featureLayer.DataSet.Features[fid]].IsSelected;
                    }
                    dataGridView1.ResumeLayout();
                    _ignoreSelectionChanged = false;
                    _ignoreTableSelectionChanged = false;
                }
                else
                {
                    foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                    {
                        for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                        {
                            dataGridView1.Rows[row].Selected = fs.Filter.DrawnStates[_featureLayer.DataSet.Features[row]].IsSelected;
                        }
                    }
                }
            }
            _ignoreSelectionChanged = false;
            _ignoreTableSelectionChanged = false;
            dataGridView1.ResumeLayout();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Zoom to selected rows (features)
        /// </summary>
        public void ZoomToSelected()
        {
            if (_featureLayer == null) return;
            if (_featureLayer.Selection.Count == 0) return;

            _featureLayer.ZoomToSelectedFeatures();
        }

        /// <summary>
        /// Zooms to the row which is being edited
        /// </summary>
        public void ZoomToEditedRow()
        {
            if (dataGridView1.CurrentRow == null) return;
            DataRowView drv = dataGridView1.CurrentRow.DataBoundItem as DataRowView;
            if (drv == null) return;
            IFeature currentFeature = _featureLayer.DataSet.FeatureFromRow(drv.Row);
            LayerFrame frame = _featureLayer.ParentMapFrame() as LayerFrame;
            if (frame == null) return;
            IEnvelope env = currentFeature.Envelope.Copy();

            if (env.Width == 0 || env.Height == 0)
            {
                env.ExpandBy(2.0, 2.0);
            }
            frame.ViewExtents = env.ToExtent();
        }

        /// <summary>
        /// This assumes that the datarows displayed correspond to features in the data Table.
        /// </summary>
        /// <param name="features"></param>
        public void SelectFeatures(IEnumerable<IFeature> features)
        {
            // Prevent infinite recursion by checking to ensure that the update did not originate from here.
            if (_ignoreSelectionChanged) return;
            List<int> rows = new List<int>();
            foreach (IFeature f in features)
            {
                rows.Add(f.Fid);
            }
            _ignoreSelectionChanged = true;
            dataGridView1.ClearSelection();
            foreach (int row in rows)
            {
                //dataGridView1.SelectedRows[row].Selected = true;
                //It should be full row of collectiion
                dataGridView1.Rows[row].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = row;
            }
            DisplaySelectedRowNumberLabel(rows.Count);
            dataGridView1.Refresh();
            _ignoreSelectionChanged = false;
        }

        /// <summary>
        /// This will update the Label of Selected Number of rows.
        /// </summary>
        /// <param name="numRows"></param>
        public void DisplaySelectedRowNumberLabel(int numRows)
        {
            lblSelectedNumber.Text = (numRows + SymbologyFormsMessageStrings.TableEditorControl_DisplaySelectedRowNumberLable__of_ + (dataGridView1.RowCount - 1) + SymbologyFormsMessageStrings.TableEditorControl_DisplaySelectedRowNumberLable__Selected);
        }

        /// <summary>
        /// This will update the Label of File path.
        /// </summary>
        /// <param name="file"></param>
        public void DisplayFilePathLabel(string file)
        {
            if (File.Exists(file))
            {
                lblFilePath.Text = Path.GetFullPath(file);
            }
            else
            {
                lblFilePath.Text = Resources.TableEditorControl_FileNameInMemory;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// If set to true, only the selected rows are displayed.
        /// If set to false, all rows are displayed.
        /// </summary>
        public bool ShowSelectedRowsOnly
        {
            get
            {
                return _showOnlySelectedRows;
            }
            set
            {
                if (value == _showOnlySelectedRows) return;

                //if we are changing the property..
                if (value)
                {
                    ShowOnlySelectedRows();
                }
                else
                {
                    ShowAllRows();
                }
            }
        }

        /// <summary>
        /// Gets or sets the feature layer used by this data Table
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFeatureLayer FeatureLayer
        {
            get { return _featureLayer; }
            set
            {
                _featureLayer = value;
                //to show the Filename label
                if (_featureLayer != null)
                {
                    if (_featureLayer.DataSet != null)
                    {
                        this.DisplayFilePathLabel(FeatureLayer.DataSet.Filename);
                    }
                }
                FeatureLayerSetup();
                if (ParentForm == null || _loaded) return;
                ParentForm.Shown += ParentFormShown;
                ParentForm.Closing += ParentFormClosing;
                _loaded = true;
            }
        }

        /// <summary>
        /// Gets the collection of selected data rows.  The row indices are
        /// 1 based instead of zero based, so be sure to subtract one before matching with a feature.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewSelectedRowCollection Selection
        {
            get { return dataGridView1.SelectedRows; }
        }

        /// <summary>
        /// Gets or sets the boolean that controls whether or not this form will throw an
        /// event during the selection changed process.
        /// </summary>
        public bool IgnoreSelectionChanged
        {
            get { return _ignoreSelectionChanged; }
            set { _ignoreSelectionChanged = value; }
        }

        /// <summary>
        /// set or get the relavant full featureset
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFeatureSet FeatureSetData
        {
            get { return _featureLayer.DataSet; }
        }

        /// <summary>
        /// gets or sets the visibility of the main menu strip
        /// </summary>
        public bool ShowMenuStrip
        {
            get { return menuStrip1.Visible; }
            set { menuStrip1.Visible = value; }
        }

        /// <summary>
        /// gets or sets the visibility of the main tool strip
        /// </summary>
        public bool ShowToolStrip
        {
            get { return toolStrip.Visible; }
            set { toolStrip.Visible = value; }
        }

        /// <summary>
        /// Gets or sets if the file path is shown in the status bar
        /// </summary>
        public bool ShowFileName
        {
            get
            {
                return lblFilePath.Visible;
            }
            set
            {
                if (value)
                {
                    progressBar.Visible = false;
                    lblFilePath.Visible = true;
                }
                else
                {
                    progressBar.Visible = true;
                    lblFilePath.Visible = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets if the progress bar is visible
        /// </summary>
        public bool ShowProgressBar
        {
            get { return progressBar.Visible; }
            set
            {
                if (value == progressBar.Visible) return;
                if (value)
                {
                    progressBar.Visible = true;
                    lblFilePath.Visible = false;
                }
                else
                {
                    progressBar.Visible = false;
                    lblFilePath.Visible = true;
                }
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether the Table is editable by the user
        /// </summary>
        public bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                _isEditable = value;
                SetEditableIcons();
            }
        }

        private void FeatureLayerSetup()
        {
            if (_featureLayer == null) return;
            if (_featureLayer.DataSet.NumRows() < 50000 && !_featureLayer.DataSet.AttributesPopulated)
            {
                _featureLayer.DataSet.FillAttributes();
            }

            if (_featureLayer.DataSet.AttributesPopulated)
            {
                dataGridView1.VirtualMode = false;
                dataGridView1.AllowUserToOrderColumns = true;
                dataGridView1.DataSource = _featureLayer.DataSet.DataTable;
                if (_virtualHooked)
                {
                    dataGridView1.CellValueNeeded -= DataGridView1CellValueNeeded;
                    dataGridView1.CellValuePushed -= dataGridView1_CellValuePushed;
                    dataGridView1.RowValidated -= dataGridView1_RowValidated;
                }
                AddFid(_featureLayer.DataSet.DataTable);
            }
            else
            {
                dataGridView1.VirtualMode = true;
                dataGridView1.AllowUserToOrderColumns = false;
                if (!_virtualHooked)
                {
                    dataGridView1.CellValueNeeded += DataGridView1CellValueNeeded;
                    dataGridView1.CellValuePushed += dataGridView1_CellValuePushed;
                    dataGridView1.RowValidated += dataGridView1_RowValidated;
                    _virtualHooked = true;
                }
                _attributeCache = new AttributeCache(FeatureLayer.DataSet, 16);
                DataColumn[] columns = _featureLayer.DataSet.GetColumns();
                foreach (DataColumn field in columns)
                {
                    dataGridView1.Columns.Add(field.ColumnName, field.ColumnName);
                }
                dataGridView1.RowCount = _featureLayer.DataSet.NumRows();
            }
            _featureLayer.SelectionChanged += SelectedFeaturesChanged;
        }

        private void AddFid(DataTable table)
        {
            const string name = "FID";
            int i = 0;
            while (table.Columns.Contains(name + i))
            {
                i++;
            }
            _fidField = name + i;
            table.Columns.Add(_fidField, typeof(int));
            for (int row = 0; row < table.Rows.Count; row++)
            {
                table.Rows[row][_fidField] = row;
            }
        }

        private void DataGridView1SelectionChanged(object sender, EventArgs e)
        {
            if (_ignoreTableSelectionChanged) return;
            _ignoreSelectionChanged = true;

            if (_featureLayer.DataSet.AttributesPopulated)
            {
                //manage selection using the Selection property
                IndexSelection sel = _featureLayer.Selection as IndexSelection;
                if (sel != null)
                {
                    //check the 'fid' field
                    if (string.IsNullOrEmpty(_fidField) || !_featureLayer.DataSet.DataTable.Columns.Contains(_fidField))
                    {
                        _ignoreSelectionChanged = false;
                        return;
                    }
                    //set the selected state of the corresponding feature
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        int fid = (int)row.Cells[_fidField].Value;
                        if (row.Selected)
                        {
                            sel.Add(fid);
                        }
                        else
                        {
                            sel.Remove(fid);
                        }
                    }
                }
                else
                {
                    List<int> adds = new List<int>();
                    List<int> removes = _selectedRows.ToList();
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        if (!_selectedRows.Contains(row.Index))
                        {
                            adds.Add(row.Index);
                        }
                        removes.Remove(row.Index);
                    }
                    _featureLayer.Select(adds);
                    _featureLayer.UnSelect(removes);
                }

                if (_featureLayer != null)
                {
                    _featureLayer.Invalidate();
                }
                _ignoreSelectionChanged = false;
            }
        }

        #endregion

        #region Event Handlers

        //when the 'Enable editing' menu is checked or unchecked
        private void EnableEditingToolStripMenuItemCheckedChanged(object sender, EventArgs e)
        {
            IsEditable = enableEditingToolStripMenuItem.Checked;
        }

        //execute query
        private void tsbtnQuery_Click(object sender, EventArgs e)
        {
            QueryExe();
        }

        //reload the data source
        private void tsbtnRefresh_Click(object sender, EventArgs e)
        {
            //ReloadDataSource();
        }

        //refresh the map
        private void tsbtnRefreshMap_Click(object sender, EventArgs e)
        {
            OnRefreshMap();
        }

        //start field calculator
        private void tsbtnFieldCalculator_Click(object sender, EventArgs e)
        {
            FieldCalculationExecute();
        }

        private void tsbtnImportFieldsFromDBF_Click(object sender, EventArgs e)
        {
            ImportFieldsFromDbf();
        }

        //limit the display to selected rows only
        private void tsbtnShowSelected_Click(object sender, EventArgs e)
        {
            if (Equals(tsbtnShowSelected.Checked, true))
            {
                ShowOnlySelectedRows();
            }
            else
            {
                ShowAllRows();
            }
        }

        //zoom to selected rows
        private void tsbtnZoomToSelected_Click(object sender, EventArgs e)
        {
            ZoomToSelected();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the SelectionChanged event whenver the selection on this dialog has been altered
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            if (_ignoreSelectionChanged) return;

            //when selection is changed by the user..

            _ignoreSelectionChanged = true;

            //fire the SelectionChanged event of the layer
            //_featureLayer.Selection.

            if (SelectionChanged != null)
            {
                SelectionChanged(this, new EventArgs());
            }
            _ignoreSelectionChanged = false;
        }

        /// <summary>
        /// Fires the SelectioinZoom event whenver the selectionZoom button click
        /// </summary>
        protected virtual void OnSelectionZoom()
        {
            if (_ignoreSelectionChanged) return;

            _ignoreSelectionChanged = true;
            //if (tsbtnZoomToSelected.Click != null)
            SelectionZoom(this, new EventArgs());
            _ignoreSelectionChanged = false;
        }

        /// <summary>
        /// Fires the OnRefreshMap event whenver the RefreshMap button click
        /// </summary>
        protected virtual void OnRefreshMap()
        {
            if (_ignoreSelectionChanged) return;
            _ignoreSelectionChanged = true;

            //Call the event Handler
            MapRefreshed(this, new EventArgs());
            _ignoreSelectionChanged = false;
        }

        /// <summary>
        /// Fires the OnFieldCalculation event whenver the zoomToShapeBeingEdited menu click
        /// </summary>
        protected virtual void OnzoomToShapeBeingEdited()
        {
            if (_ignoreSelectionChanged) return;
            _ignoreSelectionChanged = true;

            //Call the event Handeler

            ZoomToShapeBeingEdited(this, new EventArgs());
            _ignoreSelectionChanged = false;
        }

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

        #endregion

        #region ToolStripMenuItem_Click Events

        //add field
        private void addFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewColumn();

            SelectNone();
        }

        //remove field
        private void removeFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_featureLayer.DataSet.AttributesPopulated)
            {
                List<string> field = new List<string>();
                List<string> selectedField = new List<string>();

                //collect the field

                //foreach (DataColumn dc in _featureLayer.DataSet.GetColumns())
                foreach (DataColumn dc in _featureLayer.DataSet.DataTable.Columns)
                {
                    field.Add(dc.ToString());
                }
                DeleteFieldsDialog del = new DeleteFieldsDialog(field);
                if (del.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_RemoveFields, SymbologyFormsMessageStrings.TableEditorControl_Confirm, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        selectedField = del.SelectedFieldIdList;
                }
                dataGridView1.SuspendLayout();
                dataGridView1.SelectionChanged -= DataGridView1SelectionChanged;
                foreach (string fi in selectedField)
                {
                    _featureLayer.DataSet.DataTable.Columns.Remove(fi);
                }
                dataGridView1.SelectionChanged += DataGridView1SelectionChanged;
                dataGridView1.ResumeLayout();
                SelectNone();
            }
            else
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_removeFieldToolStripMenuItem_Click_Column_edits_are_not_yet_supported_for_datasets_with_more_than_50_000_attributes);
            }
        }

        //rename field
        private void renameFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_featureLayer.DataSet.AttributesPopulated != true)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_renameFieldToolStripMenuItem_Click_This_feature_is_not_yet_supported_for_datasets_with_larger_than_50_000_rows_);
                return;
            }
            //collect the field
            List<string> field = new List<string>();
            DataTable dt = _featureLayer.DataSet.DataTable;
            if (dt == null) return;
            foreach (DataColumn dc in dt.Columns)
                field.Add(dc.ToString());

            RenameFieldDialog renameFieldDialog = new RenameFieldDialog(field);
            if (renameFieldDialog.ShowDialog() != DialogResult.OK) return;
            int index = dt.Columns.IndexOf(renameFieldDialog.ResultCombination[0]);
            dt.Columns[index].ColumnName = renameFieldDialog.ResultCombination[1]; //rename column
            SelectNone();
        }

        //show only selected rows (selected features)
        private void showOnlySelectedShapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (showOnlySelectedShapesToolStripMenuItem.Checked)
            {
                ShowOnlySelectedRows();
            }
            else
            {
                ShowAllRows();
            }
        }

        //zoom to selected shapes
        private void zoomToSelectedShapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomToSelected();
        }

        //zoom to shape being edited (corresponding to current row)
        private void zoomToShapeBeingEditedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            ZoomToEditedRow();
        }

        private void flashSelectedShapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void queryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueryExe();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectNone();
        }

        private void invertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvertSelection();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputBox inbox = new InputBox(SymbologyFormsMessageStrings.TableEditorControl_EnterFindString, SymbologyFormsMessageStrings.TableEditorControl_Find);
            if (inbox.ShowDialog() != DialogResult.OK) return;

            string exp = inbox.Result;
            if (exp == null) return;
            if (FindString(exp) == false)
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_NoMatch);
        }

        //Export selected features
        private void exportSelectedFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sdlg = new SaveFileDialog
                                      {
                                          Filter =
                                              SymbologyFormsMessageStrings.
                                              TableEditorControl_exportSelectedFeaturesToolStripMenuItem_Click_Shapefiles____shp_____SHP
                                      };
            if (sdlg.ShowDialog() == DialogResult.OK)
            {
                _featureLayer.ExportSelection(sdlg.FileName);
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SeachAndReplaceDialog frmSeachAndReplaceDialog = new SeachAndReplaceDialog();
            if (frmSeachAndReplaceDialog.ShowDialog() != DialogResult.OK) return;
            string findString = frmSeachAndReplaceDialog.FindString;
            string replaceString = frmSeachAndReplaceDialog.ReplaceString;
            if (ReplaceString(findString, replaceString) == false)
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_CouldNotFindReplace);
        }

        private void importFieldDefinitionsFromDBFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportFieldsFromDbf();
        }

        private void fieldCalculatorToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FieldCalculationExecute();
        }

        private void generateOrUpdateMWShapeIDFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void copyShapeIDsToSpecifiedFieldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_featureLayer.DataSet.AttributesPopulated)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.LargeEditsNotSupported);
                return;
            }
            List<string> field = new List<string>();

            bool isNew = false;

            if (_featureLayer.DataSet.DataTable == null) return;
            foreach (DataColumn dc in _featureLayer.DataSet.DataTable.Columns)
            {
                field.Add(dc.ToString());
            }
            SelectField frmSeFie = new SelectField(field);
            if (frmSeFie.ShowDialog() == DialogResult.OK)
            {
                string fieldName = frmSeFie.FieldName;
                int index = _featureLayer.DataSet.DataTable.Columns.IndexOf(frmSeFie.FieldName);
                if (index == -1)
                    isNew = true;
                CopyFid(fieldName, isNew);
            }
        }

        //save edits
        private void tsbtnSaveEdits_Click(object sender, EventArgs e)
        {
            SaveEdits();
        }

        private void saveEditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveEdits();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// when 'IsEditable' is set to false, some toolbar icons are
        /// hidden
        /// </summary>
        private void SetEditableIcons()
        {
            if (_isEditable)
            {
                tsbtnSaveEdits.Visible = true;
                tsbtnFieldCalculator.Visible = true;
                tsbtnImportFieldsFromDBF.Visible = true;
                tsbtnRefreshMap.Visible = true;

                saveEditsToolStripMenuItem.Enabled = true;
                addFieldToolStripMenuItem.Enabled = true;
                removeFieldToolStripMenuItem.Enabled = true;
                renameFieldToolStripMenuItem.Enabled = true;
                fieldCalculatorToolToolStripMenuItem.Enabled = true;
                importFieldDefinitionsFromDBFToolStripMenuItem.Enabled = true;
                copyShapeIDsToSpecifiedFieldToolStripMenuItem.Enabled = true;

                dataGridView1.ReadOnly = false;
            }
            else
            {
                tsbtnSaveEdits.Visible = false;
                tsbtnFieldCalculator.Visible = false;
                tsbtnImportFieldsFromDBF.Visible = false;
                tsbtnRefreshMap.Visible = false;

                saveEditsToolStripMenuItem.Enabled = false;
                addFieldToolStripMenuItem.Enabled = false;
                removeFieldToolStripMenuItem.Enabled = false;
                renameFieldToolStripMenuItem.Enabled = false;
                fieldCalculatorToolToolStripMenuItem.Enabled = false;
                importFieldDefinitionsFromDBFToolStripMenuItem.Enabled = false;
                copyShapeIDsToSpecifiedFieldToolStripMenuItem.Enabled = false;

                dataGridView1.ReadOnly = true;
            }
        }

        //Shows all rows (both selected and unselected)
        private void ShowAllRows()
        {
            _showOnlySelectedRows = false;
            if (_featureLayer.DataSet.AttributesPopulated)
            {
                _ignoreTableSelectionChanged = true;
                _ignoreSelectionChanged = true;
                dataGridView1.DataSource = _featureLayer.DataSet.DataTable;
                foreach (int row in _selectedRows)
                {
                    dataGridView1.Rows[row].Selected = true;
                }
                _ignoreTableSelectionChanged = false;
                _ignoreSelectionChanged = false;
            }
            else
            {
                _attributeCache = new AttributeCache(_featureLayer.DataSet, 16);
                dataGridView1.RowCount = _featureLayer.DrawnStates.Length;
            }

            Refresh();
        }

        //Limits the displayed rows only to rows which are selected
        private void ShowOnlySelectedRows()
        {
            if (_featureLayer.DataSet.AttributesPopulated)
            {
                int numRows = _featureLayer.DataSet.DataTable.Rows.Count;
                dataGridView1.SuspendLayout();
                _selection = new DataTable();
                _selection.Columns.AddRange(_featureLayer.DataSet.GetColumns());

                if (!_selection.Columns.Contains(_fidField))
                {
                    _selection.Columns.Add(_fidField, typeof(int));
                }
                if (_selectionIndices == null) _selectionIndices = new List<int>();
                _selectionIndices.Clear();
                for (int row = 0; row < numRows; row++)
                {
                    if (!_featureLayer.DrawnStates[row].Selected) continue;
                    DataRow dr = _selection.NewRow();
                    dr.ItemArray = _featureLayer.DataSet.DataTable.Rows[row].ItemArray;
                    _selection.Rows.Add(dr);
                    _selectionIndices.Add(row);
                }
                dataGridView1.DataSource = _selection;
                dataGridView1.SelectAll();
                dataGridView1.ResumeLayout();
            }
            else
            {
                _attributeCache = new AttributeCache(_featureLayer.Selection, 16);
                dataGridView1.Rows.Clear(); // without this setting rowCount takes a looooong time
                dataGridView1.RowCount = _featureLayer.Selection.Count;
            }
            _showOnlySelectedRows = true;
            Refresh();
        }

        private void FieldCalculationExecute()
        {
            AttributeCalculator attributeCal = new AttributeCalculator();
            attributeCal.Show();
            attributeCal.FeatureSet = _featureLayer.DataSet;
            List<string> fieldList = new List<string>();
            foreach (DataColumn dataCol in _featureLayer.DataSet.DataTable.Columns)
                fieldList.Add(dataCol.ToString());

            attributeCal.LoadTableField(fieldList); //Load all fields
            attributeCal.NewFieldAdded += AttributeCalNewFieldAdded; //when user click new field to put the calulated values.
        }

        private void AttributeCalNewFieldAdded(object sender, EventArgs e)
        {
            AttributeCalculator attributeCal = sender as AttributeCalculator;
            if (attributeCal != null) _featureLayer.DataSet = attributeCal.FeatureSet;
        }

        private void ShowSelectedRowCount()
        {
            int numSelected = dataGridView1.SelectedRows.Count;

            lblSelectedNumber.Text = String.Format(SymbologyFormsMessageStrings.TableEditorControl_SelectedRowCountStringFormat, numSelected, _featureLayer.DataSet.NumRows());
        }

        //saves edits to the data Table
        private void SaveEdits()
        {
            try
            {
                dataGridView1.SuspendLayout();

                //remove temporary columns

                _featureLayer.DataSet.Save();

                dataGridView1.ResumeLayout();

                //ReloadDataSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_SaveEdits_Unable_to_save_edits__ + ex.Message);
            }
        }

        private void ImportFieldsFromDbf()
        {
            OpenFileDialog dlg = new OpenFileDialog
                                     {
                                         Filter =
                                             SymbologyFormsMessageStrings.
                                             TableEditorControl_ImportFieldsFromDbf_DBase_Files____dbf____DBF
                                     };
            FeatureSet fsTemp = new FeatureSet();
            fsTemp.CopyFeatures(_featureLayer.DataSet, true);
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_ImportFieldsFromDbf_Could_not_import_column_fields);
                return;
            }
            string shapeFilePath2 = dlg.FileName;
            int count = shapeFilePath2.Length;
            shapeFilePath2 = shapeFilePath2.Remove(count - 4, 4); //remove the extension of the file
            shapeFilePath2 = shapeFilePath2 + ".shp"; //add
            IFeatureSet fs;
            fs = FeatureSet.Open(shapeFilePath2);

            int noOfCol = fs.DataTable.Columns.Count;
            //Add all column header
            for (int i = 0; i < noOfCol; i++)
            {
                DataColumn dtcol = new DataColumn(fs.DataTable.Columns[i].ColumnName, fs.DataTable.Columns[i].DataType);
                if (fsTemp.DataTable.Columns.Contains(fs.DataTable.Columns[i].ColumnName) == false)
                    fsTemp.DataTable.Columns.Add(dtcol);
            }
            dataGridView1.DataSource = fsTemp.DataTable;
            return;
        }

        //Executes a query
        private void QueryExe()
        {
            if (_featureLayer.DataSet.AttributesPopulated)
            {
                _queryDialog.Table = _featureLayer.DataSet.DataTable;
            }
            else
            {
                _queryDialog.AttributeSource = _featureLayer.DataSet;
            }

            _queryDialog.ShowDialog(this);
        }

        private void QueryDialog_ChangesApplied(object sender, EventArgs e)
        {
            string resultExpresion = _queryDialog.Expression;
            if (resultExpresion != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    _featureLayer.SelectByAttribute(resultExpresion); //attempt to execute expression
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            else
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_CouldNotExecuteQuery);
            }
        }

        //add a new field (column)
        private void CreateNewColumn()
        {
            if (!_featureLayer.DataSet.AttributesPopulated)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.LargeEditsNotSupported);
                return;
            }
            if (_featureLayer.DataSet.DataTable == null)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_NewFieldFail);
                return;
            }

            AddNewColum addCol = new AddNewColum();

            if (addCol.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_NewFieldFail);
            }
            _featureLayer.DataSet.DataTable.Columns.Add(addCol.Name, addCol.Type);
            dataGridView1.ClearSelection();
        }

        //select all features in the Table and map
        private void SelectAll()
        {
            if (_showOnlySelectedRows)
            {
                if (_featureLayer != null)
                {
                    if (_featureLayer.DataSet.AttributesPopulated)
                    {
                        _ignoreSelectionChanged = true;
                        _ignoreTableSelectionChanged = true;
                        List<int> fids = new List<int>();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            row.Selected = true;
                            fids.Add((int)row.Cells[_fidField].Value);
                        }
                        _featureLayer.Select(fids);
                        _ignoreSelectionChanged = false;
                        _ignoreTableSelectionChanged = false;
                    }
                    else
                    {
                        if (_showOnlySelectedRows) dataGridView1.RowCount = _featureLayer.DataSet.NumRows();
                        foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                        {
                            for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                            {
                                dataGridView1.Rows[row].Selected = true;
                            }
                        }
                    }
                }

                return;
            }
            _ignoreSelectionChanged = true;
            _selectedRows = new List<int>();
            for (int i = 0; i < _featureLayer.DataSet.NumRows(); i++)
            {
                _selectedRows.Add(i);
            }
            if (_featureLayer != null)
            {
                _featureLayer.SelectAll();
                _featureLayer.Invalidate();
                _ignoreTableSelectionChanged = true;
                if (_featureLayer.DataSet.AttributesPopulated)
                {
                    dataGridView1.SelectAll();
                }
                else
                {
                    if (_showOnlySelectedRows) dataGridView1.RowCount = _featureLayer.DataSet.NumRows();
                    if (dataGridView1.VirtualMode)
                    {
                        foreach (DataGridViewRow r in dataGridView1.Rows)
                        {
                            r.Selected = true;
                        }
                    }
                    else
                    {
                        foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                        {
                            for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                            {
                                dataGridView1.Rows[row].Selected = true;
                            }
                        }
                    }
                }
                _ignoreTableSelectionChanged = false;
            }

            _ignoreSelectionChanged = false;
        }

        //unselect all features in the Table and map
        private void SelectNone()
        {
            _ignoreSelectionChanged = true;
            _selectedRows = new List<int>();
            _ignoreTableSelectionChanged = true;
            if (_featureLayer != null)
            {
                _featureLayer.ClearSelection();
                if (!_featureLayer.DataSet.AttributesPopulated)
                {
                    foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                    {
                        for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                        {
                            dataGridView1.Rows[row].Selected = false;
                        }
                    }
                }
                else
                {
                    dataGridView1.ClearSelection();
                }
            }
            _ignoreTableSelectionChanged = false;
            _ignoreSelectionChanged = false;
        }

        //invert selection
        private void InvertSelection()
        {
            if (_ignoreSelectionChanged) return;

            _ignoreSelectionChanged = true;
            if (_featureLayer != null)
            {
                if (_featureLayer.DataSet.AttributesPopulated)
                {
                    if (string.IsNullOrEmpty(_fidField) || !_featureLayer.DataSet.DataTable.Columns.Contains(_fidField)) return;

                    _ignoreTableSelectionChanged = true;
                    List<int> adds = new List<int>();
                    List<int> removes = new List<int>();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        bool sel = !row.Selected;
                        row.Selected = sel;
                        if (sel)
                        {
                            adds.Add((int)row.Cells[_fidField].Value);
                        }
                        else
                        {
                            removes.Add((int)row.Cells[_fidField].Value);
                        }
                    }
                    _featureLayer.Selection.SuspendChanges();
                    _featureLayer.Select(adds);
                    _featureLayer.UnSelect(removes);
                    _featureLayer.Selection.ResumeChanges();
                    _ignoreTableSelectionChanged = false;
                }
                else
                {
                    _featureLayer.InvertSelection();
                    _featureLayer.Invalidate();
                    foreach (AttributeCache.DataPage page in _attributeCache.Pages)
                    {
                        for (int row = page.LowestIndex; row <= page.HighestIndex; row++)
                        {
                            dataGridView1.Rows[row].Selected = _featureLayer.DrawnStates[row].Selected;
                        }
                    }
                }
            }

            _ignoreSelectionChanged = false;
        }

        /// <summary>
        /// Will find the string in the attribute Table (Search Operation)
        /// </summary>
        /// <param name="exp">the string to be found</param>
        /// <returns>true if the string was found, false otherwise</returns>
        private bool FindString(string exp)
        {
            if (exp == null) return false;

            exp.Trim();
            exp = exp.ToLower();

            string expression = BuildFindExpression(exp);
            _featureLayer.SelectByAttribute(expression);

            return _featureLayer.Selection.Count != 0;
        }

        /// <summary>
        /// Builds a 'find' select expression to find a string
        /// </summary>
        /// <param name="findString"></param>
        /// <returns></returns>
        private string BuildFindExpression(string findString)
        {
            List<string> conditions = new List<string>();
            DataColumn[] columns = _featureLayer.DataSet.GetColumns();
            //for each column in the data grid view
            foreach (Field col in columns)
            {
                if (col.DataType != typeof(string)) continue;
                string condition = "[" + col.ColumnName + "] LIKE '" + findString + "'";
                conditions.Add(condition);
            }

            StringBuilder stb = new StringBuilder();
            for (int i = 0; i < conditions.Count; i++)
            {
                stb.Append(conditions[i]);
                if (i < conditions.Count - 1)
                {
                    stb.Append(" OR ");
                }
            }

            return stb.ToString();
        }

        /// <summary>
        /// Will find the string in the dataGridView and replace
        /// </summary>
        /// <param name="exp">Find expression string</param>
        /// <param name="expReplace">replace expression string</param>
        /// <returns></returns>
        private bool ReplaceString(string exp, string expReplace)
        {
            if (exp == null) return false;
            exp.Trim();
            exp = exp.ToLower();
            bool rowFiended = false;
            string dgExp;
            int numRow = dataGridView1.RowCount;
            int numCol = dataGridView1.ColumnCount;
            progressBar.Visible = true;
            progressBar.Minimum = 0;
            progressBar.Maximum = numRow - 1;
            progressBar.Value = 1;
            progressBar.Step = 1;

            int category;
            if (exp.IndexOf("*", 0) == 0)
            {
                //starting with "*"
                category = 1;
                exp = exp.Remove(0, 1);
            }
            else if (exp.IndexOf("*", exp.Length - 1) == exp.Length - 1)
            {
                //ending with "*"
                category = 2;
                exp = exp.Remove(exp.Length - 1, 1);
            }
            else
            {
                //take as normal case
                category = 0;
            }

            for (int r = 0; r < numRow; r++)
            {
                // ******Progress Bar
                progressBar.PerformStep();

                for (int c = 0; c < numCol; c++)
                {
                    if (dataGridView1[c, r].Value == null) continue;
                    dgExp = dataGridView1[c, r].Value.ToString(); //cell value
                    dgExp = dgExp.ToLower();
                    bool itemReplaced = false;
                    if (category == 1)
                    {
                        //stating with "*"
                        if (dgExp.EndsWith(exp))
                        {
                            //check it occur at the end
                            dataGridView1.Rows[r].Selected = true;
                            rowFiended = true;
                            itemReplaced = true;
                        }
                    }
                    else if (category == 2)
                    {
                        //ending with "*"
                        if (dgExp.StartsWith(exp))
                        {
                            //check it occur at the begining
                            dataGridView1.Rows[r].Selected = true;
                            rowFiended = true;
                            itemReplaced = true;
                        }
                    }
                    else
                    {
                        //take as normal case
                        if (dgExp == exp)
                        {
                            //check it occur exacly same work
                            dataGridView1.Rows[r].Selected = true;
                            rowFiended = true;
                            itemReplaced = true;
                        }
                    }
                    //Replace the values
                    if (itemReplaced)
                    {
                        if (dataGridView1[c, r].ValueType == typeof(string))
                            dataGridView1[c, r].Value = expReplace;
                        else if (dataGridView1[c, r].ValueType == typeof(double))
                            dataGridView1[c, r].Value = Convert.ToDouble(expReplace);
                        else if (dataGridView1[c, r].ValueType == typeof(int))
                            dataGridView1[c, r].Value = Convert.ToInt32(expReplace);
                        else
                        {
                            MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_ReplaceString_Unfamilar_data_type_to_replace);
                            return false;
                        }
                    }
                }
            }
            //OnSelectionChanged();
            dataGridView1.Refresh();
            return rowFiended;
        }

        /// <summary>
        /// This will copy the FID of features to given column
        /// </summary>
        /// <param name="field">the field where FID should be copied</param>
        /// <param name="isNewField">if isNewField is true, a new field will be added</param>
        private void CopyFid(string field, bool isNewField)
        {
            if (!_featureLayer.DataSet.AttributesPopulated)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.LargeEditsNotSupported);
                return;
            }
            if (field == null)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_CopyFid_Could_not_copy_FID);
                return;
            }
            int colIndex = -1;
            if (isNewField)
            {
                //create new field
                _featureLayer.DataSet.DataTable.Columns.Add(field, typeof(int));
                colIndex = _featureLayer.DataSet.DataTable.Columns.Count - 1;
                dataGridView1.Refresh();
            }
            else
            {
                //use existing field (column)
                foreach (DataColumn dtCol in _featureLayer.DataSet.DataTable.Columns)
                {
                    //Take Column Index
                    if (field.ToLower() == dtCol.ColumnName.ToLower())
                    {
                        colIndex = dtCol.Ordinal;
                    }
                }
                if (colIndex == -1)
                {
                    MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_CopyFid_Couldnt_find_field);
                    return;
                }
            }

            IFeatureSet fs = _featureLayer.DataSet;
            for (int i = 0; i < _featureLayer.DataSet.DataTable.Rows.Count; i++)
            {
                //assign the values
                DataRow r = _featureLayer.DataSet.DataTable.Rows[i];
                IFeature f = fs.FeatureFromRow(r);
                _featureLayer.DataSet.DataTable.Rows[i][colIndex] = f.Fid;
            }
            SelectNone();

            return;
        }

        #endregion

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            ShowSelectedRowCount();
            OnSelectionChanged();
        }

        private void lblSelectedNumber_Click(object sender, EventArgs e)
        {
        }
    }
}