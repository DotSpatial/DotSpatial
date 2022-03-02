// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// AddCustomSymbolDialog.
    /// </summary>
    public partial class AddCustomSymbolDialog : Form
    {
        #region Fields

        private readonly List<string> _categories;
        private readonly IFeatureSymbolizer _symbolizer;
        private CustomSymbolizer _customSymbolizer;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddCustomSymbolDialog"/> class.
        /// </summary>
        /// <param name="symbolCategories">The symbol categories.</param>
        /// <param name="symbolizer">The symbolizer.</param>
        public AddCustomSymbolDialog(List<string> symbolCategories, IFeatureSymbolizer symbolizer)
        {
            InitializeComponent();
            _symbolizer = symbolizer;
            _categories = symbolCategories;

            UpdateCategories();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the custom symbolizer edited by this form.
        /// </summary>
        public CustomSymbolizer CustomSymbolizer => _customSymbolizer;

        #endregion

        #region Methods

        private void LblNameClick(object sender, EventArgs e)
        {
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            // creates the new custom symbolizer
            CustomSymbolizer newCustSym = CreateCustomSymbolizer();
            if (newCustSym != null)
            {
                _customSymbolizer = newCustSym;
            }

            Close();
        }

        /// <summary>
        /// Creates the new custom symbolizer with the specified name and category.
        /// </summary>
        /// <returns>the custom symbolizer.</returns>
        private CustomSymbolizer CreateCustomSymbolizer()
        {
            CustomSymbolizer custSym = null;
            if (_symbolizer is PointSymbolizer)
            {
                custSym = new CustomPointSymbolizer();
            }
            else if (_symbolizer is LineSymbolizer)
            {
                custSym = new CustomLineSymbolizer();
            }
            else if (_symbolizer is PolygonSymbolizer)
            {
                custSym = new CustomPolygonSymbolizer();
            }

            if (custSym != null)
            {
                custSym.Symbolizer = _symbolizer;
                custSym.Name = _txtSymbolName.Text;
                custSym.Category = _cmbSymbolCategory.Text;
                return custSym;
            }

            return null;
        }

        private void UpdateCategories()
        {
            // the default new category will be called 'My Symbols'.
            if (!_categories.Contains("My Symbols"))
            {
                _categories.Insert(0, "My Symbols");
            }

            _cmbSymbolCategory.SuspendLayout();
            _cmbSymbolCategory.Items.Clear();
            foreach (string cat in _categories)
            {
                _cmbSymbolCategory.Items.Add(cat);
            }

            _cmbSymbolCategory.SelectedIndex = 0;
            _cmbSymbolCategory.ResumeLayout();
        }

        #endregion
    }
}