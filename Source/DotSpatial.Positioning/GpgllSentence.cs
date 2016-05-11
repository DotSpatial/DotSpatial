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
    /// $GPGLL
    /// Geographic Position, Latitude / Longitude and time.
    /// http://aprs.gids.nl/nmea/#gll
    /// eg1. $GPGLL, 3751.65, S, 14507.36, E*77
    /// eg2. $GPGLL, 4916.45, N, 12311.12, W, 225444, A
    /// 4916.46, N    Latitude 49 deg. 16.45 min. North
    /// 12311.12, W   Longitude 123 deg. 11.12 min. West
    /// 225444       Fix taken at 22:54:44 UTC
    /// A            Data valid
    /// eg3. $GPGLL, 5133.81, N, 00042.25, W*75
    /// 1    2     3    4 5
    /// 1    5133.81   Current latitude
    /// 2    N         North/South
    /// 3    00042.25  Current longitude
    /// 4    W         East/West
    /// 5    *75       checksum
    /// $--GLL, lll.ll, a, yyyyy.yy, a, hhmmss.ss, A
    /// llll.ll = Latitude of location
    /// a = N or S
    /// yyyyy.yy = Longitude of location
    /// a = E or W
    /// hhmmss.ss = UTC of location
    /// A = status: A = valid data
    /// </summary>
    public sealed class GpgllSentence : NmeaSentence, IUtcTimeSentence, IPositionSentence, IFixStatusSentence
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgllSentence"/> class.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="commandWord">The command word.</param>
        /// <param name="words">The words.</param>
        /// <param name="validChecksum">The valid checksum.</param>
        internal GpgllSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { SetPropertiesFromSentence(); }

        /// <summary>
        /// Creates a GpgllSentence from the specified string
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GpgllSentence(string sentence)
            : base(sentence)
        { SetPropertiesFromSentence(); }

        /// <summary>
        /// Creates a GpgllSentence from the specified parameters
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="utcTime">The UTC time.</param>
        /// <param name="fixStatus">The fix status.</param>
        public GpgllSentence(Position position, TimeSpan utcTime, FixStatus fixStatus)
        {
            // Build a sentence
            StringBuilder builder = new StringBuilder(128);

            // Append the command word
            builder.Append("$GPGLL");
            builder.Append(',');

            // Append latitude in the format HHMM.MMMM.
            builder.Append(position.Latitude.ToString(LatitudeFormat, NmeaCultureInfo));
            // Append Longitude in the format HHHMM.MMMM.
            builder.Append(position.Longitude.ToString(LongitudeFormat, NmeaCultureInfo));

            // Append the UTC time
            builder.Append(utcTime.Hours.ToString("0#", NmeaCultureInfo));
            builder.Append(utcTime.Minutes.ToString("0#", NmeaCultureInfo));
            builder.Append(utcTime.Seconds.ToString("0#", NmeaCultureInfo));
            builder.Append(".");
            builder.Append(utcTime.Milliseconds.ToString("00#", NmeaCultureInfo));
            builder.Append(",");

            //Append the fix status
            switch (fixStatus)
            {
                case FixStatus.Fix:
                    builder.Append("A");
                    break;
                default:
                    builder.Append("V");  // V = INvalid
                    break;
            }

            // Set this object's sentence
            Sentence = builder.ToString();
            SetPropertiesFromSentence();

            // Finally, append the checksum
            AppendChecksum();
        }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Corrects this classes properties after the base sentence was changed.
        /// </summary>
        private new void SetPropertiesFromSentence()
        {
            Position = ParsePosition(0, 1, 2, 3);
            UtcTime = ParseUtcTimeSpan(4);
            FixStatus = ParseFixStatus(5);
        }

        #endregion Overrides

        #region Properties

        /// <summary>
        /// Gets the time in UTC from the IUtcTimeSentence
        /// </summary>
        public TimeSpan UtcTime { get; private set; }

        /// <summary>
        /// Represents an NMEA sentence which contains a position.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// The Fix Status
        /// </summary>
        public FixStatus FixStatus { get; private set; }

        #endregion
    }
}