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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2010 1:43:16 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

//ORIGINAL HEADER FROM C++ source which was converted to C# by Ted Dunsford 2/25/2010
/******************************************************************************
 * $Id: hfadictionary.cpp, v 1.8 2005/09/27 18:01:37 fwarmerdam Exp $
 *
 * Project:  Erdas Imagine (.img) Translator
 * Purpose:  Implementation of the HFADictionary class for managing the
 *           dictionary read from the HFA file.  Most work done by the
 *           HFAType, and HFAField classes.
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
 * $Log: hfadictionary.cpp, v $
 * Revision 1.8  2005/09/27 18:01:37  fwarmerdam
 * Ensure Edsc_Table is defined, fixed default def support
 *
 * Revision 1.7  2004/01/26 18:28:32  warmerda
 * provide default types if not defined in file
 *
 * Revision 1.6  2003/04/22 19:40:36  warmerda
 * fixed email address
 *
 * Revision 1.5  2003/02/25 18:03:47  warmerda
 * added support to auto-define Edsc_Column as the defn is sometimes missing
 *
 * Revision 1.4  2001/07/18 04:51:57  warmerda
 * added CPL_CVSID
 *
 * Revision 1.3  1999/01/22 17:36:13  warmerda
 * fixed return value
 *
 * Revision 1.2  1999/01/04 22:52:47  warmerda
 * field access working
 *
 * Revision 1.1  1999/01/04 05:28:12  warmerda
 * New
 *
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaDictionary
    /// </summary>
    public class HfaDictionary : Dictionary<string, HfaType>
    {
        #region Private Variables

        static readonly string[] DefDefn =
            {
                "Edsc_Table",
                "{1:lnumrows, }Edsc_Table",

                "Edsc_Column",
                "{1:lnumRows, 1:LcolumnDataPtr, 1:e4:integer, real, complex, string, dataType, 1:lmaxNumChars, }Edsc_Column",

                "Eprj_Size",
                "{1:dwidth, 1:dheight, }Eprj_Size",

                "Eprj_Coordinate",
                "{1:dx, 1:dy, }Eprj_Coordinate",

                "Eprj_MapInfo",
                "{0:pcproName, 1:*oEprj_Coordinate, upperLeftCenter, 1:*oEprj_Coordinate, lowerRightCenter, 1:*oEprj_Size, pixelSize, 0:pcunits, }Eprj_MapInfo",

                "Eimg_StatisticsParameters830",
                "{0:poEmif_String, LayerNames, 1:*bExcludedValues, 1:oEmif_String, AOIname, 1:lSkipFactorX, 1:lSkipFactorY, 1:*oEdsc_BinFunction, BinFunction, }Eimg_StatisticsParameters830",

                "Esta_Statistics",
                "{1:dminimum, 1:dmaximum, 1:dmean, 1:dmedian, 1:dmode, 1:dstddev, }Esta_Statistics",

                "Edsc_BinFunction",
                "{1:lnumBins, 1:e4:direct, linear, logarithmic, explicit, binFunctionType, 1:dminLimit, 1:dmaxLimit, 1:*bbinLimits, }Edsc_BinFunction",

                "Eimg_NonInitializedValue",
                "{1:*bvalueBD, }Eimg_NonInitializedValue",
            };

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of HfaDictionary
        /// </summary>
        public HfaDictionary(string content)
        {
            int end = 0;

            // Read the data up to the period
            string substring = content.ExtractTo(ref end, ".");

            // Extract all the types
            while (!string.IsNullOrEmpty(substring))
            {
                HfaType type = new HfaType();
                // A null return means the effort failed.
                substring = type.Intialize(substring);
                if (substring != null)
                {
                    AddType(type);
                }
            }

            // provide hardcoded values for some definitions that are sometimes missing from
            // the data dictionary
            for (int i = 0; i < DefDefn.Length - 1; i += 2)
            {
                if (!ContainsKey(DefDefn[i]))
                {
                    HfaType type = new HfaType();
                    type.Intialize(DefDefn[i + 1]);
                    AddType(type);
                }
            }

            // Complete the definitions
            foreach (KeyValuePair<string, HfaType> kvp in this)
            {
                kvp.Value.CompleteDefn(this);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the type using the TypeName as the key
        /// </summary>
        /// <param name="type">The type to add</param>
        public void AddType(HfaType type)
        {
            Add(type.TypeName, type);
        }

        /// <summary>
        /// Writes all the elements from this dictionary to the specified stream.
        /// </summary>
        /// <param name="fp"></param>
        public void Dump(Stream fp)
        {
            StreamWriter sw = new StreamWriter(fp);
            sw.Write("\nHFADictionary:\n");
            foreach (KeyValuePair<string, HfaType> pair in this)
            {
                pair.Value.Dump(fp);
            }
        }

        /// <summary>
        /// Given a character type, this calculates a size.  This was originally
        /// on the Dictionary, but I moved this to the HfaInfo instead.
        /// </summary>
        /// <param name="charType"></param>
        /// <returns></returns>
        public static int GetItemSize(char charType)
        {
            switch (charType)
            {
                case '1':
                case '2':
                case '4':
                case 'c':
                case 'C':
                    return 1;
                case 'e':
                case 's':
                case 'S':
                    return 2;
                case 't':
                case 'l':
                case 'L':
                case 'f':
                    return 4;
                case 'd':
                case 'm':
                    return 8;
                case 'M':
                    return 16;
                case 'b':
                    return -1;
                case 'o':
                case 'x':
                    return 0;

                default:
                    Debug.WriteLine("Could not GetItemSize for character: '" + charType + "'.");
                    break;
            }
            return 0;
        }

        #endregion

        #region Properties

        #endregion
    }
}