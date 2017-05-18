// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaFieldTypeException
    /// </summary>
    public class HfaFieldTypeException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaFieldTypeException"/> class.
        /// </summary>
        /// <param name="code">The unknown character.</param>
        public HfaFieldTypeException(char code)
            : base(string.Format(DataStrings.HfaFieldTypeException, code))
        {
        }

        #endregion
    }
}