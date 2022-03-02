#if PocketPC
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using GeoFramework.Gps.Nmea;

namespace GeoFramework.Gps.IO
{
    /// <summary>
    /// Represents the GPS Intermediate Driver on hand-held Windows Mobile 5.0+ devices.
    /// </summary>
    /// <remarks><para>The GPS Intermediate Driver (or GPSID for short) is a multiplexer which allows multiple applications
    /// to share GPS data.  This driver is included in all Windows Mobile 5.0 (and above) devices.
    /// When the GPSID is used, applications must not connect directly to a GPS device, but instead needs to connect
    /// to a special "Program Port" created by the GPSID.  This class serves as a managed interface to the GPSID, allowing
    /// developers to perform configuration changes through code.</para>
    /// <para>This class is provided as a convenience for advanced GPS developers.  In most situations, GPS.NET will assume
    /// responsibility for properly configuring the GPSID for the purposes of enabling GPS data with minimal effort by both
    /// developers or end-users.  Exercise caution when changing settings by hand; it is preferable to use this class on
    /// a read-only basis whenever possible.</para>
    /// </remarks>
    public sealed class GpsIntermediateDriver : SerialDevice 
    {
        private bool _IsRestartRequired = false;
        private bool _IsAutomaticallyConfigured = true;
        private string _CurrentLogFile = String.Empty;
        private string _OldLogFile = string.Empty;
        private string _DriverInterface = string.Empty;

        private static GpsIntermediateDriver _Current;

        #region Constants

        internal const string _RootKeyName = @"System\CurrentControlSet\GPS Intermediate Driver";
        internal const string _DriversKeyName = _RootKeyName + @"\Drivers";
        internal const string _MultiplexerKeyName = _RootKeyName + @"\Multiplexer";

        #endregion

        #region Fields

        /// <summary>
        /// Returns the GPS Intermediate Driver for the mobile device.
        /// </summary>
        public static GpsIntermediateDriver Current
        {
            get { return _Current; }
        }

        #endregion

        #region Constructors

        static GpsIntermediateDriver()
        {
            if (IsSupported)
                _Current = new GpsIntermediateDriver();
        }

        internal GpsIntermediateDriver()
        {
            /* Most GPSID registry settings will specify a port to connect to.  However, some devices
             * have been encountered (such as the Intermic CN3) which has no registry value specified.
             * In this situation, we'll have to configure the GPSID to use a default of "GPD1:"  (?)
             */

            // Is the GPSID supported?
            if (string.IsNullOrEmpty(Port))
                this.Port = "GPD1:";

            // I'm a big baud rate, and I need a big cereal
            BaudRate = 115200;
        }    

        #endregion

        #region Public Properties

        /// <summary>
        /// Controls the port used to open a shared connection.
        /// </summary>
        /// <remarks>This property controls the name of the port used for all shared GPS connections.  Applications which use this port can
        /// trust that the GPS device will also be available to other applications using the same port.  When this property is changed, the
        /// GPSID registry control panel will also be updated.</remarks>
        public override string Port
        {
            get
            {
                return Convert.ToString(LoadSetting(_MultiplexerKeyName, "DriverInterface"));
            }
            set
            {
                // Validate
                if (value == null)
                    throw new ArgumentNullException("Port");

                // The port name must start with COM or GPD
#if !PocketPC
                string name = value.ToUpperInvariant();
#else
                string name = value.ToUpper(CultureInfo.InvariantCulture);
#endif
                if (!name.StartsWith("COM") && !name.StartsWith("GPD"))
                    throw new ArgumentOutOfRangeException("Port", "The GPS Intermediate Driver port must begin with 'COM' or 'GPD'.  The value '" + value + "' is not allowed.");

                // Commit the setting
                SaveSetting(_MultiplexerKeyName, "DriverInterface", value);

                // We'll need to refresh settings
                _IsRestartRequired = true;

                // Set the base port name
                base.Port = value;
            }
        }

        /// <summary>
        /// Controls the serial GPS device managed by the GPS Intermediate Driver.
        /// </summary>
        public SerialDevice HardwarePort
        {
            get
            {
                // We can only return a value if we have a serial device
                if (InterfaceType != GpsIntermediateDriverInterfaceType.SerialDevice)
                    return null;

                // Get the port of the serial device
                string currentDevice = Convert.ToString(LoadSetting(_DriversKeyName, "CurrentDriver"));
                if (currentDevice.Length != 0)
                {
                    // If we get here, we have the current device, but no friendly name.  Build a name
                    string port = Convert.ToString(LoadSetting(_DriversKeyName + @"\" + currentDevice, "CommPort"));

                    // Look for this device in the cache
                    IList<SerialDevice> serialDevices = SerialDevice.Cache;
                    for (int index = 0; index < serialDevices.Count; index++)
                    {
                        // Do the ports match?
                        if (serialDevices[index].Port.Equals(port, StringComparison.InvariantCultureIgnoreCase))
                        {
                            // Yes.  Return it.
                            return serialDevices[index];
                        }
                    }

                    // Return a new device
                    return new SerialDevice(port);
                }

                // If we get here, no current driver is specified
                return null;
            }
            set
            {
                /* The GPSID will save any changes made in the Control Panel to the key specified
                 * as the "CurrentDriver".  This value varies on different devices.  For example, the 
                 * Samsung Omina saves changes to "Samsung GPS Hardware".  So, let's follow suit.
                 */

                RegistryKey rootKey = null;
                try
                {
                    rootKey = Registry.LocalMachine.OpenSubKey(_DriversKeyName, false);
                    if (rootKey != null)
                    {
                        #region Create a brand new device

                        // Get the active source device
                        string currentDevice = Convert.ToString(rootKey.GetValue("CurrentDriver"));

                        // This value may be empty.  If so, let's see if there are any drivers at all
                        if (string.IsNullOrEmpty(currentDevice))
                        {
                            // Any subkeys?
                            if (rootKey.GetSubKeyNames().Length == 0)
                            {
                                /* No.  This could very well be a hard-reset device with no
                                 * settings whatsoever.  Crate a key.
                                 */
                                RegistryKey newDeviceKey = null;
                                try
                                {
                                    newDeviceKey = rootKey.CreateSubKey("Control Panel Configured Device");
                                    newDeviceKey.SetValue("InterfaceType", "COMM");
                                    newDeviceKey.SetValue("CommPort", value.Port);
                                    newDeviceKey.SetValue("Baud", value.BaudRate);
                                    if (!value.Name.Equals(value.Port))
                                        newDeviceKey.SetValue("FriendlyName", value.Name);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    throw new InvalidOperationException("The GPS Intermediate Driver cannot be automatically configured because of insufficient registry permissions.  Please modify GPSID settings by hand to use '" + value.Port + "' for the 'Program Port' with a baud rate of " + value.BaudRate.ToString());
                                }
                                finally
                                {
                                    if (newDeviceKey != null)
                                        newDeviceKey.Close();
                                }
                            }

                            // Finally, tell the GPSID to use this device
                            rootKey.SetValue("CurrentDriver", "Control Panel Configured Device");
                        }
                        else
                        {
                            // Modify existing device settings
                            RegistryKey newDeviceKey = null;
                            try
                            {
                                newDeviceKey = rootKey.OpenSubKey(currentDevice, true);
                                newDeviceKey.SetValue("InterfaceType", "COMM");
                                newDeviceKey.SetValue("CommPort", value.Port);
                                newDeviceKey.SetValue("Baud", value.BaudRate);
                                if (!value.Name.Equals(value.Port))
                                    newDeviceKey.SetValue("FriendlyName", value.Name);
                            }
                            catch { }
                            finally
                            {
                                if (newDeviceKey != null)
                                    newDeviceKey.Close();
                            }
                        }

                        // And we need a restart
                        _IsRestartRequired = true;

                        #endregion
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (rootKey != null)
                        rootKey.Close();
                }
            }
        }

        /// <summary>
        /// Controls whether changes to the driver are performed automatically.
        /// </summary>
        /// <remarks>The GPS Intermediate Driver requires accurate information in order to function properly.  For example, the
        /// serial port name and baud rate of the actual GPS device must be specified or the GPSID will fail.  When this property is
        /// <strong>True</strong>, GPS.NET will configure the GPSID to what it believes to be the most reliable GPS device.  Additional
        /// changes may be made, such as the name of the "Program Port," if no value exists.</remarks>
        public bool IsAutomaticallyConfigured
        {
            get
            {
                return _IsAutomaticallyConfigured;
            }
            set
            {
                _IsAutomaticallyConfigured = value;
            }
        }

        /// <summary>
        /// Returns whether the GPS Intermediate Driver is supported on this computer.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                // Look for a registry key
                RegistryKey key = null;
                try
                {
                    key = Registry.LocalMachine.OpenSubKey(_RootKeyName, false);
                    return key != null;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (key != null)
                        key.Close();
                }
            }
        }

        /// <summary>
        /// Controls the path to a log file used to capture incoming NMEA data.
        /// </summary>
        /// <remarks>This property is typically used for debugging purposes only.  When this property is set to a path, any incoming NMEA data
        /// from the GPSID will be logged to this file.  If, however, the property is set to an empty string, no logging will be performed.</remarks>
        public string CurrentLogFile
        {
            get
            {
                return Convert.ToString(LoadSetting(_RootKeyName, "CurrentLogFile"));
            }
            set
            {
                // Save the new value
                SaveSetting(_RootKeyName, "CurrentLogFile", value);

                // Flag that the driver needs a restart
                _IsRestartRequired = true;
            }
        }

        /// <summary>
        /// Controls the path to a second log file, used for older GPS data.
        /// </summary>
        /// <remarks>This property controls the absolute filename for a secondary log file.  When NMEA data is being logged,
        /// the current log file will be renamed to this filename once it reaches capacity.  A secondary log file lets developers access older GPS data while
        /// newer data is being collected.</remarks>
        public string OldLogFile
        {
            get
            {
                return Convert.ToString(LoadSetting(_RootKeyName, "OldLogFile"));
            }
            set
            {
                // Save the new value
                SaveSetting(_RootKeyName, "OldLogFile", _OldLogFile);

                // Flag that the driver needs a restart
                _IsRestartRequired = true;
            }
        }

        /// <summary>
        /// Controls the maximum number of bytes allowed in the receiving buffer for GPS data.
        /// </summary>
        /// <remarks>The maximum buffer size </remarks>
        public int MaximumBufferSize
        {
            get
            {
                return Convert.ToInt32(LoadSetting(_RootKeyName + @"\Multiplexer", "MaxBufferSize"));
            }
            set
            {
                // Validate values
                if (value <= 0)
                {
#if !PocketPC
                    throw new ArgumentOutOfRangeException("MaximumBufferSize", value, "The maximum GPSID buffer size must be greater than zero.");
#else
                    throw new ArgumentOutOfRangeException("MaximumBufferSize", "The maximum GPSID buffer size must be greater than zero.");
#endif
                }

                // Save the new value
                SaveSetting(_RootKeyName + @"\Multiplexer", "MaxBufferSize", value);

                // Flag that the driver needs a restart
                _IsRestartRequired = true;
            }
        }

        /// <summary>
        /// Controls the maximum allowed size of a log file, in bytes.
        /// </summary>
        /// <remarks>This property is typically used for debugging purposes only.</remarks>
        public int MaximumLogFileSize
        {
            get
            {
                return Convert.ToInt32(LoadSetting(_RootKeyName, "MaxLogFileSize"));
            }
            set
            {
                if (value <= 1024)
                    throw new ArgumentOutOfRangeException("MaximumLogFileSize", "The maximum log file size cannot be less than 1024.  A value of several kilobytes is recommended.");

                // Set the new value
                SaveSetting(_RootKeyName, "MaxLogFileSize", value);
            }
        }

        /// <summary>
        /// Controls whether the GPSID will configure itself to use plug-and-play GPS devices.
        /// </summary>
        /// <remarks>Some newer GPS devices, such as SecureDigital card devices, are usable immediately after they are plugged into
        /// a mobile device.  When this property is enabled, the GPSID will configure itself to use the device.  When this is disabled,
        /// GPS hardware settings must be specified manually, or by GPS.NET.</remarks>
        public bool AllowPlugAndPlay
        {
            get
            {
                return Convert.ToBoolean(LoadSetting(_DriversKeyName, "AllowPlugAndPlay"));
            }
            set
            {
                // Set the new value
                SaveSetting(_DriversKeyName, "AllowPlugAndPlay", value);

                // Indicate we need a restart
                _IsRestartRequired = true;
            }
        }

        public override string Name
        {
            get
            {
                /* For some devices, a "FriendlyName" registry value will exist.  If this exists, we can use the value,
                 * otherwise, we'll just return what we can.
                 */

                string currentDevice = Convert.ToString(LoadSetting(_DriversKeyName, "CurrentDriver"));
                if (currentDevice.Length != 0)
                {
                    // Try to get the friendly name for this device
                    string friendlyName = Convert.ToString(LoadSetting(_DriversKeyName + @"\" + currentDevice, "FriendlyName"));
                    if (friendlyName.Length != 0)
                    {
                        // We have a friendly name!  Use it
                        return "GPS Intermediate Driver (" + friendlyName + ")";
                    }

                    // If we get here, we have the current device, but no friendly name.  Build a name
                    string interfaceType = Convert.ToString(LoadSetting(_DriversKeyName + @"\" + currentDevice, "InterfaceType"));
#if !PocketPC
                    interfaceType = interfaceType.ToLowerInvariant();
#else
                    interfaceType = interfaceType.ToLower(CultureInfo.InvariantCulture);
#endif
                    switch (interfaceType)
                    {
                        case "comm":
                            // Use the "CommPort" value
                            string commPort = Convert.ToString(LoadSetting(_DriversKeyName + @"\" + currentDevice, "CommPort"));
                            if (commPort.Length != 0)
                                return "GPS Intermediate Driver (Device on " + commPort + ")";
                            else
                                return "GPS Intermediate Driver (Invalid serial device)";
                        case "file":
                            return "GPS Intermediate Driver (Text File)";
                        case "poll":
                            return "GPS Intermediate Driver (Manually polled device)";
                        default:
                            return "GPS Intermediate Driver (Unrecognized device)";
                    }
                }

                // No current device is specified!
                return "GPS Intermediate Driver";
            }
        }

        /// <summary>
        /// Returns the source of raw GPS data for the GPS Intermediate Driver.
        /// </summary>
        public GpsIntermediateDriverInterfaceType InterfaceType
        {
            get
            {
                /* For some devices, a "FriendlyName" registry value will exist.  If this exists, we can use the value,
                * otherwise, we'll just return what we can.
                */

                string currentDevice = Convert.ToString(LoadSetting(_DriversKeyName, "CurrentDriver"));
                if (currentDevice.Length != 0)
                {
                    // If we get here, we have the current device, but no friendly name.  Build a name
                    string interfaceType = Convert.ToString(LoadSetting(_DriversKeyName + @"\" + currentDevice, "InterfaceType"));
#if !PocketPC
                    interfaceType = interfaceType.ToLowerInvariant();
#else
                    interfaceType = interfaceType.ToLower(CultureInfo.InvariantCulture);
#endif
                    switch (interfaceType)
                    {
                        case "comm":
                            return GpsIntermediateDriverInterfaceType.SerialDevice;
                        case "file":
                            return GpsIntermediateDriverInterfaceType.File;
                        case "poll":
                            return GpsIntermediateDriverInterfaceType.Manual;
                    }
                }

                return GpsIntermediateDriverInterfaceType.Unknown;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Causes the GPS Intermediate Driver to restart.
        /// </summary>
        /// <remarks>This method is only used when changes have been made to GPSID registry settings.  When called,
        /// the GPSID is instructed to restart itself and load fresh registry settings.</remarks>
        public void Restart()
        {
            /* In order to reset the GPSID, a call is necessary to the "CreateFile" API to "GPD0:" a port
             * reserved for the GPSID.  Then, a command is sent to refresh the service.  This code was written
             * based on MSDN documentation here: http://msdn.microsoft.com/en-us/library/bb202123.aspx
             * 
               HANDLE hGPS = CreateFile(L"GPD0:", GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
               if (hGPS != INVALID_HANDLE_VALUE) {
                  DeviceIoControl(hGPS,IOCTL_SERVICE_REFRESH,0,0,0,0,0,0);
                  CloseHandle(hGPS);
               }
             */

            // Open a basic connection to GPD0:
            IntPtr handle = NativeMethods.CreateFile("GPD0:",
                FileAccess.Read,
                FileShare.ReadWrite,
                0,
                FileMode.Open,
                FileAttributes.Normal,
                IntPtr.Zero);

            // If it succeeded, continue with the refresh command
            if (handle == NativeMethods.INVALID_HANDLE)
                return;

            /* Instruct the driver to refresh all of its settings.  What this does is tell the
             * GPSID to reload registry settings.
             */
            NativeMethods.DeviceIoControl(handle, NativeMethods.IOCTL_SERVICE_REFRESH, IntPtr.Zero, 0, IntPtr.Zero, 0, 0, IntPtr.Zero);

            // If the handle is open, close it
            NativeMethods.CloseHandle(handle);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Controls whether connections are allowed to the GPS Intermediate Driver.
        /// </summary>
        /// <remarks>When this property is set to <strong>True</strong>, the GPS Intermediate Driver will be responsible
        /// for managing all GPS device communications.  GPS.NET will only process data arriving from the GPSID.  When this
        /// property is set to <strong>False</strong>, the GPSID is bypassed and connections will be made directly to a device.</remarks>
        public override bool AllowConnections
        {
            get
            {
                // If it's forbidden, return false
                if (!Devices.AllowGpsIntermediateDriver)
                    return false;

                // Get the raw value
                object value = LoadSetting(_RootKeyName, "IsEnabled");

                /* If it's null, there's no value!  In devices like this, such as the ETEN Glofiish M700,
                 * the GPSID reports itself as enabled despite no registry value.  As a result, I think
                 * we can assume that no registry value means it's enabled.  I'm reluctant to write
                 * the registry value, however, just because of all the proprietary shit out there.  The
                 * HTC was a disaster when we wrote registry values.
                 */
                if (value == null)
                {
                    // Set the setting to true
                    base.AllowConnections = true;
                    return true;
                }
                else
                {
                    // Parse the registry value
                    return Convert.ToBoolean(value);
                }
            }
            set
            {
                // If it isn't supported then just quit


                // Update the registry
                SaveSetting(_RootKeyName, "IsEnabled", value);

                // For consistency, set the base class
                base.AllowConnections = value;

                // We'll need a refresh
                _IsRestartRequired = true;
            }
        }

        protected override Stream OpenStream(FileAccess access, FileShare sharing)
        {
            // Yes.  Make sure the GPSID is up to snuff.
            if (String.IsNullOrEmpty(Port))
                Port = "GPD1:";

            // If settings have changed, we need to refresh the driver
            if (_IsRestartRequired)
            {
                // We no longer need a restart
                _IsRestartRequired = false;

                // Refresh the GPSID to use the latest registry settings
                Restart();
            }

            // Yes.  Continue as usual
            return new SerialStream(Port, 115200, access, sharing);
        }

        protected override bool DetectProtocol()
        {
            // If no connections are allowed, exit
            if (!AllowConnections)
            {
                Devices.OnDeviceDetectionAttemptFailed(
                    new DeviceDetectionException(this, "Connections are not allowed on this device."));
                return false;
            }

            // Open a connection
            try
            {
                Open();
            }
            catch (Win32Exception ex)
            {
                /* If the GPSID is misconfigured, or the Hardware Port is not responding, we'll get error 21 (Device not ready)
                 * So, make it friendly.
                 */
                if (ex.NativeErrorCode == 21)
                {
                    // Are we allowed to make changes?
                    if (_IsAutomaticallyConfigured)
                    {
                        Devices.OnDeviceDetectionAttemptFailed(
                            new DeviceDetectionException(this, "The GPS device on " + HardwarePort.Port + " is not responding.  The GPSID will be reconfigured if another GPS device is found."));
                    }
                    else
                    {
                        Devices.OnDeviceDetectionAttemptFailed(
                            new DeviceDetectionException(this, "The GPS Intermediate Driver can't open the GPS device on " + HardwarePort.Port + ".  No repair will be attempted."));
                    }
                    return false;
                }

                throw;
            }

            // The GPSID works at any baud rate.  So, just try and read stuff in
            StreamReader reader = new StreamReader(BaseStream, ASCIIEncoding.ASCII, false, NmeaReader.IdealNmeaBufferSize);
            for (int count = 0; count < 15; count++)
            {
                // Read a line
                string testLine = null;
                try
                {
                    testLine = reader.ReadLine();
                }
                catch (TimeoutException ex)
                {
                    Devices.OnDeviceDetectionAttemptFailed(
                        new DeviceDetectionException(this, "A timeout occurred while trying to read from the GPS Intermediate Driver.", ex));
                    return false;
                }


                if (
                    // Is it null?
                    testLine != null
                    // Does it begin with a dollar sign?
                    && testLine.StartsWith("$")
                    // Is there an asterisk before that last two characters?
                    && testLine.IndexOf("*") == testLine.Length - 3)
                {
                    // Yes!  This is an NMEA device.
                    return true;
                }
            }

            // If we get here, it's garbage
            Devices.OnDeviceDetectionAttemptFailed(
                new DeviceDetectionException(this, "A connection was opened to the GPS Intermediate Driver but no data was found.  The \"Hardware Port\" settings are incorrect, or the GPS device is not responding.", null));
            return false;
        }

        #endregion

        #region Internal Methods

        // Loads a value from the registry.
        internal static object LoadSetting(string keyName, string valueName)
        {
            RegistryKey rootKey = null;
            try
            {
                rootKey = Registry.LocalMachine.OpenSubKey(keyName, false);
                if (rootKey != null)
                    return rootKey.GetValue(valueName);
                else
                    return null;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (rootKey != null)
                    rootKey.Close();
            }

        }

        internal static void DeleteSetting(string keyName, string valueName)
        {
            RegistryKey rootKey = null;
            try
            {
                rootKey = Registry.LocalMachine.OpenSubKey(keyName, true);
                if (rootKey != null)
                    rootKey.DeleteValue(valueName);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (rootKey != null)
                    rootKey.Close();
            }
        }

        // Saves a value to the registry
        internal static void SaveSetting(string keyName, string valueName, object value)
        {
            RegistryKey rootKey = null;
            try
            {
                rootKey = Registry.LocalMachine.OpenSubKey(keyName, true);
                if (rootKey != null)
                    rootKey.SetValue(valueName, value);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (rootKey != null)
                    rootKey.Close();
            }
        }

        #endregion


        #region Unused Code (Commented Out)

        //protected override void OnWriteToCache()
        //{
        //    //throw new NotImplementedException();
        //}

        //protected override void OnReadFromCache()
        //{
        //    //throw new NotImplementedException();
        //}

        #endregion
    }

    public enum GpsIntermediateDriverInterfaceType
    {
        /// <summary>
        /// The setting is custom or unrecognized.
        /// </summary>
        Unknown,
        /// <summary>
        /// A serial GPS device is providing raw data.
        /// </summary>
        SerialDevice,
        /// <summary>
        /// Raw data is coming from a file.
        /// </summary>
        File,
        /// <summary>
        /// Raw GPS data is being polled manually.
        /// </summary>
        Manual
    }
}
#endif