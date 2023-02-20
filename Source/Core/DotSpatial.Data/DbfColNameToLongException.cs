// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    public class DbfColNameToLongException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbfColNameToLongException"/> class.
        /// </summary>
        /// <param name="message">The string message to parse.</param>
        public DbfColNameToLongException(string message)
            : base(message)
        {
        }
    }
}
