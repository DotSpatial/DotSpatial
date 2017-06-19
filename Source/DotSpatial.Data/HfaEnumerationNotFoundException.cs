// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaEnumerationNotFoundException
    /// </summary>
    public class HfaEnumerationNotFoundException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaEnumerationNotFoundException"/> class.
        /// </summary>
        /// <param name="name">The unknown value.</param>
        public HfaEnumerationNotFoundException(string name)
            : base(string.Format(DataStrings.HfaEnumerationNotFound, name))
        {
        }

        #endregion
    }
}