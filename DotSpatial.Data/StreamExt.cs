// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2010 11:54:49 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// Stream extensions
    /// </summary>
    public static class StreamExt
    {
        /// <summary>
        /// Attempts to read count of bytes from stream.
        /// </summary>
        /// <param name="stream">Input stream.</param>
        /// <param name="count">Count of bytes.</param>
        /// <returns>Bytes array.</returns>
        public static byte[] ReadBytes(this Stream stream, int count)
        {
            var bytes = new byte[count];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Attempts to read the specified T.  If this system is
        /// doesn't match the specified endian, then this will reverse the array of bytes,
        /// so that it corresponds with the big-endian format.
        /// </summary>
        /// <param name="stream">The stream to read the value from</param>
        /// <param name="endian">Specifies what endian property should be used.</param>
        /// <returns>The integer value</returns>
        public static int ReadInt32(this Stream stream, Endian endian = Endian.LittleEndian)
        {
            var val = new byte[4];
            stream.Read(val, 0, 4);
            if ((endian == Endian.LittleEndian) != BitConverter.IsLittleEndian)
            {
                Array.Reverse(val);
            }
            return BitConverter.ToInt32(val, 0);
        }

        /// <summary>
        /// Reads the specified number of integers.  If a value other than the
        /// systems endian format is specified the values will be reversed.
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="count">The integer count of integers to read</param>
        /// <param name="endian">The endian order of the bytes.</param>
        /// <returns>The array of integers that will have count integers.</returns>
        public static int[] ReadInt32(this Stream stream, int count, Endian endian = Endian.LittleEndian)
        {
            var result = new int[count];
            var val = new byte[4 * count];
            stream.Read(val, 0, val.Length);
            if ((endian == Endian.LittleEndian) != BitConverter.IsLittleEndian)
            {
                for (var i = 0; i < count; i++)
                {
                    var temp = new byte[4];
                    Array.Copy(val, i * 4, temp, 0, 4);
                    Array.Reverse(temp);
                    Array.Copy(temp, 0, val, i * 4, 4);
                }
            }
            Buffer.BlockCopy(val, 0, result, 0, count * 4);
            return result;
        }

        /// <summary>
        /// Reads a double precision value from the stream.  If this system
        /// is not little endian, it will reverse the individual memebrs.
        /// </summary>
        /// <param name="stream">The stream to read the values from.</param>
        /// <returns>A double precision value</returns>
        public static double ReadDouble(this Stream stream)
        {
            return ReadDouble(stream, 1)[0];
        }

        /// <summary>
        /// Reads the specified number of double precision values.  If this system
        /// does not match the specified endian, the bytes will be reversed.
        /// </summary>
        /// <param name="stream">The stream to read the values from.</param>
        /// <param name="count">The integer count of doubles to read.</param>
        /// <param name="endian">The endian to use.</param>
        /// <returns></returns>
        public static double[] ReadDouble(this Stream stream, int count, Endian endian = Endian.LittleEndian)
        {
            var val = new byte[count * 8];
            stream.Read(val, 0, count * 8);
            if ((endian == Endian.LittleEndian) != BitConverter.IsLittleEndian)
            {
                for (var i = 0; i < count; i++)
                {
                    var temp = new byte[8];
                    Array.Copy(val, i * 8, temp, 0, 8);
                    Array.Reverse(temp);
                    Array.Copy(temp, 0, val, i * 8, 8);
                }
            }
            var result = new double[count];
            Buffer.BlockCopy(val, 0, result, 0, count * 8);
            return result;
        }

        /// <summary>
        /// Writes the integer as big endian
        /// </summary>
        /// <param name="stream">The IO stream </param>
        /// <param name="value"></param>
        public static void WriteBe(this Stream stream, int value)
        {
            var val = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(val);
            stream.Write(val, 0, 4);
        }

        /// <summary>
        /// Writes the endian as little endian
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void WriteLe(this Stream stream, int value)
        {
            var val = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(val);
            stream.Write(val, 0, 4);
        }

        /// <summary>
        /// Checks that the endian order is ok for integers and then writes
        /// the entire array to the stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="values"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public static void WriteLe(this Stream stream, int[] values, int startIndex, int count)
        {
            var bytes = new byte[count * 4];
            Buffer.BlockCopy(values, startIndex * 4, bytes, 0, bytes.Length);
            if (!BitConverter.IsLittleEndian)
            {
                // Reverse to little endian if this is a big endian machine
                var temp = bytes;
                bytes = new byte[temp.Length];
                Array.Reverse(temp);
                for (var i = 0; i < count; i++)
                {
                    Array.Copy(temp, i * 4, bytes, (count - i - 1) * 4, 4);
                }
            }
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Writes the specified double value to the stream as little endian
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="value"></param>
        public static void WriteLe(this Stream stream, double value)
        {
            var val = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian) Array.Reverse(val);
            stream.Write(val, 0, 8);
        }

        /// <summary>
        /// Checks that the endian order is ok for doubles and then writes
        /// the entire array to the stream.
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="values">The double values to write in little endian form</param>
        /// <param name="startIndex">The integer start index in the double array to begin writing</param>
        /// <param name="count">The integer count of doubles to write.</param>
        public static void WriteLe(this Stream stream, double[] values, int startIndex, int count)
        {
            var bytes = new byte[count * 8];
            Buffer.BlockCopy(values, startIndex * 8, bytes, 0, bytes.Length);
            if (!BitConverter.IsLittleEndian)
            {
                // Reverse to little endian if this is a big endian machine
                var temp = bytes;
                bytes = new byte[temp.Length];
                Array.Reverse(temp);
                for (var i = 0; i < count; i++)
                {
                    Array.Copy(temp, i * 8, bytes, (count - i - 1) * 8, 8);
                }
            }
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}