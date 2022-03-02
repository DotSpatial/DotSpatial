// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// The window can be used to query SpatiaLite databases.
    /// </summary>
    public partial class FrmQuery : Form
    {
        private readonly SpatiaLiteHelper _slh;
        private readonly IMap _mainMap;
        private IFeatureSet _queryResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmQuery"/> class.
        /// </summary>
        /// <param name="slh">The SpatiaLiteHelper that is connected to the database.</param>
        /// <param name="map">The map layers get added to.</param>
        public FrmQuery(SpatiaLiteHelper slh, IMap map)
        {
            InitializeComponent();

            _slh = slh;
            _mainMap = map;

            List<GeometryColumnInfo> geometryColumnList = _slh.GetGeometryColumns();

            // get names of tables
            List<string> tableNameList = slh.GetTableNames();
            foreach (string tableName in tableNameList)
            {
                try
                {
                    List<string> colNames = slh.GetColumnNames(tableName);
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
            _queryResult = _slh.ReadFeatureSet(txtQuery.Text);

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