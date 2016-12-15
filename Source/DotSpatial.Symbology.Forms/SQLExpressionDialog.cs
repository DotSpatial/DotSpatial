// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/30/2009 11:29:27 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Data;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SQLExpressionDialog
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial class SQLExpressionDialog : Form
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of CollectionPropertyGrid
        /// </summary>
        public SQLExpressionDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler<FilterEventArgs> ChangesApplied;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Attribute source instead of the table
        /// </summary>
        public IAttributeSource AttributeSource
        {
            get { return sqlQueryControl1.AttributeSource; }
            set { sqlQueryControl1.AttributeSource = value; }
        }

        /// <summary>
        /// Gets or sets the string expression.  Setting this will set the initial
        /// text content in the dialog.
        /// </summary>
        public string Expression
        {
            get { return sqlQueryControl1.ExpressionText; }
            set { sqlQueryControl1.ExpressionText = value; }
        }

        /// <summary>
        /// Gets or sets the DataTable that the expression dialog uses.
        /// </summary>
        public DataTable Table
        {
            get { return sqlQueryControl1.Table; }
            set { sqlQueryControl1.Table = value; }
        }

        #endregion

        #region Methods

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (!sqlQueryControl1.ValidateExpression()) return;
            OnApplyChanges();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!sqlQueryControl1.ValidateExpression())
            {
                DialogResult = DialogResult.None;
                return;
            }

            OnApplyChanges();
            Close();
        }

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            if (ChangesApplied != null) ChangesApplied(this, new FilterEventArgs(sqlQueryControl1.ExpressionText));
        }

        #endregion
    }
}