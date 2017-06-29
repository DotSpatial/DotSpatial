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
#if !PocketPC

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace DotSpatial.Positioning
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
        /// <summary>
        ///
        /// </summary>
        private IntPtr _handle;
        /// <summary>
        ///
        /// </summary>
        private readonly string _name;
        /// <summary>
        ///
        /// </summary>
        private readonly DeviceClass _class;
        /// <summary>
        ///
        /// </summary>
        private readonly DeviceClass _minorClass;
        /// <summary>
        ///
        /// </summary>
        private readonly ServiceClass _serviceClass;

        /// <summary>
        ///
        /// </summary>
        private static readonly BluetoothRadio _current;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        [SecurityCritical]
        static BluetoothRadio()
        {
            try
            {
                // Make generic parameters for the search
                NativeMethods2.BluetoothFindRadioParams radio = new NativeMethods2.BluetoothFindRadioParams();

                // Make a handle for the radio itself
                IntPtr phRadio = IntPtr.Zero;

                // Search for the first radio
                SafeBluetoothRadioFindHandle findHandle = NativeMethods2.BluetoothFindFirstRadio(radio, ref phRadio);

                // Was a radio found?
                //if (findHandle == IntPtr.Zero)
                //{
                //    int errorCode = Marshal.GetLastWin32Error();
                //    switch (errorCode)
                //    {
                //        case 259:
                //            // No more data
                //            return;
                //        default:
                //            throw new Win32Exception(Marshal.GetLastWin32Error());
                //    }
                //}

                //  Yes.  Close the search
                //NativeMethods2.BluetoothFindRadioClose(findHandle);
                findHandle.Close();

                // If we have a radio, turn it into an object
                if (phRadio != IntPtr.Zero)
                    _current = new BluetoothRadio(phRadio);
            }
            catch (Win32Exception ex)
            {
                // Notice: Do not allow exceptions to be thrown in a static constructor.
                // Instead, raise the DeviceDetectionAttemptFailed event to allow the exception to be handled
                Devices.OnDeviceDetectionAttemptFailed(new DeviceDetectionException(null, ex));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BluetoothRadio"/> class.
        /// </summary>
        /// <param name="handle">The handle.</param>
        [SecurityCritical]
        internal BluetoothRadio(IntPtr handle)
        {
            _handle = handle;

            NativeMethods2.BluetoothRadioInfo info = new NativeMethods2.BluetoothRadioInfo { ByteSize = 520 };

            // Get information for this radio
            int errorCode = NativeMethods2.BluetoothGetRadioInfo(_handle, ref info);
            if (errorCode != 0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            // Load up information
            _name = info.Name;
            _class = (DeviceClass)(info.DeviceClass & 0x1F00);
            _minorClass = (DeviceClass)(info.DeviceClass & 0xFC);
            _serviceClass = (ServiceClass)(info.DeviceClass & 0xFFE000);
        }

        #endregion Constructors

        #region Static Properties

        /// <summary>
        /// Returns the current Bluetooth radio if one is installed.
        /// </summary>
        public static BluetoothRadio Current
        {
            get { return _current; }
        }

        #endregion Static Properties

        #region Public Properties

        /// <summary>
        /// Gets the handle.
        /// </summary>
        internal IntPtr Handle
        {
            get { return _handle; }
        }

        /// <summary>
        /// Returns the name of the radio.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Returns the primary purpose of the device.
        /// </summary>
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
        public ServiceClass ServiceClass
        {
            get
            {
                return _serviceClass;
            }
        }

        /// <summary>
        /// Controls whether the radio can accept incoming connections.
        /// </summary>
        /// <returns></returns>
        [SecurityCritical]
        public bool GetIsConnectable()
        {
            return NativeMethods2.BluetoothIsConnectable(_handle);
        }

        /// <summary>
        /// Controls whether the radio can accept incoming connections.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        [SecurityCritical]
        public void SetIsConnectable(bool value)
        {
            NativeMethods2.BluetoothEnableIncomingConnections(_handle, value);
        }

        ///// <summary>
        ///// Controls whether the radio can be found by other devices.
        ///// </summary>
        //public bool IsDiscoverable
        //{
        //    get
        //    {
        //        return NativeMethods2.BluetoothIsDiscoverable(_handle);
        //    }
        //    set
        //    {
        //        NativeMethods2.BluetoothEnableDiscovery(_handle, value);
        //    }
        //}

        #endregion Public Properties

        #region Overrides

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (obj is BluetoothRadio)
                return Equals((BluetoothRadio)obj);
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _handle.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return _name;
        }

        #endregion Overrides

        #region IEquatable<BluetoothRadio> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(BluetoothRadio other)
        {
            return _handle.Equals(other.Handle);
        }

        #endregion IEquatable<BluetoothRadio> Members
    }
}

#endif