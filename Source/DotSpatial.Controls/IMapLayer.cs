// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Layer with capability to draw itself on map.
    /// </summary>
    public interface IMapLayer : ILayer
    {
        #region Methods

        /// <summary>
        /// This draws content from the specified geographic regions onto the specified graphics
        /// object specified by MapArgs.
        /// </summary>
        /// <param name="args">The map args.</param>
        /// <param name="regions">The regions.</param>
        /// <param name="selected">Indicates whether to draw the normal colored features or the selection colored features.</param>
        void DrawRegions(MapArgs args, List<Extent> regions, bool selected);

        #endregion
    }
}