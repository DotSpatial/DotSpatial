using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DotSpatial.Positioning.Gps.IO;

namespace DotSpatial.Positioning.Gps.Nmea
{
    /// <summary>
    /// Represents an interpreter for GPS data from the National Marine Electronics Association (NMEA).
    /// </summary>
    public class NmeaInterpreter : Interpreter
    {
        private NmeaReader _Stream;

        /// <summary>
        /// Represents a synchronization object used to prevent changes to GPS data when reading multiple GPS values.
        /// </summary>
        public object DataChangeSyncRoot = new object();

        #region Constructors

        /// <summary>
        /// Creates an interpreter to read NMEA streams
        /// </summary>
        public NmeaInterpreter()
        {
            Initialize();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a new line of NMEA data has arrived.
        /// </summary>
        [Obsolete("This event is intended for debugging.  Use it to confirm that GPS data is arriving from your GPS device, but avoid it during production use to improve your application's performance.")]
        public event EventHandler<NmeaSentenceEventArgs> SentenceReceived;

        ///// <summary>
        ///// Obsolete.  See compiler warnings for upgrade help.
        ///// </summary>
        //[Obsolete("In GPS.NET 3.0, valid NMEA sentences can be partially processed.  Use the 'SentenceReceived' event to examine the flow of raw GPS data.")]
        //public event EventHandler<NmeaSentenceEventArgs> SentenceInterpreted;

        /// <summary>
        /// Occurs when a packet of data has been recorded to the recording stream.
        /// </summary>
        public event EventHandler<NmeaSentenceEventArgs> SentenceRecorded;

        /// <summary>
        /// SentanceRecieved event handler
        /// </summary>
        /// <param name="sentence"></param>
        protected void OnSentenceReceived(NmeaSentence sentence)
        {
            if (SentenceReceived != null)
                SentenceReceived(this, new NmeaSentenceEventArgs(sentence));
        }

        protected void OnSentenceRecorded(NmeaSentence sentence)
        {
            if (SentenceRecorded != null)
                SentenceRecorded(this, new NmeaSentenceEventArgs(sentence));
        }

        #endregion

        #region Public Methods


        public void Parse(NmeaSentence sentence)
        {
            /* NMEA data is parsed in a specific order to maximize data quality and also to reduce latency
             * problems.  The date/time is processed first to minimize latency.  Then, dilution of precision
             * values are processed.  Finally, if precision is good enough to work with, remaining values are 
             * processed.
             */

            // Is this a fix method message?
            IFixMethodSentence fixMethodSentence = sentence as IFixMethodSentence;
            if (fixMethodSentence != null)
                SetFixMethod(fixMethodSentence.FixMethod);

            // Is this a fix quality message?
            IFixQualitySentence fixQualitySentence = sentence as IFixQualitySentence;
            if (fixQualitySentence != null)
                SetFixQuality(fixQualitySentence.FixQuality);

            #region Process common GPS information

            // If a fix is required, don't process time
            if (!IsFixRequired || (IsFixRequired && IsFixed))
            {
                // Does this sentence support the UTC date and time?
                IUtcDateTimeSentence dateTimeSentence = sentence as IUtcDateTimeSentence;
                if (dateTimeSentence != null)
                    SetDateTimes(dateTimeSentence.UtcDateTime);

                /* Some NMEA sentences provide UTC time information, but no date!  To make this work,
                 * we must combine the time report with the current UTC date.
                 */
                IUtcTimeSentence timeSentence = sentence as IUtcTimeSentence;
                if (timeSentence != null && !timeSentence.UtcTime.Equals(TimeSpan.MinValue) && !timeSentence.UtcTime.Equals(TimeSpan.Zero))
                    SetDateTimes(new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day,
                                        timeSentence.UtcTime.Hours, timeSentence.UtcTime.Minutes, timeSentence.UtcTime.Seconds, timeSentence.UtcTime.Milliseconds,
                                        DateTimeKind.Utc));
            }

            // Does this sentence support horizontal DOP?
            IHorizontalDilutionOfPrecisionSentence hdopSentence = sentence as IHorizontalDilutionOfPrecisionSentence;
            if (hdopSentence != null)
                SetHorizontalDilutionOfPrecision(hdopSentence.HorizontalDilutionOfPrecision);

            // Does the sentence support vertical DOP?
            IVerticalDilutionOfPrecisionSentence vdopSentence = sentence as IVerticalDilutionOfPrecisionSentence;
            if (vdopSentence != null)
                SetVerticalDilutionOfPrecision(vdopSentence.VerticalDilutionOfPrecision);

            // Does the sentence support mean DOP?
            IPositionDilutionOfPrecisionSentence mdopSentence = sentence as IPositionDilutionOfPrecisionSentence;
            if (mdopSentence != null)
                SetMeanDilutionOfPrecision(mdopSentence.PositionDilutionOfPrecision);

            #endregion

            // If a fix is required, don't process time
            if (!IsFixRequired || (IsFixRequired && IsFixed))
            {
                // Is precision good enough to work with?
                if (this.HorizontalDilutionOfPrecision.Value <= this.MaximumHorizontalDilutionOfPrecision.Value)
                {
                    #region Process real-time positional data

                    // Does this sentence support lat/long info?
                    IPositionSentence positionSentence = sentence as IPositionSentence;
                    if (positionSentence != null)
                        SetPosition(positionSentence.Position);

                    // Does this sentence support bearing?
                    IBearingSentence bearingSentence = sentence as IBearingSentence;
                    if (bearingSentence != null)
                        SetBearing(bearingSentence.Bearing);

                    // Does this sentence support speed?
                    ISpeedSentence speedSentence = sentence as ISpeedSentence;
                    if (speedSentence != null)
                        SetSpeed(speedSentence.Speed);

                    #endregion
                }

                // Is the vertical DOP low enough to be worth processing?
                if (this.VerticalDilutionOfPrecision.Value <= this.MaximumVerticalDilutionOfPrecision.Value)
                {
                    #region Process altitude data

                    // Does this sentence support altitude?
                    IAltitudeSentence altitudeSentence = sentence as IAltitudeSentence;
                    if (altitudeSentence != null)
                        SetAltitude(altitudeSentence.Altitude);

                    // Does this sentence support altitude?
                    IAltitudeAboveEllipsoidSentence altitudeAboveEllipsoidSentence = sentence as IAltitudeAboveEllipsoidSentence;
                    if (altitudeAboveEllipsoidSentence != null)
                        SetAltitude(altitudeAboveEllipsoidSentence.AltitudeAboveEllipsoid);

                    // Does this sentence support geoidal separation?
                    IGeoidalSeparationSentence geoidSeparationSentence = sentence as IGeoidalSeparationSentence;
                    if (geoidSeparationSentence != null)
                        SetGeoidalSeparation(geoidSeparationSentence.GeoidalSeparation);

                    #endregion
                }
            }

            #region Lower-priority information

            // Is this a fix mode sentence?
            IFixModeSentence fixModeSentence = sentence as IFixModeSentence;
            if (fixModeSentence != null)
                SetFixMode(fixModeSentence.FixMode);

            // Does this sentence have fix status?
            IFixStatusSentence fixedSentence = sentence as IFixStatusSentence;
            if (fixedSentence != null)
                SetFixStatus(fixedSentence.FixStatus);

            // Does this sentence support magnetic variation?
            IMagneticVariationSentence magVarSentence = sentence as IMagneticVariationSentence;
            if (magVarSentence != null)
                SetMagneticVariation(magVarSentence.MagneticVariation);

            // Process satellite data
            ISatelliteCollectionSentence satelliteSentence = sentence as ISatelliteCollectionSentence;
            if (satelliteSentence != null)
            {
                /* GPS.NET 2.0 performed thorough comparison of satellites in order to update
                 * an *existing* instance of Satellite objects.  I think now that this was overkill.
                 * For performance and limited memory use, satellites are just overwritten.
                 */
                AppendSatellites(satelliteSentence.Satellites);
            }

            // Fixed satellite count
            IFixedSatelliteCountSentence fixedCountSentence = sentence as IFixedSatelliteCountSentence;
            if (fixedCountSentence != null)
                SetFixedSatelliteCount(fixedCountSentence.FixedSatelliteCount);

            // Process fixed satellites
            IFixedSatellitesSentence fixedSatellitesSentence = sentence as IFixedSatellitesSentence;
            if (fixedSatellitesSentence != null)
            {
                SetFixedSatellites(fixedSatellitesSentence.FixedSatellites);
            }

            #endregion
        }

        #endregion

        #region Overrides

        protected override void OnDeviceChanged()
        {
            // Wrap it in an NMEA stream
            _Stream = new NmeaReader(Device.BaseStream);
        }

        protected override void OnReadPacket()
        {
            // Read a sentence from the underlying stream
            NmeaSentence sentence = _Stream.ReadTypedSentence();

            // If we have a sentence, the device is NMEA!  Flag it if needed.
            if (Device != null && !Device.IsGpsDevice)
                Device.SetIsGpsDevice(true);

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
            lock(RecordingSyncRoot)
            {
                if (RecordingStream != null)
                {
                    byte[] buffer = ASCIIEncoding.ASCII.GetBytes(sentence.Sentence + "\r\n");
                    RecordingStream.Write(buffer, 0, buffer.Length);
                    OnSentenceRecorded(sentence);
                }
            }

            // Notify of the sentence
            OnSentenceReceived(sentence);
        }

        protected override void OnStopped()
        {
            // Clear all values
            Initialize();

            // Continue stopping
            base.OnStopped();
        }

        #endregion

        //#region GPS.NET 2.0 upgrade help
        
        ///// <summary>
        ///// Obsolete.  See compiler warnings for upgrade help.
        ///// </summary>
        //[Obsolete("Timeouts are now controlled at the device level.  You can modify the static 'DefaultReadTimeout' property in the devices class.  If this line of code is part of a Form, you can safely delete this line to use default values.")]
        //public TimeSpan WriteTimeout
        //{
        //    get { throw new NotSupportedException(); }
        //    set { throw new NotSupportedException(); }
        //}

        ///// <summary>
        ///// Obsolete.  See compiler warnings for upgrade help.
        ///// </summary>
        //[Obsolete("Timeouts are now controlled at the device level.  You can modify the static 'DefaultReadTimeout' property in the devices class.  If this line of code is part of a Form, you can safely delete this line to use default values.")]
        //public TimeSpan ReadTimeout
        //{
        //    get { throw new NotSupportedException(); }
        //    set { throw new NotSupportedException(); }
        //}

        //#endregion
    }
}
