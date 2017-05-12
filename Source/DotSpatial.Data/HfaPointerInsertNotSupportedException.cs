// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2010 10:18:43 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaPointerInsertNotSupportedException
    /// </summary>
    public class HfaPointerInsertNotSupportedException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HfaPointerInsertNotSupportedException"/> class.
        /// </summary>
        public HfaPointerInsertNotSupportedException()
            : base(DataStrings.HfaPointerInsertNotSupportedException)
        {
        }

        #endregion
    }
}