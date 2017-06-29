using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using DotSpatial.Positioning.Gps.Nmea;
using Microsoft.Win32;
#if !PocketPC || ICodeInAVacuum
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning.Gps.IO
{
    /// <summary>
    /// Represents a GPS device which is accessed via a socket connection.
    /// </summary>
    public class NetworkDevice : Device
    {
        private AddressFamily _AddressFamily;
        private SocketType _SocketType;
        private ProtocolType _ProtocolType;
        private EndPoint _EndPoint;
        private Socket _Socket;

        private static TimeSpan _DefaultConnectTimeout = TimeSpan.FromSeconds(30);

        #region Constants

        private const string RootKeyName = Devices.RootKeyName + @"Network\";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a NetworkDevice from the specified parameters
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <param name="socketType"></param>
        /// <param name="protocolType"></param>
        public NetworkDevice(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
            : this(addressFamily, socketType, protocolType, null)
        { }

        /// <summary>
        /// Creates a NetworkDevice from the specified parameters
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <param name="socketType"></param>
        /// <param name="protocolType"></param>
        /// <param name="endPoint"></param>
        public NetworkDevice(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, EndPoint endPoint)
        {
            _AddressFamily = addressFamily;
            _SocketType = socketType;
            _ProtocolType = protocolType;
            _EndPoint = endPoint;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns the addressing scheme the socket can use. 
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the addressing scheme the socket can use. ")]
        [Browsable(true)]
#endif
        public AddressFamily AddressFamily
        {
            get
            {
                return _AddressFamily;
            }
        }

        /// <summary>
        /// Returns the type of protocol used by the socket.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the type of protocol used by the socket.")]
        [Browsable(true)]
#endif
        public SocketType SocketType
        {
            get
            {
                return _SocketType;
            }
        }

        /// <summary>
        /// Returns the protocol supported by the socket.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the protocol supported by the socket.")]
        [Browsable(true)]
#endif
        public ProtocolType ProtocolType
        {
            get
            {
                return _ProtocolType;
            }
        }

        /// <summary>
        /// Returns the network address for the device. 
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the network address for the device.")]
        [Browsable(true)]
        [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
        public EndPoint EndPoint
        {
            get
            {
                return _EndPoint;
            }
            set
            {
                _EndPoint = value;
            }
        }

        /// <summary>
        /// Returns the socket associated with this device.
        /// </summary>
#if !PocketPC
        [Browsable(false)]
#endif
        protected Socket Socket
        {
            get { return _Socket; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new connection to the specified endpoint.
        /// </summary>
        /// <param name="endPoint"></param>
        public void Open(EndPoint endPoint)
        {
            // If we're already open, complain.
            if (IsOpen)
                throw new InvalidOperationException("The Open method was called on a device which is already connected.  Please call the Close method before calling the Open method to ensure that connections are cleaned up properly.");

            // Remember the endpoint
            _EndPoint = endPoint;

            // And continue opening a connection
            Open();
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Controls the amount of time allowed for a connection to be successfully opened.
        /// </summary>
        public static TimeSpan DefaultConnectTimeout
        {
            get { return _DefaultConnectTimeout; }
            set
            {
                // The timeout must be greater than zero
                if (value.TotalSeconds <= 0)
                    throw new ArgumentOutOfRangeException("DefaultConnectTimeout", "The default connect timeout for a network device must be greater than zero.  A value of five to ten seconds is recommended.");

                // Set the value
                _DefaultConnectTimeout = value;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Occurs immediately before a socket is opened.
        /// </summary>
        protected virtual void OnConfigureSocket()
        { }

        #endregion

        #region Overrides

        public override string Name
        {
            get
            {
                return "Network Device";
            }
        }

        protected override Stream OpenStream(FileAccess access, FileShare sharing)
        {
            // Give the consumer a chance to set socket options
            OnConfigureSocket();

            // Is the socket already connected?
            if (_Socket == null || !_Socket.Connected)
            {
                // Close any exsisting socket
                if (_Socket != null)
                    _Socket.Close();

                // Create a socket 
                _Socket = new Socket(_AddressFamily, _SocketType, _ProtocolType);

#if !PocketPC
                // A smaller buffer size reduces latency.  We want to process data as soon as it's transmitted
                _Socket.ReceiveBufferSize = NmeaReader.IdealNmeaBufferSize;
                _Socket.SendBufferSize = NmeaReader.IdealNmeaBufferSize;

                // Specify the timeout for read/write ops
                _Socket.ReceiveTimeout = 10000; // (int)DefaultReadTimeout.TotalMilliseconds;
                _Socket.SendTimeout = 10000; // = (int)DefaultWriteTimeout.TotalMilliseconds;
#endif
                // Begin connecting asynchronously
                IAsyncResult connectResult = _Socket.BeginConnect(_EndPoint, null, null);

                // Wait up to the timeout for the connection to complete
                if (!connectResult.AsyncWaitHandle.WaitOne((int)_DefaultConnectTimeout.TotalMilliseconds, false))
                    throw new TimeoutException(Name + " could not connect within the timeout period.");

                _Socket.EndConnect(connectResult);
            }

            // Wrap a network stream around the socket. 
            return new NetworkStream(_Socket, access, true);

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

            #region Flip-floppy ownership

            //#if PocketPC

//            /* EXTREMELY IMPORTANT
//             * 
//             * The .NET implementation of the Socket and NetworkStream classes is such that
//             * any attempt to close a socket will prevent all future connection attempts!
//             * I'm not sure exactly why, but I believe that a simple close is inadvertantly
//             * shutting down the entire Bluetooth socket system.  This appears to only
//             * affect the Compact Framework.
//             * 
//             * As a result, it's very important to leave "ownsSocket" as FALSE.
//             */

//            return new NetworkStream(_Socket, access, false);
//            //                                        ^^^^^ very important to be False.
//#else
//            // And wrap a network stream around it
//            NetworkStream result = new NetworkStream(_Socket, access, true);

//            // Specify read/write timeouts
//            result.ReadTimeout = Convert.ToInt32(DefaultReadTimeout.TotalMilliseconds);
//            result.WriteTimeout = Convert.ToInt32(DefaultWriteTimeout.TotalMilliseconds);

//            return result;
//#endif

            #endregion

        }

        protected override void Dispose(bool disposing)
        {
            if (_Socket != null)
            {
                // Close disconnects the socket and disposes it.
                _Socket.Close();
            }

            // Dispose of the base, closing the stream
            base.Dispose(disposing);

            if (disposing)
            {
                _EndPoint = null;
                _Socket = null;
            }
        }

        protected override void OnCacheRemove()
        {
            try
            {
                // Endpoints are serialized in their raw byte form to improve compatibility                
                SocketAddress address = _EndPoint.Serialize();
                StringBuilder hexEndPoint = new StringBuilder();
                for (int index = 0; index < address.Size; index++)
                    hexEndPoint.Append(address[index].ToString("X2"));

                Registry.LocalMachine.DeleteSubKeyTree(RootKeyName + hexEndPoint.ToString());
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

        protected override void OnCacheWrite()
        {
            // Endpoints are serialized in their raw byte form to improve compatibility                
            SocketAddress address = _EndPoint.Serialize();
            StringBuilder hexEndPoint = new StringBuilder();
            for (int index = 0; index < address.Size; index++)
                hexEndPoint.Append(address[index].ToString("X2"));

            // Save device information
            RegistryKey deviceKey = Registry.LocalMachine.CreateSubKey(RootKeyName + hexEndPoint.ToString());
            if (deviceKey == null)
                return;

            // Update the success/fail statistics
            deviceKey.SetValue("Number of Times Detected", SuccessfulDetectionCount);
            deviceKey.SetValue("Number of Times Failed", FailedDetectionCount);
            deviceKey.SetValue("Date Last Detected", DateDetected.ToString("R", CultureInfo.InvariantCulture));
            deviceKey.SetValue("Date Last Connected", DateConnected.ToString("R", CultureInfo.InvariantCulture));
            deviceKey.Close();
        }

        protected override void OnCacheRead()
        {
            // Endpoints are serialized in their raw byte form to improve compatibility                
            SocketAddress address = _EndPoint.Serialize();
            StringBuilder hexEndPoint = new StringBuilder();
            for (int index = 0; index < address.Size; index++)
                hexEndPoint.Append(address[index].ToString("X2"));

            // Save device stats
            RegistryKey deviceKey = Registry.LocalMachine.OpenSubKey(RootKeyName + hexEndPoint.ToString(), false);
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

        #endregion
    }
}
