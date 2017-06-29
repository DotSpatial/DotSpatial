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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2010 10:56:39 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Hfa
    /// </summary>
    public static class Hfa
    {
        #region Methods

        /// <summary>
        /// Gets the bitcount of a single member of the specified data type.
        /// </summary>
        /// <param name="dataType">The data type to get the byte count of</param>
        /// <returns>An integer that represents the bit count of the specified type</returns>
        public static int GetBitCount(this HfaEPT dataType)
        {
            switch (dataType)
            {
                case HfaEPT.U1:
                    return 1;
                case HfaEPT.U2:
                    return 2;
                case HfaEPT.U4:
                    return 4;
                case HfaEPT.U8:
                case HfaEPT.S8:
                    return 8;
                case HfaEPT.U16:
                case HfaEPT.S16:
                    return 16;
                case HfaEPT.U32:
                case HfaEPT.S32:
                case HfaEPT.Single:
                    return 32;
                case HfaEPT.Double:
                case HfaEPT.Char64:
                    return 64;
                case HfaEPT.Char128:
                    return 128;
            }

            return 0;
        }

        /// <summary>
        /// Reverses the byte order on Big-Endian systems like Unix to conform with the
        /// HFA standard of little endian.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] LittleEndian(byte[] value)
        {
            if (!BitConverter.IsLittleEndian) Array.Reverse(value);
            return value;
        }

        /// <summary>
        /// Return ReadType
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static HfaEPT ReadType(byte[] data, long offset)
        {
            byte[] vals = new byte[2];
            Array.Copy(data, offset, vals, 0, 2);
            return (HfaEPT)BitConverter.ToInt16(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 32 bit integer.
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static short ReadInt16(byte[] data, long offset)
        {
            byte[] vals = new byte[4];
            Array.Copy(data, offset, vals, 0, 4);
            return BitConverter.ToInt16(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 16 bit unsigned short integer.
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int ReadUInt16(byte[] data, long offset)
        {
            byte[] vals = new byte[2];
            Array.Copy(data, offset, vals, 0, 2);
            return BitConverter.ToUInt16(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 32 bit integer.
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
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
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static float ReadSingle(byte[] data, long offset)
        {
            byte[] vals = new byte[4];
            Array.Copy(data, offset, vals, 0, 4);
            return BitConverter.ToSingle(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 64 bit floating point value
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static double ReadDouble(byte[] data, long offset)
        {
            byte[] vals = new byte[8];
            Array.Copy(data, offset, vals, 0, 8);
            return BitConverter.ToSingle(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Given the byte array, this reads four bytes and converts this to a 32 bit unsigned integer.
        /// This always uses little endian byte order.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns>A UInt32 value stored in a long because UInt32 is not CLS Compliant</returns>
        public static long ReadUInt32(byte[] data, long offset)
        {
            byte[] vals = new byte[4];
            Array.Copy(data, offset, vals, 0, 4);
            return BitConverter.ToUInt32(LittleEndian(vals), 0);
        }

        /// <summary>
        /// Obtains the 4 byte equivalent for 32 bit Unsigned Integer values
        /// </summary>
        /// <param name="value">The unsigned integer value to convert into bytes</param>
        /// <returns>The bytes in LittleEndian standard, regardless of the system architecture</returns>
        public static byte[] LittleEndianAsUint32(long value)
        {
            byte[] bValue = BitConverter.GetBytes(Convert.ToUInt32(value));
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Obtains the 4 byte equivalent for 32 bit Integer values
        /// </summary>
        /// <param name="value">The unsigned integer value to convert into bytes</param>
        /// <returns>The bytes in LittleEndian standard, regardless of the system architecture</returns>
        public static byte[] LittleEndian(int value)
        {
            byte[] bValue = BitConverter.GetBytes(value);
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Obtains the 4 byte equivalent for 32bit floating point values
        /// </summary>
        /// <param name="value">The unsigned integer value to convert into bytes</param>
        /// <returns>The bytes in LittleEndian standard, regardless of the system architecture</returns>
        public static byte[] LittleEndian(float value)
        {
            byte[] bValue = BitConverter.GetBytes(value);
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Obtains the 8 byte equivalent for 64 bit floating point values
        /// </summary>
        /// <param name="value">The unsigned integer value to convert into bytes</param>
        /// <returns>The bytes in LittleEndian standard, regardless of the system architecture</returns>
        public static byte[] LittleEndian(double value)
        {
            byte[] bValue = BitConverter.GetBytes(value);
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Gets the 2 byte equivalent of an unsigned short in little endian format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] LittleEndianAsUShort(int value)
        {
            byte[] bValue = BitConverter.GetBytes(Convert.ToUInt16(value));
            return LittleEndian(bValue);
        }

        /// <summary>
        /// Gets the 2 byte equivalent of a short in little endian format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] LittleEndian(short value)
        {
            byte[] bValue = BitConverter.GetBytes(value);
            return LittleEndian(bValue);
        }

        #endregion

        #region Properties

        #endregion
    }
}