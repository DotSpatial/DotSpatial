// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Controls
{
    /// <summary>
    /// Describe different behaviors that map functions can have when working with
    /// respect to other map-functions.
    /// </summary>
    public enum ActionMode
    {
        /// <summary>
        /// Prompt the user to decide if Layers should be reprojected
        /// </summary>
        Prompt,

        /// <summary>
        /// Always reproject layers to match the MapFrame projection
        /// </summary>
        Always,

        /// <summary>
        /// Never reproject layers to match the MapFrame projection
        /// </summary>
        Never,

        /// <summary>
        /// Prompt once and accept that answer for all future layers added to the map frame.
        /// </summary>
        PromptOnce,
    }
}