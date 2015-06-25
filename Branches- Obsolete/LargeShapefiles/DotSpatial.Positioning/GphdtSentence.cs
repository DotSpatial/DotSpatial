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
// --------------------------------------------------------------------------------------------------------
// |    Developer                |    Date    |                             Comments
// |-----------------------------|------------|------------------------------------------------------------
// | Tidyup  (Ben Tombs)         | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford)    | 10/22/2010 | Added file headers reviewed formatting with resharper.
// | VladimirArias (Colombia)	 | 02/03/2014 | Added hdt nmea sentence for heading orientation
// ********************************************************************************************************
namespace DotSpatial.Positioning
{
    /// <summary>
    /// Heading, True
    /// </summary>
    public sealed class GphdtSentence : NmeaSentence, IHeadingSentence
    {
        /// <summary>
        ///
        /// </summary>
        private Azimuth _heading;

        #region Constructors

        /// <summary>
        /// Creates a heading sentence instance from the specified sentence
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GphdtSentence(string sentence)
            : base(sentence)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GphdtSentence"/> class.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="commandWord">The command word.</param>
        /// <param name="words">The words.</param>
        /// <param name="validChecksum">The valid checksum.</param>
        internal GphdtSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Overrides OnSentanceChanged for the GPVTGSentence
        /// </summary>
        protected override void OnSentenceChanged()
        {
            // First, process the basic info for the sentence
            base.OnSentenceChanged();

            // Cache the sentence words
            string[] words = Words;
            int wordCount = words.Length;

            /*
             * $GPVTG

                Track Made Good and Ground Speed.

                eg1. $GPVTG, 360.0, T, 348.7, M, 000.0, N, 000.0, K*43
                eg2. $GPVTG, 054.7, T, 034.4, M, 005.5, N, 010.2, K

                           054.7, T      True track made good
                           034.4, M      Magnetic track made good
                           005.5, N      Ground speed, knots
                           010.2, K      Ground speed, Kilometers per hour

                eg3. $GPVTG, t, T, ,, s.ss, N, s.ss, K*hh
                0    = Track made good
                1    = Fixed text 'T' indicates that track made good is relative to true north
                2    = not used
                3    = not used
                4    = Speed over ground in knots
                5    = Fixed text 'N' indicates that speed over ground in in knots
                6    = Speed over ground in kilometers/hour
                7    = Fixed text 'K' indicates that speed over ground is in kilometers/hour

             *
             *
             */

            #region Heading

            if (wordCount >= 1 && words[0].Length != 0)
                _heading = Azimuth.Parse(words[0], NmeaCultureInfo);
            else
                _heading = Azimuth.Invalid;

            #endregion Heading

        }

        #endregion Overrides

        #region IHeadingSentence Members

        /// <summary>
        /// the Heading
        /// </summary>
        public Azimuth Heading
        {
            get { return _heading; }
        }

        #endregion IHeadingSentence Members

    }
}