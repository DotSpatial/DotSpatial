// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Plugins.SpatiaLite.Properties;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// This form shows the tables of a SQLite database so they can be added to map.
    /// </summary>
    public partial class FrmAddLayer : Form
    {
        private readonly IMap _mainMap;
        private readonly SpatiaLiteHelper _slh;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmAddLayer"/> class.
        /// </summary>
        /// <param name="slh">The SpatiaLiteHelper that is connected to the SQLite database.</param>
        /// <param name="map">The map the layer will be added to.</param>
        public FrmAddLayer(SpatiaLiteHelper slh, IMap map)
        {
            InitializeComponent();

            label2.Text = string.Format(Resources.Database0, SqLiteHelper.GetSqLiteFileName(slh.ConnectionString));

            _slh = slh;
            _mainMap = map;

            List<GeometryColumnInfo> geometryColumnList = _slh.GetGeometryColumns();
            dgGeometryColumns.DataSource = geometryColumnList;
        }

        // when clicking "OK"
        private void BtnOkClick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dgGeometryColumns.SelectedRows)
            {
                GeometryColumnInfo item = r.DataBoundItem as GeometryColumnInfo;
                if (item != null)
                {
                    IFeatureSet fs = _slh.ReadFeatureSet(item);
                    _mainMap.Layers.Add(fs);
                }
            }

            Close();
        }
    }
}