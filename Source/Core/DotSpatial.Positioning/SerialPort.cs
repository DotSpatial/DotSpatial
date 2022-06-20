// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

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
 : System.IO.Ports.SerialPort
    {
        /// <summary>
        ///
        /// </summary>
        private StreamReader _reader;
        private readonly bool onMono = Type.GetType("Mono.Runtime") != null;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.ComponentModel.Component"/> class.
        /// </summary>
        public SerialPort()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialPort"/> class.
        /// </summary>
        /// <param name="portName">Name of the port.</param>
        /// <param name="baudRate">The baud rate.</param>
        public SerialPort(string portName, int baudRate)
            : this(portName, baudRate, Parity.None, 8, StopBits.One)
        {
        }

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
            if (!onMono) // ReceivedBytesThreshold is not implemented on mono, thus throwing an exception.
            {
                ReceivedBytesThreshold = 65535;  // We don't need this event, so max out the threshold
            }

            Encoding = Encoding.ASCII;
        }

        #endregion Constructors

        #region Public Properties

        #endregion Public Properties

        #region Public Methods

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

        #endregion Implementation of IDisposable
    }
}