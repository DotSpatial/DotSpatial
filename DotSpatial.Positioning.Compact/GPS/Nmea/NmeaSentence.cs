using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DotSpatial.Positioning.Gps;

namespace DotSpatial.Positioning.Gps.Nmea
{
    /// <summary>
    /// Represents a single line of NMEA GPS data.
    /// </summary>
    public class NmeaSentence : Packet, IEquatable<NmeaSentence>
    {
        private string _Sentence;
        private string _CommandWord;
        private string[] _Words;
        private string _ExistingChecksum;
        private string _CorrectChecksum;
        private bool _IsValid;

        #region Fields

        /// <summary>
        /// Represents the culture used to process all NMEA GPS data, including numbers and dates.
        /// </summary>
        public static readonly CultureInfo NmeaCultureInfo = CultureInfo.InvariantCulture;

        #endregion

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
            _Sentence = sentence;

            // Notify of the change
            OnSentenceChanged();
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
            _Sentence = sentence;
            _CommandWord = commandWord;
            _Words = words;
            _ExistingChecksum = validChecksum;
            _CorrectChecksum = validChecksum;
            _IsValid = true;

            // Notify of the change
            OnSentenceChanged();
        }

        #endregion

        #region Protected Members

        protected void SetSentence(string sentence)
        {
            // Set the new value
            _Sentence = sentence;

            // Notify of the change
            OnSentenceChanged();
        }

        protected virtual void OnSentenceChanged()
        {
            // Get the location of the dollar sign
            int dollarSignIndex = _Sentence.IndexOf("$", StringComparison.Ordinal);

            // If it's -1, this is invalid
            if (dollarSignIndex == -1)
                return;

            // Get the location of the first comma.  This will mark the end of the
            // command word and the start of data.
            int firstCommaIndex = _Sentence.IndexOf(",", StringComparison.Ordinal);

            // If it's -1, this is invalid
            if (firstCommaIndex == -1)
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
            _CommandWord = string.Intern(_Sentence.Substring(dollarSignIndex, firstCommaIndex - dollarSignIndex));

            // Next, get the index of the asterisk
            int asteriskIndex = _Sentence.IndexOf("*", StringComparison.Ordinal);
            
            // Is an asterisk present?
            int dataEndIndex;
            if (asteriskIndex == -1)
            {
                // No.  The end of the data is the last character of the sentence
                dataEndIndex = _Sentence.Length - 1;
            }
            else
            {
                // Yes.  The end of the data is just before the asterisk
                dataEndIndex = asteriskIndex - 1;
            }
            
            /* Extract the words for the sentence, starting just after the first comma and ending
             * just before the asterisk or at the end of the string.
             *
             * $GPRMC,11,22,33,44,55*CC    OR    $GPRMC,11,22,33,44,55
             *        ^^^^^^^^^^^^^^                    ^^^^^^^^^^^^^^
             *        7            20        
             */

            _Words = _Sentence.Substring(dataStartIndex, dataEndIndex - dataStartIndex + 1).Split(',');

            /* Extract the data portion of the sentence. This portion is used to calculate the checksum
             * 
             * $GPRMC,11,22,33,44,55*CC    OR    $GPRMC,11,22,33,44,55
             */

            // Calculate the checksum for this portion
            byte checksum = (byte)_Sentence[dollarSignIndex + 1];
            for(int index = dollarSignIndex + 2; index <= dataEndIndex; index++)
                checksum ^= (byte)_Sentence[index];

            // The checksum is the two-character hexadecimal value
            _CorrectChecksum = checksum.ToString("X2", NmeaCultureInfo);

            // If there's no asterisk, there's no existing checksum
            if (asteriskIndex == -1)
                return;

            // Get the existing checksum
            _ExistingChecksum = _Sentence.Substring(asteriskIndex + 1);

            // If it matches the current checksum, the sentence is valid
            _IsValid = _ExistingChecksum.Equals(_CorrectChecksum, StringComparison.Ordinal);
        }

        /// <summary>
        /// Calculates and adds a checksum to the end of the sentence.
        /// </summary>
        public void AppendChecksum()
        {
            // Does it already have a checksum?
            if (_ExistingChecksum != null)
                return;

            // No.  Set the current checksum
            _ExistingChecksum = _CorrectChecksum;

            // Does it contain an asterisk? If not, add one
            if (_Sentence.IndexOf("*", StringComparison.Ordinal) == -1)
                _Sentence = _Sentence + '*';

            // Calculate and append the checksum
            _Sentence = _Sentence + _CorrectChecksum;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns the body of the sentence split by commas into strings.
        /// </summary>
        public string[] Words
        {
            get
            {
                return _Words;
            }
        }

        /// <summary>
        /// Returns the first word of the sentence which indicates its purpose.
        /// </summary>
        public string CommandWord
        {
            get
            {
                return _CommandWord;
            }
        }

        /// <summary>
        /// Returns the current checksum of the sentence.
        /// </summary>
        public string ExistingChecksum
        {
            get
            {
                return _ExistingChecksum;
            }
        }

        /// <summary>
        /// Returns the checksum which matches the content of the sentence.
        /// </summary>
        public string CorrectChecksum
        {
            get
            {
                return _CorrectChecksum;
            }
        }

        /// <summary>
        /// Returns a string representing the entire NMEA sentence.
        /// </summary>
        public string Sentence
        {
            get
            {
                return _Sentence;
            }
        }

        #endregion

        #region Overrides

        public override bool IsValid
        {
            get 
            {
                return _IsValid;                
            }
        }

        public override byte[] ToByteArray()
        {
            return ASCIIEncoding.ASCII.GetBytes(_Sentence);
        }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return _Sentence;
        }

        #endregion

        #region IEquatable<NmeaSentence> Members

        public bool Equals(NmeaSentence other)
        {
            return _Sentence.Equals(other.Sentence, StringComparison.Ordinal);
        }

        #endregion
    }


    /// <summary>
    /// Represents information about raw data received from a GPS device.
    /// </summary>
    /// <remarks>This object is used primarily by the <see cref="Receiver.SentenceReceived">SentenceReceived</see>
    /// and <see cref="Receiver.UnrecognizedSentenceReceived">UnrecognizedSentenceReceived</see> events of
    /// the <see cref="Receiver">Receiver</see> class to provide notification when raw data has been received
    /// from the GPS device.</remarks>
    /// <example>This example demonstrates how to use this class when raising an event.
    /// <code lang="VB">
    /// ' Declare a new event
    /// Dim MySentenceEvent As SentenceEventHandler
    /// ' Create an NMEA $GPGLL sentence
    /// Dim MySentence As String = "$GPGLL,4122,N,14628,W,104243.00,A,D*7E"
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
    /// string MySentence = "$GPGLL,4122,N,14628,W,104243.00,A,D*7E"
    /// 
    /// void Main()
    /// {
    ///   // Raise our custom event, also indicating that the sentence was completely recognized
    ///   MySentenceEvent(this, New SentenceEventArgs(MySentence, SentenceType.Recognized));
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="Receiver">Receiver Class</seealso>
    /// <seealso cref="Receiver.SentenceReceived">SentenceReceived Event (Receiver Class)</seealso>
    /// <seealso cref="Receiver.UnrecognizedSentenceReceived">UnrecognizedSentenceReceived Event (Receiver Class)</seealso>
    public sealed class NmeaSentenceEventArgs : EventArgs
    {
        private NmeaSentence _Sentence;

        /// <summary>
        /// Creates a new instance with the specified block of raw GPS data and a flag indicating if the sentence was fully parsed.
        /// </summary>
        /// <param name="sentence">A <strong>String</strong> containing raw GPS data.</param>
        /// <param name="sentenceType">A value from the <see cref="SentenceType">SentenceType</see> enumeration indicating if the sentence was fully parsed or not.</param>
        /// <seealso cref="Type">Type Property</seealso>
        /// <seealso cref="Sentence">Sentence Property</seealso>
        public NmeaSentenceEventArgs(NmeaSentence sentence)
        {
            _Sentence = sentence;
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
                return _Sentence;
            }
        }
    }
}
