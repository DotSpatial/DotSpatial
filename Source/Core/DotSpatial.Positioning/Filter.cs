// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Kalman filter for improved positioning
    /// </summary>
    public abstract class PrecisionFilter
    {
        #region Static Members

        /// <summary>
        ///
        /// </summary>
        /// <summary>
        /// The default Kalman filter
        /// </summary>
        public static KalmanFilter Default { get; } = new();

        #endregion

        /// <summary>
        /// The observed position
        /// </summary>
        public Position ObservedPosition => new(ObservedLocation.Latitude, ObservedLocation.Longitude);

        /// <summary>
        /// The filtered position
        /// </summary>
        public Position FilteredPosition => new(FilteredLocation.Latitude, FilteredLocation.Longitude);

        /// <summary>
        /// The observed location
        /// </summary>
        public abstract Position3D ObservedLocation { get; }

        /// <summary>
        /// The filtered location
        /// </summary>
        public abstract Position3D FilteredLocation { get; }

        /// <summary>
        /// Is the precision filter enabled
        /// </summary>
        public abstract bool IsInitialized { get; }

        /// <summary>
        /// Gets the delay
        /// </summary>
        public abstract TimeSpan Delay { get; }

        /// <summary>
        /// Initialise the filter from a specified Position
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        public abstract void Initialize(Position gpsPosition);

        /// <summary>
        /// Initialise the filter from a specified Position3D
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        public abstract void Initialize(Position3D gpsPosition);

        /// <summary>
        /// Filter the Position
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        /// <returns></returns>
        public abstract Position Filter(Position gpsPosition);

        /// <summary>
        /// Return filtered position from specified parameters
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        /// <param name="deviceError">The device error.</param>
        /// <param name="horizontalDOP">The horizontal DOP.</param>
        /// <param name="verticalDOP">The vertical DOP.</param>
        /// <param name="bearing">The bearing.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public abstract Position Filter(Position gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed);

        /// <summary>
        /// Return a filtered Position3d
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        /// <returns></returns>
        public abstract Position3D Filter(Position3D gpsPosition);

        /// <summary>
        /// Return a filtered Position3D from the specified parameters
        /// </summary>
        /// <param name="gpsPosition">The GPS position.</param>
        /// <param name="deviceError">The device error.</param>
        /// <param name="horizontalDOP">The horizontal DOP.</param>
        /// <param name="verticalDOP">The vertical DOP.</param>
        /// <param name="bearing">The bearing.</param>
        /// <param name="speed">The speed.</param>
        /// <returns></returns>
        public abstract Position3D Filter(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed);
    }
}