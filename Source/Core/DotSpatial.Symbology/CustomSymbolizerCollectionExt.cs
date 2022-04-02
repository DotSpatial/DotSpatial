// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Extension methods for a collection of custom symbolizers.
    /// </summary>
    public static class CustomSymbolizerCollectionExt
    {
        #region Methods

        /// <summary>
        /// Saves a list of custom line symbolizers to a file.
        /// </summary>
        /// <param name="self">The list of custom line symbolizers.</param>
        /// <param name="fileName">the file name.</param>
        public static void Save(this IEnumerable<CustomLineSymbolizer> self, string fileName)
        {
            using var myStream = File.Open(fileName, FileMode.Create);
            var bformatter = new BinaryFormatter();
            bformatter.Serialize(myStream, self);
        }

        #endregion
    }
}