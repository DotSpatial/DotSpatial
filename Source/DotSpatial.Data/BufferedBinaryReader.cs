// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// The buffered binary reader was originally designed by Ted Dunsford to make shapefile reading more
    /// efficient, but ostensibly could be used for other binary reading exercises. To use this class,
    /// simply specify the BufferSize in bytes that you would like to use and begin reading values.
    /// </summary>
    public class BufferedBinaryReader
    {
        private readonly string _fileName;

        private BinaryReader _binaryReader;
        private byte[] _buffer;

        private FileStream _fileStream;

        private int _maxBufferSize = 9600000; // Approximately around ten megs, divisible by 8

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedBinaryReader"/> class.
        /// </summary>
        /// <param name="fileName">The string path of a file to open using this BufferedBinaryReader.</param>
        public BufferedBinaryReader(string fileName)
            : this(fileName, null)
        {
            // This is just an overload that sends the default null value in for the progressHandler
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedBinaryReader"/> class and specifies where to send progress messages.
        /// </summary>
        /// <param name="fileName">The string path of a file to open using this BufferedBinaryReader.</param>
        /// <param name="progressHandler">Any implementation of IProgressHandler for receiving progress messages.</param>
        public BufferedBinaryReader(string fileName, IProgressHandler progressHandler)
        {
            // Modified 9/22/09 by L. C. Wilson to open as read-only so it is possible to open read-only files
            // Not sure if elsewhere write access is required
            _fileName = Path.GetFullPath(fileName);
            _fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
            _binaryReader = new BinaryReader(_fileStream);

            FileInfo fi = new FileInfo(fileName);

            FileLength = fi.Length;
            FileOffset = 0;

            ReadOffset = -1; // There is no buffer loaded.

            BufferSize = 0;
            BufferOffset = -1; // -1 means no buffer is loaded.

            IsFinishedBuffering = false;
            IsFinishedReading = false;
            ProgressMeter = new ProgressMeter(
                progressHandler,
                "Reading from " + Path.GetFileName(fileName),
                FileLength);

            if (FileLength < 10000000) ProgressMeter.StepPercent = 5;
            if (FileLength < 5000000) ProgressMeter.StepPercent = 10;
            if (FileLength < 100000) ProgressMeter.StepPercent = 50;
            if (FileLength < 10000) ProgressMeter.StepPercent = 100;

            // long testMax = _fileLength / _progressMeter.StepPercent;
            // if (testMax < (long)9600000) _maxBufferSize = Convert.ToInt32(testMax); // otherwise keep it at 96000000
        }

        /// <summary>
        /// Occurs when the end of the last portion of the file has been
        /// loaded into the file and the file has been closed.
        /// </summary>
        public event EventHandler FinishedBuffering;

        /// <summary>
        /// Occurs when this reader has read every byte from the file.
        /// </summary>
        public event EventHandler FinishedReading;

        /// <summary>
        /// Gets a long integer specifying the starting position of the currently loaded buffer
        /// relative to the start of the file. A value of -1 indicates that no buffer is
        /// currently loaded.
        /// </summary>
        public long BufferOffset { get; private set; }

        /// <summary>
        /// Gets an integer value specifying the size of the buffer currently loaded into memory.
        /// This will either be the MaxBufferSize, or a smaller buffer representing a smaller
        /// remainder existing in the file.
        /// </summary>
        public int BufferSize { get; private set; }

        /// <summary>
        /// Gets the length in bytes of the file being read.
        /// </summary>
        public long FileLength { get; }

        /// <summary>
        /// Gets the current read position in the file in bytes.
        /// </summary>
        public long FileOffset { get; private set; }

        /// <summary>
        /// Gets a long value specifying how many bytes have not yet been read in the file.
        /// </summary>
        public long FileRemainder => FileLength - FileOffset;

        /// <summary>
        /// Gets a value indicating whether there is currently any information loaded into the buffer.
        /// </summary>
        public bool IsBufferLoaded { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the entire file has been loaded into memory.
        /// This usually will occur before any reading even takes place.
        /// </summary>
        public bool IsFinishedBuffering { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the offset has reached the end of the file.
        /// and every byte value has been read from the file.
        /// </summary>
        public bool IsFinishedReading { get; private set; }

        /// <summary>
        /// Gets or sets the buffer size to read in chunks. This does not
        /// describe the size of the actual buffer.
        /// </summary>
        public int MaxBufferSize
        {
            get
            {
                return _maxBufferSize;
            }

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
            get
            {
                return ProgressMeter.Key;
            }

            set
            {
                ProgressMeter.Key = value;
            }
        }

        /// <summary>
        /// Gets the progress meter.
        /// </summary>
        public ProgressMeter ProgressMeter { get; }

        /// <summary>
        /// Gets the read offset. This acts like a placeholder on the buffer and indicates where reading will begin (relative to the start of the buffer).
        /// </summary>
        public int ReadOffset { get; private set; }

        /// <summary>
        /// Closes the internal binary reader and underlying file, but does not free
        /// the buffer that is in memory. For that, call the dispose method.
        /// </summary>
        public void Close()
        {
            _binaryReader?.Close();
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
            _fileStream.Dispose();
        }

        /// <summary>
        /// Instructs the reader to fill its buffer with data. This only does something
        /// if the buffer is not loaded yet. This method is optional since the first
        /// effort at reading the file will automatically load the buffer.
        /// </summary>
        public void FillBuffer()
        {
            if (!IsBufferLoaded)
            {
                AdvanceBuffer();
            }
        }

        /// <summary>
        /// This method will both assign a new maximum buffer size to the reader and
        /// force the reader to load the values into memory. This is unnecessary
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
        /// copies count bytes from the internal buffer to the specified buffer index as the starting point in the specified buffer.
        /// </summary>
        /// <param name="buffer">A previously dimensioned array of byte values to fill with data</param>
        /// <param name="index">The index in the argument array to start pasting values to</param>
        /// <param name="count">The number of values to copy into the parameter buffer</param>
        public void Read(byte[] buffer, int index, int count)
        {
            bool finished = false;
            int bytesPasted = 0;
            if (IsBufferLoaded == false) AdvanceBuffer();
            do
            {
                int bytesInBuffer = BufferSize - ReadOffset;

                if (count - bytesPasted <= bytesInBuffer)
                {
                    // Read the specified bytes and advance
                    Buffer.BlockCopy(_buffer, ReadOffset, buffer, index, count - bytesPasted);
                    ReadOffset += count - bytesPasted;
                    FileOffset += count - bytesPasted;
                    if (FileOffset >= FileLength)
                    {
                        OnFinishedReading(); // We got to the end of the file.
                    }

                    finished = true;
                }
                else
                {
                    // Read what we can from the array and then advance the buffer
                    Buffer.BlockCopy(_buffer, ReadOffset, buffer, index, bytesInBuffer);
                    index += bytesInBuffer;
                    bytesPasted += bytesInBuffer;
                    FileOffset += bytesInBuffer;

                    if (bytesPasted >= count)
                    {
                        // Sometimes there might be less than count bytes left in the file.
                        // In those cases, we actually finish reading down here instead.
                        if (FileOffset >= FileLength)
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
            if (IsFinishedReading == false) ProgressMeter.CurrentValue = FileOffset;
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
        /// Reads the specified number of bytes. This will throw an exception
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
        /// Reads a character from two bytes in the buffer, automatically loading the next buffer if necessary.
        /// </summary>
        /// <returns>The character that was read.</returns>
        public char ReadChar()
        {
            byte[] data = ReadBytes(2);
            return BitConverter.ToChar(data, 0);
        }

        /// <summary>
        /// Reads an array of character from two bytes in the buffer, automatically loading the next buffer if necessary.
        /// </summary>
        /// <param name="count">Number of characters that should be read.</param>
        /// <returns>The characters that were read.</returns>
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
        /// Reads the specified number of doubles into an array.
        /// This uses Buffer.CopyBlock, and seems to work ok for little-endian in windows.
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
        /// Reads a short, sixteen bit integer as bytes in little-endian order
        /// </summary>
        /// <returns>A short value</returns>
        public short ReadInt16()
        {
            byte[] data = ReadBytes(2);
            return BitConverter.ToInt16(data, 0);
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
        /// Reads double precision X and Y values that are interwoven.
        /// </summary>
        /// <param name="count">The number of vertices that should be read.</param>
        /// <returns>A Vertex array with the vertices that were read.</returns>
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
        /// Uses the seek method to quickly reach a desired location to begin reading.
        /// This will not buffer or read values. If the new position is beyond the end
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
                    startPosition = ReadOffset + offset;
                    break;
                case SeekOrigin.End:
                    startPosition = FileLength - offset;
                    break;
            }

            if (startPosition >= FileLength || startPosition < 0)
            {
                // regardless of what direction, we need a start position inside the file
                throw new EndOfStreamException(DataStrings.EndOfFile);
            }

            // Only worry about resetting the buffer or repositioning the position
            // inside the buffer if a buffer is actually loaded.
            if (IsBufferLoaded)
            {
                long delta = startPosition - FileOffset;

                if (delta > BufferSize - ReadOffset || delta < 0)
                {
                    // The new position is beyond our current buffer
                    _buffer = null;
                    ReadOffset = -1;
                    BufferOffset = -1;
                    IsBufferLoaded = false;
                    if (IsFinishedBuffering || _binaryReader == null)
                    {
                        _fileStream = new FileStream(_fileName, FileMode.Open, FileAccess.Read);
                        _binaryReader = new BinaryReader(_fileStream);
                    }

                    IsFinishedBuffering = false;
                }
                else
                {
                    // The new position is still inside the buffer
                    ReadOffset += Convert.ToInt32(delta);
                    FileOffset += delta;

                    // we don't want to actually seek in the internal reader
                    return;
                }
            }

            // If no buffer is loaded, the file may not be open and may cause an exception when trying to seek.
            // probably better for tracking than not throwing one.
            FileOffset = startPosition;
            if (_fileStream.CanSeek) _fileStream.Seek(offset, origin);
        }

        /// <summary>
        /// Fires the FinishedBuffering event.
        /// </summary>
        protected virtual void OnFinishedBuffering()
        {
            IsFinishedBuffering = true;
            FinishedBuffering?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the FinishedReading event.
        /// </summary>
        protected virtual void OnFinishedReading()
        {
            IsFinishedReading = true;
            ProgressMeter.Reset();
            _buffer = null;
            FinishedReading?.Invoke(this, EventArgs.Empty);
        }

        // This internal method attempts to advance the buffer.
        private void AdvanceBuffer()
        {
            if (IsFinishedReading)
            {
                throw new EndOfStreamException(DataStrings.EndOfFile);
            }

            if (IsFinishedBuffering == false)
            {
                if (_maxBufferSize > FileRemainder)
                {
                    // If the buffer size is greater than the remainder, load the entire remainder and close the file.
                    BufferSize = Convert.ToInt32(FileRemainder);
                    _buffer = new byte[BufferSize];

                    _binaryReader.Read(_buffer, 0, BufferSize);

                    Close();
                }
                else
                {
                    BufferSize = _maxBufferSize;
                    _buffer = new byte[BufferSize];

                    _binaryReader.Read(_buffer, 0, BufferSize);
                }

                IsBufferLoaded = true;
                BufferOffset = FileOffset;
                ReadOffset = 0;
            }
            else
            {
                // We are done, so throw the event to indicate we are done.
                OnFinishedReading();
            }
        }
    }
}