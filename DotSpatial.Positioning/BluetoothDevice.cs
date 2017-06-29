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
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;

namespace DotSpatial.Positioning
{
    // [RegistryPermission(SecurityAction.LinkDemand, ViewAndModify = @"HKEY_LOCAL_MACHINE\SOFTWARE\DotSpatial.Positioning\GPS.NET\3.0\Devices\Bluetooth")]
    /// <summary>
    /// Represents a service on a Bluetooth(tm) device.
    /// </summary>
    /// <remarks>Bluetooth GPS devices often provide access to data using sockets.  In order to establish a connection
    /// to a Bluetooth GPS device,</remarks>
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    public sealed class BluetoothDevice : NetworkDevice, IEquatable<BluetoothDevice>
    {
        /// <summary>
        ///
        /// </summary>
        private string _name;
        /// <summary>
        ///
        /// </summary>
        private BluetoothAddress _address;
        /// <summary>
        ///
        /// </summary>
        private SerialDevice _virtualSerialPort;
        /// <summary>
        ///
        /// </summary>
        private DeviceClass _class;
        /// <summary>
        ///
        /// </summary>
        private DeviceClass _minorClass;
        /// <summary>
        ///
        /// </summary>
        private ServiceClass _serviceClass;
#if ServicesSupported
        /* For our purposes, we can assume that Bluetooth devices will offer a generic L2Cap/RFCOMM service.
         * that's good enough.
         */
        private Thread _ServiceDiscoveryThread;
        private bool _IsServiceDiscoveryInProgress;
        private bool _IsServiceDiscoveryCacheFlushed;
#endif

        /// <summary>
        ///
        /// </summary>
        internal static Thread DeviceDiscoveryThread;
        /// <summary>
        ///
        /// </summary>
        private static readonly ManualResetEvent _discoveryStartedWaitHandle = new ManualResetEvent(false);
        /// <summary>
        ///
        /// </summary>
        private static bool _isDeviceDiscoveryInProgress;
        /// <summary>
        ///
        /// </summary>
        private static bool _isDeviceDiscoveryCacheFlushed;
        /// <summary>
        ///
        /// </summary>
        private static TimeSpan _discoveryTimeout = TimeSpan.FromSeconds(5);
        /// <summary>
        ///
        /// </summary>
        private static int _maximumAllowedFailures = 20;

        #region Constants

        /// <summary>
        ///
        /// </summary>
        private const string MY_ROOT_KEY_NAME = Devices.ROOT_KEY_NAME + @"Bluetooth\";
        /// <summary>
        ///
        /// </summary>
        private const AddressFamily BLUETOOTH_FAMILY = (AddressFamily)32;
        /// <summary>
        ///
        /// </summary>
        private const ProtocolType RF_COMM = (ProtocolType)0x0003;

        #endregion Constants

        #region Fields

        ///// <summary>
        ///// Returns a GUID which serves as the base of all Bluetooth services.
        ///// </summary>
        //private static readonly Guid BaseServiceGuid = new Guid(0x00000000, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        /// <summary>
        /// Returns a GUID which represents the RFComm service.
        /// </summary>
        private static readonly Guid _rfCommServiceGuid = new Guid(0x00000003, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);
        ///// <summary>
        ///// Returns a GUID which represents the L2Cap service.
        ///// </summary>
        //private static readonly Guid L2CapServiceGuid = new Guid(0x00000100, 0x0000, 0x1000, 0x80, 0x00, 0x00, 0x80, 0x5F, 0x9B, 0x34, 0xFB);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        public BluetoothDevice(BluetoothAddress address)
            : this(address, address.ToString())
        { }

        /// <summary>
        /// Creates a new instance using the specified address and name.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="name">The name.</param>
        public BluetoothDevice(BluetoothAddress address, string name)
            : base(
                // Open a Bluetooth connection as a stream
               BLUETOOTH_FAMILY,
                // Streams are used for all communications
               SocketType.Stream,
                // All Bluetooth connections will use RFComm (?)
               RF_COMM,
                /* After dicking around with many Bluetooth devices, I find that a vast majority of them
                 * expose a single service on port one (1), with no GUID.  So, it's a pretty safe assumption.
                 * However, a service GUID *is* required on mobile devices.
                 */
               new BluetoothEndPoint(address, _rfCommServiceGuid))
        {
            _address = address;
            _name = name;

            // Read the cache
            OnCacheRead();
        }

        /// <summary>
        /// Creates a new instance using the specified endpoint and friendly name.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <param name="name">The name.</param>
        public BluetoothDevice(BluetoothEndPoint endPoint, string name)
            : base(
                // Open a Bluetooth connection as a stream
                BLUETOOTH_FAMILY,
                // Streams are used for all communications
                SocketType.Stream,
                // All Bluetooth connections will use RFComm
                RF_COMM,
                // Use the endpoint
                endPoint)
        {
            _address = endPoint.Address;
            _name = name;

            // Populate cache information
            OnCacheRead();
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Occurs when a new Bluetooth device has been detected.
        /// </summary>
        public static event EventHandler<DeviceEventArgs> DeviceDiscovered;

        #endregion Events

        #region Public Properties

        /// <summary>
        /// Returns the address of the device.
        /// </summary>
        [Category("Bluetooth")]
        [Description("Returns the address of the device.")]
        [Browsable(true)]
        [ParenthesizePropertyName(true)]
        public BluetoothAddress Address
        {
            get
            {
                return _address;
            }
        }

        /// <summary>
        /// Returns the primary purpose of the device.
        /// </summary>
        [Category("Bluetooth")]
        [Description("Returns the primary purpose of the device.")]
        [Browsable(true)]
        public DeviceClass Class
        {
            get
            {
                return _class;
            }
        }

        /// <summary>
        /// Returns a sub-category describing the purpose of the device.
        /// </summary>
        [Category("Bluetooth")]
        [Description("Returns a sub-category describing the purpose of the device.")]
        [Browsable(true)]
        public DeviceClass MinorClass
        {
            get
            {
                return _minorClass;
            }
        }

        /// <summary>
        /// Returns the major type of device.
        /// </summary>
        [Category("Bluetooth")]
        [Description("Returns the major type of device.")]
        [Browsable(true)]
        public ServiceClass ServiceClass
        {
            get
            {
                return _serviceClass;
            }
        }

        /// <summary>
        /// Returns a serial device which has been linked to this device.
        /// </summary>
        [Category("Bluetooth")]
        [Description("Returns a serial device which has been linked to this device.")]
        [Browsable(true)]
        public SerialDevice VirtualSerialPort
        {
            get
            {
                if (_virtualSerialPort == null)
                {
                    /* Sigh.  Microsoft has no way to easily identify the port associated with
                     * a Bluetooth device.  However, after a lot of registry snooping, there is
                     * a way to use the Address to find the port name.
                     *
                     * Let's say we have a GPS BT address of "00:12:6F:00:45:E5".  The address can
                     * be found by examining the "HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\BTHENUM"
                     * branch.  On my box, I see this key: "8&6c39ac4&0&00126F0045E5_C00000000".  If you
                     * look closely, the address is in there (just before the underscore).  By matching up
                     * the strings, the "PortName" value can be read.  I wish this was a joke.
                     */

                    // Start with the device address, without the colons
                    string address = _address.ToString().Replace(":", string.Empty);

                    /* Now examine the registry
                     *
                     * HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\BTHENUM\
                     *      {00001101-0000-1000-8000-00805f9b34fb}_LOCALMFG&000f\
                     *          8&6c39ac4&0&00126F0045E5_C00000000\
                     *              Device Parameters
                     */

                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum\BTHENUM", false);

                    // If this is null, there are no virtual ports at all
                    if (key == null)
                        return null;

                    // Yes.  Enumerate the enumerators
                    string[] bluetoothEnumerators = key.GetSubKeyNames();
                    foreach (string enumerator in bluetoothEnumerators)
                    {
                        // Next, enumerate the first level of keys underneath, looking for a match
                        RegistryKey enumeratorKey = key.OpenSubKey(enumerator);
                        if (enumeratorKey != null)
                        {
                            string[] virtualPorts = enumeratorKey.GetSubKeyNames();

                            foreach (string virtualPort in virtualPorts)
                            {
                                // Does this string contain the address?
                                if (virtualPort.Contains(address))
                                {
                                    // Sweet zombie Jesus it does!  Ok, now drill into the friendly port name
                                    RegistryKey virtualPortKey = enumeratorKey.OpenSubKey(virtualPort + @"\Device Parameters", false);

                                    if (virtualPortKey != null)
                                    {
                                        // Finally, get the friendly name
                                        string friendlyName = virtualPortKey.GetValue("PortName") + ":";

                                        // Clean up
                                        virtualPortKey.Close();

                                        // And make a device out of it
                                        _virtualSerialPort = new SerialDevice(friendlyName, 115200);

                                        // Set the name of the port
                                        _virtualSerialPort.SetName("Virtual Serial Port for " + Name);
                                    }
                                }
                            }
                        }

                        // Clean up
                        if (enumeratorKey != null) enumeratorKey.Close();
                    }

                    // Clean up
                    key.Close();
                }

                return _virtualSerialPort;
            }
        }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Returns the name of the Bluetooth device.
        /// </summary>
        /// <inheritdocs/>
        /// <remarks>If the device has been identified, the actual name of the device will be returned.  Otherwise, the
        /// device's address will be returned.</remarks>
        public override string Name
        {
            get
            {
                // If the name is not known, try to get it now
                if (_name.Equals(_address.ToString()))
                    Refresh();

                return _name;
            }
        }

        /// <summary>
        /// Controls whether the device can be queried for GPS data.
        /// </summary>
        /// <value><c>true</c> if [allow connections]; otherwise, <c>false</c>.</value>
        public override bool AllowConnections
        {
            get
            {
                return base.AllowConnections && Devices.AllowBluetoothConnections;
            }
            set
            {
                base.AllowConnections = value;
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_virtualSerialPort != null)
                _virtualSerialPort.Dispose();

            if (disposing)
            {
                _name = null;
                _virtualSerialPort = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Performs a test on the device to confirm that it transmits GPS data.
        /// </summary>
        /// <returns>A <strong>Boolean</strong> value, <strong>True</strong> if the device is confirmed.</returns>
        protected override bool DetectProtocol()
        {
            // If no connections are allowed, exit
            if (!AllowConnections)
            {
                Debug.WriteLine(Name + " will not be tested because connections are disabled", Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(
                    new DeviceDetectionException(this, "Connections are not allowed to " + Name));
                return false;
            }

            // Is this a computer?
            if (Class == DeviceClass.Computer
                || Class == DeviceClass.Phone)
            {
                Debug.WriteLine(Name + " will not be tested because it is a computer", Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(
                    new DeviceDetectionException(this, Name + " will not be tested because it is a computer."));
                return false;
            }

            // If the device is an epic fail, don't even test it anymore
            if (SuccessfulDetectionCount == 0 && MaximumAllowedFailures > 0 && FailedDetectionCount > MaximumAllowedFailures)
            {
                Debug.WriteLine(Name + " will not be tested because it has failed over " + MaximumAllowedFailures + " times", Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(
                    new DeviceDetectionException(this, Name + " has never been successfully detected and has exceeded the maximum allowed number of failures."));
                return false;
            }

            // If we have a track record of success and a connection worked in the past hour, that's good enough.
            if (SuccessfulDetectionCount > 5 && DateTime.Now.Subtract(DateDetected) < TimeSpan.FromHours(1))
                return true;

#if ServicesSupported
            // If we're still discovering services, wait.
            if (_ServiceDiscoveryThread != null

                && _ServiceDiscoveryThread.IsAlive
                )
                _ServiceDiscoveryThread.Join();
#endif

            // Get the current endpoint
            BluetoothEndPoint endPoint = (BluetoothEndPoint)EndPoint;

            // Open a connection to the device
            Open(endPoint);

            // Is this NMEA?
            if (!NmeaReader.IsNmea(BaseStream))
            {
                // No.  Close the connection
                Close();

                // Bump up the failure count
                endPoint.FailedDetectionCount++;

                // No endpoints succeeded
                Debug.WriteLine("A connection was made to " + Name + " but no valid GPS data was found", Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(
                    new DeviceDetectionException(this, "A connection was made to " + Name + " but no valid GPS data was found."));
                return false;
            }

            // Bump up the count for the endpoint
            endPoint.SuccessfulDetectionCount++;

            // RESET the failure count.  We're only interested in CONSECUTIVE failures.  I DONT KNOW WHAT WE'RE YELLING ABOUT
            endPoint.FailedDetectionCount = 0;

            // Yes!  If nobody needs this stream, and we created it, close it
            if (!Devices.IsStreamNeeded)
                Close();

            // Return success
            return true;
        }

        /// <summary>
        /// Called when [cache remove].
        /// </summary>
        protected override void OnCacheRemove()
        {
            try
            {
                Registry.LocalMachine.DeleteSubKeyTree(MY_ROOT_KEY_NAME + _address);
            }
            catch (UnauthorizedAccessException)
            { }
            finally
            {
                // Reset the cache properties
                SetSuccessfulDetectionCount(0);
                SetFailedDetectionCount(0);
                SetDateDetected(DateTime.MinValue);
                SetDateConnected(DateTime.MinValue);
                SetTotalConnectionTime(TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Called when [cache write].
        /// </summary>
        protected override void OnCacheWrite()
        {
            RegistryKey deviceKey = null;
            try
            {
                // Save device information
                deviceKey = Registry.LocalMachine.CreateSubKey(MY_ROOT_KEY_NAME + _address);
                if (deviceKey != null)
                {
                    // Save the friendly name of the device
                    deviceKey.SetValue(string.Empty, _name);

                    // Update the success/fail statistics
                    deviceKey.SetValue("Number of Times Detected", SuccessfulDetectionCount);
                    deviceKey.SetValue("Number of Times Failed", FailedDetectionCount);
                    deviceKey.SetValue("Date Last Detected", DateDetected.ToString("G", CultureInfo.InvariantCulture));
                    deviceKey.SetValue("Date Last Connected", DateConnected.ToString("G", CultureInfo.InvariantCulture));
                    deviceKey.SetValue("Total Connection Time", TotalConnectionTime.ToString());
                    deviceKey.SetValue("Class", Class.ToString());
                    deviceKey.SetValue("Minor Class", MinorClass.ToString());
                    deviceKey.SetValue("Service Class", ServiceClass.ToString());

                    // Do we have an endpoint?
                    if (EndPoint != null)
                    {
                        // Yes.  Write the endpoint to the registry
                        BluetoothEndPoint endPoint = (BluetoothEndPoint)EndPoint;
                        RegistryKey endPointKey = deviceKey.CreateSubKey(endPoint.Port.ToString(CultureInfo.InvariantCulture));
                        if (endPointKey != null)
                        {
                            // Save the name
                            if (endPoint.Name != null)
                                endPointKey.SetValue(string.Empty, endPoint.Name);

                            // Save the service GUID
                            endPointKey.SetValue("GUID", endPoint.Service.ToString());

                            // Update the success/fail statistics
                            endPointKey.SetValue("Number of Times Detected", endPoint.SuccessfulDetectionCount);
                            endPointKey.SetValue("Number of Times Failed", endPoint.FailedDetectionCount);

                            endPointKey.Close();
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                /* This will likely happen if the program is not run as an Administrator. */
            }
            finally
            {
                if (deviceKey != null)
                    deviceKey.Close();
            }
        }

        /// <summary>
        /// Called when [cache read].
        /// </summary>
        protected override void OnCacheRead()
        {
            // Save device stats
            RegistryKey deviceKey = Registry.LocalMachine.OpenSubKey(MY_ROOT_KEY_NAME + _address, false);
            if (deviceKey == null)
                return;

            // Update the baud rate and etc.
            foreach (string name in deviceKey.GetValueNames())
            {
                switch (name)
                {
                    case "":
                        _name = Convert.ToString(deviceKey.GetValue(name), CultureInfo.InvariantCulture);
                        break;
                    case "Number of Times Detected":
                        SetSuccessfulDetectionCount(Convert.ToInt32(deviceKey.GetValue(name)));
                        break;
                    case "Number of Times Failed":
                        SetFailedDetectionCount(Convert.ToInt32(deviceKey.GetValue(name)));
                        break;
                    case "Date Last Detected":
                        SetDateDetected(Convert.ToDateTime(deviceKey.GetValue(name), CultureInfo.InvariantCulture));
                        break;
                    case "Date Last Connected":
                        SetDateConnected(Convert.ToDateTime(deviceKey.GetValue(name), CultureInfo.InvariantCulture));
                        break;
                    case "Total Connection Time":
                        SetTotalConnectionTime(TimeSpan.Parse(Convert.ToString(deviceKey.GetValue(name), CultureInfo.InvariantCulture)));
                        break;
                    case "Class":
                        _class = (DeviceClass)Enum.Parse(typeof(DeviceClass), Convert.ToString(deviceKey.GetValue(name), CultureInfo.InvariantCulture), false);
                        break;
                    case "Minor Class":
                        _minorClass = (DeviceClass)Enum.Parse(typeof(DeviceClass), Convert.ToString(deviceKey.GetValue(name), CultureInfo.InvariantCulture), false);
                        break;
                    case "Service Class":
                        _serviceClass = (ServiceClass)Enum.Parse(typeof(ServiceClass), Convert.ToString(deviceKey.GetValue(name), CultureInfo.InvariantCulture), false);
                        break;
                }
            }

            // Now enumerate endpoint keys
            foreach (string portName in deviceKey.GetSubKeyNames())
            {
                RegistryKey endPointKey = deviceKey.OpenSubKey(portName, false);
                if (endPointKey == null)
                    continue;

                // Deserialize the endpoint
                string name = Convert.ToString(endPointKey.GetValue(string.Empty), CultureInfo.InvariantCulture);
                int port = Convert.ToInt32(portName);
                Guid guid = new Guid(Convert.ToString(endPointKey.GetValue("GUID"), CultureInfo.InvariantCulture));
                BluetoothEndPoint endPoint = new BluetoothEndPoint(_address, guid, port);
                endPoint.SetName(name);
                endPoint.SuccessfulDetectionCount = Convert.ToInt32(endPointKey.GetValue("Number of Times Detected"));
                endPoint.FailedDetectionCount = Convert.ToInt32(endPointKey.GetValue("Number of Times Failed"));

                // Set the default endpoint
                EndPoint = endPoint;

                endPointKey.Close();
            }

            // Try to reload device info
            Refresh();
        }

        #endregion Overrides

        #region Static Properties

        /// <summary>
        /// Controls the maximum allowed detection failures before a device is excluded from detection.
        /// </summary>
        /// <value>The maximum allowed failures.</value>
        /// <remarks>Some devices involved with device detection are not GPS devices.  For example, a Bluetooth search
        /// could return wireless headphones, or the neighbor's computer.  This property controls how many times a device will be
        /// tested before it is no longer included for searches.  If a device's failure count goes beyond this number, attempts
        /// will no longer be made to connect to the device.</remarks>
        public static int MaximumAllowedFailures
        {
            get
            {
                return _maximumAllowedFailures;
            }
            set
            {
                _maximumAllowedFailures = value;
            }
        }

        /// <summary>
        /// Returns a list of known Bluetooth devices.
        /// </summary>
        /// <remarks><para>To maximize performance, GPS.NET will record information about each device it encounters for the purposes of organizing
        /// and speeding up the GPS device detection process.  This property returns a list of all known Bluetooth devices.</para>
        ///   <para>Since this list is managed automatically, it should not be modified.</para></remarks>
        public static IList<BluetoothDevice> Cache
        {
            get
            {
                List<BluetoothDevice> devices = new List<BluetoothDevice>();

                // Pass 1: Look for devices recorded in the registry
                LoadCachedDevices(devices);

                // Pass 2: Look for devices discovered already by Windows
                LoadWindowsDevices(devices);

                // Sort the cache based on the most reliable device first
                devices.Sort(BestDeviceComparer);

                return devices;
            }
        }

        /// <summary>
        /// Controls the amount of time spent searching for Bluetooth devices.
        /// </summary>
        /// <value>The discovery timeout.</value>
        public static TimeSpan DiscoveryTimeout
        {
            get
            {
                return _discoveryTimeout;
            }
            set
            {
                if (value.TotalSeconds > 255)
                    throw new ArgumentOutOfRangeException("DiscoveryTimeout", "The discovery timeout must be a value between 0 and 255 seconds.");

                _discoveryTimeout = value;
            }
        }

        #endregion Static Properties

        #region Static Methods

        /// <summary>
        /// Searches the room for Bluetooth devices.
        /// </summary>
        /// <remarks>This method will perform discovery of nearby Bluetooth devices.  Discovery will take place on a separate thread,
        /// and the <strong>DeviceDiscovered</strong> event will be raised for each device service discovered.  If a device publishes multiple
        /// services, multiple events will be raised for each service.</remarks>
        public static void DiscoverDevices()
        {
            DiscoverDevices(false);
        }

        /// <summary>
        /// Searches the room for Bluetooth devices.
        /// </summary>
        /// <param name="isCacheFlushed">A <strong>Boolean</strong> indicating whether the cache of Bluetooth information should be cleared.</param>
        /// <remarks><para>This method will perform discovery of nearby Bluetooth devices.  Discovery will take place on a separate thread,
        /// and the <strong>DeviceDiscovered</strong> event will be raised for each device service discovered.  If a device publishes multiple
        /// services, multiple events will be raised for each service.</para>
        ///   <para>If the <strong>IsCacheFlushed</strong> parameter is <strong>True</strong>, any cached information will be cleared, and a
        /// wireless scan of devices will be performed.  This kind of scan is preferable since it can detect which devices are actually responding
        /// right now, even though the query will take several seconds to occur.  A value of <strong>False</strong> is typically used if a
        /// device scan was recently performed and responsiveness is necessary.</para></remarks>
        [SecurityCritical]
        public static void DiscoverDevices(bool isCacheFlushed)
        {
            // Is discovery already in progress?
            if (_isDeviceDiscoveryInProgress)
                return;

            // Is Bluetooth enabled?
            if (!Devices.IsBluetoothEnabled)
                throw new InvalidOperationException("Bluetooth is currently turned off.");

            // Signal that we've started
            _isDeviceDiscoveryInProgress = true;

            // Update the caching flags
            _isDeviceDiscoveryCacheFlushed = isCacheFlushed;

            // Start Bluetooth discovery
            DeviceDiscoveryThread = new Thread(DiscoverDevicesThreadProc)
                                        {
                                            IsBackground = true,
                                            Name =
                                                "GPS.NET Bluetooth Device Discovery Thread (http://dotspatial.codeplex.com)",
                                            Priority = ThreadPriority.Lowest
                                        };

            DeviceDiscoveryThread.Start();

            // Block until the thread is actually alive
            _discoveryStartedWaitHandle.WaitOne();
        }

        /// <summary>
        /// Wait for discovery
        /// </summary>
        /// <returns>boolean</returns>
        public static bool WaitForDiscovery()
        {
            return WaitForDiscovery(_discoveryTimeout);
        }

        /// <summary>
        /// Waits for discovery, specifying a timeout
        /// </summary>
        /// <param name="timeout">The TimeSpan specifying the timeout where waiting ends.</param>
        /// <returns></returns>
        public static bool WaitForDiscovery(TimeSpan timeout)
        {
            if (DeviceDiscoveryThread == null

                || !DeviceDiscoveryThread.IsAlive

            )
                return true;

            return DeviceDiscoveryThread.Join(timeout);
        }

#if ServicesSupported

        public void DiscoverServices()
        {
            DiscoverServices(false);
        }

        public void DiscoverServices(bool isCacheFlushed)
        {
            // Is discovery already in progress?
            if (_IsServiceDiscoveryInProgress)
                return;

            // Signal that we've started
            _IsServiceDiscoveryInProgress = true;
            _IsServiceDiscoveryCacheFlushed = isCacheFlushed;

            // Start Bluetooth discovery
            _ServiceDiscoveryThread = new Thread(new ThreadStart(DiscoverServicesThreadProc));
            _ServiceDiscoveryThread.IsBackground = true;
            _ServiceDiscoveryThread.Name = "GPS.NET Bluetooth Service Discovery on " + Name + " (http://dotspatial.codeplex.com)";
            _ServiceDiscoveryThread.Priority = ThreadPriority.Lowest;
            _ServiceDiscoveryThread.Start();
        }

        private void DiscoverServicesThreadProc()
        {
        #region Variables which must be cleanly finalized

            NativeMethods.WSAQUERYSET query = new NativeMethods.WSAQUERYSET();
            IntPtr queryHandle = IntPtr.Zero;
            GCHandle serviceGuidHandle = GCHandle.Alloc(0, GCHandleType.Weak);     // There is no "null" GCHandle.  As a result, we just assign it to
            GCHandle deviceAddressHandle = GCHandle.Alloc(0, GCHandleType.Weak);   // a weak, arbitrary var.
            GCHandle queryResultHandle = GCHandle.Alloc(0, GCHandleType.Weak);

        #endregion Variables which must be cleanly finalized

            /* This section deals a lot with unmanaged memory.  As a result, it's very important that the 'finally'
             * clause get execute in ALL circumstances to ensure that memory is properly cleaned up.  For this, we
             * use the RuntimeHelpers class to instruct .NET to execute this clause no matter what.  This is a
             * 'critical finalizer'.
             */

#if !PocketPC
            RuntimeHelpers.PrepareConstrainedRegions();
#endif
            try
            {
                /* Service discovery for Bluetooth devices is very similar to discovery for new devices.
                 * In fact, the same method (WSALookupServiceBegin) is used.  The only difference is in the
                 * calling parameters.  For services, the address of a device is specified.
                 *
                 * This code was developed based on MSDN documentation for the WSAServiceLookupBegin method.
                 * http://msdn.microsoft.com/en-us/library/aa362914(VS.85).aspx
                 */

                // First, start up a WSA session.  This call is mandatory (along with WSAShutdown)
                NativeMethods.WSAData data = new NativeMethods.WSAData();
                int returnCode = NativeMethods.WSAStartup(36, data);
                switch (returnCode)
                {
                    case 0:
                    case 10014:
                        // The operation completed successfully.
                        break;
                    default:
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                // Configure parameters for this query
                query = new NativeMethods.WSAQUERYSET();

                // The query will look for ALL services.  Create a parameter, then pass in a pointer to it
                byte[] serviceGuidBytes = BluetoothDevice.L2CapServiceGuid.ToByteArray();

                // Pin this object in memory while we're working with it
                serviceGuidHandle = GCHandle.Alloc(serviceGuidBytes, GCHandleType.Pinned);

                // Assign the pointer to the query
                query.lpServiceClassId = serviceGuidHandle.AddrOfPinnedObject();

                /* Next, the address of the device is specified to make this a search
                 * for services.
                 */

                // Get the address of the device as text
                string deviceAddress = _Address.ToString();

                // Prevent the object from changing places in memory
                deviceAddressHandle = GCHandle.Alloc(deviceAddress, GCHandleType.Pinned);

                // Pass in a pointer to the device
                query.lpszContext = deviceAddressHandle.AddrOfPinnedObject();

#if !PocketPC
                // Next, flags are configured which instruct the API call to return specific information
                NativeMethods.LookupFlags flags = NativeMethods.LookupFlags.None;

                // If we're flushing the cache, add that flag
                if (_IsDeviceDiscoveryCacheFlushed)
                    flags |= NativeMethods.LookupFlags.FlushCache;
#else
                // Next, flags are configured which instruct the API call to return specific information
                NativeMethods.LookupFlags flags = NativeMethods.LookupFlags.Containers;
#endif

                // Perform the query
                returnCode = NativeMethods.WSALookupServiceBegin(query, flags, ref queryHandle);

                // What's the result of the query?
                if (returnCode == -1)
                {
                    // An error.  What's the specific code?
                    returnCode = Marshal.GetLastWin32Error();

                    switch (returnCode)
                    {
                        case 10102:  // No more records (old)
                        case 10103:  // Canceled
                        case 10108:  // No services exist
                        case 10110:  // No more records
                        case 10111:
                            return;
                    }

                    // A legitimate error has occurred
                    throw new Win32Exception(returnCode);
                }

        #region Process search results

                /* When a search returns records, the WSALookupServiceNext method is used to move to
                 * the next result.  The goal here is to get a friendly name and a GUID of each service.
                 */

                // Create a chunk of memory the size of a WSAQUERYSET
                byte[] resultBuffer = new byte[2048];
                int resultBufferSize = resultBuffer.Length;

                // Pin it so we can get a pointer
                queryResultHandle = GCHandle.Alloc(resultBuffer, GCHandleType.Pinned);

                // Get the address
                IntPtr queryResultPointer = queryResultHandle.AddrOfPinnedObject();

                // Anything?
                for (int index = 0; index < 1; index++) //  (returnCode == 0)
                {
                    returnCode = NativeMethods.WSALookupServiceNext(
                        // Use the handle of our current query
                        queryHandle,
#if !PocketPC
                        // Indicate that we want the GUID, name and and comments
                        NativeMethods.LookupFlags.ReturnType
                        | NativeMethods.LookupFlags.ReturnAddr
                        | NativeMethods.LookupFlags.ReturnName
                        | NativeMethods.LookupFlags.ReturnComment,
#else
                        // Indicate that we want the GUID and the name
                        NativeMethods.LookupFlags.None,
#endif
                        // Indicate the size of the block of memory
                        ref resultBufferSize,
                        // Pass a pointer to the memory block itself
                        resultBuffer);

                    if (returnCode == -1)
                    {
                        // What exactly was the error?
                        int lastError = Marshal.GetLastWin32Error();
                        switch (lastError)
                        {
                            case 10050:  // INVALID INPUT.  This means there's a b u g to fix
                            case 10110:  // No more records
                                break;
                        }
                        throw new Win32Exception(lastError);
                    }

                    // Create an object to pour data into
                    NativeMethods.WSAQUERYSET result = new NativeMethods.WSAQUERYSET();

                    // Marshal the data block into the object
                    Marshal.PtrToStructure(queryResultPointer, result);

                    // Get the endpoint for this service
                    BluetoothEndPoint endPoint = result.AddressInfo.RemoteAddr.EndPoint;

                    // Set the friendly name of the endpoint
                    endPoint.SetName(result.FriendlyName);

                    // Use it for connections
                    base.EndPoint = endPoint;
                }

        #endregion Process search results

                // Update the cache
                OnWriteToCache();
            }
            catch (ThreadAbortException)
            {
                return;
            }
            finally
            {
                /* Since we deal a lot with unmanaged memory, it's critical that this memory gets de-allocated, even
                 * in the event of a ThreadAbortException.  As a result, we'll examine and de-allocate memory as necessary here.
                 */

                // Free up pinned objects
                serviceGuidHandle.Free();
                deviceAddressHandle.Free();
                queryResultHandle.Free();

                // If a query was in progress, end it
                if (queryHandle != IntPtr.Zero)
                    NativeMethods.WSALookupServiceEnd(queryHandle);

                // Finally, finish with stuffs
                NativeMethods.WSACleanup();

                // and signal we're done
                _IsServiceDiscoveryInProgress = false;
            }
        }
#endif

        /// <summary>
        /// Discovers the devices thread proc.
        /// </summary>
        [SecurityCritical]
        private static void DiscoverDevicesThreadProc()
        {
            /* The code required for the desktop is vastly different from the code required for the CF.
             * As a result, for the sake of readability I'm creating two whole blocks of code instead
             * of trying to stitch compiler directives in everywhere.
             */

            // Signal that we've started
            Debug.WriteLine("Starting Bluetooth device discovery", Devices.DEBUG_CATEGORY);
            _discoveryStartedWaitHandle.Set();

            #region Variables which must be cleanly finalized

            NativeMethods2.WsaQuerySet query = new NativeMethods2.WsaQuerySet();
            NativeMethods2.Blob blob;
            GCHandle blobHandle = GCHandle.Alloc(0, GCHandleType.Weak);
            IntPtr queryHandle = IntPtr.Zero;
            GCHandle optionsHandle = GCHandle.Alloc(0, GCHandleType.Weak);
            GCHandle queryResultHandle = GCHandle.Alloc(0, GCHandleType.Weak);

            #endregion Variables which must be cleanly finalized

            /* This section deals a lot with unmanaged memory.  As a result, it's very important that the 'finally'
             * clause get execute in ALL circumstances to ensure that memory is properly cleaned up.  For this, we
             * use the RuntimeHelpers class to instruct .NET to execute this clause no matter what.  This is a
             * 'critical finalizer'.
             */

            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                /* Bluetooth device discovery involves making calls to native methods, namely
                 * WSALookupServiceBegin, WSALookupServiceNext, and WSALookupServiceEnd.  The old
                 * GPS.NET 2.0 library used Peter Foot's "32Feet.net" library, but the library was
                 * very poorly documented and bloated.  This code is a ground-up rewrite in an attempt
                 * to minimize calls and produce well-documented code.
                 *
                 * Code was developed based on MSDN documentation here:
                 * http://msdn.microsoft.com/en-us/library/aa362913(VS.85).aspx
                 */

                // First, start up a WSA session.  This call is mandatory (along with WSAShutdown)
                NativeMethods2.WsaData data = new NativeMethods2.WsaData();
                int returnCode = NativeMethods2.WSAStartup(36, data);
                if (returnCode != 0)
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                #region Start a search for devices

                // Control flags which determine how the search is performed
                NativeMethods2.WasLookupControlOptions controlOptions =
                    // This flag is manadatory
                    NativeMethods2.WasLookupControlOptions.Containers;

                // Indicate that we want a fat blob describing the device
                controlOptions |= NativeMethods2.WasLookupControlOptions.ReturnName
                        | NativeMethods2.WasLookupControlOptions.ReturnAddresses
                        | NativeMethods2.WasLookupControlOptions.ReturnType
                        | NativeMethods2.WasLookupControlOptions.ReturnBlob;

                // Are we ignoring the cache?  If so, add the flag
                if (_isDeviceDiscoveryCacheFlushed)
                    controlOptions |= NativeMethods2.WasLookupControlOptions.FlushCache;

                /* For this circumstance, we must specify additional query information in the
                 * form of a BLOB containing a BTH_QUERY_DEVICE  (I know lol)
                 */

                // Create an object to store options
                NativeMethods2.BthQueryDevice options = new NativeMethods2.BthQueryDevice();

                // Ensure that the object does not change places in memory
                optionsHandle = GCHandle.Alloc(options, GCHandleType.Pinned);

                // Make a blob to house the options
                blob = new NativeMethods2.Blob();

                // Make sure the blob cannot change places in memory
                blobHandle = GCHandle.Alloc(blob, GCHandleType.Pinned);

                // Specify the discovery timeout (in seconds)
                options.Length = Convert.ToByte(_discoveryTimeout.TotalSeconds);

                // Smoosh this data into a BLOB object.
                blob.Size = Convert.ToUInt32(Marshal.SizeOf(options));
                blob.Info = optionsHandle.AddrOfPinnedObject();

                /* The .NET Garbage Collector likes to move objects around in memory.  This would totally
                 * mess with what we're doing now, unless we "pin" objects we're working with.  Pinning
                 * will prevent the object from changing location in memory.
                 */

                // Assign this BLOB to the query
                query.EntityBlob = blobHandle.AddrOfPinnedObject();

                // Begin the search and look for an error code
                returnCode = NativeMethods2.WSALookupServiceBegin(query, controlOptions, ref queryHandle);

                // What's the result of the query?
                if (returnCode == -1)
                {
                    // An error.  What's the specific code?
                    int errorCode = Marshal.GetLastWin32Error();
                    if (errorCode == 10108)
                    {
                        // No services found!
                        return;
                    }
                    // A legitimate error has occurred
                    throw new Win32Exception(errorCode);
                }

                #endregion Start a search for devices

                #region Process search results

                /* Search results will be returned in the form of a WSAQUERYSET object, inside of which is
                 * a BTH_DEVICE_INFO object describing a Bluetooth device.  As a result, we'll give the method
                 * a chunk of memory, and the marshal that block back into objects.
                 */

                // Create a chunk of memory the size of a WSAQUERYSET
                byte[] resultBuffer = new byte[2048];
                int resultBufferSize = resultBuffer.Length;

                // Pin it so we can get a pointer
                queryResultHandle = GCHandle.Alloc(resultBuffer, GCHandleType.Pinned);

                // Get the address
                IntPtr queryResultPointer = queryResultHandle.AddrOfPinnedObject();

                // DO EET FOREVER
                while (true)
                {
                    #region Find the next device

                    // Find the next Bluetooth device
                    returnCode = NativeMethods2.WSALookupServiceNext(
                        // Use our opened query handle
                        queryHandle,
                        // Indicate that we want a fat blob describing the device
                        NativeMethods2.WasLookupControlOptions.ReturnBlob,
                        // Indicate the size of the block of memory
                        ref resultBufferSize,
                        // Pass a pointer to the memory block itself
                        resultBuffer);

                    if (returnCode == -1)
                    {
                        // What exactly was the error?
                        int lastError = Marshal.GetLastWin32Error();

                        switch (lastError)
                        {
                            case 10110:
                                // No more results
                                return;
                            case 10050:
                                // Bluetooth is not turned on!
                                return;
                            default:
                                throw new Win32Exception(lastError);
                        }
                    }

                    // Create an object to pour data into
                    NativeMethods2.WsaQuerySet result = new NativeMethods2.WsaQuerySet();

                    // Marshal the data block into the object
                    Marshal.PtrToStructure(queryResultPointer, result);

                    #endregion Find the next device

                    #region Decode the device information

                    /* At this point, the WSAQUERYSET object contains plenty of information for one Buletooth device.
                     * The name of the device is included, along with its address, and an object further describing its
                     * characteristics.  From this information, we'll build a managed BluetoothDevice class for GPS.NET.
                     */

                    // Now get a pointer to the device information
                    NativeMethods2.Blob resultBlob = new NativeMethods2.Blob();

                    // Is there no blob?
                    if (result.EntityBlob == IntPtr.Zero)
                        continue;

                    // Deserialize the blob into delicious cake
                    Marshal.PtrToStructure(result.EntityBlob, resultBlob);

                    // The blob points to a BLUETOOTH_DEVICE_INFO object
                    NativeMethods2.BthDeviceInfo deviceInfo = new NativeMethods2.BthDeviceInfo();

                    // Deserialize the object
                    Marshal.PtrToStructure(resultBlob.Info, deviceInfo);

                    /* Yay!  We now have the device name, address, and "classification"  (what kind of device it is).
                     * The next step is to discover services provided by the device.
                     */

                    // Do we already have this device recorded?
                    bool alreadyExists = false;
                    foreach (BluetoothDevice existing in Devices.BluetoothDevices)
                    {
                        if (existing.Address.Equals(deviceInfo.Address))
                            alreadyExists = true;
                    }

                    // If it already exists, continue
                    if (alreadyExists)
                        continue;

                    // Create a device from this information
                    BluetoothDevice device = deviceInfo.ToDevice();

                    // Now,  populate the object with information
                    device.Refresh();

                    // Raise en event signaling the detection
                    if (DeviceDiscovered != null)
                        DeviceDiscovered(null, new DeviceEventArgs(device));

                    #endregion Decode the device information
                }

                #endregion Process search results
            }
            catch (ThreadAbortException)
            {
                Debug.WriteLine("Bluetooth device discovery has been canceled", Devices.DEBUG_CATEGORY);

                // Abort immediately
                return;
            }
            catch
            {
                Debug.WriteLine("Bluetooth device discovery failed", Devices.DEBUG_CATEGORY);

                // A legitimate error occurred.  This probably means that the Bluetooth stack isn't supported.
                return;
            }
            finally
            {
                /* Since we deal a lot with unmanaged memory, it's critical that this memory gets de-allocated, even
                 * in the event of a ThreadAbortException.  As a result, we'll examine and de-allocate memory as necessary here.
                 */

                // Free up pinned objects
                optionsHandle.Free();

                blobHandle.Free();

                queryResultHandle.Free();

                // If a query was in progress, end it
                if (queryHandle != IntPtr.Zero)
                    NativeMethods2.WSALookupServiceEnd(queryHandle);

                // Finally, finish with stuffs
                NativeMethods2.WSACleanup();

                // And signal we're done
                _isDeviceDiscoveryInProgress = false;

                Debug.WriteLine("Bluetooth device discovery has completed", Devices.DEBUG_CATEGORY);
            }
        }

        #endregion Static Methods

        #region Internal Methods

#if !PocketPC

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        [SecuritySafeCritical]
        internal void Refresh()
        {
            /* Attempt to populate information about this device, such as the name, and
             * whether the device is paired, connected, remembered, etc.
             */

            // Do we have a Bluetooth radio to work with?
            if (BluetoothRadio.Current == null)
                return;

            // Make an API call to retrieve information for the device
            NativeMethods2.BluetoothDeviceInfo info = new NativeMethods2.BluetoothDeviceInfo(_address.ToInt64());
            int errorCode = NativeMethods2.BluetoothGetDeviceInfo(BluetoothRadio.Current.Handle, ref info);
            if (errorCode != 0)
            {
                // And error code of "1168" means the device was not found.  It just means that
                // the device is unknown by the system and no info is available.
                if (errorCode == 1168)
                    return;

                throw new Win32Exception(errorCode);
            }

            /* FxCop says this is unused
             *
            // Is this device authenticated?  (Meaning the PIN code is supplied)
            _IsAuthenticated = info.fAuthenticated;

            // Is the device currently connected?
            _IsConnected = info.fConnected;

            // Is this device "remembered"?  This means the device information was cached from a previous detection
            _IsRemembered = info.fRemembered;
             */

            // If the date/time the device was last used exists, use it
            if (info.LastUsed.Year != 0)
                SetDateConnected(info.LastUsed.ToDateTime());

            // Was a friendly name returned?  If so, use it
            if (!String.IsNullOrEmpty(info.Name))
                _name = info.Name;

            // Some additional shiz; the class and sub-class for the device.  GPS is surprisingly unorganized here!
            // The class will typically be "Miscellaneous" for BT GPS devices :P
            _class = (DeviceClass)(info.DeviceClass & 0x1F00);
            _minorClass = (DeviceClass)(info.DeviceClass & 0xFC);
            _serviceClass = (ServiceClass)(info.DeviceClass & 0xFFE000);

            // Save everything to the registry
            OnCacheWrite();
        }

#else
        // Deserializes an int into class and service information
        internal void SetClassOfDevice(uint classOfDevice)
        {
            _Class = (DeviceClass)(classOfDevice & 0x1F00);
            _MinorClass = (DeviceClass)(classOfDevice & 0xFC);
            _ServiceClass = (ServiceClass)(classOfDevice & 0xFFE000);
        }
#endif

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Loads the Bluetooth devices that have been cached by GPS.Net. This list contains previously-detected GPS devices,
        /// along with devices which were tested but found to NOT be GPS devices. By keeping these statistics,
        /// the detection system can become faster over time by first testing devices which have a better success rate.
        /// </summary>
        /// <param name="devices">The list to which the cached devices are added.</param>
        private static void LoadCachedDevices(IList<BluetoothDevice> devices)
        {
            RegistryKey bluetoothDevicesKey = Registry.LocalMachine.OpenSubKey(MY_ROOT_KEY_NAME, false);

            // Anything to do?
            if (bluetoothDevicesKey != null)
            {
                // Yes.  Enumerate the sub-keys
                string[] deviceKeys = bluetoothDevicesKey.GetSubKeyNames();
                foreach (string addressValue in deviceKeys)
                {
                    // This value is a Bluetooth socket address
                    BluetoothAddress address = BluetoothAddress.Parse(addressValue);

                    // Finally, create a device from this information
                    BluetoothDevice device = new BluetoothDevice(address);

                    // And add it
                    devices.Add(device);
                }

                // Finally, close the key
                bluetoothDevicesKey.Close();
            }
        }

        /// <summary>
        /// Loads the Bluetooth devices that have already been discovered by Windows. This list includes non-GPS devices.
        /// </summary>
        /// <param name="devices">The list to which the devices are added.</param>
        private static void LoadWindowsDevices(IList<BluetoothDevice> devices)
        {
#if !PocketPC
            RegistryKey existingDevicesKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices", false);
            if (existingDevicesKey != null)
            {
                // Each subkey is a device
                foreach (string subKeyName in existingDevicesKey.GetSubKeyNames())
                {
                    // The subkey name is an address
                    int count = subKeyName.Length / 2;

                    // The address is pairs of hex numbers
                    byte[] addressBytes = new byte[count];
                    for (int index = 0; index < count; index++)
                    {
                        addressBytes[count - index - 1] = Convert.ToByte(subKeyName.Substring(index * 2, 2), 16);
                    }

                    // Make a new device object
                    BluetoothDevice device = new BluetoothDevice(new BluetoothAddress(addressBytes));

                    // If it's not already in the list, add it
                    if (!devices.Contains(device))
                        devices.Add(device);
                }

                // Close the key
                existingDevicesKey.Close();
            }
#endif
        }

        #endregion Private Methods

        #region IEquatable<BluetoothDevice> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(BluetoothDevice other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return _address.Equals(other.Address);
        }

        #endregion IEquatable<BluetoothDevice> Members

        #region Unused Code (Commented Out)

        //public override void Close()
        //{
        //    lock (BluetoothStackSyncRoot)
        //    {
        //        base.Close();
        //    }
        //}

        #endregion Unused Code (Commented Out)
    }

    /// <summary>
    /// Indicates the classification of a Bluetooth device.
    /// </summary>
    public enum DeviceClass
    {
        /// <summary>
        /// Misc
        /// </summary>
        Miscellaneous = 0x000,
        /// <summary>
        /// Computer
        /// </summary>
        Computer = 0x100,
        /// <summary>
        /// Phone
        /// </summary>
        Phone = 0x200,
        /// <summary>
        /// Lan
        /// </summary>
        LAN = 0x300,
        /// <summary>
        /// Audio
        /// </summary>
        AudioVideo = 0x400,
        /// <summary>
        /// Peripheral
        /// </summary>
        Peripheral = 0x500,
        /// <summary>
        /// Imaging
        /// </summary>
        Imaging = 0x600,
        /// <summary>
        /// Unclassified
        /// </summary>
        Unclassified = 0x1F00
    }

    /// <summary>
    /// Indicates the kind of service provided by a Bluetooth device.
    /// </summary>
    public enum ServiceClass
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Limited Disoverable mode
        /// </summary>
        LimitedDiscoverableMode = 0x2000,
        /// <summary>
        /// Positioning
        /// </summary>
        Positioning = 0x10000,
        /// <summary>
        /// Networking
        /// </summary>
        Networking = 0x20000,
        /// <summary>
        /// Rendering
        /// </summary>
        Rendering = 0x40000,
        /// <summary>
        /// Capturing
        /// </summary>
        Capturing = 0x80000,
        /// <summary>
        /// Object Transfer
        /// </summary>
        ObjectTransfer = 0x100000,
        /// <summary>
        /// Audio
        /// </summary>
        Audio = 0x200000,
        /// <summary>
        /// Telephony
        /// </summary>
        Telephony = 0x400000,
        /// <summary>
        /// Information
        /// </summary>
        Information = 0x800000,
        /// <summary>
        /// all
        /// </summary>
        All = 0xFFE000
    }
}