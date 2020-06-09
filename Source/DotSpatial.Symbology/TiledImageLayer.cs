// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// TiledImageLayer.
    /// </summary>
    public class TiledImageLayer : Layer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TiledImageLayer"/> class.
        /// </summary>
        /// <param name="dataset">TiledImage the layer is based on.</param>
        public TiledImageLayer(ITiledImage dataset)
        {
            DataSet = dataset;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tiled image dataset.
        /// </summary>
        public new ITiledImage DataSet { get; set; }

        /// <summary>
        /// Gets the bounding envelope of the image layer.
        /// </summary>
        public override Extent Extent => DataSet.Extent;

        #endregion
    }
}