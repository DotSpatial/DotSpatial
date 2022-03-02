using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
#if !PocketPC
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning.Gps.IO
{
    /// <summary>
    /// Represents a Bluetooth service on a device.
    /// </summary>
#if !PocketPC
    [Description("Returns a GUID indentifying the service.")]
    [DefaultProperty("Name")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
    public sealed class BluetoothEndPoint : EndPoint, IEquatable<BluetoothEndPoint>, IFormattable
    {
        private SocketAddress _SocketAddress;
        private BluetoothAddress _Address;
        private Guid _Service;
        private int _Port;
        private string _Name;
        private int _SuccessfulDetectionCount;
        private int _FailedDetectionCount;

        private const AddressFamily BluetoothFamily = (AddressFamily)32;
#if !PocketPC
        private const int PacketSize = 30;
        private const int AddressOffset = 2;
        private const int ServiceGuidOffset = 10;
        private const int PortOffset = 26;
        private const int DefaultPort = 1;
#else
        private const int PacketSize = 40;
        private const int AddressOffset = 8;
        private const int ServiceGuidOffset = 16;
        private const int PortOffset = 32;
        private const int DefaultPort = 0;
#endif

        #region Constructors

        /* FxCop says this is unused
         * 
        //internal BluetoothEndPoint(byte[] data)
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
        /// <param name="address"></param>
        public BluetoothEndPoint(BluetoothAddress address)
            : this(address, Guid.Empty)
        { }

        /// <summary>
        /// Creates a new instance using the specified address and service GUID.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="service"></param>
        public BluetoothEndPoint(BluetoothAddress address, Guid service)
            : this(address, service, DefaultPort)
        { }

        /// <summary>
        /// Creates a new instance using the specified address, service GUID, and remote port number.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="service"></param>
        public BluetoothEndPoint(BluetoothAddress address, Guid service, int port)
        {
            _Address = address;
            _Service = service;
            _Port = port;

            // Create a new Bluetooth socket address
            _SocketAddress = new SocketAddress(BluetoothFamily, PacketSize);

            // Specify the address family (again?)
            _SocketAddress[0] = 32;

            // Copy the device address in
            byte[] addressBytes = address.Address;
            for (int index = 0; index < 6; index++)
                _SocketAddress[index + AddressOffset] = addressBytes[index];

            // Copy in the service GUID
            byte[] guidBytes = service.ToByteArray();
            for (int index = 0; index < 16; index++)
                _SocketAddress[index + ServiceGuidOffset] = guidBytes[index];

            // Copy in the port
            byte[] portBytes = BitConverter.GetBytes(port);
            for (int index = 0; index < 4; index++)
                _SocketAddress[index + PortOffset] = portBytes[index];
        }

        /// <summary>
        /// Creates a new instance from the specified socket address.
        /// </summary>
        /// <param name="socketAddress"></param>
        public BluetoothEndPoint(SocketAddress socketAddress)
        {
            _SocketAddress = socketAddress;

            // Deserialize the address
            byte[] addressBytes = new byte[6];
            for (int index = 0; index < 6; index++)
                addressBytes[index] = socketAddress[index + AddressOffset];
            _Address = new BluetoothAddress(addressBytes);

            // Deserialize the service GUID
            byte[] serviceBytes = new byte[16];
            for (int index = 0; index < 16; index++)
                serviceBytes[index] = socketAddress[index + ServiceGuidOffset];
            _Service = new Guid(serviceBytes);

            // Deserialize the port
            byte[] portBytes = new byte[4];
            for (int index = 0; index < 4; index++)
                portBytes[index] = socketAddress[index + PortOffset];
            _Port = BitConverter.ToInt32(portBytes, 0);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns a GUID indentifying the service.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns a GUID indentifying the service.")]
        [Browsable(true)]
#endif
        public Guid Service
        {
            get
            {
                return _Service;
            }
        }

        /// <summary>
        /// Returns the port used for opening connections.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the port used for opening connections.")]
        [Browsable(true)]
#endif
        public int Port
        {
            get
            {
                return _Port;
            }
        }

        /// <summary>
        /// Returns a friendly name for the endpoint.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns a friendly name for the endpoint.")]
        [Browsable(true)]
        [ParenthesizePropertyName(true)]
#endif
        public string Name
        {
            get
            {
                return _Name;
            }
        }


        /// <summary>
        /// Returns the unique address of the device, used for connections.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the unique address of the device, used for connections.")]
        [Browsable(true)]
#endif
        public BluetoothAddress Address
        {
            get
            {
                return _Address;
            }
        }


        /// <summary>
        /// Controls the number of times this endpoint has been identified as a GPS service.
        /// </summary>
        /// 
#if !PocketPC
        [Category("Data")]
        [Description("Controls the number of times this endpoint has been identified as a GPS service.")]
        [Browsable(true)]
#endif
        public int SuccessfulDetectionCount
        {
            get { return _SuccessfulDetectionCount; }
            set { _SuccessfulDetectionCount = value; }
        }

        /// <summary>
        /// Controls the number of times this endpoint has failed to identify itself as a GPS service.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Controls the number of times this endpoint has failed to identify itself as a GPS service.")]
        [Browsable(true)]
#endif
        public int FailedDetectionCount
        {
            get { return _FailedDetectionCount; }
            set { _FailedDetectionCount = value; }
        }

        #endregion

        #region Internal Methods

        internal void SetName(string name)
        {
            _Name = name;
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return _Address.GetHashCode() ^ _Port.GetHashCode() ^ _Service.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            // If it's unll, return false
            if (object.ReferenceEquals(obj, null))
                return false;

            // Try to cast as this object type
            BluetoothEndPoint endPoint = obj as BluetoothEndPoint;
            if (endPoint != null)
                return Equals(endPoint);

            // Not our kind of object
            return base.Equals(obj);
        }

        public override EndPoint Create(SocketAddress socketAddress)
        {
            // If it's not a BT socket, skip it
            if (socketAddress.Family != BluetoothFamily)
                return base.Create(socketAddress);
            else
                return new BluetoothEndPoint(socketAddress);
        }

        public override SocketAddress Serialize()
        {
            return _SocketAddress;
        }

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentUICulture);
        }
        
        public override AddressFamily AddressFamily
        {
            get
            {
                return BluetoothFamily;
            }
        }


        #endregion

        #region IEquatable<BluetoothEndPoint> Members

        public bool Equals(BluetoothEndPoint other)
        {
            return _Address.Equals(other.Address)
                && _Port.Equals(other.Port)
                && _Service.Equals(other.Service);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (String.IsNullOrEmpty(_Name))
            {
                CultureInfo culture = (CultureInfo)formatProvider;

                if (culture == null)
                    culture = CultureInfo.CurrentCulture;

                if (format == null || format.Length == 0)
                    format = "G";

                // Start with the address
                string result = _Address.ToString(format, formatProvider);

                // Append the port if it's non-zero
                if (_Port != 0)
                    result += culture.TextInfo.ListSeparator + " Port " + _Port.ToString(format, formatProvider);

                // Append the GUID if it's not empty
                if (!_Service.Equals(Guid.Empty))
                {
                    // Guid objects have very limited formats, the default "D" format is used.
                    result += culture.TextInfo.ListSeparator + " Service " + _Service.ToString("D", formatProvider);
                }

                return result;
            }
            else
                return _Name;
        }

        #endregion
    }
}
    