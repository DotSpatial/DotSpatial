// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// An enumeration listing the types of image band interpretations supported.
    /// </summary>
    public enum ImageBandType
    {
        /// <summary>
        /// Alpha, Red, Green, Blue 4 bytes per pixel, 4 bands
        /// </summary>
        ARGB,

        /// <summary>
        ///  Red, Green, Blue 3 bytes per pixel, 3 bands
        /// </summary>
        RGB,

        /// <summary>
        /// Gray as one byte per pixel, 1 band
        /// </summary>
        Gray,

        /// <summary>
        /// Colors encoded 1 byte per pixel, 1 band
        /// </summary>
        PalletCoded
    }
}