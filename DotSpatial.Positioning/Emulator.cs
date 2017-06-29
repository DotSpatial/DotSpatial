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
// ********************************************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a simulated GPS device.
    /// </summary>
    public abstract class Emulator : Stream
    {
        /// <summary>
        ///
        /// </summary>
        private readonly string _name;
        // Stores data to be read by the client
        /// <summary>
        ///
        /// </summary>
        private ManualResetEvent _readDataAvailableWaitHandle;
        /// <summary>
        ///
        /// </summary>
        private List<byte> _readBuffer;
        // Stores data sent by the client
        /// <summary>
        ///
        /// </summary>
        private ManualResetEvent _writeDataAvailableWaitHandle;
        /// <summary>
        ///
        /// </summary>
        private List<byte> _writeBuffer;

        /// <summary>
        ///
        /// </summary>
        private Thread _thread;
        /// <summary>
        ///
        /// </summary>
        private ManualResetEvent _emulationIntervalWaitHandle;

        // Default timeouts for reading and writing
        /// <summary>
        ///
        /// </summary>
        private TimeSpan _readTimeout = _defaultReadTimeout;
        /// <summary>
        ///
        /// </summary>
        private TimeSpan _writeTimeout = _defaultWriteTimeout;
        // Signals when the threadproc is done and associated handles/objects can be safely disposed.

        /// <summary>
        ///
        /// </summary>
        private bool _isDisposed;
        /// <summary>
        ///
        /// </summary>
        private bool _isRunning;

        // Controls the date/time when stats were last recalculated
        /// <summary>
        ///
        /// </summary>
        private DateTime _utcDateTime;
        // GPS information
        /// <summary>
        ///
        /// </summary>
        private Speed _speed;
        /// <summary>
        ///
        /// </summary>
        private Azimuth _bearing;
        /// <summary>
        ///
        /// </summary>
        private Position _currentPosition;
        /// <summary>
        ///
        /// </summary>
        private Position _currentDestination;
        /// <summary>
        ///
        /// </summary>
        private Distance _altitude;
        /// <summary>
        ///
        /// </summary>
        private List<Position> _route;
        /// <summary>
        ///
        /// </summary>
        private int _routeIndex;
        /// <summary>
        ///
        /// </summary>
        private readonly List<Satellite> _satellites;
        /// <summary>
        ///
        /// </summary>
        private FixQuality _fixQuality;
        /// <summary>
        ///
        /// </summary>
        private FixMode _fixMode;
        /// <summary>
        ///
        /// </summary>
        private FixMethod _fixMethod;
        /// <summary>
        ///
        /// </summary>
        private FixStatus _fixStatus;
        /// <summary>
        ///
        /// </summary>
        private DilutionOfPrecision _horizontalDop;
        /// <summary>
        ///
        /// </summary>
        private DilutionOfPrecision _verticalDop;
        /// <summary>
        ///
        /// </summary>
        private DilutionOfPrecision _meanDop;
        /// <summary>
        ///
        /// </summary>
        private TimeSpan _interval = TimeSpan.FromSeconds(1);

        // Random emulation variables
        /// <summary>
        ///
        /// </summary>
        private Random _seed;
        /// <summary>
        ///
        /// </summary>
        private double _speedLow;
        /// <summary>
        ///
        /// </summary>
        private double _speedHigh;
        /// <summary>
        ///
        /// </summary>
        private double _bearingStart;
        /// <summary>
        ///
        /// </summary>
        private double _bearingArc;
        /// <summary>
        ///
        /// </summary>
        private bool _isRandom;

#if PocketPC
        private bool _IsEmulationThreadAlive;
#endif

        /// <summary>
        ///
        /// </summary>
        private static int _defaultReadBufferSize = 4096;
        /// <summary>
        ///
        /// </summary>
        private static int _defaultWriteBufferSize = 4096;
        /// <summary>
        ///
        /// </summary>
        private static TimeSpan _defaultReadTimeout = TimeSpan.FromSeconds(5);
        /// <summary>
        ///
        /// </summary>
        private static TimeSpan _defaultWriteTimeout = TimeSpan.FromSeconds(5);

        #region Static Properties

        /// <summary>
        /// Default Read Buffer Size
        /// </summary>
        /// <value>The default size of the read buffer.</value>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int DefaultReadBufferSize
        {
            get { return _defaultReadBufferSize; }
            set
            {
                if (value < 128)
                    throw new ArgumentOutOfRangeException("DefaultReadBufferSize", "The default read buffer size must be 128 bytes or greater.");

                _defaultReadBufferSize = value;
            }
        }

        /// <summary>
        /// Default Write Buffer Size
        /// </summary>
        /// <value>The default size of the write buffer.</value>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static int DefaultWriteBufferSize
        {
            get { return _defaultWriteBufferSize; }
            set
            {
                if (value < 128)
                    throw new ArgumentOutOfRangeException("DefaultWriteBufferSize", "The default write buffer size must be 128 bytes or greater.");

                _defaultWriteBufferSize = value;
            }
        }

        /// <summary>
        /// Default Read Timeout
        /// </summary>
        /// <value>The default read timeout.</value>
        public static TimeSpan DefaultReadTimeout
        {
            get { return _defaultReadTimeout; }
            set
            {
                if (value.TotalMilliseconds < 500)
                    throw new ArgumentOutOfRangeException("DefaultReadTimeout", "The default read timeout should be 500ms or greater to avoid premature timeout issues.");

                _defaultReadTimeout = value;
            }
        }

        /// <summary>
        /// The Timespan of before the write operation times out.
        /// </summary>
        /// <value>The default write timeout.</value>
        public static TimeSpan DefaultWriteTimeout
        {
            get { return _defaultWriteTimeout; }
            set
            {
                if (value.TotalMilliseconds < 500)
                    throw new ArgumentOutOfRangeException("DefaultWriteTimeout", "The default write timeout should be 500ms or greater to avoid premature timeout issues.");

                _defaultWriteTimeout = value;
            }
        }

        #endregion Static Properties

        #region Events

        /// <summary>
        /// An Excpetion occured
        /// </summary>
        public EventHandler<ExceptionEventArgs> ExceptionOccurred;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.Stream"/> class.
        /// </summary>
        protected Emulator()
            : this(@"GPS.NET NMEA-0183 Emulator (http://dotspatial.codeplex.com)")
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Emulator"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        protected Emulator(string name)
        {
            _name = name;

            // Create new buffers for reading and writing
            _readBuffer = new List<byte>(_defaultReadBufferSize);
            _writeBuffer = new List<byte>(_defaultWriteBufferSize);

            // Initialize simulated values
            _readDataAvailableWaitHandle = new ManualResetEvent(false);
            _writeDataAvailableWaitHandle = new ManualResetEvent(false);
            _emulationIntervalWaitHandle = new ManualResetEvent(false);

            // Default timeouts for reading and writing
            _readTimeout = _defaultReadTimeout;
            _writeTimeout = _defaultWriteTimeout;

            // Simulated values
            _seed = new Random();
            _utcDateTime = DateTime.UtcNow;
            _currentPosition = Positioning.Position.Empty;
            _altitude = Distance.FromFeet(1000);
            _route = new List<Position>();
            _satellites = new List<Satellite>();
            _fixQuality = FixQuality.GpsFix;
            _fixMode = FixMode.Automatic;
            _fixMethod = FixMethod.Fix3D;
            _fixStatus = FixStatus.Fix;
            _horizontalDop = DilutionOfPrecision.Good;
            _verticalDop = DilutionOfPrecision.Good;
            _meanDop = DilutionOfPrecision.Good;

            _speed = Speed.FromStatuteMilesPerHour(20);
            _speedLow = Speed.FromKilometersPerSecond(10).Value;
            _speedHigh = Speed.FromKilometersPerSecond(25).Value;

            _bearing = Azimuth.Southwest;
            _bearingStart = _seed.NextDouble() * 360;
            _bearingArc = 10;
        }

        #endregion Constructors

        #region Emulation thread

        /// <summary>
        /// Open
        /// </summary>
        public void Open()
        {
            // It's already open
            if (_thread != null)
                return;

            _isRunning = true;

            // Create a thread which processes the incoming (from the client) and outgoing (to the client) data
            _thread = new Thread(EmulatorThreadProc)
                          {
                              IsBackground = true,
                              Priority = ThreadPriority.Normal,
                              Name = _name
                          };
            _thread.Start();

#if PocketPC
            _IsEmulationThreadAlive = true;
#endif
        }

        /// <summary>
        /// Emulators the thread proc.
        /// </summary>
        private void EmulatorThreadProc()
        {
            try
            {
                while (!_isDisposed && _isRunning)
                {
                    // Call the emulation routine
                    OnEmulation();

                    /* Thread.Sleep is uninterruptable, causing the emulator to appear to
                     * hang for the length of the interval during shutdown.
                     */

                    _emulationIntervalWaitHandle.WaitOne((int)Interval.TotalMilliseconds, false);
                }
            }
            catch (ThreadAbortException)
            {
                // Nuthin
            }
            catch (Exception ex)
            {
                // Raise the error via an event
                if (ExceptionOccurred != null)
                    ExceptionOccurred(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Called when [emulation].
        /// </summary>
        protected virtual void OnEmulation()
        {
            /* The emulator, on its own, will update real-time simulated values such
             * as the position, bearing, altitude, and speed.
             */

            // How much time has passed since the last emulation?
            TimeSpan elapsedTime = DateTime.UtcNow.Subtract(_utcDateTime);

            // Set the new update date
            _utcDateTime = DateTime.UtcNow;

            // How much have we moved since the last update?
            Distance distanceMoved = _speed.ToDistance(elapsedTime);

            // Do we have a route?
            if (_route.Count == 0)
            {
                // Randomize the speed and direction if required.
                if (_isRandom) Randomize();

                // No.  Just move in a straight line
                _currentPosition = _currentPosition.TranslateTo(_bearing, distanceMoved);
            }
            else
            {
                // How close are we to the next route point?
                Distance toWaypoint = _currentPosition.DistanceTo(_currentDestination);

                // Is the distance to the waypoint small?
                if (toWaypoint.ToMeters().Value < _speed.ToDistance(Interval).ToMeters().Value)
                {
                    // Close enough to "snap" to the waypoint
                    _currentPosition = _currentDestination;

                    // Select the next position in the route as our destination.
                    _routeIndex++;
                    if (_routeIndex > _route.Count - 1)
                        _routeIndex = 0;

                    // Set the next destination point
                    _currentDestination = _route[_routeIndex];
                }
                else
                {
                    _currentPosition = _currentPosition.TranslateTo(_bearing, distanceMoved);
                }

                // Get the bearing to the next destination
                _bearing = _currentPosition.BearingTo(_currentDestination);
            }
        }

        #endregion Emulation thread

        #region Private/Protected Members

        /// <summary>
        /// Gets a value indicating whether this instance is emulation thread alive.
        /// </summary>
        private bool IsEmulationThreadAlive
        {
            get
            {
#if !PocketPC
                return _thread.IsAlive;
#else
                return _IsEmulationThreadAlive;
#endif
            }
        }

        /// <summary>
        /// Returns the current randomizer for emulation.
        /// </summary>
        protected Random Seed
        {
            get { return _seed; }
        }

        /// <summary>
        /// Returns the amount of time the emulator waits before processing new data.
        /// </summary>
        /// <value>The interval.</value>
        public virtual TimeSpan Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        /// <summary>
        /// Toggles the IsRandom flag.
        /// </summary>
        /// <param name="isRandom">The state to toggle the flag.</param>
        protected void SetRandom(bool isRandom)
        {
            _isRandom = isRandom;
        }

        #endregion Private/Protected Members

        #region Public Members

        /// <summary>
        /// Indicates whether enough satellite signals exist to determine the current location.
        /// </summary>
        public bool IsFixed
        {
            get { return _fixMethod == FixMethod.Fix3D; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the emulator is generating data that
        /// changes in random manner.
        /// </summary>
        public bool IsRandom
        {
            get { return _isRandom; }
        }

        /// <summary>
        /// Randomizes the emulation by changing speed and direction
        /// </summary>
        /// <remarks>GPS coordinate emulation can be randomized by any number of factors, depending on the emulator type used.
        /// Any emulation can have it's direction and speed randomized within specified tolerances. By default, speed is
        /// limited to between 0 (low) and 5 (high) meters/second and bearing changes are limited to +/- 45 degrees
        /// (a 90 degree arc) from North (0) degrees.</remarks>
        public virtual void Randomize()
        {
            // Flag it so the emulation will use random values
            _isRandom = true;

            // Randomize the speed within range
            _speed = Speed.FromMetersPerSecond((_seed.NextDouble() * (_speedHigh - _speedLow)) + _speedLow);

            // Randomize the bearing within the arc.
            _bearing = new Azimuth(_seed.NextDouble() * _bearingArc + (_bearingStart - (_bearingArc * .5))).Normalize();

            // Reset the bearing origin
            _bearingStart = _bearing.DecimalDegrees;
        }

        /// <summary>
        /// Randomizes the emulation by changing speed and direction
        /// </summary>
        /// <param name="seed">The randomizer to use.</param>
        /// <param name="speedLow">The minimum travel speed.</param>
        /// <param name="speedHigh">The maximum travel speed.</param>
        /// <param name="bearingStart">The initial direction of travel.</param>
        /// <param name="bearingArc">The arc in which random directional changes will occur.</param>
        /// <remarks>GPS coordinate emulation can be randomized by any number of factors, depending on the emulator type used.
        /// Any emulation can have it's direction and speed randomized within specified tolerances. By default, speed is
        /// limited to between 0 (low) and 5 (high) meters/second and bearing changes are limited to +/- 45 degrees
        /// (a 90 degree arc) from North (0) degrees.</remarks>
        public void Randomize(Random seed, Speed speedLow, Speed speedHigh, Azimuth bearingStart, Azimuth bearingArc)
        {
            // Flag it so the emulation will use random values
            _isRandom = true;

            // Get the randomizer parameters
            _seed = seed;

            // Get the speed variance
            _speedLow = speedLow.ToMetersPerSecond().Value;
            _speedHigh = speedHigh.ToMetersPerSecond().Value;

            // Get the normalized arc values
            _bearingStart = bearingStart.Normalize().DecimalDegrees;
            _bearingArc = bearingArc.Normalize().DecimalDegrees;
        }

        #endregion Public Members

        /// <summary>
        /// the string Name ofhte emulator
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <inheritdocs/>
        public override string ToString()
        {
            return _name;
        }

        #region Real-time specified

        /// <summary>
        /// The string altitude
        /// </summary>
        /// <value>The altitude.</value>
        public Distance Altitude
        {
            get { return _altitude; }
            set
            {
                _altitude = value;
                _isRandom = false;
            }
        }

        /// <summary>
        /// the speed
        /// </summary>
        /// <value>The speed.</value>
        public Speed Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                _isRandom = false;
            }
        }

        /// <summary>
        /// The Directional azimuth angle
        /// </summary>
        /// <value>The bearing.</value>
        public Azimuth Bearing
        {
            get { return _bearing; }
            set
            {
                // If we're following a route, changing the bearing is not allowed
                if (_route.Count != 0)
                    throw new InvalidOperationException("The current bearing can only be set if there is no route to follow.  With a route specified, the emulator will automatically recalculate the bearing.");

                _bearing = value;
                _isRandom = false;
            }
        }

        /// <summary>
        /// The integer count of fixed satellites
        /// </summary>
        public int FixedSatelliteCount
        {
            get
            {
                // Bump up the count whenever a fixed satellite is found

                return _satellites.Count(satellite => satellite.IsFixed);
            }
        }

        /// <summary>
        /// The quality of the GPS signal
        /// </summary>
        /// <value>The fix quality.</value>
        public FixQuality FixQuality
        {
            get { return _fixQuality; }
            set { _fixQuality = value; }
        }

        /// <summary>
        /// The mode of the signal fix
        /// </summary>
        /// <value>The fix mode.</value>
        public FixMode FixMode
        {
            get { return _fixMode; }
            set { _fixMode = value; }
        }

        /// <summary>
        /// the fix method
        /// </summary>
        /// <value>The fix method.</value>
        public FixMethod FixMethod
        {
            get { return _fixMethod; }
            set { _fixMethod = value; }
        }

        /// <summary>
        /// The status of the fix
        /// </summary>
        /// <value>The fix status.</value>
        public FixStatus FixStatus
        {
            get { return _fixStatus; }
            set { _fixStatus = value; }
        }

        /// <summary>
        /// the Horizontal Dilution of Precision (HPDOP)
        /// </summary>
        /// <value>The horizontal dilution of precision.</value>
        public DilutionOfPrecision HorizontalDilutionOfPrecision
        {
            get { return _horizontalDop; }
            set { _horizontalDop = value; }
        }

        /// <summary>
        /// The Vertical Dilution of Precision (VPDOP)
        /// </summary>
        /// <value>The vertical dilution of precision.</value>
        public DilutionOfPrecision VerticalDilutionOfPrecision
        {
            get { return _verticalDop; }
            set { _verticalDop = value; }
        }

        /// <summary>
        /// The average of the Dilution of precision values.
        /// </summary>
        /// <value>The mean dilution of precision.</value>
        public DilutionOfPrecision MeanDilutionOfPrecision
        {
            get { return _meanDop; }
            set { _meanDop = value; }
        }

        /// <summary>
        /// Gets the list of satellites.
        /// </summary>
        public IList<Satellite> Satellites
        {
            get { return _satellites; }
        }

        /// <summary>
        /// Gets the list of positions that make up this route
        /// </summary>
        public IList<Position> Route
        {
            get { return _route; }
        }

        /// <summary>
        /// Gets the Position structure for the current position
        /// </summary>
        /// <value>The current position.</value>
        public Position CurrentPosition
        {
            get { return _currentPosition; }
            set
            {
                if (_route.Count != 0)
                    throw new InvalidOperationException("The current position can only be set if there is no route to follow.  With a route specified, the emulator will traverse it automatically.");

                _currentPosition = value;
            }
        }

        /// <summary>
        /// Gets or sets the position with the new destination
        /// </summary>
        /// <value>The current destination.</value>
        public Position CurrentDestination
        {
            get { return _currentDestination; }
            set
            {
                if (_route.Count > 0)
                    throw new InvalidOperationException("The destination can only be set if there is no route to follow.  With a route specified, the emulator will traverse it automatically.");

                _currentDestination = value;
            }
        }

        /// <summary>
        /// Gets the DateTime structure for Now
        /// </summary>
        public DateTime DateTime
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Gets the UtcCorrected value for Now
        /// </summary>
        public DateTime UtcDateTime
        {
            get { return DateTime.UtcNow; }
        }

        #endregion Real-time specified

        #region Overrides

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <returns>true if the stream supports reading; otherwise, false.</returns>
        public override bool CanRead
        {
            get { return !_isDisposed; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <returns>true if the stream supports seeking; otherwise, false.</returns>
        public override bool CanSeek
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value that determines whether the current stream can time out.
        /// </summary>
        /// <returns>A value that determines whether the current stream can time out.</returns>
        public override bool CanTimeout
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Flush()
        {
            // Clear all buffers
            _readBuffer.Clear();
            _writeBuffer.Clear();
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.</returns>
        /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length. </exception>
        ///
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null. </exception>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
        ///
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            /* This method is called by the client.  If no data is available
             * to read, the method will block for a moment for new data.  The
             * emulation thread will add to the buffer.
             */

            // If there's nothing to read, block a while for data
            if (_readBuffer.Count == 0
                // Wait for data.  If there is none, throw a big baby tantrum
#if PocketPC
                && !_ReadDataAvailableWaitHandle.WaitOne(ReadTimeout, false)
#else
                && !_readDataAvailableWaitHandle.WaitOne(_readTimeout)
#endif
)
            {
                throw new TimeoutException("The emulator has not generated any data within the read timeout period.");
            }

            // Calculate the number of bytes to read
            int bytesToRead = _readBuffer.Count < count ? _readBuffer.Count : count;

            // Copy the read buffer to the target array
            _readBuffer.CopyTo(offset, buffer, 0, bytesToRead);

            // Remove the read bytes from the buffer
            _readBuffer.RemoveRange(0, bytesToRead);

            // If the buffer is empty, reset
            if (_readBuffer.Count == 0)
                _readDataAvailableWaitHandle.Reset();

            // Return the bytes read
            return bytesToRead;
        }

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
        /// </summary>
        /// <returns>The unsigned byte cast to an Int32, or -1 if at the end of the stream.</returns>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override int ReadByte()
        {
            // If there's nothing to read, block a while for data
            if (_readBuffer.Count == 0)
            {
                // Wait for data.  If there is none, return -1
#if PocketPC
                if (!_ReadDataAvailableWaitHandle.WaitOne(ReadTimeout, false))
#else
                if (!_readDataAvailableWaitHandle.WaitOne(ReadTimeout))
#endif
                    return -1;
            }

            // Anything?  If not, return -1
            if (_readBuffer.Count == 0)
                return -1;

            // Yes.  Get the first byte
            int result = Convert.ToInt32(_readBuffer[0]);

            // And remove it from the list
            _readBuffer.RemoveAt(0);

            // If the buffer is empty, reset
            if (_readBuffer.Count == 0)
                _readDataAvailableWaitHandle.Reset();

            // Return the byte read
            return result;
        }

        /// <summary>
        /// Gets the read buffer.
        /// </summary>
        protected List<byte> ReadBuffer
        {
            get { return _readBuffer; }
        }

        /// <summary>
        /// Gets the read data available wait handle.
        /// </summary>
        protected ManualResetEvent ReadDataAvailableWaitHandle
        {
            get { return _readDataAvailableWaitHandle; }
        }

        /// <summary>
        /// Gets the write buffer.
        /// </summary>
        protected IList<byte> WriteBuffer
        {
            get { return _writeBuffer; }
        }

        /// <summary>
        /// Gets the write data available wait handle.
        /// </summary>
        protected ManualResetEvent WriteDataAvailableWaitHandle
        {
            get { return _writeDataAvailableWaitHandle; }
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length. </exception>
        ///
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="buffer"/> is null. </exception>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
        ///
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            // Write the buffer, byte-by-byte to the incoming buffer
            for (int index = offset; index < offset + count; index++)
                _writeBuffer.Add(buffer[index]);

            // Set the writing wait handle
            _writeDataAvailableWaitHandle.Set();
        }

        /// <summary>
        /// Writes a byte to the current position in the stream and advances the position within the stream by one byte.
        /// </summary>
        /// <param name="value">The byte to write to the stream.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">The stream does not support writing, or the stream is already closed. </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override void WriteByte(byte value)
        {
            // Add the byte
            _writeBuffer.Add(value);

            // Set the writing wait handle
            _writeDataAvailableWaitHandle.Set();
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <value>The position.</value>
        /// <returns>The current position within the stream.</returns>
        ///
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Position
        {
            get
            {
                return 0;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <returns>A long value representing the length of the stream in bytes.</returns>
        ///
        /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
        ///
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Length
        {
            get { return _readBuffer.Count; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <returns>true if the stream supports writing; otherwise, false.</returns>
        public override bool CanWrite
        {
            get
            {
                //  By default, emulators are read-only.  Implementers (such as Garmin or SiRF)
                // will override this.
                return false;
            }
        }

        /// <summary>
        /// Gets or sets a value, in miliseconds, that determines how long the stream will attempt to read before timing out.
        /// </summary>
        /// <value>The read timeout.</value>
        /// <returns>A value, in miliseconds, that determines how long the stream will attempt to read before timing out.</returns>
        ///
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.ReadTimeout"/> method always throws an <see cref="T:System.InvalidOperationException"/>. </exception>
        public override int ReadTimeout
        {
            get
            {
                return Convert.ToInt32(_readTimeout.TotalMilliseconds);
            }
            set
            {
                _readTimeout = TimeSpan.FromMilliseconds(value);
            }
        }

        /// <summary>
        /// Gets or sets a value, in miliseconds, that determines how long the stream will attempt to write before timing out.
        /// </summary>
        /// <value>The write timeout.</value>
        /// <returns>A value, in miliseconds, that determines how long the stream will attempt to write before timing out.</returns>
        ///
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.WriteTimeout"/> method always throws an <see cref="T:System.InvalidOperationException"/>. </exception>
        public override int WriteTimeout
        {
            get
            {
                return Convert.ToInt32(_writeTimeout.TotalMilliseconds);
            }
            set
            {
                _writeTimeout = TimeSpan.FromMilliseconds(value);
            }
        }

        #region Cloning

        // TODO: Cloning isn't necessary for NMEA. I imagine it's not necessary for
        // the text emulator either, so it's removed.

        //public abstract Emulator Clone();

        //protected Emulator Clone(Emulator emulator)
        //{
        //    // Copy over config values
        //    emulator._Altitude = this._Altitude;

        //    // Set values which are only valid when there's no route
        //    if (this._Route.Count == 0)
        //    {
        //        emulator._Bearing = this._Bearing;
        //        if (this._CurrentDestination != null)
        //            emulator._CurrentDestination = this._CurrentDestination;
        //        if (this._CurrentPosition != null)
        //            emulator._CurrentPosition = this._CurrentPosition;

        //        emulator._isRandom = this._isRandom;

        //        emulator._speedLow = _speedLow;
        //        emulator._speedHigh = _speedHigh;
        //        emulator._bearingStart = _bearingStart;
        //        emulator._bearingArc = _bearingArc;
        //    }
        //    else
        //    {
        //        emulator._Route.AddRange(this._Route);
        //    }

        //    return emulator;

        //}

        #endregion Cloning

        /// <summary>
        /// Closes the emulation stream, but doesn't dispose of it.
        /// </summary>
        /// <remarks>The Emulator.Close() method simply terminates the thread that feeds data to
        /// a virtual Device. This allows the emulator to be reused indefinately.</remarks>
        public override void Close()
        {
            if (_emulationIntervalWaitHandle != null)
                _emulationIntervalWaitHandle.Set();

            _isRunning = false;

            // Shut down the emulator
            if (_thread != null && IsEmulationThreadAlive && !_thread.Join(ReadTimeout))
            {
                _thread.Abort();
            }

            _thread = null;

            // Now clean up the handles
            if (_readDataAvailableWaitHandle != null)
                _readDataAvailableWaitHandle.Reset();
            if (_writeDataAvailableWaitHandle != null)
                _writeDataAvailableWaitHandle.Reset();
            if (_emulationIntervalWaitHandle != null)
                _emulationIntervalWaitHandle.Reset();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // If we're already disposed, just exit
            if (_isDisposed)
                return;

            // Indicate that we're disposed
            _isDisposed = true;

            // Stop the thread
            Close();

            // Now clean up the handles
            if (_readDataAvailableWaitHandle != null)
                _readDataAvailableWaitHandle.Close();
            if (_writeDataAvailableWaitHandle != null)
                _writeDataAvailableWaitHandle.Close();
            if (_emulationIntervalWaitHandle != null)
                _emulationIntervalWaitHandle.Close();

            // Are we disposing of managed objects?
            if (disposing)
            {
                _writeBuffer = null;
                _readBuffer = null;
                _readDataAvailableWaitHandle = null;
                _writeDataAvailableWaitHandle = null;
                _emulationIntervalWaitHandle = null;
                _thread = null;
                _route = null;
            }
        }

        #endregion Overrides
    }
}