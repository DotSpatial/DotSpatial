using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DotSpatial.Positioning;
using DotSpatial.Positioning.Gps.Nmea;
using DotSpatial.Positioning.Gps.IO;
using DotSpatial.Positioning.Drawing;

namespace DotSpatial.Positioning.Gps.Emulators
{
    /// <summary>
    /// Represents a simulated GPS device.
    /// </summary>
    public abstract class Emulator : Stream
    {
        private string _Name;
        // Stores data to be read by the client
        private ManualResetEvent _ReadDataAvailableWaitHandle;
        private List<byte> _ReadBuffer;
        // Stores data sent by the client
        private ManualResetEvent _WriteDataAvailableWaitHandle;
        private List<byte> _WriteBuffer;
        
        private Thread _Thread;
        private ManualResetEvent _EmulationIntervalWaitHandle;
        
        // Default timeouts for reading and writing
        private TimeSpan _ReadTimeout = _DefaultReadTimeout;
        private TimeSpan _WriteTimeout = _DefaultWriteTimeout;
        // Signals when the threadproc is done and associated handles/objects can be safely disposed.

        private bool _IsDisposed;
        private bool _IsRunning;
        
        // Controls the date/time when stats were last recalculated
        private DateTime _UtcDateTime;
        // GPS information
        private Speed _Speed;
        private Azimuth _Bearing;
        private Position _CurrentPosition;
        private Position _CurrentDestination;
        private Distance _Altitude;
        private List<Position> _Route;
        private int _RouteIndex;
        private List<Satellite> _Satellites;
        private FixQuality _FixQuality;
        private FixMode _FixMode;
        private FixMethod _FixMethod;
        private FixStatus _FixStatus;
        private DilutionOfPrecision _HorizontalDop;
        private DilutionOfPrecision _VerticalDop;
        private DilutionOfPrecision _MeanDop;
        private TimeSpan _interval = TimeSpan.FromSeconds(1);

        // Random emulation variables
        private Random _seed;
        private double _speedLow;
        private double _speedHigh;
        private double _bearingStart;
        private double _bearingArc;
        private bool _isRandom;

#if PocketPC
        private bool _IsEmulationThreadAlive;
#endif

        private static int _DefaultReadBufferSize = 4096;
        private static int _DefaultWriteBufferSize = 4096;
        private static TimeSpan _DefaultReadTimeout = TimeSpan.FromSeconds(5);
        private static TimeSpan _DefaultWriteTimeout = TimeSpan.FromSeconds(5);

        #region Static Properties

        public static int DefaultReadBufferSize
        {
            get { return _DefaultReadBufferSize; }
            set
            {
                if (value < 128)
                    throw new ArgumentOutOfRangeException("DefaultReadBufferSize", "The default read buffer size must be 128 bytes or greater.");

                _DefaultReadBufferSize = value;
            }
        }

        public static int DefaultWriteBufferSize
        {
            get { return _DefaultWriteBufferSize; }
            set
            {
                if (value < 128)
                    throw new ArgumentOutOfRangeException("DefaultWriteBufferSize", "The default write buffer size must be 128 bytes or greater.");

                _DefaultWriteBufferSize = value;
            }
        }

        public static TimeSpan DefaultReadTimeout
        {
            get { return _DefaultReadTimeout; }
            set
            {
                if (value.TotalMilliseconds < 500)
                    throw new ArgumentOutOfRangeException("DefaultReadTimeout", "The default read timeout should be 500ms or greater to avoid premature timeout issues.");

                _DefaultReadTimeout = value;
            }
        }

        public static TimeSpan DefaultWriteTimeout
        {
            get { return _DefaultWriteTimeout; }
            set
            {
                if (value.TotalMilliseconds < 500)
                    throw new ArgumentOutOfRangeException("DefaultReadTimeout", "The default write timeout should be 500ms or greater to avoid premature timeout issues.");

                _DefaultWriteTimeout = value;
            }
        }

        #endregion

        #region Events

        public EventHandler<ExceptionEventArgs> ExceptionOccurred;

        #endregion

        #region Constructors

        protected Emulator()
            : this(@"GPS.NET NMEA-0183 Emulator (http://dotspatial.codeplex.com)")
        { }

        protected Emulator(string name)
        {
            _Name = name;

            // Create new buffers for reading and writing
            _ReadBuffer = new List<byte>(_DefaultReadBufferSize);
            _WriteBuffer = new List<byte>(_DefaultWriteBufferSize);

            // Initialize simulated values
            _ReadDataAvailableWaitHandle = new ManualResetEvent(false);
            _WriteDataAvailableWaitHandle = new ManualResetEvent(false);
            _EmulationIntervalWaitHandle = new ManualResetEvent(false);

            // Default timeouts for reading and writing
            _ReadTimeout = _DefaultReadTimeout;
            _WriteTimeout = _DefaultWriteTimeout;
            
            // Simulated values
            _seed = new Random();
            _UtcDateTime = DateTime.UtcNow;
            _CurrentPosition = Positioning.Position.Empty;            
            _Altitude = Distance.FromFeet(1000);
            _Route = new List<Position>();
            _Satellites = new List<Satellite>();
            _FixQuality = FixQuality.GpsFix;
            _FixMode = FixMode.Automatic;
            _FixMethod = FixMethod.Fix3D;
            _FixStatus = FixStatus.Fix;
            _HorizontalDop = DilutionOfPrecision.Good;
            _VerticalDop = DilutionOfPrecision.Good;
            _MeanDop = DilutionOfPrecision.Good;

            _Speed = Speed.FromStatuteMilesPerHour(20);
            _speedLow = Speed.FromKilometersPerSecond(10).Value;
            _speedHigh = Speed.FromKilometersPerSecond(25).Value;

            _Bearing = Azimuth.Southwest;
            _bearingStart = _seed.NextDouble() * 360;
            _bearingArc = 10;

        }

        #endregion

        #region Emulation thread

        public void Open()
        {
            // It's already open
            if (_Thread != null)
                return;

            _IsRunning = true;

            // Create a thread which processes the incoming (from the client) and outgoing (to the client) data
            _Thread = new Thread(new ThreadStart(EmulatorThreadProc));
            _Thread.IsBackground = true;
            _Thread.Priority = ThreadPriority.Normal;
            _Thread.Name = _Name;
            _Thread.Start();

#if PocketPC
            _IsEmulationThreadAlive = true;
#endif
        }

        private void EmulatorThreadProc()
        {
            try
            {
                while (!_IsDisposed && _IsRunning)
                {
                    // Call the emulation routine
                    OnEmulation();

                    /* Thread.Sleep is uninterruptable, causing the emulator to appear to
                     * hang for the length of the interval during shutdown.
                     */

                    _EmulationIntervalWaitHandle.WaitOne((int)Interval.TotalMilliseconds, false);
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

        protected virtual void OnEmulation()
        {
            /* The emulator, on its own, will update real-time simulated values such 
             * as the position, bearing, altitude, and speed.
             */

            // How much time has passed since the last emulation?
            TimeSpan elapsedTime = DateTime.UtcNow.Subtract(_UtcDateTime);

            // Set the new update date
            _UtcDateTime = DateTime.UtcNow;

            // How much have we moved since the last update?
            Distance distanceMoved = _Speed.ToDistance(elapsedTime);

            // Do we have a route?
            if (_Route.Count == 0)
            {
                // Randomize the speed and direction if required.
                if (_isRandom) Randomize();

                // No.  Just move in a straight line
                _CurrentPosition = _CurrentPosition.TranslateTo(_Bearing, distanceMoved);
            }
            else
            {
                // How close are we to the next route point?
                Distance toWaypoint = _CurrentPosition.DistanceTo(_CurrentDestination);

                // Is the distance to the waypoint small?
                if (toWaypoint.ToMeters().Value < _Speed.ToDistance(Interval).ToMeters().Value)
                {
                    // Close enough to "snap" to the waypoint
                    _CurrentPosition = _CurrentDestination;

                    // Select the next position in the route as our destination.
                    _RouteIndex++;
                    if (_RouteIndex > _Route.Count - 1)
                        _RouteIndex = 0;

                    // Set the next destination point
                    _CurrentDestination = _Route[_RouteIndex];
                }
                else
                {
                    _CurrentPosition = _CurrentPosition.TranslateTo(_Bearing, distanceMoved);
                }
 
                // Get the bearing to the next destination
                _Bearing = _CurrentPosition.BearingTo(_CurrentDestination);
            }
        }

        #endregion

        #region Private/Protected Members

        private bool IsEmulationThreadAlive
        {
            get
            {
#if !PocketPC
                return _Thread.IsAlive;
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
        public virtual TimeSpan Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        /// <summary>
        /// Toggles the IsRandom flag.
        /// </summary>
        /// <param name="isRandom"> The state to toggle the flag. </param>
        protected void SetRandom(bool isRandom)
        {
            _isRandom = isRandom;
        }

        #endregion

        #region Public Members

        /// <summary>
        /// Indicates whether enough satellite signals exist to determine the current location.
        /// </summary>
        public bool IsFixed
        {
            get { return _FixMethod == FixMethod.Fix3D; }
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
        /// <remarks>
        /// GPS coordinate emulation can be randomized by any number of factors, depending on the emulator type used.
        /// Any emulation can have it's direction and speed randomized within specified tolerances. By default, speed is 
        /// limited to between 0 (low) and 5 (high) meters/second and bearing changes are limited to +/- 45 degrees 
        /// (a 90 degree arc) from North (0) degrees.
        /// </remarks>
        public virtual void Randomize()
        {
            // Flag it so the emulation will use random values
            _isRandom = true;

            // Randomize the speed within range
            _Speed = Speed.FromMetersPerSecond((_seed.NextDouble() * (_speedHigh - _speedLow)) + _speedLow);

            // Randomize the bearing within the arc.
            _Bearing = new Azimuth(_seed.NextDouble() * _bearingArc + (_bearingStart - (_bearingArc * .5))).Normalize();

            // Reset the bearing origin
            _bearingStart = _Bearing.DecimalDegrees;
        }

        /// <summary>
        /// Randomizes the emulation by changing speed and direction
        /// </summary>
        /// <param name="seed"> The randomizer to use. </param>
        /// <param name="speedLow"> The minimum travel speed. </param>
        /// <param name="speedHigh"> The maximum travel speed. </param>
        /// <param name="bearingStart"> The initial direction of travel. </param>
        /// <param name="bearingArc"> The arc in which random directional changes will occur. </param>
        /// <remarks>
        /// GPS coordinate emulation can be randomized by any number of factors, depending on the emulator type used.
        /// Any emulation can have it's direction and speed randomized within specified tolerances. By default, speed is 
        /// limited to between 0 (low) and 5 (high) meters/second and bearing changes are limited to +/- 45 degrees 
        /// (a 90 degree arc) from North (0) degrees.
        /// </remarks>
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

        #endregion

        public string Name
        {
            get { return _Name; }
        }

        public override string ToString()
        {
            return _Name;
        }

        #region Real-time specified

        public Distance Altitude
        {
            get { return _Altitude; }
            set
            {
                _Altitude = value;
                _isRandom = false;
            }
        }

        public Speed Speed
        {
            get { return _Speed; }
            set
            {
                _Speed = value;
                _isRandom = false;
            }
        }

        public Azimuth Bearing
        {
            get { return _Bearing; }
            set
            {
                // If we're following a route, changing the bearing is not allowed
                if (_Route.Count != 0)
                    throw new InvalidOperationException("The current bearing can only be set if there is no route to follow.  With a route specified, the emulator will automatically recalculate the bearing.");

                _Bearing = value;
                _isRandom = false;
            }
        }

        public int FixedSatelliteCount
        {
            get
            {
                int count= 0;

                // Bump up the count whenever a fixed satellite is found
                for (int index = 0; index < _Satellites.Count; index++)
                {
                    Satellite satellite = _Satellites[index];
                    if (satellite.IsFixed)
                        count++;
                }

                return count;
            }
        }

        public FixQuality FixQuality
        {
            get { return _FixQuality; }
            set { _FixQuality = value; }
        }

        public FixMode FixMode
        {
            get { return _FixMode; }
            set { _FixMode = value; }
        }

        public FixMethod FixMethod
        {
            get { return _FixMethod; }
            set { _FixMethod = value; }
        }

        public FixStatus FixStatus
        {
            get { return _FixStatus; }
            set { _FixStatus = value; }
        }

        public DilutionOfPrecision HorizontalDilutionOfPrecision
        {
            get { return _HorizontalDop; }
            set { _HorizontalDop = value; }
        }

        public DilutionOfPrecision VerticalDilutionOfPrecision
        {
            get { return _VerticalDop; }
            set { _VerticalDop = value; }
        }

        public DilutionOfPrecision MeanDilutionOfPrecision
        {
            get { return _MeanDop; }
            set { _MeanDop = value; }
        }

        public IList<Satellite> Satellites
        {
            get { return _Satellites; }
        }

        public IList<Position> Route
        {
            get { return _Route; }
        }

        public Position CurrentPosition
        {
            get { return _CurrentPosition; }
            set
            {
                //// Is the new value null?  Fuck that.
                //if (value == null)
                //    throw new ArgumentNullException("The current simulated location of an emulator cannot be null.");

                // Do we have a route?
                if (_Route.Count != 0)
                    throw new InvalidOperationException("The current position can only be set if there is no route to follow.  With a route specified, the emulator will traverse it automatically.");

                _CurrentPosition = value;
            }
        }

        public Position CurrentDestination
        {
            get { return _CurrentDestination; }
            set
            {
                //// Is the new value null?  Fuck that.
                //if (value == null)
                //    throw new ArgumentNullException("The current simulated destination of an emulator cannot be null.");

                // Do we have a route?
                if (_Route.Count > 0)
                    throw new InvalidOperationException("The destination can only be set if there is no route to follow.  With a route specified, the emulator will traverse it automatically.");

                _CurrentDestination = value;
            }
        }

        public DateTime DateTime
        {
            get { return DateTime.Now; }
        }

        public DateTime UtcDateTime
        {
            get { return DateTime.UtcNow; }
        }

        #endregion

        #region Overrides

        public override bool CanRead
        {
            get { return !_IsDisposed; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanTimeout
        {
            get
            {
                return true;
            }
        }

        public override void Flush()
        {
            // Clear all buffers
            _ReadBuffer.Clear();
            _WriteBuffer.Clear();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            /* This method is called by the client.  If no data is available
             * to read, the method will block for a moment for new data.  The
             * emulation thread will add to the buffer.
             */

            // If there's nothing to read, block a while for data
            if (_ReadBuffer.Count == 0
                // Wait for data.  If there is none, throw a big baby tantrum
#if PocketPC
                && !_ReadDataAvailableWaitHandle.WaitOne(ReadTimeout, false)
#else
                && !_ReadDataAvailableWaitHandle.WaitOne(_ReadTimeout)
#endif
)
            {
                throw new TimeoutException("The emulator has not generated any data within the read timeout period.");
            }

            // Calculate the number of bytes to read
            int bytesToRead = _ReadBuffer.Count < count ? _ReadBuffer.Count : count;

            // Copy the read buffer to the target array
            _ReadBuffer.CopyTo(offset, buffer, 0, bytesToRead);

            // Remove the read bytes from the buffer
            _ReadBuffer.RemoveRange(0, bytesToRead);

            // If the buffer is empty, reset
            if (_ReadBuffer.Count == 0)
                _ReadDataAvailableWaitHandle.Reset();

            // Return the bytes read
            return bytesToRead;
        }

        public override int ReadByte()
        {
            // If there's nothing to read, block a while for data
            if (_ReadBuffer.Count == 0)
            {
                // Wait for data.  If there is none, return -1
#if PocketPC
                if (!_ReadDataAvailableWaitHandle.WaitOne(ReadTimeout, false))
#else
                if (!_ReadDataAvailableWaitHandle.WaitOne(ReadTimeout))
#endif
                    return -1;
            }

            // Anything?  If not, return -1
            if (_ReadBuffer.Count == 0)
                return -1;

            // Yes.  Get the first byte
            int result = Convert.ToInt32(_ReadBuffer[0]);

            // And remove it from the list
            _ReadBuffer.RemoveAt(0);

            // If the buffer is empty, reset
            if (_ReadBuffer.Count == 0)
                _ReadDataAvailableWaitHandle.Reset();

            // Return the byte read
            return result;
        }

        protected List<byte> ReadBuffer
        {
            get { return _ReadBuffer; }
        }

        protected ManualResetEvent ReadDataAvailableWaitHandle
        {
            get { return _ReadDataAvailableWaitHandle; }
        }

        protected IList<byte> WriteBuffer
        {
            get { return _WriteBuffer; }
        }

        protected ManualResetEvent WriteDataAvailableWaitHandle
        {
            get { return _WriteDataAvailableWaitHandle; }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // Write the buffer, byte-by-byte to the incoming buffer
            for (int index = offset; index < offset + count; index++)
                _WriteBuffer.Add(buffer[index]);

            // Set the writing wait handle
            _WriteDataAvailableWaitHandle.Set();
        }

        public override void WriteByte(byte value)
        {
            // Add the byte
            _WriteBuffer.Add(value);

            // Set the writing wait handle
            _WriteDataAvailableWaitHandle.Set();
        }

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

        public override long Length
        {
            get { return _ReadBuffer.Count; }
        }

        public override bool CanWrite
        {
            get
            {
                //  By default, emulators are read-only.  Implementers (such as Garmin or SiRF) 
                // will override this.
                return false;
            }
        }

        public override int ReadTimeout
        {
            get
            {
                return Convert.ToInt32(_ReadTimeout.TotalMilliseconds);
            }
            set
            {
                _ReadTimeout = TimeSpan.FromMilliseconds(value);
            }
        }

        public override int WriteTimeout
        {
            get
            {
                return Convert.ToInt32(_WriteTimeout.TotalMilliseconds);
            }
            set
            {
                _WriteTimeout = TimeSpan.FromMilliseconds(value);
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

        #endregion

        /// <summary>
        /// Closes the emulation stream, but doesn't dispose of it.
        /// </summary>
        /// <remarks>
        /// The Emulator.Close() method simply terminates the thread that feeds data to
        /// a virtual Device. This allows the emulator to be reused indefinately.
        /// </remarks>
        public override void Close()
        {
            if (_EmulationIntervalWaitHandle != null)
                _EmulationIntervalWaitHandle.Set();

            _IsRunning = false;

            // Shut down the emulator
            if (_Thread != null && IsEmulationThreadAlive && !_Thread.Join(ReadTimeout))
            {
                _Thread.Abort();
            }

            _Thread = null;

            // Now clean up the handles
            if (_ReadDataAvailableWaitHandle != null)
                _ReadDataAvailableWaitHandle.Reset();
            if (_WriteDataAvailableWaitHandle != null)
                _WriteDataAvailableWaitHandle.Reset();
            if (_EmulationIntervalWaitHandle != null)
                _EmulationIntervalWaitHandle.Reset();
        }

        protected override void Dispose(bool disposing)
        {
            // If we're already disposed, just exit
            if (_IsDisposed)
                return;

            // Indicate that we're disposed
            _IsDisposed = true;

            // Stop the thread
            Close();

            // Now clean up the handles
            if (_ReadDataAvailableWaitHandle != null)
                _ReadDataAvailableWaitHandle.Close();
            if (_WriteDataAvailableWaitHandle != null)
                _WriteDataAvailableWaitHandle.Close();
            if (_EmulationIntervalWaitHandle != null)
                _EmulationIntervalWaitHandle.Close();

            // Are we disposing of managed objects?
            if (disposing)
            {
                _WriteBuffer = null;
                _ReadBuffer = null;
                _ReadDataAvailableWaitHandle = null;
                _WriteDataAvailableWaitHandle = null;
                _EmulationIntervalWaitHandle = null;
                _Thread = null;
                _Route = null;
            }
        }

        #endregion

    }
}
