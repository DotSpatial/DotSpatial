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

namespace DotSpatial.Controls
{
    /// <summary>
    /// Map Identify Function. Used to show information about map layers under map cursor.
    /// </summary>
    public class MapFunctionIdentify : MapFunction
    {
        #region Private Variables

        private FeatureIdentifier _frmFeatureIdentifier;

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
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            var rtol = new Rectangle(e.X - 8, e.Y - 8, 16, 16);
            var rstr = new Rectangle(e.X - 1, e.Y - 1, 2, 2);
            var tolerant = e.Map.PixelToProj(rtol);
            var strict = e.Map.PixelToProj(rstr);

            if (_frmFeatureIdentifier == null || _frmFeatureIdentifier.IsDisposed)
            {
                _frmFeatureIdentifier = new FeatureIdentifier();
            }
            _frmFeatureIdentifier.treFeatures.BeginUpdate();
            _frmFeatureIdentifier.SuspendLayout();
            _frmFeatureIdentifier.Clear();

            Identify(e.Map.MapFrame.GetLayers(), strict, tolerant);

            _frmFeatureIdentifier.ReSelect();
            _frmFeatureIdentifier.ResumeLayout();
           
            SetSelectToSelectedNode(e.Map);
            _frmFeatureIdentifier.treFeatures.EndUpdate();

            if (!_frmFeatureIdentifier.Visible)
            {
                _frmFeatureIdentifier.Show ();
            }

            base.OnMouseUp(e);
        }

        private void SetSelectToSelectedNode(IMap map)
        {
            // todo: Maxim: not sure that we need this...
            // Why we are selecting only one layer when the tool can return more than one? 
            // Maybe need to do this selecting stuff optional?

            /* This logic is used to clear all selections on the entire map and only select a single feature when using the identify tool
                 To get it exactly as desired, I had to get the top layer, which is the mapframe, and perform a ClearSelection from there and then return
                 to the original layer selected in the legend. */

            var layers = map.MapFrame.GetAllLayers();
            ILayer tempLayer = null;
            foreach (var mapLayer in layers)
            {
                if (mapLayer.IsSelected)
                {
                    tempLayer = mapLayer;
                    mapLayer.IsSelected = false;
                }
            }
            if (tempLayer == null)
            {
                tempLayer = map.MapFrame;
            }
            map.MapFrame.IsSelected = true;
            IEnvelope env;
            map.MapFrame.ClearSelection(out env);
            map.MapFrame.IsSelected = false;
            tempLayer.IsSelected = true;

            var selectedNode = _frmFeatureIdentifier.treFeatures.SelectedNode;
            if (selectedNode == null || selectedNode.Parent == null) return;
            var feature = selectedNode.Tag as IFeature;
            var layer = selectedNode.Parent.Tag as IFeatureLayer;
            if (feature != null && layer != null && layer.IsVisible)
            {
                layer.Select(feature);
            }
        }

        private void Identify(IEnumerable<ILayer> layers, Extent strict, Extent tolerant)
        {
            if (layers is IGroup) layers = layers.Reverse();
            foreach (var lr in layers)
            {
                var grp = lr as IGroup;
                if (grp != null)
                {
                    Identify(grp, strict, tolerant);
                }
                else
                {
                    var gfl = lr as IMapFeatureLayer;
                    if (gfl != null && gfl.IsVisible)
                    {
                        _frmFeatureIdentifier.Add(gfl,
                            gfl.DataSet.FeatureType == FeatureType.Polygon ? strict : tolerant);
                        continue;
                    }

                    var rl = lr as IMapRasterLayer;
                    if (rl != null)
                    {
                        _frmFeatureIdentifier.Add(rl, strict);
                        continue;
                    }
                }
            }
        }

        #endregion
    }
}