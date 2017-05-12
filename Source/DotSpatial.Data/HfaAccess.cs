// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 3:17:27 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaAccess
    /// </summary>
    public enum HfaAccess
    {
        /// <summary>
        /// Read Only with no update access
        /// </summary>
        ReadOnly = 0,

        /// <summary>
        /// Read/Write access
        /// </summary>
        Update = 1,
    }
}