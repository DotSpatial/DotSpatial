// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2010 2:59:55 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// HfaTypeException
    /// </summary>
    public class HfaFieldTypeException : ApplicationException
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of HfaTypeException
        /// </summary>
        public HfaFieldTypeException(char code)
            : base(DataStrings.HfaFieldTypeException.Replace("%S", code.ToString()))
        {
        }

        #endregion
    }
}