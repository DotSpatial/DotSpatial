// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

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