// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// ByteBlock.
    /// </summary>
    public class ByteBlock
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteBlock"/> class.
        /// </summary>
        /// <param name="blockSize">Block size of the arrays.</param>
        public ByteBlock(int blockSize)
        {
            BlockSize = blockSize;
            Blocks = new List<byte[]> { new byte[BlockSize] };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets all the blocks.
        /// </summary>
        public List<byte[]> Blocks { get; set; }

        /// <summary>
        /// Gets the block size of the arrays.
        /// </summary>
        public int BlockSize { get; }

        /// <summary>
        /// Gets or sets the current block index.
        /// </summary>
        public int CurrentBlock { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public int Offset { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// If the bytes were converted to a single contiguous double array, this returns
        /// the offset in that array.
        /// </summary>
        /// <returns>Current offset in the array.</returns>
        public int DoubleOffset()
        {
            return ((Blocks.Count - 1) * BlockSize + Offset) / 8;
        }

        /// <summary>
        /// If the bytes were converted to a single, contiguous integer array, this returns
        /// the current offset in that array.
        /// </summary>
        /// <returns>Current offset in the array.</returns>
        public int IntOffset()
        {
            return ((Blocks.Count - 1) * BlockSize + Offset) / 4;
        }

        /// <summary>
        /// Reads the number of bytes using the specified reader.
        /// This handles copying across blocks if necessary.
        /// </summary>
        /// <param name="numBytes">The integer number of bytes to read.</param>
        /// <param name="stream">The file or data stream to read from.</param>
        public void Read(int numBytes, Stream stream)
        {
            DoRead(numBytes, (bytes, ind, count) => stream.Read(bytes, ind, count));
        }

        /// <summary>
        /// Reads the number of bytes using the specified reader.
        /// This handles copying across blocks if necessary.
        /// </summary>
        /// <param name="numBytes">Number of bytes that should be read.</param>
        /// <param name="reader">Reader used for reading.</param>
        public void Read(int numBytes, BufferedBinaryReader reader)
        {
            DoRead(numBytes, reader.Read);
        }

        /// <summary>
        /// Resets the indices.
        /// </summary>
        public void Reset()
        {
            Offset = 0;
            CurrentBlock = 0;
        }

        /// <summary>
        /// Combines all the blocks into a single array of the specified datatype.
        /// </summary>
        /// <returns>The combined array.</returns>
        public double[] ToDoubleArray()
        {
            var result = new double[DoubleOffset()];
            CopyBlocksToArray(result);
            return result;
        }

        /// <summary>
        /// Combines all the blocks into a single array of the specified datatype.
        /// </summary>
        /// <returns>The combined array.</returns>
        public int[] ToIntArray()
        {
            var result = new int[IntOffset()];
            CopyBlocksToArray(result);
            return result;
        }

        private void CopyBlocksToArray(Array dest)
        {
            for (var iblock = 0; iblock < CurrentBlock; iblock++)
            {
                Buffer.BlockCopy(Blocks[iblock], 0, dest, BlockSize * iblock, BlockSize);
            }

            Buffer.BlockCopy(Blocks[CurrentBlock], 0, dest, BlockSize * CurrentBlock, Offset);
        }

        private void DoRead(int numBytes, Action<byte[], int, int> reader)
        {
            while (true)
            {
                if (Offset + numBytes < BlockSize)
                {
                    if (numBytes > 0)
                    {
                        reader(Blocks[CurrentBlock], Offset, numBytes);
                        Offset += numBytes;
                    }

                    return;
                }

                var firstLen = BlockSize - Offset;
                var secondLen = numBytes - firstLen;
                reader(Blocks[CurrentBlock], Offset, firstLen);
                Offset = 0;
                CurrentBlock += 1;
                if (Blocks.Count <= CurrentBlock) Blocks.Add(new byte[BlockSize]);

                // Read remaining parts
                numBytes = secondLen;
            }
        }

        #endregion
    }
}