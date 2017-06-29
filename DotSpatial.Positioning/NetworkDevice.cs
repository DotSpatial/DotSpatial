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
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Win32;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a GPS device which is accessed via a socket connection.
    /// </summary>
    public class NetworkDevice : Device
    {
        /// <summary>
        ///
        /// </summary>
        private readonly AddressFamily _addressFamily;
        /// <summary>
        ///
        /// </summary>
        private readonly SocketType _socketType;
        /// <summary>
        ///
        /// </summary>
        private readonly ProtocolType _protocolType;
        /// <summary>
        ///
        /// </summary>
        private EndPoint _endPoint;
        /// <summary>
        ///
        /// </summary>
        private Socket _socket;

        /// <summary>
        ///
        /// </summary>
        private static TimeSpan _defaultConnectTimeout = TimeSpan.FromSeconds(30);

        #region Constants

        /// <summary>
        ///
        /// </summary>
        private const string ROOT_KEY_NAME = Devices.ROOT_KEY_NAME + @"Network\";

        #endregion Constants

        #region Constructors

        /// <summary>
        /// Creates a NetworkDevice from the specified parameters
        /// </summary>
        /// <param name="addressFamily">The address family.</param>
        /// <param name="socketType">Type of the socket.</param>
        /// <param name="protocolType">Type of the protocol.</param>
        public NetworkDevice(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
            : this(addressFamily, socketType, protocolType, null)
        { }

        /// <summary>
        /// Creates a NetworkDevice from the specified parameters
        /// </summary>
        /// <param name="addressFamily">The address family.</param>
        /// <param name="socketType">Type of the socket.</param>
        /// <param name="protocolType">Type of the protocol.</param>
        /// <param name="endPoint">The end point.</param>
        public NetworkDevice(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, EndPoint endPoint)
        {
            _addressFamily = addressFamily;
            _socketType = socketType;
            _protocolType = protocolType;
            _endPoint = endPoint;
        }

        #endregion Constructors

        #region Public Properties

#if !PocketPC

        /// <summary>
        /// Returns the addressing scheme the socket can use.
        /// </summary>
        [Category("Data")]
        [Description("Returns the addressing scheme the socket can use. ")]
        [Browsable(true)]
#endif
        public AddressFamily AddressFamily
        {
            get
            {
                return _addressFamily;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the type of protocol used by the socket.
        /// </summary>
        [Category("Data")]
        [Description("Returns the type of protocol used by the socket.")]
        [Browsable(true)]
#endif
        public SocketType SocketType
        {
            get
            {
                return _socketType;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the protocol supported by the socket.
        /// </summary>
        [Category("Data")]
        [Description("Returns the protocol supported by the socket.")]
        [Browsable(true)]
#endif
        public ProtocolType ProtocolType
        {
            get
            {
                return _protocolType;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the network address for the device.
        /// </summary>
        /// <value>The end point.</value>
        [Category("Data")]
        [Description("Returns the network address for the device.")]
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
        public EndPoint EndPoint
        {
            get
            {
                return _endPoint;
            }
            set
            {
                _endPoint = value;
            }
        }

#if !PocketPC

        /// <summary>
        /// Returns the socket associated with this device.
        /// </summary>
        [Browsable(false)]
#endif
        protected Socket Socket
        {
            get { return _socket; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates a new connection to the specified endpoint.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        public void Open(EndPoint endPoint)
        {
            // If we're already open, complain.
            if (IsOpen)
                throw new InvalidOperationException("The Open method was called on a device which is already connected.  Please call the Close method before calling the Open method to ensure that connections are cleaned up properly.");

            // Remember the endpoint
            _endPoint = endPoint;

            // And continue opening a connection
            Open();
        }

        #endregion Public Methods

        #region Static Properties

        /// <summary>
        /// Controls the amount of time allowed for a connection to be successfully opened.
        /// </summary>
        /// <value>The default connect timeout.</value>
        public static TimeSpan DefaultConnectTimeout
        {
            get { return _defaultConnectTimeout; }
            set
            {
                // The timeout must be greater than zero
                if (value.TotalSeconds <= 0)
                    throw new ArgumentOutOfRangeException("DefaultConnectTimeout", "The default connect timeout for a network device must be greater than zero.  A value of five to ten seconds is recommended.");

                // Set the value
                _defaultConnectTimeout = value;
            }
        }

        #endregion Static Properties

        #region Protected Methods

        /// <summary>
        /// Occurs immediately before a socket is opened.
        /// </summary>
        protected virtual void OnConfigureSocket()
        { }

        #endregion Protected Methods

        #region Overrides

        /// <summary>
        /// Returns a natural language name for the device.
        /// </summary>
        /// <inheritdocs/>
        public override string Name
        {
            get
            {
                return "Network Device";
            }
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
            // Give the consumer a chance to set socket options
            OnConfigureSocket();

            // Is the socket already connected?
            if (_socket == null || !_socket.Connected)
            {
                // Close any exsisting socket
                if (_socket != null)
                    _socket.Close();

                // Create a socket
                _socket = new Socket(_addressFamily, _socketType, _protocolType);

                // A smaller buffer size reduces latency.  We want to process data as soon as it's transmitted
                _socket.ReceiveBufferSize = NmeaReader.IDEAL_NMEA_BUFFER_SIZE;
                _socket.SendBufferSize = NmeaReader.IDEAL_NMEA_BUFFER_SIZE;

                // Specify the timeout for read/write ops
                _socket.ReceiveTimeout = 10000; // (int)DefaultReadTimeout.TotalMilliseconds;
                _socket.SendTimeout = 10000; // = (int)DefaultWriteTimeout.TotalMilliseconds;

                // Begin connecting asynchronously
                IAsyncResult connectResult = _socket.BeginConnect(_endPoint, null, null);

                // Wait up to the timeout for the connection to complete
                if (!connectResult.AsyncWaitHandle.WaitOne((int)_defaultConnectTimeout.TotalMilliseconds, false))
                    throw new TimeoutException(Name + " could not connect within the timeout period.");

                _socket.EndConnect(connectResult);
            }

            // Wrap a network stream around the socket.
            return new NetworkStream(_socket, access, true);

            // We don't need to set the streams timeouts because they are implied by the
            // socket timeouts set above.

            /*
             * This is documented (see Socket.Shutdown()). Closed sockets shouldn't be
             * reopened. Above, a non null socket wasn't being closed, which caused errors
             * suggesting that attempts were being made to reopen a socket.
             *
             * We also don't need to set the streams timeouts because they are implied by the
             * socket timeouts.
             */
        }

        /// <summary>
        /// Disposes of any unmanaged (or optionally, managed) resources used by the device.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_socket != null)
            {
                // Close disconnects the socket and disposes it.
                _socket.Close();
            }

            // Dispose of the base, closing the stream
            base.Dispose(disposing);

            if (disposing)
            {
                _endPoint = null;
                _socket = null;
            }
        }

        /// <summary>
        /// Removes previously cached information for this device from the registry.
        /// </summary>
        protected override void OnCacheRemove()
        {
            try
            {
                // Endpoints are serialized in their raw byte form to improve compatibility
                SocketAddress address = _endPoint.Serialize();
                StringBuilder hexEndPoint = new StringBuilder();
                for (int index = 0; index < address.Size; index++)
                    hexEndPoint.Append(address[index].ToString("X2"));

                Registry.LocalMachine.DeleteSubKeyTree(ROOT_KEY_NAME + hexEndPoint);
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
        /// Records information about this device to the registry.
        /// </summary>
        protected override void OnCacheWrite()
        {
            // Endpoints are serialized in their raw byte form to improve compatibility
            SocketAddress address = _endPoint.Serialize();
            StringBuilder hexEndPoint = new StringBuilder();
            for (int index = 0; index < address.Size; index++)
                hexEndPoint.Append(address[index].ToString("X2"));

            // Save device information
            RegistryKey deviceKey = Registry.LocalMachine.CreateSubKey(ROOT_KEY_NAME + hexEndPoint);
            if (deviceKey == null)
                return;

            // Update the success/fail statistics
            deviceKey.SetValue("Number of Times Detected", SuccessfulDetectionCount);
            deviceKey.SetValue("Number of Times Failed", FailedDetectionCount);
            deviceKey.SetValue("Date Last Detected", DateDetected.ToString("R", CultureInfo.InvariantCulture));
            deviceKey.SetValue("Date Last Connected", DateConnected.ToString("R", CultureInfo.InvariantCulture));
            deviceKey.Close();
        }

        /// <summary>
        /// Reads information about this device from the registry.
        /// </summary>
        protected override void OnCacheRead()
        {
            // Endpoints are serialized in their raw byte form to improve compatibility
            SocketAddress address = _endPoint.Serialize();
            StringBuilder hexEndPoint = new StringBuilder();
            for (int index = 0; index < address.Size; index++)
                hexEndPoint.Append(address[index].ToString("X2"));

            // Save device stats
            RegistryKey deviceKey = Registry.LocalMachine.OpenSubKey(ROOT_KEY_NAME + hexEndPoint, false);
            if (deviceKey == null)
                return;

            // Update the baud rate and etc.
            foreach (string name in deviceKey.GetValueNames())
            {
                switch (name)
                {
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
                }
            }
        }

        #endregion Overrides
    }
}