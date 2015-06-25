// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/21/2009 3:34:26 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
    public partial class SQLQueryControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when the text in the expression textbox has changed.
        /// </summary>
        public event EventHandler ExpressionTextChanged;

        #endregion

        private IAttributeSource _attributeSource;
        private DataTable _table;

        /// <summary>
        /// Gets or sets the string expression text
        /// </summary>
        public string ExpressionText
        {
            get { return rtbFilterText.Text; }
            set { rtbFilterText.Text = value; }
        }

        private void rtbFilterText_TextChanged(object sender, EventArgs e)
        {
            if (ExpressionTextChanged != null) ExpressionTextChanged(this, EventArgs.Empty);
        }

        private void lbxUniqueValues_DoubleClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = lbxUniqueValues.SelectedItem.ToString() + " ";
        }

        /// <summary>
        /// Creates a new instance of the control with no fields specified.
        /// </summary>
        public SQLQueryControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the data Table for this control. Setting this will
        /// automatically update the fields shown in the list.
        /// </summary>
        public DataTable Table
        {
            get { return _table; }
            set
            {
                _table = value;
                if (_table != null)
                {
                    UpdateFields();
                }
            }
        }

        /// <summary>
        /// Setting this is an alternative to specifying the table.  This allows the
        /// query control to build a query using pages of data instead of the whole
        /// table at once.
        /// </summary>
        public IAttributeSource AttributeSource
        {
            get { return _attributeSource; }
            set
            {
                _attributeSource = value;
                UpdateFields();
            }
        }

        /// <summary>
        /// Gets or sets the string that appears in the filter text.
        /// </summary>
        public override string Text
        {
            get { return rtbFilterText.Text; }
            set { rtbFilterText.Text = value; }
        }

        #region Event Handlers

        private void lbxFields_SelectedIndexChanged(object sender, EventArgs e)
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
                    GetMinMax(table.Rows.Cast<DataRow>(), field, ref min, ref max);
                }
            }
            if (_table != null)
            {
                GetMinMax(_table.Rows.Cast<DataRow>(), field, ref min, ref max);
            }
            if (min != null) lblMin.Text = min.ToString();
            if (max != null) lblMax.Text = max.ToString();
        }

        private static void GetMinMax(IEnumerable<DataRow> rows,  string field, ref IComparable min, ref IComparable max)
        {
            foreach (var dr in rows)
            {
                if ((dr[field] is DBNull)) continue;
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

        private void btnGetUniqueValues_Click(object sender, EventArgs e)
        {
            // Sorting should be done as the original objects, not as strings.
            var lst = new HashSet<object>();
            string fieldName = lbxFields.SelectedItem.ToString();
            bool useAll = false;
            bool isString = true;
            if (_attributeSource != null)
            {
                isString = (_attributeSource.GetColumn(fieldName).DataType == typeof(string));
                int numPages = (int)Math.Ceiling((double)_attributeSource.NumRows() / 10000);
                for (int page = 0; page < numPages; page++)
                {
                    DataTable table = _attributeSource.GetAttributes(page * 10000, 10000);
                    foreach (DataRow dr in table.Rows)
                    {
                        if (dr[fieldName] is DBNull) continue;
                        if (lst.Contains(dr[fieldName])) continue;
                        lst.Add(dr[fieldName]);
                    }
                    if (lst.Count <= 10000 || useAll) continue;
                    if (MessageBox.Show("There are more than 10, 000 unique values... do you wish to show all of them?",
                                        "Large Number of Unique Values", MessageBoxButtons.YesNo) == DialogResult.Yes)
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
                isString = (_table.Columns[fieldName].DataType == typeof(string));
                foreach (DataRow dr in _table.Rows)
                {
                    if (dr[fieldName] is DBNull) continue;
                    if (lst.Contains(dr[fieldName])) continue;
                    lst.Add(dr[fieldName]);
                }
            }
            
            var text = new object[lst.Count];
            int i = 0;
            foreach (var o in lst.OrderBy(_ => _))
            {
                if (isString)
                {
                    text[i++] ="'" + ((string)o).Replace("'", "''") + "'";
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

        private void btnEquals_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "= ";
        }

        private void btnNotEqual_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "<> ";
        }

        private void btnLike_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "LIKE ";
        }

        private void btnGreaterThan_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "> ";
        }

        private void btnGreaterThanOrEqual_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = ">= ";
        }

        private void btnAnd_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "AND ";
        }

        private void btnLessThan_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "< ";
        }

        private void btnLessThanOrEqual_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "<= ";
        }

        private void btnOr_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "OR ";
        }

        private void btnAsterix_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "*";
        }

        private void btnParenthasis_Click(object sender, EventArgs e)
        {
            string str = rtbFilterText.SelectedText;
            str = "(" + str + ")";
            rtbFilterText.SelectedText = str;
        }

        private void btnNot_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "NOT ";
        }

        private void lbxFields_DoubleClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = "[" + lbxFields.SelectedItem + "] ";
        }

        private void btnNull_Click(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = " is Null";
        }

        #endregion

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

        private void btnNotNull_Click(object sender, EventArgs e)
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
    }
}