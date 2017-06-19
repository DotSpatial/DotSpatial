// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Location of a click event within the legend.
    /// </summary>
    public enum ClickLocation
    {
        /// <summary>The user clicked outside of any group or layer.</summary>
        None = 0,

        /// <summary>The user clicked on a layer.</summary>
        Layer = 1,

        /// <summary>The user clicked on a group.</summary>
        Group = 2
    }
}