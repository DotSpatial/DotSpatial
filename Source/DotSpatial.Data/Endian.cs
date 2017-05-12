// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in 10/10/2010 2:26 PM.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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