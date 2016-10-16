// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created Before 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |   Ted Dunsford       |  6/30/2010  |  Moved to DotSpatial
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// Clarifies how the file should be used
    /// </summary>
    public enum RasterFileMode
    {
        /// <summary>
        /// Read will attempt to read the file if it exists and throw an exception if the file is not found
        /// </summary>
        Read,
        /// <summary>
        /// Write will create a new file, overwriting it if it previously exists.
        /// </summary>
        Write
    }
}