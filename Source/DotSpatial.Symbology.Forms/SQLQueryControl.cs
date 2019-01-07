// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Creates a new instance of the SQLQueryControl
    /// </summary>
    public partial class SqlQueryControl : UserControl
    {
        #region Fields

        private IAttributeSource _attributeSource;
        private IDataTable _table;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlQueryControl"/> class.
        /// </summary>
        public SqlQueryControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the text in the expression textbox has changed.
        /// </summary>
        public event EventHandler ExpressionTextChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the attribute source. Setting this is an alternative to specifying the table. This allows the
        /// query control to build a query using pages of data instead of the whole
        /// table at once.
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
        /// Gets or sets the string that appears in the filter text.
        /// </summary>
        public string ExpressionText
        {
            get
            {
                return rtbFilterText.Text.Trim();
            }

            set
            {
                rtbFilterText.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the data Table for this control. Setting this will
        /// automatically update the fields shown in the list.
        /// </summary>
        public IDataTable Table
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

        /// <summary>
        /// Gets or sets the string that appears in the filter text.
        /// </summary>
        public override string Text
        {
            get
            {
                return rtbFilterText.Text.Trim();
            }

            set
            {
                rtbFilterText.Text = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the expression.
        /// </summary>
        /// <returns>True, if Expression is valid.</returns>
        public bool ValidateExpression()
        {
            try
            {
                if (_attributeSource != null)
                    _attributeSource.GetCounts(new[] { rtbFilterText.Text }, null, 1);
                else
                    _table?.Select(rtbFilterText.Text);
            }
            catch (Exception e)
            {
                lblResult.Text = e.Message;
                lblResult.ForeColor = Color.Red;
                return false;
            }

            lblResult.Text = SymbologyFormsMessageStrings.SQLQueryControl_ExpressionIsValid;
            lblResult.ForeColor = Color.Black;
            return true;
        }

        private static void GetMinMax(IEnumerable<IDataRow> rows, string field, ref IComparable min, ref IComparable max)
        {
            foreach (var dr in rows)
            {
                if (dr[field] is DBNull) continue;
                if (min == null)
                {
                    min = dr[field] as IComparable;
                }
                else
                {
                    if (min.CompareTo(dr[field]) > 0) min = dr[field] as IComparable;
                }

                if (max == null)
                {
                    max = dr[field] as IComparable;
                }
                else
                {
                    if (max.CompareTo(dr[field]) < 0) max = dr[field] as IComparable;
                }
            }
        }

        private void BtnAndClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "AND ";
        }

        private void BtnAsterixClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "*";
        }

        private void BtnEqualsClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "= ";
        }

        private void BtnGetUniqueValuesClick(object sender, EventArgs e)
        {
            // Sorting should be done as the original objects, not as strings.
            var lst = new HashSet<object>();
            string fieldName = lbxFields.SelectedItem.ToString();
            bool useAll = false;
            bool isString = true;
            if (_attributeSource != null)
            {
                isString = _attributeSource.GetColumn(fieldName).DataType == typeof(string);
                int numPages = (int)Math.Ceiling((double)_attributeSource.NumRows() / 10000);
                for (int page = 0; page < numPages; page++)
                {
                    IDataTable table = _attributeSource.GetAttributes(page * 10000, 10000);
                    foreach (IDataRow dr in table.Rows)
                    {
                        if (dr[fieldName] is DBNull) continue;
                        if (lst.Contains(dr[fieldName])) continue;
                        lst.Add(dr[fieldName]);
                    }

                    if (lst.Count <= 10000 || useAll) continue;
                    if (MessageBox.Show(SymbologyFormsMessageStrings.SQLQueryControl_MoreThan10000UniqueValues, SymbologyFormsMessageStrings.SQLQueryControl_LargeNumberOfUniqueValues, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        useAll = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (_table != null)
            {
                isString = _table.Columns[fieldName].DataType == typeof(string);
                foreach (object row in _table.Rows)
                {
                    object val = null;

                    if (row is IDataRow idr)
                        val = idr[fieldName];
                    else if (row is DataRow dr)
                        val = dr[fieldName];
                    else
                        continue;


                    if (val is DBNull) continue;
                    if (lst.Contains(val)) continue;
                    lst.Add(val);
                }
            }

            var text = new object[lst.Count];
            int i = 0;
            foreach (var o in lst.OrderBy(_ => _))
            {
                if (isString)
                {
                    text[i++] = "'" + ((string)o).Replace("'", "''") + "'";
                }
                else
                {
                    text[i++] = o.ToString();
                }
            }

            lbxUniqueValues.SuspendLayout();
            lbxUniqueValues.Items.Clear();
            lbxUniqueValues.Items.AddRange(text);
            lbxUniqueValues.ResumeLayout();
            lbxUniqueValues.Enabled = true;
            lbxUniqueValues.BackColor = Color.White;
            btnGetUniqueValues.Enabled = false;
        }

        private void BtnGreaterThanClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "> ";
        }

        private void BtnGreaterThanOrEqualClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = ">= ";
        }

        private void BtnLessThanClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "< ";
        }

        private void BtnLessThanOrEqualClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "<= ";
        }

        private void BtnLikeClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "LIKE ";
        }

        private void BtnNotClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "NOT ";
        }

        private void BtnNotEqualClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "<> ";
        }

        private void BtnNotNullClick(object sender, EventArgs e)
        {
            string str = rtbFilterText.SelectedText;
            if (str.Length == 0)
            {
                str = rtbFilterText.Text;
                str = "NOT ((" + str + ") is Null)";
                rtbFilterText.Text = str;
            }
            else
            {
                str = "NOT ((" + str + ") is Null)";
                rtbFilterText.SelectedText = str;
            }
        }

        private void BtnNullClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = " is Null";
        }

        private void BtnOrClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "OR ";
        }

        private void BtnParenthasisClick(object sender, EventArgs e)
        {
            string str = rtbFilterText.SelectedText;
            str = "(" + str + ")";
            rtbFilterText.SelectedText = str;
        }

        private void BtnValidateClick(object sender, EventArgs e)
        {
            ValidateExpression();
        }

        private void LbxFieldsDoubleClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "[" + lbxFields.SelectedItem + "] ";
        }

        private void LbxFieldsSelectedIndexChanged(object sender, EventArgs e)
        {
            btnGetUniqueValues.Enabled = true;
            string field = lbxFields.SelectedItem as string;
            if (field == null) return;
            IComparable min = null;
            lblMin.Text = string.Empty;
            IComparable max = null;
            lblMax.Text = string.Empty;
            if (_attributeSource != null)
            {
                int numPages = (int)Math.Ceiling((double)_attributeSource.NumRows() / 10000);
                for (int page = 0; page < numPages; page++)
                {
                    var table = _attributeSource.GetAttributes(page * 10000, 10000);
                    GetMinMax(table.Rows.Cast<IDataRow>(), field, ref min, ref max);
                }
            }

            if (_table != null)
            {
                GetMinMax(_table.Rows.Cast<IDataRow>(), field, ref min, ref max);
            }

            if (min != null) lblMin.Text = min.ToString();
            if (max != null) lblMax.Text = max.ToString();
        }

        private void LbxUniqueValuesDoubleClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = lbxUniqueValues.SelectedItem + " ";
        }

        private void RtbFilterTextTextChanged(object sender, EventArgs e)
        {
            ExpressionTextChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateFields()
        {
            lbxFields.SuspendLayout();
            lbxFields.Items.Clear();
            if (_attributeSource != null)
            {
                DataColumn[] columns = _attributeSource.GetColumns();
                foreach (DataColumn dc in columns)
                {
                    lbxFields.Items.Add(dc.ColumnName);
                }
            }
            else if (_table != null)
            {
                foreach (DataColumn dc in _table.Columns)
                {
                    lbxFields.Items.Add(dc.ColumnName);
                }
            }

            lbxFields.ResumeLayout();
        }

        #endregion
    }
}