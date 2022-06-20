// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

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
                NativeMethods2.BluetoothFindRadioParams radio = new();

                // Make a handle for the radio itself
                IntPtr phRadio = IntPtr.Zero;

                // Search for the first radio
                SafeBluetoothRadioFindHandle findHandle = NativeMethods2.BluetoothFindFirstRadio(radio, ref phRadio);

                // Was a radio found?
                // if (findHandle == IntPtr.Zero)
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
                // NativeMethods2.BluetoothFindRadioClose(findHandle);
                findHandle.Close();

                // If we have a radio, turn it into an object
                if (phRadio != IntPtr.Zero)
                {
                    Current = new BluetoothRadio(phRadio);
                }
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
            Handle = handle;

            NativeMethods2.BluetoothRadioInfo info = new() { ByteSize = 520 };

            // Get information for this radio
            int errorCode = NativeMethods2.BluetoothGetRadioInfo(Handle, ref info);
            if (errorCode != 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }

            // Load up information
            Name = info.Name;
            Class = (DeviceClass)(info.DeviceClass & 0x1F00);
            MinorClass = (DeviceClass)(info.DeviceClass & 0xFC);
            ServiceClass = (ServiceClass)(info.DeviceClass & 0xFFE000);
        }

        #endregion Constructors

        #region Static Properties

        /// <summary>
        /// Returns the current Bluetooth radio if one is installed.
        /// </summary>
        public static BluetoothRadio Current { get; private set; }

        #endregion Static Properties

        #region Public Properties

        /// <summary>
        /// Gets the handle.
        /// </summary>
        internal IntPtr Handle { get; }

        /// <summary>
        /// Returns the name of the radio.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Returns the primary purpose of the device.
        /// </summary>
        public DeviceClass Class { get; }

        /// <summary>
        /// Returns a sub-category describing the purpose of the device.
        /// </summary>
        public DeviceClass MinorClass { get; }

        /// <summary>
        /// Returns the major type of device.
        /// </summary>
        public ServiceClass ServiceClass { get; }

        /// <summary>
        /// Controls whether the radio can accept incoming connections.
        /// </summary>
        /// <returns></returns>
        [SecurityCritical]
        public bool GetIsConnectable()
        {
            return NativeMethods2.BluetoothIsConnectable(Handle);
        }

        /// <summary>
        /// Controls whether the radio can accept incoming connections.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        [SecurityCritical]
        public void SetIsConnectable(bool value)
        {
            NativeMethods2.BluetoothEnableIncomingConnections(Handle, value);
        }

        ///// <summary>
        ///// Controls whether the radio can be found by other devices.
        ///// </summary>
        // public bool IsDiscoverable
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
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is BluetoothRadio radio && Equals(radio);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            return Name;
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
            return Handle.Equals(other.Handle);
        }

        #endregion IEquatable<BluetoothRadio> Members
    }
}