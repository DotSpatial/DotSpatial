using System;
using System.Collections.Generic;

namespace DotSpatial.Positioning.Gps.Nmea
{
    /// <summary>
    /// Represents an NMEA sentence which contains latitude and longitude values.
    /// </summary>
    public interface IPositionSentence
    {
        /// <summary>
        /// Represents an NMEA sentence which contains a position.
        /// </summary>
        Position Position { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains date and time in UTC.
    /// </summary>
    public interface IUtcDateTimeSentence
    {
        /// <summary>
        /// Represents an NMEA sentence which contains date and time in UTC.
        /// </summary>
        DateTime UtcDateTime { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains time in UTC.
    /// </summary>
    public interface IUtcTimeSentence
    {
        /// <summary>
        /// Gets the time in UTC from the IUtcTimeSentence
        /// </summary>
        TimeSpan UtcTime { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains differential GPS information.
    /// </summary>
    public interface IDifferentialGpsSentence
    {
        /// <summary>
        /// Gets the Differential Gps Station ID
        /// </summary>
        int DifferentialGpsStationID { get; }
        /// <summary>
        /// Gets the age of the Differential Gps
        /// </summary>
        TimeSpan DifferentialGpsAge { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which describes the current fix.
    /// </summary>
    public interface IFixModeSentence
    {
        /// <summary>
        /// Gets the fix mode
        /// </summary>
        FixMode FixMode { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains the method used to acquire a fix.
    /// </summary>
    public interface IFixMethodSentence
    {
        FixMethod FixMethod { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains GPS satellite information.
    /// </summary>
    public interface ISatelliteCollectionSentence
    {
        IList<Satellite> Satellites { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which describes how the fix is being obtained.
    /// </summary>
    public interface IFixQualitySentence
    {
        FixQuality FixQuality { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which returns the number of GPS satellites involved in the current fix.
    /// </summary>
    public interface IFixedSatelliteCountSentence
    {
        int FixedSatelliteCount { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains a list of fixed GPS satellites.
    /// </summary>
    public interface IFixedSatellitesSentence
    {
        IList<Satellite> FixedSatellites { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains the direction of travel.
    /// </summary>
    public interface IBearingSentence
    {
        Azimuth Bearing { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains whether a fix is currently acquired.
    /// </summary>
    public interface IFixStatusSentence
    {
        FixStatus FixStatus { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains 
    /// </summary>
    public interface ISpeedSentence
    {
        Speed Speed { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains 
    /// </summary>
    public interface IMagneticVariationSentence
    {
        Longitude MagneticVariation { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains 
    /// </summary>
    public interface IAltitudeSentence
    {
        Distance Altitude { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains 
    /// </summary>
    public interface IAltitudeAboveEllipsoidSentence
    {
        Distance AltitudeAboveEllipsoid { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains 
    /// </summary>
    public interface IGeoidalSeparationSentence
    {
        Distance GeoidalSeparation { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains 
    /// </summary>
    public interface IHorizontalDilutionOfPrecisionSentence
    {
        DilutionOfPrecision HorizontalDilutionOfPrecision { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains 
    /// </summary>
    public interface IVerticalDilutionOfPrecisionSentence
    {
        DilutionOfPrecision VerticalDilutionOfPrecision { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains 
    /// </summary>
    public interface IPositionDilutionOfPrecisionSentence
    {
        DilutionOfPrecision PositionDilutionOfPrecision { get; }
    }
}
