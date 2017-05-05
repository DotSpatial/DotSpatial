// ********************************************************************************************************
// Product Name: ExpressionControl.cs
// Description:  Control to create, edit and validate label expressions.
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// 2015-03-02 - jany_ - created control
// ********************************************************************************************************

using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Control to create, edit and validate label expressions.
    /// </summary>
    public partial class ExpressionControl : UserControl
    {
        #region Fields

        private readonly Expression _exp;
        private IAttributeSource _attributeSource;
        private DataTable _table;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionControl"/> class.
        /// </summary>
        public ExpressionControl()
        {
            InitializeComponent();
            _exp = new Expression();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether empty expressions are valid.
        /// </summary>
        public bool AllowEmptyExpression { get; set; }

        /// <summary>
        /// Gets or sets the attribute source. Setting this is an alternative to specifying the table. This allows the
        /// query control to build a query using pages of data instead of the whole table at once.
        /// </summary>
        public IAttributeSource AttributeSource
        {
            get
            {
                return _attributeSource;
            }

            set
            {
                _attributeSource = value;
                UpdateFields();
            }
        }

        /// <summary>
        /// Gets or sets the expression that gets edited and validated in ExpressionControl.
        /// </summary>
        public string ExpressionText
        {
            get
            {
                return rtbExpression.Text.Trim();
            }

            set
            {
                rtbExpression.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the data Table for this control. Setting this will
        /// automatically update the fields shown in the list.
        /// </summary>
        public DataTable Table
        {
            get
            {
                return _table;
            }

            set
            {
                _table = value;
                UpdateFields();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the Expression including syntax and operations.
        /// </summary>
        /// <returns>True, if Expression is valid.</returns>
        public bool ValidateExpression()
        {
            if (string.IsNullOrWhiteSpace(rtbExpression.Text))
            {
                if (AllowEmptyExpression)
                {
                    lblResult.Text = string.Empty;
                    return true;
                }

                lblResult.Text = SymbologyFormsMessageStrings.ExpressionControl_EmptyExpression;
                lblResult.ForeColor = Color.Red;
                return false;
            }

            var res = _exp.ParseExpression(rtbExpression.Text);
            if (!res)
            {
                lblResult.Text = _exp.ErrorMessage;
                lblResult.ForeColor = Color.Red;
                return false;
            }

            string retVal = string.Empty;
            if (_exp.IsValidOperation(ref retVal, _table.Rows.Count > 0 ? _table.Rows[0] : null))
            {
                // calculate with real values if possible else use temporary values
                lblResult.Text = retVal;
                lblResult.ForeColor = Color.Black;
                return true;
            }

            lblResult.Text = _exp.ErrorMessage;
            lblResult.ForeColor = Color.Red;
            return false;
        }

        /// <summary>
        /// Adds a new line.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void BtNewLineClick(object sender, EventArgs e)
        {
            rtbExpression.SelectedText = "\n";
        }

        /// <summary>
        /// Validates the expression and shows the error or result.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void BtValidateClick(object sender, EventArgs e)
        {
            ValidateExpression();
        }

        /// <summary>
        /// Adds the field that was double clicked to the expression.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void DgvFieldsCellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > dgvFields.Rows.Count - 1) return;

            rtbExpression.SelectedText = "[" + dgvFields.Rows[e.RowIndex].Cells[dgvcName.Name].Value + "]";
        }

        /// <summary>
        /// Suppress tab if it was entered because it is illegal in expressions.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void RtbExpressionKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Replaces the expression fields by the columns of the current _attributeSource or _table.
        /// </summary>
        private void UpdateFields()
        {
            bool hasFid = false;
            _exp.ClearFields();
            dgvFields.SuspendLayout();
            dgvFields.Rows.Clear();
            if (_attributeSource != null)
            {
                DataColumn[] columns = _attributeSource.GetColumns();
                foreach (DataColumn dc in columns)
                {
                    _exp.AddField(dc);
                    dgvFields.Rows.Add(dc.ColumnName, dc.DataType.ToString().Replace("System.", string.Empty));
                    if (dc.ColumnName.ToLower() == "fid") hasFid = true;
                }
            }
            else if (_table != null)
            {
                foreach (DataColumn dc in _table.Columns)
                {
                    _exp.AddField(dc);
                    dgvFields.Rows.Add(dc.ColumnName, dc.DataType.ToString().Replace("System.", string.Empty));
                    if (dc.ColumnName.ToLower() == "fid") hasFid = true;
                }
            }

            if (!hasFid) dgvFields.Rows.Add("FID", typeof(int).ToString().Replace("System.", string.Empty));
            dgvFields.ResumeLayout();
        }

        #endregion
    }
}