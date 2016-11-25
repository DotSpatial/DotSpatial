// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 8:21:20 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Data types specific to grids
    /// </summary>
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
        /// Byte
        /// </summary>
        BYTE = 5,

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