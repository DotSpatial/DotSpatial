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
        #region Constructors

        /// <summary>
        /// Creates a new GpggaSentence
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GpggaSentence(string sentence)
            : base(sentence)
        {
            FixedSatelliteCount = -1;
            SetPropertiesFromSentence();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpggaSentence"/> class.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="commandWord">The command word.</param>
        /// <param name="words">The words.</param>
        /// <param name="validChecksum">The valid checksum.</param>
        internal GpggaSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        {
            FixedSatelliteCount = -1;
            SetPropertiesFromSentence();
        }

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
        /// <param name="differentialGpsStationId">The differential GPS station ID.</param>
        public GpggaSentence(TimeSpan utcTime, Position position, FixQuality fixQuality, int trackedSatelliteCount, DilutionOfPrecision horizontalDilutionOfPrecision,
                             Distance altitude, Distance geoidalSeparation, TimeSpan differentialGpsAge, int differentialGpsStationId)
        {
            FixedSatelliteCount = -1;
            // Use a string builder to create the sentence text
            StringBuilder builder = new StringBuilder(128);

            // Append the command word
            builder.Append("$GPGGA");
            builder.Append(',');

            // Convert UTC time to a string in the form HHMMSS.SSSS. Any value less than 10 will be padded with a zero.
            builder.Append(utcTime.Hours.ToString("0#", NmeaCultureInfo));
            builder.Append(utcTime.Minutes.ToString("0#", NmeaCultureInfo));
            builder.Append(utcTime.Seconds.ToString("0#", NmeaCultureInfo));
            builder.Append(".");
            builder.Append(utcTime.Milliseconds.ToString("00#", NmeaCultureInfo));
            builder.Append(',');

            #region Append the position

            // Append latitude in the format HHMM.MMMM.
            builder.Append(position.Latitude.ToString(LatitudeFormat, NmeaCultureInfo));
            // Append Longitude in the format HHHMM.MMMM.
            builder.Append(position.Longitude.ToString(LongitudeFormat, NmeaCultureInfo));

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
            // Append a comma
            builder.Append(",");
            #endregion Append fix quality
            
            // Append the tracked (signal strength is > 0) satellite count
            builder.Append(trackedSatelliteCount.ToString(NmeaCultureInfo));
            builder.Append(",");

            // Append the numerical value of HDOP
            builder.Append(horizontalDilutionOfPrecision.Value.ToString(NmeaCultureInfo));
            builder.Append(",");

            #region Altitude above sea level

            // Append the numerical value in meters
            builder.Append(altitude.ToMeters().Value.ToString(NmeaCultureInfo));

            // Append a comma, the unit (M = meters), and another comma
            builder.Append(",M,");

            #endregion Altitude above sea level

            #region Geoidal separation

            // Append the numerical value in meters
            builder.Append(geoidalSeparation.ToMeters().Value.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(",M,");

            #endregion Geoidal separation

            #region Differential GPS information

            // Differential signal age in seconds
            if (!differentialGpsAge.Equals(TimeSpan.MinValue))
                builder.Append(differentialGpsAge.TotalSeconds.ToString(NmeaCultureInfo));
            builder.Append(",");

            // Station ID
            if (differentialGpsStationId != -1)
                builder.Append(differentialGpsStationId.ToString(NmeaCultureInfo));

            #endregion Differential GPS information

            // Set this object's sentence
            Sentence = builder.ToString();
            SetPropertiesFromSentence();

            // Finally, append the checksum
            AppendChecksum();
        }

        #endregion Constructors

        /// <summary>
        /// Corrects this classes properties after the base sentence was changed.
        /// </summary>
        private new void SetPropertiesFromSentence()
        {
            // Cache the words
            string[] words = Words;
            int wordCount = words.Length;

            UtcTime = ParseUtcTimeSpan(0);
            Position = ParsePosition(1, 2, 3, 4);
            FixQuality = ParseFixQuality(5);

            // Number of satellites in view is skipped.  We'll work off of GPGSV data.
            if (wordCount >= 7 && words[6].Length != 0)
                FixedSatelliteCount = int.Parse(words[6], NmeaCultureInfo);

            HorizontalDilutionOfPrecision = ParseDilution(7);
            Altitude = ParseDistance(8, DistanceUnit.Meters);
            GeoidalSeparation = ParseDistance(10, DistanceUnit.Meters);

            // Is there enough info to process Differential GPS info?
            if (wordCount >= 14 && words[12].Length != 0 && words[13].Length != 0)
            {
                DifferentialGpsAge = words[12].Length != 0 ? TimeSpan.FromSeconds(float.Parse(words[12], NmeaCultureInfo)) : TimeSpan.MinValue;
                DifferentialGpsStationID = words[13].Length != 0 ? int.Parse(words[13], NmeaCultureInfo) : -1;
            }
            else
            {
                DifferentialGpsStationID = -1;
                DifferentialGpsAge = TimeSpan.MinValue;
            }
        }

        #region Properties

        /// <summary>
        /// Represents an NMEA sentence which contains a position.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// Gets the time in UTC from the IUtcTimeSentence
        /// </summary>
        public TimeSpan UtcTime { get; private set; }

        /// <summary>
        /// The Altitude
        /// </summary>
        public Distance Altitude { get; private set; }

        /// <summary>
        /// The Horizontal Dilution of Precision
        /// </summary>
        public DilutionOfPrecision HorizontalDilutionOfPrecision { get; private set; }

        /// <summary>
        /// The Geoidal Separation
        /// </summary>
        public Distance GeoidalSeparation { get; private set; }

        /// <summary>
        /// The Fix Quality
        /// </summary>
        public FixQuality FixQuality { get; private set; }

        /// <summary>
        /// The integer ID of the GPS Station
        /// </summary>
        public int DifferentialGpsStationID { get; private set; }

        /// <summary>
        /// Differential GPS Age
        /// </summary>
        public TimeSpan DifferentialGpsAge { get; private set; }

        /// <summary>
        /// The Fixed Satellite Count
        /// </summary>
        public int FixedSatelliteCount { get; private set; }

        #endregion
    }
}