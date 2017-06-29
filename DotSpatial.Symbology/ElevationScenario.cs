// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. 2/17/2008 5:00:07 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Some of the more common relationships between elevation and geographic coordinates
    /// </summary>
    public enum ElevationScenario
    {
        /// <summary>
        /// The elevation values are in centimeters, but the geographic projection uses decimal degrees
        /// </summary>
        ElevationCentiMeters_ProjectionDegrees,
        /// <summary>
        /// The elevation values are in centimeters, but the geographic projection uses Meters
        /// </summary>
        ElevationCentiMeters_ProjectionMeters,
        /// <summary>
        /// The elevation values are in centimeters, but the geographic projection uses Feet
        /// </summary>
        ElevationCentiMeters_ProjectionFeet,
        /// <summary>
        /// The elevation values are in feet, but the geographic projection uses decimal degrees
        /// </summary>
        ElevationFeet_ProjectionDegrees,
        /// <summary>
        /// The elevation values are in feet, but the geographic projection uses meters
        /// </summary>
        ElevationFeet_ProjectionMeters,
        /// <summary>
        /// The elevation values are in feet, but the geographic projection uses feet
        /// </summary>
        ElevationFeet_ProjectionFeet,
        /// <summary>
        /// The elevation values are in meters, but the geographic projection uses decimal degrees
        /// </summary>
        ElevationMeters_ProjectionDegrees,
        /// <summary>
        /// The elevation values are in meters, but the geographic projection uses meters
        /// </summary>
        ElevationMeters_ProjectionMeters,
        /// <summary>
        /// The elevation values are in meters, but the geographic projection uses feet
        /// </summary>
        ElevationMeters_ProjectionFeet
    }
}