using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using DotSpatial.Positioning.Gps.IO;
using Microsoft.Win32;

namespace DotSpatial.Positioning.Gps.IO
{
    /// <summary>
    /// Encapsulates GPS device detection features and information about known devices.
    /// </summary>
    public static class Devices
    {
        private static List<ManualResetEvent> _CurrentlyDetectingWaitHandles = new List<ManualResetEvent>(16);
        private static List<SerialDevice> _SerialDevices;
        private static List<BluetoothDevice> _BluetoothDevices;
        private static List<Device> _GpsDevices;
        private static Thread _DetectionThread;
        private static bool _IsDetectionInProgress;
        private static bool _IsClockSynchronizationEnabled;
        private static ManualResetEvent _DeviceDetectedWaitHandle = new ManualResetEvent(false);
        private static ManualResetEvent _DetectionCompleteWaitHandle = new ManualResetEvent(false);
        private static TimeSpan _DeviceDetectionTimeout = TimeSpan.FromMinutes(20);
        private static bool _IsStreamNeeded;
        private static bool _IsOnlyFirstDeviceDetected;
        private static bool _AllowBluetoothConnections = true;
        private static bool _AllowSerialConnections = true;
        private static bool _AllowExhaustiveSerialPortScanning = false;
        private static int _MaximumSerialPortNumber = 20;

        private static Position _Position;
        private static Distance _Altitude;
        private static DateTime _UtcDateTime;
        private static Azimuth _Bearing;
        private static Speed _Speed;
        private static List<Satellite> _Satellites;

#if PocketPC
        private static bool _AllowGpsIntermediateDriver = true;
        private static bool _AllowInfraredConnections = false;
        private static bool _IsDetectionThreadAlive;
#endif

        #region Constants

        internal const string DebugCategory = "GPS.Net";
        internal const string RootKeyName = @"SOFTWARE\DotSpatial.Positioning\GPS.NET\3.0\Devices\";

        #endregion

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

        #endregion

        #region Constructors

        static Devices()
        {           
            // Get notified when a BT device is discovered
            BluetoothDevice.DeviceDiscovered += new EventHandler<DeviceEventArgs>(BluetoothDevice_DeviceDiscovered);

            // Reset everything
            _GpsDevices = new List<Device>();
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns a GPS device which is connectable and is reporting data.
        /// </summary>
        /// <remarks></remarks>
        public static Device Any
        {
            get
            {
                try
                {
                    // A stream is needed!
                    _IsStreamNeeded = true;

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
                    _GpsDevices.Sort(Device.BestDeviceComparer);

                    // Examine each device
                    for (int index = 0; index < _GpsDevices.Count; index++)
                    {
                        // Get the device and it's base stream
                        device = _GpsDevices[index];

                        // Skip devices which are not open
                        if (!device.IsOpen)
                            continue;

                        // Return the stream
                        return device;
                    }

                    #endregion

                    /* If we get here, there are no devices with an open connection.  So,
                     * try opening new connections.
                     */

                    #region Pass 2: Attempt new connections

                    // Test all known GPS devices
                    for (int index = 0; index < _GpsDevices.Count; index++)
                    {
                        try
                        {
                            // Get the device
                            device = _GpsDevices[index];

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
                            device.Close();

                            // We may get all kinds of exceptions when trying to open varying kinds of streams.
                            // If anything fails, just try the next device.
                            connectionException = ex;
                            continue;
                        }
                    }

                    #endregion

                    #region Pass #3: Any detected devices have failed.  Restart detection.

                    // No.  Go look for one now.
                    BeginDetection();

                    // Wait for a device to be found.
                    WaitForDetection();

                    // Try one last time for devices
                    for (int index = 0; index < _GpsDevices.Count; index++)
                    {
                        try
                        {
                            // Get the device
                            device = _GpsDevices[index];

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
                            device.Close();

                            // We may get all kinds of exceptions when trying to open varying kinds of streams.
                            // If anything fails, just try the next device.
                            connectionException = ex;
                            continue;
                        }
                    }

                    #endregion

                    // If we get here, no connection is possible!
                    if (connectionException != null)
                    {
                        // Some exception occurred, so re-throw it to help people troubleshoot their connections.
                        throw connectionException;
                    }
                    else
                    {
                        // No device was found, and no exception was raised.  Return null.
                        return null;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    // Flag that we no longer need a stream.
                    _IsStreamNeeded = false;
                }
            }
        }

        /// <summary>
        /// Controls whether Bluetooth devices are included in the search for GPS devices.
        /// </summary>
        public static bool AllowBluetoothConnections
        {
            get { return _AllowBluetoothConnections; }
            set { _AllowBluetoothConnections = value; }
        }

        /// <summary>
        /// Controls whether serial devices are included in the search for GPS devices.
        /// </summary>
        public static bool AllowSerialConnections
        {
            get { return _AllowSerialConnections; }
            set { _AllowSerialConnections = value; }
        }

        /// <summary>
        /// Controls whether a complete range of serial devices is searched, regardless of which device appear to actually exist.
        /// </summary>
        public static bool AllowExhaustiveSerialPortScanning
        {
            get { return _AllowExhaustiveSerialPortScanning; }
            set { _AllowExhaustiveSerialPortScanning = value; }
        }

        /// <summary>
        /// Controls the maximum serial port to test when exhaustive detection is enabled.
        /// </summary>
        public static int MaximumSerialPortNumber
        {
            get { return _MaximumSerialPortNumber; }
            set
            {
                if (_MaximumSerialPortNumber < 0 || _MaximumSerialPortNumber > 100)
                {
#if !PocketPC
                    throw new ArgumentOutOfRangeException("MaximumSerialPortNumber", _MaximumSerialPortNumber, "The maximum serial port number must be between 0 (for COM0:) and 100 (for COM100:).");
#else
                    throw new ArgumentOutOfRangeException("MaximumSerialPortNumber", "The maximum serial port number must be between 0 (for COM0:) and 100 (for COM100:).");
#endif
                }

                _MaximumSerialPortNumber = value;
            }
        }

#if PocketPC
        public static bool AllowInfraredConnections
        {
            get { return _AllowInfraredConnections; }
            set { _AllowInfraredConnections = value; }
        }
#endif

        /// <summary>
        /// Returns a list of confirmed GPS devices.
        /// </summary>
        public static IList<Device> GpsDevices
        {
            get
            {
                return _GpsDevices;
            }
        }

        /// <summary>
        /// Returns a list of known wireless Bluetooth devices (not necessarily GPS devices).
        /// </summary>
        public static IList<BluetoothDevice> BluetoothDevices
        {
            get
            {
                if (_BluetoothDevices == null)
                    _BluetoothDevices = new List<BluetoothDevice>(BluetoothDevice.Cache);

                return _BluetoothDevices;
            }
        }

        /// <summary>
        /// Returns a list of known serial devices (not necessarily GPS devices).
        /// </summary>
        public static IList<SerialDevice> SerialDevices
        {
            get
            {
                if (_SerialDevices == null)
                    _SerialDevices = new List<SerialDevice>(SerialDevice.Cache);

                return _SerialDevices;
            }
        }

        /// <summary>
        /// Controls the amount of time allowed for device detection to complete before it is aborted.
        /// </summary>
        public static TimeSpan DeviceDetectionTimeout
        {
            get
            {
                return _DeviceDetectionTimeout;
            }
            set
            {
                // Valid8
                if (value.TotalMilliseconds <= 0)
                {
#if !PocketPC
                    throw new ArgumentOutOfRangeException("DeviceDetectionTimeout", value, "The total timeout for device detection must be a value greater than zero.  Typically, about ten seconds are required to complete detection.");
#else
                    throw new ArgumentOutOfRangeException("DeviceDetectionTimeout", "The total timeout for device detection must be a value greater than zero.  Typically, about ten seconds are required to complete detection.");
#endif
                }

                // Set the new value
                _DeviceDetectionTimeout = value;
            }
        }

#if PocketPC
        /// <summary>
        /// Returns the current GPS multiplexer if it is supported by the system.
        /// </summary>
        public static GpsIntermediateDriver GpsIntermediateDriver
        {
            get { return GpsIntermediateDriver.Current; }
        }

        /// <summary>
        /// Controls whether the GPS Intermediate Driver is used.
        /// </summary>
        public static bool AllowGpsIntermediateDriver
        {
            get { return _AllowGpsIntermediateDriver; }
            set { _AllowGpsIntermediateDriver = value; }
        }
#endif

        /// <summary>
        /// Controls whether detection is aborted once one device has been found.
        /// </summary>
        public static bool IsOnlyFirstDeviceDetected
        {
            get
            {
                return _IsOnlyFirstDeviceDetected;
            }
            set
            {
                _IsOnlyFirstDeviceDetected = value;
            }
        }

        /// <summary>
        /// Controls whether the system clock should be synchronized to GPS-derived date and time.
        /// </summary>
        public static bool IsClockSynchronizationEnabled
        {
            get { return _IsClockSynchronizationEnabled; }
            set { _IsClockSynchronizationEnabled = value; }
        }


        /// <summary>
        /// Controls whether the Bluetooth receiver is on and accepting connections.
        /// </summary>
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
                    && BluetoothRadio.Current.IsConnectable;
#endif
            }
            set
            {
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
#if PocketPC
                try
                {
                    // TODO: Is there any more specific way to detect MICROSOFT bluetooth without falsely detecting a stack which doesn't support sockets?

                    // Try and get the Bluetooth Radio status
                    DotSpatial.Positioning.Gps.IO.NativeMethods.BluetoothRadioMode mode =
                        DotSpatial.Positioning.Gps.IO.NativeMethods.BluetoothRadioMode.PowerOff;
                    int errorCode = DotSpatial.Positioning.Gps.IO.NativeMethods.BthGetMode(out mode);

                    // If there's no error, we can proceed.
                    return errorCode == 0;
                }
                catch
                {
                    // Nope, not supported
                    return false;
                }
#else
                /* The Microsoft Bluetooth stack provides an API used to enumerate all of the 
                 * "radios" on the local machine.  A "radio" is just a Bluetooth transmitter.
                 * A vast majority of people will only have one radio; multiple radios would happen
                 * if, say, somebody had two USB Bluetooth dongles plugged in.
                 * 
                 * We can confirm that Bluetooth is supported by looking for a local radio.
                 * This method will return immediately with a non-zero handle if one exists.
                 */
                return BluetoothRadio.Current != null;
#endif
            }
        }

        /// <summary>
        /// Returns whether a GPS device has been found.
        /// </summary>
        public static bool IsDeviceDetected
        {
            get
            {
                return _GpsDevices.Count != 0;
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
                return _DetectionThread != null && _DetectionThread.IsAlive;
#else
                return _DetectionThread != null && _IsDetectionThreadAlive;
#endif
            }
        }

        /// <summary>
        /// Controls the current location on Earth's surface.
        /// </summary>
        public static Position Position
        {
            get { return _Position; }
            set
            {
                // Has anything actually changed?
                if(_Position.Equals(value))
                    return;

                // Yes.
                _Position = value;

                // Raise an event
                if(PositionChanged != null)
                    PositionChanged(null, new PositionEventArgs(_Position));
            }
        }

        /// <summary>
        /// Controls the current rate of travel.
        /// </summary>
        public static Speed Speed
        {
            get { return _Speed; }
            set
            {
                // Has anything actually changed?
                if (_Speed.Equals(value))
                    return;

                // Yes.
                _Speed = value;

                // Raise an event
                if (SpeedChanged != null)
                    SpeedChanged(null, new SpeedEventArgs(_Speed));
            }
        }

        /// <summary>
        /// Controls the current list of GPS satellites.
        /// </summary>
        public static List<Satellite> Satellites
        {
            get { return _Satellites; }
            set 
            {
                // Look for changes.  A quick check is for a varying number of
                // items in the list.
                bool isChanged = _Satellites.Count != value.Count;

                // Has anything changed?
                if (!isChanged)
                {
                    // No.  Yet, the lists match counts.  Compare them
                    for (int index = 0; index < _Satellites.Count; index++)
                    {
                        if (!_Satellites[index].Equals(value[index]))
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
                _Satellites = value; 

                // Raise an event
                if (SatellitesChanged != null)
                    SatellitesChanged(null, new SatelliteListEventArgs(_Satellites));
            }
        }

        /// <summary>
        /// Controls the current satellite-derived date and time.
        /// </summary>
        public static DateTime UtcDateTime
        {
            get { return _UtcDateTime; }
            set
            {
                // Has anything actually changed?
                if (_UtcDateTime.Equals(value))
                    return;

                // Yes.
                _UtcDateTime = value;

                // Raise an event
                if (UtcDateTimeChanged != null)
                    UtcDateTimeChanged(null, new DateTimeEventArgs(_UtcDateTime));
            }
        }

        /// <summary>
        /// Controls the current satellite-derived date and time.
        /// </summary>
        public static DateTime DateTime
        {
            get 
            { 
                return _UtcDateTime.ToLocalTime(); 
            }
            set
            {
                UtcDateTime = value.ToUniversalTime();
            }
        }

        /// <summary>
        /// Controls the current distance above sea level.
        /// </summary>
        public static Distance Altitude
        {
            get { return _Altitude; }
            set
            {
                // Has anything actually changed?
                if (_Altitude.Equals(value))
                    return;

                // Yes.
                _Altitude = value;

                // Raise an event
                if (AltitudeChanged != null)
                    AltitudeChanged(null, new DistanceEventArgs(_Altitude));
            }
        }

        /// <summary>
        /// Controls the current direction of travel.
        /// </summary>
        public static Azimuth Bearing
        {
            get { return _Bearing; }
            set
            {
                // Has anything actually changed?
                if (_Bearing.Equals(value))
                    return;

                // Yes.
                _Bearing = value;

                // Raise an event
                if (BearingChanged != null)
                    BearingChanged(null, new AzimuthEventArgs(_Bearing));
            }
        }

        #endregion

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
        /// <param name="async">
        /// If set to <see langword="true"/>, then the method will return immediately rather than waiting
        /// for the cancellation to complete.
        /// </param>
        public static void CancelDetection(bool async)
        {
            // If the detection thread is alive, abort it
            if (IsDetectionInProgress)
            {
                // Abort the thread
                Debug.WriteLine("Canceling device detection", DebugCategory);
                _DetectionThread.Abort(async);

                if (!async)
                {
                    // Wait for the abort to wrap up
                    _DetectionCompleteWaitHandle.WaitOne();
                }

                // Detection is complete
                Debug.WriteLine("Device detection has been canceled successfully", DebugCategory);
                OnDeviceDetectionCompleted();
            }
        }

        /// <summary>
        /// Starts looking for GPS devices on a separate thread.
        /// </summary>
        public static void BeginDetection()
        {
            // Start detection on another thread.
            if (_IsDetectionInProgress)
                return;

            // Signal that detection is in progress
            _IsDetectionInProgress = true;

            // Start a thread for managing detection
            _DetectionThread = new Thread(new ThreadStart(DetectionThreadProc));
            _DetectionThread.Name = "GPS.NET Device Detector (http://dotspatial.codeplex.com)";
            _DetectionThread.IsBackground = true;

#if !PocketPC
            // Do detection in the background
            _DetectionThread.Priority = ThreadPriority.Lowest;
#endif

            _DetectionThread.Start();

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
            foreach (Device device in _BluetoothDevices)
                device.Undetect();
            foreach (Device device in _SerialDevices)
                device.Undetect();

#if PocketPC
            if (GpsIntermediateDriver.Current != null)
                GpsIntermediateDriver.Current.Undetect();
#endif

            try
            {
                // Clear any remaining entries in the device cache by deleting the root registry key
                Registry.LocalMachine.DeleteSubKeyTree(RootKeyName);
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
                ManualResetEvent[] waiters = new ManualResetEvent[] {
                    _DetectionCompleteWaitHandle, _DeviceDetectedWaitHandle };
                ManualResetEvent.WaitAny(waiters, timeout);
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
        /// <returns></returns>
        public static bool WaitForDetection(TimeSpan timeout)
        {
            if (!IsDetectionInProgress)
                return true;

#if PocketPC
            return _DetectionCompleteWaitHandle.WaitOne((int)timeout.TotalMilliseconds, false);
#else
            return _DetectionCompleteWaitHandle.WaitOne(timeout, false);
#endif
        }

        /// <summary>
        /// Raises the <see cref="FixLost"/> event.
        /// </summary>
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
        internal static void RaiseFixAcquired(DeviceEventArgs e)
        {
            EventHandler<DeviceEventArgs> handler = FixAcquired;
            if (handler != null)
            {
                handler(null, e);
            }
        }

        #endregion

        #region Private Methods

        private static void BluetoothDevice_DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            /* When this event occurs, a new Bluetooth device has been found.  Check
             * to see if the device is already in our list of known devices.
             */
            BluetoothDevice newDevice = (BluetoothDevice)e.Device;

            // Examine each known device
            for (int index = 0; index < _BluetoothDevices.Count; index++)
            {
                BluetoothDevice device = _BluetoothDevices[index];

                // Is it the same address as this new device?
                if (device.Address.Equals(newDevice.Address))
                {
                    // Yes.  No need to add it again.  Get rid of it.
                    newDevice.Dispose();
                    return;
                }
            }

            // If we get here, the device is brand new.  Add it to the list
            _BluetoothDevices.Add(newDevice);

            // Notify of the discovery
            OnDeviceDiscovered(e.Device);

            // And start detection
            newDevice.BeginDetection();
        }

        private static void DetectionThreadProc()
        {
            try
            {
                // Signal that it started
                Debug.WriteLine("Starting GPS device detection", DebugCategory);
                OnDeviceDetectionStarted();

                // Monitor this thread up to the timeout, then quit
                ThreadPool.QueueUserWorkItem(new WaitCallback(DetectionThreadProcWatcher));

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
                        if(_IsOnlyFirstDeviceDetected)
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

                #endregion
#endif
                if (!IsDeviceDetected && AllowSerialConnections)
                {
                    // If we get here, and there are still no GPSes detected, then try detecting serial devices again,
                    // but this time omitting the colon suffix. On some systems, the colon is not included in the port name.
                    Debug.WriteLine("No serial devices were detected in the first pass. Starting second pass...", DebugCategory);
                    BeginSerialDetection(true);
                    WaitForDetectionInternal();
                }

                // Signal completion
                Debug.WriteLine("Device detection has completed", DebugCategory);
                OnDeviceDetectionCompleted();
            }
            catch (ThreadAbortException ex)
            {
                #region Abort detection for all devices

                Debug.WriteLine("Aborting all device detection threads", DebugCategory);

#if PocketPC
                // Stop detection for the GPSID
                if(GpsIntermediateDriver.Current != null)
                    GpsIntermediateDriver.Current.CancelDetection();
#endif

                // Stop detection for each Bluetooth device
                for (int index = 0; index < _BluetoothDevices.Count; index++)
                    _BluetoothDevices[index].CancelDetection();

                // Stop detection for each serial device
                for (int index = 0; index < _SerialDevices.Count; index++)
                    _SerialDevices[index].CancelDetection();

                #endregion

                // Determine whether we should block until all detection threads have canceled
                bool async = false;
                if (ex.ExceptionState != null && ex.ExceptionState is bool)
                {
                    async = (bool)ex.ExceptionState;
                }

                if (!async)
                {
                    // Wait for all the threads to die.  Just... sit and watch.  And wait.
                    Debug.WriteLine("Waiting for device detection threads to abort", DebugCategory);
                    while (_CurrentlyDetectingWaitHandles.Count != 0)
                    {
                        try { _CurrentlyDetectingWaitHandles[0].WaitOne(); }
                        catch { }
                        finally { _CurrentlyDetectingWaitHandles.RemoveAt(0); }
                    }
                }

                // Signal the cancellation
                Debug.WriteLine("All device detection threads have been aborted", DebugCategory);
                if (DeviceDetectionCanceled != null)
                    DeviceDetectionCanceled(null, EventArgs.Empty);
            }
            finally
            {
                // Detection is no longer in progress
                _DetectionCompleteWaitHandle.Set();
                _CurrentlyDetectingWaitHandles.Clear();    // <--  Already empty?
                _IsDetectionInProgress = false;

#if PocketPC
                // Signal that the thread is alive (no Thread.IsAlive on the CF :P)
                _IsDetectionThreadAlive = false;
#endif
            }
        }

        /// <summary>
        /// This method, spawned by the ThreadPool, monitors detection and aborts it if it's taking too long.
        /// </summary>
        private static void DetectionThreadProcWatcher(object over9000)
        {
            if (_DetectionCompleteWaitHandle.WaitOne((int)_DeviceDetectionTimeout.TotalMilliseconds, false))
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
                Debug.WriteLine("Detecting Bluetooth devices", DebugCategory);

                // Start bluetooth detection for each device
                int count = BluetoothDevices.Count;
                for (int index = 0; index < count; index++)
                    _BluetoothDevices[index].BeginDetection();
            }
        }

        /// <summary>
        /// Begins detecting all serial devices to determine if they are GPS devices.
        /// All detection is done asynchronously, so this method returns immediately.
        /// Use the <see cref="WaitForDetection()"/> method if you need to block until detection is completed.
        /// </summary>
        /// <param name="omitColonSuffix">
        /// If set to <see langword="true"/>, the colon character (":") will be omitted from the port names.
        /// This is required on some systems in order for the device to be detected properly.
        /// </param>
        private static void BeginSerialDetection(bool omitColonSuffix)
        {
            if (AllowSerialConnections)
            {
                Debug.WriteLine("Detecting serial devices", DebugCategory);

                // Begin detection for each of the known serial devices
                int count = SerialDevices.Count;
                for (int index = 0; index < count; index++)
                {
                    SerialDevice device = _SerialDevices[index];

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
                if (_AllowExhaustiveSerialPortScanning)
                {
                    Debug.WriteLine("Scanning all serial ports", DebugCategory);

                    // Try all ports from COM0: up to the maximum port number
                    for (int index = 0; index <= _MaximumSerialPortNumber; index++)
                    {
                        // Is this port already being checked?
                        bool alreadyBeingScanned = false;
                        for (int existingIndex = 0; existingIndex < _SerialDevices.Count; existingIndex++)
                        {
                            if (_SerialDevices[existingIndex].PortNumber.Equals(index))
                            {
                                // Yes.  Don't test it again
                                alreadyBeingScanned = true;
                                break;
                            }

                            // If it's already being scanned, stop
                            if (alreadyBeingScanned)
                                break;
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
                        Debug.WriteLine("Checking " + portName + " for GPS device", DebugCategory);
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
                /* NOTE: For mobile devices, only one connection is allowed at a time.
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
                Debug.WriteLine("Discovering new Bluetooth devices", DebugCategory);
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
            Debug.WriteLine("Waiting for device detection to finish", DebugCategory);

            /* A list holds the wait handles of devices being detected.  When it is empty, 
             * detection has finished on all threads.
             */
            while (_CurrentlyDetectingWaitHandles.Count != 0)
            {
                try
                {
                    ManualResetEvent handle = _CurrentlyDetectingWaitHandles[0];
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
                    _CurrentlyDetectingWaitHandles.RemoveAt(0);
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
            for (int existingDevice = 0; existingDevice < _SerialDevices.Count; existingDevice++)
            {
                if (_SerialDevices[existingDevice].Port.Equals(newName, StringComparison.OrdinalIgnoreCase))
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
            _BluetoothDevices = null;
            _SerialDevices = null;

            // Clear the list of known GPS devices. This will be repopulated as new GPS devices are detected.
            _GpsDevices.Clear();
        }

        internal static void OnDeviceDetectionAttempted(Device device)
        {
            // Add the wait handle to the list of handles to wait upon
            _CurrentlyDetectingWaitHandles.Add(device.DetectionWaitHandle);

            // Notify via an event
            if (DeviceDetectionAttempted != null)
                DeviceDetectionAttempted(device, new DeviceEventArgs(device));
        }

        internal static void OnDeviceDetectionAttemptFailed(DeviceDetectionException exception)
        {
            if (DeviceDetectionAttemptFailed != null)
                DeviceDetectionAttemptFailed(exception.Device, new DeviceDetectionExceptionEventArgs(exception));
        }

        internal static void OnDeviceDetectionStarted()
        {
            _DetectionCompleteWaitHandle.Reset();

            // Signal that detection has started
            if (DeviceDetectionStarted != null)
                DeviceDetectionStarted(null, EventArgs.Empty);
        }

        internal static void OnDeviceDetectionCompleted()
        {
            // Signal that detection has started
            if (DeviceDetectionCompleted != null)
                DeviceDetectionCompleted(null, EventArgs.Empty);
        }

        internal static void OnDeviceDiscovered(Device device)
        {
            if (DeviceDiscovered != null)
                DeviceDiscovered(null, new DeviceEventArgs(device));
        }

        /// <summary>
        /// Adds a GPS device to the list of known GPS devices.
        /// </summary>
        /// <param name="device"></param>
        public static void Add(Device device)
        {
            // Is this device already detected? 
            if (!_GpsDevices.Contains(device))
            {
                // Nope, add it
                _GpsDevices.Add(device);

                // Sort the list based on the most reliable device first
                _GpsDevices.Sort(Device.BestDeviceComparer);
            }

            // Signal that a device is found
            _DeviceDetectedWaitHandle.Set();

            // Raise an event
            if (DeviceDetected != null)
                DeviceDetected(device, new DeviceEventArgs(device));

            // Are we only detecting the first device?  If so, abort now
            if (_IsOnlyFirstDeviceDetected)
                CancelDetection(true);
        }

        internal static bool IsStreamNeeded
        {
            get
            {
                return _IsStreamNeeded;
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a problem which has occured during device detection.
    /// </summary>
    public class DeviceDetectionException : IOException
    {
        private Device _Device;

        public DeviceDetectionException(Device device, Exception innerException)
            : base(innerException.Message, innerException)
        {
            _Device = device;
        }

        public DeviceDetectionException(Device device, string message)
            : base(message)
        {
            _Device = device;
        }

        public DeviceDetectionException(Device device, string message, Exception innerException)
            : base(message, innerException)
        {
            _Device = device;
        }

        public Device Device
        {
            get { return _Device; }
        }
    }

    /// <summary>
    /// Represents information about a device detection problem during detection-related events.
    /// </summary>
    public class DeviceDetectionExceptionEventArgs : EventArgs
    {
        private DeviceDetectionException _Exception;

        public DeviceDetectionExceptionEventArgs(DeviceDetectionException exception)
        {
            _Exception = exception;
        }

        public Device Device
        {
            get
            {
                return _Exception.Device;
            }
        }

        public DeviceDetectionException Exception
        {
            get
            {
                return _Exception;
            }
        }


    }
}
