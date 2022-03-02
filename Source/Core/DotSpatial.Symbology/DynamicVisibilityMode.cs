// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Represents mode whether the layer is visible when zoomed in closer than the dynamic
    /// visibility width or only when further away from the dynamic visibility width.
    /// </summary>
    public enum DynamicVisibilityMode
    {
        /// <summary>
        /// The layer will only be visible when zoomed in closer than the
        /// DynamicVisibilityWidth.
        /// </summary>
        ZoomedIn,

        /// <summary>
        /// The layer will only be visible when zoomed out beyond the
        /// DynamicVisibilityWidth.
        /// </summary>
        ZoomedOut
    }
}