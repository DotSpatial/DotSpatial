using System;
using System.Collections.Generic;
using System.Text;

namespace DotSpatial.Positioning.Gps.Nmea
{
    /// <summary>
    /// Track made good and ground speed sentence
    /// </summary>
    public sealed class GpvtgSentence : NmeaSentence, IBearingSentence, ISpeedSentence, IMagneticVariationSentence 
    {
        private Azimuth _Bearing;
        private Longitude _MagneticVariation;
        private Speed _Speed;
        
        #region Constructors

        /// <summary>
        /// Creates a track made good and ground speed sentence instance from the specified sentence
        /// </summary>
        /// <param name="sentence"></param>
        public GpvtgSentence(string sentence)
            : base(sentence)
        { }

        internal GpvtgSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        #endregion

        #region Overrides

        /// <summary>
        /// Overrides OnSentanceChanged for the GPVTGSentence
        /// </summary>
        protected override void OnSentenceChanged()
        {
            // First, process the basic info for the sentence
            base.OnSentenceChanged();

            // Cache the sentence words
            string[] words = base.Words;
            int wordCount = words.Length;

            /*
             * $GPVTG

                Track Made Good and Ground Speed.

                eg1. $GPVTG,360.0,T,348.7,M,000.0,N,000.0,K*43
                eg2. $GPVTG,054.7,T,034.4,M,005.5,N,010.2,K


                           054.7,T      True track made good
                           034.4,M      Magnetic track made good
                           005.5,N      Ground speed, knots
                           010.2,K      Ground speed, Kilometers per hour


                eg3. $GPVTG,t,T,,,s.ss,N,s.ss,K*hh
                1    = Track made good
                2    = Fixed text 'T' indicates that track made good is relative to true north
                3    = not used
                4    = not used
                5    = Speed over ground in knots
                6    = Fixed text 'N' indicates that speed over ground in in knots
                7    = Speed over ground in kilometers/hour
                8    = Fixed text 'K' indicates that speed over ground is in kilometers/hour

             * 
             * 
             */

            #region Bearing

            if (wordCount >= 1 && words[0].Length != 0)
                _Bearing = Azimuth.Parse(words[0], NmeaCultureInfo);
            else
                _Bearing = Azimuth.Invalid;

            #endregion

            #region Magnetic Variation

            if (wordCount >= 3 && words[2].Length != 0)
                _MagneticVariation = new Longitude(double.Parse(words[2], NmeaCultureInfo) - _Bearing.DecimalDegrees);
            else
                _MagneticVariation = Longitude.Invalid;

            #endregion

            #region Speed

            /* Speed is reported as both knots and KM/H.  We can parse either of the values.
             * First, try to parse knots.  If that fails, parse KM/h.
             */

            if (wordCount > 6 && words[5].Length != 0)
            {
                _Speed = new Speed(
                    // Parse the numeric portion
                    double.Parse(words[5], NmeaCultureInfo),
                    // Use knots 
                    SpeedUnit.Knots);
            }
            else if (wordCount > 8 && words[7].Length != 0)
            {
                _Speed = new Speed(
                    // Parse the numeric portion
                    double.Parse(words[7], NmeaCultureInfo),
                    // Use knots 
                    SpeedUnit.KilometersPerHour);
            }
            else
            {
                // Invalid speed
                _Speed = Speed.Invalid;
            }

            #endregion
        }

        #endregion

        #region IBearingSentence Members

        public Azimuth Bearing
        {
            get { return _Bearing; }
        }

        #endregion

        #region ISpeedSentence Members

        public Speed Speed
        {
            get { return _Speed; }
        }

        #endregion

        #region IMagneticVariationSentence Members

        public Longitude MagneticVariation
        {
            get { return _MagneticVariation; }
        }

        #endregion
    }
}
