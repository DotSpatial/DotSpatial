using System;
using System.Collections.Generic;
using System.Text;



namespace DotSpatial.Positioning.Gps.Nmea
{
    ///<summary>
    ///$GPGSA
    /// GPS DOP and active satellites
    /// eg1. $GPGSA,A,3,,,,,,16,18,,22,24,,,3.6,2.1,2.2*3C
    ///      $GPGSA,A,1,,,,,,,,,,,,,6,6,6
    /// eg2. $GPGSA,A,3,19,28,14,18,27,22,31,39,,,,,1.7,1.0,1.3*35
    /// 1    = Method:
    ///        M=Manual, forced to operate in 2D or 3D
    ///        A=Automatic, 3D/2D
    /// 2    = Mode:
    ///        1=Fix not available
    ///        2=2D
    ///        3=3D
    /// 3-14 = IDs of SVs used in position fix (null for unused fields)
    /// 15   = PDOP
    /// 16   = HDOP
    /// 17   = VDOP
    /// </summary>
    public sealed class GpgsaSentence : NmeaSentence, IFixMethodSentence, IFixModeSentence, IFixedSatellitesSentence, IPositionDilutionOfPrecisionSentence,
                                        IHorizontalDilutionOfPrecisionSentence, IVerticalDilutionOfPrecisionSentence 
    {
        private FixMethod _FixMethod;
        private FixMode _FixMode;
        private List<Satellite> _FixedSatellites;
        private DilutionOfPrecision _PositionDop;
        private DilutionOfPrecision _HorizontalDop;
        private DilutionOfPrecision _VerticalDop;
                
        #region Constructors

        /// <summary>
        /// Creates a GpgsaSentence from the specified string
        /// </summary>
        /// <param name="sentence"></param>
        public GpgsaSentence(string sentence)
            : base(sentence)
        { }

        internal GpgsaSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        public GpgsaSentence(FixMode fixMode, FixMethod fixMethod, IList<Satellite> satellites,
            DilutionOfPrecision positionDilutionOfPrecision,
            DilutionOfPrecision horizontalDilutionOfPrecision,
            DilutionOfPrecision verticalDilutionOfPrecision)
        {
            // Use a string builder to create the sentence text
            StringBuilder builder = new StringBuilder(128);

            // Append the command word
            builder.Append("$GPGSA");

            // Append a comma
            builder.Append(',');

            #region Append the fix method

            switch (_FixMode)
            {
                case FixMode.Automatic:
                    builder.Append("A");
                    break;
                default:
                    builder.Append("M");
                    break;
            }

            #endregion

            // Append a comma
            builder.Append(',');
        
            #region Append the fix method

            switch (_FixMethod)
            {
                case FixMethod.Fix2D:
                    builder.Append("2");
                    break;
                case FixMethod.Fix3D:
                    builder.Append("3");
                    break;
                default:
                    builder.Append("1");
                    break;
            }

            #endregion

            // Append a comma
            builder.Append(',');

            #region Fixed satellites

            /* A comma-delimited list of satellites involved in a fix.  Up to 12 satellites can be serialized.
             * This one concerns me, because while the limit is 12, ever more satellites are being launched.
             * Should we just serialize everything??
             */

            // Get a count of satellites to write, up to 123.  We'll scrub the list to ensure only fixed satellites are written
            int fixedSatellitesWritten = 0;
            for(int index = 0; index < satellites.Count; index++)
            {
                // Get the satellite
                Satellite item = satellites[index];

                // Is it fixed?
                if(item.IsFixed)
                {
                    // Yes.  It cannot have babies
                    builder.Append(item.PseudorandomNumber.ToString(NmeaCultureInfo));

                    // Append a comma
                    builder.Append(",");

                    // Update the count
                    fixedSatellitesWritten++;

                    // If we're at 12, that's the limit.  Stop here
                    if(fixedSatellitesWritten == 12)
                        break;
                }
            }

            // If we wrote less than 12 satellites, write commas for the remainder
            for(int index = 0; index < 12 - fixedSatellitesWritten; index++)
                builder.Append(",");

            #endregion

            // NOTE: Commas have been written at this point

            // Position Dilution of Precision
            builder.Append(positionDilutionOfPrecision.Value.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(",");

            // Horizontal Dilution of Precision
            builder.Append(horizontalDilutionOfPrecision.Value.ToString(NmeaCultureInfo));

            // Append a comma
            builder.Append(",");

            // Vertical Dilution of Precision
            builder.Append(verticalDilutionOfPrecision.Value.ToString(NmeaCultureInfo));

            // Set this object's sentence
            SetSentence(builder.ToString());

            // Finally, append the checksum
            AppendChecksum();
        }

        #endregion

        #region Overrides

        protected override void OnSentenceChanged()
        {
            // First, process the basic info for the sentence
            base.OnSentenceChanged();

            // Cache the sentence words
            string[] words = base.Words;
            int wordCount = words.Length;

            #region Fix mode

            if (wordCount >= 1 && words[0].Length != 0)
            {
                switch (words[0])
                {
                    case "A":
                        _FixMode = FixMode.Automatic;
                        break;
                    case "M":
                        _FixMode = FixMode.Manual;
                        break;
                    default:
                        _FixMode = FixMode.Unknown;
                        break;
                }
            }
            else
            {
                _FixMode = FixMode.Unknown;
            }

            #endregion

            #region Fix method

            // Get the fix quality information
            if (wordCount >= 2 && words[1].Length != 0)
            {
                switch (words[1])
                {
                    case "1":
                        _FixMethod = FixMethod.NoFix;
                        break;
                    case "2":
                        _FixMethod = FixMethod.Fix2D;
                        break;
                    case "3":
                        _FixMethod = FixMethod.Fix3D;
                        break;
                    default:
                        _FixMethod = FixMethod.Unknown;
                        break;
                }
            }
            else
            {
                _FixMethod = FixMethod.Unknown;
            }

            #endregion

            #region Fixed satellites

            if (wordCount >= 3)
            {
                // The sentence supports up to 12 satellites
                _FixedSatellites = new List<Satellite>(12);

                // Get each satellite PRN number
                int count = wordCount < 14 ? wordCount : 14;
                for (int index = 2; index < count; index++)
                    // Is the word empty?
                    if (words[index].Length != 0)
                        // No.  Add a satellite
                        _FixedSatellites.Add(
                            // We'll only have an empty object for now
                            new Satellite(int.Parse(words[index], NmeaCultureInfo)));
            }

            #endregion

            #region Dilution of Precision

            // Set overall dilution of precision
            if (wordCount >= 15 && words[14].Length != 0)
                _PositionDop = new DilutionOfPrecision(float.Parse(words[14], NmeaCultureInfo));
            else
                _PositionDop = DilutionOfPrecision.Invalid;

            // Set horizontal dilution of precision
            if (wordCount >= 16 && words[15].Length != 0)
                _HorizontalDop = new DilutionOfPrecision(float.Parse(words[15], NmeaCultureInfo));
            else
                _HorizontalDop = DilutionOfPrecision.Invalid;

            // Set vertical dilution of precision
            if (wordCount >= 17 && words[16].Length != 0)
                _VerticalDop = new DilutionOfPrecision(float.Parse(words[16], NmeaCultureInfo));
            else
                _VerticalDop = DilutionOfPrecision.Invalid;

            #endregion
        }

        #endregion

        #region IFixedSatellitesSentence Members

        public IList<Satellite> FixedSatellites
        {
            get { return _FixedSatellites; }
        }

        #endregion

        #region IFixMethodSentence Members

        public FixMethod FixMethod
        {
            get { return _FixMethod; }
        }

        #endregion

        #region IMeanDilutionOfPrecisionSentence Members

        public DilutionOfPrecision PositionDilutionOfPrecision
        {
            get { return _PositionDop; }
        }

        #endregion

        #region IVerticalDilutionOfPrecisionSentence Members

        public DilutionOfPrecision VerticalDilutionOfPrecision
        {
            get { return _VerticalDop; }
        }

        #endregion

        #region IHorizontalDilutionOfPrecisionSentence Members

        public DilutionOfPrecision HorizontalDilutionOfPrecision
        {
            get { return _HorizontalDop; }
        }

        #endregion

        #region IFixModeSentence Members

        public FixMode FixMode
        {
            get { return _FixMode; }
        }

        #endregion
    }
}
