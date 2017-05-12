// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/7/2008 10:54:31 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An exception that is specifically fo the NumberConverter class
    /// </summary>
    public class NumberException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberException"/> class.
        /// </summary>
        /// <param name="message">The message for the exception</param>
        public NumberException(string message)
            : base(message)
        {
        }
    }
}