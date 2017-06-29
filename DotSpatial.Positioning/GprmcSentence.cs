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
    /// Represents the "recommended minimum" GPS sentence.
    /// </summary>
    public sealed class GprmcSentence : NmeaSentence, IPositionSentence, IUtcDateTimeSentence,
                                    IBearingSentence, ISpeedSentence, IMagneticVariationSentence,
                                    IFixStatusSentence
    {
        /// <summary>
        ///
        /// </summary>
        private DateTime _utcDateTime;
        /// <summary>
        ///
        /// </summary>
        private FixStatus _fixStatus;
        /// <summary>
        ///
        /// </summary>
        private Position _position;
        /// <summary>
        ///
        /// </summary>
        private Speed _speed;
        /// <summary>
        ///
        /// </summary>
        private Azimuth _bearing;
        /// <summary>
        ///
        /// </summary>
        private Longitude _magneticVariation;

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
        { }

        /// <summary>
        /// Creates a GprmcSentence from the specified string
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GprmcSentence(string sentence)
            : base(sentence)
        { }

        /// <summary>
        /// Creates a GprmcSentence from the specified parameters
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

            // Append a comma
            builder.Append(',');

            #region Append the UTC time

            builder.Append(utcDateTime.Hour.ToString("0#", NmeaCultureInfo));
            builder.Append(utcDateTime.Minute.ToString("0#", NmeaCultureInfo));
            builder.Append(utcDateTime.Second.ToString("0#", NmeaCultureInfo));
            builder.Append(".");
            builder.Append(utcDateTime.Millisecond.ToString("00#", NmeaCultureInfo));

            #endregion Append the UTC time

            // Append a comma
            builder.Append(',');

            #region Append the fix status

            // Write fix information
            builder.Append(isFixAcquired ? "A" : "V");

            #endregion Append the fix status

            // Append a comma
            builder.Append(',');

            #region Append the position

            // Append latitude in the format HHMM.MMMM.
            builder.Append(position.Latitude.ToString(NmeaSentence.LatitudeFormat, NmeaCultureInfo));
            // Append Longitude in the format HHHMM.MMMM.
            builder.Append(position.Longitude.ToString(NmeaSentence.LongitudeFormat, NmeaCultureInfo));

            #endregion Append the position

            // Append the speed (in knots)
            builder.Append(speed.ToKnots().ToString("v.v", NmeaCultureInfo));

            // Append a comma
            builder.Append(',');

            // Append the bearing
            builder.Append(bearing.ToString("d.d", NmeaCultureInfo));

            // Append a comma
            builder.Append(',');

            #region Append the UTC date

            // Append UTC date
            builder.Append(utcDateTime.Day.ToString("0#", NmeaCultureInfo));
            builder.Append(utcDateTime.Month.ToString("0#", NmeaCultureInfo));

            // Append the year (year minus 2000)
            int year = utcDateTime.Year - 2000;
            builder.Append(year.ToString("0#", NmeaCultureInfo));

            #endregion Append the UTC date

            // Append a comma
            builder.Append(',');

            // Append magnetic variation
            builder.Append(magneticVariation.ToString("d.d", NmeaCultureInfo));

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
            // First, process the basic info for the sentence
            base.OnSentenceChanged();

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
                               1    2    3    4    5     6    7    8      9     10  11 12

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

            // Do we have enough words to parse the fix status?
            if (wordCount >= 2 && words[1].Length != 0)
            {
                #region Fix status

                // Get the fix flag
                _fixStatus = words[1].Equals("A", StringComparison.OrdinalIgnoreCase) ? FixStatus.Fix : FixStatus.NoFix;

                #endregion Fix status
            }
            else
            {
                // The fix status is invalid
                _fixStatus = FixStatus.Unknown;
            }

            // Do we have enough words to parse the UTC date/time?
            if (wordCount >= 9 && words[0].Length != 0 && words[8].Length != 0)
            {
                #region Parse the UTC time

                string utcTimeWord = words[0];
                int utcHours = int.Parse(utcTimeWord.Substring(0, 2), NmeaCultureInfo);                 // AA
                int utcMinutes = int.Parse(utcTimeWord.Substring(2, 2), NmeaCultureInfo);               // BB
                int utcSeconds = int.Parse(utcTimeWord.Substring(4, 2), NmeaCultureInfo);               // CC
                int utcMilliseconds = 0;
                if (utcTimeWord.Length > 6)
                    utcMilliseconds = Convert.ToInt32(float.Parse(utcTimeWord.Substring(6), NmeaCultureInfo) * 1000, NmeaCultureInfo);    // DDDD

                #endregion Parse the UTC time

                #region Parse the UTC date

                string utcDateWord = words[8];
                int utcDay = int.Parse(utcDateWord.Substring(0, 2), NmeaCultureInfo);
                int utcMonth = int.Parse(utcDateWord.Substring(2, 2), NmeaCultureInfo);
                int utcYear = int.Parse(utcDateWord.Substring(4, 2), NmeaCultureInfo) + 2000;

                #endregion Parse the UTC date

                #region Build a UTC date/time

                _utcDateTime = new DateTime(utcYear, utcMonth, utcDay, utcHours, utcMinutes, utcSeconds, utcMilliseconds, DateTimeKind.Utc);

                #endregion Build a UTC date/time
            }
            else
            {
                // The UTC date/time is invalid
                _utcDateTime = DateTime.MinValue;
            }

            // Do we have enough data to parse the location?
            if (wordCount >= 6 && words[2].Length != 0 && words[3].Length != 0 && words[4].Length != 0 && words[5].Length != 0)
            {
                #region Parse the latitude

                string latitudeWord = words[2];
                int latitudeHours = int.Parse(latitudeWord.Substring(0, 2), NmeaCultureInfo);
                double latitudeDecimalMinutes = double.Parse(latitudeWord.Substring(2), NmeaCultureInfo);
                LatitudeHemisphere latitudeHemisphere =
                    words[3].Equals("N", StringComparison.Ordinal) ? LatitudeHemisphere.North : LatitudeHemisphere.South;

                #endregion Parse the latitude

                #region Parse the longitude

                string longitudeWord = words[4];
                int longitudeHours = int.Parse(longitudeWord.Substring(0, 3), NmeaCultureInfo);
                double longitudeDecimalMinutes = double.Parse(longitudeWord.Substring(3), NmeaCultureInfo);
                LongitudeHemisphere longitudeHemisphere =
                    words[5].Equals("E", StringComparison.Ordinal) ? LongitudeHemisphere.East : LongitudeHemisphere.West;

                #endregion Parse the longitude

                #region Build a Position from the latitude and longitude

                _position = new Position(
                                new Latitude(latitudeHours, latitudeDecimalMinutes, latitudeHemisphere),
                                new Longitude(longitudeHours, longitudeDecimalMinutes, longitudeHemisphere));

                #endregion Build a Position from the latitude and longitude
            }
            else
            {
                _position = Position.Invalid;
            }

            // Do we have enough info to process speed?
            if (wordCount >= 7 && words[6].Length != 0)
            {
                #region Speed

                // The speed is the sixth word, expressed in knots
                _speed = new Speed(double.Parse(words[6], NmeaCultureInfo), SpeedUnit.Knots);

                #endregion Speed
            }
            else
            {
                // The speed is invalid
                _speed = Speed.Invalid;
            }

            // do we have enough info to process the bearing?
            if (wordCount >= 8 && words[7].Length != 0)
            {
                #region Bearing

                // The bearing is the seventh word
                _bearing = new Azimuth(double.Parse(words[7], NmeaCultureInfo));

                #endregion Bearing
            }
            else
            {
                // The bearing is invalid
                _bearing = Azimuth.Invalid;
            }

            // Do we have enough info for magnetic variation?
            if (wordCount >= 10 && words[9].Length != 0)
            {
                // Parse the value
                _magneticVariation = new Longitude(double.Parse(words[9], NmeaCultureInfo));
            }
            else
            {
                // The magnetic variation is invalid
                _magneticVariation = Longitude.Invalid;
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
        /// Represents an NMEA sentence which contains date and time in UTC.
        /// </summary>
        public DateTime UtcDateTime
        {
            get { return _utcDateTime; }
        }

        #endregion IUtcDatetimeSentence Members

        #region IBearingSentence Members

        /// <summary>
        /// the Bearing
        /// </summary>
        public Azimuth Bearing
        {
            get { return _bearing; }
        }

        #endregion IBearingSentence Members

        #region ISpeedSentence Members

        /// <summary>
        /// The Speed
        /// </summary>
        public Speed Speed
        {
            get { return _speed; }
        }

        #endregion ISpeedSentence Members

        #region IMagneticVariationSentence Members

        /// <summary>
        /// The Magnetic Variation
        /// </summary>
        public Longitude MagneticVariation
        {
            get { return _magneticVariation; }
        }

        #endregion IMagneticVariationSentence Members

        #region IFixStatusSentence Members

        /// <summary>
        /// The Fix Status
        /// </summary>
        public FixStatus FixStatus
        {
            get { return _fixStatus; }
        }

        #endregion IFixStatusSentence Members
    }
}