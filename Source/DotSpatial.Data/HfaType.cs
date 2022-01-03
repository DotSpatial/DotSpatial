// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

// ORIGINAL HEADER FROM C++ source which was converted to C# by Ted Dunsford 2/24/2010
/******************************************************************************
 * $Id: hfatype.cpp, v 1.11 2006/05/07 04:04:03 fwarmerdam Exp $
 *
 * Project:  Erdas Imagine (.img) Translator
 * Purpose:  Implementation of the HFAType class, for managing one type
 *           defined in the HFA data dictionary. Managed by HFADictionary.
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
 * $Log: hfatype.cpp, v $
 * Revision 1.11  2006/05/07 04:04:03  fwarmerdam
 * fixed serious multithreading issue with ExtractInstValue (bug 1132)
 *
 * Revision 1.10  2005/05/13 02:45:16  fwarmerdam
 * fixed GetInstCount() error return
 *
 * Revision 1.9  2005/05/10 00:55:30  fwarmerdam
 * Added GetInstCount method
 *
 * Revision 1.8  2004/01/26 18:28:51  warmerda
 * added error recover after corrupt/unrecognised entries - bug 411
 *
 * Revision 1.7  2003/04/22 19:40:36  warmerda
 * fixed email address
 *
 * Revision 1.6  2003/02/21 15:40:58  dron
 * Added support for writing large (>4 GB) Erdas Imagine files.
 *
 * Revision 1.5  2001/07/18 04:51:57  warmerda
 * added CPL_CVSID
 *
 * Revision 1.4  2000/12/29 16:37:32  warmerda
 * Use GUInt32 for all file offsets
 *
 * Revision 1.3  2000/09/29 21:42:38  warmerda
 * preliminary write support implemented
 *
 * Revision 1.2  1999/01/22 17:36:47  warmerda
 * Added GetInstBytes(), track unknown sizes properly
 *
 * Revision 1.1  1999/01/04 22:52:10  warmerda
 * New
 *
 */

using System.Collections.Generic;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaType.
    /// </summary>
    public class HfaType
    {
        #region Properties

        /// <summary>
        /// Gets or sets the number of fields.
        /// </summary>
        public int FieldCount { get; set; }

        /// <summary>
        /// Gets or sets the list of fields.
        /// </summary>
        public List<HfaField> Fields { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes.
        /// </summary>
        public int NumBytes { get; set; }

        /// <summary>
        /// Gets or sets the type name.
        /// </summary>
        public string TypeName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Completes the defenition of this type based on the existing dictionary.
        /// </summary>
        /// <param name="dictionary">Dictionary used for completion.</param>
        public void CompleteDefn(HfaDictionary dictionary)
        {
            // This may already be done, if an earlier object required this
            // object (as a field), and forced and early computation of the size
            if (NumBytes != 0) return;

            // Complete each fo the fields, totaling up the sizes. This
            // isn't really accurate for objects with variable sized
            // subobjects.
            foreach (HfaField field in Fields)
            {
                field.CompleteDefn(dictionary);
                if (field.NumBytes < 0 || NumBytes == -1)
                {
                    NumBytes = -1;
                }
                else
                {
                    NumBytes += field.NumBytes;
                }
            }
        }

        /// <summary>
        /// This function writes content to the file for this entire type by writing
        /// the type name and number of bytes, followed by cycling through and writing each
        /// of the fields.
        /// </summary>
        /// <param name="stream">Stream to write to.</param>
        public void Dump(Stream stream)
        {
            StreamWriter sw = new StreamWriter(stream);
            sw.Write("HFAType " + TypeName + "/" + NumBytes + "\n");
            foreach (HfaField field in Fields)
            {
                field.Dump(stream);
            }
        }

        /// <summary>
        /// Triggers a dump on all the fields of this type.
        /// </summary>
        /// <param name="fpOut">The output stream.</param>
        /// <param name="data">The data.</param>
        /// <param name="dataOffset">The offset to start from.</param>
        /// <param name="dataSize">The data size.</param>
        /// <param name="pszPrefix">The prefix.</param>
        public void DumpInstValue(Stream fpOut, byte[] data, long dataOffset, int dataSize, string pszPrefix)
        {
            foreach (HfaField field in Fields)
            {
                field.DumpInstValue(fpOut, data, dataOffset, dataSize, pszPrefix);
                int nInstBytes = field.GetInstBytes(data, dataOffset);
                dataOffset += nInstBytes;
                dataSize -= nInstBytes;
            }
        }

        /// <summary>
        /// Extracts the value form the byte array.
        /// </summary>
        /// <param name="fieldPath">The field path.</param>
        /// <param name="data">The data.</param>
        /// <param name="dataOffset">The offset to start from.</param>
        /// <param name="dataSize">The data size.</param>
        /// <param name="reqType">The req type.</param>
        /// <param name="reqReturn">The req return.</param>
        /// <returns>True, if the value could be extracted.</returns>
        public bool ExtractInstValue(string fieldPath, byte[] data, long dataOffset, int dataSize, char reqType, out object reqReturn)
        {
            reqReturn = null;
            int arrayIndex, byteOffset, extraOffset;
            string remainder;
            HfaField field = ParseFieldPath(fieldPath, data, dataOffset, out remainder, out arrayIndex, out byteOffset);
            return field != null && field.ExtractInstValue(remainder, arrayIndex, data, dataOffset + byteOffset, dataSize - byteOffset, reqType, out reqReturn, out extraOffset);
        }

        /// <summary>
        /// Gets the number of bytes for this type by adding up the byte contribution from each of its fields.
        /// </summary>
        /// <param name="data">The array of bytes to scan.</param>
        /// <param name="dataOffset">The integer index in the array where scanning should begin.</param>
        /// <returns>The integer count.</returns>
        public int GetInstBytes(byte[] data, long dataOffset)
        {
            if (NumBytes >= 0)
            {
                return NumBytes;
            }

            int nTotal = 0;
            foreach (HfaField field in Fields)
            {
                nTotal += field.GetInstBytes(data, dataOffset + nTotal);
            }

            return nTotal;
        }

        /// <summary>
        /// Attempts to find the specified field in the field path and extracts the count of the specified field.
        /// </summary>
        /// <param name="fieldPath">The field path.</param>
        /// <param name="data">The data.</param>
        /// <param name="dataOffset">The data offset.</param>
        /// <param name="dataSize">The data size.</param>
        /// <returns>The count for a particular instance of a field.</returns>
        public int GetInstCount(string fieldPath, byte[] data, long dataOffset, int dataSize)
        {
            int arrayIndex, byteOffset;
            string remainder;
            HfaField field = ParseFieldPath(fieldPath, data, dataOffset, out remainder, out arrayIndex, out byteOffset);
            if (field != null) return field.GetInstCount(data, dataOffset + byteOffset);

            return -1;
        }

        /// <summary>
        /// Originally Initialize.
        /// </summary>
        /// <param name="input">The input string that contains content for this type.</param>
        /// <returns>The remaining string content, unless this fails in which case this may return null.</returns>
        public string Intialize(string input)
        {
            if (!input.Contains("{")) return null;

            string partialInput = input.SkipTo("{");
            while (partialInput != null && partialInput[0] != '}')
            {
                HfaField fld = new HfaField();

                // If the initialize fails, the return string is null.
                partialInput = fld.Initialize(partialInput);
                if (partialInput == null) continue;

                if (Fields == null) Fields = new List<HfaField>();
                Fields.Add(fld);
                FieldCount++;
            }

            // If we have run out of content, we can't complete the type.
            if (partialInput == null) return null;

            // Get the name
            int start = 0;
            TypeName = partialInput.ExtractTo(ref start, ",");
            return partialInput.Substring(start, partialInput.Length - start);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="fieldPath">The field path.</param>
        /// <param name="data">The data.</param>
        /// <param name="dataOffset">The data offset.</param>
        /// <param name="dataSize">The data size.</param>
        /// <param name="reqType">The req type.</param>
        /// <param name="value">The value.</param>
        public void SetInstValue(string fieldPath, byte[] data, long dataOffset, int dataSize, char reqType, object value)
        {
            int arrayIndex, byteOffset;
            string remainder;
            HfaField field = ParseFieldPath(fieldPath, data, dataOffset, out remainder, out arrayIndex, out byteOffset);
            field.SetInstValue(remainder, arrayIndex, data, dataOffset + byteOffset, dataSize - byteOffset, reqType, value);
        }

        private HfaField ParseFieldPath(string fieldPath, byte[] data, long dataOffset, out string remainder, out int arrayIndex, out int byteOffset)
        {
            arrayIndex = 0;
            string name;
            remainder = null;

            if (fieldPath.Contains("["))
            {
                // In the array case we have the format: name[2].subname
                // so only read up to the [ for the name but we also need to read the integer after that bracket.
                string arrayVal;
                name = fieldPath.ExtractTo("[", out arrayVal);
                arrayIndex = arrayVal.ExtractInteger();

                // Finally, we still need the subname after the period, but we can ignore the return value and just use the remainder.
                fieldPath.ExtractTo(".", out remainder);
            }
            else if (fieldPath.Contains("."))
            {
                // This is separating name.subname into two separate parts, even if there are many further sub-divisions of the name.
                name = fieldPath.ExtractTo(".", out remainder);
            }
            else
            {
                name = fieldPath;
            }

            // Find the field within this type, if possible
            byteOffset = 0;
            foreach (HfaField field in Fields)
            {
                if (field.FieldName == name)
                {
                    return field;
                }

                byteOffset += field.GetInstBytes(data, dataOffset + byteOffset);
            }

            return null;
        }

        #endregion
    }
}