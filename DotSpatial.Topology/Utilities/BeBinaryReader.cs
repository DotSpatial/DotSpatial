// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://dotspatial.codeplex.com/license and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// Extends the <see cref="BinaryReader" /> class to allow reading values in the BigEndian format.
    /// </summary>
    /// <remarks>
    /// While BigEndianBinaryReader extends BinaryReader
    /// adding methods for reading integer values
    /// and double values  in the BigEndian format,
    /// this implementation overrides methods, such <see cref="BinaryReader.ReadInt32" />
    /// and <see cref="BinaryReader.ReadDouble" /> and more,
    /// for reading ByteOrder.BigEndian values in the BigEndian format.
    /// </remarks>
    public class BeBinaryReader : BinaryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeBinaryReader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public BeBinaryReader(Stream stream) : base(stream) { }

        /// <summary>
        /// Initializes a new instance of the BEBinaryReader class.
        /// </summary>
        /// <param name="input">The supplied stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <exception cref="T:System.ArgumentNullException">encoding is null. </exception>
        /// <exception cref="T:System.ArgumentException">The stream does not support reading, the stream is null, or the stream is already closed. </exception>
        public BeBinaryReader(Stream input, Encoding encoding) : base(input, encoding) { }

        /// <summary>
        /// Reads a 2-byte signed integer from the current stream using big endian encoding
        /// and advances the current position of the stream by two bytes.
        /// </summary>
        /// <returns>
        /// A 2-byte signed integer read from the current stream.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        public override short ReadInt16()
        {
            byte[] byteArray = new byte[2];
            int iBytesRead = Read(byteArray, 0, 2);
            Debug.Assert(iBytesRead == 2);

            Array.Reverse(byteArray);
            return BitConverter.ToInt16(byteArray, 0);
        }

        // NOT CLS COMPLIANT
        ///// <summary>
        ///// Reads a 2-byte unsigned integer from the current stream using big endian encoding
        ///// and advances the position of the stream by two bytes.
        ///// </summary>
        ///// <returns>
        ///// A 2-byte unsigned integer read from this stream.
        ///// </returns>
        ///// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        //public override ushort ReadUInt16()
        //{
        //    byte[] byteArray = new byte[2];
        //    int iBytesRead = Read(byteArray, 0, 2);
        //    Debug.Assert(iBytesRead == 2);

        //    Array.Reverse(byteArray);
        //    return BitConverter.ToUInt16(byteArray, 0);
        //}

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream using big endian encoding
        /// and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>
        /// A 4-byte signed integer read from the current stream.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        public override int ReadInt32()
        {
            byte[] byteArray = new byte[4];
            int iBytesRead = Read(byteArray, 0, 4);
            Debug.Assert(iBytesRead == 4);

            Array.Reverse(byteArray);
            return BitConverter.ToInt32(byteArray, 0);
        }

        // NOT CLS COMPLIANT
        ///// <summary>
        ///// Reads a 4-byte unsigned integer from the current stream using big endian encoding
        ///// and advances the position of the stream by four bytes.
        ///// </summary>
        ///// <returns>
        ///// A 4-byte unsigned integer read from this stream.
        ///// </returns>
        ///// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        //public override uint ReadUInt32()
        //{
        //    byte[] byteArray = new byte[4];
        //    int iBytesRead = Read(byteArray, 0, 4);
        //    Debug.Assert(iBytesRead == 4);

        //    Array.Reverse(byteArray);
        //    return BitConverter.ToUInt32(byteArray, 0);
        //}

        /// <summary>
        /// Reads an 8-byte signed integer from the current stream using big endian encoding
        /// and advances the current position of the stream by eight bytes.
        /// </summary>
        /// <returns>
        /// An 8-byte signed integer read from the current stream.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        public override long ReadInt64()
        {
            byte[] byteArray = new byte[8];
            int iBytesRead = Read(byteArray, 0, 8);
            Debug.Assert(iBytesRead == 8);

            Array.Reverse(byteArray);
            return BitConverter.ToInt64(byteArray, 0);
        }

        // NOT CLS COMPLIANT
        ///// <summary>
        ///// Reads an 8-byte unsigned integer from the current stream using big endian encoding
        ///// and advances the position of the stream by eight bytes.
        ///// </summary>
        ///// <returns>
        ///// An 8-byte unsigned integer read from this stream.
        ///// </returns>
        ///// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        ///// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        ///// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        //public override ulong ReadUInt64()
        //{
        //    byte[] byteArray = new byte[8];
        //    int iBytesRead = Read(byteArray, 0, 8);
        //    Debug.Assert(iBytesRead == 8);

        //    Array.Reverse(byteArray);
        //    return BitConverter.ToUInt64(byteArray, 0);
        //}

        /// <summary>
        /// Reads a 4-byte floating point value from the current stream using big endian encoding
        /// and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>
        /// A 4-byte floating point value read from the current stream.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        public override float ReadSingle()
        {
            byte[] byteArray = new byte[4];
            int iBytesRead = Read(byteArray, 0, 4);
            Debug.Assert(iBytesRead == 4);

            Array.Reverse(byteArray);
            return BitConverter.ToSingle(byteArray, 0);
        }

        /// <summary>
        /// Reads an 8-byte floating point value from the current stream using big endian encoding
        /// and advances the current position of the stream by eight bytes.
        /// </summary>
        /// <returns>
        /// An 8-byte floating point value read from the current stream.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        public override double ReadDouble()
        {
            byte[] byteArray = new byte[8];
            int iBytesRead = Read(byteArray, 0, 8);
            Debug.Assert(iBytesRead == 8);

            Array.Reverse(byteArray);
            return BitConverter.ToDouble(byteArray, 0);
        }

        /// <inheritdoc />
        public override decimal ReadDecimal()
        {
            byte[] byteArray = new byte[16];
            int iBytesRead = Read(byteArray, 0, 16);
            Debug.Assert(iBytesRead == 16);

            Array.Reverse(byteArray);
            MemoryStream ms = new MemoryStream(byteArray);
            BinaryReader br = new BinaryReader(ms);
            return br.ReadDecimal();
        }
    }
}