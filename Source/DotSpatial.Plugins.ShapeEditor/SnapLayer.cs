// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Symbology;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// Class to control layers snappable property through the datagridview.
    /// </summary>
    public class SnapLayer
    {
        #region Fields

        /// <summary>
        /// Layer, whose snappable property can be changed.
        /// </summary>
        private readonly IFeatureLayer _layer;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnapLayer"/> class.
        /// </summary>
        /// <param name="pLayer">Layer, for which the SnapLayer-object gets created.</param>
        public SnapLayer(IFeatureLayer pLayer)
        {
            _layer = pLayer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the layer name so it can be shown in the DataGridView.
        /// </summary>
        public string LayerName => _layer.LegendText;

        /// <summary>
        /// Gets or sets a value indicating whether snapping to the coordinates of the layers features is allowed.
        /// </summary>
        public bool Snappable
        {
            get
            {
                return _layer.Snappable;
            }

            set
            {
                _layer.Snappable = value;
            }
        }

        #endregion
    }
}