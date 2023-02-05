// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Interface for the map point layer.
    /// </summary>
    public interface IMapPointLayer : IPointLayer, IMapFeatureLayer
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to prevent collision.
        /// The point layer in the map will only draw points that are not in the space which have been drawn by other points.
        /// This should increase drawing speed for layers that have a large number of points.
        /// </summary>
        bool PreventCollisions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the width of collision space. The default is 1.
        /// This is useful if we have only enabled PreventCollisions.
        /// </summary>
        int CollisionWidth { get; set; }

        #endregion
    }
}