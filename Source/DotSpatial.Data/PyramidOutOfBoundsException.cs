// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// The exception that is thrown when range specified is outside the bounds for the specified image scale.
    /// </summary>
    public class PyramidOutOfBoundsException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PyramidOutOfBoundsException"/> class.
        /// </summary>
        public PyramidOutOfBoundsException()
            : base(DataStrings.PyramidOutOfBoundsException)
        {
        }

        #endregion
    }
}