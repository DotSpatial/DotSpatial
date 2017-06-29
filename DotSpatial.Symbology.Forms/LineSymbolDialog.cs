// ********************************************************************************************************
// Product Name: DotSpatial.Forms.dll Alpha
// Description:  The basic module for DotSpatial.Forms version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.Forms.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/18/2009 9:06:46 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Initial dialog for selecting a predefined line symbol
    /// </summary>
    public class LineSymbolDialog : Form
    {
        #region Events

        /// <summary>
        /// Fires an event indicating that changes should be applied.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Private Variables

        private List<string> _categories;
        private ILineSymbolizer _original;
        private ILineSymbolizer _symbolizer;
        private ILineSymbolizer _symbolizer2;
        private Button btnSymbolDetails;
        private ComboBox cmbCategories;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private DialogButtons dialogButtons1;

        private Label lblPredefinedSymbol;
        private Label lblSymbolPreview;
        private Label lblSymbologyType;
        private PredefinedLineSymbolControl predefinedLineSymbolControl1;
        private SymbolPreview symbolPreview1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DetailedLineSymbolDialog
        /// </summary>
        public LineSymbolDialog()
        {
            InitializeComponent();

            _original = new LineSymbolizer();
            _symbolizer = new LineSymbolizer();
            _symbolizer2 = new LineSymbolizer();
            Configure();
        }

        /// <summary>
        /// Creates a new Detailed Line Symbol Dialog
        /// </summary>
        /// <param name="symbolizer"></param>
        public LineSymbolDialog(ILineSymbolizer symbolizer)
        {
            InitializeComponent();

            _original = symbolizer;
            _symbolizer = symbolizer.Copy();
            Configure();
        }

        private void Configure()
        {
            dialogButtons1.OkClicked += btnOK_Click;
            dialogButtons1.CancelClicked += btnCancel_Click;
            dialogButtons1.ApplyClicked += btnApply_Click;

            LoadDefaultSymbols();
            UpdatePreview();

            predefinedLineSymbolControl1.IsSelected = false;
            predefinedLineSymbolControl1.SymbolSelected += predefinedSymbolControl_SymbolSelected;
            predefinedLineSymbolControl1.DoubleClick += predefinedLineSymbolControl1_DoubleClick;
            symbolPreview1.DoubleClick += SymbolPreview_DoubleClick;
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineSymbolDialog));
            this.lblSymbologyType = new System.Windows.Forms.Label();
            this.lblPredefinedSymbol = new System.Windows.Forms.Label();
            this.lblSymbolPreview = new System.Windows.Forms.Label();
            this.btnSymbolDetails = new System.Windows.Forms.Button();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.predefinedLineSymbolControl1 = new DotSpatial.Symbology.Forms.PredefinedLineSymbolControl();
            this.symbolPreview1 = new DotSpatial.Symbology.Forms.SymbolPreview();
            this.dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            this.SuspendLayout();
            //
            // lblSymbologyType
            //
            resources.ApplyResources(this.lblSymbologyType, "lblSymbologyType");
            this.lblSymbologyType.Name = "lblSymbologyType";
            //
            // lblPredefinedSymbol
            //
            resources.ApplyResources(this.lblPredefinedSymbol, "lblPredefinedSymbol");
            this.lblPredefinedSymbol.Name = "lblPredefinedSymbol";
            //
            // lblSymbolPreview
            //
            resources.ApplyResources(this.lblSymbolPreview, "lblSymbolPreview");
            this.lblSymbolPreview.Name = "lblSymbolPreview";
            //
            // btnSymbolDetails
            //
            resources.ApplyResources(this.btnSymbolDetails, "btnSymbolDetails");
            this.btnSymbolDetails.Name = "btnSymbolDetails";
            this.btnSymbolDetails.UseVisualStyleBackColor = true;
            this.btnSymbolDetails.Click += new System.EventHandler(this.btnSymbolDetails_Click);
            //
            // cmbCategories
            //
            resources.ApplyResources(this.cmbCategories, "cmbCategories");
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.SelectedIndexChanged += new System.EventHandler(this.cmbCategories_SelectedIndexChanged);
            //
            // predefinedLineSymbolControl1
            //
            resources.ApplyResources(this.predefinedLineSymbolControl1, "predefinedLineSymbolControl1");
            this.predefinedLineSymbolControl1.BackColor = System.Drawing.Color.White;
            this.predefinedLineSymbolControl1.CategoryFilter = String.Empty;
            this.predefinedLineSymbolControl1.CellMargin = 8;
            this.predefinedLineSymbolControl1.CellSize = new System.Drawing.Size(62, 62);
            this.predefinedLineSymbolControl1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 272, 253);
            this.predefinedLineSymbolControl1.DefaultCategoryFilter = "All";
            this.predefinedLineSymbolControl1.DynamicColumns = true;
            this.predefinedLineSymbolControl1.IsInitialized = false;
            this.predefinedLineSymbolControl1.IsSelected = true;
            this.predefinedLineSymbolControl1.Name = "predefinedLineSymbolControl1";
            this.predefinedLineSymbolControl1.SelectedIndex = -1;
            this.predefinedLineSymbolControl1.SelectionBackColor = System.Drawing.Color.LightGray;
            this.predefinedLineSymbolControl1.SelectionForeColor = System.Drawing.Color.White;
            this.predefinedLineSymbolControl1.ShowSymbolNames = true;
            this.predefinedLineSymbolControl1.TextFont = new System.Drawing.Font("Arial", 8F);
            this.predefinedLineSymbolControl1.VerticalScrollEnabled = true;
            //
            // symbolPreview1
            //
            resources.ApplyResources(this.symbolPreview1, "symbolPreview1");
            this.symbolPreview1.BackColor = System.Drawing.Color.White;
            this.symbolPreview1.Name = "symbolPreview1";
            //
            // dialogButtons1
            //
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.Name = "dialogButtons1";
            //
            // LineSymbolDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.dialogButtons1);
            this.Controls.Add(this.predefinedLineSymbolControl1);
            this.Controls.Add(this.cmbCategories);
            this.Controls.Add(this.symbolPreview1);
            this.Controls.Add(this.btnSymbolDetails);
            this.Controls.Add(this.lblSymbolPreview);
            this.Controls.Add(this.lblPredefinedSymbol);
            this.Controls.Add(this.lblSymbologyType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LineSymbolDialog";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Event Handlers

        private void predefinedSymbolControl_SymbolSelected(object sender, EventArgs e)
        {
            CustomLineSymbolizer customSymbol = predefinedLineSymbolControl1.SelectedSymbolizer;

            if (customSymbol != null)
            {
                _symbolizer = customSymbol.Symbolizer;
                UpdatePreview();
            }
        }

        private void SymbolPreview_DoubleClick(object sender, EventArgs e)
        {
            if (_symbolizer != null && _original != null)
            {
                ShowDetailsDialog();
            }
        }

        private void predefinedLineSymbolControl1_DoubleClick(object sender, EventArgs e)
        {
            OnApplyChanges();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnSymbolDetails_Click(object sender, EventArgs e)
        {
            if (_symbolizer != null && _original != null)
            {
                ShowDetailsDialog();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            predefinedLineSymbolControl1.CategoryFilter = cmbCategories.SelectedItem.ToString();
        }

        //when the user clicks 'Add to custom symbols' on the details dialog
        private void detailsDialog_AddToCustomSymbols(object sender, LineSymbolizerEventArgs e)
        {
            // Here a dialog is displayed. The user can enter the custom symbol name and category
            // in the dialog.
            AddCustomSymbolDialog dlg = new AddCustomSymbolDialog(_categories, e.Symbolizer);
            dlg.ShowDialog();

            CustomLineSymbolizer newSym = dlg.CustomSymbolizer as CustomLineSymbolizer;
            if (newSym != null)
            {
                //check if user added a new category
                if (!_categories.Contains(newSym.Category))
                {
                    _categories.Add(newSym.Category);
                }
                UpdateCategories();

                predefinedLineSymbolControl1.SymbolizerList.Insert(0, newSym);
                predefinedLineSymbolControl1.Invalidate();
            }

            //TODO: save the custom symbolizer to xml / serialized file.
            //predefinedLineSymbolControl1.SaveToXml("test.xml");
        }

        private void detailsDialog_ChangesApplied(object sender, EventArgs e)
        {
            //unselect any symbolizers in the control
            predefinedLineSymbolControl1.IsSelected = false;

            UpdatePreview(_symbolizer2);
        }

        #endregion

        #region Methods

        private void UpdatePreview()
        {
            symbolPreview1.UpdatePreview(_symbolizer);
        }

        private void UpdatePreview(IFeatureSymbolizer symbolizer)
        {
            symbolPreview1.UpdatePreview(symbolizer);
        }

        /// <summary>
        /// Shows the 'Symbol Details' dialog
        /// </summary>
        private void ShowDetailsDialog()
        {
            DetailedLineSymbolDialog detailsDialog = new DetailedLineSymbolDialog(_original);
            detailsDialog.ChangesApplied += detailsDialog_ChangesApplied;
            detailsDialog.ShowDialog();
        }

        //this loads the default symbols and initializes the control
        //as well as the available categories
        private void LoadDefaultSymbols()
        {
            CustomLineSymbolProvider prov = new CustomLineSymbolProvider();

            _categories = prov.GetAvailableCategories();
            UpdateCategories();
        }

        private void UpdateCategories()
        {
            cmbCategories.SuspendLayout();
            cmbCategories.Items.Clear();
            cmbCategories.Items.Add("All");
            foreach (string cat in _categories)
            {
                cmbCategories.Items.Add(cat);
            }
            cmbCategories.SelectedIndex = 0;
            cmbCategories.ResumeLayout();
        }

        #endregion

        #region Properties

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected void OnApplyChanges()
        {
            UpdatePreview();
            _original.CopyProperties(_symbolizer);
            if (ChangesApplied != null) ChangesApplied(this, EventArgs.Empty);
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
    }
}