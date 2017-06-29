using System;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.SpatiaLite.Properties;

namespace DotSpatial.Plugins.SpatiaLite
{
    public class SpatiaLitePlugin : Extension
    {
        /// <summary>
        /// When the plugin is activated
        /// </summary>
        public override void Activate()
        {
            //try setting environment variables..
            SpatiaLiteHelper.SetEnvironmentVars();

            string spatiaLiteGroup = "SpatiaLite";

            var bOpenLayer = new SimpleActionItem("Open Layer", ButtonClick)
            {
                LargeImage = Resources.spatialite_open_32,
                SmallImage = Resources.spatialite_open_16,
                ToolTipText = "Add Layer from SpatiaLite",
                GroupCaption = spatiaLiteGroup
            };
            App.HeaderControl.Add(bOpenLayer);

            //query
            var bQuery = new SimpleActionItem("SpatiaLite Query", bQuery_Click)
            {
                LargeImage = Resources.spatialite_query_32,
                SmallImage = Resources.spatialite_query_16,
                ToolTipText = "Run SpatiaLite Query",
                GroupCaption = spatiaLiteGroup
            };
            App.HeaderControl.Add(bQuery);

            //save layer (not implemented yet)
            var bSaveLayer = new SimpleActionItem("Save Layer", bSaveLayer_Click)
            {
                LargeImage = Resources.spatialite_save_32,
                SmallImage = Resources.spatialite_save_16,
                ToolTipText = "Save Layer to SpatiaLite Database",
                GroupCaption = spatiaLiteGroup
            };
            App.HeaderControl.Add(bSaveLayer);
            base.Activate();
        }

        /// <summary>
        /// When the plugin is deactivated
        /// </summary>
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        private void bQuery_Click(object sender, EventArgs e)
        {
            //check if it's a valid SpatiaLite layer
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Open SpatiaLite database";
            fd.Filter = "SpatiaLite database|*.sqlite";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                string cs = SQLiteHelper.GetSQLiteConnectionString(fd.FileName);
                var frm = new frmQuery(cs, App.Map);
                frm.Show();
            }
        }

        private void bSaveLayer_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This operation is not implemented yet");
        }

        public void ButtonClick(object sender, EventArgs e)
        {
            //check if it's a valid SpatiaLite layer
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Open SpatiaLite database";
            fd.Filter = "SpatiaLite database|*.sqlite";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                string cs = SQLiteHelper.GetSQLiteConnectionString(fd.FileName);
                bool isValidSchema = SpatiaLiteHelper.CheckSpatiaLiteSchema(cs);
                if (!isValidSchema)
                {
                    MessageBox.Show("The database " + fd.FileName + " is not a valid SpatiaLite database. Table geometry_columns not found.");
                    return;
                }
                else
                {
                    frmAddLayer frm = new frmAddLayer(cs, App.Map);
                    frm.Show();
                }
            }
        }
    }
}