// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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