// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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