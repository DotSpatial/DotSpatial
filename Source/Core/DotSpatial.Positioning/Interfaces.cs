// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotSpatial.Positioning
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
        /// <summary>
        /// The Fix Method
        /// </summary>
        FixMethod FixMethod { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains GPS satellite information.
    /// </summary>
    public interface ISatelliteCollectionSentence
    {
        /// <summary>
        /// The Satellites
        /// </summary>
        IList<Satellite> Satellites { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which describes how the fix is being obtained.
    /// </summary>
    public interface IFixQualitySentence
    {
        /// <summary>
        /// The Fix Quality
        /// </summary>
        FixQuality FixQuality { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which returns the number of GPS satellites involved in the current fix.
    /// </summary>
    public interface IFixedSatelliteCountSentence
    {
        /// <summary>
        /// The Fixed Satellite Count
        /// </summary>
        int FixedSatelliteCount { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains a list of fixed GPS satellites.
    /// </summary>
    public interface IFixedSatellitesSentence
    {
        /// <summary>
        /// the list of FixedSatellites
        /// </summary>
        IList<Satellite> FixedSatellites { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains the direction of travel.
    /// </summary>
    public interface IBearingSentence
    {
        /// <summary>
        /// the Bearing
        /// </summary>
        Azimuth Bearing { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains the direction of heading.
    /// </summary>
    public interface IHeadingSentence
    {
        /// <summary>
        /// the Heading
        /// </summary>
        Azimuth Heading { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains whether a fix is currently acquired.
    /// </summary>
    public interface IFixStatusSentence
    {
        /// <summary>
        /// The Fix Status
        /// </summary>
        FixStatus FixStatus { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains
    /// </summary>
    public interface ISpeedSentence
    {
        /// <summary>
        /// The Speed
        /// </summary>
        Speed Speed { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains
    /// </summary>
    public interface IMagneticVariationSentence
    {
        /// <summary>
        /// The Magnetic Variation
        /// </summary>
        Longitude MagneticVariation { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains
    /// </summary>
    public interface IAltitudeSentence
    {
        /// <summary>
        /// The Altitude
        /// </summary>
        Distance Altitude { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains
    /// </summary>
    public interface IAltitudeAboveEllipsoidSentence
    {
        /// <summary>
        /// The Altitude Above Ellipsoid
        /// </summary>
        Distance AltitudeAboveEllipsoid { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains
    /// </summary>
    public interface IGeoidalSeparationSentence
    {
        /// <summary>
        /// The Geoidal Separation
        /// </summary>
        Distance GeoidalSeparation { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains
    /// </summary>
    public interface IHorizontalDilutionOfPrecisionSentence
    {
        /// <summary>
        /// The Horizontal Dilution of Precision
        /// </summary>
        DilutionOfPrecision HorizontalDilutionOfPrecision { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains
    /// </summary>
    public interface IVerticalDilutionOfPrecisionSentence
    {
        /// <summary>
        /// The Vertical Dilution of Precision
        /// </summary>
        DilutionOfPrecision VerticalDilutionOfPrecision { get; }
    }

    /// <summary>
    /// Represents an NMEA sentence which contains
    /// </summary>
    public interface IPositionDilutionOfPrecisionSentence
    {
        /// <summary>
        /// The Position Dilution of Precision (PDOP)
        /// </summary>
        DilutionOfPrecision PositionDilutionOfPrecision { get; }
    }
}