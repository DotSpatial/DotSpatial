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
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// $GPGGA, hhmmss.ss, ddmm.mmmm, n, dddmm.mmmm, e, q, ss, y.y, a.a, z, g.g, z, t.t, iii*CC
    /// http://aprs.gids.nl/nmea/#gga
    /// Global Positioning System Fix Data. Time, location and fix related data for a GPS receiver.
    /// eg2. $--GGA, hhmmss.ss, llll.ll, a, yyyyy.yy, a, x, xx, x.x, x.x, M, x.x, M, x.x, xxxx
    /// hhmmss.ss = UTC of location
    /// llll.ll = latitude of location
    /// a = N or S
    /// yyyyy.yy = Longitude of location
    /// a = E or W
    /// x = GPS Quality indicator (0=no fix, 1=GPS fix, 2=Dif. GPS fix)
    /// xx = number of satellites in use
    /// x.x = horizontal dilution of precision
    /// x.x = Antenna altitude above mean-sea-level
    /// M = units of antenna altitude, meters
    /// x.x = Geoidal separation
    /// M = units of geoidal separation, meters
    /// x.x = Age of Differential GPS data (seconds)
    /// xxxx = Differential reference station ID
    /// eg3. $GPGGA, hhmmss.ss, hhmm.mm, i, hhhmm.mm, i, f, ss, x.x, x.x, M, x.x, M, x.x, xxxx*hh
    /// 0    = Time in UTC
    /// 1    = Latitude
    /// 2    = N or S
    /// 3    = Longitude
    /// 4    = E or W
    /// 5    = GPS quality indicator (0=invalid; 1=GPS fix; 2=Diff. GPS fix)
    /// 6    = Number of satellites in use [not those in view]
    /// 7    = Horizontal dilution of precision
    /// 8    = Antenna altitude above/below mean sea level (geoid)
    /// 9    = Meters  (Antenna height unit)
    /// 10   = Geoidal separation (Diff. between WGS-84 earth ellipsoid and
    /// mean sea level.  -=geoid is below WGS-84 ellipsoid)
    /// 11   = Meters  (Units of geoidal separation)
    /// 12   = Age in seconds since last update from diff. reference station
    /// 13   = Diff. reference station ID#
    /// 14   = Checksum
    /// </summary>
    public sealed class GpggaSentence : NmeaSentence, IPositionSentence, IUtcTimeSentence, IAltitudeSentence,
                                    IGeoidalSeparationSentence, IDifferentialGpsSentence, IFixQualitySentence,
                                    IHorizontalDilutionOfPrecisionSentence, IFixedSatelliteCountSentence
    {
        /// <summary>
        ///
        /// </summary>
        private Position _position;
        /// <summary>
        ///
        /// </summary>
        private TimeSpan _utcTime;
        /// <summary>
        ///
        /// </summary>
        private FixQuality _fixQuality;
        /// <summary>
        ///
        /// </summary>
        private int _fixedSatelliteCount = -1;
        /// <summary>
        ///
        /// </summary>
        private Distance _altitude;
        /// <summary>
        ///
        /// </summary>
        private Distance _geoidalSeparation;
        /// <summary>
        ///
        /// </summary>
        private DilutionOfPrecision _horizontalDilutionOfPrecision;
        /// <summary>
        ///
        /// </summary>
        private int _differentialGpsStationID;
        /// <summary>
        ///
        /// </summary>
        private TimeSpan _differentialGpsAge;

        #region Constructors

        /// <summary>
        /// Creates a new GpggaSentence
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GpggaSentence(string sentence)
            : base(sentence)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpggaSentence"/> class.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="commandWord">The command word.</param>
        /// <param name="words">The words.</param>
        /// <param name="validChecksum">The valid checksum.</param>
        internal GpggaSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        /// <summary>
        /// Creates a new sentence
        /// </summary>
        /// <param name="utcTime">The UTC time.</param>
        /// <param name="position">The position.</param>
        /// <param name="fixQuality">The fix quality.</param>
        /// <param name="trackedSatelliteCount">The tracked satellite count.</param>
        /// <param name="horizontalDilutionOfPrecision">The horizontal dilution of precision.</param>
        /// <param name="altitude">The altitude.</param>
        /// <param name="geoidalSeparation">The geoidal separation.</param>
        /// <param name="differentialGpsAge">The differential GPS age.</param>
        /// <param name="differentialGpsStationID">The differential GPS station ID.</param>
        public GpggaSentence(TimeSpan utcTime, Position position, FixQuality fixQuality, int trackedSatelliteCount,
                    DilutionOfPrecision horizontalDilutionOfPrecision, Distance altitude, Distance geoidalSeparation,
                    TimeSpan differentialGpsAge, int differentialGpsStationID)
        {
            // Use a string builder to create the sentence text
            StringBuilder builder = new StringBuilder(128);

            #region Append the command word

            // Append the command word
            builder.Append("$GPGGA");

            #endregion Append the command word

            // Append a comma
            builder.Append(',');

            #region Append the UTC time

            /* Convert UTC time to a string in the form HHMMSS.SSSS. Any value less than 10 will be
             * padded with a zero.
             */

            builder.Append(utcTime.Hours.ToString("0#", NmeaCultureInfo));
            builder.Append(utcTime.Minutes.ToString("0#", NmeaCultureInfo));
            builder.Append(utcTime.Seconds.ToString("0#", NmeaCultureInfo));
            builder.Append(".");
            builder.Append(utcTime.Milliseconds.ToString("00#", NmeaCultureInfo));

            #endregion Append the UTC time

            // Append a comma
            builder.Append(',');

            #region Append the position

            // Append latitude in the format HHMM.MMMM.
            builder.Append(position.Latitude.ToString(NmeaSentence.LatitudeFormat, NmeaCultureInfo));
            // Append Longitude in the format HHHMM.MMMM.
            builder.Append(position.Longitude.ToString(NmeaSentence.LongitudeFormat, NmeaCultureInfo));

            #endregion Append the position

            #region Append fix quality

            switch (fixQuality)
            {
                case FixQuality.NoFix:
                    builder.Append("0");
                    break;
                case FixQuality.GpsFix:
                    builder.Append("1");
                    break;
                case FixQuality.DifferentialGpsFix:
                    builder.Append("2");
                    break;
                case FixQuality.PulsePerSecond:
                    builder.Append("3");
                    break;
                case FixQuality.FixedRealTimeKinematic:
                    builder.Append("4");
                    break;
                case FixQuality.FloatRealTimeKinematic:
                    builder.Append("5");
                    break;
                case FixQuality.Estimated:
                    builder.Append("6");
                    break;
                case FixQuality.ManualInput:
                    builder.Append("7");
                    break;
                case FixQuality.Simulated:
                    builder.Append("8");
                    break;
            }

            #endregion Append fix quality

            // Append a comma
            builder.Append(",");

            // Append the tracked (signal strength is > 0) satellite count
            builder.Append(trackedSatelliteCount.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(",");

            // Append the numerical value of HDOP
            builder.Append(horizontalDilutionOfPrecision.Value.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(",");

            #region Altitude above sea level

            // Append the numerical value in meters
            builder.Append(altitude.ToMeters().Value.ToString(NmeaCultureInfo));

            // Append a comma, the unit (M = meters), and another comma
            builder.Append(", M,");

            #endregion Altitude above sea level

            #region Geoidal separation

            // Append the numerical value in meters
            builder.Append(geoidalSeparation.ToMeters().Value.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(", M,");

            #endregion Geoidal separation

            #region Differential GPS information

            // Differnetial signal age in seconds
            if (!differentialGpsAge.Equals(TimeSpan.MinValue))
                builder.Append(differentialGpsAge.TotalSeconds.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(",");

            // Station ID
            if (differentialGpsStationID != -1)
                builder.Append(differentialGpsStationID.ToString(NmeaCultureInfo));

            #endregion Differential GPS information

            // Set this object's sentence
            SetSentence(builder.ToString());

            // Finally, append the checksum
            AppendChecksum();
        }

        #endregion Constructors

        /// <summary>
        /// Called when [sentence changed].
        /// </summary>
        protected override void OnSentenceChanged()
        {
            // Parse the basic sentence information
            base.OnSentenceChanged();

            // Cache the words
            string[] words = Words;
            int wordCount = words.Length;

            // Do we have enough data to process the UTC time?
            if (wordCount >= 1 && words[0].Length != 0)
            {
                #region UTC Time

                string utcTimeWord = words[0];
                int utcHours = int.Parse(utcTimeWord.Substring(0, 2), NmeaCultureInfo);
                int utcMinutes = int.Parse(utcTimeWord.Substring(2, 2), NmeaCultureInfo);
                int utcSeconds = int.Parse(utcTimeWord.Substring(4, 2), NmeaCultureInfo);
                int utcMilliseconds = 0;
                if (utcTimeWord.Length > 6)
                    utcMilliseconds = Convert.ToInt32(float.Parse(utcTimeWord.Substring(6), NmeaCultureInfo) * 1000, NmeaCultureInfo);

                // Build a TimeSpan for this value
                _utcTime = new TimeSpan(0, utcHours, utcMinutes, utcSeconds, utcMilliseconds);

                #endregion UTC Time
            }
            else
            {
                // The UTC time is invalid
                _utcTime = TimeSpan.MinValue;
            }

            // Do we have enough data for locations?
            if (wordCount >= 5 && words[1].Length != 0 && words[2].Length != 0 && words[3].Length != 0 && words[4].Length != 0)
            {
                #region Latitude

                string latitudeWord = words[1];
                int latitudeHours = int.Parse(latitudeWord.Substring(0, 2), NmeaCultureInfo);
                double latitudeDecimalMinutes = double.Parse(latitudeWord.Substring(2), NmeaCultureInfo);
                LatitudeHemisphere latitudeHemisphere =
                    words[2].Equals("N", StringComparison.OrdinalIgnoreCase) ? LatitudeHemisphere.North : LatitudeHemisphere.South;

                #endregion Latitude

                #region Longitude

                string longitudeWord = words[3];
                int longitudeHours = int.Parse(longitudeWord.Substring(0, 3), NmeaCultureInfo);
                double longitudeDecimalMinutes = double.Parse(longitudeWord.Substring(3), NmeaCultureInfo);
                LongitudeHemisphere longitudeHemisphere =
                    words[4].Equals("E", StringComparison.OrdinalIgnoreCase) ? LongitudeHemisphere.East : LongitudeHemisphere.West;

                #endregion Longitude

                #region Position

                _position = new Position(
                                new Latitude(latitudeHours, latitudeDecimalMinutes, latitudeHemisphere),
                                new Longitude(longitudeHours, longitudeDecimalMinutes, longitudeHemisphere));

                #endregion Position
            }
            else
            {
                _position = Position.Invalid;
            }

            // Do we have enough data for fix quality?
            if (wordCount >= 6 && words[5].Length != 0)
            {
                #region Fix Quality

                switch (int.Parse(words[5], NmeaCultureInfo))
                {
                    case 0:
                        _fixQuality = FixQuality.NoFix;
                        break;
                    case 1:
                        _fixQuality = FixQuality.GpsFix;
                        break;
                    case 2:
                        _fixQuality = FixQuality.DifferentialGpsFix;
                        break;
                    case 3:
                        _fixQuality = FixQuality.PulsePerSecond;
                        break;
                    case 4:
                        _fixQuality = FixQuality.FixedRealTimeKinematic;
                        break;
                    case 5:
                        _fixQuality = FixQuality.FloatRealTimeKinematic;
                        break;
                    case 6:
                        _fixQuality = FixQuality.Estimated;
                        break;
                    case 7:
                        _fixQuality = FixQuality.ManualInput;
                        break;
                    case 8:
                        _fixQuality = FixQuality.Simulated;
                        break;
                    default:
                        _fixQuality = FixQuality.Unknown;
                        break;
                }

                #endregion Fix Quality
            }
            else
            {
                // This fix quality is invalid
                _fixQuality = FixQuality.Unknown;
            }

            // Number of satellites in view is skipped.  We'll work off of GPGSV data.
            if (wordCount >= 7 && words[6].Length != 0)
            {
                _fixedSatelliteCount = int.Parse(words[6], NmeaCultureInfo);
            }

            // Is there enough information to process horizontal dilution of precision?
            if (wordCount >= 8 && words[7].Length != 0)
            {
                #region Horizontal Dilution of Precision

                try
                {
                    _horizontalDilutionOfPrecision =
                        new DilutionOfPrecision(float.Parse(words[7], NmeaCultureInfo));
                }
                catch (ArgumentException)
                {
                    _horizontalDilutionOfPrecision = DilutionOfPrecision.Invalid;
                }

                #endregion Horizontal Dilution of Precision
            }
            else
            {
                // The HDOP is invalid
                _horizontalDilutionOfPrecision = DilutionOfPrecision.Invalid;
            }

            // Is there enough information to process altitude?
            if (wordCount >= 9 && words[8].Length != 0)
            {
                #region Altitude

                // Altitude is the 8th NMEA word
                _altitude = new Distance(float.Parse(words[8], NmeaCultureInfo), DistanceUnit.Meters);

                #endregion Altitude
            }
            else
            {
                // The altitude is invalid
                _altitude = Distance.Invalid;
            }

            // Is there enough information to process geoidal separation?
            if (wordCount >= 11 && words[10].Length != 0)
            {
                #region Geoidal Separation

                // Parse the geoidal separation
                _geoidalSeparation = new Distance(float.Parse(words[10], NmeaCultureInfo), DistanceUnit.Meters);

                #endregion Geoidal Separation
            }
            else
            {
                // The geoidal separation is invalid
                _geoidalSeparation = Distance.Invalid;
            }

            // Is there enough info to process Differential GPS info?
            if (wordCount >= 14 && words[12].Length != 0 && words[13].Length != 0)
            {
                #region Differential GPS information

                _differentialGpsAge = words[12].Length != 0 ? TimeSpan.FromSeconds(float.Parse(words[12], NmeaCultureInfo)) : TimeSpan.MinValue;

                if (words[13].Length != 0)
                    _differentialGpsStationID = int.Parse(words[13], NmeaCultureInfo);
                else
                    _differentialGpsStationID = -1;

                #endregion Differential GPS information
            }
            else
            {
                _differentialGpsStationID = -1;
                _differentialGpsAge = TimeSpan.MinValue;
            }
        }

        #region IPositionSentence Members

        /// <summary>
        /// Represents an NMEA sentence which contains a position.
        /// </summary>
        public Position Position
        {
            get { return _position; }
        }

        #endregion IPositionSentence Members

        #region IUtcDatetimeSentence Members

        /// <summary>
        /// Gets the time in UTC from the IUtcTimeSentence
        /// </summary>
        public TimeSpan UtcTime
        {
            get { return _utcTime; }
        }

        #endregion IUtcDatetimeSentence Members

        #region IAltitudeSentence Members

        /// <summary>
        /// The Altitude
        /// </summary>
        public Distance Altitude
        {
            get { return _altitude; }
        }

        #endregion IAltitudeSentence Members

        #region IHorizontalDilutionOfPrecisionSentence Members

        /// <summary>
        /// The Horizontal Dilution of Precision
        /// </summary>
        public DilutionOfPrecision HorizontalDilutionOfPrecision
        {
            get { return _horizontalDilutionOfPrecision; }
        }

        #endregion IHorizontalDilutionOfPrecisionSentence Members

        #region IGeoidalSeparationSentence Members

        /// <summary>
        /// The Geoidal Separation
        /// </summary>
        public Distance GeoidalSeparation
        {
            get { return _geoidalSeparation; }
        }

        #endregion IGeoidalSeparationSentence Members

        #region IFixQualitySentence Members

        /// <summary>
        /// The Fix Quality
        /// </summary>
        public FixQuality FixQuality
        {
            get { return _fixQuality; }
        }

        #endregion IFixQualitySentence Members

        #region IDifferentialGpsSentence Members

        /// <summary>
        /// The integer ID of the GPS Station
        /// </summary>
        public int DifferentialGpsStationID
        {
            get { return _differentialGpsStationID; }
        }

        /// <summary>
        /// Differntial GPS Age
        /// </summary>
        public TimeSpan DifferentialGpsAge
        {
            get { return _differentialGpsAge; }
        }

        #endregion IDifferentialGpsSentence Members

        #region IFixedSatelliteCountSentence Members

        /// <summary>
        /// The Fixed Satellite Count
        /// </summary>
        public int FixedSatelliteCount
        {
            get { return _fixedSatelliteCount; }
        }

        #endregion IFixedSatelliteCountSentence Members
    }
}