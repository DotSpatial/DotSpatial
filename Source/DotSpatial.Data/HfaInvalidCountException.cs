// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/27/2010 3:20:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaInvalidCountException
    /// </summary>
    public class HfaInvalidCountException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of HfaInvalidCountException
        /// </summary>
        public HfaInvalidCountException(int nRows, int nCols)
            : base(DataStrings.HfaInvalidCountException.Replace("%S1", nRows.ToString()).Replace("%S2", nCols.ToString()))
        {
        }

        #endregion
    }
}