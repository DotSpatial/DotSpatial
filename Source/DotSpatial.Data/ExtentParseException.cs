// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/14/2009 3:58:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Runtime.Serialization;

namespace DotSpatial.Data
{
    public class ExtentParseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ExtentParseException
        /// </summary>
        /// <param name="message">The string message to parse.</param>
        public ExtentParseException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ExtentParseException
        /// </summary>
        /// <param name="message">The string message to parse.</param>
        /// <param name="innerException">The inner exception that caused this exception.</param>
        public ExtentParseException(string message, Exception innerException)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ExtentParseException
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
        /// Gets the string expression that provoked the exception.
        /// </summary>
        public string Expression { get; set; }
    }
}