// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// Supports bounding in the M Dimension.
    /// </summary>
    public interface IExtentM
    {
        /// <summary>
        /// Gets or sets the minimum M value.
        /// </summary>
        double MinM { get; set; }

        /// <summary>
        /// Gets or sets the maximum M value.
        /// </summary>
        double MaxM { get; set; }
    }
}