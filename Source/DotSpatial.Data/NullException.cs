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
    /// NullExceptioncs
    /// </summary>
    public class NullException : ArgumentNullException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of the NullLogException, but does not set the message
        /// or log the exception.
        /// </summary>
        public NullException()
        {
        }

        /// <summary>
        /// Creates a new instance of NullException
        /// </summary>
        public NullException(string parameterName)
            : base(DataStrings.Argument_Null_S.Replace("%S", parameterName))
        {
        }

        #endregion
    }
}