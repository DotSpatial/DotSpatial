// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SQLExpressionDialog
    /// </summary>
    public partial class SqlExpressionDialog : Form
    {
        private CultureInfo _extensionCulture;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlExpressionDialog"/> class.
        /// </summary>
        public SqlExpressionDialog()
        {
            InitializeComponent();
            DialogCulture = new CultureInfo(string.Empty);
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
            get
            {
                return sqlQueryControl1.AttributeSource;
            }

            set
            {
                sqlQueryControl1.AttributeSource = value;
            }
        }

        /// <summary>
        /// Gets or sets the string expression. Setting this will set the initial
        /// text content in the dialog.
        /// </summary>
        public string Expression
        {
            get
            {
                return sqlQueryControl1.ExpressionText;
            }

            set
            {
                sqlQueryControl1.ExpressionText = value;
            }
        }

        /// <summary>
        /// Gets or sets the DataTable that the expression dialog uses.
        /// </summary>
        public DataTable Table
        {
            get
            {
                return sqlQueryControl1.Table;
            }

            set
            {
                sqlQueryControl1.Table = value;
            }
        }

        /// <summary>
        /// sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo DialogCulture
        {
           set
            {
                if (_extensionCulture == value) return;
                _extensionCulture = value;
                if (_extensionCulture == null) _extensionCulture = new CultureInfo(string.Empty);
                Thread.CurrentThread.CurrentCulture = _extensionCulture;
                Thread.CurrentThread.CurrentUICulture = _extensionCulture;

                UpdateDialogItems();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the ChangesApplied event.
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            ChangesApplied?.Invoke(this, new FilterEventArgs(sqlQueryControl1.ExpressionText));
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            if (!sqlQueryControl1.ValidateExpression()) return;
            OnApplyChanges();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (!sqlQueryControl1.ValidateExpression())
            {
                DialogResult = DialogResult.None;
                return;
            }

            OnApplyChanges();
            Close();
        }

        private void UpdateDialogItems()
        {
            Refresh();
        }

        #endregion
    }
}