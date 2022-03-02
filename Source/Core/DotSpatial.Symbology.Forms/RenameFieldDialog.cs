// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This will pop when user want to rename the field.
    /// </summary>
    public partial class RenameFieldDialog : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RenameFieldDialog"/> class.
        /// </summary>
        public RenameFieldDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenameFieldDialog"/> class.
        /// </summary>
        /// <param name="field">List of fields that get added to the combobox.</param>
        public RenameFieldDialog(List<string> field)
        {
            InitializeComponent();
            foreach (string st in field)
                cmbField.Items.Add(st);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the ResultCombination consisting of the field that gets renamed and the new name.
        /// </summary>
        public string[] ResultCombination { get; set; } = { string.Empty, string.Empty };

        #endregion

        #region Methods

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Hide();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            ResultCombination[0] = cmbField.SelectedItem as string;
            ResultCombination[1] = txtName.Text;
            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}