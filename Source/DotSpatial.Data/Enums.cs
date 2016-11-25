// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Clarifies whether a value is cached in a local variable or updated dynamically
    /// </summary>
    public enum CacheTypes
    {
        /// <summary>
        /// The value is cached locally, rather than calculated on the fly
        /// </summary>
        Cached,

        /// <summary>
        /// The value is calculated each type, rather than using a local cache
        /// </summary>
        Dynamic
    }

    /// <summary>
    /// Byte order
    /// </summary>
    public enum ByteOrder
    {
        /// <summary>
        /// Big Endian
        /// </summary>
        BigEndian = 0x00,

        /// <summary>
        /// Little Endian
        /// </summary>
        LittleEndian = 0x01,
    }
}