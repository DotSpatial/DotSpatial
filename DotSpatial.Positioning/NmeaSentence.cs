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
// | DonK                     |  5/25/2011 | Fixed the bug in parsing NMEA sentence (Dotspatial issue 295)
// ********************************************************************************************************
using System;
using System.Globalization;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a single line of NMEA GPS data.
    /// </summary>
    public class NmeaSentence : Packet, IEquatable<NmeaSentence>
    {
        public const string LatitudeFormat = "HHMM.MMMM,I,";
        public const string LongitudeFormat = "HHHMM.MMMM,I,";
        private string _sentence;
        private string _commandWord;
        private string[] _words;
        private string _existingChecksum;
        private string _correctChecksum;
        private bool _isValid;

        #region Fields

        /// <summary>
        /// Represents the culture used to process all NMEA GPS data, including numbers and dates.
        /// </summary>
        public static readonly CultureInfo NmeaCultureInfo = CultureInfo.InvariantCulture;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        protected NmeaSentence()
        { }

        /// <summary>
        /// Creates a new instance from the specified string.
        /// </summary>
        /// <param name="sentence"></param>
        public NmeaSentence(string sentence)
        {
            // Set this sentence's value
            _sentence = sentence;

            // Notify of the change
            DoSentenceChanged();
        }

        /// <summary>
        /// Creates a new instance from known values.
        /// </summary>
        /// <param name="sentence">A <strong>String</strong> containing the entire text of the sentence.</param>
        /// <param name="commandWord">A <strong>String</strong>, the first word of the sentence.</param>
        /// <param name="words">A <strong>String</strong> array, the comma-separated strings between the command word and checksum.</param>
        /// <param name="validChecksum">A <strong>String</strong>, the correct checksum for the sentence.</param>
        /// <remarks>This constructor is typically used when it some processing of the sentence has already occurred.
        /// GPS.NET performs an analysis of the sentence to determine its type.  For the sake of speed, processed information can
        /// be preserved using this constructor.  To process an entire sentence, use the entire NMEA string as a constructor.</remarks>
        protected NmeaSentence(string sentence, string commandWord, string[] words, string validChecksum)
        {
            _sentence = sentence;
            _commandWord = commandWord;
            _words = words;
            _existingChecksum = validChecksum;
            _correctChecksum = validChecksum;
            _isValid = true;

            // Notify of the change
            //DoSentenceChanged();
            /*
             * DonK 2011-05-19: Refactoring based on commments in Issue #295
             * http://dotspatial.codeplex.com/workitem/295
             */
            OnSentenceChanged();
        }

        #endregion Constructors

        #region Protected Members

        /// <summary>
        /// Sets the sentence.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        protected void SetSentence(string sentence)
        {
            // Set the new value
            _sentence = sentence;

            // Notify of the change
            //OnSentenceChanged();
            /*
             * DonK 2011-05-19: Refactoring based on commments in Issue #295
             * http://dotspatial.codeplex.com/workitem/295
             */
            DoSentenceChanged();
        }

        /// <summary>
        /// Called when [sentence changed].
        /// </summary>
        protected virtual void OnSentenceChanged()
        {
            //DoSentenceChanged();
            /*
             * DonK 2011-05-19: Refactoring based on commments in Issue #295
             * http://dotspatial.codeplex.com/workitem/295
             */
            CalculateChecksum();
        }

        private void DoSentenceChanged()
        {
            // Get the location of the dollar sign
            int dollarSignIndex = _sentence.IndexOf("$", StringComparison.Ordinal);

            // If it's -1, this is invalid
            if (dollarSignIndex == -1)
                return;

            // Get the location of the first comma. This will mark the end of the
            // command word and the start of data.
            int firstCommaIndex = _sentence.IndexOf(",", StringComparison.Ordinal);

            // If it's -1, this is invalid
            if (firstCommaIndex == -1)
                return;

            // Determine if the data is not properly formated
            // it will lead to a negative length
            if (firstCommaIndex < dollarSignIndex)
                return;

            // The data starts just after the first comma
            int dataStartIndex = firstCommaIndex + 1;

            /* Extract the command word. This is the text between the dollar sign and the first comma.
            *
            * $GPRMC,
            * ^^^^^^
            * 0123456
            *
            * ... the string is interned, which allows us to compare the word by reference.
            */
            _commandWord = string.Intern(_sentence.Substring(dollarSignIndex, firstCommaIndex - dollarSignIndex));

            // Next, get the index of the asterisk
            int asteriskIndex = _sentence.IndexOf("*", StringComparison.Ordinal);

            // Is an asterisk present?
            int dataEndIndex;
            if (asteriskIndex == -1)
            {
                // No. The end of the data is the last character of the sentence
                dataEndIndex = _sentence.Length - 1;
            }
            else
            {
                // Yes. The end of the data is just before the asterisk
                dataEndIndex = asteriskIndex - 1;
            }

            /* Extract the words for the sentence, starting just after the first comma and ending
            * just before the asterisk or at the end of the string.
            *
            * $GPRMC, 11, 22, 33, 44, 55*CC OR $GPRMC, 11, 22, 33, 44, 55
            * ^^^^^^^^^^^^^^ ^^^^^^^^^^^^^^
            * 7 20
            */

            // Determine if the data is not properly formated
            // This will lead to a negative length
            if (dataEndIndex < dataStartIndex)
                return;

            _words = _sentence.Substring(dataStartIndex, dataEndIndex - dataStartIndex + 1).Split(',');

            /*
            * DonK 2011-05-19: Refactoring based on commments in Issue #295
            * http://dotspatial.codeplex.com/workitem/295
            */
            CalculateChecksum();
        }

        /*
         * DonK 2011-05-19: Added method based on commments in Issue #295
         * http://dotspatial.codeplex.com/workitem/295
         */

        private void CalculateChecksum()
        {
            /* Extract the data portion of the sentence. This portion is used to calculate the checksum
             *
             * $GPRMC, 11, 22, 33, 44, 55*CC    OR    $GPRMC, 11, 22, 33, 44, 55
             */

            // Get the location of the dollar sign
            int dollarSignIndex = _sentence.IndexOf("$", StringComparison.Ordinal);

            // If it's -1, this is invalid
            if (dollarSignIndex == -1)
                return;

            // Next, get the index of the asterisk
            int asteriskIndex = _sentence.IndexOf("*", StringComparison.Ordinal);

            // Is an asterisk present?
            int dataEndIndex;
            if (asteriskIndex == -1)
            {
                // No. The end of the data is the last character of the sentence
                dataEndIndex = _sentence.Length - 1;
            }
            else
            {
                // Yes. The end of the data is just before the asterisk
                dataEndIndex = asteriskIndex - 1;
            }

            // Calculate the checksum for this portion
            byte checksum = (byte)_sentence[dollarSignIndex + 1];
            for (int index = dollarSignIndex + 2; index <= dataEndIndex; index++)
                checksum ^= (byte)_sentence[index];

            // The checksum is the two-character hexadecimal value
            _correctChecksum = checksum.ToString("X2", NmeaCultureInfo);

            // Get existing checksum
            // The checksum is limited to two characters and we expect a \r\n to follow them
            if (asteriskIndex != -1 && _sentence.Length >= asteriskIndex + 1 + 2)
            {
                // Get the existing checksum
                _existingChecksum = _sentence.Substring(asteriskIndex + 1, 2);

                // If it matches the current checksum, the sentence is valid
                _isValid = _existingChecksum.Equals(_correctChecksum, StringComparison.Ordinal);
            }
        }

        /// <summary>
        /// Calculates and adds a checksum to the end of the sentence.
        /// </summary>
        public void AppendChecksum()
        {
            // Does it already have a checksum?
            if (_existingChecksum != null)
                return;

            // No. Set the current checksum
            _existingChecksum = _correctChecksum;
            _isValid = _existingChecksum.Equals(_correctChecksum, StringComparison.Ordinal);

            // Does it contain an asterisk? If not, add one
            if (_sentence.IndexOf("*", StringComparison.Ordinal) == -1)
                _sentence = _sentence + '*';

            // Calculate and append the checksum
            _sentence = _sentence + _correctChecksum;
        }

        #endregion Protected Members

        #region Public Properties

        /// <summary>
        /// Returns the body of the sentence split by commas into strings.
        /// </summary>
        public string[] Words
        {
            get
            {
                return _words;
            }
        }

        /// <summary>
        /// Returns the first word of the sentence which indicates its purpose.
        /// </summary>
        public string CommandWord
        {
            get
            {
                return _commandWord;
            }
        }

        /// <summary>
        /// Returns the current checksum of the sentence.
        /// </summary>
        public string ExistingChecksum
        {
            get
            {
                return _existingChecksum;
            }
        }

        /// <summary>
        /// Returns the checksum which matches the content of the sentence.
        /// </summary>
        public string CorrectChecksum
        {
            get
            {
                return _correctChecksum;
            }
        }

        /// <summary>
        /// Returns a string representing the entire NMEA sentence.
        /// </summary>
        public string Sentence
        {
            get
            {
                return _sentence;
            }
        }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Returns whether the pack data is well-formed.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return _isValid;
            }
        }

        /// <summary>
        /// Converts the packet into an array of bytes.
        /// </summary>
        /// <returns></returns>
        public override byte[] ToByteArray()
        {
            return Encoding.ASCII.GetBytes(_sentence);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return _sentence;
        }

        #endregion Overrides

        #region IEquatable<NmeaSentence> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(NmeaSentence other)
        {
            return _sentence.Equals(other.Sentence, StringComparison.Ordinal);
        }

        #endregion IEquatable<NmeaSentence> Members
    }

    /// <summary>
    /// Represents information about raw data received from a GPS device.
    /// </summary>
    /// <remarks>This object is used primarily by the SentenceReceived
    /// and UnrecognizedSentenceReceived events of
    /// the Receiver class to provide notification when raw data has been received
    /// from the GPS device.</remarks>
    /// <example>This example demonstrates how to use this class when raising an event.
    /// <code lang="VB">
    /// ' Declare a new event
    /// Dim MySentenceEvent As SentenceEventHandler
    /// ' Create an NMEA $GPGLL sentence
    /// Dim MySentence As String = "$GPGLL, 4122, N, 14628, W, 104243.00, A, D*7E"
    ///
    /// Sub Main()
    ///   ' Raise our custom event, also indicating that the sentence was completely recognized
    ///   RaiseEvent MySentenceEvent(Me, New SentenceEventArgs(MySentence, SentenceType.Recognized))
    /// End Sub
    /// </code>
    /// <code lang="C#">
    /// // Declare a new event
    /// SentenceEventHandler MySentenceEvent;
    /// // Create an NMEA $GPGLL sentence
    /// string MySentence = "$GPGLL, 4122, N, 14628, W, 104243.00, A, D*7E"
    ///
    /// void Main()
    /// {
    ///   // Raise our custom event, also indicating that the sentence was completely recognized
    ///   MySentenceEvent(this, New SentenceEventArgs(MySentence, SentenceType.Recognized));
    /// }
    /// </code>
    /// </example>
    public sealed class NmeaSentenceEventArgs : EventArgs
    {
        private readonly NmeaSentence _sentence;

        /// <summary>
        /// Creates a new instance with the specified block of raw GPS data and a flag indicating if the sentence was fully parsed.
        /// </summary>
        /// <param name="sentence">A <strong>String</strong> containing raw GPS data.</param>
        /// <seealso cref="Type">Type Property</seealso>
        /// <seealso cref="Sentence">Sentence Property</seealso>
        public NmeaSentenceEventArgs(NmeaSentence sentence)
        {
            _sentence = sentence;
        }

        /// <summary>
        /// Stores one line of raw GPS data.
        /// </summary>
        /// <value>A String containing one line of raw NMEA or Garmin® text data.</value>
        /// <remarks>When using the NMEA-0183 or Garmin® text protocols, this property stores text for one individual line.  For the Garmin® binary protocol, a signature such as "(Garmin waypoint data)" is stored instead of actual binary data.</remarks>
        /// <seealso cref="Type">Type Property</seealso>
        public NmeaSentence Sentence
        {
            get
            {
                return _sentence;
            }
        }
    }
}