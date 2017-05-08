// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. 2/17/2008 5:00:07 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Some of the more common relationships between elevation and geographic coordinates.
    /// </summary>
    public enum ElevationScenario
    {
        /// <summary>
        /// The elevation values are in centimeters, but the geographic projection uses decimal degrees
        /// </summary>
        ElevationCentimetersProjectionDegrees,

        /// <summary>
        /// The elevation values are in centimeters, but the geographic projection uses Meters
        /// </summary>
        ElevationCentimetersProjectionMeters,

        /// <summary>
        /// The elevation values are in centimeters, but the geographic projection uses Feet
        /// </summary>
        ElevationCentimetersProjectionFeet,

        /// <summary>
        /// The elevation values are in feet, but the geographic projection uses decimal degrees
        /// </summary>
        ElevationFeetProjectionDegrees,

        /// <summary>
        /// The elevation values are in feet, but the geographic projection uses meters
        /// </summary>
        ElevationFeetProjectionMeters,

        /// <summary>
        /// The elevation values are in feet, but the geographic projection uses feet
        /// </summary>
        ElevationFeetProjectionFeet,

        /// <summary>
        /// The elevation values are in meters, but the geographic projection uses decimal degrees
        /// </summary>
        ElevationMetersProjectionDegrees,

        /// <summary>
        /// The elevation values are in meters, but the geographic projection uses meters
        /// </summary>
        ElevationMetersProjectionMeters,

        /// <summary>
        /// The elevation values are in meters, but the geographic projection uses feet
        /// </summary>
        ElevationMetersProjectionFeet
    }
}