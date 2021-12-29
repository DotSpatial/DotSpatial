// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// The buffered binary reader was originally designed by Ted Dunsford to make shapefile reading more
    /// efficient, but ostensibly could be used for other binary reading exercises. To use this class,
    /// simply specify the BufferSize in bytes that you would like to use and begin reading values.
    /// </summary>
    public class BufferedBinaryWriter
    {
        #region Private Variables

        private readonly long _fileLength;
        private BinaryWriter _binaryWriter;

        private Stream _fileStream;
        private bool _isBufferLoaded;
        private int _maxBufferSize;
        private IProgressHandler _progressHandler;
        private ProgressMeter _progressMeter;
        private int _writeOffset;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedBinaryWriter"/> class.
        /// </summary>
        /// <param name="fileName">The string path of a file to open using this BufferedBinaryReader.</param>
        public BufferedBinaryWriter(string fileName)
            : this(fileName, null, 100000)
        {
            // This is just an overload that sends the default null value in for the progressHandler
            _progressHandler = DataManager.DefaultDataManager.ProgressHandler;
            _progressMeter = new ProgressMeter(_progressHandler);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedBinaryWriter"/> class on an arbitrary stream.
        /// </summary>
        /// <param name="s">Stream used for writing.</param>
        public BufferedBinaryWriter(Stream s)
        {
            long expectedByteCount = 100000;

            _fileStream = s;
            _fileLength = 0;
            FileOffset = 0;

            Buffer = new byte[expectedByteCount];
            BufferSize = Convert.ToInt32(expectedByteCount);
            _maxBufferSize = BufferSize;
            _writeOffset = -1; // There is no buffer loaded.
            BufferOffset = -1; // -1 means no buffer is loaded.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedBinaryWriter"/> class and specifies where to send progress messages.
        /// </summary>
        /// <param name="fileName">The string path of a file to open using this BufferedBinaryReader.</param>
        /// <param name="progressHandler">Any implementation of IProgressHandler for receiving progress messages.</param>
        /// <param name="expectedByteCount">A long specifying the number of bytes that will be written for the purposes of tracking progress.</param>
        public BufferedBinaryWriter(string fileName, IProgressHandler progressHandler, long expectedByteCount)
        {
            if (File.Exists(fileName))
            {
                // Imagine we have written a header and want to add more stuff
                _fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                FileInfo fi = new FileInfo(fileName);
                _fileLength = fi.Length;
                FileOffset = 0;
            }
            else
            {
                // In this case, we just create the new file from scratch
                _fileStream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                _fileLength = 0;
                FileOffset = 0;
            }

            _binaryWriter = new BinaryWriter(_fileStream);
            Buffer = new byte[expectedByteCount];
            BufferSize = Convert.ToInt32(expectedByteCount);
            _maxBufferSize = BufferSize;
            _writeOffset = -1; // There is no buffer loaded.
            BufferOffset = -1; // -1 means no buffer is loaded.

            // report progress
            if (progressHandler != null)
            {
                _progressHandler = progressHandler;
                _progressMeter.Key = "Writing to " + Path.GetFileName(fileName);
                _progressMeter.EndValue = expectedByteCount;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the actual array of bytes currently in the buffer.
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// Gets or sets a long integer specifying the starting position of the currently loaded buffer
        /// relative to the start of the file. A value of -1 indicates that no buffer is
        /// currently loaded.
        /// </summary>
        public long BufferOffset { get; protected set; }

        /// <summary>
        /// Gets or sets an integer value specifying the size of the buffer currently loaded into memory.
        /// This will either be the MaxBufferSize, or a smaller buffer representing a smaller
        /// remainder existing in the file.
        /// </summary>
        public int BufferSize { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is currently any information loaded into the buffer.
        /// </summary>
        public virtual bool IsBufferLoaded
        {
            get { return _isBufferLoaded; }
            protected set { _isBufferLoaded = value; }
        }

        /// <summary>
        /// Gets or sets the current read position in the file in bytes.
        /// </summary>
        public long FileOffset { get; protected set; }

        /// <summary>
        /// Gets or sets the buffer size to read in chunks. This does not
        /// describe the size of the actual.
        /// </summary>
        public virtual int MaxBufferSize
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
        /// Gets or sets the progress handler for this binary writer.
        /// </summary>
        public virtual IProgressHandler ProgressHandler
        {
            get
            {
                return _progressHandler;
            }

            set
            {
                _progressHandler = value;
                _progressMeter = new ProgressMeter(_progressHandler);
            }
        }

        /// <summary>
        /// Gets or sets where reading will begin (relative to the start of the buffer).
        /// </summary>
        public virtual int WriteOffset
        {
            get { return _writeOffset; }
            protected set { _writeOffset = value; }
        }

        /// <summary>
        /// Gets or sets the progress meter that is directly linked to the progress handler.
        /// </summary>
        protected virtual ProgressMeter ProgressMeter
        {
            get { return _progressMeter; }
            set { _progressMeter = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finishes writing whatever is in memory to the file, closes the internal binary writer, the underlying file, clears the memory
        /// and disposes the filestream.
        /// </summary>
        public void Close()
        {
            if (_binaryWriter != null)
            {
                // Finish pasting any residual data to the file
                PasteBuffer();

                // Close the binary writer and underlying filestream
                _binaryWriter.Close();
            }

            _binaryWriter = null;
            Buffer = null;
            _progressMeter = null; // the IProgressHandler could be an undesired handle to a whole form or something
            _fileStream?.Dispose();
            _fileStream = null;
        }

        /// <summary>
        /// This seeks both in the file AND in the buffer. This is used to write only the desired portions of a buffer that is in memory to a file.
        /// </summary>
        /// <param name="offset">A 64 bit integer specifying where to skip to in the file.</param>
        /// <param name="origin">A System.IO.SeekOrigin enumeration specifying how to estimate the location.</param>
        /// <returns>The position.</returns>
        public virtual long Seek(long offset, SeekOrigin origin)
        {
            long startPosition = 0;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    startPosition = offset;
                    break;
                case SeekOrigin.Current:
                    startPosition = _writeOffset + offset;
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
                long delta = startPosition - FileOffset;

                if (delta > BufferSize - _writeOffset)
                {
                    // The new position is beyond our current buffer
                    Buffer = null;
                    _writeOffset = -1;
                    BufferOffset = -1;
                    _isBufferLoaded = false;
                }
                else
                {
                    // The new position is still inside the buffer
                    _writeOffset += Convert.ToInt32(delta);
                    FileOffset = startPosition;

                    // we don't want to actually seek in the internal reader
                    return startPosition;
                }
            }

            // If no buffer is loaded, the file may not be open and may cause an exception when trying to seek.
            // probably better for tracking than not throwing one.
            FileOffset = startPosition;
            if (_fileStream.CanSeek) _fileStream.Seek(offset, origin);
            return startPosition;
        }

        #region Write Methods

        /// <summary>
        /// Writes a boolean to the buffer.
        /// </summary>
        /// <param name="value">Value that gets written.</param>
        public void Write(bool value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Write(data);
        }

        /// <summary>
        /// Writes a character to the buffer.
        /// </summary>
        /// <param name="value">A character to write to the buffer, and eventually the file.</param>
        public void Write(char value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Write(data);
        }

        /// <summary>
        /// Writes an array of character to the buffer.
        /// </summary>
        /// <param name="values">Values that get written.</param>
        public void Write(char[] values)
        {
            List<byte> lstData = new List<byte>();
            foreach (var b in values)
            {
                lstData.AddRange(BitConverter.GetBytes(b));
            }

            byte[] data = lstData.ToArray();
            Write(data);
        }

        /// <summary>
        /// Writes a double to the buffer.
        /// </summary>
        /// <param name="value">Value that gets written.</param>
        public void Write(double value)
        {
            Write(value, true);
        }

        /// <summary>
        /// Writes a double-precision floating point to the buffer.
        /// </summary>
        /// <param name="value">A double-precision floating point decimal value to write as 8 bytes.</param>
        /// <param name="isLittleEndian">Boolean, true if the value should be returned with little endian byte ordering.</param>
        public void Write(double value, bool isLittleEndian)
        {
            // Integers are 8 Bytes long.
            byte[] data = BitConverter.GetBytes(value);

            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                // The IsLittleEndian property on the BitConverter tells us what the system is using by default.
                // The isLittleEndian argument tells us what bit order the output should be in.
                Array.Reverse(data);
            }

            Write(data);
        }

        /// <summary>
        /// Writes the specified array of doubles to the file.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(double[] values)
        {
            byte[] rawbytes = new byte[values.Length * 8];
            System.Buffer.BlockCopy(values, 0, rawbytes, 0, rawbytes.Length);
            Write(rawbytes);
        }

        /// <summary>
        /// Writes the specified array of integers to the file.
        /// </summary>
        /// <param name="values">The values to write.</param>
        public void Write(int[] values)
        {
            byte[] rawbytes = new byte[values.Length * 4];
            System.Buffer.BlockCopy(values, 0, rawbytes, 0, rawbytes.Length);
            Write(rawbytes);
        }

        /// <summary>
        /// By default, this will use little Endian ordering.
        /// </summary>
        /// <param name="value">Value that gets written.</param>
        public void Write(int value)
        {
            Write(value, true);
        }

        /// <summary>
        /// Reads an integer from the file, using the isLittleEndian argument
        /// to decide whether to flip the bits or not.
        /// </summary>
        /// <param name="value">A 32-bit integer to write as 4 bytes in the buffer.</param>
        /// <param name="isLittleEndian">Boolean, true if the value should be returned with little endian byte ordering.</param>
        public virtual void Write(int value, bool isLittleEndian)
        {
            // Integers are 4 Bytes long.
            byte[] data = BitConverter.GetBytes(value);

            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                // The IsLittleEndian property on the BitConverter tells us what the system is using by default.
                // The isLittleEndian argument tells us what bit order the output should be in.
                Array.Reverse(data);
            }

            Write(data);
        }

        /// <summary>
        /// Writes an Int16 to the buffer.
        /// </summary>
        /// <param name="value">An Int16 to convert into 2 bytes to write to the buffer.</param>
        public virtual void Write(short value)
        {
            byte[] data = BitConverter.GetBytes(value);
            Write(data);
        }

        /// <summary>
        /// Writes a single-precision floading point to 4 bytes in the buffer, automatically loading the next buffer if necessary.
        /// This assumes the value should be little endian.
        /// </summary>
        /// <param name="value">A Single to convert to 4 bytes to write to the buffer.</param>
        public virtual void Write(float value)
        {
            Write(value, true);
        }

        /// <summary>
        /// Reads a single-precision floading point from 4 bytes in the buffer, automatically loading the next buffer if necessary.
        /// </summary>
        /// <param name="value">A single-precision floating point converted from four bytes.</param>
        /// <param name="isLittleEndian">Boolean, true if the value should be returned with little endian byte ordering.</param>
        public virtual void Write(float value, bool isLittleEndian)
        {
            // Integers are 4 Bytes long.
            byte[] data = BitConverter.GetBytes(value);

            if (isLittleEndian != BitConverter.IsLittleEndian)
            {
                // The IsLittleEndian property on the BitConverter tells us what the system is using by default.
                // The isLittleEndian argument tells us what bit order the output should be in.
                Array.Reverse(data);
            }

            Write(data);
        }

        /// <summary>
        /// Writes the specified bytes to the buffer, advancing the buffer automatically if necessary.
        /// </summary>
        /// <param name="value">An array of byte values to write to the buffer.</param>
        public virtual void Write(byte[] value)
        {
            bool finished = false;
            int bytesPasted = 0;
            int index = 0;
            int count = value.Length;
            if (_isBufferLoaded == false) AdvanceBuffer();
            do
            {
                int bytesInBuffer = BufferSize - _writeOffset;
                if (count < bytesInBuffer)
                {
                    Array.Copy(value, index, Buffer, _writeOffset, count - bytesPasted);
                    _writeOffset += count - bytesPasted;
                    FileOffset += count - bytesPasted;
                    finished = true;
                }
                else
                {
                    int sourceLeft = count - index;
                    if (sourceLeft > bytesInBuffer)
                    {
                        Array.Copy(value, index, Buffer, _writeOffset, bytesInBuffer);
                        index += bytesInBuffer;
                        bytesPasted += bytesInBuffer;
                        FileOffset += bytesInBuffer;
                        _writeOffset += bytesInBuffer;
                        if (bytesPasted >= count)
                        {
                            finished = true;
                        }
                        else
                        {
                            AdvanceBuffer();
                        }
                    }
                    else
                    {
                        Array.Copy(value, index, Buffer, _writeOffset, sourceLeft);
                        index += sourceLeft;
                        bytesPasted += sourceLeft;
                        FileOffset += sourceLeft;
                        _writeOffset += sourceLeft;
                        finished = true;
                    }
                }
            }
            while (finished == false);
        }

        #endregion

        // This internal method attempts to advance the buffer.
        private void AdvanceBuffer()
        {
            if (_isBufferLoaded)
            {
                // write the contents of the buffer
                _binaryWriter.Write(Buffer);

                // reposition the buffer after the last paste
                BufferOffset = 0;  // file offset is tracked at the time of an individual write event
            }
            else
            {
                _isBufferLoaded = true;
            }

            // either way, dimension the next chunk
            Buffer = new byte[_maxBufferSize];

            // indicate where to start writing in the buffer
            _writeOffset = 0;
        }

        /// <summary>
        /// Forces the buffer to paste all its existing values into the file, but does not
        /// advance the buffer, or in fact do anything to the buffer. It does advance
        /// the position of the file index.
        /// </summary>
        private void PasteBuffer()
        {
            if (_writeOffset > -1)
            {
                _binaryWriter.Write(Buffer, 0, _writeOffset);
                FileOffset += _writeOffset;
            }
        }

        #endregion

    }
}