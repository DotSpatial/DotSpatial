// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using System.Threading;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a device on the local machine.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [DefaultProperty("Name")]
    public abstract class Device : IDisposable, IFormattable, IComparable<Device>
    {
        /// <summary>
        ///
        /// </summary>
        private bool _allowConnections = true;

        /// <summary>
        ///
        /// </summary>
        private DateTime _connectionStarted;

        /// <summary>
        ///
        /// </summary>
        private Thread _detectionThread;
        /// <summary>
        ///
        /// </summary>
        private ManualResetEvent _detectionStartedWaitHandle = new(false);

        /// <summary>
        ///
        /// </summary>
        private readonly object _syncRoot = new();

        /// <summary>
        ///
        /// </summary>
        private static TimeSpan _defaultReadTimeout = TimeSpan.FromSeconds(3);
        /// <summary>
        ///
        /// </summary>
        private static TimeSpan _defaultWriteTimeout = TimeSpan.FromSeconds(3);

        /// <summary>
        ///
        /// </summary>
        internal const string DEFAULT_REGISTRY_VALUE_NAME = "";

        #region Constructors

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="Device"/> is reclaimed by garbage collection.
        /// </summary>
        ~Device()
        {
            Dispose(false);
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when a connection is about to be opened.
        /// </summary>
        public event EventHandler Connecting;
        /// <summary>
        /// Occurs when a new connection has opened successfully.
        /// </summary>
        public event EventHandler Connected;
        /// <summary>
        /// Occurs when an open connection is about to be closed.
        /// </summary>
        public event EventHandler Disconnecting;
        /// <summary>
        /// Occurs when an open connection has been closed.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Occurs when a connection is about to be attempted.
        /// </summary>
        protected virtual void OnConnecting()
        {
            // Note the time. We'll use this to calculate the connection time later
            _connectionStarted = DateTime.Now;

            Connecting?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when a connection has been successfully established.
        /// </summary>
        protected virtual void OnConnected()
        {
            // How long did it take to connect?
            TotalConnectionTime = TotalConnectionTime.Add(DateTime.Now.Subtract(_connectionStarted));

            // Raise an event
            Connected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when an open connection is about to be closed.
        /// </summary>
        protected virtual void OnDisconnecting()
        {
            Disconnecting?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when an open connection has been successfully closed.
        /// </summary>
        protected virtual void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        #endregion Events

        #region Public Properties

        /// <summary>
        /// Returns a natural language name for the device.
        /// </summary>
        [Category("Data")]
        [Description("Returns a natural language name for the device.")]
        [Browsable(true)]
        [ParenthesizePropertyName(true)]
        public virtual string Name => "Unidentified Device";

        /// <summary>
        /// Returns a reset event used to determine when GPS detection has completed.
        /// </summary>
        [Browsable(false)]
        public ManualResetEvent DetectionWaitHandle { get; private set; } = new(false);

        /// <summary>
        /// Returns whether a connection is established with the device.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns whether a connection is established with the device.")]
        [Browsable(true)]
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Returns the stream associated with this device.
        /// </summary>
        /// <remarks>This property is provided solely for more advanced developers who need to interact directly with a device.
        /// During normal operations, the stream returned by this property is managed by this class.  As a result, it is not necessary
        /// to dispose of this stream.  If no connection is open, this property will return <strong>null</strong>.</remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Stream BaseStream { get; private set; }

        /// <summary>
        /// Returns the date and time the device was last confirmed as a GPS device.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the date and time the device was last confirmed as a GPS device.")]
        [Browsable(true)]
        public DateTime DateDetected { get; private set; }

        /// <summary>
        /// Returns the date and time the device last opened a connection.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the date and time the device last opened a connection.")]
        [Browsable(true)]
        public DateTime DateConnected { get; private set; }

        /// <summary>
        /// Returns whether the device is currently being examined for GPS data.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns whether the device is currently being examined for GPS data.")]
        [Browsable(true)]
        public bool IsDetectionInProgress => _detectionThread != null && _detectionThread.IsAlive;

        /// <summary>
        /// Controls whether the device can be queried for GPS data.
        /// </summary>
        /// <value><c>true</c> if [allow connections]; otherwise, <c>false</c>.</value>
        /// <remarks>In some cases, an attempt to open a connection to a device can cause problems.  For example,
        /// a serial port may be assigned to a barcode reader, or a Bluetooth device may represent a downstairs neighbor's
        /// computer.  This property allows a device to be left alone; no connection will be attempted to the device
        /// for any reason.</remarks>
        [Category("Behavior")]
        [Description("Controls whether the device can be queried for GPS data.")]
        [Browsable(true)]
        [DefaultValue(true)]
        public virtual bool AllowConnections
        {
            get => _allowConnections;
            set => _allowConnections = value;
        }

        /// <summary>
        /// Returns whether GPS protocol detection has been completed.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns whether GPS protocol detection has been completed.")]
        [Browsable(true)]
        public bool IsDetectionCompleted { get; private set; }

        /// <summary>
        /// Returns whether the device has been confirmed as a GPS device.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns whether the device has been confirmed as a GPS device.")]
        [Browsable(true)]
        public bool IsGpsDevice { get; private set; }

        /// <summary>
        /// Controls the number of times this device has been confirmed as a GPS device.
        /// </summary>
        /// <remarks>In order to maximize the performance of detecting GPS devices, statistics are maintained for each device.  This
        /// property indicates how many times the device has been confirmed as a GPS device.  It will be incremented automatically if
        /// a call to <strong>DetectProtocol</strong> has been successful.</remarks>
        [Category("Statistics")]
        [Description("Controls the number of times this device has been confirmed as a GPS device.")]
        [Browsable(true)]
        public int SuccessfulDetectionCount { get; private set; }

        /// <summary>
        /// Controls the number of times this device has failed to identify itself as a GPS device.
        /// </summary>
        /// <remarks>In order to prioritize and maximize performance of GPS device detection, statistics are kept for each device.
        /// These statistics control how detection is performed in the future.  For example, a device with a success rate of 100 and a failure
        /// rate of 1 would be tested before a device with a success rate of zero.  This property is updated automatically based on the
        /// results of a call to <strong>DetectProtocol</strong>.</remarks>
        [Category("Statistics")]
        [Description("Controls the number of times this device has failed to identify itself as a GPS device.")]
        [Browsable(true)]
        public int FailedDetectionCount { get; private set; }

        /// <summary>
        /// Returns the chance that a connection to the device will be successful.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the chance that a connection to the device will be successful.")]
        [Browsable(true)]
        [ParenthesizePropertyName(true)]
        public Percent Reliability
        {
            get
            {
                if (SuccessfulDetectionCount == 0)
                {
                    return Percent.Zero;
                }

                if (FailedDetectionCount == 0)
                {
                    return Percent.OneHundredPercent;
                }

                return new Percent(SuccessfulDetectionCount / Convert.ToSingle(SuccessfulDetectionCount + FailedDetectionCount));
            }
        }

        /// <summary>
        /// Returns the average amount of time required to open a connection.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the average amount of time required to open a connection.")]
        [Browsable(true)]
        public TimeSpan AverageConnectionTime
        {
            get
            {
                if (SuccessfulDetectionCount == 0)
                {
                    return TimeSpan.Zero;
                }

                return TimeSpan.FromMilliseconds(TotalConnectionTime.TotalMilliseconds / SuccessfulDetectionCount);
            }
        }

        /// <summary>
        /// Returns the total amount of time spent so far opening a connection.
        /// </summary>
        [Category("Statistics")]
        [Description("Returns the total amount of time spent so far opening a connection.")]
        [Browsable(true)]
        public TimeSpan TotalConnectionTime { get; private set; }

        #endregion Public Properties

        #region Protected Methods

        /// <summary>
        /// Updates the date a connection was last opened.
        /// </summary>
        /// <param name="date">The date.</param>
        protected void SetDateConnected(DateTime date)
        {
            DateConnected = date;
        }

        /// <summary>
        /// Updates the date the device was last confirmed as a GPS device.
        /// </summary>
        /// <param name="date">The date.</param>
        protected void SetDateDetected(DateTime date)
        {
            DateDetected = date;
        }

        /// <summary>
        /// Updates the total time spent connecting to this device.
        /// </summary>
        /// <param name="time">The time.</param>
        protected void SetTotalConnectionTime(TimeSpan time)
        {
            TotalConnectionTime = time;
        }

        /// <summary>
        /// Updates the number of times detection has failed.
        /// </summary>
        /// <param name="count">The count.</param>
        protected void SetFailedDetectionCount(int count)
        {
            FailedDetectionCount = count;
        }

        /// <summary>
        /// Updates the number of times detection has succeeded.
        /// </summary>
        /// <param name="count">The count.</param>
        protected void SetSuccessfulDetectionCount(int count)
        {
            SuccessfulDetectionCount = count;
        }

        /// <summary>
        /// Creates a new Stream object for the device.
        /// </summary>
        /// <param name="access">The access.</param>
        /// <param name="sharing">The sharing.</param>
        /// <returns>A <strong>Stream</strong> object.</returns>
        /// <remarks>Developers who design their own GPS device classes will need to override this method and use it to establish
        /// a new connection.  This method is called by the <strong>Open</strong> method.  The <strong>Stream</strong> returned by
        /// this method will be assigned to the <strong>BaseStream</strong> property, it it will be used for all device communications,
        /// including GPS protocol detection.</remarks>
        protected abstract Stream OpenStream(FileAccess access, FileShare sharing);

        /// <summary>
        /// Records information about this device to the registry.
        /// </summary>
        /// <remarks>In order to organize and maximize performance of GPS device detection, information about devices
        /// is saved to the registry.  This method, when overridden, serializes the device to the registry.  All registry
        /// keys should be saved under the branch <strong>HKLM\Software\DotSpatial.Positioning\GPS.NET\3.0\Devices\[Type]\[ID]</strong>, where
        /// <strong>[Type]</strong> is the name of the technology used by the device (e.g. Serial, Bluetooth, etc.), and <strong>[ID]</strong>
        /// is a unique ID for the device (e.g. "COM3", an address, etc.).  Enough information should be written to be able to
        /// establish a connection to the device after reading registry values.</remarks>
        protected abstract void OnCacheWrite();

        /// <summary>
        /// Reads information about this device from the registry.
        /// </summary>
        /// <remarks>In order to organize and maximize performance of GPS device detection, information about devices
        /// is saved to the registry.  This method, when overridden, reads information for this device from the registry.  All registry
        /// values should have been previously saved via the <strong>OnCacheWrite</strong> method.</remarks>
        protected abstract void OnCacheRead();

        /// <summary>
        /// Removes previously cached information for this device from the registry.
        /// </summary>
        /// <remarks>In some cases, the information about previously detected devices may interfere with system changes such as adding
        /// a replacement GPS device.  This method will remove the entire registry key for a device, causing GPS.NET 3.0 to start over when
        /// GPS devices need to be detected.</remarks>
        protected abstract void OnCacheRemove();

        /// <summary>
        /// Performs a test on the device to confirm that it transmits GPS data.
        /// </summary>
        /// <returns>A <strong>Boolean</strong> value, <strong>True</strong> if the device is confirmed.</returns>
        /// <remarks><para>This method will open a connection if necessary via the <strong>Open</strong> method, then proceed to
        /// examine it for GPS data.  This method will always be called on a separate thread to keep detection from slowing
        /// down or blocking the main application.</para>
        ///   <para>Developers who override this method should ensure that the method can clean up any resources used, even if a
        /// ThreadAbortException is raised, as a result of the detection thread being aborted.  This can be done by wrapping all code in a
        ///   <strong>try..catch</strong> block, and placing all clean-up code in a <strong>finally</strong> block.</para></remarks>
        protected virtual bool DetectProtocol()
        {
            // Is it already detected?
            if (IsGpsDevice)
            {
                Devices.Add(this);
                return true;
            }

            bool isOpenedByUs = false;

            // Is the device connected?
            if (!IsOpen)
            {
                try
                {
                    // No. Open a connection
                    Open();

                    // Indicate that we opened it
                    isOpenedByUs = true;
                }
                catch (Exception ex)
                {
                    // Report the error
                    Debug.WriteLine(Name + " could not be opened due to the following error: " + ex, Devices.DEBUG_CATEGORY);
                    Devices.OnDeviceDetectionAttemptFailed(
                        new DeviceDetectionException(this, "The device '" + Name + "' could not be identified as a GPS device because a connection could not be opened.  See InnerException for details.", ex));

                    // And exit
                    return false;
                }
            }

            // By default, a stream is opened, and it is tested.  Straightforward enough
            try
            {
                // Is this NMEA?  If not, return failure
                if (!NmeaReader.IsNmea(BaseStream))
                {
                    return false;
                }

                // Yes!  If nobody needs this stream, and we created it, close it
                if (!Devices.IsStreamNeeded && isOpenedByUs)
                {
                    Close();
                }

                // Return success
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Name + " could not be tested for GPS data due to the following error: " + ex, Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(
                    new DeviceDetectionException(this, "The device '" + Name + "' could not be tested for GPS data.  See Inner Exception for details.", ex));

                // If we opened the connection, close it to clean up
                if (isOpenedByUs)
                {
                    Close();
                }

                // And return failure
                return false;
            }
        }

        #endregion Protected Methods

        #region Public Methods

        /// <summary>
        /// Opens a new connection to the device.
        /// </summary>
        /// <remarks>This method will create a new connection to the device.  The connection will be created in the form of a
        /// <strong>Stream</strong> object.  If a connection is already open, this method has no effect.</remarks>
        public void Open()
        {
            /* By default, GPS operations are entirely read-only.  People can still have a need to write to a
             * device, however.  They can use the Open(FileAccess, FileShare) overload in this case.
             */
            Open(FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Opens a new connection to the device.
        /// </summary>
        /// <param name="access">The access.</param>
        /// <param name="sharing">The sharing.</param>
        /// <remarks>This method will create a new connection to the device.  The connection will be created in the form of a
        /// <strong>Stream</strong> object.  If a connection is already open, this method has no effect.</remarks>
        public virtual void Open(FileAccess access, FileShare sharing)
        {
            // Prevent other threads from opening or closing while we work
            lock (_syncRoot)
            {
                // Are we already connected?  If so, exit
                if (IsOpen)
                {
                    return;
                }

                //  Signal that we're connecting
                OnConnecting();

                try
                {
                    // Make a new connection
                    BaseStream = OpenStream(access, sharing);

                    // If it worked, then we need the finalizer
                    GC.ReRegisterForFinalize(this);

                    // Update the last-connected date
                    DateConnected = DateTime.Now;
                }
                finally
                {
                    // Do we have a stream?
                    if (BaseStream != null)
                    {
                        // Yes.  Signal that we're connected
                        IsOpen = true;
                        OnConnected();
                    }
                    else
                    {
                        // No.
                        IsOpen = false;
                        OnDisconnected();
                    }
                }
            }
        }

        /// <summary>
        /// Forces a device to a closed state without disposing the underlying stream.
        /// </summary>
        public virtual void Reset()
        {
            if (!IsOpen)
            {
                return;
            }

            // Flag that we're closed
            IsOpen = false;

            /* Do not close the stream!  We set it to null and just de-reference it.  We do this
             * because in some cases (such as a suspend/resume), the PDA has cleaned up the connection
             * already; a call to close the stream results in a hang.  This works around suspend/resume
             * closure by just resetting the device.
             */
            BaseStream = null;
        }

        /// <summary>
        /// Cancels detection and removes any cached information about the device.
        /// Use the <see cref="BeginDetection"/> method to re-detect the device and re-add it to the device cache.
        /// </summary>
        public void Undetect()
        {
            // Cancel any active detection
            CancelDetection();

            // Remove the device from the detection cache
            OnCacheRemove();

            // And clear all flags
            IsGpsDevice = false;
            IsDetectionCompleted = false;
            IsOpen = false;
            _connectionStarted = DateTime.MinValue;
        }

        /// <summary>
        /// Performs an analysis of the device and its capabilities.
        /// </summary>
        /// <returns></returns>
        public DeviceAnalysis Test()
        {
            bool isWorkingProperly = false;
            bool isPositionSupported = false;
            bool isAltitudeSupported = false;
            bool isBearingSupported = false;
            bool isPrecisionSupported = false;
            bool isSpeedSupported = false;
            bool isSatellitesSupported = false;
            List<string> sentences = new();

            // Open the device
            Open();

            // Write a header
            StringBuilder log = new();
            log.Append("------------------------------------------------------------------------------------------------------------------------------\r\n");
            log.Append("GPS.NET 3.0 Diagnostics    Copyright © 2009  DotSpatial.Positioning\r\n");
            log.Append("                                   http://dotspatial.codeplex.com\r\n");
            log.Append("\r\n");
            log.Append("A. RAW NMEA DATA FOR ");
            log.Append(Name.ToUpper());
            log.Append("\r\n\r\n");

            // Wrap the device's raw data stream in an NmeaReader
            NmeaReader stream = new(BaseStream);

            try
            {
                // Now analyze it for up to 100 sentences
                for (int index = 0; index < 100; index++)
                {
                    // Read a valid sentence
                    NmeaSentence sentence = stream.ReadTypedSentence();

                    // Write it to the log file
                    log.Append(sentence.Sentence);
                    log.Append("\r\n");

                    // Get the command word for the sentence
                    if (!sentences.Contains(sentence.CommandWord))
                    {
                        sentences.Add(sentence.CommandWord);
                    }

                    // What features are supported?
                    if (sentence is IPositionSentence positionSentence)
                    {
                        isPositionSupported = true;
                    }

                    if (sentence is IAltitudeSentence altitudeSentence)
                    {
                        isAltitudeSupported = true;
                    }

                    if (sentence is IBearingSentence bearingSentence)
                    {
                        isBearingSupported = true;
                    }

                    if (sentence is IHorizontalDilutionOfPrecisionSentence hdopSentence)
                    {
                        isPrecisionSupported = true;
                    }

                    if (sentence is ISpeedSentence speedSentence)
                    {
                        isSpeedSupported = true;
                    }

                    if (sentence is ISatelliteCollectionSentence satellitesSentence)
                    {
                        isSatellitesSupported = true;
                    }

                    // Is everything supported?  If so, we have a healthy GPS device.  It's okay to exit
                    if (isPositionSupported && isAltitudeSupported && isBearingSupported && isPrecisionSupported && isSatellitesSupported && isSpeedSupported)
                    {
                        isWorkingProperly = true;
                        break;
                    }
                }

                // Summarize the log
                log.Append("\r\n");
                log.Append("B. SUMMARY\r\n");
                log.Append("\r\n");

                // Set images based on supported features
                log.Append(isPositionSupported
                               ? "           Latitude and longitude are supported.\r\n"
                               : "WARNING    Latitude and longitude are not supported.\r\n");

                log.Append(isAltitudeSupported
                               ? "           Altitude is supported.\r\n"
                               : "WARNING    Altitude is not supported.\r\n");

                log.Append(isBearingSupported
                               ? "           Bearing is supported.\r\n"
                               : "WARNING    Bearing is not supported.\r\n");

                log.Append(isPrecisionSupported
                               ? "           Dilution of Precision is supported.\r\n"
                               : "WARNING    Dilution of Precision is not supported.\r\n");

                log.Append(isSpeedSupported
                               ? "           Speed is supported.\r\n"
                               : "WARNING    Speed is not supported.\r\n");

                log.Append(isSatellitesSupported
                               ? "           GPS satellite data is supported.\r\n"
                               : "WARNING    GPS satellite data is not supported.\r\n");

                // Finish the log
                log.Append("------------------------------------------------------------------------------------------------------------------------------\r\n");

                // Generate and return a result
                return new DeviceAnalysis(this, isWorkingProperly, log.ToString(), sentences.ToArray(),
                    isPositionSupported, isAltitudeSupported, isBearingSupported, isPrecisionSupported, isSpeedSupported, isSatellitesSupported);
            }
            catch (Exception ex)
            {
                log.Append("ERROR   An exception occurred during analysis:\r\n");
                log.Append("\r\n");
                log.Append(ex.ToString());

                // Finish the log
                log.Append("------------------------------------------------------------------------------------------------------------------------------\r\n");

                // Generate and return a result
                return new DeviceAnalysis(this, isWorkingProperly, log.ToString(), sentences.ToArray(),
                    isPositionSupported, isAltitudeSupported, isBearingSupported, isPrecisionSupported, isSpeedSupported, isSatellitesSupported);
            }
            finally
            {
                // Close the device
                Close();
            }
        }

        /// <summary>
        /// Starts a new thread which checks the device for GPS data in the background.
        /// </summary>
        [SecurityCritical]
        public void BeginDetection()
        {
            // If we're already detecting, exit
            if (IsDetectionInProgress)
            {
                return;
            }

            // In some rare cases I get a NullReferenceException below.  This happens if the object has been finalized.
            if (DetectionWaitHandle == null)
            {
                return;
            }

            // Indicate we're in progress
            DetectionWaitHandle.Reset();

            // Rev up a new thread
            _detectionThread = new Thread(DetectionThreadProc)
            {
                Name =
                                           "GPS.NET Protocol Detector on " + Name + " (http://dotspatial.codeplex.com)",
                IsBackground = true
            };
            _detectionThread.Start();

            // Wait for the thread to fire up
            _detectionStartedWaitHandle.WaitOne();
        }

        /// <summary>
        /// Closes any open connection to the device.
        /// </summary>
        /// <remarks>This method will close any open connection to the device.  Any associated <strong>Stream</strong> object
        /// will be disposed.  If no open connection exists, this method has no effect.  This method is called automatically
        /// when the device is disposed, either manually or automatically via the finalizer.</remarks>
        public void Close()
        {
            lock (_syncRoot)
            {
                if (!IsOpen)
                {
                    return;
                }

                // Signal that we're not open
                IsOpen = false;

                // Signal that we're disconnecting
                OnDisconnecting();

                try
                {
                    // Close the connection
                    BaseStream.Close();
                    BaseStream.Dispose();
                }
                catch (ObjectDisposedException) { }
                catch (NullReferenceException) { }
                finally
                {
                    // Null it out for the GC
                    BaseStream = null;
                }

                // Signal that we're disconnected
                OnDisconnected();
            }
        }

        /// <summary>
        /// Waits for the device to be checked for GPS data.
        /// </summary>
        /// <returns></returns>
        public bool WaitForDetection()
        {
            return WaitForDetection(Devices.DeviceDetectionTimeout);
        }

        /// <summary>
        /// Waits for the device to be checked for GPS data, up to the specified timeout period.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public bool WaitForDetection(TimeSpan timeout)
        {
            // Is detection in progress?
            if (!IsDetectionInProgress)
            {
                return false;
            }

            // Yes.  Wait up to the timeout to complete
            DetectionWaitHandle.WaitOne(timeout, false);
            return IsGpsDevice;
        }

        /// <summary>
        /// Stops any GPS protocol detection in progress.
        /// </summary>
        public virtual void CancelDetection()
        {
            if (IsDetectionInProgress)
            {
                Debug.WriteLine("Aborting detection thread for " + Name, Devices.DEBUG_CATEGORY);
                _detectionThread.Abort();
            }

            _detectionThread = null;

            try
            {
                _detectionStartedWaitHandle.Reset();
            }
            catch (ObjectDisposedException)
            { }

            try
            {
                DetectionWaitHandle.Reset();
            }
            catch (ObjectDisposedException)
            { }
        }

        /// <summary>
        /// Detections the thread proc.
        /// </summary>
        [SecurityCritical]
        private void DetectionThreadProc()
        {
            Debug.WriteLine("Device detection attempt started for " + Name, Devices.DEBUG_CATEGORY);

            // Signal that the thread has started
            _detectionStartedWaitHandle.Set();

            // Signal that detection has started
            IsDetectionCompleted = false;

            // It is critical that finalizers for this task complete.  This ensures connections get closed.
            try
            {
                // Is it already detected?
                if (!IsGpsDevice)
                {
                    // Signal that detection started
                    Devices.OnDeviceDetectionAttempted(this);

                    // Find out if this is a GPS device
                    IsGpsDevice = DetectProtocol();
                }
            }
            catch (DeviceDetectionException ex)
            {
                Debug.WriteLine("Device detection failed for " + Name, Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(ex);
            }
            catch (ThreadAbortException ex)
            {
                Debug.WriteLine("Device detection failed for " + Name, Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(new DeviceDetectionException(this, ex));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Device detection failed for " + Name, Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(new DeviceDetectionException(this, ex));
            }
            finally
            {
                // Is a connection open?
                if (IsOpen)
                {
                    if (
                        // Close the port if detection failed
                        !IsGpsDevice
                        // Close the port if no connection is needed
                        || !Devices.IsStreamNeeded)
                    {
                        // Close the port
                        Close();
                    }
                }

                // If it is, register it.
                if (IsGpsDevice)
                {
                    Debug.WriteLine(Name + " is a valid GPS device", Devices.DEBUG_CATEGORY);

                    // Raise an event for this
                    Devices.Add(this);

                    // Increase the success statistic
                    SuccessfulDetectionCount++;

                    // RESET the failure count.  We're only interested in CONSECUTIVE failures.
                    FailedDetectionCount = 0;

                    // Update the date last detected
                    DateDetected = DateTime.Now;
                }
                else
                {
                    Debug.WriteLine(Name + " is not a valid GPS device", Devices.DEBUG_CATEGORY);

                    // Increase the failure statistic
                    FailedDetectionCount++;
                }

                // Save information to the registry
                OnCacheWrite();

                // Signal proper completion
                IsDetectionCompleted = true;

                // Indicate completion
                if (DetectionWaitHandle != null)
                {
                    DetectionWaitHandle.Set();
                }
            }
        }

        #endregion Public Methods

        #region Internal Methods

        /// <summary>
        /// Sets the is GPS device.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        internal void SetIsGpsDevice(bool value)
        {
            if (IsGpsDevice == value)
            {
                return;
            }

            IsGpsDevice = value;

            if (IsGpsDevice)
            {
                Devices.Add(this);
            }
        }

        #endregion Internal Methods

        #region Static Properties

        /// <summary>
        /// Controls the amount of time to wait before aborting a read operation.
        /// </summary>
        /// <value>The default read timeout.</value>
        public static TimeSpan DefaultReadTimeout
        {
            get => _defaultReadTimeout;
            set
            {
                // The timeout must be greater than zero
                if (value.TotalSeconds <= 0)
                {
                    throw new ArgumentOutOfRangeException("DefaultReadTimeout", "The default read timeout for a device must be greater than zero.  A value of five to ten seconds is recommended.");
                }

                // Set the value
                _defaultReadTimeout = value;
            }
        }

        /// <summary>
        /// Controls the amount of time to wait before aborting a write operation.
        /// </summary>
        /// <value>The default write timeout.</value>
        public static TimeSpan DefaultWriteTimeout
        {
            get => _defaultWriteTimeout;
            set
            {
                // The timeout must be greater than zero
                if (value.TotalSeconds <= 0)
                {
                    throw new ArgumentOutOfRangeException("DefaultWriteTimeout", "The default Write timeout for a device must be greater than zero.  A value of five to ten seconds is recommended.");
                }

                // Set the value
                _defaultWriteTimeout = value;
            }
        }

        #endregion Static Properties

        #region Static Methods

        /// <summary>
        /// Estimates device precision based on the fix quality.
        /// </summary>
        /// <param name="quality">The current fix quality of a device or emulation</param>
        /// <returns>The estimated error of latitude/longitude coordinates attributed to the device.</returns>
        /// <remarks>If a the fix quality is unknown or NoFix, this method returns the value stored
        /// in the DilutionOfPrecision.CurrentAverageDevicePrecision property.</remarks>
        public static Distance PrecisionEstimate(FixQuality quality)
        {
            // We only need the expected device error
            return quality switch
            {
                FixQuality.NoFix => DilutionOfPrecision.CurrentAverageDevicePrecision,// In this case, there's no fix.  Return 6 meters
                FixQuality.DifferentialGpsFix => Distance.FromMeters(2.75),/* Differential GPS (meaning a geosynchronous satellite such as WAAS, EGNOS or MSAS)
                     * is between 0.5m and 5m, or 2.75m on average.
                     */
                FixQuality.FixedRealTimeKinematic => Distance.FromCentimeters(3),/* Real-Time Kinematic or RTK is a very high precision tech used for surveying.  It's
                     * not really meant for being in motion like in a car, but surveying equipment.
                     */
                FixQuality.FloatRealTimeKinematic => Distance.FromCentimeters(60),/* Floating Real-Time Kinematic (Float RTK) is also high precision, but not as good as
                     * fixed RTK (see above)
                     */
                _ => DilutionOfPrecision.CurrentAverageDevicePrecision,/* Consumer GPS devices are capable of about 5-7 meters of precision.  In cases not,
                     * handled above, there's no special fix in effect.
                     */
            };
        }

        /// <summary>
        /// Compares two devices for the purpose of finding the device most likely to provide a successful connection.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>An <strong>Integer</strong> value.</returns>
        /// <remarks>This method is used during <strong>Sort</strong> methods to choose the device most likely to respond.
        /// The date the device last successfully opened a connection is examined, along with historical connection statistics.
        /// The "best" device is the device which has most recently opened a connection successfully, with the fewest amount of
        /// connection failures.</remarks>
        public static int BestDeviceComparer(Device left, Device right)
        {
            return left.CompareTo(right);
        }

        #endregion Static Methods

        #region IDisposable Members

        /// <summary>
        /// Disposes of any managed or unmanaged resources used by the device.
        /// </summary>
        /// <remarks>Since the <strong>Device</strong> class implements the <strong>IDisposable</strong> interface, all managed or
        /// unmanaged resources used by the class will be disposed.  Any open connection will be closed.  The <strong>Dispose</strong>
        /// method is also called (if necessary) during the finalizer for this class.</remarks>
        public void Dispose()
        {
            Dispose(true);

            // We no longer need to finalize
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of any unmanaged (or optionally, managed) resources used by the device.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <remarks>This method is called when the object is no longer needed.  Typically, this happens during the shutdown of the parent
        /// application.  If device detection is in progress, it will be immediately cancelled.  If any connection is open, it will immediately
        /// be closed and disposed.  This method is called via the finalizer if necessary, thus removing the need for explicit calls.  Developers
        /// who prefer to call this method explicitly, however, may do so.</remarks>
        protected virtual void Dispose(bool disposing)
        {
            // Shut down any detection
            CancelDetection();

            // Close any open connection
            Close();

            // Shut down the wait handle
            if (DetectionWaitHandle != null)
            {
                DetectionWaitHandle.Close();
            }

            if (_detectionStartedWaitHandle != null)
            {
                _detectionStartedWaitHandle.Close();
            }

            // If we're disposing managed objects, release them
            if (disposing)
            {
                BaseStream = null;
                _detectionThread = null;
                DetectionWaitHandle = null;
                _detectionStartedWaitHandle = null;
            }
        }

        #endregion IDisposable Members

        #region IFormattable Members

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            return Name;
        }

        #endregion IFormattable Members

        #region IComparable<Device> Members

        /// <summary>
        /// Compares two devices for the purpose of finding the device most likely to provide a successful connection.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>An <strong>Integer</strong> value.</returns>
        /// <remarks>This method is used during <strong>Sort</strong> methods to choose the device most likely to respond.
        /// The date the device last successfully opened a connection is examined, along with historical connection statistics.
        /// The "best" device is the device which has most recently opened a connection successfully, with the fewest amount of
        /// connection failures.</remarks>
        public virtual int CompareTo(Device other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            // Which has a better rate of success?
            int result = other.Reliability.Value.CompareTo(Reliability.Value);
            if (result == 0)
            {
                // Both of them.  Which one connects faster?
                result = AverageConnectionTime.CompareTo(other.AverageConnectionTime);
            }

            return result;
        }

        #endregion IComparable<Device> Members
    }

    /// <summary>
    /// Represents information about a GPS device when a device-related event is raised.
    /// </summary>
    public sealed class DeviceEventArgs : EventArgs
    {

        /// <summary>
        /// A default instance
        /// </summary>
        public static new readonly DeviceEventArgs Empty = new(null);

        /// <summary>
        /// Creates a new instance of the event args
        /// </summary>
        /// <param name="device">The device.</param>
        public DeviceEventArgs(Device device)
        {
            Device = device;
        }

        /// <summary>
        /// The device
        /// </summary>
        public Device Device { get; }
    }

    /// <summary>
    /// Represents an examination of a GPS device for its capabilities and health.
    /// </summary>
    public class DeviceAnalysis
    {
        /// <summary>
        ///
        /// </summary>
        private readonly Device _device;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceAnalysis"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="isWorkingProperly">if set to <c>true</c> [is working properly].</param>
        /// <param name="log">The log.</param>
        /// <param name="supportedSentences">The supported sentences.</param>
        /// <param name="isPositionSupported">if set to <c>true</c> [is position supported].</param>
        /// <param name="isAltitudeSupported">if set to <c>true</c> [is altitude supported].</param>
        /// <param name="isBearingSupported">if set to <c>true</c> [is bearing supported].</param>
        /// <param name="isPrecisionSupported">if set to <c>true</c> [is precision supported].</param>
        /// <param name="isSpeedSupported">if set to <c>true</c> [is speed supported].</param>
        /// <param name="isSatellitesSupported">if set to <c>true</c> [is satellites supported].</param>
        internal DeviceAnalysis(Device device, bool isWorkingProperly, string log, string[] supportedSentences,
    bool isPositionSupported, bool isAltitudeSupported, bool isBearingSupported,
    bool isPrecisionSupported, bool isSpeedSupported, bool isSatellitesSupported)
        {
            _device = device;
            IsWorkingProperly = isWorkingProperly;
            Log = log;
            SupportedSentences = supportedSentences;
            IsPositionSupported = isPositionSupported;
            IsAltitudeSupported = isAltitudeSupported;
            IsBearingSupported = isBearingSupported;
            IsPrecisionSupported = isPrecisionSupported;
            IsSpeedSupported = isSpeedSupported;
            IsSatellitesSupported = isSatellitesSupported;
        }

        /// <summary>
        /// Returns a log of activity while the device was tested.
        /// </summary>
        public string Log { get; }

        /// <summary>
        /// Returns whether the device is healthy
        /// </summary>
        public bool IsWorkingProperly { get; }

        /// <summary>
        /// Returns NMEA sentences discovered during analysis.
        /// </summary>
        public string[] SupportedSentences { get; }

        /// <summary>
        /// Returns whether real-time latitude and longitude are reported by the device.
        /// </summary>
        public bool IsPositionSupported { get; }

        /// <summary>
        /// Returns whether the distance above sea level is reported by the device.
        /// </summary>
        public bool IsAltitudeSupported { get; }

        /// <summary>
        /// Returns whether the real-time direction of travel is reported by the device.
        /// </summary>
        public bool IsBearingSupported { get; }

        /// <summary>
        /// Returns whether dilution of precision information is reported by the device.
        /// </summary>
        public bool IsPrecisionSupported { get; }

        /// <summary>
        /// Returns whether the real-time rate of travel is reported by the device.
        /// </summary>
        public bool IsSpeedSupported { get; }

        /// <summary>
        /// Returns whether real-time satellite information is reported by the device.
        /// </summary>
        public bool IsSatellitesSupported { get; }

        /// <summary>
        /// Sends this analysis anonymously to DotSpatial.Positioning for further study.
        /// </summary>
        /// <returns>A <strong>Boolean</strong> value, <strong>True</strong> if the HTTP response indicates success.</returns>
        /// <remarks><para>DotSpatial.Positioning collects device detection information for the purposes of improving
        /// software, widening support for devices, and providing faster technical support.  This method
        /// will cause an HTTP POST to be sent to the DotSpatial.Positioning web server with the values in this class.
        /// The information is collected anonymously.</para>
        ///   <para>One benefit of sending an analysis is that it can instantly update statistics for supported
        /// GPS devices.  The anonymous results will be compiled into an RSS feed which developers can use to
        /// instantly be notified of both healthy and problematic devices.</para>
        ///   <para>The DotSpatial.Positioning server can handle repeated calls to this method, though requests should not
        /// be sent more than every few seconds.  Duplicate reports are automatically ignored by our system.  So
        /// long as customers are aware of the implications of an HTTP request (such as possible cell phone charges),
        /// you are welcome to use this functionality in your application.</para></remarks>
        public bool SendAnonymousLog()
        {
            return SendAnonymousLog(new Uri("http://dotspatial.codeplex.com/GPSDeviceDiagnostics.aspx"));
        }

        /// <summary>
        /// Sends this analysis anonymously to the specified Uri for further study.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>A <strong>Boolean</strong> value, <strong>True</strong> if the HTTP response indicates success.</returns>
        /// <remarks><para>Developers may wish to generate detection logs from customers for the purposes of
        /// improving technical support.  This can also be used to collect statistics about GPS devices which
        /// customers use.  This method will create an HTTP POST to the specified address with the information in this
        /// class.</para>
        ///   <para>The actual fields sent will vary depending on the type of object being reported.  Currently, this
        /// feature works for SerialDevice, BluetoothDevice, and GpsIntermediateDriver device objects.</para></remarks>
        public bool SendAnonymousLog(Uri uri)
        {
            // Make a new web request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            // Provide standard credentials for the web request
            request.ContentType = "application/x-www-form-urlencoded";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.UserAgent = "GPS Diagnostics";
            request.Method = "POST";

            string data = "Device=" + _device.Name;
            data += "&IsGps=" + _device.IsGpsDevice;
            data += "&Reliability=" + _device.Reliability.ToString().Replace("%", string.Empty);

            if (_device is SerialDevice serialDevice)
            {
                data += "&Type=Serial";
                data += "&Port=" + serialDevice.Port;
                data += "&BaudRate=" + serialDevice.BaudRate;
            }
            else if (_device is BluetoothDevice bluetoothDevice)
            {
                data += "&Type=Bluetooth";
                data += "&Address=" + bluetoothDevice.Address;

                BluetoothEndPoint endPoint = (BluetoothEndPoint)bluetoothDevice.EndPoint;
                data += "&Service=" + endPoint.Name;
            }

            // Now append the log
            data += "&Log=" + Log;

            // Get a byte array
            byte[] buffer = Encoding.UTF8.GetBytes(data);

            // Transmit the request
            request.ContentLength = buffer.Length;

            // Get the request stream.
            Stream dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(buffer, 0, buffer.Length);

            // Close the Stream object.
            dataStream.Close();

            // Get the response.
            HttpWebResponse response = null;
            try
            {
                response = request.GetResponse() as HttpWebResponse;

                // Display the status.
                if (response != null)
                {
                    return response.StatusDescription == "OK";
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return true;
        }
    }
}
