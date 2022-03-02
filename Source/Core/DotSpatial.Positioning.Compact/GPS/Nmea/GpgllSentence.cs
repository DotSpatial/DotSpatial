using System;
using System.Collections.Generic;
using System.Text;


namespace DotSpatial.Positioning.Gps.Nmea
{
    /// <summary>
     /// $GPGLL
     ///Geographic Position, Latitude / Longitude and time. 
     ///http://aprs.gids.nl/nmea/#gll
     ///eg1. $GPGLL,3751.65,S,14507.36,E*77
     ///eg2. $GPGLL,4916.45,N,12311.12,W,225444,A
     ///            4916.46,N    Latitude 49 deg. 16.45 min. North
     ///            12311.12,W   Longitude 123 deg. 11.12 min. West
     ///            225444       Fix taken at 22:54:44 UTC
     ///            A            Data valid
     /// eg3. $GPGLL,5133.81,N,00042.25,W*75
     ///                1    2     3    4 5
     ///       1    5133.81   Current latitude
     ///       2    N         North/South
     ///       3    00042.25  Current longitude
     ///       4    W         East/West
     ///       5    *75       checksum
     /// $--GLL,lll.ll,a,yyyyy.yy,a,hhmmss.ss,A 
     /// llll.ll = Latitude of location 
     /// a = N or S 
     /// yyyyy.yy = Longitude of location 
     /// a = E or W 
     /// hhmmss.ss = UTC of location 
     /// A = status: A = valid data 
    /// </summary>
    public sealed class GpgllSentence : NmeaSentence, IUtcTimeSentence, IPositionSentence, IFixStatusSentence
    {
        private TimeSpan _UtcTime;
        private Position _Position;
        private FixStatus _FixStatus;

        #region Constructors

        internal GpgllSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        /// <summary>
        /// Creates a GpgllSentence from the specified string
        /// </summary>
        /// <param name="sentence"></param>
        public GpgllSentence(string sentence)
            : base(sentence)
        { }

        /// <summary>
        /// Creates a GpgllSentence from the specified parameters
        /// </summary>
        /// <param name="position"></param>
        /// <param name="utcTime"></param>
        /// <param name="fixStatus"></param>
        public GpgllSentence(Position position, TimeSpan utcTime, FixStatus fixStatus)
        {
            // Build a sentence
            StringBuilder builder = new StringBuilder(128);

            #region Append the command word

            // Append the command word
            builder.Append("$GPGLL");

            #endregion

            // Append a comma
            builder.Append(',');

            #region Append the position

            // Append latitude in the format HHMM.MMMM. 
            builder.Append(position.Latitude.ToString("HHMM.MMMM,I,", NmeaCultureInfo));
            // Append Longitude in the format HHHMM.MMMM.
            builder.Append(position.Longitude.ToString("HHHMM.MMMM,I,", NmeaCultureInfo));

            #endregion

            #region Append the UTC time

            builder.Append(utcTime.Hours.ToString("0#", NmeaCultureInfo));
            builder.Append(utcTime.Minutes.ToString("0#", NmeaCultureInfo));
            builder.Append(utcTime.Seconds.ToString("0#", NmeaCultureInfo));
            builder.Append(".");
            builder.Append(utcTime.Milliseconds.ToString("00#", NmeaCultureInfo));

            #endregion

            // Append a comma
            builder.Append(",");

            #region Append the fix status

            switch (fixStatus)
            {
                case FixStatus.Fix:
                    builder.Append("A");
                    break;
                default:
                    builder.Append("V");  // V = INvalid
                    break;
            }

            #endregion

            // Set this object's sentence
            SetSentence(builder.ToString());

            // Finally, append the checksum
            AppendChecksum();
        }


        #endregion

        #region Overrides

        protected override void OnSentenceChanged()
        {
            // Parse the basic sentence information
            base.OnSentenceChanged();

            // Cache the words
            string[] words = base.Words;
            int wordCount = words.Length;

            // Do we have enough words to make a lat/long location?
            if (wordCount >= 4 && words[0].Length != 0 && words[1].Length != 0 && words[2].Length != 0 && words[3].Length != 0)
            {
                #region Latitude

                string latitudeWord = words[0];
                int latitudeHours = int.Parse(latitudeWord.Substring(0, 2), NmeaCultureInfo);                            
                double latitudeDecimalMinutes = double.Parse(latitudeWord.Substring(2), NmeaCultureInfo);                
                LatitudeHemisphere latitudeHemisphere =
                    words[1].Equals("N", StringComparison.OrdinalIgnoreCase) ? LatitudeHemisphere.North : LatitudeHemisphere.South;         

                #endregion

                #region Longitude

                string longitudeWord = words[2];
                int longitudeHours = int.Parse(longitudeWord.Substring(0, 3), NmeaCultureInfo);
                double longitudeDecimalMinutes = double.Parse(longitudeWord.Substring(3), NmeaCultureInfo);                
                LongitudeHemisphere longitudeHemisphere =
                    words[3].Equals("E", StringComparison.OrdinalIgnoreCase) ? LongitudeHemisphere.East : LongitudeHemisphere.West;           

                #endregion

                #region Position

                _Position = new Position(
                                new Latitude(latitudeHours, latitudeDecimalMinutes, latitudeHemisphere),
                                new Longitude(longitudeHours, longitudeDecimalMinutes, longitudeHemisphere));

                #endregion
            }


            // Do we have enough data to process the UTC time?
            if (wordCount >= 5 && words[4].Length != 0)
            {
                #region UTC Time

                string utcTimeWord = words[4];
                int utcHours = int.Parse(utcTimeWord.Substring(0, 2), NmeaCultureInfo);
                int utcMinutes = int.Parse(utcTimeWord.Substring(2, 2), NmeaCultureInfo);
                int utcSeconds = int.Parse(utcTimeWord.Substring(4, 2), NmeaCultureInfo);
                int utcMilliseconds = 0;
                if (utcTimeWord.Length > 6)
                    utcMilliseconds = Convert.ToInt32(float.Parse(utcTimeWord.Substring(6), NmeaCultureInfo) * 1000, NmeaCultureInfo);

                // Build a TimeSpan for this value
                _UtcTime = new TimeSpan(0, utcHours, utcMinutes, utcSeconds, utcMilliseconds);

                #endregion
            }
            else
            {
                // The UTC time is invalid
                _UtcTime = TimeSpan.MinValue;
            }


            // Do we have enough data to get the fix status?
            if (wordCount >= 6 && words[5].Length != 0)
            {
                #region Fix Status

                // An "A" means a valid fix
                _FixStatus = words[5].Equals("A", StringComparison.OrdinalIgnoreCase) ? FixStatus.Fix : FixStatus.NoFix;

                #endregion
            }
            else
            {
                // The fix status is invalid
                _FixStatus = FixStatus.Unknown;
            }
        }

        #endregion

        #region IUtcDateTimeSentence Members

        public TimeSpan UtcTime
        {
            get { return _UtcTime; }
        }

        #endregion

        #region IPositionSentence Members

        public Position Position
        {
            get { return _Position; }
        }

        #endregion

        #region IFixStatusSentence Members

        public FixStatus FixStatus
        {
            get { return _FixStatus; }
        }

        #endregion
    }
}
