// ********************************************************************************************************
// Product Name: DotSpatial.Serialization.dll
// Description:  A module that supports common functions like serialization.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/26/2010 6:56:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|---------|---------------------------------------------------------------------
// |      Name       |  Date   |                        Comments
// |-----------------|---------|---------------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Runtime.Serialization;

namespace DotSpatial.Serialization
{
    /// <summary>
    /// This exception will occur if an assembly referenced by a saved map layer could not be found.
    /// </summary>
    [Obsolete("Do not use it. This class is not used in DotSpatial anymore.")] // Marked in 1.7
    public class AssemblyMissingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the AssemblyMissingException.
        /// </summary>
        /// <param name="message">The string message to show as the error.</param>
        public AssemblyMissingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AssemblyMissingException.
        /// </summary>
        /// <param name="message">The string message to show as the error.</param>
        /// <param name="innerException">The inner exception to send as part of this exception.</param>
        public AssemblyMissingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the AssemblyMissingException.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the information about the serialized object about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public AssemblyMissingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}