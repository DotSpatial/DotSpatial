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

        #endregion Constructors

        /// <summary>
        /// Corrects this classes properties after the base sentence was changed.
        /// </summary>
        new void SetPropertiesFromSentence()
        {
            UtcDateTime = ParseUtcDateTime(0, 1);
            Position = ParsePosition(2, 3, 4, 5);
            FixQuality = ParseFixQuality(7);
            PositionDilutionOfPrecision = ParseDilution(9);             // Process the mean DOP
            AltitudeAboveEllipsoid = ParseDistance(10, DistanceUnit.Meters).ToLocalUnitType();
        }

        #region Properties

        /// <summary>
        /// Represents an NMEA sentence which contains a position.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// The Fix Quality
        /// </summary>
        public FixQuality FixQuality { get; private set; }

        /// <summary>
        /// The Position Dilution of Precision (PDOP)
        /// </summary>
        public DilutionOfPrecision PositionDilutionOfPrecision { get; private set; }

        /// <summary>
        /// The Altitude Above Ellipsoid
        /// </summary>
        public Distance AltitudeAboveEllipsoid { get; private set; }

        /// <summary>
        /// Represents an NMEA sentence which contains date and time in UTC.
        /// </summary>
        public DateTime UtcDateTime { get; private set; }

        #endregion
    }
}