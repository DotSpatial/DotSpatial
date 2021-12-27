// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaInvalidCountException.
    /// </summary>
    public class HfaInvalidCountException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaInvalidCountException"/> class.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of columns.</param>
        public HfaInvalidCountException(int rows, int cols)
            : base(string.Format(DataStrings.HfaInvalidCountException, rows, cols))
        {
        }

        #endregion
    }
}