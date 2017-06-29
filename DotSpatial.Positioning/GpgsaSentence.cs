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
using System.Collections.Generic;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// $GPGSA
    /// GPS DOP and active satellites
    /// eg1. $GPGSA, A, 3, ,, ,, ,16, 18, ,22, 24, ,, 3.6, 2.1, 2.2*3C
    /// $GPGSA, A, 1, ,, ,, ,, ,, ,, ,, 6, 6, 6
    /// eg2. $GPGSA, A, 3, 19, 28, 14, 18, 27, 22, 31, 39, ,, ,, 1.7, 1.0, 1.3*35
    /// 1    = Method:
    /// M=Manual, forced to operate in 2D or 3D
    /// A=Automatic, 3D/2D
    /// 2    = Mode:
    /// 1=Fix not available
    /// 2=2D
    /// 3=3D
    /// 3-14 = IDs of SVs used in position fix (null for unused fields)
    /// 15   = PDOP
    /// 16   = HDOP
    /// 17   = VDOP
    /// </summary>
    public sealed class GpgsaSentence : NmeaSentence, IFixMethodSentence, IFixModeSentence, IFixedSatellitesSentence, IPositionDilutionOfPrecisionSentence,
                                    IHorizontalDilutionOfPrecisionSentence, IVerticalDilutionOfPrecisionSentence
    {
        /// <summary>
        ///
        /// </summary>
        private FixMethod _fixMethod;
        /// <summary>
        ///
        /// </summary>
        private FixMode _fixMode;
        /// <summary>
        ///
        /// </summary>
        private List<Satellite> _fixedSatellites;
        /// <summary>
        ///
        /// </summary>
        private DilutionOfPrecision _positionDop;
        /// <summary>
        ///
        /// </summary>
        private DilutionOfPrecision _horizontalDop;
        /// <summary>
        ///
        /// </summary>
        private DilutionOfPrecision _verticalDop;

        #region Constructors

        /// <summary>
        /// Creates a GpgsaSentence from the specified string
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GpgsaSentence(string sentence)
            : base(sentence)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgsaSentence"/> class.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="commandWord">The command word.</param>
        /// <param name="words">The words.</param>
        /// <param name="validChecksum">The valid checksum.</param>
        internal GpgsaSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        /// <summary>
        /// Creates a new GpgsaSentence
        /// </summary>
        /// <param name="fixMode">The fix mode.</param>
        /// <param name="fixMethod">The fix method.</param>
        /// <param name="satellites">The satellites.</param>
        /// <param name="positionDilutionOfPrecision">The position dilution of precision.</param>
        /// <param name="horizontalDilutionOfPrecision">The horizontal dilution of precision.</param>
        /// <param name="verticalDilutionOfPrecision">The vertical dilution of precision.</param>
        public GpgsaSentence(FixMode fixMode, FixMethod fixMethod, IEnumerable<Satellite> satellites,
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

            switch (_fixMode)
            {
                case FixMode.Automatic:
                    builder.Append("A");
                    break;
                default:
                    builder.Append("M");
                    break;
            }

            #endregion Append the fix method

            // Append a comma
            builder.Append(',');

            #region Append the fix method

            switch (_fixMethod)
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

            #endregion Append the fix method

            // Append a comma
            builder.Append(',');

            #region Fixed satellites

            /* A comma-delimited list of satellites involved in a fix.  Up to 12 satellites can be serialized.
             * This one concerns me, because while the limit is 12, ever more satellites are being launched.
             * Should we just serialize everything??
             */

            // Get a count of satellites to write, up to 123.  We'll scrub the list to ensure only fixed satellites are written
            int fixedSatellitesWritten = 0;
            foreach (Satellite item in satellites)
            {
                // Is it fixed?
                if (item.IsFixed)
                {
                    // Yes.  It cannot have babies
                    builder.Append(item.PseudorandomNumber.ToString(NmeaCultureInfo));

                    // Append a comma
                    builder.Append(",");

                    // Update the count
                    fixedSatellitesWritten++;

                    // If we're at 12, that's the limit.  Stop here
                    if (fixedSatellitesWritten == 12)
                        break;
                }
            }

            // If we wrote less than 12 satellites, write commas for the remainder
            for (int index = 0; index < 12 - fixedSatellitesWritten; index++)
                builder.Append(",");

            #endregion Fixed satellites

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

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Called when [sentence changed].
        /// </summary>
        protected override void OnSentenceChanged()
        {
            // First, process the basic info for the sentence
            base.OnSentenceChanged();

            // Cache the sentence words
            string[] words = Words;
            int wordCount = words.Length;

            #region Fix mode

            if (wordCount >= 1 && words[0].Length != 0)
            {
                switch (words[0])
                {
                    case "A":
                        _fixMode = FixMode.Automatic;
                        break;
                    case "M":
                        _fixMode = FixMode.Manual;
                        break;
                    default:
                        _fixMode = FixMode.Unknown;
                        break;
                }
            }
            else
            {
                _fixMode = FixMode.Unknown;
            }

            #endregion Fix mode

            #region Fix method

            // Get the fix quality information
            if (wordCount >= 2 && words[1].Length != 0)
            {
                switch (words[1])
                {
                    case "1":
                        _fixMethod = FixMethod.NoFix;
                        break;
                    case "2":
                        _fixMethod = FixMethod.Fix2D;
                        break;
                    case "3":
                        _fixMethod = FixMethod.Fix3D;
                        break;
                    default:
                        _fixMethod = FixMethod.Unknown;
                        break;
                }
            }
            else
            {
                _fixMethod = FixMethod.Unknown;
            }

            #endregion Fix method

            #region Fixed satellites

            if (wordCount >= 3)
            {
                // The sentence supports up to 12 satellites
                _fixedSatellites = new List<Satellite>(12);

                // Get each satellite PRN number
                int count = wordCount < 14 ? wordCount : 14;
                for (int index = 2; index < count; index++)
                    // Is the word empty?
                    if (words[index].Length != 0)
                        // No.  Add a satellite
                        _fixedSatellites.Add(
                            // We'll only have an empty object for now
                            new Satellite(int.Parse(words[index], NmeaCultureInfo)));
            }

            #endregion Fixed satellites

            #region Dilution of Precision

            // Set overall dilution of precision
            if (wordCount >= 15 && words[14].Length != 0)
                _positionDop = new DilutionOfPrecision(float.Parse(words[14], NmeaCultureInfo));
            else
                _positionDop = DilutionOfPrecision.Invalid;

            // Set horizontal dilution of precision
            if (wordCount >= 16 && words[15].Length != 0)
                _horizontalDop = new DilutionOfPrecision(float.Parse(words[15], NmeaCultureInfo));
            else
                _horizontalDop = DilutionOfPrecision.Invalid;

            // Set vertical dilution of precision
            if (wordCount >= 17 && words[16].Length != 0)
                _verticalDop = new DilutionOfPrecision(float.Parse(words[16], NmeaCultureInfo));
            else
                _verticalDop = DilutionOfPrecision.Invalid;

            #endregion Dilution of Precision
        }

        #endregion Overrides

        #region IFixedSatellitesSentence Members

        /// <summary>
        /// the list of FixedSatellites
        /// </summary>
        public IList<Satellite> FixedSatellites
        {
            get { return _fixedSatellites; }
        }

        #endregion IFixedSatellitesSentence Members

        #region IFixMethodSentence Members

        /// <summary>
        /// The Fix Method
        /// </summary>
        public FixMethod FixMethod
        {
            get { return _fixMethod; }
        }

        #endregion IFixMethodSentence Members

        #region IMeanDilutionOfPrecisionSentence Members

        /// <summary>
        /// The Position Dilution of Precision (PDOP)
        /// </summary>
        public DilutionOfPrecision PositionDilutionOfPrecision
        {
            get { return _positionDop; }
        }

        #endregion IMeanDilutionOfPrecisionSentence Members

        #region IVerticalDilutionOfPrecisionSentence Members

        /// <summary>
        /// The Vertical Dilution of Precision
        /// </summary>
        public DilutionOfPrecision VerticalDilutionOfPrecision
        {
            get { return _verticalDop; }
        }

        #endregion IVerticalDilutionOfPrecisionSentence Members

        #region IHorizontalDilutionOfPrecisionSentence Members

        /// <summary>
        /// The Horizontal Dilution of Precision
        /// </summary>
        public DilutionOfPrecision HorizontalDilutionOfPrecision
        {
            get { return _horizontalDop; }
        }

        #endregion IHorizontalDilutionOfPrecisionSentence Members

        #region IFixModeSentence Members

        /// <summary>
        /// Gets the fix mode
        /// </summary>
        public FixMode FixMode
        {
            get { return _fixMode; }
        }

        #endregion IFixModeSentence Members
    }
}