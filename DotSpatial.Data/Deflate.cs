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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/19/2010 12:10:46 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.IO;
using System.IO.Compression;

namespace DotSpatial.Data
{
    /// <summary>
    /// Deflated data content is widely understood by gzip utilities, but this system is specifically designed to work
    /// with png data format, and so will only work with code 8 compression strategy.
    /// </summary>
    public static class Deflate
    {
        #region Methods

        /// <summary>
        /// Compress
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] values)
        {
            MemoryStream msOut = new MemoryStream(values.Length);
            DeflateStream deflator = new DeflateStream(msOut, CompressionMode.Compress);
            deflator.Write(values, 0, values.Length);

            byte[] deflateArray = msOut.ToArray();
            byte[] result = new byte[deflateArray.Length + 6];
            result[0] = 120;
            result[1] = 94;
            Array.Copy(deflateArray, 0, result, 3, deflateArray.Length);
            uint check = Adler32(values);
            byte[] chkbytes = BitConverter.GetBytes(check);
            if (BitConverter.IsLittleEndian) Array.Reverse(chkbytes);
            Array.Copy(chkbytes, 0, result, deflateArray.Length + 2, 4);
            return result;
        }

        /// <summary>
        /// Decompress
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] values)
        {
            // chop off header and tail
            byte[] raw = new byte[values.Length - 6];
            Array.Copy(values, 2, raw, 0, values.Length - 6);
            MemoryStream msOut = new MemoryStream(values.Length);
            DeflateStream deflator = new DeflateStream(msOut, CompressionMode.Decompress);
            deflator.Write(raw, 0, values.Length);
            return msOut.ToArray();
        }

        private static uint Adler32(byte[] data)
        {
            uint a = 1, b = 0;
            for (int index = 0; index < data.Length; index++)
            {
                a = (a + data[index]) % 65521;
                b = (b + a) % 65521;
            }
            return (b << 16) | a;
        }

        #endregion Methods
    }
}