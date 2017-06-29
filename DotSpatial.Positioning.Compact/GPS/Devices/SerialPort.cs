using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using DotSpatial.Positioning.Gps.Nmea;

namespace DotSpatial.Positioning.Gps.IO
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

        private StreamReader _Reader;

        #region Constructors

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
        public SerialPort(string portName, int baudRate)
            : this(portName, baudRate, Parity.None, 8, StopBits.One)
        {
        }
#endif

#if !PocketPC
        public SerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
            : base(portName, baudRate, parity, dataBits, stopBits)
        {
            ReadTimeout = (int)Device.DefaultReadTimeout.TotalMilliseconds;
            WriteTimeout = (int)Device.DefaultWriteTimeout.TotalMilliseconds;
            NewLine = "\r\n";
            WriteBufferSize = NmeaReader.IdealNmeaBufferSize;
            ReadBufferSize = NmeaReader.IdealNmeaBufferSize; 
            ReceivedBytesThreshold = 65535;  // We don't need this event, so max out the threshold
            Encoding = Encoding.ASCII;
        }
#endif

        #endregion

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

        #endregion

        #region Public Methods

#if PocketPC
        public void Open(FileAccess access, FileShare sharing)
        {
            _BaseStream = new SerialStream(_Port, _BaudRate, access, sharing);
        }
#else
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

        public new string ReadLine()
        {
            /* On the HTC P3300, an OutOfMemoryException occurs when using SerialPort.ReadLine().
             * However, a StreamReader.ReadLine() works just fine.  This suggests that SerialPort.ReadLine()
             * is buggy!  Use a StreamReader to get the job done.
             */
            if (_Reader == null)
            {
                _Reader = new StreamReader(BaseStream, Encoding.ASCII, false, NmeaReader.IdealNmeaBufferSize);
            }

            return _Reader.ReadLine();
        }

        #endregion

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

        #endregion
    }
}