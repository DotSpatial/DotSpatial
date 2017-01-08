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
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a $GPGSV sentence describing the location and signal strength of GPS
    /// satellites.
    /// </summary>
    /// <remarks>This sentence is used to determine the location of GPS satellites relative
    /// to the current location, as well as to indicate the strength of a satellite's radio
    /// signal.</remarks>
    public sealed class GpgsvSentence : NmeaSentence, ISatelliteCollectionSentence
    {
        #region Constructors

        /// <summary>
        /// Creates a GPGSV sentence instance from the specified string.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        public GpgsvSentence(string sentence)
            : base(sentence)
        { SetPropertiesFromSentence(); }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpgsvSentence"/> class.
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        /// <param name="commandWord">The command word.</param>
        /// <param name="words">The words.</param>
        /// <param name="validChecksum">The valid checksum.</param>
        internal GpgsvSentence(string sentence, string commandWord, string[] words, string validChecksum)
            : base(sentence, commandWord, words, validChecksum)
        { SetPropertiesFromSentence(); }

        /// <summary>
        /// Creates a GPSV sentence instance from the specified parameters describing the location and signal strength of GPS satellites
        /// </summary>
        /// <param name="totalMessageCount">The total message count.</param>
        /// <param name="currentMessageNumber">The current message number.</param>
        /// <param name="satellitesInView">The satellites in view.</param>
        /// <param name="satellites">The satellites.</param>
        public GpgsvSentence(int totalMessageCount, int currentMessageNumber, int satellitesInView, IList<Satellite> satellites)
        {
            TotalMessageCount = totalMessageCount;
            CurrentMessageNumber = currentMessageNumber;
            SatellitesInView = satellitesInView;
            Satellites = satellites;

            // Build a sentence
            StringBuilder builder = new StringBuilder(128);

            // Append the command word
            builder.Append("$GPGSV");
            builder.Append(',');

            // Total message count
            builder.Append(TotalMessageCount);
            builder.Append(',');

            // Current message number
            builder.Append(CurrentMessageNumber);
            builder.Append(',');

            // Satellites in view
            builder.Append(SatellitesInView);

            int count = Satellites.Count;
            for (int index = 0; index < count; index++)
            {
                Satellite satellite = Satellites[index];

                // Serialize this satellite
                builder.Append(",");
                builder.Append(satellite.PseudorandomNumber.ToString("0#", NmeaCultureInfo));
                builder.Append(",");
                builder.Append(satellite.Elevation.ToString("dd", NmeaCultureInfo));
                builder.Append(",");
                builder.Append(satellite.Azimuth.ToString("ddd", NmeaCultureInfo));
                builder.Append(",");
                builder.Append(satellite.SignalToNoiseRatio.Value.ToString("0#", NmeaCultureInfo));
            }

            // Set this object's sentence
            Sentence = builder.ToString();
            SetPropertiesFromSentence();

            // Finally, append the checksum
            AppendChecksum();
        }

        #endregion Constructors

        #region Overrides

        /// <summary>
        /// Corrects this classes properties after the base sentence was changed.
        /// </summary>
        private new void SetPropertiesFromSentence()
        {
            // Cache the words
            string[] words = Words;
            int wordCount = words.Length;

            /*  $GPGSV
             *
             *  GPS Satellites in view
             *
             *  eg. $GPGSV, 3, 1, 11, 03, 03, 111, 00, 04, 15, 270, 00, 06, 01, 010, 00, 13, 06, 292, 00*74
             *  $GPGSV, 3, 2, 11, 14, 25, 170, 00, 16, 57, 208, 39, 18, 67, 296, 40, 19, 40, 246, 00*74
             *  $GPGSV, 3, 3, 11, 22, 42, 067, 42, 24, 14, 311, 43, 27, 05, 244, 00, ,, ,*4D
             *
             *
             *  $GPGSV, 1, 1, 13, 02, 02, 213, ,03, -3, 000, ,11, 00, 121, ,14, 13, 172, 05*62
             *
             *
             *  1    = Total number of messages of this type in this cycle
             *  2    = Message number
             *  3    = Total number of SVs in view
             *  4    = SV PRN number
             *  5    = Elevation in degrees, 90 maximum
             *  6    = Azimuth, degrees from true north, 000 to 359
             *  7    = SNR, 00-99 dB (null when not tracking)
             *  8-11 = Information about second SV, same as field 4-7
             *  12-15= Information about third SV, same as field 4-7
             *  16-19= Information about fourth SV, same as field 4-7
             */

            // Example (signal not acquired): $GPGSV, 1, 1, 01, 21, 00, 000, *4B
            // Example (signal acquired): $GPGSV, 3, 1, 10, 20, 78, 331, 45, 01, 59, 235, 47, 22, 41, 069, ,13, 32, 252, 45*70

            // Get the total message count
            if (wordCount > 0 && words[0].Length != 0)
                TotalMessageCount = int.Parse(words[0], NmeaCultureInfo);

            // Get the current message number
            if (wordCount > 1 && words[1].Length != 0)
                CurrentMessageNumber = int.Parse(words[1], NmeaCultureInfo);

            // Get the total message count
            if (wordCount > 2 && words[2].Length != 0)
                SatellitesInView = int.Parse(words[2], NmeaCultureInfo);

            // Make a new list of satellites
            Satellites = new List<Satellite>();

            // Now process each satellite
            for (int index = 0; index < 6; index++)
            {
                int currentWordIndex = index * 4 + 3;
                // Are we past the length of words?
                if (currentWordIndex > wordCount - 1)
                    break;
                // No.  Get the unique code for the satellite
                if (words[currentWordIndex].Length == 0)
                    continue;

                int pseudorandomNumber = int.Parse(words[currentWordIndex], NmeaCultureInfo);
                Elevation newElevation;
                Azimuth newAzimuth;
                SignalToNoiseRatio newSignalToNoiseRatio;

                // Update the elevation
                if (wordCount > currentWordIndex + 1 && words[currentWordIndex + 1].Length != 0)
                    newElevation = Elevation.Parse(words[currentWordIndex + 1], NmeaCultureInfo);
                else
                    newElevation = Elevation.Empty;

                // Update the azimuth
                if (wordCount > currentWordIndex + 2 && words[currentWordIndex + 2].Length != 0)
                    newAzimuth = Azimuth.Parse(words[currentWordIndex + 2], NmeaCultureInfo);
                else
                    newAzimuth = Azimuth.Empty;

                // Update the signal strength
                if (wordCount > currentWordIndex + 3 && words[currentWordIndex + 3].Length != 0)
                    newSignalToNoiseRatio = SignalToNoiseRatio.Parse(words[currentWordIndex + 3], NmeaCultureInfo);
                else
                    newSignalToNoiseRatio = SignalToNoiseRatio.Empty;

                // Add the satellite to the collection
                Satellites.Add(new Satellite(pseudorandomNumber, newAzimuth, newElevation, newSignalToNoiseRatio, false));
            }
        }

        #endregion Overrides

        #region Static Members

        /// <summary>
        /// Returns a collection of $GPGSV sentences fully describing the specified collection of satellites.
        /// </summary>
        /// <param name="satellites">The satellites.</param>
        /// <returns></returns>
        public static IList<GpgsvSentence> FromSatellites(IList<Satellite> satellites)
        {
            // Divide the collecting into 4-satellite chunks
            int totalMessages = (int)Math.Ceiling(satellites.Count / 4d);
            IList<GpgsvSentence> result = new List<GpgsvSentence>(totalMessages);
            // And populate each member of the array
            for (int index = 1; index <= totalMessages; index++)
            {
                // Make a collection with just the satellites we want
                List<Satellite> messageSatellites = new List<Satellite>();
                for (int count = 0; count < 4; count++)
                {
                    // Calculate the satellite to add
                    int satelliteIndex = (index - 1) * 4 + count;
                    // Stop if were at the end of the collection
                    if (satelliteIndex >= satellites.Count)
                        break;
                    // Copy the satellite in
                    messageSatellites.Add(satellites[satelliteIndex]);
                }
                // Now make the sentence
                result.Add(new GpgsvSentence(totalMessages, index, satellites.Count, messageSatellites));
            }

            // return all sentences
            return result;
        }

        #endregion Static Members

        #region Public Members

        /// <summary>
        /// Returns a collection of <strong>Satellite</strong> objects describing current
        /// satellite information.
        /// </summary>
        public IList<Satellite> Satellites { get; private set; }

        /// <summary>
        /// Returns the total number of $GPGSV sentence in a sequence.
        /// </summary>
        public int TotalMessageCount { get; private set; }

        /// <summary>
        /// Returns the current message index when the sentence is one of several
        /// messages.
        /// </summary>
        public int CurrentMessageNumber { get; private set; }

        /// <summary>
        /// Returns the number of satellites whose signals are detected by the GPS
        /// device.
        /// </summary>
        public int SatellitesInView { get; private set; }

        #endregion Public Members
    }
}