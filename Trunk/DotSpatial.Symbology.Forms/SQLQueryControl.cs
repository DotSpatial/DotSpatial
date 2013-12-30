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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Creates a new instance of the SQLQueryControl
    /// </summary>
    public class SQLQueryControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when the text in the expression textbox has changed.
        /// </summary>
        public event EventHandler ExpressionTextChanged;

        #endregion

        private IAttributeSource _attributeSource;
        private DataTable _table;
        private Button btnAnd;
        private Button btnAsterix;
        private Button btnEquals;
        private Button btnGetUniqueValues;
        private Button btnGreaterThan;
        private Button btnGreaterThanOrEqual;
        private Button btnLessThan;
        private Button btnLessThanOrEqual;
        private Button btnLike;
        private Button btnNot;
        private Button btnNotEqual;
        private Button btnNotNull;
        private Button btnNull;
        private Button btnOr;
        private Button btnParenthasis;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private Label label1;
        private Label label2;
        private Label lblFields;
        private Label lblMax;
        private Label lblMin;
        private Label lblSelectPrecursor;
        private Label lblUniqueValues;
        private ListBox lbxFields;
        private ListBox lbxUniqueValues;
        private RichTextBox rtbFilterText;

        #region Component Designer generated code

        /// <summary>
        /// Gets or sets the string expression text
        /// </summary>
        public string ExpressionText
        {
            get { return rtbFilterText.Text; }
            set { rtbFilterText.Text = value; }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQLQueryControl));
            this.lblMax = new System.Windows.Forms.Label();
            this.lblMin = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFields = new System.Windows.Forms.Label();
            this.lblUniqueValues = new System.Windows.Forms.Label();
            this.lblSelectPrecursor = new System.Windows.Forms.Label();
            this.rtbFilterText = new System.Windows.Forms.RichTextBox();
            this.btnGetUniqueValues = new System.Windows.Forms.Button();
            this.btnNot = new System.Windows.Forms.Button();
            this.btnParenthasis = new System.Windows.Forms.Button();
            this.btnAsterix = new System.Windows.Forms.Button();
            this.btnOr = new System.Windows.Forms.Button();
            this.btnLessThanOrEqual = new System.Windows.Forms.Button();
            this.btnLessThan = new System.Windows.Forms.Button();
            this.btnAnd = new System.Windows.Forms.Button();
            this.btnGreaterThanOrEqual = new System.Windows.Forms.Button();
            this.btnGreaterThan = new System.Windows.Forms.Button();
            this.btnLike = new System.Windows.Forms.Button();
            this.btnNotEqual = new System.Windows.Forms.Button();
            this.btnEquals = new System.Windows.Forms.Button();
            this.lbxUniqueValues = new System.Windows.Forms.ListBox();
            this.lbxFields = new System.Windows.Forms.ListBox();
            this.btnNull = new System.Windows.Forms.Button();
            this.btnNotNull = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblMax
            //
            resources.ApplyResources(this.lblMax, "lblMax");
            this.lblMax.BackColor = System.Drawing.Color.White;
            this.lblMax.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMax.Name = "lblMax";
            //
            // lblMin
            //
            resources.ApplyResources(this.lblMin, "lblMin");
            this.lblMin.BackColor = System.Drawing.Color.White;
            this.lblMin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMin.Name = "lblMin";
            //
            // label2
            //
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            //
            // label1
            //
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            //
            // lblFields
            //
            resources.ApplyResources(this.lblFields, "lblFields");
            this.lblFields.Name = "lblFields";
            //
            // lblUniqueValues
            //
            resources.ApplyResources(this.lblUniqueValues, "lblUniqueValues");
            this.lblUniqueValues.Name = "lblUniqueValues";
            //
            // lblSelectPrecursor
            //
            resources.ApplyResources(this.lblSelectPrecursor, "lblSelectPrecursor");
            this.lblSelectPrecursor.Name = "lblSelectPrecursor";
            //
            // rtbFilterText
            //
            resources.ApplyResources(this.rtbFilterText, "rtbFilterText");
            this.rtbFilterText.Name = "rtbFilterText";
            this.rtbFilterText.TextChanged += new System.EventHandler(this.rtbFilterText_TextChanged);
            //
            // btnGetUniqueValues
            //
            resources.ApplyResources(this.btnGetUniqueValues, "btnGetUniqueValues");
            this.btnGetUniqueValues.Name = "btnGetUniqueValues";
            this.btnGetUniqueValues.UseVisualStyleBackColor = true;
            this.btnGetUniqueValues.Click += new System.EventHandler(this.btnGetUniqueValues_Click);
            //
            // btnNot
            //
            resources.ApplyResources(this.btnNot, "btnNot");
            this.btnNot.Name = "btnNot";
            this.btnNot.UseVisualStyleBackColor = true;
            this.btnNot.Click += new System.EventHandler(this.btnNot_Click);
            //
            // btnParenthasis
            //
            resources.ApplyResources(this.btnParenthasis, "btnParenthasis");
            this.btnParenthasis.Name = "btnParenthasis";
            this.btnParenthasis.UseVisualStyleBackColor = true;
            this.btnParenthasis.Click += new System.EventHandler(this.btnParenthasis_Click);
            //
            // btnAsterix
            //
            resources.ApplyResources(this.btnAsterix, "btnAsterix");
            this.btnAsterix.Name = "btnAsterix";
            this.btnAsterix.UseVisualStyleBackColor = true;
            this.btnAsterix.Click += new System.EventHandler(this.btnAsterix_Click);
            //
            // btnOr
            //
            resources.ApplyResources(this.btnOr, "btnOr");
            this.btnOr.Name = "btnOr";
            this.btnOr.UseVisualStyleBackColor = true;
            this.btnOr.Click += new System.EventHandler(this.btnOr_Click);
            //
            // btnLessThanOrEqual
            //
            resources.ApplyResources(this.btnLessThanOrEqual, "btnLessThanOrEqual");
            this.btnLessThanOrEqual.Name = "btnLessThanOrEqual";
            this.btnLessThanOrEqual.UseVisualStyleBackColor = true;
            this.btnLessThanOrEqual.Click += new System.EventHandler(this.btnLessThanOrEqual_Click);
            //
            // btnLessThan
            //
            resources.ApplyResources(this.btnLessThan, "btnLessThan");
            this.btnLessThan.Name = "btnLessThan";
            this.btnLessThan.UseVisualStyleBackColor = true;
            this.btnLessThan.Click += new System.EventHandler(this.btnLessThan_Click);
            //
            // btnAnd
            //
            resources.ApplyResources(this.btnAnd, "btnAnd");
            this.btnAnd.Name = "btnAnd";
            this.btnAnd.UseVisualStyleBackColor = true;
            this.btnAnd.Click += new System.EventHandler(this.btnAnd_Click);
            //
            // btnGreaterThanOrEqual
            //
            resources.ApplyResources(this.btnGreaterThanOrEqual, "btnGreaterThanOrEqual");
            this.btnGreaterThanOrEqual.Name = "btnGreaterThanOrEqual";
            this.btnGreaterThanOrEqual.UseVisualStyleBackColor = true;
            this.btnGreaterThanOrEqual.Click += new System.EventHandler(this.btnGreaterThanOrEqual_Click);
            //
            // btnGreaterThan
            //
            resources.ApplyResources(this.btnGreaterThan, "btnGreaterThan");
            this.btnGreaterThan.Name = "btnGreaterThan";
            this.btnGreaterThan.UseVisualStyleBackColor = true;
            this.btnGreaterThan.Click += new System.EventHandler(this.btnGreaterThan_Click);
            //
            // btnLike
            //
            resources.ApplyResources(this.btnLike, "btnLike");
            this.btnLike.Name = "btnLike";
            this.btnLike.UseVisualStyleBackColor = true;
            this.btnLike.Click += new System.EventHandler(this.btnLike_Click);
            //
            // btnNotEqual
            //
            resources.ApplyResources(this.btnNotEqual, "btnNotEqual");
            this.btnNotEqual.Name = "btnNotEqual";
            this.btnNotEqual.UseVisualStyleBackColor = true;
            this.btnNotEqual.Click += new System.EventHandler(this.btnNotEqual_Click);
            //
            // btnEquals
            //
            resources.ApplyResources(this.btnEquals, "btnEquals");
            this.btnEquals.Name = "btnEquals";
            this.btnEquals.UseVisualStyleBackColor = true;
            this.btnEquals.Click += new System.EventHandler(this.btnEquals_Click);
            //
            // lbxUniqueValues
            //
            resources.ApplyResources(this.lbxUniqueValues, "lbxUniqueValues");
            this.lbxUniqueValues.BackColor = System.Drawing.SystemColors.Control;
            this.lbxUniqueValues.FormattingEnabled = true;
            this.lbxUniqueValues.Name = "lbxUniqueValues";
            this.lbxUniqueValues.DoubleClick += new System.EventHandler(this.lbxUniqueValues_DoubleClick);
            //
            // lbxFields
            //
            this.lbxFields.FormattingEnabled = true;
            resources.ApplyResources(this.lbxFields, "lbxFields");
            this.lbxFields.Name = "lbxFields";
            this.lbxFields.SelectedIndexChanged += new System.EventHandler(this.lbxFields_SelectedIndexChanged);
            this.lbxFields.DoubleClick += new System.EventHandler(this.lbxFields_DoubleClick);
            //
            // btnNull
            //
            resources.ApplyResources(this.btnNull, "btnNull");
            this.btnNull.Name = "btnNull";
            this.btnNull.UseVisualStyleBackColor = true;
            this.btnNull.Click += new System.EventHandler(this.btnNull_Click);
            //
            // btnNotNull
            //
            resources.ApplyResources(this.btnNotNull, "btnNotNull");
            this.btnNotNull.Name = "btnNotNull";
            this.btnNotNull.UseVisualStyleBackColor = true;
            this.btnNotNull.Click += new System.EventHandler(this.btnNotNull_Click);
            //
            // SQLQueryControl
            //
            this.Controls.Add(this.btnNotNull);
            this.Controls.Add(this.btnNull);
            this.Controls.Add(this.lblMax);
            this.Controls.Add(this.lblMin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblFields);
            this.Controls.Add(this.lblUniqueValues);
            this.Controls.Add(this.lblSelectPrecursor);
            this.Controls.Add(this.rtbFilterText);
            this.Controls.Add(this.btnGetUniqueValues);
            this.Controls.Add(this.btnNot);
            this.Controls.Add(this.btnParenthasis);
            this.Controls.Add(this.btnAsterix);
            this.Controls.Add(this.btnOr);
            this.Controls.Add(this.btnLessThanOrEqual);
            this.Controls.Add(this.btnLessThan);
            this.Controls.Add(this.btnAnd);
            this.Controls.Add(this.btnGreaterThanOrEqual);
            this.Controls.Add(this.btnGreaterThan);
            this.Controls.Add(this.btnLike);
            this.Controls.Add(this.btnNotEqual);
            this.Controls.Add(this.btnEquals);
            this.Controls.Add(this.lbxUniqueValues);
            this.Controls.Add(this.lbxFields);
            this.Name = "SQLQueryControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void rtbFilterText_TextChanged(object sender, EventArgs e)
        {
            if (ExpressionTextChanged != null) ExpressionTextChanged(this, new EventArgs());
        }

        private void lbxUniqueValues_DoubleClick(object sender, EventArgs e)
        {
            rtbFilterText.SelectedText = lbxUniqueValues.SelectedItem.ToString() + " ";
        }

        #endregion

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

        #region Protected Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

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
                    DataTable table = _attributeSource.GetAttributes(page * 10000, 10000);
                    foreach (DataRow dr in table.Rows)
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
            }
            if (_table != null)
            {
                foreach (DataRow dr in _table.Rows)
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
            if (min != null) lblMin.Text = min.ToString();
            if (max != null) lblMax.Text = max.ToString();
        }

        private void btnGetUniqueValues_Click(object sender, EventArgs e)
        {
            // Sorting should be done as the original objects, not as strings.
            ArrayList lst = new ArrayList();
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

            lst.Sort();
            List<string> text = new List<string>();

            foreach (object o in lst)
            {
                if (isString)
                {
                    text.Add("'" + ((string)o).Replace("'", "''") + "'");
                }
                else
                {
                    text.Add(o.ToString());
                }
            }
            lbxUniqueValues.SuspendLayout();
            lbxUniqueValues.Items.Clear();
            lbxUniqueValues.Items.AddRange(text.ToArray());
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