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
    /// Represents the "recommended minimum" GPS sentence.
    /// </summary>
    public sealed class GprmcSentence : NmeaSentence, IPositionSentence, IUtcDateTimeSentence, IBearingSentence, ISpeedSentence, IMagneticVariationSentence, IFixStatusSentence
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GprmcSentence"/> class.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="commandWord">The command word.</param>
        /// <param name="words">The words.</param>
        /// <param name="validChecksum">The valid checksum.</param>
        internal GprmcSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        {
            SetPropertiesFromSentence(); // correct this classes properties based on the sentence
        }

        /// <summary>
        /// Creates a GprmcSentence from the specified string.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GprmcSentence(string sentence)
            : base(sentence)
        {
            SetPropertiesFromSentence(); // correct this classes properties based on the sentence
        }

        /// <summary>
        /// Creates a GprmcSentence from the specified parameters.
        /// </summary>
        /// <param name="utcDateTime">The UTC date time.</param>
        /// <param name="isFixAcquired">if set to <c>true</c> [is fix acquired].</param>
        /// <param name="position">The position.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="bearing">The bearing.</param>
        /// <param name="magneticVariation">The magnetic variation.</param>
        public GprmcSentence(DateTime utcDateTime, bool isFixAcquired, Position position, Speed speed, Azimuth bearing, Longitude magneticVariation)
        {
            // Use a string builder to create the sentence text
            StringBuilder builder = new StringBuilder(128);

            /* GPRMC sentences have the following format:
             *
             * $GPRMC, 040302.663, A, 3939.7, N, 10506.6, W, 0.27, 358.86, 200804, ,*1A
             */

            // Append the command word, $GPRMC
            builder.Append("$GPRMC");
            builder.Append(',');

            // Append the UTC time
            builder.Append(utcDateTime.Hour.ToString("0#", NmeaCultureInfo));
            builder.Append(utcDateTime.Minute.ToString("0#", NmeaCultureInfo));
            builder.Append(utcDateTime.Second.ToString("0#", NmeaCultureInfo));
            builder.Append(".");
            builder.Append(utcDateTime.Millisecond.ToString("00#", NmeaCultureInfo));
            builder.Append(',');

            // Write fix information
            builder.Append(isFixAcquired ? "A" : "V");
            builder.Append(',');

            // Append the position
            // Append latitude in the format HHMM.MMMM.
            builder.Append(position.Latitude.ToString(LatitudeFormat, NmeaCultureInfo));
            // Append Longitude in the format HHHMM.MMMM.
            builder.Append(position.Longitude.ToString(LongitudeFormat, NmeaCultureInfo));

            // Append the speed (in knots)
            builder.Append(speed.ToKnots().ToString("v.v", NmeaCultureInfo));
            builder.Append(',');

            // Append the bearing
            builder.Append(bearing.ToString("d.d", NmeaCultureInfo));
            builder.Append(',');

            // Append UTC date
            builder.Append(utcDateTime.Day.ToString("0#", NmeaCultureInfo));
            builder.Append(utcDateTime.Month.ToString("0#", NmeaCultureInfo));

            // Append the year (year minus 2000)
            int year = utcDateTime.Year - 2000;
            builder.Append(year.ToString("0#", NmeaCultureInfo));
            builder.Append(',');

            // Append magnetic variation
            builder.Append(Math.Abs(magneticVariation.DecimalDegrees).ToString("0.0", NmeaCultureInfo));
            builder.Append(",");
            builder.Append(magneticVariation.Hemisphere == LongitudeHemisphere.West ? "W" : "E");

            // Set this object's sentence
            Sentence = builder.ToString();
            SetPropertiesFromSentence(); // correct this classes properties based on the sentence

            // Finally, append the checksum
            AppendChecksum();
        }

        #endregion Constructors

        /// <summary>
        /// Corrects this classes properties after the base sentence was changed.
        /// </summary>
        private new void SetPropertiesFromSentence()
        {
            // Cache the sentence words
            string[] words = Words;
            int wordCount = words.Length;

            /*
             * $GPRMC

                 Recommended minimum specific GPS/Transit data

                 eg1. $GPRMC, 081836, A, 3751.65, S, 14507.36, E, 000.0, 360.0, 130998, 011.3, E*62
                 eg2. $GPRMC, 225446, A, 4916.45, N, 12311.12, W, 000.5, 054.7, 191194, 020.3, E*68

                            225446       Time of fix 22:54:46 UTC
                            A            Navigation receiver warning A = OK, V = warning
                            4916.45, N    Latitude 49 deg. 16.45 min North
                            12311.12, W   Longitude 123 deg. 11.12 min West
                            000.5        Speed over ground, Knots
                            054.7        Course Made Good, True
                            191194       Date of fix  19 November 1994
                            020.3, E      Magnetic variation 20.3 deg East
                            *68          mandatory checksum

                 eg3. $GPRMC, 220516, A, 5133.82, N, 00042.24, W, 173.8, 231.8, 130694, 004.2, W*70
                               1      2    3      4    5       6    7      8      9      10   11 12

                       1   220516     Time Stamp
                       2   A          validity - A-ok, V-invalid
                       3   5133.82    current Latitude
                       4   N          North/South
                       5   00042.24   current Longitude
                       6   W          East/West
                       7   173.8      Speed in knots
                       8   231.8      True course
                       9   130694     Date Stamp
                       10  004.2      Variation
                       11  W          East/West
                       12  *70        checksum

                 eg4. $GPRMC, hhmmss.ss, A, llll.ll, a, yyyyy.yy, a, x.x, x.x, ddmmyy, x.x, a*hh
                 1    = UTC of position fix
                 2    = Data status (V=navigation receiver warning)
                 3    = Latitude of fix
                 4    = N or S
                 5    = Longitude of fix
                 6    = E or W
                 7    = Speed over ground in knots
                 8    = Track made good in degrees True
                 9    = UT date
                 10   = Magnetic variation degrees (Easterly var. subtracts from true course)
                 11   = E or W
                 12   = Checksum
             */

            FixStatus = ParseFixStatus(1);
            UtcDateTime = ParseUtcDateTime(0, 8);
            Position = ParsePosition(2, 3, 4, 5);
            Speed = ParseSpeed(6, SpeedUnit.Knots);
            Bearing = ParseAzimuth(7);


            // Do we have enough info for magnetic variation?
            if (wordCount > 10 && words[9].Length != 0 && words[10].Length != 0)
                MagneticVariation = new Longitude(double.Parse(words[9], NmeaCultureInfo), words[10].Equals("E", StringComparison.Ordinal) ? LongitudeHemisphere.East : LongitudeHemisphere.West);
            else
                MagneticVariation = Longitude.Invalid;
        }

        #region Properties

        /// <summary>
        /// Represents an NMEA sentence which contains a position.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// Represents an NMEA sentence which contains date and time in UTC.
        /// </summary>
        public DateTime UtcDateTime { get; private set; }

        /// <summary>
        /// the Bearing
        /// </summary>
        public Azimuth Bearing { get; private set; }

        /// <summary>
        /// The Speed
        /// </summary>
        public Speed Speed { get; private set; }

        /// <summary>
        /// The Magnetic Variation
        /// </summary>
        public Longitude MagneticVariation { get; private set; }

        /// <summary>
        /// The Fix Status
        /// </summary>
        public FixStatus FixStatus { get; private set; }

        #endregion
    }
}