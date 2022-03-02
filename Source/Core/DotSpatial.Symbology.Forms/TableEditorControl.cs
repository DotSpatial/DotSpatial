// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Symbology.Forms.Properties;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A Table editor user control. This may be used for displaying attributes of a feature layer.
    /// </summary>
    public partial class TableEditorControl : UserControl
    {
        #region Fields

        private AttributeCache _attributeCache;

        private IFeatureLayer _featureLayer;
        private string _fidField;
        private bool _ignoreTableSelectionChanged;
        private bool _isEditable = true;
        private List<int> _selectedRows;
        private List<int> _selectionIndices;
        private bool _showOnlySelectedRows;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TableEditorControl"/> class without any data.
        /// </summary>
        public TableEditorControl()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableEditorControl"/> class for editing a feature layer's attribute values.
        /// This allows interaction with the map. If a row is selected in the Table the corresponding row is selected in the map.
        /// </summary>
        /// <param name="layer">The symbolizer on the map.</param>
        public TableEditorControl(IFeatureLayer layer)
        {
            InitializeComponent();

            FeatureLayer = layer;
            _selectedRows = new List<int>();
            enableEditingToolStripMenuItem.CheckedChanged += EnableEditingToolStripMenuItemCheckedChanged;
            RemoveUnusedButtonsFromToolstrip();

            Disposed += (sender, args) => FeatureLayer = null;
            Load += (sender, args) =>
                {
                    SetSelectionFromLayer();
                    dataGridView1.SelectionChanged += DataGridView1SelectionChanged;
                };
        }

        #endregion

        #region Events

        /// <summary>
        /// occurs whenever the user click RefreshMap button.
        /// </summary>
        public event EventHandler MapRefreshed;

        /// <summary>
        /// Occurs whenever the user selects, de-selects or in any way updates the row selections
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Occurs whenever the user click SelectioinZoom button.
        /// </summary>
        public event EventHandler SelectionZoom;

        /// <summary>
        /// This will fire when user press ZoomToShapeBeingEdited button.
        /// </summary>
        public event EventHandler ZoomToShapeBeingEdited;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the feature layer used by this data Table.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public IFeatureLayer FeatureLayer
        {
            get
            {
                return _featureLayer;
            }

            set
            {
                if (_featureLayer == value) return;
                if (_featureLayer != null)
                {
                    _featureLayer.SelectionChanged -= SelectedFeaturesChanged;
                    if (_fidField != null)
                    {
                        _featureLayer.DataSet.DataTable.Columns.Remove(_fidField);
                        _fidField = null;
                    }
                }

                _featureLayer = value;

                dataGridView1.CellValueNeeded -= DataGridView1CellValueNeeded;
                dataGridView1.CellValuePushed -= DataGridView1CellValuePushed;
                dataGridView1.RowValidated -= DataGridView1RowValidated;

                if (_featureLayer != null)
                {
                    // to show the Filename label
                    DisplayFilePathLabel(FeatureLayer.DataSet.Filename);

                    _featureLayer.ProgressHandler = null;

                    if (_featureLayer.DataSet.NumRows() < 10000 && !_featureLayer.DataSet.AttributesPopulated)
                    {
                        _featureLayer.DataSet.FillAttributes();
                    }

                    if (_featureLayer.DataSet.AttributesPopulated)
                    {
                        dataGridView1.VirtualMode = false;
                        AddFid(_featureLayer.DataSet.DataTable);
                        dataGridView1.DataSource = _featureLayer.DataSet.DataTable;
                    }
                    else
                    {
                        dataGridView1.VirtualMode = true;
                        dataGridView1.CellValueNeeded += DataGridView1CellValueNeeded;
                        dataGridView1.CellValuePushed += DataGridView1CellValuePushed;
                        dataGridView1.RowValidated += DataGridView1RowValidated;

                        _attributeCache = new AttributeCache(FeatureLayer.DataSet, 16);
                        foreach (var field in _featureLayer.DataSet.GetColumns())
                        {
                            dataGridView1.Columns.Add(field.ColumnName, field.ColumnName);
                        }

                        dataGridView1.RowCount = _featureLayer.DataSet.NumRows();
                    }

                    _featureLayer.SelectionChanged += SelectedFeaturesChanged;
                }
            }
        }

        /// <summary>
        /// Gets the relavant full featureset.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public IFeatureSet FeatureSetData => _featureLayer.DataSet;

        /// <summary>
        /// Gets or sets a value indicating whether or not this form will throw an
        /// event during the selection changed process.
        /// </summary>
        public bool IgnoreSelectionChanged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Table is editable by the user.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }

            set
            {
                _isEditable = value;
                SetEditableIcons();
            }
        }

        /// <summary>
        /// Gets the collection of selected data rows. The row indices are
        /// 1 based instead of zero based, so be sure to subtract one before matching with a feature.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridViewSelectedRowCollection Selection => dataGridView1.SelectedRows;

        /// <summary>
        /// Gets or sets a value indicating whether the file path is shown in the status bar.
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
        /// Gets or sets a value indicating whether the the main menu strip is visible.
        /// </summary>
        public bool ShowMenuStrip
        {
            get
            {
                return menuStrip1.Visible;
            }

            set
            {
                menuStrip1.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the progress bar is visible.
        /// </summary>
        public bool ShowProgressBar
        {
            get
            {
                return progressBar.Visible;
            }

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
        /// Gets or sets a value indicating whether only the selected rows should be visible. If set to true, only the selected rows are displayed.
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

                // if we are changing the property..
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
        /// Gets or sets a value indicating whether the main tool strip is visible.
        /// </summary>
        public bool ShowToolStrip
        {
            get
            {
                return toolStrip.Visible;
            }

            set
            {
                toolStrip.Visible = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This will update the Label of File path.
        /// </summary>
        /// <param name="file">The file path.</param>
        public void DisplayFilePathLabel(string file)
        {
            lblFilePath.Text = File.Exists(file) ? Path.GetFullPath(file) : Resources.TableEditorControl_FileNameInMemory;
        }

        /// <summary>
        /// This will update the Label of Selected Number of rows.
        /// </summary>
        /// <param name="numRows">Number of selected rows.</param>
        public void DisplaySelectedRowNumberLabel(int numRows)
        {
            lblSelectedNumber.Text = numRows + SymbologyFormsMessageStrings.TableEditorControl_DisplaySelectedRowNumberLable__of_ + (dataGridView1.RowCount - 1) + SymbologyFormsMessageStrings.TableEditorControl_DisplaySelectedRowNumberLable__Selected;
        }

        /// <summary>
        /// This assumes that the displayed datarows correspond to features in the dataTable.
        /// </summary>
        /// <param name="features">Features that are selected.</param>
        public void SelectFeatures(IEnumerable<IFeature> features)
        {
            // Prevent infinite recursion by checking to ensure that the update did not originate from here.
            if (IgnoreSelectionChanged) return;
            List<int> rows = new List<int>();
            foreach (IFeature f in features)
            {
                rows.Add(f.Fid);
            }

            IgnoreSelectionChanged = true;
            dataGridView1.ClearSelection();
            foreach (int row in rows)
            {
                // dataGridView1.SelectedRows[row].Selected = true;
                // It should be full row of collection
                dataGridView1.Rows[row].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = row;
            }

            DisplaySelectedRowNumberLabel(rows.Count);
            dataGridView1.Refresh();
            IgnoreSelectionChanged = false;
        }

        /// <summary>
        /// Zooms to the row which is being edited.
        /// </summary>
        public void ZoomToEditedRow()
        {
            DataRowView drv = dataGridView1.CurrentRow?.DataBoundItem as DataRowView;
            if (drv == null) return;
            IFeature currentFeature = _featureLayer.DataSet.FeatureFromRow(drv.Row);
            LayerFrame frame = _featureLayer.ParentMapFrame() as LayerFrame;
            if (frame == null) return;
            var env = currentFeature.Geometry.EnvelopeInternal.Copy();

            if (env.Width == 0 || env.Height == 0)
            {
                env.ExpandBy(2.0, 2.0);
            }

            frame.ViewExtents = env.ToExtent();
        }

        /// <summary>
        /// Zoom to selected rows (features).
        /// </summary>
        public void ZoomToSelected()
        {
            if (_featureLayer == null) return;
            if (_featureLayer.Selection.Count == 0) return;

            _featureLayer.ZoomToSelectedFeatures();
        }

        /// <summary>
        /// Fires the OnRefreshMap event whenver the RefreshMap button click.
        /// </summary>
        protected virtual void OnRefreshMap()
        {
            if (IgnoreSelectionChanged) return;

            IgnoreSelectionChanged = true;
            MapRefreshed?.Invoke(this, EventArgs.Empty);
            IgnoreSelectionChanged = false;
        }

        /// <summary>
        /// Fires the SelectionChanged event whenver the selection on this dialog has been altered.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            if (IgnoreSelectionChanged) return;

            IgnoreSelectionChanged = true;
            SelectionChanged?.Invoke(this, EventArgs.Empty);
            IgnoreSelectionChanged = false;
        }

        /// <summary>
        /// Fires the SelectioinZoom event whenver the selectionZoom button click.
        /// </summary>
        protected virtual void OnSelectionZoom()
        {
            if (IgnoreSelectionChanged) return;

            IgnoreSelectionChanged = true;
            SelectionZoom?.Invoke(this, EventArgs.Empty);
            IgnoreSelectionChanged = false;
        }

        /// <summary>
        /// Fires the OnFieldCalculation event whenver the zoomToShapeBeingEdited menu click.
        /// </summary>
        protected virtual void OnzoomToShapeBeingEdited()
        {
            if (IgnoreSelectionChanged) return;
            IgnoreSelectionChanged = true;

            ZoomToShapeBeingEdited?.Invoke(this, EventArgs.Empty);

            IgnoreSelectionChanged = false;
        }

        private void AddFid(DataTable table)
        {
            const string Fid = "FID";
            int i = 0;
            while (table.Columns.Contains(Fid + i))
            {
                i++;
            }

            _fidField = Fid + i;
            table.Columns.Add(_fidField, typeof(int));
            for (var row = 0; row < table.Rows.Count; row++)
            {
                table.Rows[row][_fidField] = row;
            }
        }

        // add field
        private void AddFieldToolStripMenuItemClick(object sender, EventArgs e)
        {
            CreateNewColumn();
            SelectNone();
        }

        private void AttributeCalNewFieldAdded(object sender, EventArgs e)
        {
            AttributeCalculator attributeCal = sender as AttributeCalculator;
            if (attributeCal != null) _featureLayer.DataSet = attributeCal.FeatureSet;
        }

        /// <summary>
        /// Builds a 'find' select expression to find a string.
        /// </summary>
        /// <param name="findString">String that will be searched for.</param>
        /// <returns>The resulting expression.</returns>
        private string BuildFindExpression(string findString)
        {
            List<string> conditions = new List<string>();

            // for each column in the data grid view
            foreach (var col in _featureLayer.DataSet.GetColumns())
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
        /// This will copy the FID of the features to the given column.
        /// </summary>
        /// <param name="field">The field where FID should be copied to.</param>
        /// <param name="isNewField">If isNewField is true, the given field will be added.</param>
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
                // create new field
                _featureLayer.DataSet.DataTable.Columns.Add(field, typeof(int));
                colIndex = _featureLayer.DataSet.DataTable.Columns.Count - 1;
                dataGridView1.Refresh();
            }
            else
            {
                // use existing field (column)
                foreach (DataColumn dtCol in _featureLayer.DataSet.DataTable.Columns)
                {
                    // Take Column Index
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
                // assign the values
                DataRow r = _featureLayer.DataSet.DataTable.Rows[i];
                IFeature f = fs.FeatureFromRow(r);
                _featureLayer.DataSet.DataTable.Rows[i][colIndex] = f.Fid;
            }

            SelectNone();
        }

        private void CopyShapeIDsToSpecifiedFieldToolStripMenuItemClick(object sender, EventArgs e)
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

        /// <summary>
        /// Add a new field (column).
        /// </summary>
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

            var addCol = new AddNewColum();
            if (addCol.ShowDialog(this) != DialogResult.OK)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_NewFieldFail);
                return;
            }

            _featureLayer.DataSet.DataTable.Columns.Add(addCol.Name, addCol.Type);
            dataGridView1.ClearSelection();
        }

        private void DataGridView1CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (_attributeCache.EditRowIndex != dataGridView1.CurrentCell.RowIndex)
            {
                _attributeCache.EditRowIndex = dataGridView1.CurrentCell.RowIndex;
            }

            _attributeCache.EditRow[dataGridView1.Columns[e.ColumnIndex].Name] = e.Value;
        }

        private void DataGridView1RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (_attributeCache.EditRow != null)
            {
                _attributeCache.SaveChanges();
            }

            _attributeCache.EditRowIndex = -1; // this also sets the EditRow to null;
        }

        private void UpdatedLblSelectedNumber(object sender, EventArgs e)
        {
            lblSelectedNumber.Text = string.Format(SymbologyFormsMessageStrings.TableEditorControl_SelectedRowCountStringFormat, dataGridView1.SelectedRows.Count, _featureLayer.DataSet.NumRows());

            OnSelectionChanged();
        }

        private void DataGridView1CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = _attributeCache.RetrieveElement(e.RowIndex, e.ColumnIndex);
        }

        private void DataGridView1SelectionChanged(object sender, EventArgs e)
        {
            if (!_featureLayer.DataSet.AttributesPopulated) return; // For now can handle only populated data sets with fid column
            Debug.Assert(_fidField != null, "_fidField may not be null");

            if (_ignoreTableSelectionChanged) return;
            IgnoreSelectionChanged = true;

            try
            {
                // manage selection using the Selection property
                IndexSelection sel = _featureLayer.Selection as IndexSelection;
                if (sel != null)
                {
                    sel.SuspendChanges();

                    // set the selected state of the corresponding feature
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

                    sel.ResumeChanges();
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
            }
            finally
            {
                IgnoreSelectionChanged = false;
            }
        }

        // when the 'Enable editing' menu is checked or unchecked
        private void EnableEditingToolStripMenuItemCheckedChanged(object sender, EventArgs e)
        {
            IsEditable = enableEditingToolStripMenuItem.Checked;
        }

        // Export selected features
        private void ExportSelectedFeaturesToolStripMenuItemClick(object sender, EventArgs e)
        {
            SaveFileDialog sdlg = new SaveFileDialog
            {
                Filter = SymbologyFormsMessageStrings.TableEditorControl_exportSelectedFeaturesToolStripMenuItem_Click_Shapefiles____shp_____SHP
            };
            if (sdlg.ShowDialog() == DialogResult.OK)
            {
                _featureLayer.ExportSelection(sdlg.FileName);
            }
        }

        private void FieldCalculationExecute()
        {
            AttributeCalculator attributeCal = new AttributeCalculator();
            attributeCal.Show();
            attributeCal.FeatureSet = _featureLayer.DataSet;
            List<string> fieldList = new List<string>();
            foreach (DataColumn dataCol in _featureLayer.DataSet.DataTable.Columns)
                fieldList.Add(dataCol.ToString());

            attributeCal.LoadTableField(fieldList); // Load all fields
            attributeCal.NewFieldAdded += AttributeCalNewFieldAdded; // when user click new field to put the calulated values.
        }

        private void FieldCalculatorToolToolStripMenuItemClick(object sender, EventArgs e)
        {
            FieldCalculationExecute();
        }

        /// <summary>
        /// Will find the string in the attribute Table (Search Operation).
        /// </summary>
        /// <param name="exp">the string to be found.</param>
        /// <returns>true if the string was found, false otherwise.</returns>
        private bool FindString(string exp)
        {
            if (exp == null) return false;
            exp = exp.Trim().ToLower();
            string expression = BuildFindExpression(exp);
            _featureLayer.SelectByAttribute(expression);

            return _featureLayer.Selection.Count != 0;
        }

        private void FindToolStripMenuItemClick(object sender, EventArgs e)
        {
            InputBox inbox = new InputBox(SymbologyFormsMessageStrings.TableEditorControl_EnterFindString, SymbologyFormsMessageStrings.TableEditorControl_Find);
            if (inbox.ShowDialog() != DialogResult.OK) return;

            string exp = inbox.Result;
            if (exp == null) return;
            if (FindString(exp) == false)
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_NoMatch);
        }

        private void FlashSelectedShapesToolStripMenuItemClick(object sender, EventArgs e)
        {
        }

        private void GenerateOrUpdateMwShapeIdFieldsToolStripMenuItemClick(object sender, EventArgs e)
        {
        }

        private void ImportFieldDefinitionsFromDbfToolStripMenuItemClick(object sender, EventArgs e)
        {
            ImportFieldsFromDbf();
        }

        private void ImportFieldsFromDbf()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = SymbologyFormsMessageStrings.TableEditorControl_ImportFieldsFromDbf_DBase_Files____dbf____DBF
            };
            IFeatureSet fsTemp = _featureLayer.DataSet.CopySubset(string.Empty);
            if (dlg.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_ImportFieldsFromDbf_Could_not_import_column_fields);
                return;
            }

            string shapeFilePath2 = dlg.FileName;
            int count = shapeFilePath2.Length;
            shapeFilePath2 = shapeFilePath2.Remove(count - 4, 4); // remove the extension of the file
            shapeFilePath2 = shapeFilePath2 + ".shp"; // add
            IFeatureSet fs = FeatureSet.Open(shapeFilePath2);

            int noOfCol = fs.DataTable.Columns.Count;

            // Add all column header
            for (int i = 0; i < noOfCol; i++)
            {
                DataColumn dtcol = new DataColumn(fs.DataTable.Columns[i].ColumnName, fs.DataTable.Columns[i].DataType);
                if (fsTemp.DataTable.Columns.Contains(fs.DataTable.Columns[i].ColumnName) == false)
                    fsTemp.DataTable.Columns.Add(dtcol);
            }

            dataGridView1.DataSource = fsTemp.DataTable;
        }

        /// <summary>
        /// Invert selection.
        /// </summary>
        private void InvertSelection()
        {
            if (IgnoreSelectionChanged) return;

            IgnoreSelectionChanged = true;
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

            IgnoreSelectionChanged = false;
        }

        private void InvertSelectionToolStripMenuItemClick(object sender, EventArgs e)
        {
            InvertSelection();
        }

        private void QueryDialogChangesApplied(object sender, EventArgs e)
        {
            string resultExpresion = ((SqlExpressionDialog)sender).Expression;
            if (resultExpresion != null)
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    _featureLayer.SelectByAttribute(resultExpresion); // attempt to execute expression
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

        /// <summary>
        /// Executes a query.
        /// </summary>
        private void QueryExe()
        {
            var queryDialog = new SqlExpressionDialog();
            queryDialog.ChangesApplied += QueryDialogChangesApplied;
            if (_featureLayer.DataSet.AttributesPopulated)
            {
                queryDialog.Table = _featureLayer.DataSet.DataTable;
            }
            else
            {
                queryDialog.AttributeSource = _featureLayer.DataSet;
            }

            queryDialog.ShowDialog(this);
            queryDialog.ChangesApplied -= QueryDialogChangesApplied;
        }

        private void QueryToolStripMenuItemClick(object sender, EventArgs e)
        {
            QueryExe();
        }

        // remove field
        private void RemoveFieldToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_featureLayer.DataSet.AttributesPopulated)
            {
                List<string> field = new List<string>();
                List<string> selectedField = new List<string>();

                // collect the field
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

        private void RemoveUnusedButtonsFromToolstrip()
        {
            // removes the toolstrip buttons that are not working
            toolStrip.Items.Remove(tsbtnFieldCalculator);
            toolStrip.Items.Remove(tsbtnImportFieldsFromDBF);
            toolStrip.Items.Remove(tsbtnRefresh);
            toolStrip.Items.Remove(tsbtnRefreshMap);

            // removes the menu items that are not working
            mnuSelection.DropDownItems.Remove(flashSelectedShapesToolStripMenuItem);
            mnuSelection.DropDownItems.Remove(zoomToShapeBeingEditedToolStripMenuItem);
            mnuTools.DropDownItems.Remove(copyShapeIDsToSpecifiedFieldToolStripMenuItem);
            mnuTools.DropDownItems.Remove(importFieldDefinitionsFromDBFToolStripMenuItem);
            mnuTools.DropDownItems.Remove(generateOrUpdateMWShapeIDFieldsToolStripMenuItem);
            mnuTools.DropDownItems.Remove(fieldCalculatorToolToolStripMenuItem);
        }

        // rename field
        private void RenameFieldToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (_featureLayer.DataSet.AttributesPopulated != true)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_renameFieldToolStripMenuItem_Click_This_feature_is_not_yet_supported_for_datasets_with_larger_than_50_000_rows_);
                return;
            }

            // collect the field
            List<string> field = new List<string>();
            DataTable dt = _featureLayer.DataSet.DataTable;
            if (dt == null) return;
            foreach (DataColumn dc in dt.Columns)
                field.Add(dc.ToString());

            RenameFieldDialog renameFieldDialog = new RenameFieldDialog(field);
            if (renameFieldDialog.ShowDialog() != DialogResult.OK) return;
            int index = dt.Columns.IndexOf(renameFieldDialog.ResultCombination[0]);
            dt.Columns[index].ColumnName = renameFieldDialog.ResultCombination[1]; // rename column
            SelectNone();
        }

        /// <summary>
        /// Will find the string in the dataGridView and replace it.
        /// </summary>
        /// <param name="exp">Find expression string.</param>
        /// <param name="expReplace">replace expression string.</param>
        /// <returns>True, if something was replaced.</returns>
        private bool ReplaceString(string exp, string expReplace)
        {
            if (string.IsNullOrWhiteSpace(exp)) return false;
            exp = exp.Trim().ToLower();
            bool rowFiended = false;
            int numRow = dataGridView1.RowCount;
            int numCol = dataGridView1.ColumnCount;
            progressBar.Visible = true;
            progressBar.Minimum = 0;
            progressBar.Maximum = numRow - 1;
            progressBar.Value = 1;
            progressBar.Step = 1;

            Func<string, bool> categoryCheck;
            if (exp.IndexOf("*", 0, StringComparison.Ordinal) == 0)
            {
                // starting with "*"
                exp = exp.Remove(0, 1);

                // check it occur at the end
                categoryCheck = s => s.EndsWith(exp);
            }
            else if (exp.IndexOf("*", exp.Length - 1, StringComparison.Ordinal) == exp.Length - 1)
            {
                // ending with "*"
                exp = exp.Remove(exp.Length - 1, 1);

                // check it occur at the begining
                categoryCheck = s => s.StartsWith(exp);
            }
            else
            {
                // take as normal case
                categoryCheck = s => s == exp;
            }

            for (int r = 0; r < numRow; r++)
            {
                // ******Progress Bar
                progressBar.PerformStep();

                for (int c = 0; c < numCol; c++)
                {
                    if (dataGridView1[c, r].Value == null) continue;
                    string dgExp = dataGridView1[c, r].Value.ToString();
                    dgExp = dgExp.ToLower();
                    var itemReplaced = categoryCheck(dgExp);
                    if (itemReplaced)
                    {
                        dataGridView1.Rows[r].Selected = true;
                        rowFiended = true;
                    }

                    // Replace the values
                    if (itemReplaced)
                    {
                        if (dataGridView1[c, r].ValueType == typeof(string))
                        {
                            dataGridView1[c, r].Value = expReplace;
                        }
                        else if (dataGridView1[c, r].ValueType == typeof(double))
                        {
                            dataGridView1[c, r].Value = Convert.ToDouble(expReplace);
                        }
                        else if (dataGridView1[c, r].ValueType == typeof(int))
                        {
                            dataGridView1[c, r].Value = Convert.ToInt32(expReplace);
                        }
                        else
                        {
                            MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_ReplaceString_Unfamilar_data_type_to_replace);
                            return false;
                        }
                    }
                }
            }

            dataGridView1.Refresh();
            return rowFiended;
        }

        private void ReplaceToolStripMenuItemClick(object sender, EventArgs e)
        {
            using (var frmSeachAndReplaceDialog = new SearchAndReplaceDialog())
            {
                if (frmSeachAndReplaceDialog.ShowDialog(this) != DialogResult.OK) return;
                if (!ReplaceString(frmSeachAndReplaceDialog.FindString, frmSeachAndReplaceDialog.ReplaceString))
                {
                    MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_CouldNotFindReplace);
                }
            }
        }

        private void SaveEdits()
        {
            dataGridView1.SuspendLayout();
            _ignoreTableSelectionChanged = true;

            try
            {
                // remove fid column
                if (_fidField != null)
                {
                    _featureLayer.DataSet.DataTable.Columns.Remove(_fidField);
                }

                _featureLayer.DataSet.Save();

                // restore fid column
                if (_fidField != null)
                {
                    AddFid(_featureLayer.DataSet.DataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.TableEditorControl_SaveEdits_Unable_to_save_edits__ + ex.Message);
            }
            finally
            {
                _ignoreTableSelectionChanged = false;
                dataGridView1.ResumeLayout();
            }
        }

        private void SaveEditsToolStripMenuItemClick(object sender, EventArgs e)
        {
            SaveEdits();
        }

        /// <summary>
        /// Select all features in the table and map.
        /// </summary>
        private void SelectAll()
        {
            if (_showOnlySelectedRows)
            {
                if (_featureLayer != null)
                {
                    if (_featureLayer.DataSet.AttributesPopulated)
                    {
                        IgnoreSelectionChanged = true;
                        _ignoreTableSelectionChanged = true;
                        List<int> fids = new List<int>();
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            row.Selected = true;
                            fids.Add((int)row.Cells[_fidField].Value);
                        }

                        _featureLayer.Select(fids);
                        IgnoreSelectionChanged = false;
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

            IgnoreSelectionChanged = true;
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

            IgnoreSelectionChanged = false;
        }

        private void SelectAllToolStripMenuItemClick(object sender, EventArgs e)
        {
            SelectAll();
        }

        /// <summary>
        /// Updates the selection shown in the datagridview when the selected features are changed on the feature layer.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void SelectedFeaturesChanged(object sender, EventArgs e)
        {
            SetSelectionFromLayer();
        }

        /// <summary>
        /// Unselect all features in the Table and map.
        /// </summary>
        private void SelectNone()
        {
            IgnoreSelectionChanged = true;
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
            IgnoreSelectionChanged = false;
        }

        private void SelectNoneToolStripMenuItemClick(object sender, EventArgs e)
        {
            SelectNone();
        }

        /// <summary>
        /// When 'IsEditable' is set to false, some toolbar icons are hidden.
        /// </summary>
        private void SetEditableIcons()
        {
            tsbtnSaveEdits.Visible = _isEditable;
            tsbtnFieldCalculator.Visible = _isEditable;
            tsbtnImportFieldsFromDBF.Visible = _isEditable;
            tsbtnRefreshMap.Visible = _isEditable;

            saveEditsToolStripMenuItem.Enabled = _isEditable;
            addFieldToolStripMenuItem.Enabled = _isEditable;
            removeFieldToolStripMenuItem.Enabled = _isEditable;
            renameFieldToolStripMenuItem.Enabled = _isEditable;
            fieldCalculatorToolToolStripMenuItem.Enabled = _isEditable;
            importFieldDefinitionsFromDBFToolStripMenuItem.Enabled = _isEditable;
            copyShapeIDsToSpecifiedFieldToolStripMenuItem.Enabled = _isEditable;

            dataGridView1.ReadOnly = !_isEditable;
        }

        private void SetSelectionFromLayer()
        {
            if (_featureLayer == null) return;
            if (IgnoreSelectionChanged) return;

            dataGridView1.SuspendLayout();
            IgnoreSelectionChanged = true;
            _ignoreTableSelectionChanged = true;

            try
            {
                if (!_featureLayer.EditMode)
                {
                    FastDrawnState[] states = _featureLayer.DrawnStates;
                    if (states == null) return;

                    if (_featureLayer.DataSet.AttributesPopulated)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            int fid = (int)row.Cells[_fidField].Value;
                            row.Selected = states[fid].Selected;
                        }
                    }
                    else
                    {
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
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            int fid = (int)row.Cells[_fidField].Value;
                            row.Selected = fs.Filter.DrawnStates[_featureLayer.DataSet.Features[fid]].IsSelected;
                        }
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
            }
            finally
            {
                IgnoreSelectionChanged = false;
                _ignoreTableSelectionChanged = false;
                dataGridView1.ResumeLayout();
            }
        }

        /// <summary>
        /// Shows all rows (both selected and unselected).
        /// </summary>
        private void ShowAllRows()
        {
            _showOnlySelectedRows = false;
            if (_featureLayer.DataSet.AttributesPopulated)
            {
                _ignoreTableSelectionChanged = true;
                IgnoreSelectionChanged = true;
                dataGridView1.DataSource = _featureLayer.DataSet.DataTable;
                foreach (int row in _selectedRows)
                {
                    dataGridView1.Rows[row].Selected = true;
                }

                _ignoreTableSelectionChanged = false;
                IgnoreSelectionChanged = false;
            }
            else
            {
                _attributeCache = new AttributeCache(_featureLayer.DataSet, 16);
                dataGridView1.RowCount = _featureLayer.DrawnStates.Length;
            }

            Refresh();
        }

        /// <summary>
        /// Limits the displayed rows only to rows which are selected.
        /// </summary>
        private void ShowOnlySelectedRows()
        {
            IgnoreSelectionChanged = true;
            _ignoreTableSelectionChanged = true;
            if (_featureLayer.DataSet.AttributesPopulated)
            {
                int numRows = _featureLayer.DataSet.DataTable.Rows.Count;
                dataGridView1.SuspendLayout();
                var selection = new DataTable();
                selection.Columns.AddRange(_featureLayer.DataSet.GetColumns());

                if (!selection.Columns.Contains(_fidField))
                {
                    selection.Columns.Add(_fidField, typeof(int));
                }

                if (_selectionIndices == null) _selectionIndices = new List<int>();
                _selectionIndices.Clear();
                _selectedRows.Clear();
                for (int row = 0; row < numRows; row++)
                {
                    if (!_featureLayer.DrawnStates[row].Selected) continue;
                    DataRow dr = selection.NewRow();
                    dr.ItemArray = _featureLayer.DataSet.DataTable.Rows[row].ItemArray;
                    selection.Rows.Add(dr);
                    _selectionIndices.Add(row);
                    _selectedRows.Add(row);
                }

                dataGridView1.DataSource = selection;
                dataGridView1.SelectAll();
                dataGridView1.ResumeLayout();
            }
            else
            {
                _attributeCache = new AttributeCache(_featureLayer.Selection, 16);
                dataGridView1.Rows.Clear(); // without this setting rowCount takes a looooong time
                dataGridView1.RowCount = _featureLayer.Selection.Count;
            }

            _ignoreTableSelectionChanged = false;
            IgnoreSelectionChanged = false;
            _showOnlySelectedRows = true;
        }

        // show only selected rows (selected features)
        private void ShowOnlySelectedShapesToolStripMenuItemClick(object sender, EventArgs e)
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

        // start field calculator
        private void TsbtnFieldCalculatorClick(object sender, EventArgs e)
        {
            FieldCalculationExecute();
        }

        private void TsbtnImportFieldsFromDbfClick(object sender, EventArgs e)
        {
            ImportFieldsFromDbf();
        }

        // execute query
        private void TsbtnQueryClick(object sender, EventArgs e)
        {
            QueryExe();
        }

        // reload the data source
        private void TsbtnRefreshClick(object sender, EventArgs e)
        {
            // ReloadDataSource();
        }

        // refresh the map
        private void TsbtnRefreshMapClick(object sender, EventArgs e)
        {
            OnRefreshMap();
        }

        // save edits
        private void TsbtnSaveEditsClick(object sender, EventArgs e)
        {
            SaveEdits();
        }

        // limit the display to selected rows only
        private void TsbtnShowSelectedClick(object sender, EventArgs e)
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

        // zoom to selected rows
        private void TsbtnZoomToSelectedClick(object sender, EventArgs e)
        {
            ZoomToSelected();
            OnSelectionZoom();
        }

        // zoom to selected shapes
        private void ZoomToSelectedShapesToolStripMenuItemClick(object sender, EventArgs e)
        {
            ZoomToSelected();
        }

        // zoom to shape being edited (corresponding to current row)
        private void ZoomToShapeBeingEditedToolStripMenuItemClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            ZoomToEditedRow();
        }

        #endregion
    }
}