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

namespace DotSpatial.Positioning
{
    /// <summary>
    /// $--GGK    Header including Talker ID
    /// hhmmss.ss UTC time of location
    /// mmddyy    UTC date
    /// llll.ll   Latitude
    /// a         Hemisphere (N or S)
    /// yyyyy.yy  Longitude
    /// a         (E or W)
    /// x         GPS quality indicator
    ///   0 = Fix not available or invalid
    ///   1 = No real-time location, navigation fix
    ///   2 = Real-time location, ambiguities not fixed
    ///   3 = Real-time location, ambiguities fixed
    /// xx        Number of satellites in use, 00 to 12.
    /// x.x       Geometric dilution of precision (GDOP)
    /// EHT       Ellipsoidal height
    /// x.x       Altitude of location marker as local ellipsoidal height. If the local ellipsoidal
    ///           height is not available, the WGS 1984 ellipsoidal height will be exported.
    /// </summary>
    public sealed class GpggkSentence : NmeaSentence, IPositionSentence, IUtcDateTimeSentence, IFixQualitySentence, IPositionDilutionOfPrecisionSentence,
                                        IAltitudeAboveEllipsoidSentence
    {
        #region Constructors

        /// <summary>
        /// Creates a GpggkSentence from the specified string
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GpggkSentence(string sentence)
            : base(sentence)
        {
            SetPropertiesFromSentence();
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
        {
            SetPropertiesFromSentence();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The Altitude Above Ellipsoid
        /// </summary>
        public Distance AltitudeAboveEllipsoid { get; private set; }

        /// <summary>
        /// The Fix Quality
        /// </summary>
        public FixQuality FixQuality { get; private set; }

        /// <summary>
        /// Represents an NMEA sentence which contains a position.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// The Position Dilution of Precision (PDOP)
        /// </summary>
        public DilutionOfPrecision PositionDilutionOfPrecision { get; private set; }

        /// <summary>
        /// Indicates how many satellites are used.
        /// </summary>
        public int SatellitesInUse { get; private set; }

        /// <summary>
        /// Represents an NMEA sentence which contains date and time in UTC.
        /// </summary>
        public DateTime UtcDateTime { get; private set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses the word at the given position to Distance.
        /// </summary>
        /// <param name="position">Position of the Words array that gets parsed to Distance.</param>
        /// <param name="unit">DistanceUnit that is used for parsing.</param>
        /// <returns>Invalid if position is outside of Words array or word at the position is empty.</returns>
        private new Distance ParseDistance(int position, DistanceUnit unit)
        {
            string word = Words[position];
            Words[position] = Words[position].Replace("EHT", "");
            var retval = base.ParseDistance(position, unit);             // Process the mean DOP
            Words[position] = word;
            return retval;
        }

        /// <summary>
        /// Corrects this classes properties after the base sentence was changed.
        /// </summary>
        private new void SetPropertiesFromSentence()
        {
            UtcDateTime = ParseUtcDateTime(0, 1);
            Position = ParsePosition(2, 3, 4, 5);
            FixQuality = ParseFixQuality(6);
            PositionDilutionOfPrecision = ParseDilution(8);
            AltitudeAboveEllipsoid = ParseDistance(9, DistanceUnit.Meters).ToLocalUnitType();

            if (Words.Length > 7 && Words[7].Length != 0)
                SatellitesInUse = int.Parse(Words[7], NmeaCultureInfo);
        }

        #endregion
    }
}