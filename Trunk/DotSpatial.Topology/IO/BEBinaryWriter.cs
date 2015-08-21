// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DotSpatial.Topology.IO
{
    /// <summary>
    /// Extends the <see cref="BinaryWriter" /> class to allow writing values in the BigEndian format.    
    /// </summary>
    /// <remarks>
    /// While <see cref="BEBinaryWriter" /> extends <see cref="BinaryWriter" /> 
    /// adding methods for writing integer values (<see cref="BEBinaryWriter.Write(int)" />)
    /// and double values (<see cref="BEBinaryWriter.Write(double)" />) in the BigEndian format,
    /// this implementation overrides methods, such <see cref="BinaryWriter.Write(int)" /> 
    /// and <see cref="BinaryWriter.Write(double)" /> and more, 
    /// for writing <see cref="ByteOrder.BigEndian" /> values in the BigEndian format.
    /// </remarks>
    public class BEBinaryWriter : BinaryWriter
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BEBinaryWriter"/> class.
        /// </summary>
        public BEBinaryWriter() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEBinaryWriter"/> class.
        /// </summary>
        /// <param name="output">The supplied stream.</param>
        /// <exception cref="T:System.ArgumentNullException">output is null. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// The stream does not support writing, or the stream is already closed. </exception>
        public BEBinaryWriter(Stream output) : base(output) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BEBinaryWriter"/> class.
        /// </summary>
        /// <param name="output">The supplied stream.</param>
        /// <param name="encoding">The character encoding.</param>
        /// <exception cref="T:System.ArgumentNullException">output or encoding is null. </exception>
        /// <exception cref="T:System.ArgumentException">
        /// The stream does not support writing, or the stream is already closed. </exception>
        public BEBinaryWriter(Stream output, Encoding encoding) : base(output, encoding) { }

        #endregion

        #region Methods

        /// <summary>
        /// Writes a two-byte signed integer to the current stream using BigEndian encoding
        /// and advances the stream position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte signed integer to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Write(short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Debug.Assert(bytes.Length == 2);

            Array.Reverse(bytes, 0, 2);
            Write(bytes);
        }

        /// <summary>
        /// Writes a two-byte unsigned integer to the current stream  using BigEndian encoding
        /// and advances the stream position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte unsigned integer to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        [CLSCompliant(false)]
        public override void Write(ushort value)
        {            
            byte[] bytes = BitConverter.GetBytes(value);
            Debug.Assert(bytes.Length == 2);

            Array.Reverse(bytes, 0, 2);
            Write(bytes);
        }

        /// <summary>
        /// Writes a four-byte signed integer to the current stream using BigEndian encoding
        /// and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte signed integer to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Write(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Debug.Assert(bytes.Length == 4);

            Array.Reverse(bytes, 0, 4);
            Write(bytes);
        }

        /// <summary>
        /// Writes a four-byte unsigned integer to the current stream using BigEndian encoding
        /// and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte unsigned integer to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        [CLSCompliant(false)]
        public override void Write(uint value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Debug.Assert(bytes.Length == 4);

            Array.Reverse(bytes, 0, 4);
            Write(bytes);
        }

        /// <summary>
        /// Writes an eight-byte signed integer to the current stream using BigEndian encoding
        /// and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte signed integer to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Write(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Debug.Assert(bytes.Length == 8);

            Array.Reverse(bytes, 0, 8);
            Write(bytes);
        }

        /// <summary>
        /// Writes an eight-byte unsigned integer to the current stream using BigEndian encoding
        /// and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte unsigned integer to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        [CLSCompliant(false)]
        public override void Write(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Debug.Assert(bytes.Length == 8);

            Array.Reverse(bytes, 0, 8);
            Write(bytes);
        }

        /// <summary>
        /// Writes a four-byte floating-point value to the current stream using BigEndian encoding
        /// and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte floating-point value to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Write(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Debug.Assert(bytes.Length == 4);

            Array.Reverse(bytes, 0, 4);
            Write(bytes);
        }

        /// <summary>
        /// Writes an eight-byte floating-point value to the current stream using BigEndian encoding
        /// and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte floating-point value to write.</param>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Write(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Debug.Assert(bytes.Length == 8);

            Array.Reverse(bytes, 0, 8);
            Write(bytes);
        }

        #endregion
    }
}