// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 09/17/09.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//  Name               Date                Comments
// ---------------------------------------------------------------------------------------------
// Ted Dunsford     |  9/19/2009    |  Added some xml comments
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This form diplay to user to find perticular value or string in DataGridView and replace.
    /// </summary>
    public partial class SearchAndReplaceDialog : Form
    {
        #region Variable

        private string _find;
        private string _replace;

        /// <summary>
        /// get the Find String
        /// </summary>
        public string FindString
        {
            // set { _find = value; }
            get { return _find; }
        }

        /// <summary>
        /// get the ReplaceString
        /// </summary>
        public string ReplaceString
        {
            // set { _replace = value; }
            get { return _replace; }
        }

        #endregion

        /// <summary>
        /// Creates a new instance of the replace form.
        /// </summary>
        public SearchAndReplaceDialog()
        {
            InitializeComponent();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtFind.Text))
            {
                DialogResult = DialogResult.None;
                return;
            }

            _find = txtFind.Text;
            _replace = txtReplace.Text;
        }
    }
}