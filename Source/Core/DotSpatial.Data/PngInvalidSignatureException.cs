// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// PngInvalidSignatureException.
    /// </summary>
    public class PngInvalidSignatureException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PngInvalidSignatureException"/> class.
        /// </summary>
        public PngInvalidSignatureException()
            : base(DataStrings.PngInvalidSignatureException)
        {
        }

        #endregion
    }
}