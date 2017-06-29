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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2010 3:37:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

//ORIGINAL HEADER FROM C++ source which was converted to C# by Ted Dunsford 2/24/2010
/******************************************************************************
 * $Id: hfafield.cpp, v 1.21 2006/05/07 04:04:03 fwarmerdam Exp $
 *
 * Project:  Erdas Imagine (.img) Translator
 * Purpose:  Implementation of the HFAField class for managing information
 *           about one field in a HFA dictionary type.  Managed by HFAType.
 * Author:   Frank Warmerdam, warmerdam@pobox.com
 *
 ******************************************************************************
 * Copyright (c) 1999, Intergraph Corporation
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
 ******************************************************************************
 *
 * $Log: hfafield.cpp, v $
 * Revision 1.21  2006/05/07 04:04:03  fwarmerdam
 * fixed serious multithreading issue with ExtractInstValue (bug 1132)
 *
 * Revision 1.20  2006/04/03 04:33:16  fwarmerdam
 * Report basedata type.  Support reading basedata as a 1D array.  Fix
 * bug in basedata reading ... wasn't skippig 2 byte code before data.
 *
 * Revision 1.19  2006/03/29 14:24:04  fwarmerdam
 * added preliminary nodata support (readonly)
 *
 * Revision 1.18  2005/10/02 15:14:48  fwarmerdam
 * fixed size for <8bit basedata items
 *
 * Revision 1.17  2005/09/28 19:38:07  fwarmerdam
 * Added partial support for inline defined types.
 *
 * Revision 1.16  2005/05/10 00:56:17  fwarmerdam
 * fixed bug with setting entries in an array (with count setting)
 *
 * Revision 1.15  2004/02/13 15:58:11  warmerda
 * Fixed serious bug with GetInstBytes() for BASEDATA * fields with
 * a count of zero.  Such as the Excluded field of most stats nodes!
 *
 * Revision 1.14  2003/12/08 19:09:34  warmerda
 * implemented DumpInstValue and GetInstBytes for basedata
 *
 * Revision 1.13  2003/05/21 15:35:05  warmerda
 * cleanup type conversion warnings
 *
 * Revision 1.12  2003/04/22 19:40:36  warmerda
 * fixed email address
 *
 * Revision 1.11  2001/07/18 04:51:57  warmerda
 * added CPL_CVSID
 *
 * Revision 1.10  2000/12/29 16:37:32  warmerda
 * Use GUInt32 for all file offsets
 *
 * Revision 1.9  2000/10/12 19:30:32  warmerda
 * substantially improved write support
 *
 * Revision 1.8  2000/09/29 21:42:38  warmerda
 * preliminary write support implemented
 *
 * Revision 1.7  1999/06/01 13:07:59  warmerda
 * added speed up for indexing into fixes size object arrays
 *
 * Revision 1.6  1999/02/15 19:06:18  warmerda
 * Disable warning on field offsets for Intergraph delivery
 *
 * Revision 1.5  1999/01/28 18:28:28  warmerda
 * minor simplification of code
 *
 * Revision 1.4  1999/01/28 18:03:07  warmerda
 * Fixed some byte swapping problems, and problems with accessing data from
 * the file that isn't on a word boundary.
 *
 * Revision 1.3  1999/01/22 19:23:11  warmerda
 * Fixed bug with offset into arrays of structures.
 *
 * Revision 1.2  1999/01/22 17:37:59  warmerda
 * Fixed up support for variable sizes, and arrays of variable sized objects
 *
 * Revision 1.1  1999/01/04 22:52:10  warmerda
 * New
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaField
    /// </summary>
    public class HfaField
    {
        #region Private Variables

        private const int MAX_ENTRY_REPORT = 16;
        private List<string> _enumNames;
        private string _fieldName;
        private int _itemCount;
        private HfaType _itemObjectType;
        private string _itemObjectTypeString;
        private char _itemType;
        private int _numBytes;
        private char _pointer;

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="dict"></param>
        public void CompleteDefn(HfaDictionary dict)
        {
            // Get a reference to the type object if we have a type name
            // for this field (not a build in).
            if (_itemObjectTypeString != null)
            {
                _itemObjectType = dict[_itemObjectTypeString];
            }
            // Figure out the size
            if (_pointer == 'p')
            {
                _numBytes = -1;
            }
            else if (_itemObjectType != null)
            {
                _itemObjectType.CompleteDefn(dict);
                if (_itemObjectType.NumBytes == -1)
                {
                    _numBytes = -1;
                }
                else
                {
                    _numBytes = (short)(_itemObjectType.NumBytes * _itemCount);
                }
                if (_pointer == '*' && _numBytes != -1) _numBytes += 8;
            }
            else
            {
                _numBytes = (short)(HfaDictionary.GetItemSize(_itemType) * _itemCount);
            }
        }

        /// <summary>
        /// This writes formatted content for this field to the specified IO stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void Dump(Stream stream)
        {
            string typename = string.Empty;
            StreamWriter sw = new StreamWriter(stream);
            switch (_itemType)
            {
                case '1':
                    typename = "U1";
                    break;
                case '2':
                    typename = "U2";
                    break;
                case '4':
                    typename = "U4";
                    break;
                case 'c':
                    typename = "UChar";
                    break;
                case 'C':
                    typename = "CHAR";
                    break;
                case 'e':
                    typename = "ENUM";
                    break;
                case 's':
                    typename = "USHORT";
                    break;

                case 'S':
                    typename = "SHORT";
                    break;

                case 't':
                    typename = "TIME";
                    break;

                case 'l':
                    typename = "ULONG";
                    break;

                case 'L':
                    typename = "LONG";
                    break;

                case 'f':
                    typename = "FLOAT";
                    break;

                case 'd':
                    typename = "DOUBLE";
                    break;

                case 'm':
                    typename = "COMPLEX";
                    break;

                case 'M':
                    typename = "DCOMPLEX";
                    break;

                case 'b':
                    typename = "BASEDATA";
                    break;

                case 'o':
                    typename = _itemObjectTypeString;
                    break;

                case 'x':
                    typename = "InlineType";
                    break;

                default:
                    typename = "Unknowm";
                    break;
            }
            string tc = (_pointer == 'p' || _pointer == '*') ? _pointer.ToString() : " ";
            string name = typename.PadRight(19);
            sw.WriteLine("    " + name + " " + tc + " " + _fieldName + "[" + _itemCount + "];");
            if (_enumNames == null || _enumNames.Count <= 0) return;
            for (int i = 0; i < _enumNames.Count; i++)
            {
                sw.Write("        " + _enumNames[i] + "=" + i);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fpOut"></param>
        /// <param name="data"></param>
        /// <param name="dataOffset"></param>
        /// <param name="dataSize"></param>
        /// <param name="prefix"></param>
        public void DumpInstValue(Stream fpOut, byte[] data, long dataOffset, int dataSize, string prefix)
        {
            StreamWriter sw = new StreamWriter(fpOut);
            int extraOffset;
            int nEntries = GetInstCount(data, dataOffset);
            // Special case for arrays of characters or uchars which are printed as a string
            if ((_itemType == 'c' || _itemType == 'C') && nEntries > 0)
            {
                object value;
                if (ExtractInstValue(null, 0, data, dataOffset, dataSize, 's', out value, out extraOffset))
                {
                    sw.WriteLine(prefix + _fieldName + " = '" + (string)value + "'");
                }
                else
                {
                    sw.WriteLine(prefix + _fieldName + " = (access failed)");
                }
            }
            for (int iEntry = 0; iEntry < Math.Min(MAX_ENTRY_REPORT, nEntries); iEntry++)
            {
                sw.Write(prefix + _fieldName);
                if (nEntries > 1) sw.Write("[" + iEntry + "]");
                sw.Write(" = ");
                object value;
                switch (_itemType)
                {
                    case 'f':
                    case 'd':
                        if (ExtractInstValue(null, iEntry, data, dataOffset, dataSize, 'd', out value, out extraOffset))
                        {
                            sw.Write(value + "\n");
                        }
                        else
                        {
                            sw.Write("(access failed)\n");
                        }
                        break;
                    case 'b':
                        int nRows = Hfa.ReadInt32(data, dataOffset + 8);
                        int nColumns = Hfa.ReadInt32(data, dataOffset + 12);
                        HfaEPT type = Hfa.ReadType(data, dataOffset + 16);
                        sw.Write(nRows + "x" + nColumns + " basedata of type " + type);
                        break;
                    case 'e':

                        if (ExtractInstValue(null, iEntry, data, dataOffset, dataSize, 's', out value, out extraOffset))
                        {
                            sw.Write((string)value);
                        }
                        else
                        {
                            sw.Write("(accessfailed)");
                        }
                        break;
                    case 'o':
                        if (!ExtractInstValue(null, iEntry, data, dataOffset, dataSize, 'p', out value, out extraOffset))
                        {
                            sw.WriteLine("(access failed)");
                        }
                        else
                        {
                            // the pointer logic here is death!  Originally the pointer address was used
                            // nByteOffset = ((GByte*) val) - pabyData;
                            // This doesn't work in a safe context.
                            sw.Write("\n");
                            string szLongFieldName = prefix;
                            if (prefix.Length > 256) szLongFieldName = prefix.Substring(0, 256);
                            _itemObjectType.DumpInstValue(fpOut, data, dataOffset + extraOffset, dataSize - extraOffset, szLongFieldName);
                        }
                        break;
                    default:
                        if (ExtractInstValue(null, iEntry, data, dataOffset, dataSize, 'i', out value, out extraOffset))
                        {
                            sw.WriteLine(value);
                        }
                        else
                        {
                            sw.WriteLine("(access failed)\n");
                        }
                        break;
                }
            }
            if (nEntries > MAX_ENTRY_REPORT)
            {
                sw.Write(prefix + " ... remaining instances omitted ...\n");
            }
            if (nEntries == 0)
            {
                sw.Write(prefix + _fieldName + " = (no values)\n");
            }
        }

        /// <summary>
        /// SetInstValue
        /// </summary>
        /// <param name="sField"></param>
        /// <param name="indexValue"></param>
        /// <param name="data"></param>
        /// <param name="dataOffset"></param>
        /// <param name="dataSize"></param>
        /// <param name="reqType"></param>
        /// <param name="value"></param>
        /// <exception cref="HfaPointerInsertNotSupportedException">Attempting to insert a pointer is not supported.</exception>
        /// <exception cref="HfaEnumerationNotFoundException">Occurs if the specified value is not a valid member of the enumeration for this field.</exception>
        public void SetInstValue(string sField, int indexValue, byte[] data, long dataOffset, int dataSize, char reqType, object value)
        {
            // If this field contains a pointer, then we wil adjust the data offset relative to it.
            // This updates the offset info, but doesn't change the first four bytes.
            if (_pointer != '\0')
            {
                int nCount;
                if (NumBytes > -1)
                {
                    nCount = _itemCount;
                }
                else if (reqType == 's' && (_itemType == 'c' || _itemType == 'C'))
                {
                    // Either a string or a character array
                    nCount = 0;
                    IEnumerable<char> strVal = value as IEnumerable<char>;
                    if (strVal != null) nCount = strVal.Count() + 1;
                }
                else
                {
                    nCount = indexValue + 1;
                }
                uint nOffset = (uint)nCount;
                Array.Copy(Hfa.LittleEndian(nOffset), 0, data, dataOffset + 4, 4);
                dataOffset += 8;
                dataSize -= 8;
            }
            // Pointers to char or uchar arrays requested as strings are handled as a special case
            if ((_itemType == 'c' || _itemType == 'C') && reqType == 's')
            {
                int nBytesToCopy = NumBytes;
                IEnumerable<char> strVal = value as IEnumerable<char>;
                if (strVal != null) nBytesToCopy = strVal.Count();
                if (NumBytes == -1 && strVal != null) nBytesToCopy = strVal.Count();

                // Force a blank erase to remove previous characters
                byte[] blank = new byte[nBytesToCopy];
                Array.Copy(blank, 0, data, dataOffset, nBytesToCopy);
                if (strVal != null)
                {
                    ASCIIEncoding ascii = new ASCIIEncoding();
                    string str = new string(strVal.ToArray());
                    byte[] charData = ascii.GetBytes(str);
                    Array.Copy(charData, 0, data, dataOffset, charData.Length);
                }
                return;
            }
            // Translate the passed type into different representations
            int nIntValue;
            double dfDoubleValue;
            if (reqType == 's')
            {
                nIntValue = int.Parse((string)value);
                dfDoubleValue = double.Parse((string)value);
            }
            else if (reqType == 'd')
            {
                dfDoubleValue = (double)value;
                nIntValue = Convert.ToInt32((double)value);
            }
            else if (reqType == 'i')
            {
                dfDoubleValue = Convert.ToDouble((int)value);
                nIntValue = (int)value;
            }
            else if (reqType == 'p')
            {
                throw new HfaPointerInsertNotSupportedException();
            }
            else
            {
                return;
            }

            // Handle by type
            switch (_itemType)
            {
                case 'c': // Char64
                case 'C': // Char128
                    if (reqType == 's')
                    {
                        // handled earlier as special case,
                    }
                    else
                    {
                        data[dataOffset + nIntValue] = (byte)nIntValue;
                    }
                    break;
                case 'e': // enums are stored as ushort
                case 's': // little s  = ushort type
                    {
                        if (_itemType == 'e' && reqType == 's')
                        {
                            nIntValue = _enumNames.IndexOf((string)value);
                            if (nIntValue == -1)
                            {
                                throw new HfaEnumerationNotFoundException((string)value);
                            }
                        }
                    }
                    // Each enumeration is stored as a 2-bit unsigned short entry.
                    ushort num = (ushort)nIntValue;
                    Array.Copy(Hfa.LittleEndian(num), 0, data, dataOffset + 2 * indexValue, 2);
                    break;
                case 'S': // signed short
                    {
                        short nNumber = (short)nIntValue;
                        Array.Copy(Hfa.LittleEndian(nNumber), 0, data, dataOffset + indexValue * 2, 2);
                    }
                    break;
                case 't':
                case 'l':
                    {
                        uint nNumber = (uint)nIntValue;
                        Array.Copy(Hfa.LittleEndian(nNumber), 0, data, dataOffset + indexValue * 4, 4);
                    }
                    break;
                case 'L': // Int32
                    {
                        int nNumber = nIntValue;
                        Array.Copy(Hfa.LittleEndian(nNumber), 0, data, dataOffset + indexValue * 4, 4);
                    }
                    break;
                case 'f': // Float (32 bit)
                    {
                        float dfNumber = Convert.ToSingle(dfDoubleValue);
                        Array.Copy(Hfa.LittleEndian(dfNumber), 0, data, dataOffset + indexValue * 4, 4);
                    }
                    break;
                case 'd': // Double (float 64)
                    {
                        Array.Copy(Hfa.LittleEndian(dfDoubleValue), 0, data, dataOffset + 8 * indexValue, 8);
                    }
                    break;
                case 'o': // object
                    {
                        if (_itemObjectType == null) break;
                        int nExtraOffset = 0;

                        if (_itemObjectType.NumBytes > 0)
                        {
                            nExtraOffset = _itemObjectType.NumBytes * indexValue;
                        }
                        else
                        {
                            for (int iIndexCounter = 0; iIndexCounter < indexValue; iIndexCounter++)
                            {
                                nExtraOffset += _itemObjectType.GetInstBytes(data, dataOffset + nExtraOffset);
                            }
                        }
                        if (!string.IsNullOrEmpty(sField))
                        {
                            _itemObjectType.SetInstValue(sField, data, dataOffset + nExtraOffset,
                                                         dataSize - nExtraOffset, reqType, value);
                        }
                    }
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Scans through the array and estimates the byte size of the field in this case.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataOffset"></param>
        /// <returns></returns>
        public int GetInstBytes(byte[] data, long dataOffset)
        {
            int nCount;
            int nInstBytes = 0;
            long offset = dataOffset;
            // Hard sized fields just return their constant size
            if (NumBytes > -1) return NumBytes;

            // Pointers have a 4 byte integer count and a 4 byte uint offset
            if (_pointer != '\0')
            {
                nCount = Hfa.ReadInt32(data, offset);
                offset += 8;
                nInstBytes += 8;
            }
            else
            {
                // Anything other than a pointer only can have one item.
                nCount = 1;
            }

            if (_itemType == 'b' && nCount != 0)
            {
                // BASEDATA
                int nRows = Hfa.ReadInt32(data, offset);
                offset += 4;
                int nColumns = Hfa.ReadInt32(data, offset);
                offset += 4;
                HfaEPT baseItemType = (HfaEPT)Hfa.ReadInt16(data, offset);
                nInstBytes += 12;
                nInstBytes += ((baseItemType.GetBitCount() + 7) / 8) * nRows * nColumns;
            }
            else if (_itemObjectType == null)
            {
                nInstBytes += nCount * HfaDictionary.GetItemSize(_itemType);
            }
            else
            {
                for (int i = 0; i < nCount; i++)
                {
                    nInstBytes += _itemObjectType.GetInstBytes(data, offset + nInstBytes);
                }
            }
            return nInstBytes;
        }

        /// <summary>
        /// Gets the count for a particular instance of a field.  This will normally be
        /// the built in value, but for variable fields, this is extracted from the
        /// data itself.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataOffset"></param>
        /// <returns></returns>
        public int GetInstCount(byte[] data, long dataOffset)
        {
            if (_pointer == '\0')
            {
                return _itemCount;
            }
            else if (_itemType == 'b')
            {
                int numRows = Hfa.ReadInt32(data, dataOffset + 8);
                int numColumns = Hfa.ReadInt32(data, dataOffset + 12);
                return numRows * numColumns;
            }
            else
            {
                return Hfa.ReadInt32(data, dataOffset);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pszField"></param>
        /// <param name="nIndexValue"></param>
        /// <param name="data"></param>
        /// <param name="dataOffset"></param>
        /// <param name="nDataSize"></param>
        /// <param name="reqType"></param>
        /// <param name="pReqReturn"></param>
        /// <param name="extraOffset">This is used in the case of 'object' pointers where the indexed object is further in the data block.</param>
        /// <returns></returns>
        /// <exception cref="HfaInvalidCountException">Occurs if the count is less than zero for the header of a block of base data</exception>
        public bool ExtractInstValue(string pszField, int nIndexValue, byte[] data, long dataOffset, int nDataSize, char reqType, out object pReqReturn, out int extraOffset)
        {
            extraOffset = 0;
            pReqReturn = null;
            string returnString = null;
            int nIntRet = 0;
            double dfDoubleRet = 0.0;
            // it doesn't appear like remove this line will have side effects.
            //int size = GetInstBytes(data, dataOffset);
            int nInstItemCount = GetInstCount(data, dataOffset);
            byte[] rawData = null;
            long offset = dataOffset;
            // Check the index value is valid.  Eventually this will have to account for variable fields.
            if (nIndexValue < 0 || nIndexValue >= nInstItemCount) return false;
            // if this field contains a pointer then we will adjust the data offset relative to it.
            if (_pointer != '\0')
            {
                long nOffset = Hfa.ReadUInt32(data, dataOffset + 4);
                if (nOffset != (uint)(dataOffset + 8))
                {
                    // It seems there was originally an exception that would have gone here.
                    // Original exception would have been pszFieldname.pszField points at nOffset, not nDataOffset +8 as expected.
                }
                offset += 8;
                dataOffset += 8;
                nDataSize -= 8;
            }
            // pointers to chara or uchar arrays are read as strings and then handled as a special case.
            if ((_itemType == 'c' || _itemType == 'C') && reqType == 's')
            {
                // Ok, nasty, the original code simply "pointed" to the byte data at this point and cast the pointer.
                // We probably need to cycle through until we reach the null character.
                List<char> chars = new List<char>();
                while ((char)data[offset] != '\0')
                {
                    chars.Add((char)data[offset]);
                    offset++;
                    dataOffset++;
                }
                pReqReturn = new String(chars.ToArray());
            }
            switch (_itemType)
            {
                case 'c':
                case 'C':
                    nIntRet = data[dataOffset + nIndexValue];
                    dfDoubleRet = nIntRet;
                    break;
                case 'e':
                case 's':
                    {
                        int nNumber = Hfa.ReadUInt16(data, dataOffset + nIndexValue * 2);
                        nIntRet = nNumber;
                        dfDoubleRet = nIntRet;
                        if (_itemType == 'e' && nIntRet >= 0 && nIntRet < _enumNames.Count())
                        {
                            returnString = _enumNames[nIntRet];
                        }
                    }
                    break;
                case 'S':
                    {
                        short nNumber = Hfa.ReadInt16(data, dataOffset + nIndexValue * 2);
                        nIntRet = nNumber;
                        dfDoubleRet = nNumber;
                    }
                    break;
                case 't':
                case 'l':
                    {
                        long nNumber = Hfa.ReadUInt32(data, dataOffset + nIndexValue * 2);
                        nIntRet = (int)nNumber;
                        dfDoubleRet = nNumber;
                    }
                    break;
                case 'L':
                    {
                        int nNumber = Hfa.ReadInt32(data, dataOffset + nIndexValue * 2);
                        nIntRet = nNumber;
                        dfDoubleRet = nNumber;
                    }
                    break;
                case 'f':
                    {
                        float fNumber = Hfa.ReadSingle(data, dataOffset + nIndexValue * 4);
                        dfDoubleRet = fNumber;
                        nIntRet = Convert.ToInt32(fNumber);
                    }
                    break;
                case 'd':
                    {
                        dfDoubleRet = Hfa.ReadDouble(data, dataOffset + nInstItemCount * 8);
                        nIntRet = Convert.ToInt32(dfDoubleRet);
                    }
                    break;
                case 'b': // BASE DATA
                    {
                        int nRows = Hfa.ReadInt32(data, dataOffset);
                        int nColumns = Hfa.ReadInt32(data, dataOffset + 4);
                        if (nIndexValue < 0 || nIndexValue >= nRows * nColumns) return false;
                        HfaEPT type = (HfaEPT)Hfa.ReadUInt16(data, dataOffset + 8);
                        // Ignore the 2 byte objecttype value
                        dataOffset += 12;
                        if (nRows < 0 || nColumns < 0) throw new HfaInvalidCountException(nRows, nColumns);

                        switch (type)
                        {
                            case HfaEPT.U8:
                                dfDoubleRet = data[dataOffset + nIndexValue];
                                nIntRet = data[offset + nIndexValue];
                                break;
                            case HfaEPT.S16:
                                short tShort = Hfa.ReadInt16(data, dataOffset + nIndexValue * 2);
                                dfDoubleRet = tShort;
                                nIntRet = tShort;
                                break;
                            case HfaEPT.U16:
                                int tUShort = Hfa.ReadUInt16(data, dataOffset + nIndexValue * 2);
                                dfDoubleRet = tUShort;
                                nIntRet = tUShort;
                                break;
                            case HfaEPT.Single:
                                Single tSingle = Hfa.ReadSingle(data, dataOffset + nIndexValue * 4);
                                dfDoubleRet = tSingle;
                                nIntRet = Convert.ToInt32(tSingle);
                                break;
                            case HfaEPT.Double:
                                dfDoubleRet = Hfa.ReadDouble(data, dataOffset + nIndexValue * 8);
                                nIntRet = Convert.ToInt32(dfDoubleRet);
                                break;
                            default:
                                pReqReturn = null;
                                return false;
                        }
                    }
                    break;
                case 'o':
                    if (_itemObjectType != null)
                    {
                        if (_itemObjectType.NumBytes > 0)
                        {
                            extraOffset = _itemObjectType.NumBytes * nIndexValue;
                        }
                        else
                        {
                            for (int iIndexCounter = 0; iIndexCounter < nIndexValue; iIndexCounter++)
                            {
                                extraOffset += _itemObjectType.GetInstBytes(data, dataOffset + extraOffset);
                            }
                        }
                        int len = _itemObjectType.GetInstBytes(data, dataOffset + extraOffset);
                        rawData = new byte[len];
                        Array.Copy(data, dataOffset + extraOffset, rawData, 0, len);
                        if (!string.IsNullOrEmpty(pszField))
                        {
                            return
                                (_itemObjectType.ExtractInstValue(pszField, rawData, 0, rawData.Length, reqType,
                                                                  out pReqReturn));
                        }
                    }
                    break;
                default:
                    return false;
            }
            // Handle appropriate representations
            switch (reqType)
            {
                case 's':
                    {
                        if (returnString == null)
                        {
                            returnString = nIntRet.ToString();
                        }
                        pReqReturn = returnString;
                        return true;
                    }
                case 'd':
                    pReqReturn = dfDoubleRet;
                    return true;
                case 'i':
                    pReqReturn = nIntRet;
                    return true;
                case 'p':
                    pReqReturn = rawData;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Parses the input string into a valid HfaField, or returns null
        /// if one could not be created.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Initialize(string input)
        {
            int length = input.Length;
            int start = 0;
            // Read the number
            if (!input.Contains(":")) return null;
            _itemCount = short.Parse(input.ExtractTo(ref start, ":"));

            // is this a pointer?
            if (input[start] == 'p' || input[start] == '*')
            {
                start += 1;
                _pointer = input[start];
            }
            // Get the general type
            start += 1;
            if (start == length) return null;
            _itemType = input[start];
            if (!"124cCesStlLfdmMbox".Contains(_itemType.ToString()))
            {
                throw new HfaFieldTypeException(_itemType);
            }

            // If this is an object, we extract the type of the object
            if (_itemType == 'o')
            {
                _itemObjectTypeString = input.ExtractTo(ref start, ",");
            }

            // If this is an inline object, we need to skip past the
            // definition, and then extract the object class name.
            // we ignore the actual definition, so if the object type isn't
            // already defined, things will not work properly.  See the
            // file lceugr250)00)pct.aus for an example of inline defs.
            if (_itemType == 'x' && input[start] == '{')
            {
                int braceDepth = 1;
                start += 1;
                // Skip past the definition in braces
                while (braceDepth > 0 && start < input.Length)
                {
                    if (input[start] == '{') braceDepth++;
                    if (input[start] == '}') braceDepth--;
                    start++;
                }
                _itemType = 'o';
                _itemObjectTypeString = input.ExtractTo(ref start, ",");
            }

            // If this is an enumeration, we have to extract all the values.
            if (_itemType == 'e')
            {
                if (!input.Contains(":")) return null;
                int enumCount = int.Parse(input.ExtractTo(ref start, ":"));
                if (_enumNames == null) _enumNames = new List<string>();
                for (int ienum = 0; ienum < enumCount; ienum++)
                {
                    _enumNames.Add(input.ExtractTo(ref start, ","));
                }
            }

            // Extract the field name
            _fieldName = input.ExtractTo(ref start, ",");

            // Return whatever is left in the string, which should be a new field.
            return input.Substring(start, input.Length - start);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the short integer number of bytes
        /// </summary>
        public int NumBytes
        {
            get { return _numBytes; }
            set { _numBytes = value; }
        }

        /// <summary>
        /// Gets or sets the short integer item count
        /// </summary>
        public int ItemCount
        {
            get { return _itemCount; }
            set { _itemCount = value; }
        }

        /// <summary>
        /// Gets or sets '\0', '*' or 'p'
        /// </summary>
        public char Pointer
        {
            get { return _pointer; }
            set { _pointer = value; }
        }

        /// <summary>
        /// Gets or sets 1|2|4|e|...
        /// </summary>
        public char ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        /// <summary>
        /// If ItemType == 'o'
        /// </summary>
        public string ItemObjectTypeString
        {
            get { return _itemObjectTypeString; }
            set { _itemObjectTypeString = value; }
        }

        /// <summary>
        /// Gets or sets the item object type
        /// </summary>
        public HfaType ItemObjectType
        {
            get { return _itemObjectType; }
            set { _itemObjectType = value; }
        }

        /// <summary>
        /// Normally null unless this is an enum
        /// </summary>
        public List<string> EnumNames
        {
            get { return _enumNames; }
            set { _enumNames = value; }
        }

        /// <summary>
        /// Gets or sets the field name
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        #endregion
    }
}