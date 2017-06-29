using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;

namespace DotSpatial.Plugins.SpatiaLite
{
    public partial class frmAddLayer : Form
    {
        private string connString;
        private IMap mainMap;

        public frmAddLayer(string dbConnection, IMap map)
        {
            InitializeComponent();

            label2.Text = "Database: " + SQLiteHelper.GetSQLiteFileName(dbConnection);

            connString = dbConnection;
            mainMap = map;

            SpatiaLiteHelper slh = new SpatiaLiteHelper();
            List<GeometryColumnInfo> geometryColumnList = slh.GetGeometryColumns(dbConnection);
            dgGeometryColumns.DataSource = geometryColumnList;
        }

        //when clicking "OK"
        private void btnOK_Click(object sender, EventArgs e)
        {
            SpatiaLiteHelper slh = new SpatiaLiteHelper();

            foreach (DataGridViewRow r in dgGeometryColumns.Rows)
            {
                if (r.Selected)
                {
                    GeometryColumnInfo item = r.DataBoundItem as GeometryColumnInfo;
                    if (item != null)
                    {
                        IFeatureSet fs = slh.ReadFeatureSet(connString, item);

                        IMapFeatureLayer lay = mainMap.Layers.Add(fs);
                        //lay.EditMode = false;
                    }
                }
            }
        }
    }
}