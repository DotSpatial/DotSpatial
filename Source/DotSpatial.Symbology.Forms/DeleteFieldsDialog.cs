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
        #region Private Variables

        private readonly List<string> _fields = new List<string>();
        private List<string> _fieldSelected = new List<string>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of the form for deleting a field
        /// </summary>
        public DeleteFieldsDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new instance of the form for deleting a field while passing in a new field.
        /// </summary>
        /// <param name="fields"></param>
        public DeleteFieldsDialog(List<string> fields)
        {
            InitializeComponent();
            _fields = fields;
            foreach (string st in _fields)
            {
                clb.Items.Add(st, false);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// get or set the list of selected Fields
        /// </summary>
        public List<string> SelectedFieldIdList
        {
            get { return _fieldSelected; }
            set { _fieldSelected = value; }
        }

        #endregion

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _fieldSelected.Clear();
            CheckedListBox.CheckedItemCollection sItems = clb.CheckedItems;
            foreach (string st in sItems)
                _fieldSelected.Add(st);
            DialogResult = DialogResult.OK;
        }
    }
}