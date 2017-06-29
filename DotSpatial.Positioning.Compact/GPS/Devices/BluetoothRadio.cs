#if !PocketPC

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace DotSpatial.Positioning.Gps.IO
{
    /// <summary>
    /// Represents a Bluetooth radio.
    /// </summary>
    /// <remarks>Most computers have a single Bluetooth radio attached to them.  The radio is responsible for wireless communications
    /// with all Bluetooth devices.  This class provides the ability to enable or disable the radio, as well as to access devices which
    /// the radio can detect.</remarks>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class BluetoothRadio : IEquatable<BluetoothRadio>
    {
        private IntPtr _Handle;
        private string _Name;
        private DeviceClass _Class;
        private DeviceClass _MinorClass;
        private ServiceClass _ServiceClass;

        private static BluetoothRadio _Current;

        #region Constructors

        static BluetoothRadio()
        {
            try
            {
                // Make generic parameters for the search
                NativeMethods.BLUETOOTH_FIND_RADIO_PARAMS radio = new NativeMethods.BLUETOOTH_FIND_RADIO_PARAMS();

                // Make a handle for the radio itself
                IntPtr phRadio = IntPtr.Zero;

                // Search for the first radio 
                IntPtr findHandle = NativeMethods.BluetoothFindFirstRadio(radio, ref phRadio);

                // Was a radio found?
                if (findHandle == IntPtr.Zero)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    switch (errorCode)
                    {
                        case 259:
                            // No more data
                            return;
                        default:
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }

                //  Yes.  Close the search
                NativeMethods.BluetoothFindRadioClose(findHandle);            

                // If we have a radio, turn it into an object
                if (phRadio != IntPtr.Zero)
                    _Current = new BluetoothRadio(phRadio);
            }
            catch (Win32Exception ex)
            {
                // Note: Do not allow exceptions to be thrown in a static constructor.
                // Instead, raise the DeviceDetectionAttemptFailed event to allow the exception to be handled
                Devices.OnDeviceDetectionAttemptFailed(new DeviceDetectionException(null, ex));
            }
        }

        internal BluetoothRadio(IntPtr handle)
        {
            _Handle = handle;

            NativeMethods.BLUETOOTH_RADIO_INFO info = new NativeMethods.BLUETOOTH_RADIO_INFO();
            info.dwSize = 520;

            // Get information for this radio
            int errorCode = NativeMethods.BluetoothGetRadioInfo(_Handle, ref info);
            if (errorCode != 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            // Load up information
            _Name = info.szName;
            _Class = (DeviceClass)(info.classOfDevice & 0x1F00);
            _MinorClass = (DeviceClass)(info.classOfDevice & 0xFC);
            _ServiceClass = (ServiceClass)(info.classOfDevice & 0xFFE000);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Returns the current Bluetooth radio if one is installed.
        /// </summary>
        public static BluetoothRadio Current
        {
            get { return _Current; }
        }

        #endregion

        #region Public Properties

        internal IntPtr Handle
        {
            get { return _Handle; }
        }

        /// <summary>
        /// Returns the name of the radio.
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// Returns the primary purpose of the device.
        /// </summary>
        public DeviceClass Class
        {
            get
            {
                return _Class;
            }
        }

        /// <summary>
        /// Returns a sub-category describing the purpose of the device.
        /// </summary>
        public DeviceClass MinorClass
        {
            get
            {
                return _MinorClass;
            }
        }

        /// <summary>
        /// Returns the major type of device.
        /// </summary>
        public ServiceClass ServiceClass
        {
            get
            {
                return _ServiceClass;
            }
        }

        /// <summary>
        /// Controls whether the radio can accept incoming connections.
        /// </summary>
        public bool IsConnectable
        {
            get
            {
                return NativeMethods.BluetoothIsConnectable(_Handle);
            }
            set
            {
                NativeMethods.BluetoothEnableIncomingConnections(_Handle, value);
            }
        }

        /// <summary>
        /// Controls whether the radio can be found by other devices.
        /// </summary>
        public bool IsDiscoverable
        {
            get
            {
                return NativeMethods.BluetoothIsDiscoverable(_Handle);
            }
            set
            {
                NativeMethods.BluetoothEnableDiscovery(_Handle, value);
            }
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;
            if (obj is BluetoothRadio)
                return Equals((BluetoothRadio)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return _Handle.GetHashCode();
        }

        public override string ToString()
        {
            return _Name;
        }

        #endregion

        #region IEquatable<BluetoothRadio> Members

        public bool Equals(BluetoothRadio other)
        {
            return _Handle.Equals(other.Handle);
        }

        #endregion
    }
}

#endif
