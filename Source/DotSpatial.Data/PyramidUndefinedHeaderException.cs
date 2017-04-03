// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/11/2010 11:34:18 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// The exception that is thrown when attempting to write data before the headers are defined.
    /// </summary>
    public class PyramidUndefinedHeaderException : Exception
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="PyramidUndefinedHeaderException"/>.
        /// </summary>
        public PyramidUndefinedHeaderException()
            : base(DataStrings.PyramidHeaderException)
        {
        }

        #endregion
    }
}