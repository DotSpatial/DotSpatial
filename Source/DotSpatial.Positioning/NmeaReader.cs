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
// | VladimirArias (Colombia) | 02/03/2014 | Added hdt nmea sentence for heading orientation
// ********************************************************************************************************

using System;
using System.IO;
using System.Text;
using System.Threading;

namespace DotSpatial.Positioning
{
    /* Notice: This class does NOT implement IDisposable because we don't want responsibility
     * for closing the underlying Stream.  StreamReader unfortunately closes the underlying stream
     * even though it doesn't own it.   >.<
     */

    /// <summary>
    /// Represents a reader which deserializes raw data into NMEA sentences.
    /// </summary>
    public sealed class NmeaReader
    {
        /// <summary>
        ///
        /// </summary>
        private readonly Stream _stream;
        /// <summary>
        ///
        /// </summary>
        private readonly StreamReader _reader;

        #region Constants

        /* The "ideal buffer size" is very important.  It controls the size of buffers and amounts of bytes
         * read at a time.  It's preferable to have a smaller buffer size (such as 128 bytes) but this is also
         * a good value to tune in order to measure latency.  All other classes such as NmeaStream and device
         * detection will use this number!
         */
        /// <summary>
        ///
        /// </summary>
        internal const int IDEAL_NMEA_BUFFER_SIZE = 128;

        #endregion Constants

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public NmeaReader(Stream stream)
        {
            // If the stream is null, complain
            if (stream == null)
                throw new ArgumentNullException("stream", "The base stream of an NMEA stream cannot be null.");

            // Remember the stream
            _stream = stream;

            // Create an ASCII reader for this stream
            _reader = new StreamReader(_stream, Encoding.ASCII, false, IDEAL_NMEA_BUFFER_SIZE);
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when an unknown sentence type is encountered.
        /// </summary>
        public static event EventHandler<NmeaSentenceResolverEventArgs> ResolveSentence;

        #endregion Events

        #region Static Methods

        /// <summary>
        /// Returns whether the specified stream contains valid NMEA data.
        /// </summary>
        /// <param name="stream">An open <strong>Stream</strong> object which supports read operations.</param>
        /// <returns>A <strong>Boolean</strong> value, <strong>True</strong> if valid NMEA data has been read from the stream.</returns>
        public static bool IsNmea(Stream stream)
        {
            // Wrap it in an NMEA Stream.  NMEA is always ASCII
            StreamReader reader = new StreamReader(stream, Encoding.ASCII, false, IDEAL_NMEA_BUFFER_SIZE);
            string testLine;

            // We have ASCII.  Try up to 10 times to get a full sentences
            for (int count = 0; count < 10; count++)
            {
                // Read a line
                try
                {
                    testLine = reader.ReadLine();
                }
                catch (IOException)
                {
                    /* I get this exception when a device such as a computer is being scanned.
                     * Services such as OBEX (file transfer) are picked up, but attempts to scan
                     * result in this exception.
                     */

                    return false;
                }

                // See if this is a valid NMEA sentence
                if (testLine != null)
                    if (
                        // Does it begin with a dollar sign?
                        testLine.StartsWith("$", StringComparison.OrdinalIgnoreCase)
                        // Is there an asterisk before that last two characters?
                        && testLine.IndexOf("*", StringComparison.OrdinalIgnoreCase) == testLine.Length - 3)
                    {
                        return true;
                    }
            }

            return false;
        }

        #endregion Static Methods

        #region Public Methods

        /// <summary>
        /// Reads the next available sentence, valid or invalid, from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public NmeaSentence ReadSentence()
        {
            try
            {
                /* In some rare cases (such as the TDS Nomad, I'm seeing valid
                 * data with some intermittant failures.  As a result, let's not panic,
                 * but simply try a few more times to get a sentence.
                 */
                for (int index = 0; index < 5; index++)
                {
                    // Read a line
                    string value = _reader.ReadLine();

                    // If it's non-null, return it
                    if (value != null)
                        return new NmeaSentence(value);

                    // Wait for a moment
                    Thread.Sleep(100);
                }

                // If it's null, there was no data
                throw new IOException("No data is available from the underlying stream.");
            }
            catch (TimeoutException tex)
            {
                /* The Samsung Omina GPSID driver appears to ignore any sort of timeout value we specify.
                 * It *should* be about 6000ms, but I see timeouts immediately whenever there's no data
                 * available to read :P.  However, making a few additional tries solves the issue; after
                 * a moment the buffer repopulates.
                 */

                /* UPDATE 4/17/2009: This needs testing now that we're using a custom SerialStream. */

                for (int index = 0; index < 5; index++)
                {
                    try
                    {
                        // Try a few more times!
                        return new NmeaSentence(_reader.ReadLine());
                    }
                    catch (TimeoutException)
                    {
                        continue;
                    }
                }

                throw new ApplicationException("GPS Has probably been disconnected or shut off", tex);
            }
        }

        /// <summary>
        /// Reads a well-formed NMEA sentence (with a valid checksum) from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public NmeaSentence ReadValidSentence()
        {
            // Read a sentence
            NmeaSentence sentence = ReadSentence();
            // If it's invalid, keep reading
            while (!sentence.IsValid)
                sentence = ReadSentence();
            // Return the sentence
            return sentence;
        }

        /// <summary>
        /// Reads a single NMEA sentence then attempts to convert it into an object-oriented form.
        /// </summary>
        /// <returns></returns>
        public NmeaSentence ReadTypedSentence()
        {
            // First, read a valid sentence
            NmeaSentence sentence = ReadValidSentence();

            /* 5/4/07: The original design of GPS.NET 2.0 checked the entire command word to determine
             *         the sentence. However, the NMEA specification states that the first two letters of
             *         a sentence may change.  For example, for "$GPGSV" there may be variations such as
             *         "$__GSV" where the first two letters change.  As a result, we need only test the last three
             *         characters.
             */

            // Is this a GPRMC sentence?
            if (sentence.CommandWord.EndsWith("RMC", StringComparison.Ordinal))
            {
                // Yes.  Convert it using the fast pre-parseed constructor
                return new GprmcSentence(sentence.Sentence, sentence.CommandWord, sentence.Words, sentence.ExistingChecksum);
            }
            if (sentence.CommandWord.EndsWith("RMF", StringComparison.Ordinal))
            {
                // Yes.  Convert it using the fast pre-parseed constructor
                return new PgrmfSentence(sentence.Sentence, sentence.CommandWord, sentence.Words, sentence.ExistingChecksum);
            }
            if (sentence.CommandWord.EndsWith("GGA", StringComparison.Ordinal))
            {
                // Yes.  Convert it using the fast pre-parseed constructor
                return new GpggaSentence(sentence.Sentence, sentence.CommandWord, sentence.Words, sentence.ExistingChecksum);
            }
            if (sentence.CommandWord.EndsWith("GLL", StringComparison.Ordinal))
            {
                // Yes.  Convert it using the fast pre-parseed constructor
                return new GpgllSentence(sentence.Sentence, sentence.CommandWord, sentence.Words, sentence.ExistingChecksum);
            }
            if (sentence.CommandWord.EndsWith("GSV", StringComparison.Ordinal))
            {
                // Yes.  Convert it using the fast pre-parseed constructor
                return new GpgsvSentence(sentence.Sentence, sentence.CommandWord, sentence.Words, sentence.ExistingChecksum);
            }
            if (sentence.CommandWord.EndsWith("GSA", StringComparison.Ordinal))
            {
                // Yes.  Convert it using the fast pre-parseed constructor
                return new GpgsaSentence(sentence.Sentence, sentence.CommandWord, sentence.Words, sentence.ExistingChecksum);
            }
            if (sentence.CommandWord.EndsWith("VTG", StringComparison.Ordinal))
            {
                // Yes. Convert it using the fast pre-parseed constructor
                return new GpvtgSentence(sentence.Sentence, sentence.CommandWord, sentence.Words, sentence.ExistingChecksum);
            }
            if (sentence.CommandWord.EndsWith("HDT", StringComparison.Ordinal))
            {
                // Yes. Convert it using the fast pre-parseed constructor
                
                return new GphdtSentence(sentence.Sentence, sentence.CommandWord, sentence.Words, sentence.ExistingChecksum);
            }

            // Raise an event to try and parse this
            if (ResolveSentence != null)
            {
                NmeaSentenceResolverEventArgs e = new NmeaSentenceResolverEventArgs(sentence);
                ResolveSentence(this, e);

                // Was anything returned?
                if (e.Result != null)
                    return e.Result;
            }

            // It's completely unrecognized, so return it as is
            return sentence;
        }

        /// <summary>
        /// Reads the current latitude and longitude from the next available sentence.
        /// </summary>
        /// <returns></returns>
        public Position ReadPosition()
        {
            // Does it support the value we want?
            IPositionSentence sentence = ReadTypedSentence() as IPositionSentence;
            // If not, start over (recorsive)
            while (sentence == null)
                sentence = ReadTypedSentence() as IPositionSentence;
            // Return the location
            return sentence.Position;
        }

        /// <summary>
        /// Reads the UTC date and time from the next available sentence.
        /// </summary>
        /// <returns></returns>
        public DateTime ReadUtcDateTime()
        {
            // Does it support the value we want?
            IUtcDateTimeSentence sentence = ReadTypedSentence() as IUtcDateTimeSentence;
            // If not, start over (recorsive)
            while (sentence == null)
                sentence = ReadTypedSentence() as IUtcDateTimeSentence;
            // Return the location
            return sentence.UtcDateTime;
        }

        /// <summary>
        /// Returns the current date and time from the next available sentence.
        /// </summary>
        /// <returns></returns>
        public DateTime ReadDateTime()
        {
            return ReadUtcDateTime().ToLocalTime();
        }

        /// <summary>
        /// Returns the current rate of travel from the next available sentence.
        /// </summary>
        /// <returns></returns>
        public Speed ReadSpeed()
        {
            // Does it support the value we want?
            ISpeedSentence sentence = ReadTypedSentence() as ISpeedSentence;
            // If not, start over (recorsive)
            while (sentence == null)
                sentence = ReadTypedSentence() as ISpeedSentence;
            // Return the location
            return sentence.Speed;
        }

        /// <summary>
        /// Reads the Bearing direction
        /// </summary>
        /// <returns>The direction as an azimuth angle</returns>
        public Azimuth ReadBearing()
        {
            // Does it support the value we want?
            IBearingSentence sentence = ReadTypedSentence() as IBearingSentence;
            // If not, start over (recorsive)
            while (sentence == null)
                sentence = ReadTypedSentence() as IBearingSentence;
            // Return the location
            return sentence.Bearing;
        }
        
        /// <summary>
        /// Reads the Heading direction
        /// </summary>
        /// <returns>The direction as an azimuth angle</returns>
        public Azimuth ReadHeading()
        {
            // Does it support the value we want?
            IHeadingSentence sentence = ReadTypedSentence() as IHeadingSentence;
            // If not, start over (recorsive)
            while (sentence == null)
                sentence = ReadTypedSentence() as IHeadingSentence;
            // Return the location
            return sentence.Heading;
        }

        /// <summary>
        /// Reads the current type of fix acquired from the next available sentence.
        /// </summary>
        /// <returns></returns>
        public FixQuality ReadFixQuality()
        {
            // Does it support the value we want?
            IFixQualitySentence sentence = ReadTypedSentence() as IFixQualitySentence;
            // If not, start over (recorsive)
            while (sentence == null)
                sentence = ReadTypedSentence() as IFixQualitySentence;
            // Return the location
            return sentence.FixQuality;
        }

        /// <summary>
        /// Reads the current distance above sea level from the next available sentence.
        /// </summary>
        /// <returns></returns>
        public Distance ReadAltitude()
        {
            // Does it support the value we want?
            IAltitudeSentence sentence = ReadTypedSentence() as IAltitudeSentence;
            // If not, start over (recorsive)
            while (sentence == null)
                sentence = ReadTypedSentence() as IAltitudeSentence;
            // Return the location
            return sentence.Altitude;
        }

        /// <summary>
        /// Reads the current precision as it relates to latitude and longitude from the next available sentence.
        /// </summary>
        /// <returns></returns>
        public DilutionOfPrecision ReadHorizontalDilutionOfPrecision()
        {
            // Does it support the value we want?
            IHorizontalDilutionOfPrecisionSentence sentence = ReadTypedSentence() as IHorizontalDilutionOfPrecisionSentence;
            // If not, start over (recorsive)
            while (sentence == null)
                sentence = ReadTypedSentence() as IHorizontalDilutionOfPrecisionSentence;
            // Return the location
            return sentence.HorizontalDilutionOfPrecision;
        }

        #endregion Public Methods
    }

    /// <summary>
    /// Represents information about an unknown NMEA sentence which has been encountered.
    /// </summary>
    public class NmeaSentenceResolverEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        private readonly NmeaSentence _sentence;
        /// <summary>
        ///
        /// </summary>
        private NmeaSentence _result;

        /// <summary>
        /// Creates a new instance of the Nmea Sentence Resolver Event arguments class
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public NmeaSentenceResolverEventArgs(NmeaSentence sentence)
        {
            _sentence = sentence;
        }

        /// <summary>
        /// Returns the NMEA sentence which needs to be resolved.
        /// </summary>
        public NmeaSentence Sentence
        {
            get
            {
                return _sentence;
            }
        }

        /// <summary>
        /// Controls a more-strongly-typed NMEA sentence if resolution is successful.
        /// </summary>
        /// <value>The result.</value>
        public NmeaSentence Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }
    }
}