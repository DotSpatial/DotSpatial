// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// JoinDialog.
    /// </summary>
    public partial class JoinDialog : Form
    {
        #region Fields

        private readonly IFeatureSet _featureSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinDialog"/> class.
        /// </summary>
        /// <param name="fs">FeatureSet to join the excel file to.</param>
        public JoinDialog(IFeatureSet fs)
        {
            InitializeComponent();
            _featureSet = fs;
            foreach (DataColumn col in fs.GetColumns())
            {
                cbLocalField.Items.Add(col.ColumnName);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the fileName of the created shapefile.
        /// </summary>
        public string Filename => tbSave.Text;

        #endregion

        #region Methods

        private static List<string> GetColumnNamesFromExcel(string xlsFilePath)
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + xlsFilePath + "; Extended Properties=Excel 8.0");

            // GIS Group of Maryland Environmental Service recommended this query string.
            OleDbDataAdapter da = new OleDbDataAdapter("select * from [Data$]", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return (from DataColumn column in dt.Columns select column.ColumnName).ToList();
        }

        private void CmdBrowseClick(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = @"Excel Files|*.xls" })
            {
                if (ofd.ShowDialog(this) != DialogResult.OK) return;

                tbPath.Text = ofd.FileName;
                List<string> cols = GetColumnNamesFromExcel(ofd.FileName);
                cbForeignField.Items.Clear();
                foreach (var col in cols)
                {
                    cbForeignField.Items.Add(col);
                }
            }
        }

        private void CmdOkClick(object sender, EventArgs e)
        {
            if (tbPath.Text == null)
            {
                MessageBox.Show(SymbologyFormsMessageStrings.JoinDialog_SelectExcelFile);
                return;
            }

            string local = (string)cbLocalField.SelectedItem;
            if (string.IsNullOrEmpty(local))
            {
                MessageBox.Show(SymbologyFormsMessageStrings.JoinDialog_SelectLocalField);
                return;
            }

            string foreign = (string)cbForeignField.SelectedItem;
            if (string.IsNullOrEmpty(foreign))
            {
                MessageBox.Show(SymbologyFormsMessageStrings.JoinDialog_SelectForeignField);
                return;
            }

            if (string.IsNullOrEmpty(tbSave.Text))
            {
                MessageBox.Show(SymbologyFormsMessageStrings.JoinDialog_SelectOutputFile);
                return;
            }

            IFeatureSet temp = _featureSet.Join(tbPath.Text, local, foreign);
            temp.SaveAs(tbSave.Text, true);
        }

        private void CmdSaveClick(object sender, EventArgs e)
        {
            using (SaveFileDialog ofd = new SaveFileDialog { Filter = DataManager.DefaultDataManager.VectorWriteFilter })
            {
                if (ofd.ShowDialog(this) != DialogResult.OK) return;
                tbSave.Text = ofd.FileName;
            }
        }

        #endregion
    }
}