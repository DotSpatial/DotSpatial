// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Map Identify Function. Used to show information about map layers under map cursor.
    /// </summary>
    public class MapFunctionIdentify : MapFunction
    {
        #region Fields

        private FeatureIdentifier _frmFeatureIdentifier;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFunctionIdentify"/> class.
        /// </summary>
        /// <param name="inMap">The map the tool should work on.</param>
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
        /// <param name="e">The event args.</param>
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

            Identify(e.Map.MapFrame.GetLayers().Reverse(), strict, tolerant);

            _frmFeatureIdentifier.ReSelect();
            _frmFeatureIdentifier.ResumeLayout();

            SetSelectToSelectedNode(e.Map);
            _frmFeatureIdentifier.treFeatures.EndUpdate();

            if (!_frmFeatureIdentifier.Visible)
            {
                _frmFeatureIdentifier.Show(Map.MapFrame?.Parent);
            }

            base.OnMouseUp(e);
        }

        private void Identify(IEnumerable<ILayer> layers, Extent strict, Extent tolerant)
        {
            if (layers is IGroup) layers = layers.Reverse();
            foreach (var lr in layers)
            {
                if (lr is IGroup grp)
                {
                    Identify(grp, strict, tolerant);
                }
                else
                {
                    if (lr is IMapFeatureLayer gfl && gfl.IsVisible)
                    {
                        _frmFeatureIdentifier.Add(gfl, gfl.DataSet.FeatureType == FeatureType.Polygon ? strict : tolerant);
                        continue;
                    }

                    if (lr is IMapRasterLayer rl)
                    {
                        _frmFeatureIdentifier.Add(rl, strict);
                    }
                }
            }
        }

        /// <summary>
        /// Highlights the feature that is selected in the indentify windows treeview.
        /// </summary>
        /// <param name="map">The map used to clear the selection.</param>
        private void SetSelectToSelectedNode(IMap map)
        {
            map.ClearSelection();

            var selectedNode = _frmFeatureIdentifier.treFeatures.SelectedNode;
            if (selectedNode?.Parent == null) return;
            if (selectedNode.Tag is IFeature feature && selectedNode.Parent.Tag is IFeatureLayer layer && layer.IsVisible)
            {
                layer.Select(feature);
            }
        }

        #endregion
    }
}