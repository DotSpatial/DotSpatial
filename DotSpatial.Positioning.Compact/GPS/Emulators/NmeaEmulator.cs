using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DotSpatial.Positioning.Gps.Nmea;
using DotSpatial.Positioning;
using DotSpatial.Positioning.Drawing;

namespace DotSpatial.Positioning.Gps.Emulators
{
    /// <summary>
    /// Emultor for MNEA devices
    /// </summary>
    public class NmeaEmulator : Emulator
    {
        private DateTime _GpggaLastSent = DateTime.UtcNow;
        private DateTime _GpgsaLastSent = DateTime.UtcNow;
        private DateTime _GpgllLastSent = DateTime.UtcNow;
        private DateTime _GpgsvLastSent = DateTime.UtcNow;
        private DateTime _GprmcLastSent = DateTime.UtcNow;
		private TimeSpan _GpggaInterval = TimeSpan.FromSeconds(1);
        private TimeSpan _GpgsaInterval = TimeSpan.FromSeconds(1);
        private TimeSpan _GpgllInterval = TimeSpan.FromSeconds(1);
        private TimeSpan _GpgsvInterval = TimeSpan.FromSeconds(5);
        private TimeSpan _GprmcInterval = TimeSpan.FromSeconds(1);

        // Emulation settings
        private DilutionOfPrecision _HorizontalDOP = DilutionOfPrecision.Good;
        private DilutionOfPrecision _VerticalDOP = DilutionOfPrecision.Good;
        private DilutionOfPrecision _MeanDOP = DilutionOfPrecision.Good;
        private FixMode _FixMode = FixMode.Automatic;
        private FixStatus _FixStatus = FixStatus.Fix;
        private FixMethod _FixMethod = FixMethod.Fix3D;
        private FixQuality _FixQuality = FixQuality.Simulated;
        private Longitude _MagneticVariation = new Longitude(1.0);

        // Random emulation variables
        private double _minHDOP = 1;
        private double _maxHDOP = 6;
        private double _minVDOP = 1;
        private double _maxVDOP = 6;

        /// <summary>
        /// Creates a generic NMEA-0183 Emulator
        /// </summary>
        public NmeaEmulator()
            : this("Generic NMEA-0183 Emulator (http://dotspatial.codeplex.com)")
        { }

        /// <summary>
        /// Creates a generic NMEA-0183 Emulator from the specified string name
        /// </summary>
        ///<param name="name"></param>        
        public NmeaEmulator(string name)
            : base(name)
        { }

        ///// <summary>
        ///// Copies the settings of the NMEA Emulator.
        ///// </summary>
        ///// <returns> A new NMEA Emulator with the same settings. </returns>
        //public override Emulator Clone()
        //{
        //    // Make a new emulator
        //    NmeaEmulator emulator = (NmeaEmulator)Clone(new NmeaEmulator());

        //    emulator._HorizontalDOP = _HorizontalDOP;
        //    emulator._VerticalDOP = _VerticalDOP;
        //    emulator._MeanDOP = _MeanDOP;
        //    emulator._FixMode = _FixMode;
        //    emulator._FixStatus = _FixStatus;
        //    emulator._FixMethod = _FixMethod;
        //    emulator._FixQuality = _FixQuality;
        //    emulator._MagneticVariation = _MagneticVariation;

        //    emulator._minHDOP = _minHDOP;
        //    emulator._maxHDOP = _maxHDOP;
        //    emulator._minVDOP = _minVDOP;
        //    emulator._maxVDOP = _maxVDOP;

        //    return emulator;
        //}

        /// <summary>
        /// Sets the update intervals for the NMEA Emulator's sentence generation.
        /// </summary>
        public override TimeSpan Interval
        {
            get
            {
                return base.Interval;
            }
            set
            {
                base.Interval = value;
                _GpggaInterval = _GpgsaInterval = _GpgllInterval = _GprmcInterval = value;
                _GpgsvInterval = TimeSpan.FromMilliseconds(value.TotalMilliseconds * 5);
            }
        }

        public override void Randomize()
        {
            // Randomize the base emulation for speed/bearing
            base.Randomize();

            _HorizontalDOP = new DilutionOfPrecision((float)(Seed.NextDouble() * (_maxHDOP - _minHDOP) + _minHDOP));
            _VerticalDOP = new DilutionOfPrecision((float)(Seed.NextDouble() * (_maxVDOP - _minVDOP) + _minVDOP));

            // Mean is hypotenuse of the (X,Y,Z,n) axes.
            _MeanDOP = new DilutionOfPrecision((float)Math.Sqrt(Math.Pow(_HorizontalDOP.Value, 2) + Math.Pow(_VerticalDOP.Value, 2)));

            lock (Satellites)
            {
                if (Satellites.Count == 0)
                {
                    int sats = Seed.Next(4, 12);

                    //Satellites.Add(new Satellite(32, new Azimuth(225), new Elevation(45), new SignalToNoiseRatio(25)));

                    Satellites.Add(new Satellite(32, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 1)
                        Satellites.Add(new Satellite(24, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 2)
                        Satellites.Add(new Satellite(25, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 3)
                        Satellites.Add(new Satellite(26, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 4)
                        Satellites.Add(new Satellite(27, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 5)
                        Satellites.Add(new Satellite(16, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 6)
                        Satellites.Add(new Satellite(14, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 7)
                        Satellites.Add(new Satellite(6, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 8)
                        Satellites.Add(new Satellite(7, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 9)
                        Satellites.Add(new Satellite(4, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 10)
                        Satellites.Add(new Satellite(19, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                    if (sats > 11)
                        Satellites.Add(new Satellite(8, new Azimuth(Seed.Next(360)), new Elevation(Seed.Next(90)), new SignalToNoiseRatio(Seed.Next(50))));
                }
            }

            SetRandom(true);
        }

        public void Randomize(
            DilutionOfPrecision maxHDOP,
            DilutionOfPrecision maxVDOP)
        {
            _minHDOP = 1;
            _maxHDOP = maxHDOP.Value;
            _minVDOP = 1;
            _maxVDOP = maxVDOP.Value;

            SetRandom(true);
        }

        public void Randomize(
            DilutionOfPrecision minHDOP,
            DilutionOfPrecision maxHDOP,
            DilutionOfPrecision minVDOP,
            DilutionOfPrecision maxVDOP)
        {
            _minHDOP = minHDOP.Value;
            _maxHDOP = maxHDOP.Value;
            _minVDOP = minVDOP.Value;
            _maxVDOP = maxVDOP.Value;

            SetRandom(true);
        }

        public void Randomize(
            Random seed, 
            Speed speedLow, 
            Speed speedHigh, 
            Azimuth bearingStart, 
            Azimuth bearingArc,
            DilutionOfPrecision minHDOP,
            DilutionOfPrecision maxHDOP,
            DilutionOfPrecision minVDOP,
            DilutionOfPrecision maxVDOP)
        {
            base.Randomize(seed, speedLow, speedHigh, bearingStart, bearingArc);

            _minHDOP = minHDOP.Value;
            _maxHDOP = maxHDOP.Value;
            _minVDOP = minVDOP.Value;
            _maxVDOP = maxVDOP.Value;

            SetRandom(true);
        }

		/// <summary>
		/// Generates actual data to send to the client.
		/// </summary>
		/// <remarks>Data is sent according to the behavior of a typical GPS device: $GPGGA,
		/// $GPGSA, $GPRMC, $GPGSV sentences are sent every second, and a $GPGSV sentence 
		/// is sent every five seconds.  
		/// Developers who want to emulate a specific model of GPS device should override this
		/// method and generate the sentences specific to that device.</remarks>
		protected override void OnEmulation()
		{
            // Update real-time position, speed, bearing, etc.
            base.OnEmulation();

            if (Route.Count == 0)
                CurrentPosition = EmulatePositionError(CurrentPosition);

            /* NMEA devices will transmit "bursts" of NMEA sentences, followed by a one-second pause.
             * Other sentences (usually $GPGSV) are transmitted once every few seconds.  This emulator,
             * by default, will transmit the most common NMEA sentences.
             */

			// $GPGGA
            if (!_GpggaInterval.Equals(TimeSpan.Zero)
                // Has enough time elapsed to send the sentence?
                && UtcDateTime.Subtract(_GpggaLastSent) > _GpggaInterval)
            {
                // Get the tracked satellite count
                int trackedCount = 0;
                foreach (Satellite item in Satellites)
                    if (item.SignalToNoiseRatio.Value > 0)
                        trackedCount++;

                // Yes
                _GpggaLastSent = UtcDateTime;
                
                // Queue the sentence to the read buffer
                WriteSentenceToClient(new GpggaSentence(UtcDateTime.TimeOfDay, CurrentPosition, _FixQuality, trackedCount,
                    _HorizontalDOP, Altitude.Add(EmulateError(_VerticalDOP)), Distance.Empty, TimeSpan.Zero, -1)); //Add an error to the altitude written to the client but don't change the actual value (otherwise it will "walk")
            }

            // $GPRMC
            if (!_GprmcInterval.Equals(TimeSpan.Zero)
                // Has enough time elapsed to send the sentence?
                && UtcDateTime.Subtract(_GprmcLastSent) > _GprmcInterval)
            {
                // Yes
                _GprmcLastSent = UtcDateTime;

                // Queue the sentence to the read buffer
                WriteSentenceToClient(new GprmcSentence(UtcDateTime, _FixStatus == FixStatus.Fix, CurrentPosition, Speed,
                    Bearing, _MagneticVariation));
            }

            // $GPGLL
            if (!_GpgllInterval.Equals(TimeSpan.Zero)
                // Has enough time elapsed to send the sentence?
                && UtcDateTime.Subtract(_GpgllLastSent) > _GpgllInterval)
            {
                // Yes
                _GpgllLastSent = UtcDateTime;

                // Write a $GPGLL to the client
                WriteSentenceToClient(new GpgllSentence(CurrentPosition, UtcDateTime.TimeOfDay, _FixStatus));
            }

			// $GPGSA
            if (!_GpgsaInterval.Equals(TimeSpan.Zero)
                // Has enough time elapsed to send the sentence?
                && UtcDateTime.Subtract(_GpgsaLastSent) > _GpgsaInterval)
            {
                // Yes
                _GpgsaLastSent = UtcDateTime;

                // Queue the sentence to the read buffer
                WriteSentenceToClient(new GpgsaSentence(_FixMode, _FixMethod, Satellites,
                    _MeanDOP, _HorizontalDOP, _VerticalDOP));
            }			

			// $GPGSV
            if (!_GpgsvInterval.Equals(TimeSpan.Zero)
                // Has enough time elapsed to send the sentence?
                && UtcDateTime.Subtract(_GpgsvLastSent) > _GpgsvInterval)
            {
                // Build a list of sentences from our satellites
                IList<GpgsvSentence> sentences = GpgsvSentence.FromSatellites(Satellites);

                // Yes
                _GpgsvLastSent = UtcDateTime;

                // Write each sentence to the read buffer
                foreach (GpgsvSentence gpgsv in sentences)
                {
                    WriteSentenceToClient(gpgsv);
                }
            }			

            // And signal that we have data (or not)
            if (ReadBuffer.Count == 0)
                ReadDataAvailableWaitHandle.Reset();
            else
                ReadDataAvailableWaitHandle.Set();

		}

        protected virtual Position EmulatePositionError(Position truth)
        {
            // Introduce the error
            return truth.TranslateTo(new Angle(Seed.NextDouble() * 360), EmulateError(_HorizontalDOP));
        }

        protected virtual Distance EmulateError(DilutionOfPrecision dop)
        {
            // Calculate the error variance
            //return Distance.FromMeters((Seed.NextDouble() * dop.Value) + DilutionOfPrecision.CurrentAverageDevicePrecision.ToMeters().Value); really? isn't that what the estimated precision is for and shouldn't it be +/- the estimated precision range divided by 2 as below
            return Distance.FromMeters(dop.EstimatedPrecision.ToMeters().Value * (Seed.NextDouble() - 0.5));

        }


        protected void WriteSentenceToClient(NmeaSentence sentence)
        {
            // Get a byte array of the sentence
            byte[] sentenceBytes = sentence.ToByteArray();

            /* Some customers were found to make an emulator, but then not actually read from it. 
             * To prevent huge buffers, we will only write a sentence if the buffer can handle it.
             * Otherwise, we'll ignore it completely.
             */
            if (ReadBuffer.Count + sentenceBytes.Length + 2 > ReadBuffer.Capacity)
                return;

            // Add the bytes
            ReadBuffer.AddRange(sentenceBytes);

            // Add a CrLf
            ReadBuffer.Add(13);
            ReadBuffer.Add(10);
        }

        public DilutionOfPrecision HorizontalDilutiuonOfPrecision
        {
            get { return _HorizontalDOP; }
            set
            {
                _HorizontalDOP = value;
                SetRandom(false);
            }
        }

        public DilutionOfPrecision VerticalDilutiuonOfPrecision
        {
            get { return _VerticalDOP; }
            set
            {
                _VerticalDOP = value; 
                SetRandom(false);
            }
        }

        public DilutionOfPrecision MeanDilutiuonOfPrecision
        {
            get { return _MeanDOP; }
            set
            {
                _MeanDOP = value;
                SetRandom(false);
            }
        }

        public FixMode EmulatedFixMode
        {
            get { return _FixMode; }
            set { _FixMode = value; }
        }

        public FixStatus EmulatedFixStatus
        {
            get { return _FixStatus; }
            set { _FixStatus = value; }
        }

        public FixMethod EmulatedFixMethod
        {
            get { return _FixMethod; }
            set { _FixMethod = value; }
        }

        public FixQuality EmulatedFixQuality
        {
            get { return _FixQuality; }
            set { _FixQuality = value; }
        }

        public Longitude MagneticVariation
        {
            get { return _MagneticVariation; }
            set { _MagneticVariation = value; }
        }
    }
}
