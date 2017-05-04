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
        private readonly string _connString;
        private readonly IMap _mainMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmAddLayer"/> class.
        /// </summary>
        /// <param name="dbConnection">The connectionstring to the SQLite database.</param>
        /// <param name="map">The map the layer will be added to.</param>
        public FrmAddLayer(string dbConnection, IMap map)
        {
            InitializeComponent();

            label2.Text = string.Format(Resources.Database0, SqLiteHelper.GetSqLiteFileName(dbConnection));

            _connString = dbConnection;
            _mainMap = map;

            SpatiaLiteHelper slh = new SpatiaLiteHelper();
            List<GeometryColumnInfo> geometryColumnList = slh.GetGeometryColumns(dbConnection);
            dgGeometryColumns.DataSource = geometryColumnList;
        }

        // when clicking "OK"
        private void BtnOkClick(object sender, EventArgs e)
        {
            SpatiaLiteHelper slh = new SpatiaLiteHelper();

            foreach (DataGridViewRow r in dgGeometryColumns.Rows)
            {
                if (r.Selected)
                {
                    GeometryColumnInfo item = r.DataBoundItem as GeometryColumnInfo;
                    if (item != null)
                    {
                        IFeatureSet fs = slh.ReadFeatureSet(_connString, item);
                        _mainMap.Layers.Add(fs);
                    }
                }
            }
        }
    }
}