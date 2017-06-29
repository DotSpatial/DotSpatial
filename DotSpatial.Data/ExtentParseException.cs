// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
    /// <summary>
    ///
    /// </summary>
    public class ExtentParseException : Exception
    {
        /// <summary>
        /// The string template that should be used.  M and Z are optional.  Do use a comma to
        /// delimit terms, but the space doesn't matter.  Do use square brackets to enclose
        /// numbers, and do use the | separator to separate min from max.  Do place min
        /// before max in each term.  The order of the terms doesn't matter.  Use periods
        /// for decimals and an invariant culture.
        /// </summary>
        public const string EXTENT_TEXT_TEMPLATE = "X[-180|180], Y[-90|90], M[0|1], Z[0|1]";

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