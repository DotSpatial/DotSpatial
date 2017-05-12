// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2008 4:41:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// An exception that gets thrown for elements that may not be null.
    /// </summary>
    public class NullException : ArgumentNullException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NullException"/> class.
        /// </summary>
        public NullException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NullException"/> class.
        /// </summary>
        /// <param name="parameterName">Name of the parameter that is null.</param>
        public NullException(string parameterName)
            : base(string.Format(DataStrings.Argument_Null_S, parameterName))
        {
        }

        #endregion
    }
}