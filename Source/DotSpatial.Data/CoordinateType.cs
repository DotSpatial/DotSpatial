// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/11/2009 11:26:24 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    public enum CoordinateType
    {
        /// <summary>
        /// X and Y coordinates only
        /// </summary>
        Regular,
        /// <summary>
        /// M values are available
        /// </summary>
        M,
        /// <summary>
        /// Z values are available
        /// </summary>
        Z,
    }
}