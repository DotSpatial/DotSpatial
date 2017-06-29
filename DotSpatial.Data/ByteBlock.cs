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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2009 10:21:38 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// ByteBlock
    /// </summary>
    public class ByteBlock
    {
        #region Private Variables

        /// <summary>
        /// The block size of the arrays
        /// </summary>
        public readonly int BlockSize;

        /// <summary>
        /// All the blocks
        /// </summary>
        public List<Byte[]> Blocks;

        /// <summary>
        /// The current block index
        /// </summary>
        public int CurrentBlock;

        /// <summary>
        /// The offset
        /// </summary>
        public int Offset;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ByteBlock
        /// </summary>
        public ByteBlock(int blockSize)
        {
            BlockSize = blockSize;
            Blocks = new List<byte[]> {new byte[BlockSize]};
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the number of bytes using the specified reader.
        /// This handles copying across blocks if necessary.
        /// </summary>
        /// <param name="numBytes">The integer number of bytes to read</param>
        /// <param name="stream">The file or data stream to read from.</param>
        public void Read(int numBytes, Stream stream)
        {
            DoRead(numBytes, (bytes, ind, count) => stream.Read(bytes, ind, count));
        }

        /// <summary>
        /// Reads the number of bytes using the specified reader.
        /// This handles copying across blocks if necessary.
        /// </summary>
        /// <param name="numBytes"></param>
        /// <param name="reader"></param>
        public void Read(int numBytes, BufferedBinaryReader reader)
        {
            DoRead(numBytes, reader.Read);
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

        /// <summary>
        /// If the bytes were converted to a single, contiguous integer array, this returns
        /// the current offset in that array.
        /// </summary>
        /// <returns></returns>
        public int IntOffset()
        {
            return ((Blocks.Count - 1) * BlockSize + Offset) / 4;
        }

        /// <summary>
        /// If the bytes were converted to a single contiguous double array, this returns
        /// the offset in that array.
        /// </summary>
        /// <returns></returns>
        public int DoubleOffset()
        {
            return ((Blocks.Count - 1) * BlockSize + Offset) / 8;
        }

        /// <summary>
        /// Resets the indices
        /// </summary>
        public void Reset()
        {
            Offset = 0;
            CurrentBlock = 0;
        }

        /// <summary>
        /// Combines all the blocks into a single array of the specified datatype
        /// </summary>
        /// <returns></returns>
        public int[] ToIntArray()
        {
            var result = new int[IntOffset()];
            CopyBlocksToArray(result);
            return result;
        }

        /// <summary>
        /// Combines all the blocks into a single array of the specified datatype
        /// </summary>
        /// <returns></returns>
        public double[] ToDoubleArray()
        {
            var result = new double[DoubleOffset()];
            CopyBlocksToArray(result);
            return result;
        }

        private void CopyBlocksToArray(Array dest)
        {
            for (var iblock = 0; iblock < CurrentBlock; iblock++)
            {
                Buffer.BlockCopy(Blocks[iblock], 0, dest, (BlockSize * iblock), BlockSize);
            }
            Buffer.BlockCopy(Blocks[CurrentBlock], 0, dest, (BlockSize * CurrentBlock), Offset);   
        }

        #endregion
    }
}