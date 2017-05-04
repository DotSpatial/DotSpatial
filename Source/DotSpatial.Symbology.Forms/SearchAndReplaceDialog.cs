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
    /// This form can be used to search for perticular values or strings in a DataGridView and replace them.
    /// </summary>
    public partial class SearchAndReplaceDialog : Form
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchAndReplaceDialog"/> class.
        /// </summary>
        public SearchAndReplaceDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the string that should be found.
        /// </summary>
        public string FindString { get; private set; }

        /// <summary>
        /// Gets the string that should be used as replacement.
        /// </summary>
        public string ReplaceString { get; private set; }

        #endregion

        #region Methods

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFind.Text))
            {
                DialogResult = DialogResult.None;
                return;
            }

            FindString = txtFind.Text;
            ReplaceString = txtReplace.Text;
        }

        #endregion
    }
}