// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        #region Methods

        /// <summary>
        /// Show the dialog to join an Excel table with a feature set.
        /// </summary>
        /// <param name="layer">Layer whose join to excel table dialog is shown.</param>
        public void ExcelJoin(IFeatureSet layer)
        {
            using (var jd = new JoinDialog(layer))
            {
                ShowDialog(jd);
            }
        }

        /// <summary>
        /// Show the dialog for exporting data from a feature layer.
        /// </summary>
        /// <param name="layer">Layer whose data gets exported.</param>
        public void ExportData(IFeatureLayer layer)
        {
            using (var frmExport = new ExportFeature())
            {
                frmExport.Filename = layer.DataSet.Filename;
                if (ShowDialog(frmExport) != DialogResult.OK) return;

                // Create a FeatureSet of features that the client wants exported
                // CGX
                // FeatureSet fs = null;
                IFeatureSet fs = null;
                // FIN CGX
                switch (frmExport.FeaturesIndex)
                {
                    case 0:
                        // CGX
                        // fs = (FeatureSet)layer.DataSet;
                        fs = layer.DataSet;
                        // Fin CGX
                        break;
                    case 1:
                        fs = layer.Selection.ToFeatureSet();
                        break;
                    case 2:
                        var features = layer.DataSet.Select(layer.MapFrame.ViewExtents);
                        fs = new FeatureSet(features)
                        {
                            Projection = layer.Projection
                        };
                        break;
                }

                if (fs == null) return;

                if (fs.Features.Count == 0)
                {
                    fs.CopyTableSchema(layer.DataSet);
                    fs.FeatureType = layer.DataSet.FeatureType;
                }

                fs.SaveAs(frmExport.Filename, true);

                if (MessageBox.Show(Owner, SymbologyFormsMessageStrings.FeatureLayerActions_LoadFeatures, SymbologyFormsMessageStrings.FeatureLayerActions_FeaturesExported, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    LoadFeatureSetAsLayer(layer, fs, Path.GetFileNameWithoutExtension(frmExport.Filename));
                }
            }
        }

        /// <summary>
        /// Show the dialog to set label extents.
        /// </summary>
        /// <param name="layer">Layer whose dynamic visibility dialog is shown.</param>
        public void LabelExtents(IDynamicVisibility layer)
        {
            using (var dvg = new DynamicVisibilityModeDialog())
            {
                if (ShowDialog(dvg) == DialogResult.OK)
                {
                    layer.DynamicVisibilityMode = dvg.DynamicVisibilityMode;
                    layer.UseDynamicVisibility = true;
                }
            }
        }

        /// <summary>
        /// Show the dialog to set up labels.
        /// </summary>
        /// <param name="layer">Layer whose label setup dialog is shown.</param>
        public void LabelSetup(ILabelLayer layer)
        {
            using (var dlg = new LabelSetup { Layer = layer })
            {
                ShowDialog(dlg);
            }
        }

        // CGX
        public void LabelSetup2(ILabelLayer e)
        {
            using (var dlg = new LabelSetup2 { Layer = e })
            {
                ShowDialog(dlg);
            }
        }
        // CGX END

        /// <inheritdoc />
        public void SelectByAttributes(FeatureLayer featureLayer)
        {
            using (var form = new SelectByAttributes(featureLayer))
            {
                form.ShowDialog(Owner);
            }
        }

        /// <summary>
        /// Show the attribute table editor.
        /// </summary>
        /// <param name="layer">Layer whose attribute table is shown.</param>
        public void ShowAttributes(IFeatureLayer layer)
        {
            using (var attributeDialog = new AttributeDialog(layer))
            {
                ShowDialog(attributeDialog);
            }
        }

        /// <summary>
        /// Show the properties of a feature layer in the legend.
        /// </summary>
        /// <param name="layer">Layer whose properties are shown.</param>
        public void ShowProperties(IFeatureLayer layer)
        {
            using (var dlg = new LayerDialog(layer, new FeatureCategoryControl()))
            {
                ShowDialog(dlg);
            }
        }

        // CGX
        // private static void LoadFeatureSetAsLayer(IFeatureLayer layer, FeatureSet fs, string newLayerName)
        private static void LoadFeatureSetAsLayer(IFeatureLayer layer, IFeatureSet fs, string newLayerName)
        {
            var layerType = layer.GetType();
            var newLayer = (FeatureLayer)Activator.CreateInstance(layerType, fs);

            var parent = layer.GetParentItem() as IGroup;
            if (parent != null)
            {
                int index = parent.IndexOf(layer);
                parent.Insert(index + 1, newLayer);
                var child = parent[index + 1];
                child.LegendText = newLayer.DataSet.Name = newLayer.Name = newLayerName;
            }
        }

        #endregion
    }
}