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

using System;
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

        private FeatureIdentifier _frmFeatureIdentifier;
        private IFeature feature;
        private IFeatureLayer layer;

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
                _frmFeatureIdentifier = new FeatureIdentifier();
            }

            _frmFeatureIdentifier.treFeatures.BeginUpdate();
            _frmFeatureIdentifier.SuspendLayout();
            _frmFeatureIdentifier.Clear();

            Identify(e.Map.MapFrame.GetLayers(), strict, tolerant);
 
            _frmFeatureIdentifier.ReSelect();
            _frmFeatureIdentifier.ResumeLayout();

             if (_frmFeatureIdentifier.Visible == false)
             {
                 _frmFeatureIdentifier.Show(Map.MapFrame != null ? Map.MapFrame.Parent : null);
             }
             base.OnMouseUp(e);

                //Code for making the Identify Tool actually highlight what is being clicked.  
                //However, it needs more adjusting to work properly and will be shelved for now
            try
            {
                
                feature = _frmFeatureIdentifier.treFeatures.SelectedNode.Tag as IFeature;
                layer = _frmFeatureIdentifier.treFeatures.SelectedNode.Parent.Tag as IFeatureLayer;

                /* This logic is used to clear all selections on the entire map and only select a single feature when using the identify tool
                 To get it exactly as desired, I had to get the top layer, which is the mapframe, and perform a ClearSelection from there and then return
                 to the original layer selected in the legend. */
                var layers = e.Map.MapFrame.GetAllLayers();
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
                    tempLayer = e.Map.MapFrame;
                }
                e.Map.MapFrame.IsSelected = true;
                IEnvelope env = new Envelope();
                e.Map.MapFrame.ClearSelection(out env);
                e.Map.MapFrame.IsSelected = false;
                tempLayer.IsSelected = true;
              

                if (feature != null && layer != null && layer.IsVisible == true)
                {
                    layer.Select(feature);
                }
            }
            catch (NullReferenceException)
            {
                Debug.WriteLine("Clicked area has a null reference");
            }
            finally
            {
                _frmFeatureIdentifier.treFeatures.EndUpdate();
            }
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
                    if (gfl != null && gfl.IsVisible)
                    {
                        if (gfl.DataSet.FeatureType == FeatureType.Polygon)
                        {
                            if (true == _frmFeatureIdentifier.Add(gfl, strict))
                                return;
                        }
                        else
                        {
                            if (true == _frmFeatureIdentifier.Add(gfl, tolerant))
                                return;
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