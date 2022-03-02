using System;
using System.Collections.Generic;
using System.Text;

namespace DotSpatial.Positioning.Gps.Nmea
{
    /// <summary>
    ///$GPGGA,hhmmss.ss,ddmm.mmmm,n,dddmm.mmmm,e,q,ss,y.y,a.a,z,g.g,z,t.t,iii*CC
    ///http://aprs.gids.nl/nmea/#gga
    ///Global Positioning System Fix Data. Time, location and fix related data for a GPS receiver. 
    ///  eg2. $--GGA,hhmmss.ss,llll.ll,a,yyyyy.yy,a,x,xx,x.x,x.x,M,x.x,M,x.x,xxxx 
    ///  hhmmss.ss = UTC of location 
    ///  llll.ll = latitude of location
    ///  a = N or S
    ///  yyyyy.yy = Longitude of location
    ///  a = E or W 
    ///  x = GPS Quality indicator (0=no fix, 1=GPS fix, 2=Dif. GPS fix) 
    ///  xx = number of satellites in use 
    ///  x.x = horizontal dilution of precision 
    ///  x.x = Antenna altitude above mean-sea-level
    ///  M = units of antenna altitude, meters 
    ///  x.x = Geoidal separation
    ///  M = units of geoidal separation, meters 
    ///  x.x = Age of Differential GPS data (seconds) 
    ///  xxxx = Differential reference station ID 
    ///  eg3. $GPGGA,hhmmss.ss,hhmm.mm,i,hhhmm.mm,i,f,ss,x.x,x.x,M,x.x,M,x.x,xxxx*hh
    ///  0    = Time in UTC    
    ///  1    = Latitude
    ///  2    = N or S
    ///  3    = Longitude
    ///  4    = E or W
    ///  5    = GPS quality indicator (0=invalid; 1=GPS fix; 2=Diff. GPS fix)
    ///  6    = Number of satellites in use [not those in view]
    ///  7    = Horizontal dilution of precision
    ///  8    = Antenna altitude above/below mean sea level (geoid)
    ///  9    = Meters  (Antenna height unit)
    ///  10   = Geoidal separation (Diff. between WGS-84 earth ellipsoid and
    ///         mean sea level.  -=geoid is below WGS-84 ellipsoid)
    ///  11   = Meters  (Units of geoidal separation)
    ///  12   = Age in seconds since last update from diff. reference station
    ///  13   = Diff. reference station ID#
    ///  14   = Checksum
    /// </summary>
    public sealed class GpggaSentence : NmeaSentence, IPositionSentence, IUtcTimeSentence, IAltitudeSentence,
                                        IGeoidalSeparationSentence, IDifferentialGpsSentence, IFixQualitySentence,
                                        IHorizontalDilutionOfPrecisionSentence, IFixedSatelliteCountSentence
    {
        private Position _Position;
        private TimeSpan _UtcTime;
        private FixQuality _FixQuality;
        private int _FixedSatelliteCount = -1;
        private Distance _Altitude;
        private Distance _GeoidalSeparation;
        private DilutionOfPrecision _HorizontalDilutionOfPrecision;
        private int _DifferentialGpsStationID;
        private TimeSpan _DifferentialGpsAge;

        #region Constructors

        public GpggaSentence(string sentence)
            : base(sentence)
        { }

        internal GpggaSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        public GpggaSentence(TimeSpan utcTime, Position position, FixQuality fixQuality, int trackedSatelliteCount,
                            DilutionOfPrecision horizontalDilutionOfPrecision, Distance altitude, Distance geoidalSeparation,
                            TimeSpan differentialGpsAge, int differentialGpsStationID)
        {        
            // Use a string builder to create the sentence text
            StringBuilder builder = new StringBuilder(128);

            #region Append the command word

            // Append the command word
            builder.Append("$GPGGA");

            #endregion

            // Append a comma
            builder.Append(',');

            #region Append the UTC time

            /* Convert UTC time to a string in the form HHMMSS.SSSS. Any value less than 10 will be
             * padded with a zero.
             */

            builder.Append(utcTime.Hours.ToString("0#", NmeaCultureInfo));
            builder.Append(utcTime.Minutes.ToString("0#", NmeaCultureInfo));
            builder.Append(utcTime.Seconds.ToString("0#", NmeaCultureInfo));
            builder.Append(".");
            builder.Append(utcTime.Milliseconds.ToString("00#", NmeaCultureInfo));

            #endregion

            // Append a comma
            builder.Append(',');

            #region Append the position

            // Append latitude in the format HHMM.MMMM. 
            builder.Append(position.Latitude.ToString("HHMM.MMMM,I,", NmeaCultureInfo));
            // Append Longitude in the format HHHMM.MMMM.
            builder.Append(position.Longitude.ToString("HHHMM.MMMM,I,", NmeaCultureInfo));

            #endregion

            #region Append fix quality

            switch (fixQuality)
            {
                case FixQuality.NoFix:
                    builder.Append("0");
                    break;
                case FixQuality.GpsFix:
                    builder.Append("1");
                    break;
                case FixQuality.DifferentialGpsFix:
                    builder.Append("2");
                    break;
                case FixQuality.PulsePerSecond:
                    builder.Append("3");
                    break;
                case FixQuality.FixedRealTimeKinematic:
                    builder.Append("4");
                    break;
                case FixQuality.FloatRealTimeKinematic:
                    builder.Append("5");
                    break;
                case FixQuality.Estimated:
                    builder.Append("6");
                    break;
                case FixQuality.ManualInput:
                    builder.Append("7");
                    break;
                case FixQuality.Simulated:
                    builder.Append("8");
                    break;              
            }

            #endregion

            // Append a comma
            builder.Append(",");

            // Append the tracked (signal strength is > 0) satellite count
            builder.Append(trackedSatelliteCount.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(",");

            // Append the numerical value of HDOP
            builder.Append(horizontalDilutionOfPrecision.Value.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(",");

            #region Altitude above sea level

            // Append the numerical value in meters
            builder.Append(altitude.ToMeters().Value.ToString(NmeaCultureInfo));

            // Append a comma, the unit (M = meters), and another comma
            builder.Append(",M,"); 

            #endregion

            #region Geoidal separation

            // Append the numerical value in meters
            builder.Append(geoidalSeparation.ToMeters().Value.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(",M,");

            #endregion

            #region Differential GPS information

            // Differnetial signal age in seconds
            if (!differentialGpsAge.Equals(TimeSpan.MinValue))
                builder.Append(differentialGpsAge.TotalSeconds.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(",");

            // Station ID
            if (differentialGpsStationID != -1)
                builder.Append(differentialGpsStationID.ToString(NmeaCultureInfo));

            #endregion

            // Set this object's sentence
            SetSentence(builder.ToString());

            // Finally, append the checksum
            AppendChecksum();
        }

        #endregion

        protected override void OnSentenceChanged()
        {
            // Parse the basic sentence information
            base.OnSentenceChanged();

            // Cache the words
            string[] words = base.Words;
            int wordCount = words.Length;

            // Do we have enough data to process the UTC time?
            if (wordCount >= 1 && words[0].Length != 0)
            {
                #region UTC Time

                string utcTimeWord = words[0];
                int utcHours = int.Parse(utcTimeWord.Substring(0, 2), NmeaCultureInfo);
                int utcMinutes = int.Parse(utcTimeWord.Substring(2, 2), NmeaCultureInfo);
                int utcSeconds = int.Parse(utcTimeWord.Substring(4, 2), NmeaCultureInfo);
                int utcMilliseconds = 0;
                if(utcTimeWord.Length > 6)
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

            // Do we have enough data for locations?
            if (wordCount >= 5 && words[1].Length != 0 && words[2].Length != 0 && words[3].Length != 0 && words[4].Length != 0)
            {
                #region Latitude

                string latitudeWord = words[1];
                int latitudeHours = int.Parse(latitudeWord.Substring(0, 2), NmeaCultureInfo);
                double latitudeDecimalMinutes = double.Parse(latitudeWord.Substring(2), NmeaCultureInfo);
                LatitudeHemisphere latitudeHemisphere =
                    words[2].Equals("N", StringComparison.OrdinalIgnoreCase) ? LatitudeHemisphere.North : LatitudeHemisphere.South;

                #endregion

                #region Longitude

                string longitudeWord = words[3];
                int longitudeHours = int.Parse(longitudeWord.Substring(0, 3), NmeaCultureInfo);
                double longitudeDecimalMinutes = double.Parse(longitudeWord.Substring(3), NmeaCultureInfo);
                LongitudeHemisphere longitudeHemisphere =
                    words[4].Equals("E", StringComparison.OrdinalIgnoreCase) ? LongitudeHemisphere.East : LongitudeHemisphere.West;

                #endregion

                #region Position

                _Position = new Position(
                                new Latitude(latitudeHours, latitudeDecimalMinutes, latitudeHemisphere),
                                new Longitude(longitudeHours, longitudeDecimalMinutes, longitudeHemisphere));

                #endregion
            }
            else
            {
                _Position = Position.Invalid;
            }

            // Do we have enough data for fix quality?
            if (wordCount >= 6 && words[5].Length != 0)
            {
                #region Fix Quality

                switch (int.Parse(words[5], NmeaCultureInfo))
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

                #endregion
            }
            else
            {
                // This fix quality is invalid
                _FixQuality = FixQuality.Unknown;
            }

            // Number of satellites in view is skipped.  We'll work off of GPGSV data.
            if (wordCount >= 7 && words[6].Length != 0)
            {
                _FixedSatelliteCount = int.Parse(words[6], NmeaCultureInfo);
            }

            // Is there enough information to process horizontal dilution of precision?
            if (wordCount >= 8 && words[7].Length != 0)
            {   
                #region Horizontal Dilution of Precision

                _HorizontalDilutionOfPrecision =
                    new DilutionOfPrecision(float.Parse(words[7], NmeaCultureInfo));
            
                #endregion
            }
            else
            {
                // The HDOP is invalid
                _HorizontalDilutionOfPrecision = DilutionOfPrecision.Invalid;
            }

            // Is there enough information to process altitude?
            if (wordCount >= 9 && words[8].Length != 0)
            {
                #region Altitude

                // Altitude is the 8th NMEA word
                _Altitude = new Distance(float.Parse(words[8], NmeaCultureInfo), DistanceUnit.Meters);

                #endregion
            }
            else
            {
                // The altitude is invalid
                _Altitude = Distance.Invalid;
            }

            // Is there enough information to process geoidal separation?
            if (wordCount >= 11 && words[10].Length != 0)
            {
                #region Geoidal Separation

                // Parse the geoidal separation
                _GeoidalSeparation = new Distance(float.Parse(words[10], NmeaCultureInfo), DistanceUnit.Meters);

                #endregion
            }
            else
            {
                // The geoidal separation is invalid
                _GeoidalSeparation = Distance.Invalid;
            }

            // Is there enough info to process Differential GPS info?
            if (wordCount >= 14 && words[12].Length != 0 && words[13].Length != 0)
            {
                #region Differential GPS information

                if (words[12].Length != 0)
                    _DifferentialGpsAge = TimeSpan.FromSeconds(float.Parse(words[12], NmeaCultureInfo));
                else
                    _DifferentialGpsAge = TimeSpan.MinValue;

                if (words[13].Length != 0)
                    _DifferentialGpsStationID = int.Parse(words[13], NmeaCultureInfo);
                else
                    _DifferentialGpsStationID = -1;

                #endregion
            }
            else
            {
                _DifferentialGpsStationID = -1;
                _DifferentialGpsAge = TimeSpan.MinValue;
            }
        }

        #region IPositionSentence Members

        public Position Position
        {
            get { return _Position; }
        }

        #endregion

        #region IUtcDatetimeSentence Members

        public TimeSpan UtcTime
        {
            get { return _UtcTime; }
        }

        #endregion

        #region IAltitudeSentence Members

        public Distance Altitude
        {
            get { return _Altitude; }
        }

        #endregion

        #region IHorizontalDilutionOfPrecisionSentence Members

        public DilutionOfPrecision HorizontalDilutionOfPrecision
        {
            get { return _HorizontalDilutionOfPrecision; }
        }

        #endregion

        #region IGeoidalSeparationSentence Members

        public Distance GeoidalSeparation
        {
            get { return _GeoidalSeparation; }
        }

        #endregion

        #region IFixQualitySentence Members

        public FixQuality FixQuality
        {
            get { return _FixQuality; }
        }

        #endregion

        #region IDifferentialGpsSentence Members

        public int DifferentialGpsStationID
        {
            get { throw new NotImplementedException(); }
        }

        public TimeSpan DifferentialGpsAge
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IFixedSatelliteCountSentence Members

        public int FixedSatelliteCount
        {
            get { return _FixedSatelliteCount; }
        }

        #endregion
    }
}
