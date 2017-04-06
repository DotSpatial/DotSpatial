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
    /// Represents a Garmin $PGRMF sentence.
    /// </summary>
    public sealed class PgrmfSentence : NmeaSentence, IUtcDateTimeSentence, IPositionSentence, IFixModeSentence,
                                        IFixMethodSentence, ISpeedSentence, IBearingSentence, IPositionDilutionOfPrecisionSentence
    {
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
        { SetPropertiesFromSentence(); }

        /// <summary>
        /// Creates a Garmin $PGRMF sentence from the specified string
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public PgrmfSentence(string sentence)
            : base(sentence)
        { SetPropertiesFromSentence(); }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// OnSentenceChanged event handler
        /// </summary>
        private new void SetPropertiesFromSentence()
        {
           /*
                Garmin produces several embedded GPS systems. They are easy to setup because Garmin provides a nice utility for uploading configuration data to the GPS. 
                You first load the utility to a PC. Connect the PC to the GPS through one of the serial ports. The utility will check each baud rate until it communicates with the GPS.
                The common configuration parameters are output sentences from the GPS unit, the communication baud rate with a host, and the required pulse per second.
                Each sentence is preceded with a ‘$’ symbol and ends with a line-feed character. At one sentence per second, the following is out put in four seconds:

                $PGRMF, 223, 424798, 041203, 215945, 13, 0000.0000, N, 00000.0000, W, A, 2, 0, 62, 2, 1*3B
                $PGRMF, 223, 424799, 041203, 215946, 13, 0000.0000, N, 00000.0000, W, A, 2, 0, 62, 2, 1*39
                $PGRMF, 223, 424800, 041203, 215947, 13, 0000.0000, N, 00000.0000, W, A, 2, 0, 62, 2, 1*34
                $PGRMF, 223, 424801, 041203, 215948, 13, 0000.0000, N, 00000.0000, W, A, 2, 0, 62, 2, 1*35

                The sentence is proprietary to the Garmin GPS Global Positioning System and is translated below.

                $PGRMF
                <0>  GPS week number (0 - 1023) 
                <1>  GPS seconds (0 - 604799) 
                <2>  UTC date of position fix, ddmmyy format 
                <3>  UTC time of position fix, hhmmss format 
                <4>  GPS leap second count 
                <5>  Latitude, ddmm.mmmm format (leading zeros will be transmitted) 
                <6>  Latitude hemisphere, N or S 
                <7>  Longitude, dddmm.mmmm format (leading zeros will be transmitted) 
                <8>  Longitude hemisphere, E or W 
                <9>  Mode, M = manual, A = automatic 
                <10> Fix type, 0 = no fix, 1 = 2D fix, 2 = 3D fix 
                <11> Speed over ground, 0 to 1051 kilometers/hour 
                <12> Course over ground, 0 to 359 degrees, true 
                <13> Position dilution of precision, 0 to 9 (rounded to nearest integer value) 
                <14> Time dilution of precision, 0 to 9 (rounded to nearest integer value) 
                *hh <CR><LF>
             */

            // TODO: Convert GPS week number/seconds to UTC date/time

            UtcDateTime = ParseUtcDateTime(3, 2);
            Position = ParsePosition(5, 6, 7, 8);
            FixMode = ParseFixMode(9);
            FixMethod = ParseFixMethod(10, false);
            Bearing = ParseAzimuth(12);
            Speed = ParseSpeed(11, SpeedUnit.KilometersPerHour);
            PositionDilutionOfPrecision = ParseDilution(13);
        }

        #endregion Overrides

        #region Properties

        /// <summary>
        /// Represents an NMEA sentence which contains date and time in UTC.
        /// </summary>
        public DateTime UtcDateTime { get; private set; }

        /// <summary>
        /// Represents an NMEA sentence which contains a position.
        /// </summary>
        public Position Position { get; private set; }

        /// <summary>
        /// Gets the fix mode
        /// </summary>
        public FixMode FixMode { get; private set; }

        /// <summary>
        /// The Fix Method.
        /// </summary>
        /// <inheritdocs/>
        public FixMethod FixMethod { get; private set; }

        /// <summary>
        /// The Speed
        /// </summary>
        /// <inheritdocs/>
        public Speed Speed { get; private set; }

        /// <summary>
        /// the Bearing
        /// </summary>
        /// <inheritdocs/>
        public Azimuth Bearing { get; private set; }

        /// <summary>
        /// The Position Dilution of Precision (PDOP)
        /// </summary>
        /// <inheritdocs/>
        public DilutionOfPrecision PositionDilutionOfPrecision { get; private set; }

        #endregion
    }
}