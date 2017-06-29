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
    /// Represents a Garmin $PGRMF sentence.
    /// </summary>
    public sealed class PgrmfSentence : NmeaSentence, IUtcDateTimeSentence, IPositionSentence, IFixModeSentence,
    IFixQualitySentence, ISpeedSentence, IBearingSentence, IPositionDilutionOfPrecisionSentence
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
        private FixMode _fixMode;
        /// <summary>
        ///
        /// </summary>
        private FixQuality _fixQuality;
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
        private DilutionOfPrecision _positionDop;

        #region Constructors

        /// <summary>
        /// Creates a Garmin $PGRMF sentence from the specified parameters
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="commandWord">The command word.</param>
        /// <param name="words">The words.</param>
        /// <param name="validChecksum">The valid checksum.</param>
        internal PgrmfSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        /// <summary>
        /// Creates a Garmin $PGRMF sentence from the specified string
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public PgrmfSentence(string sentence)
            : base(sentence)
        { }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// OnSentanceChanged event handler
        /// </summary>
        protected override void OnSentenceChanged()
        {
            base.OnSentenceChanged();

            // Cache the sentence words
            string[] words = Words;
            int wordCount = words.Length;

            /*
             *  Garmin produces several embedded GPS systems. They are easy to setup because Garmin provides a nice utility for uploading configuration data to the GPS. You first load the utility to a PC. Connect the PC to the GPS through one of the serial ports. The utility will check each baud rate until it communicates with the GPS.

                The common configuration parameters are output sentences from the GPS unit, the communication baud rate with a host, and the required pulse per second.

                Each sentence is preceded with a ‘$’ symbol and ends with a line-feed character. At one sentence per second, the following is out put in four seconds:

                $PGRMF, 223, 424798, 041203, 215945, 13, 0000.0000, N, 00000.0000, W, A, 2, 0, 62, 2, 1*3B
                $PGRMF, 223, 424799, 041203, 215946, 13, 00000.0000, N, 00000.0000, W, A, 2, 0, 62, 2, 1*39
                $PGRMF, 223, 424800, 041203, 215947, 13, 00000.0000, N, 00000.0000, W, A, 2, 0, 62, 2, 1*34
                $PGRMF, 223, 424801, 041203, 215948, 13, 00000.0000, N, 00000.0000, W, A, 2, 0, 62, 2, 1*35

                The sentence is proprietary to the Garmin GPS Global Positioning System and is translated below.

                $PGRMF
                <1>GPS Week Number(0-1023)
                <2>GPS Seconds (0 - 604799)
                <3>UTC Date of position fix, ddmmyy format
                <4>UTC time of position fix, hhmmss format
                <5>GPS leap second count
                <6>Latitude, ddmm.mmmm format (leading zeros transmitted)
                <7>Latitude hemisphere N or S
                <8>Longitude, ddmm.mmmm format (leading zeros transmitted)
                <9>Longitude hemisphere N or S
                <10>Mode M = Manual, A automatic
                <11>Fix type 0 = No Fix, 1 = 2D Fix, 2 = 3D fix
                <12>Speed over ground, 0 to 359 degrees true
                <13>Course over ground, 0 to 9 (rounded to nearest intvalue)
                <14>Time dilution of precision, 0 to 9 (rnded nearest int val)
                <15>Time dilution of precision, 0 to 9 (rnded nearest int val)
                *hh <CR><LF>
             */

            // TODO: Convert GPS week number/seconds to UTC date/time

            // Do we have enough words to parse the fix status?
            if (wordCount >= 4 && words[2].Length != 0 && words[3].Length != 0)
            {
                // Build a UTC date/time object.
                _utcDateTime = new DateTime(
                    int.Parse(words[2].Substring(4, 2), NmeaCultureInfo) + 2000, // Year
                    int.Parse(words[2].Substring(2, 2), NmeaCultureInfo), // Month
                    int.Parse(words[2].Substring(0, 2), NmeaCultureInfo), // Day
                    int.Parse(words[3].Substring(0, 2), NmeaCultureInfo), // Hour
                    int.Parse(words[3].Substring(2, 2), NmeaCultureInfo), // Minute
                    int.Parse(words[3].Substring(4, 2), NmeaCultureInfo), // Second
                    DateTimeKind.Utc);
            }

            #region Position

            // Can we parse the latitude and longitude?
            if (wordCount >= 8 && words[5].Length != 0 && words[6].Length != 0 && words[7].Length != 0 && words[8].Length != 0)
            {
                #region Parse the latitude

                string latitudeWord = words[5];
                int latitudeHours = int.Parse(latitudeWord.Substring(0, 2), NmeaCultureInfo);
                double latitudeDecimalMinutes = double.Parse(latitudeWord.Substring(2), NmeaCultureInfo);
                LatitudeHemisphere latitudeHemisphere =
                    words[6].Equals("N", StringComparison.Ordinal) ? LatitudeHemisphere.North : LatitudeHemisphere.South;

                #endregion Parse the latitude

                #region Parse the longitude

                string longitudeWord = words[7];
                int longitudeHours = int.Parse(longitudeWord.Substring(0, 3), NmeaCultureInfo);
                double longitudeDecimalMinutes = double.Parse(longitudeWord.Substring(3), NmeaCultureInfo);
                LongitudeHemisphere longitudeHemisphere =
                    words[8].Equals("E", StringComparison.Ordinal) ? LongitudeHemisphere.East : LongitudeHemisphere.West;

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

            #endregion Position

            #region Fix Mode

            if (wordCount >= 9 && words[9].Length != 0)
            {
                _fixMode = words[9] == "A" ? FixMode.Automatic : FixMode.Manual;
            }
            else
            {
                _fixMode = FixMode.Unknown;
            }

            #endregion Fix Mode

            #region Fix Quality

            // Do we have enough data for fix quality?
            if (wordCount >= 10 && words[10].Length != 0)
            {
                switch (int.Parse(words[10], NmeaCultureInfo))
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
                // This fix quality is invalid
                _fixQuality = FixQuality.Unknown;
            }

            #endregion Fix Quality

            #region Bearing

            // Do we have enough data for fix quality?
            if (wordCount >= 13 && words[12].Length != 0)
            {
                _bearing = new Azimuth(words[12], NmeaCultureInfo);
            }
            else
            {
                _bearing = Azimuth.Invalid;
            }

            #endregion Bearing

            #region Speed

            // Do we have enough data for fix quality?
            if (wordCount >= 12 && words[11].Length != 0)
            {
                _speed = Speed.FromKilometersPerHour(double.Parse(words[11], NmeaCultureInfo));
            }
            else
            {
                _speed = Speed.Invalid;
            }

            #endregion Speed

            #region Position Dilution of Precision

            // Do we have enough data for fix quality?
            if (wordCount >= 13 && words[13].Length != 0)
            {
                _positionDop = new DilutionOfPrecision(float.Parse(words[13], NmeaCultureInfo));
            }
            else
            {
                _positionDop = DilutionOfPrecision.Invalid;
            }

            #endregion Position Dilution of Precision
        }

        #endregion Overrides

        #region IUtcDateTimeSentence Members

        /// <summary>
        /// Represents an NMEA sentence which contains date and time in UTC.
        /// </summary>
        public DateTime UtcDateTime
        {
            get { return _utcDateTime; }
        }

        #endregion IUtcDateTimeSentence Members

        #region IPositionSentence Members

        /// <summary>
        /// Represents an NMEA sentence which contains a position.
        /// </summary>
        public Position Position
        {
            get { return _position; }
        }

        #endregion IPositionSentence Members

        #region IFixModeSentence Members

        /// <summary>
        /// Gets the fix mode
        /// </summary>
        public FixMode FixMode
        {
            get { return _fixMode; }
        }

        #endregion IFixModeSentence Members

        #region IFixQualitySentence Members

        /// <summary>
        /// The Fix Quality
        /// </summary>
        /// <inheritdocs/>
        public FixQuality FixQuality
        {
            get { return _fixQuality; }
        }

        #endregion IFixQualitySentence Members

        #region ISpeedSentence Members

        /// <summary>
        /// The Speed
        /// </summary>
        /// <inheritdocs/>
        public Speed Speed
        {
            get { return _speed; }
        }

        #endregion ISpeedSentence Members

        #region IBearingSentence Members

        /// <summary>
        /// the Bearing
        /// </summary>
        /// <inheritdocs/>
        public Azimuth Bearing
        {
            get { return _bearing; }
        }

        #endregion IBearingSentence Members

        #region IPositionDilutionOfPrecisionSentence Members

        /// <summary>
        /// The Position Dilution of Precision (PDOP)
        /// </summary>
        /// <inheritdocs/>
        public DilutionOfPrecision PositionDilutionOfPrecision
        {
            get { return _positionDop; }
        }

        #endregion IPositionDilutionOfPrecisionSentence Members
    }
}