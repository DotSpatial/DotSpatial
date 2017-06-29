
using System;
using System.Collections.Generic;
using System.Text;

namespace DotSpatial.Positioning.Gps.Nmea
{
    /// <summary>
    /// Represents a Garmin $PGRMF sentence.
    /// </summary>
    public sealed class PgrmfSentence : NmeaSentence, IUtcDateTimeSentence, IPositionSentence, IFixModeSentence,
        IFixQualitySentence, ISpeedSentence, IBearingSentence, IPositionDilutionOfPrecisionSentence 
    {
        private DateTime _UtcDateTime;
        private Position _Position;
        private FixMode _FixMode;
        private FixQuality _FixQuality;
        private Speed _Speed;
        private Azimuth _Bearing;
        private DilutionOfPrecision _PositionDOP;

        #region Constructors

        /// <summary>
        /// Creates a Garmin $PGRMF sentence from the specified parameters
        /// </summary>
        internal PgrmfSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        /// <summary>
        /// Creates a Garmin $PGRMF sentence from the specified string
        /// </summary>
        public PgrmfSentence(string sentence)
            : base(sentence)
        { }

        #endregion

        #region Overrides

        /// <summary>
        /// OnSentanceChanged event handler
        /// </summary>
        protected override void OnSentenceChanged()
        {
            base.OnSentenceChanged();

            // Cache the sentence words
            string[] words = base.Words;
            int wordCount = words.Length;

            /*
             *  Garmin produces several embedded GPS systems. They are easy to setup because Garmin provides a nice utility for uploading configuration data to the GPS. You first load the utility to a PC. Connect the PC to the GPS through one of the serial ports. The utility will check each baud rate until it communicates with the GPS.

                The common configuration parameters are output sentences from the GPS unit, the communication baud rate with a host, and the required pulse per second.

                Each sentence is preceded with a ‘$’ symbol and ends with a line-feed character. At one sentence per second, the following is out put in four seconds:

                $PGRMF,223,424798,041203,215945,13,0000.0000,N,00000.0000,W,A,2,0,62,2,1*3B
                $PGRMF,223,424799,041203,215946,13,00000.0000,N,00000.0000,W,A,2,0,62,2,1*39
                $PGRMF,223,424800,041203,215947,13,00000.0000,N,00000.0000,W,A,2,0,62,2,1*34
                $PGRMF,223,424801,041203,215948,13,00000.0000,N,00000.0000,W,A,2,0,62,2,1*35

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
                _UtcDateTime = new DateTime(
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

                #endregion

                #region Parse the longitude

                string longitudeWord = words[7];
                int longitudeHours = int.Parse(longitudeWord.Substring(0, 3), NmeaCultureInfo);
                double longitudeDecimalMinutes = double.Parse(longitudeWord.Substring(3), NmeaCultureInfo);
                LongitudeHemisphere longitudeHemisphere =
                    words[8].Equals("E", StringComparison.Ordinal) ? LongitudeHemisphere.East : LongitudeHemisphere.West;

                #endregion

                #region Build a Position from the latitude and longitude

                _Position = new Position(
                                new Latitude(latitudeHours, latitudeDecimalMinutes, latitudeHemisphere),
                                new Longitude(longitudeHours, longitudeDecimalMinutes, longitudeHemisphere));

                #endregion
            }
            else
            {
                _Position = Position.Invalid;
            }

            #endregion

            #region Fix Mode

            if (wordCount >= 9 && words[9].Length != 0)
            {
                if (words[9] == "A")
                    _FixMode = FixMode.Automatic;
                else
                    _FixMode = FixMode.Manual;
            }
            else
            {
                _FixMode = FixMode.Unknown;
            }

            #endregion

            #region Fix Quality

            // Do we have enough data for fix quality?
            if (wordCount >= 10 && words[10].Length != 0)
            {
                switch (int.Parse(words[10], NmeaCultureInfo))
                {
                    case 0:
                        _FixQuality = FixQuality.NoFix;
                        break;
                    case 1:
                        _FixQuality = FixQuality.GpsFix;
                        break;
                    case 2:
                        _FixQuality = FixQuality.DifferentialGpsFix;
                        break;
                    case 3:
                        _FixQuality = FixQuality.PulsePerSecond;
                        break;
                    case 4:
                        _FixQuality = FixQuality.FixedRealTimeKinematic;
                        break;
                    case 5:
                        _FixQuality = FixQuality.FloatRealTimeKinematic;
                        break;
                    case 6:
                        _FixQuality = FixQuality.Estimated;
                        break;
                    case 7:
                        _FixQuality = FixQuality.ManualInput;
                        break;
                    case 8:
                        _FixQuality = FixQuality.Simulated;
                        break;
                    default:
                        _FixQuality = FixQuality.Unknown;
                        break;
                }
            }
            else
            {
                // This fix quality is invalid
                _FixQuality = FixQuality.Unknown;
            }

            #endregion

            #region Bearing

            // Do we have enough data for fix quality?
            if (wordCount >= 13 && words[12].Length != 0)
            {
                _Bearing = new Azimuth(words[12], NmeaCultureInfo);
            }
            else
            {
                _Bearing = Azimuth.Invalid;
            }

            #endregion

            #region Speed

            // Do we have enough data for fix quality?
            if (wordCount >= 12 && words[11].Length != 0)
            {
                _Speed = Speed.FromKilometersPerHour(double.Parse(words[11], NmeaCultureInfo));
            }
            else
            {
                _Speed = Speed.Invalid;
            }

            #endregion

            #region Position Dilution of Precision

            // Do we have enough data for fix quality?
            if (wordCount >= 13 && words[13].Length != 0)
            {
                _PositionDOP = new DilutionOfPrecision(float.Parse(words[13], NmeaCultureInfo));
            }
            else
            {
                _PositionDOP = DilutionOfPrecision.Invalid;
            }


            #endregion
        }

        #endregion

        #region IUtcDateTimeSentence Members

        public DateTime UtcDateTime
        {
            get { return _UtcDateTime; }
        }

        #endregion

        #region IPositionSentence Members

        public Position Position
        {
            get { return _Position; }
        }

        #endregion

        #region IFixModeSentence Members

        public FixMode FixMode
        {
            get { return _FixMode; }
        }

        #endregion

        #region IFixQualitySentence Members

        public FixQuality FixQuality
        {
            get { return _FixQuality; }
        }

        #endregion

        #region ISpeedSentence Members

        public Speed Speed
        {
            get { return _Speed; }
        }

        #endregion

        #region IBearingSentence Members

        public Azimuth Bearing
        {
            get { return _Bearing; }
        }

        #endregion

        #region IPositionDilutionOfPrecisionSentence Members

        public DilutionOfPrecision PositionDilutionOfPrecision
        {
            get { return _PositionDOP; }
        }

        #endregion
    }
}
