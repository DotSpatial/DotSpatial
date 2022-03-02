using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GeoFramework.Gps.IO;

namespace GeoFramework.Gps
{
    /// <summary>
    /// Performs an analysis of GPS devices and possible problems on the host machine.
    /// </summary>
    /// <remarks><para>During the debugging process, developers frequently encounter bugs and connectivity problems.  However, it
    /// can sometimes be time-consuming to determine whether the problem lies in their code, or in a faulty GPS device connection.
    /// This class will plug into the GPS.NET device detection system and output a detailed analysis of results to the Output
    /// window.  If any problems are detected, they will be listed along with suggestions on how to resolve the problem.
    /// This class is meant to be used during debugging, but it can also serve as a troubleshooting log for your customers.</para>
    /// <para>You are also welcome to email the results of this log to GeoFrameworks.  Simply include information about your host device
    /// and operating system (e.g. "Samsung Omina, Windows Mobile 6") and we can use the log to identify unusual behavior and improve GPS.NET
    /// for your particular devices.  Email your logs to "support@geoframeworks.com."  However, do NOT automate this process; people
    /// who send too many log files will be blocked by our system.</para></remarks>
    public static class Diagnostics
    {
        private static bool _IsStarted;
        private static DateTime _DateStarted;
        private static TextWriter _Output = null;

        #region Constructors

        static Diagnostics()
        {
            // Hook into device detection events
            Devices.DeviceDetectionStarted += new EventHandler(Devices_DeviceDetectionStarted);
            Devices.DeviceDetectionCompleted += new EventHandler(Devices_DeviceDetectionCompleted);
            Devices.DeviceDetectionCanceled += new EventHandler(Devices_DeviceDetectionCanceled);
            Devices.DeviceDetected += new EventHandler<DeviceEventArgs>(Devices_DeviceDetected);
            Devices.DeviceDetectionAttempted += new EventHandler<DeviceEventArgs>(Devices_DeviceDetectionAttempted);
            Devices.DeviceDetectionAttemptFailed += new EventHandler<DeviceDetectionExceptionEventArgs>(Devices_DeviceDetectionAttemptFailed);
        }

        #endregion

        #region Device Detection Events

        static void Devices_DeviceDetectionCanceled(object sender, EventArgs e)
        {
            Log("WARNING   " + DateTime.Now.ToLongTimeString() + ": Detection was canceled.");
            Log("");

            if (_Output != null)
            {
                _Output.Close();
#if Framework30
                _Output.Dispose();
#endif
            }
        }

        static void Devices_DeviceDetectionAttemptFailed(object sender, DeviceDetectionExceptionEventArgs e)
        {
            Log("WARNING   " + DateTime.Now.ToLongTimeString() + ": " + e.Device.Name + " has failed detection: " + e.Exception.Message);
        }

        static void Devices_DeviceDetectionAttempted(object sender, DeviceEventArgs e)
        {
            Log("          " + DateTime.Now.ToLongTimeString() + ": Testing " + e.Device.Name);
        }

        static void Devices_DeviceDetected(object sender, DeviceEventArgs e)
        {
            Log("FOUND     " + DateTime.Now.ToLongTimeString() + ": " + e.Device.Name + " is a GPS device.");
        }

        static void Devices_DeviceDetectionCompleted(object sender, EventArgs e)
        {
            // Calculate the elapsed time since detection started
            TimeSpan elapsedTime = DateTime.Now.Subtract(_DateStarted);
            Device _PreferredGpsDevice = null;
            SerialDevice _PreferredSerialGpsDevice = null;
            SerialDevice _PreferredVirtualSerialGpsDevice = null;

            // Was any device detected?
            if (Devices.GpsDevices.Count != 0)
            {
                // Is any device detected yet?  If not, remember it
                if (_PreferredGpsDevice == null)
                    _PreferredGpsDevice = Devices.GpsDevices[0];

                // Examine all devices
                int count = Devices.GpsDevices.Count;
                for(int index = 0; index < count; index++)
                {
                    // Get the current device
                    Device device = Devices.GpsDevices[index];

                    // Is this a serial device?
                    SerialDevice serialDevice = device as SerialDevice;
                    if (serialDevice != null)
                    {
                        // Yes. If we don't have a serial device yet, remember it
                        if (_PreferredSerialGpsDevice == null)
                            _PreferredSerialGpsDevice = serialDevice;
                    }

                    // Is this a Bluetooth device?
                    BluetoothDevice bluetoothDevice = device as BluetoothDevice;
                    if (bluetoothDevice != null)
                    {
                        // Yes.  Does it have a virtual serial port?
                        SerialDevice virtualDevice = bluetoothDevice.VirtualSerialPort;
                        if (virtualDevice != null)
                        {
                            // Yes.  Do we have a preferred virtual port yet?
                            if (_PreferredVirtualSerialGpsDevice == null)
                                _PreferredVirtualSerialGpsDevice = virtualDevice;
                        }
                    }
                }
            }

            Log("          " + DateTime.Now.ToLongTimeString() + ": Detection completed in " + elapsedTime.TotalSeconds + " seconds.");
            Log("");
            Log("C. SUMMARY");
            Log("");

            // Perform an analysis of the host machine
            if (Devices.GpsDevices.Count == 0)
            {
                Log("ERROR     No GPS devices were detected!");
            }
            else
            {
                if(Devices.GpsDevices.Count == 1)
                    Log("          " + Devices.GpsDevices.Count.ToString() + " GPS device is working properly.");
                else
                    Log("          " + Devices.GpsDevices.Count.ToString() + " GPS device are working properly.");

                // Notify of each properly functioning GPS device
                foreach (Device device in Devices.GpsDevices)
                {
                    Log("          " + device.Name + " is working properly and is " + device.Reliability.ToString() + " reliable.");

                    // Remember the first serial device we find (for later)
                    SerialDevice serialDevice = device as SerialDevice;
                    if (serialDevice != null)
                        _PreferredSerialGpsDevice = serialDevice;
                }

                // Do we have a GPS device, but no *serial* devices?
                if (_PreferredSerialGpsDevice == null)
                {
                    Log("WARNING   GPS.NET 3.0 is working properly, but third-party applications not using GPS.NET may not function because no serial GPS device is present.");
                }                
            }

            if (GpsIntermediateDriver.Current == null)
            {
                Log("WARNING   The GPS Intermediate Driver is not supported on this device.");
            }
            else
            {
                Log("          The GPS Intermediate Driver (GPSID) is supported on this device.");

                // Is it enabled?
                if (GpsIntermediateDriver.Current.AllowConnections)
                {
                    Log("          The GPS Intermediate Driver (GPSID) is enabled.");
                }
                else
                {
                    Log("WARNING   The GPS Intermediate Driver (GPSID) is turned off.");
                }

                // Did it get detected?
                if (GpsIntermediateDriver.Current.IsGpsDevice)
                {
                    Log("          The GPS Intermediate Driver (GPSID) is properly configured on " + GpsIntermediateDriver.Current.Port + ".");
                }
                else
                {
                    // The GPSID wasn't functioning.  Does it look properly configured?
                    if(GpsIntermediateDriver.Current.HardwarePort == null)
                    {
                        Log("WARNING   The GPS Intermediate Driver (GPSID) has no hardware port or baud rate specified.");
                    }
                    else
                    {
                        // Does the port match?
                        if (_PreferredSerialGpsDevice == null)
                        {
                            Log("WARNING   No serial GPS devices are available for the GPS Intermediate Driver (GPSID) to use.");
                        }
                        else
                        {
                            Log("WARNING   The GPS Intermediate Driver (GPSID) may be configured to use hardware port " + _PreferredSerialGpsDevice.Port + " at " + _PreferredSerialGpsDevice.BaudRate.ToString() + " baud.");
                        }
                    }
                }
            }

            // Show a list of alternative ports
            foreach (Device device in Devices.GpsDevices)
            {
                // Skip the GPSID, we want actual serial ports
                GpsIntermediateDriver gpsid = device as GpsIntermediateDriver;
                if(gpsid != null)
                    continue;

                SerialDevice serialDevice = device as SerialDevice;
                if (serialDevice == null)
                    continue;

                Log("          Programs can also be configured to use " + serialDevice.Port + " at " + serialDevice.BaudRate.ToString() + " baud.");
            }

            // Is Bluetooth supported?
            if (Devices.IsBluetoothSupported)
            {
                // Yes.
                Log("          The Microsoft® Bluetooth stack is installed.");

                // Is it turned on?
                if (Devices.IsBluetoothEnabled)
                {
                    // Yes.
                    Log("          Bluetooth is on and working properly.");
                }
                else
                {
                    // No
                    Log("WARNING   Bluetooth is turned off.");
                }
            }
            else
            {
                // There's either no stack or a stack from another provider
                Log("WARNING   The Microsoft® Bluetooth stack is not installed.");
            }


            Log("");

            if (_Output != null)
            {
                _Output.Close();
#if Framework30
                _Output.Dispose();
#endif
            }

            _IsStarted = false;
        }

        static void Devices_DeviceDetectionStarted(object sender, EventArgs e)
        {
            Log("");
            Log("GPS.NET 3.0 Diagnostics    Copyright © 2009  GeoFrameworks, LLC");

            switch (Environment.OSVersion.Version.Major)
            {
                case 4: // PocketPC 2003
                    Log("PocketPC 2003 version " + Environment.OSVersion.Version.ToString());
                    break;
                case 5: // Windows Mobile 
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                        case 1:
                            Log("Windows Mobile 5 version " + Environment.OSVersion.Version.ToString());
                            break;
                        case 2:
                            Log("Windows Mobile 6 version " + Environment.OSVersion.Version.ToString());
                            break;
                        case 3:
                            Log("Windows Mobile 7 version " + Environment.OSVersion.Version.ToString());
                            break;
                    }
                    break;
                default:
                    Log("(____?) version " + Environment.OSVersion.Version.ToString());
                    break;
            }

            Log("");
            Log("A. DEVICES TO TEST");

            int loggedDeviceCount = 0;
            GpsIntermediateDriver gpsid = GpsIntermediateDriver.Current;
            if (gpsid != null)
            {
                loggedDeviceCount++;
                Log("");
                Log("  1. GPS Intermediate Driver (GPSID)");
                Log("           Type: GPS Device Multiplexer");
                Log("           Program Port: " + gpsid.Port);
                if (gpsid.HardwarePort == null)
                    Log("WARNING    Hardware Port: (None or built-in GPS)");
                else
                    Log("           Hardware Port: " + gpsid.HardwarePort.Port + " at " + gpsid.HardwarePort.BaudRate.ToString() + " baud");
                if (gpsid.AllowConnections)
                    Log("           Enabled? YES");
                else
                    Log("WARNING    Enabled? NO");
            }

            foreach (SerialDevice device in SerialDevice.Cache)
            {
                loggedDeviceCount++;
                Log("");
                Log("  " + loggedDeviceCount.ToString() + ". " + device.Name);
                Log("           Type: Serial Device");
                Log("           Port: " + device.Port);
                Log("           Baud Rate: " + device.BaudRate.ToString());
                Log("           Reliability: " + device.Reliability.ToString());
                Log("           Successes: " + device.SuccessfulDetectionCount.ToString());
                Log("           Failures: " + device.FailedDetectionCount.ToString());
            }

            foreach (BluetoothDevice device in BluetoothDevice.Cache)
            {
                loggedDeviceCount++;
                Log("");
                Log("  " + loggedDeviceCount.ToString() + ". " + device.Name);
                Log("           Type: Bluetooth Device");
                Log("           Class: " + device.Class.ToString());
                Log("           Address: " + device.Address.ToString());
                Log("           Reliability: " + device.Reliability.ToString());
                Log("           Successes: " + device.SuccessfulDetectionCount.ToString());
                Log("           Failures: " + device.FailedDetectionCount.ToString());
            }

            Log("");
            Log("B. DEVICE DETECTION PROGRESS");
            Log("");
            Log("          " + DateTime.Now.ToLongTimeString() + ": Detection has started.");

            _DateStarted = DateTime.Now;
        }

        #endregion

        #region Static Methods

        public static void Run()
        {
            Run(null);
        }

        public static void Run(TextWriter output)
        {                  
            if (_IsStarted)
                return;

            _IsStarted = true;
            _Output = output;

            // Begin detection
            Devices.BeginDetection();
        }

#if ASDFASDFASF

        public static Device PreferredGpsDevice
        {
            get { return _PreferredGpsDevice; }
        }

        public static SerialDevice PreferredSerialGpsDevice
        {
            get { return _PreferredSerialGpsDevice; }
        }

        public static SerialDevice PreferredVirtualSerialGpsDevice
        {
            get { return _PreferredVirtualSerialGpsDevice; }
        }

        public static bool IsGpsIntermediateDriverProperlyConfigured
        {
            get
            {
                // Is the GPSID supported?  If not, exit
                if (GpsIntermediateDriver.Current == null)
                    return false;

                // Is there a source port?
                if (GpsIntermediateDriver.Current.HardwarePort == null)
                {
                    // No.  But is it working properly?
                    if (GpsIntermediateDriver.Current.IsGpsDevice)
                    {
                        /* Yes, this could be no issue.  For example, the Samsung Omina
                         * reports no source port, yet it works just fine.  It's important to
                         * NO be agressive when changing GPSID settings, because it might
                         * break something!
                         */
                        return true;
                    }

                    // At this point it's not a GPS device.  Indicate false
                    return false;
                }

                // Is the GPSID confirmed as a GPS device?  If so, show it
                if (GpsIntermediateDriver.Current.IsGpsDevice)
                    return true;
                
            }
        }

#endif
        #endregion

        #region Private Methods

        private static void Log(string message)
        {
            //if(Debugger.IsAttached)
            //    Debug.WriteLine(message);

            if (_Output != null)
            {
                _Output.WriteLine(message);

                /* In some rare debugging cases, the device hangs.  To prevent loss of
                 * buffered data, flush this immediately.
                 */
                _Output.Flush();
            }
        }

        #endregion
    }
}
