﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace DotSpatial.Data
{
    /// <summary>
    /// ExtentParseException
    /// </summary>
    public class ExtentParseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentParseException"/> class.
        /// </summary>
        /// <param name="message">The string message to parse.</param>
        public ExtentParseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentParseException"/> class.
        /// </summary>
        /// <param name="message">The string message to parse.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public ExtentParseException(string message, Exception innerException)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentParseException"/> class.
        /// </summary>
        /// <param name="info">The Serialization info class that holds the serialized object about which
        /// the exception is being thrown.</param>
        /// <param name="context">The streaming context that contains contextual information about the
        /// source or destination.</param>
        public ExtentParseException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets or sets the string expression that provoked the exception.
        /// </summary>
        public string Expression { get; set; }
    }
}