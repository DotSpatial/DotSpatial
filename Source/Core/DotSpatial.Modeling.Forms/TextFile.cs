// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// This class is used to enable linking of tools that work with File parameters.
    /// </summary>
    public class TextFile : DataSet
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFile"/> class.
        /// </summary>
        public TextFile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextFile"/> class.
        /// </summary>
        /// <param name="fileName">the associated file name.</param>
        public TextFile(string fileName)
        {
            Filename = fileName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the file name.
        /// </summary>
        /// <returns>The file name.</returns>
        public override string ToString()
        {
            return Filename;
        }

        #endregion
    }
}