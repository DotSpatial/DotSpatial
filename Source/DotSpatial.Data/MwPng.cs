// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
// Based on W3C png specification: http://www.w3.org/TR/2003/REC-PNG-20031110/#11PLTE
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/18/2010 1:21:47 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace DotSpatial.Data
{
    /// <summary>
    /// mw.png provides read-write support for a png format that also can provide overviews etc.
    /// This cannot work with any possible png file, but rather provides at least one common
    /// format that can be used natively for large files that is better at compression than
    /// just storing the values directly.
    /// http://www.w3.org/TR/2003/REC-PNG-20031110/#11PLTE
    /// </summary>
    public class MwPng
    {
        #region Methods

        /// <summary>
        /// Many rows may be evaluated by this process, but the first value in the array should
        /// be aligned with the left side of the image.
        /// </summary>
        /// <param name="refData">The original bytes to apply the PaethPredictor to.</param>
        /// <param name="offset">The integer offset in the array where the filter should begin application. If this is 0, then
        /// it assumes that there is no previous scan-line to work with.</param>
        /// <param name="length">The number of bytes to filter, starting at the specified offset. This should be evenly divisible by the width.</param>
        /// <param name="width">The integer width of a scan-line for grabbing the c and b bytes</param>
        /// <returns>The entire length of bytes starting with the specified offset</returns>
        /// <exception cref="PngInsuficientLengthException">Thrown if the offset and the length together lay after the refData.Length.</exception>
        public static byte[] Filter(byte[] refData, int offset, int length, int width)
        {
            // the 'B' and 'C' values of the first row are considered to be 0.
            // the 'A' value of the first column is considered to be 0.
            if (refData.Length - offset < length)
            {
                throw new PngInsuficientLengthException(length, refData.Length, offset);
            }

            int numrows = length / width;

            // The output also consists of a byte before each line that specifies the Paeth prediction filter is being used
            byte[] result = new byte[numrows + length];

            int source = offset;
            int dest = 0;
            for (int row = 0; row < numrows; row++)
            {
                result[dest] = 4;
                dest++;
                for (int col = 0; col < width; col++)
                {
                    byte a = (col == 0) ? (byte)0 : refData[source - 1];
                    byte b = (row == 0 || col == 0) ? (byte)0 : refData[source - width - 1];
                    byte c = (row == 0) ? (byte)0 : refData[source - width];
                    result[dest] = (byte)((refData[source] - PaethPredictor(a, b, c)) % 256);
                    source++;
                    dest++;
                }
            }

            return result;
        }

        /// <summary>
        /// Checks whether the signature is valid.
        /// </summary>
        /// <param name="signature">The signature.</param>
        /// <returns>True, if the signatur is valid.</returns>
        public static bool SignatureIsValid(byte[] signature)
        {
            byte[] test = { 137, 80, 78, 71, 13, 10, 26, 10 };

            for (int i = 0; i < 8; i++)
            {
                if (signature[i] != test[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// Converts a value into Big-endian Uint format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static byte[] ToBytesAsUInt32(long value)
        {
            uint temp = Convert.ToUInt32(value);
            byte[] arr = BitConverter.GetBytes(temp);
            if (BitConverter.IsLittleEndian) Array.Reverse(arr);
            return arr;
        }

        /// <summary>
        /// Reads a fileName into the specified bitmap.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The created bitmap.</returns>
        /// <exception cref="PngInvalidSignatureException">If the file signature doesn't match the png file signature</exception>
        public Bitmap Read(string fileName)
        {
            Stream f = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, 1000000);
            byte[] signature = new byte[8];
            f.Read(signature, 0, 8);
            if (!SignatureIsValid(signature))
            {
                throw new PngInvalidSignatureException();
            }

            f.Close();
            return new Bitmap(10, 10);
        }

        /// <summary>
        /// Unfilters the data in order to reconstruct the original values.
        /// </summary>
        /// <param name="filterStream">The filtered but decompressed bytes</param>
        /// <param name="offset">the integer offset where reconstruction should begin</param>
        /// <param name="length">The integer length of bytes to deconstruct</param>
        /// <param name="width">The integer width of a scan-line in bytes (not counting any filter type bytes.</param>
        /// <returns>The unfiltered data.</returns>
        /// <exception cref="PngInsuficientLengthException">Thrown if the length and offset together lay behind the streams total length.</exception>
        public byte[] UnFilter(byte[] filterStream, int offset, int length, int width)
        {
            // the 'B' and 'C' values of the first row are considered to be 0.
            // the 'A' value of the first column is considered to be 0.
            if (filterStream.Length - offset < length)
            {
                throw new PngInsuficientLengthException(length, filterStream.Length, offset);
            }

            int numrows = length / width;

            // The output also consists of a byte before each line that specifies the Paeth prediction filter is being used
            byte[] result = new byte[length - numrows];

            int source = offset;
            int dest = 0;
            for (int row = 0; row < numrows; row++)
            {
                if (filterStream[source] == 0)
                {
                    source++;

                    // No filtering
                    Array.Copy(filterStream, source, result, dest, width);
                    source += width;
                    dest += width;
                }
                else if (filterStream[source] == 1)
                {
                    source++;
                    for (int col = 0; col < width; col++)
                    {
                        byte a = (col == 0) ? (byte)0 : result[dest - 1];
                        result[dest] = (byte)((filterStream[dest] + a) % 256);
                        source++;
                        dest++;
                    }
                }
                else if (filterStream[source] == 2)
                {
                    source++;
                    for (int col = 0; col < width; col++)
                    {
                        byte b = (row == 0 || col == 0) ? (byte)0 : result[dest - width - 1];

                        result[dest] = (byte)((filterStream[dest] + b) % 256);
                        source++;
                        dest++;
                    }
                }
                else if (filterStream[source] == 3)
                {
                    source++;
                    for (int col = 0; col < width; col++)
                    {
                        byte a = (col == 0) ? (byte)0 : result[dest - 1];
                        byte b = (row == 0 || col == 0) ? (byte)0 : result[dest - width - 1];
                        result[dest] = (byte)((filterStream[dest] + ((a + b) / 2)) % 256); // integer division automatically does "floor"
                        source++;
                        dest++;
                    }
                }
                else if (filterStream[source] == 4)
                {
                    source++;
                    for (int col = 0; col < width; col++)
                    {
                        byte a = (col == 0) ? (byte)0 : result[dest - 1];
                        byte b = (row == 0 || col == 0) ? (byte)0 : result[dest - width - 1];
                        byte c = (row == 0) ? (byte)0 : result[dest - width];
                        result[dest] = (byte)((filterStream[dest] + PaethPredictor(a, b, c)) % 256);
                        source++;
                        dest++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// For testing, see if we can write a png ourself that can be opened by .Net png.
        /// </summary>
        /// <param name="image">The image to write to png format</param>
        /// <param name="fileName">The string fileName</param>
        public void Write(Bitmap image, string fileName)
        {
            if (File.Exists(fileName)) File.Delete(fileName);
            Stream f = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1000000);
            WriteSignature(f);
            PngHeader header = new PngHeader(image.Width, image.Height);
            header.Write(f);
            WriteSrgb(f);
            byte[] refImage = GetBytesFromImage(image);
            byte[] filtered = Filter(refImage, 0, refImage.Length, image.Width * 4);
            byte[] compressed = Deflate.Compress(filtered);
            WriteIDat(f, compressed);
            WriteEnd(f);
            f.Flush();
            f.Close();
        }

        private static byte[] GetBytesFromImage(Bitmap image)
        {
            BitmapData bd = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            int len = image.Width * image.Height * 4;
            byte[] refImage = new byte[len];
            Marshal.Copy(bd.Scan0, refImage, 0, len);
            image.UnlockBits(bd);
            return refImage;
        }

        /// <summary>
        /// B C   - For the current pixel X, use the best fit from B, C or A to predict X.
        /// A X
        /// </summary>
        /// <param name="a">The a</param>
        /// <param name="b">The b</param>
        /// <param name="c">The c</param>
        /// <returns>The predicted value.</returns>
        private static byte PaethPredictor(byte a, byte b, byte c)
        {
            byte pR;
            int p = a + b - c;
            int pa = Math.Abs(p - a);
            int pb = Math.Abs(p - b);
            int pc = Math.Abs(p - c);
            if (pa <= pb && pa <= pc) pR = a;
            else if (pb <= pc) pR = b;
            else pR = c;
            return pR;
        }

        private static void WriteEnd(Stream f)
        {
            f.Write(BitConverter.GetBytes(0), 0, 4);
            byte[] vals = new byte[] { 73, 69, 78, 68 }; // IEND
            f.Write(vals, 0, 4);
            f.Write(ToBytesAsUInt32(Crc32.ComputeChecksum(vals)), 0, 4);
        }

        private static void WriteIDat(Stream f, byte[] data)
        {
            f.Write(ToBytesAsUInt32((uint)data.Length), 0, 4);
            byte[] tag = { 73, 68, 65, 84 };
            f.Write(tag, 0, 4); // IDAT
            f.Write(data, 0, data.Length);
            byte[] combined = new byte[data.Length + 4];
            Array.Copy(tag, 0, combined, 0, 4);
            Array.Copy(data, 0, combined, 4, data.Length);
            f.Write(ToBytesAsUInt32(Crc32.ComputeChecksum(combined)), 0, 4);
        }

        private static void WriteSignature(Stream f)
        {
            f.Write(new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 }, 0, 8);
        }

        private static void WriteSrgb(Stream f)
        {
            f.Write(ToBytesAsUInt32(1), 0, 4);
            byte[] vals = { 115, 82, 71, 66, 0 }; // sRGB and the value of 0
            f.Write(vals, 0, 5);
            f.Write(ToBytesAsUInt32(Crc32.ComputeChecksum(vals)), 0, 4);
        }

        #endregion
    }
}