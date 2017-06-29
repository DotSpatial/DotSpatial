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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/28/2010 2:31:18 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

//ORIGINAL HEADER FROM C++ source which was converted to C# by Ted Dunsford 2/28/2010
/******************************************************************************
 * $Id: hfacompress.cpp, v 1.3 2005/09/23 14:53:48 fwarmerdam Exp $
 *
 * Name:     hfadataset.cpp
 * Project:  Erdas Imagine Driver
 * Purpose:  Imagine Compression code.
 * Author:   Sam Gillingham <sam.gillingham at nrm.qld.gov>
 *
 ******************************************************************************
 * Copyright (c) 2005, Sam Gillingham
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 *****************************************************************************
 *
 * $Log: hfacompress.cpp, v $
 * Revision 1.3  2005/09/23 14:53:48  fwarmerdam
 * Bug 928: Fix initialization size of m_pCounts and m_pValues.
 *
 * Revision 1.2  2005/08/20 23:46:28  fwarmerdam
 * bug 858: fix for double compression
 *
 * Revision 1.1  2005/01/10 17:40:40  fwarmerdam
 * New
 *
 */

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaCompress - Development on hold, commented out to reduce warnings
    /// </summary>
    public class HfaCompress
    {
        //#region Private Variables

        //private byte[] _data;
        ////private long _dataOffset;

        //private byte[] _counts;
        ////private byte[] _currCount = null;
        //private uint _sizeCounts =0;

        //private byte[] _values;
        ////private byte[] _currValues;
        ////private uint _sizeValues;

        ////private uint _min;
        ////private uint _numRuns;
        ////private byte _numBits;

        //private bool _compressed;
        ////private int _dataTypeNumBits;
        //private EPT _dataType;
        ////private int _blockSize;
        //private int _blockCount;

        //#endregion

        //#region Constructors

        ///// <summary>
        ///// Creates a new instance of HfaCompress from the data byte, and starts at the data offset.
        ///// </summary>
        //public HfaCompress(byte[] data, long dataOffset, int blockSize, EPT dataType)
        //{
        //    _compressed = false;
        //    _data = data;
        //    //_dataOffset = dataOffset;
        //    //_dataTypeNumBits = dataType.GetBitCount();

        //    //_blockSize = blockSize;
        //    //_blockCount = blockSize/(_dataTypeNumBits/8);

        //    // Allocate memory for the count and values, probably too big
        //    // about right for worst case scenario
        //    //_counts = new byte[_blockSize + sizeof(uint)];
        //   // _values = new byte[_blockSize + sizeof(uint)];
        //}

        //#endregion

        //#region Methods

        ///// <summary>
        ///// Returns the number of bits required to encode a count.
        ///// </summary>
        ///// <param name="range"></param>
        ///// <returns></returns>
        //private byte FindNumBits(int range)
        //{
        //    if (range < 0xff)
        //    {
        //        return 8;
        //    }
        //    else if (range < 0xffff)
        //    {
        //        return 16;
        //    }
        //    else
        //    {
        //        return 32;
        //    }
        //}

        //private int ValueAsInt(int index)
        //{
        //    int val = 0;
        //    //switch (_dataTypeNumBits)
        //    //{
        //    //    case 8:
        //    //        val = _data[index];
        //    //        break;
        //    //    case 16:
        //    //        val = BitConverter.ToInt16(_data, index*2);
        //    //        break;
        //    //    case 32:
        //    //        val = BitConverter.ToInt32(_data, index*4);
        //    //        break;
        //    //}
        //    return val;
        //}

        //private int FindMin(byte[] data, long dataOffset)
        //{
        //    return 0;
        //    //int val;
        //    //int min = int.MaxValue;
        //    //int max = 0;
        //    //for (int count = 1; count < _blockCount; count++)
        //    //{
        //    //    val = ValueAsInt(count);

        //    //}
        //    //return 0;
        //}

        ///// <summary>
        ///// This actually compresses the data.  This will
        ///// </summary>
        ///// <returns></returns>
        //public bool CompressBlock ()
        //{
        //    // do stuff
        //    _compressed = true;
        //    return false;
        //}

        ///// <summary>
        ///// This compress algorithm only works for 8, 16 or 32 bit data types.
        ///// </summary>
        ///// <param name="dataType"></param>
        ///// <returns></returns>
        //public bool QueryDataTypeSupported(EPT dataType)
        //{
        //    int nBits = dataType.GetBitCount();
        //    return (nBits == 8 || nBits == 16 || nBits == 32);
        //}

        //#endregion

        //#region Properties

        ///// <summary>
        ///// Only valid after compression
        ///// </summary>
        ///// <exception cref="HfaNotCompressedException">This can only be accessed after compression.</exception>
        //public byte[] Counts
        //{
        //    get
        //    {
        //        if (!_compressed)throw new HfaNotCompressedException();
        //        return _counts;
        //    }
        //}

        ///// <summary>
        ///// Only valid after compression
        ///// </summary>
        ///// <exception cref="HfaNotCompressedException">This can only be accessed after compression.</exception>
        //public uint CountSize
        //{
        //    get
        //    {
        //        if (!_compressed) throw new HfaNotCompressedException();
        //        return _sizeCounts;
        //    }
        //}

        ///// <summary>
        ///// Gets the byte values
        ///// </summary>
        ///// <exception cref="HfaNotCompressedException">This can only be accessed after compression.</exception>
        //public byte[] Values
        //{
        //    get
        //    {
        //        if (!_compressed) throw new HfaNotCompressedException();
        //        return _values;
        //    }
        //}

        ///// <summary>
        ///// Gets the size of the values array
        ///// </summary>
        ///// <exception cref="HfaNotCompressedException">This can only be accessed after compression.</exception>
        //public uint ValueSize
        //{
        //    get
        //    {
        //        if (!_compressed) throw new HfaNotCompressedException();
        //        return _sizeValues;
        //    }
        //}

        ///// <summary>
        ///// Gets the min ?
        ///// </summary>
        ///// <exception cref="HfaNotCompressedException">This can only be accessed after compression.</exception>
        //public uint Min
        //{
        //    get
        //    {
        //        if (!_compressed) throw new HfaNotCompressedException();
        //        return _min;
        //    }
        //}

        ///// <summary>
        ///// Gets the number of runs
        ///// </summary>
        //public uint NumRuns
        //{
        //    get
        //    {
        //        if (!_compressed) throw new HfaNotCompressedException();
        //        return _numRuns;
        //    }
        //}

        ///// <summary>
        ///// Gets the number of bits
        ///// </summary>
        //public byte NumBits
        //{
        //    get
        //    {
        //        if (!_compressed) throw new HfaNotCompressedException();
        //        return _numBits;
        //    }
        //}

        ///// <summary>
        ///// Gets a boolean that indicates whether or not the CompressBlock expression has been called, allowing access to the
        ///// various other proeprties.
        ///// </summary>
        //public bool Compressed
        //{
        //    get { return _compressed; }
        //}
        //
        //
        //
        //#endregion
    }
}