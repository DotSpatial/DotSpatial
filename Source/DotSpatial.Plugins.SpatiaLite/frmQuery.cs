using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Plugins.SpatiaLite.Properties;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// The window can be used to query SpatiaLite databases.
    /// </summary>
    public partial class FrmQuery : Form
    {
        private readonly string _connString;
        private readonly IMap _mainMap;
        private IFeatureSet _queryResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmQuery"/> class.
        /// </summary>
        /// <param name="dbConnection">The connectionstring for the SpatiaLite database.</param>
        /// <param name="map">The map layers get added to.</param>
        public FrmQuery(string dbConnection, IMap map)
        {
            InitializeComponent();

            _connString = dbConnection;
            _mainMap = map;

            SpatiaLiteHelper slh = new SpatiaLiteHelper();
            List<GeometryColumnInfo> geometryColumnList = slh.GetGeometryColumns(dbConnection);

            // get names of tables
            List<string> tableNameList = slh.GetTableNames(_connString);
            foreach (string tableName in tableNameList)
            {
                try
                {
                    List<string> colNames = slh.GetColumnNames(_connString, tableName);
                    TreeNode nTableName = treeTables.Nodes.Add(tableName);
                    foreach (string cn in colNames)
                    {
                        nTableName.Nodes.Add(cn);
                    }
                }
                catch { }
            }
        }

        // when clicking "OK"
        private void BtnOkClick(object sender, EventArgs e)
        {
            _queryResult = null;
            SpatiaLiteHelper slh = new SpatiaLiteHelper();
            _queryResult = slh.ReadFeatureSet(_connString, txtQuery.Text);

            dgQueryResult.DataSource = _queryResult.DataTable;
        }

        private void BtnAddToMapClick(object sender, EventArgs e)
        {
            if (_queryResult == null)
            {
                MessageBox.Show(Resources.NoValidQueryResultFound);
                return;
            }

            _queryResult.Name = "Query 1";
            _mainMap.Layers.Add(_queryResult);
        }
    }
}