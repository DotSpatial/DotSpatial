// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// Header of a png file.
    /// </summary>
    public class PngHeader
    {
        #region Fields

        /// <summary>
        /// At this time, the only compression method recognized is 0 - deflate/inflate with a
        /// sliding window of at most 32768 bytes.
        /// </summary>
        public const byte CompressionMethod = 0;

        /// <summary>
        /// At this time, only filter method 0 is outlined in the international standards.
        /// (adaptive filtering with 5 basic filter types).
        /// </summary>
        public const byte FilterMethod = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PngHeader"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public PngHeader(int width, int height)
        {
            Width = width;
            Height = height;
            BitDepth = BitDepth.Eight;
            ColorType = ColorType.TruecolorAlpha;
            InterlaceMethod = InterlaceMethod.NoInterlacing;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the bit depth. Depending on the Color Type, not all are allowed:
        /// Greyscale - 1, 2, 4, 8, 16
        /// Truecolor - 8, 16
        /// Indexed - 1, 2, 4, 8
        /// Greyscale/alpha - 8, 16
        /// TrueColor/alpha - 8, 16.
        /// </summary>
        public BitDepth BitDepth { get; set; }

        /// <summary>
        /// Gets or sets the color type.
        /// </summary>
        public ColorType ColorType { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the interlacing method used for this image.
        /// </summary>
        public InterlaceMethod InterlaceMethod { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public int Width { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the important content from the stream of bytes.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <returns>The png header containing the data that was read.</returns>
        public static PngHeader FromBytes(Stream stream)
        {
            byte[] vals = new byte[25];
            stream.Read(vals, 0, 25);
            int w = (int)BitConverter.ToUInt32(vals, 9);
            int h = (int)BitConverter.ToUInt32(vals, 13);

            PngHeader result = new PngHeader(w, h)
            {
                BitDepth = (BitDepth)vals[17],
                ColorType = (ColorType)vals[18],
                InterlaceMethod = (InterlaceMethod)vals[21]
            };
            return result;
        }

        /// <summary>
        /// Returns the image header in bytes.
        /// </summary>
        /// <returns>The image header in bytes.</returns>
        public byte[] ToBytes()
        {
            byte[] vals = new byte[25];

            Write(vals, 0, 13);
            vals[4] = 73; // I
            vals[5] = 72; // H
            vals[6] = 68; // D
            vals[7] = 82; // R
            Write(vals, 8, Width);
            Write(vals, 12, Height);
            vals[16] = (byte)BitDepth;
            vals[17] = (byte)ColorType;
            vals[18] = CompressionMethod;
            vals[19] = FilterMethod;
            vals[20] = (byte)InterlaceMethod;

            // CRC check is calculated on the chunk type and chunk data, but not the length.
            byte[] data = new byte[13];
            Array.Copy(vals, 8, data, 0, 13);
            byte[] crcData = new byte[17];
            Array.Copy(vals, 4, crcData, 0, 4);
            Array.Copy(data, 0, crcData, 4, 13);
            WriteAsUint32(vals, 21, Crc32.ComputeChecksum(crcData)); // this should be ok
            return vals;
        }

        /// <summary>
        /// Writes the byte-format of this png image header chunk to the given stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void Write(Stream stream)
        {
            byte[] head = ToBytes();
            stream.Write(head, 0, head.Length);
        }

        /// <summary>
        /// Writes an integer in Big-endian Uint format.
        /// </summary>
        /// <param name="array">Array the value gets written to.</param>
        /// <param name="offset">Index in the destination array where writing should start.</param>
        /// <param name="value">Value that gets written.</param>
        public void Write(byte[] array, int offset, int value)
        {
            byte[] arr = BitConverter.GetBytes((uint)value);
            if (BitConverter.IsLittleEndian) Array.Reverse(arr);
            Array.Copy(arr, 0, array, offset, 4);
        }

        /// <summary>
        /// Writes an integer in Big-endian Uint format.
        /// </summary>
        /// <param name="array">Array the value gets written to.</param>
        /// <param name="offset">Index in the destination array where writing should start.</param>
        /// <param name="value">Value that gets written.</param>
        public void WriteAsUint32(byte[] array, int offset, long value)
        {
            byte[] arr = BitConverter.GetBytes(Convert.ToUInt32(value));
            if (BitConverter.IsLittleEndian) Array.Reverse(arr);
            Array.Copy(arr, 0, array, offset, 4);
        }

        #endregion
    }
}