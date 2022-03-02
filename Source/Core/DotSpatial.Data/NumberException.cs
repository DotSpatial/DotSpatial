// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An exception that is specifically fo the NumberConverter class.
    /// </summary>
    public class NumberException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberException"/> class.
        /// </summary>
        /// <param name="message">The message for the exception.</param>
        public NumberException(string message)
            : base(message)
        {
        }
    }
}