// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaPointerInsertNotSupportedException
    /// </summary>
    public class HfaPointerInsertNotSupportedException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaPointerInsertNotSupportedException"/> class.
        /// </summary>
        public HfaPointerInsertNotSupportedException()
            : base(DataStrings.HfaPointerInsertNotSupportedException)
        {
        }

        #endregion
    }
}