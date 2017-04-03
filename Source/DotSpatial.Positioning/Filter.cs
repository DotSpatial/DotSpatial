// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************

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
        private static readonly KalmanFilter _defaultFilter = new KalmanFilter();

        /// <summary>
        /// The default Kalman filter
        /// </summary>
        public static KalmanFilter Default
        {
            get { return _defaultFilter; }
        }

        #endregion

        /// <summary>
        /// The obsedrved position
        /// </summary>
        public Position ObservedPosition { get { return new Position(ObservedLocation.Latitude, ObservedLocation.Longitude); } }

        /// <summary>
        /// The filtered position
        /// </summary>
        public Position FilteredPosition { get { return new Position(FilteredLocation.Latitude, FilteredLocation.Longitude); } }

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