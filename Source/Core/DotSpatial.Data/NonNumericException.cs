// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// NonNumericException.
    /// </summary>
    public class NonNumericException : InvalidOperationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NonNumericException"/> class.
        /// </summary>
        /// <param name="invalidVariable">The string name of the variable, or value that cannot be parsed as a number.</param>
        public NonNumericException(string invalidVariable)
            : base("The value " + invalidVariable + " could not be parsed as a number.")
        {
        }

        #endregion
    }
}