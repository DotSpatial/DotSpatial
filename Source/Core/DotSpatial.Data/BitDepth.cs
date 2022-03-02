// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// BitDepth.
    /// </summary>
    public enum BitDepth : byte
    {
        /// <summary>
        /// One bit per band pixel
        /// </summary>
        One = 1,

        /// <summary>
        /// Two bits per band pixel
        /// </summary>
        Two = 2,

        /// <summary>
        /// Four bits per band pixel
        /// </summary>
        Four = 4,

        /// <summary>
        /// Eight bits per band pixel (normal)
        /// </summary>
        Eight = 8,

        /// <summary>
        /// Sixteen bits per band pixel
        /// </summary>
        Sixteen = 16
    }
}