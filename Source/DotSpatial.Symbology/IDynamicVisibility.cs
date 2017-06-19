// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A shared interface for members that wish to set dynamic visibility.
    /// </summary>
    public interface IDynamicVisibility
    {
        /// <summary>
        /// Gets or sets the dynamic visibility width. The dynamic visibility represents layers that only appear when you zoom in close enough.
        /// This value represents the geographic width where that happens.
        /// </summary>
        double DynamicVisibilityWidth { get; set; }

        /// <summary>
        /// Gets or sets t he dynamic visibility mode. This controls whether the layer is visible when zoomed in closer than the dynamic
        /// visiblity width or only when further away from the dynamic visibility width
        /// </summary>
        DynamicVisibilityMode DynamicVisibilityMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether dynamic visibility should be enabled.
        /// </summary>
        bool UseDynamicVisibility { get; set; }
    }
}