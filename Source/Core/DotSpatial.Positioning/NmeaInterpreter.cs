// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Security;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents an interpreter for GPS data from the National Marine Electronics Association (NMEA).
    /// </summary>
    [SecuritySafeCritical]
    public class NmeaInterpreter : Interpreter
    {
        /// <summary>
        ///
        /// </summary>
        private NmeaReader _stream;

        /// <summary>
        /// Represents a synchronization object used to prevent changes to GPS data when reading multiple GPS values.
        /// </summary>
        public object DataChangeSyncRoot = new();

        #region Constructors

        /// <summary>
        /// Creates an interpreter to read NMEA streams
        /// </summary>
        public NmeaInterpreter()
        {
            Initialize();
        }

        #endregion Constructors

        #region Events

#if DEBUG

        /// <summary>
        /// Occurs when a new line of NMEA data has arrived.
        /// </summary>
        [Obsolete("This event is intended for debugging.  Use it to confirm that GPS data is arriving from your GPS device, but avoid it during production use to improve your application's performance.")]
        public event EventHandler<NmeaSentenceEventArgs> SentenceReceived;

        /// <summary>
        /// SentenceReceived event handler
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        protected void OnSentenceReceived(NmeaSentence sentence)
        {
            SentenceReceived?.Invoke(this, new NmeaSentenceEventArgs(sentence));
        }

#endif

        /// <summary>
        /// Occurs when a packet of data has been recorded to the recording stream.
        /// </summary>
        public event EventHandler<NmeaSentenceEventArgs> SentenceRecorded;

        /// <summary>
        /// Called when [sentence recorded].
        /// </summary>
        /// <param name="sentence">The sentence.</param>
        protected void OnSentenceRecorded(NmeaSentence sentence)
        {
            SentenceRecorded?.Invoke(this, new NmeaSentenceEventArgs(sentence));
        }

        #endregion Events

        #region Public Methods

        /// <summary>
        /// Translates the NmeaSentence
        /// </summary>
        /// <param name="sentence">The NMeaSentence to parse</param>
        public void Parse(NmeaSentence sentence)
        {
            /* NMEA data is parsed in a specific order to maximize data quality and also to reduce latency
             * problems.  The date/time is processed first to minimize latency.  Then, dilution of precision
             * values are processed.  Finally, if precision is good enough to work with, remaining values are
             * processed.
             */

            // Is this a fix method message?
            if (sentence is IFixMethodSentence fixMethodSentence)
            {
                SetFixMethod(fixMethodSentence.FixMethod);
            }

            // Is this a fix quality message?
            if (sentence is IFixQualitySentence fixQualitySentence)
            {
                SetFixQuality(fixQualitySentence.FixQuality);
            }

            #region Process common GPS information

            // If a fix is required, don't process time
            if (!IsFixRequired || (IsFixRequired && IsFixed))
            {
                // Does this sentence support the UTC date and time?
                if (sentence is IUtcDateTimeSentence dateTimeSentence)
                {
                    SetDateTimes(dateTimeSentence.UtcDateTime);
                }

                /* Some NMEA sentences provide UTC time information, but no date!  To make this work,
                 * we must combine the time report with the current UTC date.
                 */
                if (sentence is IUtcTimeSentence timeSentence && !timeSentence.UtcTime.Equals(TimeSpan.MinValue) && !timeSentence.UtcTime.Equals(TimeSpan.Zero))
                {
                    SetDateTimes(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day,
                                        timeSentence.UtcTime.Hours, timeSentence.UtcTime.Minutes, timeSentence.UtcTime.Seconds, timeSentence.UtcTime.Milliseconds,
                                        DateTimeKind.Utc));
                }
            }

            // Does this sentence support horizontal DOP?
            if (sentence is IHorizontalDilutionOfPrecisionSentence hdopSentence)
            {
                SetHorizontalDilutionOfPrecision(hdopSentence.HorizontalDilutionOfPrecision);
            }

            // Does the sentence support vertical DOP?
            if (sentence is IVerticalDilutionOfPrecisionSentence vdopSentence)
            {
                SetVerticalDilutionOfPrecision(vdopSentence.VerticalDilutionOfPrecision);
            }

            // Does the sentence support mean DOP?
            if (sentence is IPositionDilutionOfPrecisionSentence mdopSentence)
            {
                SetMeanDilutionOfPrecision(mdopSentence.PositionDilutionOfPrecision);
            }

            #endregion Process common GPS information

            // If a fix is required, don't process time
            if (!IsFixRequired || (IsFixRequired && IsFixed))
            {
                // Is precision good enough to work with?
                if (HorizontalDilutionOfPrecision.Value <= MaximumHorizontalDilutionOfPrecision.Value)
                {
                    #region Process real-time positional data

                    // Does this sentence support lat/long info?
                    if (sentence is IPositionSentence positionSentence)
                    {
                        SetPosition(positionSentence.Position);
                    }

                    // Does this sentence support bearing?
                    if (sentence is IBearingSentence bearingSentence)
                    {
                        SetBearing(bearingSentence.Bearing);
                    }

                    // Does this sentence support heading?
                    if (sentence is IHeadingSentence headingSentence)
                    {
                        SetHeading(headingSentence.Heading);
                    }

                    // Does this sentence support speed?
                    if (sentence is ISpeedSentence speedSentence)
                    {
                        SetSpeed(speedSentence.Speed);
                    }

                    #endregion Process real-time positional data
                }

                // Is the vertical DOP low enough to be worth processing?
                if (VerticalDilutionOfPrecision.Value <= MaximumVerticalDilutionOfPrecision.Value)
                {
                    #region Process altitude data

                    // Does this sentence support altitude?
                    if (sentence is IAltitudeSentence altitudeSentence)
                    {
                        SetAltitude(altitudeSentence.Altitude);
                    }

                    // Does this sentence support altitude?
                    if (sentence is IAltitudeAboveEllipsoidSentence altitudeAboveEllipsoidSentence)
                    {
                        SetAltitude(altitudeAboveEllipsoidSentence.AltitudeAboveEllipsoid);
                    }

                    // Does this sentence support geoidal separation?
                    if (sentence is IGeoidalSeparationSentence geoidSeparationSentence)
                    {
                        SetGeoidalSeparation(geoidSeparationSentence.GeoidalSeparation);
                    }

                    #endregion Process altitude data
                }
            }

            #region Lower-priority information

            // Is this a fix mode sentence?
            if (sentence is IFixModeSentence fixModeSentence)
            {
                SetFixMode(fixModeSentence.FixMode);
            }

            // Does this sentence have fix status?
            if (sentence is IFixStatusSentence fixedSentence)
            {
                SetFixStatus(fixedSentence.FixStatus);
            }

            // Does this sentence support magnetic variation?
            if (sentence is IMagneticVariationSentence magVarSentence)
            {
                SetMagneticVariation(magVarSentence.MagneticVariation);
            }

            // Process satellite data
            if (sentence is ISatelliteCollectionSentence satelliteSentence)
            {
                /* GPS.NET 2.0 performed thorough comparison of satellites in order to update
                 * an *existing* instance of Satellite objects.  I think now that this was overkill.
                 * For performance and limited memory use, satellites are just overwritten.
                 */
                AppendSatellites(satelliteSentence.Satellites);
            }

            // Fixed satellite count
            if (sentence is IFixedSatelliteCountSentence fixedCountSentence)
            {
                SetFixedSatelliteCount(fixedCountSentence.FixedSatelliteCount);
            }

            // Process fixed satellites
            if (sentence is IFixedSatellitesSentence fixedSatellitesSentence)
            {
                SetFixedSatellites(fixedSatellitesSentence.FixedSatellites);
            }

            #endregion Lower-priority information
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Occurs when the interpreter is using a different device for raw data.
        /// </summary>
        protected override void OnDeviceChanged()
        {
            // Wrap it in an NMEA stream
            _stream = new NmeaReader(Device.BaseStream);
        }

        /// <summary>
        /// Occurs when new data should be read from the underlying device.
        /// </summary>
        protected override void OnReadPacket()
        {
            // Read a sentence from the underlying stream
            NmeaSentence sentence = _stream.ReadTypedSentence();

            // If we have a sentence, the device is NMEA!  Flag it if needed.
            if (Device != null && !Device.IsGpsDevice)
            {
                Device.SetIsGpsDevice(true);
            }

            /* The NmeaInterpreter in GPS.NET 3.0 uses a slimmed-down architecture to reduce
             * the memory and CPU footprint of processing NMEA data.
             */

            // All data updates in a single shot to prevent race conditions
            lock (DataChangeSyncRoot)
            {
                // Parse the sentence
                Parse(sentence);
            }

            // If we're recording, output the sentence
            lock (RecordingSyncRoot)
            {
                if (RecordingStream != null)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(sentence.Sentence + "\r\n");
                    RecordingStream.Write(buffer, 0, buffer.Length);
                    OnSentenceRecorded(sentence);
                }
            }
#if DEBUG
            // Notify of the sentence
            OnSentenceReceived(sentence);
#endif
        }

        /// <summary>
        /// Occurs immediately after the interpreter has been shut down.
        /// </summary>
        protected override void OnStopped()
        {
            // Clear all values
            Initialize();

            // Continue stopping
            base.OnStopped();
        }

        #endregion Overrides

    }
}