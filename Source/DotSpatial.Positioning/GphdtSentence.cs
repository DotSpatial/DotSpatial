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
        #region Constructors

        /// <summary>
        /// Creates a heading sentence instance from the specified sentence
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GphdtSentence(string sentence)
            : base(sentence)
        { SetPropertiesFromSentence(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="GphdtSentence"/> class.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="commandWord">The command word.</param>
        /// <param name="words">The words.</param>
        /// <param name="validChecksum">The valid checksum.</param>
        internal GphdtSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { SetPropertiesFromSentence(); }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Corrects this classes properties after the base sentence was changed.
        /// </summary>
        private new void SetPropertiesFromSentence()
        {
            /*
             * $GPHDT
                Heading, True.
                Actual vessel heading in degrees Ture produced by any device or system producing true heading.

                $GPHDT,x.x,T
                x.x = Heading, degrees True 
            */
            Heading = ParseAzimuth(0);
           }

        #endregion Overrides

        #region Properties

        /// <summary>
        /// the Heading
        /// </summary>
        public Azimuth Heading { get; private set; }

        #endregion

    }
}