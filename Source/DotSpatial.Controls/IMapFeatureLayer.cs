// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is should not be instantiated because it cannot in itself perform the necessary functions.
    /// Instead, most of the specified functionality must be implemented in the more specific classes.
    /// This is also why there is no direct constructor for this class. You can use the static
    /// "FromFile" or "FromFeatureLayer" to create FeatureLayers from a file.
    /// </summary>
    public interface IMapFeatureLayer : IMapLayer, IFeatureLayer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the label layer that is associated with this feature layer.
        /// </summary>
        new IMapLabelLayer LabelLayer { get; set; }

        #endregion
    }
}