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
using System.IO;
using System.IO.Ports;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Wraps the .Net <see cref="System.IO.Ports.SerialPort"/> class and implements workarounds for some
    /// known bugs and limitations.
    /// </summary>
    internal class SerialPort
#if PocketPC
        : IDisposable
#else
 : System.IO.Ports.SerialPort
#endif
    {
#if PocketPC
        /* Older devices run into problems when trying to use .NET's SerialPort class.
         * For example, it will barf trying to open the GPS Intermediate Driver because
         * the driver may *temporarily* report error #21 until the underlying GPS port
         * is opened.  So, we need a custom SerialStream class for this case.
         */
        private SerialStream _BaseStream;
        private string _Port;
        private int _BaudRate;
#endif

        /// <summary>
        ///
        /// </summary>
        private StreamReader _reader;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.ComponentModel.Component"/> class.
        /// </summary>
        public SerialPort()
        {
        }

#if PocketPC
        public SerialPort(string portName, int baudRate)
        {
            _Port = portName;
            _BaudRate = baudRate;
        }
#else

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPort"/> class.
        /// </summary>
        /// <param name="portName">Name of the port.</param>
        /// <param name="baudRate">The baud rate.</param>
        public SerialPort(string portName, int baudRate)
            : this(portName, baudRate, Parity.None, 8, StopBits.One)
        {
        }

#endif

#if !PocketPC

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.Ports.SerialPort"/> class using the specified port name, baud rate, parity bit, data bits, and stop bit.
        /// </summary>
        /// <param name="portName">The port to use (for example, COM1).</param>
        /// <param name="baudRate">The baud rate.</param>
        /// <param name="parity">One of the <see cref="P:System.IO.Ports.SerialPort.Parity"/> values.</param>
        /// <param name="dataBits">The data bits value.</param>
        /// <param name="stopBits">One of the <see cref="P:System.IO.Ports.SerialPort.StopBits"/> values.</param>
        /// <exception cref="T:System.IO.IOException">The specified port could not be found or opened.</exception>
        public SerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : base(portName, baudRate, parity, dataBits, stopBits)
        {
            ReadTimeout = (int)Device.DefaultReadTimeout.TotalMilliseconds;
            WriteTimeout = (int)Device.DefaultWriteTimeout.TotalMilliseconds;
            NewLine = "\r\n";
            WriteBufferSize = NmeaReader.IDEAL_NMEA_BUFFER_SIZE;
            ReadBufferSize = NmeaReader.IDEAL_NMEA_BUFFER_SIZE;
            ReceivedBytesThreshold = 65535;  // We don't need this event, so max out the threshold
            Encoding = Encoding.ASCII;
        }

#endif

        #endregion Constructors

        #region Public Properties

#if PocketPC
        public string PortName
        {
            get
            {
                return _Port;
            }
            set
            {
                _Port = value;
            }
        }

        public int BaudRate
        {
            get
            {
                return _BaudRate;
            }
            set
            {
                _BaudRate = value;
            }
        }

        public SerialStream BaseStream
        {
            get
            {
                return _BaseStream;
            }
        }

        public bool IsOpen
        {
            get
            {
                return _BaseStream != null;
            }
        }

        public int ReadTimeout
        {
            get
            {
                return _BaseStream.ReadTimeout;
            }
            set
            {
                _BaseStream.ReadTimeout = value;
            }
        }
#endif

        #endregion Public Properties

        #region Public Methods

#if PocketPC
        public void Open(FileAccess access, FileShare sharing)
        {
            _BaseStream = new SerialStream(_Port, _BaudRate, access, sharing);
        }
#else

        /// <summary>
        /// Opens a new serial port connection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The specified port is open. </exception>
        ///
        /// <exception cref="T:System.ArgumentOutOfRangeException">One or more of the properties for this instance are invalid. For example, the <see cref="P:System.IO.Ports.SerialPort.Parity"/>, <see cref="P:System.IO.Ports.SerialPort.DataBits"/>, or <see cref="P:System.IO.Ports.SerialPort.Handshake"/> properties are not valid values; the <see cref="P:System.IO.Ports.SerialPort.BaudRate"/> is less than or equal to zero; the <see cref="P:System.IO.Ports.SerialPort.ReadTimeout"/> or <see cref="P:System.IO.Ports.SerialPort.WriteTimeout"/> property is less than zero and is not <see cref="F:System.IO.Ports.SerialPort.InfiniteTimeout"/>. </exception>
        ///
        /// <exception cref="T:System.ArgumentException">The port name does not begin with "COM". - or -The file type of the port is not supported.</exception>
        ///
        /// <exception cref="T:System.IO.IOException">The port is in an invalid state.  - or - An attempt to set the state of the underlying port failed. For example, the parameters passed from this <see cref="T:System.IO.Ports.SerialPort"/> object were invalid.</exception>
        ///
        /// <exception cref="T:System.UnauthorizedAccessException">Access is denied to the port. </exception>
        ///
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        ///   </PermissionSet>
        public new void Open()
        {
            base.Open();

            /* The .Net SerialStream class has a bug that causes its finalizer to crash when working
             * with virtual COM ports (e.g. FTDI, Prolific, etc.) See the following page for details:
             * http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/8a1825d2-c84b-4620-91e7-3934a4d47330
             * To work around this bug, we suppress the finalizer for the BaseStream and close it ourselves instead.
             * See the Dispose method for the other half of this workaround.
             */
            GC.SuppressFinalize(BaseStream);
        }

#endif

#if PocketPC
        public void Close()
        {
            if (_BaseStream != null)
            {
                _BaseStream.Close();
                _BaseStream = null;
            }
        }
#endif

#if PocketPC
        public void DiscardInBuffer()
        {
            _BaseStream.DiscardInBuffer();
        }
#endif

        /// <summary>
        /// Reads up to the <see cref="P:System.IO.Ports.SerialPort.NewLine"/> value in the input buffer.
        /// </summary>
        /// <returns>The contents of the input buffer up to the first occurrence of a <see cref="P:System.IO.Ports.SerialPort.NewLine"/> value.</returns>
        /// <exception cref="T:System.InvalidOperationException">The specified port is not open. </exception>
        ///
        /// <exception cref="T:System.TimeoutException">The operation did not complete before the time-out period ended.- or -No bytes were read.</exception>
        ///
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/>
        ///   </PermissionSet>
        public new string ReadLine()
        {
            /* On the HTC P3300, an OutOfMemoryException occurs when using SerialPort.ReadLine().
             * However, a StreamReader.ReadLine() works just fine.  This suggests that SerialPort.ReadLine()
             * is buggy!  Use a StreamReader to get the job done.
             */
            if (_reader == null)
            {
                _reader = new StreamReader(BaseStream, Encoding.ASCII, false, NmeaReader.IDEAL_NMEA_BUFFER_SIZE);
            }

            return _reader.ReadLine();
        }

        #endregion Public Methods

        #region Implementation of IDisposable

#if PocketPC
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
#endif

#if PocketPC
        protected void Dispose(bool disposing)
        {
            if (disposing)
                Close();
        }
#else

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Ports.SerialPort"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        /// <exception cref="T:System.IO.IOException">The port is in an invalid state.  - or -An attempt to set the state of the underlying port failed. For example, the parameters passed from this <see cref="T:System.IO.Ports.SerialPort"/> object were invalid.</exception>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    /* The .Net SerialStream class has a bug that causes its finalizer to crash when working
                     * with virtual COM ports (e.g. FTDI, Prolific, etc.) See the following page for details:
                     * http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/8a1825d2-c84b-4620-91e7-3934a4d47330
                     * To work around this bug, we suppress the finalizer for the BaseStream and close it ourselves instead.
                     * See the Open method for the other half of this workaround.
                     */
                    if (IsOpen)
                    {
                        BaseStream.Close();
                    }
                }
                catch
                {
                    // The BaseStream is already closed, disposed, or in an invalid state. Ignore and continue disposing.
                }
            }

            base.Dispose(disposing);
        }

#endif

        #endregion Implementation of IDisposable
    }
}