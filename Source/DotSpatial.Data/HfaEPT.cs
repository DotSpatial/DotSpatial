// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// EPT ERDAS DATA TYPES.
    /// </summary>
    public enum HfaEpt
    {
        /// <summary>
        /// Unsigned 1 bit
        /// </summary>
        U1 = 0,

        /// <summary>
        /// Unsigned 2 bit
        /// </summary>
        U2 = 1,

        /// <summary>
        /// Unsigned 4 bit
        /// </summary>
        U4 = 2,

        /// <summary>
        /// Unsigned 8 bit
        /// </summary>
        U8 = 3,

        /// <summary>
        /// Signed 8 bit
        /// </summary>
        S8 = 4,

        /// <summary>
        /// Unsigned 16 bit
        /// </summary>
        U16 = 5,

        /// <summary>
        /// Signed 16 bit
        /// </summary>
        S16 = 6,

        /// <summary>
        /// Unsigned 32 bit
        /// </summary>
        U32 = 7,

        /// <summary>
        /// Signed 32 bit
        /// </summary>
        S32 = 8,

        /// <summary>
        /// Single precisions 32 bit floating point
        /// </summary>
        Single = 9,

        /// <summary>
        /// Double precision 64 bit floating point
        /// </summary>
        Double = 10,

        /// <summary>
        /// 64 bit character?
        /// </summary>
        Char64 = 11,

        /// <summary>
        /// 128 bit character?
        /// </summary>
        Char128 = 12
    }
}