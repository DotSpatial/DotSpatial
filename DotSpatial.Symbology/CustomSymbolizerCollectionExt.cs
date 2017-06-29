// ********************************************************************************************************
// Product Name: DotSpatial.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for DotSpatial.Drawing version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.Drawing.PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/27/2009 12:33:20 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Extension methods for a collection of custom symbolizers
    /// </summary>
    public static class CustomSymbolizerCollectionExt
    {
        #region Methods

        /// <summary>
        /// Saves a list of custom line symbolizers to a file.
        /// </summary>
        /// <param name="self">The list of custom line symbolizers</param>
        /// <param name="fileName">the file name</param>
        public static void Save(this IEnumerable<CustomLineSymbolizer> self, string fileName)
        {
            using (var myStream = File.Open(fileName, FileMode.Create))
            {
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(myStream, self);
            }
        }
       

        #endregion
    }
}