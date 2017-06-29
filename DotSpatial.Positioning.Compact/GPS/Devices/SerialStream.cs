#if PocketPC

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using GeoFramework.Gps.Nmea;

namespace GeoFramework.Gps.IO
{
    /// <summary>
    /// Represents a stream-based interface to a serial device.
    /// </summary>
    public sealed class SerialStream : Stream
    {
        private string _PortName;
        private IntPtr _Handle = NativeMethods.INVALID_HANDLE;      // The unmanaged handle to the GPSID port
        private uint _BytesTransmitted = 0;                         // A very frequently used var, stores the # of bytes read
        private byte[] _SingleByteBuffer = new byte[1];             // A very frequently used var, stores a single byte
        private bool _Result;                                       // A very frequently used var, stores the return code of API calls
        private NativeMethods.DCB _DCB = new NativeMethods.DCB();   // Stores current configuration of the port
        private NativeMethods.COMMTIMEOUTS _Timeouts = new NativeMethods.COMMTIMEOUTS();   // Stores read/write timeout operations
        private NativeMethods.COMMSTAT _CommStat = new NativeMethods.COMMSTAT();
        private FileAccess _Access;

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified port name.
        /// </summary>
        /// <param name="portName"></param>
        public SerialStream(string portName)
            : this(portName, 4800)
        {}

        /// <summary>
        /// Creates a new instance using the specified port name and baud rate.
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        public SerialStream(string portName, int baudRate)
            : this(portName, baudRate, FileAccess.Read, FileShare.Read)
        {}

        /// <summary>
        /// Creates a new instance using the specified port name, baud rate, read/write access and sharing modes.
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="baudRate"></param>
        /// <param name="access"></param>
        /// <param name="sharing"></param>
        public SerialStream(string portName, int baudRate, FileAccess access, FileShare sharing)
        {
            try
            {
                // Remember the port name and baud rate
                _PortName = portName;
                _Access = access;
                BaudRate = baudRate;

                // Open a connection to the port
                _Handle = NativeMethods.CreateFile(
                    portName,
                    access,
                    sharing,
                    0,
                    FileMode.Open,
                    FileAttributes.Normal,
                    IntPtr.Zero);

                // Throw if we have no handle
                if (_Handle == NativeMethods.INVALID_HANDLE)
                    CheckError();

                /* We may need a few tries before the port's in a state we can work with.
                 * For example, the GPSID may need a few seconds to open the actual GPS device 
                 * port.
                 */
                int tries, errorCode = 0;
                for (tries = 0; tries < 16; tries++)
                {
                    // Get the configuration information for the port 
                    _DCB.DCBLength = (uint)Marshal.SizeOf(_DCB);                    // 44
                    _Result = NativeMethods.GetCommState(_Handle, ref _DCB);
                    
                    // Did it succeed?  If so, stop looping
                    if (_Result) break;

                    // It didn't succeed.  What exactly happened?
                    errorCode = Marshal.GetLastWin32Error();

                    /* On the GPSID, I'm getting error 21 (Device Not Ready), which is recorverable.
                     * Wait a moment, then try again.
                     */
                    switch (errorCode)
                    {
                        case 21: // Device not ready
                            System.Threading.Thread.Sleep(250);
                            continue;
                    }

                    // Throw the exception
                    throw new Win32Exception(errorCode);
                }

                // If it failed after 4 seconds and is still error 21 (Device not ready), then give up.
                if (errorCode == 21 && tries == 16)
                    throw new Win32Exception(errorCode); 

                /* Now, make some modifications, mainly the baud rate.  We want the fastest possible baud rate
                 * in order to reduce latency (the amount of time between a GPS sending data and the time it's 
                 * actually parsed).   Fortunately, the GPSID is supportive of high baud rates.
                 */
                BaudRate = baudRate;

                // We'll be using a smaller buffer size of around 128 bytes in order to more quickly get an NMEA sentence
                SetLength(NmeaReader.IdealNmeaBufferSize);

                /* Configure our timeout behavior. According to MSDN (http://msdn.microsoft.com/en-us/library/aa363190(VS.85).aspx)
                 * we can get read operations to return immediately.
                 */
                _Timeouts.ReadIntervalTimeout = 0;

                // Timeout "multipliers" (per-byte timeouts) are not necessary
                _Timeouts.ReadTotalTimeoutMultiplier = 0;
                _Timeouts.WriteTotalTimeoutMultiplier = 0;

                // Set default read/write timeouts
                _Timeouts.ReadTotalTimeoutConstant = (uint)SerialDevice.DefaultReadTimeout.TotalMilliseconds;
                _Timeouts.WriteTotalTimeoutConstant = (uint)SerialDevice.DefaultWriteTimeout.TotalMilliseconds;

                // Set timeout values
                _Result = NativeMethods.SetCommTimeouts(_Handle, ref _Timeouts);
                if (!_Result) CheckError();

                /* We don't really need to clear errors since we don't use fAbortOnError.  However,
                 * we can 
                 */
                ClearError();
            }
            catch
            {
                // Argh, we got an error!  Close the port.
                Close();

                // And re-throw
                throw;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Controls the speed of communications for this device.
        /// </summary>
        /// <remarks>This property controls the speed of read and write operations for the device.  The baud rate specified must precisely
        /// match a rate supported by the device, otherwise data will be unrecognizable.  GPS devices are required to support a minimum baud rate of 4800
        /// in order to comply with the NMEA-0183 standard (though not all devices do this).  A higher rate is preferable.</remarks>
        public int BaudRate
        {
            get
            {
                return (int)_DCB.BaudRate;
            }
            set
            {
                // Set the new baud rate
                _DCB.BaudRate = (uint)value;

                // Configure the port
                if (_Handle != NativeMethods.INVALID_HANDLE)
                {
                    _Result = NativeMethods.SetCommState(_Handle, ref _DCB);
                    if (!_Result) CheckError();
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Ignores any characters waiting to be read from the device.
        /// </summary>
        public void DiscardInBuffer()
        {
            NativeMethods.PurgeComm(_Handle, 0x0008);
        }

        /// <summary>
        /// Ignores any characters waiting to be sent to the device.
        /// </summary>
        public void DiscardOutBuffer()
        {
            NativeMethods.PurgeComm(_Handle, 0x0004);
        }        

        #endregion

        #region Overrides

        public override bool CanRead
        {
            get
            {
                return _Handle != NativeMethods.INVALID_HANDLE
                    && (_Access == FileAccess.Read || _Access == FileAccess.ReadWrite);
            }
        }

        public override bool CanWrite
        {
            get
            {
                return _Handle != NativeMethods.INVALID_HANDLE
                    && (_Access == FileAccess.Write || _Access == FileAccess.ReadWrite);
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {            
            _Result = NativeMethods.ReadFile(_Handle, buffer, (uint)count, out _BytesTransmitted, IntPtr.Zero);
            if (!_Result) CheckError();
            return (int)_BytesTransmitted;
        }

        public override int ReadByte()
        {
            _Result = NativeMethods.ReadFile(_Handle, _SingleByteBuffer, 1, out _BytesTransmitted, IntPtr.Zero);
            if (!_Result) CheckError();
            return (int)_SingleByteBuffer[0];
        }

        public override int ReadTimeout
        {
            get
            {
                return (int)_Timeouts.ReadTotalTimeoutConstant;
            }
            set
            {
                _Timeouts.ReadTotalTimeoutConstant = (uint)value;
                _Result = NativeMethods.SetCommTimeouts(_Handle, ref _Timeouts);
                if (!_Result) CheckError();
            }
        }

        public override int WriteTimeout
        {
            get
            {
                return (int)_Timeouts.WriteTotalTimeoutConstant;
            }
            set
            {
                _Timeouts.WriteTotalTimeoutConstant = (uint)value;
                _Result = NativeMethods.SetCommTimeouts(_Handle, ref _Timeouts);
                if (!_Result) CheckError();
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _Result = NativeMethods.WriteFile(_Handle, buffer, (uint)count, out _BytesTransmitted, IntPtr.Zero);
            if (!_Result) CheckError();
        }

        public override void WriteByte(byte value)
        {
            _SingleByteBuffer[0] = value;
            _Result = NativeMethods.WriteFile(_Handle, _SingleByteBuffer, 1, out _BytesTransmitted, IntPtr.Zero);
            if (!_Result) CheckError();
        }

        public override string ToString()
        {
            if (CanRead)
                return _PortName + " at " + _DCB.BaudRate.ToString() + " baud [Open]";
            else
                return _PortName + " at " + _DCB.BaudRate.ToString() + " baud [Closed]";
        }

        public override void Close()
        {
            // We no longer need to finalize
            GC.SuppressFinalize(this);

            // Is the stream already closed?
            if (_Handle == NativeMethods.INVALID_HANDLE)
                return;

            // Close the handle
            _Result = NativeMethods.CloseHandle(_Handle);
            if (!_Result) CheckError();

            // And clear it
            _Handle = NativeMethods.INVALID_HANDLE;
        }

        protected override void Dispose(bool disposing)
        {
            Close();
        }

        public override void Flush()
        { 
            // This method has no effect because we use no internal buffering.
        }

        private void ClearError()
        {
            uint flags = 0x1e;
            _Result = NativeMethods.ClearCommError(_Handle, ref flags, _CommStat);
            if (!_Result) CheckError();
        }

        public override long Length
        {
            get 
            {
                ClearError();
                return _CommStat.dbInQue;
            }
        }

        public override long Position
        {
            get
            {
                return 0;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            // Pass along the buffer size to the port
            _Result = NativeMethods.SetupComm(_Handle, (uint)value, (uint)value);
            if (!_Result) CheckError();
        }

        #endregion

        #region Internal Methods

        private void CheckError()
        {
            int errorCode = Marshal.GetLastWin32Error();
            switch (errorCode)
            {
                case 0:   // No error
                    return;
                case 55: // Device no longer available
                    throw new IOException(_PortName + " is not available.");
                case 31: // Device is not functioning
                    throw new IOException(_PortName + " is not responding.  It may be turned off or unplugged.");
                default:  // Unhandled/unspecific error

                    //....................../´¯/)
                    //....................,/¯../
                    //.................../..../
                    //............./´¯/'...'/´¯¯`·¸
                    //........../'/.../..../......./¨¯\
                    //........('(...´...´.... ¯~/'...')
                    //.........\.................'...../
                    //..........''...\.......... _.·´
                    //............\..............(
                    //..............\.............\... 

                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        #endregion

    }
}

#endif