using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.FindFeature.Properties;
using DotSpatial.Symbology;
using DotSpatial.Symbology.Forms;

namespace DotSpatial.Plugins.FindFeature
{
    /// <summary>
    /// Allows a user to select polygons that match a query.
    /// </summary>
    public class FindFeaturePlugin : Extension
    {
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem(HeaderControl.HomeRootItemKey, "Find", FindTool_Click)
            {
                GroupCaption = "Map Tool",
                SmallImage = Resources.page_white_find_16x16,
                LargeImage = Resources.page_white_find,
            });

            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        /// <summary>
        /// Find a feature by query expression
        /// </summary>
        private void FindTool_Click(object sender, EventArgs e)
        {
            Map mainMap = App.Map as Map;
            List<ILayer> layers;

            if (mainMap != null)
                layers = mainMap.GetAllLayers();
            else
                layers = mainMap.GetLayers();

            IFeatureLayer fl = null;
            foreach (ILayer layer in layers)
            {
                if (layer.IsSelected)
                {
                    fl = layer as IFeatureLayer;
                    break;
                }
            }

            if (fl == null)
            {
                MessageBox.Show("Please select a feature layer.");
                return;
            }

            SQLExpressionDialog qd = new SQLExpressionDialog();
            if (fl.DataSet.AttributesPopulated)
                qd.Table = fl.DataSet.DataTable;
            else
                qd.AttributeSource = fl.DataSet;

            // Note: User must click ok button to see anything.
            if (qd.ShowDialog() == DialogResult.Cancel)
                return;

            if (!String.IsNullOrWhiteSpace(qd.Expression))
            {
                try
                {
                    fl.SelectByAttribute(qd.Expression);
                }
                catch (SyntaxErrorException ex)
                {
                    MessageBox.Show("The syntax of that query isn't quite right: " + ex.Message);
                }
            }
        }
    }
}