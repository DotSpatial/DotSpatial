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

namespace DotSpatial.Positioning
{
    /// <summary>
    /// $--GGK Header including Talker ID
    /// hhmmss.ss UTC time of location
    /// mmddyy    UTC date
    /// llll.ll(Latitude)
    /// a(Hemisphere, North Or South)
    /// yyyyy.yy(Longitude)
    /// a(East Or West)
    /// x GPS quality indicator
    /// 0 = Fix not available or invalid
    /// 1 = No real-time location, navigation fix
    /// 2 = Real-time location, ambiguities not fixed
    /// 3 = Real-time location, ambiguities fixed
    /// xx Number of satellites in use, 00 to 12.
    /// x.x(GDOP)
    /// EHT Ellipsoidal height
    /// x.x Altitude of location marker as local ellipsoidal height. If the local ellipsoidal
    /// height is not available, the WGS 1984 ellipsoidal height will be exported.
    /// </summary>
    public sealed class GpggkSentence : NmeaSentence, IPositionSentence, IUtcDateTimeSentence, IFixQualitySentence, IPositionDilutionOfPrecisionSentence,
                                    IAltitudeAboveEllipsoidSentence
    {
        /// <summary>
        ///
        /// </summary>
        private DateTime _utcDateTime;
        /// <summary>
        ///
        /// </summary>
        private Position _position;
        /// <summary>
        ///
        /// </summary>
        private FixQuality _fixQuality;
        /// <summary>
        ///
        /// </summary>
        private DilutionOfPrecision _meanDilutionOfPrecision;
        /// <summary>
        ///
        /// </summary>
        private Distance _altitudeAboveEllipsoid;

        #region Constructors

        /// <summary>
        /// Creates a GpggkSentence from the specified string
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GpggkSentence(string sentence)
            : base(sentence)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpggkSentence"/> class.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="commandWord">The command word.</param>
        /// <param name="words">The words.</param>
        /// <param name="validChecksum">The valid checksum.</param>
        internal GpggkSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        #endregion Constructors

        /// <summary>
        /// Called when [sentence changed].
        /// </summary>
        protected override void OnSentenceChanged()
        {
            // Process the basic sentence elements
            base.OnSentenceChanged();

            // Cache the word array
            string[] words = Words;
            int wordCount = words.Length;

            // Do we have enough words to parse the UTC date/time?
            if (wordCount >= 2 && words[0].Length != 0 && words[1].Length != 0)
            {
                #region Parse the UTC time

                string utcTimeWord = words[0];
                int utcHours = int.Parse(utcTimeWord.Substring(0, 2), NmeaCultureInfo);
                int utcMinutes = int.Parse(utcTimeWord.Substring(2, 2), NmeaCultureInfo);
                int utcSeconds = int.Parse(utcTimeWord.Substring(4, 2), NmeaCultureInfo);
                int utcMilliseconds = Convert.ToInt32(float.Parse(utcTimeWord.Substring(6), NmeaCultureInfo) * 1000, NmeaCultureInfo);

                #endregion Parse the UTC time

                #region Parse the UTC date

                string utcDateWord = words[1];
                int utcMonth = int.Parse(utcDateWord.Substring(0, 2), NmeaCultureInfo);
                int utcDay = int.Parse(utcDateWord.Substring(2, 2), NmeaCultureInfo);
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
                    words[3].Equals("N", StringComparison.OrdinalIgnoreCase) ? LatitudeHemisphere.North : LatitudeHemisphere.South;

                #endregion Parse the latitude

                #region Parse the longitude

                string longitudeWord = words[4];
                int longitudeHours = int.Parse(longitudeWord.Substring(0, 3), NmeaCultureInfo);
                double longitudeDecimalMinutes = double.Parse(longitudeWord.Substring(3), NmeaCultureInfo);
                LongitudeHemisphere longitudeHemisphere =
                    words[5].Equals("E", StringComparison.OrdinalIgnoreCase) ? LongitudeHemisphere.East : LongitudeHemisphere.West;

                #endregion Parse the longitude

                #region Build a Position from the latitude and longitude

                _position = new Position(
                                new Latitude(latitudeHours, latitudeDecimalMinutes, latitudeHemisphere),
                                new Longitude(longitudeHours, longitudeDecimalMinutes, longitudeHemisphere));

                #endregion Build a Position from the latitude and longitude
            }

            // Do we have enough data for the fix quality?
            if (wordCount > 7 && words[7].Length != 0)
            {
                switch (Convert.ToInt32(words[7], NmeaCultureInfo))
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
            }
            else
            {
                // The fix quality is invalid
                _fixQuality = FixQuality.Unknown;
            }

            // Process the fixed satellite count
            //_fixedSatelliteCount = wordCount > 8 ? int.Parse(Words[8], NmeaCultureInfo) : 0;

            // Process the mean DOP
            if (wordCount > 9 && Words[9].Length != 0)
                _meanDilutionOfPrecision = new DilutionOfPrecision(float.Parse(Words[9], NmeaCultureInfo));
            else
                _meanDilutionOfPrecision = DilutionOfPrecision.Maximum;

            // Altitude above ellipsoid
            if (wordCount > 10 && Words[10].Length != 0)
                _altitudeAboveEllipsoid = new Distance(double.Parse(Words[10], NmeaCultureInfo), DistanceUnit.Meters).ToLocalUnitType();
            else
                _altitudeAboveEllipsoid = Distance.Empty;
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

        #region IFixQualitySentence Members

        /// <summary>
        /// The Fix Quality
        /// </summary>
        public FixQuality FixQuality
        {
            get { return _fixQuality; }
        }

        #endregion IFixQualitySentence Members

        #region IMeanDilutionOfPrecisionSentence Members

        /// <summary>
        /// The Position Dilution of Precision (PDOP)
        /// </summary>
        public DilutionOfPrecision PositionDilutionOfPrecision
        {
            get { return _meanDilutionOfPrecision; }
        }

        #endregion IMeanDilutionOfPrecisionSentence Members

        #region IAltitudeAboveEllipsoidSentence Members

        /// <summary>
        /// The Altitude Above Ellipsoid
        /// </summary>
        public Distance AltitudeAboveEllipsoid
        {
            get { return _altitudeAboveEllipsoid; }
        }

        #endregion IAltitudeAboveEllipsoidSentence Members

        #region IUtcDateTimeSentence Members

        /// <summary>
        /// Represents an NMEA sentence which contains date and time in UTC.
        /// </summary>
        public DateTime UtcDateTime
        {
            get { return _utcDateTime; }
        }

        #endregion IUtcDateTimeSentence Members
    }
}