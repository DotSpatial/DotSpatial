// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// The exception that is thrown when attempting to write data before the headers are defined.
    /// </summary>
    public class PyramidUndefinedHeaderException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PyramidUndefinedHeaderException"/> class.
        /// </summary>
        public PyramidUndefinedHeaderException()
            : base(DataStrings.PyramidHeaderException)
        {
        }

        #endregion
    }
}