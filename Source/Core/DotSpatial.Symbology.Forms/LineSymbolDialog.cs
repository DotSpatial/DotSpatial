// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Initial dialog for selecting a predefined line symbol.
    /// </summary>
    public partial class LineSymbolDialog : Form
    {
        #region Fields
        private readonly ILineSymbolizer _symbolizer2;
        private readonly ILineSymbolizer _original;
        private List<string> _categories;
        private ILineSymbolizer _symbolizer;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineSymbolDialog"/> class.
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
        /// Initializes a new instance of the <see cref="LineSymbolDialog"/> class.
        /// </summary>
        /// <param name="symbolizer">The line symbolizer.</param>
        public LineSymbolDialog(ILineSymbolizer symbolizer)
        {
            InitializeComponent();

            _original = symbolizer;
            _symbolizer = symbolizer.Copy();
            Configure();
        }

        #endregion

        #region Events

        /// <summary>
        /// Fires an event indicating that changes should be applied.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Methods

        /// <summary>
        /// Fires the ChangesApplied event.
        /// </summary>
        protected void OnApplyChanges()
        {
            UpdatePreview();
            _original.CopyProperties(_symbolizer);
            ChangesApplied?.Invoke(this, EventArgs.Empty);
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            OnApplyChanges();
            Close();
        }

        private void BtnSymbolDetailsClick(object sender, EventArgs e)
        {
            if (_symbolizer != null && _original != null)
            {
                ShowDetailsDialog();
            }
        }

        private void CmbCategoriesSelectedIndexChanged(object sender, EventArgs e)
        {
            predefinedLineSymbolControl1.CategoryFilter = cmbCategories.SelectedItem.ToString();
        }

        private void Configure()
        {
            dialogButtons1.OkClicked += BtnOkClick;
            dialogButtons1.CancelClicked += BtnCancelClick;
            dialogButtons1.ApplyClicked += BtnApplyClick;

            LoadDefaultSymbols();
            UpdatePreview();

            predefinedLineSymbolControl1.IsSelected = false;
            predefinedLineSymbolControl1.SymbolSelected += PredefinedSymbolControlSymbolSelected;
            predefinedLineSymbolControl1.DoubleClick += PredefinedLineSymbolControl1DoubleClick;
            symbolPreview1.DoubleClick += SymbolPreviewDoubleClick;
        }

        // when the user clicks 'Add to custom symbols' on the details dialog
        private void DetailsDialogAddToCustomSymbols(object sender, LineSymbolizerEventArgs e)
        {
            // Here a dialog is displayed. The user can enter the custom symbol name and category
            // in the dialog.
            AddCustomSymbolDialog dlg = new(_categories, e.Symbolizer);
            dlg.ShowDialog();

            if (dlg.CustomSymbolizer is CustomLineSymbolizer newSym)
            {
                // check if user added a new category
                if (!_categories.Contains(newSym.Category))
                {
                    _categories.Add(newSym.Category);
                }

                UpdateCategories();

                predefinedLineSymbolControl1.SymbolizerList.Insert(0, newSym);
                predefinedLineSymbolControl1.Invalidate();
            }

            // TODO: save the custom symbolizer to xml / serialized file.
            // predefinedLineSymbolControl1.SaveToXml("test.xml");
        }

        private void DetailsDialogChangesApplied(object sender, EventArgs e)
        {
            // unselect any symbolizers in the control
            predefinedLineSymbolControl1.IsSelected = false;

            UpdatePreview(_symbolizer2);
        }

        // this loads the default symbols and initializes the control
        // as well as the available categories
        private void LoadDefaultSymbols()
        {
            CustomLineSymbolProvider prov = new();

            _categories = prov.GetAvailableCategories();
            UpdateCategories();
        }

        private void PredefinedLineSymbolControl1DoubleClick(object sender, EventArgs e)
        {
            OnApplyChanges();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void PredefinedSymbolControlSymbolSelected(object sender, EventArgs e)
        {
            CustomLineSymbolizer customSymbol = predefinedLineSymbolControl1.SelectedSymbolizer;

            if (customSymbol != null)
            {
                _symbolizer = customSymbol.Symbolizer;
                UpdatePreview();
            }
        }

        /// <summary>
        /// Shows the 'Symbol Details' dialog.
        /// </summary>
        private void ShowDetailsDialog()
        {
            DetailedLineSymbolDialog detailsDialog = new(_original);
            detailsDialog.ChangesApplied += DetailsDialogChangesApplied;
            detailsDialog.ShowDialog();
        }

        private void SymbolPreviewDoubleClick(object sender, EventArgs e)
        {
            if (_symbolizer != null && _original != null)
            {
                ShowDetailsDialog();
            }
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

        private void UpdatePreview()
        {
            symbolPreview1.UpdatePreview(_symbolizer);
        }

        private void UpdatePreview(IFeatureSymbolizer symbolizer)
        {
            symbolPreview1.UpdatePreview(symbolizer);
        }

        #endregion
    }
}