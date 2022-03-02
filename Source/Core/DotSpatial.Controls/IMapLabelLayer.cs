// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Interface for the map label layer.
    /// </summary>
    public interface IMapLabelLayer : ILabelLayer, IMapLayer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the feature layer that this label layer is attached to.
        /// </summary>
        new IMapFeatureLayer FeatureLayer { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Resolves ambiguity.
        /// </summary>
        new void Invalidate();

        #endregion
    }
}