// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Serialization.Properties;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// ReadOnlyException.
    /// </summary>
    public class ReadOnlyException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyException"/> class.
        /// </summary>
        public ReadOnlyException()
            : base(Resources.ReadOnly)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyException"/> class.
        /// </summary>
        /// <param name="message">Message, the ReadOnlyException should show.</param>
        public ReadOnlyException(string message)
            : base(message)
        {
        }

        #endregion
    }
}