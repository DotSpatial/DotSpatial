// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 09/15/09.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//  Name               Date                Comments
// ---------------------------------------------------------------------------------------------
// Ted Dunsford     |  9/19/2009    |  Added some xml comments
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This will load to user select field to remove.
    /// </summary>
    public partial class DeleteFieldsDialog : Form
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFieldsDialog"/> class for deleting a field.
        /// </summary>
        public DeleteFieldsDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteFieldsDialog"/> class for deleting a field while passing in a new field.
        /// </summary>
        /// <param name="fields">The fields that get added to the checked list box.</param>
        public DeleteFieldsDialog(List<string> fields)
        {
            InitializeComponent();
            foreach (string st in fields)
            {
                clb.Items.Add(st, false);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of selected Fields.
        /// </summary>
        public List<string> SelectedFieldIdList { get; set; } = new List<string>();

        #endregion

        #region Methods

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Hide();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            SelectedFieldIdList.Clear();
            CheckedListBox.CheckedItemCollection sItems = clb.CheckedItems;
            foreach (string st in sItems) SelectedFieldIdList.Add(st);

            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}