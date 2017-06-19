// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// An enumeration for specifying the endian byte order to use.
    /// </summary>
    public enum Endian
    {
        /// <summary>
        /// Specifies big endian like mainframe or unix system.
        /// </summary>
        BigEndian,

        /// <summary>
        /// Specifies little endian like most pc systems.
        /// </summary>
        LittleEndian,
    }
}