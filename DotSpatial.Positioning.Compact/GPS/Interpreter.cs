using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using DotSpatial.Positioning.Gps.Filters;
using DotSpatial.Positioning.Gps.IO;
#if !PocketPC
using System.Runtime.ConstrainedExecution;
using System.Runtime.CompilerServices;
using System.Drawing;
#endif

namespace DotSpatial.Positioning.Gps
{
    /// <summary>Represents a base class for designing a GPS data interpreter.</summary>
    /// <remarks>
    /// 	<para>This class serves as the base class for all GPS data interpreters, regardless
    ///     of the protocol being used. For example, the <strong>NmeaInterpreter</strong> class
    ///     inherits from this class to process NMEA-0183 data from any data source. This class
    ///     provides basic functionality to start, pause, resume and stop the processing of GPS
    ///     data, and provides management of a thread used to process the next set of incoming
    ///     data.</para>
    /// 	<para>Inheritors should override the <strong>OnReadPacket</strong> event and
    ///     provide functionality to read the next packet of data from the underlying stream.
    ///     All raw GPS data must be provided in the form of a <strong>Stream</strong> object,
    ///     and the method should read and process only a single packet of data.</para>
    /// </remarks>
    /// <seealso cref="OnReadPacket">OnReadPacket Method</seealso>
#if !PocketPC
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
#endif    
    public abstract class Interpreter : Component
    {
        #region Private variables

        // Real-time GPS information
        private DateTime _utcDateTime;
        private DateTime _dateTime;
        private Distance _GeoidalSeparation;
        private Distance _Altitude;
        private Distance _AltitudeAboveEllipsoid;
        private Speed _Speed;
        private Azimuth _Bearing;
        private Position _Position;
        private Longitude _MagneticVariation;
        private FixStatus _FixStatus;
        private FixMode _FixMode;
        private FixMethod _FixMethod;
        private FixQuality _FixQuality;
        private int _FixedSatelliteCount;
        private bool _IsFixRequired;
        private DilutionOfPrecision _MaximumHorizontalDOP = DilutionOfPrecision.Maximum;
        private DilutionOfPrecision _MaximumVerticalDOP = DilutionOfPrecision.Maximum;
        private DilutionOfPrecision _HorizontalDOP;
        private DilutionOfPrecision _VerticalDOP;
        private DilutionOfPrecision _MeanDOP;
        private List<Satellite> _Satellites = new List<Satellite>(16);

        // Filtering
        private PrecisionFilter _Filter = KalmanFilter.Default;
        private bool _IsFilterEnabled = true;

        // Engine management
        private bool _IsRunning;
        private Device _Device;
#if !PocketPC
        private ThreadPriority _ThreadPriority = ThreadPriority.Normal;
#else
        private ThreadPriority _ThreadPriority = ThreadPriority.BelowNormal;
#endif
        private Thread _ParsingThread;
        private ManualResetEvent _PausedWaitHandle = new ManualResetEvent(true);
        private int _MaximumReconnectionAttempts = -1;
        private int _ReconnectionAttemptCount;
        private Stream _RecordingStream;

        private bool _IsDisposed;
        private bool _AllowAutomaticReconnection = true;

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
        private IAsyncResult _positionChangedAsyncResult;

        #endregion
#endif

        #endregion

        private static TimeSpan _CommandTimeout = TimeSpan.FromSeconds(5);
        private static TimeSpan _ReadTimeout = TimeSpan.FromSeconds(5);

        /// <summary>Represents a synchronization object which is locked during state changes.</summary>
        /// <value>An <strong>Object</strong>.</value>
        /// <remarks>
        /// 	<para>Since the <strong>Interpreter</strong> class is
        ///     multithreaded, this object is used to prevent changes in state from all happening
        ///     at once. For example, if two threads attempt to start and stop the interpreter,
        ///     this object is locked so that only one action can occur at a time. This approach
        ///     greatly improves the stability of the class.</para>
        /// 	<para>By default, this object is locked during calls to <strong>Start</strong>,
        ///     <strong>Stop</strong>, <strong>Pause</strong>, <strong>Resume</strong> and
        ///     <strong>OnReadPacket</strong>.</para>
        /// </remarks>
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
        /// Occurs when a new bearing report has been received, even if the value has not changed.
        /// </summary>
        public event EventHandler<AzimuthEventArgs> BearingReceived;
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
        /// <summary>Occurs when the magnetic variation for the current location becomes known.</summary>
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
                Starting(this, new DeviceEventArgs(_Device));
        }

        /// <summary>
        ///  Occurs immediately before data is processed.
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


        ///// <summary>
        ///// Obsolete.  See compiler warnings for upgrade help.
        ///// </summary>
        //[Obsolete("Use the 'OnDeviceChanged' override to be notified when the interpreter is using a new GPS device.")]
        //public virtual void OnBaseStreamChanged(Stream obsolete) { throw new NotSupportedException(); }

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
        /// <param name="ex"></param>
        protected virtual void OnExceptionOccurred(Exception ex)
        {
            if (ExceptionOccurred != null)
                ExceptionOccurred(this, new ExceptionEventArgs(ex));
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        protected Interpreter()  
            : base()
        { }

        #endregion

        #region Static members

        /// <summary>
        /// Controls the amount of time to wait for the next packet of GPS data to arrive.
        /// </summary>
        /// <remarks></remarks>
        public static TimeSpan ReadTimeout
        {
            get
            {
                return _ReadTimeout;
            }
            set
            {
                // Disallow any value zero or less
                if (_ReadTimeout.TotalMilliseconds <= 0)
                    throw new ArgumentOutOfRangeException("ReadTimeout", "The read timeout of a GPS interpreter must be greater than zero.  A value of about five seconds is typical.");

                // Set the new value
                _ReadTimeout = value;
            }
        }

        /// <summary>
        /// Controls the amount of time allowed to perform a start, stop, pause or resume action.
        /// </summary>
        /// <remarks>The <strong>Interpreter</strong> class is multithreaded and is also thread-safe.  Still, however, in some rare cases,
        /// two threads may attempt to change the state of the interpreter at the same time.  Critical sections will allow both threads to
        /// succees whenever possible, but in the event of a deadlock, this property control how much time to allow before giving up.</remarks>
        public static TimeSpan CommandTimeout
        {
            get
            {
                return _CommandTimeout;
            }
            set
            {
                if (_CommandTimeout.TotalSeconds < 1)
                    throw new ArgumentOutOfRangeException("CommandTimeout", "The command timeout of a GPS interpreter cannot be less than one second.  A value of about ten seconds is recommended.");

                _CommandTimeout = value;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns the device providing raw GPS data to the interpreter.
        /// </summary>
#if !PocketPC
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
                return _Device;
            }
        }

        /// <summary>
        /// Controls the priority of the thread which processes GPS data.
        /// </summary>
#if !PocketPC
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
                return _ThreadPriority;
            }
            set
            {
                // Set the new value
                _ThreadPriority = value;

                // If the parsing thread is alive, update it
                if (_ParsingThread != null
#if !PocketPC
 && _ParsingThread.IsAlive
#else
                    && _IsParsingThreadAlive 
#endif
)
                    _ParsingThread.Priority = _ThreadPriority;
            }
        }


        /// <summary>
        /// Returns the GPS-derived date and time, adjusted to the local time zone.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the GPS-derived date and time, adjusted to the local time zone.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public DateTime DateTime
        {
            get { return _dateTime; }
        }

        /// <summary>
        /// Returns the GPS-derived date and time in UTC (GMT-0).
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the GPS-derived date and time in UTC (GMT-0).")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public DateTime UtcDateTime
        {
            get { return _utcDateTime; }
        }

        /// <summary>
        /// Returns the current estimated precision as it relates to latitude and longitude.
        /// </summary>
        /// <remarks> Horizontal Dilution of Precision (HDOP) is the accumulated 
        /// error of latitude and longitude coordinates in X and Y directions
        /// (displacement on the surface of the ellipsoid).
        /// </remarks>
#if !PocketPC
        [Category("Precision")]
        [Description("Returns the current estimated precision as it relates to latitude and longitude.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public DilutionOfPrecision HorizontalDilutionOfPrecision
        {
            get { return _HorizontalDOP; }
        }

        /// <summary>
        /// Returns the current estimated precision as it relates to altitude.
        /// </summary>
        /// <remarks> Vertical Dilution of Precision (VDOP) is the accumulated 
        /// error of latitude and longitude coordinates in the Z direction (measurement 
        /// from the center of the ellipsoid).
        /// </remarks>
#if !PocketPC
        [Category("Precision")]
        [Description("Returns the current estimated precision as it relates to altitude.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public DilutionOfPrecision VerticalDilutionOfPrecision
        {
            get { return _VerticalDOP; }
        }

        /// <summary>
        /// Returns the kind of fix acquired by the GPS device.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the kind of fix acquired by the GPS device.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public FixMode FixMode
        {
            get { return _FixMode; }
        }

        /// <summary>
        /// Returns the number of satellites being used to calculate the current location.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the number of satellites being used to calculate the current location.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public int FixedSatelliteCount
        {
            get { return _FixedSatelliteCount; }
        }

        /// <summary>
        /// Returns the quality of the fix and what kinds of technologies are involved.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the quality of the fix and what kinds of technologies are involved.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public FixQuality FixQuality
        {
            get { return _FixQuality; }
        }

        /// <summary>
        /// Controls whether GPS data is ignored until a fix is acquired.
        /// </summary>
#if !PocketPC
        [Category("Behavior")]
        [Description("Controls whether GPS data is ignored until a fix is acquired.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(false)]
#endif
        public bool IsFixRequired
        {
            get { return _IsFixRequired; }
            set { _IsFixRequired = value; }
        }

        /// <summary>
        /// Returns whether a fix is currently acquired by the GPS device.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns whether a fix is currently acquired by the GPS device.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public bool IsFixed
        {
            get { return _FixStatus == FixStatus.Fix; }
        }

        /// <summary>
        /// Returns the difference between magnetic North and True North.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the difference between magnetic North and True North.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public Longitude MagneticVariation
        {
            get { return _MagneticVariation; }
        }

        /// <summary>
        /// Returns the current location on Earth's surface.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the current location on Earth's surface.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
#endif
        public Position Position
        {
            get { return _Position; }
        }

        /// <summary>
        /// Returns the avereage precision tolerance based on the fix quality reported
        /// by the device.
        /// </summary>
        /// <remarks>
        /// This property returns the estimated error attributed to the device. To get 
        /// a total error estimation, add the Horizontal or the Mean DOP to the 
        /// FixPrecisionEstimate.
        /// </remarks>
        public Distance FixPrecisionEstimate
        {
            get { return Device.PrecisionEstimate(_FixQuality); }
        }

        /// <summary>
        /// Returns the current rate of travel.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the current rate of travel.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public Speed Speed
        {
            get { return _Speed; }
        }

        /// <summary>
        /// Returns the current distance above sea level.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the current distance above sea level.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public Distance Altitude
        {
            get { return _Altitude; }
        }

#if !PocketPC
        [Category("Data")]
        [Description("Returns the current direction of travel.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public Azimuth Bearing
        {
            get { return _Bearing; }
        }

        /// <summary>
        /// Controls the largest amount of precision error allowed before GPS data is ignored.
        /// </summary>
        /// <remarks>This property is important for commercial GPS softwaqre development because it helps the interpreter determine
        /// when GPS data reports are precise enough to utilize.  Live GPS data can be inaccurate by up to a factor of fifty, or nearly
        /// the size of an American football field!  As a result, setting a vlue for this property can help to reduce precision errors.
        /// When set, reports of latitude, longitude, speed, and bearing are ignored if precision is not at or below the set value.
        /// For more on Dilution of Precision and how to determine your precision needs, please refer to our online article here:
        /// http://dotspatial.codeplex.com/Articles/WritingApps2_1.aspx.</remarks>
#if !PocketPC
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
                return _MaximumHorizontalDOP;
            }
            set
            {
                if (_MaximumHorizontalDOP.Equals(value))
                    return;

                if (value.Value <= 0.0f || value.Value > 50.0f)
                    throw new ArgumentOutOfRangeException("MaximumHorizontalDilutionOfPrecision", "The maximum allowed horizontal Dilution of Precision must be a value between 1 and 50.");

                _MaximumHorizontalDOP = value;
            }
        }

        /// <summary>
        /// Controls the maximum number of consecutive reconnection retries allowed.
        /// </summary>
#if !PocketPC
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
                return _MaximumReconnectionAttempts;
            }
            set
            {
                if (value < -1)
                    throw new ArgumentOutOfRangeException("MaximumReconnectionAttempts", "The maximum reconnection attempts for an interpreter must be -1 for infinite retries, or greater than zero for a specific amount.");

                _MaximumReconnectionAttempts = value;
            }
        }

        /// <summary>
        /// Controls the largest amount of precision error allowed before GPS data is ignored.
        /// </summary>
        /// <remarks>This property is important for commercial GPS softwaqre development because it helps the interpreter determine
        /// when GPS data reports are precise enough to utilize.  Live GPS data can be inaccurate by up to a factor of fifty, or nearly
        /// the size of an American football field!  As a result, setting a vlue for this property can help to reduce precision errors.
        /// When set, reports of altitude are ignored if precision is not at or below the set value.
        /// For more on Dilution of Precision and how to determine your precision needs, please refer to our online article here:
        /// http://dotspatial.codeplex.com/Articles/WritingApps2_1.aspx.</remarks>
#if !PocketPC
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
                return _MaximumVerticalDOP;
            }
            set
            {
                if (_MaximumVerticalDOP.Equals(value))
                    return;

                if (value.Value <= 0.0f || value.Value > 50.0f)
                    throw new ArgumentOutOfRangeException("MaximumVerticalDilutionOfPrecision", "The maximum allowed vertical Dilution of Precision must be a value between 1 and 50.");

                _MaximumVerticalDOP = value;
            }
        }

        /// <summary>
        /// Controls whether real-time GPS data is made more precise using a filter.
        /// </summary>
#if !PocketPC
        [Category("Precision")]
        [Description("Controls whether real-time GPS data is made more precise using a filter.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(true)]
#endif
        public bool IsFilterEnabled
        {
            get { return _IsFilterEnabled; }
            set { _IsFilterEnabled = value; }
        }

        /// <summary>
        /// Controls the technology used to reduce GPS precision error.
        /// </summary>
#if !PocketPC
        [Category("Precision")]
        [Description("Controls whether real-time GPS data is made more precise using a filter.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
#endif
        public PrecisionFilter Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        /// <summary>
        /// Controls whether the interpreter will try to automatically attempt to reconnect anytime a connection is lost.
        /// </summary>
        /// <remarks><para>Interpreters are able to automatically try to recover from connection failures.  When this property is set to <strong>True</strong>,
        /// the interpreter will detect a sudden loss of connection, then attempt to make a new connection to restore the flow of data.  If multiple GPS
        /// devices have been detected, any of them may be utilized as a "fail-over device."  Recovery attempts will continue repeatedly until a connection
        /// is restored, the interpreter is stopped, or the interpreter is disposed.</para>
        /// <para>For most applications, this property should be enabled to help improve the stability of the application.  In most cases, a sudden loss of
        /// data is only temporary, caused by a loss of battery power or when a wireless device moves too far out of range.</para></remarks>
#if !PocketPC
        [Category("Behavior")]
        [Description("Controls whether the interpreter will try to automatically attempt to reconnect anytime a connection is lost.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [DefaultValue(true)]
#endif
        public bool AllowAutomaticReconnection
        {
            get { return _AllowAutomaticReconnection; }
            set { _AllowAutomaticReconnection = value; }
        }

        /// <summary>
        /// Returns a list of known GPS satellites.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns a list of known GPS satellites.")]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
        public IList<Satellite> Satellites
        {
            get { return _Satellites; }
        }

        /// <summary>
        /// Returns whether resources in this object has been shut down.
        /// </summary>
        public bool IsDisposed
        {
            get { return _IsDisposed; }
        }

        /// <summary>
        /// Returns the stream used to output data received from the GPS device.
        /// </summary>
        public Stream RecordingStream
        {
            get { return _RecordingStream; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts processing GPS data using any available GPS device.
        /// </summary>
        /// <remarks>This method is used to begin processing GPS data.  If no GPS devices are known, GPS.NET will search for GPS devices and use the
        /// first device it finds.  If no device can be found, an exception is raised.</remarks>
        public void Start()
        {
            // If we're disposed, complain
            if (_IsDisposed)
                throw new ObjectDisposedException("The Interpreter cannot be started because it has been disposed.");

            // Are we already running?
            if(_IsRunning)
                return;

            // Prevent state changes while the interpreter is started
#if !PocketPC
            if (Monitor.TryEnter(SyncRoot, _CommandTimeout))
#else
            if (Monitor.TryEnter(SyncRoot))
#endif
            {
                try
                {
                    // Set the stream
                    _Device = Devices.Any;

                    // Is the stream null?
                    if (_Device == null)
                    {
                        // And report the problem
                        throw new InvalidOperationException("After performing a search, no GPS devices were found.");
                    }

                    // Signal that we're starting
                    OnStarting();

                    // Signal that the stream has changed
                    OnDeviceChanged();

                    // Indicate that we're running
                    _IsRunning = true;

                    // If the thread isn't alive, start it now
                    if (_ParsingThread == null 
#if !PocketPC
                        || !_ParsingThread.IsAlive
#else
                        || !_IsParsingThreadAlive 
#endif
                        )
                    {
                        _ParsingThread = new Thread(new ThreadStart(ParsingThreadProc));
                        _ParsingThread.IsBackground = true;
                        _ParsingThread.Priority = _ThreadPriority;
                        _ParsingThread.Name = "GPS.NET Parsing Thread (http://dotspatial.codeplex.com)";
                        _ParsingThread.Start();

                        // And signal it
                        OnStarted();
                    }
                    else
                    {
                        // Otherwise, allow parsing to continue
                        _PausedWaitHandle.Set();

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
        /// <remarks>
        /// This method will start the <strong>Interpreter</strong> using a separate thread.
        /// The <strong>OnReadPacket</strong> is then called repeatedly from that thread to process
        /// incoming data. The Pause, Resume and Stop methods are typically called after this
        /// method to change the interpreter's behavior. Finally, a call to
        /// <strong>Dispose</strong> will close the underlying stream, stop all processing, and
        /// shut down the processing thread.
        /// </remarks>
        /// <param name="stream">A <strong>Stream</strong> object providing GPS data to process.</param>
        public void Start(Device device)
        {
            // If we're disposed, complain
            if (_IsDisposed)
                throw new ObjectDisposedException("The Interpreter cannot be started because it has been disposed.");

            // Prevent state changes while the interpreter is started
#if !PocketPC
            if (Monitor.TryEnter(SyncRoot, _CommandTimeout))
#else
            if (Monitor.TryEnter(SyncRoot))
#endif
            {
                try
                {
                    // Set the device
                    _Device = device;

                    // Signal that we're starting
                    OnStarting();

                    // If it's not open, open it now
                    if (!_Device.IsOpen)
                        _Device.Open();

                    // Indicate that we're running
                    _IsRunning = true;

                    // Signal that the stream has changed
                    OnDeviceChanged();

                    // If the thread isn't alive, start it now
                    if (_ParsingThread == null 
#if !PocketPC
                        || !_ParsingThread.IsAlive
#else
                        || !_IsParsingThreadAlive 
#endif
                        )
                    {
                        _ParsingThread = new Thread(new ThreadStart(ParsingThreadProc));
                        _ParsingThread.IsBackground = true;
                        _ParsingThread.Priority = _ThreadPriority;
                        _ParsingThread.Name = "GPS.NET Parsing Thread (http://dotspatial.codeplex.com)";
                        _ParsingThread.Start();

                        // And signal the start
                        OnStarted();
                    }
                    else
                    {                        
                        // Otherwise, allow parsing to continue
                        _PausedWaitHandle.Set();

                        // Signal that we've resumed
                        OnResumed();
                    }

                }
                catch (Exception ex)
                {
                    // Close the device
                    _Device.Close();

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
        /// <param name="output"></param>
        public void StartRecording(Stream output)
        {
            // Prevent state changes while the interpreter is started
#if !PocketPC
            if (Monitor.TryEnter(RecordingSyncRoot, _CommandTimeout))
#else
            if (Monitor.TryEnter(RecordingSyncRoot))
#endif
            {
                // Set the recording stream
                _RecordingStream = output;

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
            if (Monitor.TryEnter(RecordingSyncRoot, _CommandTimeout))
#else
            if (Monitor.TryEnter(RecordingSyncRoot))
#endif
            {
                // Set the recording stream
                _RecordingStream = null;

                // And exit the context
                Monitor.Exit(RecordingSyncRoot);
            }
        }

        /// <summary>Stops all processing of GPS data.</summary>
        /// <remarks>
        /// This method is used some time after a call to the <strong>Start</strong> method.
        /// When called, the GPS processing thread is immediately shut down and all processing
        /// stops.
        /// </remarks>
        public void Stop()
        {
            // If we're disposed, complain
            if (_IsDisposed)
                throw new ObjectDisposedException("The Interpreter cannot be stopped because it has been disposed.");

#if !PocketPC
            if(Monitor.TryEnter(SyncRoot, _CommandTimeout))
#else
            if(Monitor.TryEnter(SyncRoot))
#endif
            {
                try
                {
                    // Are we already stopped?
                    if (!_IsRunning)
                        return;

                    // Signal that we're no longer running
                    _IsRunning = false;

                    // Signal that a stop is underway
                    OnStopping();

                    // Unpause the interpreter
                    _PausedWaitHandle.Set();

                    // Signal the thread to stop. If it takes too long, exit
                    if (_ParsingThread != null
#if !PocketPC
                        && _ParsingThread.IsAlive 
                        && !_ParsingThread.Join(_CommandTimeout))
#else
                        && _IsParsingThreadAlive
                        && !_ParsingThread.Join((int)_CommandTimeout.TotalMilliseconds))
#endif
                    {
                        _ParsingThread.Abort();
                    }

                    // Close the connection
                    _Device.Close();

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

        ///// <summary>
        ///// Obsolete.  See compiler warnings for upgrade help.
        ///// </summary>
        //[Obsolete("In GPS.NET 3.0, recording operations cannot be paused or resumed.  Use 'StopRecording' instead.")]
        //public void PauseRecording() { throw new NotSupportedException(); }

        ///// <summary>
        ///// Obsolete.  See compiler warnings for upgrade help.
        ///// </summary>
        //[Obsolete("In GPS.NET 3.0, recording operations cannot be paused or resumed.  Use 'StartRecording' instead.")]
        //public void ResumeRecording() { throw new NotSupportedException(); }

        /// <summary>Temporarily halts processing of GPS data.</summary>
        /// <remarks>
        /// This method will suspend the processing of GPS data, but will keep the thread and
        /// raw GPS data stream open. This method is intended as a temporary means of stopping
        /// processing. An interpreter should not be paused for an extended period of time because
        /// it can cause a backlog of GPS data
        /// </remarks>
        public void Pause()
        {
            // If we're disposed, complain
            if (_IsDisposed)
                throw new ObjectDisposedException("The interpreter cannot be paused because it has been disposed.");

#if !PocketPC
            if(Monitor.TryEnter(SyncRoot, _CommandTimeout))
#else
            if (Monitor.TryEnter(SyncRoot))
#endif
            {
                try
                {
                    // Reset the wait handle.  This will cause the thread to pause on its next time around
                    _PausedWaitHandle.Reset();

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
            if (_IsDisposed)
                throw new ObjectDisposedException("The interpreter cannot be resumed because it has been disposed.");

#if !PocketPC
            if (Monitor.TryEnter(SyncRoot, _CommandTimeout))
#else
            if (Monitor.TryEnter(SyncRoot))
#endif
            {
                try
                {
                    // Set the wait handle.  This will cause the thread to continue processing.
                    _PausedWaitHandle.Set();
                    
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

        #endregion

        #region Protected Methods

        /// <summary>
        /// Resets the interpreter to it's default values.
        /// </summary>
        protected virtual void Initialize()
        {
            _Altitude = Distance.Invalid;
            _AltitudeAboveEllipsoid = _Altitude;
            _Bearing = Azimuth.Invalid;
            _FixStatus = FixStatus.Unknown;
            _HorizontalDOP = DilutionOfPrecision.Maximum;
            _MagneticVariation = Longitude.Invalid;
            _Position = Position.Invalid;
            _Speed = Speed.Invalid;
            _MeanDOP = DilutionOfPrecision.Invalid;
            _VerticalDOP = DilutionOfPrecision.Invalid;
            if (_Satellites != null)
                _Satellites.Clear();
        }

        /// <summary>
        /// Updates the UTC and local date/time to the specified UTC value.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetDateTimes(DateTime value)
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
                    // Note: Setting the system clock to UTC will still respect local time zone and DST settings
                    NativeMethods.SYSTEMTIME time = NativeMethods.SYSTEMTIME.FromDateTime(_utcDateTime);
                    NativeMethods.SetSystemTime(ref time);
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
        /// <param name="value"></param>
        protected virtual void SetFixQuality(FixQuality value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (value == _FixQuality || value == FixQuality.Unknown)
                return;

            // Set the new value
            _FixQuality = value;

            // And notify
            if (FixQualityChanged != null)
                FixQualityChanged(this, new FixQualityEventArgs(_FixQuality));
        }

        /// <summary>
        /// Updates the fix mode to the specified value.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetFixMode(FixMode value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (value == _FixMode || value == FixMode.Unknown)
                return;

            // Set the new value
            _FixMode = value;

            // And notify
            if (FixModeChanged != null)
                FixModeChanged(this, new FixModeEventArgs(_FixMode));
        }

        /// <summary>
        /// Updates the fix method to the specified value.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetFixMethod(FixMethod value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (value == _FixMethod || value == FixMethod.Unknown)
                return;

            // Set the new value
            _FixMethod = value;

            // And notify
            if (FixMethodChanged != null)
                FixMethodChanged(this, new FixMethodEventArgs(_FixMethod));
        }

        /// <summary>
        /// Updates the precision as it relates to latitude and longitude.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetHorizontalDilutionOfPrecision(DilutionOfPrecision value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (_HorizontalDOP.Equals(value) || value.IsInvalid)
                return;

            // Yes.  Update the value
            _HorizontalDOP = value;

            // And notify of the change
            if (HorizontalDilutionOfPrecisionChanged != null)
                HorizontalDilutionOfPrecisionChanged(this, new DilutionOfPrecisionEventArgs(_HorizontalDOP));
        }

        /// <summary>
        /// Updates the precision as it relates to altitude.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetVerticalDilutionOfPrecision(DilutionOfPrecision value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (_VerticalDOP.Equals(value) || value.IsInvalid)
                return;

            // Yes.  Update the value
            _VerticalDOP = value;

            // And notify of the change
            if (VerticalDilutionOfPrecisionChanged != null)
                VerticalDilutionOfPrecisionChanged(this, new DilutionOfPrecisionEventArgs(_VerticalDOP));
        }

        /// <summary>
        /// Updates the precision as it relates to latitude, longitude and altitude.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetMeanDilutionOfPrecision(DilutionOfPrecision value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (_MeanDOP.Equals(value) || value.IsInvalid)
                return;

            // Yes.  Update the value
            _MeanDOP = value;

            // And notify of the change
            if (MeanDilutionOfPrecisionChanged != null)
                MeanDilutionOfPrecisionChanged(this, new DilutionOfPrecisionEventArgs(_MeanDOP));
        }

        /// <summary>
        /// Updates the fix status to the specified value.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetFixStatus(FixStatus value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (value == _FixStatus || value == FixStatus.Unknown)
                return;

            // Set the new status
            _FixStatus = value;

            DeviceEventArgs e = new DeviceEventArgs(_Device);

            // Is a fix acquired or lost?
            if (_FixStatus == FixStatus.Fix)
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
        /// <param name="value"></param>
        protected virtual void SetMagneticVariation(Longitude value)
        {
            // If the new value is the same or it's invalid, ignore it
            if (_MagneticVariation.Equals(value) || value.IsInvalid)
                return;

            // Yes.  Set the new value
            _MagneticVariation = value;

            // And notify of the change
            if (MagneticVariationAvailable != null)
                MagneticVariationAvailable(this, new LongitudeEventArgs(_MagneticVariation));
        }

        /// <summary>
        /// Updates the current location on Earth's surface.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetPosition(Position value)
        {
            // If the new value is invalid, ignore it
            if (value.IsInvalid)
                return;

            // Change the devices class
            Devices.Position = _Position;

            // Notify of the value, even if it hasn't changed
            if (PositionReceived != null)
                PositionReceived(this, new PositionEventArgs(_Position));

            // Has the value actually changed?  If not, skip it
            if(_Position.Equals(value))
                return;

            #region Kalman Filtered Miller Genuine Draft

            // Are we using a filter?
            if (_IsFilterEnabled)
            {
                if (!_Filter.IsInitialized)
                {
                    _Filter.Initialize(value);
                    _Position = value;
                }
                else
                {
                    // Do we have enough information to apply a filter?
                    double fail = this.FixPrecisionEstimate.Value * this._HorizontalDOP.Value * this._VerticalDOP.Value;
                    if (fail == 0 || double.IsNaN(fail) || double.IsInfinity(fail))
                    {
                        // Nope. So just use the raw value
                        _Position = value;
                    }
                    else
                    {
                        // Yep. So apply the filter
                        _Position = _Filter.Filter(
                            value,
                            this.FixPrecisionEstimate,
                            this._HorizontalDOP,
                            this._VerticalDOP,
                            this._Bearing,
                            this._Speed);
                    }
                }
            }
            else
            {
                // Yes. Set the new value
                _Position = value;
            }

            #endregion

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
                    _positionChangedAsyncResult = PositionChanged.BeginInvoke(this, new PositionEventArgs(_Position), null, null);
                }
            }
#endif
        }

        /// <summary>
        /// Updates the current direction of travel.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetBearing(Azimuth value)
        {
            // If the new value is invalid, ignore it
            if (value.IsInvalid)
                return;

            // Notify of the receipt
            if (BearingReceived != null)
                BearingReceived(this, new AzimuthEventArgs(_Bearing));

            // Change the devices class
            Devices.Bearing = _Bearing;

            // If the value hasn't changed, skiiiip
            if (_Bearing.Equals(value))
                return;

            // Yes. Set the new value
            _Bearing = value;

            // Notify of the change
            if (BearingChanged != null)
                BearingChanged(this, new AzimuthEventArgs(_Bearing));
        }

        /// <summary>
        /// Updates the list of known GPS satellites.
        /// </summary>
        /// <param name="value"></param>
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
                if (!_Satellites.Contains(satellite))
                    _Satellites.Add(satellite);
            }

            // Notify of the change
            if (SatellitesChanged != null)
                SatellitesChanged(this, new SatelliteListEventArgs(_Satellites));
        }

        protected virtual void SetFixedSatelliteCount(int value)
        {
            _FixedSatelliteCount = value;
        }

        /// <summary>
        /// Updates the list of fixed GPS satellites.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetFixedSatellites(IList<Satellite> value)
        {
            /* Jon: GPS.NET 2.0 tested each satellite to determine when it changed, which
             * I think turned out to be overkill.  For 3.0, let's just use whatever new list
             * arrives and see if people complain about not being able to hook into satellite events.
             */

            int count = _Satellites.Count;
            int fixedCount = value.Count;
            bool hasChanged = false;

            for (int index = 0; index < count; index++)
            {
                // Get the existing satellite
                Satellite existing = _Satellites[index];

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
                    hasChanged = true;
                }
            }

            // Update the fixed-count
            SetFixedSatelliteCount(value.Count);

            // Notify of the change
            if (hasChanged)
            {
                if (SatellitesChanged != null)
                    SatellitesChanged(this, new SatelliteListEventArgs(_Satellites));
            }
        }

        /// <summary>
        /// Updates the current rate of travel.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetSpeed(Speed value)
        {
            // Is the new value invalid?
            if (value.IsInvalid)
                return;

            // Notify of the receipt
            if (SpeedReceived != null)
                SpeedReceived(this, new SpeedEventArgs(_Speed));

            // Change the devices class
            Devices.Speed = _Speed;

            // Has anything changed?
            if (_Speed.Equals(value))
                return;

            // Yes. Set the new value
            _Speed = value;

            // Notify of the change
            if (SpeedChanged != null)
                SpeedChanged(this, new SpeedEventArgs(_Speed));
        }

        /// <summary>
        /// Updates the distance between the ellipsoid surface and the current altitude.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetGeoidalSeparation(Distance value)
        {
            // If the value is the same or invalid, ignore it
            if (value.IsInvalid || _GeoidalSeparation.Equals(value))
                return;

            // Yes. Set the new value
            _GeoidalSeparation = value;

            // Notify of the change
            if (GeoidalSeparationChanged != null)
                GeoidalSeparationChanged(this, new DistanceEventArgs(_GeoidalSeparation));
        }

        /// <summary>
        /// Updates the current distance above sea level.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetAltitude(Distance value)
        {
            // Is the new value invalid?
            if (value.IsInvalid)
                return;

            // Notify of the receipt
            if (AltitudeReceived != null)
                AltitudeReceived(this, new DistanceEventArgs(_Altitude));

            // Change the devices class
            Devices.Altitude = _Altitude;

            // Has anything changed?
            if (_Altitude.Equals(value))
                return;

            // Yes. Set the new value
            _Altitude = value;

            // Notify of the change
            if (AltitudeChanged != null)
                AltitudeChanged(this, new DistanceEventArgs(_Altitude));
        }

        /// <summary>
        /// Updates the current distance above the ellipsoid surface.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void SetAltitudeAboveEllipsoid(Distance value)
        {
            // Notify of the receipt
            if (AltitudeAboveEllipsoidReceived != null)
                AltitudeAboveEllipsoidReceived(this, new DistanceEventArgs(_AltitudeAboveEllipsoid));

            // Has anything changed?
            if (_AltitudeAboveEllipsoid.Equals(value))
                return;

            // Yes. Set the new value
            _AltitudeAboveEllipsoid = value;

            // Notify of the change
            if (AltitudeAboveEllipsoidChanged != null)
                AltitudeAboveEllipsoidChanged(this, new DistanceEventArgs(_AltitudeAboveEllipsoid));
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Occurs when new data should be read from the underlying device.
        /// </summary>
        protected abstract void OnReadPacket();

        /// <summary>
        /// Occurs when the interpreter is using a different device for raw data.
        /// </summary>
        protected abstract void OnDeviceChanged();

        #endregion

        #region Overrides
        
        protected override void Dispose(bool disposing)
        {
            // Are we already disposed?
            if (_IsDisposed)
                return;

            // We're disposed
            _IsDisposed = true;

            // It's critical that these finalizers get run
#if !PocketPC
            RuntimeHelpers.PrepareConstrainedRegions();
#endif
            try
            {
                // Resume if we're paused
                if (_PausedWaitHandle != null
#if PocketPC
                    && _PausedWaitHandle.Handle.ToInt32() != -1
#else
                    && !_PausedWaitHandle.SafeWaitHandle.IsClosed
#endif
                    )
                {
                    _PausedWaitHandle.Set();
                    _PausedWaitHandle.Close();
                }
               
                // Close the parsing thread
                if(_ParsingThread != null
#if PocketPC
                    && _IsParsingThreadAlive 
#else
                    && _ParsingThread.IsAlive
#endif
                    )
                {
                    _ParsingThread.Abort();
                }

                // Close the stream
                if(_Device != null)
                    _Device.Close();
            }
            catch { }
            finally
            {
                // Are we disposing of managed resources?
                if (disposing)
                {
                    #region Dispose of managed resources

                    _RecordingStream = null;
                    _ParsingThread = null;
                    _PausedWaitHandle = null;
                    _Device = null;
                    _Filter = null;

                    // Clear out the satellites
                    if (_Satellites != null)
                    {
                        _Satellites.Clear();
                        _Satellites = null;
                    }

                    // Destroy all event subscriptions
                    AltitudeChanged = null;
                    AltitudeReceived = null;
                    BearingChanged = null;
                    BearingReceived = null;
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

                    #endregion
                }

                // Are we running?
                if (_IsRunning)
                {
                    OnStopped();
                    _IsRunning = false;
                }

                // Continue disposing of the component
                base.Dispose(disposing);
            }
        }

        #endregion

        #region Private Methods

        private void ParsingThreadProc()
        {
#if PocketPC
            try
            {
                // Indicate that the parsing thread is active
                _IsParsingThreadAlive = true;
#endif

            // Loop while we're allowed
            while (!_IsDisposed && _IsRunning)
            {
                try
                {
                    // Wait until we're allowed to parse data
                    _PausedWaitHandle.WaitOne();

                    // Do we have any stream?
                    if (_Device == null || !_Device.IsOpen)
                    {
                        /* No.  This most likely occurs when an attempt to recover a connection just failed.
                         * In that situation, we just try again to make a connection.
                         */

                        // Try to find another device (or the same device)
                        _Device = Devices.Any;

                        // If it's null then wait a while and try again
                        if (_Device == null)
                        {
                            if (QueryReconnectAllowed())
                                continue;
                            else
                                return;
                        }

                        // Signal that we're starting
                        OnStarting();

                        // Signal that the stream has changed
                        OnDeviceChanged();

                        // And we're started
                        OnStarted();

                        // Reset the reconnection counter, since we only care about consecutive reconnects
                        _ReconnectionAttemptCount = 0;

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
                    else
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
                    else
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
            if (_Device != null)
                _Device.Reset();

            // Signal the stop
            OnStopped();
        }

        /// <summary>
        /// Determines if automatic reconnection is currently allowed, based on the values of 
        /// <see cref="AllowAutomaticReconnection"/>, <see cref="MaximumReconnectionAttempts"/>, and <see cref="_ReconnectionAttemptCount"/>.
        /// If reconnection is allowed, then <see cref="_ReconnectionAttemptCount"/> is incremented after a short delay.
        /// </summary>
        /// <returns>
        /// <see langword="true">True</see> if another reconnection attempt should be made; otherwise, <see langword="false"/>.
        /// </returns>
        private bool QueryReconnectAllowed()
        {
            // Are we automatically reconnecting?
            if (_AllowAutomaticReconnection)
            {
                // Determine if we've exceeded the maximum reconnects
                if (_MaximumReconnectionAttempts == -1
                    || _ReconnectionAttemptCount < _MaximumReconnectionAttempts)
                {
                    /* Wait just a moment before reconnecting. This gives software such as the Bluetooth stack
                     * the ability to properly reset and make connections again.
                     */
                    Thread.Sleep(1000);

                    // Bump up the failure count
                    _ReconnectionAttemptCount++;

                    return true;
                }
            }

            // Automatic reconnection is not allowed, or we have exceeded the maximum reconnects
            return false;
        }

        #endregion
    }
}
