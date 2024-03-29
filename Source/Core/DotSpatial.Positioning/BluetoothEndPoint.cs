﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a Bluetooth service on a device.
    /// </summary>
    [Description("Returns a GUID indentifying the service.")]
    [DefaultProperty("Name")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class BluetoothEndPoint : EndPoint, IEquatable<BluetoothEndPoint>, IFormattable
    {
        /// <summary>
        ///
        /// </summary>
        private readonly SocketAddress _socketAddress;

        /// <summary>
        ///
        /// </summary>
        private const AddressFamily BLUETOOTH_FAMILY = (AddressFamily)32;

        /// <summary>
        ///
        /// </summary>
        private const int PACKET_SIZE = 30;
        /// <summary>
        ///
        /// </summary>
        private const int ADDRESS_OFFSET = 2;
        /// <summary>
        ///
        /// </summary>
        private const int SERVICE_GUID_OFFSET = 10;
        /// <summary>
        ///
        /// </summary>
        private const int PORT_OFFSET = 26;
        /// <summary>
        ///
        /// </summary>
        private const int DEFAULT_PORT = 1;

        #region Constructors

        /* FxCop says this is unused
         *
        // internal BluetoothEndPoint(byte[] data)
        //{
        //    /* Socket endpoints are different on the desktop vs. mobile devices.
        //     * As a result, we'll serialize the same Address/Port/Guid differently.
        //     */

        //    // Create a new Bluetooth socket address
        //    _SocketAddress = new SocketAddress(BluetoothFamily, PacketSize);

        //    // Copy all the data over
        //    for (int index = 0; index < PacketSize; index++)
        //        _SocketAddress[index] = data[index];

        //    // Deserialize the address
        //    _Address = new BluetoothAddress(BitConverter.ToInt32(data, AddressOffset));

        //    // Deserialize the service GUID
        //    byte[] serviceBytes = new byte[16];
        //    Array.Copy(data, ServiceGuidOffset, serviceBytes, 0, 16);
        //    _Service = new Guid(serviceBytes);

        //    // Deserialize the port
        //    _Port = BitConverter.ToInt32(data, PortOffset);
        //}

        /// <summary>
        /// Creates a new instance using the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        public BluetoothEndPoint(BluetoothAddress address)
            : this(address, Guid.Empty)
        { }

        /// <summary>
        /// Creates a new instance using the specified address and service GUID.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="service">The service.</param>
        public BluetoothEndPoint(BluetoothAddress address, Guid service)
            : this(address, service, DEFAULT_PORT)
        { }

        /// <summary>
        /// Creates a new instance using the specified address, service GUID, and remote port number.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="service">The service.</param>
        /// <param name="port">The port.</param>
        public BluetoothEndPoint(BluetoothAddress address, Guid service, int port)
        {
            Address = address;
            Service = service;
            Port = port;

            // Create a new Bluetooth socket address
            _socketAddress = new SocketAddress(BLUETOOTH_FAMILY, PACKET_SIZE);

            // Specify the address family (again?)
            _socketAddress[0] = 32;

            // Copy the device address in
            byte[] addressBytes = address.Address;
            for (int index = 0; index < 6; index++)
            {
                _socketAddress[index + ADDRESS_OFFSET] = addressBytes[index];
            }

            // Copy in the service GUID
            byte[] guidBytes = service.ToByteArray();
            for (int index = 0; index < 16; index++)
            {
                _socketAddress[index + SERVICE_GUID_OFFSET] = guidBytes[index];
            }

            // Copy in the port
            byte[] portBytes = BitConverter.GetBytes(port);
            for (int index = 0; index < 4; index++)
            {
                _socketAddress[index + PORT_OFFSET] = portBytes[index];
            }
        }

        /// <summary>
        /// Creates a new instance from the specified socket address.
        /// </summary>
        /// <param name="socketAddress">The socket address.</param>
        public BluetoothEndPoint(SocketAddress socketAddress)
        {
            _socketAddress = socketAddress;

            // Deserialize the address
            byte[] addressBytes = new byte[6];
            for (int index = 0; index < 6; index++)
            {
                addressBytes[index] = socketAddress[index + ADDRESS_OFFSET];
            }

            Address = new BluetoothAddress(addressBytes);

            // Deserialize the service GUID
            byte[] serviceBytes = new byte[16];
            for (int index = 0; index < 16; index++)
            {
                serviceBytes[index] = socketAddress[index + SERVICE_GUID_OFFSET];
            }

            Service = new Guid(serviceBytes);

            // Deserialize the port
            byte[] portBytes = new byte[4];
            for (int index = 0; index < 4; index++)
            {
                portBytes[index] = socketAddress[index + PORT_OFFSET];
            }

            Port = BitConverter.ToInt32(portBytes, 0);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns a GUID identifying the service.
        /// </summary>
        [Category("Data")]
        [Description("Returns a GUID indentifying the service.")]
        [Browsable(true)]
        public Guid Service { get; }

        /// <summary>
        /// Returns the port used for opening connections.
        /// </summary>
        [Category("Data")]
        [Description("Returns the port used for opening connections.")]
        [Browsable(true)]
        public int Port { get; }

        /// <summary>
        /// Returns a friendly name for the endpoint.
        /// </summary>
        [Category("Data")]
        [Description("Returns a friendly name for the endpoint.")]
        [Browsable(true)]
        [ParenthesizePropertyName(true)]
        public string Name { get; private set; }

        /// <summary>
        /// Returns the unique address of the device, used for connections.
        /// </summary>
        [Category("Data")]
        [Description("Returns the unique address of the device, used for connections.")]
        [Browsable(true)]
        public BluetoothAddress Address { get; }

        /// <summary>
        /// Controls the number of times this endpoint has been identified as a GPS service.
        /// </summary>
        /// <value>The successful detection count.</value>
        [Category("Data")]
        [Description("Controls the number of times this endpoint has been identified as a GPS service.")]
        [Browsable(true)]
        public int SuccessfulDetectionCount { get; set; }

        /// <summary>
        /// Controls the number of times this endpoint has failed to identify itself as a GPS service.
        /// </summary>
        /// <value>The failed detection count.</value>
        [Category("Data")]
        [Description("Controls the number of times this endpoint has failed to identify itself as a GPS service.")]
        [Browsable(true)]
        public int FailedDetectionCount { get; set; }

        #endregion Public Properties

        #region Internal Methods

        /// <summary>
        /// Sets the name.
        /// </summary>
        /// <param name="name">The name.</param>
        internal void SetName(string name)
        {
            Name = name;
        }

        #endregion Internal Methods

        #region Overrides

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Address.GetHashCode() ^ Port.GetHashCode() ^ Service.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            // If it's unll, return false
            if (obj is null)
            {
                return false;
            }

            // Try to cast as this object type
            if (obj is BluetoothEndPoint endPoint)
            {
                return Equals(endPoint);
            }

            // Not our kind of object
            return base.Equals(obj);
        }

        /// <summary>
        /// Creates an <see cref="T:System.Net.EndPoint"/> instance from a <see cref="T:System.Net.SocketAddress"/> instance.
        /// </summary>
        /// <param name="socketAddress">The socket address that serves as the endpoint for a connection.</param>
        /// <returns>A new <see cref="T:System.Net.EndPoint"/> instance that is initialized from the specified <see cref="T:System.Net.SocketAddress"/> instance.</returns>
        /// <exception cref="T:System.NotImplementedException">Any attempt is made to access the method when the method is not overridden in a descendant class. </exception>
        ///
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        ///   </PermissionSet>
        public override EndPoint Create(SocketAddress socketAddress)
        {
            // If it's not a BT socket, skip it
            if (socketAddress.Family != BLUETOOTH_FAMILY)
            {
                return base.Create(socketAddress);
            }

            return new BluetoothEndPoint(socketAddress);
        }

        /// <summary>
        /// Serializes endpoint information into a <see cref="T:System.Net.SocketAddress"/> instance.
        /// </summary>
        /// <returns>A <see cref="T:System.Net.SocketAddress"/> instance that contains the endpoint information.</returns>
        /// <exception cref="T:System.NotImplementedException">Any attempt is made to access the method when the method is not overridden in a descendant class. </exception>
        ///
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        ///   </PermissionSet>
        public override SocketAddress Serialize()
        {
            return _socketAddress;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// Gets the address family to which the endpoint belongs.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Net.Sockets.AddressFamily"/> values.</returns>
        ///
        /// <exception cref="T:System.NotImplementedException">Any attempt is made to get or set the property when the property is not overridden in a descendant class. </exception>
        ///
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        ///   </PermissionSet>
        public override AddressFamily AddressFamily => BLUETOOTH_FAMILY;

        #endregion Overrides

        #region IEquatable<BluetoothEndPoint> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(BluetoothEndPoint other)
        {
            return Address.Equals(other.Address)
                && Port.Equals(other.Port)
                && Service.Equals(other.Service);
        }

        #endregion IEquatable<BluetoothEndPoint> Members

        #region IFormattable Members

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(Name))
            {
                CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

                if (string.IsNullOrEmpty(format))
                {
                    format = "G";
                }

                // Start with the address
                string result = Address.ToString(format, formatProvider);

                // Append the port if it's non-zero
                if (Port != 0)
                {
                    result += culture.TextInfo.ListSeparator + " Port " + Port.ToString(format, formatProvider);
                }

                // Append the GUID if it's not empty
                if (!Service.Equals(Guid.Empty))
                {
                    // Guid objects have very limited formats, the default "D" format is used.
                    result += culture.TextInfo.ListSeparator + " Service " + Service.ToString("D", formatProvider);
                }

                return result;
            }

            return Name;
        }

        #endregion IFormattable Members
    }
}