using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;

namespace DotSpatial.Plugins.SpatiaLite
{
    public partial class frmQuery : Form
    {
        private string connString;
        private IMap mainMap;
        private IFeatureSet queryResult;

        public frmQuery(string dbConnection, IMap map)
        {
            InitializeComponent();

            connString = dbConnection;
            mainMap = map;

            SpatiaLiteHelper slh = new SpatiaLiteHelper();
            List<GeometryColumnInfo> geometryColumnList = slh.GetGeometryColumns(dbConnection);

            //get names of tables
            List<string> tableNameList = slh.GetTableNames(connString);
            foreach (string tableName in tableNameList)
            {
                try
                {
                    List<string> colNames = slh.GetColumnNames(connString, tableName);
                    TreeNode nTableName = treeTables.Nodes.Add(tableName);
                    foreach (string cn in colNames)
                    {
                        nTableName.Nodes.Add(cn);
                    }
                }
                catch { }
            }
        }

        //when clicking "OK"
        private void btnOK_Click(object sender, EventArgs e)
        {
            queryResult = null;
            SpatiaLiteHelper slh = new SpatiaLiteHelper();
            queryResult = slh.ReadFeatureSet(connString, txtQuery.Text);

            dgQueryResult.DataSource = queryResult.DataTable;

            //SpatiaLiteHelper slh = new SpatiaLiteHelper();

            //foreach (DataGridViewRow r in dgGeometryColumns.Rows)
            //{
            //    if (r.Selected)
            //    {
            //        GeometryColumnInfo item = r.DataBoundItem as GeometryColumnInfo;
            //        if (item != null)
            //        {
            //            IFeatureSet fs = slh.ReadFeatureSet(connString, item);

            //            IMapFeatureLayer lay = mainMap.Layers.Add(fs);
            //            //lay.EditMode = false;

            //        }
            //    }
            //}
        }

        private void btnAddToMap_Click(object sender, EventArgs e)
        {
            if (queryResult == null)
            {
                MessageBox.Show("No valid query result found. Please re-run the query.");
                return;
            }

            queryResult.Name = "Query 1";
            mainMap.Layers.Add(queryResult);
        }
    }
}