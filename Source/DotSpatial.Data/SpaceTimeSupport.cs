// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 11:55:36 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    public enum SpaceTimeSupport
    {
        /// <summary>
        /// Spatial (X, Y, Z or M information only)
        /// </summary>
        Spatial,
        /// <summary>
        /// Temporal (time information only)
        /// </summary>
        Temporal,
        /// <summary>
        /// SpatioTemporal (time and space information)
        /// </summary>
        SpatioTemporal,
        /// <summary>
        /// Other (no temporal or spatial information)
        /// </summary>
        Other
    }
}