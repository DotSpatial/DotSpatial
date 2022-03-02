// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An EventArgs class for passing around an extent.
    /// </summary>
    public class ExtentArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentArgs"/> class.
        /// </summary>
        /// <param name="value">The value for this event.</param>
        public ExtentArgs(Extent value)
        {
            Extent = value;
        }

        /// <summary>
        /// Gets or sets the Extents for this event.
        /// </summary>
        public Extent Extent { get; protected set; }
    }
}