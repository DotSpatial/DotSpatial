// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// ColorType.
    /// </summary>
    public enum ColorType : byte
    {
        /// <summary>
        /// Each pixel is a greyscale sample
        /// </summary>
        Greyscale = 0,

        /// <summary>
        /// Each pixel is an RGB triple
        /// </summary>
        Truecolor = 2,

        /// <summary>
        /// Each pixel is a palette index
        /// </summary>
        Indexed = 3,

        /// <summary>
        /// Each pixel is a greyscale sample followed by an alpha sample
        /// </summary>
        GreyscaleAlpha = 4,

        /// <summary>
        /// EAch pixel is an RGB triple followed by an alhpa sample
        /// </summary>
        TruecolorAlpha = 6
    }
}