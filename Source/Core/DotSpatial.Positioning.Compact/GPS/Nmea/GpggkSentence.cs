using System;
using System.Collections.Generic;
using System.Text;

namespace DotSpatial.Positioning.Gps.Nmea
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
        private DateTime _UtcDateTime;
        private Position _Position;
        private FixQuality _FixQuality;
        private int _FixedSatelliteCount;
        private DilutionOfPrecision _MeanDilutionOfPrecision;
        private Distance _AltitudeAboveEllipsoid;

        #region Constructors

        /// <summary>
        /// Creates a GpggkSentence from the specified string
        /// </summary>
        /// <param name="sentence"></param>
        public GpggkSentence(string sentence)
            : base(sentence)
        {
        }

        internal GpggkSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        #endregion

        protected override void OnSentenceChanged()
        {
            // Process the basic sentence elements
            base.OnSentenceChanged();

            // Cache the word array
            string[] words = base.Words;
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

                #endregion

                #region Parse the UTC date

                string utcDateWord = words[1];
                int utcMonth = int.Parse(utcDateWord.Substring(0, 2), NmeaCultureInfo);
                int utcDay = int.Parse(utcDateWord.Substring(2, 2), NmeaCultureInfo);
                int utcYear = int.Parse(utcDateWord.Substring(4, 2), NmeaCultureInfo) + 2000;

                #endregion

                #region Build a UTC date/time

                _UtcDateTime = new DateTime(utcYear, utcMonth, utcDay, utcHours, utcMinutes, utcSeconds, utcMilliseconds, DateTimeKind.Utc);

                #endregion
            }
            else
            {
                // The UTC date/time is invalid
                _UtcDateTime = DateTime.MinValue;
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

                #endregion

                #region Parse the longitude

                string longitudeWord = words[4];
                int longitudeHours = int.Parse(longitudeWord.Substring(0, 3), NmeaCultureInfo);
                double longitudeDecimalMinutes = double.Parse(longitudeWord.Substring(3), NmeaCultureInfo);
                LongitudeHemisphere longitudeHemisphere =
                    words[5].Equals("E", StringComparison.OrdinalIgnoreCase) ? LongitudeHemisphere.East : LongitudeHemisphere.West;

                #endregion

                #region Build a Position from the latitude and longitude

                _Position = new Position(
                                new Latitude(latitudeHours, latitudeDecimalMinutes, latitudeHemisphere),
                                new Longitude(longitudeHours, longitudeDecimalMinutes, longitudeHemisphere));

                #endregion
            }

            // Do we have enough data for the fix quality?
            if (wordCount > 7 && words[7].Length != 0)
            {
                switch (Convert.ToInt32(words[7], NmeaCultureInfo))
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
                // The fix quality is invalid
                _FixQuality = FixQuality.Unknown;
            }

            // Process the fixed satellite count
            if (wordCount > 8)
                _FixedSatelliteCount = int.Parse(Words[8], NmeaCultureInfo);
            else
                _FixedSatelliteCount = 0;

            // Process the mean DOP
            if (wordCount > 9 && Words[9].Length != 0)
                _MeanDilutionOfPrecision = new DilutionOfPrecision(float.Parse(Words[9], NmeaCultureInfo));
            else
                _MeanDilutionOfPrecision = DilutionOfPrecision.Maximum;

            // Altitude above ellipsoid
            if (wordCount > 10 && Words[10].Length != 0)
                _AltitudeAboveEllipsoid = new Distance(double.Parse(Words[10], NmeaCultureInfo), DistanceUnit.Meters).ToLocalUnitType();
            else
                _AltitudeAboveEllipsoid = Distance.Empty;
        }

        #region IPositionSentence Members

        public Position Position
        {
            get { return _Position; }
        }

        #endregion

        #region IFixQualitySentence Members

        public FixQuality FixQuality
        {
            get { return _FixQuality; }
        }

        #endregion

        #region IMeanDilutionOfPrecisionSentence Members

        public DilutionOfPrecision PositionDilutionOfPrecision
        {
            get { return _MeanDilutionOfPrecision; }
        }

        #endregion

        #region IAltitudeAboveEllipsoidSentence Members

        public Distance AltitudeAboveEllipsoid
        {
            get { return _AltitudeAboveEllipsoid; }
        }

        #endregion

        #region IUtcDateTimeSentence Members

        public DateTime UtcDateTime
        {
            get { return _UtcDateTime; }
        }

        #endregion
    }
}
