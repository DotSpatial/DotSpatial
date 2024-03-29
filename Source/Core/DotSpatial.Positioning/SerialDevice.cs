﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a serial (RS-232) device.
    /// </summary>
    /// <remarks><para>This class is used to connect to any device using the RS-232 protocol.  Desktop computers will typically
    /// have at least one serial port, and in some cases a "virtual" serial port is created to make a device emulate RS-232.  For
    /// example, since there is no USB standard for GPS devices, USB GPS device manufacturers typically provide a special "USB-to-serial"
    /// driver to make the device available for third-party applications.</para>
    ///   <para>Each serial port on a computer has a unique name, typically beginning with "COM" followed by a number, then a colon
    /// (e.g. COM5:).  In some special circumstances, such as the GPS Intermediate Driver on Windows Mobile devices, a different prefix
    /// is used.  Each serial port includes other configuration settings, most importantly the baud rate, which controls the speed of
    /// communications.  GPS device manufacturers must support 4800 baud in order to comply with the NMEA-0183 standard, but many newer devices
    /// use faster speeds.  The baud rate of a connection must be specified precisely, otherwise all data from the device will be
    /// unrecognizable.</para>
    ///   <para>Other settings for serial ports are the data bits, stop bits, and parity.  In the context of GPS, a vast majority of GPS
    /// devices use eight data bits, one stop bit, and no parity.  these settings are used if no settings are explicitly provided.</para></remarks>
    public class SerialDevice : Device, IEquatable<SerialDevice>
    {
        /// <summary>
        ///
        /// </summary>
        private SerialPort _port;
        /// <summary>
        ///
        /// </summary>
        private int _lastSuccessfulBaudRate;
        /// <summary>
        ///
        /// </summary>
        private string _name;

        #region Constants

        /// <summary>
        ///
        /// </summary>
        private const string ROOT_KEY_NAME = Devices.ROOT_KEY_NAME + @"Serial\";

        #endregion Constants

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public SerialDevice()
        {
            _port = new SerialPort();
        }

        /// <summary>
        /// Creates a new instance using the specified port.
        /// </summary>
        /// <param name="portName">Name of the port.</param>
        public SerialDevice(string portName)
            : this(portName, 4800)
        { }

        /// <summary>
        /// Creates a new instance using the specified port name and baud rate.
        /// </summary>
        /// <param name="portName">Name of the port.</param>
        /// <param name="baudRate">The baud rate.</param>
        public SerialDevice(string portName, int baudRate)
        {
            _port = new SerialPort(portName, baudRate);

            // Default to the port name for the friendly name
            _name = portName;

            // Read the cache
            OnCacheRead();
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the name of the port used to open a connection.
        /// </summary>
        /// <value>The port.</value>
        [Category("Data")]
        [Description("Returns the name of the port used to open a connection.")]
        [Browsable(true)]
        [ParenthesizePropertyName(true)]
        public virtual string Port
        {
            get => _port.PortName;
            set => _port.PortName = value;
        }

        /// <summary>
        /// Returns the numeric portion of the port name.
        /// </summary>
        [Category("Data")]
        [Description("Returns the numeric portion of the port name.")]
        [Browsable(true)]
        public int PortNumber
        {
            get
            {
                StringBuilder numericPortion = new(4);

                // Extract numeric digits from the name
                foreach (char character in _port.PortName)
                {
                    if (char.IsNumber(character))
                    {
                        numericPortion.Append(character);
                    }
                }

                // Did we find any numeric digits?
                if (numericPortion.Length > 0)
                {
                    // Yes.  Extract the number
                    return int.Parse(numericPortion.ToString());
                }

                // No.  Return -1 (not zero, since that's a valid port number)
                return -1;
            }
        }

        /// <summary>
        /// Controls the speed of communications for this device.
        /// </summary>
        /// <value>The baud rate.</value>
        /// <remarks>This property controls the speed of read and write operations for the device.  The baud rate specified must precisely
        /// match a rate supported by the device, otherwise data will be unrecognizable.  GPS devices are required to support a minimum baud rate of 4800
        /// in order to comply with the NMEA-0183 standard (though not all devices do this).  A higher rate is preferable.</remarks>
        [Category("Data")]
        [Description("Controls the speed of communications for this device.")]
        [Browsable(true)]
        public int BaudRate
        {
            get => _port.BaudRate;
            set => _port.BaudRate = value;
        }

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Returns a natural language name for the device.
        /// </summary>
        /// <inheritdocs/>
        public override string Name => _name;

        /// <summary>
        /// Controls whether the device can be queried for GPS data.
        /// </summary>
        /// <value><c>true</c> if [allow connections]; otherwise, <c>false</c>.</value>
        /// <inheritdocs/>
        public override bool AllowConnections
        {
            get => base.AllowConnections && Devices.AllowSerialConnections;
            set => base.AllowConnections = value;
        }

        /// <summary>
        /// Forces a device to a closed state without disposing the underlying stream.
        /// </summary>
        /// <inheritdocs/>
        public override void Reset()
        {
            // Clear out the stream
            base.Reset();

            // Clone the port
            SerialPort clone = new(_port.PortName, _port.BaudRate, _port.Parity, _port.DataBits, _port.StopBits)
            {
                ReadTimeout = _port.ReadTimeout,
                WriteTimeout = _port.WriteTimeout,
                NewLine = _port.NewLine,
                WriteBufferSize = _port.WriteBufferSize,
                ReadBufferSize = _port.ReadBufferSize,
                ReceivedBytesThreshold = _port.ReceivedBytesThreshold,
                Encoding = _port.Encoding
            };

            // Close and dispose the current reference
            if (_port.IsOpen)
            {
                _port.Close();
            }

            _port.Dispose();

            // Use this new reference.
            _port = clone;
        }

        /// <summary>
        /// Creates a new Stream object for the device.
        /// </summary>
        /// <param name="access">The access.</param>
        /// <param name="sharing">The sharing.</param>
        /// <returns>A <strong>Stream</strong> object.</returns>
        /// <inheritdocs/>
        protected override Stream OpenStream(FileAccess access, FileShare sharing)
        {
            // Open the port if it's not already open
            if (!_port.IsOpen)
            {
                _port.Open();
            }

            return _port.BaseStream;
        }

        /// <summary>
        /// Occurs when an open connection is about to be closed.
        /// </summary>
        /// <inheritdocs/>
        protected override void OnDisconnecting()
        {
            // Close the port if it's open
            if (_port != null && _port.IsOpen)
            {
                _port.Close();
            }

            // And continue
            base.OnDisconnecting();
        }

        /// <summary>
        /// Performs a test on the device to confirm that it transmits GPS data.
        /// </summary>
        /// <returns>A <strong>Boolean</strong> value, <strong>True</strong> if the device is confirmed.</returns>
        /// <inheritdocs/>
        protected override bool DetectProtocol()
        {
            // If no connections are allowed, exit
            if (!AllowConnections)
            {
                Debug.WriteLine(Name + " will not be tested because connections are disabled", Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(
                    new DeviceDetectionException(this, Name + " is excluded from testing."));
                return false;
            }

            // Is it a Bluetooth serial port?
            if (!Devices.AllowBluetoothConnections
                && Name.IndexOf("Bluetooth", StringComparison.OrdinalIgnoreCase) != -1)
            {
                Debug.WriteLine(Name + " will not be tested because Bluetooth connections are disabled", Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(
                   new DeviceDetectionException(this, Name + " will not be tested because Bluetooth devices are currently excluded."));
                return false;
            }

            // Is it Bluetooth but Bluetooth is turned off?
            if (Devices.AllowBluetoothConnections
                && Name.IndexOf("Bluetooth", StringComparison.OrdinalIgnoreCase) != -1
                && !Devices.IsBluetoothEnabled)
            {
                Debug.WriteLine(Name + " will not be tested because Bluetooth is turned off", Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(
                   new DeviceDetectionException(this, Name + " will not be tested because Bluetooth is turned off."));
                return false;
            }

            // Have we reached the maximum allowed failures?
            if (SuccessfulDetectionCount == 0 && MaximumAllowedFailures > 0 && FailedDetectionCount > MaximumAllowedFailures)
            {
                Debug.WriteLine(Name + " will not be tested because it has failed over " + MaximumAllowedFailures + " times", Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(
                   new DeviceDetectionException(this, Name + " will not be tested because it has failed detection over " + MaximumAllowedFailures.ToString(CultureInfo.CurrentCulture) + " times with no success."));
                return false;
            }

            /* The first step here is to detect the baud rate.  Since NMEA
             * devices transmit only ASCII characters, we can keep reading bytes
             * until a few ASCII characters are received.
             */

            try
            {
                // Open a connection
                Open();
            }
            catch (InvalidOperationException)
            {
                // According to MSDN docs, this means the port is already open!  So, continue.
                Debug.WriteLine(Name + " appears to already be open", Devices.DEBUG_CATEGORY);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.WriteLine(Name + " could not be opened due to the following error: " + ex, Devices.DEBUG_CATEGORY);

                // One of the parameters of the port is incorrect.  Flag the error
                Devices.OnDeviceDetectionAttemptFailed(
                    new DeviceDetectionException(this, Name + " could not be opened because of an invalid parameter.  GPS connections should be using at least a 4800, 8, N, 1, None connection to work properly.", ex));
                return false;
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine(Name + " could not be opened due to the following error: " + ex, Devices.DEBUG_CATEGORY);

                // The port name doesn't begin with "COM" or the file type of the port is not supported.
                Devices.OnDeviceDetectionAttemptFailed(
                    new DeviceDetectionException(this, Name + " does not appear to exist.", ex));
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine(Name + " could not be opened due to the following error: " + ex, Devices.DEBUG_CATEGORY);

                if (!Name.Contains("Bluetooth", StringComparison.OrdinalIgnoreCase))
                {
                    // UnauthorizedAccessException (can happen if multiple apps open the same port), if the device is actually a USB-mapped-to-serial
                    Devices.OnDeviceDetectionAttemptFailed(new DeviceDetectionException(this, ex));
                }
                else
                {
                    // Security may have denied the connection
                    Devices.OnDeviceDetectionAttemptFailed(new DeviceDetectionException(this, "A security PIN entered for " + Name + " was incorrect.  The device should be re-paired in the Bluetooth Manager.", ex));
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(Name + " could not be opened due to the following error: " + ex, Devices.DEBUG_CATEGORY);
                Devices.OnDeviceDetectionAttemptFailed(new DeviceDetectionException(this, ex));
                return false;
            }

            /* If we get here, the serial port is successfully opened. */

            /* We have a list of baud rates to test.  We can speed up this test, however, if
             * we know the "last successful baud rate," the baud rate of the port when it last
             * sent NMEA data.  So, if we know this value, test it FIRST to dramatically improve
             * speed.
             */

            List<int> baudRatesToTest = new(DetectionBaudRates);

            // Do we have the last known rate?
            if (_lastSuccessfulBaudRate != 0)
            {
                // Remove it from the list
                baudRatesToTest.Remove(_lastSuccessfulBaudRate);
                // And insert it at the beginning
                baudRatesToTest.Insert(0, _lastSuccessfulBaudRate);
            }

            /* For the purposes of detection, we should be getting data immediately.
             * As a result, we can use aggressive timeouts here.  Only a tiny handful of devices
             * need longer than two seconds to start transmitting data.
             */
            _port.ReadTimeout = 1000;

            // Loop through all baud rates to test
            int baudCount = baudRatesToTest.Count;
            for (int index = 0; index < baudCount; index++)
            {
                // Set the port baud rate
                _port.BaudRate = baudRatesToTest[index];

                // Clear any old data
                _port.DiscardInBuffer();

                // Read a new buffer
                byte[] buffer = new byte[NmeaReader.IDEAL_NMEA_BUFFER_SIZE];
                int bytesRead = 0;

                try
                {
                    // Read the buffer in
                    while (bytesRead < NmeaReader.IDEAL_NMEA_BUFFER_SIZE)
                    {
                        int read = BaseStream.Read(buffer, bytesRead, NmeaReader.IDEAL_NMEA_BUFFER_SIZE - bytesRead);
                        if (read == 0)
                        {
                            break;
                        }

                        bytesRead += read;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    // The port is not open!  Probably closed?
                    try
                    {
                        // Try opening it again.
                        Open();
                    }
                    catch
                    {
                        Debug.WriteLine(Name + " could not be opened due to the following error: " + ex, Devices.DEBUG_CATEGORY);

                        // Failure.  Abort all testing.
                        Devices.OnDeviceDetectionAttemptFailed(
                            new DeviceDetectionException(this, Name + " could not be opened.", ex));
                        return false;
                    }
                }
                catch (TimeoutException)
                {
                    // No love at this baud rate.  Step down to a lower speed
                    Debug.WriteLine(Name + " could not be opened at " + baudRatesToTest[index] + " baud", Devices.DEBUG_CATEGORY);
                    continue;
                }

                // Analyze the buffer for contiguous ASCII data
                int contiguousAsciiCharacters = 0;
                for (int count = 0; count < bytesRead; count++)
                {
                    // Get the byte
                    byte testByte = buffer[count];
                    if (testByte == 0)
                    {
                        continue;
                    }

                    // Is this byte outside of the range of ASCII characters?
                    if (testByte is < 10 or > 125)
                    {
                        // Yes.  Flag this as unlikely to be ASCII (or NMEA)
                        contiguousAsciiCharacters = 0;
                    }
                    else
                    {
                        /* This is ASCII!  How many of these do we have in a row?
                         * If we're at the correct baud rate, we'll get a string of 'em.
                         * Sometimes, we'll get a blarp of non-ASCII followed by regular
                         * ASCII.  And yes, 'blarp' is now a word meaning "garbage data
                         * which can be safely ignored".
                         */
                        contiguousAsciiCharacters++;
                    }
                }

                // If we don't have a good set of ASCII, try another baud rate
                if (contiguousAsciiCharacters < 10)
                {
                    continue;
                }

                // This is a good sign that we have NMEA.  Bump up our thread priority
                Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

                // We have ASCII.  Try up to 10 times to get a full sentences.
                for (int count = 0; count < 10; count++)
                {
                    // Read a line
                    string testLine;
                    try
                    {
                        testLine = _port.ReadLine();
                    }
                    catch (TimeoutException ex)
                    {
                        Debug.WriteLine(Name + " did not respond to an attempt to read data", Devices.DEBUG_CATEGORY);
                        Devices.OnDeviceDetectionAttemptFailed(
                            new DeviceDetectionException(this, Name + " did not respond to an attempt to read data.", ex));
                        return false;
                    }

                    if (
                        // Does it begin with a dollar sign?
                        testLine.StartsWith("$", StringComparison.OrdinalIgnoreCase)
                        // Is there an asterisk before that last two characters?
                        && testLine.IndexOf("*", StringComparison.OrdinalIgnoreCase) == testLine.Length - 3)
                    {
                        // Yes!  This is an NMEA device.

                        // Set the "last successful" baud rate
                        _lastSuccessfulBaudRate = _port.BaudRate;

                        return true;
                    }
                }
            }

            /* The device did not return any detectable data. */
            Debug.WriteLine(Name + " was tested at multiple baud rates, but no GPS data was found", Devices.DEBUG_CATEGORY);
            Devices.OnDeviceDetectionAttemptFailed(
                new DeviceDetectionException(this, Name + " was tested at multiple baud rates but no GPS data was found."));
            return false;
        }

        /// <summary>
        /// Stops any GPS protocol detection in progress.
        /// </summary>
        public override void CancelDetection()
        {
            if (IsDetectionInProgress)
            {
                if (_port != null && _port.IsOpen)
                {
                    _port.Close();
                }
            }

            // Continue to abort the thread
            base.CancelDetection();
        }

        /// <summary>
        /// Disposes of any unmanaged (or optionally, managed) resources used by the device.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_port != null)
                {
                    if (_port.IsOpen)
                    {
                        _port.Close();
                    }

                    _port.Dispose();
                    _port = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Records information about this device to the registry.
        /// </summary>
        protected override void OnCacheWrite()
        {
            // If we don't have a port name, then we can't get the registry key
            if (_port == null)
            {
                return;
            }

            // Save device stats
            RegistryKey deviceKey = null;
            try
            {
                deviceKey = Registry.LocalMachine.CreateSubKey(ROOT_KEY_NAME + Port);

                if (deviceKey != null)
                {
                    // Set the friendly name
                    if (!string.IsNullOrEmpty(_name))
                    {
                        deviceKey.SetValue(DEFAULT_REGISTRY_VALUE_NAME, _name);
                    }

                    // Update the baud rate and etc.
                    deviceKey.SetValue("Baud Rate", _port.BaudRate);

                    // Update the success/fail statistics
                    deviceKey.SetValue("Number of Times Detected", SuccessfulDetectionCount);
                    deviceKey.SetValue("Number of Times Failed", FailedDetectionCount);
                    deviceKey.SetValue("Date Last Detected", DateDetected.ToString("G", CultureInfo.InvariantCulture));
                    deviceKey.SetValue("Date Last Connected", DateConnected.ToString("G", CultureInfo.InvariantCulture));

                    // Remember the baud rate if it's not zero
                    if (_lastSuccessfulBaudRate != 0)
                    {
                        deviceKey.SetValue("Last Successful Baud Rate", _lastSuccessfulBaudRate);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            { }
            finally
            {
                if (deviceKey != null)
                {
                    deviceKey.Close();
                }
            }
        }

        /// <summary>
        /// Removes previously cached information for this device from the registry.
        /// </summary>
        protected override void OnCacheRemove()
        {
            // If we don't have a port name, then we can't get the registry key
            if (_port == null)
            {
                return;
            }

            try
            {
                // Delete the entire key for this port
                Registry.LocalMachine.DeleteSubKeyTree(ROOT_KEY_NAME + Port);
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
            }
        }

        /// <summary>
        /// Reads information about this device from the registry.
        /// </summary>
        protected sealed override void OnCacheRead()
        {
            // If we don't have a port name, then we can't get the registry key
            if (_port == null)
            {
                return;
            }

            // Save device stats
            RegistryKey deviceKey = Registry.LocalMachine.OpenSubKey(ROOT_KEY_NAME + Port, false);

            if (deviceKey == null)
            {
                return;
            }

            // Update the baud rate and etc.
            foreach (string name in deviceKey.GetValueNames())
            {
                switch (name)
                {
                    case DEFAULT_REGISTRY_VALUE_NAME:
                        _name = Convert.ToString(deviceKey.GetValue(name));
                        break;
                    case "Baud Rate":
                        _port.BaudRate = Convert.ToInt32(deviceKey.GetValue(name), CultureInfo.InvariantCulture);
                        break;
                    case "Number of Times Detected":
                        SetSuccessfulDetectionCount(Convert.ToInt32(deviceKey.GetValue(name), CultureInfo.InvariantCulture));
                        break;
                    case "Number of Times Failed":
                        SetFailedDetectionCount(Convert.ToInt32(deviceKey.GetValue(name), CultureInfo.InvariantCulture));
                        break;
                    case "Date Last Detected":
                        SetDateDetected(Convert.ToDateTime(deviceKey.GetValue(name), CultureInfo.InvariantCulture));
                        break;
                    case "Date Last Connected":
                        SetDateConnected(Convert.ToDateTime(deviceKey.GetValue(name), CultureInfo.InvariantCulture));
                        break;
                    case "Last Successful Baud Rate":
                        _lastSuccessfulBaudRate = Convert.ToInt32(deviceKey.GetValue(name), CultureInfo.InvariantCulture);
                        break;
                }
            }
        }

        #endregion Overrides

        #region Static Properties

        /// <summary>
        /// Controls the baud rates tested during GPS protocol detection.
        /// </summary>
        /// <remarks><para>A GPS device may support any number of baud rates.  As a result, the GPS.NET device detection system
        /// needs to test multiple baud rates in order to find a match.  This collection controls the list of baud rates
        /// tested during detection.</para>
        ///   <para>Advanced GPS developers can improve performance by trimming this list down to baud rates which will actually
        /// be used.</para></remarks>
        public static IList<int> DetectionBaudRates { get; } = new List<int>(new[] { 115200, 57600, 38400, 19200, 9600, 4800 });

        /// <summary>
        /// Controls the maximum allowed detection failures before a device is excluded from detection.
        /// </summary>
        /// <value>The maximum allowed failures.</value>
        /// <remarks>Some devices involved with device detection are not GPS devices.  For example, a serial device search
        /// could return a bar code scanner, an infrared port, or the neighbor's computer.  This property controls how many times a device will be
        /// tested before it is no longer included for searches.  If a device's failure count goes beyond this number, attempts
        /// will no longer be made to connect to the device.</remarks>
        public static int MaximumAllowedFailures { get; set; } = 100;

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public static void ClearCache()
        {
            // Remove the entire branch of cached devices
        }

        /// <summary>
        /// Returns a list of known Bluetooth devices.
        /// </summary>
        /// <returns></returns>
        /// <remarks><para>To maximize performance, GPS.NET will record information about each device it encounters for the purposes of organizing
        /// and speeding up the GPS device detection process.  This property returns a list of all known serial devices.</para>
        ///   <para>Since this list is managed automatically, it should not be modified.</para></remarks>
        [SecurityCritical]
        public static IList<SerialDevice> GetCache()
        {
            List<SerialDevice> devices = new();

            // Pass 1: Look for devices recorded in the registry
            LoadCachedDevices(devices);

            // Pass 2: Add virtual serial ports for Bluetooth devices
            LoadVirtualDevices(devices);

            // Pass 3: Look for devices already detected by Windows
            LoadWindowsDevices(devices);

            /* Sort the list based on the most reliable device first.
                * Device detection will execute in this order.
                */
            devices.Sort(BestDeviceComparer);

            // Return the results
            return devices;
        }

        #endregion Static Properties

        #region Internal Methods

        /// <summary>
        /// Sets the name.
        /// </summary>
        /// <param name="name">The name.</param>
        internal void SetName(string name)
        {
            _name = name;
        }

        #endregion Internal Methods

        #region Private Methods

        /// <summary>
        /// Loads the serial devices that have been cached by GPS.Net. This list contains previously-detected GPS devices,
        /// along with devices which were tested but found to NOT be GPS devices. By keeping these statistics,
        /// the detection system can become faster over time by first testing devices which have a better success rate.
        /// </summary>
        /// <param name="devices">The list to which the cached devices are added.</param>
        private static void LoadCachedDevices(IList<SerialDevice> devices)
        {
            RegistryKey serialDevicesKey = Registry.LocalMachine.OpenSubKey(ROOT_KEY_NAME, false);

            // Anything to do?
            if (serialDevicesKey != null)
            {
                // Yes.  Enumerate the sub-keys
                string[] deviceKeys = serialDevicesKey.GetSubKeyNames();
                foreach (string portName in deviceKeys)
                {
                    // Finally, create a device from this information
                    SerialDevice device = new(portName);

                    // And add it
                    devices.Add(device);
                }

                // Finally, close the key
                serialDevicesKey.Close();
            }
        }

        /// <summary>
        /// Loads any virtual serial devices that exist for other types of physical devices.
        /// This list includes non-GPS devices.
        /// </summary>
        /// <param name="devices">The list to which the cached devices are added.</param>
        [SecurityCritical]
        private static void LoadVirtualDevices(IList<SerialDevice> devices)
        {
            IList<BluetoothDevice> cache = Devices.BluetoothDevices;
            foreach (BluetoothDevice device in cache)
            {
                // Does this Bluetooth device have a serial port?
                SerialDevice virtualDevice = device.VirtualSerialPort;
                if (virtualDevice != null)
                {
                    if (!devices.Contains(virtualDevice))
                    {
                        devices.Add(virtualDevice);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the serial devices that have already been detected by Windows. This list includes non-GPS devices.
        /// </summary>
        /// <param name="devices">The list to which the devices are added.</param>
        private static void LoadWindowsDevices(IList<SerialDevice> devices)
        {
            // Open HKLM\HARDWARE\DEVICEMAP\SERIALCOMM
            RegistryKey portsKey = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM", false);
            if (portsKey != null)
            {
                // Get a list of keys underneath this one
                string[] portKeys = portsKey.GetValueNames();

                #region Pass #1: Analysis of actual ports

                int count = portKeys.Length;
                for (int index = 0; index < count; index++)
                {
                    // Get the name of the registry key for this device
                    string portKey = portKeys[index];

                    // Is this device a Bluetooth virtual serial port?
                    if (portKey.Contains(@"\BthModem"))
                    {
                        /* Yes.  We already have a way to associate BluetoothDevice objects
                             * with SerialDevice objects, so just skip this.
                             */
                        continue;
                    }

                    /* A bug in the Microsoft Bluetooth stack causes the registry value to have an extra
                         * byte, which mucks shiz up.  So, in order to scrub those out, we must use ASCII encoding
                         * to convert the string to bytes and back.  This causes any garbage chars to become "?"
                         * which is never valid for a COM: port name.
                         */

                    string portName = (string)portsKey.GetValue(portKey);

                    // Convert the ASCII bytes to a string
                    portName =
                        Encoding.ASCII.GetString(
                            // Convert the registry value to ASCII bytes
                            Encoding.ASCII.GetBytes(portName))
                            // Lastly, remove "?" characters
                            .Replace("?", string.Empty);

                    // Lastly, append a colon
                    portName += ":";

                    // Make a device with the port name
                    SerialDevice device = new(portName);

                    // Add it to the results
                    if (!devices.Contains(device))
                    {
                        devices.Add(device);
                    }
                }

                // Finally, clean up
                portsKey.Close();

                #endregion Pass #1: Analysis of actual ports
            }
        }

        #endregion Private Methods

        #region IEquatable<SerialDevice> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(SerialDevice other)
        {
            return other is not null && Port.Equals(other.Port, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="obj"/> parameter; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as SerialDevice);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion IEquatable<SerialDevice> Members
    }
}