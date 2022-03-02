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
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Threading;
#if !PocketPC

using System.Runtime.ConstrainedExecution;
using System.Runtime.CompilerServices;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC

    /// <summary>
    /// Represents a base class for designing a GPS data interpreter.
    /// </summary>
    /// <seealso cref="OnReadPacket">OnReadPacket Method</seealso>
    /// <remarks><para>This class serves as the base class for all GPS data interpreters, regardless
    /// of the protocol being used. For example, the <strong>NmeaInterpreter</strong> class
    /// inherits from this class to process NMEA-0183 data from any data source. This class
    /// provides basic functionality to start, pause, resume and stop the processing of GPS
    /// data, and provides management of a thread used to process the next set of incoming
    /// data.</para>
    ///   <para>Inheritors should override the <strong>OnReadPacket</strong> event and
    /// provide functionality to read the next packet of data from the underlying stream.
    /// All raw GPS data must be provided in the form of a <strong>Stream</strong> object,
    /// and the method should read and process only a single packet of data.</para></remarks>
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
#endif
    public abstract class Interpreter : Component
    {
        #region Private variables

        // Real-time GPS information
        /// <summary>
        ///
        /// </summary>
        private DateTime _utcDateTime;
        /// <summary>
        ///
        /// </summary>
        private DateTime _dateTime;
        /// <summary>
        ///
        /// </summary>
        private Distance _geoidalSeparation;
        /// <summary>
        ///
        /// </summary>
        private Distance _altitude;
        /// <summary>
        ///
        /// </summary>
        private Distance _altitudeAboveEllipsoid;
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
        private Azimuth _heading;
        /// <summary>
        ///
        /// </summary>
        private Position _position;
        /// <summary>
        ///
        /// </summary>
        private Longitude _magneticVariation;
        /// <summary>
        ///
        /// </summary>
        private FixStatus _fixStatus;
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
        private FixQuality _fixQuality;
        /// <summary>
        ///
        /// </summary>
        private int _fixedSatelliteCount;
        /// <summary>
        ///
        /// </summary>
        private bool _isFixRequired;
        /// <summary>
        ///
        /// </summary>
        private DilutionOfPrecision _maximumHorizontalDop = DilutionOfPrecision.Maximum;
        /// <summary>
        ///
        /// </summary>
        private DilutionOfPrecision _maximumVerticalDop = DilutionOfPrecision.Maximum;
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
        private List<Satellite> _satellites = new List<Satellite>(16);

        // Filtering
        /// <summary>
        ///
        /// </summary>
        private PrecisionFilter _filter = PrecisionFilter.Default;
        /// <summary>
        ///
        /// </summary>
        private bool _isFilterEnabled = true;

        // Engine management
        /// <summary>
        ///
        /// </summary>
        private bool _isRunning;
        /// <summary>
        ///
        /// </summary>
        private Device _device;
#if !PocketPC
        /// <summary>
        ///
        /// </summary>
        private ThreadPriority _threadPriority = ThreadPriority.Normal;
#else
        private ThreadPriority _ThreadPriority = ThreadPriority.BelowNormal;
#endif
        /// <summary>
        ///
        /// </summary>
        private Thread _parsingThread;
        /// <summary>
        ///
        /// </summary>
        private ManualResetEvent _pausedWaitHandle = new ManualResetEvent(true);
        /// <summary>
        ///
        /// </summary>
        private int _maximumReconnectionAttempts = -1;
        /// <summary>
        ///
        /// </summary>
        private int _reconnectionAttemptCount;
        /// <summary>
        ///
        /// </summary>
        private Stream _recordingStream;

        /// <summary>
        ///
        /// </summary>
        private bool _isDisposed;
        /// <summary>
        ///
        /// </summary>
        private bool _allowAutomaticReconnection = true;

#if PocketPC
        private bool _IsParsingThreadAlive;
#endif

#if !PocketPC

        #region Event stacking

        /* Event Stacking
         *
         * Synchronous invokes cause problems if consumers take too much time to
         * process the event. Raising events asynchronously and tracking their
         * completion prevents events from stacking up. Just about every event
         * in an interpreter could benefit from this.
         */
        /// <summary>
        ///
        /// </summary>
        private IAsyncResult _positionChangedAsyncResult;

        #endregion Event stacking

#endif

        #endregion Private variables

        /// <summary>
        ///
        /// </summary>
        private static TimeSpan _commandTimeout = TimeSpan.FromSeconds(5);
        /// <summary>
        ///
        /// </summary>
        private static TimeSpan _readTimeout = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Represents a synchronization object which is locked during state changes.
        /// </summary>
        /// <value>An <strong>Object</strong>.</value>
        protected readonly object SyncRoot = new object();
        /// <summary>
        /// Represents a synchronization object which is locked during state changes to recording.
        /// </summary>
        protected readonly object RecordingSyncRoot = new object();

        #region Events

        /// <summary>
        /// Occurs when the current distance above sea level has changed.
        /// </summary>
        public event EventHandler<DistanceEventArgs> AltitudeChanged;
        /// <summary>
        /// Occurs when a new altitude report has been received, even if the value has not changed.
        /// </summary>
        public event EventHandler<DistanceEventArgs> AltitudeReceived;
        /// <summary>
        /// Occurs when the current distance above the ellipsoid has changed.
        /// </summary>
        public event EventHandler<DistanceEventArgs> AltitudeAboveEllipsoidChanged;
        /// <summary>
        /// Occurs when a new altitude-above-ellipsoid report has been received, even if the value has not changed.
        /// </summary>
        public event EventHandler<DistanceEventArgs> AltitudeAboveEllipsoidReceived;
        /// <summary>
        /// Occurs when the current direction of travel has changed.
        /// </summary>
        public event EventHandler<AzimuthEventArgs> BearingChanged;
        /// <summary>
        /// Occurs when the current direction of heading has changed.
        /// </summary>
        public event EventHandler<AzimuthEventArgs> HeadingChanged;
        /// <summary>
        /// Occurs when a new bearing report has been received, even if the value has not changed.
        /// </summary>
        public event EventHandler<AzimuthEventArgs> BearingReceived;
        /// <summary>
        /// Occurs when a new heading report has been received, even if the value has not changed.
        /// </summary>
        public event EventHandler<AzimuthEventArgs> HeadingReceived;
        /// <summary>
        /// Occurs when the fix quality has changed.
        /// </summary>
        public event EventHandler<FixQualityEventArgs> FixQualityChanged;
        /// <summary>
        /// Occurs when the fix mode has changed.
        /// </summary>
        public event EventHandler<FixModeEventArgs> FixModeChanged;
        /// <summary>
        /// Occurs when the fix method has changed.
        /// </summary>
        public event EventHandler<FixMethodEventArgs> FixMethodChanged;
        /// <summary>
        /// Occurs when the geodal separation changes
        /// </summary>
        public event EventHandler<DistanceEventArgs> GeoidalSeparationChanged;
        /// <summary>
        /// Occurs when the GPS-derived date and time has changed.
        /// </summary>
        public event EventHandler<DateTimeEventArgs> UtcDateTimeChanged;
        /// <summary>
        /// Occurs when the GPS-derived local time has changed.
        /// </summary>
        public event EventHandler<DateTimeEventArgs> DateTimeChanged;
        /// <summary>
        /// Occurs when at least three GPS satellite signals are available to calculate the current location.
        /// </summary>
        public event EventHandler FixAcquired;
        /// <summary>
        /// Occurs when less than three GPS satellite signals are available.
        /// </summary>
        public event EventHandler FixLost;
        /// <summary>
        /// Occurs when the current location on Earth has changed.
        /// </summary>
        public event EventHandler<PositionEventArgs> PositionChanged;
        /// <summary>
        /// Occurs when a new position report has been received, even if the value has not changed.
        /// </summary>
        public event EventHandler<PositionEventArgs> PositionReceived;
        /// <summary>
        /// Occurs when the magnetic variation for the current location becomes known.
        /// </summary>
        public event EventHandler<LongitudeEventArgs> MagneticVariationAvailable;
        /// <summary>
        /// Occurs when the current rate of travel has changed.
        /// </summary>
        public event EventHandler<SpeedEventArgs> SpeedChanged;
        /// <summary>
        /// Occurs when a new speed report has been received, even if the value has not changed.
        /// </summary>
        public event EventHandler<SpeedEventArgs> SpeedReceived;
        /// <summary>
        /// Occurs when precision as it relates to latitude and longitude has changed.
        /// </summary>
        public event EventHandler<DilutionOfPrecisionEventArgs> HorizontalDilutionOfPrecisionChanged;
        /// <summary>
        /// Occurs when precision as it relates to altitude has changed.
        /// </summary>
        public event EventHandler<DilutionOfPrecisionEventArgs> VerticalDilutionOfPrecisionChanged;
        /// <summary>
        /// Occurs when precision as it relates to latitude, longitude and altitude has changed.
        /// </summary>
        public event EventHandler<DilutionOfPrecisionEventArgs> MeanDilutionOfPrecisionChanged;
        /// <summary>
        /// Occurs when GPS satellite information has changed.
        /// </summary>
        public event EventHandler<SatelliteListEventArgs> SatellitesChanged;
        /// <summary>
        /// Occurs when the interpreter is about to start.
        /// </summary>
        public event EventHandler<DeviceEventArgs> Starting;
        /// <summary>
        /// Occurs when the interpreter is now processing GPS data.
        /// </summary>
        public event EventHandler Started;
        /// <summary>
        /// Occurs when the interpreter is about to stop.
        /// </summary>
        public event EventHandler Stopping;
        /// <summary>
        /// Occurs when the interpreter has stopped processing GPS data.
        /// </summary>
        public event EventHandler Stopped;
        /// <summary>
        /// Occurs when the interpreter has temporarily stopped processing GPS data.
        /// </summary>
        public event EventHandler Paused;
        /// <summary>
        /// Occurs when the interpreter is no longer paused.
        /// </summary>
        public event EventHandler Resumed;
        /// <summary>
        /// Occurs when an exception has happened during processing.
        /// </summary>
        public event EventHandler<ExceptionEventArgs> ExceptionOccurred;
        /// <summary>
        /// Occurs when the flow of GPS data has been suddenly interrupted.
        /// </summary>
        public event EventHandler<ExceptionEventArgs> ConnectionLost;

        /// <summary>
        /// Occurs immediately before a connection is attempted.
        /// </summary>
        protected virtual void OnStarting()
        {
            if (Starting != null)
                Starting(this, new DeviceEventArgs(_device));
        }

        /// <summary>
        /// Occurs immediately before data is processed.
        /// </summary>
        protected virtual void OnStarted()
        {
            if (Started != null)
                Started(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs immediately before the interpreter is shut down.
        /// </summary>
        protected virtual void OnStopping()
        {
            if (Stopping != null)
                Stopping(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs immediately after the interpreter has been shut down.
        /// </summary>
        protected virtual void OnStopped()
        {
            if (Stopped != null)
                Stopped(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when a connection to a GPS device is suddenly lost.
        /// </summary>
        /// <param name="ex">An <strong>Exception</strong> which further explains why the connection was lost.</param>
        protected virtual void OnConnectionLost(Exception ex)
        {
            if (ConnectionLost != null)
                ConnectionLost(this, new ExceptionEventArgs(ex));
        }

        /// <summary>
        /// Occurs when the interpreter has temporarily stopped processing data.
        /// </summary>
        protected virtual void OnPaused()
        {
            if (Paused != null)
                Paused(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when the interpreter is no longer paused.
        /// </summary>
        protected virtual void OnResumed()
        {
            if (Resumed != null)
                Resumed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when an exception is trapped by the interpreter's thread.
        /// </summary>
        /// <param name="ex">The ex.</param>
        protected virtual void OnExceptionOccurred(Exception ex)
        {
            if (ExceptionOccurred != null)
                ExceptionOccurred(this, new ExceptionEventArgs(ex));
        }

        #endregion Events

        #region Static members

        /// <summary>
        /// Controls the amount of time to wait for the next packet of GPS data to arrive.
        /// </summary>
        /// <value>The read timeout.</value>
        public static TimeSpan ReadTimeout
        {
            get
            {
                return _readTimeout;
            }
            set
            {
                // Disallow any value zero or less
                if (_readTimeout.TotalMilliseconds <= 0)
                    throw new ArgumentOutOfRangeException("ReadTimeout", "The read timeout of a GPS interpreter must be greater than zero.  A value of about five seconds is typical.");

                // Set the new value
                _readTimeout = value;
            }
        }

        /// <summary>
        /// Controls the amount of time allowed to perform a start, stop, pause or resume action.
        /// </summary>
        /// <value>The command timeout.</value>
        /// <remarks>The <strong>Interpreter</strong> class is multithreaded and is also thread-safe.  Still, however, in some rare cases,
        /// two threads may attempt to change the state of the interpreter at the same time.  Critical sections will allow both threads to
        /// success whenever possible, but in the event of a deadlock, this property control how much time to allow before giving up.</remarks>
        public static TimeSpan CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                if (_commandTimeout.TotalSeconds < 1)
                    throw new ArgumentOutOfRangeException("CommandTimeout", "The command timeout of a GPS interpreter cannot be less than one second.  A value of about ten seconds is recommended.");

                _commandTimeout = value;
            }
        }

        #endregion Static members

        #region Public Properties

#if !PocketPC

        /// <summary>
        /// Returns the device providing raw GPS data to the interpreter.
        /// </summary>
        [Category("Data")]
        [Description("Returns the device providing raw GPS data to the interpreter.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public Device Device
        {
            get
            {
                return _device;
            }
        }

#if !PocketPC

        /// <summary>
        /// Controls the priority of the thread which processes GPS data.
        /// </summary>
        /// <value>The thread priority.</value>
        [Category("Behavior")]
        [Description("Controls the priority of the thread which processes GPS data.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(ThreadPriority.Normal)]
#endif
        public ThreadPriority ThreadPriority
        {
            get
            {
                return _threadPriority;
            }
            set
            {
                // Set the new value
                _threadPriority = value;

                // If the parsing thread is alive, update it
                if (_parsingThread != null
#if !PocketPC
 && _parsingThread.IsAlive
#else
                    && _IsParsingThreadAlive
#endif
)
                    _parsingThread.Priority = _threadPriority;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the GPS-derived date and time, adjusted to the local time zone.
        /// </summary>
        [Category("Data")]
        [Description("Returns the GPS-derived date and time, adjusted to the local time zone.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public DateTime DateTime
        {
            get { return _dateTime; }
        }

#if !PocketPC

        /// <summary>
        /// Returns the GPS-derived date and time in UTC (GMT-0).
        /// </summary>
        [Category("Data")]
        [Description("Returns the GPS-derived date and time in UTC (GMT-0).")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public DateTime UtcDateTime
        {
            get { return _utcDateTime; }
        }

#if !PocketPC

        /// <summary>
        /// Returns the current estimated precision as it relates to latitude and longitude.
        /// </summary>
        /// <remarks>Horizontal Dilution of Precision (HDOP) is the accumulated
        /// error of latitude and longitude coordinates in X and Y directions
        /// (displacement on the surface of the ellipsoid).</remarks>
        [Category("Precision")]
        [Description("Returns the current estimated precision as it relates to latitude and longitude.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public DilutionOfPrecision HorizontalDilutionOfPrecision
        {
            get { return _horizontalDop; }
        }

#if !PocketPC

        /// <summary>
        /// Returns the current estimated precision as it relates to altitude.
        /// </summary>
        /// <remarks>Vertical Dilution of Precision (VDOP) is the accumulated
        /// error of latitude and longitude coordinates in the Z direction (measurement
        /// from the center of the ellipsoid).</remarks>
        [Category("Precision")]
        [Description("Returns the current estimated precision as it relates to altitude.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public DilutionOfPrecision VerticalDilutionOfPrecision
        {
            get { return _verticalDop; }
        }

#if !PocketPC

        /// <summary>
        /// Returns the kind of fix acquired by the GPS device.
        /// </summary>
        [Category("Data")]
        [Description("Returns the kind of fix acquired by the GPS device.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public FixMode FixMode
        {
            get { return _fixMode; }
        }

#if !PocketPC

        /// <summary>
        /// Returns the number of satellites being used to calculate the current location.
        /// </summary>
        [Category("Data")]
        [Description("Returns the number of satellites being used to calculate the current location.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public int FixedSatelliteCount
        {
            get { return _fixedSatelliteCount; }
        }

#if !PocketPC

        /// <summary>
        /// Returns the quality of the fix and what kinds of technologies are involved.
        /// </summary>
        [Category("Data")]
        [Description("Returns the quality of the fix and what kinds of technologies are involved.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public FixQuality FixQuality
        {
            get { return _fixQuality; }
        }

#if !PocketPC

        /// <summary>
        /// Controls whether GPS data is ignored until a fix is acquired.
        /// </summary>
        /// <value><c>true</c> if this instance is fix required; otherwise, <c>false</c>.</value>
        [Category("Behavior")]
        [Description("Controls whether GPS data is ignored until a fix is acquired.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
#endif
        public bool IsFixRequired
        {
            get { return _isFixRequired; }
            set { _isFixRequired = value; }
        }

#if !PocketPC

        /// <summary>
        /// Returns whether a fix is currently acquired by the GPS device.
        /// </summary>
        [Category("Data")]
        [Description("Returns whether a fix is currently acquired by the GPS device.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif

        public bool IsFixed
        {
            get { return _fixStatus == FixStatus.Fix; }
        }

#if !PocketPC

        /// <summary>
        /// Returns the difference between magnetic North and True North.
        /// </summary>
        [Category("Data")]
        [Description("Returns the difference between magnetic North and True North.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public Longitude MagneticVariation
        {
            get { return _magneticVariation; }
        }

#if !PocketPC

        /// <summary>
        /// Returns the current location on Earth's surface.
        /// </summary>
        [Category("Data")]
        [Description("Returns the current location on Earth's surface.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public Position Position
        {
            get { return _position; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
        }

        /// <summary>
        /// Returns the average precision tolerance based on the fix quality reported
        /// by the device.
        /// </summary>
        /// <remarks>This property returns the estimated error attributed to the device. To get
        /// a total error estimation, add the Horizontal or the Mean DOP to the
        /// FixPrecisionEstimate.</remarks>
        public Distance FixPrecisionEstimate
        {
            get { return Device.PrecisionEstimate(_fixQuality); }
        }

#if !PocketPC

        /// <summary>
        /// Returns the current rate of travel.
        /// </summary>
        [Category("Data")]
        [Description("Returns the current rate of travel.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public Speed Speed
        {
            get { return _speed; }
        }

#if !PocketPC

        /// <summary>
        /// Returns the current distance above sea level.
        /// </summary>
        [Category("Data")]
        [Description("Returns the current distance above sea level.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public Distance Altitude
        {
            get { return _altitude; }
        }

#if !PocketPC

        /// <summary>
        /// Gets the current direction of travel as an Azimuth
        /// </summary>
        [Category("Data")]
        [Description("Returns the current direction of travel.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public Azimuth Bearing
        {
            get { return _bearing; }
        }
        
        public Azimuth Heading
        {
            get { return _heading; }
        }

#if !PocketPC

        /// <summary>
        /// Controls the largest amount of precision error allowed before GPS data is ignored.
        /// </summary>
        /// <value>The maximum horizontal dilution of precision.</value>
        /// <remarks>This property is important for commercial GPS software development because it helps the interpreter determine
        /// when GPS data reports are precise enough to utilize.  Live GPS data can be inaccurate by up to a factor of fifty, or nearly
        /// the size of an American football field!  As a result, setting a value for this property can help to reduce precision errors.
        /// When set, reports of latitude, longitude, speed, and bearing are ignored if precision is not at or below the set value.
        /// For more on Dilution of Precision and how to determine your precision needs, please refer to our online article here:
        /// http://dotspatial.codeplex.com/Articles/WritingApps2_1.aspx.</remarks>
        [Category("Precision")]
        [Description("Controls the largest amount of precision error allowed before GPS data is ignored..")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(DilutionOfPrecision), "Maximum")]
#endif
        public DilutionOfPrecision MaximumHorizontalDilutionOfPrecision
        {
            get
            {
                return _maximumHorizontalDop;
            }
            set
            {
                if (_maximumHorizontalDop.Equals(value))
                    return;

                if (value.Value <= 0.0f || value.Value > 50.0f)
                    throw new ArgumentOutOfRangeException("MaximumHorizontalDilutionOfPrecision", "The maximum allowed horizontal Dilution of Precision must be a value between 1 and 50.");

                _maximumHorizontalDop = value;
            }
        }

#if !PocketPC

        /// <summary>
        /// Controls the maximum number of consecutive reconnection retries allowed.
        /// </summary>
        /// <value>The maximum reconnection attempts.</value>
        [Category("Behavior")]
        [Description("Controls the maximum number of consecutive reconnection retries allowed.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(-1)]
#endif
        public int MaximumReconnectionAttempts
        {
            get
            {
                return _maximumReconnectionAttempts;
            }
            set
            {
                if (value < -1)
                    throw new ArgumentOutOfRangeException("MaximumReconnectionAttempts", "The maximum reconnection attempts for an interpreter must be -1 for infinite retries, or greater than zero for a specific amount.");

                _maximumReconnectionAttempts = value;
            }
        }

#if !PocketPC

        /// <summary>
        /// Controls the largest amount of precision error allowed before GPS data is ignored.
        /// </summary>
        /// <value>The maximum vertical dilution of precision.</value>
        /// <remarks>This property is important for commercial GPS software development because it helps the interpreter determine
        /// when GPS data reports are precise enough to utilize.  Live GPS data can be inaccurate by up to a factor of fifty, or nearly
        /// the size of an American football field!  As a result, setting a value for this property can help to reduce precision errors.
        /// When set, reports of altitude are ignored if precision is not at or below the set value.
        /// For more on Dilution of Precision and how to determine your precision needs, please refer to our online article here:
        /// http://dotspatial.codeplex.com/Articles/WritingApps2_1.aspx.</remarks>
        [Category("Precision")]
        [Description("Controls the largest amount of precision error allowed before GPS data is ignored.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(typeof(DilutionOfPrecision), "Maximum")]
#endif
        public DilutionOfPrecision MaximumVerticalDilutionOfPrecision
        {
            get
            {
                return _maximumVerticalDop;
            }
            set
            {
                if (_maximumVerticalDop.Equals(value))
                    return;

                if (value.Value <= 0.0f || value.Value > 50.0f)
                    throw new ArgumentOutOfRangeException("MaximumVerticalDilutionOfPrecision", "The maximum allowed vertical Dilution of Precision must be a value between 1 and 50.");

                _maximumVerticalDop = value;
            }
        }

#if !PocketPC

        /// <summary>
        /// Controls whether real-time GPS data is made more precise using a filter.
        /// </summary>
        /// <value><c>true</c> if this instance is filter enabled; otherwise, <c>false</c>.</value>
        [Category("Precision")]
        [Description("Controls whether real-time GPS data is made more precise using a filter.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(true)]
#endif
        public bool IsFilterEnabled
        {
            get { return _isFilterEnabled; }
            set { _isFilterEnabled = value; }
        }

#if !PocketPC

        /// <summary>
        /// Controls the technology used to reduce GPS precision error.
        /// </summary>
        /// <value>The filter.</value>
        [Category("Precision")]
        [Description("Controls whether real-time GPS data is made more precise using a filter.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public PrecisionFilter Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

#if !PocketPC

        /// <summary>
        /// Controls whether the interpreter will try to automatically attempt to reconnect anytime a connection is lost.
        /// </summary>
        /// <value><c>true</c> if [allow automatic reconnection]; otherwise, <c>false</c>.</value>
        /// <remarks><para>Interpreters are able to automatically try to recover from connection failures.  When this property is set to <strong>True</strong>,
        /// the interpreter will detect a sudden loss of connection, then attempt to make a new connection to restore the flow of data.  If multiple GPS
        /// devices have been detected, any of them may be utilized as a "fail-over device."  Recovery attempts will continue repeatedly until a connection
        /// is restored, the interpreter is stopped, or the interpreter is disposed.</para>
        ///   <para>For most applications, this property should be enabled to help improve the stability of the application.  In most cases, a sudden loss of
        /// data is only temporary, caused by a loss of battery power or when a wireless device moves too far out of range.</para></remarks>
        [Category("Behavior")]
        [Description("Controls whether the interpreter will try to automatically attempt to reconnect anytime a connection is lost.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(true)]
#endif
        public bool AllowAutomaticReconnection
        {
            get { return _allowAutomaticReconnection; }
            set { _allowAutomaticReconnection = value; }
        }

#if !PocketPC

        /// <summary>
        /// Returns a list of known GPS satellites.
        /// </summary>
        [Category("Data")]
        [Description("Returns a list of known GPS satellites.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
        public IList<Satellite> Satellites
        {
            get { return _satellites; }
        }

        /// <summary>
        /// Returns whether resources in this object has been shut down.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Returns the stream used to output data received from the GPS device.
        /// </summary>
        public Stream RecordingStream
        {
            get { return _recordingStream; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Starts processing GPS data using any available GPS device.
        /// </summary>
        /// <remarks>This method is used to begin processing GPS data.  If no GPS devices are known, GPS.NET will search for GPS devices and use the
        /// first device it finds.  If no device can be found, an exception is raised.</remarks>
        public void Start()
        {
            // If we're disposed, complain
            if (_isDisposed)
                throw new ObjectDisposedException("The Interpreter cannot be started because it has been disposed.");

            // Are we already running?
            if (_isRunning)
                return;

            // Prevent state changes while the interpreter is started
#if !PocketPC
            if (Monitor.TryEnter(SyncRoot, _commandTimeout))
#else
            if (Monitor.TryEnter(SyncRoot))
#endif
            {
                try
                {
                    // Set the stream
                    _device = Devices.Any;

                    // Is the stream null?
                    if (_device == null)
                    {
                        // And report the problem
                        throw new InvalidOperationException("After performing a search, no GPS devices were found.");
                    }

                    // Signal that we're starting
                    OnStarting();

                    // Signal that the stream has changed
                    OnDeviceChanged();

                    // Indicate that we're running
                    _isRunning = true;

                    // If the thread isn't alive, start it now
                    if (_parsingThread == null
#if !PocketPC
 || !_parsingThread.IsAlive
#else
                        || !_IsParsingThreadAlive
#endif
)
                    {
                        _parsingThread = new Thread(ParsingThreadProc)
                                             {
                                                 IsBackground = true,
                                                 Priority = _threadPriority,
                                                 Name = "GPS.NET Parsing Thread (http://dotspatial.codeplex.com)"
                                             };
                        _parsingThread.Start();

                        // And signal it
                        OnStarted();
                    }
                    else
                    {
                        // Otherwise, allow parsing to continue
                        _pausedWaitHandle.Set();

                        // Signal that we've resumed
                        OnResumed();
                    }
                }
                catch (Exception ex)
                {
                    // Signal that we're stopped
                    OnStopped();

                    // And report the problem
                    throw new InvalidOperationException("The interpreter could not be started. " + ex.Message, ex);
                }
                finally
                {
                    // Release the lock
                    Monitor.Exit(SyncRoot);
                }
            }
            else
            {
                // Signal that we're stopped
                OnStopped();

                // The interpreter is already busy
                throw new InvalidOperationException("The interpreter cannot be started.  It appears that another thread is already trying to start, stop, pause, or resume.");
            }
        }

        /// <summary>
        /// Starts processing GPS data from the specified stream.
        /// </summary>
        /// <param name="device">A device object providing GPS data to process.</param>
        /// <remarks>This method will start the <strong>Interpreter</strong> using a separate thread.
        /// The <strong>OnReadPacket</strong> is then called repeatedly from that thread to process
        /// incoming data. The Pause, Resume and Stop methods are typically called after this
        /// method to change the interpreter's behavior. Finally, a call to
        /// <strong>Dispose</strong> will close the underlying stream, stop all processing, and
        /// shut down the processing thread.</remarks>
        public void Start(Device device)
        {
            // If we're disposed, complain
            if (_isDisposed)
                throw new ObjectDisposedException("The Interpreter cannot be started because it has been disposed.");

            // Prevent state changes while the interpreter is started
#if !PocketPC
            if (Monitor.TryEnter(SyncRoot, _commandTimeout))
#else
            if (Monitor.TryEnter(SyncRoot))
#endif
            {
                try
                {
                    // Set the device
                    _device = device;

                    // Signal that we're starting
                    OnStarting();

                    // If it's not open, open it now
                    if (!_device.IsOpen)
                        _device.Open();

                    // Indicate that we're running
                    _isRunning = true;

                    // Signal that the stream has changed
                    OnDeviceChanged();

                    // If the thread isn't alive, start it now
                    if (_parsingThread == null
#if !PocketPC
 || !_parsingThread.IsAlive
#else
                        || !_IsParsingThreadAlive
#endif
)
                    {
                        _parsingThread = new Thread(ParsingThreadProc)
                                             {
                                                 IsBackground = true,
                                                 Priority = _threadPriority,
                                                 Name = "GPS.NET Parsing Thread (http://dotspatial.codeplex.com)"
                                             };
                        _parsingThread.Start();

                        // And signal the start
                        OnStarted();
                    }
                    else
                    {
                        // Otherwise, allow parsing to continue
                        _pausedWaitHandle.Set();

                        // Signal that we've resumed
                        OnResumed();
                    }
                }
                catch (Exception ex)
                {
                    // Close the device
                    _device.Close();

                    // Show that we're stopped
                    OnStopped();

                    // Report the problem
                    throw new InvalidOperationException("The interpreter could not be started.  " + ex.Message);
                }
                finally
                {
                    // Release the lock
                    Monitor.Exit(SyncRoot);
                }
            }
            else
            {
                // Signal a stop
                OnStopped();

                // The interpreter is already busy
                throw new InvalidOperationException("The interpreter cannot be started.  It appears that another thread is already trying to start, stop, pause, or resume.");
            }
        }

        /// <summary>
        /// Begins recording all received data to the specified stream.
        /// </summary>
        /// <param name="output">The output.</param>
        public void StartRecording(Stream output)
        {
            // Prevent state changes while the interpreter is started
#if !PocketPC
            if (Monitor.TryEnter(RecordingSyncRoot, _commandTimeout))
#else
            if (Monitor.TryEnter(RecordingSyncRoot))
#endif
            {
                // Set the recording stream
                _recordingStream = output;

                // And exit the context
                Monitor.Exit(RecordingSyncRoot);
            }
        }

        /// <summary>
        /// Causes the interpreter to no longer record incoming GPS data.
        /// </summary>
        public void StopRecording()
        {
            // Prevent state changes while the interpreter is started
#if !PocketPC
            if (Monitor.TryEnter(RecordingSyncRoot, _commandTimeout))
#else
            if (Monitor.TryEnter(RecordingSyncRoot))
#endif
            {
                // Set the recording stream
                _recordingStream = null;

                // And exit the context
                Monitor.Exit(RecordingSyncRoot);
            }
        }

        /// <summary>
        /// Stops all processing of GPS data.
        /// </summary>
        /// <remarks>This method is used some time after a call to the <strong>Start</strong> method.
        /// When called, the GPS processing thread is immediately shut down and all processing
        /// stops.</remarks>
        public void Stop()
        {
            // If we're disposed, complain
            if (_isDisposed)
                throw new ObjectDisposedException("The Interpreter cannot be stopped because it has been disposed.");

#if !PocketPC
            if (Monitor.TryEnter(SyncRoot, _commandTimeout))
#else
            if (Monitor.TryEnter(SyncRoot))
#endif
            {
                try
                {
                    // Are we already stopped?
                    if (!_isRunning)
                        return;

                    // Signal that we're no longer running
                    _isRunning = false;

                    // Signal that a stop is underway
                    OnStopping();

                    // Unpause the interpreter
                    _pausedWaitHandle.Set();

                    // Signal the thread to stop. If it takes too long, exit
                    if (_parsingThread != null
#if !PocketPC
 && _parsingThread.IsAlive
                        && !_parsingThread.Join(_commandTimeout))
#else
                        && _IsParsingThreadAlive
                        && !_ParsingThread.Join((int)_CommandTimeout.TotalMilliseconds))
#endif
                    {
                        _parsingThread.Abort();
                    }

                    // Close the connection
                    _device.Close();

                    // Notify that we've stopped
                    OnStopped();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The interpreter could not be stopped.", ex);
                }
                finally
                {
                    // Release the lock
                    Monitor.Exit(SyncRoot);
                }
            }
            else
            {
                // The interpreter is already busy
                throw new InvalidOperationException("A request to stop the interpreter has timed out.  This can occur if a Start, Pause, or Resume command is also in progress.");
            }
        }

        /// <summary>
        /// Temporarily halts processing of GPS data.
        /// </summary>
        /// <remarks>This method will suspend the processing of GPS data, but will keep the thread and
        /// raw GPS data stream open. This method is intended as a temporary means of stopping
        /// processing. An interpreter should not be paused for an extended period of time because
        /// it can cause a backlog of GPS data</remarks>
        public void Pause()
        {
            // If we're disposed, complain
            if (_isDisposed)
                throw new ObjectDisposedException("The interpreter cannot be paused because it has been disposed.");

#if !PocketPC
            if (Monitor.TryEnter(SyncRoot, _commandTimeout))
#else
            if (Monitor.TryEnter(SyncRoot))
#endif
            {
                try
                {
                    // Reset the wait handle.  This will cause the thread to pause on its next time around
                    _pausedWaitHandle.Reset();

                    // Signal the pause
                    OnPaused();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The interpreter could not be paused.", ex);
                }
                finally
                {
                    // Release the lock
                    Monitor.Exit(SyncRoot);
                }
            }
            else
            {
                // The interpreter is already busy
                throw new InvalidOperationException("The interpreter cannot be paused.  It appears that another thread is already trying to start, stop, pause, or resume.  To forcefully close the interpreter, you can call the Dispose method.");
            }
        }

        /// <summary>
        /// Un-pauses the interpreter from a previously paused state.
        /// </summary>
        public void Resume()
        {
            // If we're disposed, complain
            if (_isDisposed)
                throw new ObjectDisposedException("The interpreter cannot be resumed because it has been disposed.");

#if !PocketPC
            if (Monitor.TryEnter(SyncRoot, _commandTimeout))
#else
            if (Monitor.TryEnter(SyncRoot))
#endif
            {
                try
                {
                    // Set the wait handle.  This will cause the thread to continue processing.
                    _pausedWaitHandle.Set();

                    // Signal that we're resumed
                    OnResumed();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The interpreter could not be resumed.", ex);
                }
                finally
                {
                    // Release the lock
                    Monitor.Exit(SyncRoot);
                }
            }
            else
            {
                // The interpreter is already busy
                throw new InvalidOperationException("The interpreter cannot be resumed.  It appears that another thread is already trying to start, stop, pause, or resume.  To forcefully close the interpreter, you can call the Dispose method.");
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Resets the interpreter to it's default values.
        /// </summary>
        protected void Initialize()
        {
            DoInitialize();
        }

        /// <summary>
        /// Does the initialize.
        /// </summary>
        private void DoInitialize()
        {
            _altitude = Distance.Invalid;
            _altitudeAboveEllipsoid = _altitude;
            _bearing = Azimuth.Invalid;
            _heading = Azimuth.Invalid;
            _fixStatus = FixStatus.Unknown;
            _horizontalDop = DilutionOfPrecision.Maximum;
            _magneticVariation = Longitude.Invalid;
            _position = Position.Invalid;
            _speed = Speed.Invalid;
            _meanDop = DilutionOfPrecision.Invalid;
            _verticalDop = DilutionOfPrecision.Invalid;
            if (_satellites != null)
                _satellites.Clear();
        }

        /// <summary>
        /// Updates the UTC and local date/time to the specified UTC value.
        /// </summary>
        /// <param name="value">The value.</param>
        [SecurityCritical]
        protected void SetDateTimes(DateTime value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (value == _utcDateTime || value == DateTime.MinValue)
                return;

            // Yes. Set the new value
            _utcDateTime = value;
            _dateTime = _utcDateTime.ToLocalTime();

            // Are we updating the system clock?
            bool systemClockUpdated = false;
            if (IsFixed && Devices.IsClockSynchronizationEnabled)
            {
                // Yes. Only update the system clock if it's at least 1 second off;
                // otherwise, we'll end up updating the clock several times each second
                if (DateTime.UtcNow.Subtract(_utcDateTime).Duration().TotalSeconds > 1)
                {
                    // Notice: Setting the system clock to UTC will still respect local time zone and DST settings
                    NativeMethods2.SystemTime time = NativeMethods2.SystemTime.FromDateTime(_utcDateTime);
                    NativeMethods2.SetSystemTime(ref time);
                    systemClockUpdated = true;
                }
            }

            // Raise the events
            if (UtcDateTimeChanged != null)
                UtcDateTimeChanged(this, new DateTimeEventArgs(UtcDateTime, systemClockUpdated));
            if (DateTimeChanged != null)
                DateTimeChanged(this, new DateTimeEventArgs(DateTime, systemClockUpdated));
        }

        /// <summary>
        /// Updates the fix quality to the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetFixQuality(FixQuality value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (value == _fixQuality || value == FixQuality.Unknown)
                return;

            // Set the new value
            _fixQuality = value;

            // And notify
            if (FixQualityChanged != null)
                FixQualityChanged(this, new FixQualityEventArgs(_fixQuality));
        }

        /// <summary>
        /// Updates the fix mode to the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetFixMode(FixMode value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (value == _fixMode || value == FixMode.Unknown)
                return;

            // Set the new value
            _fixMode = value;

            // And notify
            if (FixModeChanged != null)
                FixModeChanged(this, new FixModeEventArgs(_fixMode));
        }

        /// <summary>
        /// Updates the fix method to the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetFixMethod(FixMethod value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (value == _fixMethod || value == FixMethod.Unknown)
                return;

            // Set the new value
            _fixMethod = value;

            // And notify
            if (FixMethodChanged != null)
                FixMethodChanged(this, new FixMethodEventArgs(_fixMethod));
        }

        /// <summary>
        /// Updates the precision as it relates to latitude and longitude.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetHorizontalDilutionOfPrecision(DilutionOfPrecision value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (_horizontalDop.Equals(value) || value.IsInvalid)
                return;

            // Yes.  Update the value
            _horizontalDop = value;

            // And notify of the change
            if (HorizontalDilutionOfPrecisionChanged != null)
                HorizontalDilutionOfPrecisionChanged(this, new DilutionOfPrecisionEventArgs(_horizontalDop));
        }

        /// <summary>
        /// Updates the precision as it relates to altitude.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetVerticalDilutionOfPrecision(DilutionOfPrecision value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (_verticalDop.Equals(value) || value.IsInvalid)
                return;

            // Yes.  Update the value
            _verticalDop = value;

            // And notify of the change
            if (VerticalDilutionOfPrecisionChanged != null)
                VerticalDilutionOfPrecisionChanged(this, new DilutionOfPrecisionEventArgs(_verticalDop));
        }

        /// <summary>
        /// Updates the precision as it relates to latitude, longitude and altitude.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetMeanDilutionOfPrecision(DilutionOfPrecision value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (_meanDop.Equals(value) || value.IsInvalid)
                return;

            // Yes.  Update the value
            _meanDop = value;

            // And notify of the change
            if (MeanDilutionOfPrecisionChanged != null)
                MeanDilutionOfPrecisionChanged(this, new DilutionOfPrecisionEventArgs(_meanDop));
        }

        /// <summary>
        /// Updates the fix status to the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetFixStatus(FixStatus value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (value == _fixStatus || value == FixStatus.Unknown)
                return;

            // Set the new status
            _fixStatus = value;

            DeviceEventArgs e = new DeviceEventArgs(_device);

            // Is a fix acquired or lost?
            if (_fixStatus == FixStatus.Fix)
            {
                // Acquired.
                if (FixAcquired != null)
                    FixAcquired(this, e);

                Devices.RaiseFixAcquired(e);
            }
            else
            {
                // Lost.
                if (FixLost != null)
                    FixLost(this, e);

                Devices.RaiseFixLost(e);
            }
        }

        /// <summary>
        /// Updates the difference between Magnetic North and True North.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetMagneticVariation(Longitude value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (_magneticVariation.Equals(value) || value.IsInvalid)
                return;

            // Yes.  Set the new value
            _magneticVariation = value;

            // And notify of the change
            if (MagneticVariationAvailable != null)
                MagneticVariationAvailable(this, new LongitudeEventArgs(_magneticVariation));
        }

        /// <summary>
        /// Updates the current location on Earth's surface.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetPosition(Position value)
        {
            // If the new value is invalid, ignore it
            if (value.IsInvalid)
                return;

            // Change the devices class
            Devices.Position = _position;

            // Notify of the value, even if it hasn't changed
            if (PositionReceived != null)
                PositionReceived(this, new PositionEventArgs(_position));

            // Has the value actually changed?  If not, skip it
            if (_position.Equals(value))
                return;

            #region Kalman Filtered Miller Genuine Draft

            // Are we using a filter?
            if (_isFilterEnabled)
            {
                if (!_filter.IsInitialized)
                {
                    _filter.Initialize(value);
                    _position = value;
                }
                else
                {
                    // Do we have enough information to apply a filter?
                    double fail = FixPrecisionEstimate.Value * _horizontalDop.Value * _verticalDop.Value;
                    if (fail == 0 || double.IsNaN(fail) || double.IsInfinity(fail))
                    {
                        // Nope. So just use the raw value
                        _position = value;
                    }
                    else
                    {
                        // Yep. So apply the filter
                        _position = _filter.Filter(
                            value,
                            FixPrecisionEstimate,
                            _horizontalDop,
                            _verticalDop,
                            _bearing,
                            _speed);
                    }
                }
            }
            else
            {
                // Yes. Set the new value
                _position = value;
            }

            #endregion Kalman Filtered Miller Genuine Draft

#if PocketPC
            if (PositionChanged != null)
                PositionChanged(this, new PositionEventArgs(_Position));
#else
            if (
                // Are they hooked into the event?
                PositionChanged != null
                // Is the async result null or finished?
                && (_positionChangedAsyncResult == null || _positionChangedAsyncResult.IsCompleted))
            {
                try
                {
                    // Does the call need to be completed?
                    if (_positionChangedAsyncResult != null)
                        PositionChanged.EndInvoke(_positionChangedAsyncResult);
                }
                finally
                {
                    // Invoke the new event, even if an exception occurred while completing the previous event
                    _positionChangedAsyncResult = PositionChanged.BeginInvoke(this, new PositionEventArgs(_position), null, null);
                }
            }
#endif
        }

        /// <summary>
        /// Updates the current direction of travel.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetBearing(Azimuth value)
        {
            // If the new value is invalid, ignore it
            if (value.IsInvalid)
                return;

            // Notify of the receipt
            if (BearingReceived != null)
                BearingReceived(this, new AzimuthEventArgs(_bearing));

            // Change the devices class
            Devices.Bearing = _bearing;

            // If the value hasn't changed, skip
            if (_bearing.Equals(value))
                return;

            // Yes. Set the new value
            _bearing = value;

            // Notify of the change
            if (BearingChanged != null)
                BearingChanged(this, new AzimuthEventArgs(_bearing));
        }
        
        /// <summary>
        /// Updates the current direction of heading.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetHeading(Azimuth value)
        {
            // If the new value is invalid, ignore it
            if (value.IsInvalid)
                return;

            // Notify of the receipt
            if (HeadingReceived != null)
                HeadingReceived(this, new AzimuthEventArgs(_heading));

            // Change the devices class
            Devices.Heading = _heading;

            // If the value hasn't changed, skip
            if (_heading.Equals(value))
                return;

            // Yes. Set the new value
            _heading = value;

            // Notify of the change
            if (HeadingChanged != null)
                HeadingChanged(this, new AzimuthEventArgs(_heading));
        }

        /// <summary>
        /// Updates the list of known GPS satellites.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void AppendSatellites(IList<Satellite> value)
        {
            /* Jon: GPS.NET 2.0 tested each satellite to determine when it changed, which
             * I think turned out to be overkill.  For 3.0, let's just use whatever new list
             * arrives and see if people complain about not being able to hook into satellite events.
             */
            int count = value.Count;
            for (int index = 0; index < count; index++)
            {
                Satellite satellite = value[index];
                if (!_satellites.Contains(satellite))
                    _satellites.Add(satellite);
            }

            // Notify of the change
            if (SatellitesChanged != null)
                SatellitesChanged(this, new SatelliteListEventArgs(_satellites));
        }

        /// <summary>
        /// Sets the fixed satellite count.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetFixedSatelliteCount(int value)
        {
            _fixedSatelliteCount = value;
        }

        /// <summary>
        /// Updates the list of fixed GPS satellites.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetFixedSatellites(IList<Satellite> value)
        {
            /* Jon: GPS.NET 2.0 tested each satellite to determine when it changed, which
             * I think turned out to be overkill.  For 3.0, let's just use whatever new list
             * arrives and see if people complain about not being able to hook into satellite events.
             */

            int count = _satellites.Count;
            int fixedCount = value.Count;
            bool hasChanged = false;

            for (int index = 0; index < count; index++)
            {
                // Get the existing satellite
                Satellite existing = _satellites[index];

                // Look for a matching fixed satellite
                bool isFixed = false;
                for (int fixedIndex = 0; fixedIndex < fixedCount; fixedIndex++)
                {
                    Satellite fixedSatellite = value[fixedIndex];
                    if (existing.PseudorandomNumber.Equals(fixedSatellite.PseudorandomNumber))
                    {
                        isFixed = true;
                        break;
                    }
                }

                // Update the satellite
                if (existing.IsFixed != isFixed)
                {
                    existing.IsFixed = isFixed;
                    _satellites[index] = existing; // Push updated item back into source collection
                    hasChanged = true;
                }
            }

            // Update the fixed-count
            SetFixedSatelliteCount(value.Count);

            // Notify of the change
            if (hasChanged)
            {
                if (SatellitesChanged != null)
                    SatellitesChanged(this, new SatelliteListEventArgs(_satellites));
            }
        }

        /// <summary>
        /// Updates the current rate of travel.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetSpeed(Speed value)
        {
            // Is the new value invalid?
            if (value.IsInvalid)
                return;

            // Notify of the receipt
            if (SpeedReceived != null)
                SpeedReceived(this, new SpeedEventArgs(_speed));

            // Change the devices class
            Devices.Speed = _speed;

            // Has anything changed?
            if (_speed.Equals(value))
                return;

            // Yes. Set the new value
            _speed = value;

            // Notify of the change
            if (SpeedChanged != null)
                SpeedChanged(this, new SpeedEventArgs(_speed));
        }

        /// <summary>
        /// Updates the distance between the ellipsoid surface and the current altitude.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetGeoidalSeparation(Distance value)
        {
            // If the value is the same or invalid, ignore it
            if (value.IsInvalid || _geoidalSeparation.Equals(value))
                return;

            // Yes. Set the new value
            _geoidalSeparation = value;

            // Notify of the change
            if (GeoidalSeparationChanged != null)
                GeoidalSeparationChanged(this, new DistanceEventArgs(_geoidalSeparation));
        }

        /// <summary>
        /// Updates the current distance above sea level.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetAltitude(Distance value)
        {
            // Is the new value invalid?
            if (value.IsInvalid)
                return;

            // Notify of the receipt
            if (AltitudeReceived != null)
                AltitudeReceived(this, new DistanceEventArgs(_altitude));

            // Change the devices class
            Devices.Altitude = _altitude;

            // Has anything changed?
            if (_altitude.Equals(value))
                return;

            // Yes. Set the new value
            _altitude = value;

            // Notify of the change
            if (AltitudeChanged != null)
                AltitudeChanged(this, new DistanceEventArgs(_altitude));
        }

        /// <summary>
        /// Updates the current distance above the ellipsoid surface.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void SetAltitudeAboveEllipsoid(Distance value)
        {
            // Notify of the receipt
            if (AltitudeAboveEllipsoidReceived != null)
                AltitudeAboveEllipsoidReceived(this, new DistanceEventArgs(_altitudeAboveEllipsoid));

            // Has anything changed?
            if (_altitudeAboveEllipsoid.Equals(value))
                return;

            // Yes. Set the new value
            _altitudeAboveEllipsoid = value;

            // Notify of the change
            if (AltitudeAboveEllipsoidChanged != null)
                AltitudeAboveEllipsoidChanged(this, new DistanceEventArgs(_altitudeAboveEllipsoid));
        }

        #endregion Protected Methods

        #region Abstract Methods

        /// <summary>
        /// Occurs when new data should be read from the underlying device.
        /// </summary>
        protected abstract void OnReadPacket();

        /// <summary>
        /// Occurs when the interpreter is using a different device for raw data.
        /// </summary>
        protected abstract void OnDeviceChanged();

        #endregion Abstract Methods

        #region Overrides

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        [SecuritySafeCritical]
        protected override void Dispose(bool disposing)
        {
            // Are we already disposed?
            if (_isDisposed)
                return;

            // We're disposed
            _isDisposed = true;

            // It's critical that these finalizers get run
#if !PocketPC
            RuntimeHelpers.PrepareConstrainedRegions();
#endif
            try
            {
                // Resume if we're paused
                if (_pausedWaitHandle != null
#if PocketPC
                    && _PausedWaitHandle.Handle.ToInt32() != -1
#else
 && !_pausedWaitHandle.SafeWaitHandle.IsClosed
#endif
)
                {
                    _pausedWaitHandle.Set();
                    _pausedWaitHandle.Close();
                }

                // Close the parsing thread
                if (_parsingThread != null
#if PocketPC
                    && _IsParsingThreadAlive
#else
 && _parsingThread.IsAlive
#endif
)
                {
                    _parsingThread.Abort();
                }

                // Close the stream
                if (_device != null)
                    _device.Close();
            }
            finally
            {
                // Are we disposing of managed resources?
                if (disposing)
                {
                    #region Dispose of managed resources

                    _recordingStream = null;
                    _parsingThread = null;
                    _pausedWaitHandle = null;
                    _device = null;
                    _filter = null;

                    // Clear out the satellites
                    if (_satellites != null)
                    {
                        _satellites.Clear();
                        _satellites = null;
                    }

                    // Destroy all event subscriptions
                    AltitudeChanged = null;
                    AltitudeReceived = null;
                    BearingChanged = null;
                    HeadingChanged = null;
                    BearingReceived = null;
                    HeadingReceived = null;
                    UtcDateTimeChanged = null;
                    DateTimeChanged = null;
                    FixAcquired = null;
                    FixLost = null;
                    PositionReceived = null;
                    PositionChanged = null;
                    MagneticVariationAvailable = null;
                    SpeedChanged = null;
                    SpeedReceived = null;
                    HorizontalDilutionOfPrecisionChanged = null;
                    VerticalDilutionOfPrecisionChanged = null;
                    MeanDilutionOfPrecisionChanged = null;
                    Starting = null;
                    Started = null;
                    Stopping = null;
                    Stopped = null;
                    Paused = null;
                    Resumed = null;
                    ExceptionOccurred = null;

                    #endregion Dispose of managed resources
                }

                // Are we running?
                if (_isRunning)
                {
                    OnStopped();
                    _isRunning = false;
                }

                // Continue disposing of the component
                base.Dispose(disposing);
            }
        }

        #endregion Overrides

        #region Private Methods

        /// <summary>
        /// Parsings the thread proc.
        /// </summary>
        private void ParsingThreadProc()
        {
#if PocketPC
            try
            {
                // Indicate that the parsing thread is active
                _IsParsingThreadAlive = true;
#endif

            // Loop while we're allowed
            while (!_isDisposed && _isRunning)
            {
                try
                {
                    // Wait until we're allowed to parse data
                    _pausedWaitHandle.WaitOne();

                    // Do we have any stream?
                    if (_device == null || !_device.IsOpen)
                    {
                        /* No.  This most likely occurs when an attempt to recover a connection just failed.
                         * In that situation, we just try again to make a connection.
                         */

                        // Try to find another device (or the same device)
                        _device = Devices.Any;

                        // If it's null then wait a while and try again
                        if (_device == null)
                        {
                            if (QueryReconnectAllowed())
                                continue;
                            return;
                        }

                        // Signal that we're starting
                        OnStarting();

                        // Signal that the stream has changed
                        OnDeviceChanged();

                        // And we're started
                        OnStarted();

                        // Reset the reconnection counter, since we only care about consecutive reconnects
                        _reconnectionAttemptCount = 0;

                        continue;
                    }

                    // Raise the virtual method for parsing
                    OnReadPacket();
                }
                catch (ThreadAbortException)
                {
                    // They've signaled an exit!  Stop immediately
                    return;
                }
                catch (IOException ex)
                {
                    /* An I/O exception usually indicates that the stream is no longer valid.
                     * Try to close the stream, and start again.
                     */

                    // Explain the error
                    OnConnectionLost(ex);

                    // Stop and reconnect
                    Reset();

                    // Are we automatically reconnecting?
                    if (QueryReconnectAllowed())
                        continue;
                    return;
                }
                catch (UnauthorizedAccessException ex)
                {
                    /* This exception usually indicates that the stream is no longer valid.
                     * Try to close the stream, and start again.
                     */

                    // Explain the error
                    OnConnectionLost(ex);

                    // Stop and reconnect
                    Reset();

                    // Are we automatically reconnecting?
                    if (QueryReconnectAllowed())
                        continue;
                    return;
                }
                catch (Exception ex)
                {
                    // Notify of the exception
                    OnExceptionOccurred(ex);
                }
            }

#if PocketPC
            }
            catch
            { }
            finally
            {
                // Indicate that the parsing thread is no longer active
                _IsParsingThreadAlive = false;
            }
#endif
        }

        /// <summary>
        /// Forces the current device to a closed state without disposing the underlying stream.
        /// </summary>
        private void Reset()
        {
            // Signal that we're stopping
            OnStopping();

            // Dispose of this connection
            if (_device != null)
                _device.Reset();

            // Signal the stop
            OnStopped();
        }

        /// <summary>
        /// Determines if automatic reconnection is currently allowed, based on the values of
        /// <see cref="AllowAutomaticReconnection"/>, <see cref="MaximumReconnectionAttempts"/>, and <see cref="_reconnectionAttemptCount"/>.
        /// If reconnection is allowed, then <see cref="_reconnectionAttemptCount"/> is incremented after a short delay.
        /// </summary>
        /// <returns><see langword="true">True</see> if another reconnection attempt should be made; otherwise, <see langword="false"/>.</returns>
        private bool QueryReconnectAllowed()
        {
            // Are we automatically reconnecting?
            if (_allowAutomaticReconnection)
            {
                // Determine if we've exceeded the maximum reconnects
                if (_maximumReconnectionAttempts == -1
                    || _reconnectionAttemptCount < _maximumReconnectionAttempts)
                {
                    /* Wait just a moment before reconnecting. This gives software such as the Bluetooth stack
                     * the ability to properly reset and make connections again.
                     */
                    Thread.Sleep(1000);

                    // Bump up the failure count
                    _reconnectionAttemptCount++;

                    return true;
                }
            }

            // Automatic reconnection is not allowed, or we have exceeded the maximum reconnects
            return false;
        }

        #endregion Private Methods
    }
}