// ********************************************************************************************************
// Product Name: DotSpatial.Data.IO.dll Alpha
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from DotSpatial.Data.IO.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February 2008
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// The buffered binary reader was originally designed by Ted Dunsford to make shapefile reading more
    /// efficient, but ostensibly could be used for other binary reading exercises.  To use this class,
    /// simply specify the BufferSize in bytes that you would like to use and begin reading values.
    /// </summary>
    public class BufferedBinaryReader
    {
        #region Private Variables

        private readonly long _fileLength;
        private readonly ProgressMeter _progressMeter;
        private BinaryReader _binaryReader;

        private byte[] _buffer;
        private long _bufferOffset; // Position of the start of the buffer relative to the start of the file
        private int _bufferSize;
        private readonly string _fileName;
        private long _fileOffset; // position in the entire file
        private FileStream _fileStream;
        private bool _isBufferLoaded;

        private bool _isFinishedBuffering;
        private bool _isFinishedReading;
        private int _maxBufferSize = 9600000; // Approximately around ten megs, divisible by 8
        private int _readOffset;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of BufferedBinaryReader.
        /// </summary>
        ///<param name="fileName">The string path of a file to open using this BufferedBinaryReader.</param>
        public BufferedBinaryReader(string fileName)
            : this(fileName, null)
        {
            // This is just an overload that sends the default null value in for the progressHandler
        }

        /// <summary>
        /// Creates a new instance of BufferedBinaryReader, and specifies where to send progress messages.
        /// </summary>
        /// <param name="fileName">The string path of a file to open using this BufferedBinaryReader.</param>
        /// <param name="progressHandler">Any implementation of IProgressHandler for receiving progress messages.</param>
        public BufferedBinaryReader(string fileName, IProgressHandler progressHandler)
        {
            //Modified 9/22/09 by L. C. Wilson to open as read-only so it is possible to open read-only files
            //Not sure if elsewhere write access is required
            _fileName = Path.GetFullPath(fileName);
            _fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
            _binaryReader = new BinaryReader(_fileStream);

            FileInfo fi = new FileInfo(fileName);

            _fileLength = fi.Length; // TODO: This needs looking at!
            _fileOffset = 0;

            _readOffset = -1; // There is no buffer loaded.

            _bufferSize = 0;
            _bufferOffset = -1; // -1 means no buffer is loaded.

            _isFinishedBuffering = false;
            _isFinishedReading = false;
            _progressMeter = new ProgressMeter(progressHandler, "Reading from " + Path.GetFileName(fileName), _fileLength);

            if (_fileLength < 10000000) _progressMeter.StepPercent = 5;
            if (_fileLength < 5000000) _progressMeter.StepPercent = 10;
            if (_fileLength < 100000) _progressMeter.StepPercent = 50;
            if (_fileLength < 10000) _progressMeter.StepPercent = 100;
            //long testMax = _fileLength / _progressMeter.StepPercent;
            //if (testMax < (long)9600000) _maxBufferSize = Convert.ToInt32(testMax); // otherwise keep it at 96000000
        }

        /// <summary>
        /// Creates a new instance of BufferedBinaryReader.
        /// </summary>
        ///<param name="b">The byte array.</param>
        public BufferedBinaryReader(Byte[] b)
            : this(b, null)
        {
            // This is just an overload that sends the default null value in for the progressHandler
        }

        /// <summary>
        /// Creates a new instance of BufferedBinaryReader, and specifies where to send progress messages.
        /// </summary>
        ///<param name="b">The byte array.</param>
        /// <param name="progressHandler">Any implementation of IProgressHandler for receiving progress messages.</param>
        public BufferedBinaryReader(Byte[] b, IProgressHandler progressHandler)
        {
            _binaryReader = new BinaryReader(new MemoryStream(b));

            _fileLength = b.Length;
            _fileOffset = 0;

            _readOffset = -1; // There is no buffer loaded.

            _bufferSize = 0;
            _bufferOffset = -1; // -1 means no buffer is loaded.

            _isFinishedBuffering = false;
            _isFinishedReading = false;

            _progressMeter = new ProgressMeter(progressHandler, "Reading", _fileLength);

            if (_fileLength < 10000000) _progressMeter.StepPercent = 5;
            if (_fileLength < 5000000) _progressMeter.StepPercent = 10;
            if (_fileLength < 100000) _progressMeter.StepPercent = 50;
            if (_fileLength < 10000) _progressMeter.StepPercent = 100;
            //long testMax = _fileLength / _progressMeter.StepPercent;
            //if (testMax < (long)9600000) _maxBufferSize = Convert.ToInt32(testMax); // otherwise keep it at 96000000
        }

        #endregion

        #region Methods

        // This internal method attempts to advance the buffer.
        private void AdvanceBuffer()
        {
            if (_isFinishedReading)
            {
                throw new EndOfStreamException(DataStrings.EndOfFile);
            }
            if (_isFinishedBuffering == false)
            {
                if (_maxBufferSize > FileRemainder)
                {
                    // If the buffer size is greater than the remainder, load the entire remainder and close the file.
                    _bufferSize = Convert.ToInt32(FileRemainder);
                    _buffer = new byte[_bufferSize];

                    _binaryReader.Read(_buffer, 0, _bufferSize);

                    Close();
                }
                else
                {
                    _bufferSize = _maxBufferSize;
                    _buffer = new byte[_bufferSize];

                    _binaryReader.Read(_buffer, 0, _bufferSize);
                }
                _isBufferLoaded = true;
                _bufferOffset = _fileOffset;
                _readOffset = 0;
            }
            else
            {
                // We are done, so throw the event to indicate we are done.
                OnFinishedReading();
            }
        }

        /// <summary>
        /// Closes the internal binary reader and underlying file, but does not free
        /// the buffer that is in memory.  For that, call the dispose method.
        /// </summary>
        public void Close()
        {
            if (_binaryReader != null)
            {
                _binaryReader.Close();
            }
            _binaryReader = null;

            OnFinishedBuffering();
        }

        /// <summary>
        /// This will not close the file, so be sure to close before calling Dispose.
        /// This will dispose the file stream and set the buffer to null.
        /// </summary>
        public void Dispose()
        {
            _buffer = null;
            _binaryReader = null;
            _fileStream?.Dispose();
        }

        /// <summary>
        /// Instructs the reader to fill its buffer with data.  This only does something
        /// if the buffer is not loaded yet.  This method is optional since the first
        /// effort at reading the file will automatically load the buffer.
        /// </summary>
        public void FillBuffer()
        {
            if (_isBufferLoaded == false)
            {
                AdvanceBuffer();
            }
        }

        /// <summary>
        /// This method will both assign a new maximum buffer size to the reader and
        /// force the reader to load the values into memory.  This is unnecessary
        /// unless you plan on closing the file before reading values from this class.
        /// Even if values are loaded, this will assign the MaxBufferSize property
        /// so that future buffers have the specified size.
        /// </summary>
        /// <param name="maxBufferSize">An integer buffer size to assign to the maximum buffer size before reading values.</param>
        public void FillBuffer(int maxBufferSize)
        {
            _maxBufferSize = maxBufferSize;
            FillBuffer();
        }

        /// <summary>
        /// Uses the seek method to quickly reach a desired location to begin reading.
        /// This will not buffer or read values.  If the new position is beyond the end
        /// of the current buffer, the next read will load a new buffer.
        /// </summary>
        /// <param name="offset">A 64 bit integer specifying where to skip to in the file.</param>
        /// <param name="origin">A System.IO.SeekOrigin enumeration specifying how to estimate the location.</param>
        public void Seek(long offset, SeekOrigin origin)
        {
            if (offset == 0 && origin == SeekOrigin.Current) return;
            long startPosition = 0;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    startPosition = offset;
                    break;
                case SeekOrigin.Current:
                    startPosition = _readOffset + offset;
                    break;
                case SeekOrigin.End:
                    startPosition = _fileLength - offset;
                    break;
            }

            if (startPosition >= _fileLength || startPosition < 0)
            {
                // regardless of what direction, we need a start position inside the file
                throw new EndOfStreamException(DataStrings.EndOfFile);
            }

            // Only worry about resetting the buffer or repositioning the position
            // inside the buffer if a buffer is actually loaded.
            if (_isBufferLoaded)
            {
                long delta = startPosition - _fileOffset;

                if (delta > _bufferSize - _readOffset || delta < 0)
                {
                    // The new position is beyond our current buffer
                    _buffer = null;
                    _readOffset = -1;
                    _bufferOffset = -1;
                    _isBufferLoaded = false;
                    if (_isFinishedBuffering || _binaryReader == null)
                    {
                        _fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
                        _binaryReader = new BinaryReader(_fileStream);
                    }
                    _isFinishedBuffering = false;
                }
                else
                {
                    // The new position is still inside the buffer
                    _readOffset += Convert.ToInt32(delta);
                    _fileOffset += delta;
                    // we don't want to actually seek in the internal reader
                    return;
                }
            }
            // If no buffer is loaded, the file may not be open and may cause an exception when trying to seek.
            // probably better for tracking than not throwing one.

            _fileOffset = startPosition;
            if (_fileStream.CanSeek) _fileStream.Seek(offset, origin);
        }

        #region Read Methods


        /// <summary>
        /// Gets the progress meter.
        /// </summary>
        public ProgressMeter ProgressMeter
        {
            get
            {
                return _progressMeter;
            }
        }

        /// <summary>
        /// Reads a boolean form the buffer, automatcially loading the next buffer if necessary.
        /// </summary>
        /// <returns>A boolean value converted from bytes in the file.</returns>
        public bool ReadBool()
        {
            byte[] data = ReadBytes(1);
            return BitConverter.ToBoolean(data, 0);
        }

        /// <summary>
        /// Reads a character from two bytes in the buffer, automatically loading the next buffer if necessary.
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            byte[] data = ReadBytes(2);
            return BitConverter.ToChar(data, 0);
        }

        /// <summary>
        /// Reads an array of character from two bytes in the buffer, automatically loading
        /// </summary>
        /// <returns></returns>
        public char[] ReadChars(int count)
        {
            byte[] data = ReadBytes(count * 2);
            char[] result = new char[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = BitConverter.ToChar(data, i * 2);
            }
            return result;
        }

        /// <summary>
        /// Reads a double-precision floating point from 8 bytes in the buffer, automatically loading the next buffer if necessary.
        /// </summary>
        /// <param name="isLittleEndian">Boolean, true if the value should be returned with little endian byte ordering.</param>
        /// <returns>A double value converted from bytes in the file.</returns>
        public double ReadDouble(bool isLittleEndian = true)
        {
            // Integers are 8 Bytes long.

            byte[] data = ReadBytes(8);
            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                // The IsLittleEndian property on the BitConverter tells us what the system is using by default.
                // The isLittleEndian argument tells us what bit order the output should be in.
                Array.Reverse(data);
            }

            double result = BitConverter.ToDouble(data, 0);
            return result;
        }

        /// <summary>
        /// Reads double precision X and Y values that are interwoven
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public Vertex[] ReadVertices(int count)
        {
            int len = count * 16;
            byte[] rawBytes = new byte[len];
            Read(rawBytes, 0, len);
            Vertex[] result = new Vertex[count];
            Buffer.BlockCopy(rawBytes, 0, result, 0, len);
            return result;
        }

        /// <summary>
        /// Reads the specified number of doubles into an array
        /// This uses Buffer.CopyBlock, and seems to work ok
        /// for little-endian in windows.
        /// </summary>
        /// <param name="count">The count of doubles</param>
        /// <returns>An array of doubles</returns>
        public double[] ReadDoubles(int count)
        {
            byte[] rawBytes = new byte[count * 8];
            Read(rawBytes, 0, count * 8);
            double[] result = new double[count];
            Buffer.BlockCopy(rawBytes, 0, result, 0, count * 8);
            return result;
        }

        /// <summary>
        /// Reads the specified number of integers into an array
        /// </summary>
        /// <param name="count">The integer count of integers to read</param>
        /// <returns>An array of the specified integers and length equal to count</returns>
        public int[] ReadIntegers(int count)
        {
            byte[] rawBytes = new byte[count * 4];
            Read(rawBytes, 0, count * 4);
            int[] result = new int[count];
            Buffer.BlockCopy(rawBytes, 0, result, 0, count * 4);
            return result;
        }

        /// <summary>
        /// By default, this will use little Endian ordering.
        /// </summary>
        /// <returns>An Int32 converted from the file.</returns>
        public int ReadInt32()
        {
            return ReadInt32(true);
        }

        /// <summary>
        /// Reads an integer from the file, using the isLittleEndian argument
        /// to decide whether to flip the bits or not.
        /// </summary>
        /// <param name="isLittleEndian">Boolean, true if the value should be returned with little endian byte ordering.</param>
        /// <returns>an Int32 value converted from bytes in the file.</returns>
        public int ReadInt32(bool isLittleEndian)
        {
            // Integers are 4 Bytes long.
            byte[] data = ReadBytes(4);

            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                // The IsLittleEndian property on the BitConverter tells us what the system is using by default.
                // The isLittleEndian argument tells us what bit order the output should be in.
                Array.Reverse(data);
            }

            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// Reads a short, sixteen bit integer as bytes in little-endian order
        /// </summary>
        /// <returns>A short value</returns>
        public short ReadInt16()
        {
            byte[] data = ReadBytes(2);
            return BitConverter.ToInt16(data, 0);
        }

        /// <summary>
        /// Reads a single-precision floading point from 4 bytes in the buffer, automatically loading the next buffer if necessary.
        /// This assumes the value should be little endian.
        /// </summary>
        /// <returns>A single-precision floating point converted from four bytes</returns>
        public float ReadSingle()
        {
            return ReadSingle(true);
        }

        /// <summary>
        /// Reads a single-precision floading point from 4 bytes in the buffer, automatically loading the next buffer if necessary.
        /// </summary>
        /// <param name="isLittleEndian">Boolean, true if the value should be returned with little endian byte ordering.</param>
        /// <returns>A single-precision floating point converted from four bytes</returns>
        public float ReadSingle(bool isLittleEndian)
        {
            // Integers are 4 Bytes long.
            byte[] data = ReadBytes(4);

            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                // The IsLittleEndian property on the BitConverter tells us what the system is using by default.
                // The isLittleEndian argument tells us what bit order the output should be in.
                Array.Reverse(data);
            }

            return BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// Reads the specified number of bytes.  This will throw an exception
        /// if a number of bytes is specified that exeeds the file length.
        /// </summary>
        /// <param name="byteCount">The integer count of the bytes.</param>
        /// <returns>An array of bytes</returns>
        public byte[] ReadBytes(int byteCount)
        {
            byte[] result = new byte[byteCount];
            Read(result, 0, byteCount);
            return result;
        }

        /// <summary>
        /// copies count bytes from the internal buffer to the specified buffer index as the starting point in the specified buffer.
        /// </summary>
        /// <param name="buffer">A previously dimensioned array of byte values to fill with data</param>
        /// <param name="index">The index in the argument array to start pasting values to</param>
        /// <param name="count">The number of values to copy into the parameter buffer</param>
        public void Read(byte[] buffer, int index, int count)
        {
            bool finished = false;
            int bytesPasted = 0;
            if (_isBufferLoaded == false) AdvanceBuffer();
            do
            {
                int bytesInBuffer = _bufferSize - _readOffset;

                if (count - bytesPasted <= bytesInBuffer)
                {
                    // Read the specified bytes and advance

                    Buffer.BlockCopy(_buffer, _readOffset, buffer, index, count - bytesPasted);
                    _readOffset += count - bytesPasted;
                    _fileOffset += count - bytesPasted;
                    if (_fileOffset >= _fileLength)
                    {
                        OnFinishedReading(); // We got to the end of the file.
                    }
                    finished = true;
                }
                else
                {
                    // Read what we can from the array and then advance the buffer
                    Buffer.BlockCopy(_buffer, _readOffset, buffer, index, bytesInBuffer);
                    index += bytesInBuffer;
                    bytesPasted += bytesInBuffer;
                    _fileOffset += bytesInBuffer;

                    if (bytesPasted >= count)
                    {
                        // Sometimes there might be less than count bytes left in the file.
                        // In those cases, we actually finish reading down here instead.
                        if (_fileOffset >= _fileLength)
                        {
                            OnFinishedReading(); // We reached the end of the file
                        }
                        finished = true; // Even if we aren't at the end of the file, we are done with this read session
                    }
                    else
                    {
                        // We haven't pasted enough bytes, so advance the buffer and continue reading
                        AdvanceBuffer();
                    }
                }
            }
            while (finished == false);
            if (_isFinishedReading == false) _progressMeter.CurrentValue = _fileOffset;
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets a long integer specifying the starting position of the currently loaded buffer
        /// relative to the start of the file.  A value of -1 indicates that no buffer is
        /// currently loaded.
        /// </summary>
        public long BufferOffset
        {
            get { return _bufferOffset; }
        }

        /// <summary>
        /// Gets an integer value specifying the size of the buffer currently loaded into memory.
        /// This will either be the MaxBufferSize, or a smaller buffer representing a smaller
        /// remainder existing in the file.
        /// </summary>
        public int BufferSize
        {
            get { return _bufferSize; }
        }

        /// <summary>
        /// Gets a boolean indicating whether there is currently any information loaded into the buffer.
        /// </summary>
        public bool IsBufferLoaded
        {
            get { return _isBufferLoaded; }
        }

        /// <summary>
        /// Gets a boolean value once the offset has reached the end of the file,
        /// and every byte value has been read from the file.
        /// </summary>
        public bool IsFinishedReading
        {
            get { return _isFinishedReading; }
        }

        /// <summary>
        /// Gets a boolean value once the entire file has been loaded into memory.
        /// This usually will occur before any reading even takes place.
        /// </summary>
        public bool IsFinishedBuffering
        {
            get { return _isFinishedBuffering; }
        }

        /// <summary>
        /// Gets the length in bytes of the file being read.
        /// </summary>
        public long FileLength
        {
            get { return _fileLength; }
        }

        /// <summary>
        /// Gets the current read position in the file in bytes.
        /// </summary>
        public long FileOffset
        {
            get { return _fileOffset; }
        }

        /// <summary>
        /// Gets a long value specifying how many bytes have not yet been read in the file.
        /// </summary>
        public long FileRemainder
        {
            get { return _fileLength - _fileOffset; }
        }

        /// <summary>
        /// Gets or sets the buffer size to read in chunks.  This does not
        /// describe the size of the actual
        /// </summary>
        public int MaxBufferSize
        {
            get { return _maxBufferSize; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(DataStrings.ArgumentCannotBeNegative_S.Replace("%S", "BufferSize"));
                }
                _maxBufferSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the progress message that has no percentage as part of it.
        /// </summary>
        public string ProgressBaseMessage
        {
            get { return _progressMeter.Key; }
            set { _progressMeter.Key = value; }
        }

        /// <summary>
        /// This acts like a placeholder on the buffer and indicates where reading will begin (relative to the start of the buffer)
        /// </summary>
        public int ReadOffset
        {
            get { return _readOffset; }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when this reader has read every byte from the file.
        /// </summary>
        public event EventHandler FinishedReading;

        /// <summary>
        /// Fires the FinishedReading event.
        /// </summary>
        protected virtual void OnFinishedReading()
        {
            _isFinishedReading = true;
            _progressMeter.Reset();
            _buffer = null;
            if (FinishedReading == null) return;
            FinishedReading(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when the end of the last portion of the file has been
        /// loaded into the file and the file has been closed.
        /// </summary>
        public event EventHandler FinishedBuffering;

        /// <summary>
        /// Fires the FinishedBuffering event.
        /// </summary>
        protected virtual void OnFinishedBuffering()
        {
            _isFinishedBuffering = true;

            if (FinishedBuffering == null) return;
            FinishedBuffering(this, EventArgs.Empty);
        }

        #endregion
    }
}