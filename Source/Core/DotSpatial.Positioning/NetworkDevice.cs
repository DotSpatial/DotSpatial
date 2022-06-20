// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

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
            AddressFamily = addressFamily;
            SocketType = socketType;
            ProtocolType = protocolType;
            EndPoint = endPoint;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the addressing scheme the socket can use.
        /// </summary>
        [Category("Data")]
        [Description("Returns the addressing scheme the socket can use. ")]
        [Browsable(true)]
        public AddressFamily AddressFamily { get; }

        /// <summary>
        /// Returns the type of protocol used by the socket.
        /// </summary>
        [Category("Data")]
        [Description("Returns the type of protocol used by the socket.")]
        [Browsable(true)]
        public SocketType SocketType { get; }

        /// <summary>
        /// Returns the protocol supported by the socket.
        /// </summary>
        [Category("Data")]
        [Description("Returns the protocol supported by the socket.")]
        [Browsable(true)]
        public ProtocolType ProtocolType { get; }

        /// <summary>
        /// Returns the network address for the device.
        /// </summary>
        /// <value>The end point.</value>
        [Category("Data")]
        [Description("Returns the network address for the device.")]
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public EndPoint EndPoint { get; set; }

        /// <summary>
        /// Returns the socket associated with this device.
        /// </summary>
        [Browsable(false)]
        protected Socket Socket { get; private set; }

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
            {
                throw new InvalidOperationException("The Open method was called on a device which is already connected.  Please call the Close method before calling the Open method to ensure that connections are cleaned up properly.");
            }

            // Remember the endpoint
            EndPoint = endPoint;

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
            get => _defaultConnectTimeout;
            set
            {
                // The timeout must be greater than zero
                if (value.TotalSeconds <= 0)
                {
                    throw new ArgumentOutOfRangeException("DefaultConnectTimeout", "The default connect timeout for a network device must be greater than zero.  A value of five to ten seconds is recommended.");
                }

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
        public override string Name => "Network Device";

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
            if (Socket == null || !Socket.Connected)
            {
                // Close any existing socket
                if (Socket != null)
                {
                    Socket.Close();
                }

                // Create a socket
                Socket = new Socket(AddressFamily, SocketType, ProtocolType)
                {

                    // A smaller buffer size reduces latency.  We want to process data as soon as it's transmitted
                    ReceiveBufferSize = NmeaReader.IDEAL_NMEA_BUFFER_SIZE,
                    SendBufferSize = NmeaReader.IDEAL_NMEA_BUFFER_SIZE,

                    // Specify the timeout for read/write ops
                    ReceiveTimeout = 10000, // (int)DefaultReadTimeout.TotalMilliseconds;
                    SendTimeout = 10000 // = (int)DefaultWriteTimeout.TotalMilliseconds;
                };

                // Begin connecting asynchronously
                IAsyncResult connectResult = Socket.BeginConnect(EndPoint, null, null);

                // Wait up to the timeout for the connection to complete
                if (!connectResult.AsyncWaitHandle.WaitOne((int)_defaultConnectTimeout.TotalMilliseconds, false))
                {
                    throw new TimeoutException(Name + " could not connect within the timeout period.");
                }

                Socket.EndConnect(connectResult);
            }

            // Wrap a network stream around the socket.
            return new NetworkStream(Socket, access, true);

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
            if (Socket != null)
            {
                // Close disconnects the socket and disposes it.
                Socket.Close();
            }

            // Dispose of the base, closing the stream
            base.Dispose(disposing);

            if (disposing)
            {
                EndPoint = null;
                Socket = null;
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
                SocketAddress address = EndPoint.Serialize();
                StringBuilder hexEndPoint = new();
                for (int index = 0; index < address.Size; index++)
                {
                    hexEndPoint.Append(address[index].ToString("X2"));
                }

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
            SocketAddress address = EndPoint.Serialize();
            StringBuilder hexEndPoint = new();
            for (int index = 0; index < address.Size; index++)
            {
                hexEndPoint.Append(address[index].ToString("X2"));
            }

            // Save device information
            RegistryKey deviceKey = Registry.LocalMachine.CreateSubKey(ROOT_KEY_NAME + hexEndPoint);
            if (deviceKey == null)
            {
                return;
            }

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
            SocketAddress address = EndPoint.Serialize();
            StringBuilder hexEndPoint = new();
            for (int index = 0; index < address.Size; index++)
            {
                hexEndPoint.Append(address[index].ToString("X2"));
            }

            // Save device stats
            RegistryKey deviceKey = Registry.LocalMachine.OpenSubKey(ROOT_KEY_NAME + hexEndPoint, false);
            if (deviceKey == null)
            {
                return;
            }

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