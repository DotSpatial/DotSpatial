// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

// ORIGINAL HEADER FROM C++ source which was converted to C# by Ted Dunsford 2/24/2010
/******************************************************************************
 * $Id: hfafield.cpp, v 1.21 2006/05/07 04:04:03 fwarmerdam Exp $
 *
 * Project:  Erdas Imagine (.img) Translator
 * Purpose:  Implementation of the HFAField class for managing information
 *           about one field in a HFA dictionary type. Managed by HFAType.
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
 * Report basedata type. Support reading basedata as a 1D array. Fix
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
 * a count of zero. Such as the Excluded field of most stats nodes!
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
        #region Fields

        private const int MaxEntryReport = 16;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the enum names. This is normally null unless this is an enum.
        /// </summary>
        public List<string> EnumNames { get; set; }

        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the short integer item count.
        /// </summary>
        public int ItemCount { get; set; }

        /// <summary>
        /// Gets or sets the item object type.
        /// </summary>
        public HfaType ItemObjectType { get; set; }

        /// <summary>
        /// Gets or sets the item object type string. If ItemType == 'o'.
        /// </summary>
        public string ItemObjectTypeString { get; set; }

        /// <summary>
        /// Gets or sets 1|2|4|e|...
        /// </summary>
        public char ItemType { get; set; }

        /// <summary>
        /// Gets or sets the short integer number of bytes.
        /// </summary>
        public int NumBytes { get; set; }

        /// <summary>
        /// Gets or sets '\0', '*' or 'p'
        /// </summary>
        public char Pointer { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Completes the defenition of this type based on the existing dictionary.
        /// </summary>
        /// <param name="dictionary">Dictionary used for completion.</param>
        public void CompleteDefn(HfaDictionary dictionary)
        {
            // Get a reference to the type object if we have a type name
            // for this field (not a build in).
            if (ItemObjectTypeString != null)
            {
                ItemObjectType = dictionary[ItemObjectTypeString];
            }

            // Figure out the size
            if (Pointer == 'p')
            {
                NumBytes = -1;
            }
            else if (ItemObjectType != null)
            {
                ItemObjectType.CompleteDefn(dictionary);
                if (ItemObjectType.NumBytes == -1)
                {
                    NumBytes = -1;
                }
                else
                {
                    NumBytes = (short)(ItemObjectType.NumBytes * ItemCount);
                }

                if (Pointer == '*' && NumBytes != -1) NumBytes += 8;
            }
            else
            {
                NumBytes = (short)(HfaDictionary.GetItemSize(ItemType) * ItemCount);
            }
        }

        /// <summary>
        /// This writes formatted content for this field to the specified IO stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void Dump(Stream stream)
        {
            string typename;
            StreamWriter sw = new StreamWriter(stream);
            switch (ItemType)
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
                    typename = ItemObjectTypeString;
                    break;

                case 'x':
                    typename = "InlineType";
                    break;

                default:
                    typename = "Unknowm";
                    break;
            }

            string tc = (Pointer == 'p' || Pointer == '*') ? Pointer.ToString() : " ";
            string name = typename.PadRight(19);
            sw.WriteLine("    " + name + " " + tc + " " + FieldName + "[" + ItemCount + "];");
            if (EnumNames == null || EnumNames.Count <= 0) return;

            for (int i = 0; i < EnumNames.Count; i++)
            {
                sw.Write("        " + EnumNames[i] + "=" + i);
            }
        }

        /// <summary>
        /// Dumps the fields value.
        /// </summary>
        /// <param name="fpOut">The output stream.</param>
        /// <param name="data">The data.</param>
        /// <param name="dataOffset">The offset to start from.</param>
        /// <param name="dataSize">The data size.</param>
        /// <param name="prefix">The prefix.</param>
        public void DumpInstValue(Stream fpOut, byte[] data, long dataOffset, int dataSize, string prefix)
        {
            StreamWriter sw = new StreamWriter(fpOut);
            int extraOffset;
            int nEntries = GetInstCount(data, dataOffset);

            // Special case for arrays of characters or uchars which are printed as a string
            if ((ItemType == 'c' || ItemType == 'C') && nEntries > 0)
            {
                object value;
                if (ExtractInstValue(null, 0, data, dataOffset, dataSize, 's', out value, out extraOffset))
                {
                    sw.WriteLine(prefix + FieldName + " = '" + (string)value + "'");
                }
                else
                {
                    sw.WriteLine(prefix + FieldName + " = (access failed)");
                }
            }

            for (int iEntry = 0; iEntry < Math.Min(MaxEntryReport, nEntries); iEntry++)
            {
                sw.Write(prefix + FieldName);
                if (nEntries > 1) sw.Write("[" + iEntry + "]");
                sw.Write(" = ");
                object value;
                switch (ItemType)
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
                        HfaEpt type = Hfa.ReadType(data, dataOffset + 16);
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
                            ItemObjectType.DumpInstValue(fpOut, data, dataOffset + extraOffset, dataSize - extraOffset, szLongFieldName);
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

            if (nEntries > MaxEntryReport)
            {
                sw.Write(prefix + " ... remaining instances omitted ...\n");
            }

            if (nEntries == 0)
            {
                sw.Write(prefix + FieldName + " = (no values)\n");
            }
        }

        /// <summary>
        /// Extracts the value.
        /// </summary>
        /// <param name="pszField">The psz field.</param>
        /// <param name="nIndexValue">The index value.</param>
        /// <param name="data">The data.</param>
        /// <param name="dataOffset">The offset to start from.</param>
        /// <param name="nDataSize">The data size.</param>
        /// <param name="reqType">The req type.</param>
        /// <param name="pReqReturn">The req return.</param>
        /// <param name="extraOffset">This is used in the case of 'object' pointers where the indexed object is further in the data block.</param>
        /// <returns>True, if the value could be extracted.</returns>
        /// <exception cref="HfaInvalidCountException">Occurs if the count is less than zero for the header of a block of base data</exception>
        public bool ExtractInstValue(string pszField, int nIndexValue, byte[] data, long dataOffset, int nDataSize, char reqType, out object pReqReturn, out int extraOffset)
        {
            extraOffset = 0;
            pReqReturn = null;
            string returnString = null;
            int nIntRet = 0;
            double dfDoubleRet = 0.0;

            // it doesn't appear like remove this line will have side effects.
            // int size = GetInstBytes(data, dataOffset);
            int nInstItemCount = GetInstCount(data, dataOffset);
            byte[] rawData = null;
            long offset = dataOffset;

            // Check the index value is valid. Eventually this will have to account for variable fields.
            if (nIndexValue < 0 || nIndexValue >= nInstItemCount) return false;

            // if this field contains a pointer then we will adjust the data offset relative to it.
            if (Pointer != '\0')
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
            if ((ItemType == 'c' || ItemType == 'C') && reqType == 's')
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

                pReqReturn = new string(chars.ToArray());
            }

            switch (ItemType)
            {
                case 'c':
                case 'C':
                    nIntRet = data[dataOffset + nIndexValue];
                    dfDoubleRet = nIntRet;
                    break;
                case 'e':
                case 's':
                    {
                        int nNumber = Hfa.ReadUInt16(data, dataOffset + (nIndexValue * 2));
                        nIntRet = nNumber;
                        dfDoubleRet = nIntRet;
                        if (ItemType == 'e' && nIntRet >= 0 && nIntRet < EnumNames.Count)
                        {
                            returnString = EnumNames[nIntRet];
                        }
                    }

                    break;
                case 'S':
                    {
                        short nNumber = Hfa.ReadInt16(data, dataOffset + (nIndexValue * 2));
                        nIntRet = nNumber;
                        dfDoubleRet = nNumber;
                    }

                    break;
                case 't':
                case 'l':
                    {
                        long nNumber = Hfa.ReadUInt32(data, dataOffset + (nIndexValue * 2));
                        nIntRet = (int)nNumber;
                        dfDoubleRet = nNumber;
                    }

                    break;
                case 'L':
                    {
                        int nNumber = Hfa.ReadInt32(data, dataOffset + (nIndexValue * 2));
                        nIntRet = nNumber;
                        dfDoubleRet = nNumber;
                    }

                    break;
                case 'f':
                    {
                        float fNumber = Hfa.ReadSingle(data, dataOffset + (nIndexValue * 4));
                        dfDoubleRet = fNumber;
                        nIntRet = Convert.ToInt32(fNumber);
                    }

                    break;
                case 'd':
                    {
                        dfDoubleRet = Hfa.ReadDouble(data, dataOffset + (nInstItemCount * 8));
                        nIntRet = Convert.ToInt32(dfDoubleRet);
                    }

                    break;
                case 'b':
                    {
                        // BASE DATA
                        int nRows = Hfa.ReadInt32(data, dataOffset);
                        int nColumns = Hfa.ReadInt32(data, dataOffset + 4);
                        if (nIndexValue < 0 || nIndexValue >= nRows * nColumns) return false;

                        HfaEpt type = (HfaEpt)Hfa.ReadUInt16(data, dataOffset + 8);

                        // Ignore the 2 byte objecttype value
                        dataOffset += 12;
                        if (nRows < 0 || nColumns < 0) throw new HfaInvalidCountException(nRows, nColumns);

                        switch (type)
                        {
                            case HfaEpt.U8:
                                dfDoubleRet = data[dataOffset + nIndexValue];
                                nIntRet = data[offset + nIndexValue];
                                break;
                            case HfaEpt.S16:
                                short tShort = Hfa.ReadInt16(data, dataOffset + (nIndexValue * 2));
                                dfDoubleRet = tShort;
                                nIntRet = tShort;
                                break;
                            case HfaEpt.U16:
                                int tUShort = Hfa.ReadUInt16(data, dataOffset + (nIndexValue * 2));
                                dfDoubleRet = tUShort;
                                nIntRet = tUShort;
                                break;
                            case HfaEpt.Single:
                                float tSingle = Hfa.ReadSingle(data, dataOffset + (nIndexValue * 4));
                                dfDoubleRet = tSingle;
                                nIntRet = Convert.ToInt32(tSingle);
                                break;
                            case HfaEpt.Double:
                                dfDoubleRet = Hfa.ReadDouble(data, dataOffset + (nIndexValue * 8));
                                nIntRet = Convert.ToInt32(dfDoubleRet);
                                break;
                            default:
                                pReqReturn = null;
                                return false;
                        }
                    }

                    break;
                case 'o':
                    if (ItemObjectType != null)
                    {
                        if (ItemObjectType.NumBytes > 0)
                        {
                            extraOffset = ItemObjectType.NumBytes * nIndexValue;
                        }
                        else
                        {
                            for (int iIndexCounter = 0; iIndexCounter < nIndexValue; iIndexCounter++)
                            {
                                extraOffset += ItemObjectType.GetInstBytes(data, dataOffset + extraOffset);
                            }
                        }

                        int len = ItemObjectType.GetInstBytes(data, dataOffset + extraOffset);
                        rawData = new byte[len];
                        Array.Copy(data, dataOffset + extraOffset, rawData, 0, len);
                        if (!string.IsNullOrEmpty(pszField))
                        {
                            return ItemObjectType.ExtractInstValue(pszField, rawData, 0, rawData.Length, reqType, out pReqReturn);
                        }
                    }

                    break;
                default: return false;
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
                default: return false;
            }
        }

        /// <summary>
        /// Scans through the array and estimates the byte size of the field in this case.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="dataOffset">The data offset.</param>
        /// <returns>The byte size of the field.</returns>
        public int GetInstBytes(byte[] data, long dataOffset)
        {
            int nCount;
            int nInstBytes = 0;
            long offset = dataOffset;

            // Hard sized fields just return their constant size
            if (NumBytes > -1) return NumBytes;

            // Pointers have a 4 byte integer count and a 4 byte uint offset
            if (Pointer != '\0')
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

            if (ItemType == 'b' && nCount != 0)
            {
                // BASEDATA
                int nRows = Hfa.ReadInt32(data, offset);
                offset += 4;
                int nColumns = Hfa.ReadInt32(data, offset);
                offset += 4;
                HfaEpt baseItemType = (HfaEpt)Hfa.ReadInt16(data, offset);
                nInstBytes += 12;
                nInstBytes += ((baseItemType.GetBitCount() + 7) / 8) * nRows * nColumns;
            }
            else if (ItemObjectType == null)
            {
                nInstBytes += nCount * HfaDictionary.GetItemSize(ItemType);
            }
            else
            {
                for (int i = 0; i < nCount; i++)
                {
                    nInstBytes += ItemObjectType.GetInstBytes(data, offset + nInstBytes);
                }
            }

            return nInstBytes;
        }

        /// <summary>
        /// Gets the count for a particular instance of a field. This will normally be
        /// the built in value, but for variable fields, this is extracted from the
        /// data itself.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="dataOffset">The data offset.</param>
        /// <returns>The count for a particular instance of a field.</returns>
        public int GetInstCount(byte[] data, long dataOffset)
        {
            if (Pointer == '\0')
            {
                return ItemCount;
            }

            if (ItemType == 'b')
            {
                int numRows = Hfa.ReadInt32(data, dataOffset + 8);
                int numColumns = Hfa.ReadInt32(data, dataOffset + 12);
                return numRows * numColumns;
            }

            return Hfa.ReadInt32(data, dataOffset);
        }

        /// <summary>
        /// Parses the input string into a valid HfaField, or returns null
        /// if one could not be created.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The parsed string, or null if the field could not be created.</returns>
        public string Initialize(string input)
        {
            int length = input.Length;
            int start = 0;

            // Read the number
            if (!input.Contains(":")) return null;

            ItemCount = short.Parse(input.ExtractTo(ref start, ":"));

            // is this a pointer?
            if (input[start] == 'p' || input[start] == '*')
            {
                start += 1;
                Pointer = input[start];
            }

            // Get the general type
            start += 1;
            if (start == length) return null;

            ItemType = input[start];
            if (!"124cCesStlLfdmMbox".Contains(ItemType.ToString()))
            {
                throw new HfaFieldTypeException(ItemType);
            }

            // If this is an object, we extract the type of the object
            if (ItemType == 'o')
            {
                ItemObjectTypeString = input.ExtractTo(ref start, ",");
            }

            // If this is an inline object, we need to skip past the
            // definition, and then extract the object class name.
            // we ignore the actual definition, so if the object type isn't
            // already defined, things will not work properly. See the
            // file lceugr250)00)pct.aus for an example of inline defs.
            if (ItemType == 'x' && input[start] == '{')
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

                ItemType = 'o';
                ItemObjectTypeString = input.ExtractTo(ref start, ",");
            }

            // If this is an enumeration, we have to extract all the values.
            if (ItemType == 'e')
            {
                if (!input.Contains(":")) return null;

                int enumCount = int.Parse(input.ExtractTo(ref start, ":"));
                if (EnumNames == null) EnumNames = new List<string>();
                for (int ienum = 0; ienum < enumCount; ienum++)
                {
                    EnumNames.Add(input.ExtractTo(ref start, ","));
                }
            }

            // Extract the field name
            FieldName = input.ExtractTo(ref start, ",");

            // Return whatever is left in the string, which should be a new field.
            return input.Substring(start, input.Length - start);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="sField">The field.</param>
        /// <param name="indexValue">The index value.</param>
        /// <param name="data">The data.</param>
        /// <param name="dataOffset">The data offset.</param>
        /// <param name="dataSize">The data size.</param>
        /// <param name="reqType">The req type.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="HfaPointerInsertNotSupportedException">Attempting to insert a pointer is not supported.</exception>
        /// <exception cref="HfaEnumerationNotFoundException">Occurs if the specified value is not a valid member of the enumeration for this field.</exception>
        public void SetInstValue(string sField, int indexValue, byte[] data, long dataOffset, int dataSize, char reqType, object value)
        {
            // If this field contains a pointer, then we wil adjust the data offset relative to it.
            // This updates the offset info, but doesn't change the first four bytes.
            if (Pointer != '\0')
            {
                int nCount;
                if (NumBytes > -1)
                {
                    nCount = ItemCount;
                }
                else if (reqType == 's' && (ItemType == 'c' || ItemType == 'C'))
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
            if ((ItemType == 'c' || ItemType == 'C') && reqType == 's')
            {
                int nBytesToCopy = NumBytes;
                var strVal = (value as IEnumerable<char>)?.ToArray();
                if (strVal != null) nBytesToCopy = strVal.Length;
                if (NumBytes == -1 && strVal != null) nBytesToCopy = strVal.Length;

                // Force a blank erase to remove previous characters
                byte[] blank = new byte[nBytesToCopy];
                Array.Copy(blank, 0, data, dataOffset, nBytesToCopy);
                if (strVal != null)
                {
                    ASCIIEncoding ascii = new ASCIIEncoding();
                    string str = new string(strVal);
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
            switch (ItemType)
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
                case 's':
                    // little s  = ushort type
                    if (ItemType == 'e' && reqType == 's')
                    {
                        nIntValue = EnumNames.IndexOf((string)value);
                        if (nIntValue == -1)
                        {
                            throw new HfaEnumerationNotFoundException((string)value);
                        }
                    }

                    // Each enumeration is stored as a 2-bit unsigned short entry.
                    ushort num = (ushort)nIntValue;
                    Array.Copy(Hfa.LittleEndian(num), 0, data, dataOffset + (2 * indexValue), 2);
                    break;
                case 'S':
                    // signed short
                    Array.Copy(Hfa.LittleEndian((short)nIntValue), 0, data, dataOffset + (indexValue * 2), 2);
                    break;
                case 't':
                case 'l':
                    Array.Copy(Hfa.LittleEndian((uint)nIntValue), 0, data, dataOffset + (indexValue * 4), 4);
                    break;
                case 'L':
                    // Int32
                    Array.Copy(Hfa.LittleEndian(nIntValue), 0, data, dataOffset + (indexValue * 4), 4);
                    break;
                case 'f':
                    // Float (32 bit)
                    float dfNumber = Convert.ToSingle(dfDoubleValue);
                    Array.Copy(Hfa.LittleEndian(dfNumber), 0, data, dataOffset + (indexValue * 4), 4);
                    break;
                case 'd':
                    // Double (float 64)
                    Array.Copy(Hfa.LittleEndian(dfDoubleValue), 0, data, dataOffset + (8 * indexValue), 8);
                    break;
                case 'o':
                    // object
                    if (ItemObjectType == null) break;

                    int nExtraOffset = 0;

                    if (ItemObjectType.NumBytes > 0)
                    {
                        nExtraOffset = ItemObjectType.NumBytes * indexValue;
                    }
                    else
                    {
                        for (int iIndexCounter = 0; iIndexCounter < indexValue; iIndexCounter++)
                        {
                            nExtraOffset += ItemObjectType.GetInstBytes(data, dataOffset + nExtraOffset);
                        }
                    }

                    if (!string.IsNullOrEmpty(sField))
                    {
                        ItemObjectType.SetInstValue(sField, data, dataOffset + nExtraOffset, dataSize - nExtraOffset, reqType, value);
                    }

                    break;
                default: throw new ArgumentException();
            }
        }

        #endregion
    }
}