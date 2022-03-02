using System;

namespace DotSpatial.Positioning.Gps.Filters
{
    /// <summary>
    /// Kalman filter for improved positioning
    /// </summary>
    public abstract class PrecisionFilter
    {
        #region Static Members

        private static KalmanFilter _defaultFilter = new KalmanFilter();

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
        public Position FilteredPosition { get { return new Position(FilteredLocation.Latitude, FilteredLocation.Longitude); ; } }

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
        /// <param name="gpsPosition"></param>
        public abstract void Initialize(Position gpsPosition);

        /// <summary>
        /// Initialise the filter from a specified Position3D
        /// </summary>
        /// <param name="gpsPosition"></param>
        public abstract void Initialize(Position3D gpsPosition);

        /// <summary>
        /// Filter the Position
        /// </summary>
        /// <param name="gpsPosition"></param>
        /// <returns></returns>
        public abstract Position Filter(Position gpsPosition);

        /// <summary>
        /// Return filtered position from specified parameters
        /// </summary>
        /// <param name="gpsPosition"></param>
        /// <param name="deviceError"></param>
        /// <param name="horizontalDOP"></param>
        /// <param name="verticalDOP"></param>
        /// <param name="bearing"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public abstract Position Filter(Position gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed);

        /// <summary>
        /// Return a filtered Position3d 
        /// </summary>
        /// <param name="gpsPosition"></param>
        /// <returns></returns>
        public abstract Position3D Filter(Position3D gpsPosition);

        /// <summary>
        /// Return a filtered Position3D from the specified parameters
        /// </summary>
        /// <param name="gpsPosition"></param>
        /// <param name="deviceError"></param>
        /// <param name="horizontalDOP"></param>
        /// <param name="verticalDOP"></param>
        /// <param name="bearing"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public abstract Position3D Filter(Position3D gpsPosition, Distance deviceError, DilutionOfPrecision horizontalDOP, DilutionOfPrecision verticalDOP, Azimuth bearing, Speed speed);
    }
}
