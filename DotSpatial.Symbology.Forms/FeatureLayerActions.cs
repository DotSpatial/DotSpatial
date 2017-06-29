// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This member is virtual to allow custom event handlers to be used instead.
    /// </summary>
    public class FeatureLayerActions : LegendItemActionsBase, IFeatureLayerActions
    {
        /// <summary>
        /// Show the properties of a feature layer in the legend.
        /// </summary>
        /// <param name="e"></param>
        public void ShowProperties(IFeatureLayer e)
        {
            using (var dlg = new LayerDialog(e, new FeatureCategoryControl()))
            {
                ShowDialog(dlg);
            }
        }
        /// <summary>
        /// Show the dialog to join an Excel table with a feature set.
        /// </summary>
        /// <param name="e"></param>
        public void ExcelJoin(IFeatureSet e)
        {
            using (var jd = new JoinDialog(e))
            {
                ShowDialog(jd);
            }
        }

        /// <summary>
        /// Show the dialog to set label extents.
        /// </summary>
        /// <param name="e"></param>
        public void LabelExtents(IDynamicVisibility e)
        {
            using (var dvg = new DynamicVisibilityModeDialog())
            {
                if (ShowDialog(dvg) == DialogResult.OK)
                {
                    e.DynamicVisibilityMode = dvg.DynamicVisibilityMode;
                    e.UseDynamicVisibility = true;
                }
            }
        }

        /// <summary>
        /// Show the dialog to set up labels.
        /// </summary>
        /// <param name="e"></param>
        public void LabelSetup(ILabelLayer e)
        {
            using (var dlg = new LabelSetup {Layer = e})
            {
                ShowDialog(dlg);
            }
        }

        /// <summary>
        /// Show the attribute table editor.
        /// </summary>
        /// <param name="e"></param>
        public void ShowAttributes(IFeatureLayer e)
        {
            using (var attributeDialog = new AttributeDialog(e))
            {
                ShowDialog(attributeDialog);
            }
        }

        /// <summary>
        /// Show the dialog for exporting data from a feature layer.
        /// </summary>
        /// <param name="e"></param>
        public void ExportData(IFeatureLayer e)
        {
            using (var frmExport = new ExportFeature())
            {
                frmExport.Filename = e.DataSet.Filename;
                if (ShowDialog(frmExport) != DialogResult.OK) return;

                // Create a FeatureSet of features that the client wants exported
                FeatureSet fs = null;
                switch (frmExport.FeaturesIndex)
                {
                    case 0:
                        fs = (FeatureSet) e.DataSet;
                        break;
                    case 1:
                        fs = e.Selection.ToFeatureSet();
                        break;
                    case 2:
                        var features = e.DataSet.Select(e.MapFrame.ViewExtents);
                        fs = new FeatureSet(features) {Projection = e.Projection};
                        break;
                }

                if (fs.Features.Count == 0)
                {
                    fs.CopyTableSchema(e.DataSet);
                    fs.FeatureType = e.DataSet.FeatureType;
                }

                fs.SaveAs(frmExport.Filename, true);

                if (MessageBox.Show(Owner, "Do you want to load the shapefile?",
                                    "The layer was exported.",
                                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    LoadFeatureSetAsLayer(e, fs, Path.GetFileNameWithoutExtension(frmExport.Filename));
                }
            }
        }

        private static void LoadFeatureSetAsLayer(IFeatureLayer e, FeatureSet fs, string newLayerName)
        {
            var layerType = e.GetType();
            var newLayer = (FeatureLayer)Activator.CreateInstance(layerType, fs);

            var parent = e.GetParentItem() as IGroup;
            if (parent != null)
            {
                int index = parent.IndexOf(e);
                parent.Insert(index + 1, newLayer);
                var child = parent[index + 1];
                child.LegendText = newLayer.DataSet.Name = newLayer.Name = newLayerName;

            }
        }
    }
}