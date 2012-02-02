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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using Microsoft.Win32;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Encapsulates GPS device detection features and information about known devices.
    /// </summary>
    [SecuritySafeCritical]
    public static class Devices
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly List<ManualResetEvent> _currentlyDetectingWaitHandles = new List<ManualResetEvent>(16);
        /// <summary>
        ///
        /// </summary>
        private static List<SerialDevice> _serialDevices;
        /// <summary>
        ///
        /// </summary>
        private static List<BluetoothDevice> _bluetoothDevices;
        /// <summary>
        ///
        /// </summary>
        private static readonly List<Device> _gpsDevices;
        /// <summary>
        ///
        /// </summary>
        private static Thread _detectionThread;
        /// <summary>
        ///
        /// </summary>
        private static bool _isDetectionInProgress;
        /// <summary>
        ///
        /// </summary>
        private static bool _isClockSynchronizationEnabled;
        /// <summary>
        ///
        /// </summary>
        private static readonly ManualResetEvent _deviceDetectedWaitHandle = new ManualResetEvent(false);
        /// <summary>
        ///
        /// </summary>
        private static readonly ManualResetEvent _detectionCompleteWaitHandle = new ManualResetEvent(false);
        /// <summary>
        ///
        /// </summary>
        private static TimeSpan _deviceDetectionTimeout = TimeSpan.FromMinutes(20);
        /// <summary>
        ///
        /// </summary>
        private static bool _isStreamNeeded;
        /// <summary>
        ///
        /// </summary>
        private static bool _isOnlyFirstDeviceDetected;
        /// <summary>
        ///
        /// </summary>
        private static bool _allowBluetoothConnections = true;
        /// <summary>
        ///
        /// </summary>
        private static bool _allowSerialConnections = true;
        /// <summary>
        ///
        /// </summary>
        private static bool _allowExhaustiveSerialPortScanning;
        /// <summary>
        ///
        /// </summary>
        private static int _maximumSerialPortNumber = 20;

        /// <summary>
        ///
        /// </summary>
        private static Position _position;
        /// <summary>
        ///
        /// </summary>
        private static Distance _altitude;
        /// <summary>
        ///
        /// </summary>
        private static DateTime _utcDateTime;
        /// <summary>
        ///
        /// </summary>
        private static Azimuth _bearing;
        /// <summary>
        ///
        /// </summary>
        private static Speed _speed;
        /// <summary>
        ///
        /// </summary>
        private static List<Satellite> _satellites;

        #region Constants

        /// <summary>
        ///
        /// </summary>
        internal const string DEBUG_CATEGORY = "GPS.Net";
        /// <summary>
        ///
        /// </summary>
        internal const string ROOT_KEY_NAME = @"SOFTWARE\DotSpatial.Positioning\GPS.NET\3.0\Devices\";

        #endregion Constants

        #region Events

        /// <summary>
        /// Occurs when the process of finding GPS devices has begun.
        /// </summary>
        public static event EventHandler DeviceDetectionStarted;
        /// <summary>
        /// Occurs immediately before a device is about to be tested for GPS data.
        /// </summary>
        public static event EventHandler<DeviceEventArgs> DeviceDetectionAttempted;
        /// <summary>
        /// Occurs when a device has failed to transmit recognizable GPS data.
        /// </summary>
        public static event EventHandler<DeviceDetectionExceptionEventArgs> DeviceDetectionAttemptFailed;
        /// <summary>
        /// Occurs when a device is responding and transmitting GPS data.
        /// </summary>
        public static event EventHandler<DeviceEventArgs> DeviceDetected;
        /// <summary>
        /// Occurs when a Bluetooth device has been found.
        /// </summary>
        public static event EventHandler<DeviceEventArgs> DeviceDiscovered;
        /// <summary>
        /// Occurs when the process of finding GPS devices has been interrupted.
        /// </summary>
        public static event EventHandler DeviceDetectionCanceled;
        /// <summary>
        /// Occurs when the process of finding GPS devices has finished.
        /// </summary>
        public static event EventHandler DeviceDetectionCompleted;

        /// <summary>
        /// Occurs when any interpreter detects a change in the current location.
        /// </summary>
        public static event EventHandler<PositionEventArgs> PositionChanged;
        /// <summary>
        /// Occurs when any interpreter detects a change in the distance above sea level.
        /// </summary>
        public static event EventHandler<DistanceEventArgs> AltitudeChanged;
        /// <summary>
        /// Occurs when any interpreter detects a change in the current rate of travel.
        /// </summary>
        public static event EventHandler<SpeedEventArgs> SpeedChanged;
        /// <summary>
        /// Occurs when any interpreter detects a change in GPS satellite information.
        /// </summary>
        public static event EventHandler<SatelliteListEventArgs> SatellitesChanged;
        /// <summary>
        /// Occurs when any interpreter detects a change in the direction of travel.
        /// </summary>
        public static event EventHandler<AzimuthEventArgs> BearingChanged;
        /// <summary>
        /// Occurs when any interpreter detects when a GPS device can no longer calculate the current location.
        /// </summary>
        public static event EventHandler<DeviceEventArgs> FixLost;
        /// <summary>
        /// Occurs when any interpreter detects when a GPS device becomes able to calculate the current location.
        /// </summary>
        public static event EventHandler<DeviceEventArgs> FixAcquired;
        /// <summary>
        /// Occurs when any interpreter detects a change in the satellite-derived date and time.
        /// </summary>
        public static event EventHandler<DateTimeEventArgs> UtcDateTimeChanged;

        #endregion Events

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static Devices()
        {
            // Get notified when a BT device is discovered
            BluetoothDevice.DeviceDiscovered += BluetoothDevice_DeviceDiscovered;

            // Reset everything
            _gpsDevices = new List<Device>();
        }

        #endregion Constructors

        #region Static Properties

        /// <summary>
        /// Returns a GPS device which is connectable and is reporting data.
        /// </summary>
        public static Device Any
        {
            get
            {
                try
                {
                    // A stream is needed!
                    _isStreamNeeded = true;

#if PocketPC
                    /* On mobile devices, the GPS Intermediate Driver can handle responsibility
                     * of opening and sharing connections to a GPS device.  GPS.NET will defer to the
                     * GPSID so long as it is supported AND enabled for the device.
                     */

                    // Are GPSID connections allowed?
                    GpsIntermediateDriver gpsid = GpsIntermediateDriver.Current;

                    if (
                        // Is it supported?
                        gpsid != null
                        // Are we allowing connections?
                        && gpsid.AllowConnections
                        // Has the GPSID not yet been tested?
                        && (!gpsid.IsDetectionCompleted
                            // OR, has it been tested AND it's confirmed as a GPS device?
                            || (gpsid.IsDetectionCompleted && gpsid.IsGpsDevice)
                            )
                        )
                    {
                        try
                        {
                            // Open a connection
                            gpsid.Open();

                            // It worked!
                            return gpsid;
                        }
                        catch
                        {
                            // Feck.  Continue with regular detection
                        }
                    }
#endif

                    // Is any GPS device already detected?
                    if (!IsDeviceDetected)
                    {
                        // No.  Go look for one now.
                        BeginDetection();

                        // Wait for a device to be found.
                        if (!WaitForDevice())
                        {
                            // No device was found!  Return null.
                            return null;
                        }
                    }

                    /* If we get here, a device has been found.  If detection completed while
                     * this method executes, a device will currently have it's stream OPEN
                     * in anticipation of being used by this property.
                     *
                     * So, let's first look for that device with an open stream.  Then, if none
                     * exist, start testing devices until we get a valid stream.
                     */

                    Device device = null;
                    //Stream stream = null;
                    Exception connectionException = null;

                    #region Pass 1: Look for a device with an open connection

                    // Sort the devices, "best" device first
                    _gpsDevices.Sort(Device.BestDeviceComparer);

                    // Examine each device
                    foreach (Device t in _gpsDevices)
                    {
                        // Get the device and it's base stream
                        device = t;

                        // Skip devices which are not open
                        if (!device.IsOpen)
                            continue;

                        // Return the stream
                        return device;
                    }

                    #endregion Pass 1: Look for a device with an open connection

                    /* If we get here, there are no devices with an open connection.  So,
                     * try opening new connections.
                     */

                    #region Pass 2: Attempt new connections

                    // Test all known GPS devices
                    foreach (Device t in _gpsDevices)
                    {
                        try
                        {
                            // Get the device
                            device = t;

                            // Is it allowed?
                            if (!device.AllowConnections)
                                continue;

                            // Open a new connection
                            device.Open();

                            // This stream looks valid
                            return device;
                        }
                        catch (Exception ex)
                        {
                            // Make sure the device is closed
                            if (device != null) device.Close();

                            // We may get all kinds of exceptions when trying to open varying kinds of streams.
                            // If anything fails, just try the next device.
                            connectionException = ex;
                            continue;
                        }
                    }

                    #endregion Pass 2: Attempt new connections

                    #region Pass #3: Any detected devices have failed.  Restart detection.

                    // No.  Go look for one now.
                    BeginDetection();

                    // Wait for a device to be found.
                    WaitForDetection();

                    // Try one last time for devices
                    foreach (Device t in _gpsDevices)
                    {
                        try
                        {
                            // Get the device
                            device = t;

                            // Is it allowed?
                            if (!device.AllowConnections)
                                continue;

                            // Open a new connection
                            device.Open();

                            // This stream looks valid
                            return device;
                        }
                        catch (Exception ex)
                        {
                            // Make sure the device is closed
                            if (device != null) device.Close();

                            // We may get all kinds of exceptions when trying to open varying kinds of streams.
                            // If anything fails, just try the next device.
                            connectionException = ex;
                            continue;
                        }
                    }

                    #endregion Pass #3: Any detected devices have failed.  Restart detection.

                    // If we get here, no connection is possible!
                    if (connectionException != null)
                    {
                        // Some exception occurred, so re-throw it to help people troubleshoot their connections.
                        throw connectionException;
                    }
                    // No device was found, and no exception was raised.  Return null.
                    return null;
                }
                finally
                {
                    // Flag that we no longer need a stream.
                    _isStreamNeeded = false;
                }
            }
        }

        /// <summary>
        /// Controls whether Bluetooth devices are included in the search for GPS devices.
        /// </summary>
        /// <value><c>true</c> if [allow bluetooth connections]; otherwise, <c>false</c>.</value>
        public static bool AllowBluetoothConnections
        {
            get { return _allowBluetoothConnections; }
            set { _allowBluetoothConnections = value; }
        }

        /// <summary>
        /// Controls whether serial devices are included in the search for GPS devices.
        /// </summary>
        /// <value><c>true</c> if [allow serial connections]; otherwise, <c>false</c>.</value>
        public static bool AllowSerialConnections
        {
            get { return _allowSerialConnections; }
            set { _allowSerialConnections = value; }
        }

        /// <summary>
        /// Controls whether a complete range of serial devices is searched, regardless of which device appear to actually exist.
        /// </summary>
        /// <value><c>true</c> if [allow exhaustive serial port scanning]; otherwise, <c>false</c>.</value>
        public static bool AllowExhaustiveSerialPortScanning
        {
            get { return _allowExhaustiveSerialPortScanning; }
            set { _allowExhaustiveSerialPortScanning = value; }
        }

        /// <summary>
        /// Controls the maximum serial port to test when exhaustive detection is enabled.
        /// </summary>
        /// <value>The maximum serial port number.</value>
        public static int MaximumSerialPortNumber
        {
            get { return _maximumSerialPortNumber; }
            set
            {
                if (_maximumSerialPortNumber < 0 || _maximumSerialPortNumber > 100)
                {
                    throw new ArgumentOutOfRangeException("value", _maximumSerialPortNumber, "The maximum serial port number must be between 0 (for COM0:) and 100 (for COM100:).");
                }

                _maximumSerialPortNumber = value;
            }
        }

        /// <summary>
        /// Returns a list of confirmed GPS devices.
        /// </summary>
        public static IList<Device> GpsDevices
        {
            get
            {
                return _gpsDevices;
            }
        }

        /// <summary>
        /// Returns a list of known wireless Bluetooth devices (not necessarily GPS devices).
        /// </summary>
        public static IList<BluetoothDevice> BluetoothDevices
        {
            get { return _bluetoothDevices ?? (_bluetoothDevices = new List<BluetoothDevice>(BluetoothDevice.Cache)); }
        }

        /// <summary>
        /// Returns a list of known serial devices (not necessarily GPS devices).
        /// </summary>
        public static IList<SerialDevice> SerialDevices
        {
            get { return _serialDevices ?? (_serialDevices = new List<SerialDevice>(SerialDevice.GetCache())); }
        }

        /// <summary>
        /// Controls the amount of time allowed for device detection to complete before it is aborted.
        /// </summary>
        /// <value>The device detection timeout.</value>
        public static TimeSpan DeviceDetectionTimeout
        {
            get
            {
                return _deviceDetectionTimeout;
            }
            set
            {
                // Valid8
                if (value.TotalMilliseconds <= 0)
                {
                    throw new ArgumentOutOfRangeException("DeviceDetectionTimeout", value, "The total timeout for device detection must be a value greater than zero.  Typically, about ten seconds are required to complete detection.");
                }

                // Set the new value
                _deviceDetectionTimeout = value;
            }
        }

#if PocketPC

        public static GpsIntermediateDriver GpsIntermediateDriver
        {
            get { return GpsIntermediateDriver.Current; }
        }

        public static bool AllowGpsIntermediateDriver
        {
            get { return _AllowGpsIntermediateDriver; }
            set { _AllowGpsIntermediateDriver = value; }
        }
#endif

        /// <summary>
        /// Controls whether detection is aborted once one device has been found.
        /// </summary>
        /// <value><c>true</c> if this instance is only first device detected; otherwise, <c>false</c>.</value>
        public static bool IsOnlyFirstDeviceDetected
        {
            get
            {
                return _isOnlyFirstDeviceDetected;
            }
            set
            {
                _isOnlyFirstDeviceDetected = value;
            }
        }

        /// <summary>
        /// Controls whether the system clock should be synchronized to GPS-derived date and time.
        /// </summary>
        /// <value><c>true</c> if this instance is clock synchronization enabled; otherwise, <c>false</c>.</value>
        public static bool IsClockSynchronizationEnabled
        {
            get { return _isClockSynchronizationEnabled; }
            set { _isClockSynchronizationEnabled = value; }
        }

        /// <summary>
        /// Controls whether the Bluetooth receiver is on and accepting connections.
        /// </summary>
        /// <value><c>true</c> if this instance is bluetooth enabled; otherwise, <c>false</c>.</value>
        public static bool IsBluetoothEnabled
        {
            get
            {
                /* We can get the state of the radio if it's a Microsoft stack.
                 * Thankfully, Microsoft BT stacks are part of Wista, Wnidows 7, and
                 * Windows Mobile 5+, making it very common.  However, it's still in 2nd
                 * place behind Broadcom (Widcomm).  Though, I doubt this will last long.
                 * So, screw Broadcom.
                 */

#if PocketPC
                DotSpatial.Positioning.Gps.IO.NativeMethods.BluetoothRadioMode mode =
                    DotSpatial.Positioning.Gps.IO.NativeMethods.BluetoothRadioMode.PowerOff;
                int errorCode = DotSpatial.Positioning.Gps.IO.NativeMethods.BthGetMode(out mode);

                if (errorCode != 0)
                {
                    /* I get error "1359" (Internal error) on my HP iPaq 2945, which does NOT have a Microsoft Bluetooth stack.
                     * I'm guessing that this API just isn't supported on the Widcomm stack.  Rather
                     * than throw a fit, just gracefully indicate an Off radio.  This will prevent
                     * BT functions in GPS.NET.
                     */
                    return false;
                }

                /* Connectable and Discoverable both mean "ON".  The only difference is that a
                 * "discoverable" Bluetooth radio can be seen by other devices.
                 */
                return mode != DotSpatial.Positioning.Gps.IO.NativeMethods.BluetoothRadioMode.PowerOff;
#else
                return BluetoothRadio.Current != null
                    && BluetoothRadio.Current.GetIsConnectable();
#endif
            }
            set
            {
                _isDetectionInProgress = value;
#if PocketPC
                // Convert the boolean to a numeric mode
                DotSpatial.Positioning.Gps.IO.NativeMethods.BluetoothRadioMode mode =
                    value ? DotSpatial.Positioning.Gps.IO.NativeMethods.BluetoothRadioMode.Connectable
                          : DotSpatial.Positioning.Gps.IO.NativeMethods.BluetoothRadioMode.PowerOff;

                // Set the new mode
                int result = DotSpatial.Positioning.Gps.IO.NativeMethods.BthSetMode(mode);
                if (result != 0)
                {
                    //    throw new Win32Exception(result);
                }
#else
                // TODO: Add support for turning the Bluetooth radio on or off on the desktop.
#endif
            }
        }

        /// <summary>
        /// Returns whether the Bluetooth stack on the local machine is supported by GPS.NET.
        /// </summary>
        public static bool IsBluetoothSupported
        {
            get
            {
                /* The Microsoft Bluetooth stack provides an API used to enumerate all of the
                 * "radios" on the local machine.  A "radio" is just a Bluetooth transmitter.
                 * A vast majority of people will only have one radio; multiple radios would happen
                 * if, say, somebody had two USB Bluetooth dongles plugged in.
                 *
                 * We can confirm that Bluetooth is supported by looking for a local radio.
                 * This method will return immediately with a non-zero handle if one exists.
                 */
                return BluetoothRadio.Current != null;
            }
        }

        /// <summary>
        /// Returns whether a GPS device has been found.
        /// </summary>
        public static bool IsDeviceDetected
        {
            get
            {
                return _gpsDevices.Count != 0;
            }
        }

        /// <summary>
        /// Returns whether the process of finding a GPS device is still working.
        /// </summary>
        public static bool IsDetectionInProgress
        {
            get
            {
#if !PocketPC
                return _detectionThread != null && _detectionThread.IsAlive;
#else
                return _DetectionThread != null && _IsDetectionThreadAlive;
#endif
            }
        }

        /// <summary>
        /// Controls the current location on Earth's surface.
        /// </summary>
        /// <value>The position.</value>
        public static Position Position
        {
            get { return _position; }
            set
            {
                // Has anything actually changed?
                if (_position.Equals(value))
                    return;

                // Yes.
                _position = value;

                // Raise an event
                if (PositionChanged != null)
                    PositionChanged(null, new PositionEventArgs(_position));
            }
        }

        /// <summary>
        /// Controls the current rate of travel.
        /// </summary>
        /// <value>The speed.</value>
        public static Speed Speed
        {
            get { return _speed; }
            set
            {
                // Has anything actually changed?
                if (_speed.Equals(value))
                    return;

                // Yes.
                _speed = value;

                // Raise an event
                if (SpeedChanged != null)
                    SpeedChanged(null, new SpeedEventArgs(_speed));
            }
        }

        /// <summary>
        /// Controls the current list of GPS satellites.
        /// </summary>
        /// <value>The satellites.</value>
        public static List<Satellite> Satellites
        {
            get { return _satellites; }
            set
            {
                // Look for changes.  A quick check is for a varying number of
                // items in the list.
                bool isChanged = _satellites.Count != value.Count;

                // Has anything changed?
                if (!isChanged)
                {
                    // No.  Yet, the lists match counts.  Compare them
                    for (int index = 0; index < _satellites.Count; index++)
                    {
                        if (!_satellites[index].Equals(value[index]))
                        {
                            // The object has changed
                            isChanged = true;
                            break;
                        }
                    }
                }

                if (!isChanged)
                    return;

                // Set the new value
                _satellites = value;

                // Raise an event
                if (SatellitesChanged != null)
                    SatellitesChanged(null, new SatelliteListEventArgs(_satellites));
            }
        }

        /// <summary>
        /// Controls the current satellite-derived date and time.
        /// </summary>
        /// <value>The UTC date time.</value>
        public static DateTime UtcDateTime
        {
            get { return _utcDateTime; }
            set
            {
                // Has anything actually changed?
                if (_utcDateTime.Equals(value))
                    return;

                // Yes.
                _utcDateTime = value;

                // Raise an event
                if (UtcDateTimeChanged != null)
                    UtcDateTimeChanged(null, new DateTimeEventArgs(_utcDateTime));
            }
        }

        /// <summary>
        /// Controls the current satellite-derived date and time.
        /// </summary>
        /// <value>The date time.</value>
        public static DateTime DateTime
        {
            get
            {
                return _utcDateTime.ToLocalTime();
            }
            set
            {
                UtcDateTime = value.ToUniversalTime();
            }
        }

        /// <summary>
        /// Controls the current distance above sea level.
        /// </summary>
        /// <value>The altitude.</value>
        public static Distance Altitude
        {
            get { return _altitude; }
            set
            {
                // Has anything actually changed?
                if (_altitude.Equals(value))
                    return;

                // Yes.
                _altitude = value;

                // Raise an event
                if (AltitudeChanged != null)
                    AltitudeChanged(null, new DistanceEventArgs(_altitude));
            }
        }

        /// <summary>
        /// Controls the current direction of travel.
        /// </summary>
        /// <value>The bearing.</value>
        public static Azimuth Bearing
        {
            get { return _bearing; }
            set
            {
                // Has anything actually changed?
                if (_bearing.Equals(value))
                    return;

                // Yes.
                _bearing = value;

                // Raise an event
                if (BearingChanged != null)
                    BearingChanged(null, new AzimuthEventArgs(_bearing));
            }
        }

        #endregion Static Properties

        #region Static Methods

        /// <summary>
        /// Aborts the process of finding GPS devices and blocks until the cancellation is complete.
        /// </summary>
        public static void CancelDetection()
        {
            CancelDetection(false);
        }

        /// <summary>
        /// Aborts the process of finding GPS devices and optionally blocks until the cancellation is complete.
        /// </summary>
        /// <param name="async">If set to <see langword="true"/>, then the method will return immediately rather than waiting
        /// for the cancellation to complete.</param>
        public static void CancelDetection(bool async)
        {
            // If the detection thread is alive, abort it
            if (IsDetectionInProgress)
            {
                // Abort the thread
                Debug.WriteLine("Canceling device detection", DEBUG_CATEGORY);
                _detectionThread.Abort(async);

                if (!async)
                {
                    // Wait for the abort to wrap up
                    _detectionCompleteWaitHandle.WaitOne();
                }

                // Detection is complete
                Debug.WriteLine("Device detection has been canceled successfully", DEBUG_CATEGORY);
                OnDeviceDetectionCompleted();
            }
        }

        /// <summary>
        /// Starts looking for GPS devices on a separate thread.
        /// </summary>
        public static void BeginDetection()
        {
            // Start detection on another thread.
            if (_isDetectionInProgress)
                return;

            // Signal that detection is in progress
            _isDetectionInProgress = true;

            // Start a thread for managing detection
            _detectionThread = new Thread(DetectionThreadProc)
                                   {
                                       Name = "GPS.NET Device Detector (http://dotspatial.codeplex.com)",
                                       IsBackground = true,
                                       Priority = ThreadPriority.Lowest
                                   };

#if !PocketPC
            // Do detection in the background
#endif

            _detectionThread.Start();

#if PocketPC
            // Signal that the thread is alive (no Thread.IsAlive on the CF :P)
            _IsDetectionThreadAlive = true;
#endif
        }

        /// <summary>
        /// Cancels detection and removes any cached information about known devices.
        /// Use the <see cref="BeginDetection"/> method to re-detect devices and re-create the device cache.
        /// </summary>
        public static void Undetect()
        {
            // Undetect all devices (even non-GPS devices) and clear their cache
            foreach (BluetoothDevice device in _bluetoothDevices)
                device.Undetect();
            foreach (SerialDevice device in _serialDevices)
                device.Undetect();

#if PocketPC
            if (GpsIntermediateDriver.Current != null)
                GpsIntermediateDriver.Current.Undetect();
#endif

            try
            {
                // Clear any remaining entries in the device cache by deleting the root registry key
                Registry.LocalMachine.DeleteSubKeyTree(ROOT_KEY_NAME);
            }
            catch (UnauthorizedAccessException)
            { }

            ClearDeviceCache();
        }

        /// <summary>
        /// Waits for any GPS device to be detected.
        /// </summary>
        /// <returns></returns>
        public static bool WaitForDevice()
        {
            return WaitForDevice(DeviceDetectionTimeout);
        }

        /// <summary>
        /// Waits for any GPS device to be detected up to the specified timeout period.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static bool WaitForDevice(TimeSpan timeout)
        {
            // Is a device already detected?  If so, just exit
            if (IsDeviceDetected)
                return true;

            // Is detection in progress?  If so, wait until the timeout, or a device is found
            if (IsDetectionInProgress)
            {
#if !PocketPC
                // Wait for either a device to be detected, or for detection to complete
                ManualResetEvent[] waiters = new[] {
                    _detectionCompleteWaitHandle, _deviceDetectedWaitHandle };
                WaitHandle.WaitAny(waiters, timeout);
#else
                /* Mobile devices don't support "WaitAny" to wait on two wait handles.  In rare
                 * cases, detection may have nothing to do.  So, wait briefly for the entire thread
                 * to exit.
                 */

                // Wait briefly for the entire thread to exit
                if (_DetectionCompleteWaitHandle.WaitOne(2000, false))
                    return IsDeviceDetected;

                // Wait longer for a device to be found
                _DeviceDetectedWaitHandle.WaitOne((int)timeout.TotalMilliseconds, false);
#endif
            }

            // No GPS device is known, and detection is not in progress
            return IsDeviceDetected;
        }

        /// <summary>
        /// Waits for device detection to complete.
        /// </summary>
        /// <returns></returns>
        public static bool WaitForDetection()
        {
            return WaitForDetection(DeviceDetectionTimeout);
        }

        /// <summary>
        /// Waits for device detection to complete up to the specified timeout period.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        public static bool WaitForDetection(TimeSpan timeout)
        {
            if (!IsDetectionInProgress)
                return true;

#if PocketPC
            return _DetectionCompleteWaitHandle.WaitOne((int)timeout.TotalMilliseconds, false);
#else
            return _detectionCompleteWaitHandle.WaitOne(timeout, false);
#endif
        }

        /// <summary>
        /// Raises the <see cref="FixLost"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DotSpatial.Positioning.DeviceEventArgs"/> instance containing the event data.</param>
        internal static void RaiseFixLost(DeviceEventArgs e)
        {
            EventHandler<DeviceEventArgs> handler = FixLost;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="FixAcquired"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DotSpatial.Positioning.DeviceEventArgs"/> instance containing the event data.</param>
        internal static void RaiseFixAcquired(DeviceEventArgs e)
        {
            EventHandler<DeviceEventArgs> handler = FixAcquired;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        #endregion Static Methods

        #region Private Methods

        /// <summary>
        /// Handles the DeviceDiscovered event of the BluetoothDevice control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DotSpatial.Positioning.DeviceEventArgs"/> instance containing the event data.</param>
        private static void BluetoothDevice_DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            /* When this event occurs, a new Bluetooth device has been found.  Check
             * to see if the device is already in our list of known devices.
             */
            BluetoothDevice newDevice = (BluetoothDevice)e.Device;

            // Examine each known device
            if (_bluetoothDevices.Any(device => device.Address.Equals(newDevice.Address)))
            {
                newDevice.Dispose();
                return;
            }

            // If we get here, the device is brand new.  Add it to the list
            _bluetoothDevices.Add(newDevice);

            // Notify of the discovery
            OnDeviceDiscovered(e.Device);

            // And start detection
            newDevice.BeginDetection();
        }

        /// <summary>
        /// Detections the thread proc.
        /// </summary>
        private static void DetectionThreadProc()
        {
            try
            {
                // Signal that it started
                Debug.WriteLine("Starting GPS device detection", DEBUG_CATEGORY);
                OnDeviceDetectionStarted();

                // Monitor this thread up to the timeout, then quit
                ThreadPool.QueueUserWorkItem(DetectionThreadProcWatcher);

                // Clear the device cache to force a re-scan for any new devices
                ClearDeviceCache();

#if PocketPC
                // Are we using the GPS Intermediate Driver?
                GpsIntermediateDriver gpsid = GpsIntermediateDriver.Current;

                // Is the GPSID supported?
                if (gpsid != null)
                {
                    // Yes.  Test it to be sure
                    gpsid.BeginDetection();

                    // Wait for one device to get detected.  Was it confirmed?
                    if (gpsid.WaitForDetection())
                    {
                        // Yes.  If we only need one device, exit
                        if (_IsOnlyFirstDeviceDetected)
                            return;
                    }
                }

                /* If we get here, the GPS Intermediate Driver is not responding! */

#endif
                // Inspect all Bluetooth and serial devices to see if any of them are GPSes
                BeginBluetoothDetection();
                BeginSerialDetection(false);

                // Discover any new Bluetooth devices
                DiscoverBluetoothDevices();

                // Wait for all devices to finish detection
                WaitForDetectionInternal();

#if PocketPC

                #region Reconfigure the GPS Intermediate Driver (if necessary)

                /* The GPS Intermediate Driver may not have the right "Program Port" (actual GPS port/baud rate)
                 * settings.  Now that detection has completed, let's see if the GPSID needs configuration.
                 * If it is flagged as NOT being a GPS device, then it could not connect.  In this case, let's
                 * find the most reliable serial device and use it.
                 */
                if (
                    // Is the GPSID supported?
                    gpsid != null
                    // Are we allowed to configure it?
                    && gpsid.IsAutomaticallyConfigured
                    // Is it currently NOT identified as a GPS device?  (connections failed)
                    && !gpsid.IsGpsDevice)
                {
                    // Look through each confirmed GPS device
                    int count = _GpsDevices.Count;
                    for (int index = 0; index < count; index++)
                    {
                        // Is it a serial device?
                        SerialDevice device = _GpsDevices[index] as SerialDevice;
                        if (device == null)
                            continue;

                        // Yes.  Use it!
                        try
                        {
                            gpsid.HardwarePort = device;

                            // The GPSID is now working
                            Add(gpsid);
                        }
                        catch (Exception ex)
                        {
                            // Notify of the error gracefully
                            OnDeviceDetectionAttemptFailed(new DeviceDetectionException(gpsid, ex));
                        }

                        // That's the best device, so quit
                        break;
                    }
                }

                #endregion Reconfigure the GPS Intermediate Driver (if necessary)

#endif
                if (!IsDeviceDetected && AllowSerialConnections)
                {
                    // If we get here, and there are still no GPSes detected, then try detecting serial devices again,
                    // but this time omitting the colon suffix. On some systems, the colon is not included in the port name.
                    Debug.WriteLine("No serial devices were detected in the first pass. Starting second pass...", DEBUG_CATEGORY);
                    BeginSerialDetection(true);
                    WaitForDetectionInternal();
                }

                // Signal completion
                Debug.WriteLine("Device detection has completed", DEBUG_CATEGORY);
                OnDeviceDetectionCompleted();
            }
            catch (ThreadAbortException ex)
            {
                #region Abort detection for all devices

                Debug.WriteLine("Aborting all device detection threads", DEBUG_CATEGORY);

#if PocketPC
                // Stop detection for the GPSID
                if (GpsIntermediateDriver.Current != null)
                    GpsIntermediateDriver.Current.CancelDetection();
#endif

                // Stop detection for each Bluetooth device
                foreach (BluetoothDevice t in _bluetoothDevices)
                    t.CancelDetection();

                // Stop detection for each serial device
                foreach (SerialDevice t in _serialDevices)
                    t.CancelDetection();

                #endregion Abort detection for all devices

                // Determine whether we should block until all detection threads have canceled
                bool async = false;
                if (ex.ExceptionState != null && ex.ExceptionState is bool)
                {
                    async = (bool)ex.ExceptionState;
                }

                if (!async)
                {
                    // Wait for all the threads to die.  Just... sit and watch.  And wait.
                    Debug.WriteLine("Waiting for device detection threads to abort", DEBUG_CATEGORY);
                    while (_currentlyDetectingWaitHandles.Count != 0)
                    {
                        try { _currentlyDetectingWaitHandles[0].WaitOne(); }
                        finally { _currentlyDetectingWaitHandles.RemoveAt(0); }
                    }
                }

                // Signal the cancellation
                Debug.WriteLine("All device detection threads have been aborted", DEBUG_CATEGORY);
                if (DeviceDetectionCanceled != null)
                    DeviceDetectionCanceled(null, EventArgs.Empty);
            }
            finally
            {
                // Detection is no longer in progress
                _detectionCompleteWaitHandle.Set();
                _currentlyDetectingWaitHandles.Clear();    // <--  Already empty?
                _isDetectionInProgress = false;

#if PocketPC
                // Signal that the thread is alive (no Thread.IsAlive on the CF :P)
                _IsDetectionThreadAlive = false;
#endif
            }
        }

        /// <summary>
        /// This method, spawned by the ThreadPool, monitors detection and aborts it if it's taking too long.
        /// </summary>
        /// <param name="over9000">The over9000.</param>
        private static void DetectionThreadProcWatcher(object over9000)
        {
            if (_detectionCompleteWaitHandle.WaitOne((int)_deviceDetectionTimeout.TotalMilliseconds, false))
                return;

            // If we get here, then the timeout has expired. So cancel detection.
            CancelDetection();
        }

        /// <summary>
        /// Begins detecting all bluetooth devices to determine if they are GPS devices.
        /// All detection is done asynchronously, so this method returns immediately.
        /// Use the <see cref="WaitForDetection()"/> method if you need to block until detection is completed.
        /// </summary>
        private static void BeginBluetoothDetection()
        {
            // Is Bluetooth supported and turned on?
            if (IsBluetoothSupported && IsBluetoothEnabled)
            {
                Debug.WriteLine("Detecting Bluetooth devices", DEBUG_CATEGORY);

                // Start bluetooth detection for each device
                int count = BluetoothDevices.Count;
                for (int index = 0; index < count; index++)
                    _bluetoothDevices[index].BeginDetection();
            }
        }

        /// <summary>
        /// Begins detecting all serial devices to determine if they are GPS devices.
        /// All detection is done asynchronously, so this method returns immediately.
        /// Use the <see cref="WaitForDetection()"/> method if you need to block until detection is completed.
        /// </summary>
        /// <param name="omitColonSuffix">If set to <see langword="true"/>, the colon character (":") will be omitted from the port names.
        /// This is required on some systems in order for the device to be detected properly.</param>
        private static void BeginSerialDetection(bool omitColonSuffix)
        {
            if (AllowSerialConnections)
            {
                Debug.WriteLine("Detecting serial devices", DEBUG_CATEGORY);

                // Begin detection for each of the known serial devices
                int count = SerialDevices.Count;
                for (int index = 0; index < count; index++)
                {
                    SerialDevice device = _serialDevices[index];

                    if (omitColonSuffix && device.Port.EndsWith(":", StringComparison.Ordinal))
                    {
                        // Remove the colon suffix from the port name
                        string newName = device.Port.Substring(0, device.Port.Length - 1);
                        RenameDevice(device, newName);
                    }

                    device.BeginDetection();
                }

                /* If we're performing "exhaustive" detection, ports are scanned
                 * even if there's no evidence they actually exist.  This can happen in rare
                 * cases, such as when a PCMCIA GPS device is plugged in and fails to create
                 * a registry entry.
                 */
                if (_allowExhaustiveSerialPortScanning)
                {
                    Debug.WriteLine("Scanning all serial ports", DEBUG_CATEGORY);

                    // Try all ports from COM0: up to the maximum port number
                    for (int index = 0; index <= _maximumSerialPortNumber; index++)
                    {
                        // Is this port already being checked?
                        bool alreadyBeingScanned = false;
                        foreach (SerialDevice t in _serialDevices)
                        {
                            if (t.PortNumber.Equals(index))
                            {
                                // Yes.  Don't test it again
                                alreadyBeingScanned = true;
                                break;
                            }
                        }

                        // If it's already being scanned, skip to the next port
                        if (alreadyBeingScanned)
                            continue;

                        // Build the port name
                        string portName = "COM" + index;
                        if (!omitColonSuffix)
                            portName += ":";

                        // This is a new device.  Scan it
                        SerialDevice exhaustivePort = new SerialDevice(portName);
                        Debug.WriteLine("Checking " + portName + " for GPS device", DEBUG_CATEGORY);
                        exhaustivePort.BeginDetection();
                    }
                }
            }
        }

        /// <summary>
        /// Discovers any new bluetooth devices.
        /// </summary>
        private static void DiscoverBluetoothDevices()
        {
            // Is Bluetooth supported and turned on?
            if (IsBluetoothSupported && IsBluetoothEnabled)
            {
#if PocketPC
                /* Notice: For mobile devices, only one connection is allowed at a time.
                * As a result, we use a static SyncRoot to ensure that connections
                * and discovery happens in serial.  For this reason, we will not attempt
                * to discover devices until *after* trying to detect existing ones.
                */

                // Wait for existing devices to be tested
                int count = _BluetoothDevices.Count;
                for (int index = 0; index < count; index++)
                {
                    // Complete detection for this device
                    _BluetoothDevices[index].WaitForDetection();
                }
#endif

                // Begin searching for brand new devices
                Debug.WriteLine("Discovering new Bluetooth devices", DEBUG_CATEGORY);
                BluetoothDevice.DiscoverDevices(true);

                // Block until that search completes
                BluetoothDevice.DeviceDiscoveryThread.Join();
            }
        }

        /// <summary>
        /// Waits for device detection to complete.
        /// </summary>
        private static void WaitForDetectionInternal()
        {
            Debug.WriteLine("Waiting for device detection to finish", DEBUG_CATEGORY);

            /* A list holds the wait handles of devices being detected.  When it is empty,
             * detection has finished on all threads.
             */
            while (_currentlyDetectingWaitHandles.Count != 0)
            {
                try
                {
                    ManualResetEvent handle = _currentlyDetectingWaitHandles[0];
                    if (handle != null)
                    {
#if !PocketPC
                        if (!handle.SafeWaitHandle.IsClosed)
#endif
                            handle.WaitOne();
                    }
                }
                catch (ObjectDisposedException)
                {
                    /* In some rare cases a device will get disposed of and nulled out.
                     * So, regardless of what happens we can remove the item.
                     */
                }
                finally
                {
                    _currentlyDetectingWaitHandles.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// Renames the specified serial device, only if there is not already another serial device with the specified name.
        /// </summary>
        /// <param name="device">The device to be renamed.</param>
        /// <param name="newName">The new name for the device.</param>
        private static void RenameDevice(SerialDevice device, string newName)
        {
            // Make sure this port isn't already opened by another device
            foreach (SerialDevice t in _serialDevices)
            {
                if (t.Port.Equals(newName, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }
            device.Port = newName;
            device.SetName(newName);
        }

        /// <summary>
        /// Clears the <see cref="BluetoothDevices"/>, <see cref="SerialDevices"/>, and <see cref="GpsDevices"/> lists.
        /// This will cause the lists to be rebuilt the next time the <see cref="BeginBluetoothDetection"/> or
        /// <see cref="BeginSerialDetection"/> methods are called.
        /// </summary>
        private static void ClearDeviceCache()
        {
            // Clear the lists of Bluetooth/Serial devices.
            _bluetoothDevices = null;
            _serialDevices = null;

            // Clear the list of known GPS devices. This will be repopulated as new GPS devices are detected.
            _gpsDevices.Clear();
        }

        /// <summary>
        /// Called when [device detection attempted].
        /// </summary>
        /// <param name="device">The device.</param>
        internal static void OnDeviceDetectionAttempted(Device device)
        {
            // Add the wait handle to the list of handles to wait upon
            _currentlyDetectingWaitHandles.Add(device.DetectionWaitHandle);

            // Notify via an event
            if (DeviceDetectionAttempted != null)
                DeviceDetectionAttempted(device, new DeviceEventArgs(device));
        }

        /// <summary>
        /// Called when [device detection attempt failed].
        /// </summary>
        /// <param name="exception">The exception.</param>
        internal static void OnDeviceDetectionAttemptFailed(DeviceDetectionException exception)
        {
            if (DeviceDetectionAttemptFailed != null)
                DeviceDetectionAttemptFailed(exception.Device, new DeviceDetectionExceptionEventArgs(exception));
        }

        /// <summary>
        /// Called when [device detection started].
        /// </summary>
        internal static void OnDeviceDetectionStarted()
        {
            _detectionCompleteWaitHandle.Reset();

            // Signal that detection has started
            if (DeviceDetectionStarted != null)
                DeviceDetectionStarted(null, EventArgs.Empty);
        }

        /// <summary>
        /// Called when [device detection completed].
        /// </summary>
        internal static void OnDeviceDetectionCompleted()
        {
            // Signal that detection has started
            if (DeviceDetectionCompleted != null)
                DeviceDetectionCompleted(null, EventArgs.Empty);
        }

        /// <summary>
        /// Called when [device discovered].
        /// </summary>
        /// <param name="device">The device.</param>
        internal static void OnDeviceDiscovered(Device device)
        {
            if (DeviceDiscovered != null)
                DeviceDiscovered(null, new DeviceEventArgs(device));
        }

        /// <summary>
        /// Adds a GPS device to the list of known GPS devices.
        /// </summary>
        /// <param name="device">The device.</param>
        public static void Add(Device device)
        {
            // Is this device already detected?
            if (!_gpsDevices.Contains(device))
            {
                // Nope, add it
                _gpsDevices.Add(device);

                // Sort the list based on the most reliable device first
                _gpsDevices.Sort(Device.BestDeviceComparer);
            }

            // Signal that a device is found
            _deviceDetectedWaitHandle.Set();

            // Raise an event
            if (DeviceDetected != null)
                DeviceDetected(device, new DeviceEventArgs(device));

            // Are we only detecting the first device?  If so, abort now
            if (_isOnlyFirstDeviceDetected)
                CancelDetection(true);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is stream needed.
        /// </summary>
        internal static bool IsStreamNeeded
        {
            get
            {
                return _isStreamNeeded;
            }
        }

        #endregion Private Methods
    }

    /// <summary>
    /// Represents a problem which has occured during device detection.
    /// </summary>
    public class DeviceDetectionException : IOException
    {
        /// <summary>
        ///
        /// </summary>
        private readonly Device _device;

        /// <summary>
        /// Creates a new instance of a DeviceDetectionException
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="innerException">The inner exception.</param>
        public DeviceDetectionException(Device device, Exception innerException)
            : base(innerException.Message, innerException)
        {
            _device = device;
        }

        /// <summary>
        /// Creates a new instance of a DeviceDetectionException
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="message">The message.</param>
        public DeviceDetectionException(Device device, string message)
            : base(message)
        {
            _device = device;
        }

        /// <summary>
        /// Creates a new instance of a DeviceDetectionException
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DeviceDetectionException(Device device, string message, Exception innerException)
            : base(message, innerException)
        {
            _device = device;
        }

        /// <summary>
        /// The device that caused the exception
        /// </summary>
        public Device Device
        {
            get { return _device; }
        }
    }

    /// <summary>
    /// Represents information about a device detection problem during detection-related events.
    /// </summary>
    public class DeviceDetectionExceptionEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        private readonly DeviceDetectionException _exception;

        /// <summary>
        /// Creates a new instance of the DeviceDetectionException event arguments.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public DeviceDetectionExceptionEventArgs(DeviceDetectionException exception)
        {
            _exception = exception;
        }

        /// <summary>
        /// The device that is involved in the event
        /// </summary>
        public Device Device
        {
            get
            {
                return _exception.Device;
            }
        }

        /// <summary>
        /// The exception
        /// </summary>
        public DeviceDetectionException Exception
        {
            get
            {
                return _exception;
            }
        }
    }
}