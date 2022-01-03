// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace DotSpatial.Plugins.SpatiaLite
{
    /// <summary>
    /// This exception occurs in case of invalid database schema.
    /// </summary>
    public class InvalidDatabaseSchemaException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDatabaseSchemaException"/> class.
        /// </summary>
        public InvalidDatabaseSchemaException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDatabaseSchemaException"/> class with a message.
        /// </summary>
        /// <param name="message">the error message.</param>
        public InvalidDatabaseSchemaException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDatabaseSchemaException"/> class with a message and an inner exception.
        /// </summary>
        /// <param name="message">the error messsage.</param>
        /// <param name="inner">the inner exception.</param>
        public InvalidDatabaseSchemaException(string message, Exception inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDatabaseSchemaException"/> class. with serialization info and streaming context.
        /// </summary>
        /// <param name="info">serialization info.</param>
        /// <param name="context">streaming context.</param>
        protected InvalidDatabaseSchemaException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}