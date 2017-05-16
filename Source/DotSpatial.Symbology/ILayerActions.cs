// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This interface provides ability to use in Layer any custom actions (including GUI-dependent dialogs)
    /// </summary>
    public interface ILayerActions
    {
        /// <summary>
        /// Show Dynamic Visibility dialog
        /// </summary>
        /// <param name="e">Dynamic Visibility</param>
        /// <param name="mapFrame">The map frame.</param>
        void DynamicVisibility(IDynamicVisibility e, IFrame mapFrame);
    }
}