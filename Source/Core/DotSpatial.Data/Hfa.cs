// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Hfa.
    /// </summary>
    public static class Hfa
    {
        #region Methods

        /// <summary>
        /// Gets the bitcount of a single member of the specified data type.
        /// </summary>
        /// <param name="dataType">The data type to get the byte count of.</param>
        /// <returns>An integer that represents the bit count of the specified type.</returns>
        public static int GetBitCount(this HfaEpt dataType)
        {
            return dataType switch
            {
                HfaEpt.U1 => 1,
                HfaEpt.U2 => 2,
                HfaEpt.U4 => 4,
                HfaEpt.U8 or HfaEpt.S8 => 8,
                HfaEpt.U16 or HfaEpt.S16 => 16,
                HfaEpt.U32 or HfaEpt.S32 or HfaEpt.Single => 32,
                HfaEpt.Double or HfaEpt.Char64 => 64,
                HfaEpt.Char128 => 128,
                _ => 0,
            };
        }

        /// <summary>
        /// Reverses the byte order on Big-Endian systems like Unix to conform with the
        /// HFA standard of little endian.
        /// </summary>
        /// <param name="value">The value that gets reversed.</param>
        /// <returns>The reversed value.</returns>
        public static byte[] LittleEndian(byte[] value)
        {
            if (!BitConverter.IsLittleEndian) Array.Reverse(value);
            return value;
        }

        /// <summary>
        /// Obtains the 4 byte equivalent for 32 bit Integer values.
        /// </summary>
        /// <param name="value">The unsigned integer value to convert into bytes.</param>
        /// <returns>The bytes in LittleEndian standard, regardless of the system architecture.</returns>
        public static byte[] LittleEndian(int value)
        {
            byte[] bValue = BitConverter.GetBytes(value);
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Obtains the 4 byte equivalent for 32bit floating point values.
        /// </summary>
        /// <param name="value">The unsigned integer value to convert into bytes.</param>
        /// <returns>The bytes in LittleEndian standard, regardless of the system architecture.</returns>
        public static byte[] LittleEndian(float value)
        {
            byte[] bValue = BitConverter.GetBytes(value);
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Obtains the 8 byte equivalent for 64 bit floating point values.
        /// </summary>
        /// <param name="value">The unsigned integer value to convert into bytes.</param>
        /// <returns>The bytes in LittleEndian standard, regardless of the system architecture.</returns>
        public static byte[] LittleEndian(double value)
        {
            byte[] bValue = BitConverter.GetBytes(value);
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Gets the 2 byte equivalent of a short in little endian format.
        /// </summary>
        /// <param name="value">The value to get the equivalent for.</param>
        /// <returns>The 2 byte equivalent for the value.</returns>
        public static byte[] LittleEndian(short value)
        {
            byte[] bValue = BitConverter.GetBytes(value);
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Obtains the 4 byte equivalent for 32 bit Unsigned Integer values.
        /// </summary>
        /// <param name="value">The unsigned integer value to convert into bytes.</param>
        /// <returns>The bytes in LittleEndian standard, regardless of the system architecture.</returns>
        public static byte[] LittleEndianAsUint32(long value)
        {
            byte[] bValue = BitConverter.GetBytes(Convert.ToUInt32(value));
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Gets the 2 byte equivalent of an unsigned short in little endian format.
        /// </summary>
        /// <param name="value">The value to get the equivalent for.</param>
        /// <returns>The 2 byte equivalent for the value.</returns>
        public static byte[] LittleEndianAsUShort(int value)
        {
            byte[] bValue = BitConverter.GetBytes(Convert.ToUInt16(value));
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 64 bit floating point value
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset to start reading.</param>
        /// <returns>The value.</returns>
        public static double ReadDouble(byte[] data, long offset)
        {
            byte[] vals = new byte[8];
            Array.Copy(data, offset, vals, 0, 8);
            return BitConverter.ToSingle(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 32 bit integer.
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset to start reading.</param>
        /// <returns>The read value.</returns>
        public static short ReadInt16(byte[] data, long offset)
        {
            byte[] vals = new byte[4];
            Array.Copy(data, offset, vals, 0, 4);
            return BitConverter.ToInt16(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 32 bit integer.
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset to start reading.</param>
        /// <returns>The read value.</returns>
        public static int ReadInt32(byte[] data, long offset)
        {
            byte[] vals = new byte[4];
            Array.Copy(data, offset, vals, 0, 4);
            return BitConverter.ToInt32(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 32 bit floating point value
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset to start reading.</param>
        /// <returns>The read value.</returns>
        public static float ReadSingle(byte[] data, long offset)
        {
            byte[] vals = new byte[4];
            Array.Copy(data, offset, vals, 0, 4);
            return BitConverter.ToSingle(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Returns the type that was read.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset to start reading.</param>
        /// <returns>The type.</returns>
        public static HfaEpt ReadType(byte[] data, long offset)
        {
            byte[] vals = new byte[2];
            Array.Copy(data, offset, vals, 0, 2);
            return (HfaEpt)BitConverter.ToInt16(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 16 bit unsigned short integer.
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset to start reading.</param>
        /// <returns>A UInt16 value stored in an int because UInt16 is not CLS Compliant.</returns>
        public static int ReadUInt16(byte[] data, long offset)
        {
            byte[] vals = new byte[2];
            Array.Copy(data, offset, vals, 0, 2);
            return BitConverter.ToUInt16(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 32 bit unsigned integer.
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset to start reading.</param>
        /// <returns>A UInt32 value stored in a long because UInt32 is not CLS Compliant.</returns>
        public static long ReadUInt32(byte[] data, long offset)
        {
            byte[] vals = new byte[4];
            Array.Copy(data, offset, vals, 0, 4);
            return BitConverter.ToUInt32(LittleEndian(vals), 0);
        }

        #endregion
    }
}