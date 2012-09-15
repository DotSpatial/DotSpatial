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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This member is virtual to allow custom event handlers to be used instead.
    /// </summary>
    public class FeatureLayerEventReceiver
    {
        private readonly FeatureLayerEventSender _featureLayerEventSender = FeatureLayerEventSender.Instance;

        /// <summary>
        /// Creates a new instance of a FeatureLayerEventReceiver
        /// </summary>
        public FeatureLayerEventReceiver()
        {
            _featureLayerEventSender.ShowPropertiesClicked += Notifier_ShowPropertiesClicked;
            _featureLayerEventSender.ShowAttributesClicked += FeatureLayerShowAttributesClicked;
            _featureLayerEventSender.LabelSetupClicked += FeatureLayerLabelSetupClicked;
            _featureLayerEventSender.LabelExtentsClicked += LabelExtentsClicked;
            _featureLayerEventSender.ExcelJoinClicked += ExcelJoinClicked;
            _featureLayerEventSender.ExportDataClicked += ExportDataClicked;
        }

        /// <summary>
        /// Allows setting the owner for any dialogs that need to be launched.
        /// </summary>
        public IWin32Window Owner { get; set; }

        private void Notifier_ShowPropertiesClicked(object sender, FeatureLayerEventArgs e)
        {
            OnShowPropertiesClicked(sender, e.FeatureLayer);
        }

        /// <summary>
        /// This method is virtual, allowing a differnet custom behavior to override this default behavior
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnShowPropertiesClicked(object sender, IFeatureLayer e)
        {
            LayerDialog dlg = new LayerDialog(e, new FeatureCategoryControl());
            dlg.ShowDialog();
        }

        private static void ExcelJoinClicked(object sender, FeatureSetEventArgs e)
        {
            JoinDialog jd = new JoinDialog(e.FeatureSet);
            if (jd.ShowDialog() != DialogResult.OK) return;
        }

        private void LabelExtentsClicked(object sender, DynamicVisibilityEventArgs e)
        {
            DynamicVisibilityModeDialog dvg = new DynamicVisibilityModeDialog();
            DialogResult dr = dvg.ShowDialog(Owner);
            if (dr == DialogResult.OK)
            {
                e.Item.DynamicVisibilityMode = dvg.DynamicVisibilityMode;
                e.Item.UseDynamicVisibility = true;
            }
        }

        private void FeatureLayerLabelSetupClicked(object sender, LabelLayerEventArgs e)
        {
            var lableSetupDialog = new LabelSetup { Layer = e.LabelLayer};
            lableSetupDialog.Show(Owner);
        }

        private void FeatureLayerShowAttributesClicked(object sender, FeatureLayerEventArgs e)
        {
            var attributeDialog = new AttributeDialog(e.FeatureLayer);
            attributeDialog.Show(Owner);
        }

        private static void LoadFeatureSetAsLayer(FeatureLayerEventArgs e, FeatureSet fs, string newLayerName)
        {
            Type layerType = e.FeatureLayer.GetType();
            var newLayer = (FeatureLayer)Activator.CreateInstance(layerType, fs);

            IGroup parent = e.FeatureLayer.GetParentItem() as IGroup;
            if (parent != null)
            {
                int index = parent.IndexOf(e.FeatureLayer);
                if (newLayer != null)
                {
                    parent.Insert(index + 1, newLayer);
                    var child = parent[index + 1];
                    child.LegendText = newLayer.DataSet.Name = newLayer.Name = newLayerName;
                }
            }
        }

        private void ExportDataClicked(object sender, FeatureLayerEventArgs e)
        {
            ExportFeature frmExport = new ExportFeature();
            frmExport.Filename = e.FeatureLayer.DataSet.Filename;
            if (frmExport.ShowDialog() != DialogResult.OK) return;
            // Create a FeatureSet of features that the client wants exported
            FeatureSet fs = null;
            if (frmExport.FeaturesIndex == 0)
            {
                fs = (FeatureSet)e.FeatureLayer.DataSet;
            }
            else if (frmExport.FeaturesIndex == 1)
            {
                fs = e.FeatureLayer.Selection.ToFeatureSet();
            }
            else if (frmExport.FeaturesIndex == 2)
            {
                List<IFeature> features = e.FeatureLayer.DataSet.Select(e.FeatureLayer.MapFrame.ViewExtents);
                fs = new FeatureSet(features);
                fs.Projection = e.FeatureLayer.Projection;
            }

            if (fs.Features.Count == 0)
            {
                fs.CopyTableSchema(e.FeatureLayer.DataSet);
                fs.FeatureType = e.FeatureLayer.DataSet.FeatureType;
            }

            fs.SaveAs(frmExport.Filename, true);

            DialogResult result = MessageBox.Show(Owner, "Do you want to load the shapefile?", "The layer was exported.", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
                LoadFeatureSetAsLayer(e, fs, Path.GetFileNameWithoutExtension(frmExport.Filename));
        }
    }
}