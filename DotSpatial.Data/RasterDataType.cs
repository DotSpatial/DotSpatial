// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 8:21:20 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Data types specific to grids
    /// </summary>
    // todo: Not sure that we need UNKNOWN and CUSTOM cases. For now i've marked them as Deprecated.
    public enum RasterDataType
    {
        /// <summary>
        /// An invalid data type
        /// </summary>
        INVALID = -1,

        /// <summary>
        /// Short 16 Bit integers
        /// </summary>
        SHORT = 0,

        /// <summary>
        /// 32 Bit Integers (old style long)
        /// </summary>
        INTEGER = 1,

        /// <summary>
        /// Float or Single
        /// </summary>
        SINGLE = 2,

        /// <summary>
        /// Double
        /// </summary>
        DOUBLE = 3,

        /// <summary>
        /// Unknown
        /// </summary>
        [Obsolete("Use INVALID case instead")] // Marked in 1.7
        UNKNOWN = 4,

        /// <summary>
        /// Byte
        /// </summary>
        BYTE = 5,

        /// <summary>
        /// Specified as the CustomType string
        /// </summary>
        [Obsolete("Do not use it. There is no support for this case anywhere in code.")] // Marked in 1.7
        CUSTOM = 6,

        /// <summary>
        /// Signed 64 Bit Integers
        /// </summary>
        LONG = 7,

        /// <summary>
        /// Unsigned short 16 Bit Integers
        /// </summary>
        USHORT = 8,

        /// <summary>
        /// Unsigned 32 Bit Integers
        /// </summary>
        UINTEGER = 9,

        /// <summary>
        /// Unsigned 64 Bit Integers
        /// </summary>
        ULONG = 10,

        /// <summary>
        /// Signed 8-bit Integers: -128 to 127
        /// </summary>
        SBYTE = 11,

        /// <summary>
        /// Booleans: True(1) or False(0).
        /// </summary>
        BOOL = 12,
    }
}