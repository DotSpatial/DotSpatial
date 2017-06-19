// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// PngInsuficientLengthException
    /// </summary>
    public class PngInsuficientLengthException : ArgumentException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PngInsuficientLengthException"/> class.
        /// </summary>
        /// <param name="length">The desired length.</param>
        /// <param name="totalLength">The total length.</param>
        /// <param name="offset">The offset.</param>
        public PngInsuficientLengthException(int length, int totalLength, int offset)
            : base(string.Format(DataStrings.PngInsuficientLengthException, length, totalLength, offset))
        {
        }

        #endregion
    }
}