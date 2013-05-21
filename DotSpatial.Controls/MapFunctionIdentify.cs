// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/2/2009 12:18:49 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using System.Diagnostics;



namespace DotSpatial.Controls
{
    /// <summary>
    /// IdentifyFunction
    /// </summary>
    public class MapFunctionIdentify : MapFunction
    {
        #region Private Variables

        FeatureIdentifier _frmFeatureIdentifier;
        IFeature feature;
        IFeature formerFeature;
        IFeatureLayer layer;
        IFeatureLayer formerLayer;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of IdentifyFunction
        /// </summary>
        public MapFunctionIdentify(IMap inMap)
            : base(inMap)
        {
            YieldStyle = YieldStyles.LeftButton;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Overrides the OnMouseUp event to handle the situation where we are trying to
        /// identify the vector features in the specified area.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            Rectangle rtol = new Rectangle(e.X - 8, e.Y - 8, 16, 16);
            Rectangle rstr = new Rectangle(e.X - 1, e.Y - 1, 2, 2);
            Extent tolerant = e.Map.PixelToProj(rtol);
            Extent strict = e.Map.PixelToProj(rstr);

            if (_frmFeatureIdentifier == null)
            {
                _frmFeatureIdentifier = new FeatureIdentifier(Map);
            }

           
            _frmFeatureIdentifier.SuspendLayout();
            _frmFeatureIdentifier.Clear();
            Identify(e.Map.MapFrame.GetLayers(), strict, tolerant);

            //Synchronizes the legend with the identify tool
            Legend legend = Map.Legend as Legend;
            if (legend != null)
            {
                foreach (LegendBox item in legend._legendBoxes)
                {

                    if (item.Item.IsSelected)
                    {
                        _frmFeatureIdentifier._previouslySelectedLayerName = item.Item.LegendText;
                    }
                }
                legend.AddFeatureIdentifier(_frmFeatureIdentifier);
            }

            _frmFeatureIdentifier.ReSelect();
            _frmFeatureIdentifier.ResumeLayout();

            if (_frmFeatureIdentifier.Visible == false)
            {
                _frmFeatureIdentifier.Show(Map.MapFrame != null ? Map.MapFrame.Parent : null);
            }
            base.OnMouseUp(e);
            //Code for making the Identify Tool actually highlight what is being clicked.  
            //However, it needs more adjusting to work properly and will be shelved for now
            /*
            if (_frmFeatureIdentifier.treFeatures.SelectedNode.Tag != null && _frmFeatureIdentifier.treFeatures.SelectedNode.Parent.Tag != null)
            {
                formerFeature = feature;
                formerLayer = layer;
               
                feature = _frmFeatureIdentifier.treFeatures.SelectedNode.Tag as IFeature;
                layer = _frmFeatureIdentifier.treFeatures.SelectedNode.Parent.Tag as IFeatureLayer;

                if (feature != null && layer != null)
                {
                    layer.Select(feature);
                    if (formerFeature != null && formerLayer != null)
                    {
                        formerLayer.UnSelect(formerFeature);
                    }
                }
            }
            */
        }

        private void Identify(IEnumerable<ILayer> layers, Extent strict, Extent tolerant)
        {
            
            List<ILayer> layers2 = layers.ToList();
            if (layers is IGroup) layers2.Reverse();

            foreach (IMapLayer layer in layers2)
            {
               
                IGroup grp = layer as IGroup;
                if (grp != null)
                {
                    Identify(grp, strict, tolerant);
                }
                else
                {
                    var gfl = layer as IMapFeatureLayer;
                    if (gfl != null)
                    {
                        if (gfl.DataSet.FeatureType == FeatureType.Polygon)
                        {
                            _frmFeatureIdentifier.Add(gfl, strict);
                        }
                        else
                        {
                            _frmFeatureIdentifier.Add(gfl, tolerant);
                        }
                    }

                    var rl = layer as IMapRasterLayer;
                    if (rl != null)
                    {
                        _frmFeatureIdentifier.Add(rl, strict);
                    }
                }
            }
        }

        #endregion
    }
}