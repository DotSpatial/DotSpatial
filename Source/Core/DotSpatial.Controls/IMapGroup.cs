// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Interface for the map group.
    /// </summary>
    public interface IMapGroup : IGroup, IMapLayer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the IMapLayerCollection for members contained by this group.
        /// </summary>
        IMapLayerCollection Layers { get; set; }

        /// <summary>
        /// Gets the map frame for this group.
        /// </summary>
        IMapFrame ParentMapFrame { get; }

        #endregion
    }
}