// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a single line of NMEA GPS data.
    /// </summary>
    public class NmeaSentence : Packet, IEquatable<NmeaSentence>
    {
        #region Fields

        public const string LatitudeFormat = "HHMM.MMMM,I,";
        public const string LongitudeFormat = "HHHMM.MMMM,I,";
        private bool _isValid;
        private string _sentence;

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
            Sentence = sentence; // set the sentence and parse it to the properties
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
            _sentence = sentence; // set _sentence directly so that it won't be split into the properties, because the properties are set to the parameters afterwards anyway
            CommandWord = commandWord;
            Words = words;
            ExistingChecksum = validChecksum;
            CorrectChecksum = validChecksum;
            _isValid = true;
        }

        #endregion Constructors

        #region Protected Members

        /// <summary>
        /// Calculates and adds a checksum to the end of the sentence if the sentence doesn't have a checksum.
        /// </summary>
        public void AppendChecksum()
        {
            // Does it already have a checksum?
            if (ExistingChecksum != null)
                return;

            // No. Set the current checksum
            ExistingChecksum = CorrectChecksum;
            _isValid = true;

            // Does it contain an asterisk? If not, add one
            if (Sentence.IndexOf("*", StringComparison.Ordinal) == -1)
                Sentence += '*';

            // Calculate and append the checksum
            Sentence += CorrectChecksum;
        }

        /// <summary>
        /// Sets this classes properties from the Sentence.
        /// </summary>
        protected void SetPropertiesFromSentence()
        {
            // Get the location of the dollar sign
            int dollarSignIndex = Sentence.IndexOf("$", StringComparison.Ordinal);
            if (dollarSignIndex == -1) return; // If it's -1, this is invalid

            // Get the location of the first comma. This will mark the end of the command word and the start of data.
            int firstCommaIndex = Sentence.IndexOf(",", StringComparison.Ordinal);
            if (firstCommaIndex == -1) return; // If it's -1, this is invalid

            // Determine if the data is not properly formated. Not propertly formated data leads to a negative length.
            if (firstCommaIndex < dollarSignIndex) return;

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
            CommandWord = string.Intern(Sentence.Substring(dollarSignIndex, firstCommaIndex - dollarSignIndex));

            // Next, get the index of the asterisk
            int asteriskIndex = Sentence.IndexOf("*", StringComparison.Ordinal);
            int dataEndIndex = asteriskIndex == -1 ? Sentence.Length - 1 : asteriskIndex - 1; // dataEndIndex is before asterix if it exists, otherwise it's the last character

            // Determine if the data is properly formated. Not propertly formated data leads to a negative length.
            if (dataEndIndex < dataStartIndex) return;

            /* Extract the words for the sentence, starting just after the first comma and ending
            * just before the asterisk or at the end of the string.
            *
            * $GPRMC, 11, 22, 33, 44, 55*CC OR $GPRMC, 11, 22, 33, 44, 55
            */
            Words = Sentence.Substring(dataStartIndex, dataEndIndex - dataStartIndex + 1).Split(',');

            // Calculate the checksum for this portion
            byte checksum = (byte)Sentence[dollarSignIndex + 1];
            for (int index = dollarSignIndex + 2; index <= dataEndIndex; index++)
                checksum ^= (byte)Sentence[index];

            CorrectChecksum = checksum.ToString("X2", NmeaCultureInfo); // The checksum is the two-character hexadecimal value

            // Get existing checksum
            // The checksum is limited to two characters and we expect a \r\n to follow them
            if (asteriskIndex != -1 && Sentence.Length >= asteriskIndex + 1 + 2)
            {
                ExistingChecksum = Sentence.Substring(asteriskIndex + 1, 2);

                // If the existing checksum matches the current checksum, the sentence is valid
                _isValid = ExistingChecksum.Equals(CorrectChecksum, StringComparison.Ordinal);
            }
        }

        #region ParseMethods
        /// <summary>
        /// Parses the word at the given position to Azimuth.
        /// </summary>
        /// <param name="position">Position of the Words array that gets parsed to Azimuth.</param>
        /// <returns>Invalid if position is outside of Words array or word at the position is empty.</returns>
        protected Azimuth ParseAzimuth(int position)
        {
            if (Words.Length <= position || Words[position].Length == 0) return Azimuth.Invalid;
            return Azimuth.Parse(Words[position], NmeaCultureInfo);
        }

        /// <summary>
        /// Parses the word at the given position to DilutionOfPrecision.
        /// </summary>
        /// <param name="position">Position of the Words array that gets parsed to DilutionOfPrecision.</param>
        /// <returns>Invalid if position is outside of Words array or word at the position is empty.</returns>
        protected DilutionOfPrecision ParseDilution(int position)
        {
            if (Words.Length <= position || Words[position].Length == 0 || float.Parse(Words[position], NmeaCultureInfo) <= 0) return DilutionOfPrecision.Invalid;
            return new DilutionOfPrecision(float.Parse(Words[position], NmeaCultureInfo));
        }

        /// <summary>
        /// Parses the word at the given position to Distance.
        /// </summary>
        /// <param name="position">Position of the Words array that gets parsed to Distance.</param>
        /// <param name="unit">DistanceUnit that is used for parsing.</param>
        /// <returns>Invalid if position is outside of Words array or word at the position is empty.</returns>
        protected Distance ParseDistance(int position, DistanceUnit unit)
        {
            if (Words.Length <= position || Words[position].Length == 0) return Distance.Invalid;
            return new Distance(double.Parse(Words[position], NmeaCultureInfo), unit);
        }

        /// <summary>
        /// Parses the word at the given position to FixMethod. Expected is (0 = noFix, 1 = Fix2D, 2 = Fix3D). If the order is correct but noFix starts at 1 set noFixAt1 to true.
        /// </summary>
        /// <param name="position">Position of the word that will be parsed.</param>
        /// <param name="noFixAt1">Indicates that noFix starts at 1 instead of 0.</param>
        /// <returns>Unknown if the position was outside of the Word array or the word was empty or contained an unknown value.</returns>
        protected FixMethod ParseFixMethod(int position, bool noFixAt1)
        {
            if (Words.Length <= position || Words[position].Length == 0) return FixMethod.Unknown;

            int value = int.Parse(Words[position], NmeaCultureInfo);
            if (noFixAt1) value -= 1; // if noFix starts at 1 we subtract 1 so that we can use the value in the switch block

            switch (value)
            {
                case 0:
                    return FixMethod.NoFix;
                case 1:
                    return FixMethod.Fix2D;
                case 2:
                    return FixMethod.Fix3D;
                default:
                    return FixMethod.Unknown;
            }
        }

        /// <summary>
        /// Parses the FixMode from the given position of the Words array. (A = Automatic, M = Manual, everything else = Unknown)
        /// </summary>
        /// <param name="position">Position of the Word array that will be parsed.</param>
        /// <returns></returns>
        protected FixMode ParseFixMode(int position)
        {
            if (Words.Length > position && Words[position].Length != 0)
            {
                return Words[position] == "A" ? FixMode.Automatic : Words[position] == "M" ? FixMode.Manual : FixMode.Unknown;
            }
            return FixMode.Unknown;
        }

        /// <summary>
        /// Parses the FixQuality from the given position of the Words array. (0 = NoFix, 1 = GpsFix, 2 = DifferentialGpsFix, 3 = PulsePerSecond, 4 = FixedRealTimeKinematic, 5 = FloatRealTimeKinematic, 
        /// 6 = Estimated, 7 = ManualInput, 8 = Simulated, everything else = Unknown)
        /// </summary>
        /// <param name="position">Position of the Word array that will be parsed.</param>
        /// <returns>Unknown if position is outside of Words array or word at the position is empty or the given value is not between 0 and 8.</returns>
        protected FixQuality ParseFixQuality(int position)
        {
            if (Words.Length <= position || Words[position].Length == 0) return FixQuality.Unknown;

            switch (int.Parse(Words[position], NmeaCultureInfo))
            {
                case 0:
                    return FixQuality.NoFix;
                case 1:
                    return FixQuality.GpsFix;
                case 2:
                    return FixQuality.DifferentialGpsFix;
                case 3:
                    return FixQuality.PulsePerSecond;
                case 4:
                    return FixQuality.FixedRealTimeKinematic;
                case 5:
                    return FixQuality.FloatRealTimeKinematic;
                case 6:
                    return FixQuality.Estimated;
                case 7:
                    return FixQuality.ManualInput;
                case 8:
                    return FixQuality.Simulated;
                default:
                    return FixQuality.Unknown;
            }
        }

        /// <summary>
        /// Parses the word at the given position to FixStatus (A = Fix, everything else = NoFix).
        /// </summary>
        /// <param name="position">Position of the Words array that gets parsed to FixStatus.</param>
        /// <returns>Unknown if position is outside of Words array or word at the position is empty.</returns>
        protected FixStatus ParseFixStatus(int position)
        {
            if (Words.Length <= position || Words[position].Length == 0) return FixStatus.Unknown;
            return Words[position].Equals("A", StringComparison.OrdinalIgnoreCase) ? FixStatus.Fix : FixStatus.NoFix;
        }

        /// <summary>
        /// Parses the Position based on the given indices and the Words array.
        /// </summary>
        /// <param name="latitudeValuePosition">Position of the latitude value inside the Words array.</param>
        /// <param name="latitudeHemispherePosition">Position of the latitude hemisphere inside the Words array.</param>
        /// <param name="longitudeValuePosition">Position of the longitude value inside the Words array.</param>
        /// <param name="longitudeHemispherePosition">Position of the longitude hemisphere inside the Words array.</param>
        /// <returns>Position.Invalid, if the Words array is to short or at least one of the fields is empty.</returns>
        protected Position ParsePosition(int latitudeValuePosition, int latitudeHemispherePosition, int longitudeValuePosition, int longitudeHemispherePosition)
        {
            List<int> positions = new List<int> { latitudeValuePosition, latitudeHemispherePosition, longitudeValuePosition, longitudeHemispherePosition };

            if (Words.Length <= positions.Max() || positions.Any(pos => Words[pos].Length < 1)) // not enough words or empty field result in invalid position
                return Position.Invalid;

            // Parse the latitude
            string latitudeWord = Words[latitudeValuePosition];
            int latitudeHours = int.Parse(latitudeWord.Substring(0, 2), NmeaCultureInfo);
            double latitudeDecimalMinutes = double.Parse(latitudeWord.Substring(2), NmeaCultureInfo);
            LatitudeHemisphere latitudeHemisphere = Words[latitudeHemispherePosition].Equals("N", StringComparison.Ordinal) ? LatitudeHemisphere.North : LatitudeHemisphere.South;

            // Parse the longitude
            string longitudeWord = Words[longitudeValuePosition];
            int longitudeHours = int.Parse(longitudeWord.Substring(0, 3), NmeaCultureInfo);
            double longitudeDecimalMinutes = double.Parse(longitudeWord.Substring(3), NmeaCultureInfo);
            LongitudeHemisphere longitudeHemisphere = Words[longitudeHemispherePosition].Equals("E", StringComparison.Ordinal) ? LongitudeHemisphere.East : LongitudeHemisphere.West;

            return new Position(new Latitude(latitudeHours, latitudeDecimalMinutes, latitudeHemisphere), new Longitude(longitudeHours, longitudeDecimalMinutes, longitudeHemisphere));
        }

        /// <summary>
        /// Parses the word at the given position to Speed based on the given unit.
        /// </summary>
        /// <param name="position">Position of the Words array that gets parsed to speed.</param>
        /// <param name="unit">SpeedUnit that is used for parsing.</param>
        /// <returns>Invalid if position is outside of Words array or word at the position is empty.</returns>
        protected Speed ParseSpeed(int position, SpeedUnit unit)
        {
            if (Words.Length <= position || Words[position].Length == 0) return Speed.Invalid;
            return new Speed(double.Parse(Words[position], NmeaCultureInfo), unit);
        }

        /// <summary>
        /// Parses the word at the given position to UtcDateTime.
        /// </summary>
        /// <param name="timePosition">Position of the Words array that contains the time that gets parsed to UtcDateTime. Expected is a string of format hhmmss. Every number after that will be parsed to milliseconds.</param>
        /// <param name="datePosition">Position of the Words array that contains the date that gets parsed to UtcDateTime. Expected is a string of format ddmmyy. The Year will be added to 2000, because we expect the year to be of this millennium.</param>
        /// <returns>MinValue if the bigger position is outside of Words array or if one of the words is empty.</returns>
        protected DateTime ParseUtcDateTime(int timePosition, int datePosition)
        {
            int maxPos = Math.Max(timePosition, datePosition);

            if (Words.Length <= maxPos || Words[timePosition].Length == 0 && Words[datePosition].Length == 0)
                return DateTime.MinValue;

            // Parse the UTC time
            string utcTimeWord = Words[timePosition];
            int utcHours = int.Parse(utcTimeWord.Substring(0, 2), NmeaCultureInfo);
            int utcMinutes = int.Parse(utcTimeWord.Substring(2, 2), NmeaCultureInfo);
            int utcSeconds = int.Parse(utcTimeWord.Substring(4, 2), NmeaCultureInfo);
            int utcMilliseconds = utcTimeWord.Length > 6 ? Convert.ToInt32(float.Parse(utcTimeWord.Substring(6), NmeaCultureInfo) * 1000, NmeaCultureInfo) : 0;

            // Parse the UTC date
            string utcDateWord = Words[datePosition];
            int utcDay = int.Parse(utcDateWord.Substring(0, 2), NmeaCultureInfo);
            int utcMonth = int.Parse(utcDateWord.Substring(2, 2), NmeaCultureInfo);
            int utcYear = int.Parse(utcDateWord.Substring(4, 2), NmeaCultureInfo) + 2000;

            // Build a UTC date/time
            return new DateTime(utcYear, utcMonth, utcDay, utcHours, utcMinutes, utcSeconds, utcMilliseconds, DateTimeKind.Utc);
        }

        /// <summary>
        /// Parses the word at the given position to UtcTimeSpan. Expected is a String of format hhmmss. Every number after that will be parsed to milliseconds.
        /// </summary>
        /// <param name="position">Position of the Words array that gets parsed to UtcTimeSpan.</param>
        /// <returns>MinValue if position is outside of Words array or word at the position is empty.</returns>
        protected TimeSpan ParseUtcTimeSpan(int position)
        {
            // Do we have enough data to process the UTC time?
            if (Words.Length <= position || Words[position].Length == 0) return TimeSpan.MinValue;

            string utcTimeWord = Words[position];
            int utcHours = int.Parse(utcTimeWord.Substring(0, 2), NmeaCultureInfo);
            int utcMinutes = int.Parse(utcTimeWord.Substring(2, 2), NmeaCultureInfo);
            int utcSeconds = int.Parse(utcTimeWord.Substring(4, 2), NmeaCultureInfo);
            int utcMilliseconds = utcTimeWord.Length > 6 ? Convert.ToInt32(float.Parse(utcTimeWord.Substring(6), NmeaCultureInfo) * 1000, NmeaCultureInfo) : 0;

            // Build a TimeSpan for this value
            return new TimeSpan(0, utcHours, utcMinutes, utcSeconds, utcMilliseconds);
        }
        #endregion

        #endregion Protected Members

        #region Public Properties

        /// <summary>
        /// Returns the body of the sentence split by commas into strings.
        /// </summary>
        public string[] Words { get; private set; }

        /// <summary>
        /// Returns the first word of the sentence which indicates its purpose.
        /// </summary>
        public string CommandWord { get; private set; }

        /// <summary>
        /// Returns the current checksum of the sentence.
        /// </summary>
        public string ExistingChecksum { get; private set; }

        /// <summary>
        /// Returns the checksum which matches the content of the sentence.
        /// </summary>
        public string CorrectChecksum { get; private set; }

        /// <summary>
        /// Returns a string representing the entire NMEA sentence.
        /// </summary>
        public string Sentence
        {
            get { return _sentence; }
            protected set
            {
                _sentence = value;
                SetPropertiesFromSentence();
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
            return Encoding.ASCII.GetBytes(Sentence);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return Sentence;
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
            if (other == null) return false;
            if (Sentence == null) return other.Sentence == null;
            return Sentence.Equals(other.Sentence, StringComparison.Ordinal);
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