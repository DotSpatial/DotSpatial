// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SelectField class.
    /// </summary>
    public partial class SelectField : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectField"/> class.
        /// </summary>
        public SelectField()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectField"/> class.
        /// </summary>
        /// <param name="field">List of fields that get added to the combobox.</param>
        public SelectField(List<string> field)
        {
            InitializeComponent();
            foreach (string st in field)
                cmbField.Items.Add(st);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the field name.
        /// </summary>
        public string FieldName { get; private set; }

        #endregion

        #region Methods

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (txtName.Text == string.Empty)
                FieldName = cmbField.SelectedItem as string;
            else
                FieldName = txtName.Text;

            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}