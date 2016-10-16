// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/28/2010 2:59:45 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaNotCompressedException
    /// </summary>
    public class HfaNotCompressedException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of HfaNotCompressedException
        /// </summary>
        public HfaNotCompressedException()
            : base(DataStrings.HfaNotCompressedException)
        {
        }

        #endregion
    }
}